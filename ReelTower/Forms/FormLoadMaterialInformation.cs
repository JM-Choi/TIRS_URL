#region Imports
using System;
using System.Diagnostics;
using System.Reflection;
using System.Timers;
using System.Windows.Forms;
using TechFloor.Forms;
using TechFloor.Gui;
using TechFloor.Object;
#endregion

#region Program
namespace TechFloor
{
    public partial class FormLoadMaterialInformation : FormExt
    {
        #region Fields
        protected bool autoRun = false;

        protected object lockObject = new object();

        protected readonly string towerId;

        protected readonly string towerName;

        protected readonly string articleName;

        protected readonly string carrierName;

        protected readonly int quantity;

        protected System.Timers.Timer autoCloseTimer = new System.Timers.Timer();
        #endregion

        #region Properties
        public virtual string TowerId => towerId;

        public virtual string TowerName => towerName;

        public virtual string Carrier => carrierName;
        #endregion

        #region Constructors
        public FormLoadMaterialInformation(string towername, string towerid, MaterialData data, bool automode = false, bool loaddelay = false)
        {
            InitializeComponent();
            this.Location = (App.MainForm as FormMain).Location;
            this.towerName = towername;
            this.towerId = towerid;

            if (this.autoRun = automode && Config.RejectAutoUsage)
            {
                this.autoCloseTimer = new System.Timers.Timer(Config.TimeoutOfReject * 1000);
                this.autoCloseTimer.AutoReset = false;
                this.autoCloseTimer.Elapsed += OnElapsedAutoCloseTimer;
                this.autoCloseTimer.Start();
            }
            else if (loaddelay)
            {
                this.autoCloseTimer = new System.Timers.Timer(Config.LoadDelayTimeByManual * 1000);
                this.autoCloseTimer.AutoReset = false;
                this.autoCloseTimer.Elapsed += OnElapsedAutoCloseDelayTimer;
                this.autoCloseTimer.Start();
            }

            if (data != null)
            {
                this.articleName = data.Category;
                this.carrierName = data.Name;
                this.quantity = data.Quantity;
            }
        }
        #endregion

        #region Protected methods
        #region Auto close timer methods
        protected virtual void ShowAutoCloseAbortNotification()
        {
            this.buttonAbort.PerformClick();
        }

        protected virtual void ShowAutoCloseOkNotification()
        {
            this.buttonOk.PerformClick();
        }

        protected virtual void OnElapsedAutoCloseTimer(object sneder, ElapsedEventArgs e)
        {
            if (sneder != null)
            {
                (sneder as System.Timers.Timer).Stop();
                if (InvokeRequired)
                    BeginInvoke(new Action(() => { ShowAutoCloseAbortNotification(); }));
                else
                    ShowAutoCloseAbortNotification();
            }
        }

        protected virtual void OnElapsedAutoCloseDelayTimer(object sneder, ElapsedEventArgs e)
        {
            if (sneder != null)
            {
                (sneder as System.Timers.Timer).Stop();
                if (InvokeRequired)
                    BeginInvoke(new Action(() => { ShowAutoCloseOkNotification(); }));
                else
                    ShowAutoCloseOkNotification();
            }
        }
        #endregion

        #region Form event handlers
        protected override void Dispose(bool disposing)
        {
            if (autoCloseTimer != null)
            {
                autoCloseTimer.Stop();
                autoCloseTimer.Dispose();
            }

            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        protected virtual void OnFormShown(object sender, EventArgs e)
        {
            (App.MainForm as FormMain).SetDisplayMonitor(this);

            if (autoRun)
                labelMessage.Text = string.Format(Properties.Resources.String_FormLoadMaterialInformation_MessageAutoRun, carrierName, towerName, Environment.NewLine);
            else
                labelMessage.Text = string.Format(Properties.Resources.String_FormLoadMaterialInformation_MessageManualRun, carrierName, towerName, Environment.NewLine);

            buttonOk.Visible = !autoRun;
        }

        protected virtual void OnFormLoad(object sender, EventArgs e)
        {
            try
            {
                SetDisplayLanguage();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
        }
        #endregion

        #region Set display language
        protected virtual void SetDisplayLanguage()
        {
            buttonAbort.Text = Properties.Resources.String_Abort;
            buttonOk.Text = Properties.Resources.String_Ok;
            labelTitle.Text = Properties.Resources.String_FormLoadReelInformation_labelTitle;
        }
        #endregion

        #region Abort load
        protected void OnClickButtonAbort(object sender, EventArgs e)
        {
            (App.MainSequence as ReelTowerGroupSequence).RemoveCarrier(carrierName);
            (App.MainSequence as ReelTowerGroupSequence).SetTowerState(towerName, MaterialStorageState.StorageOperationStates.Abort);
            this.DialogResult = DialogResult.Abort;
            FormMessageExt.ShowNotification(string.Format(Properties.Resources.String_FormLoadMaterialInformation_MessageAbort, carrierName, towerName));
            this.Close();
        }
        #endregion

        #region Load reel
        protected void OnClickButtonOk(object sender, EventArgs e)
        {
            (App.MainSequence as ReelTowerGroupSequence).LoadReel(towerId);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        #endregion
        #endregion
    }
}
#endregion