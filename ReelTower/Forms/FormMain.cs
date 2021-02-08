#region Imports
using TechFloor.Components;
using TechFloor.Components.Elements;
using TechFloor.Forms;
using TechFloor.Gui;
using TechFloor.Object;
using TechFloor.Service.MYCRONIC.WebService;
using TechFloor.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TechFloor.Device;
#endregion

#region Program
#pragma warning disable CS0067
namespace TechFloor
{
    public partial class FormMain : Form, IFormMain
    {
        #region Constants
        protected const int CONST_VISION_PROCESS_CHECK_TIMEOUT = 10;

        protected const int CONST_SHORT_MESSAGEBOX_CLOSE_DELAY = 3000;

        protected readonly string CONST_PROVIDE_JOB_PREFIX = $"JOB_{DateTime.Now.ToString("yyyyMMdd")}_";
        #endregion

        #region Fields
        protected bool skipInputData = false;

        protected bool stopSplash = false;

        protected bool shutdownApp = false;

        protected bool combineModuleState = false;

        protected int receivedNotificationIndex = 0;

        protected List<byte> inputBytes = new List<byte>();

        protected string inputData = string.Empty;

        protected CommunicationStates currentReelTowerWebServiceState = CommunicationStates.None;

        protected CommunicationStates currentReelHandlerCommState = CommunicationStates.None;

        protected OperationStates operationState = OperationStates.PowerOn;

        protected AlarmStates alarmState = AlarmStates.Cleared;

        protected DateTime startedDateTime = DateTime.Now;

        protected App appInstance = null;

        protected Screen displayMonitor = null;

        protected ProductionRecord productionRecord = null;

        protected SpalshWorker splashWorker = null;

        protected ReelTowerGroupSequence mainSequence = null;

        protected ManualResetEvent shutdownEvent = new ManualResetEvent(false);

        protected FormSplash splashScreen = null;

        protected FormMessageExt notificationWindow = null;

        protected FormMessageExt notificationDockWindow = null;

        protected BarcodeKeyInData barcodeData = new BarcodeKeyInData();

        protected Dictionary<string, Form> managedLoadReelNotifications = new Dictionary<string, Form>();

        protected int heartbeat_ = 0;

        protected bool amm_close = true;
        #endregion

        #region Events
        #endregion

        #region Properties
        public Screen DisplayMonitor => displayMonitor;

        public OperationStates OperationState => mainSequence.OperationState;

        public AlarmStates AlarmState
        {
            get => alarmState;
            set => alarmState = value;
        }

        public IDigitalIoManager DigitalIoManager => null; // throw new NotImplementedException();

        public IMainSequence MainSequence => mainSequence;

        public WaitHandle ShutdownEvent => shutdownEvent;
        #endregion

        #region Delegation
        public delegate void UpdateText(Control ctrl, string text);
        #endregion

        #region Constructors
        public FormMain(App app = null)
        {
            InitializeComponent();
            appInstance = app;
        }
        #endregion

        #region Protected methods
        #region Form event handlers
        protected void OnFormLoad(object sender, EventArgs e)
        {
            this.Hide();
            GetCurrentMonitor();

            splashScreen = new FormSplash();
            splashWorker = new SpalshWorker();

            new Thread(new ThreadStart(this.RunSplash)).Start();

            splashWorker.ProgressChanged += (o, ex) =>
            {
                this.splashScreen.UpdateProgress(ex.Progress);
            };

            splashWorker.WorkCompleted += (o, ex) =>
            {
                stopSplash = true;
                this.Show();
            };

            if (LoadSystemConfig())
            {
                CreateElements();
                AttachEventHandlers();
                SetDisplayLanguage();
            }
            else
            {
                splashWorker.SetProgress(100);
                this.Close();
            }
        }

        protected void OnFormShown(object sender, EventArgs e)
        {
            // tabControlMain.Enabled              =
            tabControlOperation.Enabled =
            radioButtonLoginOperator.Enabled =
            radioButtonSearchReel.Enabled =
            radioButtonJobMonitor.Enabled = false;

            if (!Config.SystemSimulation)
                mainSequence.Create();
        }

        protected void OnFormCloase(object sender, FormClosedEventArgs e) { }

        protected void OnFormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (shutdownApp || !(e.Cancel = (e.CloseReason == CloseReason.UserClosing)))
                DestroyElements();
        }
        #endregion

        #region Splash window methods
        protected void RunSplash()
        {
            splashScreen.Show();

            while (!stopSplash)
                Application.DoEvents();

            splashScreen.Close();
            splashScreen.Dispose();
        }
        #endregion

        #region System startup and shutdown methods
        protected bool LoadSystemConfig()
        {
            Logger.Create();
            Logger.AddCategory("Job");
            Logger.AddCategory("Tower");
            Logger.AddCategory("Robot");
            Logger.AddCategory("Alarm");
            Logger.Trace($"Start application (Name={App.Name}, Version={App.Version})", "System");
            labelApplicationVersion.Text = $"Revision: {App.Version}";

            if (Config.Load() & Model.Load())
            {
                SetVariables();
                splashWorker.SetProgress(10);
                return true;
            }
            else
            {
                Logger.Trace("Failed to load configuration or model information from file!");
                FormMessageExt.ShowNotification(string.Format(Properties.Resources.String_FormMain_Notification_Load_Config_Failure, Environment.NewLine));
                buttonExit.PerformClick();
                return false;
            }
        }

        protected void CreateFileSystem()
        {
            Directory.CreateDirectory(ReelTowerGroupSequence.CONST_CONFIG_PATH);
            Directory.CreateDirectory(ReelTowerGroupSequence.CONST_MODEL_PATH);
            Directory.CreateDirectory(ReelTowerGroupSequence.CONST_DATA_PATH);
        }

        protected void RedrawControls()
        {
            int count_ = 0;

            if (mainSequence.ReelTowerGroupObject.Towers.Count > 0)
            {
                float width_ = tableLayoutPanelTitle.ColumnStyles[4].Width;
                labelTower2Status.BackColor = SystemColors.Window;
                labelTower3Status.BackColor = SystemColors.Window;
                labelTower4Status.BackColor = SystemColors.Window;
                labelRobotStatus.BackColor = SystemColors.Window;
                tableLayoutPanelTitle.ColumnStyles[4].Width = 0;
                tableLayoutPanelTitle.ColumnStyles[5].Width = 0;
                tableLayoutPanelTitle.ColumnStyles[6].Width = 0;
                tableLayoutPanelTitle.ColumnStyles[7].Width = 0;

                if (Config.ReelHandlerUsage)
                    labelRobotStatus.BackColor = SystemColors.HotTrack;
                else
                {
                    tableLayoutPanelTitle.ColumnStyles[8].Width = 0;
                    checkBoxTakeoutByRobot.Visible = false;
                }

                foreach (ReelTower twr_ in mainSequence.ReelTowerGroupObject.Towers.Values)
                {
                    switch (twr_.Index)
                    {
                        case 1:
                            labelTower1Status.BackColor = SystemColors.HotTrack;
                            break;
                        case 2:
                            labelTower2Status.BackColor = SystemColors.HotTrack;
                            break;
                        case 3:
                            labelTower3Status.BackColor = SystemColors.HotTrack;
                            break;
                        case 4:
                            labelTower4Status.BackColor = SystemColors.HotTrack;
                            break;
                    }

                    if (twr_.Index > 0 && twr_.Index <= 4)
                        tableLayoutPanelTitle.ColumnStyles[3 + ++count_].Width = width_;
                }

                comboBoxAssignJobOutportValue.Items.Clear();

                foreach (KeyValuePair<int, string> output_ in Config.OutStageIds)
                    comboBoxAssignJobOutportValue.Items.Add(output_.Value);
            }

            checkBoxQueryByCustomerDB.Visible = Config.AMMUsage;

            Utility.SetDoubleBuffered(listViewReelTowerGroupNotifications, true);
            radioButtonLoginOperator.Checked = true;
            comboBoxSearchReelIdTypeValue.SelectedIndex = 0;

            if (comboBoxAssignJobOutportValue.Items.Count > 0)
                comboBoxAssignJobOutportValue.SelectedIndex = mainSequence.ReservedOutPort;

            FormExt.SetDoubleBuffered(listViewPendedReels, true);
        }

        protected void SetControlData()
        {
            int index_ = 0;
            int data_ = 0;
            bool enabled_ = false;
            string name_ = string.Empty;
            comboBoxLoadReelTowerIdValue.Items.Clear();
            comboBoxLoadReelSizeValue.Items.Clear();
            comboBoxLoadReelThicknessValue.Items.Clear();
            comboBoxQueryReelTowerIdValue.Items.Clear();
            comboBoxQueryReelSizeValue.Items.Clear();
            comboBoxQueryDatetimeOptionValue.Items.Clear();
            comboBoxQueryReelQtyOptionValue.Items.Clear();

            foreach (ReelTower twr_ in mainSequence.ReelTowerGroupObject.Towers.Values)
            {
                comboBoxLoadReelTowerIdValue.Items.Add(twr_.Name);
                comboBoxQueryReelTowerIdValue.Items.Add(twr_.Name);
            }

            foreach (ReelDiameters type_ in Enum.GetValues(typeof(ReelDiameters)))
            {
                if (type_ <= ReelDiameters.Unknown)
                    continue;

                name_ = type_.ToString().Remove(0, "ReelDiameter".Length) + " \"";
                comboBoxLoadReelSizeValue.Items.Add(name_);
                comboBoxQueryReelSizeValue.Items.Add(name_);
            }

            foreach (ReelThicknesses type_ in Enum.GetValues(typeof(ReelThicknesses)))
            {
                if (type_ <= ReelThicknesses.Unknown)
                    continue;

                name_ = type_.ToString().Remove(0, "ReelThickness".Length) + " mm";
                comboBoxLoadReelThicknessValue.Items.Add(name_);

            }

            foreach (ComparisonOperators type_ in Enum.GetValues(typeof(ComparisonOperators)))
            {
                switch (type_)
                {
                    case ComparisonOperators.EqualTo:
                        name_ = Properties.Resources.String_FormMain_Compare_Operator_EqualTo;
                        break;
                    case ComparisonOperators.NotEqualTo:
                        name_ = Properties.Resources.String_FormMain_Compare_Operator_NotEqualTo;
                        break;
                    case ComparisonOperators.LessThanOrEqualTo:
                        name_ = Properties.Resources.String_FormMain_Compare_Operator_LessThanOrEqualTo;
                        break;
                    case ComparisonOperators.LessThan:
                        name_ = Properties.Resources.String_FormMain_Compare_Operator_LessThan;
                        break;
                    case ComparisonOperators.GreaterThanOrEqualTo:
                        name_ = Properties.Resources.String_FormMain_Compare_Operator_GreaterThanOrEqualTo;
                        break;
                    case ComparisonOperators.GreaterThan:
                        name_ = Properties.Resources.String_FormMain_Compare_Operator_GreaterThan;
                        break;
                }

                comboBoxQueryDatetimeOptionValue.Items.Add(name_);
                comboBoxQueryReelQtyOptionValue.Items.Add(name_);
            }

            foreach (KeyValuePair<string, PropertyNode> property_ in Config.Properties)
            {
                enabled_ = property_.Value.Enabled;
                switch (property_.Value.ValueType)
                {
                    case TypeCode.Boolean:
                        index_ = Convert.ToBoolean(property_.Value.Value) ? 1 : 0;
                        break;
                    case TypeCode.Int32:
                        data_ = Convert.ToInt32(property_.Value.Value);
                        break;
                    case TypeCode.String:
                        name_ = Convert.ToString(property_.Value.Value);
                        break;
                }

                switch (property_.Key)
                {
                    case "MaterialValidation":
                        {
                            checkBoxMaterialValidation.Checked = enabled_;
                            comboBoxMaterialValidationValue.SelectedIndex = index_;
                        }
                        break;
                    case "MaterialArriveReport":
                        {
                            checkBoxMaterialArriveReport.Checked = enabled_;
                            comboBoxMaterialArriveReportValue.SelectedIndex = index_;
                        }
                        break;
                    case "MaterialRemoveReport":
                        {
                            checkBoxMaterialRemoveReport.Checked = enabled_;
                            comboBoxMaterialRemoveReportValue.SelectedIndex = index_;
                        }
                        break;
                    case "ReelHandlerUsage":
                        {
                            checkBoxReelHandlerUsage.Checked = enabled_;
                            comboBoxReelHandlerUsageValue.SelectedIndex = index_;
                        }
                        break;
                    case "LoadDelayTimeByManual":
                        {
                        }
                        break;
                    case "IntervalOfReelHandlerPing":
                        break;
                    case "TimeoutOfReelHandlerResponse":
                        break;
                    case "LimitOfRetry":
                        break;
                    case "JobSplitReelCount":
                        {
                            checkBoxJobSplitReelCount.Checked = enabled_;
                            numericUpDownJobSplitReelCountValue.Value = data_;
                        }
                        break;
                    case "TimeoutOfReject":
                        {
                            checkBoxTimeoutOfReject.Checked = enabled_;
                            numericUpDownTimeoutOfRejectValue.Value = data_;
                        }
                        break;
                    case "AssignedRejectPort":
                        {
                            checkBoxAssignedRejectPort.Checked = enabled_;

                            if (comboBoxAssignedRejectPortValue.Items.Count <= 0)
                            {
                                foreach (int id_ in Config.OutStageIds.Keys)
                                    comboBoxAssignedRejectPortValue.Items.Add(id_.ToString());

                                foreach (int id_ in Config.RejectStageIds.Keys)
                                    comboBoxAssignedRejectPortValue.Items.Add((Config.OutStageIds.Count + id_).ToString());
                            }

                            comboBoxAssignedRejectPortValue.SelectedItem = data_.ToString();
                        }
                        break;
                    case "RemapCreateTimeByMFG":
                        {
                            checkBoxRemapCreateTimeByMFG.Checked = enabled_;
                            comboBoxRemapCreateTimeByMFGValue.SelectedIndex = index_;
                        }
                        break;
                    case "ProvideMode":
                        {
                            checkBoxProvideMode.Checked = enabled_;
                            comboBoxProvideModeValue.SelectedItem = name_;
                        }
                        break;
                    case "AMMUsage":
                        {
                            checkBoxAMMUsage.Checked = enabled_;
                            comboBoxAMMUsageValue.SelectedIndex = index_;
                        }
                        break;
                    case "RejectAutoUsage":
                        {
                            checkBoxRejectAutoUsage.Checked = enabled_;
                            comboBoxRejectAutoUsageValue.SelectedIndex = index_;
                        }
                        break;
                    case "AMMWebserviceUsage":
                        {
                            checkBoxAMMWebserviceUsage.Checked = enabled_;
                            comboBoxAMMWebserviceUsageValue.SelectedIndex = index_;
                        }
                        break;

                }
            }

            foreach (PropertyNode timeout_ in Model.Timeouts)
            {
                enabled_ = timeout_.Enabled;
                name_ = Convert.ToString(timeout_.Value);

                switch (timeout_.Name)
                {
                    case "ReelTowerCommunicationTimeout":
                        {
                            checkBoxUnloadRejectMaterial.Checked = enabled_;
                            textBoxUnloadRejectMaterialValue.Text = Convert.ToString(timeout_.Value);
                        }
                        break;
                    case "ReelHandlerCommunicationTimeout":
                        {
                            checkBoxNotifyRejectFull.Checked = timeout_.Enabled;
                            textBoxNotifyRejectFullValue.Text = Convert.ToString(timeout_.Value);
                        }
                        break;
                    case "ReelHandlerActionTimeout":
                        {
                            checkBoxDelayLoadStart.Checked = timeout_.Enabled;
                            textBoxDelayLoadStartValue.Text = Convert.ToString(timeout_.Value);
                        }
                        break;
                }
            }


            index_ = 0;
            dataGridViewTower.DataSource = null;


            foreach (ReelTower reeltower_ in mainSequence.ReelTowerGroupObject.Towers.Values)
            {
                int comboxindex = -1;
                if (reeltower_.Usage.Equals("true"))
                    comboxindex = 0;
                else
                    comboxindex = 1;


                dataGridViewTower.Rows.Add(
                    reeltower_.Index,
                    reeltower_.Name);

                DataGridViewComboBoxCell cbcell_ = (DataGridViewComboBoxCell)dataGridViewTower.Rows[index_++].Cells[2];
                cbcell_.Value = cbcell_.Items[comboxindex].ToString();

            }


            index_ = 0;
            dataGridViewReelTowerNetworks.DataSource = null;

            foreach (NetworkNode network_ in Config.Networks)
            {
                if (network_.Endpoint != null)
                {
                    dataGridViewReelTowerNetworks.Rows.Add(
                        network_.Name,
                        network_.Endpoint.Address,
                        network_.Endpoint.Port);
                }
                else
                {
                    dataGridViewReelTowerNetworks.Rows.Add(
                        network_.Name,
                        network_.Uri,
                        string.Empty);
                }
            }

            index_ = 0;
            dataGridViewDatabase.DataSource = null;
            dataGridViewDatabase.Rows.Add(
                ++index_,
                mainSequence.ReelTowerGroupObject.ComponentDB.DBServer,
                mainSequence.ReelTowerGroupObject.ComponentDB.Name,
                mainSequence.ReelTowerGroupObject.ComponentDB.DBFile,
                mainSequence.ReelTowerGroupObject.ComponentDB.DBUser);
            dataGridViewDatabase.Rows.Add(
                ++index_,
                mainSequence.ReelTowerGroupObject.AccountDB.DBServer,
                mainSequence.ReelTowerGroupObject.AccountDB.Name,
                mainSequence.ReelTowerGroupObject.AccountDB.DBFile,
                mainSequence.ReelTowerGroupObject.AccountDB.DBUser);

            index_ = 0;
            dataGridViewDevices.DataSource = null;

            foreach (DeviceNode device_ in Config.Devices)
            {
                dataGridViewDevices.Rows.Add(
                    ++index_,
                    device_.Name,
                    device_.HidUsage,
                    device_.HardwardId,
                    string.IsNullOrEmpty(device_.ComPortSetting.PortName) ? string.Empty : device_.ComPortSetting.PrintOut(),
                    string.IsNullOrEmpty(device_.EthernetPortSetting.IpAddress) ? string.Empty : device_.EthernetPortSetting.PrintOut());
            }

            index_ = 0;
            dataGridViewCombineModules.DataSource = null;

            foreach (CombineModule module_ in Singleton<CombineModuleManager>.Instance.Modules)
            {
                dataGridViewCombineModules.Rows.Add(
                    ++index_,
                    module_.Name,
                    module_.Path,
                    module_.Caption);
            }
        }

        protected void CreateElements()
        {
            CreateFileSystem();
            splashWorker.SetProgress(30);
            mainSequence = new ReelTowerGroupSequence();

            foreach (string item in comboBoxDisplayLanguage.Items)
            {
                if (item.Contains(App.CultureInfoCode))
                {
                    comboBoxDisplayLanguage.Text = item;
                    break;
                }
            }

            if (backgroundWorker.IsBusy != true)
                backgroundWorker.RunWorkerAsync();

            //if (backgroundWorkerAMMAlive.IsBusy != true)
            //    backgroundWorkerAMMAlive.RunWorkerAsync();

            RedrawControls();
            SetControlData();
            UpdateOperationStatus();
            splashWorker.SetProgress(80);
        }

        protected void DestroyElements()
        {
            try
            {
                RemoveAllLoadReelNotification();
                shutdownEvent.Set();
                Config.Save();
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }
            finally
            {
                if (mainSequence != null)
                    mainSequence.Dispose();

                Logger.Trace($"Stop application (Name={App.Name}, Version={App.Version})");
                Logger.Destroy();
            }
        }

        protected void AttachEventHandlers()
        {
            Singleton<TransferMaterialObject>.Instance.AttachEventHander(OnChangedTransferMaterialInformation);
            Singleton<MaterialPackageManager>.Instance.AddedMaterialPackage += OnAddedMaterialPackage;

            mainSequence.InitializeProvideJos += OnInitializeProvideJobs;
            mainSequence.AddPickingJobs += OnAddPickingJobs;
            mainSequence.NotifyReelTowerGroupRequestLoad += OnNotifyReelTowerGroupRequestLoad;

            mainSequence.OperationStateChanged += OnOperationStateChanged;
            mainSequence.FinishedCycleStop += OnFinishedCycleStop;
            mainSequence.CycleStopOrderStateChanged += OnCycleStopOrderStateChanged;
            mainSequence.NotifyEvent += OnNotificationOfMainSequence;
            mainSequence.NotifyProductionInformation += OnProductionInformation;

            // Reel tower event handlers
            mainSequence.ReelTowerGroupObject.ReelTowerStateChanged += OnReelTowerStateChanged;
            mainSequence.ReelTowerGroupObject.MaterialEventRaised += OnReelTowerMaterialEventRaised;
            mainSequence.ReelTowerGroupObject.ProvideJobStateChanged += OnProvideJobStateChanged;
            mainSequence.ReelTowerGroupObject.ProvideMaterialStateChanged += OnProvideMaterialStateChanged;
            mainSequence.ReelTowerGroupObject.ReportRuntimeLog += OnReelTowerGroupRuntimeLog;
            mainSequence.ReelTowerGroupObject.ReportException += OnReelTowerGroupRuntimeLog;
            mainSequence.ReelTowerGroupObject.ReportAlarmLog += OnReelTowerGroupAlarmLog;
            mainSequence.ReportReelTowerGroupNotification += OnReelTowerGroupNotification;

            // Reel handler event handlers
            mainSequence.ReelHandlerObject.ReportRuntimeLog += OnReelHandlerRuntimeLog;
            mainSequence.ReelHandlerObject.ReportException += OnReelHandlerRuntimeLog;
            mainSequence.ReelHandlerObject.CommunicationStateChanged += OnChangedReelHandlerCommunicationState;
            mainSequence.ReelHandlerObject.RequestCommandReceived += OnRequestCommandReceived;

            mainSequence.AMMCommunicationStateChanged += OnChangedAMMCommunicationState;
        }

        protected void DetachEventHandlers()
        {
            Singleton<TransferMaterialObject>.Instance.DetachEventHandler(OnChangedTransferMaterialInformation);
            Singleton<MaterialPackageManager>.Instance.AddedMaterialPackage -= OnAddedMaterialPackage;

            mainSequence.InitializeProvideJos -= OnInitializeProvideJobs;
            mainSequence.NotifyReelTowerGroupRequestLoad -= OnNotifyReelTowerGroupRequestLoad;

            mainSequence.OperationStateChanged -= OnOperationStateChanged;
            mainSequence.FinishedCycleStop -= OnFinishedCycleStop;
            mainSequence.CycleStopOrderStateChanged -= OnCycleStopOrderStateChanged;
            mainSequence.NotifyEvent -= OnNotificationOfMainSequence;
            mainSequence.NotifyProductionInformation -= OnProductionInformation;

            // Reel tower event handlers
            mainSequence.ReelTowerGroupObject.ReelTowerStateChanged -= OnReelTowerStateChanged;
            mainSequence.ReelTowerGroupObject.MaterialEventRaised -= OnReelTowerMaterialEventRaised;
            mainSequence.ReelTowerGroupObject.ProvideJobStateChanged -= OnProvideJobStateChanged;
            mainSequence.ReelTowerGroupObject.ProvideMaterialStateChanged -= OnProvideMaterialStateChanged;
            mainSequence.ReelTowerGroupObject.ReportRuntimeLog -= OnReelTowerGroupRuntimeLog;
            mainSequence.ReelTowerGroupObject.ReportException -= OnReelTowerGroupRuntimeLog;
            mainSequence.ReelTowerGroupObject.ReportAlarmLog -= OnReelTowerGroupAlarmLog;
            mainSequence.ReportReelTowerGroupNotification -= OnReelTowerGroupNotification;

            // Reel handler event handlers
            mainSequence.ReelHandlerObject.ReportRuntimeLog -= OnReelHandlerRuntimeLog;
            mainSequence.ReelHandlerObject.ReportException -= OnReelHandlerRuntimeLog;
            mainSequence.ReelHandlerObject.CommunicationStateChanged -= OnChangedReelHandlerCommunicationState;
            mainSequence.ReelHandlerObject.RequestCommandReceived -= OnRequestCommandReceived;
        }

        protected void SetVariables()
        {
            try
            {
                checkBoxReelTowerCommunicationTimeout.Checked = Model.IsTimeoutEnabled("ReelTowerCommunicationTimeout");
                checkBoxReelHandlerCommunicationTimeout.Checked = Model.IsTimeoutEnabled("ReelHandlerCommunicationTimeout");
                checkBoxReelHandlerActionTimeout.Checked = Model.IsTimeoutEnabled("ReelHandlerActionTimeout");
                textBoxReelTowerCommunicationTimeoutValue.Text = $"{Model.GetTimeout("ReelTowerCommunicationTimeout")}";
                textBoxReelHandlerCommunicationTimeoutValue.Text = $"{Model.GetTimeout("ReelHandlerCommunicationTimeout")}";
                textBoxReelHandlerActionTimeoutValue.Text = $"{Model.GetTimeout("ReelHandlerActionTimeout")}";
                checkBoxUnloadRejectMaterial.Checked = Model.IsPropertyEnabled(ProcessCategories.MaterialStorage, "Mycronic", "UnloadRejectMaterial");
                checkBoxNotifyRejectFull.Checked = Model.IsPropertyEnabled(ProcessCategories.MaterialStorage, "Mycronic", "NotifyRejectFull");
                checkBoxDelayLoadStart.Checked = Model.IsPropertyEnabled(ProcessCategories.MaterialStorage, "Mycronic", "DelayLoadStart");
                textBoxUnloadRejectMaterialValue.Text = Model.GetProperty(ProcessCategories.MaterialStorage, "Mycronic", "UnloadRejectMaterial").ToString();
                textBoxNotifyRejectFullValue.Text = Model.GetProperty(ProcessCategories.MaterialStorage, "Mycronic", "NotifyRejectFull").ToString();
                textBoxDelayLoadStartValue.Text = Model.GetProperty(ProcessCategories.MaterialStorage, "Mycronic", "DelayLoadStart").ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }
        #endregion

        #region Update status methods
        protected void RemoveProvideJobListOnList(string jobname)
        {
            List<DataGridViewRow> rows_ = dataGridViewQueuedJobs.Rows
                .Cast<DataGridViewRow>()
                .Where(r => r.Cells["Jobname"].Value.ToString().Equals(jobname)).ToList();

            if (rows_ != null && rows_.Count > 0)
            {
                foreach (DataGridViewRow row_ in rows_)
                    dataGridViewQueuedJobs.Rows.Remove(row_);
            }

            listViewPendedReels.Items.Clear();
        }

        protected void RemoveProvideJobListOnList(DataGridViewRow row)
        {
            if (row != null)
                dataGridViewQueuedJobs.Rows.Remove(row);
        }

        protected void RemoveProvideJobListOnList(int index)
        {
            if (index <= dataGridViewQueuedJobs.Rows.Count && index >= 0)
                dataGridViewQueuedJobs.Rows.RemoveAt(index);
        }

        protected void UpdateCurrentProcessingJobInformation(ProvideJobListData job)
        {
            switch (job.State)
            {
                case ProvideJobListData.States.Providing:
                    {
                        labelProcessingReelJobNameValue.Text = job.Name;
                        labelProcessingReelUserValue.Text = job.User;
                        labelProcessingReelDstValue.Text = Config.OutStageIds.ContainsKey(job.Outport) ? Config.OutStageIds[job.Outport] : job.Outport.ToString();
                        listViewPendedReels.BeginUpdate();

                        if (listViewPendedReels.Items.Count > 0 && listViewPendedReels.Items.Count <= job.Materials.Count)
                        {
                            foreach (ProvideMaterialData item_ in job.Materials)
                            {
                                ListViewItem element_ = listViewPendedReels.FindItemWithText(item_.Name, true, 0);

                                if (element_ != null)
                                {
                                    element_.SubItems[1].Text = item_.Category;
                                    element_.SubItems[2].Text = item_.Name;
                                    element_.SubItems[3].Text = item_.Supplier;
                                    element_.SubItems[4].Text = item_.Quantity.ToString();
                                    element_.SubItems[5].Text = item_.State.ToString();
                                }
                            }
                        }
                        else
                        {
                            foreach (ProvideMaterialData item_ in job.Materials)
                            {
                                int index_ = listViewPendedReels.Items.Count;
                                listViewPendedReels.Items.Add(Convert.ToString(index_));
                                listViewPendedReels.Items[index_].SubItems.Add(item_.Category);
                                listViewPendedReels.Items[index_].SubItems.Add(item_.Name);
                                listViewPendedReels.Items[index_].SubItems.Add(item_.Supplier);
                                listViewPendedReels.Items[index_].SubItems.Add(item_.Quantity.ToString());
                                listViewPendedReels.Items[index_].SubItems.Add(item_.State.ToString());
                            }
                        }

                        listViewPendedReels.EndUpdate();
                    }
                    break;
            }
        }

        protected void UpdateCurrentProcessingMaterialInformation(ProvideMaterialData material)
        {
            if (string.IsNullOrEmpty(material.TowerId))
                return;

            ReelTowerState state_ = mainSequence.ReelTowerGroupObject.GetTowerStateById(material.TowerId);
            labelProcessingReelTowerIdValud.Text = material.TowerId;
            labelProcessingReelSrcValue.Text = material.Depot;
            labelProcessingReelArticleValue.Text = material.Category;
            labelProcessingReelLotIdValue.Text = material.LotId;
            labelProcessingReelMfgValue.Text = material.ManufacturedDatetime;
            labelProcessingReelTowerStatusValue.Text = state_ == null ? string.Empty : state_.State.ToString();
            labelProcessingReelCarrierValue.Text = material.Name;
            labelProcessingReelSupplierValue.Text = material.Supplier;
            labelProcessingReelQtyValue.Text = material.Quantity.ToString();

            if (listViewPendedReels.Items.Count > 0)
            {
                ListViewItem item_ = listViewPendedReels.FindItemWithText(material.Name, true, 0);

                if (item_ != null)
                {
                    item_.SubItems[5].Text = material.State.ToString();

                    if (material.State == ProvideMaterialData.States.Completed && !Config.ReelHandlerUsage)
                        FormMessageExt.ShowInformation(string.Format(Properties.Resources.String_Information_TakeOutProvidedReel, material.Name, mainSequence.GetTowerNameById(material.TowerId)), Buttons.Ok, true);
                }
            }
        }

        protected void ClearCurrentProcessingInformation(string jobname)
        {
            if (labelProcessingReelJobNameValue.Text == jobname)
            {
                labelProcessingReelJobNameValue.Text = string.Empty;
                labelProcessingReelTowerIdValud.Text = string.Empty;
                labelProcessingReelSrcValue.Text = string.Empty;
                labelProcessingReelArticleValue.Text = string.Empty;
                labelProcessingReelLotIdValue.Text = string.Empty;
                labelProcessingReelMfgValue.Text = string.Empty;
                labelProcessingReelUserValue.Text = string.Empty;
                labelProcessingReelTowerStatusValue.Text = string.Empty;
                labelProcessingReelDstValue.Text = string.Empty;
                labelProcessingReelCarrierValue.Text = string.Empty;
                labelProcessingReelSupplierValue.Text = string.Empty;
                labelProcessingReelQtyValue.Text = string.Empty;
            }
        }
        #endregion

        #region Operation button handler
        protected void OnClickButtonExit(object sender, EventArgs e)
        {
            MaterialStorageState.MaterialHandlingDestination orderstate_ = MaterialStorageState.MaterialHandlingDestination.None;

            if (App.OperationState == OperationStates.Run)
            {
                FormMessageExt.ShowInformation(Properties.Resources.String_Information_Stop_Process_First, Buttons.Ok, true);
                return;
            }

            if (mainSequence.IsPossibleShutdown(ref orderstate_))
            {
                switch (orderstate_)
                {
                    case MaterialStorageState.MaterialHandlingDestination.UnloadToOutStage:
                        {
                            if (FormMessageExt.ShowQuestion(string.Format(Properties.Resources.String_FormMain_Question_ProvidJob_IsNot_Completed, Environment.NewLine)) == DialogResult.No)
                                return;
                        }
                        break;
                    case MaterialStorageState.MaterialHandlingDestination.LoadToStorage:
                        {
                            if (FormMessageExt.ShowQuestion(string.Format(Properties.Resources.String_FormMain_Question_StoreOperation_IsNot_Completed, Environment.NewLine)) == DialogResult.No)
                                return;
                        }
                        break;
                }

                if (FormMessageExt.ShowQuestion(Properties.Resources.String_FormMain_Question_Exit_Program) == DialogResult.No)
                    return;

                mainSequence.FlushProvideJobs();
                Exit_DeleteProvideJobs();
            }
            else
            {
                FormMessageExt.ShowInformation(Properties.Resources.String_FormMain_Exit_Condition_WaitForProperStop, Buttons.Ok, true);
                return;
            }

            mainSequence.SetEqEnd();
            Logger.TraceKeyAndMouseEvent(sender as Control, e);
            shutdownApp = true;
            Close();
        }
        protected void Exit_DeleteProvideJobs()
        {
            bool result_ = true;

            if (dataGridViewQueuedJobs.Rows.Count > 0)
            {
                for (int i = dataGridViewQueuedJobs.Rows.Count - 1; i > -1; i--)
                {
                    if (result_)
                    {
                        if (dataGridViewQueuedJobs.Rows[i].Cells["State"].Value.ToString().ToLower() != "completed")
                            mainSequence.CancelJob(dataGridViewQueuedJobs.Rows[i].Cells["Jobname"].Value.ToString());

                        if (mainSequence.RemoveJob(dataGridViewQueuedJobs.Rows[i].Cells["Jobname"].Value.ToString()))
                            RemoveProvideJobListOnList(dataGridViewQueuedJobs.Rows[i].Cells["Jobname"].Value.ToString());
                    }
                }


            }
        }
        protected void OnClickButtonReset(object sender, EventArgs e)
        {
            Logger.TraceKeyAndMouseEvent(sender as Control, e);
            mainSequence.Reset();
        }

        protected void OnClickButtonInitialize(object sender, EventArgs e)
        {
            Logger.TraceKeyAndMouseEvent(sender as Control, e);
            mainSequence.Initialize();
        }

        protected void OnClickButtonConnectRobotController(object sender, EventArgs e)
        {
            try
            {
                mainSequence.TryReelHandlerConnect(Config.ReelHandlerEndPoint);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected void OnClickButtonAutoRun(object sender, EventArgs e)
        {
            Logger.TraceKeyAndMouseEvent(sender as Control, e);

            if (!App.CycleStop)
            {
                switch (buttonAutoRun.Text)
                {
                    case "AUTO":
                        {
                            if ((mainSequence as ReelTowerGroupSequence).ReelTowerGroupObject.IsProviding)
                            {
                                if (FormMessageExt.ShowQuestion(Properties.Resources.String_Question_Operation_Mode_Change_During_In_Providing) == DialogResult.No)
                                    return;
                            }

                            App.Stop();
                        }
                        break;
                    case "STOP":
                        App.Start();
                        break;
                }
            }
            else
                ShowModallessNotificationWindow("Cycle stop!", true);

            this.ActiveControl = tabControlMain;
            tabControlMain.Focus();
        }
        #endregion

        #region Event handlers
        #region Status notification event handlers
        protected void UpdateOperationStatus()
        {
            labelOperationState.Text = App.OperationState.ToString().ToUpper();
            labelOperationState.ForeColor = SystemColors.Window;

            switch (App.OperationState)
            {
                case OperationStates.Alarm:
                    {
                        labelOperationState.BackColor = Color.Red;
                        labelOperationState.ForeColor = Color.Yellow;
                    }
                    break;
                case OperationStates.PowerOff:
                    {
                        labelOperationState.BackColor = Color.Black;
                    }
                    break;
                case OperationStates.PowerOn:
                    {
                        labelOperationState.BackColor = Color.Gray;
                    }
                    break;
                case OperationStates.Initialize:
                case OperationStates.Setup:
                    {
                        labelOperationState.BackColor = SystemColors.HotTrack;
                    }
                    break;
                case OperationStates.Run:
                    {
                        labelOperationState.BackColor = Color.Green;
                        tabControlMain.Enabled = tabControlOperation.Enabled = radioButtonLoginOperator.Enabled = radioButtonSearchReel.Enabled = radioButtonJobMonitor.Enabled = true;
                        buttonAutoRun.Text = "AUTO";
                        buttonAutoRun.ForeColor = Color.Green;
                        buttonAutoRun.BackColor = Color.LightGreen;
                        groupBoxStoreReel.Enabled = Config.ReelHandlerUsage ? false : true;
                        groupBoxQueryReels.Enabled = Config.ReelHandlerUsage ? false : true;
                        groupBoxMaterialInformation.Visible = false;
                        UpdateSpecialMaterials();
                    }
                    break;
                case OperationStates.Stop:
                    {
                        labelOperationState.BackColor = Color.Red;
                        buttonAutoRun.Text = "STOP";
                        buttonAutoRun.ForeColor = SystemColors.Window;
                        buttonAutoRun.BackColor = Color.Red;
                        groupBoxStoreReel.Enabled = true;
                        groupBoxQueryReels.Enabled = true;
                        groupBoxMaterialInformation.Visible = true;
                    }
                    break;
                case OperationStates.Pause:
                    {
                        labelOperationState.BackColor = Color.Yellow;
                    }
                    break;
            }

            if (mainSequence.OperationState == OperationStates.Stop)
            {
                groupBoxGuiSettings.Enabled = groupBoxTimeoutProperties.Enabled = groupBoxNetworks.Enabled =
                groupBoxDatabases.Enabled = groupBoxDevices.Enabled = groupBoxCombineModules.Enabled =
                groupBoxConfig.Enabled = groupBoxModel.Enabled = buttonSaveModel.Enabled =
                buttonSaveConfig.Enabled = buttonSaveNetworks.Enabled = buttonRemoveJob.Enabled =
                buttonCleanUpMaterials.Enabled = groupBoxTower.Enabled = buttonSaveTower.Enabled = true;
            }
            else
            {
                groupBoxGuiSettings.Enabled = groupBoxTimeoutProperties.Enabled = groupBoxNetworks.Enabled =
                groupBoxDatabases.Enabled = groupBoxDevices.Enabled = groupBoxCombineModules.Enabled =
                groupBoxConfig.Enabled = groupBoxModel.Enabled = buttonSaveModel.Enabled =
                buttonSaveConfig.Enabled = buttonSaveNetworks.Enabled = buttonRemoveJob.Enabled =
                buttonCleanUpMaterials.Enabled = groupBoxTower.Enabled = buttonSaveTower.Enabled = false;
            }
        }

        protected void UpdateFinishedCycleStop()
        {
            buttonAutoRun.Text = Properties.Resources.String_FormMain_buttonStart;
            buttonAutoRun.BackColor = SystemColors.ButtonFace;
            FormMessageExt.ShowInformation(Properties.Resources.String_FormMain_Information_Completed_CycleStop);
        }

        protected void UpdateStopState(bool cyclestop = false)
        {
            if (cyclestop)
            {
                buttonAutoRun.Text = Properties.Resources.String_FormMain_buttonStart_CycleStop;
                buttonAutoRun.BackColor = SystemColors.ButtonFace;
                FormMessageExt.ShowNotification(Properties.Resources.String_FormMain_Information_Set_CycleStop);
            }
            else
            {
                buttonAutoRun.Text = Properties.Resources.String_FormMain_buttonStart;
                buttonAutoRun.BackColor = SystemColors.ButtonFace;
                FormMessageExt.ShowNotification(Properties.Resources.String_FormMain_Information_Reset_CycleStop);
            }
        }

        protected void ShowModallessNotificationWindow(string msg, bool dynamic)
        {
            try
            {
                if (dynamic)
                {
                    FormMessageExt dlg_ = new FormMessageExt(msg, Properties.Resources.String_Notification, Buttons.Ok, Icons.Asterisk, true, 10000, true);
                    dlg_.FormClosed += OnClosedDynamicNotificationWindow;
                    dlg_.SetMessageWithBuzzer(msg, Properties.Resources.String_Notification, false);
                    dlg_.Show();
                }
                else
                {
                    if (notificationWindow == null)
                    {
                        notificationWindow = new FormMessageExt(msg, Properties.Resources.String_Notification, Buttons.Ok, Icons.Asterisk, true, 10000, true);
                        notificationWindow.FormClosed += OnClosedNotificationWindow;
                    }
                    else if (notificationWindow.IsDisposed)
                    {
                        notificationWindow = null;
                    }

                    if (notificationWindow != null)
                    {
                        notificationWindow.SetMessageWithBuzzer(msg, Properties.Resources.String_Notification, false);
                        notificationWindow.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected void OnClosedDynamicNotificationWindow(object sender, FormClosedEventArgs e)
        {
        }

        protected void OnClosedNotificationWindow(object sender, FormClosedEventArgs e)
        {
            notificationWindow = null;
        }

        protected void UpdateProcessingMaterial()
        {
        }

        protected void OnOperationStateChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => { UpdateOperationStatus(); }));
            else
                UpdateOperationStatus();
        }

        protected void OnCycleStopOrderStateChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => { UpdateStopState(App.CycleStop); }));
            else
                UpdateStopState(App.CycleStop);
        }

        protected void OnFinishedCycleStop(object sender, EventArgs e)
        {
            if (mainSequence != null && App.CycleStop)
            {
                mainSequence.FinishCycleStop();

                if (InvokeRequired)
                    BeginInvoke(new Action(() => { UpdateFinishedCycleStop(); }));
                else
                    UpdateFinishedCycleStop();
            }
        }

        protected void OnNotificationOfMainSequence(object sender, Pair<string, bool> arg)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => { ShowModallessNotificationWindow(arg.first, arg.second); }));
            else
                ShowModallessNotificationWindow(arg.first, arg.second);
        }

        protected void OnProductionInformation(object sender, EventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => { UpdateProcessingMaterial(); }));
            else
                UpdateProcessingMaterial();
        }
        #endregion

        #region Reel tower event handlers
        protected void UpdateReelTowerStates(ReelTowerState e)
        {
            if (e != null)
            {
                string strTitle_ = $"labelTower{e.Index}Status";
                string strValue_ = $"labelTower{e.Index}StatusValue";

                switch (e.Index)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        {
                            Control ctrlTitle_ = tableLayoutPanelTitle.Controls[strTitle_];
                            Control ctrlValue_ = tableLayoutPanelTitle.Controls[strValue_];

                            if (ctrlTitle_ != null && ctrlValue_ != null)
                            {
                                if (e.OnlineState && mainSequence.ReelTowerGroupObject.Towers[e.Index].Usage.Equals("true"))
                                    ctrlTitle_.BackColor = SystemColors.HotTrack;
                                else
                                    ctrlTitle_.BackColor = SystemColors.ControlDarkDark;

                                if (e.HasAlarm)
                                {
                                    ctrlValue_.Text = e.AlarmCode.ToString();
                                    ctrlValue_.BackColor = Color.Red;
                                    ctrlValue_.ForeColor = Color.Yellow;
                                }
                                else
                                {
                                    ctrlValue_.Text = e.State.ToString();
                                    ctrlValue_.BackColor = SystemColors.Window;
                                    ctrlValue_.ForeColor = SystemColors.ControlText;
                                }
                            }
                        }
                        break;
                }
            }
        }

        protected void AddLoadReelNotification(string towerid, string towername, MaterialData obj, bool show = true)
        {
            if (obj != null && !managedLoadReelNotifications.ContainsKey(obj.Name))
            {
                lock (managedLoadReelNotifications)
                {
                    FormLoadMaterialInformation notification_ = new FormLoadMaterialInformation(towerid, towername, obj, Config.ReelHandlerUsage && checkBoxTakeoutByRobot.Checked && App.OperationState == OperationStates.Run, !Config.ReelHandlerUsage && Config.LoadDelayTimeByManual > 0);
                    notification_.FormClosed += OnNofigyReelTowerGroupRequestLoadClose;
                    managedLoadReelNotifications.Add(obj.Name, notification_);
                }

                if (show)
                    managedLoadReelNotifications[obj.Name].Show();
            }
        }

        protected void RemoveLoadReelNotification(string carriername, bool closed = false)
        {
            if (managedLoadReelNotifications.ContainsKey(carriername))
            {
                lock (managedLoadReelNotifications)
                {
                    if (!closed)
                        managedLoadReelNotifications[carriername].Close();

                    managedLoadReelNotifications[carriername] = null;
                    managedLoadReelNotifications.Remove(carriername);
                }
            }
        }

        protected void RemoveAllLoadReelNotification()
        {
            foreach (Form form_ in managedLoadReelNotifications.Values)
            {
                if (form_ != null && !form_.IsDisposed)
                    form_.Close();
            }

            lock (managedLoadReelNotifications)
                managedLoadReelNotifications.Clear();
        }

        protected void UpdateReelTowerMaterialState(MaterialEventArgs e)
        {
            if (e != null && e.Data != null)
            {
                MaterialData obj_ = e.Data as MaterialData;
                AddLoadReelNotification(e.Equipment, e.Port, obj_, !Config.AMMUsage);
            }
        }

        protected void UpdateProvideJobStateChanged(ProvideJobListData e)
        {
            if (e != null)
            {
                if (dataGridViewQueuedJobs.Rows.Count > 0)
                {
                    string log_ = string.Empty;
                    List<DataGridViewRow> items_ = dataGridViewQueuedJobs.Rows
                        .Cast<DataGridViewRow>()
                        .Where(r_ => r_.Cells["Jobname"].Value.ToString().Equals(e.Name)).ToList();

                    if (items_ != null && items_.Count > 0)
                    {
                        foreach (DataGridViewRow item_ in items_)
                        {
                            item_.Cells["State"].Value = e.State;
                            log_ = $"Jobname={e.Name},State={e.State},Materials=";

                            foreach (ProvideMaterialData material_ in e.Materials)
                                log_ += $"{material_.Category};{material_.Name};{material_.State}|";

                            switch (e.State)
                            {
                                case ProvideJobListData.States.Failed:
                                case ProvideJobListData.States.Created:
                                case ProvideJobListData.States.Providing:
                                    {
                                        UpdateCurrentProcessingJobInformation(e);

                                        if (e.State == ProvideJobListData.States.Failed)
                                            FormMessageExt.ShowNotification(string.Format(Properties.Resources.String_FormMain_Message_RemoveFailedTheProvideJob, e.Name));
                                    }
                                    break;
                                case ProvideJobListData.States.Canceled:
                                case ProvideJobListData.States.Completed:
                                    {
                                    }
                                    break;
                                case ProvideJobListData.States.Deleted:
                                    {
                                        RemoveProvideJobListOnList(e.Name);
                                        ClearCurrentProcessingInformation(e.Name);
                                    }
                                    break;
                            }

                            UpdateProvideJobLog(log_);
                        }
                    }
                }
            }
        }

        protected void UpdateProvideJobLog(string log)
        {
            try
            {
                if (string.IsNullOrEmpty(log))
                    return;

                Logger.Record(log, "Job");

                lock (listBoxProvideJobLog)
                {
                    if (listBoxProvideJobLog.Items.Count > 1000)
                        listBoxProvideJobLog.Items.RemoveAt(0);

                    listBoxProvideJobLog.Items.Add($"{DateTime.Now.ToString("HH:mm:ss.fff")}> {log}");
                    listBoxProvideJobLog.SelectedIndex = listBoxProvideJobLog.Items.Count > 0 ? listBoxProvideJobLog.Items.Count - 1 : 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                MessageBox.Show($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected void UpdateReelTowerGroupNotification(Pair<ReelTowerGroup.NotificationCodes, string[]> arg)
        {
            try
            {
                if (arg.second == null || arg.second.Length < 3)
                    return;

                DateTime procTimestamp_ = DateTime.Now;
                int index_ = listViewReelTowerGroupNotifications.Items.Count;

                listViewReelTowerGroupNotifications.BeginUpdate();

                if (receivedNotificationIndex >= int.MaxValue)
                    receivedNotificationIndex = 1;

                if (arg.first > Service.MYCRONIC.WebService.ReelTowerGroup.NotificationCodes.Unknown && arg.second.Length >= 3)
                {
                    DateTime sentTimestamp_ = Convert.ToDateTime(arg.second[1]);
                    DateTime receivedTimestamp_ = Convert.ToDateTime(arg.second[0]);

                    listViewReelTowerGroupNotifications.Items.Add(Convert.ToString(++receivedNotificationIndex));
                    listViewReelTowerGroupNotifications.Items[index_].SubItems.Add(arg.second[1]); // Sent
                    listViewReelTowerGroupNotifications.Items[index_].SubItems.Add(arg.second[0]); // Received
                    listViewReelTowerGroupNotifications.Items[index_].SubItems.Add(procTimestamp_.ToString("yyyy-MM-dd HH:mm:ss.fff")); // Processed
                    listViewReelTowerGroupNotifications.Items[index_].SubItems.Add(Convert.ToInt32((procTimestamp_ - sentTimestamp_).TotalMilliseconds).ToString());
                    listViewReelTowerGroupNotifications.Items[index_].SubItems.Add(arg.second[2]);
                    listViewReelTowerGroupNotifications.Items[index_].SubItems.Add((arg.second.Length >= 259) ? arg.second[3].Substring(0, 250) + "..." : arg.second[3]);

                    if (listViewReelTowerGroupNotifications.Items.Count > 100)
                    {
                        for (int i_ = listViewReelTowerGroupNotifications.Items.Count; i_ > 100; i_--)
                            listViewReelTowerGroupNotifications.Items.RemoveAt(0);

                        index_ = listViewReelTowerGroupNotifications.Items.Count - 1;
                    }

                    listViewReelTowerGroupNotifications.Items[index_].Selected = true;
                    listViewReelTowerGroupNotifications.EnsureVisible(index_);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
            finally
            {
                listViewReelTowerGroupNotifications.Items[listViewReelTowerGroupNotifications.Items.Count - 1].EnsureVisible();
                listViewReelTowerGroupNotifications.EndUpdate();
            }
        }

        protected void UpdateReportProvideMaterialState(string[] arg)
        {
        }

        protected void UpdateProvideJobStateChanged(string[] arg)
        {
        }

        protected void OnNofigyReelTowerGroupRequestLoadClose(object sender, FormClosedEventArgs e)
        {
            if (sender is FormLoadMaterialInformation)
            {   // It should be not called by default.
                if (Config.ReelHandlerUsage &&
                    checkBoxTakeoutByRobot.Checked &&
                    Config.RejectAutoUsage &&
                    (sender as FormLoadMaterialInformation).DialogResult == DialogResult.Abort)
                {
                    mainSequence.RequestRejectLoadAbortedMaterial((sender as FormLoadMaterialInformation).TowerId,
                        (sender as FormLoadMaterialInformation).TowerName,
                        $"reject_{(sender as FormLoadMaterialInformation).TowerName}_{(sender as FormLoadMaterialInformation).Carrier}");
                }

                RemoveLoadReelNotification((sender as FormLoadMaterialInformation).Carrier, true);
                ClearStoreReelContexts();
            }
        }

        protected void UpdateProvideJobList(Pair<string, ProvideJobListData> arg)
        {
            if (arg != null)
            {
                int index_ = 0;

                if (dataGridViewQueuedJobs.Columns.Count <= 0)
                {
                    dataGridViewQueuedJobs.Columns.Add("Jobname", "Jobname");
                    dataGridViewQueuedJobs.Columns.Add("User", "User");
                    dataGridViewQueuedJobs.Columns.Add("Outport", "Outport");
                    dataGridViewQueuedJobs.Columns.Add("Reels", "Reels");
                    dataGridViewQueuedJobs.Columns.Add("State", "State");
                }

                dataGridViewQueuedJobs.Rows.Add(arg.first,
                    arg.second.User,
                    arg.second.Outport.ToString(),
                    arg.second.Materials.Count,
                    arg.second.State);

                if (dataGridViewQueuedJobs.Rows.Count == 1)
                {
                    listViewPendedReels.Items.Clear();
                    listViewPendedReels.BeginUpdate();

                    foreach (ProvideMaterialData item_ in arg.second.Materials)
                    {
                        index_ = listViewPendedReels.Items.Count;
                        listViewPendedReels.Items.Add(Convert.ToString(index_));
                        listViewPendedReels.Items[index_].SubItems.Add(item_.Category);
                        listViewPendedReels.Items[index_].SubItems.Add(item_.Name);
                        listViewPendedReels.Items[index_].SubItems.Add(item_.Supplier);
                        listViewPendedReels.Items[index_].SubItems.Add(item_.Quantity.ToString());
                        listViewPendedReels.Items[index_].SubItems.Add(item_.State.ToString());
                    }

                    listViewPendedReels.EndUpdate();
                }
            }
        }

        protected void OnInitializeProvideJobs(object sender, Pair<string, ProvideJobListData> arg)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    UpdateProvideJobList(arg);
                }));
            }
            else
            {
                UpdateProvideJobList(arg);
            }
        }

        protected void OnAddPickingJobs(object sender, DataTable jobtbl_)
        {
            if (dataGridViewQueuedJobs.Columns.Count <= 0)
            {
                dataGridViewQueuedJobs.Columns.Add("Jobname", "Jobname");
                dataGridViewQueuedJobs.Columns.Add("User", "User");
                dataGridViewQueuedJobs.Columns.Add("Outport", "Outport");
                dataGridViewQueuedJobs.Columns.Add("Reels", "Reels");
                dataGridViewQueuedJobs.Columns.Add("State", "State");
            }

            //dataGridViewQueuedJobs.Rows.Add(jobtbl_.Rows[0][3].ToString(), mainSequence.User_check(jobtbl_.Rows[0][5].ToString()), stages_.First().Key, containers_[index_].Count, ProvideJobListData.States.Created.ToString()) >= 0);
        }

        protected void OnNotifyReelTowerGroupRequestLoad(object sender, params string[] args)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    if (args.Length >= 2)
                        RemoveLoadReelNotification(args[1]);
                }));
            }
            else
            {
                if (args.Length >= 2)
                    RemoveLoadReelNotification(args[1]);
            }
        }

        protected void OnReelTowerStateChanged(object sender, ReelTowerState e)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => { UpdateReelTowerStates(e); }));
            else
                UpdateReelTowerStates(e);
        }

        protected void OnReelTowerMaterialEventRaised(object sender, MaterialEventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => { UpdateReelTowerMaterialState(e); }));
            else
                UpdateReelTowerMaterialState(e);
        }

        protected void OnProvideJobStateChanged(object sender, ProvideJobListData e)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => { UpdateProvideJobStateChanged(e); }));
            else
                UpdateProvideJobStateChanged(e);
        }

        protected void OnProvideMaterialStateChanged(object sender, ProvideMaterialData e)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => { UpdateCurrentProcessingMaterialInformation(e); }));
            else
                UpdateCurrentProcessingMaterialInformation(e);
        }

        protected void OnReelTowerGroupNotification(object sender, Pair<ReelTowerGroup.NotificationCodes, string[]> e)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => { UpdateReelTowerGroupNotification(e); }));
            else
                UpdateReelTowerGroupNotification(e);
        }

        protected void OnReportProvideMaterialState(object sender, string[] e)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => { UpdateReportProvideMaterialState(e); }));
            else
                UpdateReportProvideMaterialState(e);
        }

        protected void OnProvideJobStateChanged(object sender, string[] e)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => { UpdateProvideJobStateChanged(e); }));
            else
                UpdateProvideJobStateChanged(e);
        }
        #endregion

        #region Reel tower group event handlers
        public void UpdateReelTowerRuntimeLog(string text)
        {
            try
            {
                Logger.Record(text, "Tower");

                lock (listBoxReelTowerComm)
                {
                    if (listBoxReelTowerComm.Items.Count > 1000)
                        listBoxReelTowerComm.Items.RemoveAt(0);

                    listBoxReelTowerComm.Items.Add($"{DateTime.Now.ToString("HH:mm:ss.fff")}> {text}");
                    listBoxReelTowerComm.SelectedIndex = listBoxReelTowerComm.Items.Count > 0 ? listBoxReelTowerComm.Items.Count - 1 : 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                MessageBox.Show($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public void UpdateReelTowerAlarmLog(FiveField<bool, string, string, int, string> arg)
        {
            try
            {
                string text_ = (arg.first ? $"Set Alarm={arg.fourth}," : $"Clear Alarm={arg.fourth},") + $"TowerName={arg.second},TowerId={arg.third},Message={arg.fifth}";
                Logger.Record(text_, "Alarm");

                lock (listBoxReelTowerComm)
                {
                    if (listBoxReelTowerComm.Items.Count > 1000)
                        listBoxReelTowerComm.Items.RemoveAt(0);

                    listBoxReelTowerComm.Items.Add($"{DateTime.Now.ToString("HH:mm:ss.fff")}> {text_}");
                    listBoxReelTowerComm.SelectedIndex = listBoxReelTowerComm.Items.Count > 0 ? listBoxReelTowerComm.Items.Count - 1 : 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                MessageBox.Show($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }
        #endregion

        #region Reel handler event handlers
        protected void UpdateReelHandlerRuntimeLog(string text)
        {
            try
            {
                Logger.Record(text, "Robot");

                lock (listBoxReelHandlerComm)
                {
                    listBoxReelHandlerComm.Items.Add($"{DateTime.Now.ToString("HH:mm:ss.fff")}> {text}");
                    listBoxReelHandlerComm.SelectedIndex = listBoxReelHandlerComm.Items.Count > 0 ? listBoxReelHandlerComm.Items.Count - 1 : 0;

                    if (listBoxReelHandlerComm.Items.Count > 1000)
                        listBoxReelHandlerComm.Items.RemoveAt(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                MessageBox.Show($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected void UpdateReelHandlerCommunicationState(Control displayControl, CommunicationStates state)
        {
            try
            {
                lock (displayControl)
                {
                    displayControl.ForeColor = SystemColors.ControlText;

                    switch (state)
                    {
                        case CommunicationStates.None:
                            {
                                displayControl.BackColor = SystemColors.Window;
                                displayControl.Text = string.Empty;
                            }
                            break;
                        case CommunicationStates.Listening:
                            {
                                displayControl.BackColor = SystemColors.Info;
                                displayControl.Text = Properties.Resources.String_FormMain_Communication_State_Listen;
                            }
                            break;
                        case CommunicationStates.Connecting:
                            {
                                displayControl.BackColor = Color.LightBlue;
                                displayControl.Text = Properties.Resources.String_FormMain_Communication_State_Connecting;
                            }
                            break;
                        case CommunicationStates.Accepted:
                            {
                                displayControl.BackColor = Color.LightGreen;
                                displayControl.Text = Properties.Resources.String_FormMain_Communication_State_Accepted;
                            }
                            break;
                        case CommunicationStates.Connected:
                            {
                                displayControl.BackColor = Color.Lime;
                                displayControl.Text = Properties.Resources.String_FormMain_Communication_State_Connected;
                            }
                            break;
                        case CommunicationStates.Disconnected:
                            {
                                displayControl.BackColor = SystemColors.ControlDarkDark;
                                displayControl.ForeColor = SystemColors.Window;
                                displayControl.Text = Properties.Resources.String_FormMain_Communication_State_Disconnected;
                            }
                            break;
                        case CommunicationStates.Error:
                            {
                                displayControl.BackColor = Color.Red;
                                displayControl.ForeColor = Color.Yellow;
                                displayControl.Text = Properties.Resources.String_FormMain_Communication_State_Socket_Error;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected void UpdateAMMCommunicationState(Control displayControl, CommunicationStates state)
        {
            try
            {
                lock (displayControl)
                {
                    displayControl.ForeColor = SystemColors.ControlText;

                    switch (state)
                    {
                        case CommunicationStates.Connected:
                            {
                                displayControl.BackColor = Color.Lime;
                                displayControl.Text = Properties.Resources.String_FormMain_Communication_State_Connected;
                            }
                            break;
                        case CommunicationStates.Disconnected:
                            {
                                displayControl.BackColor = SystemColors.ControlDarkDark;
                                displayControl.ForeColor = SystemColors.Window;
                                displayControl.Text = Properties.Resources.String_FormMain_Communication_State_Disconnected;
                                if (amm_close)
                                {
                                    mainSequence.Pause(ErrorCode.AMMConnectFailure);
                                    amm_close = false;
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected void UpdateRequestCommand(ReelTowerCommands command, MaterialStorageMessage message)
        {
            mainSequence.ProcessReelHandlerRequest(command, message);
        }

        protected void OnReelTowerGroupRuntimeLog(object sender, string text)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => { UpdateReelTowerRuntimeLog(text); }));
            else
                UpdateReelTowerRuntimeLog(text);
        }

        protected void OnReelTowerGroupAlarmLog(object sender, FiveField<bool, string, string, int, string> arg)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => { UpdateReelTowerAlarmLog(arg); }));
            else
                UpdateReelTowerAlarmLog(arg);
        }

        protected void OnChangedReelHandlerCommunicationState(object sender, CommunicationStates state)
        {
            if (state == currentReelHandlerCommState)
                return;

            if (InvokeRequired)
                BeginInvoke(new Action(() => { UpdateReelHandlerCommunicationState(labelRobotState, state); }));
            else
                UpdateReelHandlerCommunicationState(labelRobotState, state);
        }

        protected void OnChangedAMMCommunicationState(object sender, CommunicationStates state)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => { UpdateAMMCommunicationState(labelRobotState, state); }));
            else
                UpdateAMMCommunicationState(labelRobotState, state);
        }

        protected void OnReelHandlerRuntimeLog(object sender, string text)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => { UpdateReelHandlerRuntimeLog(text); }));
            else
                UpdateReelHandlerRuntimeLog(text);
        }

        protected void OnRequestCommandReceived(object sender, Pair<ReelTowerCommands, MaterialStorageMessage> obj)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => { UpdateRequestCommand(obj.first, obj.second); }));
            else
                UpdateRequestCommand(obj.first, obj.second);
        }
        #endregion

        #region Display status handlers
        protected void UpdateReelHandlerStatus()
        {
            string reelsize_ = string.Empty;

            if (mainSequence.ReelHandlerObject.IsConnected)
            {
                labelRobotState.BackColor = Color.Lime;
                labelRobotState.ForeColor = SystemColors.ControlText;

                switch (mainSequence.ReelHandlerObject.ActionState)
                {
                    case RobotActionStates.Stop:
                        labelRobotState.Text = Properties.Resources.String_FormMain_Robot_Action_State_Stopped;
                        break;
                    case RobotActionStates.Load:
                        labelRobotState.Text = Properties.Resources.String_FormMain_Robot_Action_State_Do_Load;
                        break;
                    case RobotActionStates.Loading:
                        labelRobotState.Text = Properties.Resources.String_FormMain_Robot_Action_State_Do_Loading;
                        break;
                    case RobotActionStates.LoadCompleted:
                        labelRobotState.Text = Properties.Resources.String_FormMain_Robot_Action_State_Completed_Load;
                        break;
                    case RobotActionStates.Unload:
                        labelRobotState.Text = Properties.Resources.String_FormMain_Robot_Action_State_Do_Unload;
                        break;
                    case RobotActionStates.Unloading:
                        labelRobotState.Text = Properties.Resources.String_FormMain_Robot_Action_State_Do_Unloading;
                        break;
                    case RobotActionStates.UnloadCompleted:
                        labelRobotState.Text = Properties.Resources.String_FormMain_Robot_Action_State_Completed_Unload;
                        break;
                }
            }
            else
            {
                labelRobotState.BackColor = Color.Red;
                labelRobotState.ForeColor = Color.Yellow;
                labelRobotState.Text = Properties.Resources.String_FormMain_Robot_Action_State_Failed;
            }
        }

        protected void UpdateProductionRecord()
        {
        }

        protected void UpdateDisplay()
        {
            #region Update elapsed time
            string elapsed_ = TimeSpan.FromMilliseconds((DateTime.Now - startedDateTime).TotalMilliseconds).ToString();
            int pos_ = elapsed_.LastIndexOf(".");
            labelElapsedValue.Text = elapsed_.Remove(pos_ + 2, elapsed_.Length - pos_ - 2);
            #endregion

            if (mainSequence.CommunicationStatesOfWebService != currentReelTowerWebServiceState)
            {
                UpdateReelHandlerCommunicationState(labelWebServiceStatusValue, mainSequence.CommunicationStatesOfWebService);
                // Asynchronous udp listen socket
                if (mainSequence.CommunicationStatesOfWebService == CommunicationStates.Listening)
                    UpdateReelHandlerCommunicationState(labelWebServiceStatusValue, CommunicationStates.Connected);

                currentReelTowerWebServiceState = mainSequence.CommunicationStatesOfWebService;
            }

            UpdateAMMCommunicationState(labelAMMStatusValue, mainSequence.CommunicationStatesOfAMM);
            UpdateReelHandlerStatus();
            UpdateProductionRecord();
        }
        #endregion

        protected void ClearMaterialPackage()
        {
        }

        protected void UpdateMaterialPackage(MaterialPackage pkg)
        {
        }

        protected void OnReceivedMaterialPackageByRemote(object sender, MaterialPackage pkg)
        {
            OnAddedMaterialPackage(sender, pkg);
            Singleton<MaterialPackageManager>.Instance.AddMaterialPackage(pkg, false, false);
        }

        protected void OnAddedMaterialPackage(object sender, MaterialPackage pkg)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => { UpdateMaterialPackage(pkg); }));
            }
            else
            {
                UpdateMaterialPackage(pkg);
            }
        }

        protected void OnRemovedMaterialPackage(object sender, string name)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => {
                    ClearMaterialPackage();
                    Singleton<MaterialPackageManager>.Instance.DeleteMaterialPackage(name);
                }));
            }
            else
            {
                ClearMaterialPackage();
                Singleton<MaterialPackageManager>.Instance.DeleteMaterialPackage(name);
            }
        }

        protected void OnRemoveAllMaterialPackages(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => {
                    ClearMaterialPackage();
                    Singleton<MaterialPackageManager>.Instance.ClearMaterialPackages();
                }));
            }
            else
            {
                ClearMaterialPackage();
                Singleton<MaterialPackageManager>.Instance.ClearMaterialPackages();
            }
        }

        protected void OnChangedTransferMaterialInformation(object sender, EventArgs e)
        {
        }

        protected void OnLoginStateChanged(object sender, EventArgs e)
        {
        }

        protected void OnTabControlDrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabcontrol = (sender as TabControl);
            TabPage page = tabcontrol.TabPages[e.Index];
            e.Graphics.FillRectangle(new SolidBrush((tabcontrol.SelectedIndex == e.Index) ? SystemColors.HotTrack : SystemColors.GradientActiveCaption), e.Bounds);

            Rectangle paddedBounds = e.Bounds;
            this.Font = new Font("Arial", 14, FontStyle.Bold);
            TextRenderer.DrawText(e.Graphics, page.Text, Font, paddedBounds, SystemColors.HighlightText);
        }

        protected void OnRefreshReelTowerServerState(object sender, EventArgs e)
        {
        }

        protected void OnClearLog(object sender, EventArgs e)
        {
            if (tabControlLogPage.SelectedTab != null)
            {
                if (tabControlLogPage.SelectedTab.Name == "tabPageAlarmHistory")
                {
                    UpdateAlarmHistory();
                }
                else
                {
                    foreach (Control ctrl_ in tabControlLogPage.SelectedTab.Controls)
                    {
                        if (ctrl_ is ListBox)
                            (ctrl_ as ListBox).Items.Clear();
                    }
                }
            }
        }

        protected void OnLogTabIndexChanged(object sender, EventArgs e)
        {
            if (tabControlLogPage.SelectedTab != null)
            {
                if (tabControlLogPage.SelectedTab.Name.ToLower().Contains("alarm"))
                    buttonClearLog.Text = Properties.Resources.String_Refresh;
                else
                    buttonClearLog.Text = Properties.Resources.String_Clear;
            }
        }

        protected string[] GetAlarmFolders(string logpath)
        {
            return Directory.GetDirectories(logpath, "*.*", SearchOption.AllDirectories);
        }

        protected string[] GetAlarmFiles(string logpath)
        {
            return Directory.GetFiles(logpath, "[Alarm]*.log", SearchOption.AllDirectories);
        }

        protected void UpdateAlarmHistory()
        {
            try
            {
                treeViewLog.BeginUpdate();

                string rootpath = string.Format(@"{0}Log", App.Path);
                char[] delimiters = { '\\' };
                treeViewLog.Nodes[0].Nodes.Clear();
                string[] folders = GetAlarmFolders(rootpath);
                string[] files = GetAlarmFiles(rootpath);

                foreach (string folder in folders)
                {
                    string[] subnames = folder.Remove(0, rootpath.Length).Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    switch (subnames.Length)
                    {
                        case 1: // year nodes
                            {
                                treeViewLog.Nodes[0].Nodes.Add(subnames[0], subnames[0]);
                            }
                            break;
                        case 2: // moth nodes
                            {
                                if (treeViewLog.Nodes[0].Nodes.Find(subnames[0], false) == null)
                                    treeViewLog.Nodes[0].Nodes.Add(subnames[0], subnames[0]);

                                treeViewLog.Nodes[0].Nodes[subnames[0]].Nodes.Add(subnames[1], subnames[1], 4, 5);
                            }
                            break;
                    }
                }

                foreach (string file in files)
                {
                    string[] subnames = file.Remove(0, rootpath.Length).Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    TreeNode[] yearnodes = treeViewLog.Nodes[0].Nodes
                        .Cast<TreeNode>()
                        .Where(r => r.Name == subnames[0])
                        .ToArray();

                    if (yearnodes.Length > 0)
                    {
                        if (subnames.Length >= 3)
                        {
                            TreeNode[] monthnodes = treeViewLog.Nodes[0].Nodes[subnames[0]].Nodes.Find(subnames[1], false);

                            if (monthnodes.Length > 0)
                                monthnodes[0].Nodes.Add(file, subnames[2], 7, 7);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format($"{GetType()}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}"));
            }
            finally
            {
                treeViewLog.EndUpdate();
            }
        }

        protected void UploadAlarmLog(string logfile)
        {
            try
            {
                if (!string.IsNullOrEmpty(logfile))
                {
                    if (File.Exists(logfile))
                    {
                        string[] records = File.ReadAllLines(logfile);

                        listBoxAlarmHistory.Items.Clear();
                        listBoxAlarmHistory.Items.AddRange(records);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format($"{GetType()}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}"));
            }
        }

        protected void OnAfterSelectNode(object sender, TreeViewEventArgs e)
        {
            try
            {
                treeViewLog.Enabled = false;

                if (listBoxAlarmHistory.InvokeRequired)
                    listBoxAlarmHistory.BeginInvoke(new Action(() => { UploadAlarmLog(e.Node.Name); }));
                else
                    UploadAlarmLog(e.Node.Name);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format($"{GetType()}.{MethodBase.GetCurrentMethod()}: Exception={ex.Message}"));
            }
            finally
            {
                treeViewLog.Enabled = true;
            }
        }

        protected void UpdateReturnReelState()
        {
        }

        protected void OnDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                while (!shutdownApp)
                {
                    if (!shutdownEvent.WaitOne(100) && mainSequence != null)
                    {
                        if (InvokeRequired)
                        {
                            BeginInvoke(new Action(() => {
                                UpdateReturnReelState();
                                UpdateDisplay();
                            }));
                        }
                        else
                        {
                            UpdateReturnReelState();
                            UpdateDisplay();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected void OnRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Canceled");
            }
            else if (e.Error != null)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Error={e.Error.Message}");
            }
            else
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Done");
            }
        }
        #endregion

        #region Save model information
        protected void OnClickButtonSaveModel(object sender, EventArgs e)
        {
            try
            {
                // Model.AlignmentRangeLimit = new PointF(Convert.ToSingle(textBoxVisionAlignmentRangeLimitX.Text),
                //     Convert.ToSingle(textBoxVisionAlignmentRangeLimitY.Text));
                // Model.RetryOfVisionFailure = Convert.ToInt32(textBoxVisionFailureRetry.Text);
                // Model.DelayOfImageProcessing = Convert.ToInt32(textBoxImageProcessingDelay.Text);
                // Model.DelayOfReturnReelSensing = Convert.ToInt32(textBoxReturnReelSensingDelay.Text);
                // Model.DelayOfUnloadReelStateUpdate = Convert.ToInt32(textBoxUnloadReelStateUpdateDelay.Text);
                // Model.RetryOfVisionAttempts = Convert.ToInt32(textBoxVisioinRetryAttempts.Text);
                // Model.TimeoutOfImageProcessing = Convert.ToInt32(textBoxImageProcessingTimeout.Text);
                // Model.DelayOfTrigger = Convert.ToInt32(textBoxDelayOfCameraTrigger.Text);

                Model.Save();
            }
            catch (Exception ex)
            {
                Logger.Trace($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
            finally
            {
                mainSequence.LoadParameters();
            }
        }
        #endregion

        #region Restart networks
        protected void OnDoubleClickRobotController(object sender, EventArgs e) { }

        protected void OnDoubleClickReelTowerManager(object sender, EventArgs e)
        {
            if (!Singleton<CombineModuleManager>.Instance.CheckProcess("SMDTSQL"))
                combineModuleState = Singleton<CombineModuleManager>.Instance.Start("SMDTSQL");
        }
        #endregion

        #region Picking list draw item
        protected void OnListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            if (sender is ListBox)
            {
                if (e.Index < 0)
                    return;

                ListBox lb = (ListBox)sender;

                if (e.Index == 0)
                    e.Graphics.FillRectangle(new SolidBrush(SystemColors.Desktop), lb.ClientRectangle);

                if ((e.State & DrawItemState.Focus) == DrawItemState.Focus ||
                    (e.State & DrawItemState.Selected) == DrawItemState.Selected ||
                    (e.State & DrawItemState.HotLight) == DrawItemState.HotLight)
                {
                    e.Graphics.FillRectangle(new SolidBrush(SystemColors.Desktop), e.Bounds);
                    e.DrawBackground();
                }
                else
                {
                    using (Brush backgroundBrush = new SolidBrush(SystemColors.Desktop))
                    {
                        e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
                    }
                }

                e.Graphics.DrawString(lb.Items[e.Index].ToString(), lb.Font, Brushes.White,
                    new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));

                if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
                    e.DrawFocusRectangle();
            }
        }
        #endregion

        #region Culture setting
        protected void SetDisplayLanguage()
        {
            labelLoginOperator.Text = Properties.Resources.String_FormMain_labelLoginOperator;
            // buttonAutoRun.Text = Properties.Resources.String_Stop;
            // buttonExit.Text = Properties.Resources.String_Exit;
            buttonClearLog.Text = Properties.Resources.String_Clear;
            buttonClearLoadReelRequest.Text = Properties.Resources.String_Clear;
            buttonClearQueryReels.Text = Properties.Resources.String_Clear;
            buttonRequestLoadReel.Text = Properties.Resources.String_Request;
            buttonExecuteQueryReels.Text = buttonQueryUser.Text = Properties.Resources.String_Query;
            buttonRemoveUser.Text = Properties.Resources.String_Remove;
            buttonAddNewUser.Text = Properties.Resources.String_Create;
            buttonAccountManagerLogin.Text = Properties.Resources.String_Login;
            buttonSearchReel.Text = Properties.Resources.String_Search;
            buttonAssignReel.Text = Properties.Resources.String_Assign;
            buttonEnqueueJob.Text = Properties.Resources.String_Enqueue;
            buttonRemoveJob.Text = Properties.Resources.String_Remove;
            buttonCleanUpMaterials.Text = Properties.Resources.String_CleanUp;
            checkBoxTakeoutByRobot.Text = Properties.Resources.String_AutoMode;

            tabPageProvideOperation.Text = Properties.Resources.String_FormMain_tabPageProvideOperation;
            tabPageStoreOperation.Text = Properties.Resources.String_FormMain_tabPageStoreOperation;
            tabPageAccount.Text = Properties.Resources.String_FormMain_tabPageAccount;
            tabPageReelTowerManager.Text = Properties.Resources.String_FormMain_tabPageReelTowerManager;
            tabPageReelTowerNotifier.Text = Properties.Resources.String_FormMain_tabPageReelTowerNotifier;
            tabPageConfig.Text = Properties.Resources.String_FormMain_tabPageConfig;
            tabPageLog.Text = Properties.Resources.String_FormMain_tabPageLog;
            tabPageReelTowerLog.Text = Properties.Resources.String_FormMain_tabPageReelTowerLog;
            tabPageProvideJobLog.Text = Properties.Resources.String_FormMain_tabPageProvideJobLog;
            tabPageRobotLog.Text = Properties.Resources.String_FormMain_tabPageRobotLog;
            tabPageAlarmHistory.Text = Properties.Resources.String_FormMain_tabPageAlarmHistory;
            tabPageAMM.Text = Properties.Resources.String_FormMain_tabPageAMM;

            radioButtonLoginOperator.Text = Properties.Resources.String_FormMain_tabPageProvideOperation_Operator;
            radioButtonSearchReel.Text = Properties.Resources.String_FormMain_tabPageProvideOperation_Search;
            radioButtonJobMonitor.Text = Properties.Resources.String_FormMain_tabPageProvideOperation_Monitor;

            splashWorker.SetProgress(100);
        }
        #endregion

        #region Monitor selection
        protected void GetCurrentMonitor()
        {
            Screen[] screens_ = Screen.AllScreens;

            for (int i = 0; i < screens_.Length; i++)
            {
                if (screens_[i].WorkingArea.Contains(this.Location))
                {
                    displayMonitor = screens_[i];
                    break;
                }
            }
        }

        public void SetDisplayMonitor(Form wnd)
        {
            // if (InvokeRequired)
            // {
            //     BeginInvoke(new Action(() =>
            //     {
            //         wnd.StartPosition = FormStartPosition.Manual;
            //         wnd.Location = new Point(displayMonitor.Bounds.Location.X + (displayMonitor.Bounds.Width - wnd.Width) / 2, displayMonitor.Bounds.Y + (displayMonitor.Bounds.Height - wnd.Height) / 2);
            //     }));
            // }
            // else
            // {
            //     wnd.StartPosition = FormStartPosition.Manual;
            //     wnd.Location = new Point(displayMonitor.Bounds.Location.X + (displayMonitor.Bounds.Width - wnd.Width) / 2, displayMonitor.Bounds.Y + (displayMonitor.Bounds.Height - wnd.Height) / 2);
            // }
        }
        #endregion

        #region Operation page
        protected void OnCheckedChangedRadioButtonPages(object sender, EventArgs e)
        {
            if (!(sender is RadioButton))
                return;

            Control ctl_ = Utility.FindControl(tabControlOperation, $"tabPageProvide{(sender as RadioButton).Name.Remove(0, ("radioButton").Length)}");

            if (ctl_ != null && (sender as RadioButton).Checked)
            {
                if ((ctl_.Name == "tabPageProvideLoginOperator" || ctl_.Name == "tabPageProvideSearchReel" || ctl_.Name == "tabPageProvideJobMonitor") &&
                    (!string.IsNullOrEmpty(mainSequence.CurrentOperator) || mainSequence.HasProvideJob))
                {
                    if (tabControlMain.SelectedTab != tabPageProvideOperation)
                        tabControlMain.SelectedTab = tabPageProvideOperation;

                    if (tabControlOperation.SelectedTab.Name == "tabPageProvideJobMonitor" && (sender as RadioButton).Name.Contains("SearchReel"))
                    {
                        radioButtonLoginOperator.PerformClick();
                    }
                    else
                    {
                        tabControlOperation.SelectedTab = (ctl_ as TabPage);
                        textBoxLoginOperatorValue.Text = string.Empty;

                        switch (ctl_.Name)
                        {
                            case "tabPageProvideLoginOperator":
                                textBoxLoginOperatorValue.Focus();
                                break;
                            case "tabPageProvideSearchReel":
                                {
                                    comboBoxAssignJobOutportValue.SelectedIndex = mainSequence.ReservedOutPort;
                                    textBoxSearchReelIdValue.Focus();
                                }
                                break;
                            case "tabPageProvideJobMonitor":
                                dataGridViewQueuedJobs.Focus();
                                break;
                        }
                    }
                }
                else
                {
                    radioButtonLoginOperator.PerformClick();
                }
            }
            else
            {
                if (string.IsNullOrEmpty(mainSequence.CurrentOperator))
                    radioButtonLoginOperator.PerformClick();
            }
        }
        #endregion

        #region Page change methods
        protected void OnSelectedIndexChangedTabControlMain(object sender, EventArgs e)
        {
            buttonClearLog.Visible = false;

            switch (tabControlMain.SelectedTab.Name)
            {
                case "tabPageStoreOperation":
                    {
                        textBoxLoadReelArticleValue.Focus();
                    }
                    break;
                case "tabPageReelTowerNotifier":
                    {
                        if (listViewReelTowerGroupNotifications.Columns.Count <= 0)
                        {
                            int width_ = (listViewReelTowerGroupNotifications.Width - 760);
                            listViewReelTowerGroupNotifications.Columns.Add("No", 60, HorizontalAlignment.Center);
                            listViewReelTowerGroupNotifications.Columns.Add("Sent", 200, HorizontalAlignment.Center);
                            listViewReelTowerGroupNotifications.Columns.Add("Received", 200, HorizontalAlignment.Center);
                            listViewReelTowerGroupNotifications.Columns.Add("Processed", 200, HorizontalAlignment.Center);
                            listViewReelTowerGroupNotifications.Columns.Add("Delay", 100, HorizontalAlignment.Center);
                            listViewReelTowerGroupNotifications.Columns.Add("Command", 200, HorizontalAlignment.Left);
                            listViewReelTowerGroupNotifications.Columns.Add("Parameter", width_, HorizontalAlignment.Left);
                            listViewReelTowerGroupNotifications.Update();
                        }
                    }
                    break;
                case "tabPageReelTowerManager":
                    {
                        mainSequence.CombineModuleManager.ForceCoimbineModule(1);
                    }
                    break;
                case "tabPageLog":
                    {
                        buttonClearLog.Visible = true;
                    }
                    break;
                case "tabPageConfig":
                    {
                    }
                    break;
                case "tabPageAMM":
                    {
                        mainSequence.CombineModuleManager.ForceCoimbineModule(2);
                    }
                    break;
            }
        }
        #endregion
        #endregion

        #region Public methods        
        public void OnReportRobotRuntimeLog(object sender, string text)
        {

            if (listBoxReelTowerComm.InvokeRequired)
                listBoxReelTowerComm.BeginInvoke(new Action(() => { UpdateRobotRuntimeLog(text); }));
            else
                UpdateRobotRuntimeLog(text);
        }

        public void UpdateRobotRuntimeLog(string text)
        {
            try
            {
                if (!text.Contains("\n"))
                    text += "\n";

                Logger.Record(text, "Robot");

                lock (listBoxReelHandlerComm)
                {
                    if (listBoxReelHandlerComm.Items.Count > 1000)
                        listBoxReelHandlerComm.Items.RemoveAt(0);

                    listBoxReelHandlerComm.Items.Add($"{DateTime.Now.ToString("HH:mm:ss.fff")}> {text}");
                    listBoxReelHandlerComm.SelectedIndex = listBoxReelHandlerComm.Items.Count > 0 ? listBoxReelHandlerComm.Items.Count - 1 : 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                MessageBox.Show($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public void UpdateTextFunc(Control ctrl, string text)
        {
            if (ctrl.InvokeRequired)
                ctrl.Invoke(new UpdateText(UpdateTextFunc), new object[] { ctrl, text });
            else
                ctrl.Text = text;
        }

        public void SetCulture(string culturecode)
        {
            if (appInstance != null)
            {
                appInstance.SetCultureCode(culturecode, false);
                Properties.Resources.Culture = new System.Globalization.CultureInfo(App.CultureInfoCode);
            }
        }

        #region ReelTower notification methods
        public virtual void RouteReelTowerNotification(object sender, string messages)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => {
                    if (mainSequence != null)
                        mainSequence.ProcessReelTowerNotifications($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")};{messages}");
                }));
            }
            else
            {
                if (mainSequence != null)
                    mainSequence.ProcessReelTowerNotifications($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")};{messages}");
            }
        }
        #endregion
        #endregion

        #region Store reel methods
        protected void ClearStoreReelContexts(bool button = false)
        {
            if (button)
            {
                comboBoxLoadReelTowerIdValue.SelectedItem = null;
                comboBoxLoadReelSizeValue.SelectedItem = null;
                comboBoxLoadReelThicknessValue.SelectedItem = null;
                comboBoxLoadReelTypeValue.SelectedItem = null;
            }
            textBoxLoadReelArticleValue.Text = string.Empty;
            textBoxLoadReelCarrierValue.Text = string.Empty;
            textBoxLoadReelLotIdValue.Text = string.Empty;
            textBoxLoadReelSupplierValue.Text = string.Empty;
            textBoxLoadReelMfgValue.Text = string.Empty;
            textBoxLoadReelCommentValue.Text = string.Empty;
            numericUpDownLoadReelQtyValue.Value = 0;

            if (!Config.ReelHandlerUsage && this.ActiveControl == tabControlMain)
                tabControlMain.Focus();
        }

        protected void OnClickButtonClearLoadReelRequest(object sender, EventArgs e)
        {
            ClearStoreReelContexts(true);
        }

        protected void OnClickButtonRequestLoadReel(object sender, EventArgs e)
        {
            string depot_ = string.Empty;
            bool result_ = false;

            if (comboBoxLoadReelTowerIdValue.SelectedItem == null ||
                comboBoxLoadReelSizeValue.SelectedItem == null ||
                comboBoxLoadReelThicknessValue.SelectedItem == null ||
                comboBoxLoadReelTypeValue.SelectedItem == null)
            {
                FormMessageExt.ShowNotification(Properties.Resources.String_Information_SelectProperMaterialInformation);
                return;
            }

            if (mainSequence.ReelTowerGroupObject.IsStoredMaterial(textBoxLoadReelArticleValue.Text, textBoxLoadReelCarrierValue.Text, ref depot_))
            {
                FormMessageExt.ShowNotification(depot_);
            }
            else
            {
                if (string.IsNullOrEmpty(depot_))
                {
                    if (!mainSequence.ReelTowerGroupObject.IsRecordExistCarrier(textBoxLoadReelCarrierValue.Text))
                    {

                        if (mainSequence.CreateCarrierInformation(
                            comboBoxLoadReelTowerIdValue.SelectedItem.ToString(),
                            textBoxLoadReelArticleValue.Text,
                            textBoxLoadReelCarrierValue.Text,
                            textBoxLoadReelLotIdValue.Text,
                            textBoxLoadReelSupplierValue.Text,
                            textBoxLoadReelMfgValue.Text,
                            textBoxLoadReelCommentValue.Text,
                            Convert.ToInt32(numericUpDownLoadReelQtyValue.Value),
                            GetSelectedReelDiameter(comboBoxLoadReelSizeValue.SelectedItem.ToString()),
                            (ReelThicknesses)Enum.GetValues(typeof(ReelThicknesses)).GetValue(comboBoxLoadReelThicknessValue.SelectedIndex),
                            (LoadMaterialTypes)Enum.GetValues(typeof(LoadMaterialTypes)).GetValue(comboBoxLoadReelTypeValue.SelectedIndex)))
                            result_ = true;
                    }
                    else
                    {
                        result_ = false;
                    }
                }
                else
                    FormMessageExt.ShowNotification(depot_);

                if (result_ && mainSequence.RequestToLoad(
                    comboBoxLoadReelTowerIdValue.SelectedItem.ToString(),
                    textBoxLoadReelArticleValue.Text,
                    textBoxLoadReelCarrierValue.Text,
                    textBoxLoadReelLotIdValue.Text,
                    textBoxLoadReelSupplierValue.Text,
                    textBoxLoadReelMfgValue.Text,
                    0,
                    string.Empty,
                    textBoxLoadReelCommentValue.Text,
                    Convert.ToInt32(numericUpDownLoadReelQtyValue.Value),
                    GetSelectedReelDiameter(comboBoxLoadReelSizeValue.SelectedItem.ToString()),
                    (ReelThicknesses)Enum.GetValues(typeof(ReelThicknesses)).GetValue(comboBoxLoadReelThicknessValue.SelectedIndex),
                    (LoadMaterialTypes)Enum.GetValues(typeof(LoadMaterialTypes)).GetValue(comboBoxLoadReelTypeValue.SelectedIndex)))
                {
                    FormMessageExt.ShowNotification(string.Format(Properties.Resources.String_Information_LoadReelOnReelTowerTerminal, textBoxLoadReelCarrierValue.Text));
                }
                else // Abnormal case
                    FormMessageExt.ShowNotification(string.Format(Properties.Resources.String_Information_FailedToCreateLoadRequest, textBoxLoadReelCarrierValue.Text));

            }

            if (Config.AMMUsage)
            {
                if (mainSequence.SetEqStatus("RUN", "RETURN", mainSequence.AMMBarcode_[comboBoxLoadReelTowerIdValue.SelectedItem.ToString()].Category, mainSequence.AMMBarcode_[comboBoxLoadReelTowerIdValue.SelectedItem.ToString()].Name) == "NG")
                {
                    mainSequence.CommunicationStatesOfAMM = CommunicationStates.Disconnected;
                    mainSequence.CommunicationWaitOfAMM = CommunicationStates.None;
                    Logger.Alarm($"AMM Alarm=SetEqStatus:NG");
                }
            }

            textBoxLoadReelArticleValue.Focus();
        }

        protected void OnClickButtonClearQueryReels(object sender, EventArgs e)
        {
            dataGridViewQueryResults.DataSource = null;
            comboBoxQueryReelTowerIdValue.SelectedItem = null;
            comboBoxQueryReelSizeValue.SelectedItem = null;
            comboBoxQueryDatetimeOptionValue.SelectedItem = null;
            comboBoxQueryReelQtyOptionValue.SelectedItem = null;
            dateTimePickerQueryReeCreateDateValue.Value = DateTime.Now;
            textBoxQueryReelArticleValue.Text = string.Empty;
            textBoxQueryReelCarrierValue.Text = string.Empty;
            textBoxQueryReelSupplierValue.Text = string.Empty;
            textBoxQueryLotIDValue.Text = string.Empty;
            numericUpDownQueryReelQtyValue.Value = 0;
            checkBoxQueryByTowerId.Checked = false;
            checkBoxQueryByReelSize.Checked = false;
            checkBoxQueryByLotID.Checked = false;
            checkBoxQueryByDatetime.Checked = false;
            checkBoxQueryByArticle.Checked = false;
            checkBoxQueryByCarrier.Checked = false;
            checkBox6QueryBySupplier.Checked = false;
            checkBoxQueryByQty.Checked = false;
            checkBoxQueryByCustomerDB.Checked = false;
        }

        protected ReelDiameters GetSelectedReelDiameter(string val)
        {
            if (string.IsNullOrEmpty(val))
                return ReelDiameters.Unknown;

            val = val.Replace(" \"", string.Empty);
            return (ReelDiameters)Enum.Parse(typeof(ReelDiameters), $"ReelDiameter{val}");
        }

        protected void OnClickButtonExecuteQueryReels(object sender, EventArgs e)
        {
            ExecuteQueryReel();
        }

        private DataSet ExecuteQueryReel(bool state = true)
        {
            string operator_ = string.Empty;
            DataSet result_ = null;
            List<Pair<string, string>> queryItems_ = new List<Pair<string, string>>();

            if (state && checkBoxQueryByCustomerDB.Checked)
            {
                return null;
            }
            else
            {
                if (checkBoxQueryByTowerId.Checked)
                {
                    if (comboBoxQueryReelTowerIdValue.SelectedItem == null)
                        return null;

                    queryItems_.Add(new Pair<string, string>("Carrier.Depot LIKE ", $"{comboBoxQueryReelTowerIdValue.SelectedItem.ToString()}"));
                }

                if (checkBoxQueryByReelSize.Checked)
                {
                    if (comboBoxQueryReelSizeValue.SelectedItem == null)
                        return null;

                    queryItems_.Add(new Pair<string, string>("Carrier.Diameter=", comboBoxQueryReelSizeValue.SelectedItem.ToString().Replace(" \"", string.Empty)));
                }

                if (checkBoxQueryByLotID.Checked)
                {
                    if (string.IsNullOrEmpty(textBoxQueryLotIDValue.Text))
                        return null;

                    if (textBoxQueryLotIDValue.Text.Length >= 5)
                        queryItems_.Add(new Pair<string, string>("Carrier.Custom1=", $"'{textBoxQueryLotIDValue.Text}'"));
                    else
                        queryItems_.Add(new Pair<string, string>("Carrier.Custom1 LIKE ", $"'%{textBoxQueryLotIDValue.Text}'"));
                }

                if (checkBoxQueryByDatetime.Checked)
                {
                    switch (comboBoxQueryDatetimeOptionValue.SelectedIndex)
                    {
                        case 0: operator_ = "Carrier.CreateDate="; break;
                        case 1: operator_ = "Carrier.CreateDate<>"; break;
                        case 2: operator_ = "Carrier.CreateDate<="; break;
                        case 3: operator_ = "Carrier.CreateDate<"; break;
                        case 4: operator_ = "Carrier.CreateDate>="; break;
                        case 5: operator_ = "Carrier.CreateDate>"; break;
                        default: return null;
                    }

                    queryItems_.Add(new Pair<string, string>(operator_, $"#{dateTimePickerQueryReeCreateDateValue.Value.Date.ToString("yyyy-MM-dd")}#"));
                }

                if (checkBoxQueryByArticle.Checked)
                {
                    if (string.IsNullOrEmpty(textBoxQueryReelArticleValue.Text))
                        return null;

                    if (textBoxQueryReelArticleValue.Text.Length >= 5)
                        queryItems_.Add(new Pair<string, string>("Carrier.Article=", textBoxQueryReelArticleValue.Text));
                    else
                        queryItems_.Add(new Pair<string, string>("Carrier.Article LIKE ", $"{textBoxQueryReelArticleValue.Text}"));
                }

                if (checkBoxQueryByCarrier.Checked)
                {
                    if (string.IsNullOrEmpty(textBoxQueryReelCarrierValue.Text))
                        return null;

                    if (textBoxQueryReelCarrierValue.Text.Length >= 5)
                        queryItems_.Add(new Pair<string, string>("Carrier.Carrier=", $"'{textBoxQueryReelCarrierValue.Text}'"));
                    else
                        queryItems_.Add(new Pair<string, string>("Carrier.Carrier LIKE ", $"'%{textBoxQueryReelCarrierValue.Text}'"));
                }

                if (checkBox6QueryBySupplier.Checked)
                {
                    if (string.IsNullOrEmpty(textBoxQueryReelSupplierValue.Text))
                        return null;

                    if (textBoxQueryReelSupplierValue.Text.Length >= 5)
                        queryItems_.Add(new Pair<string, string>("Carrier.Manufactur=", $"'{textBoxQueryReelSupplierValue.Text}'"));
                    else
                        queryItems_.Add(new Pair<string, string>("Carrier.Manufactur LIKE ", $"'%{textBoxQueryReelSupplierValue.Text}'"));
                }

                if (checkBoxQueryByQty.Checked)
                {
                    switch (comboBoxQueryReelQtyOptionValue.SelectedIndex)
                    {
                        case 0: operator_ = "Carrier.Stock="; break;
                        case 1: operator_ = "Carrier.Stock<>"; break;
                        case 2: operator_ = "Carrier.Stock<="; break;
                        case 3: operator_ = "Carrier.Stock<"; break;
                        case 4: operator_ = "Carrier.Stock>="; break;
                        case 5: operator_ = "Carrier.Stock>"; break;
                        default: return null;
                    }

                    queryItems_.Add(new Pair<string, string>(operator_, numericUpDownQueryReelQtyValue.Value.ToString()));
                }

                if ((result_ = mainSequence.QueryReelsByManual(queryItems_)) != null && result_.Tables.Count > 0)
                    dataGridViewQueryResults.DataSource = result_.Tables[0];

                return result_;
            }
        }
        protected void OnCheckedChangedQueryReels(object sender, EventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox ctrl_ = sender as CheckBox;

                switch (ctrl_.Name)
                {
                    case "checkBoxQueryByTowerId":
                        {
                            if (checkBoxQueryByCustomerDB.Checked == true)
                            {
                                if (ctrl_.Checked)
                                {
                                    comboBoxQueryReelTowerIdValue.Enabled = ctrl_.Checked;

                                    if (!comboBoxQueryReelTowerIdValue.Enabled)
                                        comboBoxQueryReelTowerIdValue.SelectedItem = null;

                                    checkBoxQueryByArticle.Checked = false;
                                    textBoxQueryReelArticleValue.Enabled = false;
                                    textBoxQueryReelArticleValue.Text = string.Empty;
                                    checkBoxQueryByCarrier.Checked = false;
                                    textBoxQueryReelCarrierValue.Enabled = false;
                                    textBoxQueryReelCarrierValue.Text = string.Empty;
                                }
                                else
                                {
                                    comboBoxQueryReelTowerIdValue.Enabled = ctrl_.Checked;

                                    if (!comboBoxQueryReelTowerIdValue.Enabled)
                                        comboBoxQueryReelTowerIdValue.SelectedItem = null;
                                }
                            }
                            else
                            {
                                comboBoxQueryReelTowerIdValue.Enabled = ctrl_.Checked;

                                if (!comboBoxQueryReelTowerIdValue.Enabled)
                                    comboBoxQueryReelTowerIdValue.SelectedItem = null;
                            }
                        }
                        break;
                    case "checkBoxQueryByReelSize":
                        {
                            comboBoxQueryReelSizeValue.Enabled = ctrl_.Checked;

                            if (!comboBoxQueryReelSizeValue.Enabled)
                                comboBoxQueryReelSizeValue.SelectedItem = null;
                        }
                        break;
                    case "checkBoxQueryByLotID":
                        {
                            textBoxQueryLotIDValue.Enabled = ctrl_.Checked;

                            if (!ctrl_.Checked)
                                textBoxQueryLotIDValue.Text = string.Empty;
                        }
                        break;
                    case "checkBoxQueryByDatetime":
                        {
                            dateTimePickerQueryReeCreateDateValue.Enabled = ctrl_.Checked;
                            labelQueryReelCreateDateCondition.Enabled = ctrl_.Checked;
                            comboBoxQueryDatetimeOptionValue.Enabled = ctrl_.Checked;

                            if (!comboBoxQueryDatetimeOptionValue.Enabled)
                                comboBoxQueryDatetimeOptionValue.SelectedItem = null;
                        }
                        break;
                    case "checkBoxQueryByArticle":
                        {
                            if (checkBoxQueryByCustomerDB.Checked == true)
                            {
                                if (ctrl_.Checked)
                                {
                                    textBoxQueryReelArticleValue.Enabled = ctrl_.Checked;

                                    if (!ctrl_.Checked)
                                        textBoxQueryReelArticleValue.Text = string.Empty;

                                    checkBoxQueryByTowerId.Checked = false;
                                    comboBoxQueryReelTowerIdValue.Enabled = false;
                                    comboBoxQueryReelTowerIdValue.SelectedItem = null;
                                    checkBoxQueryByCarrier.Checked = false;
                                    textBoxQueryReelCarrierValue.Enabled = false;
                                    textBoxQueryReelCarrierValue.Text = string.Empty;
                                }
                                else
                                {
                                    textBoxQueryReelArticleValue.Enabled = ctrl_.Checked;

                                    if (!ctrl_.Checked)
                                        textBoxQueryReelArticleValue.Text = string.Empty;
                                }
                            }
                            else
                            {
                                textBoxQueryReelArticleValue.Enabled = ctrl_.Checked;

                                if (!ctrl_.Checked)
                                    textBoxQueryReelArticleValue.Text = string.Empty;
                            }
                        }
                        break;
                    case "checkBoxQueryByCarrier":
                        {
                            if (checkBoxQueryByCustomerDB.Checked == true)
                            {
                                if (ctrl_.Checked)
                                {
                                    textBoxQueryReelCarrierValue.Enabled = ctrl_.Checked;

                                    if (!ctrl_.Checked)
                                        textBoxQueryReelCarrierValue.Text = string.Empty;

                                    checkBoxQueryByTowerId.Checked = false;
                                    comboBoxQueryReelTowerIdValue.Enabled = false;
                                    textBoxQueryReelArticleValue.Text = string.Empty;
                                    comboBoxQueryReelTowerIdValue.SelectedItem = null;
                                    checkBoxQueryByArticle.Checked = false;
                                    textBoxQueryReelArticleValue.Enabled = false;
                                }
                                else
                                {
                                    textBoxQueryReelCarrierValue.Enabled = ctrl_.Checked;

                                    if (!ctrl_.Checked)
                                        textBoxQueryReelCarrierValue.Text = string.Empty;
                                }
                            }
                            else
                            {
                                textBoxQueryReelCarrierValue.Enabled = ctrl_.Checked;

                                if (!ctrl_.Checked)
                                    textBoxQueryReelCarrierValue.Text = string.Empty;
                            }
                        }
                        break;
                    case "checkBox6QueryBySupplier":
                        {
                            textBoxQueryReelSupplierValue.Enabled = ctrl_.Checked;

                            if (!ctrl_.Checked)
                                textBoxQueryReelSupplierValue.Text = string.Empty;
                        }
                        break;
                    case "checkBoxQueryByQty":
                        {
                            numericUpDownQueryReelQtyValue.Enabled = ctrl_.Checked;
                            labelQueryReelQtyCondition.Enabled = ctrl_.Checked;
                            comboBoxQueryReelQtyOptionValue.Enabled = ctrl_.Checked;

                            if (!comboBoxQueryReelQtyOptionValue.Enabled)
                                comboBoxQueryReelQtyOptionValue.SelectedItem = null;
                        }
                        break;
                    case "checkBoxQueryByCustomerDB":
                        {
                            if (ctrl_.Checked)
                            {
                                QueryReelCheckBoxReset(ctrl_.Checked);
                                buttonMissmathSearch.Visible = ctrl_.Checked;
                                radioButtonSearchAll.Visible = ctrl_.Checked;
                                radioButtonSerachMissmatch.Visible = ctrl_.Checked;
                            }
                            else
                            {
                                QueryReelCheckBoxReset(ctrl_.Checked);
                                buttonMissmathSearch.Visible = ctrl_.Checked;
                                buttonExecuteDBSync.Visible = ctrl_.Checked;
                                buttonExecuteTowerSync.Visible = ctrl_.Checked;
                                radioButtonSearchAll.Visible = ctrl_.Checked;
                                radioButtonSerachMissmatch.Visible = ctrl_.Checked;
                            }

                        }
                        break;
                }
            }
        }

        private void QueryReelCheckBoxReset(bool check)
        {
            if (check)
            {
                checkBoxQueryByTowerId.Enabled = check;
                checkBoxQueryByReelSize.Enabled = !check;
                checkBoxQueryByLotID.Enabled = !check;
                checkBoxQueryByDatetime.Enabled = !check;
                checkBoxQueryByArticle.Enabled = check;
                checkBoxQueryByCarrier.Enabled = check;
                checkBox6QueryBySupplier.Enabled = !check;
                checkBoxQueryByQty.Enabled = !check;
            }
            else
            {
                checkBoxQueryByTowerId.Enabled = !check;
                checkBoxQueryByReelSize.Enabled = !check;
                checkBoxQueryByLotID.Enabled = !check;
                checkBoxQueryByDatetime.Enabled = !check;
                checkBoxQueryByArticle.Enabled = !check;
                checkBoxQueryByCarrier.Enabled = !check;
                checkBox6QueryBySupplier.Enabled = !check;
                checkBoxQueryByQty.Enabled = !check;

            }
        }
        #endregion

        #region Provide reel methods
        protected void OnKeyUpUnloadReelPages(object sender, KeyEventArgs e)
        {
            Control ctrl_ = sender as Control;

            switch (ctrl_.Name)
            {
                case "textBoxLoginOperatorValue":
                    {
                        if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(textBoxLoginOperatorValue.Text))
                        {
                            if (textBoxLoginOperatorValue.Text.Contains(";"))
                            {
                                if (!ParseInputContext(textBoxLoginOperatorValue.Text))
                                {
                                    if (FormMessageExt.ShowQuestion(string.Format(Properties.Resources.String_Question_NotRecognizedQrCodeRetry, Environment.NewLine, (sender as TextBox).Text)) == DialogResult.Yes)
                                    {
                                        textBoxLoadReelArticleValue.Text = string.Empty;
                                        textBoxLoadReelCarrierValue.Text = string.Empty;
                                        textBoxLoadReelLotIdValue.Text = string.Empty;
                                        textBoxLoadReelSupplierValue.Text = string.Empty;
                                        numericUpDownLoadReelQtyValue.Value = 0;
                                        textBoxLoadReelMfgValue.Text = string.Empty;
                                    }
                                }

                                textBoxLoginOperatorValue.Text = string.Empty;
                            }
                            else
                            {
                                radioButtonSearchReel.Checked = mainSequence.LoginUser(textBoxLoginOperatorValue.Text);
                                textBoxSearchReelIdValue.Text = string.Empty;
                                numericUpDownAssignReelsValue.Value = 0;
                                labelAssignJobUserValue.Text = mainSequence.CurrentOperator;
                                labelAssignJobNameValue.Text = mainSequence.CreateProvideJobName(CONST_PROVIDE_JOB_PREFIX);
                            }
                        }
                    }
                    break;
                case "textBoxSearchReelIdValue":
                    {
                        if (!string.IsNullOrEmpty(textBoxSearchReelIdValue.Text) && e.KeyCode == Keys.Enter)
                            buttonSearchReel.PerformClick();
                    }
                    break;
            }
        }

        protected void OnSelectedIndexChangedComboBoxSearchReelIdType(object sender, EventArgs e)
        {
            textBoxSearchReelIdValue.Text = string.Empty;
            numericUpDownAssignReelsValue.Value = 0;
            dataGridViewSearchedReels.DataSource = null;
        }

        protected void OnClickButtonSearchReel(object sender, EventArgs e)
        {
            try
            {
                bool searcharticle_ = false;
                DataSet result_ = null;
                List<Pair<MaterialIdentifiers, string>> items_ = new List<Pair<MaterialIdentifiers, string>>();

                dataGridViewSearchedReels.DataSource = null;

                if (comboBoxSearchReelIdTypeValue.SelectedItem == null)
                    return;

                if (!string.IsNullOrEmpty(textBoxSearchReelIdValue.Text))
                {
                    switch (comboBoxSearchReelIdTypeValue.SelectedIndex)
                    {
                        case 0:
                            {
                                searcharticle_ = true;
                                items_.Add(new Pair<MaterialIdentifiers, string>(MaterialIdentifiers.Article, textBoxSearchReelIdValue.Text));
                            }
                            break;
                        case 1:
                            items_.Add(new Pair<MaterialIdentifiers, string>(MaterialIdentifiers.Carrier, textBoxSearchReelIdValue.Text));
                            break;
                    }

                    if (searcharticle_)
                    {
                        result_ = mainSequence.SearchReelsByArticle(items_, Convert.ToInt32(numericUpDownAssignReelsValue.Value));

                        List<ThreeField<string, string, int>> alreadyassigned_ = null;
                        List<Pair<string, string>> provideitems_ = new List<Pair<string, string>>();

                        foreach (DataRow row_ in result_.Tables[0].Rows)
                            provideitems_.Add(new Pair<string, string>(
                                row_["Article"].ToString(),
                                row_["Manufactur"].ToString()));

                        alreadyassigned_ = mainSequence.ReelTowerGroupObject.GetProvideJobReel(provideitems_);

                        if (alreadyassigned_ != null)
                        {
                            foreach (DataRow row_ in result_.Tables[0].Rows)
                            {
                                if (alreadyassigned_.Where(x_ => x_.first == row_["Article"].ToString() && x_.second == row_["Manufactur"].ToString()).Any())
                                {
                                    int count_ = Convert.ToInt32(row_["Reels"]);
                                    int provided_ = alreadyassigned_.First(x_ => x_.first == row_["Article"].ToString() && x_.second == row_["Manufactur"].ToString()).third;
                                    row_["Reels"] = count_ - provided_;
                                }
                            }
                        }
                    }
                    else
                    {
                        result_ = mainSequence.SearchReelsByCarrier(items_, Convert.ToInt32(numericUpDownAssignReelsValue.Value));

                        List<int> removerows_ = new List<int>();
                        List<FourField<string, string, string, int>> alreadyassigned_ = null;
                        List<ThreeField<string, string, string>> provideitems_ = new List<ThreeField<string, string, string>>();

                        foreach (DataRow row_ in result_.Tables[0].Rows)
                            provideitems_.Add(new ThreeField<string, string, string>(
                                row_["Article"].ToString(),
                                row_["Carrier"].ToString(),
                                row_["Manufactur"].ToString()));

                        alreadyassigned_ = mainSequence.ReelTowerGroupObject.GetProvideJobReel(provideitems_);

                        if (alreadyassigned_ != null)
                        {
                            for (int i_ = 0; i_ < result_.Tables[0].Rows.Count; i_++)
                            {
                                DataRow row_ = result_.Tables[0].Rows[i_];
                                if (alreadyassigned_.Where(x_ => x_.first == row_["Article"].ToString() && x_.second == row_["Carrier"].ToString() && x_.third == row_["Manufactur"].ToString()).Any())
                                    removerows_.Add(i_);
                            }

                            foreach (int i_ in removerows_)
                                result_.Tables[0].Rows.RemoveAt(i_);
                        }
                    }

                    if (result_ != null && result_.Tables.Count > 0 && result_.Tables[0].Rows.Count > 0)
                    {
                        dataGridViewSearchedReels.DataSource = result_.Tables[0];
                    }
                    else
                        FormMessageExt.ShowNotification($"The reel ({textBoxSearchReelIdValue.Text}) is not available.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected void OnCellContentDoubleClickSearchReel(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewSearchedReels.SelectedRows.Clear();

            foreach (DataGridViewRow row in dataGridViewSearchedReels.Rows)
            {
                if (row.Index == e.RowIndex)
                    row.Selected = true;
            }

            AssignReel();
        }

        protected void OnClickButtonAssignReel(object sender, EventArgs e)
        {
            if (dataGridViewSearchedReels.Rows.Count <= 0)
                return;

            if (numericUpDownAssignReelsValue.Value <= 0)
            {
                FormMessageExt.ShowNotification(Properties.Resources.String_FormMain_Message_ChoseProperAssignMaterialCount);
                return;
            }

            AssignReel();
        }

        protected void AssignReel()
        {
            try
            {
                int reelCountIndex_ = -1;
                int selected_ = 0;
                DataSet result_ = null;
                List<Pair<MaterialIdentifiers, string>> items_ = new List<Pair<MaterialIdentifiers, string>>();

                if (dataGridViewSearchedReels.SelectedRows.Count > 0)
                {
                    string article_ = string.Empty;
                    string carrier_ = string.Empty;
                    string supplier_ = string.Empty;
                    int reels_ = 0;

                    foreach (DataGridViewColumn col_ in dataGridViewSearchedReels.Columns)
                    {
                        switch (col_.Name)
                        {
                            case "Article": article_ = dataGridViewSearchedReels.SelectedRows[0].Cells[col_.Index].Value.ToString(); break;
                            case "Carrier": carrier_ = dataGridViewSearchedReels.SelectedRows[0].Cells[col_.Index].Value.ToString(); break;
                            case "Manufactur": supplier_ = dataGridViewSearchedReels.SelectedRows[0].Cells[col_.Index].Value.ToString(); break;
                            case "Reels":
                                {
                                    reels_ = Convert.ToInt32(dataGridViewSearchedReels.SelectedRows[0].Cells[col_.Index].Value);
                                    reelCountIndex_ = col_.Index;
                                }
                                break;
                        }
                    }

                    if (reels_ >= numericUpDownAssignReelsValue.Value || reelCountIndex_ <= 0)
                    {
                        if (string.IsNullOrEmpty(carrier_))
                            result_ = mainSequence.SearchReelsByArticle(article_, supplier_, Convert.ToInt32(numericUpDownAssignReelsValue.Value), Config.ProvideMode == ProvideModes.ByCreateTime);
                        else
                            result_ = mainSequence.SearchReelsByCarrier(article_, carrier_, supplier_, Convert.ToInt32(numericUpDownAssignReelsValue.Value), Config.ProvideMode == ProvideModes.ByCreateTime);

                        if (result_ != null && result_.Tables.Count > 0 && result_.Tables[0].Rows.Count > 0)
                        {
                            if (dataGridViewAssignedReels.Columns.Count <= 0)
                            {
                                dataGridViewAssignedReels.Columns.Add("Article", "Article");
                                dataGridViewAssignedReels.Columns.Add("Carrier", "Carrier");
                                dataGridViewAssignedReels.Columns.Add("Manufactur", "Manufactur");
                                dataGridViewAssignedReels.Columns.Add("Stock", "Stock");
                                dataGridViewAssignedReels.Columns.Add("Depot", "Depot");
                            }

                            List<DataRow> removerows_ = new List<DataRow>();
                            List<Pair<string, string>> alreadyassigned_ = null;
                            List<FourField<string, string, string, int>> provideitems_ = new List<FourField<string, string, string, int>>();

                            foreach (DataRow row_ in result_.Tables[0].Rows)
                                provideitems_.Add(new FourField<string, string, string, int>(
                                    row_["Article"].ToString(),
                                    row_["Carrier"].ToString(),
                                    row_["Manufactur"].ToString(),
                                    int.Parse(row_["Stock"].ToString())));

                            alreadyassigned_ = mainSequence.ReelTowerGroupObject.HasProvideJobReel(provideitems_);

                            if (alreadyassigned_ != null)
                            {
                                for (int i_ = 0; i_ < result_.Tables[0].Rows.Count; i_++)
                                {
                                    DataRow row_ = result_.Tables[0].Rows[i_];
                                    if (alreadyassigned_.Where(x_ => x_.first == row_["Article"].ToString() && x_.second == row_["Carrier"].ToString()).Any())
                                    {
                                        removerows_.Add(row_);
                                    }
                                }

                                foreach (DataRow row_ in removerows_)
                                    result_.Tables[0].Rows.Remove(row_);
                            }

                            foreach (DataRow row_ in result_.Tables[0].Rows)
                            {
                                IEnumerable<DataGridViewRow> elements_ = dataGridViewAssignedReels.Rows
                                    .Cast<DataGridViewRow>()
                                    .Where(x_ => x_.Cells["Carrier"].Value.ToString().Equals(row_["Carrier"].ToString()));

                                if (elements_.Count() == 0)
                                {
                                    dataGridViewAssignedReels.Rows.Add(row_.ItemArray);
                                    ++selected_;
                                }

                                if (reelCountIndex_ >= 0)
                                {
                                    if (reels_ > 0)
                                        dataGridViewSearchedReels.SelectedRows[0].Cells[reelCountIndex_].Value = --reels_;

                                    if (reels_ <= 0)
                                        dataGridViewSearchedReels.Rows.Remove(dataGridViewSearchedReels.SelectedRows[0]);
                                }

                                if (selected_ >= numericUpDownAssignReelsValue.Value)
                                    break;
                            }

                            labelAssignJobTotalReelsValue.Text = dataGridViewAssignedReels.Rows.Count.ToString();
                        }
                        else
                            FormMessageExt.ShowNotification(string.Format(Properties.Resources.String_FormMain_Message_ReelIsNotAvailable, textBoxSearchReelIdValue.Text));
                    }
                    else
                        FormMessageExt.ShowNotification(Properties.Resources.String_FormMain_Message_ChoseProperAssignMaterialCount);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected void OnClickButtonRemoveReels(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row_ in dataGridViewAssignedReels.SelectedRows)
                dataGridViewAssignedReels.Rows.Remove(row_);

            labelAssignJobTotalReelsValue.Text = dataGridViewAssignedReels.Rows.Count.ToString();
        }

        protected void OnClickButtonEnqueueJob(object sender, EventArgs e)
        {
            bool result_ = false;
            int index_ = 0;
            string depot_ = string.Empty;
            string jobname_ = string.Empty;
            string tempjob_ = labelAssignJobNameValue.Text;
            string user_ = labelAssignJobUserValue.Text;
            string reels_ = labelAssignJobTotalReelsValue.Text;
            List<string> uids_ = new List<string>();
            List<Pair<string, string>> alreadyassigned_ = null;
            List<FiveField<string, string, string, int, string>> provideitems_ = new List<FiveField<string, string, string, int, string>>();

            if (string.IsNullOrEmpty(reels_) || comboBoxAssignJobOutportValue.SelectedItem == null)
                return;

            foreach (DataGridViewRow row_ in dataGridViewAssignedReels.Rows)
                provideitems_.Add(new FiveField<string, string, string, int, string>(
                    row_.Cells["Article"].Value.ToString(),
                    row_.Cells["Carrier"].Value.ToString(),
                    row_.Cells["Manufactur"].Value.ToString(),
                    int.Parse(row_.Cells["Stock"].Value.ToString()),
                    row_.Cells["Depot"].Value.ToString()));

            // mainSequence.pickingsid.Clear();
            // mainSequence.pickinguid.Clear();

            // if (mainSequence.pickingsids.ContainsKey(tempjob_))
            //     mainSequence.pickingsids.Remove(tempjob_);
            // 
            // if (mainSequence.pickinguids.ContainsKey(tempjob_))
            //     mainSequence.pickinguids.Remove(tempjob_);

            // foreach (var va in provideitems_)
            // {
            //     mainSequence.pickingsid.Add(va.first);
            //     mainSequence.pickinguid.Add(va.second);
            // }
            // 
            // mainSequence.pickingsids.Add(tempjob_, mainSequence.pickingsid);
            // mainSequence.pickinguids.Add(tempjob_, mainSequence.pickinguid);

            foreach (var va in provideitems_)
                uids_.Add(va.second);

            (App.MainSequence as ReelTowerGroupSequence).ProvideJobManager.AddJob(tempjob_, user_, uids_);

            alreadyassigned_ = mainSequence.ReelTowerGroupObject.HasProvideJobReel(provideitems_);

            if (alreadyassigned_ != null)
            {
                if (FormMessageExt.ShowQuestion(Properties.Resources.String_FormMain_Question_DuplicatedMaterialInformation) == DialogResult.No)
                    return;
                else
                {
                    foreach (Pair<string, string> item_ in alreadyassigned_)
                    {
                        FiveField<string, string, string, int, string> element_ = provideitems_.Find(x_ => x_.first == item_.first && x_.second == item_.second);

                        if (element_ != null)
                            provideitems_.Remove(element_);
                    }

                    if (provideitems_.Count > 0)
                        result_ = true;
                }
            }
            else
                result_ = true;

            if (result_ && comboBoxAssignJobOutportValue.SelectedItem != null)
            {
                var stages_ = Config.OutStageIds.Where(x_ => x_.Value == comboBoxAssignJobOutportValue.SelectedItem.ToString()).ToList();

                if (stages_ != null && stages_.Count > 0 && !string.IsNullOrEmpty(stages_.First().Value))
                {
                    List<List<ProvideMaterialData>> containers_ = new List<List<ProvideMaterialData>>();

                    if (provideitems_.Count > Config.JobSplitReelCount)
                    {
                        containers_.Add(new List<ProvideMaterialData>());

                        foreach (FiveField<string, string, string, int, string> item_ in provideitems_)
                        {
                            if (containers_[index_].Count == Config.JobSplitReelCount)
                            {
                                index_++;
                                containers_.Add(new List<ProvideMaterialData>());
                            }

                            ProvideMaterialData carrier_ = new ProvideMaterialData(new string[] { item_.first, item_.second, item_.third, item_.fourth.ToString(), item_.fifth });
                            carrier_.State = ProvideMaterialData.States.Ready;
                            containers_[index_].Add(carrier_);
                        }
                    }
                    else
                    {
                        List<ProvideMaterialData> carriers_ = new List<ProvideMaterialData>();

                        foreach (FiveField<string, string, string, int, string> item_ in provideitems_)
                        {
                            ProvideMaterialData carrier_ = new ProvideMaterialData(new string[] { item_.first, item_.second, item_.third, item_.fourth.ToString(), item_.fifth });
                            carrier_.State = ProvideMaterialData.States.Ready;
                            carriers_.Add(carrier_);
                        }

                        containers_.Add(carriers_);
                    }

                    if (dataGridViewQueuedJobs.Columns.Count <= 0)
                    {
                        dataGridViewQueuedJobs.Columns.Add("Jobname", "Jobname");
                        dataGridViewQueuedJobs.Columns.Add("User", "User");
                        dataGridViewQueuedJobs.Columns.Add("Outport", "Outport");
                        dataGridViewQueuedJobs.Columns.Add("Reels", "Reels");
                        dataGridViewQueuedJobs.Columns.Add("State", "State");
                    }

                    if (containers_.Count > 0)
                    {
                        List<DataGridViewRow> rows_ = dataGridViewQueuedJobs.Rows
                            .Cast<DataGridViewRow>()
                            .Where(r => r.Cells["Jobname"].Value.ToString().Equals(labelAssignJobNameValue.Text)).ToList();

                        if (rows_ == null || rows_.Count <= 0)
                        {
                            index_ = 0;
                            foreach (List<ProvideMaterialData> job_ in containers_)
                            {
                                jobname_ = tempjob_;

                                if (index_ > 0)
                                    jobname_ += $"_{index_}";

                                if (dataGridViewQueuedJobs.Rows.Add(jobname_, user_, stages_.First().Key, containers_[index_].Count, ProvideJobListData.States.Created.ToString()) >= 0)
                                {
                                    if (mainSequence.AddProvideJob(jobname_, user_, stages_.First().Key, containers_[index_]))
                                    {
                                        result_ = true;

                                        // Clear search results
                                        if (dataGridViewSearchedReels.DataSource != null)
                                            dataGridViewSearchedReels.DataSource = null;
                                        else
                                            dataGridViewSearchedReels.Rows.Clear();

                                        if (dataGridViewAssignedReels.DataSource != null)
                                            dataGridViewAssignedReels.DataSource = null;
                                        else
                                            dataGridViewAssignedReels.Rows.Clear();

                                        radioButtonJobMonitor.PerformClick();
                                    }
                                }

                                index_++;
                            }

                            labelAssignJobNameValue.Text = string.Empty;
                            labelAssignJobUserValue.Text = string.Empty;
                            labelAssignJobTotalReelsValue.Text = string.Empty;
                        }
                        else
                        {
                            FormMessageExt.ShowNotification(Properties.Resources.String_FormMain_Message_CheckReelTowerToUnload);
                        }
                    }
                }
                else
                {
                    FormMessageExt.ShowNotification(Properties.Resources.String_FormMain_Message_SelectProperOutput);
                }
            }
        }

        public bool dataGridViewqueuedJobs_Check(string jobname_)
        {
            bool result = false;

            if (dataGridViewQueuedJobs.Rows.Count > 0)                
            {
                for (int i = 0; i < dataGridViewQueuedJobs.Rows.Count; i++)
                {
                    if (dataGridViewQueuedJobs.Rows[i].Cells["Jobname"].Value.ToString() == jobname_)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        public void QueuedJobCreate(string jobname_, string user_, int stages_, int containers_)
        {
            if (dataGridViewQueuedJobs.Columns.Count <= 0)
            {
                dataGridViewQueuedJobs.Columns.Add("Jobname", "Jobname");
                dataGridViewQueuedJobs.Columns.Add("User", "User");
                dataGridViewQueuedJobs.Columns.Add("Outport", "Outport");
                dataGridViewQueuedJobs.Columns.Add("Reels", "Reels");
                dataGridViewQueuedJobs.Columns.Add("State", "State");
            }

            dataGridViewQueuedJobs.Rows.Add(jobname_, user_, stages_, containers_, ProvideJobListData.States.Created.ToString());
        }

        protected void OnClickButtonStartJob(object sender, EventArgs e)
        {
            if (dataGridViewQueuedJobs.SelectedRows.Count > 0 && mainSequence.GetProvideJobState(dataGridViewQueuedJobs.SelectedRows[0].Cells["Jobname"].Value.ToString()) == ProvideJobListData.States.Created)
                mainSequence.ProvideJob(dataGridViewQueuedJobs.SelectedRows[0].Cells["Jobname"].Value.ToString());
        }

        protected void OnClickButtonRefreshJobs(object sender, EventArgs e)
        {
            // DataSet result_ = null;
            // result_ = mainSequence.RefreshJob();
        }

        protected void OnClickButtonCancelJob(object sender, EventArgs e)
        {
            if (dataGridViewQueuedJobs.SelectedRows.Count > 0 && mainSequence.GetProvideJobState(dataGridViewQueuedJobs.SelectedRows[0].Cells["Jobname"].Value.ToString()) <= ProvideJobListData.States.Providing)
            {
                if (mainSequence.CancelJob(dataGridViewQueuedJobs.SelectedRows[0].Cells["Jobname"].Value.ToString()))
                    RemoveProvideJobListOnList(dataGridViewQueuedJobs.SelectedRows[0].Cells["Jobname"].Value.ToString());
            }
        }

        protected void OnClickButtonDeleteJob(object sender, EventArgs e)
        {
            bool result_ = false;

            if (dataGridViewQueuedJobs.SelectedRows.Count > 0)
            {
                switch (mainSequence.GetProvideJobState(dataGridViewQueuedJobs.SelectedRows[0].Cells["Jobname"].Value.ToString()))
                {
                    case ProvideJobListData.States.Providing:
                        {
                            if (mainSequence.IsFinishedJob(dataGridViewQueuedJobs.SelectedRows[0].Cells["Jobname"].Value.ToString()))
                                result_ = true;
                            else
                                FormMessageExt.ShowWarning(Properties.Resources.String_FormMain_Cancel, Buttons.Ok, true, 5000);
                        }
                        break;
                    default:
                        {
                            result_ = true;
                        }
                        break;
                }

                if (result_)
                {
                    // UPDATED: 20200728 (jm.choi)
                    if (!dataGridViewQueuedJobs.SelectedRows[0].Cells["Jobname"].Value.ToString().ToLower().Contains("job"))
                    {
                        mainSequence.SetPickingList_Cancel(dataGridViewQueuedJobs.SelectedRows[0].Cells["Jobname"].Value.ToString());
                        // mainSequence.pickingsids.Remove(dataGridViewQueuedJobs.SelectedRows[0].Cells["Jobname"].Value.ToString());
                        // mainSequence.pickinguids.Remove(dataGridViewQueuedJobs.SelectedRows[0].Cells["Jobname"].Value.ToString());
                        (App.MainSequence as ReelTowerGroupSequence).ProvideJobManager.RemoveJob(dataGridViewQueuedJobs.SelectedRows[0].Cells["Jobname"].Value.ToString());
                    }

                    if (dataGridViewQueuedJobs.SelectedRows[0].Cells["State"].Value.ToString().ToLower() != "completed")
                        mainSequence.CancelJob(dataGridViewQueuedJobs.SelectedRows[0].Cells["Jobname"].Value.ToString());

                    if (mainSequence.RemoveJob(dataGridViewQueuedJobs.SelectedRows[0].Cells["Jobname"].Value.ToString()))
                        RemoveProvideJobListOnList(dataGridViewQueuedJobs.SelectedRows[0].Cells["Jobname"].Value.ToString());
                }
            }
        }

        protected void OnClickButtonCleanUp(object sender, EventArgs e)
        {
            if (App.OperationState == OperationStates.Stop)
                mainSequence.CleanUpDatabase();
        }

        protected void OnComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            if (sender != null && sender is ComboBox)
            {
                ComboBox ctrl_ = sender as ComboBox;

                try
                {
                    e.DrawBackground();

                    if (e.Index >= 0)
                    {
                        using (StringFormat sf_ = new StringFormat())
                        {
                            sf_.LineAlignment = StringAlignment.Center;
                            sf_.Alignment = StringAlignment.Center;
                            Brush brush_ = new SolidBrush(ctrl_.ForeColor);

                            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                                brush_ = SystemBrushes.HighlightText;

                            e.Graphics.DrawString(ctrl_.Items[e.Index].ToString(), ctrl_.Font, brush_, e.Bounds, sf_);
                            brush_.Dispose();
                            brush_ = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                }
            }
        }
        #endregion

        #region Account manager methods
        protected void ClearQueryInformation(bool all = true)
        {
            comboBoxAccountGroupIdValue.SelectedItem    = null;
            textBoxUserPasswordValue.Text               = string.Empty;
            textBoxUserFullnameValue.Text               = string.Empty;
            textBoxUserRemarkValue.Text                 = string.Empty;

            if (all)
                textBoxUserIdValue.Text = string.Empty;
        }

        protected void OnClickButtonAccountManagerLogin(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxAccountManagerIdValue.Text))
                return;

            lock (buttonAccountManagerLogin)
            {
                if (buttonAccountManagerLogin.Text == Properties.Resources.String_Login)
                {
                    if (mainSequence.LoginAccountManager(textBoxAccountManagerIdValue.Text, textBoxAccountManagerPasswordValue.Text))
                    {
                        buttonAccountManagerLogin.Text              = Properties.Resources.String_Logout;
                        textBoxAccountManagerIdValue.Enabled        = false;
                        textBoxAccountManagerPasswordValue.Enabled  = false;
                        groupBoxNewAccount.Visible                  = true;
                    }
                    else
                        FormMessageExt.ShowNotification(string.Format(Properties.Resources.String_FormMain_Message_ManagerAccountIsNotAvailable, textBoxAccountManagerIdValue.Text), Buttons.Ok, true, CONST_SHORT_MESSAGEBOX_CLOSE_DELAY);
                }
                else if (buttonAccountManagerLogin.Text == Properties.Resources.String_Logout)
                {
                    mainSequence.LogoutAccountManager();
                    buttonAccountManagerLogin.Text              = Properties.Resources.String_Login;
                    textBoxAccountManagerIdValue.Enabled        = true;
                    textBoxAccountManagerIdValue.Text           = string.Empty;
                    textBoxAccountManagerPasswordValue.Enabled  = true;
                    textBoxAccountManagerPasswordValue.Text     = string.Empty;
                    groupBoxNewAccount.Visible                  = false;
                }
            }
        }

        protected void OnClickButtonQueryUser(object sender, EventArgs e)
        {
            ClearQueryInformation(false);

            if (string.IsNullOrEmpty(textBoxUserIdValue.Text) || textBoxUserIdValue.Text.ToLower() == "administrator")
                return;

            DataSet result_ = null;
            buttonAddNewUser.Enabled = false;
            buttonRemoveUser.Enabled = false;

            lock (buttonQueryUser)
            {
                if ((result_ = mainSequence.QueryAccount(textBoxUserIdValue.Text)) != null && result_.Tables.Count > 0 && result_.Tables[0].Rows.Count > 0)
                {
                    comboBoxAccountGroupIdValue.Text    = ((UserGroup)(Convert.ToInt32(result_.Tables[0].Rows[0].ItemArray[0].ToString()))).ToString();
                    textBoxUserIdValue.Text             = result_.Tables[0].Rows[0].ItemArray[1].ToString();
                    textBoxUserPasswordValue.Text       = result_.Tables[0].Rows[0].ItemArray[2].ToString();
                    textBoxUserFullnameValue.Text       = result_.Tables[0].Rows[0].ItemArray[3].ToString();
                    textBoxUserRemarkValue.Text         = result_.Tables[0].Rows[0].ItemArray[4].ToString();
                }
                else
                    FormMessageExt.ShowNotification(string.Format(Properties.Resources.String_FormMain_Message_UserAccountIsNotAvailable, textBoxUserIdValue.Text), Buttons.Ok, true, CONST_SHORT_MESSAGEBOX_CLOSE_DELAY);
            }

            buttonAddNewUser.Enabled = true;
            buttonRemoveUser.Enabled = true;
        }

        protected void OnClickButtonRemoveUser(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxUserIdValue.Text))
            {
                ClearQueryInformation();
                return;
            }

            bool result_ = false;
            buttonAddNewUser.Enabled = false;
            buttonQueryUser.Enabled = false;

            lock (buttonRemoveUser)
            {
                if (result_ = mainSequence.RemoveAccount(textBoxUserIdValue.Text))
                {
                    FormMessageExt.ShowNotification(string.Format(Properties.Resources.String_FormMain_Message_RemoveTheAccount, textBoxUserIdValue.Text), Buttons.Ok, true, CONST_SHORT_MESSAGEBOX_CLOSE_DELAY);
                    ClearQueryInformation();
                }
            }

            buttonAddNewUser.Enabled = true;
            buttonQueryUser.Enabled = true;
        }

        protected void OnClickButtonAddNewUser(object sender, EventArgs e)
        {
            if (comboBoxAccountGroupIdValue.SelectedItem == null || string.IsNullOrEmpty(textBoxUserIdValue.Text) || string.IsNullOrEmpty(textBoxUserPasswordValue.Text))
                return;

            bool result_ = false;
            buttonQueryUser.Enabled = false;
            buttonRemoveUser.Enabled = false;

            lock (buttonAddNewUser)
            {
                if (result_ = mainSequence.AddNewAccount(
                    (UserGroup)Enum.Parse(typeof(UserGroup), comboBoxAccountGroupIdValue.SelectedItem.ToString()),
                    textBoxUserIdValue.Text,
                    textBoxUserPasswordValue.Text,
                    textBoxUserFullnameValue.Text,
                    textBoxUserRemarkValue.Text))
                {
                    FormMessageExt.ShowNotification(string.Format(Properties.Resources.String_FormMain_Message_AddedANewAccount, textBoxUserIdValue.Text), Buttons.Ok, true, CONST_SHORT_MESSAGEBOX_CLOSE_DELAY);
                    ClearQueryInformation();
                }
            }

            buttonQueryUser.Enabled = true;
            buttonRemoveUser.Enabled = true;
        }

        protected void OnVisibleChangedUserInformation(object sender, EventArgs e)
        {
            ClearQueryInformation();
        }
        #endregion

        #region Robot operation mode
        protected void OnCheckedChangedAutoMode(object sender, EventArgs e)
        {
            if (sender is CheckBox)
            {
                mainSequence.SetTakeOutMode(!(sender as CheckBox).Checked);
                checkBoxTakeoutByRobot.Text = ((sender as CheckBox).Checked ? Properties.Resources.String_AutoMode : Properties.Resources.String_ManualMode);
            }
        }
        #endregion

        #region Keyboard event handlers
        protected bool ParseInputBytes()
        {
            bool result_ = false;

            if (inputBytes.Where(x_ => Convert.ToByte(x_) == ';').Any())
            {
                inputData = Encoding.ASCII.GetString(inputBytes.ToArray());
                MaterialData data = new MaterialData();
                Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: {inputData}");

                if (inputData.Length > 2 && inputData[0] == '1' && inputData[1] == '0' && CompositeVisionManager.GetBarcode(inputData, ref data))
                {
                    barcodeData.Clear();
                    barcodeData.CopyMaterialData(data);

                    textBoxLoadReelArticleValue.Text = barcodeData.Data.Category.Length > Config.ArticleNameLength ? barcodeData.Data.Category.Substring(0, 9) : barcodeData.Data.Category;
                    textBoxLoadReelCarrierValue.Text = barcodeData.Data.Name;
                    textBoxLoadReelLotIdValue.Text = barcodeData.Data.LotId;
                    textBoxLoadReelSupplierValue.Text = barcodeData.Data.Supplier;
                    numericUpDownLoadReelQtyValue.Value = barcodeData.Data.Quantity;
                    textBoxLoadReelMfgValue.Text = barcodeData.Data.ManufacturedDatetime;

                    if (tabControlMain.SelectedTab != tabPageStoreOperation)
                    {
                        tabControlMain.SelectedTab = tabPageStoreOperation;
                        textBoxLoadReelArticleValue.Select(textBoxLoadReelArticleValue.Text.Length, 0);
                        textBoxLoadReelArticleValue.Focus();
                    }

                    result_ = true;
                }
            }
            else if (inputBytes.Count > 2 && inputBytes[0] == 'R' && inputBytes[0] == 'Q')
            {
                inputData = Encoding.ASCII.GetString(inputBytes.ToArray());
                numericUpDownLoadReelQtyValue.Value = Convert.ToInt32(inputData.Substring(2, inputData.Length - 2));

                if (tabControlMain.SelectedTab != tabPageStoreOperation)
                {
                    tabControlMain.SelectedTab = tabPageStoreOperation;
                    textBoxLoadReelArticleValue.Select(textBoxLoadReelArticleValue.Text.Length, 0);
                    textBoxLoadReelArticleValue.Focus();
                }

                result_ = true;
            }

            return result_;
        }

        protected bool ParseInputContext(string context)
        {
            if (!string.IsNullOrEmpty(context.TrimEnd(Environment.NewLine.ToCharArray())) && context.Length > 2)
            {
                inputBytes.Clear();
                var utf8bytes = Encoding.UTF8.GetBytes(context);
                inputBytes = Encoding.Convert(Encoding.UTF8, Encoding.ASCII, utf8bytes).ToList();
                return ParseInputBytes();
            }

            inputBytes.Clear();
            return false;
        }

        protected void OnPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
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

                        if ((App.OperationState == OperationStates.Stop || !Config.ReelHandlerUsage) &&
                            (sender is Form && (sender as Form).Name == "FormMain" ||
                            sender is TabControl && (sender as TabControl).Name == "tabControlMain" ||
                            sender is TextBox && (sender as TextBox).Name == "textBoxLoadReelArticleValue"))
                        {
                            ParseInputBytes();
                        }

                        inputBytes.Clear();
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

        protected void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (skipInputData)
            {
                byte data_ = 0x00;

                switch (e.KeyCode)
                {
                    case Keys.Oemtilde: // ~,`
                        data_ = Convert.ToByte(e.Shift ? '~' : '`');
                        break;
                    case Keys.D1: // !,1->D1
                        data_ = Convert.ToByte(e.Shift ? '!' : '1');
                        break;
                    case Keys.D2: // @,2->D2...
                        data_ = Convert.ToByte(e.Shift ? '@' : '2');
                        break;
                    case Keys.D0: // ),0->D0
                        data_ = Convert.ToByte(e.Shift ? ')' : '0');
                        break;
                    case Keys.OemMinus: // _,- ->OemMinus
                        data_ = Convert.ToByte(e.Shift ? '_' : '-');
                        break;
                    case Keys.Oemplus: // +,= ->OemPlus
                        data_ = Convert.ToByte(e.Shift ? '+' : '=');
                        break;
                    case Keys.OemPipe: //|,\ -> OemPipe
                        data_ = Convert.ToByte(e.Shift ? '|' : '\\');
                        break;
                    case Keys.Back: // ← (BackSpace)->Back
                        {
                            if (inputBytes.Count > 0)
                                inputBytes.RemoveAt(inputBytes.Count - 1);
                        }
                        break;
                    case Keys.OemOpenBrackets: // {,[ -> OemOpenBrackets
                        data_ = Convert.ToByte(e.Shift ? '{' : '[');
                        break;
                    case Keys.OemCloseBrackets: // },] -> OemCloseBrackets
                        data_ = Convert.ToByte(e.Shift ? '}' : ']');
                        break;
                    case Keys.OemSemicolon: //:,; -> OemSemicolon
                        data_ = Convert.ToByte(e.Shift ? ':' : ';');
                        break;
                    case Keys.OemQuotes: // ",' -> OemQuotes
                        data_ = Convert.ToByte(e.Shift ? '"' : '\'');
                        break;
                    case Keys.Oemcomma: // <,, -> Oemcomma
                        data_ = Convert.ToByte(e.Shift ? '<' : ',');
                        break;
                    case Keys.OemPeriod: // >,. -> OemPeriod
                        data_ = Convert.ToByte(e.Shift ? '>' : '.');
                        break;
                    case Keys.OemQuestion: // ?,/ -> OemQuestion
                        data_ = Convert.ToByte(e.Shift ? '?' : '/');
                        break;
                    case Keys.Space: // SpaceBar -> Space
                        data_ = Convert.ToByte(' ');
                        break;
                    case Keys.Divide: /// -> Divide
                        data_ = Convert.ToByte('/');
                        break;
                    case Keys.Multiply: // * -> Multiply
                        data_ = Convert.ToByte('*');
                        break;
                    case Keys.Subtract: // - -> Subtract
                        data_ = Convert.ToByte('-');
                        break;
                    case Keys.Add: // + -> Add
                        data_ = Convert.ToByte('+');
                        break;
                    case Keys.NumPad0:
                        data_ = Convert.ToByte('0');
                        break;
                    case Keys.NumPad1:
                        data_ = Convert.ToByte('1');
                        break;
                    case Keys.NumPad2:
                        data_ = Convert.ToByte('2');
                        break;
                    case Keys.NumPad3:
                        data_ = Convert.ToByte('3');
                        break;
                    case Keys.NumPad4:
                        data_ = Convert.ToByte('4');
                        break;
                    case Keys.NumPad5:
                        data_ = Convert.ToByte('5');
                        break;
                    case Keys.NumPad6:
                        data_ = Convert.ToByte('6');
                        break;
                    case Keys.NumPad7:
                        data_ = Convert.ToByte('7');
                        break;
                    case Keys.NumPad8:
                        data_ = Convert.ToByte('8');
                        break;
                    case Keys.NumPad9: // 0~9까지 키패드의 숫자들 -> NumPad0 ~NumPad9
                        data_ = Convert.ToByte('9');
                        break;
                    case Keys.Decimal: // . -> Decimal
                        data_ = Convert.ToByte('.');
                        break;
                    case Keys.Enter: //  Enter -> Return
                        break;
                }

                if (sender is TextBox)
                {
                    e.Handled = true;
                    return;
                }

                if (data_ != 0x00)
                    inputBytes.Add(data_);
                else if (e.KeyValue >= 20 && e.KeyValue < 126)
                    inputBytes.Add(Convert.ToByte(e.KeyValue));

                Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: {Encoding.ASCII.GetString(inputBytes.ToArray())}");
            }
        }
        #endregion

        #region Special meterial data methods
        protected void OnClickButtonFindMaterialInformation(object sender, EventArgs e)
        {
            string val_ = string.Empty;

            if (string.IsNullOrEmpty(textBoxMaterialNameValue.Text) || comboBoxMaterialTypeValue.SelectedItem == null || string.IsNullOrEmpty(comboBoxMaterialTypeValue.SelectedItem.ToString()))
                return;

            if (mainSequence.ReelTowerGroupObject.IsSpecialMaterialData(textBoxMaterialNameValue.Text, ref val_))
                FormMessageExt.ShowInformation($"{textBoxMaterialNameValue.Text} is exist in special material data.");
        }

        protected void OnClickButtonAddMaterialInformation_(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxMaterialNameValue.Text) || comboBoxMaterialTypeValue.SelectedItem == null || string.IsNullOrEmpty(comboBoxMaterialTypeValue.SelectedItem.ToString()))
                return;

            mainSequence.ReelTowerGroupObject.AddSpecialMaterialData(textBoxMaterialNameValue.Text, comboBoxMaterialTypeValue.Text, true);
            textBoxMaterialNameValue.Text = string.Empty;
            comboBoxMaterialTypeValue.SelectedItem = null;
            UpdateSpecialMaterials();
        }

        protected void OnClickButtonDeleteMaterialInformation(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxMaterialNameValue.Text) || comboBoxMaterialTypeValue.SelectedItem == null || string.IsNullOrEmpty(comboBoxMaterialTypeValue.SelectedItem.ToString()))
                return;

            mainSequence.ReelTowerGroupObject.RemoveSpecialMaterialData(textBoxMaterialNameValue.Text, true);
            textBoxMaterialNameValue.Text = string.Empty;
            comboBoxMaterialTypeValue.SelectedItem = null;
            UpdateSpecialMaterials();
        }

        protected void OnClickButtonSaveMaterialInformation(object sender, EventArgs e)
        {
            mainSequence.ReelTowerGroupObject.SaveSpecialMaterialData();
        }
        #endregion

        #region Display language setting methods
        protected void OnClickButtonSaveGuiSettings(object sender, EventArgs e)
        {
            Config.SaveDisplayLanguage(comboBoxDisplayLanguage.SelectedItem.ToString());
        }
        #endregion

        protected void OnKeyUpStoreReelPage(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        {
                            if (sender is TextBox)
                            {
                                string context_ = (sender as TextBox).Text;
                                if (context_.Length > 2)
                                {
                                    if (context_.Contains(";"))
                                    {
                                        if (!ParseInputContext(context_))
                                        {
                                            if (FormMessageExt.ShowQuestion(string.Format(Properties.Resources.String_Question_NotRecognizedQrCodeRetry, Environment.NewLine, context_)) == DialogResult.Yes)
                                            {
                                                textBoxLoadReelArticleValue.Text = string.Empty;
                                                textBoxLoadReelCarrierValue.Text = string.Empty;
                                                textBoxLoadReelLotIdValue.Text = string.Empty;
                                                textBoxLoadReelSupplierValue.Text = string.Empty;
                                                numericUpDownLoadReelQtyValue.Value = 0;
                                                textBoxLoadReelMfgValue.Text = string.Empty;
                                            }
                                        }
                                        else
                                        {
                                            (sender as TextBox).Select(context_.Length, 0);
                                        }
                                    }
                                    else if (context_.Contains("RQ"))
                                    {
                                        int pos_ = context_.LastIndexOf("RQ");
                                        string remained_ = context_.Substring(pos_, context_.Length - pos_);

                                        if (!string.IsNullOrEmpty(remained_))
                                        {
                                            numericUpDownLoadReelQtyValue.Value = Convert.ToInt32(remained_.Substring(2, remained_.Length - 2));
                                            (sender as TextBox).Text = context_.Substring(0, pos_);
                                            (sender as TextBox).Select((sender as TextBox).Text.Length, 0);
                                        }
                                    }
                                }

                                if ((sender as TextBox).Name == "textBoxLoadReelCommentValue")
                                    textBoxLoadReelCommentValue.Text = string.Empty;
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected void OnClickButtonSaveConfig(object sender, EventArgs e)
        {
            Config.Save();
        }

        protected void UpdateSpecialMaterials()
        {
            try
            {
                dataGridViewMaterialInformation.Rows.Clear();

                foreach (KeyValuePair<string, string> item_ in mainSequence.ReelTowerGroupObject.SpecialMaterials)
                    dataGridViewMaterialInformation.Rows.Add(item_.Key, item_.Value);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        private void comboBoxAMMUsageValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.SelectedIndexChange();
        }

        private void labelRobotStatus_DoubleClick(object sender, EventArgs e)
        {
            mainSequence.ReelHandlerReset();
        }

        private void buttonSaveTower_Click(object sender, EventArgs e)
        {
            int index_ = 1;
            for (int i = 0; i < mainSequence.ReelTowerGroupObject.Towers.Count(); i++)
            {
                mainSequence.ReelTowerGroupObject.Towers[index_].Usage = dataGridViewTower.Rows[i].Cells[2].Value.ToString().ToLower();

                ReelTowerState state_ = mainSequence.ReelTowerGroupObject.GetTowerStateById(mainSequence.ReelTowerGroupObject.Towers[index_].Id);
                index_++;
                UpdateReelTowerStates(state_);
            }
        }

        private void buttonMissmathSearch_Click(object sender, EventArgs e)
        {
            string operator_ = string.Empty;
            DataTable result_ = null;
            DataSet result_Query = null;
            List<Pair<string, string>> queryItems_ = new List<Pair<string, string>>();

            if (checkBoxQueryByCustomerDB.Checked)
            {
                if (checkBoxQueryByTowerId.Checked)
                {
                    if (comboBoxQueryReelTowerIdValue.SelectedItem == null)
                        return;

                    result_ = mainSequence.MissMatchSearch(0, comboBoxQueryReelTowerIdValue.Text);
                }
                else if (checkBoxQueryByArticle.Checked)
                {
                    if (string.IsNullOrEmpty(textBoxQueryReelArticleValue.Text))
                        return;

                    result_ = mainSequence.MissMatchSearch(1, textBoxQueryReelArticleValue.Text);
                }
                else if (checkBoxQueryByCarrier.Checked)
                {
                    if (string.IsNullOrEmpty(textBoxQueryReelCarrierValue.Text))
                        return;

                    result_ = mainSequence.MissMatchSearch(2, textBoxQueryReelCarrierValue.Text);
                }
                else
                {
                    result_ = mainSequence.MissMatchSearch(3, "");
                }

                if (result_ != null && result_.Rows.Count > 0)
                {
                    result_Query = ExecuteQueryReel(false);
                    List<int> indexs_ = new List<int>();

                    if (radioButtonSearchAll.Checked)
                    {
                        for (int i = 0; i < result_Query.Tables[0].Rows.Count; i++)
                        {
                            DataRow[] rows = result_.Select($"[UID]='{result_Query.Tables[0].Rows[i][0]}'");
                            if (rows.Count() > 0)
                            {
                                int index = result_.Rows.IndexOf(rows[0]);
                                result_.Rows[index][3] = result_Query.Tables[0].Rows[i][4];
                            }
                            else
                            {
                                result_.Rows.Add(
                                    FormatStringToDate(result_Query.Tables[0].Rows[i][5].ToString(), true),
                                    mainSequence.ReelTowerGroupObject.LineCode,
                                    mainSequence.ReelTowerGroupObject.GroupName,
                                    result_Query.Tables[0].Rows[i][4],
                                    result_Query.Tables[0].Rows[i][0],
                                    result_Query.Tables[0].Rows[i][1],
                                    result_Query.Tables[0].Rows[i][10],
                                    result_Query.Tables[0].Rows[i][6],
                                    result_Query.Tables[0].Rows[i][9],
                                    FormatStringToDate(result_Query.Tables[0].Rows[i][11].ToString()),
                                    result_Query.Tables[0].Rows[i][7],
                                    "TOWER");
                            }
                            
                        }
                    }
                    else
                    {
                        for (int i = 0; i < result_Query.Tables[0].Rows.Count; i++)
                        {
                            DataRow[] rows = result_.Select($"[UID]='{result_Query.Tables[0].Rows[i][0]}'");
                            if (rows.Count() > 0)
                            {
                                result_.Rows.Remove(rows[0]);
                            }
                            else
                            {
                                result_.Rows.Add(
                                    FormatStringToDate(result_Query.Tables[0].Rows[i][5].ToString(), true),
                                    mainSequence.ReelTowerGroupObject.LineCode,
                                    mainSequence.ReelTowerGroupObject.GroupName,
                                    result_Query.Tables[0].Rows[i][4],
                                    result_Query.Tables[0].Rows[i][0],
                                    result_Query.Tables[0].Rows[i][1],
                                    result_Query.Tables[0].Rows[i][10],
                                    result_Query.Tables[0].Rows[i][6],
                                    result_Query.Tables[0].Rows[i][9],
                                    FormatStringToDate(result_Query.Tables[0].Rows[i][11].ToString()),
                                    result_Query.Tables[0].Rows[i][7],
                                    "TOWER");
                            }
                        }
                    }
                    dataGridViewQueryResults.DataSource = result_;
                    buttonExecuteDBSync.Visible = true;
                    buttonExecuteTowerSync.Visible = true;

                    for (int i = 0; i < dataGridViewQueryResults.Rows.Count; i++)
                    {
                        if (dataGridViewQueryResults.Rows[i].Cells[3].Value.ToString().Trim().Length <= 5)
                            dataGridViewQueryResults.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                        else if (dataGridViewQueryResults.Rows[i].Cells[11].Value.ToString() == "TOWER")
                            dataGridViewQueryResults.Rows[i].DefaultCellStyle.BackColor = Color.SkyBlue;
                    }
                }

            }
        }

        public static string FormatStringToDate(string tdrDate, bool state = false)
        {
            string result_ = string.Empty;

            if (state)
            {
                DateTime dt = Convert.ToDateTime(tdrDate);
                result_ = dt.ToString("yyyyMMddhhmmss");
            }
            else
            {
                result_ = DateTime.ParseExact(tdrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd");
            }

            return result_;
        }

        private void buttonExecuteDBSync_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridViewQueryResults.Rows.Count; i++)
            {
                if (dataGridViewQueryResults.Rows[i].Cells[3].Value.ToString().Trim().Length <= 5)
                {
                    string recv_result = string.Empty;
                    try
                    {
                        recv_result = mainSequence.SetUnloadOut_Manual(dataGridViewQueryResults.Rows[i].Cells[4].Value.ToString(), mainSequence.AMMWebServiceResult);
                    }
                    catch (Exception exp)
                    {
                        recv_result = "FAILDE_WEBSERVICE";
                        mainSequence.AMMWebServiceResult = false;
                        mainSequence.SetUnloadOut_Manual(dataGridViewQueryResults.Rows[i].Cells[4].Value.ToString(), mainSequence.AMMWebServiceResult);
                        Logger.Alarm($"AMM Alarm=SetUnloadOut_Manual:{recv_result}");
                    }

                    if (recv_result == "FAILDE_WEBSERVICE")
                    {
                        mainSequence.AMMWebServiceResult = false;
                        Logger.Alarm($"AMM Alarm=SetUnloadOut_Manual:{recv_result}");

                        if (!mainSequence.AMMstatusUpdateTimer.Enabled)
                            mainSequence.AMMstatusUpdateTimer.Enabled = true;
                    }
                    else if (recv_result == "NG")
                    {
                        mainSequence.CommunicationStatesOfAMM = CommunicationStates.Disconnected;
                        mainSequence.CommunicationWaitOfAMM = CommunicationStates.None;
                        Logger.Alarm($"AMM Alarm=SetUnloadOut_Manual:{recv_result}");
                    }
                }
            }

        }

        private void buttonExecuteTowerSync_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridViewQueryResults.Rows.Count; i++)
            {
                if (dataGridViewQueryResults.Rows[i].Cells[11].Value.ToString().ToUpper() == "TOWER")
                {
                    string recv_result = string.Empty;
                    string[] split_time = dataGridViewQueryResults.Rows[i].Cells[9].Value.ToString().Split('-');
                    string time = split_time[0] + split_time[1] + split_time[2];

                    string[] split_twrid = dataGridViewQueryResults.Rows[i].Cells[3].Value.ToString().Split(',');
                    split_twrid = split_twrid[0].Split(' ');

                    MaterialData md = new MaterialData();
                    md.ManufacturedDatetime = time;
                    md.TowerId = split_twrid[1];
                    md.Name = dataGridViewQueryResults.Rows[i].Cells[4].Value.ToString();
                    md.Category = dataGridViewQueryResults.Rows[i].Cells[5].Value.ToString();
                    md.LotId = dataGridViewQueryResults.Rows[i].Cells[6].Value.ToString();
                    md.Quantity = Convert.ToInt32(dataGridViewQueryResults.Rows[i].Cells[7].Value.ToString());
                    md.Supplier = dataGridViewQueryResults.Rows[i].Cells[8].Value.ToString();
                    md.Size = Convert.ToInt32(dataGridViewQueryResults.Rows[i].Cells[10].Value.ToString());
                    md.LoadType = LoadMaterialTypes.Cart;

                    try
                    {
                        recv_result = mainSequence.SetLoadComplete(md, mainSequence.AMMWebServiceResult);
                    }
                    catch (Exception exp)
                    {
                        recv_result = "FAILDE_WEBSERVICE";
                        mainSequence.AMMWebServiceResult = false;
                        recv_result = mainSequence.SetLoadComplete(md, mainSequence.AMMWebServiceResult);
                        Logger.Alarm($"AMM Alarm=SetLoadComplete:{recv_result}");
                    }

                    if (recv_result == "FAILDE_WEBSERVICE")
                    {
                        mainSequence.AMMWebServiceResult = false;
                        Logger.Alarm($"AMM Alarm=SetLoadComplete:{recv_result}");

                        if (!mainSequence.AMMstatusUpdateTimer.Enabled)
                            mainSequence.AMMstatusUpdateTimer.Enabled = true;
                    }
                    else if (recv_result == "NG")
                    {
                        mainSequence.CommunicationStatesOfAMM = CommunicationStates.Disconnected;
                        mainSequence.CommunicationWaitOfAMM = CommunicationStates.None;
                        Logger.Alarm($"AMM Alarm=SetLoadComplete:{recv_result}");
                    }
                }
            }

        }

        private void OnComboBoxSelectedIndexChangedProperty(object sender, EventArgs e)
        {
            if (sender is ComboBox)
            {
                ComboBox ctrl_ = sender as ComboBox;

                switch (ctrl_.Name)
                {
                    case "comboBoxMaterialValidationValue":
                        Config.Properties["MaterialValidation"].Value = comboBoxMaterialValidationValue.Text.ToLower();
                        break;
                    case "comboBoxMaterialArriveReportValue":
                        Config.Properties["MaterialArriveReport"].Value = comboBoxMaterialArriveReportValue.Text.ToLower();
                        break;
                    case "comboBoxMaterialRemoveReportValue":
                        Config.Properties["MaterialRemoveReport"].Value = comboBoxMaterialRemoveReportValue.Text.ToLower();
                        break;
                    case "comboBoxReelHandlerUsageValue":
                        {
                            Config.Properties["ReelHandlerUsage"].Value = comboBoxReelHandlerUsageValue.Text.ToLower();
                            if (comboBoxReelHandlerUsageValue.Text.ToLower() == "true")
                                Config.ReelHandlerUsage = true;
                            else if (comboBoxReelHandlerUsageValue.Text.ToLower() == "false")
                                Config.ReelHandlerUsage = false;
                            
                        }
                        break;
                    case "comboBoxAssignedRejectPortValue":
                        Config.Properties["AssignedRejectPort"].Value = comboBoxAssignedRejectPortValue.Text.ToLower();
                        break;
                    case "comboBoxRemapCreateTimeByMFGValue":
                        Config.Properties["RemapCreateTimeByMFG"].Value = comboBoxRemapCreateTimeByMFGValue.Text.ToLower();
                        break;
                    case "comboBoxProvideModeValue":
                        Config.Properties["ProvideMode"].Value = comboBoxProvideModeValue.Text;
                        break;
                    case "comboBoxAMMUsageValue":
                        {
                            Config.Properties["AMMUsage"].Value = comboBoxAMMUsageValue.Text.ToLower();
                            if (comboBoxAMMUsageValue.Text.ToLower() == "true")
                            {
                                Config.AMMUsage = true;
                                labelAMMStatus.BackColor = SystemColors.HotTrack;
                            }
                            else if (comboBoxAMMUsageValue.Text.ToLower() == "false")
                            {
                                Config.AMMUsage = false;
                                labelAMMStatus.BackColor = SystemColors.ControlDarkDark;
                            }
                        }
                        break;
                    case "comboBoxRejectAutoUsageValue":
                        Config.Properties["RejectAutoUsage"].Value = comboBoxRejectAutoUsageValue.Text.ToLower();
                        
                        break;
                    case "comboBoxAMMWebserviceUsageValue":
                        {
                            Config.Properties["AMMWebserviceUsage"].Value = comboBoxAMMWebserviceUsageValue.Text.ToLower();
                            if (comboBoxAMMWebserviceUsageValue.Text.ToLower() == "true")
                            {
                                Config.AMMWebserviceUsage = true;
                            }
                            else if (comboBoxAMMWebserviceUsageValue.Text.ToLower() == "false")
                            {
                                Config.AMMWebserviceUsage = false;
                            }
                        }
                        break;
                }
            }
            else if (sender is NumericUpDown)
            {
                NumericUpDown ctrl_ = sender as NumericUpDown;

                switch (ctrl_.Name)
                {
                    case "numericUpDownJobSplitReelCountValue":
                        Config.Properties["JobSplitReelCount"].Value = numericUpDownJobSplitReelCountValue.Value;
                        break;
                    case "numericUpDownTimeoutOfRejectValue":
                        Config.Properties["JobSplitReelCount"].Value = numericUpDownJobSplitReelCountValue.Value;
                        break;
                }

            }
        }

        private void backgroundWorkerAMMAlive_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (!shutdownApp)
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        //Send heart beat
                        {
                            if (mainSequence.CommunicationStatesOfAMM != CommunicationStates.Disconnected && Config.AMMUsage)
                            {
                                mainSequence.SetEqAlive(
                                                    ++heartbeat_ % 2
                                                    );
                            }

                        }
                    }));
                }
                else
                {
                    //Send heart beat
                    {
                        if (mainSequence.CommunicationStatesOfAMM != CommunicationStates.Disconnected && Config.AMMUsage)
                        {
                            mainSequence.SetEqAlive(
                                                ++heartbeat_ % 2
                                                );
                        }
                    }
                }
                Thread.Sleep(30000);
            }
        }

        private void backgroundWorkerAMMAlive_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Canceled");
            }
            else if (e.Error != null)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Error={e.Error.Message}");
            }
            else
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Done");
            }

        }

        public void SetFocus(GuiPages page = GuiPages.MainPage)
        {
            throw new NotImplementedException();
        }

        private void buttonTakeoutByRobot_Click(object sender, EventArgs e)
        {
            if (buttonTakeoutByRobot.Text == "로봇 배출")
            {
                buttonTakeoutByRobot.Text = "수동 배출";
                buttonTakeoutByRobot.ForeColor = Color.Blue;
                buttonTakeoutByRobot.BackColor = Color.LightBlue;
                checkBoxTakeoutByRobot.Checked = false;
            }
            else
            {
                buttonTakeoutByRobot.Text = "로봇 배출";
                buttonTakeoutByRobot.ForeColor = Color.Green;
                buttonTakeoutByRobot.BackColor = Color.LightGreen;
                checkBoxTakeoutByRobot.Checked = true;
            }
        }
    }
}
#endregion