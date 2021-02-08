#region Imports
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using TechFloor.Forms;
using TechFloor.Gui;
using TechFloor.Object;
using TechFloor.Device;
using System.Threading;
using TechFloor.Components;
#endregion

#region Program
namespace TechFloor
{
    public partial class FormBarcodeInput : FormExt
    {
        #region Fields
        protected bool skipInputData = false;
        protected bool flushInputData = false;
        protected RawInputDeviceManager rawinputDeviceManager = new RawInputDeviceManager();
        protected const bool onlyForeground = true;
        protected string inputData = string.Empty;
        protected StringBuilder sb = new StringBuilder();
        public BarcodeKeyInData BarcodeData = new BarcodeKeyInData();
        protected int autoCloseDelay = 30000; // 30 sec
        protected System.Timers.Timer autoCloseTimer = null;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public FormBarcodeInput(MaterialData data = null)
        {
            if (data != null)
                BarcodeData.CopyMaterialData(data);

            InitializeComponent();
            this.Location = (App.MainForm as FormMain).Location;

            autoCloseTimer = new System.Timers.Timer(autoCloseDelay);
            autoCloseTimer.Elapsed += OnElapsedAutoCloseTimer;
            autoCloseTimer.Start();
        }
        #endregion

        private void OnClickButtonExit(object sender, EventArgs e)
        {
            BarcodeData.Clear();
            BarcodeData.State = BarcodeInputStates.Canceled;
            Close();
        }

        private void OnClickButtonApply(object sender, EventArgs e)
        {
            TerminateCloseTimer();
            if (string.IsNullOrEmpty(textBoxUid.Text) || string.IsNullOrEmpty(textBoxSid.Text))
            {
                FormMessageExt.ShowInformation(Properties.Resources.String_FormBarcodeInput_Notification_Mistype_Uid);
                return;
            }
            else if (textBoxSid.Text.Length != 9)
            {
                FormMessageExt.ShowInformation(Properties.Resources.String_FormBarcodeInput_Notification_Mistype_Sid);
                return;
            }
            else
            {
                BarcodeData.Data.SetValues(
                    textBoxSid.Text,
                    textBoxUid.Text,
                    textBoxLotId.Text,
                    textBoxMaker.Text,
                    textBoxMfg.Text,
                    string.Empty,
                    string.Empty,
                    int.Parse(textBoxQty.Text));
                BarcodeData.State = BarcodeInputStates.Typed;
            }

            Close();
        }

        private void OnClickButtonBuzzerOff(object sender, EventArgs e)
        {
            TerminateCloseTimer();
            (App.DigitalIoManager as DigitalIoManager).Buzzer = false;
        }

        private void OnFormLoad(object sender, EventArgs e)
        { 
            textBoxUid.Text = BarcodeData.Data.Name;
            textBoxLotId.Text = BarcodeData.Data.LotId;
            textBoxSid.Text = BarcodeData.Data.Category;
            textBoxMaker.Text = BarcodeData.Data.Supplier;
            textBoxQty.Text = BarcodeData.Data.Quantity.ToString();
            textBoxMfg.Text = BarcodeData.Data.ManufacturedDatetime;
            App.DigitalIoManager.SetSignalTower(OperationStates.Pause, true);
            SetDisplayLanguage();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        internal static extern IntPtr GetFocus();

        private Control GetFocusedControl()
        {
            Control focusedControl = null;
            // To get hold of the focused control:
            IntPtr focusedHandle = GetFocus();
            if (focusedHandle != IntPtr.Zero)
                // Note that if the focused Control is not a .Net control, then this will return null.
                focusedControl = Control.FromHandle(focusedHandle);
            return focusedControl;
        }

        public static Control FindFocusedControl(Control control)
        {
            var container = control as IContainerControl;
            while (container != null)
            {
                control = container.ActiveControl;
                container = control as IContainerControl;
            }
            return control;
        }

        protected void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            TerminateCloseTimer();
            App.DigitalIoManager.SetSignalTower(OperationStates.Run);
        }

        #region Set display language
        protected virtual void SetDisplayLanguage()
        {
            buttonClear.Text = Properties.Resources.String_Clear;
            buttonClose.Text = Properties.Resources.String_FormBarcodeInput_buttonClose;
            buttonBuzzerOff.Text = Properties.Resources.String_FormBarcodeInput_buttonBuzzerOff;
            buttonApply.Text = Properties.Resources.String_FormBarcodeInput_buttonApply;
        }
        #endregion

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (skipInputData)
            {
                inputData += e.KeyChar;
                e.Handled = true;
                Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: {inputData}");
            }
        }

        private void OnPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Modifiers:
                case Keys.None:
                case Keys.LButton:
                case Keys.RButton:
                case Keys.Cancel:
                case Keys.MButton:
                case Keys.XButton1:
                case Keys.XButton2:
                case Keys.Tab:
                case Keys.LineFeed:
                case Keys.Clear:
                case Keys.ShiftKey:
                case Keys.ControlKey:
                case Keys.Menu:
                case Keys.Capital:
                // case Keys.CapsLock:
                case Keys.Pause:
                // case Keys.KanaMode:
                case Keys.HanguelMode:
                case Keys.JunjaMode:
                case Keys.FinalMode:
                // case Keys.KanjiMode:
                case Keys.HanjaMode:
                case Keys.Escape:
                case Keys.IMEConvert:
                case Keys.IMENonconvert:
                case Keys.IMEAccept:
                case Keys.IMEModeChange:
                //case Keys.Prior:
                case Keys.PageUp:
                case Keys.Next:
                //case Keys.PageDown:
                case Keys.End:
                case Keys.Home:
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                case Keys.Select:
                case Keys.Print:
                case Keys.Execute:
                // case Keys.Snapshot:
                case Keys.PrintScreen:
                case Keys.Insert:
                case Keys.Help:
                case Keys.LWin:
                case Keys.RWin:
                case Keys.Apps:
                case Keys.Sleep:
                case Keys.F1:
                case Keys.F2:
                case Keys.F3:
                case Keys.F4:
                case Keys.F5:
                case Keys.F6:
                case Keys.F7:
                case Keys.F8:
                case Keys.F9:
                case Keys.F10:
                case Keys.F11:
                case Keys.F12:
                case Keys.F13:
                case Keys.F14:
                case Keys.F15:
                case Keys.F16:
                case Keys.F17:
                case Keys.F18:
                case Keys.F19:
                case Keys.F20:
                case Keys.F21:
                case Keys.F22:
                case Keys.F23:
                case Keys.F24:
                case Keys.NumLock:
                case Keys.Scroll:
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                case Keys.LControlKey:
                case Keys.RControlKey:
                case Keys.LMenu:
                case Keys.RMenu:
                case Keys.BrowserBack:
                case Keys.BrowserForward:
                case Keys.BrowserRefresh:
                case Keys.BrowserStop:
                case Keys.BrowserSearch:
                case Keys.BrowserFavorites:
                case Keys.BrowserHome:
                case Keys.VolumeMute:
                case Keys.VolumeDown:
                case Keys.VolumeUp:
                case Keys.MediaNextTrack:
                case Keys.MediaPreviousTrack:
                case Keys.MediaStop:
                case Keys.MediaPlayPause:
                case Keys.LaunchMail:
                case Keys.SelectMedia:
                case Keys.LaunchApplication1:
                case Keys.LaunchApplication2:
                case Keys.ProcessKey:
                case Keys.Packet:
                case Keys.Attn:
                case Keys.Crsel:
                case Keys.Exsel:
                case Keys.EraseEof:
                case Keys.Play:
                case Keys.Zoom:
                case Keys.NoName:
                case Keys.Pa1:
                case Keys.OemClear:
                case Keys.KeyCode:
                case Keys.Shift:
                case Keys.Control:
                case Keys.Alt:
                case Keys.Back:
                case Keys.Delete:
                    skipInputData = false;
                    break;
                // case Keys.Return:
                case Keys.Enter:
                    {
                        skipInputData = false;

                        if (inputData.Contains(";"))
                        {
                            MaterialData data = new MaterialData();
                            Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: {inputData}");

                            if (CompositeVisionManager.GetBarcode(inputData, ref data))
                            {
                                BarcodeData.Clear();
                                BarcodeData.CopyMaterialData(data);
                                textBoxUid.Text = BarcodeData.Data.Name;
                                textBoxLotId.Text = BarcodeData.Data.LotId;
                                textBoxSid.Text = BarcodeData.Data.Category;
                                textBoxMaker.Text = BarcodeData.Data.Supplier;
                                textBoxQty.Text = BarcodeData.Data.Quantity.ToString();
                                textBoxMfg.Text = BarcodeData.Data.ManufacturedDatetime;
                            }
                        }
                        else if (inputData.Substring(0, 2) == "RQ")
                        {
                            textBoxQty.Text = inputData.Substring(2, inputData.Length - 2);
                        }

                        inputData = string.Empty;
                    }
                    break;
                case Keys.Space:
                case Keys.Multiply:
                case Keys.Add:
                case Keys.Separator:
                case Keys.Subtract:
                case Keys.Decimal:
                case Keys.Divide:
                case Keys.OemSemicolon:
                // case Keys.Oem1:
                case Keys.Oemplus:
                case Keys.Oemcomma:
                case Keys.OemMinus:
                case Keys.OemPeriod:
                case Keys.OemQuestion:
                // case Keys.Oem2:
                case Keys.Oemtilde:
                // case Keys.Oem3:
                case Keys.OemOpenBrackets:
                // case Keys.Oem4:
                case Keys.OemPipe:
                // case Keys.Oem5:
                case Keys.OemCloseBrackets:
                // case Keys.Oem6:
                case Keys.OemQuotes:
                // case Keys.Oem7:
                case Keys.Oem8:
                case Keys.OemBackslash:
                // case Keys.Oem102:
                default:
                    skipInputData = true;
                    break;
            }
        }

        private void OnClickButtonClear(object sender, EventArgs e)
        {
            textBoxUid.Text = string.Empty;
            textBoxLotId.Text = string.Empty;
            textBoxSid.Text = string.Empty;
            textBoxMaker.Text = string.Empty;
            textBoxQty.Text = string.Empty;
            textBoxMfg.Text = string.Empty;
            TerminateCloseTimer();
        }

        private void OnFormShown(object sender, EventArgs e)
        {
            (App.MainForm as FormMain).SetDisplayMonitor(this);
        }

        #region Auto close timer event handler
        protected void TerminateCloseTimer()
        {
            if (autoCloseTimer != null)
            {
                autoCloseTimer.Elapsed -= OnElapsedAutoCloseTimer;
                autoCloseTimer.Stop();
                autoCloseTimer.Dispose();
                autoCloseTimer = null;
            }
        }

        protected void OnElapsedAutoCloseTimer(object sender, EventArgs e)
        {
            autoCloseTimer.Stop();

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => {
                    buttonClose.PerformClick();
                }));
            }
            else
            {
                buttonClose.PerformClick();
            }
        }
        #endregion
    }
}
#endregion