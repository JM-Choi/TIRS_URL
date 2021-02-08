#region Imports
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
#endregion

#region Program
namespace TechFloor.Device
{
    #region Define delegate
    public delegate void RawInputEventHandler(object sender, RawInputEventArgs e);
    #endregion

    #region Enumerations
    public enum RawInputType
    {
        Mouse = 0,
        Keyboard = 1,
        HID = 2
    }

    enum RawInputCommand
    {
        Input = 0x10000003,
        Header = 0x10000005
    }
    #endregion

    #region Structures
    [StructLayout(LayoutKind.Sequential)]
    struct RAWINPUTDEVICE
    {
        public ushort UsagePage;
        public ushort Usage;
        public Int32 Flags;
        public IntPtr WindowHandle;

        public RAWINPUTDEVICE(ushort usagePage, ushort usage, Int32 flags, IntPtr windowHandle)
        {
            UsagePage = usagePage;
            Usage = usage;
            Flags = flags;
            WindowHandle = windowHandle;
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct RAWINPUT
    {
        [FieldOffset(0)]
        public RAWINPUTHEADER Header;
        [FieldOffset(16)]
        public RAWKEYBOARD Keyboard;
        //mouse and HID parts omitted
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RAWINPUTHEADER
    {
        public RawInputType Type;
        public int Size;
        public IntPtr Device;
        public IntPtr wParam;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RAWKEYBOARD
    {
        public ushort MakeCode;
        public ushort Flags;
        public ushort Reserved;
        public ushort VirtualKey;
        public int Message;
        public int ExtraInformation;
    }
    #endregion

    public class RawInputDeviceManager : IMessageFilter
    {
        #region Imports
        [DllImport("user32.dll")]
        static extern bool RegisterRawInputDevices([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] RAWINPUTDEVICE[] pRawInputDevices, int uiNumDevices, int cbSize);

        [DllImport("user32.dll")]
        static extern int GetRawInputData(IntPtr hRawInput, RawInputCommand uiCommand, out RAWINPUT pData, ref int pcbSize, int cbSizeHeader);

        [DllImport("user32.dll")]
        static extern int GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        static extern int MapVirtualKey(int uCode, int uMapType);

        [DllImport("user32.dll")]
        static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpKeyState, ref int lpChar, int uFlags);
        #endregion

        #region Constants
        protected const int WM_INPUT = 0x00ff;
        #endregion

        #region Fields
        private List<RAWINPUTDEVICE> devices = new List<RAWINPUTDEVICE>();
        private PreMessageFilter filter_ = null;
        #endregion

        #region Events
        public event RawInputEventHandler RawInputEvent;
        #endregion

        #region Public methods
        public void AddKeyboardDevice(IntPtr windowHandle)
        {
            AddDevice(1, 6, 0x100, windowHandle);
        }

        public void AddDevice(ushort usagePage, ushort usage, Int32 flags, IntPtr windowHandle)
        {
            devices.Add(new RAWINPUTDEVICE(usagePage, usage, flags, windowHandle));
        }

        public bool RegisterDevices()
        {
            if (devices.Count == 0) return false;

            RAWINPUTDEVICE[] d = devices.ToArray();
            bool result = RegisterRawInputDevices(d, devices.Count, Marshal.SizeOf(typeof(RAWINPUTDEVICE)));
            return result;
        }

        public void AddMessageFilter()
        {
            if (null != filter_) return;

            filter_ = new PreMessageFilter();
            Application.AddMessageFilter(filter_);
        }

        private void RemoveMessageFilter()
        {
            if (null == filter_) return;

            Application.RemoveMessageFilter(filter_);
        }
        #endregion

        #region Protected methods
        protected void ProcessRawInput(IntPtr hRawInput)
        {
            RAWINPUT pData = new RAWINPUT();
            int pcbSize = Marshal.SizeOf(typeof(RAWINPUT));
            int result = GetRawInputData(hRawInput, RawInputCommand.Input, out pData, ref pcbSize, Marshal.SizeOf(typeof(RAWINPUTHEADER)));

            if (result != -1)
            {
                if (pData.Header.Type == RawInputType.Keyboard)
                {
                    if ((pData.Keyboard.Flags == 0) || (pData.Keyboard.Flags == 2))
                    {
                        int asciiCode = VKeyToChar(pData.Keyboard.VirtualKey, pData.Keyboard.MakeCode);
                        RawInputEvent(this, new RawInputEventArgs(pData, asciiCode));
                    }
                }
            }
        }

        protected int VKeyToChar(int virtKey, int scanCode)
        {
            int asciiCode = 0;
            byte[] kbdState = new byte[256];
            GetKeyboardState(kbdState);
            ToAscii(virtKey, scanCode, kbdState, ref asciiCode, 0);
            return asciiCode;
        }
        #endregion

        #region IMessageFilter Members
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_INPUT)
            {
                if (m.WParam != (IntPtr)0) //the app is on background
                {
                    if (RawInputEvent != null) //event implemented
                    {
                        ProcessRawInput(m.LParam);
                        return true; //stop the message
                    }
                }
            }
            return false; //allow the message to continue
        }

        #endregion
    }

    public class RawInputEventArgs : EventArgs
    {
        #region Fields
        public RAWINPUT InputDevice;
        public int AsciiCode = 0;
        #endregion

        #region Constructors
        public RawInputEventArgs(RAWINPUT rawInput, int asciiCode)
        {
            this.InputDevice = rawInput;
            this.AsciiCode = asciiCode;
        }
        #endregion
    }
}
#endregion