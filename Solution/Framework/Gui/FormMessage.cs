#region Imports
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TechFloor.Object;
#endregion

#region Program
namespace TechFloor.Gui
{
    public partial class FormMessage : FormExt, IFormMessage
    {
        #region Constants
        protected const int WS_EX_TOPMOST = 0x00000008;
        #endregion

        #region Fields
        protected Buttons buttonType = Buttons.Ok;
        protected Icons iconType = Icons.Information;
        protected bool autoRecover = false;
        protected bool autoClose = false;
        protected bool buzzerOn = false;
        protected int autoCloseDelay = 5000; // 5 sec
        protected int id = -1;
        protected Thread revocerThread = null;
        protected System.Timers.Timer autoCloseTimer = null;
        #endregion

        #region Properties
        protected override bool ShowWithoutActivation => true;

        public virtual int Id => id;
        #endregion

        #region Constructors
        public FormMessage()
        {
            InitializeComponent();
        }

        public FormMessage(string message = null, string caption = null, Buttons buttons = Buttons.Ok, Icons icon = Icons.Information, bool autoclose = false, int autoclosedelay = 5000, bool buzzer = false, bool recovery = false)
        {
            InitializeComponent();

            if (buzzerOn = buzzer && App.DigitalIoManager != null)
                App.DigitalIoManager.Buzzer = true;

            autoRecover = recovery;
            Create(message, caption, buttons, icon, autoclose, autoclosedelay);
        }

        public FormMessage(int idntifier, string message = null, string caption = null, Buttons buttons = Buttons.Ok, Icons icon = Icons.Information, bool autoclose = false, int autoclosedelay = 5000, bool buzzer = false, bool recovery = false)
        {
            InitializeComponent();

            if (buzzerOn = buzzer && App.DigitalIoManager != null)
                App.DigitalIoManager.Buzzer = true;

            autoRecover = recovery;
            id = idntifier;
            Create(message, caption, buttons, icon, autoclose, autoclosedelay);
        }
        #endregion

        #region Create parameters
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= WS_EX_TOPMOST;
                return createParams;
            }
        }
        #endregion

        #region Form event handler
        protected virtual void OnFormShown(object sender, EventArgs e)
        {
            switch (iconType)
            {
                case Icons.Asterisk:
                case Icons.Information:
                    {
                        if (Parent != null)
                            CenterToParent();
                        else if (Owner != null)
                        {
                            this.StartPosition = FormStartPosition.Manual;
                            this.Location = new Point(Owner.Location.X + (Owner.Width - this.Width) / 2, Owner.Location.Y + (Owner.Height - this.Height) / 2);
                        }
                    }
                    break;
            }

            if (autoClose)
            {
                autoCloseTimer = new System.Timers.Timer(autoCloseDelay);
                autoCloseTimer.Elapsed += OnElapsedAutoCloseTimer;
                autoCloseTimer.Start();
            }
        }

        protected virtual void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (buzzerOn && App.DigitalIoManager != null)
                App.DigitalIoManager.Buzzer = false;

            if (autoCloseTimer != null)
            {
                autoCloseTimer.Elapsed -= OnElapsedAutoCloseTimer;
                autoCloseTimer.Stop();
                autoCloseTimer.Dispose();
                autoCloseTimer = null;
            }
        }

        protected virtual void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            OnFormClosedProcess();
        }
        #endregion

        #region Message window create and update methods
        protected void Create(string message, string caption, Buttons buttons = Buttons.Ok, Icons icon = Icons.Information, bool autoclose = false, int autoclosedelay = 5000)
        { 
            labelTitle.Text     = caption;
            labelMessage.Text   = message;
            buttonType          = buttons;
            autoClose           = autoclose;
            autoCloseDelay      = autoclosedelay;

            switch (iconType = icon)
            {
                case Icons.Application:
                    {
                        pictureBoxIcon.Image = SystemIcons.Application.ToBitmap();
                    }
                    break;
                case Icons.Asterisk:
                    {
                        labelMessage.TextAlign = ContentAlignment.MiddleCenter;
                        labelTitle.ForeColor = Color.MediumBlue;
                        labelMessage.ForeColor = Color.MediumBlue;
                        pictureBoxIcon.Image = SystemIcons.Asterisk.ToBitmap();
                    }
                    break;
                case Icons.Error:
                    {
                        labelMessage.TextAlign = ContentAlignment.MiddleCenter;
                        labelTitle.ForeColor = Color.IndianRed;
                        labelMessage.ForeColor = Color.IndianRed;
                        pictureBoxIcon.Image = SystemIcons.Error.ToBitmap();
                    }
                    break;
                case Icons.Exclamation:
                    {
                        labelMessage.TextAlign = ContentAlignment.MiddleCenter;
                        labelTitle.ForeColor = Color.DarkOrange;
                        labelMessage.ForeColor = Color.DarkOrange;
                        pictureBoxIcon.Image = SystemIcons.Exclamation.ToBitmap();
                    }
                    break;
                case Icons.Hand:
                    {
                        labelMessage.TextAlign = ContentAlignment.MiddleCenter;
                        labelTitle.ForeColor = Color.IndianRed;
                        labelMessage.ForeColor = Color.IndianRed;
                        pictureBoxIcon.Image = SystemIcons.Hand.ToBitmap();
                    }
                    break;
                default:
                case Icons.Information:
                    {
                        labelMessage.TextAlign = ContentAlignment.MiddleCenter;
                        labelTitle.ForeColor = Color.Navy;
                        labelMessage.ForeColor = Color.Navy;
                        pictureBoxIcon.Image = SystemIcons.Information.ToBitmap();
                    }
                    break;
                case Icons.Question:
                    {
                        labelMessage.TextAlign = ContentAlignment.MiddleCenter;
                        pictureBoxIcon.Image = SystemIcons.Question.ToBitmap();
                    }
                    break;
                case Icons.Warning:
                    { 
                        labelMessage.TextAlign = ContentAlignment.MiddleCenter;
                        labelTitle.ForeColor = Color.DarkOrange;
                        labelMessage.ForeColor = Color.DarkOrange;
                        pictureBoxIcon.Image = SystemIcons.Warning.ToBitmap();
                    }
                    break;
                case Icons.WinLogo:
                    {
                        pictureBoxIcon.Image = SystemIcons.WinLogo.ToBitmap();
                    }
                    break;
                case Icons.Shield:
                    {
                        pictureBoxIcon.Image = SystemIcons.Shield.ToBitmap();
                    }
                    break;
            }

            switch (buttons)
            {
                case Buttons.Ok:
                    {
                        DialogResult = DialogResult.OK;
                        button3.Text = "OK";
                    }
                    break;
                case Buttons.OkCancel:
                    {
                        button2.Visible = true;
                        button2.Text = "OK";
                        button3.Text = "CANCEL";
                    }
                    break;
                case Buttons.YesNo:
                    {
                        button2.Visible = true;
                        button2.Text = "YES";
                        button3.Text = "NO";
                    }
                    break;
                case Buttons.RetryCancel:
                    {
                        button2.Visible = true;
                        button2.Text = "RETRY";
                        button3.Text = "CANCEL";
                    }
                    break;
                case Buttons.YesNoCancel:
                    {
                        button1.Visible = true;
                        button2.Visible = true;
                        button1.Text = "YES";
                        button2.Text = "NO";
                        button3.Text = "CANCEL";
                    }
                    break;
                case Buttons.AbortRetryIgnore:
                    {
                        button1.Visible = true;
                        button2.Visible = true;
                        button1.Text = "ABORT";
                        button2.Text = "RETRY";
                        button3.Text = "IGNORE";
                    }
                    break;
            }
        }

        protected void UpdateMessage(string message)
        {
            this.labelMessage.Text = message;
        }
        #endregion

        #region Button event handlers
        protected virtual void OnClickButton1(object sender, EventArgs e)
        {
            switch (buttonType)
            {
                case Buttons.AbortRetryIgnore:
                    DialogResult = DialogResult.Abort;
                    break;
                case Buttons.YesNoCancel:
                    DialogResult = DialogResult.Yes;
                    break;
                default:
                    return;
            }

            Close();
        }

        protected virtual void OnClickButton2(object sender, EventArgs e)
        {
            switch (buttonType)
            {
                case Buttons.OkCancel:
                    DialogResult = DialogResult.OK;
                    break;
                case Buttons.YesNo:
                    DialogResult = DialogResult.Yes;
                    App.DataLoad = 1;
                    break;
                case Buttons.RetryCancel:
                    DialogResult = DialogResult.Retry;
                    break;
                case Buttons.AbortRetryIgnore:
                    DialogResult = DialogResult.Retry;
                    break;
                case Buttons.YesNoCancel:
                    DialogResult = DialogResult.No;
                    break;
                default:
                    return;
            }

            Close();
        }

        protected virtual void OnClickButton3(object sender, EventArgs e)
        {
            switch (buttonType)
            {
                case Buttons.Ok:
                    DialogResult = DialogResult.OK;
                    break;
                case Buttons.OkCancel:
                    DialogResult = DialogResult.Cancel;
                    break;
                case Buttons.YesNo:
                    DialogResult = DialogResult.No;
                    App.DataLoad = 0;
                    break;
                case Buttons.RetryCancel:
                    DialogResult = DialogResult.Cancel;
                    break;
                case Buttons.AbortRetryIgnore:
                    DialogResult = DialogResult.Ignore;
                    break;
                case Buttons.YesNoCancel:
                    DialogResult = DialogResult.Cancel;
                    break;
                default:
                    return;
            }

            Close();
        }
        #endregion

        #region Auto close timer event handler
        protected virtual void OnElapsedAutoCloseTimer(object sender, EventArgs e)
        {
            autoCloseTimer.Stop();

            if (InvokeRequired)
                BeginInvoke(new Action(() => { Close(); }));
            else
                Close();
        }
        #endregion

        #region Private methods
        static FormMessage Dialog = null;
        private static DialogResult ShowMessageDialog(string message, string caption, Buttons buttons, Icons icon, bool autoclose, int autoclosedelay, bool buzzer, bool recovery = false)
        {
            //FormMessage form = new FormMessage(message, caption, buttons, icon, autoclose, autoclosedelay, buzzer, recovery);
            Dialog = new FormMessage(message, caption, buttons, icon, autoclose, autoclosedelay, buzzer, recovery);
            DialogResult result = Dialog.ShowDialog();
            Dialog = null;
            return result;
        }

        private static void ShowMessage(string message, string caption, Buttons buttons, Icons icon, bool autoclose, int autoclosedelay, bool buzzer)
        {
            FormMessage form = new FormMessage(message, caption, buttons, icon, autoclose, autoclosedelay, buzzer);
            form.Show((Form)App.MainForm);
        }

        private static FormMessage ShowMessage(int identifier, string message, string caption, Buttons buttons, Icons icon, bool autoclose, int autoclosedelay, bool buzzer, FormClosedEventHandler func)
        {
            FormMessage form = new FormMessage(identifier, message, caption, buttons, icon, autoclose, autoclosedelay, buzzer);

            if (func != null)
                form.FormClosed += func;

            form.Show((Form)App.MainForm);
            return form;
        }
        #endregion

        #region Protected methods
        protected virtual void OnMouseDownCaption(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected virtual void OnMouseMoveCaption(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected virtual void OnMouseUpCaption(object sender, MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        protected virtual void OnFormClosedProcess()
        {
            if (autoRecover)
            {
                if (App.MainSequence.OperationState == OperationStates.Alarm)
                {
                    if (revocerThread == null)
                        (revocerThread = new Thread(new ThreadStart(AutoRecovery)))?.Start();
                }
            }
        }

        protected virtual void AutoRecovery()
        {
            if (App.MainSequence.CycleStop)
            {
                App.MainSequence.Reset();
            }
            else
            {
                if (App.OperationState == OperationStates.Alarm)
                {
                    if (App.MainSequence.Reset())
                    {
                        if (App.Initialized)
                            App.MainSequence.Start();
                    }
                }
                else
                    App.MainSequence.Reset();
            }
        }
        #endregion

        #region Public methods
        public static void CloseDialog()
        {
            if (Dialog != null)
            {
                Dialog.Close();
            }
        }

        public static DialogResult ShowWarning(string message, Buttons buttons = Buttons.Ok, bool autoclose = false, int autoclosedelay = 10000)
        {
            return ShowMessageDialog(message, "WARNING", buttons, Icons.Warning, autoclose, autoclosedelay, false);
        }

        public static DialogResult ShowAlert(string message, Buttons buttons = Buttons.Ok, bool autoclose = false, int autoclosedelay = 10000)
        {
            return ShowMessageDialog(message, "ALERT", buttons, Icons.Error, autoclose, autoclosedelay, true);
        }

        public static DialogResult ShowAlertWithRecovery(string message, Buttons buttons = Buttons.Ok, bool autoclose = false, int autoclosedelay = 10000)
        {
            return ShowMessageDialog(message, "ALERT", buttons, Icons.Error, autoclose, autoclosedelay, true, true);
        }

        public static DialogResult ShowQuestion(string message, Buttons buttons = Buttons.YesNo, bool autoclose = false, int autoclosedelay = 10000)
        {
            return ShowMessageDialog(message, "QUESTION", buttons, Icons.Question, autoclose, autoclosedelay, false);
        }

        public static DialogResult ShowQuestionWithBuzzer(string message, Buttons buttons = Buttons.YesNo, bool autoclose = false, int autoclosedelay = 10000)
        {
            return ShowMessageDialog(message, "QUESTION", buttons, Icons.Question, autoclose, autoclosedelay, true);
        }

        public static DialogResult ShowYesNoCancelQuestion(string message, Buttons buttons = Buttons.YesNoCancel, bool autoclose = false, int autoclosedelay = 10000)
        {
            return ShowMessageDialog(message, "QUESTION", buttons, Icons.Question, autoclose, autoclosedelay, false);
        }

        public static void ShowInformation(string message, Buttons buttons = Buttons.Ok, bool autoclose = false, int autoclosedelay = 10000)
        {
            ShowMessage(message, "INFORMATION", buttons, Icons.Information, autoclose, autoclosedelay, false);
        }

        public static void ShowInformationWithBuzzer(string message, Buttons buttons = Buttons.Ok, bool autoclose = false, int autoclosedelay = 10000)
        {
            ShowMessage(message, "INFORMATION", buttons, Icons.Information, autoclose, autoclosedelay, true);
        }

        public static FormMessage ShowManagedInformation(int identifier, string message, Buttons buttons = Buttons.Ok, FormClosedEventHandler func = null)
        {
            return ShowMessage(identifier, message, "INFORMATION", buttons, Icons.Information, false, -1, false, func);
        }

        public static FormMessage ShowManagedInformationWithBuzzer(int identifier, string message, Buttons buttons = Buttons.Ok, FormClosedEventHandler func = null)
        {
            return ShowMessage(identifier, message, "INFORMATION", buttons, Icons.Information, false, -1, true, func);
        }

        public static void ShowNotification(string message, Buttons buttons = Buttons.Ok, bool autoclose = true, int autoclosedelay = 10000)
        {
            ShowMessage(message, "NOTIFICATION", buttons, Icons.Asterisk, autoclose, autoclosedelay, false);
        }

        public static void ShowNotificationWithBuzzer(string message, Buttons buttons = Buttons.Ok, bool autoclose = true, int autoclosedelay = 10000)
        {
            ShowMessage(message, "NOTIFICATION", buttons, Icons.Asterisk, autoclose, autoclosedelay, true);
        }

        public virtual DialogResult ShowDialog(string message, string caption, Buttons buttons, Icons icon, bool autoclose, int autoclosedelay, bool buzzer)
        {
            return ShowMessageDialog(message, caption, buttons, icon, autoclose, autoclosedelay, buzzer);
        }

        public virtual void Show(string message, string caption, Buttons buttons, Icons icon, bool autoclose, int autoclosedelay, bool buzzer)
        {
            ShowMessage(message, caption, buttons, icon, autoclose, autoclosedelay, buzzer);
        }
        #endregion
    }
}
#endregion
