#region imports
using TechFloor.Object;
using TechFloor.Gui;
using System.Windows.Forms;
using System;
using System.Drawing;
#endregion

#region Program
namespace TechFloor.Forms
{
    public partial class FormMessageExt : FormMessage
    {
        public FormMessageExt()
        {
            InitializeComponent();
        }

        public FormMessageExt(string message = null, string caption = null, Buttons buttons = Buttons.Ok, Icons icon = Icons.Information, bool autoclose = false, int autoclosedelay = 5000, bool buzzer = false)
            : base(message, caption, buttons, icon, autoclose, autoclosedelay, buzzer)
        {
        }

        public virtual void SetMessageWithBuzzer(string message, string caption, bool buzzer)
        {
            labelMessage.Text = message;
            labelTitle.Text = caption;

            if (App.DigitalIoManager != null)
                App.DigitalIoManager.Buzzer = buzzer;
        }

        protected override void OnFormShown(object sender, EventArgs e)
        {
            base.OnFormShown(sender, e);
            (App.MainForm as FormMain).SetDisplayMonitor(this);
        }

        protected override void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            base.OnFormClosed(sender, e);
            (App.MainForm as FormMain).SetFocus();
        }
    }
}
#endregion