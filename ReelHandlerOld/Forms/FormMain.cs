#region Imports
using TechFloor.Components;
using TechFloor.Forms;
using TechFloor.Object;
using TechFloor.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TechFloor.Device;
using TechFloor.Gui;
using System.Threading.Tasks;
using TechFloor.Shared;
using System.ServiceProcess;
#endregion

#region Program
#pragma warning disable CS1058 CS0219 CA1401
namespace TechFloor
{
    public partial class FormMain : Form, IMainFormExt
    {
        #region Imports
        [DllImport("user32.dll", SetLastError = true)]
        protected static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        protected static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        protected static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        protected static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, EntryPoint = "SendMessage")]
        protected static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, EntryPoint = "SendMessage")]
        protected static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        [DllImport("user32.dll")]
        protected static extern IntPtr SetParent(IntPtr hWnd, IntPtr hwp);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public int Width => Right - Left;
            public int Height => Bottom - Top;
            public Point Center => new Point(Width / 2, Height / 2);
        }

        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public UInt32 cbData;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpData;
        }

        public enum CameraInputs : int
        {
            AutoRun,
            Reset,
            Trigger,
            Calibration,
            InspectionMode,
            Inspection,
            AutoLive,
            ChangeJob,
            LoadParameter,
            ChangeDisplayWindow,
            MoveWindow,
            OffsetMoveWindow,
            CloseProgram,
        }

        public enum CameraOutputs : int
        {
            AutoRun,
            Ready,
            Busy,
            GrabEnd,
            Result,
            ChangeJob,
            LightValue,
        };
        #endregion

        #region Constants
        protected const int SW_SHOWNORMAL = 1;
        protected const int SW_SHOWMINIMIZED = 2;
        protected const int SW_SHOWMAXIMIZED = 3;
        protected const int WM_QUIT = 0x0012;
        protected const int WM_CLOSE = 0x0010;
        protected const int WM_COPYDATA = 0x004A;
        protected const int HWND_TOPMOST = -1;
        protected const int HWND_NOTOPMOST = -2;
        protected const int SWP_NOMOVE = 0x0002;
        protected const int SWP_NOSIZE = 0x0001;
        protected const int SWP_NOZORDER = 0x0004;
        protected const int SWP_SHOWWINDOW = 0x0040;
        protected const int CONST_VISION_PROCESS_CHECK_TIMEOUT = 10; // Max 10 sec.
        protected const string CONST_REEL_TOWER_LOG_FILE = "TowerComm.log";
        protected readonly char[] CONST_LIGHT_VALUE_DELIMITER = { ',' };
        protected readonly char[] CONST_VISION_DATA_DELIMITER = { ';' };
        protected readonly string CONST_SERVICE_NAME = "TechFloorService";
        #endregion

        #region Fields
        protected bool visionState = false;
        protected bool visionInitialized = false;
        protected bool shutdown = false;
        protected int tickCount = 0;
        protected int elapsedTick = App.TickCount;
        protected DateTime startedDateTime = DateTime.Now;
        protected Screen displayMonitor = null;
        protected string visionResponse = string.Empty;
        protected object thisLock = new object();
        protected App appInstance_ = null;
        protected CommunicationStates currentReelTowerCommState = CommunicationStates.None;
        protected CommunicationStates currentRobotSequenceCommState = CommunicationStates.None;
        protected CommunicationStates currentRobotControllerCommState = CommunicationStates.None;
        protected CommunicationStates currentMobileRobotCommState = CommunicationStates.None;
        protected VisionControlStates currentVisionControlState = VisionControlStates.Shutdown;
        protected OperationStates operationState = OperationStates.PowerOn;
        protected AlarmStates alarmState = AlarmStates.Cleared;
        protected ManualResetEvent shutdownEvent = new ManualResetEvent(false);
        protected ReelTowerRobotSequence mainSequence = null;
        protected DigitalIoManager digitalIo = null;
        protected VisionLightManager visionLight = null;
        protected IntPtr visionWnd = IntPtr.Zero;
        protected FormBarcodeInput barcodeInputWindow = null;
        protected FormMessageExt notificationWindow = null;
        protected FormMessageExt notificationDockWindow = null;
        protected ProductionRecord StatsLogger = null;                       // Stats form object
        protected object lightSyncObject = new object();

        private FormSplash splashScreen = null;
        private FormLight lightControlWnd = null;
        private Hardworker splashWorker = null;
        private bool done = false;
        private FormMonitorStages monitorStages = null;
        #endregion

        #region Events
        public event EventHandler ChangedVisionState;
        #endregion

        #region Properties
        public Screen DisplayMonitor => displayMonitor;

        public bool IsShutdown => shutdown;

        public bool UseMobileRobot
        {
            get;
            protected set;
        }

        public OperationStates OperationState
        {
            get => mainSequence.OperationState;
        }

        public AlarmStates AlarmState
        {
            get => alarmState;
            set => alarmState = value;
        }

        public IDigitalIoManager DigitalIoManager => digitalIo;

        public DigitalIoManager DigitalIo => digitalIo;

        public IMainSequence MainSequence => mainSequence;

        public WaitHandle ShutdownEvent => shutdownEvent;

        public ReelTowerManager ReelTowerManager => mainSequence.ReelTowerManager;

        public MobileRobotManager MobileRobotManager => mainSequence.MobileRobotManager;

        public RobotSequenceManager RobotSequenceManager => mainSequence.RobotSequenceManager;

        public VisionLightManager VisionLightManager => visionLight;
        #endregion

        #region Delegation
        public delegate void UpdateText(Control ctrl, string text);
        #endregion

        #region Constructors
        public FormMain(App app = null)
        {
            InitializeComponent();
            appInstance_ = app;
            StatsLogger = new ProductionRecord();
            splashScreen = new FormSplash();
        }
        #endregion

        #region Create / Destroy elements
        protected void LoadSystemConfig()
        {
            Logger.Create();
            Logger.AddCategory("Alarm");
            Logger.Trace($"Start application (Name={App.Name}, Version={App.Version})", "System");

            if (!Config.Load() | !Model.Load())
            {
                Logger.Trace("Failed to load configuration or model information from file!");
                FormMessageExt.ShowNotification(Properties.Resources.String_FormMain_Notification_Load_Config_Failure);

                buttonExit.PerformClick();
            }

            SetVariables();
            labelApplicationVersion.Text = $"Revision: {App.Version}";
        }

        protected void CreateFileSystem()
        {
            Directory.CreateDirectory(ReelTowerRobotSequence.CONST_CONFIG_PATH);
            Directory.CreateDirectory(ReelTowerRobotSequence.CONST_MODEL_PATH);
            Directory.CreateDirectory(ReelTowerRobotSequence.CONST_DATA_PATH);
            Directory.CreateDirectory(ReelTowerRobotSequence.CONST_PACKAGE_PATH);
        }

        protected void CreateElements()
        {
            bool runVision_ = false;

            CreateFileSystem();
            splashWorker.SetProgress(30);
            mainSequence = new ReelTowerRobotSequence();
            digitalIo = new DigitalIoManager(true);
            visionLight = new VisionLightManager(Config.VisionLight);

            mainSequence.TriggerVisionControl += OnTriggerVisionControl;

            if ((DigitalIo.Simulation = Config.SystemSimulation) || DigitalIo.Open())
            {   // Set signal tower state first state.
                DigitalIo.SetSignalTower(OperationStates.PowerOn);
                visionLight.Connect(); // Don't care during simulation.
                // Show display language
                foreach (string item in comboBoxDisplayLanguage.Items)
                {
                    if (item.Contains(App.CultureInfoCode))
                    {
                        comboBoxDisplayLanguage.Text = item;
                        break;
                    }
                }

                if (!Config.SystemSimulation)
                    mainSequence.Create();

                if (backgroundWorker.IsBusy != true)
                    backgroundWorker.RunWorkerAsync();

                if (Config.ImageProcessorType == ImageProcessorTypes.Mit)
                {
                    VisionManager.ReadingDataInit();

                    if (runVision_ = CheckVisionProcess())
                        FormMessageExt.ShowNotificationWithBuzzer(Properties.Resources.String_FormMain_Notification_Vision_Process_Launch_Failure);
                }
                else
                {
                    buttonVisionStart.Text = Properties.Resources.String_FormMain_buttonVisionStart_TF_Run;
                    buttonVisionStart.Enabled = false;
                }

                if (!runVision_)
                {
                    splashWorker.SetProgress(50);
                    StartVisionProcess();
                }

                mainSequence.ForceSetCartState();
            }

            Singleton<MaterialPackageManager>.Instance.SetExportPath();
            controlDigitalIo1.Create();
            UpdateOperationState();
            splashWorker.SetProgress(80);
        }

        protected void DestroyElements()
        {   // Kill all threads and dispose resources.
            try
            {
                shutdownEvent.Set();

                if (mainSequence != null)
                {
                    mainSequence.StartElements();
                    mainSequence.Dispose();
                }

                if (DigitalIo != null)
                {   // Set signal tower state last state.
                    DigitalIo.SetSignalTower(OperationStates.PowerOff);
                    DigitalIo.UndockCart();
                    DigitalIo.Close();
                    DigitalIo.Dispose();
                }

                if (visionLight != null)
                {
                    visionLight.Disconnect();
                    visionLight.Dispose();
                }

                if (monitorStages != null)
                {
                    monitorStages.Close();
                }

                ShutdownVisionProcess();

                if (Config.ImageProcessorType == ImageProcessorTypes.Mit)
                    VisionManager._CILightOff();
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }
            finally
            {
                Logger.Trace($"Stop application (Name={App.Name}, Version={App.Version})");
                Logger.Destroy();
            }
        }

        protected void StartElements()
        {
            if (mainSequence != null)
                mainSequence.StartElements();
        }

        protected void StopElements()
        {
            if (mainSequence != null)
                mainSequence.StopElements();
        }
        #endregion

        #region Attach / Detach event handlers
        protected void AttachEventHandlers()
        {
            mainSequence.OperationStateChanged += OnOperationStateChanged;
            mainSequence.FinishedCycleStop += OnFinishedCycleStop;
            mainSequence.CycleStopOrderStateChanged += OnCycleStopOrderStateChanged;
            mainSequence.CartPresentStateChanged += OnCartPresentStateChanged;
            mainSequence.ChangedReelSizeOfCart += OnChangedReelSizeOfCart;
            mainSequence.NotifyEvent += OnNotificationOfMainSequence;
            
            ChangedVisionState += OnChangedVisionState;

            Singleton<TransferMaterialObject>.Instance.AttachEventHander(OnChangedTransferMaterialInformation);

            mainSequence.ReelTowerManager.CommunicationStateChanged += OnChangedReelTowerCommunicationState;
            mainSequence.ReelTowerManager.ReportRuntimeLog += UpdateReelTowerRuntimeLog;
            mainSequence.ReelTowerManager.ReceivedMaterialPackage += OnReceivedMaterialPackage;
            Singleton<MaterialPackageManager>.Instance.AddedMaterialPackage += OnAddedMaterialPackage;

            mainSequence.NotifyToShowBarcodeInputWindow += OnShowBarcodeInputWindow;
            mainSequence.NotifyToShowDockCartWindow += OnShowDockCartWindow;
            mainSequence.NotifyProductionInformation += OnProductionInformation;
        }

        protected void DetachEventHandlers()
        {
            mainSequence.OperationStateChanged -= OnOperationStateChanged;
            mainSequence.FinishedCycleStop -= OnFinishedCycleStop;
            mainSequence.CycleStopOrderStateChanged -= OnCycleStopOrderStateChanged;
            mainSequence.CartPresentStateChanged -= OnCartPresentStateChanged;
            mainSequence.ChangedReelSizeOfCart -= OnChangedReelSizeOfCart;
            mainSequence.NotifyEvent -= OnNotificationOfMainSequence;

            ChangedVisionState -= OnChangedVisionState;

            Singleton<TransferMaterialObject>.Instance.DetachEventHandler(OnChangedTransferMaterialInformation);

            mainSequence.ReelTowerManager.CommunicationStateChanged -= OnChangedReelTowerCommunicationState;
            mainSequence.ReelTowerManager.ReportRuntimeLog -= UpdateReelTowerRuntimeLog;
            mainSequence.ReelTowerManager.ReceivedMaterialPackage -= OnReceivedMaterialPackage;
            Singleton<MaterialPackageManager>.Instance.AddedMaterialPackage -= OnAddedMaterialPackage;

            mainSequence.NotifyToShowBarcodeInputWindow -= OnShowBarcodeInputWindow;
            mainSequence.NotifyToShowDockCartWindow -= OnShowDockCartWindow;
            mainSequence.NotifyProductionInformation -= OnProductionInformation;
        }
        #endregion

        #region Form event handlers
        private void ShowSplashScreen()
        {
            splashScreen.Show();
            while (!done)
            {
                Application.DoEvents();
            }
            splashScreen.Close();
            this.splashScreen.Dispose();
        }

        protected bool IsServiceRunning(string serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);

            switch (sc.Status)
            {
                case ServiceControllerStatus.Running:
                    return true;
                case ServiceControllerStatus.Stopped:
                case ServiceControllerStatus.Paused:
                case ServiceControllerStatus.StopPending:
                case ServiceControllerStatus.StartPending:
                default:
                    return false;
            }
        }

        protected void StartService(string serviceName, int timeout = 10000)
        {
            ServiceController sc = new ServiceController(serviceName);
            try
            {
                TimeSpan t = TimeSpan.FromMilliseconds(timeout);
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, t);
            }
            catch (Exception ex)
            {
                Logger.Trace($"StartService: Exception={ex.Message}");
            }
        }

        protected void StopService(string serviceName, int timeout = 10000)
        {
            ServiceController sc = new ServiceController(serviceName);
            try
            {
                TimeSpan t = TimeSpan.FromMilliseconds(timeout);
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Running, t);
            }
            catch (Exception ex)
            {
                Logger.Trace($"StartService: Exception={ex.Message}");
            }
        }

        protected void OnFormLoad(object sender, EventArgs e)
        {
            this.Hide();
            GetCurrentMonitor();
            Thread thread = new Thread(new ThreadStart(this.ShowSplashScreen));
            thread.Start();
            splashWorker = new Hardworker();
            splashWorker.ProgressChanged += (o, ex) => {
                this.splashScreen.UpdateProgress(ex.Progress);
            };
            splashWorker.HardWorkDone += (o, ex) => {
                done = true;
                this.Show();
            };

            if (!IsServiceRunning(CONST_SERVICE_NAME))
                StartService(CONST_SERVICE_NAME);

            LoadSystemConfig();
            splashWorker.SetProgress(10);
            CreateElements();
            AttachEventHandlers();
            StartElements();
            SetDisplayLanguage();
            splashWorker.SetProgress(100);
        }

        protected void OnFormClosed(object sender, FormClosedEventArgs e)
        {
        }

        protected void OnFormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (shutdown || !(e.Cancel = (e.CloseReason == CloseReason.UserClosing)))
            {
                VisionLightManager.SendCommand(VisionLightManager.ControllerCommands.Off, 1, 0);// "F1000#");
                DestroyElements();
            }
        }
        #endregion

        #region Configuration methods
        protected void SetVariables()
        {   // Set default value of variables and properties.
            UseMobileRobot = true;
            labelReelTowerId1.Text = Config.ReelTowerIds[1].second;
            labelReelTowerId2.Text = Config.ReelTowerIds[2].second;
            labelReelTowerId3.Text = Config.ReelTowerIds[3].second;
            labelReelTowerId4.Text = Config.ReelTowerIds[4].second;
            textBoxReelTowerAddress.Text = Config.ReelTowerLocalServerAddress;
            textBoxReelTowerPort.Text = $"{Config.ReelTowerLocalServerPort}";
            textBoxRobotPort.Text = $"{Config.RobotLocalServerPort}";
            textBoxRobotControllerAddress.Text = Config.RobotRemoteServerAddress;
            textBoxRobotControllerPort.Text = $"{Config.RobotRemoteServerPort}";
            textBoxMobileRobotAddress.Text = Config.MobilRobotLocalServerAddress;
            textBoxMobileRobotPort.Text = $"{Config.MobileRobotLocalServerPort}";
            textBoxReelTowerResponseTimeout.Text = $"{Config.TimeoutOfReelTowerResponse}";
            textBoxCartInOutCheckTimeout.Text = $"{Config.TimeoutOfCartInOutCheck}";
            textBoxRobotCommunicationTimeout.Text = $"{Config.TimeoutOfRobotCommunication}";
            textBoxRobotProgramLoadTimeout.Text = $"{Config.TimeoutOfRobotProgramLoad}";
            textBoxRobotProgramPlayTimeout.Text = $"{Config.TimeoutOfRobotProgramPlay}";
            textBoxRobotActionTimeout.Text = $"{Config.TimeoutOfRobotAction}";
            textBoxRobotMoveTimeout.Text = $"{Config.TimeoutOfRobotMove}";
            textBoxRobotHomeTimeout.Text = $"{Config.TimeoutOfRobotHome}";
            textBoxVisionAlignmentRangeLimitX.Text = $"{Model.AlignmentRangeLimit.X}";
            textBoxVisionAlignmentRangeLimitY.Text = $"{Model.AlignmentRangeLimit.Y}";
            textBoxVisionFailureRetry.Text = $"{Model.RetryOfVisionFailure}";
            textBoxVisioinRetryAttempts.Text = $"{Model.RetryOfVisionAttempts}";
            textBoxImageProcessingTimeout.Text = $"{Model.TimeoutOfImageProcessing}";
            textBoxDelayOfCameraTrigger.Text = $"{Model.DelayOfTrigger}";
            textBoxReturnReelSensingDelay.Text = $"{Model.DelayOfReturnReelSensing}";
            textBoxImageProcessingDelay.Text = $"{Model.DelayOfImageProcessing}";
            textBoxUnloadReelStateUpdateDelay.Text = $"{Model.DelayOfUnloadReelStateUpdate}";
        }
        #endregion

        #region Update state information
        protected void UpdateCommunicationState(Control displayControl, CommunicationStates state)
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

        protected void UpdateRobotSequenceState()
        {
            string reelsize_ = string.Empty;

            if (mainSequence.RobotSequenceManager.IsFailure)
            {
                labelRobotState.BackColor = Color.Red;
                labelRobotState.ForeColor = Color.Yellow;
                labelRobotState.Text = Properties.Resources.String_FormMain_Robot_Action_State_Failed;
            }
            else
            {
                labelRobotState.BackColor = Color.Lime;
                labelRobotState.ForeColor = SystemColors.ControlText;

                switch (mainSequence.RobotSequenceManager.ActionState)
                {
                    case RobotActionStates.Stop:
                        labelRobotState.Text = Properties.Resources.String_FormMain_Robot_Action_State_Stopped;
                        break;
                    case RobotActionStates.Loading:
                        labelRobotState.Text = Properties.Resources.String_FormMain_Robot_Action_State_Do_Loading;
                        break;
                    case RobotActionStates.LoadCompleted:
                        labelRobotState.Text = Properties.Resources.String_FormMain_Robot_Action_State_Completed_Load;
                        break;
                    case RobotActionStates.Unloading:
                        labelRobotState.Text = Properties.Resources.String_FormMain_Robot_Action_State_Do_Unloading;
                        break;
                    case RobotActionStates.UnloadCompleted:
                        labelRobotState.Text = Properties.Resources.String_FormMain_Robot_Action_State_Completed_Unload;
                        break;
                    default:
                        {
                            if (mainSequence.RobotSequenceManager.ActionState == RobotActionStates.Unknown)
                            {
                                labelRobotState.BackColor = SystemColors.ControlDarkDark;
                                labelRobotState.ForeColor = SystemColors.Window;
                            }

                            labelRobotState.Text = mainSequence.RobotSequenceManager.ActionState.ToString().ToUpper();
                        }
                        break;
                }
            }

            switch (mainSequence.RobotStep)
            {
                case ReelHandlerSteps.None:
                case ReelHandlerSteps.Ready:
                case ReelHandlerSteps.CheckProgramState:
                    break;
                case ReelHandlerSteps.SetReelTypeOfReturnToRobot:
                case ReelHandlerSteps.MoveToFrontOfReturnStage:
                case ReelHandlerSteps.ApproachToReelHeightCheckPointAtReturnStage:
                case ReelHandlerSteps.MeasureReelHeightOnReturnStage:
                    {
                        switch (mainSequence.CurrentReelTypeOfReturn)
                        {
                            case ReelDiameters.ReelDiameter7:
                                reelsize_ = Properties.Resources.String_FormMain_Return_Reel_Type_7_Inch;
                                break;
                            case ReelDiameters.ReelDiameter13:
                                reelsize_ = Properties.Resources.String_FormMain_Return_Reel_Type_13_Inch;
                                break;
                            default:
                                reelsize_ = "-";
                                break;
                        }

                        if (reelsize_ != controlStatusLabelCartInchCheck.ValueText)
                            controlStatusLabelCartInchCheck.SetStatusValue(reelsize_, Color.Lime, SystemColors.ControlText, SystemColors.Desktop, SystemColors.Window);

                        if (controlStatusLabelVisionAlign.StatusBackColor != SystemColors.Highlight)
                            controlStatusLabelVisionAlign.SetStatusValue("-", SystemColors.Highlight, SystemColors.Window, SystemColors.Desktop, SystemColors.Window);

                        if (controlStatusLabelDecodeQrBarcode.StatusBackColor != SystemColors.Highlight)
                            controlStatusLabelDecodeQrBarcode.SetStatusValue("-", SystemColors.Highlight, SystemColors.Window, SystemColors.Desktop, SystemColors.Window);

                        if (controlStatusLabelPickup.StatusBackColor != SystemColors.Highlight)
                            controlStatusLabelPickup.SetStatusValue("-", SystemColors.Highlight, SystemColors.Window, SystemColors.Desktop, SystemColors.Window);

                        if (controlStatusLabelPutReel.StatusBackColor != SystemColors.Highlight)
                            controlStatusLabelPutReel.SetStatusValue("-", SystemColors.Highlight, SystemColors.Window, SystemColors.Desktop, SystemColors.Window);

                        if (controlStatusLabelCompleteLoad.StatusBackColor != SystemColors.Highlight)
                            controlStatusLabelCompleteLoad.SetStatusValue("-", SystemColors.Highlight, SystemColors.Window, SystemColors.Desktop, SystemColors.Window);
                    }
                    break;
                case ReelHandlerSteps.CheckCartReelType:
                case ReelHandlerSteps.SetReelTypeOfCartToRobot:
                case ReelHandlerSteps.PrepareToLoadReelFromCart:
                case ReelHandlerSteps.SetWorkSlotToRobot:
                case ReelHandlerSteps.GoToHomeBeforeReelHeightCheck:
                case ReelHandlerSteps.MoveToReelHeightCheckPositionOfWorkSlot:
                case ReelHandlerSteps.MeasureReelHeightOnCart:
                case ReelHandlerSteps.PrepareToChangeWorkSlotOfCart:
                case ReelHandlerSteps.ChangeWorkSlotOfCart:
                case ReelHandlerSteps.PrepareToLoadReturnReel:
                    {
                        switch (mainSequence.CurrentReelTypeOfReturn)
                        {
                            case ReelDiameters.ReelDiameter7:
                                reelsize_ = Properties.Resources.String_FormMain_Cart_Reel_Type_7_Inch;
                                break;
                            case ReelDiameters.ReelDiameter13:
                                reelsize_ = Properties.Resources.String_FormMain_Cart_Reel_Type_13_Inch;
                                break;
                            default:
                                reelsize_ = "-";
                                break;
                        }

                        if (reelsize_ != controlStatusLabelCartInchCheck.ValueText)
                            controlStatusLabelCartInchCheck.SetStatusValue(reelsize_, Color.Lime, SystemColors.ControlText, SystemColors.Desktop, SystemColors.Window);

                        if (controlStatusLabelVisionAlign.StatusBackColor != SystemColors.Highlight)
                            controlStatusLabelVisionAlign.SetStatusValue("-", SystemColors.Highlight, SystemColors.Window, SystemColors.Desktop, SystemColors.Window);

                        if (controlStatusLabelDecodeQrBarcode.StatusBackColor != SystemColors.Highlight)
                            controlStatusLabelDecodeQrBarcode.SetStatusValue("-", SystemColors.Highlight, SystemColors.Window, SystemColors.Desktop, SystemColors.Window);

                        if (controlStatusLabelPickup.StatusBackColor != SystemColors.Highlight)
                            controlStatusLabelPickup.SetStatusValue("-", SystemColors.Highlight, SystemColors.Window, SystemColors.Desktop, SystemColors.Window);

                        if (controlStatusLabelPutReel.StatusBackColor != SystemColors.Highlight)
                            controlStatusLabelPutReel.SetStatusValue("-", SystemColors.Highlight, SystemColors.Window, SystemColors.Desktop, SystemColors.Window);

                        if (controlStatusLabelCompleteLoad.StatusBackColor != SystemColors.Highlight)
                            controlStatusLabelCompleteLoad.SetStatusValue("-", SystemColors.Highlight, SystemColors.Window, SystemColors.Desktop, SystemColors.Window);
                    }
                    break;
                case ReelHandlerSteps.CheckReelAlignmentOnCart:
                case ReelHandlerSteps.CheckReelAlignmentOnReturnStage:
                    {
                        if (controlStatusLabelVisionAlign.StatusBackColor != Color.Lime)
                            controlStatusLabelVisionAlign.SetStatusColor(Color.Lime, SystemColors.ControlText);

                        if (controlStatusLabelDecodeQrBarcode.StatusBackColor != SystemColors.Highlight)
                            controlStatusLabelDecodeQrBarcode.SetStatusColor(SystemColors.Highlight, SystemColors.Window);
                    }
                    break;
                case ReelHandlerSteps.RequestToReelLoadConfirm:
                case ReelHandlerSteps.RequestToReturnReelLoadConfirm:
                    {
                        controlStatusLabelVisionAlign.SetStatusValue("OK", Color.Lime, SystemColors.ControlText, SystemColors.Desktop, SystemColors.Window);
                    }
                    break;
                case ReelHandlerSteps.PrepareToReadBarcodeOnReel:
                case ReelHandlerSteps.ReadBarcodeOnReel:
                case ReelHandlerSteps.RequestToConfirmLoadReelBarcodeOfCart:
                case ReelHandlerSteps.RequestToConfirmLoadReelBarcodeOfReturnStage:
                    {
                        if (controlStatusLabelDecodeQrBarcode.StatusBackColor != Color.Lime)
                            controlStatusLabelDecodeQrBarcode.SetStatusColor(Color.Lime, SystemColors.ControlText);
                    }
                    break;
                case ReelHandlerSteps.AdjustPositionAndPickupReelOfCart:
                case ReelHandlerSteps.AdjustPositionAndPickupReelOfReturnStage:
                    {
                        controlStatusLabelDecodeQrBarcode.SetStatusValue(labelSid.Text, Color.Lime, SystemColors.ControlText, SystemColors.Desktop, SystemColors.Window);
                    }
                    break;
                case ReelHandlerSteps.GoToHomeAfterPickUpReel:
                case ReelHandlerSteps.MoveToFrontOfTower:
                    {
                        if (mainSequence.RobotSequenceManager.HasAReel)
                        {
                            if (controlStatusLabelPickup.StatusBackColor != Color.Lime)
                                controlStatusLabelPickup.SetStatusValue(Properties.Resources.String_Reel_Detected, Color.Lime, SystemColors.ControlText, SystemColors.Desktop, SystemColors.Window);
                        }
                        else
                        {
                            controlStatusLabelPickup.SetStatusValue("-", SystemColors.Highlight, SystemColors.Window, SystemColors.Desktop, SystemColors.Window);
                        }
                    }
                    break;
                case ReelHandlerSteps.PutReelIntoTower:
                    {
                        if (controlStatusLabelPutReel.StatusBackColor != Color.Lime)
                            controlStatusLabelPutReel.SetStatusValue(labelReelTransferDestination.Text, Color.Lime, SystemColors.ControlText, SystemColors.Desktop, SystemColors.Window);

                        if (Singleton<TransferMaterialObject>.Instance.State == TransferMaterialObject.TransferStates.RequestToLoadAssignment ||
                            Singleton<TransferMaterialObject>.Instance.State == TransferMaterialObject.TransferStates.WaitForLoadAssignment ||
                            Singleton<TransferMaterialObject>.Instance.State == TransferMaterialObject.TransferStates.CompleteLoad)
                            controlStatusLabelCompleteLoad.SetStatusValue(Properties.Resources.String_Process_Done, Color.Lime, SystemColors.ControlText, SystemColors.Desktop, SystemColors.Window);
                    }
                    break;
                case ReelHandlerSteps.RequestToConfirmLoadedReelAssign:
                case ReelHandlerSteps.CompletedToLoadReel:
                    {
                        if (controlStatusLabelCompleteLoad.StatusBackColor != Color.Lime)
                            controlStatusLabelCompleteLoad.SetStatusValue(Properties.Resources.String_Process_Done, Color.Lime, SystemColors.ControlText, SystemColors.Desktop, SystemColors.Window);
                    }
                    break;
                case ReelHandlerSteps.MoveBackToHomeByVisionAlignmentFailureToLoad:
                case ReelHandlerSteps.MoveBackToHomeByReelTowerResponseTimeoutToLoad:
                case ReelHandlerSteps.MoveBackToHomeByReelTowerRefuseToLoad:
                case ReelHandlerSteps.MoveBackToHomeByCancelBarcodeInputToLoad:
                case ReelHandlerSteps.MoveBackToHomeByReelPickupFailureToLoad:
                case ReelHandlerSteps.WaitForBarcodeInput:
                case ReelHandlerSteps.MoveBackToFrontOfReturnStage:
                case ReelHandlerSteps.MoveBackToFrontOfReturnStageByVisionAlignmentFailureToLoadReturn:
                case ReelHandlerSteps.MoveBackToFrontOfReturnStageByResponseTimeoutToLoadReturn:
                case ReelHandlerSteps.MoveBackToFrontOfReturnStageByReelTowerRefuseToLoadReturn:
                case ReelHandlerSteps.MoveBackToFrontOfReturnStageByCancelBarcodeInputToLoadReturn:
                case ReelHandlerSteps.MoveBackToFrontOfReturnStageByReelPickupFailureToLoadReturn:
                    break;
                case ReelHandlerSteps.PrepareToUnloadTowerReel:
                    {
                        if (controlStatusLabelPickList.StatusBackColor != SystemColors.Highlight)
                            controlStatusLabelPickList.SetStatusValue("-", SystemColors.Highlight, SystemColors.Window, SystemColors.Desktop, SystemColors.Window);

                        if (controlStatusLabelMoveToTower.StatusBackColor != SystemColors.Highlight)
                            controlStatusLabelMoveToTower.SetStatusValue("-", SystemColors.Highlight, SystemColors.Window, SystemColors.Desktop, SystemColors.Window);

                        if (controlStatusLabelTakeReelFromTower.StatusBackColor != SystemColors.Highlight)
                            controlStatusLabelTakeReelFromTower.SetStatusValue("-", SystemColors.Highlight, SystemColors.Window, SystemColors.Desktop, SystemColors.Window);

                        if (controlStatusLabelPutUnloadReelToOutput.StatusBackColor != SystemColors.Highlight)
                            controlStatusLabelPutUnloadReelToOutput.SetStatusValue("-", SystemColors.Highlight, SystemColors.Window, SystemColors.Desktop, SystemColors.Window);

                        if (controlStatusLabelCompleteUnload.StatusBackColor != SystemColors.Highlight)
                            controlStatusLabelCompleteUnload.SetStatusValue("-", SystemColors.Highlight, SystemColors.Window, SystemColors.Desktop, SystemColors.Window);
                    }
                    break;
                case ReelHandlerSteps.MoveToFrontOfUnloadTower:
                    {
                        if (labelPickingIdValue.Text != controlStatusLabelPickList.ValueText)
                            controlStatusLabelPickList.SetStatusValue(labelPickingIdValue.Text, Color.Lime, SystemColors.ControlText, SystemColors.Desktop, SystemColors.Window);

                        if (controlStatusLabelMoveToTower.StatusBackColor != Color.Lime)
                            controlStatusLabelMoveToTower.SetStatusValue(labelReelTransferSource.Text, Color.Lime, SystemColors.ControlText, SystemColors.Desktop, SystemColors.Window);
                    }
                    break;
                case ReelHandlerSteps.TakeReelFromUnloadTower:
                case ReelHandlerSteps.RequestToUnloadReelAssignment:
                    {
                        if (controlStatusLabelTakeReelFromTower.StatusBackColor != Color.Lime)
                            controlStatusLabelTakeReelFromTower.SetStatusValue(labelUid.Text, Color.Lime, SystemColors.ControlText, SystemColors.Desktop, SystemColors.Window);
                    }
                    break;
                case ReelHandlerSteps.MoveToFrontOfOutputStage:
                case ReelHandlerSteps.CheckUpStateOfOutputStage:
                case ReelHandlerSteps.ApproachOutputStage:
                case ReelHandlerSteps.PutReelIntoOutputStage:
                    {
                        if (controlStatusLabelPutUnloadReelToOutput.StatusBackColor != Color.Lime)
                            controlStatusLabelPutUnloadReelToOutput.SetStatusValue(labelReelTransferDestination.Text, Color.Lime, SystemColors.ControlText, SystemColors.Desktop, SystemColors.Window);
                    }
                    break;
                case ReelHandlerSteps.RequestToConfirmUnloadedReelAssign:
                case ReelHandlerSteps.CompletedToUnloadReelFromTower:
                    {
                        if (controlStatusLabelCompleteUnload.StatusBackColor != Color.Lime)
                            controlStatusLabelCompleteUnload.SetStatusValue(Properties.Resources.String_Process_Done, Color.Lime, SystemColors.ControlText, SystemColors.Desktop, SystemColors.Window);
                    }
                    break;
                case ReelHandlerSteps.MoveBackToFrontOfTowerByReelPickupFailureToUnload:
                case ReelHandlerSteps.PrepareToRejectReel:
                case ReelHandlerSteps.CheckUpStateOfRejectStage:
                case ReelHandlerSteps.MoveToFrontOfRejectStage:
                case ReelHandlerSteps.ApproachRejectStage:
                case ReelHandlerSteps.PutReelIntoRejectStage:
                case ReelHandlerSteps.CompletedToRejectReel:
                case ReelHandlerSteps.MoveBackToFrontOfTowerByReelPickupFailureToReject:
                case ReelHandlerSteps.Done:
                    break;
            }
        }

        protected void UpdateCartInformation()
        {
            labelMobileRobotState.ForeColor = SystemColors.ControlText;

            switch (mainSequence.CartMode)
            {
                case MobileRobotOperationModes.Idle:
                    {
                        if (labelMobileRobotState.Text != Properties.Resources.String_Process_Idle)
                        {
                            labelMobileRobotState.BackColor = SystemColors.Window;
                            labelMobileRobotState.Text = Properties.Resources.String_Process_Idle;
                        }
                    }
                    break;
                case MobileRobotOperationModes.Ready:
                    {
                        if (labelMobileRobotState.Text != Properties.Resources.String_Process_Ready)
                        {
                            labelMobileRobotState.BackColor = Color.Lime;
                            labelMobileRobotState.Text = Properties.Resources.String_Process_Ready;
                        }
                    }
                    break;
                case MobileRobotOperationModes.Load:
                    {
                        if (labelMobileRobotState.Text != Properties.Resources.String_Process_Load)
                        {
                            labelMobileRobotState.BackColor = Color.Lime;
                            labelMobileRobotState.Text = Properties.Resources.String_Process_Load;
                        }
                    }
                    break;
                case MobileRobotOperationModes.Unload:
                    {
                        if (labelMobileRobotState.Text != Properties.Resources.String_Process_Unload)
                        {
                            labelMobileRobotState.BackColor = Color.Lime;
                            labelMobileRobotState.Text = Properties.Resources.String_Process_Unload;
                        }
                    }
                    break;
            }

            if (!UseMobileRobot)
            {
                if ((App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                {
                    if ((App.DigitalIoManager as DigitalIoManager).IsCartHooked)
                        buttonDock.Text = Properties.Resources.String_Process_Undock;
                    else
                        buttonDock.Text = Properties.Resources.String_Process_Dock;
                }
                else
                {
                    if (!(App.DigitalIoManager as DigitalIoManager).IsCartHooked)
                        buttonDock.Text = Properties.Resources.String_Process_Dock;
                    else
                        buttonDock.Text = Properties.Resources.String_Process_Undock;
                }
            }

            if (mainSequence.IsFinishedLoadReelFromCart)
            {
                if (buttonMobileRobotControllerUsage.BackColor != Color.Lime)
                {
                    buttonMobileRobotControllerUsage.BackColor = Color.Lime;
                    buttonMobileRobotControllerUsage.ForeColor = SystemColors.WindowText;
                }
            }
            else
            {
                if (buttonMobileRobotControllerUsage.BackColor != SystemColors.ControlDarkDark)
                {
                    buttonMobileRobotControllerUsage.BackColor = SystemColors.ControlDarkDark;
                    buttonMobileRobotControllerUsage.ForeColor = SystemColors.Window;
                }
            }
        }

        protected void UpdateVisionControlState(Control displayControl, VisionControlStates state)
        {
            UpdateVisionControlsEnabled();
        }

        protected void UpdateProductInformation()
        {
            labelUid.Text = mainSequence.ReelBarcodeContexts.Name;
            labelSid.Text = mainSequence.ReelBarcodeContexts.Category;
            labelLotId.Text = mainSequence.ReelBarcodeContexts.LotId;
            labelSupplier.Text = mainSequence.ReelBarcodeContexts.Supplier;
            labelQty.Text = mainSequence.ReelBarcodeContexts.Quantity.ToString();
            labelMfg.Text = mainSequence.ReelBarcodeContexts.ManufacturedDatetime;
        }

        protected void UpdateMobileRobotSequenceStates()
        {
            if ((App.DigitalIoManager as DigitalIoManager).IsCartHooked)
            {
                if (controlStatusLabelCartInOut.StatusBackColor != Color.Lime)
                    controlStatusLabelCartInOut.SetStatusColor(Color.Lime, SystemColors.ControlText);
            }
            else
            {
                if (controlStatusLabelCartInOut.StatusBackColor != SystemColors.Highlight)
                    controlStatusLabelCartInOut.SetStatusColor(SystemColors.Highlight, SystemColors.Window);
            }

            switch (mainSequence.MobileRobotManager.CartDockingSequence)
            {
                case CartDockingSequences.Unknown:
                case CartDockingSequences.Ready:
                case CartDockingSequences.RequestLoadCart:
                case CartDockingSequences.ReceivedLoadCartResponse:
                case CartDockingSequences.RequestUnloadCart:
                case CartDockingSequences.ReceivedUnloadCartResponse:
                    break;
                case CartDockingSequences.StartLoadCart:
                case CartDockingSequences.ArriveAtWorkZoneToLoad:
                    {
                        if (controlStatusLabelMrbtMove.StatusBackColor != Color.Lime)
                            controlStatusLabelMrbtMove.SetStatusColor(Color.Lime, SystemColors.ControlText);

                        if (controlStatusLabelMrbtDocking.StatusBackColor != SystemColors.Highlight)
                            controlStatusLabelMrbtDocking.SetStatusColor(SystemColors.Highlight, SystemColors.Window);
                    }
                    break;
                case CartDockingSequences.MoveToLoadWorkZone:
                    {
                        if (controlStatusLabelMrbtMove.StatusBackColor != Color.Lime)
                            controlStatusLabelMrbtMove.SetStatusColor(Color.Lime, SystemColors.ControlText);

                        if (controlStatusLabelMrbtDocking.StatusBackColor != Color.Lime)
                            controlStatusLabelMrbtDocking.SetStatusValue(Properties.Resources.String_Process_State_Loading, Color.Lime, SystemColors.ControlText, SystemColors.Desktop, SystemColors.Window);
                    }
                    break;
                case CartDockingSequences.CompleteLoadCart:
                case CartDockingSequences.CameOutWorkZone:
                case CartDockingSequences.CompleteUnloadCart:
                    {
                        if (controlStatusLabelMrbtMove.StatusBackColor != SystemColors.Highlight)
                            controlStatusLabelMrbtMove.SetStatusColor(SystemColors.Highlight, SystemColors.Window);

                        if (controlStatusLabelMrbtDocking.StatusBackColor != SystemColors.Highlight)
                            controlStatusLabelMrbtDocking.SetStatusValue("-", SystemColors.Highlight, SystemColors.Window, SystemColors.Desktop, SystemColors.Window);

                        if (controlStatusLabelCartInchCheck.StatusBackColor != SystemColors.Highlight)
                            controlStatusLabelCartInchCheck.SetStatusValue("-", SystemColors.Highlight, SystemColors.Window, SystemColors.Desktop, SystemColors.Window);
                    }
                    break;
                case CartDockingSequences.StartUnloadCart:
                case CartDockingSequences.ArriveAtWorkZoneToUnload:
                case CartDockingSequences.MoveToUnloadWorkZone:
                    {
                        if (controlStatusLabelMrbtMove.StatusBackColor != Color.Lime)
                            controlStatusLabelMrbtMove.SetStatusColor(Color.Lime, SystemColors.ControlText);

                        if (controlStatusLabelMrbtDocking.StatusBackColor != Color.Lime)
                            controlStatusLabelMrbtDocking.SetStatusValue(Properties.Resources.String_Process_State_Unloading, Color.Lime, SystemColors.ControlText, SystemColors.Desktop, SystemColors.Window);
                    }
                    break;
            }
        }

        protected void UpdatePerformanceInformation()
        {
            if (ReelTowerManager.IsConnected)
            {
                lblTowerMode1.Text = mainSequence.CurrentTowerStates[0].Mode.ToString();
                lblTowerState1.Text = mainSequence.CurrentTowerStates[0].State.ToString();
                lblTowerMode2.Text = mainSequence.CurrentTowerStates[1].Mode.ToString();
                lblTowerState2.Text = mainSequence.CurrentTowerStates[1].State.ToString();
                lblTowerMode3.Text = mainSequence.CurrentTowerStates[2].Mode.ToString();
                lblTowerState3.Text = mainSequence.CurrentTowerStates[2].State.ToString();
                lblTowerMode4.Text = mainSequence.CurrentTowerStates[3].Mode.ToString();
                lblTowerState4.Text = mainSequence.CurrentTowerStates[3].State.ToString();
            }

            string elapsed = TimeSpan.FromMilliseconds((DateTime.Now - startedDateTime).TotalMilliseconds).ToString();
            int inx = elapsed.LastIndexOf(".");
            elapsed = elapsed.Remove(inx + 2, elapsed.Length - inx - 2);
            labelRobotStepValue.Text = mainSequence.RobotStep.ToString();
            labelMobileRobotStepValue.Text = mainSequence.MobileRobotStep.ToString();
            labelReelTowerStepValue.Text = mainSequence.ReelTowerStep.ToString();
            labelElapsedValue.Text = elapsed;
            labelTotalLoadReelCountValue.Text = mainSequence.TotalLoadReels.ToString();
            labelTotalReturnReelCountValue.Text = mainSequence.TotalReturnReels.ToString();
            labelTotalUnloadReelCountValue.Text = mainSequence.TotalUnloadReels.ToString();
            labelLoadErrorCountValue.Text = mainSequence.LoadErrors.ToString();
            labelReturnErrorCountValue.Text = mainSequence.ReturnErrors.ToString();
            labelUnloadErrorCountValue.Text = mainSequence.UnloadErrors.ToString();
            labelVisionAlignErrorValue.Text = mainSequence.VisionAlignErrors.ToString();
            labelVisionDecodeErrorValue.Text = mainSequence.VisionDecodeErrors.ToString();

            labelTransferModeRSValue.Text = Singleton<TransferMaterialObject>.Instance.Mode.ToString();
            labelTransferStateRSValue.Text = Singleton<TransferMaterialObject>.Instance.State.ToString();
            labelLastCommandRSValue.Text = RobotSequenceManager.LastExecutedCommand.ToString();
            labelCurrentWaypointRSValue.Text = RobotSequenceManager.CurrentWaypoint.ToString();
            labelNextWaypointRSValue.Text = RobotSequenceManager.NextWaypoint.ToString();
            labelLastCycleTime.Text = string.Concat(mainSequence.LastCycleTime.ToString(@"s\.fff"), " sec");
        }
        #endregion

        #region Sequence control methods
        protected void EnableControls(bool enable)
        {
            buttonReset.Enabled = enable;
        }
        #endregion

        #region Operation button handler
        protected void OnClickButtonExit(object sender, EventArgs e)
        {
            if (App.OperationState == OperationStates.Run)
            {
                FormMessageExt.ShowInformation(Properties.Resources.String_Information_Stop_Process_First, Buttons.Ok, true);
                ReelTowerManager.SendTowerMessage(ReelTowerCommands.REQUEST_ALL_LOAD_RESET, null);
                return;
            }

            if (FormMessageExt.ShowQuestion(Properties.Resources.String_FormMain_Question_Exit_Program) == DialogResult.No)
                return;

            Logger.TraceKeyAndMouseEvent(sender as Control, e);
            shutdown = true;

            mainSequence.StopRobotController();
            FlushProductionDataLog();
            Close();
        }

        protected void OnClickButtonStop(object sender, EventArgs e)
        {
            Logger.TraceKeyAndMouseEvent(sender as Control, e);

            if (App.OperationState == OperationStates.Run && mainSequence != null)
            {
                if (mainSequence.IsPossibleImmediateStop)
                    App.Stop();
                else
                    App.TryCycleStop(!mainSequence.CycleStop);
            }
        }

        protected void OnClickButtonReset(object sender, EventArgs e)
        {
            Logger.TraceKeyAndMouseEvent(sender as Control, e);

            if (Config.ImageProcessorType == ImageProcessorTypes.Mit)
                VisionManager._CILightOff();

            mainSequence.Reset();
        }

        protected void OnClickButtonInitialize(object sender, EventArgs e)
        {
            Logger.TraceKeyAndMouseEvent(sender as Control, e);
            // Turn off vision light first.
            (App.MainForm as FormMain).VisionLightManager.SendCommand(VisionLightManager.ControllerCommands.Off, 1, 0);// "F1000#");
            mainSequence.Initialize();
        }

        protected void OnClickButtonConnectRobotController(object sender, EventArgs e)
        {
            mainSequence.TryRobotControllerConnect(textBoxRobotControllerAddress.Text, int.Parse(textBoxRobotControllerPort.Text));
        }

        protected void OnClickButtonStart(object sender, EventArgs e)
        {
            bool visionReady = true;
            Logger.TraceKeyAndMouseEvent(sender as Control, e);

            if (!visionState)
                visionReady = StartVision();

            if (!App.CycleStop && visionReady)
                App.Start();
            else
                ShowModallessNotificationWindow(Properties.Resources.String_Notification_Check_Vision_State_Stop_Live);
        }

        protected void InitializeVisionPage(object sender, EventArgs e)
        {
            bool result_ = false;

            if (!(visionWnd = FindWindow(null, "Cam1")).Equals(IntPtr.Zero))
            {   // Just check vision process.
                SetParent(visionWnd, tabPageVision.Handle);
                result_ = true;
            }

            if (!result_)
                FormMessageExt.ShowNotification(Properties.Resources.String_FormMain_Notification_Check_Vision_Run);
        }

        protected void OnClickButtonDock(object sender, EventArgs e)
        {
            if (buttonDock.Text == Properties.Resources.String_Process_Dock)
            {
                mainSequence.ForceSetCartState();
            }
            else
            {
                if (OperationState == OperationStates.Run)
                {
                    FormMessageExt.ShowInformation(Properties.Resources.String_FormMain_Notification_Stop_To_Undock_Confirm);
                    return;
                }

                if (FormMessageExt.ShowQuestion(Properties.Resources.String_FormMain_Notification_Undock_Confirm) == DialogResult.Yes)
                {
                    mainSequence.ForceSetCartState(false);
                }
            }
        }
        #endregion

        #region Cylinder operation
        protected void OnClickButtonCylinder1(object sender, EventArgs e)
        {
            if (DigitalIo.CartAlignSensorLeftForward)
            {
                DigitalIo.CartAlignCylinderLeft = false;
                DigitalIo.CartAlignCylinderRight = false;
            }
            else
            {
                DigitalIo.CartAlignCylinderLeft = true;
                DigitalIo.CartAlignCylinderRight = true;
            }
        }

        protected void OnClickButtonCylinder2(object sender, EventArgs e)
        {
            if (DigitalIo.CartAlignSensorRightForward)
            {
                DigitalIo.CartAlignCylinderLeft = false;
                DigitalIo.CartAlignCylinderRight = false;
            }
            else
            {
                DigitalIo.CartAlignCylinderLeft = true;
                DigitalIo.CartAlignCylinderRight = true;
            }
        }
        #endregion

        #region Simulation
        protected void OnClickButtonMobileRobotManagerLoad(object sender, EventArgs e)
        {
            mainSequence.SendMobileRobotCommand(MobileRobotManagerCommands.WorkZoneLoad);
        }

        protected void OnClickButtonMobileRobotManagerUnload(object sender, EventArgs e)
        {
            mainSequence.SendMobileRobotCommand(MobileRobotManagerCommands.WorkZoneUnload);
        }

        protected void OnClickButtonMobileRobotManagerLoadBufferCart(object sender, EventArgs e)
        {
            // mainSequence.SendMobileRobotMessage("TRANSFERREQ;BUFFZONELOAD;\r\n");
        }

        protected void OnClickLabelSetting(object sender, EventArgs e)
        {
            groupBoxNetworkSetting.Visible = !groupBoxNetworkSetting.Visible;
            groupBoxTimeoutProperties.Visible = !groupBoxTimeoutProperties.Visible;
        }
        #endregion

        #region Log and state display
        protected void OnChangedCartState(object sender, EventArgs e)
        {
            if (labelOperationState.InvokeRequired)
                labelOperationState.BeginInvoke(new Action(() => { UpdateCartState(); }));
            else
                UpdateCartState();
        }

        protected void UpdateOperationState()
        {
            bool visionControlState = true;

            buttonReset.Enabled = (App.OperationState == OperationStates.Alarm || App.OperationState == OperationStates.Stop);
            buttonStop.Enabled = (App.OperationState == OperationStates.Run);
            buttonStart.Enabled = (!buttonStop.Enabled & App.Initialized);
            buttonInitialize.Enabled = buttonStart.Enabled || App.OperationState == OperationStates.PowerOn || !App.Initialized;
            DigitalIo.SetSignalTower(App.OperationState, App.OperationState == OperationStates.Alarm);

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
                        visionControlState = false;
                    }
                    break;
                case OperationStates.Stop:
                    {
                        labelOperationState.BackColor = Color.Red;
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
                groupBoxGuiSettings.Enabled = groupBoxTimeoutProperties.Enabled = groupBoxNetworkSetting.Enabled = groupBoxModel.Enabled =
                buttonSaveModel.Enabled = buttonSaveTimeoutProperties.Enabled = buttonSaveNetworkSettings.Enabled = true;
            }
            else
            {
                groupBoxGuiSettings.Enabled = groupBoxTimeoutProperties.Enabled = groupBoxNetworkSetting.Enabled = groupBoxModel.Enabled =
                buttonSaveModel.Enabled = buttonSaveTimeoutProperties.Enabled = buttonSaveNetworkSettings.Enabled = false;
            }

            if (visionControl1 != null)
                visionControl1.BlockControl = !visionControlState;
        }

        protected void UpdateCartState()
        {
            switch (mainSequence.OperationState)
            {
                case OperationStates.Run:
                    {
                    }
                    break;
            }
        }


        public void UpdateReelTye(int reeltype)
        {
        }

        public void OnChangedReelTowerCommunicationState(object sender, CommunicationStates state)
        {
            if (state == currentReelTowerCommState)
                return;

            if (labelReelTowerCommunicationStateValue.InvokeRequired)
                labelReelTowerCommunicationStateValue.BeginInvoke(new Action(() => { UpdateCommunicationState(labelReelTowerCommunicationStateValue, state); }));
            else
                UpdateCommunicationState(labelReelTowerCommunicationStateValue, state);
        }

        public void UpdateRobotSequenceCommunicationState(object sender, CommunicationStates state)
        {
            if (state == currentRobotSequenceCommState)
                return;

            mainSequence.CheckRobotControllerProgram();

            if (labelRobotCommunicationStateValue.InvokeRequired)
            {
                labelRobotCommunicationStateValue.BeginInvoke(new Action(() =>
                {
                    UpdateCommunicationState(labelRobotCommunicationStateValue, state);
                }));
            }
            else
            {
                UpdateCommunicationState(labelRobotCommunicationStateValue, state);
            }
        }

        public void OnChangedRobotControllerCommunicationState(object sender, CommunicationStates state)
        {
            if (state == currentRobotControllerCommState)
                return;

            if (lblURClientConnectState.InvokeRequired)
            {
                lblURClientConnectState.BeginInvoke(new Action(() =>
                {
                    UpdateCommunicationState(lblURClientConnectState, state);
                }));
            }
            else
                UpdateCommunicationState(lblURClientConnectState, state);
        }

        public void OnChangedMobileRobotCommunicationState(object sender, CommunicationStates state)
        {
            if (state == currentMobileRobotCommState)
                return;

            if (labelMobileRobotCommunicationStateValue.InvokeRequired)
                labelMobileRobotCommunicationStateValue.BeginInvoke(new Action(() => { UpdateCommunicationState(labelMobileRobotCommunicationStateValue, state); }));
            else
                UpdateCommunicationState(labelMobileRobotCommunicationStateValue, state);
        }

        public void OnChangedVisionControlState(object sender, VisionControlStates state)
        {
            if (state == currentVisionControlState)
                return;

            if (buttonVisionStart.InvokeRequired)
                buttonVisionStart.BeginInvoke(new Action(() => { UpdateVisionControlState(labelMobileRobotCommunicationStateValue, state); }));
            else
                UpdateVisionControlState(buttonVisionStart, state);

            currentVisionControlState = state;
        }

        public void UpdateReelTowerRuntimeLog(object sender, string text)
        {
            if (listBoxReelTowerComm.InvokeRequired)
                listBoxReelTowerComm.BeginInvoke(new Action(() => { UpdateReelTowerRuntimeLog(text); }));
            else
                UpdateReelTowerRuntimeLog(text);
        }

        public void UpdateProductionDataLog()
        {
            try
            {
                if (StatsLogger.IsChangedDate())
                {   // Flush previous date records first. 
                    StatsLogger.FlushData();
                    // Reset records and write reset records
                    mainSequence.ResetProductionCount();
                    ProductionRecord._TotalLoadCountInt = mainSequence.TotalLoadReels;
                    ProductionRecord._TotalReturnCountInt = mainSequence.TotalReturnReels;
                    ProductionRecord._TotalUnloadCountInt = mainSequence.TotalUnloadReels;
                    ProductionRecord._TotalLoadErrorCountInt = mainSequence.LoadErrors;
                    ProductionRecord._TotalReturnErrorCountInt = mainSequence.ReturnErrors;
                    ProductionRecord._TotalUnloadErrorCountInt = mainSequence.UnloadErrors;
                    ProductionRecord._VisionAlignmentErrorCountInt = mainSequence.VisionAlignErrors;
                    ProductionRecord._VisionDecodeErrorCountInt = mainSequence.VisionDecodeErrors;
                    StatsLogger.FlushData(true);
                }
                else
                {
                    ProductionRecord._TotalLoadCountInt = mainSequence.TotalLoadReels;
                    ProductionRecord._TotalReturnCountInt = mainSequence.TotalReturnReels;
                    ProductionRecord._TotalUnloadCountInt = mainSequence.TotalUnloadReels;
                    ProductionRecord._TotalLoadErrorCountInt = mainSequence.LoadErrors;
                    ProductionRecord._TotalReturnErrorCountInt = mainSequence.ReturnErrors;
                    ProductionRecord._TotalUnloadErrorCountInt = mainSequence.UnloadErrors;
                    ProductionRecord._VisionAlignmentErrorCountInt = mainSequence.VisionAlignErrors;
                    ProductionRecord._VisionDecodeErrorCountInt = mainSequence.VisionDecodeErrors;
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"{GetType()}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public void FlushProductionDataLog()
        {
            try
            {
                if (StatsLogger.IsChangedDate())
                {   // Flush previous date records first. 
                    StatsLogger.FlushData();
                    // Reset records and write reset records
                    mainSequence.ResetProductionCount();
                    ProductionRecord._TotalLoadCountInt = mainSequence.TotalLoadReels;
                    ProductionRecord._TotalReturnCountInt = mainSequence.TotalReturnReels;
                    ProductionRecord._TotalUnloadCountInt = mainSequence.TotalUnloadReels;
                    ProductionRecord._TotalLoadErrorCountInt = mainSequence.LoadErrors;
                    ProductionRecord._TotalReturnErrorCountInt = mainSequence.ReturnErrors;
                    ProductionRecord._TotalUnloadErrorCountInt = mainSequence.UnloadErrors;
                    ProductionRecord._VisionAlignmentErrorCountInt = mainSequence.VisionAlignErrors;
                    ProductionRecord._VisionDecodeErrorCountInt = mainSequence.VisionDecodeErrors;
                    StatsLogger.FlushData(true);
                }
                else
                {
                    ProductionRecord._TotalLoadCountInt = mainSequence.TotalLoadReels;
                    ProductionRecord._TotalReturnCountInt = mainSequence.TotalReturnReels;
                    ProductionRecord._TotalUnloadCountInt = mainSequence.TotalUnloadReels;
                    ProductionRecord._TotalLoadErrorCountInt = mainSequence.LoadErrors;
                    ProductionRecord._TotalReturnErrorCountInt = mainSequence.ReturnErrors;
                    ProductionRecord._TotalUnloadErrorCountInt = mainSequence.UnloadErrors;
                    ProductionRecord._VisionAlignmentErrorCountInt = mainSequence.VisionAlignErrors;
                    ProductionRecord._VisionDecodeErrorCountInt = mainSequence.VisionDecodeErrors;
                    StatsLogger.FlushData();
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"{GetType()}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public void OnReportRobotRuntimeLog(object sender, string text)
        {

            if (listBoxRobotComm.InvokeRequired)
                listBoxRobotComm.BeginInvoke(new Action(() => { UpdateRobotRuntimeLog(text); }));
            else
                UpdateRobotRuntimeLog(text);
        }

        public void UpdateReelTowerRuntimeLog(string text)
        {
            try
            {
                Logger.Trace(text, "Tower");

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

        public void UpdateRobotRuntimeLog(string text)
        {
            try
            {
                if (!text.Contains("\n"))
                    text += "\n";

                Logger.Trace(text, "Robot");

                lock (listBoxRobotComm)
                {
                    if (listBoxRobotComm.Items.Count > 1000)
                        listBoxRobotComm.Items.RemoveAt(0);

                    listBoxRobotComm.Items.Add($"{DateTime.Now.ToString("HH:mm:ss.fff")}> {text}");
                    listBoxRobotComm.SelectedIndex = listBoxRobotComm.Items.Count > 0 ? listBoxRobotComm.Items.Count - 1 : 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                MessageBox.Show($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public void OnReportAlarmLog(object sender, string text)
        {

            if (listBoxAlarmHistory.InvokeRequired)
                listBoxAlarmHistory.BeginInvoke(new Action(() => { UpdateAlarmLog(text); }));
            else
                UpdateAlarmLog(text);
        }

        public void UpdateAlarmLog(string text)
        {
            try
            {
                if (!text.Contains("\n"))
                    text += "\n";

                Logger.Trace(text, "Alarm");

                lock (listBoxAlarmHistory)
                {
                    if (listBoxAlarmHistory.Items.Count > 1000)
                        listBoxAlarmHistory.Items.RemoveAt(0);

                    listBoxAlarmHistory.Items.Add($"{DateTime.Now.ToString("HH:mm:ss.fff")}> {text}");
                    listBoxAlarmHistory.SelectedIndex = listBoxAlarmHistory.Items.Count > 0 ? listBoxAlarmHistory.Items.Count - 1 : 0;
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
        #endregion

        #region Event handlers
        protected void UpdateFormViewButtonState()
        {
        }

        protected void UpdateControlsByState()
        {
        }

        protected void UpdateVisionState()
        {
            switch (Config.ImageProcessorType)
            {
                case ImageProcessorTypes.Mit:
                    {
                        if (visionState = !visionState)
                        {
                            buttonVisionStart.Text = Properties.Resources.String_FormMain_buttonVisionStart_Stopped;
                            buttonVisionStart.BackColor = SystemColors.ButtonFace;
                        }
                        else
                        {
                            buttonVisionStart.Text = Properties.Resources.String_FormMain_buttonVisionStart_Run;
                            buttonVisionStart.BackColor = Color.Lime;
                        }
                    }
                    break;
                case ImageProcessorTypes.TechFloor:
                    {
                        buttonVisionStart.Text = Properties.Resources.String_FormMain_buttonVisionStart_TF_Run;
                        buttonVisionStart.BackColor = Color.Lime;
                    }
                    break;
            }
        }

        protected void OnChangedVisionState(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => {
                    UpdateVisionState();
                }));
            }
            else
            {
                UpdateVisionState();
            }
        }

        protected void ClearMaterialPackage()
        {
            listBoxOutputReelList.Items.Clear();
            labelPickingIdValue.Text = string.Empty;
            labelOutputLocationValue.Text = string.Empty;
            labelOutputReelCountValue.Text = string.Empty;
            labelOutputReelDateTimeValue.Text = string.Empty;
        }

        protected void UpdateMaterialPackage(MaterialPackage pkg)
        {
            mainSequence.SetPickList(pkg);
            listBoxOutputReelList.Items.Clear();
            labelPickingIdValue.Text = pkg.Name;
            labelOutputLocationValue.Text = pkg.OutputPortName;
            labelOutputReelCountValue.Text = pkg.MaterialCount.ToString();
            labelOutputReelDateTimeValue.Text = DateTime.Now.ToString(Properties.Resources.String_Generic_DateTime_Format);

            foreach (Pair<string, ReelUnloadReportStates> obj in pkg.Materials)
            {
                listBoxOutputReelList.Items.Add($"{obj.first}");
            }
        }

        protected void OnReceivedMaterialPackage(object sender, MaterialPackage pkg)
        {
            OnAddedMaterialPackage(sender, pkg);
            Singleton<MaterialPackageManager>.Instance.AddMaterialPackage(pkg, false, false);
        }

        protected void OnAddedMaterialPackage(object sender, MaterialPackage pkg)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => {
                    UpdateMaterialPackage(pkg);
                }));
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

        protected void OnProductionInformation(object sender, EventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => { UpdateProductInformation(); }));
            else
                UpdateProductInformation();
        }

        protected void OnShowBarcodeInputWindow(object sender, MaterialData data)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => {
                    if (barcodeInputWindow == null)
                    {
                        barcodeInputWindow = new FormBarcodeInput(data);
                        barcodeInputWindow.FormClosed += OnClosedBarcodeInputWindow;
                        barcodeInputWindow.Show();
                    }
                }));
            }
            else
            {
                if (barcodeInputWindow == null)
                {
                    barcodeInputWindow = new FormBarcodeInput(data);
                    barcodeInputWindow.FormClosed += OnClosedBarcodeInputWindow;
                    barcodeInputWindow.Show();
                }
            }
        }

        protected void OnShowDockCartWindow(object sender, bool dock)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    if (barcodeInputWindow == null)
                    {
                        notificationDockWindow = new FormMessageExt(Properties.Resources.String_FormMain_Notification_Dock_Cart);
                        notificationDockWindow.FormClosed += OnClosedDockCartWindow;
                        notificationDockWindow.Show();
                    }
                }));
            }
            else
            {
                if (barcodeInputWindow == null)
                {
                    notificationDockWindow = new FormMessageExt(Properties.Resources.String_FormMain_Notification_Undock_Cart);
                    notificationDockWindow.FormClosed += OnClosedUndockCartWindow;
                    notificationDockWindow.Show();
                }
            }
        }

        protected void OnClosedBarcodeInputWindow(object sender, FormClosedEventArgs e)
        {
            if (barcodeInputWindow != null && mainSequence != null)
            {
                mainSequence.SetBarcodeInputData(barcodeInputWindow.BarcodeData);
                barcodeInputWindow = null;
            }

            (App.MainForm as FormMain).SetFocus();
        }

        protected void OnClosedDockCartWindow(object sender, FormClosedEventArgs e)
        {
            if (notificationDockWindow != null && mainSequence != null)
            {
                mainSequence.SetDockState();
                notificationDockWindow = null;
            }
        }

        protected void OnClosedUndockCartWindow(object sender, FormClosedEventArgs e)
        {
            if (notificationDockWindow != null && mainSequence != null)
            {
                mainSequence.SetUndockState();
                notificationDockWindow = null;
            }
        }

        protected void OnChangedTransferMaterialInformation(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => {
                    labelReelTransferMode.Text = Singleton<TransferMaterialObject>.Instance.Mode.ToString();
                    labelReelTransferStateValue.Text = Singleton<TransferMaterialObject>.Instance.State.ToString();
                    labelReelTransferSource.Text = Singleton<TransferMaterialObject>.Instance.TransferSource.ToString();
                    labelReelTransferDestination.Text = Singleton<TransferMaterialObject>.Instance.TransferDestination.ToString();

                    if (Singleton<TransferMaterialObject>.Instance.Data != null && !string.IsNullOrEmpty(Singleton<TransferMaterialObject>.Instance.Data.Name))
                        labelUid.Text = Singleton<TransferMaterialObject>.Instance.Data.Name;
                }));
            }
            else
            {
                labelReelTransferMode.Text = Singleton<TransferMaterialObject>.Instance.Mode.ToString();
                labelReelTransferStateValue.Text = Singleton<TransferMaterialObject>.Instance.State.ToString();
                labelReelTransferSource.Text = Singleton<TransferMaterialObject>.Instance.TransferSource.ToString();
                labelReelTransferDestination.Text = Singleton<TransferMaterialObject>.Instance.TransferDestination.ToString();

                if (Singleton<TransferMaterialObject>.Instance.Data != null && !string.IsNullOrEmpty(Singleton<TransferMaterialObject>.Instance.Data.Name))
                    labelUid.Text = Singleton<TransferMaterialObject>.Instance.Data.Name;
            }
        }

        protected void OnOperationStateChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    UpdateOperationState();
                }));
            }
            else
                UpdateOperationState();
        }

        protected void OnLoginStateChanged(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() => { UpdateFormViewButtonState(); }));
        }

        protected void OnFinishedCycleStop(object sender, EventArgs e)
        {
            if (mainSequence != null && App.CycleStop)
            {
                mainSequence.FinishCycleStop();

                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {   // Turn back to Run
                        buttonStart.Text = Properties.Resources.String_FormMain_buttonStart;
                        buttonStart.BackColor = SystemColors.ButtonFace;
                        FormMessageExt.ShowInformation(Properties.Resources.String_FormMain_Information_Completed_CycleStop);
                    }));
                }
                else
                {
                    buttonStart.Text = Properties.Resources.String_FormMain_buttonStart;
                    buttonStart.BackColor = SystemColors.ButtonFace;
                    FormMessageExt.ShowNotification(Properties.Resources.String_FormMain_Information_Completed_CycleStop);
                }
            }
        }

        protected void OnCycleStopOrderStateChanged(object sender, EventArgs e)
        {
            if (App.CycleStop)
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        buttonStart.Text = Properties.Resources.String_FormMain_buttonStart_CycleStop;
                        buttonStart.BackColor = SystemColors.ButtonFace;
                        FormMessageExt.ShowNotification(Properties.Resources.String_FormMain_Information_Set_CycleStop);
                    }));
                }
                else
                {
                    buttonStart.Text = Properties.Resources.String_FormMain_buttonStart_CycleStop;
                    buttonStart.BackColor = SystemColors.ButtonFace;
                    FormMessageExt.ShowNotification(Properties.Resources.String_FormMain_Information_Set_CycleStop);
                }
            }
            else
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        buttonStart.Text = Properties.Resources.String_FormMain_buttonStart;
                        buttonStart.BackColor = SystemColors.ButtonFace;
                        FormMessageExt.ShowNotification(Properties.Resources.String_FormMain_Information_Reset_CycleStop);
                    }));
                }
                else
                {
                    buttonStart.Text = Properties.Resources.String_FormMain_buttonStart;
                    buttonStart.BackColor = SystemColors.ButtonFace;
                    FormMessageExt.ShowNotification(Properties.Resources.String_FormMain_Information_Reset_CycleStop);
                }
            }
        }

        protected void OnCartPresentStateChanged(object sender, CartPresentStates state)
        {

        }

        protected void OnChangedReelSizeOfCart(object sender, int reelsize)
        {
            string str_ = "CART IN / OUT [REEL SIZE: " + (reelsize <= 0 ? "X ]" : $"{reelsize} ]");

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => {
                    groupBoxCartInOut.Text = str_;
                    controlStatusLabelCartInchCheck.SetStatusValue($"REEL {reelsize.ToString()}", Color.Lime, SystemColors.ControlText, SystemColors.Desktop, SystemColors.Window);
                }));
            }
            else
            {
                groupBoxCartInOut.Text = str_;
                controlStatusLabelCartInchCheck.SetStatusValue($"REEL {reelsize.ToString()}", Color.Lime, SystemColors.ControlText, SystemColors.Desktop, SystemColors.Window);
            }
        }

        protected void ShowModallessNotificationWindow(string msg)
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected void OnClosedNotificationWindow(object sender, FormClosedEventArgs e)
        {
            notificationWindow = null;
        }

        protected void OnNotificationOfMainSequence(object sender, string msg)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    ShowModallessNotificationWindow(msg);
                }));
            }
            else
            {
                ShowModallessNotificationWindow(msg);
            }
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
            UpdateCommunicationState(labelReelTowerCommunicationStateValue, ReelTowerManager.CommunicationState);
        }

        private void OnClearLog(object sender, EventArgs e)
        {
            switch (tabControlLogPage.SelectedIndex)
            {
                case 0:
                    {
                        listBoxReelTowerComm.Items.Clear();
                    }
                    break;
                case 1:
                    {
                        listBoxRobotComm.Items.Clear();
                    }
                    break;
                case 2:
                    {
                        UpdateAlarmHistory();
                    }
                    break;
            }
        }

        protected void OnLogTabIndexChanged(object sender, EventArgs e)
        {
            switch (tabControlLogPage.SelectedIndex)
            {
                case 0:
                case 1:
                    {
                        buttonClearLog.Text = Properties.Resources.String_Clear;
                    }
                    break;
                case 2:
                    {
                        buttonClearLog.Text = Properties.Resources.String_Refresh;
                    }
                    break;
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

        private void OnFormShown(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                controlDigitalIo1.RefreshInputs();
                controlDigitalIo1.RefreshOutputs();
                (monitorStages = new FormMonitorStages()).Show();
            }
        }

        private void OnClickButtonMRBTMove(object sender, EventArgs e)
        {
            switch (mainSequence.CartMode)
            {
                case MobileRobotOperationModes.Load:
                    {
                        FormMessageExt.ShowInformation(Properties.Resources.String_FormMain_Information_Wait_Mrbt_Load);
                    }
                    break;
                case MobileRobotOperationModes.Unload:
                    {
                        FormMessageExt.ShowInformation(Properties.Resources.String_FormMain_Information_Wait_Mrbt_Unload);
                    }
                    break;
                default:
                    {
                        buttonMobileRobotControllerUsage.Text = ((UseMobileRobot = !UseMobileRobot) ? Properties.Resources.String_Mrbt_Auto_Mode : Properties.Resources.String_Mrbt_Manual_Mode);

                        if (mainSequence.GetDockingStates() == CartDockingStates.LoadCompleted)
                            buttonDock.Text = Properties.Resources.String_Process_Undock;
                        else
                            buttonDock.Text = Properties.Resources.String_Process_Dock;

                        if (UseMobileRobot)
                        {   
                            buttonDock.Visible = false;
                        }
                        else
                        {
                            if ((App.DigitalIoManager as DigitalIoManager).IsCartInWorkZone)
                            {
                                (App.DigitalIoManager as DigitalIoManager).DockCart();
                            }
                            else
                            {
                                (App.DigitalIoManager as DigitalIoManager).UndockCart();
                                new FormMessageExt(Properties.Resources.String_FormMain_Notification_Dock_Cart).ShowDialog();
                            }

                            mainSequence.ForceSetCartState();
                            buttonDock.Visible = true;
                        }
                    }
                    break;
            }
        }
        #endregion

        #region Config file handling methods
        #endregion

        #region Vision process methods
        public void FireChangedVisionState()
        {
            ChangedVisionState?.Invoke(this, EventArgs.Empty);
        }

        protected bool CheckVisionProcess()
        {
            try
            {
                Process[] pss = Process.GetProcessesByName("CamExE");
                return (pss.Length > 0);
            }
            catch (Exception ex)
            {
                Logger.Trace($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                return false;
            }
        }

        protected void StartVisionProcess()
        {
            switch (Config.ImageProcessorType)
            {
                case ImageProcessorTypes.Mit:
                    {
                        visionControl1.Visible = false;

                        if (File.Exists(@"D:\VISION\Cam1\CamExE.exe"))
                        {
                            ProcessStartInfo info = new ProcessStartInfo();
                            info.FileName = @"D:\VISION\Cam1\CamExE.exe";
                            info.Arguments = string.Empty;
                            info.UseShellExecute = true;
                            info.WindowStyle = ProcessWindowStyle.Maximized;
                            info.RedirectStandardInput = false;
                            info.RedirectStandardOutput = false;
                            info.RedirectStandardError = false;
                            // info.Verb = "open";
                            // info.CreateNoWindow = true;

                            System.Diagnostics.Process p = System.Diagnostics.Process.Start(info);
                            p.WaitForInputIdle();
                            Thread.Sleep(100);

                            tabPageVision.SizeChanged += new EventHandler(InitializeVisionPage);
                            InitializeVisionPage(this, EventArgs.Empty);
                        }
                        else
                        {
                            if (!Config.SystemSimulation)
                                FormMessageExt.ShowNotification(Properties.Resources.String_Notification_Check_Vision_Program_Path);
                        }
                    }
                    break;
                case ImageProcessorTypes.TechFloor:
                    {
                        groupBoxVisionAndLight.Visible = false;
                        visionControl1.LoadControl(App.Path);
                        visionControl1.RequestToSetLight += OnRequestToSetLight;
                        visionControl1.RequestToTurnOffLight += OnRequestToTurnOffLight;
                        visionControl1.RequestToOpenLightControlWindow += OnRequestToOpenLightControlWindow;
                        visionControl1.RequestToCheckTowerBasePoints += OnRequestToCheckTowerBasePoints;
                        visionControl1.RequestToCheckCartGuidePoints += OnRequestToCheckCartGuidePoints;
                        visionControl1.ReportProcessResult += OnReportProcessResult;
                    }
                    break;
            }
        }

        protected void SendMessage(int index, int camera, string data)
        {
            string message_ = string.Format($"{index},{data};");

            if (visionWnd.Equals(IntPtr.Zero))
                visionWnd = FindWindow(null, "Cam1");

            if (!visionWnd.Equals(IntPtr.Zero))
            {
                COPYDATASTRUCT cds = new COPYDATASTRUCT();
                cds.dwData = new IntPtr(camera);
                cds.cbData = (uint)Encoding.Unicode.GetBytes(message_).Length + 1;
                cds.lpData = message_;
                SendMessage(visionWnd, WM_COPYDATA, IntPtr.Zero, ref cds);
            }
        }

        protected void ShutdownVisionProcess()
        {
            switch (Config.ImageProcessorType)
            {
                case ImageProcessorTypes.Mit:
                    {
                        SendMessage(Convert.ToInt32(CameraInputs.CloseProgram), 1, string.Empty);
                    }
                    break;
                case ImageProcessorTypes.TechFloor:
                    {
                        if (visionControl1 != null)
                            visionControl1.Close();
                    }
                    break;
            }
        }

        protected bool StartVision()
        {
            switch (Config.ImageProcessorType)
            {
                case ImageProcessorTypes.Mit:
                    {
                        if (visionWnd.Equals(IntPtr.Zero))
                            visionWnd = FindWindow(null, "Cam1");

                        if (visionWnd != null)
                        {
                            RECT rect;
                            GetWindowRect(visionWnd, out rect);
                            Point pt_ = new Point(tabPageVision.Left + tabPageVision.Width / 2 - rect.Center.X,
                                tabPageVision.Top + tabPageVision.Height / 2 - rect.Center.Y);

                            if (rect.Center.X != tabPageVision.Width / 2 && !visionInitialized)
                            {   // SetParent(visionWnd, tabPageVision.Handle);
                                SetWindowPos(visionWnd, IntPtr.Zero, pt_.X, pt_.Y, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_SHOWWINDOW);
                                ShowWindowAsync(visionWnd, SW_SHOWMAXIMIZED);
                                visionInitialized = true;
                            }

                            SendMessage(Convert.ToInt32(CameraInputs.AutoRun), 1, "T");
                            visionState = true;
                        }
                        else
                            FormMessageExt.ShowNotification(Properties.Resources.String_FormMain_Notification_Vision_Program_Exists);
                    }
                    break;
                case ImageProcessorTypes.TechFloor:
                    {
                        if (visionControl1.CurrentRunState != TechFloor.Vision.CompositeImageProcessControlOld.RunState.Stopped)
                        {
                            if (visionControl1.CurrentRunState == TechFloor.Vision.CompositeImageProcessControlOld.RunState.RunningLive)
                                visionControl1.StopLive();
                            return false;
                        }
                    }
                    break;
            }

            FireChangedVisionState();
            return true;
        }

        protected void StopVision()
        {
            switch (Config.ImageProcessorType)
            {
                case ImageProcessorTypes.Mit:
                    {
                        SendMessage(Convert.ToInt32(CameraInputs.AutoRun), 1, "F");
                        visionState = false;
                    }
                    break;
                case ImageProcessorTypes.TechFloor:
                    {
                    }
                    break;
            }

            FireChangedVisionState();
        }

        protected void OnChangedAutoRunStateOfCamera(string data)
        {
            switch (data)
            {
                case "T":
                    visionState = true;
                    break;
                default:
                    visionState = false;
                    break;
            }

            Logger.Trace(visionState ? "Started vision camera 1." : "Stopped vision camera 1.");
        }

        protected void OnChangedReadyStateOfCamera(string data)
        {
            Logger.Trace(data == "T" ? "Vision camera 1 is ready." : "Vision camera 1 is not ready.");
        }

        protected void OnChangedBusyStateOfCamera(string data)
        {
            Logger.Trace(data == "T" ? "Vision camera 1 is busy." : "Vision camera 1 is idle.");
        }

        protected void OnChangedGrabStateOfCamera(string data)
        {
            Logger.Trace(data == "T" ? "Vision camera 1 is busy." : "Vision camera 1 is idle.");
        }

        protected void OnChangedResultOfCamera(string data)
        {

        }

        protected void OnChangedJobOfCamera(string data)
        {

        }

        protected void OnChangedLightValueOfCamera(string data)
        {
            string[] tokens_ = data.Split(CONST_LIGHT_VALUE_DELIMITER, StringSplitOptions.RemoveEmptyEntries);

            switch (Convert.ToInt32(tokens_[0]))
            {
                case 1:
                    {
                        visionLight.Send(tokens_[1]);
                        Logger.Trace($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Vision {tokens_[0]} light={tokens_[1]}");
                    }
                    break;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (Config.ImageProcessorType == ImageProcessorTypes.Mit)
            {
                switch (m.Msg)
                {
                    case WM_COPYDATA:
                        {
                            if (!shutdownEvent.WaitOne(1))
                            {
                                COPYDATASTRUCT cds = (COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT));

                                if ((int)cds.dwData == 1)
                                    ReadCameraResponse(cds.lpData);
                            }
                        }
                        break;
                    default:
                        base.WndProc(ref m);
                        break;
                }
            }
            else
                base.WndProc(ref m);
        }

        protected void ReadCameraResponse(string response)
        {
            switch (Config.ImageProcessorType)
            {
                case ImageProcessorTypes.Mit:
                    {
                        try
                        {
                            string strReceived_ = response;
                            string strTemp_ = string.Empty;
                            string argument = string.Empty;
                            string[] tokens_ = null;
                            int length = strReceived_.Length;
                            int posDelimiter_ = -1;
                            int responseCode_ = -1;

                            if (shutdown) return;

                            if (length > 0)
                            {
                                strReceived_ = visionResponse + strReceived_;
                                visionResponse = string.Empty;

                                if ((posDelimiter_ = strReceived_.LastIndexOf(";")) >= 0)
                                {
                                    tokens_ = strReceived_.Split(CONST_VISION_DATA_DELIMITER, StringSplitOptions.RemoveEmptyEntries);
                                    strReceived_ = string.Empty;

                                    if (posDelimiter_ < strReceived_.Length - 1)
                                        visionResponse = tokens_[tokens_.Length - 1];
                                }
                                else
                                {
                                    visionResponse = strReceived_;
                                    return;
                                }
                            }
                            else
                            {
                                return;
                            }

                            string csCamResult_X = string.Empty;
                            string csCamResult_Y = string.Empty;
                            string csCamResult_T = string.Empty;

                            foreach (string token_ in tokens_)
                            {
                                posDelimiter_ = token_.IndexOf(",");

                                if (posDelimiter_ < 0)
                                    continue;

                                responseCode_ = Convert.ToInt32(token_.Substring(0, posDelimiter_));
                                argument = token_.Substring(posDelimiter_ + 1, token_.Length - posDelimiter_ - 1);

                                if (string.IsNullOrEmpty(argument))
                                    return; // Abnormal

                                switch ((CameraOutputs)responseCode_)
                                {
                                    case CameraOutputs.AutoRun:
                                        OnChangedAutoRunStateOfCamera(argument);
                                        break;

                                    //READY 
                                    case CameraOutputs.Ready:
                                        OnChangedReadyStateOfCamera(argument);
                                        break;

                                    //BUSY
                                    case CameraOutputs.Busy:
                                        OnChangedBusyStateOfCamera(argument);
                                        break;

                                    //GRABEND
                                    case CameraOutputs.GrabEnd:
                                        OnChangedGrabStateOfCamera(argument);
                                        break;

                                    //RESULT
                                    case CameraOutputs.Result:
                                        OnChangedResultOfCamera(argument);
                                        break;

                                    case CameraOutputs.ChangeJob:
                                        OnChangedJobOfCamera(argument);
                                        break;

                                    case CameraOutputs.LightValue:
                                        OnChangedLightValueOfCamera(argument);
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Trace($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                        }
                    }
                    break;
                case ImageProcessorTypes.TechFloor:
                    {

                    }
                    break;
            }
        }

        protected void OnClickButtionStartVision(object sender, EventArgs e)
        {

            if (!visionState)
            {
                StopVision();
            }
            else
            {
                if (!CheckVisionProcess())
                {
                    FormMessageExt.ShowNotificationWithBuzzer(Properties.Resources.String_FormMain_Notification_Check_Vision_Run);
                }
                else
                {
                    StartVision();
                }
            }
        }
        protected void TextChangedButtonStartVision(object sender, EventArgs e)
        {
            buttonVisionRestart.Visible = visionState;
        }

        protected void UpdateDisplay()
        {
            if (mainSequence.CommunicationStateOfReelTower != currentReelTowerCommState)
                UpdateCommunicationState(labelReelTowerCommunicationStateValue, mainSequence.CommunicationStateOfReelTower);

            UpdateRobotSequenceState();
            UpdateCartInformation();
            UpdateMobileRobotSequenceStates();
            UpdatePerformanceInformation();
        }

        protected void UpdateOutputStageState()
        {
            if (mainSequence != null && mainSequence.IsLatestVersion)
                mainSequence.ValidateOutputStageFullSignal();
        }

        protected void UpdateRejectStageState()
        {
            if (mainSequence != null && Config.EnableReturnReelTypeWatcher)
                mainSequence.ValidateRejectStageFullSignal();
        }

        protected void UpdateReturnReelState()
        {
            if (mainSequence != null && Config.EnableReturnReelTypeWatcher)
                mainSequence.ValidateReturnReelPresentSignal();
        }

        protected void OnDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                while (!shutdown)
                {
                    if (!shutdownEvent.WaitOne(100) && mainSequence != null)
                    {
                        if (InvokeRequired)
                        {
                            BeginInvoke(new Action(() =>
                            {
                                UpdateReturnReelState();
                                UpdateOutputStageState();
                                UpdateRejectStageState();
                                UpdateDisplay();
                            }));
                        }
                        else
                        {
                            UpdateReturnReelState();
                            UpdateOutputStageState();
                            UpdateRejectStageState();
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

        protected void OnClickButtonReel7LightOn(object sender, EventArgs e)
        {
            if (Config.ImageProcessorType == ImageProcessorTypes.Mit)
                CompositeVisionManager.SetVisionLight(ReelDiameters.ReelDiameter7);
        }

        protected void OnClickButtonReel13LightOn(object sender, EventArgs e)
        {
            if (Config.ImageProcessorType == ImageProcessorTypes.Mit)
                CompositeVisionManager.SetVisionLight(ReelDiameters.ReelDiameter13);
        }

        protected void OnClickButtonLightOff(object sender, EventArgs e)
        {
            VisionManager._CILightOff();
        }

        protected void OnClickButtonVisionGrabReel7(object sender, EventArgs e)
        {
            VisionManager.TriggerForReel7Alignment();
        }

        protected void OnClickButtonVisionGrabReel13(object sender, EventArgs e)
        {
            VisionManager.TriggerForReel13Alignment();
        }

        protected void OnClickButtonVisionFindCenter(object sender, EventArgs e)
        {
            string alignmentCoordX = string.Empty;
            string alignmentCoordY = string.Empty;
            ImageProcssingResults imageProcessingResult = CompositeVisionManager.GetAdjustmentValue(
                mainSequence.DistanceXOfAlignError,
                mainSequence.DistanceYOfAlignError,
                ref alignmentCoordX,
                ref alignmentCoordY);

            switch (imageProcessingResult)
            {
                case ImageProcssingResults.Success:
                case ImageProcssingResults.OverScope:
                    labelAlignmentResult.Text = $"X={alignmentCoordX}, Y={alignmentCoordY}";
                    break;
                case ImageProcssingResults.Empty:
                    labelAlignmentResult.Text = "Empty";
                    break;
                case ImageProcssingResults.Exception:
                    labelAlignmentResult.Text = "Exception";
                    break;
                case ImageProcssingResults.Unknown:
                    labelAlignmentResult.Text = "Unknown";
                    break;
            }
        }

        protected void OnClickButtonVisionReadBarcode(object sender, EventArgs e)
        {
            MaterialData reelBarcodeContexts = new MaterialData();
            
            if (CompositeVisionManager.GetBarcode(ref reelBarcodeContexts)) // Just test
            {   // Validate barcode contexts.
                labelDecodeBarcodeResult.Text = $"MFG={reelBarcodeContexts.ManufacturedDatetime},RID={reelBarcodeContexts.Name}";
            }
            else
            {
                labelDecodeBarcodeResult.Text = "Failed to decode.";
            }
        }
        #endregion

        #region Save model information
        protected void OnClickButtonSaveModel(object sender, EventArgs e)
        {
            try
            {
                Model.AlignmentRangeLimit = new PointF(Convert.ToSingle(textBoxVisionAlignmentRangeLimitX.Text),
                    Convert.ToSingle(textBoxVisionAlignmentRangeLimitY.Text));
                Model.RetryOfVisionFailure = Convert.ToInt32(textBoxVisionFailureRetry.Text);
                Model.DelayOfImageProcessing = Convert.ToInt32(textBoxImageProcessingDelay.Text);
                Model.DelayOfReturnReelSensing = Convert.ToInt32(textBoxReturnReelSensingDelay.Text);
                Model.DelayOfUnloadReelStateUpdate = Convert.ToInt32(textBoxUnloadReelStateUpdateDelay.Text);
                Model.RetryOfVisionAttempts = Convert.ToInt32(textBoxVisioinRetryAttempts.Text);
                Model.TimeoutOfImageProcessing = Convert.ToInt32(textBoxImageProcessingTimeout.Text);
                Model.DelayOfTrigger = Convert.ToInt32(textBoxDelayOfCameraTrigger.Text);

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
        protected void OnDoubleClickMrbtCommunication(object sender, EventArgs e)
        {
            mainSequence.MobileRobotManager.RestartMobileRobotServiceManager(Config.MobileRobotLocalServerPort);
        }

        protected void OnDoubleClickRobotSequenceManager(object sender, EventArgs e)
        {
            mainSequence.RobotSequenceManager.RestartRobotSequenceManager(Config.RobotLocalServerPort);
        }

        protected void OnDoubleClickReelTowerManager(object sender, EventArgs e)
        {
            mainSequence.ReelTowerManager.RestartReelTowerManager(Config.ReelTowerLocalServerPort);
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

        private void OnClickVisionRestart(object sender, EventArgs e)
        {
            bool runVision_ = false;
            visionInitialized = false;

            if (Config.ImageProcessorType == ImageProcessorTypes.Mit)
            {
                VisionManager.ReadingDataInit();

                if (runVision_ = CheckVisionProcess())
                    FormMessageExt.ShowNotificationWithBuzzer(Properties.Resources.String_FormMain_Notification_Check_Vision_Run);
            }

            if (!runVision_)
                StartVisionProcess();
        }

        #region Vision control buttons
        public void UpdateVisionResult(object sender, EventArgs e)
        {

        }

        protected void UpdateVisionControlsEnabled()
        {

        }

        protected void OnClickButtonOpenSetting(object sender, EventArgs e)
        {
            
        }

        protected void OnClickButtonSaveSetting(object sender, EventArgs e)
        {

        }

        protected void OnChangedStateCheckBoxLiveVision(object sender, EventArgs e)
        {

        }

        protected void OnClickButtonTestVision(object sender, EventArgs e)
        {

        }

        protected void OnClickButtonApplyLight(object sender, EventArgs e)
        {

        }

        protected void OnClickButtonSaveLight(object sender, EventArgs e)
        {

        }

        protected void OnSelectedIndexChangedProcessType(object sender, EventArgs e)
        {
           
        }
        #endregion

        #endregion

        #region Light control window
        protected void OnRequestToSetLight(object sender, int model)
        {
            if (Model.VisionLights.Count > 0)
            {
                int value = Model.Process.GetLightValue(model, 1);

                if (value > 0)
                    VisionLightManager.SendCommand(VisionLightManager.ControllerCommands.Set, 1, value);// string.Format("B1{0:000}#", value));
            }
        }

        protected void OnRequestToTurnOffLight(object sender, EventArgs e)
        {   // Manual reset
            VisionLightManager.SendCommand(VisionLightManager.ControllerCommands.Off, 1, 0);// "F1000#");

            if (mainSequence != null)
            {
                mainSequence.SetVisionProcessResult(visionControl1.LastRunResult);
                mainSequence.SetVisionProcessLockState(0); // Unlock
            }
        }

        protected void OnRequestToOpenLightControlWindow(object sender, EventArgs e)
        {
            lock (lightSyncObject)
            {
                if (lightControlWnd == null)
                {
                    lightControlWnd = new FormLight();
                    lightControlWnd.FormClosed += OnFormClosedLightControlWnd;
                    lightControlWnd.Show();
                }
            }
        }

        protected void OnRequestToCheckTowerBasePoints(object sender, EventArgs e)
        {
            if (App.OperationState != OperationStates.Stop)
                FormMessageExt.ShowInformation(Properties.Resources.String_Information_Stop_Process_First);
            else
            {
                if (App.Initialized)
                    mainSequence.Calibrate();
                else
                    FormMessageExt.ShowInformation(Properties.Resources.String_Information_Initialization_First);
            }
        }

        protected void OnRequestToCheckCartGuidePoints(object sender, EventArgs e)
        {
        }

        protected void OnFormClosedLightControlWnd(object sender, FormClosedEventArgs e)
        {
            lightControlWnd = null;
        }

        protected void OnReportProcessResult(object sender, EventArgs e)
        {
            if (mainSequence != null)
            {
                mainSequence.SetVisionProcessResult(visionControl1.LastRunResult);
                mainSequence.SetVisionProcessLockState(0); // Unlock
            }
        }
        #endregion

        #region Culture setting
        protected void SetDisplayLanguage()
        {
            buttonStart.Text = Properties.Resources.String_FormMain_buttonStart;
            buttonStop.Text = Properties.Resources.String_FormMain_buttonStop;
            buttonReset.Text = Properties.Resources.String_FormMain_buttonReset;
            buttonInitialize.Text = Properties.Resources.String_FormMain_buttonInitialize;
            buttonClearLog.Text = Properties.Resources.String_Clear;
            buttonMobileRobotControllerUsage.Text = Properties.Resources.String_Mrbt_Auto_Mode;

            tabPageOperation.Text = Properties.Resources.String_FormMain_tabPageOperation;
            tabPageVision.Text = Properties.Resources.String_FormMain_tabPageVision;
            tabPageMaintenance.Text = Properties.Resources.String_FormMain_tabPageMaintenance;
            tabPageConfig.Text = Properties.Resources.String_FormMain_tabPageConfig;
            tabPageLog.Text = Properties.Resources.String_FormMain_tabPageLog;
            tabPageReelTowerLog.Text = Properties.Resources.String_FormMain_tabPageReelTowerLog;
            tabPageRobotLog.Text = Properties.Resources.String_FormMain_tabPageRobotLog;
            tabPageAlarmHistory.Text = Properties.Resources.String_FormMain_tabPageAlarmHistory;
            labelReelTowerCommunicationState.Text = Properties.Resources.String_FormMain_labelReelTowerCommunicationState;
            labelMobileRobotCommunication.Text = Properties.Resources.String_FormMain_labelMobileRobotCommunication;
            labelRobotCommunicationState.Text = Properties.Resources.String_FormMain_labelRobotCommunicationState;
        }

        public void SetCulture(string culturecode)
        {
            if (appInstance_ != null)
            {
                appInstance_.SetCultureCode(culturecode, false);
                Properties.Resources.Culture = new System.Globalization.CultureInfo(App.CultureInfoCode);
            }
        }
        #endregion

        #region Vision control methods
        protected void TriggerVisionControl(VisionProcessEventArgs e)
        {
            try
            {
                if (visionControl1 != null)
                {   // Set vision process type
                    Dictionary<int, object> items = new Dictionary<int, object>();

                    switch (e.ProcessType)
                    {
                        default:
                            {   
                                items.Add((int)ProcessArguments.ProcessType, e.ProcessType);
                                items.Add((int)ProcessArguments.CenterXOffsetLimit, e.CenterXOffsetLimit);
                                items.Add((int)ProcessArguments.CenterYOffsetLimit, e.CenterYOffsetLimit);

                                switch (e.ProcessType)
                                {
                                    case 12:
                                        {
                                            items.Add((int)ProcessArguments.TowerBasePointMode, e.OptionMode);
                                            items.Add((int)ProcessArguments.TowerBasePointReferenceX, e.OptionCoordX);
                                            items.Add((int)ProcessArguments.TowerBasePointReferenceY, e.OptionCoordY);
                                        }
                                        break;
                                    case 13:
                                        {
                                            items.Add((int)ProcessArguments.CartGuidePointMode, e.OptionMode);
                                            items.Add((int)ProcessArguments.CartGuidePointPitchX, e.OptionCoordX);
                                            items.Add((int)ProcessArguments.CartGuidePointPitchY, e.OptionCoordY);
                                        }
                                        break;
                                }
                            }
                            break;
                    }

                    visionControl1.RunOnce(e.LightCategory, e.TriggerDelay, new VisionProcessDataObject(VisionProcessDataObjectTypes.ProcessArgument, items));
                }
            }
            catch (Exception ex)
            {   // Reset waiting flag
                mainSequence.SetVisionProcessResult(null);
                mainSequence.SetVisionProcessLockState(0); // Unlock
                Logger.Trace($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
            catch { throw; }
        }

        protected void OnTriggerVisionControl(object sender, VisionProcessEventArgs e)
        {
            if (visionControl1 != null)
            {
                if (visionControl1.InvokeRequired)
                {
                    BeginInvoke(new Action(() => { TriggerVisionControl(e); }));
                }
                else
                {
                    TriggerVisionControl(e);
                }
            }
        }
        #endregion

        #region Monitor selection
        protected void GetCurrentMonitor()
        {
            Screen[] screens = Screen.AllScreens;
            for (int i = 0; i < screens.Length; i++)
            {
                if (screens[i].WorkingArea.Contains(this.Location))
                {
                    displayMonitor = screens[i];
                    break;
                }
            }
        }

        public void SetDisplayMonitor(Form wnd)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    wnd.StartPosition = FormStartPosition.Manual;
                    wnd.Location = new Point(displayMonitor.Bounds.Location.X + (displayMonitor.Bounds.Width - wnd.Width) / 2, displayMonitor.Bounds.Y + (displayMonitor.Bounds.Height - wnd.Height) / 2);
                }));
            }
            else
            {
                wnd.StartPosition = FormStartPosition.Manual;
                wnd.Location = new Point(displayMonitor.Bounds.Location.X + (displayMonitor.Bounds.Width - wnd.Width) / 2, displayMonitor.Bounds.Y + (displayMonitor.Bounds.Height - wnd.Height) / 2);
            }
        }
        #endregion

        #region Queued list 
        protected void OnClickQueuedList(object sender, EventArgs e)
        {
            if (OperationState == OperationStates.Run)
            {
                FormMessageExt.ShowInformation(Properties.Resources.String_FormQuedList_Notification_Stop_To_Show_Confirm);
                return;
            }

            if ((new FormQueuedList()).ShowDialog() == DialogResult.OK)
            {
                ClearMaterialPackage();
                ReelTowerManager.ClearAllReelTowerStates();
            }
            else if (Singleton<MaterialPackageManager>.Instance.Materials.Count <= 0)
            {
                if (ReelTowerManager.HasUnloadRequest())
                {
                    ReelTowerManager.ClearAllReelTowerStates();
                    FormMessageExt.ShowInformation(Properties.Resources.String_FormQuedList_Notification_Cleared_Remained_Unload_Requests);
                }
            }
        }

        public void SetFocus(GuiPages page = GuiPages.MainPage)
        {
            switch (page)
            {
                default:
                case GuiPages.MainPage:
                    {
                        this.ActiveControl = tabControlMain;
                        tabControlMain.Focus();
                    }
                    break;
            }
        }
        #endregion
    }
}
#endregion