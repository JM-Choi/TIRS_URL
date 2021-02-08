namespace TechFloor
{
    partial class FormMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Log", 4, 5);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.labelCurrentRecipe = new System.Windows.Forms.Label();
            this.labelOperationState = new System.Windows.Forms.Label();
            this.buttonExit = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.labelCycleLoTowerInput = new System.Windows.Forms.Label();
            this.labelAverageLoTowerInput = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.labelMobileRobotState = new System.Windows.Forms.Label();
            this.labelRobotState = new System.Windows.Forms.Label();
            this.labelRobot = new System.Windows.Forms.Label();
            this.labelMobileRobotCommunication = new System.Windows.Forms.Label();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.tableLayoutPanelTitle = new System.Windows.Forms.TableLayoutPanel();
            this.labelVision = new System.Windows.Forms.Label();
            this.buttonVisionStart = new System.Windows.Forms.Button();
            this.labelRobotCommunicationState = new System.Windows.Forms.Label();
            this.labelMobileRobot = new System.Windows.Forms.Label();
            this.labelReelTowerCommunicationStateValue = new System.Windows.Forms.Label();
            this.labelReelTowerCommunicationState = new System.Windows.Forms.Label();
            this.labelMobileRobotCommunicationStateValue = new System.Windows.Forms.Label();
            this.labelRobotCommunicationStateValue = new System.Windows.Forms.Label();
            this.tableLayoutPanelControlMenu = new System.Windows.Forms.TableLayoutPanel();
            this.labelApplicationVersion = new System.Windows.Forms.Label();
            this.buttonInitialize = new System.Windows.Forms.Button();
            this.groupBoxRuntimeTrace = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelRuntimeTrace = new System.Windows.Forms.TableLayoutPanel();
            this.labelLastCycleTime = new System.Windows.Forms.Label();
            this.labelRobotStep = new System.Windows.Forms.Label();
            this.labelRobotStepValue = new System.Windows.Forms.Label();
            this.labelMobileRobotStep = new System.Windows.Forms.Label();
            this.labelTransferStateRSValue = new System.Windows.Forms.Label();
            this.labelReelTowerStep = new System.Windows.Forms.Label();
            this.labelMobileRobotStepValue = new System.Windows.Forms.Label();
            this.labelReelTowerStepValue = new System.Windows.Forms.Label();
            this.labelTransferModeRSValue = new System.Windows.Forms.Label();
            this.labelLastCommandRSValue = new System.Windows.Forms.Label();
            this.labelNextWaypointRSValue = new System.Windows.Forms.Label();
            this.labelCurrentWaypointRSValue = new System.Windows.Forms.Label();
            this.label54 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.groupBoxPerformance = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelPerformance = new System.Windows.Forms.TableLayoutPanel();
            this.labelVisionDecodeError = new System.Windows.Forms.Label();
            this.labelVisionDecodeErrorValue = new System.Windows.Forms.Label();
            this.labelVisionAlignError = new System.Windows.Forms.Label();
            this.labelVisionAlignErrorValue = new System.Windows.Forms.Label();
            this.labelReturnErrorCount = new System.Windows.Forms.Label();
            this.labelReturnErrorCountValue = new System.Windows.Forms.Label();
            this.labelUnloadErrorCount = new System.Windows.Forms.Label();
            this.labelUnloadErrorCountValue = new System.Windows.Forms.Label();
            this.labelLoadErrorCount = new System.Windows.Forms.Label();
            this.labelLoadErrorCountValue = new System.Windows.Forms.Label();
            this.labelTotalReturn = new System.Windows.Forms.Label();
            this.labelTotalReturnReelCountValue = new System.Windows.Forms.Label();
            this.labelTotalUnloadReelCount = new System.Windows.Forms.Label();
            this.labelTotalUnloadReelCountValue = new System.Windows.Forms.Label();
            this.labelTotalLoadReelCount = new System.Windows.Forms.Label();
            this.labelElapsedValue = new System.Windows.Forms.Label();
            this.labelElapsed = new System.Windows.Forms.Label();
            this.labelTotalLoadReelCountValue = new System.Windows.Forms.Label();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageOperation = new System.Windows.Forms.TabPage();
            this.buttonShowQueue = new System.Windows.Forms.Button();
            this.groupBoxAnimation = new System.Windows.Forms.GroupBox();
            this.groupBoxUnloadReel = new System.Windows.Forms.GroupBox();
            this.controlStatusLabelCompleteUnload = new TechFloor.Gui.ControlStatusLabel();
            this.controlStatusLabelPutUnloadReelToOutput = new TechFloor.Gui.ControlStatusLabel();
            this.controlStatusLabelTakeReelFromTower = new TechFloor.Gui.ControlStatusLabel();
            this.controlStatusLabelMoveToTower = new TechFloor.Gui.ControlStatusLabel();
            this.controlStatusLabelPickList = new TechFloor.Gui.ControlStatusLabel();
            this.groupBoxCartInOut = new System.Windows.Forms.GroupBox();
            this.controlStatusLabelCartInOut = new TechFloor.Gui.ControlStatusLabel();
            this.controlStatusLabelMrbtDocking = new TechFloor.Gui.ControlStatusLabel();
            this.controlStatusLabelMrbtMove = new TechFloor.Gui.ControlStatusLabel();
            this.buttonDock = new System.Windows.Forms.Button();
            this.buttonMobileRobotControllerUsage = new System.Windows.Forms.Button();
            this.groupBoxLoadReel = new System.Windows.Forms.GroupBox();
            this.controlStatusLabelCompleteLoad = new TechFloor.Gui.ControlStatusLabel();
            this.controlStatusLabelPutReel = new TechFloor.Gui.ControlStatusLabel();
            this.controlStatusLabelPickup = new TechFloor.Gui.ControlStatusLabel();
            this.controlStatusLabelDecodeQrBarcode = new TechFloor.Gui.ControlStatusLabel();
            this.controlStatusLabelVisionAlign = new TechFloor.Gui.ControlStatusLabel();
            this.controlStatusLabelCartInchCheck = new TechFloor.Gui.ControlStatusLabel();
            this.tableLayoutPanelPickingList = new System.Windows.Forms.TableLayoutPanel();
            this.labelOutputReelDateTimeValue = new System.Windows.Forms.Label();
            this.labelOutputReelDateTime = new System.Windows.Forms.Label();
            this.labelOutputReelCountValue = new System.Windows.Forms.Label();
            this.labelOutputLocationValue = new System.Windows.Forms.Label();
            this.labelPickingIdValue = new System.Windows.Forms.Label();
            this.labelPickingId = new System.Windows.Forms.Label();
            this.labelOutputLocation = new System.Windows.Forms.Label();
            this.labelOutputReelCount = new System.Windows.Forms.Label();
            this.labelOutputReelList = new System.Windows.Forms.Label();
            this.listBoxOutputReelList = new System.Windows.Forms.ListBox();
            this.labelPickingList = new System.Windows.Forms.Label();
            this.tableLayoutPanelTransferReelInformation = new System.Windows.Forms.TableLayoutPanel();
            this.labelReelTransferStateValue = new System.Windows.Forms.Label();
            this.labelReelTransferState = new System.Windows.Forms.Label();
            this.labelReelTransferMode = new System.Windows.Forms.Label();
            this.labelTransferMode = new System.Windows.Forms.Label();
            this.labelMfg = new System.Windows.Forms.Label();
            this.labelQty = new System.Windows.Forms.Label();
            this.labelSupplier = new System.Windows.Forms.Label();
            this.labelLotId = new System.Windows.Forms.Label();
            this.labelSid = new System.Windows.Forms.Label();
            this.labelUid = new System.Windows.Forms.Label();
            this.labelReelTransferDestination = new System.Windows.Forms.Label();
            this.labelReelTransferSource = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.labelTransferDestination = new System.Windows.Forms.Label();
            this.labelTrasferSource = new System.Windows.Forms.Label();
            this.labelTransferReelInformation = new System.Windows.Forms.Label();
            this.labelReelTower = new System.Windows.Forms.Label();
            this.tableLayoutPanelReelTowerStates = new System.Windows.Forms.TableLayoutPanel();
            this.labelReelTowerState2 = new System.Windows.Forms.Label();
            this.labelReelTowerState1 = new System.Windows.Forms.Label();
            this.labelReelTowerState3 = new System.Windows.Forms.Label();
            this.labelReelTowerState4 = new System.Windows.Forms.Label();
            this.tableLayoutPanelReelTower1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTowerMode1 = new System.Windows.Forms.Label();
            this.labelTowerCodeId1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelDoorStateTower1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelReelTowerId1 = new System.Windows.Forms.Label();
            this.lblTowerState1 = new System.Windows.Forms.Label();
            this.tableLayoutPanelReelTower2 = new System.Windows.Forms.TableLayoutPanel();
            this.label32 = new System.Windows.Forms.Label();
            this.labelTowerCodeId2 = new System.Windows.Forms.Label();
            this.labelDoorStateTower2 = new System.Windows.Forms.Label();
            this.lblTowerState2 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.labelReelTowerId2 = new System.Windows.Forms.Label();
            this.lblTowerMode2 = new System.Windows.Forms.Label();
            this.tableLayoutPanelReelTower3 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTowerState3 = new System.Windows.Forms.Label();
            this.labelTowerCodeId3 = new System.Windows.Forms.Label();
            this.labelReelTowerId3 = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.labelDoorStateTower3 = new System.Windows.Forms.Label();
            this.label70 = new System.Windows.Forms.Label();
            this.label71 = new System.Windows.Forms.Label();
            this.lblTowerMode3 = new System.Windows.Forms.Label();
            this.tableLayoutPanelReelTower4 = new System.Windows.Forms.TableLayoutPanel();
            this.label76 = new System.Windows.Forms.Label();
            this.labelTowerCodeId4 = new System.Windows.Forms.Label();
            this.labelDoorStateTower4 = new System.Windows.Forms.Label();
            this.lblTowerState4 = new System.Windows.Forms.Label();
            this.label78 = new System.Windows.Forms.Label();
            this.label79 = new System.Windows.Forms.Label();
            this.labelReelTowerId4 = new System.Windows.Forms.Label();
            this.lblTowerMode4 = new System.Windows.Forms.Label();
            this.lblURClientConnectState = new System.Windows.Forms.Label();
            this.btnClientConnect = new System.Windows.Forms.Button();
            this.tabPageVision = new System.Windows.Forms.TabPage();
            this.visionControl1 = new TechFloor.Vision.CompositeImageProcessControl();
            this.tabPageMaintenance = new System.Windows.Forms.TabPage();
            this.groupBoxVisionAndLight = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelVisionAndLight = new System.Windows.Forms.TableLayoutPanel();
            this.buttonVisionRestart = new System.Windows.Forms.Button();
            this.labelDecodeBarcodeResult = new System.Windows.Forms.Label();
            this.labelAlignmentResult = new System.Windows.Forms.Label();
            this.buttonVisionGrabReel13 = new System.Windows.Forms.Button();
            this.labelReel13 = new System.Windows.Forms.Label();
            this.labelReel7 = new System.Windows.Forms.Label();
            this.buttonLightOnReel7 = new System.Windows.Forms.Button();
            this.buttonVisionFindCenter = new System.Windows.Forms.Button();
            this.buttonLightOnReel13 = new System.Windows.Forms.Button();
            this.buttonReadBarcode = new System.Windows.Forms.Button();
            this.buttonLightOff = new System.Windows.Forms.Button();
            this.buttonVisionGrabReel7 = new System.Windows.Forms.Button();
            this.groupBoxDigitalIo = new System.Windows.Forms.GroupBox();
            this.controlDigitalIo1 = new TechFloor.Gui.ControlDigitalIo();
            this.tabPageConfig = new System.Windows.Forms.TabPage();
            this.buttonSaveGuiSettings = new System.Windows.Forms.Button();
            this.buttonSaveModel = new System.Windows.Forms.Button();
            this.groupBoxVisionProcessImage = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxVisionProcessCompressImage = new System.Windows.Forms.CheckBox();
            this.textBoxVisionProcessRejectImagePath = new System.Windows.Forms.TextBox();
            this.checkBoxVisionProcessSaveImage = new System.Windows.Forms.CheckBox();
            this.labelVisionProcessRejectImageFilePath = new System.Windows.Forms.Label();
            this.labelVisionProcessAcceptImagePath = new System.Windows.Forms.Label();
            this.labelVisionProcessImageFileExtension = new System.Windows.Forms.Label();
            this.textBoxVisionProcessImageFileExtension = new System.Windows.Forms.TextBox();
            this.textBoxVisionProcessImageFileNameFormat = new System.Windows.Forms.TextBox();
            this.labelVisionProcessImageFileNameFormat = new System.Windows.Forms.Label();
            this.textBoxVisionProcessAcceptImagePath = new System.Windows.Forms.TextBox();
            this.groupBoxModel = new System.Windows.Forms.GroupBox();
            this.checkBoxVisionProcessProductionMode = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelModelVisionProperties = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxVisioinRetryAttempts = new System.Windows.Forms.TextBox();
            this.labelVisionAlignmentRangeLimit = new System.Windows.Forms.Label();
            this.labelVisionRetryAttempts = new System.Windows.Forms.Label();
            this.textBoxVisionAlignmentRangeLimitX = new System.Windows.Forms.TextBox();
            this.textBoxVisionAlignmentRangeLimitY = new System.Windows.Forms.TextBox();
            this.textBoxVisionFailureRetry = new System.Windows.Forms.TextBox();
            this.labelVisionFailureRetry = new System.Windows.Forms.Label();
            this.labelDelayOfCameraTriggger = new System.Windows.Forms.Label();
            this.labelImageProcessingTimeout = new System.Windows.Forms.Label();
            this.textBoxImageProcessingTimeout = new System.Windows.Forms.TextBox();
            this.textBoxDelayOfCameraTrigger = new System.Windows.Forms.TextBox();
            this.groupBoxDelayProperties = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelModelDelayProperties = new System.Windows.Forms.TableLayoutPanel();
            this.labelUnloadReelStateUpdateDelay = new System.Windows.Forms.Label();
            this.labelReturnReelSensingDelay = new System.Windows.Forms.Label();
            this.textBoxReturnReelSensingDelay = new System.Windows.Forms.TextBox();
            this.textBoxImageProcessingDelay = new System.Windows.Forms.TextBox();
            this.labelReelSizeDetectingDelay = new System.Windows.Forms.Label();
            this.textBoxUnloadReelStateUpdateDelay = new System.Windows.Forms.TextBox();
            this.buttonSaveTimeoutProperties = new System.Windows.Forms.Button();
            this.buttonSaveNetworkSettings = new System.Windows.Forms.Button();
            this.groupBoxTimeoutProperties = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelTimeout = new System.Windows.Forms.TableLayoutPanel();
            this.labelRobotHomeTimeout = new System.Windows.Forms.Label();
            this.labelRobotMoveTimeout = new System.Windows.Forms.Label();
            this.labelReelTowerResponseTimeout = new System.Windows.Forms.Label();
            this.textBoxReelTowerResponseTimeout = new System.Windows.Forms.TextBox();
            this.textBoxCartInOutCheckTimeout = new System.Windows.Forms.TextBox();
            this.labelCartInOutCheckTimeout = new System.Windows.Forms.Label();
            this.labelRobotCommunicationTimeout = new System.Windows.Forms.Label();
            this.labelRobotProgramLoadTimeout = new System.Windows.Forms.Label();
            this.labelRobotProgramPlayTimeout = new System.Windows.Forms.Label();
            this.labelRobotActionTimeout = new System.Windows.Forms.Label();
            this.textBoxRobotCommunicationTimeout = new System.Windows.Forms.TextBox();
            this.textBoxRobotProgramLoadTimeout = new System.Windows.Forms.TextBox();
            this.textBoxRobotProgramPlayTimeout = new System.Windows.Forms.TextBox();
            this.textBoxRobotActionTimeout = new System.Windows.Forms.TextBox();
            this.textBoxRobotMoveTimeout = new System.Windows.Forms.TextBox();
            this.textBoxRobotHomeTimeout = new System.Windows.Forms.TextBox();
            this.groupBoxNetworkSetting = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelNetwork = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxRobotControllerAddress = new System.Windows.Forms.TextBox();
            this.label38 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxReelTowerAddress = new System.Windows.Forms.TextBox();
            this.textBoxReelTowerPort = new System.Windows.Forms.TextBox();
            this.textBoxMobileRobotAddress = new System.Windows.Forms.TextBox();
            this.textBoxMobileRobotPort = new System.Windows.Forms.TextBox();
            this.textBoxRobotControllerPort = new System.Windows.Forms.TextBox();
            this.textBoxRobotPort = new System.Windows.Forms.TextBox();
            this.groupBoxGuiSettings = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelDisplayLanguage = new System.Windows.Forms.Label();
            this.comboBoxDisplayLanguage = new System.Windows.Forms.ComboBox();
            this.tabPageLog = new System.Windows.Forms.TabPage();
            this.buttonClearLog = new System.Windows.Forms.Button();
            this.tabControlLogPage = new System.Windows.Forms.TabControl();
            this.tabPageReelTowerLog = new System.Windows.Forms.TabPage();
            this.listBoxReelTowerComm = new System.Windows.Forms.ListBox();
            this.tabPageRobotLog = new System.Windows.Forms.TabPage();
            this.listBoxRobotComm = new System.Windows.Forms.ListBox();
            this.tabPageAlarmHistory = new System.Windows.Forms.TabPage();
            this.treeViewLog = new System.Windows.Forms.TreeView();
            this.imageListFileSystem = new System.Windows.Forms.ImageList(this.components);
            this.listBoxAlarmHistory = new System.Windows.Forms.ListBox();
            this.tableLayoutPanelMainFrame = new System.Windows.Forms.TableLayoutPanel();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanelTitle.SuspendLayout();
            this.tableLayoutPanelControlMenu.SuspendLayout();
            this.groupBoxRuntimeTrace.SuspendLayout();
            this.tableLayoutPanelRuntimeTrace.SuspendLayout();
            this.groupBoxPerformance.SuspendLayout();
            this.tableLayoutPanelPerformance.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageOperation.SuspendLayout();
            this.groupBoxUnloadReel.SuspendLayout();
            this.groupBoxCartInOut.SuspendLayout();
            this.groupBoxLoadReel.SuspendLayout();
            this.tableLayoutPanelPickingList.SuspendLayout();
            this.tableLayoutPanelTransferReelInformation.SuspendLayout();
            this.tableLayoutPanelReelTowerStates.SuspendLayout();
            this.tableLayoutPanelReelTower1.SuspendLayout();
            this.tableLayoutPanelReelTower2.SuspendLayout();
            this.tableLayoutPanelReelTower3.SuspendLayout();
            this.tableLayoutPanelReelTower4.SuspendLayout();
            this.tabPageVision.SuspendLayout();
            this.tabPageMaintenance.SuspendLayout();
            this.groupBoxVisionAndLight.SuspendLayout();
            this.tableLayoutPanelVisionAndLight.SuspendLayout();
            this.groupBoxDigitalIo.SuspendLayout();
            this.tabPageConfig.SuspendLayout();
            this.groupBoxVisionProcessImage.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxModel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanelModelVisionProperties.SuspendLayout();
            this.groupBoxDelayProperties.SuspendLayout();
            this.tableLayoutPanelModelDelayProperties.SuspendLayout();
            this.groupBoxTimeoutProperties.SuspendLayout();
            this.tableLayoutPanelTimeout.SuspendLayout();
            this.groupBoxNetworkSetting.SuspendLayout();
            this.tableLayoutPanelNetwork.SuspendLayout();
            this.groupBoxGuiSettings.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabPageLog.SuspendLayout();
            this.tabControlLogPage.SuspendLayout();
            this.tabPageReelTowerLog.SuspendLayout();
            this.tabPageRobotLog.SuspendLayout();
            this.tabPageAlarmHistory.SuspendLayout();
            this.tableLayoutPanelMainFrame.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelCurrentRecipe
            // 
            this.labelCurrentRecipe.BackColor = System.Drawing.SystemColors.Window;
            this.labelCurrentRecipe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCurrentRecipe.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentRecipe.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelCurrentRecipe.Location = new System.Drawing.Point(3, 3);
            this.labelCurrentRecipe.Margin = new System.Windows.Forms.Padding(3);
            this.labelCurrentRecipe.Name = "labelCurrentRecipe";
            this.tableLayoutPanelTitle.SetRowSpan(this.labelCurrentRecipe, 2);
            this.labelCurrentRecipe.Size = new System.Drawing.Size(454, 74);
            this.labelCurrentRecipe.TabIndex = 45;
            this.labelCurrentRecipe.Text = "REEL STORAGE HANDLER";
            this.labelCurrentRecipe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelOperationState
            // 
            this.labelOperationState.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelOperationState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOperationState.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOperationState.ForeColor = System.Drawing.SystemColors.Window;
            this.labelOperationState.Location = new System.Drawing.Point(463, 3);
            this.labelOperationState.Margin = new System.Windows.Forms.Padding(3);
            this.labelOperationState.Name = "labelOperationState";
            this.tableLayoutPanelTitle.SetRowSpan(this.labelOperationState, 2);
            this.labelOperationState.Size = new System.Drawing.Size(454, 74);
            this.labelOperationState.TabIndex = 168;
            this.labelOperationState.Text = "PAUSE";
            this.labelOperationState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonExit
            // 
            this.buttonExit.BackColor = System.Drawing.Color.LightCoral;
            this.buttonExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonExit.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.buttonExit.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.buttonExit.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.buttonExit.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonExit.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExit.ForeColor = System.Drawing.SystemColors.Window;
            this.buttonExit.Location = new System.Drawing.Point(1820, 0);
            this.buttonExit.Margin = new System.Windows.Forms.Padding(0);
            this.buttonExit.Name = "buttonExit";
            this.tableLayoutPanelTitle.SetRowSpan(this.buttonExit, 2);
            this.buttonExit.Size = new System.Drawing.Size(100, 80);
            this.buttonExit.TabIndex = 107;
            this.buttonExit.Text = "EXIT";
            this.buttonExit.UseVisualStyleBackColor = false;
            this.buttonExit.Click += new System.EventHandler(this.OnClickButtonExit);
            // 
            // buttonReset
            // 
            this.buttonReset.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonReset.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.buttonReset.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.buttonReset.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.buttonReset.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonReset.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonReset.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.buttonReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonReset.Location = new System.Drawing.Point(3, 163);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(295, 74);
            this.buttonReset.TabIndex = 107;
            this.buttonReset.Text = "RESET";
            this.buttonReset.UseVisualStyleBackColor = false;
            this.buttonReset.Click += new System.EventHandler(this.OnClickButtonReset);
            // 
            // labelCycleLoTowerInput
            // 
            this.labelCycleLoTowerInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.labelCycleLoTowerInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelCycleLoTowerInput.Font = new System.Drawing.Font("굴림", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelCycleLoTowerInput.ForeColor = System.Drawing.Color.LightCyan;
            this.labelCycleLoTowerInput.Location = new System.Drawing.Point(-3, 608);
            this.labelCycleLoTowerInput.Name = "labelCycleLoTowerInput";
            this.labelCycleLoTowerInput.Size = new System.Drawing.Size(311, 32);
            this.labelCycleLoTowerInput.TabIndex = 207;
            this.labelCycleLoTowerInput.Text = "24:24:66.999";
            this.labelCycleLoTowerInput.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelAverageLoTowerInput
            // 
            this.labelAverageLoTowerInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.labelAverageLoTowerInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelAverageLoTowerInput.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelAverageLoTowerInput.ForeColor = System.Drawing.Color.LightCyan;
            this.labelAverageLoTowerInput.Location = new System.Drawing.Point(157, 608);
            this.labelAverageLoTowerInput.Name = "labelAverageLoTowerInput";
            this.labelAverageLoTowerInput.Size = new System.Drawing.Size(150, 32);
            this.labelAverageLoTowerInput.TabIndex = 206;
            this.labelAverageLoTowerInput.Text = "24:24:66.999";
            this.labelAverageLoTowerInput.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelAverageLoTowerInput.Visible = false;
            // 
            // label55
            // 
            this.label55.BackColor = System.Drawing.Color.DimGray;
            this.label55.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label55.Location = new System.Drawing.Point(-3, 570);
            this.label55.Name = "label55";
            this.label55.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label55.Size = new System.Drawing.Size(311, 28);
            this.label55.TabIndex = 197;
            this.label55.Text = "Tower Input Cycle ";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMobileRobotState
            // 
            this.labelMobileRobotState.BackColor = System.Drawing.SystemColors.Window;
            this.labelMobileRobotState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMobileRobotState.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMobileRobotState.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelMobileRobotState.Location = new System.Drawing.Point(1523, 43);
            this.labelMobileRobotState.Margin = new System.Windows.Forms.Padding(3);
            this.labelMobileRobotState.Name = "labelMobileRobotState";
            this.labelMobileRobotState.Size = new System.Drawing.Size(94, 34);
            this.labelMobileRobotState.TabIndex = 94;
            this.labelMobileRobotState.Text = "IDLE";
            this.labelMobileRobotState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRobotState
            // 
            this.labelRobotState.BackColor = System.Drawing.SystemColors.Window;
            this.labelRobotState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRobotState.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRobotState.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelRobotState.Location = new System.Drawing.Point(1623, 43);
            this.labelRobotState.Margin = new System.Windows.Forms.Padding(3);
            this.labelRobotState.Name = "labelRobotState";
            this.labelRobotState.Size = new System.Drawing.Size(94, 34);
            this.labelRobotState.TabIndex = 94;
            this.labelRobotState.Text = "IDLE";
            this.labelRobotState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRobot
            // 
            this.labelRobot.BackColor = System.Drawing.SystemColors.HotTrack;
            this.labelRobot.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRobot.ForeColor = System.Drawing.SystemColors.Window;
            this.labelRobot.Location = new System.Drawing.Point(1623, 3);
            this.labelRobot.Margin = new System.Windows.Forms.Padding(3);
            this.labelRobot.Name = "labelRobot";
            this.labelRobot.Size = new System.Drawing.Size(94, 34);
            this.labelRobot.TabIndex = 129;
            this.labelRobot.Text = "ROBOT";
            this.labelRobot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMobileRobotCommunication
            // 
            this.labelMobileRobotCommunication.BackColor = System.Drawing.SystemColors.HotTrack;
            this.labelMobileRobotCommunication.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelMobileRobotCommunication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMobileRobotCommunication.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMobileRobotCommunication.ForeColor = System.Drawing.SystemColors.Window;
            this.labelMobileRobotCommunication.Location = new System.Drawing.Point(1123, 3);
            this.labelMobileRobotCommunication.Margin = new System.Windows.Forms.Padding(3);
            this.labelMobileRobotCommunication.Name = "labelMobileRobotCommunication";
            this.labelMobileRobotCommunication.Size = new System.Drawing.Size(194, 34);
            this.labelMobileRobotCommunication.TabIndex = 129;
            this.labelMobileRobotCommunication.Text = "MRBT COM.";
            this.labelMobileRobotCommunication.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelMobileRobotCommunication.DoubleClick += new System.EventHandler(this.OnDoubleClickMrbtCommunication);
            // 
            // buttonStop
            // 
            this.buttonStop.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tableLayoutPanelControlMenu.SetColumnSpan(this.buttonStop, 3);
            this.buttonStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStop.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.buttonStop.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.buttonStop.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.buttonStop.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonStop.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStop.ForeColor = System.Drawing.Color.Red;
            this.buttonStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonStop.Location = new System.Drawing.Point(3, 83);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(295, 74);
            this.buttonStop.TabIndex = 108;
            this.buttonStop.Text = "STOP";
            this.buttonStop.UseVisualStyleBackColor = false;
            this.buttonStop.Click += new System.EventHandler(this.OnClickButtonStop);
            // 
            // buttonStart
            // 
            this.buttonStart.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStart.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.buttonStart.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.buttonStart.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.buttonStart.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonStart.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStart.ForeColor = System.Drawing.Color.Green;
            this.buttonStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonStart.Location = new System.Drawing.Point(3, 3);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(295, 74);
            this.buttonStart.TabIndex = 218;
            this.buttonStart.Text = "START";
            this.buttonStart.UseVisualStyleBackColor = false;
            this.buttonStart.Click += new System.EventHandler(this.OnClickButtonStart);
            // 
            // tableLayoutPanelTitle
            // 
            this.tableLayoutPanelTitle.ColumnCount = 9;
            this.tableLayoutPanelMainFrame.SetColumnSpan(this.tableLayoutPanelTitle, 3);
            this.tableLayoutPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelTitle.Controls.Add(this.labelVision, 7, 0);
            this.tableLayoutPanelTitle.Controls.Add(this.buttonVisionStart, 7, 1);
            this.tableLayoutPanelTitle.Controls.Add(this.labelOperationState, 1, 0);
            this.tableLayoutPanelTitle.Controls.Add(this.labelCurrentRecipe, 0, 0);
            this.tableLayoutPanelTitle.Controls.Add(this.labelRobot, 6, 0);
            this.tableLayoutPanelTitle.Controls.Add(this.buttonExit, 8, 0);
            this.tableLayoutPanelTitle.Controls.Add(this.labelMobileRobotCommunication, 3, 0);
            this.tableLayoutPanelTitle.Controls.Add(this.labelRobotState, 6, 1);
            this.tableLayoutPanelTitle.Controls.Add(this.labelMobileRobotState, 5, 1);
            this.tableLayoutPanelTitle.Controls.Add(this.labelRobotCommunicationState, 4, 0);
            this.tableLayoutPanelTitle.Controls.Add(this.labelMobileRobot, 5, 0);
            this.tableLayoutPanelTitle.Controls.Add(this.labelReelTowerCommunicationStateValue, 2, 1);
            this.tableLayoutPanelTitle.Controls.Add(this.labelReelTowerCommunicationState, 2, 0);
            this.tableLayoutPanelTitle.Controls.Add(this.labelMobileRobotCommunicationStateValue, 3, 1);
            this.tableLayoutPanelTitle.Controls.Add(this.labelRobotCommunicationStateValue, 4, 1);
            this.tableLayoutPanelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTitle.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelTitle.Name = "tableLayoutPanelTitle";
            this.tableLayoutPanelTitle.RowCount = 2;
            this.tableLayoutPanelTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelTitle.Size = new System.Drawing.Size(1920, 80);
            this.tableLayoutPanelTitle.TabIndex = 221;
            // 
            // labelVision
            // 
            this.labelVision.BackColor = System.Drawing.SystemColors.HotTrack;
            this.labelVision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelVision.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVision.ForeColor = System.Drawing.SystemColors.Window;
            this.labelVision.Location = new System.Drawing.Point(1723, 3);
            this.labelVision.Margin = new System.Windows.Forms.Padding(3);
            this.labelVision.Name = "labelVision";
            this.labelVision.Size = new System.Drawing.Size(94, 34);
            this.labelVision.TabIndex = 252;
            this.labelVision.Text = "VISION";
            this.labelVision.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonVisionStart
            // 
            this.buttonVisionStart.BackColor = System.Drawing.SystemColors.Window;
            this.buttonVisionStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonVisionStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonVisionStart.FlatAppearance.BorderSize = 0;
            this.buttonVisionStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonVisionStart.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonVisionStart.Location = new System.Drawing.Point(1723, 43);
            this.buttonVisionStart.Name = "buttonVisionStart";
            this.buttonVisionStart.Size = new System.Drawing.Size(94, 34);
            this.buttonVisionStart.TabIndex = 252;
            this.buttonVisionStart.Text = "STOP";
            this.buttonVisionStart.UseVisualStyleBackColor = false;
            this.buttonVisionStart.TextChanged += new System.EventHandler(this.TextChangedButtonStartVision);
            this.buttonVisionStart.Click += new System.EventHandler(this.OnClickButtionStartVision);
            // 
            // labelRobotCommunicationState
            // 
            this.labelRobotCommunicationState.BackColor = System.Drawing.SystemColors.HotTrack;
            this.labelRobotCommunicationState.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelRobotCommunicationState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRobotCommunicationState.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRobotCommunicationState.ForeColor = System.Drawing.SystemColors.Window;
            this.labelRobotCommunicationState.Location = new System.Drawing.Point(1323, 3);
            this.labelRobotCommunicationState.Margin = new System.Windows.Forms.Padding(3);
            this.labelRobotCommunicationState.Name = "labelRobotCommunicationState";
            this.labelRobotCommunicationState.Size = new System.Drawing.Size(194, 34);
            this.labelRobotCommunicationState.TabIndex = 169;
            this.labelRobotCommunicationState.Text = "ROBOT COM.";
            this.labelRobotCommunicationState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelRobotCommunicationState.DoubleClick += new System.EventHandler(this.OnDoubleClickRobotSequenceManager);
            // 
            // labelMobileRobot
            // 
            this.labelMobileRobot.BackColor = System.Drawing.SystemColors.HotTrack;
            this.labelMobileRobot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMobileRobot.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMobileRobot.ForeColor = System.Drawing.SystemColors.Window;
            this.labelMobileRobot.Location = new System.Drawing.Point(1523, 3);
            this.labelMobileRobot.Margin = new System.Windows.Forms.Padding(3);
            this.labelMobileRobot.Name = "labelMobileRobot";
            this.labelMobileRobot.Size = new System.Drawing.Size(94, 34);
            this.labelMobileRobot.TabIndex = 170;
            this.labelMobileRobot.Text = "MRBT";
            this.labelMobileRobot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReelTowerCommunicationStateValue
            // 
            this.labelReelTowerCommunicationStateValue.BackColor = System.Drawing.SystemColors.Window;
            this.labelReelTowerCommunicationStateValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReelTowerCommunicationStateValue.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReelTowerCommunicationStateValue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelReelTowerCommunicationStateValue.Location = new System.Drawing.Point(923, 43);
            this.labelReelTowerCommunicationStateValue.Margin = new System.Windows.Forms.Padding(3);
            this.labelReelTowerCommunicationStateValue.Name = "labelReelTowerCommunicationStateValue";
            this.labelReelTowerCommunicationStateValue.Size = new System.Drawing.Size(194, 34);
            this.labelReelTowerCommunicationStateValue.TabIndex = 93;
            this.labelReelTowerCommunicationStateValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReelTowerCommunicationState
            // 
            this.labelReelTowerCommunicationState.BackColor = System.Drawing.SystemColors.HotTrack;
            this.labelReelTowerCommunicationState.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelReelTowerCommunicationState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReelTowerCommunicationState.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReelTowerCommunicationState.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReelTowerCommunicationState.Location = new System.Drawing.Point(923, 3);
            this.labelReelTowerCommunicationState.Margin = new System.Windows.Forms.Padding(3);
            this.labelReelTowerCommunicationState.Name = "labelReelTowerCommunicationState";
            this.labelReelTowerCommunicationState.Size = new System.Drawing.Size(194, 34);
            this.labelReelTowerCommunicationState.TabIndex = 171;
            this.labelReelTowerCommunicationState.Text = "REEL TWR COM.";
            this.labelReelTowerCommunicationState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelReelTowerCommunicationState.DoubleClick += new System.EventHandler(this.OnDoubleClickReelTowerManager);
            // 
            // labelMobileRobotCommunicationStateValue
            // 
            this.labelMobileRobotCommunicationStateValue.BackColor = System.Drawing.SystemColors.Window;
            this.labelMobileRobotCommunicationStateValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMobileRobotCommunicationStateValue.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMobileRobotCommunicationStateValue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelMobileRobotCommunicationStateValue.Location = new System.Drawing.Point(1123, 43);
            this.labelMobileRobotCommunicationStateValue.Margin = new System.Windows.Forms.Padding(3);
            this.labelMobileRobotCommunicationStateValue.Name = "labelMobileRobotCommunicationStateValue";
            this.labelMobileRobotCommunicationStateValue.Size = new System.Drawing.Size(194, 34);
            this.labelMobileRobotCommunicationStateValue.TabIndex = 94;
            this.labelMobileRobotCommunicationStateValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRobotCommunicationStateValue
            // 
            this.labelRobotCommunicationStateValue.BackColor = System.Drawing.SystemColors.Window;
            this.labelRobotCommunicationStateValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRobotCommunicationStateValue.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRobotCommunicationStateValue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelRobotCommunicationStateValue.Location = new System.Drawing.Point(1323, 43);
            this.labelRobotCommunicationStateValue.Margin = new System.Windows.Forms.Padding(3);
            this.labelRobotCommunicationStateValue.Name = "labelRobotCommunicationStateValue";
            this.labelRobotCommunicationStateValue.Size = new System.Drawing.Size(194, 34);
            this.labelRobotCommunicationStateValue.TabIndex = 94;
            this.labelRobotCommunicationStateValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanelControlMenu
            // 
            this.tableLayoutPanelControlMenu.ColumnCount = 1;
            this.tableLayoutPanelControlMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelControlMenu.Controls.Add(this.labelApplicationVersion, 0, 4);
            this.tableLayoutPanelControlMenu.Controls.Add(this.buttonInitialize, 0, 3);
            this.tableLayoutPanelControlMenu.Controls.Add(this.buttonStart, 0, 0);
            this.tableLayoutPanelControlMenu.Controls.Add(this.buttonStop, 0, 1);
            this.tableLayoutPanelControlMenu.Controls.Add(this.buttonReset, 0, 2);
            this.tableLayoutPanelControlMenu.Controls.Add(this.groupBoxRuntimeTrace, 0, 6);
            this.tableLayoutPanelControlMenu.Controls.Add(this.groupBoxPerformance, 0, 5);
            this.tableLayoutPanelControlMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelControlMenu.Location = new System.Drawing.Point(1619, 80);
            this.tableLayoutPanelControlMenu.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelControlMenu.Name = "tableLayoutPanelControlMenu";
            this.tableLayoutPanelControlMenu.RowCount = 7;
            this.tableLayoutPanelMainFrame.SetRowSpan(this.tableLayoutPanelControlMenu, 2);
            this.tableLayoutPanelControlMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelControlMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelControlMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelControlMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelControlMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelControlMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelControlMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelControlMenu.Size = new System.Drawing.Size(301, 981);
            this.tableLayoutPanelControlMenu.TabIndex = 222;
            // 
            // labelApplicationVersion
            // 
            this.labelApplicationVersion.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelApplicationVersion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelApplicationVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelApplicationVersion.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelApplicationVersion.ForeColor = System.Drawing.SystemColors.Window;
            this.labelApplicationVersion.Location = new System.Drawing.Point(8, 328);
            this.labelApplicationVersion.Margin = new System.Windows.Forms.Padding(8);
            this.labelApplicationVersion.Name = "labelApplicationVersion";
            this.labelApplicationVersion.Size = new System.Drawing.Size(285, 24);
            this.labelApplicationVersion.TabIndex = 239;
            this.labelApplicationVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonInitialize
            // 
            this.buttonInitialize.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonInitialize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonInitialize.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.buttonInitialize.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.buttonInitialize.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.buttonInitialize.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonInitialize.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonInitialize.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonInitialize.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonInitialize.Location = new System.Drawing.Point(3, 243);
            this.buttonInitialize.Name = "buttonInitialize";
            this.buttonInitialize.Size = new System.Drawing.Size(295, 74);
            this.buttonInitialize.TabIndex = 221;
            this.buttonInitialize.Text = "INITIALIZE";
            this.buttonInitialize.UseVisualStyleBackColor = false;
            this.buttonInitialize.Click += new System.EventHandler(this.OnClickButtonInitialize);
            // 
            // groupBoxRuntimeTrace
            // 
            this.groupBoxRuntimeTrace.Controls.Add(this.tableLayoutPanelRuntimeTrace);
            this.groupBoxRuntimeTrace.Controls.Add(this.label54);
            this.groupBoxRuntimeTrace.Controls.Add(this.label56);
            this.groupBoxRuntimeTrace.Controls.Add(this.label57);
            this.groupBoxRuntimeTrace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxRuntimeTrace.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxRuntimeTrace.Location = new System.Drawing.Point(3, 673);
            this.groupBoxRuntimeTrace.Name = "groupBoxRuntimeTrace";
            this.groupBoxRuntimeTrace.Size = new System.Drawing.Size(295, 305);
            this.groupBoxRuntimeTrace.TabIndex = 224;
            this.groupBoxRuntimeTrace.TabStop = false;
            this.groupBoxRuntimeTrace.Text = " RUNTIME TRACE ";
            // 
            // tableLayoutPanelRuntimeTrace
            // 
            this.tableLayoutPanelRuntimeTrace.ColumnCount = 2;
            this.tableLayoutPanelRuntimeTrace.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62F));
            this.tableLayoutPanelRuntimeTrace.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.tableLayoutPanelRuntimeTrace.Controls.Add(this.labelLastCycleTime, 1, 8);
            this.tableLayoutPanelRuntimeTrace.Controls.Add(this.labelRobotStep, 0, 0);
            this.tableLayoutPanelRuntimeTrace.Controls.Add(this.labelRobotStepValue, 0, 1);
            this.tableLayoutPanelRuntimeTrace.Controls.Add(this.labelMobileRobotStep, 0, 2);
            this.tableLayoutPanelRuntimeTrace.Controls.Add(this.labelTransferStateRSValue, 0, 7);
            this.tableLayoutPanelRuntimeTrace.Controls.Add(this.labelReelTowerStep, 0, 4);
            this.tableLayoutPanelRuntimeTrace.Controls.Add(this.labelMobileRobotStepValue, 0, 3);
            this.tableLayoutPanelRuntimeTrace.Controls.Add(this.labelReelTowerStepValue, 0, 5);
            this.tableLayoutPanelRuntimeTrace.Controls.Add(this.labelTransferModeRSValue, 0, 6);
            this.tableLayoutPanelRuntimeTrace.Controls.Add(this.labelLastCommandRSValue, 0, 8);
            this.tableLayoutPanelRuntimeTrace.Controls.Add(this.labelNextWaypointRSValue, 1, 6);
            this.tableLayoutPanelRuntimeTrace.Controls.Add(this.labelCurrentWaypointRSValue, 1, 7);
            this.tableLayoutPanelRuntimeTrace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRuntimeTrace.Location = new System.Drawing.Point(3, 28);
            this.tableLayoutPanelRuntimeTrace.Name = "tableLayoutPanelRuntimeTrace";
            this.tableLayoutPanelRuntimeTrace.RowCount = 9;
            this.tableLayoutPanelRuntimeTrace.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelRuntimeTrace.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelRuntimeTrace.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelRuntimeTrace.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelRuntimeTrace.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelRuntimeTrace.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelRuntimeTrace.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelRuntimeTrace.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelRuntimeTrace.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelRuntimeTrace.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRuntimeTrace.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRuntimeTrace.Size = new System.Drawing.Size(289, 274);
            this.tableLayoutPanelRuntimeTrace.TabIndex = 208;
            // 
            // labelLastCycleTime
            // 
            this.labelLastCycleTime.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelLastCycleTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelLastCycleTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLastCycleTime.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLastCycleTime.ForeColor = System.Drawing.SystemColors.Window;
            this.labelLastCycleTime.Location = new System.Drawing.Point(179, 240);
            this.labelLastCycleTime.Margin = new System.Windows.Forms.Padding(0);
            this.labelLastCycleTime.Name = "labelLastCycleTime";
            this.labelLastCycleTime.Size = new System.Drawing.Size(110, 34);
            this.labelLastCycleTime.TabIndex = 11;
            this.labelLastCycleTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRobotStep
            // 
            this.labelRobotStep.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelRobotStep.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanelRuntimeTrace.SetColumnSpan(this.labelRobotStep, 2);
            this.labelRobotStep.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelRobotStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRobotStep.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRobotStep.ForeColor = System.Drawing.SystemColors.Window;
            this.labelRobotStep.Location = new System.Drawing.Point(0, 0);
            this.labelRobotStep.Margin = new System.Windows.Forms.Padding(0);
            this.labelRobotStep.Name = "labelRobotStep";
            this.labelRobotStep.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelRobotStep.Size = new System.Drawing.Size(289, 30);
            this.labelRobotStep.TabIndex = 224;
            this.labelRobotStep.Text = "ROBOT STEP";
            this.labelRobotStep.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelRobotStepValue
            // 
            this.labelRobotStepValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelRobotStepValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanelRuntimeTrace.SetColumnSpan(this.labelRobotStepValue, 2);
            this.labelRobotStepValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRobotStepValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRobotStepValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelRobotStepValue.Location = new System.Drawing.Point(0, 30);
            this.labelRobotStepValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelRobotStepValue.Name = "labelRobotStepValue";
            this.labelRobotStepValue.Size = new System.Drawing.Size(289, 30);
            this.labelRobotStepValue.TabIndex = 225;
            this.labelRobotStepValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMobileRobotStep
            // 
            this.labelMobileRobotStep.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelMobileRobotStep.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanelRuntimeTrace.SetColumnSpan(this.labelMobileRobotStep, 2);
            this.labelMobileRobotStep.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelMobileRobotStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMobileRobotStep.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMobileRobotStep.ForeColor = System.Drawing.SystemColors.Window;
            this.labelMobileRobotStep.Location = new System.Drawing.Point(0, 60);
            this.labelMobileRobotStep.Margin = new System.Windows.Forms.Padding(0);
            this.labelMobileRobotStep.Name = "labelMobileRobotStep";
            this.labelMobileRobotStep.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelMobileRobotStep.Size = new System.Drawing.Size(289, 30);
            this.labelMobileRobotStep.TabIndex = 231;
            this.labelMobileRobotStep.Text = "MOBILE STEP";
            this.labelMobileRobotStep.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTransferStateRSValue
            // 
            this.labelTransferStateRSValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelTransferStateRSValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTransferStateRSValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTransferStateRSValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTransferStateRSValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelTransferStateRSValue.Location = new System.Drawing.Point(0, 210);
            this.labelTransferStateRSValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTransferStateRSValue.Name = "labelTransferStateRSValue";
            this.labelTransferStateRSValue.Size = new System.Drawing.Size(179, 30);
            this.labelTransferStateRSValue.TabIndex = 7;
            this.labelTransferStateRSValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReelTowerStep
            // 
            this.labelReelTowerStep.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelReelTowerStep.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanelRuntimeTrace.SetColumnSpan(this.labelReelTowerStep, 2);
            this.labelReelTowerStep.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelReelTowerStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReelTowerStep.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReelTowerStep.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReelTowerStep.Location = new System.Drawing.Point(0, 120);
            this.labelReelTowerStep.Margin = new System.Windows.Forms.Padding(0);
            this.labelReelTowerStep.Name = "labelReelTowerStep";
            this.labelReelTowerStep.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelReelTowerStep.Size = new System.Drawing.Size(289, 30);
            this.labelReelTowerStep.TabIndex = 237;
            this.labelReelTowerStep.Text = "TOWER STEP";
            this.labelReelTowerStep.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMobileRobotStepValue
            // 
            this.labelMobileRobotStepValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelMobileRobotStepValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanelRuntimeTrace.SetColumnSpan(this.labelMobileRobotStepValue, 2);
            this.labelMobileRobotStepValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMobileRobotStepValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMobileRobotStepValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelMobileRobotStepValue.Location = new System.Drawing.Point(0, 90);
            this.labelMobileRobotStepValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelMobileRobotStepValue.Name = "labelMobileRobotStepValue";
            this.labelMobileRobotStepValue.Size = new System.Drawing.Size(289, 30);
            this.labelMobileRobotStepValue.TabIndex = 232;
            this.labelMobileRobotStepValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReelTowerStepValue
            // 
            this.labelReelTowerStepValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelReelTowerStepValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanelRuntimeTrace.SetColumnSpan(this.labelReelTowerStepValue, 2);
            this.labelReelTowerStepValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReelTowerStepValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReelTowerStepValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReelTowerStepValue.Location = new System.Drawing.Point(0, 150);
            this.labelReelTowerStepValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelReelTowerStepValue.Name = "labelReelTowerStepValue";
            this.labelReelTowerStepValue.Size = new System.Drawing.Size(289, 30);
            this.labelReelTowerStepValue.TabIndex = 238;
            this.labelReelTowerStepValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTransferModeRSValue
            // 
            this.labelTransferModeRSValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelTransferModeRSValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTransferModeRSValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTransferModeRSValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTransferModeRSValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelTransferModeRSValue.Location = new System.Drawing.Point(0, 180);
            this.labelTransferModeRSValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTransferModeRSValue.Name = "labelTransferModeRSValue";
            this.labelTransferModeRSValue.Size = new System.Drawing.Size(179, 30);
            this.labelTransferModeRSValue.TabIndex = 6;
            this.labelTransferModeRSValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLastCommandRSValue
            // 
            this.labelLastCommandRSValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelLastCommandRSValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelLastCommandRSValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLastCommandRSValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLastCommandRSValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelLastCommandRSValue.Location = new System.Drawing.Point(0, 240);
            this.labelLastCommandRSValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelLastCommandRSValue.Name = "labelLastCommandRSValue";
            this.labelLastCommandRSValue.Size = new System.Drawing.Size(179, 34);
            this.labelLastCommandRSValue.TabIndex = 8;
            this.labelLastCommandRSValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelNextWaypointRSValue
            // 
            this.labelNextWaypointRSValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelNextWaypointRSValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelNextWaypointRSValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNextWaypointRSValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNextWaypointRSValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelNextWaypointRSValue.Location = new System.Drawing.Point(179, 180);
            this.labelNextWaypointRSValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelNextWaypointRSValue.Name = "labelNextWaypointRSValue";
            this.labelNextWaypointRSValue.Size = new System.Drawing.Size(110, 30);
            this.labelNextWaypointRSValue.TabIndex = 11;
            this.labelNextWaypointRSValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelCurrentWaypointRSValue
            // 
            this.labelCurrentWaypointRSValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelCurrentWaypointRSValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelCurrentWaypointRSValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCurrentWaypointRSValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentWaypointRSValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelCurrentWaypointRSValue.Location = new System.Drawing.Point(179, 210);
            this.labelCurrentWaypointRSValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelCurrentWaypointRSValue.Name = "labelCurrentWaypointRSValue";
            this.labelCurrentWaypointRSValue.Size = new System.Drawing.Size(110, 30);
            this.labelCurrentWaypointRSValue.TabIndex = 10;
            this.labelCurrentWaypointRSValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label54
            // 
            this.label54.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.label54.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label54.Font = new System.Drawing.Font("굴림", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label54.ForeColor = System.Drawing.Color.LightCyan;
            this.label54.Location = new System.Drawing.Point(-3, 608);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(311, 32);
            this.label54.TabIndex = 207;
            this.label54.Text = "24:24:66.999";
            this.label54.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label56
            // 
            this.label56.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.label56.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label56.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label56.ForeColor = System.Drawing.Color.LightCyan;
            this.label56.Location = new System.Drawing.Point(157, 608);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(150, 32);
            this.label56.TabIndex = 206;
            this.label56.Text = "24:24:66.999";
            this.label56.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label56.Visible = false;
            // 
            // label57
            // 
            this.label57.BackColor = System.Drawing.Color.DimGray;
            this.label57.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label57.Location = new System.Drawing.Point(-3, 570);
            this.label57.Name = "label57";
            this.label57.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label57.Size = new System.Drawing.Size(311, 28);
            this.label57.TabIndex = 197;
            this.label57.Text = "Tower Input Cycle ";
            this.label57.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBoxPerformance
            // 
            this.groupBoxPerformance.Controls.Add(this.tableLayoutPanelPerformance);
            this.groupBoxPerformance.Controls.Add(this.labelCycleLoTowerInput);
            this.groupBoxPerformance.Controls.Add(this.labelAverageLoTowerInput);
            this.groupBoxPerformance.Controls.Add(this.label55);
            this.groupBoxPerformance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxPerformance.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxPerformance.Location = new System.Drawing.Point(3, 363);
            this.groupBoxPerformance.Name = "groupBoxPerformance";
            this.groupBoxPerformance.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxPerformance.Size = new System.Drawing.Size(295, 304);
            this.groupBoxPerformance.TabIndex = 223;
            this.groupBoxPerformance.TabStop = false;
            this.groupBoxPerformance.Text = " PERFORMANCE ";
            // 
            // tableLayoutPanelPerformance
            // 
            this.tableLayoutPanelPerformance.ColumnCount = 2;
            this.tableLayoutPanelPerformance.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62F));
            this.tableLayoutPanelPerformance.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.tableLayoutPanelPerformance.Controls.Add(this.labelVisionDecodeError, 0, 8);
            this.tableLayoutPanelPerformance.Controls.Add(this.labelVisionDecodeErrorValue, 1, 8);
            this.tableLayoutPanelPerformance.Controls.Add(this.labelVisionAlignError, 0, 7);
            this.tableLayoutPanelPerformance.Controls.Add(this.labelVisionAlignErrorValue, 1, 7);
            this.tableLayoutPanelPerformance.Controls.Add(this.labelReturnErrorCount, 0, 6);
            this.tableLayoutPanelPerformance.Controls.Add(this.labelReturnErrorCountValue, 1, 6);
            this.tableLayoutPanelPerformance.Controls.Add(this.labelUnloadErrorCount, 0, 5);
            this.tableLayoutPanelPerformance.Controls.Add(this.labelUnloadErrorCountValue, 1, 5);
            this.tableLayoutPanelPerformance.Controls.Add(this.labelLoadErrorCount, 0, 4);
            this.tableLayoutPanelPerformance.Controls.Add(this.labelLoadErrorCountValue, 1, 4);
            this.tableLayoutPanelPerformance.Controls.Add(this.labelTotalReturn, 0, 3);
            this.tableLayoutPanelPerformance.Controls.Add(this.labelTotalReturnReelCountValue, 1, 3);
            this.tableLayoutPanelPerformance.Controls.Add(this.labelTotalUnloadReelCount, 0, 2);
            this.tableLayoutPanelPerformance.Controls.Add(this.labelTotalUnloadReelCountValue, 1, 2);
            this.tableLayoutPanelPerformance.Controls.Add(this.labelTotalLoadReelCount, 0, 1);
            this.tableLayoutPanelPerformance.Controls.Add(this.labelElapsedValue, 1, 0);
            this.tableLayoutPanelPerformance.Controls.Add(this.labelElapsed, 0, 0);
            this.tableLayoutPanelPerformance.Controls.Add(this.labelTotalLoadReelCountValue, 1, 1);
            this.tableLayoutPanelPerformance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelPerformance.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanelPerformance.Name = "tableLayoutPanelPerformance";
            this.tableLayoutPanelPerformance.RowCount = 9;
            this.tableLayoutPanelPerformance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelPerformance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelPerformance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelPerformance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelPerformance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelPerformance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelPerformance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelPerformance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelPerformance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelPerformance.Size = new System.Drawing.Size(295, 279);
            this.tableLayoutPanelPerformance.TabIndex = 208;
            // 
            // labelVisionDecodeError
            // 
            this.labelVisionDecodeError.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelVisionDecodeError.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelVisionDecodeError.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelVisionDecodeError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelVisionDecodeError.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVisionDecodeError.ForeColor = System.Drawing.SystemColors.Window;
            this.labelVisionDecodeError.Location = new System.Drawing.Point(0, 248);
            this.labelVisionDecodeError.Margin = new System.Windows.Forms.Padding(0);
            this.labelVisionDecodeError.Name = "labelVisionDecodeError";
            this.labelVisionDecodeError.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelVisionDecodeError.Size = new System.Drawing.Size(182, 31);
            this.labelVisionDecodeError.TabIndex = 239;
            this.labelVisionDecodeError.Text = "DECODE ERROR";
            this.labelVisionDecodeError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelVisionDecodeErrorValue
            // 
            this.labelVisionDecodeErrorValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelVisionDecodeErrorValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelVisionDecodeErrorValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelVisionDecodeErrorValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVisionDecodeErrorValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelVisionDecodeErrorValue.Location = new System.Drawing.Point(182, 248);
            this.labelVisionDecodeErrorValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelVisionDecodeErrorValue.Name = "labelVisionDecodeErrorValue";
            this.labelVisionDecodeErrorValue.Size = new System.Drawing.Size(113, 31);
            this.labelVisionDecodeErrorValue.TabIndex = 240;
            this.labelVisionDecodeErrorValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelVisionAlignError
            // 
            this.labelVisionAlignError.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelVisionAlignError.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelVisionAlignError.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelVisionAlignError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelVisionAlignError.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVisionAlignError.ForeColor = System.Drawing.SystemColors.Window;
            this.labelVisionAlignError.Location = new System.Drawing.Point(0, 217);
            this.labelVisionAlignError.Margin = new System.Windows.Forms.Padding(0);
            this.labelVisionAlignError.Name = "labelVisionAlignError";
            this.labelVisionAlignError.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelVisionAlignError.Size = new System.Drawing.Size(182, 31);
            this.labelVisionAlignError.TabIndex = 237;
            this.labelVisionAlignError.Text = "ALIGN ERROR";
            this.labelVisionAlignError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelVisionAlignErrorValue
            // 
            this.labelVisionAlignErrorValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelVisionAlignErrorValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelVisionAlignErrorValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelVisionAlignErrorValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVisionAlignErrorValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelVisionAlignErrorValue.Location = new System.Drawing.Point(182, 217);
            this.labelVisionAlignErrorValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelVisionAlignErrorValue.Name = "labelVisionAlignErrorValue";
            this.labelVisionAlignErrorValue.Size = new System.Drawing.Size(113, 31);
            this.labelVisionAlignErrorValue.TabIndex = 238;
            this.labelVisionAlignErrorValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReturnErrorCount
            // 
            this.labelReturnErrorCount.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelReturnErrorCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelReturnErrorCount.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelReturnErrorCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReturnErrorCount.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReturnErrorCount.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReturnErrorCount.Location = new System.Drawing.Point(0, 186);
            this.labelReturnErrorCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelReturnErrorCount.Name = "labelReturnErrorCount";
            this.labelReturnErrorCount.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelReturnErrorCount.Size = new System.Drawing.Size(182, 31);
            this.labelReturnErrorCount.TabIndex = 235;
            this.labelReturnErrorCount.Text = "RETURN ERROR";
            this.labelReturnErrorCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelReturnErrorCountValue
            // 
            this.labelReturnErrorCountValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelReturnErrorCountValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelReturnErrorCountValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReturnErrorCountValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReturnErrorCountValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReturnErrorCountValue.Location = new System.Drawing.Point(182, 186);
            this.labelReturnErrorCountValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelReturnErrorCountValue.Name = "labelReturnErrorCountValue";
            this.labelReturnErrorCountValue.Size = new System.Drawing.Size(113, 31);
            this.labelReturnErrorCountValue.TabIndex = 236;
            this.labelReturnErrorCountValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelUnloadErrorCount
            // 
            this.labelUnloadErrorCount.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelUnloadErrorCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelUnloadErrorCount.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelUnloadErrorCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUnloadErrorCount.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUnloadErrorCount.ForeColor = System.Drawing.SystemColors.Window;
            this.labelUnloadErrorCount.Location = new System.Drawing.Point(0, 155);
            this.labelUnloadErrorCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelUnloadErrorCount.Name = "labelUnloadErrorCount";
            this.labelUnloadErrorCount.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelUnloadErrorCount.Size = new System.Drawing.Size(182, 31);
            this.labelUnloadErrorCount.TabIndex = 233;
            this.labelUnloadErrorCount.Text = "UNLOAD ERROR";
            this.labelUnloadErrorCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelUnloadErrorCountValue
            // 
            this.labelUnloadErrorCountValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelUnloadErrorCountValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelUnloadErrorCountValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUnloadErrorCountValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUnloadErrorCountValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelUnloadErrorCountValue.Location = new System.Drawing.Point(182, 155);
            this.labelUnloadErrorCountValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelUnloadErrorCountValue.Name = "labelUnloadErrorCountValue";
            this.labelUnloadErrorCountValue.Size = new System.Drawing.Size(113, 31);
            this.labelUnloadErrorCountValue.TabIndex = 234;
            this.labelUnloadErrorCountValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLoadErrorCount
            // 
            this.labelLoadErrorCount.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelLoadErrorCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelLoadErrorCount.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelLoadErrorCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLoadErrorCount.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLoadErrorCount.ForeColor = System.Drawing.SystemColors.Window;
            this.labelLoadErrorCount.Location = new System.Drawing.Point(0, 124);
            this.labelLoadErrorCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelLoadErrorCount.Name = "labelLoadErrorCount";
            this.labelLoadErrorCount.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelLoadErrorCount.Size = new System.Drawing.Size(182, 31);
            this.labelLoadErrorCount.TabIndex = 231;
            this.labelLoadErrorCount.Text = "LOAD ERROR";
            this.labelLoadErrorCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelLoadErrorCountValue
            // 
            this.labelLoadErrorCountValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelLoadErrorCountValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelLoadErrorCountValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLoadErrorCountValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLoadErrorCountValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelLoadErrorCountValue.Location = new System.Drawing.Point(182, 124);
            this.labelLoadErrorCountValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelLoadErrorCountValue.Name = "labelLoadErrorCountValue";
            this.labelLoadErrorCountValue.Size = new System.Drawing.Size(113, 31);
            this.labelLoadErrorCountValue.TabIndex = 232;
            this.labelLoadErrorCountValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTotalReturn
            // 
            this.labelTotalReturn.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelTotalReturn.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTotalReturn.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelTotalReturn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTotalReturn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalReturn.ForeColor = System.Drawing.SystemColors.Window;
            this.labelTotalReturn.Location = new System.Drawing.Point(0, 93);
            this.labelTotalReturn.Margin = new System.Windows.Forms.Padding(0);
            this.labelTotalReturn.Name = "labelTotalReturn";
            this.labelTotalReturn.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelTotalReturn.Size = new System.Drawing.Size(182, 31);
            this.labelTotalReturn.TabIndex = 229;
            this.labelTotalReturn.Text = "TOTAL RETURN";
            this.labelTotalReturn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTotalReturnReelCountValue
            // 
            this.labelTotalReturnReelCountValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelTotalReturnReelCountValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTotalReturnReelCountValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTotalReturnReelCountValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalReturnReelCountValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelTotalReturnReelCountValue.Location = new System.Drawing.Point(182, 93);
            this.labelTotalReturnReelCountValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTotalReturnReelCountValue.Name = "labelTotalReturnReelCountValue";
            this.labelTotalReturnReelCountValue.Padding = new System.Windows.Forms.Padding(8);
            this.labelTotalReturnReelCountValue.Size = new System.Drawing.Size(113, 31);
            this.labelTotalReturnReelCountValue.TabIndex = 230;
            this.labelTotalReturnReelCountValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTotalUnloadReelCount
            // 
            this.labelTotalUnloadReelCount.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelTotalUnloadReelCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTotalUnloadReelCount.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelTotalUnloadReelCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTotalUnloadReelCount.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalUnloadReelCount.ForeColor = System.Drawing.SystemColors.Window;
            this.labelTotalUnloadReelCount.Location = new System.Drawing.Point(0, 62);
            this.labelTotalUnloadReelCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelTotalUnloadReelCount.Name = "labelTotalUnloadReelCount";
            this.labelTotalUnloadReelCount.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelTotalUnloadReelCount.Size = new System.Drawing.Size(182, 31);
            this.labelTotalUnloadReelCount.TabIndex = 227;
            this.labelTotalUnloadReelCount.Text = "TOTAL UNLOAD";
            this.labelTotalUnloadReelCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTotalUnloadReelCountValue
            // 
            this.labelTotalUnloadReelCountValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelTotalUnloadReelCountValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTotalUnloadReelCountValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTotalUnloadReelCountValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalUnloadReelCountValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelTotalUnloadReelCountValue.Location = new System.Drawing.Point(182, 62);
            this.labelTotalUnloadReelCountValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTotalUnloadReelCountValue.Name = "labelTotalUnloadReelCountValue";
            this.labelTotalUnloadReelCountValue.Size = new System.Drawing.Size(113, 31);
            this.labelTotalUnloadReelCountValue.TabIndex = 228;
            this.labelTotalUnloadReelCountValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTotalLoadReelCount
            // 
            this.labelTotalLoadReelCount.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelTotalLoadReelCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTotalLoadReelCount.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelTotalLoadReelCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTotalLoadReelCount.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalLoadReelCount.ForeColor = System.Drawing.SystemColors.Window;
            this.labelTotalLoadReelCount.Location = new System.Drawing.Point(0, 31);
            this.labelTotalLoadReelCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelTotalLoadReelCount.Name = "labelTotalLoadReelCount";
            this.labelTotalLoadReelCount.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelTotalLoadReelCount.Size = new System.Drawing.Size(182, 31);
            this.labelTotalLoadReelCount.TabIndex = 225;
            this.labelTotalLoadReelCount.Text = "TOTAL LOAD";
            this.labelTotalLoadReelCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelElapsedValue
            // 
            this.labelElapsedValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelElapsedValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelElapsedValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelElapsedValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelElapsedValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelElapsedValue.Location = new System.Drawing.Point(182, 0);
            this.labelElapsedValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelElapsedValue.Name = "labelElapsedValue";
            this.labelElapsedValue.Size = new System.Drawing.Size(113, 31);
            this.labelElapsedValue.TabIndex = 225;
            this.labelElapsedValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelElapsed
            // 
            this.labelElapsed.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelElapsed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelElapsed.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelElapsed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelElapsed.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelElapsed.ForeColor = System.Drawing.SystemColors.Window;
            this.labelElapsed.Location = new System.Drawing.Point(0, 0);
            this.labelElapsed.Margin = new System.Windows.Forms.Padding(0);
            this.labelElapsed.Name = "labelElapsed";
            this.labelElapsed.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelElapsed.Size = new System.Drawing.Size(182, 31);
            this.labelElapsed.TabIndex = 224;
            this.labelElapsed.Text = "ELAPSED";
            this.labelElapsed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTotalLoadReelCountValue
            // 
            this.labelTotalLoadReelCountValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelTotalLoadReelCountValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTotalLoadReelCountValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTotalLoadReelCountValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalLoadReelCountValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelTotalLoadReelCountValue.Location = new System.Drawing.Point(182, 31);
            this.labelTotalLoadReelCountValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelTotalLoadReelCountValue.Name = "labelTotalLoadReelCountValue";
            this.labelTotalLoadReelCountValue.Size = new System.Drawing.Size(113, 31);
            this.labelTotalLoadReelCountValue.TabIndex = 226;
            this.labelTotalLoadReelCountValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControlMain
            // 
            this.tabControlMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tableLayoutPanelMainFrame.SetColumnSpan(this.tabControlMain, 2);
            this.tabControlMain.Controls.Add(this.tabPageOperation);
            this.tabControlMain.Controls.Add(this.tabPageVision);
            this.tabControlMain.Controls.Add(this.tabPageMaintenance);
            this.tabControlMain.Controls.Add(this.tabPageConfig);
            this.tabControlMain.Controls.Add(this.tabPageLog);
            this.tabControlMain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControlMain.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlMain.ItemSize = new System.Drawing.Size(140, 40);
            this.tabControlMain.Location = new System.Drawing.Point(3, 85);
            this.tabControlMain.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.Padding = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMainFrame.SetRowSpan(this.tabControlMain, 2);
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1613, 973);
            this.tabControlMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControlMain.TabIndex = 223;
            this.tabControlMain.TabStop = false;
            this.tabControlMain.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnTabControlDrawItem);
            // 
            // tabPageOperation
            // 
            this.tabPageOperation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPageOperation.Controls.Add(this.buttonShowQueue);
            this.tabPageOperation.Controls.Add(this.groupBoxAnimation);
            this.tabPageOperation.Controls.Add(this.groupBoxUnloadReel);
            this.tabPageOperation.Controls.Add(this.groupBoxCartInOut);
            this.tabPageOperation.Controls.Add(this.groupBoxLoadReel);
            this.tabPageOperation.Controls.Add(this.tableLayoutPanelPickingList);
            this.tabPageOperation.Controls.Add(this.labelPickingList);
            this.tabPageOperation.Controls.Add(this.tableLayoutPanelTransferReelInformation);
            this.tabPageOperation.Controls.Add(this.labelTransferReelInformation);
            this.tabPageOperation.Controls.Add(this.labelReelTower);
            this.tabPageOperation.Controls.Add(this.tableLayoutPanelReelTowerStates);
            this.tabPageOperation.Controls.Add(this.lblURClientConnectState);
            this.tabPageOperation.Controls.Add(this.btnClientConnect);
            this.tabPageOperation.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageOperation.Location = new System.Drawing.Point(4, 44);
            this.tabPageOperation.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageOperation.Name = "tabPageOperation";
            this.tabPageOperation.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOperation.Size = new System.Drawing.Size(1605, 925);
            this.tabPageOperation.TabIndex = 0;
            this.tabPageOperation.Text = "OPERATION";
            this.tabPageOperation.UseVisualStyleBackColor = true;
            // 
            // buttonShowQueue
            // 
            this.buttonShowQueue.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonShowQueue.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.buttonShowQueue.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonShowQueue.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.buttonShowQueue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonShowQueue.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonShowQueue.ForeColor = System.Drawing.SystemColors.Window;
            this.buttonShowQueue.Location = new System.Drawing.Point(754, 377);
            this.buttonShowQueue.Margin = new System.Windows.Forms.Padding(0);
            this.buttonShowQueue.Name = "buttonShowQueue";
            this.buttonShowQueue.Size = new System.Drawing.Size(208, 38);
            this.buttonShowQueue.TabIndex = 221;
            this.buttonShowQueue.Text = "QUEUE";
            this.buttonShowQueue.UseVisualStyleBackColor = false;
            this.buttonShowQueue.Click += new System.EventHandler(this.OnClickQueuedList);
            // 
            // groupBoxAnimation
            // 
            this.groupBoxAnimation.Location = new System.Drawing.Point(6, 376);
            this.groupBoxAnimation.Name = "groupBoxAnimation";
            this.groupBoxAnimation.Size = new System.Drawing.Size(742, 560);
            this.groupBoxAnimation.TabIndex = 252;
            this.groupBoxAnimation.TabStop = false;
            this.groupBoxAnimation.Text = " ROBOT DISPLAY ";
            // 
            // groupBoxUnloadReel
            // 
            this.groupBoxUnloadReel.Controls.Add(this.controlStatusLabelCompleteUnload);
            this.groupBoxUnloadReel.Controls.Add(this.controlStatusLabelPutUnloadReelToOutput);
            this.groupBoxUnloadReel.Controls.Add(this.controlStatusLabelTakeReelFromTower);
            this.groupBoxUnloadReel.Controls.Add(this.controlStatusLabelMoveToTower);
            this.groupBoxUnloadReel.Controls.Add(this.controlStatusLabelPickList);
            this.groupBoxUnloadReel.Location = new System.Drawing.Point(6, 250);
            this.groupBoxUnloadReel.Name = "groupBoxUnloadReel";
            this.groupBoxUnloadReel.Size = new System.Drawing.Size(742, 120);
            this.groupBoxUnloadReel.TabIndex = 251;
            this.groupBoxUnloadReel.TabStop = false;
            this.groupBoxUnloadReel.Text = " UNLOAD REEL ";
            // 
            // controlStatusLabelCompleteUnload
            // 
            this.controlStatusLabelCompleteUnload.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelCompleteUnload.Location = new System.Drawing.Point(493, 28);
            this.controlStatusLabelCompleteUnload.Margin = new System.Windows.Forms.Padding(0);
            this.controlStatusLabelCompleteUnload.Name = "controlStatusLabelCompleteUnload";
            this.controlStatusLabelCompleteUnload.Size = new System.Drawing.Size(120, 80);
            this.controlStatusLabelCompleteUnload.StatusFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelCompleteUnload.StatusText = "COMPLETE UNLOAD";
            this.controlStatusLabelCompleteUnload.TabIndex = 232;
            this.controlStatusLabelCompleteUnload.ValueFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelCompleteUnload.ValueText = "-";
            // 
            // controlStatusLabelPutUnloadReelToOutput
            // 
            this.controlStatusLabelPutUnloadReelToOutput.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelPutUnloadReelToOutput.Location = new System.Drawing.Point(373, 28);
            this.controlStatusLabelPutUnloadReelToOutput.Margin = new System.Windows.Forms.Padding(0);
            this.controlStatusLabelPutUnloadReelToOutput.Name = "controlStatusLabelPutUnloadReelToOutput";
            this.controlStatusLabelPutUnloadReelToOutput.Size = new System.Drawing.Size(120, 80);
            this.controlStatusLabelPutUnloadReelToOutput.StatusFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelPutUnloadReelToOutput.StatusText = "PUT TO OUTPUT";
            this.controlStatusLabelPutUnloadReelToOutput.TabIndex = 231;
            this.controlStatusLabelPutUnloadReelToOutput.ValueFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelPutUnloadReelToOutput.ValueText = "-";
            // 
            // controlStatusLabelTakeReelFromTower
            // 
            this.controlStatusLabelTakeReelFromTower.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelTakeReelFromTower.Location = new System.Drawing.Point(253, 28);
            this.controlStatusLabelTakeReelFromTower.Margin = new System.Windows.Forms.Padding(0);
            this.controlStatusLabelTakeReelFromTower.Name = "controlStatusLabelTakeReelFromTower";
            this.controlStatusLabelTakeReelFromTower.Size = new System.Drawing.Size(120, 80);
            this.controlStatusLabelTakeReelFromTower.StatusFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelTakeReelFromTower.StatusText = "TAKE REEL";
            this.controlStatusLabelTakeReelFromTower.TabIndex = 230;
            this.controlStatusLabelTakeReelFromTower.ValueFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelTakeReelFromTower.ValueText = "-";
            // 
            // controlStatusLabelMoveToTower
            // 
            this.controlStatusLabelMoveToTower.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelMoveToTower.Location = new System.Drawing.Point(133, 28);
            this.controlStatusLabelMoveToTower.Margin = new System.Windows.Forms.Padding(0);
            this.controlStatusLabelMoveToTower.Name = "controlStatusLabelMoveToTower";
            this.controlStatusLabelMoveToTower.Size = new System.Drawing.Size(120, 80);
            this.controlStatusLabelMoveToTower.StatusFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelMoveToTower.StatusText = "MOVE TO TOWER";
            this.controlStatusLabelMoveToTower.TabIndex = 229;
            this.controlStatusLabelMoveToTower.ValueFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelMoveToTower.ValueText = "-";
            // 
            // controlStatusLabelPickList
            // 
            this.controlStatusLabelPickList.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelPickList.Location = new System.Drawing.Point(13, 28);
            this.controlStatusLabelPickList.Margin = new System.Windows.Forms.Padding(0);
            this.controlStatusLabelPickList.Name = "controlStatusLabelPickList";
            this.controlStatusLabelPickList.Size = new System.Drawing.Size(120, 80);
            this.controlStatusLabelPickList.StatusFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelPickList.StatusText = "PICK LIST";
            this.controlStatusLabelPickList.TabIndex = 228;
            this.controlStatusLabelPickList.ValueFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelPickList.ValueText = "-";
            // 
            // groupBoxCartInOut
            // 
            this.groupBoxCartInOut.Controls.Add(this.controlStatusLabelCartInOut);
            this.groupBoxCartInOut.Controls.Add(this.controlStatusLabelMrbtDocking);
            this.groupBoxCartInOut.Controls.Add(this.controlStatusLabelMrbtMove);
            this.groupBoxCartInOut.Controls.Add(this.buttonDock);
            this.groupBoxCartInOut.Controls.Add(this.buttonMobileRobotControllerUsage);
            this.groupBoxCartInOut.Location = new System.Drawing.Point(6, 8);
            this.groupBoxCartInOut.Name = "groupBoxCartInOut";
            this.groupBoxCartInOut.Size = new System.Drawing.Size(742, 120);
            this.groupBoxCartInOut.TabIndex = 250;
            this.groupBoxCartInOut.TabStop = false;
            this.groupBoxCartInOut.Text = "CART IN / OUT [ REEL SIZE: X ]";
            // 
            // controlStatusLabelCartInOut
            // 
            this.controlStatusLabelCartInOut.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelCartInOut.Location = new System.Drawing.Point(253, 28);
            this.controlStatusLabelCartInOut.Margin = new System.Windows.Forms.Padding(0);
            this.controlStatusLabelCartInOut.Name = "controlStatusLabelCartInOut";
            this.controlStatusLabelCartInOut.Size = new System.Drawing.Size(120, 80);
            this.controlStatusLabelCartInOut.StatusFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelCartInOut.StatusText = "CART IN-OUT";
            this.controlStatusLabelCartInOut.TabIndex = 227;
            this.controlStatusLabelCartInOut.ValueFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelCartInOut.ValueText = "-";
            // 
            // controlStatusLabelMrbtDocking
            // 
            this.controlStatusLabelMrbtDocking.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelMrbtDocking.Location = new System.Drawing.Point(133, 28);
            this.controlStatusLabelMrbtDocking.Margin = new System.Windows.Forms.Padding(0);
            this.controlStatusLabelMrbtDocking.Name = "controlStatusLabelMrbtDocking";
            this.controlStatusLabelMrbtDocking.Size = new System.Drawing.Size(120, 80);
            this.controlStatusLabelMrbtDocking.StatusFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelMrbtDocking.StatusText = "DOCKING";
            this.controlStatusLabelMrbtDocking.TabIndex = 226;
            this.controlStatusLabelMrbtDocking.ValueFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelMrbtDocking.ValueText = "-";
            // 
            // controlStatusLabelMrbtMove
            // 
            this.controlStatusLabelMrbtMove.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelMrbtMove.Location = new System.Drawing.Point(13, 28);
            this.controlStatusLabelMrbtMove.Margin = new System.Windows.Forms.Padding(0);
            this.controlStatusLabelMrbtMove.Name = "controlStatusLabelMrbtMove";
            this.controlStatusLabelMrbtMove.Size = new System.Drawing.Size(120, 80);
            this.controlStatusLabelMrbtMove.StatusFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelMrbtMove.StatusText = "MOVE MRBT";
            this.controlStatusLabelMrbtMove.TabIndex = 225;
            this.controlStatusLabelMrbtMove.ValueFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelMrbtMove.ValueText = "-";
            // 
            // buttonDock
            // 
            this.buttonDock.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonDock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDock.ForeColor = System.Drawing.SystemColors.Window;
            this.buttonDock.Location = new System.Drawing.Point(499, 48);
            this.buttonDock.Name = "buttonDock";
            this.buttonDock.Size = new System.Drawing.Size(240, 40);
            this.buttonDock.TabIndex = 224;
            this.buttonDock.Text = "DOCK";
            this.buttonDock.UseVisualStyleBackColor = false;
            this.buttonDock.Visible = false;
            this.buttonDock.Click += new System.EventHandler(this.OnClickButtonDock);
            // 
            // buttonMobileRobotControllerUsage
            // 
            this.buttonMobileRobotControllerUsage.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonMobileRobotControllerUsage.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.buttonMobileRobotControllerUsage.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonMobileRobotControllerUsage.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.buttonMobileRobotControllerUsage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMobileRobotControllerUsage.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonMobileRobotControllerUsage.ForeColor = System.Drawing.SystemColors.Window;
            this.buttonMobileRobotControllerUsage.Location = new System.Drawing.Point(499, 2);
            this.buttonMobileRobotControllerUsage.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMobileRobotControllerUsage.Name = "buttonMobileRobotControllerUsage";
            this.buttonMobileRobotControllerUsage.Size = new System.Drawing.Size(240, 40);
            this.buttonMobileRobotControllerUsage.TabIndex = 220;
            this.buttonMobileRobotControllerUsage.Text = "MRBT AUTO";
            this.buttonMobileRobotControllerUsage.UseVisualStyleBackColor = false;
            this.buttonMobileRobotControllerUsage.Click += new System.EventHandler(this.OnClickButtonMRBTMove);
            // 
            // groupBoxLoadReel
            // 
            this.groupBoxLoadReel.Controls.Add(this.controlStatusLabelCompleteLoad);
            this.groupBoxLoadReel.Controls.Add(this.controlStatusLabelPutReel);
            this.groupBoxLoadReel.Controls.Add(this.controlStatusLabelPickup);
            this.groupBoxLoadReel.Controls.Add(this.controlStatusLabelDecodeQrBarcode);
            this.groupBoxLoadReel.Controls.Add(this.controlStatusLabelVisionAlign);
            this.groupBoxLoadReel.Controls.Add(this.controlStatusLabelCartInchCheck);
            this.groupBoxLoadReel.Location = new System.Drawing.Point(6, 129);
            this.groupBoxLoadReel.Name = "groupBoxLoadReel";
            this.groupBoxLoadReel.Size = new System.Drawing.Size(742, 120);
            this.groupBoxLoadReel.TabIndex = 249;
            this.groupBoxLoadReel.TabStop = false;
            this.groupBoxLoadReel.Text = " LOAD REEL ";
            // 
            // controlStatusLabelCompleteLoad
            // 
            this.controlStatusLabelCompleteLoad.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelCompleteLoad.Location = new System.Drawing.Point(613, 28);
            this.controlStatusLabelCompleteLoad.Margin = new System.Windows.Forms.Padding(0);
            this.controlStatusLabelCompleteLoad.Name = "controlStatusLabelCompleteLoad";
            this.controlStatusLabelCompleteLoad.Size = new System.Drawing.Size(120, 80);
            this.controlStatusLabelCompleteLoad.StatusFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelCompleteLoad.StatusText = "COMPLETE LOAD";
            this.controlStatusLabelCompleteLoad.TabIndex = 233;
            this.controlStatusLabelCompleteLoad.ValueFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelCompleteLoad.ValueText = "-";
            // 
            // controlStatusLabelPutReel
            // 
            this.controlStatusLabelPutReel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelPutReel.Location = new System.Drawing.Point(493, 28);
            this.controlStatusLabelPutReel.Margin = new System.Windows.Forms.Padding(0);
            this.controlStatusLabelPutReel.Name = "controlStatusLabelPutReel";
            this.controlStatusLabelPutReel.Size = new System.Drawing.Size(120, 80);
            this.controlStatusLabelPutReel.StatusFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelPutReel.StatusText = "PUT LOAD REEL";
            this.controlStatusLabelPutReel.TabIndex = 232;
            this.controlStatusLabelPutReel.ValueFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelPutReel.ValueText = "-";
            // 
            // controlStatusLabelPickup
            // 
            this.controlStatusLabelPickup.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelPickup.Location = new System.Drawing.Point(373, 28);
            this.controlStatusLabelPickup.Margin = new System.Windows.Forms.Padding(0);
            this.controlStatusLabelPickup.Name = "controlStatusLabelPickup";
            this.controlStatusLabelPickup.Size = new System.Drawing.Size(120, 80);
            this.controlStatusLabelPickup.StatusFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelPickup.StatusText = "PICK UP REEL";
            this.controlStatusLabelPickup.TabIndex = 231;
            this.controlStatusLabelPickup.ValueFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelPickup.ValueText = "-";
            // 
            // controlStatusLabelDecodeQrBarcode
            // 
            this.controlStatusLabelDecodeQrBarcode.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelDecodeQrBarcode.Location = new System.Drawing.Point(253, 28);
            this.controlStatusLabelDecodeQrBarcode.Margin = new System.Windows.Forms.Padding(0);
            this.controlStatusLabelDecodeQrBarcode.Name = "controlStatusLabelDecodeQrBarcode";
            this.controlStatusLabelDecodeQrBarcode.Size = new System.Drawing.Size(120, 80);
            this.controlStatusLabelDecodeQrBarcode.StatusFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelDecodeQrBarcode.StatusText = "VISION DECODE QR";
            this.controlStatusLabelDecodeQrBarcode.TabIndex = 230;
            this.controlStatusLabelDecodeQrBarcode.ValueFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelDecodeQrBarcode.ValueText = "-";
            // 
            // controlStatusLabelVisionAlign
            // 
            this.controlStatusLabelVisionAlign.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelVisionAlign.Location = new System.Drawing.Point(133, 28);
            this.controlStatusLabelVisionAlign.Margin = new System.Windows.Forms.Padding(0);
            this.controlStatusLabelVisionAlign.Name = "controlStatusLabelVisionAlign";
            this.controlStatusLabelVisionAlign.Size = new System.Drawing.Size(120, 80);
            this.controlStatusLabelVisionAlign.StatusFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelVisionAlign.StatusText = "VISION ALIGNMENT";
            this.controlStatusLabelVisionAlign.TabIndex = 229;
            this.controlStatusLabelVisionAlign.ValueFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelVisionAlign.ValueText = "-";
            // 
            // controlStatusLabelCartInchCheck
            // 
            this.controlStatusLabelCartInchCheck.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelCartInchCheck.Location = new System.Drawing.Point(13, 28);
            this.controlStatusLabelCartInchCheck.Margin = new System.Windows.Forms.Padding(0);
            this.controlStatusLabelCartInchCheck.Name = "controlStatusLabelCartInchCheck";
            this.controlStatusLabelCartInchCheck.Size = new System.Drawing.Size(120, 80);
            this.controlStatusLabelCartInchCheck.StatusFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelCartInchCheck.StatusText = "CHECK REEL SIZE";
            this.controlStatusLabelCartInchCheck.TabIndex = 228;
            this.controlStatusLabelCartInchCheck.ValueFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlStatusLabelCartInchCheck.ValueText = "-";
            // 
            // tableLayoutPanelPickingList
            // 
            this.tableLayoutPanelPickingList.BackColor = System.Drawing.SystemColors.Desktop;
            this.tableLayoutPanelPickingList.ColumnCount = 2;
            this.tableLayoutPanelPickingList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelPickingList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPickingList.Controls.Add(this.labelOutputReelDateTimeValue, 1, 3);
            this.tableLayoutPanelPickingList.Controls.Add(this.labelOutputReelDateTime, 0, 3);
            this.tableLayoutPanelPickingList.Controls.Add(this.labelOutputReelCountValue, 1, 2);
            this.tableLayoutPanelPickingList.Controls.Add(this.labelOutputLocationValue, 1, 1);
            this.tableLayoutPanelPickingList.Controls.Add(this.labelPickingIdValue, 1, 0);
            this.tableLayoutPanelPickingList.Controls.Add(this.labelPickingId, 0, 0);
            this.tableLayoutPanelPickingList.Controls.Add(this.labelOutputLocation, 0, 1);
            this.tableLayoutPanelPickingList.Controls.Add(this.labelOutputReelCount, 0, 2);
            this.tableLayoutPanelPickingList.Controls.Add(this.labelOutputReelList, 0, 4);
            this.tableLayoutPanelPickingList.Controls.Add(this.listBoxOutputReelList, 1, 4);
            this.tableLayoutPanelPickingList.Location = new System.Drawing.Point(754, 418);
            this.tableLayoutPanelPickingList.Name = "tableLayoutPanelPickingList";
            this.tableLayoutPanelPickingList.RowCount = 5;
            this.tableLayoutPanelPickingList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelPickingList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelPickingList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelPickingList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelPickingList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPickingList.Size = new System.Drawing.Size(843, 518);
            this.tableLayoutPanelPickingList.TabIndex = 222;
            // 
            // labelOutputReelDateTimeValue
            // 
            this.labelOutputReelDateTimeValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelOutputReelDateTimeValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelOutputReelDateTimeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOutputReelDateTimeValue.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOutputReelDateTimeValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelOutputReelDateTimeValue.Location = new System.Drawing.Point(100, 90);
            this.labelOutputReelDateTimeValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelOutputReelDateTimeValue.Name = "labelOutputReelDateTimeValue";
            this.labelOutputReelDateTimeValue.Size = new System.Drawing.Size(743, 30);
            this.labelOutputReelDateTimeValue.TabIndex = 252;
            this.labelOutputReelDateTimeValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelOutputReelDateTime
            // 
            this.labelOutputReelDateTime.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelOutputReelDateTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelOutputReelDateTime.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelOutputReelDateTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOutputReelDateTime.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOutputReelDateTime.ForeColor = System.Drawing.SystemColors.Window;
            this.labelOutputReelDateTime.Location = new System.Drawing.Point(0, 90);
            this.labelOutputReelDateTime.Margin = new System.Windows.Forms.Padding(0);
            this.labelOutputReelDateTime.Name = "labelOutputReelDateTime";
            this.labelOutputReelDateTime.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelOutputReelDateTime.Size = new System.Drawing.Size(100, 30);
            this.labelOutputReelDateTime.TabIndex = 252;
            this.labelOutputReelDateTime.Text = "TIME";
            this.labelOutputReelDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelOutputReelCountValue
            // 
            this.labelOutputReelCountValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelOutputReelCountValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelOutputReelCountValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOutputReelCountValue.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOutputReelCountValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelOutputReelCountValue.Location = new System.Drawing.Point(100, 60);
            this.labelOutputReelCountValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelOutputReelCountValue.Name = "labelOutputReelCountValue";
            this.labelOutputReelCountValue.Size = new System.Drawing.Size(743, 30);
            this.labelOutputReelCountValue.TabIndex = 225;
            this.labelOutputReelCountValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelOutputLocationValue
            // 
            this.labelOutputLocationValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelOutputLocationValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelOutputLocationValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOutputLocationValue.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOutputLocationValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelOutputLocationValue.Location = new System.Drawing.Point(100, 30);
            this.labelOutputLocationValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelOutputLocationValue.Name = "labelOutputLocationValue";
            this.labelOutputLocationValue.Size = new System.Drawing.Size(743, 30);
            this.labelOutputLocationValue.TabIndex = 224;
            this.labelOutputLocationValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPickingIdValue
            // 
            this.labelPickingIdValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelPickingIdValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelPickingIdValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPickingIdValue.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPickingIdValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelPickingIdValue.Location = new System.Drawing.Point(100, 0);
            this.labelPickingIdValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelPickingIdValue.Name = "labelPickingIdValue";
            this.labelPickingIdValue.Size = new System.Drawing.Size(743, 30);
            this.labelPickingIdValue.TabIndex = 223;
            this.labelPickingIdValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPickingId
            // 
            this.labelPickingId.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelPickingId.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelPickingId.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelPickingId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPickingId.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPickingId.ForeColor = System.Drawing.SystemColors.Window;
            this.labelPickingId.Location = new System.Drawing.Point(0, 0);
            this.labelPickingId.Margin = new System.Windows.Forms.Padding(0);
            this.labelPickingId.Name = "labelPickingId";
            this.labelPickingId.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelPickingId.Size = new System.Drawing.Size(100, 30);
            this.labelPickingId.TabIndex = 220;
            this.labelPickingId.Text = "ID";
            this.labelPickingId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelOutputLocation
            // 
            this.labelOutputLocation.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelOutputLocation.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelOutputLocation.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelOutputLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOutputLocation.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOutputLocation.ForeColor = System.Drawing.SystemColors.Window;
            this.labelOutputLocation.Location = new System.Drawing.Point(0, 30);
            this.labelOutputLocation.Margin = new System.Windows.Forms.Padding(0);
            this.labelOutputLocation.Name = "labelOutputLocation";
            this.labelOutputLocation.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelOutputLocation.Size = new System.Drawing.Size(100, 30);
            this.labelOutputLocation.TabIndex = 221;
            this.labelOutputLocation.Text = "TARGET";
            this.labelOutputLocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelOutputReelCount
            // 
            this.labelOutputReelCount.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelOutputReelCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelOutputReelCount.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelOutputReelCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOutputReelCount.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOutputReelCount.ForeColor = System.Drawing.SystemColors.Window;
            this.labelOutputReelCount.Location = new System.Drawing.Point(0, 60);
            this.labelOutputReelCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelOutputReelCount.Name = "labelOutputReelCount";
            this.labelOutputReelCount.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelOutputReelCount.Size = new System.Drawing.Size(100, 30);
            this.labelOutputReelCount.TabIndex = 222;
            this.labelOutputReelCount.Text = "COUNT";
            this.labelOutputReelCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelOutputReelList
            // 
            this.labelOutputReelList.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelOutputReelList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelOutputReelList.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelOutputReelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOutputReelList.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOutputReelList.ForeColor = System.Drawing.SystemColors.Window;
            this.labelOutputReelList.Location = new System.Drawing.Point(0, 120);
            this.labelOutputReelList.Margin = new System.Windows.Forms.Padding(0);
            this.labelOutputReelList.Name = "labelOutputReelList";
            this.labelOutputReelList.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelOutputReelList.Size = new System.Drawing.Size(100, 398);
            this.labelOutputReelList.TabIndex = 147;
            this.labelOutputReelList.Text = "LIST";
            this.labelOutputReelList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listBoxOutputReelList
            // 
            this.listBoxOutputReelList.BackColor = System.Drawing.SystemColors.Desktop;
            this.listBoxOutputReelList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxOutputReelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxOutputReelList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxOutputReelList.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxOutputReelList.ForeColor = System.Drawing.SystemColors.Window;
            this.listBoxOutputReelList.FormattingEnabled = true;
            this.listBoxOutputReelList.ItemHeight = 24;
            this.listBoxOutputReelList.Location = new System.Drawing.Point(100, 120);
            this.listBoxOutputReelList.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxOutputReelList.Name = "listBoxOutputReelList";
            this.listBoxOutputReelList.Size = new System.Drawing.Size(743, 398);
            this.listBoxOutputReelList.TabIndex = 226;
            this.listBoxOutputReelList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnListBoxDrawItem);
            // 
            // labelPickingList
            // 
            this.labelPickingList.BackColor = System.Drawing.SystemColors.HotTrack;
            this.labelPickingList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelPickingList.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPickingList.ForeColor = System.Drawing.SystemColors.Window;
            this.labelPickingList.Location = new System.Drawing.Point(754, 376);
            this.labelPickingList.Margin = new System.Windows.Forms.Padding(0);
            this.labelPickingList.Name = "labelPickingList";
            this.labelPickingList.Size = new System.Drawing.Size(843, 40);
            this.labelPickingList.TabIndex = 221;
            this.labelPickingList.Text = "PICKING LIST";
            this.labelPickingList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanelTransferReelInformation
            // 
            this.tableLayoutPanelTransferReelInformation.ColumnCount = 2;
            this.tableLayoutPanelTransferReelInformation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelTransferReelInformation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTransferReelInformation.Controls.Add(this.labelReelTransferStateValue, 1, 1);
            this.tableLayoutPanelTransferReelInformation.Controls.Add(this.labelReelTransferState, 0, 1);
            this.tableLayoutPanelTransferReelInformation.Controls.Add(this.labelReelTransferMode, 1, 0);
            this.tableLayoutPanelTransferReelInformation.Controls.Add(this.labelTransferMode, 0, 0);
            this.tableLayoutPanelTransferReelInformation.Controls.Add(this.labelMfg, 1, 9);
            this.tableLayoutPanelTransferReelInformation.Controls.Add(this.labelQty, 1, 8);
            this.tableLayoutPanelTransferReelInformation.Controls.Add(this.labelSupplier, 1, 7);
            this.tableLayoutPanelTransferReelInformation.Controls.Add(this.labelLotId, 1, 6);
            this.tableLayoutPanelTransferReelInformation.Controls.Add(this.labelSid, 1, 5);
            this.tableLayoutPanelTransferReelInformation.Controls.Add(this.labelUid, 1, 4);
            this.tableLayoutPanelTransferReelInformation.Controls.Add(this.labelReelTransferDestination, 1, 3);
            this.tableLayoutPanelTransferReelInformation.Controls.Add(this.labelReelTransferSource, 1, 2);
            this.tableLayoutPanelTransferReelInformation.Controls.Add(this.label47, 0, 9);
            this.tableLayoutPanelTransferReelInformation.Controls.Add(this.label31, 0, 8);
            this.tableLayoutPanelTransferReelInformation.Controls.Add(this.label30, 0, 7);
            this.tableLayoutPanelTransferReelInformation.Controls.Add(this.label29, 0, 6);
            this.tableLayoutPanelTransferReelInformation.Controls.Add(this.label26, 0, 5);
            this.tableLayoutPanelTransferReelInformation.Controls.Add(this.label27, 0, 4);
            this.tableLayoutPanelTransferReelInformation.Controls.Add(this.labelTransferDestination, 0, 3);
            this.tableLayoutPanelTransferReelInformation.Controls.Add(this.labelTrasferSource, 0, 2);
            this.tableLayoutPanelTransferReelInformation.Location = new System.Drawing.Point(1177, 51);
            this.tableLayoutPanelTransferReelInformation.Name = "tableLayoutPanelTransferReelInformation";
            this.tableLayoutPanelTransferReelInformation.RowCount = 10;
            this.tableLayoutPanelTransferReelInformation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelTransferReelInformation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelTransferReelInformation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelTransferReelInformation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelTransferReelInformation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelTransferReelInformation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelTransferReelInformation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelTransferReelInformation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelTransferReelInformation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelTransferReelInformation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelTransferReelInformation.Size = new System.Drawing.Size(420, 320);
            this.tableLayoutPanelTransferReelInformation.TabIndex = 219;
            // 
            // labelReelTransferStateValue
            // 
            this.labelReelTransferStateValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelReelTransferStateValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelReelTransferStateValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReelTransferStateValue.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReelTransferStateValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReelTransferStateValue.Location = new System.Drawing.Point(100, 32);
            this.labelReelTransferStateValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelReelTransferStateValue.Name = "labelReelTransferStateValue";
            this.labelReelTransferStateValue.Size = new System.Drawing.Size(320, 32);
            this.labelReelTransferStateValue.TabIndex = 225;
            this.labelReelTransferStateValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReelTransferState
            // 
            this.labelReelTransferState.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelReelTransferState.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelReelTransferState.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelReelTransferState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReelTransferState.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReelTransferState.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReelTransferState.Location = new System.Drawing.Point(0, 32);
            this.labelReelTransferState.Margin = new System.Windows.Forms.Padding(0);
            this.labelReelTransferState.Name = "labelReelTransferState";
            this.labelReelTransferState.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelReelTransferState.Size = new System.Drawing.Size(100, 32);
            this.labelReelTransferState.TabIndex = 223;
            this.labelReelTransferState.Text = "STATE";
            this.labelReelTransferState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelReelTransferMode
            // 
            this.labelReelTransferMode.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelReelTransferMode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelReelTransferMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReelTransferMode.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReelTransferMode.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReelTransferMode.Location = new System.Drawing.Point(100, 0);
            this.labelReelTransferMode.Margin = new System.Windows.Forms.Padding(0);
            this.labelReelTransferMode.Name = "labelReelTransferMode";
            this.labelReelTransferMode.Size = new System.Drawing.Size(320, 32);
            this.labelReelTransferMode.TabIndex = 223;
            this.labelReelTransferMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTransferMode
            // 
            this.labelTransferMode.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelTransferMode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTransferMode.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelTransferMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTransferMode.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTransferMode.ForeColor = System.Drawing.SystemColors.Window;
            this.labelTransferMode.Location = new System.Drawing.Point(0, 0);
            this.labelTransferMode.Margin = new System.Windows.Forms.Padding(0);
            this.labelTransferMode.Name = "labelTransferMode";
            this.labelTransferMode.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelTransferMode.Size = new System.Drawing.Size(100, 32);
            this.labelTransferMode.TabIndex = 220;
            this.labelTransferMode.Text = "MODE";
            this.labelTransferMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMfg
            // 
            this.labelMfg.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelMfg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelMfg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMfg.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMfg.ForeColor = System.Drawing.SystemColors.Window;
            this.labelMfg.Location = new System.Drawing.Point(100, 288);
            this.labelMfg.Margin = new System.Windows.Forms.Padding(0);
            this.labelMfg.Name = "labelMfg";
            this.labelMfg.Size = new System.Drawing.Size(320, 32);
            this.labelMfg.TabIndex = 141;
            this.labelMfg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelQty
            // 
            this.labelQty.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelQty.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelQty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelQty.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelQty.ForeColor = System.Drawing.SystemColors.Window;
            this.labelQty.Location = new System.Drawing.Point(100, 256);
            this.labelQty.Margin = new System.Windows.Forms.Padding(0);
            this.labelQty.Name = "labelQty";
            this.labelQty.Size = new System.Drawing.Size(320, 32);
            this.labelQty.TabIndex = 141;
            this.labelQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSupplier
            // 
            this.labelSupplier.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelSupplier.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSupplier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSupplier.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSupplier.ForeColor = System.Drawing.SystemColors.Window;
            this.labelSupplier.Location = new System.Drawing.Point(100, 224);
            this.labelSupplier.Margin = new System.Windows.Forms.Padding(0);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(320, 32);
            this.labelSupplier.TabIndex = 142;
            this.labelSupplier.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLotId
            // 
            this.labelLotId.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelLotId.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelLotId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLotId.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLotId.ForeColor = System.Drawing.SystemColors.Window;
            this.labelLotId.Location = new System.Drawing.Point(100, 192);
            this.labelLotId.Margin = new System.Windows.Forms.Padding(0);
            this.labelLotId.Name = "labelLotId";
            this.labelLotId.Size = new System.Drawing.Size(320, 32);
            this.labelLotId.TabIndex = 143;
            this.labelLotId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSid
            // 
            this.labelSid.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelSid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSid.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSid.ForeColor = System.Drawing.SystemColors.Window;
            this.labelSid.Location = new System.Drawing.Point(100, 160);
            this.labelSid.Margin = new System.Windows.Forms.Padding(0);
            this.labelSid.Name = "labelSid";
            this.labelSid.Size = new System.Drawing.Size(320, 32);
            this.labelSid.TabIndex = 144;
            this.labelSid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelUid
            // 
            this.labelUid.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelUid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelUid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUid.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUid.ForeColor = System.Drawing.SystemColors.Window;
            this.labelUid.Location = new System.Drawing.Point(100, 128);
            this.labelUid.Margin = new System.Windows.Forms.Padding(0);
            this.labelUid.Name = "labelUid";
            this.labelUid.Size = new System.Drawing.Size(320, 32);
            this.labelUid.TabIndex = 146;
            this.labelUid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReelTransferDestination
            // 
            this.labelReelTransferDestination.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelReelTransferDestination.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelReelTransferDestination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReelTransferDestination.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReelTransferDestination.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReelTransferDestination.Location = new System.Drawing.Point(100, 96);
            this.labelReelTransferDestination.Margin = new System.Windows.Forms.Padding(0);
            this.labelReelTransferDestination.Name = "labelReelTransferDestination";
            this.labelReelTransferDestination.Size = new System.Drawing.Size(320, 32);
            this.labelReelTransferDestination.TabIndex = 225;
            this.labelReelTransferDestination.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReelTransferSource
            // 
            this.labelReelTransferSource.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelReelTransferSource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelReelTransferSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReelTransferSource.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReelTransferSource.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReelTransferSource.Location = new System.Drawing.Point(100, 64);
            this.labelReelTransferSource.Margin = new System.Windows.Forms.Padding(0);
            this.labelReelTransferSource.Name = "labelReelTransferSource";
            this.labelReelTransferSource.Size = new System.Drawing.Size(320, 32);
            this.labelReelTransferSource.TabIndex = 224;
            this.labelReelTransferSource.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label47
            // 
            this.label47.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label47.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label47.Cursor = System.Windows.Forms.Cursors.Default;
            this.label47.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label47.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label47.ForeColor = System.Drawing.SystemColors.Window;
            this.label47.Location = new System.Drawing.Point(0, 288);
            this.label47.Margin = new System.Windows.Forms.Padding(0);
            this.label47.Name = "label47";
            this.label47.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label47.Size = new System.Drawing.Size(100, 32);
            this.label47.TabIndex = 150;
            this.label47.Text = "MFG";
            this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label31
            // 
            this.label31.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label31.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label31.Cursor = System.Windows.Forms.Cursors.Default;
            this.label31.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label31.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.ForeColor = System.Drawing.SystemColors.Window;
            this.label31.Location = new System.Drawing.Point(0, 256);
            this.label31.Margin = new System.Windows.Forms.Padding(0);
            this.label31.Name = "label31";
            this.label31.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label31.Size = new System.Drawing.Size(100, 32);
            this.label31.TabIndex = 150;
            this.label31.Text = "QTY";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label30
            // 
            this.label30.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label30.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label30.Cursor = System.Windows.Forms.Cursors.Default;
            this.label30.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label30.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.ForeColor = System.Drawing.SystemColors.Window;
            this.label30.Location = new System.Drawing.Point(0, 224);
            this.label30.Margin = new System.Windows.Forms.Padding(0);
            this.label30.Name = "label30";
            this.label30.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label30.Size = new System.Drawing.Size(100, 32);
            this.label30.TabIndex = 148;
            this.label30.Text = "SUPLR";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label29
            // 
            this.label29.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label29.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label29.Cursor = System.Windows.Forms.Cursors.Default;
            this.label29.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label29.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.ForeColor = System.Drawing.SystemColors.Window;
            this.label29.Location = new System.Drawing.Point(0, 192);
            this.label29.Margin = new System.Windows.Forms.Padding(0);
            this.label29.Name = "label29";
            this.label29.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label29.Size = new System.Drawing.Size(100, 32);
            this.label29.TabIndex = 149;
            this.label29.Text = "LOTID";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label26
            // 
            this.label26.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label26.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label26.Cursor = System.Windows.Forms.Cursors.Default;
            this.label26.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label26.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.SystemColors.Window;
            this.label26.Location = new System.Drawing.Point(0, 160);
            this.label26.Margin = new System.Windows.Forms.Padding(0);
            this.label26.Name = "label26";
            this.label26.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label26.Size = new System.Drawing.Size(100, 32);
            this.label26.TabIndex = 145;
            this.label26.Text = "SID";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label27
            // 
            this.label27.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label27.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label27.Cursor = System.Windows.Forms.Cursors.Default;
            this.label27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label27.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ForeColor = System.Drawing.SystemColors.Window;
            this.label27.Location = new System.Drawing.Point(0, 128);
            this.label27.Margin = new System.Windows.Forms.Padding(0);
            this.label27.Name = "label27";
            this.label27.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label27.Size = new System.Drawing.Size(100, 32);
            this.label27.TabIndex = 147;
            this.label27.Text = "UID";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTransferDestination
            // 
            this.labelTransferDestination.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelTransferDestination.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTransferDestination.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelTransferDestination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTransferDestination.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTransferDestination.ForeColor = System.Drawing.SystemColors.Window;
            this.labelTransferDestination.Location = new System.Drawing.Point(0, 96);
            this.labelTransferDestination.Margin = new System.Windows.Forms.Padding(0);
            this.labelTransferDestination.Name = "labelTransferDestination";
            this.labelTransferDestination.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelTransferDestination.Size = new System.Drawing.Size(100, 32);
            this.labelTransferDestination.TabIndex = 222;
            this.labelTransferDestination.Text = "TO";
            this.labelTransferDestination.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTrasferSource
            // 
            this.labelTrasferSource.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelTrasferSource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTrasferSource.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelTrasferSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTrasferSource.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTrasferSource.ForeColor = System.Drawing.SystemColors.Window;
            this.labelTrasferSource.Location = new System.Drawing.Point(0, 64);
            this.labelTrasferSource.Margin = new System.Windows.Forms.Padding(0);
            this.labelTrasferSource.Name = "labelTrasferSource";
            this.labelTrasferSource.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelTrasferSource.Size = new System.Drawing.Size(100, 32);
            this.labelTrasferSource.TabIndex = 221;
            this.labelTrasferSource.Text = "FROM";
            this.labelTrasferSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTransferReelInformation
            // 
            this.labelTransferReelInformation.BackColor = System.Drawing.SystemColors.HotTrack;
            this.labelTransferReelInformation.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTransferReelInformation.ForeColor = System.Drawing.SystemColors.Window;
            this.labelTransferReelInformation.Location = new System.Drawing.Point(1177, 8);
            this.labelTransferReelInformation.Margin = new System.Windows.Forms.Padding(0);
            this.labelTransferReelInformation.Name = "labelTransferReelInformation";
            this.labelTransferReelInformation.Size = new System.Drawing.Size(420, 40);
            this.labelTransferReelInformation.TabIndex = 218;
            this.labelTransferReelInformation.Text = "REEL INFORMATION";
            this.labelTransferReelInformation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReelTower
            // 
            this.labelReelTower.BackColor = System.Drawing.SystemColors.HotTrack;
            this.labelReelTower.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReelTower.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReelTower.Location = new System.Drawing.Point(754, 8);
            this.labelReelTower.Margin = new System.Windows.Forms.Padding(0);
            this.labelReelTower.Name = "labelReelTower";
            this.labelReelTower.Size = new System.Drawing.Size(420, 40);
            this.labelReelTower.TabIndex = 217;
            this.labelReelTower.Text = "TOWER STATES";
            this.labelReelTower.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanelReelTowerStates
            // 
            this.tableLayoutPanelReelTowerStates.ColumnCount = 3;
            this.tableLayoutPanelReelTowerStates.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelReelTowerStates.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanelReelTowerStates.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelReelTowerStates.Controls.Add(this.labelReelTowerState2, 2, 0);
            this.tableLayoutPanelReelTowerStates.Controls.Add(this.labelReelTowerState1, 0, 0);
            this.tableLayoutPanelReelTowerStates.Controls.Add(this.labelReelTowerState3, 0, 3);
            this.tableLayoutPanelReelTowerStates.Controls.Add(this.labelReelTowerState4, 2, 3);
            this.tableLayoutPanelReelTowerStates.Controls.Add(this.tableLayoutPanelReelTower1, 0, 1);
            this.tableLayoutPanelReelTowerStates.Controls.Add(this.tableLayoutPanelReelTower2, 2, 1);
            this.tableLayoutPanelReelTowerStates.Controls.Add(this.tableLayoutPanelReelTower3, 0, 4);
            this.tableLayoutPanelReelTowerStates.Controls.Add(this.tableLayoutPanelReelTower4, 2, 4);
            this.tableLayoutPanelReelTowerStates.Location = new System.Drawing.Point(755, 51);
            this.tableLayoutPanelReelTowerStates.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelReelTowerStates.Name = "tableLayoutPanelReelTowerStates";
            this.tableLayoutPanelReelTowerStates.RowCount = 5;
            this.tableLayoutPanelReelTowerStates.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelReelTowerStates.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelReelTowerStates.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanelReelTowerStates.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelReelTowerStates.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelReelTowerStates.Size = new System.Drawing.Size(420, 320);
            this.tableLayoutPanelReelTowerStates.TabIndex = 216;
            // 
            // labelReelTowerState2
            // 
            this.labelReelTowerState2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelReelTowerState2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelReelTowerState2.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelReelTowerState2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReelTowerState2.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReelTowerState2.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReelTowerState2.Location = new System.Drawing.Point(212, 0);
            this.labelReelTowerState2.Margin = new System.Windows.Forms.Padding(0);
            this.labelReelTowerState2.Name = "labelReelTowerState2";
            this.labelReelTowerState2.Size = new System.Drawing.Size(208, 30);
            this.labelReelTowerState2.TabIndex = 155;
            this.labelReelTowerState2.Text = "TOWER 2";
            this.labelReelTowerState2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReelTowerState1
            // 
            this.labelReelTowerState1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelReelTowerState1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelReelTowerState1.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelReelTowerState1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReelTowerState1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReelTowerState1.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReelTowerState1.Location = new System.Drawing.Point(0, 0);
            this.labelReelTowerState1.Margin = new System.Windows.Forms.Padding(0);
            this.labelReelTowerState1.Name = "labelReelTowerState1";
            this.labelReelTowerState1.Size = new System.Drawing.Size(207, 30);
            this.labelReelTowerState1.TabIndex = 155;
            this.labelReelTowerState1.Text = "TOWER 1";
            this.labelReelTowerState1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReelTowerState3
            // 
            this.labelReelTowerState3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelReelTowerState3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelReelTowerState3.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelReelTowerState3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReelTowerState3.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReelTowerState3.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReelTowerState3.Location = new System.Drawing.Point(0, 162);
            this.labelReelTowerState3.Margin = new System.Windows.Forms.Padding(0);
            this.labelReelTowerState3.Name = "labelReelTowerState3";
            this.labelReelTowerState3.Size = new System.Drawing.Size(207, 30);
            this.labelReelTowerState3.TabIndex = 156;
            this.labelReelTowerState3.Text = "TOWER 3";
            this.labelReelTowerState3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReelTowerState4
            // 
            this.labelReelTowerState4.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelReelTowerState4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelReelTowerState4.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelReelTowerState4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReelTowerState4.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReelTowerState4.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReelTowerState4.Location = new System.Drawing.Point(212, 162);
            this.labelReelTowerState4.Margin = new System.Windows.Forms.Padding(0);
            this.labelReelTowerState4.Name = "labelReelTowerState4";
            this.labelReelTowerState4.Size = new System.Drawing.Size(208, 30);
            this.labelReelTowerState4.TabIndex = 156;
            this.labelReelTowerState4.Text = "TOWER 4";
            this.labelReelTowerState4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanelReelTower1
            // 
            this.tableLayoutPanelReelTower1.ColumnCount = 2;
            this.tableLayoutPanelReelTower1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelReelTower1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelReelTower1.Controls.Add(this.lblTowerMode1, 1, 1);
            this.tableLayoutPanelReelTower1.Controls.Add(this.labelTowerCodeId1, 1, 3);
            this.tableLayoutPanelReelTower1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanelReelTower1.Controls.Add(this.labelDoorStateTower1, 0, 3);
            this.tableLayoutPanelReelTower1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanelReelTower1.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanelReelTower1.Controls.Add(this.labelReelTowerId1, 1, 0);
            this.tableLayoutPanelReelTower1.Controls.Add(this.lblTowerState1, 1, 2);
            this.tableLayoutPanelReelTower1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelReelTower1.Location = new System.Drawing.Point(0, 30);
            this.tableLayoutPanelReelTower1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelReelTower1.Name = "tableLayoutPanelReelTower1";
            this.tableLayoutPanelReelTower1.RowCount = 4;
            this.tableLayoutPanelReelTower1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelReelTower1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelReelTower1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelReelTower1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelReelTower1.Size = new System.Drawing.Size(207, 127);
            this.tableLayoutPanelReelTower1.TabIndex = 219;
            // 
            // lblTowerMode1
            // 
            this.lblTowerMode1.BackColor = System.Drawing.SystemColors.Desktop;
            this.lblTowerMode1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTowerMode1.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblTowerMode1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTowerMode1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTowerMode1.ForeColor = System.Drawing.SystemColors.Window;
            this.lblTowerMode1.Location = new System.Drawing.Point(100, 31);
            this.lblTowerMode1.Margin = new System.Windows.Forms.Padding(0);
            this.lblTowerMode1.Name = "lblTowerMode1";
            this.lblTowerMode1.Size = new System.Drawing.Size(107, 31);
            this.lblTowerMode1.TabIndex = 96;
            this.lblTowerMode1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTowerCodeId1
            // 
            this.labelTowerCodeId1.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelTowerCodeId1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTowerCodeId1.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelTowerCodeId1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTowerCodeId1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTowerCodeId1.ForeColor = System.Drawing.SystemColors.Window;
            this.labelTowerCodeId1.Location = new System.Drawing.Point(100, 93);
            this.labelTowerCodeId1.Margin = new System.Windows.Forms.Padding(0);
            this.labelTowerCodeId1.Name = "labelTowerCodeId1";
            this.labelTowerCodeId1.Size = new System.Drawing.Size(107, 34);
            this.labelTowerCodeId1.TabIndex = 98;
            this.labelTowerCodeId1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Cursor = System.Windows.Forms.Cursors.Default;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Window;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label3.Size = new System.Drawing.Size(100, 31);
            this.label3.TabIndex = 52;
            this.label3.Text = "ID";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDoorStateTower1
            // 
            this.labelDoorStateTower1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelDoorStateTower1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelDoorStateTower1.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelDoorStateTower1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDoorStateTower1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDoorStateTower1.ForeColor = System.Drawing.SystemColors.Window;
            this.labelDoorStateTower1.Location = new System.Drawing.Point(0, 93);
            this.labelDoorStateTower1.Margin = new System.Windows.Forms.Padding(0);
            this.labelDoorStateTower1.Name = "labelDoorStateTower1";
            this.labelDoorStateTower1.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelDoorStateTower1.Size = new System.Drawing.Size(100, 34);
            this.labelDoorStateTower1.TabIndex = 97;
            this.labelDoorStateTower1.Text = "DOOR";
            this.labelDoorStateTower1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(0, 31);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label2.Size = new System.Drawing.Size(100, 31);
            this.label2.TabIndex = 95;
            this.label2.Text = "MODE";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Cursor = System.Windows.Forms.Cursors.Default;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.Window;
            this.label5.Location = new System.Drawing.Point(0, 62);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label5.Size = new System.Drawing.Size(100, 31);
            this.label5.TabIndex = 50;
            this.label5.Text = "STATE";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelReelTowerId1
            // 
            this.labelReelTowerId1.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelReelTowerId1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelReelTowerId1.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelReelTowerId1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReelTowerId1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReelTowerId1.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReelTowerId1.Location = new System.Drawing.Point(100, 0);
            this.labelReelTowerId1.Margin = new System.Windows.Forms.Padding(0);
            this.labelReelTowerId1.Name = "labelReelTowerId1";
            this.labelReelTowerId1.Size = new System.Drawing.Size(107, 31);
            this.labelReelTowerId1.TabIndex = 77;
            this.labelReelTowerId1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTowerState1
            // 
            this.lblTowerState1.BackColor = System.Drawing.SystemColors.Desktop;
            this.lblTowerState1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTowerState1.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblTowerState1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTowerState1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTowerState1.ForeColor = System.Drawing.SystemColors.Window;
            this.lblTowerState1.Location = new System.Drawing.Point(100, 62);
            this.lblTowerState1.Margin = new System.Windows.Forms.Padding(0);
            this.lblTowerState1.Name = "lblTowerState1";
            this.lblTowerState1.Size = new System.Drawing.Size(107, 31);
            this.lblTowerState1.TabIndex = 51;
            this.lblTowerState1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanelReelTower2
            // 
            this.tableLayoutPanelReelTower2.ColumnCount = 2;
            this.tableLayoutPanelReelTower2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelReelTower2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelReelTower2.Controls.Add(this.label32, 0, 0);
            this.tableLayoutPanelReelTower2.Controls.Add(this.labelTowerCodeId2, 1, 3);
            this.tableLayoutPanelReelTower2.Controls.Add(this.labelDoorStateTower2, 0, 3);
            this.tableLayoutPanelReelTower2.Controls.Add(this.lblTowerState2, 1, 2);
            this.tableLayoutPanelReelTower2.Controls.Add(this.label44, 0, 1);
            this.tableLayoutPanelReelTower2.Controls.Add(this.label48, 0, 2);
            this.tableLayoutPanelReelTower2.Controls.Add(this.labelReelTowerId2, 1, 0);
            this.tableLayoutPanelReelTower2.Controls.Add(this.lblTowerMode2, 1, 1);
            this.tableLayoutPanelReelTower2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelReelTower2.Location = new System.Drawing.Point(212, 30);
            this.tableLayoutPanelReelTower2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelReelTower2.Name = "tableLayoutPanelReelTower2";
            this.tableLayoutPanelReelTower2.RowCount = 4;
            this.tableLayoutPanelReelTower2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelReelTower2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelReelTower2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelReelTower2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelReelTower2.Size = new System.Drawing.Size(208, 127);
            this.tableLayoutPanelReelTower2.TabIndex = 220;
            // 
            // label32
            // 
            this.label32.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label32.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label32.Cursor = System.Windows.Forms.Cursors.Default;
            this.label32.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label32.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.SystemColors.Window;
            this.label32.Location = new System.Drawing.Point(0, 0);
            this.label32.Margin = new System.Windows.Forms.Padding(0);
            this.label32.Name = "label32";
            this.label32.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label32.Size = new System.Drawing.Size(100, 31);
            this.label32.TabIndex = 52;
            this.label32.Text = "ID";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTowerCodeId2
            // 
            this.labelTowerCodeId2.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelTowerCodeId2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTowerCodeId2.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelTowerCodeId2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTowerCodeId2.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTowerCodeId2.ForeColor = System.Drawing.SystemColors.Window;
            this.labelTowerCodeId2.Location = new System.Drawing.Point(100, 93);
            this.labelTowerCodeId2.Margin = new System.Windows.Forms.Padding(0);
            this.labelTowerCodeId2.Name = "labelTowerCodeId2";
            this.labelTowerCodeId2.Size = new System.Drawing.Size(108, 34);
            this.labelTowerCodeId2.TabIndex = 98;
            this.labelTowerCodeId2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDoorStateTower2
            // 
            this.labelDoorStateTower2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelDoorStateTower2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelDoorStateTower2.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelDoorStateTower2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDoorStateTower2.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDoorStateTower2.ForeColor = System.Drawing.SystemColors.Window;
            this.labelDoorStateTower2.Location = new System.Drawing.Point(0, 93);
            this.labelDoorStateTower2.Margin = new System.Windows.Forms.Padding(0);
            this.labelDoorStateTower2.Name = "labelDoorStateTower2";
            this.labelDoorStateTower2.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelDoorStateTower2.Size = new System.Drawing.Size(100, 34);
            this.labelDoorStateTower2.TabIndex = 97;
            this.labelDoorStateTower2.Text = "DOOR";
            this.labelDoorStateTower2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTowerState2
            // 
            this.lblTowerState2.BackColor = System.Drawing.SystemColors.Desktop;
            this.lblTowerState2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTowerState2.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblTowerState2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTowerState2.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTowerState2.ForeColor = System.Drawing.SystemColors.Window;
            this.lblTowerState2.Location = new System.Drawing.Point(100, 62);
            this.lblTowerState2.Margin = new System.Windows.Forms.Padding(0);
            this.lblTowerState2.Name = "lblTowerState2";
            this.lblTowerState2.Size = new System.Drawing.Size(108, 31);
            this.lblTowerState2.TabIndex = 98;
            this.lblTowerState2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label44
            // 
            this.label44.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label44.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label44.Cursor = System.Windows.Forms.Cursors.Default;
            this.label44.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label44.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label44.ForeColor = System.Drawing.SystemColors.Window;
            this.label44.Location = new System.Drawing.Point(0, 31);
            this.label44.Margin = new System.Windows.Forms.Padding(0);
            this.label44.Name = "label44";
            this.label44.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label44.Size = new System.Drawing.Size(100, 31);
            this.label44.TabIndex = 95;
            this.label44.Text = "MODE";
            this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label48
            // 
            this.label48.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label48.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label48.Cursor = System.Windows.Forms.Cursors.Default;
            this.label48.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label48.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label48.ForeColor = System.Drawing.SystemColors.Window;
            this.label48.Location = new System.Drawing.Point(0, 62);
            this.label48.Margin = new System.Windows.Forms.Padding(0);
            this.label48.Name = "label48";
            this.label48.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label48.Size = new System.Drawing.Size(100, 31);
            this.label48.TabIndex = 50;
            this.label48.Text = "STATE";
            this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelReelTowerId2
            // 
            this.labelReelTowerId2.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelReelTowerId2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelReelTowerId2.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelReelTowerId2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReelTowerId2.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReelTowerId2.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReelTowerId2.Location = new System.Drawing.Point(100, 0);
            this.labelReelTowerId2.Margin = new System.Windows.Forms.Padding(0);
            this.labelReelTowerId2.Name = "labelReelTowerId2";
            this.labelReelTowerId2.Size = new System.Drawing.Size(108, 31);
            this.labelReelTowerId2.TabIndex = 63;
            this.labelReelTowerId2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTowerMode2
            // 
            this.lblTowerMode2.BackColor = System.Drawing.SystemColors.Desktop;
            this.lblTowerMode2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTowerMode2.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblTowerMode2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTowerMode2.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTowerMode2.ForeColor = System.Drawing.SystemColors.Window;
            this.lblTowerMode2.Location = new System.Drawing.Point(100, 31);
            this.lblTowerMode2.Margin = new System.Windows.Forms.Padding(0);
            this.lblTowerMode2.Name = "lblTowerMode2";
            this.lblTowerMode2.Size = new System.Drawing.Size(108, 31);
            this.lblTowerMode2.TabIndex = 61;
            this.lblTowerMode2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanelReelTower3
            // 
            this.tableLayoutPanelReelTower3.ColumnCount = 2;
            this.tableLayoutPanelReelTower3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelReelTower3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelReelTower3.Controls.Add(this.lblTowerState3, 1, 2);
            this.tableLayoutPanelReelTower3.Controls.Add(this.labelTowerCodeId3, 1, 3);
            this.tableLayoutPanelReelTower3.Controls.Add(this.labelReelTowerId3, 1, 0);
            this.tableLayoutPanelReelTower3.Controls.Add(this.label66, 0, 0);
            this.tableLayoutPanelReelTower3.Controls.Add(this.labelDoorStateTower3, 0, 3);
            this.tableLayoutPanelReelTower3.Controls.Add(this.label70, 0, 1);
            this.tableLayoutPanelReelTower3.Controls.Add(this.label71, 0, 2);
            this.tableLayoutPanelReelTower3.Controls.Add(this.lblTowerMode3, 1, 1);
            this.tableLayoutPanelReelTower3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelReelTower3.Location = new System.Drawing.Point(0, 192);
            this.tableLayoutPanelReelTower3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelReelTower3.Name = "tableLayoutPanelReelTower3";
            this.tableLayoutPanelReelTower3.RowCount = 4;
            this.tableLayoutPanelReelTower3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelReelTower3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelReelTower3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelReelTower3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelReelTower3.Size = new System.Drawing.Size(207, 128);
            this.tableLayoutPanelReelTower3.TabIndex = 221;
            // 
            // lblTowerState3
            // 
            this.lblTowerState3.BackColor = System.Drawing.SystemColors.Desktop;
            this.lblTowerState3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTowerState3.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblTowerState3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTowerState3.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTowerState3.ForeColor = System.Drawing.SystemColors.Window;
            this.lblTowerState3.Location = new System.Drawing.Point(100, 64);
            this.lblTowerState3.Margin = new System.Windows.Forms.Padding(0);
            this.lblTowerState3.Name = "lblTowerState3";
            this.lblTowerState3.Size = new System.Drawing.Size(107, 32);
            this.lblTowerState3.TabIndex = 100;
            this.lblTowerState3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTowerCodeId3
            // 
            this.labelTowerCodeId3.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelTowerCodeId3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTowerCodeId3.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelTowerCodeId3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTowerCodeId3.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTowerCodeId3.ForeColor = System.Drawing.SystemColors.Window;
            this.labelTowerCodeId3.Location = new System.Drawing.Point(100, 96);
            this.labelTowerCodeId3.Margin = new System.Windows.Forms.Padding(0);
            this.labelTowerCodeId3.Name = "labelTowerCodeId3";
            this.labelTowerCodeId3.Size = new System.Drawing.Size(107, 32);
            this.labelTowerCodeId3.TabIndex = 98;
            this.labelTowerCodeId3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReelTowerId3
            // 
            this.labelReelTowerId3.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelReelTowerId3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelReelTowerId3.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelReelTowerId3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReelTowerId3.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReelTowerId3.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReelTowerId3.Location = new System.Drawing.Point(100, 0);
            this.labelReelTowerId3.Margin = new System.Windows.Forms.Padding(0);
            this.labelReelTowerId3.Name = "labelReelTowerId3";
            this.labelReelTowerId3.Size = new System.Drawing.Size(107, 32);
            this.labelReelTowerId3.TabIndex = 58;
            this.labelReelTowerId3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label66
            // 
            this.label66.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label66.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label66.Cursor = System.Windows.Forms.Cursors.Default;
            this.label66.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label66.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label66.ForeColor = System.Drawing.SystemColors.Window;
            this.label66.Location = new System.Drawing.Point(0, 0);
            this.label66.Margin = new System.Windows.Forms.Padding(0);
            this.label66.Name = "label66";
            this.label66.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label66.Size = new System.Drawing.Size(100, 32);
            this.label66.TabIndex = 52;
            this.label66.Text = "ID";
            this.label66.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDoorStateTower3
            // 
            this.labelDoorStateTower3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelDoorStateTower3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelDoorStateTower3.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelDoorStateTower3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDoorStateTower3.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDoorStateTower3.ForeColor = System.Drawing.SystemColors.Window;
            this.labelDoorStateTower3.Location = new System.Drawing.Point(0, 96);
            this.labelDoorStateTower3.Margin = new System.Windows.Forms.Padding(0);
            this.labelDoorStateTower3.Name = "labelDoorStateTower3";
            this.labelDoorStateTower3.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelDoorStateTower3.Size = new System.Drawing.Size(100, 32);
            this.labelDoorStateTower3.TabIndex = 97;
            this.labelDoorStateTower3.Text = "DOOR";
            this.labelDoorStateTower3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label70
            // 
            this.label70.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label70.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label70.Cursor = System.Windows.Forms.Cursors.Default;
            this.label70.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label70.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label70.ForeColor = System.Drawing.SystemColors.Window;
            this.label70.Location = new System.Drawing.Point(0, 32);
            this.label70.Margin = new System.Windows.Forms.Padding(0);
            this.label70.Name = "label70";
            this.label70.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label70.Size = new System.Drawing.Size(100, 32);
            this.label70.TabIndex = 95;
            this.label70.Text = "MODE";
            this.label70.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label71
            // 
            this.label71.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label71.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label71.Cursor = System.Windows.Forms.Cursors.Default;
            this.label71.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label71.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label71.ForeColor = System.Drawing.SystemColors.Window;
            this.label71.Location = new System.Drawing.Point(0, 64);
            this.label71.Margin = new System.Windows.Forms.Padding(0);
            this.label71.Name = "label71";
            this.label71.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label71.Size = new System.Drawing.Size(100, 32);
            this.label71.TabIndex = 50;
            this.label71.Text = "STATE";
            this.label71.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTowerMode3
            // 
            this.lblTowerMode3.BackColor = System.Drawing.SystemColors.Desktop;
            this.lblTowerMode3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTowerMode3.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblTowerMode3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTowerMode3.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTowerMode3.ForeColor = System.Drawing.SystemColors.Window;
            this.lblTowerMode3.Location = new System.Drawing.Point(100, 32);
            this.lblTowerMode3.Margin = new System.Windows.Forms.Padding(0);
            this.lblTowerMode3.Name = "lblTowerMode3";
            this.lblTowerMode3.Size = new System.Drawing.Size(107, 32);
            this.lblTowerMode3.TabIndex = 56;
            this.lblTowerMode3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanelReelTower4
            // 
            this.tableLayoutPanelReelTower4.ColumnCount = 2;
            this.tableLayoutPanelReelTower4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelReelTower4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelReelTower4.Controls.Add(this.label76, 0, 0);
            this.tableLayoutPanelReelTower4.Controls.Add(this.labelTowerCodeId4, 1, 3);
            this.tableLayoutPanelReelTower4.Controls.Add(this.labelDoorStateTower4, 0, 3);
            this.tableLayoutPanelReelTower4.Controls.Add(this.lblTowerState4, 1, 2);
            this.tableLayoutPanelReelTower4.Controls.Add(this.label78, 0, 1);
            this.tableLayoutPanelReelTower4.Controls.Add(this.label79, 0, 2);
            this.tableLayoutPanelReelTower4.Controls.Add(this.labelReelTowerId4, 1, 0);
            this.tableLayoutPanelReelTower4.Controls.Add(this.lblTowerMode4, 1, 1);
            this.tableLayoutPanelReelTower4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelReelTower4.Location = new System.Drawing.Point(212, 192);
            this.tableLayoutPanelReelTower4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelReelTower4.Name = "tableLayoutPanelReelTower4";
            this.tableLayoutPanelReelTower4.RowCount = 4;
            this.tableLayoutPanelReelTower4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelReelTower4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelReelTower4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelReelTower4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelReelTower4.Size = new System.Drawing.Size(208, 128);
            this.tableLayoutPanelReelTower4.TabIndex = 222;
            // 
            // label76
            // 
            this.label76.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label76.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label76.Cursor = System.Windows.Forms.Cursors.Default;
            this.label76.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label76.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label76.ForeColor = System.Drawing.SystemColors.Window;
            this.label76.Location = new System.Drawing.Point(0, 0);
            this.label76.Margin = new System.Windows.Forms.Padding(0);
            this.label76.Name = "label76";
            this.label76.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label76.Size = new System.Drawing.Size(100, 32);
            this.label76.TabIndex = 52;
            this.label76.Text = "ID";
            this.label76.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTowerCodeId4
            // 
            this.labelTowerCodeId4.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelTowerCodeId4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTowerCodeId4.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelTowerCodeId4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTowerCodeId4.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTowerCodeId4.ForeColor = System.Drawing.SystemColors.Window;
            this.labelTowerCodeId4.Location = new System.Drawing.Point(100, 96);
            this.labelTowerCodeId4.Margin = new System.Windows.Forms.Padding(0);
            this.labelTowerCodeId4.Name = "labelTowerCodeId4";
            this.labelTowerCodeId4.Size = new System.Drawing.Size(108, 32);
            this.labelTowerCodeId4.TabIndex = 98;
            this.labelTowerCodeId4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDoorStateTower4
            // 
            this.labelDoorStateTower4.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelDoorStateTower4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelDoorStateTower4.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelDoorStateTower4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDoorStateTower4.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDoorStateTower4.ForeColor = System.Drawing.SystemColors.Window;
            this.labelDoorStateTower4.Location = new System.Drawing.Point(0, 96);
            this.labelDoorStateTower4.Margin = new System.Windows.Forms.Padding(0);
            this.labelDoorStateTower4.Name = "labelDoorStateTower4";
            this.labelDoorStateTower4.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.labelDoorStateTower4.Size = new System.Drawing.Size(100, 32);
            this.labelDoorStateTower4.TabIndex = 97;
            this.labelDoorStateTower4.Text = "DOOR";
            this.labelDoorStateTower4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTowerState4
            // 
            this.lblTowerState4.BackColor = System.Drawing.SystemColors.Desktop;
            this.lblTowerState4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTowerState4.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblTowerState4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTowerState4.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTowerState4.ForeColor = System.Drawing.SystemColors.Window;
            this.lblTowerState4.Location = new System.Drawing.Point(100, 64);
            this.lblTowerState4.Margin = new System.Windows.Forms.Padding(0);
            this.lblTowerState4.Name = "lblTowerState4";
            this.lblTowerState4.Size = new System.Drawing.Size(108, 32);
            this.lblTowerState4.TabIndex = 102;
            this.lblTowerState4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label78
            // 
            this.label78.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label78.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label78.Cursor = System.Windows.Forms.Cursors.Default;
            this.label78.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label78.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label78.ForeColor = System.Drawing.SystemColors.Window;
            this.label78.Location = new System.Drawing.Point(0, 32);
            this.label78.Margin = new System.Windows.Forms.Padding(0);
            this.label78.Name = "label78";
            this.label78.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label78.Size = new System.Drawing.Size(100, 32);
            this.label78.TabIndex = 95;
            this.label78.Text = "MODE";
            this.label78.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label79
            // 
            this.label79.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label79.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label79.Cursor = System.Windows.Forms.Cursors.Default;
            this.label79.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label79.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label79.ForeColor = System.Drawing.SystemColors.Window;
            this.label79.Location = new System.Drawing.Point(0, 64);
            this.label79.Margin = new System.Windows.Forms.Padding(0);
            this.label79.Name = "label79";
            this.label79.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label79.Size = new System.Drawing.Size(100, 32);
            this.label79.TabIndex = 50;
            this.label79.Text = "STATE";
            this.label79.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelReelTowerId4
            // 
            this.labelReelTowerId4.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelReelTowerId4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelReelTowerId4.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelReelTowerId4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReelTowerId4.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReelTowerId4.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReelTowerId4.Location = new System.Drawing.Point(100, 0);
            this.labelReelTowerId4.Margin = new System.Windows.Forms.Padding(0);
            this.labelReelTowerId4.Name = "labelReelTowerId4";
            this.labelReelTowerId4.Size = new System.Drawing.Size(108, 32);
            this.labelReelTowerId4.TabIndex = 68;
            this.labelReelTowerId4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTowerMode4
            // 
            this.lblTowerMode4.BackColor = System.Drawing.SystemColors.Desktop;
            this.lblTowerMode4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTowerMode4.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblTowerMode4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTowerMode4.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTowerMode4.ForeColor = System.Drawing.SystemColors.Window;
            this.lblTowerMode4.Location = new System.Drawing.Point(100, 32);
            this.lblTowerMode4.Margin = new System.Windows.Forms.Padding(0);
            this.lblTowerMode4.Name = "lblTowerMode4";
            this.lblTowerMode4.Size = new System.Drawing.Size(108, 32);
            this.lblTowerMode4.TabIndex = 66;
            this.lblTowerMode4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblURClientConnectState
            // 
            this.lblURClientConnectState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.lblURClientConnectState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblURClientConnectState.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblURClientConnectState.Location = new System.Drawing.Point(1520, 1026);
            this.lblURClientConnectState.Name = "lblURClientConnectState";
            this.lblURClientConnectState.Size = new System.Drawing.Size(77, 31);
            this.lblURClientConnectState.TabIndex = 131;
            this.lblURClientConnectState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClientConnect
            // 
            this.btnClientConnect.BackColor = System.Drawing.Color.DimGray;
            this.btnClientConnect.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnClientConnect.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.btnClientConnect.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnClientConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClientConnect.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClientConnect.Location = new System.Drawing.Point(1390, 1027);
            this.btnClientConnect.Name = "btnClientConnect";
            this.btnClientConnect.Size = new System.Drawing.Size(139, 30);
            this.btnClientConnect.TabIndex = 130;
            this.btnClientConnect.Text = "[D/B Client]";
            this.btnClientConnect.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClientConnect.UseVisualStyleBackColor = false;
            // 
            // tabPageVision
            // 
            this.tabPageVision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPageVision.Controls.Add(this.visionControl1);
            this.tabPageVision.Location = new System.Drawing.Point(4, 44);
            this.tabPageVision.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageVision.Name = "tabPageVision";
            this.tabPageVision.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageVision.Size = new System.Drawing.Size(1605, 925);
            this.tabPageVision.TabIndex = 4;
            this.tabPageVision.Text = "VISION";
            this.tabPageVision.UseVisualStyleBackColor = true;
            // 
            // visionControl1
            // 
            this.visionControl1.BlockControl = false;
            this.visionControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visionControl1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.visionControl1.Location = new System.Drawing.Point(3, 3);
            this.visionControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.visionControl1.Name = "visionControl1";
            this.visionControl1.Size = new System.Drawing.Size(1597, 917);
            this.visionControl1.TabIndex = 0;
            // 
            // tabPageMaintenance
            // 
            this.tabPageMaintenance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPageMaintenance.Controls.Add(this.groupBoxVisionAndLight);
            this.tabPageMaintenance.Controls.Add(this.groupBoxDigitalIo);
            this.tabPageMaintenance.Location = new System.Drawing.Point(4, 44);
            this.tabPageMaintenance.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageMaintenance.Name = "tabPageMaintenance";
            this.tabPageMaintenance.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMaintenance.Size = new System.Drawing.Size(1605, 925);
            this.tabPageMaintenance.TabIndex = 3;
            this.tabPageMaintenance.Text = "MAINTENANCE";
            this.tabPageMaintenance.UseVisualStyleBackColor = true;
            // 
            // groupBoxVisionAndLight
            // 
            this.groupBoxVisionAndLight.Controls.Add(this.tableLayoutPanelVisionAndLight);
            this.groupBoxVisionAndLight.Location = new System.Drawing.Point(652, 10);
            this.groupBoxVisionAndLight.Name = "groupBoxVisionAndLight";
            this.groupBoxVisionAndLight.Size = new System.Drawing.Size(494, 471);
            this.groupBoxVisionAndLight.TabIndex = 3;
            this.groupBoxVisionAndLight.TabStop = false;
            this.groupBoxVisionAndLight.Text = "VISION AND LIGHT ";
            // 
            // tableLayoutPanelVisionAndLight
            // 
            this.tableLayoutPanelVisionAndLight.ColumnCount = 2;
            this.tableLayoutPanelVisionAndLight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelVisionAndLight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelVisionAndLight.Controls.Add(this.buttonVisionRestart, 1, 6);
            this.tableLayoutPanelVisionAndLight.Controls.Add(this.labelDecodeBarcodeResult, 1, 5);
            this.tableLayoutPanelVisionAndLight.Controls.Add(this.labelAlignmentResult, 0, 5);
            this.tableLayoutPanelVisionAndLight.Controls.Add(this.buttonVisionGrabReel13, 1, 3);
            this.tableLayoutPanelVisionAndLight.Controls.Add(this.labelReel13, 1, 0);
            this.tableLayoutPanelVisionAndLight.Controls.Add(this.labelReel7, 0, 0);
            this.tableLayoutPanelVisionAndLight.Controls.Add(this.buttonLightOnReel7, 0, 1);
            this.tableLayoutPanelVisionAndLight.Controls.Add(this.buttonVisionFindCenter, 0, 4);
            this.tableLayoutPanelVisionAndLight.Controls.Add(this.buttonLightOnReel13, 1, 1);
            this.tableLayoutPanelVisionAndLight.Controls.Add(this.buttonReadBarcode, 1, 4);
            this.tableLayoutPanelVisionAndLight.Controls.Add(this.buttonLightOff, 0, 2);
            this.tableLayoutPanelVisionAndLight.Controls.Add(this.buttonVisionGrabReel7, 0, 3);
            this.tableLayoutPanelVisionAndLight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelVisionAndLight.Location = new System.Drawing.Point(3, 28);
            this.tableLayoutPanelVisionAndLight.Name = "tableLayoutPanelVisionAndLight";
            this.tableLayoutPanelVisionAndLight.RowCount = 7;
            this.tableLayoutPanelVisionAndLight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanelVisionAndLight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanelVisionAndLight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanelVisionAndLight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanelVisionAndLight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanelVisionAndLight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanelVisionAndLight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanelVisionAndLight.Size = new System.Drawing.Size(488, 440);
            this.tableLayoutPanelVisionAndLight.TabIndex = 6;
            // 
            // buttonVisionRestart
            // 
            this.buttonVisionRestart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonVisionRestart.Location = new System.Drawing.Point(247, 375);
            this.buttonVisionRestart.Name = "buttonVisionRestart";
            this.buttonVisionRestart.Size = new System.Drawing.Size(238, 62);
            this.buttonVisionRestart.TabIndex = 5;
            this.buttonVisionRestart.Text = "VISION RESTART";
            this.buttonVisionRestart.UseVisualStyleBackColor = true;
            this.buttonVisionRestart.Click += new System.EventHandler(this.OnClickVisionRestart);
            // 
            // labelDecodeBarcodeResult
            // 
            this.labelDecodeBarcodeResult.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelDecodeBarcodeResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelDecodeBarcodeResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDecodeBarcodeResult.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDecodeBarcodeResult.ForeColor = System.Drawing.SystemColors.Window;
            this.labelDecodeBarcodeResult.Location = new System.Drawing.Point(247, 313);
            this.labelDecodeBarcodeResult.Margin = new System.Windows.Forms.Padding(3);
            this.labelDecodeBarcodeResult.Name = "labelDecodeBarcodeResult";
            this.labelDecodeBarcodeResult.Size = new System.Drawing.Size(238, 56);
            this.labelDecodeBarcodeResult.TabIndex = 221;
            this.labelDecodeBarcodeResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelAlignmentResult
            // 
            this.labelAlignmentResult.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelAlignmentResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelAlignmentResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAlignmentResult.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAlignmentResult.ForeColor = System.Drawing.SystemColors.Window;
            this.labelAlignmentResult.Location = new System.Drawing.Point(3, 313);
            this.labelAlignmentResult.Margin = new System.Windows.Forms.Padding(3);
            this.labelAlignmentResult.Name = "labelAlignmentResult";
            this.labelAlignmentResult.Size = new System.Drawing.Size(238, 56);
            this.labelAlignmentResult.TabIndex = 142;
            this.labelAlignmentResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonVisionGrabReel13
            // 
            this.buttonVisionGrabReel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonVisionGrabReel13.Location = new System.Drawing.Point(247, 189);
            this.buttonVisionGrabReel13.Name = "buttonVisionGrabReel13";
            this.buttonVisionGrabReel13.Size = new System.Drawing.Size(238, 56);
            this.buttonVisionGrabReel13.TabIndex = 220;
            this.buttonVisionGrabReel13.Text = "GRAB";
            this.buttonVisionGrabReel13.UseVisualStyleBackColor = true;
            this.buttonVisionGrabReel13.Click += new System.EventHandler(this.OnClickButtonVisionGrabReel13);
            // 
            // labelReel13
            // 
            this.labelReel13.BackColor = System.Drawing.SystemColors.HotTrack;
            this.labelReel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReel13.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReel13.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReel13.Location = new System.Drawing.Point(247, 3);
            this.labelReel13.Margin = new System.Windows.Forms.Padding(3);
            this.labelReel13.Name = "labelReel13";
            this.labelReel13.Size = new System.Drawing.Size(238, 56);
            this.labelReel13.TabIndex = 219;
            this.labelReel13.Text = "REEL 13";
            this.labelReel13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReel7
            // 
            this.labelReel7.BackColor = System.Drawing.SystemColors.HotTrack;
            this.labelReel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReel7.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReel7.ForeColor = System.Drawing.SystemColors.Window;
            this.labelReel7.Location = new System.Drawing.Point(3, 3);
            this.labelReel7.Margin = new System.Windows.Forms.Padding(3);
            this.labelReel7.Name = "labelReel7";
            this.labelReel7.Size = new System.Drawing.Size(238, 56);
            this.labelReel7.TabIndex = 218;
            this.labelReel7.Text = "REEL 7";
            this.labelReel7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonLightOnReel7
            // 
            this.buttonLightOnReel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLightOnReel7.Location = new System.Drawing.Point(3, 65);
            this.buttonLightOnReel7.Name = "buttonLightOnReel7";
            this.buttonLightOnReel7.Size = new System.Drawing.Size(238, 56);
            this.buttonLightOnReel7.TabIndex = 3;
            this.buttonLightOnReel7.Text = "LIGHT ON";
            this.buttonLightOnReel7.UseVisualStyleBackColor = true;
            this.buttonLightOnReel7.Click += new System.EventHandler(this.OnClickButtonReel7LightOn);
            // 
            // buttonVisionFindCenter
            // 
            this.buttonVisionFindCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonVisionFindCenter.Location = new System.Drawing.Point(3, 251);
            this.buttonVisionFindCenter.Name = "buttonVisionFindCenter";
            this.buttonVisionFindCenter.Size = new System.Drawing.Size(238, 56);
            this.buttonVisionFindCenter.TabIndex = 5;
            this.buttonVisionFindCenter.Text = "FIND CENTER";
            this.buttonVisionFindCenter.UseVisualStyleBackColor = true;
            this.buttonVisionFindCenter.Click += new System.EventHandler(this.OnClickButtonVisionFindCenter);
            // 
            // buttonLightOnReel13
            // 
            this.buttonLightOnReel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLightOnReel13.Location = new System.Drawing.Point(247, 65);
            this.buttonLightOnReel13.Name = "buttonLightOnReel13";
            this.buttonLightOnReel13.Size = new System.Drawing.Size(238, 56);
            this.buttonLightOnReel13.TabIndex = 1;
            this.buttonLightOnReel13.Text = "LIGHT ON";
            this.buttonLightOnReel13.UseVisualStyleBackColor = true;
            this.buttonLightOnReel13.Click += new System.EventHandler(this.OnClickButtonReel13LightOn);
            // 
            // buttonReadBarcode
            // 
            this.buttonReadBarcode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonReadBarcode.Location = new System.Drawing.Point(247, 251);
            this.buttonReadBarcode.Name = "buttonReadBarcode";
            this.buttonReadBarcode.Size = new System.Drawing.Size(238, 56);
            this.buttonReadBarcode.TabIndex = 6;
            this.buttonReadBarcode.Text = "READ BARCODE";
            this.buttonReadBarcode.UseVisualStyleBackColor = true;
            this.buttonReadBarcode.Click += new System.EventHandler(this.OnClickButtonVisionReadBarcode);
            // 
            // buttonLightOff
            // 
            this.tableLayoutPanelVisionAndLight.SetColumnSpan(this.buttonLightOff, 2);
            this.buttonLightOff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLightOff.Location = new System.Drawing.Point(3, 127);
            this.buttonLightOff.Name = "buttonLightOff";
            this.buttonLightOff.Size = new System.Drawing.Size(482, 56);
            this.buttonLightOff.TabIndex = 2;
            this.buttonLightOff.Text = "LIGHT OFF";
            this.buttonLightOff.UseVisualStyleBackColor = true;
            this.buttonLightOff.Click += new System.EventHandler(this.OnClickButtonLightOff);
            // 
            // buttonVisionGrabReel7
            // 
            this.buttonVisionGrabReel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonVisionGrabReel7.Location = new System.Drawing.Point(3, 189);
            this.buttonVisionGrabReel7.Name = "buttonVisionGrabReel7";
            this.buttonVisionGrabReel7.Size = new System.Drawing.Size(238, 56);
            this.buttonVisionGrabReel7.TabIndex = 4;
            this.buttonVisionGrabReel7.Text = "GRAB";
            this.buttonVisionGrabReel7.UseVisualStyleBackColor = true;
            this.buttonVisionGrabReel7.Click += new System.EventHandler(this.OnClickButtonVisionGrabReel7);
            // 
            // groupBoxDigitalIo
            // 
            this.groupBoxDigitalIo.Controls.Add(this.controlDigitalIo1);
            this.groupBoxDigitalIo.Location = new System.Drawing.Point(6, 10);
            this.groupBoxDigitalIo.Name = "groupBoxDigitalIo";
            this.groupBoxDigitalIo.Size = new System.Drawing.Size(640, 903);
            this.groupBoxDigitalIo.TabIndex = 0;
            this.groupBoxDigitalIo.TabStop = false;
            this.groupBoxDigitalIo.Text = " DIGITAL I/O ";
            // 
            // controlDigitalIo1
            // 
            this.controlDigitalIo1.BackColor = System.Drawing.SystemColors.Control;
            this.controlDigitalIo1.Cursor = System.Windows.Forms.Cursors.Default;
            this.controlDigitalIo1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlDigitalIo1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.controlDigitalIo1.Location = new System.Drawing.Point(3, 28);
            this.controlDigitalIo1.Margin = new System.Windows.Forms.Padding(0);
            this.controlDigitalIo1.Name = "controlDigitalIo1";
            this.controlDigitalIo1.Padding = new System.Windows.Forms.Padding(8);
            this.controlDigitalIo1.Size = new System.Drawing.Size(634, 872);
            this.controlDigitalIo1.TabIndex = 0;
            // 
            // tabPageConfig
            // 
            this.tabPageConfig.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPageConfig.Controls.Add(this.buttonSaveGuiSettings);
            this.tabPageConfig.Controls.Add(this.buttonSaveModel);
            this.tabPageConfig.Controls.Add(this.groupBoxVisionProcessImage);
            this.tabPageConfig.Controls.Add(this.groupBoxModel);
            this.tabPageConfig.Controls.Add(this.buttonSaveTimeoutProperties);
            this.tabPageConfig.Controls.Add(this.buttonSaveNetworkSettings);
            this.tabPageConfig.Controls.Add(this.groupBoxTimeoutProperties);
            this.tabPageConfig.Controls.Add(this.groupBoxNetworkSetting);
            this.tabPageConfig.Controls.Add(this.groupBoxGuiSettings);
            this.tabPageConfig.Location = new System.Drawing.Point(4, 44);
            this.tabPageConfig.Name = "tabPageConfig";
            this.tabPageConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConfig.Size = new System.Drawing.Size(1605, 925);
            this.tabPageConfig.TabIndex = 2;
            this.tabPageConfig.Text = "CONFIG";
            this.tabPageConfig.UseVisualStyleBackColor = true;
            // 
            // buttonSaveGuiSettings
            // 
            this.buttonSaveGuiSettings.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSaveGuiSettings.Location = new System.Drawing.Point(1470, 4);
            this.buttonSaveGuiSettings.Name = "buttonSaveGuiSettings";
            this.buttonSaveGuiSettings.Size = new System.Drawing.Size(120, 40);
            this.buttonSaveGuiSettings.TabIndex = 230;
            this.buttonSaveGuiSettings.Text = "SAVE";
            this.buttonSaveGuiSettings.UseVisualStyleBackColor = true;
            this.buttonSaveGuiSettings.Visible = false;
            // 
            // buttonSaveModel
            // 
            this.buttonSaveModel.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSaveModel.Location = new System.Drawing.Point(457, 365);
            this.buttonSaveModel.Name = "buttonSaveModel";
            this.buttonSaveModel.Size = new System.Drawing.Size(120, 40);
            this.buttonSaveModel.TabIndex = 224;
            this.buttonSaveModel.Text = "SAVE";
            this.buttonSaveModel.UseVisualStyleBackColor = true;
            this.buttonSaveModel.Click += new System.EventHandler(this.OnClickButtonSaveModel);
            // 
            // groupBoxVisionProcessImage
            // 
            this.groupBoxVisionProcessImage.Controls.Add(this.tableLayoutPanel1);
            this.groupBoxVisionProcessImage.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxVisionProcessImage.Location = new System.Drawing.Point(590, 382);
            this.groupBoxVisionProcessImage.Name = "groupBoxVisionProcessImage";
            this.groupBoxVisionProcessImage.Size = new System.Drawing.Size(560, 204);
            this.groupBoxVisionProcessImage.TabIndex = 228;
            this.groupBoxVisionProcessImage.TabStop = false;
            this.groupBoxVisionProcessImage.Text = " IMAGE ";
            this.groupBoxVisionProcessImage.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 124F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 116F));
            this.tableLayoutPanel1.Controls.Add(this.checkBoxVisionProcessCompressImage, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.textBoxVisionProcessRejectImagePath, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxVisionProcessSaveImage, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelVisionProcessRejectImageFilePath, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelVisionProcessAcceptImagePath, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelVisionProcessImageFileExtension, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxVisionProcessImageFileExtension, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxVisionProcessImageFileNameFormat, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelVisionProcessImageFileNameFormat, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxVisionProcessAcceptImagePath, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 28);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(554, 173);
            this.tableLayoutPanel1.TabIndex = 220;
            // 
            // checkBoxVisionProcessCompressImage
            // 
            this.checkBoxVisionProcessCompressImage.AutoSize = true;
            this.checkBoxVisionProcessCompressImage.Checked = true;
            this.checkBoxVisionProcessCompressImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanel1.SetColumnSpan(this.checkBoxVisionProcessCompressImage, 2);
            this.checkBoxVisionProcessCompressImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxVisionProcessCompressImage.Location = new System.Drawing.Point(317, 139);
            this.checkBoxVisionProcessCompressImage.Name = "checkBoxVisionProcessCompressImage";
            this.checkBoxVisionProcessCompressImage.Size = new System.Drawing.Size(234, 31);
            this.checkBoxVisionProcessCompressImage.TabIndex = 227;
            this.checkBoxVisionProcessCompressImage.Text = "Image Processing Timeout";
            this.checkBoxVisionProcessCompressImage.UseVisualStyleBackColor = true;
            this.checkBoxVisionProcessCompressImage.Visible = false;
            // 
            // textBoxVisionProcessRejectImagePath
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxVisionProcessRejectImagePath, 2);
            this.textBoxVisionProcessRejectImagePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxVisionProcessRejectImagePath.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxVisionProcessRejectImagePath.Location = new System.Drawing.Point(317, 105);
            this.textBoxVisionProcessRejectImagePath.Name = "textBoxVisionProcessRejectImagePath";
            this.textBoxVisionProcessRejectImagePath.Size = new System.Drawing.Size(234, 32);
            this.textBoxVisionProcessRejectImagePath.TabIndex = 5;
            this.textBoxVisionProcessRejectImagePath.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // checkBoxVisionProcessSaveImage
            // 
            this.checkBoxVisionProcessSaveImage.AutoSize = true;
            this.checkBoxVisionProcessSaveImage.Checked = true;
            this.checkBoxVisionProcessSaveImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxVisionProcessSaveImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxVisionProcessSaveImage.Location = new System.Drawing.Point(3, 139);
            this.checkBoxVisionProcessSaveImage.Name = "checkBoxVisionProcessSaveImage";
            this.checkBoxVisionProcessSaveImage.Size = new System.Drawing.Size(308, 31);
            this.checkBoxVisionProcessSaveImage.TabIndex = 226;
            this.checkBoxVisionProcessSaveImage.Text = "SAVE IMAGE";
            this.checkBoxVisionProcessSaveImage.UseVisualStyleBackColor = true;
            this.checkBoxVisionProcessSaveImage.Visible = false;
            // 
            // labelVisionProcessRejectImageFilePath
            // 
            this.labelVisionProcessRejectImageFilePath.AutoSize = true;
            this.labelVisionProcessRejectImageFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelVisionProcessRejectImageFilePath.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVisionProcessRejectImageFilePath.Location = new System.Drawing.Point(3, 102);
            this.labelVisionProcessRejectImageFilePath.Name = "labelVisionProcessRejectImageFilePath";
            this.labelVisionProcessRejectImageFilePath.Size = new System.Drawing.Size(308, 34);
            this.labelVisionProcessRejectImageFilePath.TabIndex = 4;
            this.labelVisionProcessRejectImageFilePath.Text = "Reject image file path";
            this.labelVisionProcessRejectImageFilePath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelVisionProcessAcceptImagePath
            // 
            this.labelVisionProcessAcceptImagePath.AutoSize = true;
            this.labelVisionProcessAcceptImagePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelVisionProcessAcceptImagePath.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVisionProcessAcceptImagePath.Location = new System.Drawing.Point(3, 68);
            this.labelVisionProcessAcceptImagePath.Name = "labelVisionProcessAcceptImagePath";
            this.labelVisionProcessAcceptImagePath.Size = new System.Drawing.Size(308, 34);
            this.labelVisionProcessAcceptImagePath.TabIndex = 2;
            this.labelVisionProcessAcceptImagePath.Text = "Accept image file path";
            this.labelVisionProcessAcceptImagePath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelVisionProcessImageFileExtension
            // 
            this.labelVisionProcessImageFileExtension.AutoSize = true;
            this.labelVisionProcessImageFileExtension.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelVisionProcessImageFileExtension.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVisionProcessImageFileExtension.Location = new System.Drawing.Point(3, 0);
            this.labelVisionProcessImageFileExtension.Name = "labelVisionProcessImageFileExtension";
            this.labelVisionProcessImageFileExtension.Size = new System.Drawing.Size(308, 34);
            this.labelVisionProcessImageFileExtension.TabIndex = 0;
            this.labelVisionProcessImageFileExtension.Text = "Image file extension";
            this.labelVisionProcessImageFileExtension.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxVisionProcessImageFileExtension
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxVisionProcessImageFileExtension, 2);
            this.textBoxVisionProcessImageFileExtension.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxVisionProcessImageFileExtension.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxVisionProcessImageFileExtension.Location = new System.Drawing.Point(317, 3);
            this.textBoxVisionProcessImageFileExtension.Name = "textBoxVisionProcessImageFileExtension";
            this.textBoxVisionProcessImageFileExtension.Size = new System.Drawing.Size(234, 32);
            this.textBoxVisionProcessImageFileExtension.TabIndex = 1;
            this.textBoxVisionProcessImageFileExtension.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxVisionProcessImageFileNameFormat
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxVisionProcessImageFileNameFormat, 2);
            this.textBoxVisionProcessImageFileNameFormat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxVisionProcessImageFileNameFormat.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxVisionProcessImageFileNameFormat.Location = new System.Drawing.Point(317, 37);
            this.textBoxVisionProcessImageFileNameFormat.Name = "textBoxVisionProcessImageFileNameFormat";
            this.textBoxVisionProcessImageFileNameFormat.Size = new System.Drawing.Size(234, 32);
            this.textBoxVisionProcessImageFileNameFormat.TabIndex = 1;
            this.textBoxVisionProcessImageFileNameFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelVisionProcessImageFileNameFormat
            // 
            this.labelVisionProcessImageFileNameFormat.AutoSize = true;
            this.labelVisionProcessImageFileNameFormat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelVisionProcessImageFileNameFormat.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVisionProcessImageFileNameFormat.Location = new System.Drawing.Point(3, 34);
            this.labelVisionProcessImageFileNameFormat.Name = "labelVisionProcessImageFileNameFormat";
            this.labelVisionProcessImageFileNameFormat.Size = new System.Drawing.Size(308, 34);
            this.labelVisionProcessImageFileNameFormat.TabIndex = 0;
            this.labelVisionProcessImageFileNameFormat.Text = "Image file name format";
            this.labelVisionProcessImageFileNameFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxVisionProcessAcceptImagePath
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxVisionProcessAcceptImagePath, 2);
            this.textBoxVisionProcessAcceptImagePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxVisionProcessAcceptImagePath.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxVisionProcessAcceptImagePath.Location = new System.Drawing.Point(317, 71);
            this.textBoxVisionProcessAcceptImagePath.Name = "textBoxVisionProcessAcceptImagePath";
            this.textBoxVisionProcessAcceptImagePath.Size = new System.Drawing.Size(234, 32);
            this.textBoxVisionProcessAcceptImagePath.TabIndex = 3;
            this.textBoxVisionProcessAcceptImagePath.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBoxModel
            // 
            this.groupBoxModel.Controls.Add(this.checkBoxVisionProcessProductionMode);
            this.groupBoxModel.Controls.Add(this.groupBox2);
            this.groupBoxModel.Controls.Add(this.groupBoxDelayProperties);
            this.groupBoxModel.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxModel.Location = new System.Drawing.Point(3, 380);
            this.groupBoxModel.Name = "groupBoxModel";
            this.groupBoxModel.Size = new System.Drawing.Size(580, 401);
            this.groupBoxModel.TabIndex = 223;
            this.groupBoxModel.TabStop = false;
            this.groupBoxModel.Text = " MODEL ";
            // 
            // checkBoxVisionProcessProductionMode
            // 
            this.checkBoxVisionProcessProductionMode.AutoSize = true;
            this.checkBoxVisionProcessProductionMode.Checked = true;
            this.checkBoxVisionProcessProductionMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxVisionProcessProductionMode.Location = new System.Drawing.Point(108, 0);
            this.checkBoxVisionProcessProductionMode.Name = "checkBoxVisionProcessProductionMode";
            this.checkBoxVisionProcessProductionMode.Size = new System.Drawing.Size(282, 28);
            this.checkBoxVisionProcessProductionMode.TabIndex = 225;
            this.checkBoxVisionProcessProductionMode.Text = "USE PRODUCTION MODE";
            this.checkBoxVisionProcessProductionMode.UseVisualStyleBackColor = true;
            this.checkBoxVisionProcessProductionMode.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanelModelVisionProperties);
            this.groupBox2.Location = new System.Drawing.Point(7, 30);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(566, 212);
            this.groupBox2.TabIndex = 219;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " VISION ";
            // 
            // tableLayoutPanelModelVisionProperties
            // 
            this.tableLayoutPanelModelVisionProperties.ColumnCount = 3;
            this.tableLayoutPanelModelVisionProperties.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelModelVisionProperties.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanelModelVisionProperties.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanelModelVisionProperties.Controls.Add(this.textBoxVisioinRetryAttempts, 1, 2);
            this.tableLayoutPanelModelVisionProperties.Controls.Add(this.labelVisionAlignmentRangeLimit, 0, 0);
            this.tableLayoutPanelModelVisionProperties.Controls.Add(this.labelVisionRetryAttempts, 0, 2);
            this.tableLayoutPanelModelVisionProperties.Controls.Add(this.textBoxVisionAlignmentRangeLimitX, 1, 0);
            this.tableLayoutPanelModelVisionProperties.Controls.Add(this.textBoxVisionAlignmentRangeLimitY, 2, 0);
            this.tableLayoutPanelModelVisionProperties.Controls.Add(this.textBoxVisionFailureRetry, 1, 1);
            this.tableLayoutPanelModelVisionProperties.Controls.Add(this.labelVisionFailureRetry, 0, 1);
            this.tableLayoutPanelModelVisionProperties.Controls.Add(this.labelDelayOfCameraTriggger, 0, 4);
            this.tableLayoutPanelModelVisionProperties.Controls.Add(this.labelImageProcessingTimeout, 0, 3);
            this.tableLayoutPanelModelVisionProperties.Controls.Add(this.textBoxImageProcessingTimeout, 1, 3);
            this.tableLayoutPanelModelVisionProperties.Controls.Add(this.textBoxDelayOfCameraTrigger, 1, 4);
            this.tableLayoutPanelModelVisionProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelModelVisionProperties.Location = new System.Drawing.Point(3, 28);
            this.tableLayoutPanelModelVisionProperties.Name = "tableLayoutPanelModelVisionProperties";
            this.tableLayoutPanelModelVisionProperties.RowCount = 5;
            this.tableLayoutPanelModelVisionProperties.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelModelVisionProperties.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelModelVisionProperties.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelModelVisionProperties.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelModelVisionProperties.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelModelVisionProperties.Size = new System.Drawing.Size(560, 181);
            this.tableLayoutPanelModelVisionProperties.TabIndex = 219;
            // 
            // textBoxVisioinRetryAttempts
            // 
            this.textBoxVisioinRetryAttempts.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxVisioinRetryAttempts.Location = new System.Drawing.Point(323, 75);
            this.textBoxVisioinRetryAttempts.Name = "textBoxVisioinRetryAttempts";
            this.textBoxVisioinRetryAttempts.Size = new System.Drawing.Size(114, 32);
            this.textBoxVisioinRetryAttempts.TabIndex = 5;
            this.textBoxVisioinRetryAttempts.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelVisionAlignmentRangeLimit
            // 
            this.labelVisionAlignmentRangeLimit.AutoSize = true;
            this.labelVisionAlignmentRangeLimit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelVisionAlignmentRangeLimit.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVisionAlignmentRangeLimit.Location = new System.Drawing.Point(3, 0);
            this.labelVisionAlignmentRangeLimit.Name = "labelVisionAlignmentRangeLimit";
            this.labelVisionAlignmentRangeLimit.Size = new System.Drawing.Size(314, 36);
            this.labelVisionAlignmentRangeLimit.TabIndex = 0;
            this.labelVisionAlignmentRangeLimit.Text = "Alignment range limit";
            this.labelVisionAlignmentRangeLimit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelVisionRetryAttempts
            // 
            this.labelVisionRetryAttempts.AutoSize = true;
            this.labelVisionRetryAttempts.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVisionRetryAttempts.Location = new System.Drawing.Point(3, 72);
            this.labelVisionRetryAttempts.Name = "labelVisionRetryAttempts";
            this.labelVisionRetryAttempts.Size = new System.Drawing.Size(218, 24);
            this.labelVisionRetryAttempts.TabIndex = 4;
            this.labelVisionRetryAttempts.Text = "Vision retry attempts";
            this.labelVisionRetryAttempts.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxVisionAlignmentRangeLimitX
            // 
            this.textBoxVisionAlignmentRangeLimitX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxVisionAlignmentRangeLimitX.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxVisionAlignmentRangeLimitX.Location = new System.Drawing.Point(323, 3);
            this.textBoxVisionAlignmentRangeLimitX.Name = "textBoxVisionAlignmentRangeLimitX";
            this.textBoxVisionAlignmentRangeLimitX.Size = new System.Drawing.Size(114, 32);
            this.textBoxVisionAlignmentRangeLimitX.TabIndex = 1;
            this.textBoxVisionAlignmentRangeLimitX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxVisionAlignmentRangeLimitY
            // 
            this.textBoxVisionAlignmentRangeLimitY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxVisionAlignmentRangeLimitY.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxVisionAlignmentRangeLimitY.Location = new System.Drawing.Point(443, 3);
            this.textBoxVisionAlignmentRangeLimitY.Name = "textBoxVisionAlignmentRangeLimitY";
            this.textBoxVisionAlignmentRangeLimitY.Size = new System.Drawing.Size(114, 32);
            this.textBoxVisionAlignmentRangeLimitY.TabIndex = 1;
            this.textBoxVisionAlignmentRangeLimitY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxVisionFailureRetry
            // 
            this.textBoxVisionFailureRetry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxVisionFailureRetry.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxVisionFailureRetry.Location = new System.Drawing.Point(323, 39);
            this.textBoxVisionFailureRetry.Name = "textBoxVisionFailureRetry";
            this.textBoxVisionFailureRetry.Size = new System.Drawing.Size(114, 32);
            this.textBoxVisionFailureRetry.TabIndex = 1;
            this.textBoxVisionFailureRetry.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelVisionFailureRetry
            // 
            this.labelVisionFailureRetry.AutoSize = true;
            this.labelVisionFailureRetry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelVisionFailureRetry.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVisionFailureRetry.Location = new System.Drawing.Point(3, 36);
            this.labelVisionFailureRetry.Name = "labelVisionFailureRetry";
            this.labelVisionFailureRetry.Size = new System.Drawing.Size(314, 36);
            this.labelVisionFailureRetry.TabIndex = 0;
            this.labelVisionFailureRetry.Text = "Failure retry";
            this.labelVisionFailureRetry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDelayOfCameraTriggger
            // 
            this.labelDelayOfCameraTriggger.AutoSize = true;
            this.labelDelayOfCameraTriggger.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDelayOfCameraTriggger.Location = new System.Drawing.Point(3, 144);
            this.labelDelayOfCameraTriggger.Name = "labelDelayOfCameraTriggger";
            this.labelDelayOfCameraTriggger.Size = new System.Drawing.Size(244, 24);
            this.labelDelayOfCameraTriggger.TabIndex = 6;
            this.labelDelayOfCameraTriggger.Text = "Delay of camera trigger";
            this.labelDelayOfCameraTriggger.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelImageProcessingTimeout
            // 
            this.labelImageProcessingTimeout.AutoSize = true;
            this.labelImageProcessingTimeout.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelImageProcessingTimeout.Location = new System.Drawing.Point(3, 108);
            this.labelImageProcessingTimeout.Name = "labelImageProcessingTimeout";
            this.labelImageProcessingTimeout.Size = new System.Drawing.Size(272, 24);
            this.labelImageProcessingTimeout.TabIndex = 7;
            this.labelImageProcessingTimeout.Text = "Image orocessing timeout";
            this.labelImageProcessingTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxImageProcessingTimeout
            // 
            this.textBoxImageProcessingTimeout.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxImageProcessingTimeout.Location = new System.Drawing.Point(323, 111);
            this.textBoxImageProcessingTimeout.Name = "textBoxImageProcessingTimeout";
            this.textBoxImageProcessingTimeout.Size = new System.Drawing.Size(114, 32);
            this.textBoxImageProcessingTimeout.TabIndex = 8;
            this.textBoxImageProcessingTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxDelayOfCameraTrigger
            // 
            this.textBoxDelayOfCameraTrigger.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDelayOfCameraTrigger.Location = new System.Drawing.Point(323, 147);
            this.textBoxDelayOfCameraTrigger.Name = "textBoxDelayOfCameraTrigger";
            this.textBoxDelayOfCameraTrigger.Size = new System.Drawing.Size(114, 32);
            this.textBoxDelayOfCameraTrigger.TabIndex = 9;
            this.textBoxDelayOfCameraTrigger.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBoxDelayProperties
            // 
            this.groupBoxDelayProperties.Controls.Add(this.tableLayoutPanelModelDelayProperties);
            this.groupBoxDelayProperties.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxDelayProperties.Location = new System.Drawing.Point(6, 248);
            this.groupBoxDelayProperties.Name = "groupBoxDelayProperties";
            this.groupBoxDelayProperties.Size = new System.Drawing.Size(560, 143);
            this.groupBoxDelayProperties.TabIndex = 218;
            this.groupBoxDelayProperties.TabStop = false;
            this.groupBoxDelayProperties.Text = " DELAY && TIMEOUT";
            // 
            // tableLayoutPanelModelDelayProperties
            // 
            this.tableLayoutPanelModelDelayProperties.ColumnCount = 3;
            this.tableLayoutPanelModelDelayProperties.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelModelDelayProperties.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 124F));
            this.tableLayoutPanelModelDelayProperties.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 116F));
            this.tableLayoutPanelModelDelayProperties.Controls.Add(this.labelUnloadReelStateUpdateDelay, 0, 2);
            this.tableLayoutPanelModelDelayProperties.Controls.Add(this.labelReturnReelSensingDelay, 0, 0);
            this.tableLayoutPanelModelDelayProperties.Controls.Add(this.textBoxReturnReelSensingDelay, 1, 0);
            this.tableLayoutPanelModelDelayProperties.Controls.Add(this.textBoxImageProcessingDelay, 1, 1);
            this.tableLayoutPanelModelDelayProperties.Controls.Add(this.labelReelSizeDetectingDelay, 0, 1);
            this.tableLayoutPanelModelDelayProperties.Controls.Add(this.textBoxUnloadReelStateUpdateDelay, 1, 2);
            this.tableLayoutPanelModelDelayProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelModelDelayProperties.Location = new System.Drawing.Point(3, 28);
            this.tableLayoutPanelModelDelayProperties.Name = "tableLayoutPanelModelDelayProperties";
            this.tableLayoutPanelModelDelayProperties.RowCount = 3;
            this.tableLayoutPanelModelDelayProperties.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelModelDelayProperties.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelModelDelayProperties.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelModelDelayProperties.Size = new System.Drawing.Size(554, 112);
            this.tableLayoutPanelModelDelayProperties.TabIndex = 220;
            // 
            // labelUnloadReelStateUpdateDelay
            // 
            this.labelUnloadReelStateUpdateDelay.AutoSize = true;
            this.labelUnloadReelStateUpdateDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUnloadReelStateUpdateDelay.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUnloadReelStateUpdateDelay.Location = new System.Drawing.Point(3, 74);
            this.labelUnloadReelStateUpdateDelay.Name = "labelUnloadReelStateUpdateDelay";
            this.labelUnloadReelStateUpdateDelay.Size = new System.Drawing.Size(308, 38);
            this.labelUnloadReelStateUpdateDelay.TabIndex = 2;
            this.labelUnloadReelStateUpdateDelay.Text = "Unload state update delay";
            this.labelUnloadReelStateUpdateDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelReturnReelSensingDelay
            // 
            this.labelReturnReelSensingDelay.AutoSize = true;
            this.labelReturnReelSensingDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReturnReelSensingDelay.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReturnReelSensingDelay.Location = new System.Drawing.Point(3, 0);
            this.labelReturnReelSensingDelay.Name = "labelReturnReelSensingDelay";
            this.labelReturnReelSensingDelay.Size = new System.Drawing.Size(308, 37);
            this.labelReturnReelSensingDelay.TabIndex = 0;
            this.labelReturnReelSensingDelay.Text = "Return reel sensing delay";
            this.labelReturnReelSensingDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxReturnReelSensingDelay
            // 
            this.textBoxReturnReelSensingDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxReturnReelSensingDelay.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxReturnReelSensingDelay.Location = new System.Drawing.Point(317, 3);
            this.textBoxReturnReelSensingDelay.Name = "textBoxReturnReelSensingDelay";
            this.textBoxReturnReelSensingDelay.Size = new System.Drawing.Size(118, 32);
            this.textBoxReturnReelSensingDelay.TabIndex = 1;
            this.textBoxReturnReelSensingDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxImageProcessingDelay
            // 
            this.textBoxImageProcessingDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxImageProcessingDelay.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxImageProcessingDelay.Location = new System.Drawing.Point(317, 40);
            this.textBoxImageProcessingDelay.Name = "textBoxImageProcessingDelay";
            this.textBoxImageProcessingDelay.Size = new System.Drawing.Size(118, 32);
            this.textBoxImageProcessingDelay.TabIndex = 1;
            this.textBoxImageProcessingDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelReelSizeDetectingDelay
            // 
            this.labelReelSizeDetectingDelay.AutoSize = true;
            this.labelReelSizeDetectingDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReelSizeDetectingDelay.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReelSizeDetectingDelay.Location = new System.Drawing.Point(3, 37);
            this.labelReelSizeDetectingDelay.Name = "labelReelSizeDetectingDelay";
            this.labelReelSizeDetectingDelay.Size = new System.Drawing.Size(308, 37);
            this.labelReelSizeDetectingDelay.TabIndex = 0;
            this.labelReelSizeDetectingDelay.Text = "Image processing delay";
            this.labelReelSizeDetectingDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxUnloadReelStateUpdateDelay
            // 
            this.textBoxUnloadReelStateUpdateDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxUnloadReelStateUpdateDelay.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxUnloadReelStateUpdateDelay.Location = new System.Drawing.Point(317, 77);
            this.textBoxUnloadReelStateUpdateDelay.Name = "textBoxUnloadReelStateUpdateDelay";
            this.textBoxUnloadReelStateUpdateDelay.Size = new System.Drawing.Size(118, 32);
            this.textBoxUnloadReelStateUpdateDelay.TabIndex = 3;
            this.textBoxUnloadReelStateUpdateDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonSaveTimeoutProperties
            // 
            this.buttonSaveTimeoutProperties.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSaveTimeoutProperties.Location = new System.Drawing.Point(456, 4);
            this.buttonSaveTimeoutProperties.Name = "buttonSaveTimeoutProperties";
            this.buttonSaveTimeoutProperties.Size = new System.Drawing.Size(120, 40);
            this.buttonSaveTimeoutProperties.TabIndex = 222;
            this.buttonSaveTimeoutProperties.Text = "SAVE";
            this.buttonSaveTimeoutProperties.UseVisualStyleBackColor = true;
            // 
            // buttonSaveNetworkSettings
            // 
            this.buttonSaveNetworkSettings.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSaveNetworkSettings.Location = new System.Drawing.Point(1044, 4);
            this.buttonSaveNetworkSettings.Name = "buttonSaveNetworkSettings";
            this.buttonSaveNetworkSettings.Size = new System.Drawing.Size(120, 40);
            this.buttonSaveNetworkSettings.TabIndex = 216;
            this.buttonSaveNetworkSettings.Text = "SAVE";
            this.buttonSaveNetworkSettings.UseVisualStyleBackColor = true;
            this.buttonSaveNetworkSettings.Visible = false;
            // 
            // groupBoxTimeoutProperties
            // 
            this.groupBoxTimeoutProperties.Controls.Add(this.tableLayoutPanelTimeout);
            this.groupBoxTimeoutProperties.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxTimeoutProperties.Location = new System.Drawing.Point(3, 21);
            this.groupBoxTimeoutProperties.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxTimeoutProperties.Name = "groupBoxTimeoutProperties";
            this.groupBoxTimeoutProperties.Padding = new System.Windows.Forms.Padding(7);
            this.groupBoxTimeoutProperties.Size = new System.Drawing.Size(580, 333);
            this.groupBoxTimeoutProperties.TabIndex = 215;
            this.groupBoxTimeoutProperties.TabStop = false;
            this.groupBoxTimeoutProperties.Text = " TIMEOUT ";
            // 
            // tableLayoutPanelTimeout
            // 
            this.tableLayoutPanelTimeout.ColumnCount = 3;
            this.tableLayoutPanelTimeout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTimeout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanelTimeout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanelTimeout.Controls.Add(this.labelRobotHomeTimeout, 0, 7);
            this.tableLayoutPanelTimeout.Controls.Add(this.labelRobotMoveTimeout, 0, 6);
            this.tableLayoutPanelTimeout.Controls.Add(this.labelReelTowerResponseTimeout, 0, 0);
            this.tableLayoutPanelTimeout.Controls.Add(this.textBoxReelTowerResponseTimeout, 1, 0);
            this.tableLayoutPanelTimeout.Controls.Add(this.textBoxCartInOutCheckTimeout, 1, 1);
            this.tableLayoutPanelTimeout.Controls.Add(this.labelCartInOutCheckTimeout, 0, 1);
            this.tableLayoutPanelTimeout.Controls.Add(this.labelRobotCommunicationTimeout, 0, 2);
            this.tableLayoutPanelTimeout.Controls.Add(this.labelRobotProgramLoadTimeout, 0, 3);
            this.tableLayoutPanelTimeout.Controls.Add(this.labelRobotProgramPlayTimeout, 0, 4);
            this.tableLayoutPanelTimeout.Controls.Add(this.labelRobotActionTimeout, 0, 5);
            this.tableLayoutPanelTimeout.Controls.Add(this.textBoxRobotCommunicationTimeout, 1, 2);
            this.tableLayoutPanelTimeout.Controls.Add(this.textBoxRobotProgramLoadTimeout, 1, 3);
            this.tableLayoutPanelTimeout.Controls.Add(this.textBoxRobotProgramPlayTimeout, 1, 4);
            this.tableLayoutPanelTimeout.Controls.Add(this.textBoxRobotActionTimeout, 1, 5);
            this.tableLayoutPanelTimeout.Controls.Add(this.textBoxRobotMoveTimeout, 1, 6);
            this.tableLayoutPanelTimeout.Controls.Add(this.textBoxRobotHomeTimeout, 1, 7);
            this.tableLayoutPanelTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTimeout.Location = new System.Drawing.Point(7, 32);
            this.tableLayoutPanelTimeout.Name = "tableLayoutPanelTimeout";
            this.tableLayoutPanelTimeout.RowCount = 8;
            this.tableLayoutPanelTimeout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanelTimeout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanelTimeout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanelTimeout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanelTimeout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanelTimeout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanelTimeout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanelTimeout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanelTimeout.Size = new System.Drawing.Size(566, 294);
            this.tableLayoutPanelTimeout.TabIndex = 216;
            // 
            // labelRobotHomeTimeout
            // 
            this.labelRobotHomeTimeout.AutoSize = true;
            this.labelRobotHomeTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRobotHomeTimeout.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRobotHomeTimeout.Location = new System.Drawing.Point(3, 252);
            this.labelRobotHomeTimeout.Name = "labelRobotHomeTimeout";
            this.labelRobotHomeTimeout.Size = new System.Drawing.Size(320, 42);
            this.labelRobotHomeTimeout.TabIndex = 12;
            this.labelRobotHomeTimeout.Text = "Robot home timeout";
            this.labelRobotHomeTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelRobotMoveTimeout
            // 
            this.labelRobotMoveTimeout.AutoSize = true;
            this.labelRobotMoveTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRobotMoveTimeout.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRobotMoveTimeout.Location = new System.Drawing.Point(3, 216);
            this.labelRobotMoveTimeout.Name = "labelRobotMoveTimeout";
            this.labelRobotMoveTimeout.Size = new System.Drawing.Size(320, 36);
            this.labelRobotMoveTimeout.TabIndex = 6;
            this.labelRobotMoveTimeout.Text = "Robot move timeout";
            this.labelRobotMoveTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelReelTowerResponseTimeout
            // 
            this.labelReelTowerResponseTimeout.AutoSize = true;
            this.labelReelTowerResponseTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReelTowerResponseTimeout.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReelTowerResponseTimeout.Location = new System.Drawing.Point(3, 0);
            this.labelReelTowerResponseTimeout.Name = "labelReelTowerResponseTimeout";
            this.labelReelTowerResponseTimeout.Size = new System.Drawing.Size(320, 36);
            this.labelReelTowerResponseTimeout.TabIndex = 0;
            this.labelReelTowerResponseTimeout.Text = "Reel tower response timeout";
            this.labelReelTowerResponseTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxReelTowerResponseTimeout
            // 
            this.textBoxReelTowerResponseTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxReelTowerResponseTimeout.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxReelTowerResponseTimeout.Location = new System.Drawing.Point(329, 3);
            this.textBoxReelTowerResponseTimeout.Name = "textBoxReelTowerResponseTimeout";
            this.textBoxReelTowerResponseTimeout.Size = new System.Drawing.Size(114, 32);
            this.textBoxReelTowerResponseTimeout.TabIndex = 1;
            this.textBoxReelTowerResponseTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxCartInOutCheckTimeout
            // 
            this.textBoxCartInOutCheckTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCartInOutCheckTimeout.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCartInOutCheckTimeout.Location = new System.Drawing.Point(329, 39);
            this.textBoxCartInOutCheckTimeout.Name = "textBoxCartInOutCheckTimeout";
            this.textBoxCartInOutCheckTimeout.Size = new System.Drawing.Size(114, 32);
            this.textBoxCartInOutCheckTimeout.TabIndex = 1;
            this.textBoxCartInOutCheckTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelCartInOutCheckTimeout
            // 
            this.labelCartInOutCheckTimeout.AutoSize = true;
            this.labelCartInOutCheckTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCartInOutCheckTimeout.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCartInOutCheckTimeout.Location = new System.Drawing.Point(3, 36);
            this.labelCartInOutCheckTimeout.Name = "labelCartInOutCheckTimeout";
            this.labelCartInOutCheckTimeout.Size = new System.Drawing.Size(320, 36);
            this.labelCartInOutCheckTimeout.TabIndex = 0;
            this.labelCartInOutCheckTimeout.Text = "Cart in / out check timeout";
            this.labelCartInOutCheckTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelRobotCommunicationTimeout
            // 
            this.labelRobotCommunicationTimeout.AutoSize = true;
            this.labelRobotCommunicationTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRobotCommunicationTimeout.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRobotCommunicationTimeout.Location = new System.Drawing.Point(3, 72);
            this.labelRobotCommunicationTimeout.Name = "labelRobotCommunicationTimeout";
            this.labelRobotCommunicationTimeout.Size = new System.Drawing.Size(320, 36);
            this.labelRobotCommunicationTimeout.TabIndex = 2;
            this.labelRobotCommunicationTimeout.Text = "Robot communication timeout";
            this.labelRobotCommunicationTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelRobotProgramLoadTimeout
            // 
            this.labelRobotProgramLoadTimeout.AutoSize = true;
            this.labelRobotProgramLoadTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRobotProgramLoadTimeout.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRobotProgramLoadTimeout.Location = new System.Drawing.Point(3, 108);
            this.labelRobotProgramLoadTimeout.Name = "labelRobotProgramLoadTimeout";
            this.labelRobotProgramLoadTimeout.Size = new System.Drawing.Size(320, 36);
            this.labelRobotProgramLoadTimeout.TabIndex = 3;
            this.labelRobotProgramLoadTimeout.Text = "Robot program load timeout";
            this.labelRobotProgramLoadTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelRobotProgramPlayTimeout
            // 
            this.labelRobotProgramPlayTimeout.AutoSize = true;
            this.labelRobotProgramPlayTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRobotProgramPlayTimeout.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRobotProgramPlayTimeout.Location = new System.Drawing.Point(3, 144);
            this.labelRobotProgramPlayTimeout.Name = "labelRobotProgramPlayTimeout";
            this.labelRobotProgramPlayTimeout.Size = new System.Drawing.Size(320, 36);
            this.labelRobotProgramPlayTimeout.TabIndex = 4;
            this.labelRobotProgramPlayTimeout.Text = "Robot program play timeout";
            this.labelRobotProgramPlayTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelRobotActionTimeout
            // 
            this.labelRobotActionTimeout.AutoSize = true;
            this.labelRobotActionTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRobotActionTimeout.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRobotActionTimeout.Location = new System.Drawing.Point(3, 180);
            this.labelRobotActionTimeout.Name = "labelRobotActionTimeout";
            this.labelRobotActionTimeout.Size = new System.Drawing.Size(320, 36);
            this.labelRobotActionTimeout.TabIndex = 5;
            this.labelRobotActionTimeout.Text = "Robot action timeout";
            this.labelRobotActionTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxRobotCommunicationTimeout
            // 
            this.textBoxRobotCommunicationTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRobotCommunicationTimeout.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRobotCommunicationTimeout.Location = new System.Drawing.Point(329, 75);
            this.textBoxRobotCommunicationTimeout.Name = "textBoxRobotCommunicationTimeout";
            this.textBoxRobotCommunicationTimeout.Size = new System.Drawing.Size(114, 32);
            this.textBoxRobotCommunicationTimeout.TabIndex = 7;
            this.textBoxRobotCommunicationTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxRobotProgramLoadTimeout
            // 
            this.textBoxRobotProgramLoadTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRobotProgramLoadTimeout.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRobotProgramLoadTimeout.Location = new System.Drawing.Point(329, 111);
            this.textBoxRobotProgramLoadTimeout.Name = "textBoxRobotProgramLoadTimeout";
            this.textBoxRobotProgramLoadTimeout.Size = new System.Drawing.Size(114, 32);
            this.textBoxRobotProgramLoadTimeout.TabIndex = 8;
            this.textBoxRobotProgramLoadTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxRobotProgramPlayTimeout
            // 
            this.textBoxRobotProgramPlayTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRobotProgramPlayTimeout.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRobotProgramPlayTimeout.Location = new System.Drawing.Point(329, 147);
            this.textBoxRobotProgramPlayTimeout.Name = "textBoxRobotProgramPlayTimeout";
            this.textBoxRobotProgramPlayTimeout.Size = new System.Drawing.Size(114, 32);
            this.textBoxRobotProgramPlayTimeout.TabIndex = 9;
            this.textBoxRobotProgramPlayTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxRobotActionTimeout
            // 
            this.textBoxRobotActionTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRobotActionTimeout.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRobotActionTimeout.Location = new System.Drawing.Point(329, 183);
            this.textBoxRobotActionTimeout.Name = "textBoxRobotActionTimeout";
            this.textBoxRobotActionTimeout.Size = new System.Drawing.Size(114, 32);
            this.textBoxRobotActionTimeout.TabIndex = 10;
            this.textBoxRobotActionTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxRobotMoveTimeout
            // 
            this.textBoxRobotMoveTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRobotMoveTimeout.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRobotMoveTimeout.Location = new System.Drawing.Point(329, 219);
            this.textBoxRobotMoveTimeout.Name = "textBoxRobotMoveTimeout";
            this.textBoxRobotMoveTimeout.Size = new System.Drawing.Size(114, 32);
            this.textBoxRobotMoveTimeout.TabIndex = 11;
            this.textBoxRobotMoveTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxRobotHomeTimeout
            // 
            this.textBoxRobotHomeTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRobotHomeTimeout.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRobotHomeTimeout.Location = new System.Drawing.Point(329, 255);
            this.textBoxRobotHomeTimeout.Name = "textBoxRobotHomeTimeout";
            this.textBoxRobotHomeTimeout.Size = new System.Drawing.Size(114, 32);
            this.textBoxRobotHomeTimeout.TabIndex = 13;
            this.textBoxRobotHomeTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBoxNetworkSetting
            // 
            this.groupBoxNetworkSetting.BackColor = System.Drawing.SystemColors.Control;
            this.groupBoxNetworkSetting.Controls.Add(this.tableLayoutPanelNetwork);
            this.groupBoxNetworkSetting.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxNetworkSetting.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBoxNetworkSetting.Location = new System.Drawing.Point(587, 21);
            this.groupBoxNetworkSetting.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxNetworkSetting.Name = "groupBoxNetworkSetting";
            this.groupBoxNetworkSetting.Size = new System.Drawing.Size(580, 220);
            this.groupBoxNetworkSetting.TabIndex = 214;
            this.groupBoxNetworkSetting.TabStop = false;
            this.groupBoxNetworkSetting.Text = " NETWORK ";
            // 
            // tableLayoutPanelNetwork
            // 
            this.tableLayoutPanelNetwork.ColumnCount = 3;
            this.tableLayoutPanelNetwork.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelNetwork.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanelNetwork.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanelNetwork.Controls.Add(this.textBoxRobotControllerAddress, 1, 0);
            this.tableLayoutPanelNetwork.Controls.Add(this.label38, 0, 4);
            this.tableLayoutPanelNetwork.Controls.Add(this.label17, 0, 0);
            this.tableLayoutPanelNetwork.Controls.Add(this.label13, 0, 3);
            this.tableLayoutPanelNetwork.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanelNetwork.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanelNetwork.Controls.Add(this.textBoxReelTowerAddress, 1, 3);
            this.tableLayoutPanelNetwork.Controls.Add(this.textBoxReelTowerPort, 2, 3);
            this.tableLayoutPanelNetwork.Controls.Add(this.textBoxMobileRobotAddress, 1, 4);
            this.tableLayoutPanelNetwork.Controls.Add(this.textBoxMobileRobotPort, 2, 4);
            this.tableLayoutPanelNetwork.Controls.Add(this.textBoxRobotControllerPort, 2, 1);
            this.tableLayoutPanelNetwork.Controls.Add(this.textBoxRobotPort, 2, 2);
            this.tableLayoutPanelNetwork.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelNetwork.Location = new System.Drawing.Point(3, 28);
            this.tableLayoutPanelNetwork.Name = "tableLayoutPanelNetwork";
            this.tableLayoutPanelNetwork.RowCount = 5;
            this.tableLayoutPanelNetwork.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelNetwork.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelNetwork.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelNetwork.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelNetwork.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelNetwork.Size = new System.Drawing.Size(574, 189);
            this.tableLayoutPanelNetwork.TabIndex = 217;
            // 
            // textBoxRobotControllerAddress
            // 
            this.textBoxRobotControllerAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRobotControllerAddress.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRobotControllerAddress.Location = new System.Drawing.Point(257, 3);
            this.textBoxRobotControllerAddress.Name = "textBoxRobotControllerAddress";
            this.textBoxRobotControllerAddress.Size = new System.Drawing.Size(194, 32);
            this.textBoxRobotControllerAddress.TabIndex = 216;
            this.textBoxRobotControllerAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label38
            // 
            this.label38.BackColor = System.Drawing.SystemColors.Control;
            this.label38.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label38.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label38.Location = new System.Drawing.Point(0, 148);
            this.label38.Margin = new System.Windows.Forms.Padding(0);
            this.label38.Name = "label38";
            this.label38.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label38.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label38.Size = new System.Drawing.Size(254, 41);
            this.label38.TabIndex = 118;
            this.label38.Text = "Mobile robot network";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.SystemColors.Control;
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label17.Location = new System.Drawing.Point(0, 0);
            this.label17.Margin = new System.Windows.Forms.Padding(0);
            this.label17.Name = "label17";
            this.label17.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label17.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label17.Size = new System.Drawing.Size(254, 37);
            this.label17.TabIndex = 120;
            this.label17.Text = "Robot address";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.SystemColors.Control;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(0, 111);
            this.label13.Margin = new System.Windows.Forms.Padding(0);
            this.label13.Name = "label13";
            this.label13.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label13.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label13.Size = new System.Drawing.Size(254, 37);
            this.label13.TabIndex = 112;
            this.label13.Text = "Reel tower network";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.Control;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(0, 37);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(254, 37);
            this.label4.TabIndex = 110;
            this.label4.Text = "Robot controller port";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.Control;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(0, 74);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label9.Size = new System.Drawing.Size(254, 37);
            this.label9.TabIndex = 118;
            this.label9.Text = "Robot remote port";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxReelTowerAddress
            // 
            this.textBoxReelTowerAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxReelTowerAddress.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxReelTowerAddress.Location = new System.Drawing.Point(257, 114);
            this.textBoxReelTowerAddress.Name = "textBoxReelTowerAddress";
            this.textBoxReelTowerAddress.Size = new System.Drawing.Size(194, 32);
            this.textBoxReelTowerAddress.TabIndex = 219;
            this.textBoxReelTowerAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxReelTowerPort
            // 
            this.textBoxReelTowerPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxReelTowerPort.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxReelTowerPort.Location = new System.Drawing.Point(457, 114);
            this.textBoxReelTowerPort.Name = "textBoxReelTowerPort";
            this.textBoxReelTowerPort.Size = new System.Drawing.Size(114, 32);
            this.textBoxReelTowerPort.TabIndex = 220;
            this.textBoxReelTowerPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxMobileRobotAddress
            // 
            this.textBoxMobileRobotAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMobileRobotAddress.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMobileRobotAddress.Location = new System.Drawing.Point(257, 151);
            this.textBoxMobileRobotAddress.Name = "textBoxMobileRobotAddress";
            this.textBoxMobileRobotAddress.Size = new System.Drawing.Size(194, 32);
            this.textBoxMobileRobotAddress.TabIndex = 221;
            this.textBoxMobileRobotAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxMobileRobotPort
            // 
            this.textBoxMobileRobotPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMobileRobotPort.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMobileRobotPort.Location = new System.Drawing.Point(457, 151);
            this.textBoxMobileRobotPort.Name = "textBoxMobileRobotPort";
            this.textBoxMobileRobotPort.Size = new System.Drawing.Size(114, 32);
            this.textBoxMobileRobotPort.TabIndex = 222;
            this.textBoxMobileRobotPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxRobotControllerPort
            // 
            this.textBoxRobotControllerPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRobotControllerPort.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRobotControllerPort.Location = new System.Drawing.Point(457, 40);
            this.textBoxRobotControllerPort.Name = "textBoxRobotControllerPort";
            this.textBoxRobotControllerPort.Size = new System.Drawing.Size(114, 32);
            this.textBoxRobotControllerPort.TabIndex = 217;
            this.textBoxRobotControllerPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxRobotPort
            // 
            this.textBoxRobotPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRobotPort.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRobotPort.Location = new System.Drawing.Point(457, 77);
            this.textBoxRobotPort.Name = "textBoxRobotPort";
            this.textBoxRobotPort.Size = new System.Drawing.Size(114, 32);
            this.textBoxRobotPort.TabIndex = 218;
            this.textBoxRobotPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBoxGuiSettings
            // 
            this.groupBoxGuiSettings.Controls.Add(this.tableLayoutPanel2);
            this.groupBoxGuiSettings.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxGuiSettings.Location = new System.Drawing.Point(1171, 21);
            this.groupBoxGuiSettings.Name = "groupBoxGuiSettings";
            this.groupBoxGuiSettings.Size = new System.Drawing.Size(425, 68);
            this.groupBoxGuiSettings.TabIndex = 229;
            this.groupBoxGuiSettings.TabStop = false;
            this.groupBoxGuiSettings.Text = " GUI ";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.labelDisplayLanguage, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxDisplayLanguage, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 28);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(419, 37);
            this.tableLayoutPanel2.TabIndex = 220;
            // 
            // labelDisplayLanguage
            // 
            this.labelDisplayLanguage.AutoSize = true;
            this.labelDisplayLanguage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDisplayLanguage.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDisplayLanguage.Location = new System.Drawing.Point(3, 0);
            this.labelDisplayLanguage.Name = "labelDisplayLanguage";
            this.labelDisplayLanguage.Size = new System.Drawing.Size(203, 37);
            this.labelDisplayLanguage.TabIndex = 0;
            this.labelDisplayLanguage.Text = "Display language";
            this.labelDisplayLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxDisplayLanguage
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.comboBoxDisplayLanguage, 2);
            this.comboBoxDisplayLanguage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxDisplayLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDisplayLanguage.Enabled = false;
            this.comboBoxDisplayLanguage.FormattingEnabled = true;
            this.comboBoxDisplayLanguage.Items.AddRange(new object[] {
            "English (en-US)",
            "Korean (ko-KR)"});
            this.comboBoxDisplayLanguage.Location = new System.Drawing.Point(212, 3);
            this.comboBoxDisplayLanguage.Name = "comboBoxDisplayLanguage";
            this.comboBoxDisplayLanguage.Size = new System.Drawing.Size(204, 32);
            this.comboBoxDisplayLanguage.TabIndex = 1;
            // 
            // tabPageLog
            // 
            this.tabPageLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPageLog.Controls.Add(this.buttonClearLog);
            this.tabPageLog.Controls.Add(this.tabControlLogPage);
            this.tabPageLog.Location = new System.Drawing.Point(4, 44);
            this.tabPageLog.Name = "tabPageLog";
            this.tabPageLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLog.Size = new System.Drawing.Size(1605, 925);
            this.tabPageLog.TabIndex = 1;
            this.tabPageLog.Text = "LOG";
            this.tabPageLog.UseVisualStyleBackColor = true;
            // 
            // buttonClearLog
            // 
            this.buttonClearLog.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClearLog.Location = new System.Drawing.Point(1480, 3);
            this.buttonClearLog.Name = "buttonClearLog";
            this.buttonClearLog.Size = new System.Drawing.Size(120, 40);
            this.buttonClearLog.TabIndex = 90;
            this.buttonClearLog.Text = "CLEAR";
            this.buttonClearLog.UseVisualStyleBackColor = true;
            this.buttonClearLog.Click += new System.EventHandler(this.OnClearLog);
            // 
            // tabControlLogPage
            // 
            this.tabControlLogPage.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControlLogPage.Controls.Add(this.tabPageReelTowerLog);
            this.tabControlLogPage.Controls.Add(this.tabPageRobotLog);
            this.tabControlLogPage.Controls.Add(this.tabPageAlarmHistory);
            this.tabControlLogPage.Dock = System.Windows.Forms.DockStyle.Left;
            this.tabControlLogPage.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControlLogPage.ItemSize = new System.Drawing.Size(200, 40);
            this.tabControlLogPage.Location = new System.Drawing.Point(3, 3);
            this.tabControlLogPage.Name = "tabControlLogPage";
            this.tabControlLogPage.SelectedIndex = 0;
            this.tabControlLogPage.Size = new System.Drawing.Size(1602, 917);
            this.tabControlLogPage.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControlLogPage.TabIndex = 219;
            this.tabControlLogPage.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnTabControlDrawItem);
            this.tabControlLogPage.SelectedIndexChanged += new System.EventHandler(this.OnLogTabIndexChanged);
            this.tabControlLogPage.TabIndexChanged += new System.EventHandler(this.OnLogTabIndexChanged);
            // 
            // tabPageReelTowerLog
            // 
            this.tabPageReelTowerLog.Controls.Add(this.listBoxReelTowerComm);
            this.tabPageReelTowerLog.Location = new System.Drawing.Point(4, 44);
            this.tabPageReelTowerLog.Name = "tabPageReelTowerLog";
            this.tabPageReelTowerLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageReelTowerLog.Size = new System.Drawing.Size(1594, 869);
            this.tabPageReelTowerLog.TabIndex = 0;
            this.tabPageReelTowerLog.Text = "  REEL TOWER  ";
            this.tabPageReelTowerLog.UseVisualStyleBackColor = true;
            // 
            // listBoxReelTowerComm
            // 
            this.listBoxReelTowerComm.BackColor = System.Drawing.SystemColors.Control;
            this.listBoxReelTowerComm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxReelTowerComm.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxReelTowerComm.ForeColor = System.Drawing.SystemColors.ControlText;
            this.listBoxReelTowerComm.FormattingEnabled = true;
            this.listBoxReelTowerComm.HorizontalScrollbar = true;
            this.listBoxReelTowerComm.ItemHeight = 18;
            this.listBoxReelTowerComm.Location = new System.Drawing.Point(3, 3);
            this.listBoxReelTowerComm.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxReelTowerComm.Name = "listBoxReelTowerComm";
            this.listBoxReelTowerComm.Size = new System.Drawing.Size(1588, 863);
            this.listBoxReelTowerComm.TabIndex = 89;
            // 
            // tabPageRobotLog
            // 
            this.tabPageRobotLog.Controls.Add(this.listBoxRobotComm);
            this.tabPageRobotLog.Location = new System.Drawing.Point(4, 44);
            this.tabPageRobotLog.Name = "tabPageRobotLog";
            this.tabPageRobotLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRobotLog.Size = new System.Drawing.Size(1471, 869);
            this.tabPageRobotLog.TabIndex = 1;
            this.tabPageRobotLog.Text = "  ROBOT  && MOBILE  ";
            this.tabPageRobotLog.UseVisualStyleBackColor = true;
            // 
            // listBoxRobotComm
            // 
            this.listBoxRobotComm.BackColor = System.Drawing.SystemColors.Control;
            this.listBoxRobotComm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxRobotComm.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxRobotComm.ForeColor = System.Drawing.SystemColors.ControlText;
            this.listBoxRobotComm.FormattingEnabled = true;
            this.listBoxRobotComm.HorizontalScrollbar = true;
            this.listBoxRobotComm.ItemHeight = 18;
            this.listBoxRobotComm.Location = new System.Drawing.Point(3, 3);
            this.listBoxRobotComm.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxRobotComm.Name = "listBoxRobotComm";
            this.listBoxRobotComm.Size = new System.Drawing.Size(1465, 863);
            this.listBoxRobotComm.TabIndex = 90;
            // 
            // tabPageAlarmHistory
            // 
            this.tabPageAlarmHistory.Controls.Add(this.treeViewLog);
            this.tabPageAlarmHistory.Controls.Add(this.listBoxAlarmHistory);
            this.tabPageAlarmHistory.Location = new System.Drawing.Point(4, 44);
            this.tabPageAlarmHistory.Name = "tabPageAlarmHistory";
            this.tabPageAlarmHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAlarmHistory.Size = new System.Drawing.Size(1471, 869);
            this.tabPageAlarmHistory.TabIndex = 2;
            this.tabPageAlarmHistory.Text = "  ALARM HISTORY  ";
            this.tabPageAlarmHistory.UseVisualStyleBackColor = true;
            // 
            // treeViewLog
            // 
            this.treeViewLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewLog.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewLog.ImageIndex = 4;
            this.treeViewLog.ImageList = this.imageListFileSystem;
            this.treeViewLog.Location = new System.Drawing.Point(3, 3);
            this.treeViewLog.Name = "treeViewLog";
            treeNode2.ImageIndex = 4;
            treeNode2.Name = "Log";
            treeNode2.NodeFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode2.SelectedImageIndex = 5;
            treeNode2.Text = "Log";
            this.treeViewLog.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.treeViewLog.SelectedImageIndex = 5;
            this.treeViewLog.Size = new System.Drawing.Size(265, 863);
            this.treeViewLog.TabIndex = 92;
            this.treeViewLog.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnAfterSelectNode);
            // 
            // imageListFileSystem
            // 
            this.imageListFileSystem.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListFileSystem.ImageStream")));
            this.imageListFileSystem.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListFileSystem.Images.SetKeyName(0, "Icon_Internet.ico");
            this.imageListFileSystem.Images.SetKeyName(1, "Icon_Desktop.ico");
            this.imageListFileSystem.Images.SetKeyName(2, "Icon_Monitor_Off.ico");
            this.imageListFileSystem.Images.SetKeyName(3, "Icon_Monitor_On.ico");
            this.imageListFileSystem.Images.SetKeyName(4, "Icon_Folder.ico");
            this.imageListFileSystem.Images.SetKeyName(5, "Icon_Opened_Folder.ico");
            this.imageListFileSystem.Images.SetKeyName(6, "Icon_New_File.ico");
            this.imageListFileSystem.Images.SetKeyName(7, "Icon_Text_File.ico");
            // 
            // listBoxAlarmHistory
            // 
            this.listBoxAlarmHistory.BackColor = System.Drawing.SystemColors.Control;
            this.listBoxAlarmHistory.Dock = System.Windows.Forms.DockStyle.Right;
            this.listBoxAlarmHistory.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxAlarmHistory.ForeColor = System.Drawing.SystemColors.ControlText;
            this.listBoxAlarmHistory.FormattingEnabled = true;
            this.listBoxAlarmHistory.HorizontalScrollbar = true;
            this.listBoxAlarmHistory.ItemHeight = 18;
            this.listBoxAlarmHistory.Location = new System.Drawing.Point(268, 3);
            this.listBoxAlarmHistory.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxAlarmHistory.Name = "listBoxAlarmHistory";
            this.listBoxAlarmHistory.Size = new System.Drawing.Size(1200, 863);
            this.listBoxAlarmHistory.TabIndex = 91;
            // 
            // tableLayoutPanelMainFrame
            // 
            this.tableLayoutPanelMainFrame.ColumnCount = 3;
            this.tableLayoutPanelMainFrame.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMainFrame.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.00001F));
            this.tableLayoutPanelMainFrame.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanelMainFrame.Controls.Add(this.tabControlMain, 0, 1);
            this.tableLayoutPanelMainFrame.Controls.Add(this.tableLayoutPanelControlMenu, 2, 1);
            this.tableLayoutPanelMainFrame.Controls.Add(this.tableLayoutPanelTitle, 0, 0);
            this.tableLayoutPanelMainFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMainFrame.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMainFrame.Name = "tableLayoutPanelMainFrame";
            this.tableLayoutPanelMainFrame.RowCount = 3;
            this.tableLayoutPanelMainFrame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelMainFrame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMainFrame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelMainFrame.Size = new System.Drawing.Size(1920, 1061);
            this.tableLayoutPanelMainFrame.TabIndex = 224;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.OnDoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.OnRunWorkerCompleted);
            // 
            // FormMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1920, 1061);
            this.Controls.Add(this.tableLayoutPanelMainFrame);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main View";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.Shown += new System.EventHandler(this.OnFormShown);
            this.tableLayoutPanelTitle.ResumeLayout(false);
            this.tableLayoutPanelControlMenu.ResumeLayout(false);
            this.groupBoxRuntimeTrace.ResumeLayout(false);
            this.tableLayoutPanelRuntimeTrace.ResumeLayout(false);
            this.groupBoxPerformance.ResumeLayout(false);
            this.tableLayoutPanelPerformance.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageOperation.ResumeLayout(false);
            this.groupBoxUnloadReel.ResumeLayout(false);
            this.groupBoxCartInOut.ResumeLayout(false);
            this.groupBoxLoadReel.ResumeLayout(false);
            this.tableLayoutPanelPickingList.ResumeLayout(false);
            this.tableLayoutPanelTransferReelInformation.ResumeLayout(false);
            this.tableLayoutPanelReelTowerStates.ResumeLayout(false);
            this.tableLayoutPanelReelTower1.ResumeLayout(false);
            this.tableLayoutPanelReelTower2.ResumeLayout(false);
            this.tableLayoutPanelReelTower3.ResumeLayout(false);
            this.tableLayoutPanelReelTower4.ResumeLayout(false);
            this.tabPageVision.ResumeLayout(false);
            this.tabPageMaintenance.ResumeLayout(false);
            this.groupBoxVisionAndLight.ResumeLayout(false);
            this.tableLayoutPanelVisionAndLight.ResumeLayout(false);
            this.groupBoxDigitalIo.ResumeLayout(false);
            this.tabPageConfig.ResumeLayout(false);
            this.groupBoxVisionProcessImage.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBoxModel.ResumeLayout(false);
            this.groupBoxModel.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanelModelVisionProperties.ResumeLayout(false);
            this.tableLayoutPanelModelVisionProperties.PerformLayout();
            this.groupBoxDelayProperties.ResumeLayout(false);
            this.tableLayoutPanelModelDelayProperties.ResumeLayout(false);
            this.tableLayoutPanelModelDelayProperties.PerformLayout();
            this.groupBoxTimeoutProperties.ResumeLayout(false);
            this.tableLayoutPanelTimeout.ResumeLayout(false);
            this.tableLayoutPanelTimeout.PerformLayout();
            this.groupBoxNetworkSetting.ResumeLayout(false);
            this.tableLayoutPanelNetwork.ResumeLayout(false);
            this.tableLayoutPanelNetwork.PerformLayout();
            this.groupBoxGuiSettings.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tabPageLog.ResumeLayout(false);
            this.tabControlLogPage.ResumeLayout(false);
            this.tabPageReelTowerLog.ResumeLayout(false);
            this.tabPageRobotLog.ResumeLayout(false);
            this.tabPageAlarmHistory.ResumeLayout(false);
            this.tableLayoutPanelMainFrame.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Label labelCurrentRecipe;
        public System.Windows.Forms.Label labelOperationState;
        public System.Windows.Forms.Button buttonExit;
        public System.Windows.Forms.Button buttonReset;
        public System.Windows.Forms.Label labelCycleLoTowerInput;
        public System.Windows.Forms.Label labelAverageLoTowerInput;
        public System.Windows.Forms.Label label55;
        public System.Windows.Forms.Label labelMobileRobotState;
        public System.Windows.Forms.Label labelRobotState;
        public System.Windows.Forms.Label labelRobot;
        public System.Windows.Forms.Label labelMobileRobotCommunication;
        public System.Windows.Forms.Button buttonStop;
        public System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTitle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelControlMenu;
        private System.Windows.Forms.GroupBox groupBoxPerformance;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageOperation;
        public System.Windows.Forms.Label labelReelTowerState1;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label lblTowerState1;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label lblTowerMode3;
        public System.Windows.Forms.Label labelRobotCommunicationStateValue;
        public System.Windows.Forms.Label lblURClientConnectState;
        public System.Windows.Forms.Label labelReelTowerId3;
        public System.Windows.Forms.Label labelMobileRobotCommunicationStateValue;
        public System.Windows.Forms.Button btnClientConnect;
        public System.Windows.Forms.Label lblTowerMode2;
        public System.Windows.Forms.Label labelReelTowerState4;
        // public System.Windows.Forms.Label label33;
        public System.Windows.Forms.Label labelReelTowerState3;
        public System.Windows.Forms.Label labelReelTowerId2;
        public System.Windows.Forms.Label labelReelTowerState2;
        public System.Windows.Forms.Label label47;
        public System.Windows.Forms.Label lblTowerMode4;
        public System.Windows.Forms.Label label31;
        public System.Windows.Forms.Label label29;
        public System.Windows.Forms.Label labelReelTowerId4;
        public System.Windows.Forms.Label label30;
        public System.Windows.Forms.Label labelReelTowerId1;
        public System.Windows.Forms.Label label27;
        public System.Windows.Forms.Label labelUid;
        public System.Windows.Forms.Label labelReelTowerCommunicationStateValue;
        public System.Windows.Forms.Label label26;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label labelSid;
        public System.Windows.Forms.Label lblTowerMode1;
        public System.Windows.Forms.Label labelLotId;
        public System.Windows.Forms.Label labelSupplier;
        public System.Windows.Forms.Label labelMfg;
        public System.Windows.Forms.Label labelDoorStateTower1;
        public System.Windows.Forms.Label labelQty;
        public System.Windows.Forms.Label lblTowerState2;
        public System.Windows.Forms.Label labelTowerCodeId2;
        public System.Windows.Forms.Label labelTowerCodeId3;
        public System.Windows.Forms.Label labelTowerCodeId1;
        public System.Windows.Forms.Label lblTowerState4;
        public System.Windows.Forms.Label labelTowerCodeId4;
        public System.Windows.Forms.Label lblTowerState3;
        private System.Windows.Forms.TabPage tabPageLog;
        private System.Windows.Forms.TabControl tabControlLogPage;
        private System.Windows.Forms.TabPage tabPageReelTowerLog;
        public System.Windows.Forms.ListBox listBoxReelTowerComm;
        private System.Windows.Forms.TabPage tabPageRobotLog;
        public System.Windows.Forms.ListBox listBoxRobotComm;
        private System.Windows.Forms.TabPage tabPageConfig;
        private System.Windows.Forms.GroupBox groupBoxTimeoutProperties;
        private System.Windows.Forms.TextBox textBoxCartInOutCheckTimeout;
        private System.Windows.Forms.TextBox textBoxReelTowerResponseTimeout;
        private System.Windows.Forms.Label labelCartInOutCheckTimeout;
        private System.Windows.Forms.Label labelReelTowerResponseTimeout;
        private System.Windows.Forms.GroupBox groupBoxNetworkSetting;
        public System.Windows.Forms.Label label17;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label label13;
        public System.Windows.Forms.Label label9;
        public System.Windows.Forms.Label label38;
        public System.Windows.Forms.Label labelRobotCommunicationState;
        public System.Windows.Forms.Label labelReelTowerCommunicationState;
        public System.Windows.Forms.Label labelReelTower;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelReelTowerStates;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelReelTower1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelReelTower2;
        public System.Windows.Forms.Label label32;
        public System.Windows.Forms.Label labelDoorStateTower2;
        public System.Windows.Forms.Label label44;
        public System.Windows.Forms.Label label48;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelReelTower3;
        public System.Windows.Forms.Label label66;
        public System.Windows.Forms.Label labelDoorStateTower3;
        public System.Windows.Forms.Label label70;
        public System.Windows.Forms.Label label71;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelReelTower4;
        public System.Windows.Forms.Label label76;
        public System.Windows.Forms.Label labelDoorStateTower4;
        public System.Windows.Forms.Label label78;
        public System.Windows.Forms.Label label79;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTransferReelInformation;
        public System.Windows.Forms.Label labelTransferReelInformation;
        public System.Windows.Forms.Label labelReelTransferDestination;
        public System.Windows.Forms.Label labelReelTransferSource;
        public System.Windows.Forms.Label labelReelTransferMode;
        public System.Windows.Forms.Label labelTransferMode;
        public System.Windows.Forms.Label labelTrasferSource;
        public System.Windows.Forms.Label labelTransferDestination;
        public System.Windows.Forms.Label labelMobileRobot;
        public System.Windows.Forms.Button buttonMobileRobotControllerUsage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTimeout;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelNetwork;
        private System.Windows.Forms.TextBox textBoxRobotControllerAddress;
        private System.Windows.Forms.TextBox textBoxReelTowerAddress;
        private System.Windows.Forms.TextBox textBoxReelTowerPort;
        private System.Windows.Forms.TextBox textBoxMobileRobotAddress;
        private System.Windows.Forms.TextBox textBoxMobileRobotPort;
        private System.Windows.Forms.TextBox textBoxRobotControllerPort;
        private System.Windows.Forms.TextBox textBoxRobotPort;
        private System.Windows.Forms.Button buttonSaveNetworkSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMainFrame;
        private System.Windows.Forms.Button buttonClearLog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPerformance;
        public System.Windows.Forms.Label labelVisionDecodeError;
        public System.Windows.Forms.Label labelVisionDecodeErrorValue;
        public System.Windows.Forms.Label labelVisionAlignError;
        public System.Windows.Forms.Label labelVisionAlignErrorValue;
        public System.Windows.Forms.Label labelReturnErrorCount;
        public System.Windows.Forms.Label labelReturnErrorCountValue;
        public System.Windows.Forms.Label labelUnloadErrorCount;
        public System.Windows.Forms.Label labelUnloadErrorCountValue;
        public System.Windows.Forms.Label labelLoadErrorCount;
        public System.Windows.Forms.Label labelLoadErrorCountValue;
        public System.Windows.Forms.Label labelTotalReturn;
        public System.Windows.Forms.Label labelTotalReturnReelCountValue;
        public System.Windows.Forms.Label labelTotalUnloadReelCount;
        public System.Windows.Forms.Label labelTotalUnloadReelCountValue;
        public System.Windows.Forms.Label labelTotalLoadReelCount;
        public System.Windows.Forms.Label labelElapsedValue;
        public System.Windows.Forms.Label labelElapsed;
        public System.Windows.Forms.Label labelTotalLoadReelCountValue;
        private System.Windows.Forms.GroupBox groupBoxRuntimeTrace;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRuntimeTrace;
        public System.Windows.Forms.Label labelReelTowerStep;
        public System.Windows.Forms.Label labelReelTowerStepValue;
        public System.Windows.Forms.Label labelMobileRobotStep;
        public System.Windows.Forms.Label labelMobileRobotStepValue;
        public System.Windows.Forms.Label labelRobotStepValue;
        public System.Windows.Forms.Label labelRobotStep;
        public System.Windows.Forms.Label label54;
        public System.Windows.Forms.Label label56;
        public System.Windows.Forms.Label label57;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPickingList;
        public System.Windows.Forms.Label labelOutputReelCountValue;
        public System.Windows.Forms.Label labelOutputLocationValue;
        public System.Windows.Forms.Label labelPickingIdValue;
        public System.Windows.Forms.Label labelPickingId;
        public System.Windows.Forms.Label labelOutputReelList;
        public System.Windows.Forms.Label labelOutputLocation;
        public System.Windows.Forms.Label labelOutputReelCount;
        private System.Windows.Forms.ListBox listBoxOutputReelList;
        public System.Windows.Forms.Label labelPickingList;
        public System.Windows.Forms.Button buttonInitialize;
        private System.Windows.Forms.GroupBox groupBoxLoadReel;
        public System.Windows.Forms.Label labelReelTransferStateValue;
        public System.Windows.Forms.Label labelReelTransferState;
        private System.Windows.Forms.GroupBox groupBoxUnloadReel;
        private System.Windows.Forms.GroupBox groupBoxCartInOut;
        private System.Windows.Forms.TabPage tabPageMaintenance;
        private System.Windows.Forms.GroupBox groupBoxDigitalIo;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelModelVisionProperties;
        private System.Windows.Forms.Label labelVisionAlignmentRangeLimit;
        private System.Windows.Forms.TextBox textBoxVisionAlignmentRangeLimitX;
        private System.Windows.Forms.TextBox textBoxVisionFailureRetry;
        private System.Windows.Forms.Label labelVisionFailureRetry;
        private System.Windows.Forms.TextBox textBoxVisionAlignmentRangeLimitY;
        private System.Windows.Forms.GroupBox groupBoxDelayProperties;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelModelDelayProperties;
        private System.Windows.Forms.Label labelReturnReelSensingDelay;
        private System.Windows.Forms.TextBox textBoxReturnReelSensingDelay;
        private System.Windows.Forms.TextBox textBoxImageProcessingDelay;
        private System.Windows.Forms.Label labelReelSizeDetectingDelay;
        private System.Windows.Forms.Label labelRobotMoveTimeout;
        private System.Windows.Forms.Label labelRobotCommunicationTimeout;
        private System.Windows.Forms.Label labelRobotProgramLoadTimeout;
        private System.Windows.Forms.Label labelRobotProgramPlayTimeout;
        private System.Windows.Forms.Label labelRobotActionTimeout;
        private System.Windows.Forms.TextBox textBoxRobotCommunicationTimeout;
        private System.Windows.Forms.TextBox textBoxRobotProgramLoadTimeout;
        private System.Windows.Forms.TextBox textBoxRobotProgramPlayTimeout;
        private System.Windows.Forms.TextBox textBoxRobotActionTimeout;
        private System.Windows.Forms.TextBox textBoxRobotMoveTimeout;
        private System.Windows.Forms.Label labelRobotHomeTimeout;
        private System.Windows.Forms.TextBox textBoxRobotHomeTimeout;
        private System.Windows.Forms.Button buttonSaveTimeoutProperties;
        private System.Windows.Forms.Button buttonSaveModel;
        private System.Windows.Forms.GroupBox groupBoxModel;
        private System.Windows.Forms.Button buttonVisionStart;
        public System.Windows.Forms.Label labelVision;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Button buttonLightOff;
        private System.Windows.Forms.Button buttonLightOnReel13;
        private System.Windows.Forms.GroupBox groupBoxVisionAndLight;
        private System.Windows.Forms.Button buttonLightOnReel7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelVisionAndLight;
        private System.Windows.Forms.Button buttonVisionFindCenter;
        private System.Windows.Forms.Button buttonVisionGrabReel7;
        private System.Windows.Forms.Button buttonReadBarcode;
        public System.Windows.Forms.Label labelReel13;
        public System.Windows.Forms.Label labelReel7;
        private System.Windows.Forms.Button buttonVisionGrabReel13;
        public System.Windows.Forms.Label labelDecodeBarcodeResult;
        public System.Windows.Forms.Label labelAlignmentResult;
        public System.Windows.Forms.Label labelApplicationVersion;
        private System.Windows.Forms.Label labelVisionRetryAttempts;
        private System.Windows.Forms.Label labelUnloadReelStateUpdateDelay;
        private System.Windows.Forms.TextBox textBoxUnloadReelStateUpdateDelay;
        private System.Windows.Forms.TextBox textBoxVisioinRetryAttempts;
        public System.Windows.Forms.Label labelOutputReelDateTimeValue;
        public System.Windows.Forms.Label labelOutputReelDateTime;
        private System.Windows.Forms.Button buttonDock;
        private System.Windows.Forms.GroupBox groupBoxAnimation;
        private System.Windows.Forms.Label labelNextWaypointRSValue;
        private System.Windows.Forms.Label labelCurrentWaypointRSValue;
        private System.Windows.Forms.Label labelLastCommandRSValue;
        private System.Windows.Forms.Label labelTransferStateRSValue;
        private System.Windows.Forms.Label labelTransferModeRSValue;
        private System.Windows.Forms.Button buttonVisionRestart;
        private System.Windows.Forms.CheckBox checkBoxVisionProcessProductionMode;
        private System.Windows.Forms.CheckBox checkBoxVisionProcessCompressImage;
        private System.Windows.Forms.CheckBox checkBoxVisionProcessSaveImage;
        private System.Windows.Forms.GroupBox groupBoxVisionProcessImage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox textBoxVisionProcessRejectImagePath;
        private System.Windows.Forms.Label labelVisionProcessRejectImageFilePath;
        private System.Windows.Forms.Label labelVisionProcessAcceptImagePath;
        private System.Windows.Forms.Label labelVisionProcessImageFileExtension;
        private System.Windows.Forms.TextBox textBoxVisionProcessImageFileExtension;
        private System.Windows.Forms.TextBox textBoxVisionProcessImageFileNameFormat;
        private System.Windows.Forms.Label labelVisionProcessImageFileNameFormat;
        private System.Windows.Forms.TextBox textBoxVisionProcessAcceptImagePath;
        private System.Windows.Forms.TabPage tabPageVision;
        private Gui.ControlDigitalIo controlDigitalIo1;
        private Gui.ControlStatusLabel controlStatusLabelCompleteUnload;
        private Gui.ControlStatusLabel controlStatusLabelPutUnloadReelToOutput;
        private Gui.ControlStatusLabel controlStatusLabelTakeReelFromTower;
        private Gui.ControlStatusLabel controlStatusLabelMoveToTower;
        private Gui.ControlStatusLabel controlStatusLabelPickList;
        private Gui.ControlStatusLabel controlStatusLabelCartInOut;
        private Gui.ControlStatusLabel controlStatusLabelMrbtDocking;
        private Gui.ControlStatusLabel controlStatusLabelMrbtMove;
        private Gui.ControlStatusLabel controlStatusLabelCompleteLoad;
        private Gui.ControlStatusLabel controlStatusLabelPutReel;
        private Gui.ControlStatusLabel controlStatusLabelPickup;
        private Gui.ControlStatusLabel controlStatusLabelDecodeQrBarcode;
        private Gui.ControlStatusLabel controlStatusLabelVisionAlign;
        private Gui.ControlStatusLabel controlStatusLabelCartInchCheck;
        private System.Windows.Forms.Label labelDelayOfCameraTriggger;
        private System.Windows.Forms.Label labelImageProcessingTimeout;
        private System.Windows.Forms.TextBox textBoxImageProcessingTimeout;
        private System.Windows.Forms.TextBox textBoxDelayOfCameraTrigger;
        private System.Windows.Forms.Button buttonSaveGuiSettings;
        private System.Windows.Forms.GroupBox groupBoxGuiSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label labelDisplayLanguage;
        private System.Windows.Forms.Label labelLastCycleTime;
        public System.Windows.Forms.Button buttonShowQueue;
        private System.Windows.Forms.TabPage tabPageAlarmHistory;
        public System.Windows.Forms.ListBox listBoxAlarmHistory;
        private System.Windows.Forms.TreeView treeViewLog;
        private System.Windows.Forms.ImageList imageListFileSystem;
        protected System.Windows.Forms.ComboBox comboBoxDisplayLanguage;
        private Vision.CompositeImageProcessControl visionControl1;
    }
}

