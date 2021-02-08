#region Licenses
///////////////////////////////////////////////////////////////////////////////
/// MIT License
/// 
/// Copyright (c) 2019 Marcus Software Ltd.
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in all
/// copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
/// SOFTWARE.
///////////////////////////////////////////////////////////////////////////////
///          Copyright Joe Coder 2004 - 2006.
/// Distributed under the Boost Software License, Version 1.0.
///    (See accompanying file LICENSE_1_0.txt or copy at
///          https://www.boost.org/LICENSE_1_0.txt)
///////////////////////////////////////////////////////////////////////////////
/// 저작권 (c) 2019 Marcus Software Ltd. (isadrastea.kor@gmail.com)
///
/// 본 라이선스의 적용을 받는 소프트웨어와 동봉된 문서(소프트웨어)를 획득하는 
/// 모든 개인이나 기관은 소프트웨어를 Marcus Software (isadrastea.kor@gmail.com)
/// 에 신고하고, 허용 의사를 서면으로 득하여, 사용, 복제, 전시, 배포, 실행 및
/// 전송할 수 있고, 소프트웨어의 파생 저작물을 생성할 수 있으며, 소프트웨어가
/// 제공된 제3자에게 그러한 행위를 허용할 수 있다. 단, 이 모든 행위는 다음과 
/// 같은 조건에 의해 제한 한다.:
///
/// 소프트웨어의 저작권 고지, 그리고 위의 라이선스 부여와 이 규정과 아래의 부인 
/// 조항을 포함한 이 글의 전문이 소프트웨어를 전체적으로나 부분적으로 복제한 
/// 모든 복제본과 소프트웨어의 모든 파생 저작물 내에 포함되어야 한다. 단, 해당 
/// 복제본이나 파생저작물이 소스 언어 프로세서에 의해 생성된, 컴퓨터로 인식 
/// 가능한 오브젝트 코드의 형식으로만 되어 있는 경우는 제외된다.
///
/// 이 소프트웨어는 상품성, 특정 목적에의 적합성, 소유권, 비침해에 대한 보증을 
/// 포함한, 이에 국한되지는 않는, 모든 종류의 명시적이거나 묵시적인 보증 없이 
///“있는 그대로의 상태”로 제공된다. 저작권자나 소프트웨어의 배포자는 어떤 
/// 경우에도 소프트웨어 자체나 소프트웨어의 취급과 관련하여 발생한 손해나 기타 
/// 책임에 대하여, 계약이나 불법행위 등에 관계 없이 어떠한 책임도 지지 않는다.
///////////////////////////////////////////////////////////////////////////////
/// project ReelTower 
/// @author Marcus (isadrastea.kor@gmail.com)
/// @namespace Marcus.Solution.TechFloor
/// @file FormBarcodeInput.cs
/// @brief
/// @details
/// @date 2019, 5, 21, 오후 1:13
///////////////////////////////////////////////////////////////////////////////
#endregion

#region Imports
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Marcus.Solution.TechFloor.Forms;
using Marcus.Solution.TechFloor.Gui;
using Marcus.Solution.TechFloor.Object;
using Marcus.Solution.TechFloor.Device;
using System.Threading;
#endregion

#region Program
namespace Marcus.Solution.TechFloor
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
                    textBoxLotId.Text,
                    textBoxMaker.Text,
                    textBoxQty.Text,
                    textBoxUid.Text,
                    textBoxMfg.Text);
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
            textBoxUid.Text = BarcodeData.Data.Rid;
            textBoxLotId.Text = BarcodeData.Data.LotId;
            textBoxSid.Text = BarcodeData.Data.Sid;
            textBoxMaker.Text = BarcodeData.Data.Maker;
            textBoxQty.Text = BarcodeData.Data.Qty;
            textBoxMfg.Text = BarcodeData.Data.Mfg;
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

                            if (BarcodeAndVision.GetBarcode(inputData, ref data))
                            {
                                BarcodeData.Clear();
                                BarcodeData.CopyMaterialData(data);
                                textBoxUid.Text = BarcodeData.Data.Rid;
                                textBoxLotId.Text = BarcodeData.Data.LotId;
                                textBoxSid.Text = BarcodeData.Data.Sid;
                                textBoxMaker.Text = BarcodeData.Data.Maker;
                                textBoxQty.Text = BarcodeData.Data.Qty;
                                textBoxMfg.Text = BarcodeData.Data.Mfg;
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