using System.Windows.Forms;

namespace TechFloor.Vision
{
  partial class CompositeImageProcessControl
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        if (mJM != null)
          mJM.Shutdown();
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    // cognex.wizard.initializecomponent
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            this.cogToolPropertyProvider = new Cognex.VisionPro.CogToolPropertyProvider();
            this.tabPage_Job0_URL = new System.Windows.Forms.TabPage();
            this.groupBox_RESULT = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.label_Job0_CODE = new System.Windows.Forms.Label();
            this.textBox_Job0_MESSAGE = new System.Windows.Forms.TextBox();
            this.label_Job0_DECODEDATA = new System.Windows.Forms.Label();
            this.textBox_Job0_REMAINEDPPM = new System.Windows.Forms.TextBox();
            this.label_Job0_REMAINEDPPM = new System.Windows.Forms.Label();
            this.textBox_Job0_BARCODEPPM = new System.Windows.Forms.TextBox();
            this.label_Job0_BARCODEPPM = new System.Windows.Forms.Label();
            this.textBox_Job0_QRCOUNT = new System.Windows.Forms.TextBox();
            this.label_Job0_QRCOUNT = new System.Windows.Forms.Label();
            this.textBox_Job0_REMAINED = new System.Windows.Forms.TextBox();
            this.label_Job0_REMAINED = new System.Windows.Forms.Label();
            this.textBox_Job0_RMSERRORVALUE = new System.Windows.Forms.TextBox();
            this.label_Job0_RMSERRORValue = new System.Windows.Forms.Label();
            this.textBox_Job0_SCALING = new System.Windows.Forms.TextBox();
            this.label_Job0_SCALING = new System.Windows.Forms.Label();
            this.textBox_Job0_RADIUS = new System.Windows.Forms.TextBox();
            this.label_Job0_RADIUS = new System.Windows.Forms.Label();
            this.textBox_Job0_SLOTEMPTY = new System.Windows.Forms.TextBox();
            this.label_Job0_SLOTEMPTY = new System.Windows.Forms.Label();
            this.textBox_Job0_SCORE = new System.Windows.Forms.TextBox();
            this.label_Job0_SCORE = new System.Windows.Forms.Label();
            this.textBox_Job0_CENTERY = new System.Windows.Forms.TextBox();
            this.textBox_Job0_CENTERX = new System.Windows.Forms.TextBox();
            this.label_Job0_CENTERX = new System.Windows.Forms.Label();
            this.label_Job0_CENTERY = new System.Windows.Forms.Label();
            this.textBox_Job0_CARTTYPE = new System.Windows.Forms.TextBox();
            this.label_Job0_CARTTYPE = new System.Windows.Forms.Label();
            this.textBox_Job0_TOTALTIME = new System.Windows.Forms.TextBox();
            this.label_Job0_TOTALTIME = new System.Windows.Forms.Label();
            this.textBox_Job0_PSTIME = new System.Windows.Forms.TextBox();
            this.label_Job0_PSTIME = new System.Windows.Forms.Label();
            this.textBox_Job0_STATUS = new System.Windows.Forms.TextBox();
            this.textBox_Job0_CODE = new System.Windows.Forms.TextBox();
            this.label_Job0_STATUS = new System.Windows.Forms.Label();
            this.textBox_Job0_DECODEDATA = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOpenLight = new System.Windows.Forms.Button();
            this.button_Configuration = new System.Windows.Forms.Button();
            this.checkBox_LiveDisplay = new System.Windows.Forms.CheckBox();
            this.button_SaveSettings = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.groupBox_MODEL = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBox_Job0_USEFOUNDCIRCLEVERIFY = new System.Windows.Forms.CheckBox();
            this.textBox_Job0_REEL13 = new Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox();
            this.label_Job0_REEL13 = new System.Windows.Forms.Label();
            this.textBox_Job0_REEL7 = new Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox();
            this.label_Job0_REEL7 = new System.Windows.Forms.Label();
            this.textBox_Job0_TARGETPOINT = new Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox();
            this.label_Job0_TARGETPOINT = new System.Windows.Forms.Label();
            this.textBox_Job0_LIMITY = new Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox();
            this.label_Job0_LIMITY = new System.Windows.Forms.Label();
            this.textBox_Job0_LIMITX = new Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox();
            this.label_Job0_LIMITX = new System.Windows.Forms.Label();
            this.textBox_Job0_SCORELIMIT = new Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox();
            this.label_Job0_SCORELIMIT = new System.Windows.Forms.Label();
            this.textBox_Job0_RMSERROR = new Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox();
            this.label_Job0_RMSERROR = new System.Windows.Forms.Label();
            this.textBox_Job0_PROCESS = new Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox();
            this.label_Job0_PROCESS = new System.Windows.Forms.Label();
            this.checkBox_Job0_USEREELCENTERPATTERN = new System.Windows.Forms.CheckBox();
            this.tabPage_Job0_BASEPOINT = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonBasePointRun = new System.Windows.Forms.Button();
            this.groupBox_RESULT_1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.textBox_Job0_DISAngle = new System.Windows.Forms.TextBox();
            this.textBox_Job0_DISY = new System.Windows.Forms.TextBox();
            this.textBox_Job0_DISX = new System.Windows.Forms.TextBox();
            this.label_Job0_DISX = new System.Windows.Forms.Label();
            this.label_Job0_DISY = new System.Windows.Forms.Label();
            this.label_Job0_DISAngle = new System.Windows.Forms.Label();
            this.textBox_Job0_2ndAngle = new System.Windows.Forms.TextBox();
            this.textBox_Job0_2ndY = new System.Windows.Forms.TextBox();
            this.textBox_Job0_2ndX = new System.Windows.Forms.TextBox();
            this.label_Job0_2ndX = new System.Windows.Forms.Label();
            this.label_Job0_2ndY = new System.Windows.Forms.Label();
            this.label_Job0_2ndAngle = new System.Windows.Forms.Label();
            this.textBox_Job0_1stAngle = new System.Windows.Forms.TextBox();
            this.label_Job0_1stAngle = new System.Windows.Forms.Label();
            this.textBox_Job0_1stY = new System.Windows.Forms.TextBox();
            this.textBox_Job0_1stX = new System.Windows.Forms.TextBox();
            this.textBox_Job0_FOUND2nd = new System.Windows.Forms.TextBox();
            this.textBox_Job0_FOUND1st = new System.Windows.Forms.TextBox();
            this.textBox_Job0_VERIFIED = new System.Windows.Forms.TextBox();
            this.label_Job0_VERIFIED = new System.Windows.Forms.Label();
            this.label_Job0_FOUND1st = new System.Windows.Forms.Label();
            this.label_Job0_FOUND2nd = new System.Windows.Forms.Label();
            this.label_Job0_1stX = new System.Windows.Forms.Label();
            this.label_Job0_1stY = new System.Windows.Forms.Label();
            this.groupBox_PARAMETERS = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.label_Job0_REFERENCEX = new System.Windows.Forms.Label();
            this.textBox_Job0_BASEPOINTMODE = new Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox();
            this.label_Job0_BASEPOINTMODE = new System.Windows.Forms.Label();
            this.textBox_Job0_REFERENCEX = new Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox();
            this.label_Job0_REFERENCEY = new System.Windows.Forms.Label();
            this.textBox_Job0_REFERENCEY = new Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox();
            this.label_AcquisitionResults_Overruns = new System.Windows.Forms.Label();
            this.textBox_JobN_TotalAcquisitionErrors = new System.Windows.Forms.TextBox();
            this.textBox_JobN_TotalAcquisitionOverruns = new System.Windows.Forms.TextBox();
            this.groupBox_AcquisitionResults = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label_AcquisitionResults_TotalAcquisitions = new System.Windows.Forms.Label();
            this.label_AcquisitionResults_Errors = new System.Windows.Forms.Label();
            this.textBox_JobN_TotalAcquisitions = new System.Windows.Forms.TextBox();
            this.label_JobThroughput_persec = new System.Windows.Forms.Label();
            this.groupBox_JobThroughput = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label_JobThroughput_TotalThroughput = new System.Windows.Forms.Label();
            this.label_JobThroughput_Max = new System.Windows.Forms.Label();
            this.textBox_JobN_MaxThroughput = new System.Windows.Forms.TextBox();
            this.label_JobThroughput_Min = new System.Windows.Forms.Label();
            this.textBox_JobN_MinThroughput = new System.Windows.Forms.TextBox();
            this.textBox_JobN_Throughput = new System.Windows.Forms.TextBox();
            this.tabControlJobTabs = new System.Windows.Forms.TabControl();
            this.tabPage_JobN_JobStatistics = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label_ResultBar = new System.Windows.Forms.Label();
            this.button_ResetStatistics = new System.Windows.Forms.Button();
            this.button_ResetStatisticsForAllJobs = new System.Windows.Forms.Button();
            this.label_controlErrorMessage = new System.Windows.Forms.Label();
            this.groupBox_JobResults = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label_JobResults_Percent = new System.Windows.Forms.Label();
            this.label_JobResults_TotalIterations = new System.Windows.Forms.Label();
            this.textBox_JobN_TotalAccept_Percent = new System.Windows.Forms.TextBox();
            this.label_JobResults_Accept = new System.Windows.Forms.Label();
            this.textBox_JobN_TotalError = new System.Windows.Forms.TextBox();
            this.textBox_JobN_TotalWarning = new System.Windows.Forms.TextBox();
            this.label_JobResults_Error = new System.Windows.Forms.Label();
            this.textBox_JobN_TotalIterations = new System.Windows.Forms.TextBox();
            this.textBox_JobN_TotalAccept = new System.Windows.Forms.TextBox();
            this.label_JobResults_Reject = new System.Windows.Forms.Label();
            this.label_JobResults_Warning = new System.Windows.Forms.Label();
            this.textBox_JobN_TotalReject = new System.Windows.Forms.TextBox();
            this.tabPage_Job0_GUIDEPOINT = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonGuidePointRun = new System.Windows.Forms.Button();
            this.groupBox_GuidePoint_Result = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.textBox_Job0_1stY_1 = new System.Windows.Forms.TextBox();
            this.textBox_Job0_1stX_1 = new System.Windows.Forms.TextBox();
            this.textBox_Job0_VERIFIED_1 = new System.Windows.Forms.TextBox();
            this.label_Job0_VERIFIED_1 = new System.Windows.Forms.Label();
            this.label_Job0_1stX_1 = new System.Windows.Forms.Label();
            this.label_Job0_1stY_1 = new System.Windows.Forms.Label();
            this.label_Job0_1stAngle_1 = new System.Windows.Forms.Label();
            this.textBox_Job0_1stAngle_1 = new System.Windows.Forms.TextBox();
            this.label_Job0_2ndX_1 = new System.Windows.Forms.Label();
            this.textBox_Job0_2ndAngle_1 = new System.Windows.Forms.TextBox();
            this.textBox_Job0_2ndY_1 = new System.Windows.Forms.TextBox();
            this.textBox_Job0_2ndX_1 = new System.Windows.Forms.TextBox();
            this.label_Job0_2ndY_1 = new System.Windows.Forms.Label();
            this.label_Job0_2ndAngle_1 = new System.Windows.Forms.Label();
            this.label_Job0_3rdX = new System.Windows.Forms.Label();
            this.label_Job0_3rdY = new System.Windows.Forms.Label();
            this.label_Job0_3rdAngle = new System.Windows.Forms.Label();
            this.textBox_Job0_3rdX = new System.Windows.Forms.TextBox();
            this.textBox_Job0_3rdY = new System.Windows.Forms.TextBox();
            this.textBox_Job0_3rdAngle = new System.Windows.Forms.TextBox();
            this.groupBox_GuidePoint_Parameters = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.label_Job0_PITCHX = new System.Windows.Forms.Label();
            this.label_Job0_PITCHY = new System.Windows.Forms.Label();
            this.textBox_Job0_PITCHX = new Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox();
            this.textBox_Job0_PITCHY = new Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox();
            this.textBox_Job0_GUIDEPOINTMODE = new Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox();
            this.label_Job0_GUIDEPOINTMODE = new System.Windows.Forms.Label();
            this.applicationErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.splitContainerCompositeImageProcessControl = new System.Windows.Forms.SplitContainer();
            this.cogRecordsDisplay1 = new Cognex.VisionPro.CogRecordsDisplay();
            this.tabPage_Job0_URL.SuspendLayout();
            this.groupBox_RESULT.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.groupBox_MODEL.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tabPage_Job0_BASEPOINT.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.groupBox_RESULT_1.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.groupBox_PARAMETERS.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.groupBox_AcquisitionResults.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox_JobThroughput.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabControlJobTabs.SuspendLayout();
            this.tabPage_JobN_JobStatistics.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox_JobResults.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabPage_Job0_GUIDEPOINT.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.groupBox_GuidePoint_Result.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.groupBox_GuidePoint_Parameters.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.applicationErrorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerCompositeImageProcessControl)).BeginInit();
            this.splitContainerCompositeImageProcessControl.Panel1.SuspendLayout();
            this.splitContainerCompositeImageProcessControl.Panel2.SuspendLayout();
            this.splitContainerCompositeImageProcessControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // cogToolPropertyProvider
            // 
            this.cogToolPropertyProvider.ElectricProvider = null;
            this.cogToolPropertyProvider.EnableDelegateQueuing = false;
            this.cogToolPropertyProvider.ErrorProvider = null;
            this.cogToolPropertyProvider.Subject = null;
            this.cogToolPropertyProvider.SubjectInUse = false;
            // 
            // tabPage_Job0_URL
            // 
            this.tabPage_Job0_URL.AutoScroll = true;
            this.tabPage_Job0_URL.Controls.Add(this.groupBox_RESULT);
            this.tabPage_Job0_URL.Controls.Add(this.tableLayoutPanel7);
            this.tabPage_Job0_URL.Controls.Add(this.groupBox_MODEL);
            this.tabPage_Job0_URL.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage_Job0_URL.Location = new System.Drawing.Point(4, 24);
            this.tabPage_Job0_URL.Name = "tabPage_Job0_URL";
            this.tabPage_Job0_URL.Padding = new System.Windows.Forms.Padding(8);
            this.tabPage_Job0_URL.Size = new System.Drawing.Size(536, 1020);
            this.tabPage_Job0_URL.TabIndex = 0;
            this.tabPage_Job0_URL.Text = "URL";
            // 
            // groupBox_RESULT
            // 
            this.groupBox_RESULT.Controls.Add(this.tableLayoutPanel6);
            this.groupBox_RESULT.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_RESULT.Location = new System.Drawing.Point(8, 194);
            this.groupBox_RESULT.Name = "groupBox_RESULT";
            this.groupBox_RESULT.Padding = new System.Windows.Forms.Padding(8);
            this.groupBox_RESULT.Size = new System.Drawing.Size(520, 430);
            this.groupBox_RESULT.TabIndex = 0;
            this.groupBox_RESULT.TabStop = false;
            this.groupBox_RESULT.Text = "RESULT";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 4;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.Controls.Add(this.label_Job0_CODE, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.textBox_Job0_MESSAGE, 0, 11);
            this.tableLayoutPanel6.Controls.Add(this.label_Job0_DECODEDATA, 0, 8);
            this.tableLayoutPanel6.Controls.Add(this.textBox_Job0_REMAINEDPPM, 3, 7);
            this.tableLayoutPanel6.Controls.Add(this.label_Job0_REMAINEDPPM, 2, 7);
            this.tableLayoutPanel6.Controls.Add(this.textBox_Job0_BARCODEPPM, 1, 7);
            this.tableLayoutPanel6.Controls.Add(this.label_Job0_BARCODEPPM, 0, 7);
            this.tableLayoutPanel6.Controls.Add(this.textBox_Job0_QRCOUNT, 1, 6);
            this.tableLayoutPanel6.Controls.Add(this.label_Job0_QRCOUNT, 0, 6);
            this.tableLayoutPanel6.Controls.Add(this.textBox_Job0_REMAINED, 3, 6);
            this.tableLayoutPanel6.Controls.Add(this.label_Job0_REMAINED, 2, 6);
            this.tableLayoutPanel6.Controls.Add(this.textBox_Job0_RMSERRORVALUE, 3, 5);
            this.tableLayoutPanel6.Controls.Add(this.label_Job0_RMSERRORValue, 2, 5);
            this.tableLayoutPanel6.Controls.Add(this.textBox_Job0_SCALING, 1, 5);
            this.tableLayoutPanel6.Controls.Add(this.label_Job0_SCALING, 0, 5);
            this.tableLayoutPanel6.Controls.Add(this.textBox_Job0_RADIUS, 3, 4);
            this.tableLayoutPanel6.Controls.Add(this.label_Job0_RADIUS, 2, 4);
            this.tableLayoutPanel6.Controls.Add(this.textBox_Job0_SLOTEMPTY, 1, 3);
            this.tableLayoutPanel6.Controls.Add(this.label_Job0_SLOTEMPTY, 0, 3);
            this.tableLayoutPanel6.Controls.Add(this.textBox_Job0_SCORE, 1, 4);
            this.tableLayoutPanel6.Controls.Add(this.label_Job0_SCORE, 0, 4);
            this.tableLayoutPanel6.Controls.Add(this.textBox_Job0_CENTERY, 3, 3);
            this.tableLayoutPanel6.Controls.Add(this.textBox_Job0_CENTERX, 3, 2);
            this.tableLayoutPanel6.Controls.Add(this.label_Job0_CENTERX, 2, 2);
            this.tableLayoutPanel6.Controls.Add(this.label_Job0_CENTERY, 2, 3);
            this.tableLayoutPanel6.Controls.Add(this.textBox_Job0_CARTTYPE, 1, 2);
            this.tableLayoutPanel6.Controls.Add(this.label_Job0_CARTTYPE, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.textBox_Job0_TOTALTIME, 3, 1);
            this.tableLayoutPanel6.Controls.Add(this.label_Job0_TOTALTIME, 2, 1);
            this.tableLayoutPanel6.Controls.Add(this.textBox_Job0_PSTIME, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.label_Job0_PSTIME, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.textBox_Job0_STATUS, 3, 0);
            this.tableLayoutPanel6.Controls.Add(this.textBox_Job0_CODE, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.label_Job0_STATUS, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.textBox_Job0_DECODEDATA, 0, 9);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(8, 27);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 13;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(504, 395);
            this.tableLayoutPanel6.TabIndex = 0;
            // 
            // label_Job0_CODE
            // 
            this.label_Job0_CODE.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Job0_CODE.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_CODE.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_CODE.Location = new System.Drawing.Point(3, 4);
            this.label_Job0_CODE.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_Job0_CODE.Name = "label_Job0_CODE";
            this.label_Job0_CODE.Size = new System.Drawing.Size(120, 22);
            this.label_Job0_CODE.TabIndex = 0;
            this.label_Job0_CODE.Text = "CODE";
            this.label_Job0_CODE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_MESSAGE
            // 
            this.textBox_Job0_MESSAGE.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanel6.SetColumnSpan(this.textBox_Job0_MESSAGE, 4);
            this.textBox_Job0_MESSAGE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_MESSAGE.Location = new System.Drawing.Point(3, 334);
            this.textBox_Job0_MESSAGE.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_Job0_MESSAGE.Multiline = true;
            this.textBox_Job0_MESSAGE.Name = "textBox_Job0_MESSAGE";
            this.textBox_Job0_MESSAGE.ReadOnly = true;
            this.tableLayoutPanel6.SetRowSpan(this.textBox_Job0_MESSAGE, 2);
            this.textBox_Job0_MESSAGE.Size = new System.Drawing.Size(498, 57);
            this.textBox_Job0_MESSAGE.TabIndex = 0;
            this.textBox_Job0_MESSAGE.TabStop = false;
            this.textBox_Job0_MESSAGE.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label_Job0_DECODEDATA
            // 
            this.label_Job0_DECODEDATA.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.tableLayoutPanel6.SetColumnSpan(this.label_Job0_DECODEDATA, 4);
            this.label_Job0_DECODEDATA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_DECODEDATA.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_DECODEDATA.Location = new System.Drawing.Point(3, 244);
            this.label_Job0_DECODEDATA.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_Job0_DECODEDATA.Name = "label_Job0_DECODEDATA";
            this.label_Job0_DECODEDATA.Size = new System.Drawing.Size(498, 22);
            this.label_Job0_DECODEDATA.TabIndex = 0;
            this.label_Job0_DECODEDATA.Text = "DECODE DATA";
            this.label_Job0_DECODEDATA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_REMAINEDPPM
            // 
            this.textBox_Job0_REMAINEDPPM.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_REMAINEDPPM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_REMAINEDPPM.Location = new System.Drawing.Point(381, 214);
            this.textBox_Job0_REMAINEDPPM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_Job0_REMAINEDPPM.Name = "textBox_Job0_REMAINEDPPM";
            this.textBox_Job0_REMAINEDPPM.ReadOnly = true;
            this.textBox_Job0_REMAINEDPPM.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_REMAINEDPPM.TabIndex = 0;
            this.textBox_Job0_REMAINEDPPM.TabStop = false;
            this.textBox_Job0_REMAINEDPPM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_REMAINEDPPM
            // 
            this.label_Job0_REMAINEDPPM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Job0_REMAINEDPPM.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_REMAINEDPPM.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_REMAINEDPPM.Location = new System.Drawing.Point(255, 214);
            this.label_Job0_REMAINEDPPM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_Job0_REMAINEDPPM.Name = "label_Job0_REMAINEDPPM";
            this.label_Job0_REMAINEDPPM.Size = new System.Drawing.Size(120, 22);
            this.label_Job0_REMAINEDPPM.TabIndex = 0;
            this.label_Job0_REMAINEDPPM.Text = "PPM";
            this.label_Job0_REMAINEDPPM.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_BARCODEPPM
            // 
            this.textBox_Job0_BARCODEPPM.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_BARCODEPPM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_BARCODEPPM.Location = new System.Drawing.Point(129, 214);
            this.textBox_Job0_BARCODEPPM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_Job0_BARCODEPPM.Name = "textBox_Job0_BARCODEPPM";
            this.textBox_Job0_BARCODEPPM.ReadOnly = true;
            this.textBox_Job0_BARCODEPPM.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_BARCODEPPM.TabIndex = 0;
            this.textBox_Job0_BARCODEPPM.TabStop = false;
            this.textBox_Job0_BARCODEPPM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_BARCODEPPM
            // 
            this.label_Job0_BARCODEPPM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Job0_BARCODEPPM.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_BARCODEPPM.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_BARCODEPPM.Location = new System.Drawing.Point(3, 214);
            this.label_Job0_BARCODEPPM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_Job0_BARCODEPPM.Name = "label_Job0_BARCODEPPM";
            this.label_Job0_BARCODEPPM.Size = new System.Drawing.Size(120, 22);
            this.label_Job0_BARCODEPPM.TabIndex = 0;
            this.label_Job0_BARCODEPPM.Text = "PPM";
            this.label_Job0_BARCODEPPM.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_QRCOUNT
            // 
            this.textBox_Job0_QRCOUNT.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_QRCOUNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_QRCOUNT.Location = new System.Drawing.Point(129, 184);
            this.textBox_Job0_QRCOUNT.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_Job0_QRCOUNT.Name = "textBox_Job0_QRCOUNT";
            this.textBox_Job0_QRCOUNT.ReadOnly = true;
            this.textBox_Job0_QRCOUNT.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_QRCOUNT.TabIndex = 0;
            this.textBox_Job0_QRCOUNT.TabStop = false;
            this.textBox_Job0_QRCOUNT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_QRCOUNT
            // 
            this.label_Job0_QRCOUNT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Job0_QRCOUNT.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_QRCOUNT.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_QRCOUNT.Location = new System.Drawing.Point(3, 184);
            this.label_Job0_QRCOUNT.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_Job0_QRCOUNT.Name = "label_Job0_QRCOUNT";
            this.label_Job0_QRCOUNT.Size = new System.Drawing.Size(120, 22);
            this.label_Job0_QRCOUNT.TabIndex = 0;
            this.label_Job0_QRCOUNT.Text = "QR COUNT";
            this.label_Job0_QRCOUNT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_REMAINED
            // 
            this.textBox_Job0_REMAINED.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_REMAINED.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_REMAINED.Location = new System.Drawing.Point(381, 184);
            this.textBox_Job0_REMAINED.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_Job0_REMAINED.Name = "textBox_Job0_REMAINED";
            this.textBox_Job0_REMAINED.ReadOnly = true;
            this.textBox_Job0_REMAINED.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_REMAINED.TabIndex = 0;
            this.textBox_Job0_REMAINED.TabStop = false;
            this.textBox_Job0_REMAINED.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_REMAINED
            // 
            this.label_Job0_REMAINED.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Job0_REMAINED.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_REMAINED.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_REMAINED.Location = new System.Drawing.Point(255, 184);
            this.label_Job0_REMAINED.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_Job0_REMAINED.Name = "label_Job0_REMAINED";
            this.label_Job0_REMAINED.Size = new System.Drawing.Size(120, 22);
            this.label_Job0_REMAINED.TabIndex = 0;
            this.label_Job0_REMAINED.Text = "REMAINED";
            this.label_Job0_REMAINED.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_RMSERRORVALUE
            // 
            this.textBox_Job0_RMSERRORVALUE.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_RMSERRORVALUE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_RMSERRORVALUE.Location = new System.Drawing.Point(381, 154);
            this.textBox_Job0_RMSERRORVALUE.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_Job0_RMSERRORVALUE.Name = "textBox_Job0_RMSERRORVALUE";
            this.textBox_Job0_RMSERRORVALUE.ReadOnly = true;
            this.textBox_Job0_RMSERRORVALUE.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_RMSERRORVALUE.TabIndex = 0;
            this.textBox_Job0_RMSERRORVALUE.TabStop = false;
            this.textBox_Job0_RMSERRORVALUE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_RMSERRORValue
            // 
            this.label_Job0_RMSERRORValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Job0_RMSERRORValue.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_RMSERRORValue.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_RMSERRORValue.Location = new System.Drawing.Point(255, 154);
            this.label_Job0_RMSERRORValue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_Job0_RMSERRORValue.Name = "label_Job0_RMSERRORValue";
            this.label_Job0_RMSERRORValue.Size = new System.Drawing.Size(120, 22);
            this.label_Job0_RMSERRORValue.TabIndex = 0;
            this.label_Job0_RMSERRORValue.Text = "RMS ERROR";
            this.label_Job0_RMSERRORValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_SCALING
            // 
            this.textBox_Job0_SCALING.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_SCALING.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_SCALING.Location = new System.Drawing.Point(129, 154);
            this.textBox_Job0_SCALING.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_Job0_SCALING.Name = "textBox_Job0_SCALING";
            this.textBox_Job0_SCALING.ReadOnly = true;
            this.textBox_Job0_SCALING.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_SCALING.TabIndex = 0;
            this.textBox_Job0_SCALING.TabStop = false;
            this.textBox_Job0_SCALING.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_SCALING
            // 
            this.label_Job0_SCALING.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Job0_SCALING.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_SCALING.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_SCALING.Location = new System.Drawing.Point(3, 154);
            this.label_Job0_SCALING.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_Job0_SCALING.Name = "label_Job0_SCALING";
            this.label_Job0_SCALING.Size = new System.Drawing.Size(120, 22);
            this.label_Job0_SCALING.TabIndex = 0;
            this.label_Job0_SCALING.Text = "SCALING";
            this.label_Job0_SCALING.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_RADIUS
            // 
            this.textBox_Job0_RADIUS.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_RADIUS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_RADIUS.Location = new System.Drawing.Point(381, 124);
            this.textBox_Job0_RADIUS.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_Job0_RADIUS.Name = "textBox_Job0_RADIUS";
            this.textBox_Job0_RADIUS.ReadOnly = true;
            this.textBox_Job0_RADIUS.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_RADIUS.TabIndex = 0;
            this.textBox_Job0_RADIUS.TabStop = false;
            this.textBox_Job0_RADIUS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_RADIUS
            // 
            this.label_Job0_RADIUS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Job0_RADIUS.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_RADIUS.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_RADIUS.Location = new System.Drawing.Point(255, 124);
            this.label_Job0_RADIUS.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_Job0_RADIUS.Name = "label_Job0_RADIUS";
            this.label_Job0_RADIUS.Size = new System.Drawing.Size(120, 22);
            this.label_Job0_RADIUS.TabIndex = 0;
            this.label_Job0_RADIUS.Text = "RADIUS";
            this.label_Job0_RADIUS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_SLOTEMPTY
            // 
            this.textBox_Job0_SLOTEMPTY.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_SLOTEMPTY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_SLOTEMPTY.Location = new System.Drawing.Point(129, 94);
            this.textBox_Job0_SLOTEMPTY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_Job0_SLOTEMPTY.Name = "textBox_Job0_SLOTEMPTY";
            this.textBox_Job0_SLOTEMPTY.ReadOnly = true;
            this.textBox_Job0_SLOTEMPTY.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_SLOTEMPTY.TabIndex = 0;
            this.textBox_Job0_SLOTEMPTY.TabStop = false;
            this.textBox_Job0_SLOTEMPTY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_SLOTEMPTY
            // 
            this.label_Job0_SLOTEMPTY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Job0_SLOTEMPTY.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_SLOTEMPTY.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_SLOTEMPTY.Location = new System.Drawing.Point(3, 94);
            this.label_Job0_SLOTEMPTY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_Job0_SLOTEMPTY.Name = "label_Job0_SLOTEMPTY";
            this.label_Job0_SLOTEMPTY.Size = new System.Drawing.Size(120, 22);
            this.label_Job0_SLOTEMPTY.TabIndex = 0;
            this.label_Job0_SLOTEMPTY.Text = "SLOT EMPTY";
            this.label_Job0_SLOTEMPTY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_SCORE
            // 
            this.textBox_Job0_SCORE.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_SCORE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_SCORE.Location = new System.Drawing.Point(129, 124);
            this.textBox_Job0_SCORE.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_Job0_SCORE.Name = "textBox_Job0_SCORE";
            this.textBox_Job0_SCORE.ReadOnly = true;
            this.textBox_Job0_SCORE.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_SCORE.TabIndex = 0;
            this.textBox_Job0_SCORE.TabStop = false;
            this.textBox_Job0_SCORE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_SCORE
            // 
            this.label_Job0_SCORE.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Job0_SCORE.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_SCORE.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_SCORE.Location = new System.Drawing.Point(3, 124);
            this.label_Job0_SCORE.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_Job0_SCORE.Name = "label_Job0_SCORE";
            this.label_Job0_SCORE.Size = new System.Drawing.Size(120, 22);
            this.label_Job0_SCORE.TabIndex = 0;
            this.label_Job0_SCORE.Text = "SCORE";
            this.label_Job0_SCORE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_CENTERY
            // 
            this.textBox_Job0_CENTERY.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_CENTERY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_CENTERY.Location = new System.Drawing.Point(381, 94);
            this.textBox_Job0_CENTERY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_Job0_CENTERY.Name = "textBox_Job0_CENTERY";
            this.textBox_Job0_CENTERY.ReadOnly = true;
            this.textBox_Job0_CENTERY.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_CENTERY.TabIndex = 0;
            this.textBox_Job0_CENTERY.TabStop = false;
            this.textBox_Job0_CENTERY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Job0_CENTERX
            // 
            this.textBox_Job0_CENTERX.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_CENTERX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_CENTERX.Location = new System.Drawing.Point(381, 64);
            this.textBox_Job0_CENTERX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_Job0_CENTERX.Name = "textBox_Job0_CENTERX";
            this.textBox_Job0_CENTERX.ReadOnly = true;
            this.textBox_Job0_CENTERX.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_CENTERX.TabIndex = 0;
            this.textBox_Job0_CENTERX.TabStop = false;
            this.textBox_Job0_CENTERX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_CENTERX
            // 
            this.label_Job0_CENTERX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Job0_CENTERX.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_CENTERX.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_CENTERX.Location = new System.Drawing.Point(255, 64);
            this.label_Job0_CENTERX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_Job0_CENTERX.Name = "label_Job0_CENTERX";
            this.label_Job0_CENTERX.Size = new System.Drawing.Size(120, 22);
            this.label_Job0_CENTERX.TabIndex = 0;
            this.label_Job0_CENTERX.Text = "CENTER X";
            this.label_Job0_CENTERX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Job0_CENTERY
            // 
            this.label_Job0_CENTERY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Job0_CENTERY.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_CENTERY.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_CENTERY.Location = new System.Drawing.Point(255, 94);
            this.label_Job0_CENTERY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_Job0_CENTERY.Name = "label_Job0_CENTERY";
            this.label_Job0_CENTERY.Size = new System.Drawing.Size(120, 22);
            this.label_Job0_CENTERY.TabIndex = 0;
            this.label_Job0_CENTERY.Text = "CENTER Y";
            this.label_Job0_CENTERY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_CARTTYPE
            // 
            this.textBox_Job0_CARTTYPE.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_CARTTYPE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_CARTTYPE.Location = new System.Drawing.Point(129, 64);
            this.textBox_Job0_CARTTYPE.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_Job0_CARTTYPE.Name = "textBox_Job0_CARTTYPE";
            this.textBox_Job0_CARTTYPE.ReadOnly = true;
            this.textBox_Job0_CARTTYPE.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_CARTTYPE.TabIndex = 0;
            this.textBox_Job0_CARTTYPE.TabStop = false;
            this.textBox_Job0_CARTTYPE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_CARTTYPE
            // 
            this.label_Job0_CARTTYPE.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Job0_CARTTYPE.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_CARTTYPE.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_CARTTYPE.Location = new System.Drawing.Point(3, 64);
            this.label_Job0_CARTTYPE.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_Job0_CARTTYPE.Name = "label_Job0_CARTTYPE";
            this.label_Job0_CARTTYPE.Size = new System.Drawing.Size(120, 22);
            this.label_Job0_CARTTYPE.TabIndex = 0;
            this.label_Job0_CARTTYPE.Text = "CART TYPE";
            this.label_Job0_CARTTYPE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_TOTALTIME
            // 
            this.textBox_Job0_TOTALTIME.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_TOTALTIME.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_TOTALTIME.Location = new System.Drawing.Point(381, 34);
            this.textBox_Job0_TOTALTIME.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_Job0_TOTALTIME.Name = "textBox_Job0_TOTALTIME";
            this.textBox_Job0_TOTALTIME.ReadOnly = true;
            this.textBox_Job0_TOTALTIME.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_TOTALTIME.TabIndex = 0;
            this.textBox_Job0_TOTALTIME.TabStop = false;
            this.textBox_Job0_TOTALTIME.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_TOTALTIME
            // 
            this.label_Job0_TOTALTIME.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Job0_TOTALTIME.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_TOTALTIME.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_TOTALTIME.Location = new System.Drawing.Point(255, 34);
            this.label_Job0_TOTALTIME.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_Job0_TOTALTIME.Name = "label_Job0_TOTALTIME";
            this.label_Job0_TOTALTIME.Size = new System.Drawing.Size(120, 22);
            this.label_Job0_TOTALTIME.TabIndex = 0;
            this.label_Job0_TOTALTIME.Text = "TOTAL TIME";
            this.label_Job0_TOTALTIME.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_PSTIME
            // 
            this.textBox_Job0_PSTIME.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_PSTIME.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_PSTIME.Location = new System.Drawing.Point(129, 34);
            this.textBox_Job0_PSTIME.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_Job0_PSTIME.Name = "textBox_Job0_PSTIME";
            this.textBox_Job0_PSTIME.ReadOnly = true;
            this.textBox_Job0_PSTIME.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_PSTIME.TabIndex = 0;
            this.textBox_Job0_PSTIME.TabStop = false;
            this.textBox_Job0_PSTIME.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_PSTIME
            // 
            this.label_Job0_PSTIME.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Job0_PSTIME.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_PSTIME.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_PSTIME.Location = new System.Drawing.Point(3, 34);
            this.label_Job0_PSTIME.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_Job0_PSTIME.Name = "label_Job0_PSTIME";
            this.label_Job0_PSTIME.Size = new System.Drawing.Size(120, 22);
            this.label_Job0_PSTIME.TabIndex = 0;
            this.label_Job0_PSTIME.Text = "PS TIME";
            this.label_Job0_PSTIME.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_STATUS
            // 
            this.textBox_Job0_STATUS.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_STATUS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_STATUS.Location = new System.Drawing.Point(381, 4);
            this.textBox_Job0_STATUS.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_Job0_STATUS.Name = "textBox_Job0_STATUS";
            this.textBox_Job0_STATUS.ReadOnly = true;
            this.textBox_Job0_STATUS.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_STATUS.TabIndex = 0;
            this.textBox_Job0_STATUS.TabStop = false;
            this.textBox_Job0_STATUS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Job0_CODE
            // 
            this.textBox_Job0_CODE.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_CODE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_CODE.Location = new System.Drawing.Point(129, 4);
            this.textBox_Job0_CODE.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_Job0_CODE.Name = "textBox_Job0_CODE";
            this.textBox_Job0_CODE.ReadOnly = true;
            this.textBox_Job0_CODE.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_CODE.TabIndex = 0;
            this.textBox_Job0_CODE.TabStop = false;
            this.textBox_Job0_CODE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_STATUS
            // 
            this.label_Job0_STATUS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Job0_STATUS.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_STATUS.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_STATUS.Location = new System.Drawing.Point(255, 4);
            this.label_Job0_STATUS.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_Job0_STATUS.Name = "label_Job0_STATUS";
            this.label_Job0_STATUS.Size = new System.Drawing.Size(120, 22);
            this.label_Job0_STATUS.TabIndex = 0;
            this.label_Job0_STATUS.Text = "STATUS";
            this.label_Job0_STATUS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_DECODEDATA
            // 
            this.textBox_Job0_DECODEDATA.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanel6.SetColumnSpan(this.textBox_Job0_DECODEDATA, 4);
            this.textBox_Job0_DECODEDATA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_DECODEDATA.Location = new System.Drawing.Point(3, 274);
            this.textBox_Job0_DECODEDATA.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_Job0_DECODEDATA.Multiline = true;
            this.textBox_Job0_DECODEDATA.Name = "textBox_Job0_DECODEDATA";
            this.textBox_Job0_DECODEDATA.ReadOnly = true;
            this.tableLayoutPanel6.SetRowSpan(this.textBox_Job0_DECODEDATA, 2);
            this.textBox_Job0_DECODEDATA.Size = new System.Drawing.Size(498, 52);
            this.textBox_Job0_DECODEDATA.TabIndex = 0;
            this.textBox_Job0_DECODEDATA.TabStop = false;
            this.textBox_Job0_DECODEDATA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 4;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.Controls.Add(this.buttonOpenLight, 3, 0);
            this.tableLayoutPanel7.Controls.Add(this.button_Configuration, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.checkBox_LiveDisplay, 2, 1);
            this.tableLayoutPanel7.Controls.Add(this.button_SaveSettings, 1, 1);
            this.tableLayoutPanel7.Controls.Add(this.btnRun, 3, 1);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(8, 844);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.Padding = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel7.RowCount = 3;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(520, 168);
            this.tableLayoutPanel7.TabIndex = 0;
            // 
            // buttonOpenLight
            // 
            this.buttonOpenLight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOpenLight.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOpenLight.Location = new System.Drawing.Point(391, 8);
            this.buttonOpenLight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonOpenLight.Name = "buttonOpenLight";
            this.buttonOpenLight.Size = new System.Drawing.Size(122, 42);
            this.buttonOpenLight.TabIndex = 0;
            this.buttonOpenLight.TabStop = false;
            this.buttonOpenLight.Text = "LIGHT";
            this.buttonOpenLight.UseVisualStyleBackColor = true;
            this.buttonOpenLight.Click += new System.EventHandler(this.OnClickButtonLight);
            // 
            // button_Configuration
            // 
            this.button_Configuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Configuration.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Configuration.Location = new System.Drawing.Point(7, 58);
            this.button_Configuration.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_Configuration.Name = "button_Configuration";
            this.button_Configuration.Size = new System.Drawing.Size(122, 42);
            this.button_Configuration.TabIndex = 0;
            this.button_Configuration.TabStop = false;
            this.button_Configuration.Text = "OPEN";
            this.button_Configuration.Click += new System.EventHandler(this.button_Configuration_Click);
            // 
            // checkBox_LiveDisplay
            // 
            this.checkBox_LiveDisplay.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox_LiveDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_LiveDisplay.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_LiveDisplay.Location = new System.Drawing.Point(263, 58);
            this.checkBox_LiveDisplay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBox_LiveDisplay.Name = "checkBox_LiveDisplay";
            this.checkBox_LiveDisplay.Size = new System.Drawing.Size(122, 42);
            this.checkBox_LiveDisplay.TabIndex = 0;
            this.checkBox_LiveDisplay.TabStop = false;
            this.checkBox_LiveDisplay.Text = "LIVE";
            this.checkBox_LiveDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_LiveDisplay.Click += new System.EventHandler(this.checkBox_LiveDisplay_CheckedChanged);
            // 
            // button_SaveSettings
            // 
            this.button_SaveSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_SaveSettings.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_SaveSettings.Location = new System.Drawing.Point(135, 58);
            this.button_SaveSettings.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_SaveSettings.Name = "button_SaveSettings";
            this.button_SaveSettings.Size = new System.Drawing.Size(122, 42);
            this.button_SaveSettings.TabIndex = 0;
            this.button_SaveSettings.TabStop = false;
            this.button_SaveSettings.Text = "SAVE";
            this.button_SaveSettings.Click += new System.EventHandler(this.button_SaveSettings_Click);
            // 
            // btnRun
            // 
            this.btnRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRun.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRun.Location = new System.Drawing.Point(391, 58);
            this.btnRun.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(122, 42);
            this.btnRun.TabIndex = 0;
            this.btnRun.TabStop = false;
            this.btnRun.Text = "RUN";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // groupBox_MODEL
            // 
            this.groupBox_MODEL.Controls.Add(this.tableLayoutPanel5);
            this.groupBox_MODEL.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_MODEL.Location = new System.Drawing.Point(8, 8);
            this.groupBox_MODEL.Name = "groupBox_MODEL";
            this.groupBox_MODEL.Padding = new System.Windows.Forms.Padding(8);
            this.groupBox_MODEL.Size = new System.Drawing.Size(520, 186);
            this.groupBox_MODEL.TabIndex = 0;
            this.groupBox_MODEL.TabStop = false;
            this.groupBox_MODEL.Text = "MODEL";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 4;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.Controls.Add(this.checkBox_Job0_USEFOUNDCIRCLEVERIFY, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.textBox_Job0_REEL13, 3, 3);
            this.tableLayoutPanel5.Controls.Add(this.label_Job0_REEL13, 2, 3);
            this.tableLayoutPanel5.Controls.Add(this.textBox_Job0_REEL7, 3, 2);
            this.tableLayoutPanel5.Controls.Add(this.label_Job0_REEL7, 2, 2);
            this.tableLayoutPanel5.Controls.Add(this.textBox_Job0_TARGETPOINT, 3, 1);
            this.tableLayoutPanel5.Controls.Add(this.label_Job0_TARGETPOINT, 2, 1);
            this.tableLayoutPanel5.Controls.Add(this.textBox_Job0_LIMITY, 3, 4);
            this.tableLayoutPanel5.Controls.Add(this.label_Job0_LIMITY, 2, 4);
            this.tableLayoutPanel5.Controls.Add(this.textBox_Job0_LIMITX, 1, 4);
            this.tableLayoutPanel5.Controls.Add(this.label_Job0_LIMITX, 0, 4);
            this.tableLayoutPanel5.Controls.Add(this.textBox_Job0_SCORELIMIT, 1, 3);
            this.tableLayoutPanel5.Controls.Add(this.label_Job0_SCORELIMIT, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.textBox_Job0_RMSERROR, 1, 2);
            this.tableLayoutPanel5.Controls.Add(this.label_Job0_RMSERROR, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.textBox_Job0_PROCESS, 3, 0);
            this.tableLayoutPanel5.Controls.Add(this.label_Job0_PROCESS, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.checkBox_Job0_USEREELCENTERPATTERN, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(8, 27);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 5;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(504, 151);
            this.tableLayoutPanel5.TabIndex = 18;
            // 
            // checkBox_Job0_USEFOUNDCIRCLEVERIFY
            // 
            this.checkBox_Job0_USEFOUNDCIRCLEVERIFY.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.tableLayoutPanel5.SetColumnSpan(this.checkBox_Job0_USEFOUNDCIRCLEVERIFY, 2);
            this.checkBox_Job0_USEFOUNDCIRCLEVERIFY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_Job0_USEFOUNDCIRCLEVERIFY.ForeColor = System.Drawing.SystemColors.Window;
            this.checkBox_Job0_USEFOUNDCIRCLEVERIFY.Location = new System.Drawing.Point(3, 3);
            this.checkBox_Job0_USEFOUNDCIRCLEVERIFY.Name = "checkBox_Job0_USEFOUNDCIRCLEVERIFY";
            this.checkBox_Job0_USEFOUNDCIRCLEVERIFY.Size = new System.Drawing.Size(246, 24);
            this.checkBox_Job0_USEFOUNDCIRCLEVERIFY.TabIndex = 0;
            this.checkBox_Job0_USEFOUNDCIRCLEVERIFY.TabStop = false;
            this.checkBox_Job0_USEFOUNDCIRCLEVERIFY.Text = "USE FOUND CIRCLE VERIFY";
            this.checkBox_Job0_USEFOUNDCIRCLEVERIFY.UseVisualStyleBackColor = false;
            // 
            // textBox_Job0_REEL13
            // 
            this.textBox_Job0_REEL13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_REEL13.Location = new System.Drawing.Point(381, 93);
            this.textBox_Job0_REEL13.Name = "textBox_Job0_REEL13";
            this.textBox_Job0_REEL13.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_REEL13.TabIndex = 0;
            this.textBox_Job0_REEL13.TabStop = false;
            this.textBox_Job0_REEL13.Text = "-1";
            this.textBox_Job0_REEL13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_REEL13
            // 
            this.label_Job0_REEL13.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_REEL13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_REEL13.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_REEL13.Location = new System.Drawing.Point(255, 93);
            this.label_Job0_REEL13.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_REEL13.Name = "label_Job0_REEL13";
            this.label_Job0_REEL13.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_REEL13.TabIndex = 0;
            this.label_Job0_REEL13.Text = "REEL13";
            this.label_Job0_REEL13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_REEL7
            // 
            this.textBox_Job0_REEL7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_REEL7.Location = new System.Drawing.Point(381, 63);
            this.textBox_Job0_REEL7.Name = "textBox_Job0_REEL7";
            this.textBox_Job0_REEL7.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_REEL7.TabIndex = 0;
            this.textBox_Job0_REEL7.TabStop = false;
            this.textBox_Job0_REEL7.Text = "-1";
            this.textBox_Job0_REEL7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_REEL7
            // 
            this.label_Job0_REEL7.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_REEL7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_REEL7.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_REEL7.Location = new System.Drawing.Point(255, 63);
            this.label_Job0_REEL7.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_REEL7.Name = "label_Job0_REEL7";
            this.label_Job0_REEL7.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_REEL7.TabIndex = 0;
            this.label_Job0_REEL7.Text = "REEL7";
            this.label_Job0_REEL7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_TARGETPOINT
            // 
            this.textBox_Job0_TARGETPOINT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_TARGETPOINT.Location = new System.Drawing.Point(381, 33);
            this.textBox_Job0_TARGETPOINT.Name = "textBox_Job0_TARGETPOINT";
            this.textBox_Job0_TARGETPOINT.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_TARGETPOINT.TabIndex = 0;
            this.textBox_Job0_TARGETPOINT.TabStop = false;
            this.textBox_Job0_TARGETPOINT.Text = "-1";
            this.textBox_Job0_TARGETPOINT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_TARGETPOINT
            // 
            this.label_Job0_TARGETPOINT.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_TARGETPOINT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_TARGETPOINT.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_TARGETPOINT.Location = new System.Drawing.Point(255, 33);
            this.label_Job0_TARGETPOINT.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_TARGETPOINT.Name = "label_Job0_TARGETPOINT";
            this.label_Job0_TARGETPOINT.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_TARGETPOINT.TabIndex = 0;
            this.label_Job0_TARGETPOINT.Text = "TARGET";
            this.label_Job0_TARGETPOINT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_LIMITY
            // 
            this.textBox_Job0_LIMITY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_LIMITY.Location = new System.Drawing.Point(381, 123);
            this.textBox_Job0_LIMITY.Name = "textBox_Job0_LIMITY";
            this.textBox_Job0_LIMITY.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_LIMITY.TabIndex = 0;
            this.textBox_Job0_LIMITY.TabStop = false;
            this.textBox_Job0_LIMITY.Text = "-1";
            this.textBox_Job0_LIMITY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_LIMITY
            // 
            this.label_Job0_LIMITY.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_LIMITY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_LIMITY.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_LIMITY.Location = new System.Drawing.Point(255, 123);
            this.label_Job0_LIMITY.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_LIMITY.Name = "label_Job0_LIMITY";
            this.label_Job0_LIMITY.Size = new System.Drawing.Size(120, 25);
            this.label_Job0_LIMITY.TabIndex = 0;
            this.label_Job0_LIMITY.Text = "LIMIT Y";
            this.label_Job0_LIMITY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_LIMITX
            // 
            this.textBox_Job0_LIMITX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_LIMITX.Location = new System.Drawing.Point(129, 123);
            this.textBox_Job0_LIMITX.Name = "textBox_Job0_LIMITX";
            this.textBox_Job0_LIMITX.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_LIMITX.TabIndex = 0;
            this.textBox_Job0_LIMITX.TabStop = false;
            this.textBox_Job0_LIMITX.Text = "-1";
            this.textBox_Job0_LIMITX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_LIMITX
            // 
            this.label_Job0_LIMITX.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_LIMITX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_LIMITX.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_LIMITX.Location = new System.Drawing.Point(3, 123);
            this.label_Job0_LIMITX.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_LIMITX.Name = "label_Job0_LIMITX";
            this.label_Job0_LIMITX.Size = new System.Drawing.Size(120, 25);
            this.label_Job0_LIMITX.TabIndex = 0;
            this.label_Job0_LIMITX.Text = "LIMIT X";
            this.label_Job0_LIMITX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_SCORELIMIT
            // 
            this.textBox_Job0_SCORELIMIT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_SCORELIMIT.Location = new System.Drawing.Point(129, 93);
            this.textBox_Job0_SCORELIMIT.Name = "textBox_Job0_SCORELIMIT";
            this.textBox_Job0_SCORELIMIT.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_SCORELIMIT.TabIndex = 0;
            this.textBox_Job0_SCORELIMIT.TabStop = false;
            this.textBox_Job0_SCORELIMIT.Text = "-1";
            this.textBox_Job0_SCORELIMIT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_SCORELIMIT
            // 
            this.label_Job0_SCORELIMIT.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_SCORELIMIT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_SCORELIMIT.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_SCORELIMIT.Location = new System.Drawing.Point(3, 93);
            this.label_Job0_SCORELIMIT.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_SCORELIMIT.Name = "label_Job0_SCORELIMIT";
            this.label_Job0_SCORELIMIT.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_SCORELIMIT.TabIndex = 0;
            this.label_Job0_SCORELIMIT.Text = "SCORE LIMIT";
            this.label_Job0_SCORELIMIT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_RMSERROR
            // 
            this.textBox_Job0_RMSERROR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_RMSERROR.Location = new System.Drawing.Point(129, 63);
            this.textBox_Job0_RMSERROR.Name = "textBox_Job0_RMSERROR";
            this.textBox_Job0_RMSERROR.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_RMSERROR.TabIndex = 0;
            this.textBox_Job0_RMSERROR.TabStop = false;
            this.textBox_Job0_RMSERROR.Text = "-1";
            this.textBox_Job0_RMSERROR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_RMSERROR
            // 
            this.label_Job0_RMSERROR.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_RMSERROR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_RMSERROR.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_RMSERROR.Location = new System.Drawing.Point(3, 63);
            this.label_Job0_RMSERROR.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_RMSERROR.Name = "label_Job0_RMSERROR";
            this.label_Job0_RMSERROR.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_RMSERROR.TabIndex = 0;
            this.label_Job0_RMSERROR.Text = "RMS ERROR";
            this.label_Job0_RMSERROR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_PROCESS
            // 
            this.textBox_Job0_PROCESS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_PROCESS.Location = new System.Drawing.Point(381, 3);
            this.textBox_Job0_PROCESS.Name = "textBox_Job0_PROCESS";
            this.textBox_Job0_PROCESS.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_PROCESS.TabIndex = 0;
            this.textBox_Job0_PROCESS.TabStop = false;
            this.textBox_Job0_PROCESS.Text = "-1";
            this.textBox_Job0_PROCESS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_PROCESS
            // 
            this.label_Job0_PROCESS.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_PROCESS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_PROCESS.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_PROCESS.Location = new System.Drawing.Point(255, 3);
            this.label_Job0_PROCESS.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_PROCESS.Name = "label_Job0_PROCESS";
            this.label_Job0_PROCESS.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_PROCESS.TabIndex = 0;
            this.label_Job0_PROCESS.Text = "PROCESS";
            this.label_Job0_PROCESS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBox_Job0_USEREELCENTERPATTERN
            // 
            this.checkBox_Job0_USEREELCENTERPATTERN.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.tableLayoutPanel5.SetColumnSpan(this.checkBox_Job0_USEREELCENTERPATTERN, 2);
            this.checkBox_Job0_USEREELCENTERPATTERN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_Job0_USEREELCENTERPATTERN.ForeColor = System.Drawing.SystemColors.Window;
            this.checkBox_Job0_USEREELCENTERPATTERN.Location = new System.Drawing.Point(3, 33);
            this.checkBox_Job0_USEREELCENTERPATTERN.Name = "checkBox_Job0_USEREELCENTERPATTERN";
            this.checkBox_Job0_USEREELCENTERPATTERN.Size = new System.Drawing.Size(246, 24);
            this.checkBox_Job0_USEREELCENTERPATTERN.TabIndex = 0;
            this.checkBox_Job0_USEREELCENTERPATTERN.TabStop = false;
            this.checkBox_Job0_USEREELCENTERPATTERN.Text = "USE REEL CENTER PATTERN";
            this.checkBox_Job0_USEREELCENTERPATTERN.UseVisualStyleBackColor = false;
            // 
            // tabPage_Job0_BASEPOINT
            // 
            this.tabPage_Job0_BASEPOINT.AutoScroll = true;
            this.tabPage_Job0_BASEPOINT.Controls.Add(this.tableLayoutPanel12);
            this.tabPage_Job0_BASEPOINT.Controls.Add(this.groupBox_RESULT_1);
            this.tabPage_Job0_BASEPOINT.Controls.Add(this.groupBox_PARAMETERS);
            this.tabPage_Job0_BASEPOINT.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage_Job0_BASEPOINT.Location = new System.Drawing.Point(4, 24);
            this.tabPage_Job0_BASEPOINT.Name = "tabPage_Job0_BASEPOINT";
            this.tabPage_Job0_BASEPOINT.Padding = new System.Windows.Forms.Padding(8);
            this.tabPage_Job0_BASEPOINT.Size = new System.Drawing.Size(536, 1020);
            this.tabPage_Job0_BASEPOINT.TabIndex = 0;
            this.tabPage_Job0_BASEPOINT.Text = "BASE POINT";
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.ColumnCount = 4;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel12.Controls.Add(this.buttonBasePointRun, 3, 1);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(8, 844);
            this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.Padding = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel12.RowCount = 3;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(520, 168);
            this.tableLayoutPanel12.TabIndex = 0;
            // 
            // buttonBasePointRun
            // 
            this.buttonBasePointRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonBasePointRun.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBasePointRun.Location = new System.Drawing.Point(391, 58);
            this.buttonBasePointRun.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonBasePointRun.Name = "buttonBasePointRun";
            this.buttonBasePointRun.Size = new System.Drawing.Size(122, 42);
            this.buttonBasePointRun.TabIndex = 0;
            this.buttonBasePointRun.TabStop = false;
            this.buttonBasePointRun.Text = "RUN";
            this.buttonBasePointRun.Click += new System.EventHandler(this.OnClickButtonBasePointRun);
            // 
            // groupBox_RESULT_1
            // 
            this.groupBox_RESULT_1.Controls.Add(this.tableLayoutPanel9);
            this.groupBox_RESULT_1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_RESULT_1.Location = new System.Drawing.Point(8, 106);
            this.groupBox_RESULT_1.Name = "groupBox_RESULT_1";
            this.groupBox_RESULT_1.Padding = new System.Windows.Forms.Padding(8);
            this.groupBox_RESULT_1.Size = new System.Drawing.Size(520, 218);
            this.groupBox_RESULT_1.TabIndex = 0;
            this.groupBox_RESULT_1.TabStop = false;
            this.groupBox_RESULT_1.Text = "RESULT";
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 4;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel9.Controls.Add(this.textBox_Job0_DISAngle, 3, 2);
            this.tableLayoutPanel9.Controls.Add(this.textBox_Job0_DISY, 3, 1);
            this.tableLayoutPanel9.Controls.Add(this.textBox_Job0_DISX, 3, 0);
            this.tableLayoutPanel9.Controls.Add(this.label_Job0_DISX, 2, 0);
            this.tableLayoutPanel9.Controls.Add(this.label_Job0_DISY, 2, 1);
            this.tableLayoutPanel9.Controls.Add(this.label_Job0_DISAngle, 2, 2);
            this.tableLayoutPanel9.Controls.Add(this.textBox_Job0_2ndAngle, 3, 5);
            this.tableLayoutPanel9.Controls.Add(this.textBox_Job0_2ndY, 3, 4);
            this.tableLayoutPanel9.Controls.Add(this.textBox_Job0_2ndX, 3, 3);
            this.tableLayoutPanel9.Controls.Add(this.label_Job0_2ndX, 2, 3);
            this.tableLayoutPanel9.Controls.Add(this.label_Job0_2ndY, 2, 4);
            this.tableLayoutPanel9.Controls.Add(this.label_Job0_2ndAngle, 2, 5);
            this.tableLayoutPanel9.Controls.Add(this.textBox_Job0_1stAngle, 1, 5);
            this.tableLayoutPanel9.Controls.Add(this.label_Job0_1stAngle, 0, 5);
            this.tableLayoutPanel9.Controls.Add(this.textBox_Job0_1stY, 1, 4);
            this.tableLayoutPanel9.Controls.Add(this.textBox_Job0_1stX, 1, 3);
            this.tableLayoutPanel9.Controls.Add(this.textBox_Job0_FOUND2nd, 1, 2);
            this.tableLayoutPanel9.Controls.Add(this.textBox_Job0_FOUND1st, 1, 1);
            this.tableLayoutPanel9.Controls.Add(this.textBox_Job0_VERIFIED, 1, 0);
            this.tableLayoutPanel9.Controls.Add(this.label_Job0_VERIFIED, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.label_Job0_FOUND1st, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.label_Job0_FOUND2nd, 0, 2);
            this.tableLayoutPanel9.Controls.Add(this.label_Job0_1stX, 0, 3);
            this.tableLayoutPanel9.Controls.Add(this.label_Job0_1stY, 0, 4);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(8, 27);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 6;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(504, 183);
            this.tableLayoutPanel9.TabIndex = 0;
            // 
            // textBox_Job0_DISAngle
            // 
            this.textBox_Job0_DISAngle.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_DISAngle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_DISAngle.Location = new System.Drawing.Point(381, 63);
            this.textBox_Job0_DISAngle.Name = "textBox_Job0_DISAngle";
            this.textBox_Job0_DISAngle.ReadOnly = true;
            this.textBox_Job0_DISAngle.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_DISAngle.TabIndex = 0;
            this.textBox_Job0_DISAngle.TabStop = false;
            this.textBox_Job0_DISAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Job0_DISY
            // 
            this.textBox_Job0_DISY.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_DISY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_DISY.Location = new System.Drawing.Point(381, 33);
            this.textBox_Job0_DISY.Name = "textBox_Job0_DISY";
            this.textBox_Job0_DISY.ReadOnly = true;
            this.textBox_Job0_DISY.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_DISY.TabIndex = 0;
            this.textBox_Job0_DISY.TabStop = false;
            this.textBox_Job0_DISY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Job0_DISX
            // 
            this.textBox_Job0_DISX.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_DISX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_DISX.Location = new System.Drawing.Point(381, 3);
            this.textBox_Job0_DISX.Name = "textBox_Job0_DISX";
            this.textBox_Job0_DISX.ReadOnly = true;
            this.textBox_Job0_DISX.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_DISX.TabIndex = 0;
            this.textBox_Job0_DISX.TabStop = false;
            this.textBox_Job0_DISX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_DISX
            // 
            this.label_Job0_DISX.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_DISX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_DISX.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_DISX.Location = new System.Drawing.Point(255, 3);
            this.label_Job0_DISX.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_DISX.Name = "label_Job0_DISX";
            this.label_Job0_DISX.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_DISX.TabIndex = 0;
            this.label_Job0_DISX.Text = "DIS. X";
            this.label_Job0_DISX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Job0_DISY
            // 
            this.label_Job0_DISY.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_DISY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_DISY.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_DISY.Location = new System.Drawing.Point(255, 33);
            this.label_Job0_DISY.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_DISY.Name = "label_Job0_DISY";
            this.label_Job0_DISY.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_DISY.TabIndex = 0;
            this.label_Job0_DISY.Text = "DIS. Y";
            this.label_Job0_DISY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Job0_DISAngle
            // 
            this.label_Job0_DISAngle.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_DISAngle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_DISAngle.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_DISAngle.Location = new System.Drawing.Point(255, 63);
            this.label_Job0_DISAngle.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_DISAngle.Name = "label_Job0_DISAngle";
            this.label_Job0_DISAngle.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_DISAngle.TabIndex = 0;
            this.label_Job0_DISAngle.Text = "DIS. Angle";
            this.label_Job0_DISAngle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_2ndAngle
            // 
            this.textBox_Job0_2ndAngle.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_2ndAngle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_2ndAngle.Location = new System.Drawing.Point(381, 153);
            this.textBox_Job0_2ndAngle.Name = "textBox_Job0_2ndAngle";
            this.textBox_Job0_2ndAngle.ReadOnly = true;
            this.textBox_Job0_2ndAngle.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_2ndAngle.TabIndex = 0;
            this.textBox_Job0_2ndAngle.TabStop = false;
            this.textBox_Job0_2ndAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Job0_2ndY
            // 
            this.textBox_Job0_2ndY.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_2ndY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_2ndY.Location = new System.Drawing.Point(381, 123);
            this.textBox_Job0_2ndY.Name = "textBox_Job0_2ndY";
            this.textBox_Job0_2ndY.ReadOnly = true;
            this.textBox_Job0_2ndY.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_2ndY.TabIndex = 0;
            this.textBox_Job0_2ndY.TabStop = false;
            this.textBox_Job0_2ndY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Job0_2ndX
            // 
            this.textBox_Job0_2ndX.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_2ndX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_2ndX.Location = new System.Drawing.Point(381, 93);
            this.textBox_Job0_2ndX.Name = "textBox_Job0_2ndX";
            this.textBox_Job0_2ndX.ReadOnly = true;
            this.textBox_Job0_2ndX.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_2ndX.TabIndex = 0;
            this.textBox_Job0_2ndX.TabStop = false;
            this.textBox_Job0_2ndX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_2ndX
            // 
            this.label_Job0_2ndX.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_2ndX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_2ndX.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_2ndX.Location = new System.Drawing.Point(255, 93);
            this.label_Job0_2ndX.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_2ndX.Name = "label_Job0_2ndX";
            this.label_Job0_2ndX.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_2ndX.TabIndex = 0;
            this.label_Job0_2ndX.Text = "2nd X";
            this.label_Job0_2ndX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Job0_2ndY
            // 
            this.label_Job0_2ndY.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_2ndY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_2ndY.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_2ndY.Location = new System.Drawing.Point(255, 123);
            this.label_Job0_2ndY.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_2ndY.Name = "label_Job0_2ndY";
            this.label_Job0_2ndY.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_2ndY.TabIndex = 0;
            this.label_Job0_2ndY.Text = "2nd Y";
            this.label_Job0_2ndY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Job0_2ndAngle
            // 
            this.label_Job0_2ndAngle.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_2ndAngle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_2ndAngle.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_2ndAngle.Location = new System.Drawing.Point(255, 153);
            this.label_Job0_2ndAngle.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_2ndAngle.Name = "label_Job0_2ndAngle";
            this.label_Job0_2ndAngle.Size = new System.Drawing.Size(120, 27);
            this.label_Job0_2ndAngle.TabIndex = 0;
            this.label_Job0_2ndAngle.Text = "2nd Angle";
            this.label_Job0_2ndAngle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_1stAngle
            // 
            this.textBox_Job0_1stAngle.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_1stAngle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_1stAngle.Location = new System.Drawing.Point(129, 153);
            this.textBox_Job0_1stAngle.Name = "textBox_Job0_1stAngle";
            this.textBox_Job0_1stAngle.ReadOnly = true;
            this.textBox_Job0_1stAngle.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_1stAngle.TabIndex = 0;
            this.textBox_Job0_1stAngle.TabStop = false;
            this.textBox_Job0_1stAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_1stAngle
            // 
            this.label_Job0_1stAngle.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_1stAngle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_1stAngle.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_1stAngle.Location = new System.Drawing.Point(3, 153);
            this.label_Job0_1stAngle.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_1stAngle.Name = "label_Job0_1stAngle";
            this.label_Job0_1stAngle.Size = new System.Drawing.Size(120, 27);
            this.label_Job0_1stAngle.TabIndex = 0;
            this.label_Job0_1stAngle.Text = "1st Angle";
            this.label_Job0_1stAngle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_1stY
            // 
            this.textBox_Job0_1stY.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_1stY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_1stY.Location = new System.Drawing.Point(129, 123);
            this.textBox_Job0_1stY.Name = "textBox_Job0_1stY";
            this.textBox_Job0_1stY.ReadOnly = true;
            this.textBox_Job0_1stY.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_1stY.TabIndex = 0;
            this.textBox_Job0_1stY.TabStop = false;
            this.textBox_Job0_1stY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Job0_1stX
            // 
            this.textBox_Job0_1stX.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_1stX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_1stX.Location = new System.Drawing.Point(129, 93);
            this.textBox_Job0_1stX.Name = "textBox_Job0_1stX";
            this.textBox_Job0_1stX.ReadOnly = true;
            this.textBox_Job0_1stX.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_1stX.TabIndex = 0;
            this.textBox_Job0_1stX.TabStop = false;
            this.textBox_Job0_1stX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Job0_FOUND2nd
            // 
            this.textBox_Job0_FOUND2nd.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_FOUND2nd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_FOUND2nd.Location = new System.Drawing.Point(129, 63);
            this.textBox_Job0_FOUND2nd.Name = "textBox_Job0_FOUND2nd";
            this.textBox_Job0_FOUND2nd.ReadOnly = true;
            this.textBox_Job0_FOUND2nd.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_FOUND2nd.TabIndex = 0;
            this.textBox_Job0_FOUND2nd.TabStop = false;
            this.textBox_Job0_FOUND2nd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Job0_FOUND1st
            // 
            this.textBox_Job0_FOUND1st.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_FOUND1st.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_FOUND1st.Location = new System.Drawing.Point(129, 33);
            this.textBox_Job0_FOUND1st.Name = "textBox_Job0_FOUND1st";
            this.textBox_Job0_FOUND1st.ReadOnly = true;
            this.textBox_Job0_FOUND1st.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_FOUND1st.TabIndex = 0;
            this.textBox_Job0_FOUND1st.TabStop = false;
            this.textBox_Job0_FOUND1st.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Job0_VERIFIED
            // 
            this.textBox_Job0_VERIFIED.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_VERIFIED.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_VERIFIED.Location = new System.Drawing.Point(129, 3);
            this.textBox_Job0_VERIFIED.Name = "textBox_Job0_VERIFIED";
            this.textBox_Job0_VERIFIED.ReadOnly = true;
            this.textBox_Job0_VERIFIED.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_VERIFIED.TabIndex = 0;
            this.textBox_Job0_VERIFIED.TabStop = false;
            this.textBox_Job0_VERIFIED.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_VERIFIED
            // 
            this.label_Job0_VERIFIED.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_VERIFIED.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_VERIFIED.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_VERIFIED.Location = new System.Drawing.Point(3, 3);
            this.label_Job0_VERIFIED.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_VERIFIED.Name = "label_Job0_VERIFIED";
            this.label_Job0_VERIFIED.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_VERIFIED.TabIndex = 0;
            this.label_Job0_VERIFIED.Text = "VERIFIED";
            this.label_Job0_VERIFIED.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Job0_FOUND1st
            // 
            this.label_Job0_FOUND1st.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_FOUND1st.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_FOUND1st.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_FOUND1st.Location = new System.Drawing.Point(3, 33);
            this.label_Job0_FOUND1st.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_FOUND1st.Name = "label_Job0_FOUND1st";
            this.label_Job0_FOUND1st.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_FOUND1st.TabIndex = 0;
            this.label_Job0_FOUND1st.Text = "FOUND 1st";
            this.label_Job0_FOUND1st.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Job0_FOUND2nd
            // 
            this.label_Job0_FOUND2nd.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_FOUND2nd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_FOUND2nd.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_FOUND2nd.Location = new System.Drawing.Point(3, 63);
            this.label_Job0_FOUND2nd.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_FOUND2nd.Name = "label_Job0_FOUND2nd";
            this.label_Job0_FOUND2nd.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_FOUND2nd.TabIndex = 0;
            this.label_Job0_FOUND2nd.Text = "FOUND 2nd";
            this.label_Job0_FOUND2nd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Job0_1stX
            // 
            this.label_Job0_1stX.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_1stX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_1stX.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_1stX.Location = new System.Drawing.Point(3, 93);
            this.label_Job0_1stX.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_1stX.Name = "label_Job0_1stX";
            this.label_Job0_1stX.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_1stX.TabIndex = 0;
            this.label_Job0_1stX.Text = "1st X";
            this.label_Job0_1stX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Job0_1stY
            // 
            this.label_Job0_1stY.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_1stY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_1stY.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_1stY.Location = new System.Drawing.Point(3, 123);
            this.label_Job0_1stY.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_1stY.Name = "label_Job0_1stY";
            this.label_Job0_1stY.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_1stY.TabIndex = 0;
            this.label_Job0_1stY.Text = "1st Y";
            this.label_Job0_1stY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox_PARAMETERS
            // 
            this.groupBox_PARAMETERS.Controls.Add(this.tableLayoutPanel8);
            this.groupBox_PARAMETERS.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_PARAMETERS.Location = new System.Drawing.Point(8, 8);
            this.groupBox_PARAMETERS.Name = "groupBox_PARAMETERS";
            this.groupBox_PARAMETERS.Padding = new System.Windows.Forms.Padding(8);
            this.groupBox_PARAMETERS.Size = new System.Drawing.Size(520, 98);
            this.groupBox_PARAMETERS.TabIndex = 0;
            this.groupBox_PARAMETERS.TabStop = false;
            this.groupBox_PARAMETERS.Text = "PARAMETERS";
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 4;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel8.Controls.Add(this.label_Job0_REFERENCEX, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.textBox_Job0_BASEPOINTMODE, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.label_Job0_BASEPOINTMODE, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.textBox_Job0_REFERENCEX, 1, 1);
            this.tableLayoutPanel8.Controls.Add(this.label_Job0_REFERENCEY, 2, 1);
            this.tableLayoutPanel8.Controls.Add(this.textBox_Job0_REFERENCEY, 3, 1);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(8, 27);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 2;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(504, 63);
            this.tableLayoutPanel8.TabIndex = 0;
            // 
            // label_Job0_REFERENCEX
            // 
            this.label_Job0_REFERENCEX.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_REFERENCEX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_REFERENCEX.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_REFERENCEX.Location = new System.Drawing.Point(3, 33);
            this.label_Job0_REFERENCEX.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_REFERENCEX.Name = "label_Job0_REFERENCEX";
            this.label_Job0_REFERENCEX.Size = new System.Drawing.Size(120, 27);
            this.label_Job0_REFERENCEX.TabIndex = 0;
            this.label_Job0_REFERENCEX.Text = "REF X";
            this.label_Job0_REFERENCEX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_BASEPOINTMODE
            // 
            this.textBox_Job0_BASEPOINTMODE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_BASEPOINTMODE.Location = new System.Drawing.Point(129, 3);
            this.textBox_Job0_BASEPOINTMODE.Name = "textBox_Job0_BASEPOINTMODE";
            this.textBox_Job0_BASEPOINTMODE.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_BASEPOINTMODE.TabIndex = 0;
            this.textBox_Job0_BASEPOINTMODE.TabStop = false;
            this.textBox_Job0_BASEPOINTMODE.Text = "-1";
            this.textBox_Job0_BASEPOINTMODE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_BASEPOINTMODE
            // 
            this.label_Job0_BASEPOINTMODE.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_BASEPOINTMODE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_BASEPOINTMODE.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_BASEPOINTMODE.Location = new System.Drawing.Point(3, 3);
            this.label_Job0_BASEPOINTMODE.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_BASEPOINTMODE.Name = "label_Job0_BASEPOINTMODE";
            this.label_Job0_BASEPOINTMODE.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_BASEPOINTMODE.TabIndex = 0;
            this.label_Job0_BASEPOINTMODE.Text = "MODE";
            this.label_Job0_BASEPOINTMODE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_REFERENCEX
            // 
            this.textBox_Job0_REFERENCEX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_REFERENCEX.Location = new System.Drawing.Point(129, 33);
            this.textBox_Job0_REFERENCEX.Name = "textBox_Job0_REFERENCEX";
            this.textBox_Job0_REFERENCEX.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_REFERENCEX.TabIndex = 0;
            this.textBox_Job0_REFERENCEX.TabStop = false;
            this.textBox_Job0_REFERENCEX.Text = "-1";
            this.textBox_Job0_REFERENCEX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_REFERENCEY
            // 
            this.label_Job0_REFERENCEY.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_REFERENCEY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_REFERENCEY.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_REFERENCEY.Location = new System.Drawing.Point(255, 33);
            this.label_Job0_REFERENCEY.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_REFERENCEY.Name = "label_Job0_REFERENCEY";
            this.label_Job0_REFERENCEY.Size = new System.Drawing.Size(120, 27);
            this.label_Job0_REFERENCEY.TabIndex = 0;
            this.label_Job0_REFERENCEY.Text = "REF Y";
            this.label_Job0_REFERENCEY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_REFERENCEY
            // 
            this.textBox_Job0_REFERENCEY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_REFERENCEY.Location = new System.Drawing.Point(381, 33);
            this.textBox_Job0_REFERENCEY.Name = "textBox_Job0_REFERENCEY";
            this.textBox_Job0_REFERENCEY.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_REFERENCEY.TabIndex = 0;
            this.textBox_Job0_REFERENCEY.TabStop = false;
            this.textBox_Job0_REFERENCEY.Text = "-1";
            this.textBox_Job0_REFERENCEY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_AcquisitionResults_Overruns
            // 
            this.label_AcquisitionResults_Overruns.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_AcquisitionResults_Overruns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_AcquisitionResults_Overruns.ForeColor = System.Drawing.SystemColors.Window;
            this.label_AcquisitionResults_Overruns.Location = new System.Drawing.Point(3, 64);
            this.label_AcquisitionResults_Overruns.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_AcquisitionResults_Overruns.Name = "label_AcquisitionResults_Overruns";
            this.label_AcquisitionResults_Overruns.Size = new System.Drawing.Size(182, 27);
            this.label_AcquisitionResults_Overruns.TabIndex = 0;
            this.label_AcquisitionResults_Overruns.Text = "OVERRUNS";
            this.label_AcquisitionResults_Overruns.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_JobN_TotalAcquisitionErrors
            // 
            this.textBox_JobN_TotalAcquisitionErrors.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_JobN_TotalAcquisitionErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_JobN_TotalAcquisitionErrors.Location = new System.Drawing.Point(191, 34);
            this.textBox_JobN_TotalAcquisitionErrors.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_JobN_TotalAcquisitionErrors.Name = "textBox_JobN_TotalAcquisitionErrors";
            this.textBox_JobN_TotalAcquisitionErrors.ReadOnly = true;
            this.textBox_JobN_TotalAcquisitionErrors.Size = new System.Drawing.Size(135, 26);
            this.textBox_JobN_TotalAcquisitionErrors.TabIndex = 0;
            this.textBox_JobN_TotalAcquisitionErrors.TabStop = false;
            this.textBox_JobN_TotalAcquisitionErrors.Text = "textBox1";
            this.textBox_JobN_TotalAcquisitionErrors.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox_JobN_TotalAcquisitionOverruns
            // 
            this.textBox_JobN_TotalAcquisitionOverruns.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_JobN_TotalAcquisitionOverruns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_JobN_TotalAcquisitionOverruns.Location = new System.Drawing.Point(191, 64);
            this.textBox_JobN_TotalAcquisitionOverruns.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_JobN_TotalAcquisitionOverruns.Name = "textBox_JobN_TotalAcquisitionOverruns";
            this.textBox_JobN_TotalAcquisitionOverruns.ReadOnly = true;
            this.textBox_JobN_TotalAcquisitionOverruns.Size = new System.Drawing.Size(135, 26);
            this.textBox_JobN_TotalAcquisitionOverruns.TabIndex = 0;
            this.textBox_JobN_TotalAcquisitionOverruns.TabStop = false;
            this.textBox_JobN_TotalAcquisitionOverruns.Text = "textBox2";
            this.textBox_JobN_TotalAcquisitionOverruns.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupBox_AcquisitionResults
            // 
            this.groupBox_AcquisitionResults.Controls.Add(this.tableLayoutPanel4);
            this.groupBox_AcquisitionResults.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_AcquisitionResults.Location = new System.Drawing.Point(8, 325);
            this.groupBox_AcquisitionResults.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox_AcquisitionResults.Name = "groupBox_AcquisitionResults";
            this.groupBox_AcquisitionResults.Padding = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.groupBox_AcquisitionResults.Size = new System.Drawing.Size(520, 134);
            this.groupBox_AcquisitionResults.TabIndex = 0;
            this.groupBox_AcquisitionResults.TabStop = false;
            this.groupBox_AcquisitionResults.Text = " ACQUISITION ";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel4.Controls.Add(this.label_AcquisitionResults_TotalAcquisitions, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.textBox_JobN_TotalAcquisitionOverruns, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.label_AcquisitionResults_Errors, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label_AcquisitionResults_Overruns, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.textBox_JobN_TotalAcquisitions, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.textBox_JobN_TotalAcquisitionErrors, 1, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(8, 29);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(504, 95);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // label_AcquisitionResults_TotalAcquisitions
            // 
            this.label_AcquisitionResults_TotalAcquisitions.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_AcquisitionResults_TotalAcquisitions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_AcquisitionResults_TotalAcquisitions.ForeColor = System.Drawing.SystemColors.Window;
            this.label_AcquisitionResults_TotalAcquisitions.Location = new System.Drawing.Point(3, 4);
            this.label_AcquisitionResults_TotalAcquisitions.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_AcquisitionResults_TotalAcquisitions.Name = "label_AcquisitionResults_TotalAcquisitions";
            this.label_AcquisitionResults_TotalAcquisitions.Size = new System.Drawing.Size(182, 22);
            this.label_AcquisitionResults_TotalAcquisitions.TabIndex = 0;
            this.label_AcquisitionResults_TotalAcquisitions.Text = "TOTAL ACQUISITIONS";
            this.label_AcquisitionResults_TotalAcquisitions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_AcquisitionResults_Errors
            // 
            this.label_AcquisitionResults_Errors.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_AcquisitionResults_Errors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_AcquisitionResults_Errors.ForeColor = System.Drawing.SystemColors.Window;
            this.label_AcquisitionResults_Errors.Location = new System.Drawing.Point(3, 34);
            this.label_AcquisitionResults_Errors.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_AcquisitionResults_Errors.Name = "label_AcquisitionResults_Errors";
            this.label_AcquisitionResults_Errors.Size = new System.Drawing.Size(182, 22);
            this.label_AcquisitionResults_Errors.TabIndex = 0;
            this.label_AcquisitionResults_Errors.Text = "ERRORS";
            this.label_AcquisitionResults_Errors.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_JobN_TotalAcquisitions
            // 
            this.textBox_JobN_TotalAcquisitions.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_JobN_TotalAcquisitions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_JobN_TotalAcquisitions.Location = new System.Drawing.Point(191, 4);
            this.textBox_JobN_TotalAcquisitions.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_JobN_TotalAcquisitions.Name = "textBox_JobN_TotalAcquisitions";
            this.textBox_JobN_TotalAcquisitions.ReadOnly = true;
            this.textBox_JobN_TotalAcquisitions.Size = new System.Drawing.Size(135, 26);
            this.textBox_JobN_TotalAcquisitions.TabIndex = 0;
            this.textBox_JobN_TotalAcquisitions.TabStop = false;
            this.textBox_JobN_TotalAcquisitions.Text = "textBox1";
            this.textBox_JobN_TotalAcquisitions.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label_JobThroughput_persec
            // 
            this.label_JobThroughput_persec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_JobThroughput_persec.Location = new System.Drawing.Point(332, 0);
            this.label_JobThroughput_persec.Name = "label_JobThroughput_persec";
            this.label_JobThroughput_persec.Size = new System.Drawing.Size(135, 30);
            this.label_JobThroughput_persec.TabIndex = 0;
            this.label_JobThroughput_persec.Text = "per sec";
            this.label_JobThroughput_persec.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox_JobThroughput
            // 
            this.groupBox_JobThroughput.Controls.Add(this.tableLayoutPanel3);
            this.groupBox_JobThroughput.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_JobThroughput.Location = new System.Drawing.Point(8, 197);
            this.groupBox_JobThroughput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox_JobThroughput.Name = "groupBox_JobThroughput";
            this.groupBox_JobThroughput.Padding = new System.Windows.Forms.Padding(8);
            this.groupBox_JobThroughput.Size = new System.Drawing.Size(520, 128);
            this.groupBox_JobThroughput.TabIndex = 0;
            this.groupBox_JobThroughput.TabStop = false;
            this.groupBox_JobThroughput.Text = " THROUGHPUT ";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel3.Controls.Add(this.label_JobThroughput_TotalThroughput, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label_JobThroughput_Max, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.textBox_JobN_MaxThroughput, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.label_JobThroughput_Min, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label_JobThroughput_persec, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.textBox_JobN_MinThroughput, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.textBox_JobN_Throughput, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(8, 27);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(504, 93);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label_JobThroughput_TotalThroughput
            // 
            this.label_JobThroughput_TotalThroughput.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_JobThroughput_TotalThroughput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_JobThroughput_TotalThroughput.ForeColor = System.Drawing.SystemColors.Window;
            this.label_JobThroughput_TotalThroughput.Location = new System.Drawing.Point(3, 4);
            this.label_JobThroughput_TotalThroughput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_JobThroughput_TotalThroughput.Name = "label_JobThroughput_TotalThroughput";
            this.label_JobThroughput_TotalThroughput.Size = new System.Drawing.Size(182, 22);
            this.label_JobThroughput_TotalThroughput.TabIndex = 0;
            this.label_JobThroughput_TotalThroughput.Text = "TOTAL THROUGHPUT";
            this.label_JobThroughput_TotalThroughput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_JobThroughput_Max
            // 
            this.label_JobThroughput_Max.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_JobThroughput_Max.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_JobThroughput_Max.ForeColor = System.Drawing.SystemColors.Window;
            this.label_JobThroughput_Max.Location = new System.Drawing.Point(3, 64);
            this.label_JobThroughput_Max.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_JobThroughput_Max.Name = "label_JobThroughput_Max";
            this.label_JobThroughput_Max.Size = new System.Drawing.Size(182, 25);
            this.label_JobThroughput_Max.TabIndex = 0;
            this.label_JobThroughput_Max.Text = "MAX";
            this.label_JobThroughput_Max.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_JobN_MaxThroughput
            // 
            this.textBox_JobN_MaxThroughput.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_JobN_MaxThroughput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_JobN_MaxThroughput.Location = new System.Drawing.Point(191, 63);
            this.textBox_JobN_MaxThroughput.Name = "textBox_JobN_MaxThroughput";
            this.textBox_JobN_MaxThroughput.ReadOnly = true;
            this.textBox_JobN_MaxThroughput.Size = new System.Drawing.Size(135, 26);
            this.textBox_JobN_MaxThroughput.TabIndex = 0;
            this.textBox_JobN_MaxThroughput.TabStop = false;
            this.textBox_JobN_MaxThroughput.Text = "textBox1";
            this.textBox_JobN_MaxThroughput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label_JobThroughput_Min
            // 
            this.label_JobThroughput_Min.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_JobThroughput_Min.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_JobThroughput_Min.ForeColor = System.Drawing.SystemColors.Window;
            this.label_JobThroughput_Min.Location = new System.Drawing.Point(3, 34);
            this.label_JobThroughput_Min.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_JobThroughput_Min.Name = "label_JobThroughput_Min";
            this.label_JobThroughput_Min.Size = new System.Drawing.Size(182, 22);
            this.label_JobThroughput_Min.TabIndex = 0;
            this.label_JobThroughput_Min.Text = "MIN";
            this.label_JobThroughput_Min.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_JobN_MinThroughput
            // 
            this.textBox_JobN_MinThroughput.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_JobN_MinThroughput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_JobN_MinThroughput.Location = new System.Drawing.Point(191, 34);
            this.textBox_JobN_MinThroughput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_JobN_MinThroughput.Name = "textBox_JobN_MinThroughput";
            this.textBox_JobN_MinThroughput.ReadOnly = true;
            this.textBox_JobN_MinThroughput.Size = new System.Drawing.Size(135, 26);
            this.textBox_JobN_MinThroughput.TabIndex = 0;
            this.textBox_JobN_MinThroughput.TabStop = false;
            this.textBox_JobN_MinThroughput.Text = "textBox1";
            this.textBox_JobN_MinThroughput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox_JobN_Throughput
            // 
            this.textBox_JobN_Throughput.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_JobN_Throughput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_JobN_Throughput.Location = new System.Drawing.Point(191, 4);
            this.textBox_JobN_Throughput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_JobN_Throughput.Name = "textBox_JobN_Throughput";
            this.textBox_JobN_Throughput.ReadOnly = true;
            this.textBox_JobN_Throughput.Size = new System.Drawing.Size(135, 26);
            this.textBox_JobN_Throughput.TabIndex = 0;
            this.textBox_JobN_Throughput.TabStop = false;
            this.textBox_JobN_Throughput.Text = "textBox1";
            this.textBox_JobN_Throughput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tabControlJobTabs
            // 
            this.tabControlJobTabs.Controls.Add(this.tabPage_JobN_JobStatistics);
            this.tabControlJobTabs.Controls.Add(this.tabPage_Job0_URL);
            this.tabControlJobTabs.Controls.Add(this.tabPage_Job0_BASEPOINT);
            this.tabControlJobTabs.Controls.Add(this.tabPage_Job0_GUIDEPOINT);
            this.tabControlJobTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlJobTabs.Location = new System.Drawing.Point(0, 0);
            this.tabControlJobTabs.Name = "tabControlJobTabs";
            this.tabControlJobTabs.SelectedIndex = 0;
            this.tabControlJobTabs.Size = new System.Drawing.Size(544, 1048);
            this.tabControlJobTabs.TabIndex = 0;
            this.tabControlJobTabs.TabStop = false;
            this.tabControlJobTabs.Tag = "";
            // 
            // tabPage_JobN_JobStatistics
            // 
            this.tabPage_JobN_JobStatistics.AutoScroll = true;
            this.tabPage_JobN_JobStatistics.Controls.Add(this.tableLayoutPanel1);
            this.tabPage_JobN_JobStatistics.Controls.Add(this.groupBox_AcquisitionResults);
            this.tabPage_JobN_JobStatistics.Controls.Add(this.groupBox_JobThroughput);
            this.tabPage_JobN_JobStatistics.Controls.Add(this.groupBox_JobResults);
            this.tabPage_JobN_JobStatistics.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage_JobN_JobStatistics.Location = new System.Drawing.Point(4, 24);
            this.tabPage_JobN_JobStatistics.Name = "tabPage_JobN_JobStatistics";
            this.tabPage_JobN_JobStatistics.Padding = new System.Windows.Forms.Padding(8);
            this.tabPage_JobN_JobStatistics.Size = new System.Drawing.Size(536, 1020);
            this.tabPage_JobN_JobStatistics.TabIndex = 0;
            this.tabPage_JobN_JobStatistics.Text = "STATISTICS";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62F));
            this.tableLayoutPanel1.Controls.Add(this.label_ResultBar, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.button_ResetStatistics, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button_ResetStatisticsForAllJobs, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label_controlErrorMessage, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(8, 459);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(520, 180);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label_ResultBar
            // 
            this.label_ResultBar.BackColor = System.Drawing.SystemColors.Control;
            this.label_ResultBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.label_ResultBar, 2);
            this.label_ResultBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_ResultBar.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_ResultBar.Location = new System.Drawing.Point(7, 48);
            this.label_ResultBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_ResultBar.Name = "label_ResultBar";
            this.label_ResultBar.Size = new System.Drawing.Size(506, 58);
            this.label_ResultBar.TabIndex = 0;
            this.label_ResultBar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_ResetStatistics
            // 
            this.button_ResetStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_ResetStatistics.Location = new System.Drawing.Point(7, 8);
            this.button_ResetStatistics.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_ResetStatistics.Name = "button_ResetStatistics";
            this.button_ResetStatistics.Size = new System.Drawing.Size(188, 32);
            this.button_ResetStatistics.TabIndex = 0;
            this.button_ResetStatistics.TabStop = false;
            this.button_ResetStatistics.Text = "RESET STATISTICS";
            this.button_ResetStatistics.Click += new System.EventHandler(this.button_ResetStatistics_Click);
            // 
            // button_ResetStatisticsForAllJobs
            // 
            this.button_ResetStatisticsForAllJobs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_ResetStatisticsForAllJobs.Location = new System.Drawing.Point(201, 8);
            this.button_ResetStatisticsForAllJobs.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_ResetStatisticsForAllJobs.Name = "button_ResetStatisticsForAllJobs";
            this.button_ResetStatisticsForAllJobs.Size = new System.Drawing.Size(312, 32);
            this.button_ResetStatisticsForAllJobs.TabIndex = 0;
            this.button_ResetStatisticsForAllJobs.TabStop = false;
            this.button_ResetStatisticsForAllJobs.Text = "RESET STATISTICS FOR ALL JOBS";
            this.button_ResetStatisticsForAllJobs.Click += new System.EventHandler(this.button_ResetStatisticsForAllJobs_Click);
            // 
            // label_controlErrorMessage
            // 
            this.label_controlErrorMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.label_controlErrorMessage, 2);
            this.label_controlErrorMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_controlErrorMessage.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_controlErrorMessage.Location = new System.Drawing.Point(7, 114);
            this.label_controlErrorMessage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_controlErrorMessage.Name = "label_controlErrorMessage";
            this.label_controlErrorMessage.Size = new System.Drawing.Size(506, 58);
            this.label_controlErrorMessage.TabIndex = 0;
            this.label_controlErrorMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox_JobResults
            // 
            this.groupBox_JobResults.Controls.Add(this.tableLayoutPanel2);
            this.groupBox_JobResults.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_JobResults.Location = new System.Drawing.Point(8, 8);
            this.groupBox_JobResults.Name = "groupBox_JobResults";
            this.groupBox_JobResults.Padding = new System.Windows.Forms.Padding(8);
            this.groupBox_JobResults.Size = new System.Drawing.Size(520, 189);
            this.groupBox_JobResults.TabIndex = 0;
            this.groupBox_JobResults.TabStop = false;
            this.groupBox_JobResults.Text = " RESULTS ";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel2.Controls.Add(this.label_JobResults_Percent, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.label_JobResults_TotalIterations, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBox_JobN_TotalAccept_Percent, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label_JobResults_Accept, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBox_JobN_TotalError, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.textBox_JobN_TotalWarning, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label_JobResults_Error, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.textBox_JobN_TotalIterations, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBox_JobN_TotalAccept, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label_JobResults_Reject, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label_JobResults_Warning, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.textBox_JobN_TotalReject, 1, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(8, 27);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(504, 154);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label_JobResults_Percent
            // 
            this.label_JobResults_Percent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_JobResults_Percent.Location = new System.Drawing.Point(473, 30);
            this.label_JobResults_Percent.Name = "label_JobResults_Percent";
            this.label_JobResults_Percent.Size = new System.Drawing.Size(28, 30);
            this.label_JobResults_Percent.TabIndex = 0;
            this.label_JobResults_Percent.Text = "%";
            this.label_JobResults_Percent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_JobResults_TotalIterations
            // 
            this.label_JobResults_TotalIterations.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_JobResults_TotalIterations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_JobResults_TotalIterations.ForeColor = System.Drawing.SystemColors.Window;
            this.label_JobResults_TotalIterations.Location = new System.Drawing.Point(3, 4);
            this.label_JobResults_TotalIterations.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_JobResults_TotalIterations.Name = "label_JobResults_TotalIterations";
            this.label_JobResults_TotalIterations.Size = new System.Drawing.Size(182, 22);
            this.label_JobResults_TotalIterations.TabIndex = 0;
            this.label_JobResults_TotalIterations.Text = "TOTAL ITERATIONS";
            this.label_JobResults_TotalIterations.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_JobN_TotalAccept_Percent
            // 
            this.textBox_JobN_TotalAccept_Percent.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_JobN_TotalAccept_Percent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_JobN_TotalAccept_Percent.Location = new System.Drawing.Point(332, 34);
            this.textBox_JobN_TotalAccept_Percent.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_JobN_TotalAccept_Percent.Name = "textBox_JobN_TotalAccept_Percent";
            this.textBox_JobN_TotalAccept_Percent.ReadOnly = true;
            this.textBox_JobN_TotalAccept_Percent.Size = new System.Drawing.Size(135, 26);
            this.textBox_JobN_TotalAccept_Percent.TabIndex = 0;
            this.textBox_JobN_TotalAccept_Percent.TabStop = false;
            this.textBox_JobN_TotalAccept_Percent.Text = "textBox1";
            this.textBox_JobN_TotalAccept_Percent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label_JobResults_Accept
            // 
            this.label_JobResults_Accept.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_JobResults_Accept.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_JobResults_Accept.ForeColor = System.Drawing.SystemColors.Window;
            this.label_JobResults_Accept.Location = new System.Drawing.Point(3, 34);
            this.label_JobResults_Accept.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_JobResults_Accept.Name = "label_JobResults_Accept";
            this.label_JobResults_Accept.Size = new System.Drawing.Size(182, 22);
            this.label_JobResults_Accept.TabIndex = 0;
            this.label_JobResults_Accept.Text = "ACCEPT";
            this.label_JobResults_Accept.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_JobN_TotalError
            // 
            this.textBox_JobN_TotalError.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_JobN_TotalError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_JobN_TotalError.Location = new System.Drawing.Point(191, 124);
            this.textBox_JobN_TotalError.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_JobN_TotalError.Name = "textBox_JobN_TotalError";
            this.textBox_JobN_TotalError.ReadOnly = true;
            this.textBox_JobN_TotalError.Size = new System.Drawing.Size(135, 26);
            this.textBox_JobN_TotalError.TabIndex = 0;
            this.textBox_JobN_TotalError.TabStop = false;
            this.textBox_JobN_TotalError.Text = "textBox3";
            this.textBox_JobN_TotalError.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox_JobN_TotalWarning
            // 
            this.textBox_JobN_TotalWarning.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_JobN_TotalWarning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_JobN_TotalWarning.Location = new System.Drawing.Point(191, 94);
            this.textBox_JobN_TotalWarning.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_JobN_TotalWarning.Name = "textBox_JobN_TotalWarning";
            this.textBox_JobN_TotalWarning.ReadOnly = true;
            this.textBox_JobN_TotalWarning.Size = new System.Drawing.Size(135, 26);
            this.textBox_JobN_TotalWarning.TabIndex = 0;
            this.textBox_JobN_TotalWarning.TabStop = false;
            this.textBox_JobN_TotalWarning.Text = "textBox4";
            this.textBox_JobN_TotalWarning.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label_JobResults_Error
            // 
            this.label_JobResults_Error.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_JobResults_Error.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_JobResults_Error.ForeColor = System.Drawing.SystemColors.Window;
            this.label_JobResults_Error.Location = new System.Drawing.Point(3, 124);
            this.label_JobResults_Error.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_JobResults_Error.Name = "label_JobResults_Error";
            this.label_JobResults_Error.Size = new System.Drawing.Size(182, 26);
            this.label_JobResults_Error.TabIndex = 0;
            this.label_JobResults_Error.Text = "ERROR";
            this.label_JobResults_Error.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_JobN_TotalIterations
            // 
            this.textBox_JobN_TotalIterations.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_JobN_TotalIterations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_JobN_TotalIterations.Location = new System.Drawing.Point(191, 4);
            this.textBox_JobN_TotalIterations.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_JobN_TotalIterations.Name = "textBox_JobN_TotalIterations";
            this.textBox_JobN_TotalIterations.ReadOnly = true;
            this.textBox_JobN_TotalIterations.Size = new System.Drawing.Size(135, 26);
            this.textBox_JobN_TotalIterations.TabIndex = 0;
            this.textBox_JobN_TotalIterations.TabStop = false;
            this.textBox_JobN_TotalIterations.Text = "textBox1";
            this.textBox_JobN_TotalIterations.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox_JobN_TotalAccept
            // 
            this.textBox_JobN_TotalAccept.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_JobN_TotalAccept.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_JobN_TotalAccept.Location = new System.Drawing.Point(191, 34);
            this.textBox_JobN_TotalAccept.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_JobN_TotalAccept.Name = "textBox_JobN_TotalAccept";
            this.textBox_JobN_TotalAccept.ReadOnly = true;
            this.textBox_JobN_TotalAccept.Size = new System.Drawing.Size(135, 26);
            this.textBox_JobN_TotalAccept.TabIndex = 0;
            this.textBox_JobN_TotalAccept.TabStop = false;
            this.textBox_JobN_TotalAccept.Text = "textBox1";
            this.textBox_JobN_TotalAccept.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label_JobResults_Reject
            // 
            this.label_JobResults_Reject.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_JobResults_Reject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_JobResults_Reject.ForeColor = System.Drawing.SystemColors.Window;
            this.label_JobResults_Reject.Location = new System.Drawing.Point(3, 64);
            this.label_JobResults_Reject.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_JobResults_Reject.Name = "label_JobResults_Reject";
            this.label_JobResults_Reject.Size = new System.Drawing.Size(182, 22);
            this.label_JobResults_Reject.TabIndex = 0;
            this.label_JobResults_Reject.Text = "REJECT";
            this.label_JobResults_Reject.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_JobResults_Warning
            // 
            this.label_JobResults_Warning.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_JobResults_Warning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_JobResults_Warning.ForeColor = System.Drawing.SystemColors.Window;
            this.label_JobResults_Warning.Location = new System.Drawing.Point(3, 94);
            this.label_JobResults_Warning.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_JobResults_Warning.Name = "label_JobResults_Warning";
            this.label_JobResults_Warning.Size = new System.Drawing.Size(182, 22);
            this.label_JobResults_Warning.TabIndex = 0;
            this.label_JobResults_Warning.Text = "WARNING";
            this.label_JobResults_Warning.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_JobN_TotalReject
            // 
            this.textBox_JobN_TotalReject.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_JobN_TotalReject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_JobN_TotalReject.Location = new System.Drawing.Point(191, 64);
            this.textBox_JobN_TotalReject.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_JobN_TotalReject.Name = "textBox_JobN_TotalReject";
            this.textBox_JobN_TotalReject.ReadOnly = true;
            this.textBox_JobN_TotalReject.Size = new System.Drawing.Size(135, 26);
            this.textBox_JobN_TotalReject.TabIndex = 0;
            this.textBox_JobN_TotalReject.TabStop = false;
            this.textBox_JobN_TotalReject.Text = "textBox2";
            this.textBox_JobN_TotalReject.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tabPage_Job0_GUIDEPOINT
            // 
            this.tabPage_Job0_GUIDEPOINT.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_Job0_GUIDEPOINT.Controls.Add(this.tableLayoutPanel13);
            this.tabPage_Job0_GUIDEPOINT.Controls.Add(this.groupBox_GuidePoint_Result);
            this.tabPage_Job0_GUIDEPOINT.Controls.Add(this.groupBox_GuidePoint_Parameters);
            this.tabPage_Job0_GUIDEPOINT.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage_Job0_GUIDEPOINT.Location = new System.Drawing.Point(4, 24);
            this.tabPage_Job0_GUIDEPOINT.Name = "tabPage_Job0_GUIDEPOINT";
            this.tabPage_Job0_GUIDEPOINT.Padding = new System.Windows.Forms.Padding(8);
            this.tabPage_Job0_GUIDEPOINT.Size = new System.Drawing.Size(536, 1020);
            this.tabPage_Job0_GUIDEPOINT.TabIndex = 1;
            this.tabPage_Job0_GUIDEPOINT.Text = "GUIDE POINT";
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.ColumnCount = 4;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel13.Controls.Add(this.buttonGuidePointRun, 3, 1);
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(8, 844);
            this.tableLayoutPanel13.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.Padding = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel13.RowCount = 3;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(520, 168);
            this.tableLayoutPanel13.TabIndex = 0;
            this.tableLayoutPanel13.Visible = false;
            // 
            // buttonGuidePointRun
            // 
            this.buttonGuidePointRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonGuidePointRun.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGuidePointRun.Location = new System.Drawing.Point(391, 58);
            this.buttonGuidePointRun.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonGuidePointRun.Name = "buttonGuidePointRun";
            this.buttonGuidePointRun.Size = new System.Drawing.Size(122, 42);
            this.buttonGuidePointRun.TabIndex = 0;
            this.buttonGuidePointRun.TabStop = false;
            this.buttonGuidePointRun.Text = "RUN";
            this.buttonGuidePointRun.Click += new System.EventHandler(this.OnClickButtonGuidePointRun);
            // 
            // groupBox_GuidePoint_Result
            // 
            this.groupBox_GuidePoint_Result.Controls.Add(this.tableLayoutPanel10);
            this.groupBox_GuidePoint_Result.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_GuidePoint_Result.Location = new System.Drawing.Point(8, 107);
            this.groupBox_GuidePoint_Result.Name = "groupBox_GuidePoint_Result";
            this.groupBox_GuidePoint_Result.Padding = new System.Windows.Forms.Padding(8);
            this.groupBox_GuidePoint_Result.Size = new System.Drawing.Size(520, 247);
            this.groupBox_GuidePoint_Result.TabIndex = 0;
            this.groupBox_GuidePoint_Result.TabStop = false;
            this.groupBox_GuidePoint_Result.Text = "RESULT";
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 4;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel10.Controls.Add(this.textBox_Job0_1stY_1, 1, 2);
            this.tableLayoutPanel10.Controls.Add(this.textBox_Job0_1stX_1, 1, 1);
            this.tableLayoutPanel10.Controls.Add(this.textBox_Job0_VERIFIED_1, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.label_Job0_VERIFIED_1, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.label_Job0_1stX_1, 0, 1);
            this.tableLayoutPanel10.Controls.Add(this.label_Job0_1stY_1, 0, 2);
            this.tableLayoutPanel10.Controls.Add(this.label_Job0_1stAngle_1, 0, 3);
            this.tableLayoutPanel10.Controls.Add(this.textBox_Job0_1stAngle_1, 1, 3);
            this.tableLayoutPanel10.Controls.Add(this.label_Job0_2ndX_1, 2, 1);
            this.tableLayoutPanel10.Controls.Add(this.textBox_Job0_2ndAngle_1, 3, 3);
            this.tableLayoutPanel10.Controls.Add(this.textBox_Job0_2ndY_1, 3, 2);
            this.tableLayoutPanel10.Controls.Add(this.textBox_Job0_2ndX_1, 3, 1);
            this.tableLayoutPanel10.Controls.Add(this.label_Job0_2ndY_1, 2, 2);
            this.tableLayoutPanel10.Controls.Add(this.label_Job0_2ndAngle_1, 2, 3);
            this.tableLayoutPanel10.Controls.Add(this.label_Job0_3rdX, 0, 4);
            this.tableLayoutPanel10.Controls.Add(this.label_Job0_3rdY, 0, 5);
            this.tableLayoutPanel10.Controls.Add(this.label_Job0_3rdAngle, 0, 6);
            this.tableLayoutPanel10.Controls.Add(this.textBox_Job0_3rdX, 1, 4);
            this.tableLayoutPanel10.Controls.Add(this.textBox_Job0_3rdY, 1, 5);
            this.tableLayoutPanel10.Controls.Add(this.textBox_Job0_3rdAngle, 1, 6);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(8, 27);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 7;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(504, 212);
            this.tableLayoutPanel10.TabIndex = 3;
            // 
            // textBox_Job0_1stY_1
            // 
            this.textBox_Job0_1stY_1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_1stY_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_1stY_1.Location = new System.Drawing.Point(129, 63);
            this.textBox_Job0_1stY_1.Name = "textBox_Job0_1stY_1";
            this.textBox_Job0_1stY_1.ReadOnly = true;
            this.textBox_Job0_1stY_1.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_1stY_1.TabIndex = 5;
            this.textBox_Job0_1stY_1.TabStop = false;
            this.textBox_Job0_1stY_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Job0_1stX_1
            // 
            this.textBox_Job0_1stX_1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_1stX_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_1stX_1.Location = new System.Drawing.Point(129, 33);
            this.textBox_Job0_1stX_1.Name = "textBox_Job0_1stX_1";
            this.textBox_Job0_1stX_1.ReadOnly = true;
            this.textBox_Job0_1stX_1.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_1stX_1.TabIndex = 3;
            this.textBox_Job0_1stX_1.TabStop = false;
            this.textBox_Job0_1stX_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Job0_VERIFIED_1
            // 
            this.textBox_Job0_VERIFIED_1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_VERIFIED_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_VERIFIED_1.Location = new System.Drawing.Point(129, 3);
            this.textBox_Job0_VERIFIED_1.Name = "textBox_Job0_VERIFIED_1";
            this.textBox_Job0_VERIFIED_1.ReadOnly = true;
            this.textBox_Job0_VERIFIED_1.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_VERIFIED_1.TabIndex = 1;
            this.textBox_Job0_VERIFIED_1.TabStop = false;
            this.textBox_Job0_VERIFIED_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_VERIFIED_1
            // 
            this.label_Job0_VERIFIED_1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_VERIFIED_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_VERIFIED_1.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_VERIFIED_1.Location = new System.Drawing.Point(3, 3);
            this.label_Job0_VERIFIED_1.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_VERIFIED_1.Name = "label_Job0_VERIFIED_1";
            this.label_Job0_VERIFIED_1.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_VERIFIED_1.TabIndex = 0;
            this.label_Job0_VERIFIED_1.Text = "VERIFIED";
            this.label_Job0_VERIFIED_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Job0_1stX_1
            // 
            this.label_Job0_1stX_1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_1stX_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_1stX_1.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_1stX_1.Location = new System.Drawing.Point(3, 33);
            this.label_Job0_1stX_1.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_1stX_1.Name = "label_Job0_1stX_1";
            this.label_Job0_1stX_1.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_1stX_1.TabIndex = 0;
            this.label_Job0_1stX_1.Text = "1st X";
            this.label_Job0_1stX_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Job0_1stY_1
            // 
            this.label_Job0_1stY_1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_1stY_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_1stY_1.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_1stY_1.Location = new System.Drawing.Point(3, 63);
            this.label_Job0_1stY_1.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_1stY_1.Name = "label_Job0_1stY_1";
            this.label_Job0_1stY_1.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_1stY_1.TabIndex = 0;
            this.label_Job0_1stY_1.Text = "1st Y";
            this.label_Job0_1stY_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Job0_1stAngle_1
            // 
            this.label_Job0_1stAngle_1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_1stAngle_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_1stAngle_1.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_1stAngle_1.Location = new System.Drawing.Point(3, 93);
            this.label_Job0_1stAngle_1.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_1stAngle_1.Name = "label_Job0_1stAngle_1";
            this.label_Job0_1stAngle_1.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_1stAngle_1.TabIndex = 0;
            this.label_Job0_1stAngle_1.Text = "1st Angle";
            this.label_Job0_1stAngle_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_1stAngle_1
            // 
            this.textBox_Job0_1stAngle_1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_1stAngle_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_1stAngle_1.Location = new System.Drawing.Point(129, 93);
            this.textBox_Job0_1stAngle_1.Name = "textBox_Job0_1stAngle_1";
            this.textBox_Job0_1stAngle_1.ReadOnly = true;
            this.textBox_Job0_1stAngle_1.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_1stAngle_1.TabIndex = 26;
            this.textBox_Job0_1stAngle_1.TabStop = false;
            this.textBox_Job0_1stAngle_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_2ndX_1
            // 
            this.label_Job0_2ndX_1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_2ndX_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_2ndX_1.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_2ndX_1.Location = new System.Drawing.Point(255, 33);
            this.label_Job0_2ndX_1.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_2ndX_1.Name = "label_Job0_2ndX_1";
            this.label_Job0_2ndX_1.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_2ndX_1.TabIndex = 0;
            this.label_Job0_2ndX_1.Text = "2nd X";
            this.label_Job0_2ndX_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_2ndAngle_1
            // 
            this.textBox_Job0_2ndAngle_1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_2ndAngle_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_2ndAngle_1.Location = new System.Drawing.Point(381, 93);
            this.textBox_Job0_2ndAngle_1.Name = "textBox_Job0_2ndAngle_1";
            this.textBox_Job0_2ndAngle_1.ReadOnly = true;
            this.textBox_Job0_2ndAngle_1.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_2ndAngle_1.TabIndex = 27;
            this.textBox_Job0_2ndAngle_1.TabStop = false;
            this.textBox_Job0_2ndAngle_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Job0_2ndY_1
            // 
            this.textBox_Job0_2ndY_1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_2ndY_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_2ndY_1.Location = new System.Drawing.Point(381, 63);
            this.textBox_Job0_2ndY_1.Name = "textBox_Job0_2ndY_1";
            this.textBox_Job0_2ndY_1.ReadOnly = true;
            this.textBox_Job0_2ndY_1.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_2ndY_1.TabIndex = 21;
            this.textBox_Job0_2ndY_1.TabStop = false;
            this.textBox_Job0_2ndY_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Job0_2ndX_1
            // 
            this.textBox_Job0_2ndX_1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_2ndX_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_2ndX_1.Location = new System.Drawing.Point(381, 33);
            this.textBox_Job0_2ndX_1.Name = "textBox_Job0_2ndX_1";
            this.textBox_Job0_2ndX_1.ReadOnly = true;
            this.textBox_Job0_2ndX_1.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_2ndX_1.TabIndex = 19;
            this.textBox_Job0_2ndX_1.TabStop = false;
            this.textBox_Job0_2ndX_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_2ndY_1
            // 
            this.label_Job0_2ndY_1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_2ndY_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_2ndY_1.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_2ndY_1.Location = new System.Drawing.Point(255, 63);
            this.label_Job0_2ndY_1.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_2ndY_1.Name = "label_Job0_2ndY_1";
            this.label_Job0_2ndY_1.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_2ndY_1.TabIndex = 0;
            this.label_Job0_2ndY_1.Text = "2nd Y";
            this.label_Job0_2ndY_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Job0_2ndAngle_1
            // 
            this.label_Job0_2ndAngle_1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_2ndAngle_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_2ndAngle_1.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_2ndAngle_1.Location = new System.Drawing.Point(255, 93);
            this.label_Job0_2ndAngle_1.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_2ndAngle_1.Name = "label_Job0_2ndAngle_1";
            this.label_Job0_2ndAngle_1.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_2ndAngle_1.TabIndex = 0;
            this.label_Job0_2ndAngle_1.Text = "2nd Angle";
            this.label_Job0_2ndAngle_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Job0_3rdX
            // 
            this.label_Job0_3rdX.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_3rdX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_3rdX.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_3rdX.Location = new System.Drawing.Point(3, 123);
            this.label_Job0_3rdX.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_3rdX.Name = "label_Job0_3rdX";
            this.label_Job0_3rdX.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_3rdX.TabIndex = 0;
            this.label_Job0_3rdX.Text = "3rd X";
            this.label_Job0_3rdX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Job0_3rdY
            // 
            this.label_Job0_3rdY.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_3rdY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_3rdY.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_3rdY.Location = new System.Drawing.Point(3, 153);
            this.label_Job0_3rdY.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_3rdY.Name = "label_Job0_3rdY";
            this.label_Job0_3rdY.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_3rdY.TabIndex = 0;
            this.label_Job0_3rdY.Text = "3rd Y";
            this.label_Job0_3rdY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Job0_3rdAngle
            // 
            this.label_Job0_3rdAngle.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_3rdAngle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_3rdAngle.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_3rdAngle.Location = new System.Drawing.Point(3, 183);
            this.label_Job0_3rdAngle.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_3rdAngle.Name = "label_Job0_3rdAngle";
            this.label_Job0_3rdAngle.Size = new System.Drawing.Size(120, 26);
            this.label_Job0_3rdAngle.TabIndex = 0;
            this.label_Job0_3rdAngle.Text = "3rd Angle";
            this.label_Job0_3rdAngle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Job0_3rdX
            // 
            this.textBox_Job0_3rdX.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_3rdX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_3rdX.Location = new System.Drawing.Point(129, 123);
            this.textBox_Job0_3rdX.Name = "textBox_Job0_3rdX";
            this.textBox_Job0_3rdX.ReadOnly = true;
            this.textBox_Job0_3rdX.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_3rdX.TabIndex = 7;
            this.textBox_Job0_3rdX.TabStop = false;
            this.textBox_Job0_3rdX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Job0_3rdY
            // 
            this.textBox_Job0_3rdY.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_3rdY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_3rdY.Location = new System.Drawing.Point(129, 153);
            this.textBox_Job0_3rdY.Name = "textBox_Job0_3rdY";
            this.textBox_Job0_3rdY.ReadOnly = true;
            this.textBox_Job0_3rdY.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_3rdY.TabIndex = 9;
            this.textBox_Job0_3rdY.TabStop = false;
            this.textBox_Job0_3rdY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Job0_3rdAngle
            // 
            this.textBox_Job0_3rdAngle.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Job0_3rdAngle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_3rdAngle.Location = new System.Drawing.Point(129, 183);
            this.textBox_Job0_3rdAngle.Name = "textBox_Job0_3rdAngle";
            this.textBox_Job0_3rdAngle.ReadOnly = true;
            this.textBox_Job0_3rdAngle.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_3rdAngle.TabIndex = 29;
            this.textBox_Job0_3rdAngle.TabStop = false;
            this.textBox_Job0_3rdAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox_GuidePoint_Parameters
            // 
            this.groupBox_GuidePoint_Parameters.Controls.Add(this.tableLayoutPanel11);
            this.groupBox_GuidePoint_Parameters.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_GuidePoint_Parameters.Location = new System.Drawing.Point(8, 8);
            this.groupBox_GuidePoint_Parameters.Name = "groupBox_GuidePoint_Parameters";
            this.groupBox_GuidePoint_Parameters.Padding = new System.Windows.Forms.Padding(8);
            this.groupBox_GuidePoint_Parameters.Size = new System.Drawing.Size(520, 99);
            this.groupBox_GuidePoint_Parameters.TabIndex = 0;
            this.groupBox_GuidePoint_Parameters.TabStop = false;
            this.groupBox_GuidePoint_Parameters.Text = "PARAMETERS";
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.ColumnCount = 4;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel11.Controls.Add(this.label_Job0_PITCHX, 0, 1);
            this.tableLayoutPanel11.Controls.Add(this.label_Job0_PITCHY, 2, 1);
            this.tableLayoutPanel11.Controls.Add(this.textBox_Job0_PITCHX, 1, 1);
            this.tableLayoutPanel11.Controls.Add(this.textBox_Job0_PITCHY, 3, 1);
            this.tableLayoutPanel11.Controls.Add(this.textBox_Job0_GUIDEPOINTMODE, 1, 0);
            this.tableLayoutPanel11.Controls.Add(this.label_Job0_GUIDEPOINTMODE, 0, 0);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(8, 27);
            this.tableLayoutPanel11.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 2;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(504, 64);
            this.tableLayoutPanel11.TabIndex = 3;
            // 
            // label_Job0_PITCHX
            // 
            this.label_Job0_PITCHX.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_PITCHX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_PITCHX.Enabled = false;
            this.label_Job0_PITCHX.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_PITCHX.Location = new System.Drawing.Point(3, 33);
            this.label_Job0_PITCHX.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_PITCHX.Name = "label_Job0_PITCHX";
            this.label_Job0_PITCHX.Size = new System.Drawing.Size(120, 28);
            this.label_Job0_PITCHX.TabIndex = 2;
            this.label_Job0_PITCHX.Text = "PITCH X";
            this.label_Job0_PITCHX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label_Job0_PITCHX.Visible = false;
            // 
            // label_Job0_PITCHY
            // 
            this.label_Job0_PITCHY.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_PITCHY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_PITCHY.Enabled = false;
            this.label_Job0_PITCHY.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_PITCHY.Location = new System.Drawing.Point(255, 33);
            this.label_Job0_PITCHY.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_PITCHY.Name = "label_Job0_PITCHY";
            this.label_Job0_PITCHY.Size = new System.Drawing.Size(120, 28);
            this.label_Job0_PITCHY.TabIndex = 4;
            this.label_Job0_PITCHY.Text = "PITCH Y";
            this.label_Job0_PITCHY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label_Job0_PITCHY.Visible = false;
            // 
            // textBox_Job0_PITCHX
            // 
            this.textBox_Job0_PITCHX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_PITCHX.Enabled = false;
            this.textBox_Job0_PITCHX.Location = new System.Drawing.Point(129, 33);
            this.textBox_Job0_PITCHX.Name = "textBox_Job0_PITCHX";
            this.textBox_Job0_PITCHX.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_PITCHX.TabIndex = 3;
            this.textBox_Job0_PITCHX.Text = "-1";
            this.textBox_Job0_PITCHX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_Job0_PITCHX.Visible = false;
            // 
            // textBox_Job0_PITCHY
            // 
            this.textBox_Job0_PITCHY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_PITCHY.Enabled = false;
            this.textBox_Job0_PITCHY.Location = new System.Drawing.Point(381, 33);
            this.textBox_Job0_PITCHY.Name = "textBox_Job0_PITCHY";
            this.textBox_Job0_PITCHY.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_PITCHY.TabIndex = 5;
            this.textBox_Job0_PITCHY.Text = "-1";
            this.textBox_Job0_PITCHY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_Job0_PITCHY.Visible = false;
            // 
            // textBox_Job0_GUIDEPOINTMODE
            // 
            this.textBox_Job0_GUIDEPOINTMODE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Job0_GUIDEPOINTMODE.Location = new System.Drawing.Point(129, 3);
            this.textBox_Job0_GUIDEPOINTMODE.Name = "textBox_Job0_GUIDEPOINTMODE";
            this.textBox_Job0_GUIDEPOINTMODE.Size = new System.Drawing.Size(120, 26);
            this.textBox_Job0_GUIDEPOINTMODE.TabIndex = 6;
            this.textBox_Job0_GUIDEPOINTMODE.TabStop = false;
            this.textBox_Job0_GUIDEPOINTMODE.Text = "-1";
            this.textBox_Job0_GUIDEPOINTMODE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_Job0_GUIDEPOINTMODE
            // 
            this.label_Job0_GUIDEPOINTMODE.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_Job0_GUIDEPOINTMODE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Job0_GUIDEPOINTMODE.ForeColor = System.Drawing.SystemColors.Window;
            this.label_Job0_GUIDEPOINTMODE.Location = new System.Drawing.Point(3, 3);
            this.label_Job0_GUIDEPOINTMODE.Margin = new System.Windows.Forms.Padding(3);
            this.label_Job0_GUIDEPOINTMODE.Name = "label_Job0_GUIDEPOINTMODE";
            this.label_Job0_GUIDEPOINTMODE.Size = new System.Drawing.Size(120, 24);
            this.label_Job0_GUIDEPOINTMODE.TabIndex = 7;
            this.label_Job0_GUIDEPOINTMODE.Text = "MODE";
            this.label_Job0_GUIDEPOINTMODE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // applicationErrorProvider
            // 
            this.applicationErrorProvider.ContainerControl = this;
            // 
            // splitContainerCompositeImageProcessControl
            // 
            this.splitContainerCompositeImageProcessControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerCompositeImageProcessControl.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.splitContainerCompositeImageProcessControl.Location = new System.Drawing.Point(0, 0);
            this.splitContainerCompositeImageProcessControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainerCompositeImageProcessControl.Name = "splitContainerCompositeImageProcessControl";
            // 
            // splitContainerCompositeImageProcessControl.Panel1
            // 
            this.splitContainerCompositeImageProcessControl.Panel1.Controls.Add(this.cogRecordsDisplay1);
            // 
            // splitContainerCompositeImageProcessControl.Panel2
            // 
            this.splitContainerCompositeImageProcessControl.Panel2.Controls.Add(this.tabControlJobTabs);
            this.splitContainerCompositeImageProcessControl.Panel2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.splitContainerCompositeImageProcessControl.Size = new System.Drawing.Size(1437, 1048);
            this.splitContainerCompositeImageProcessControl.SplitterDistance = 889;
            this.splitContainerCompositeImageProcessControl.TabIndex = 30;
            // 
            // cogRecordsDisplay1
            // 
            this.cogRecordsDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogRecordsDisplay1.Location = new System.Drawing.Point(0, 0);
            this.cogRecordsDisplay1.Name = "cogRecordsDisplay1";
            this.cogRecordsDisplay1.SelectedRecordKey = null;
            this.cogRecordsDisplay1.ShowRecordsDropDown = true;
            this.cogRecordsDisplay1.Size = new System.Drawing.Size(889, 1048);
            this.cogRecordsDisplay1.Subject = null;
            this.cogRecordsDisplay1.TabIndex = 0;
            // 
            // CompositeImageProcessControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.splitContainerCompositeImageProcessControl);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "CompositeImageProcessControl";
            this.Size = new System.Drawing.Size(1437, 1048);
            this.tabPage_Job0_URL.ResumeLayout(false);
            this.groupBox_RESULT.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel7.ResumeLayout(false);
            this.groupBox_MODEL.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tabPage_Job0_BASEPOINT.ResumeLayout(false);
            this.tableLayoutPanel12.ResumeLayout(false);
            this.groupBox_RESULT_1.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.groupBox_PARAMETERS.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.groupBox_AcquisitionResults.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.groupBox_JobThroughput.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tabControlJobTabs.ResumeLayout(false);
            this.tabPage_JobN_JobStatistics.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox_JobResults.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tabPage_Job0_GUIDEPOINT.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.groupBox_GuidePoint_Result.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.groupBox_GuidePoint_Parameters.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.applicationErrorProvider)).EndInit();
            this.splitContainerCompositeImageProcessControl.Panel1.ResumeLayout(false);
            this.splitContainerCompositeImageProcessControl.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerCompositeImageProcessControl)).EndInit();
            this.splitContainerCompositeImageProcessControl.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    ///////////////////////// START WIZARD GENERATED
    // cognex.wizard.controldeclarations.begin
    private Cognex.VisionPro.CogToolPropertyProvider cogToolPropertyProvider;
    private TabPage tabPage_Job0_URL;
    private GroupBox groupBox_MODEL;
    private CheckBox checkBox_Job0_USEFOUNDCIRCLEVERIFY;
    private CheckBox checkBox_Job0_USEREELCENTERPATTERN;
    private Label label_Job0_PROCESS;
    private Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox textBox_Job0_PROCESS;
    private Label label_Job0_RMSERROR;
    private Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox textBox_Job0_RMSERROR;
    private Label label_Job0_SCORELIMIT;
    private Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox textBox_Job0_SCORELIMIT;
    private Label label_Job0_LIMITX;
    private Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox textBox_Job0_LIMITX;
    private Label label_Job0_LIMITY;
    private Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox textBox_Job0_LIMITY;
    private Label label_Job0_TARGETPOINT;
    private Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox textBox_Job0_TARGETPOINT;
    private Label label_Job0_REEL7;
    private Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox textBox_Job0_REEL7;
    private Label label_Job0_REEL13;
    private Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox textBox_Job0_REEL13;
    private TabPage tabPage_Job0_BASEPOINT;
    private GroupBox groupBox_PARAMETERS;
    private Label label_Job0_BASEPOINTMODE;
    private Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox textBox_Job0_BASEPOINTMODE;
    private Label label_Job0_REFERENCEX;
    private Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox textBox_Job0_REFERENCEX;
    private Label label_Job0_REFERENCEY;
    private Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox textBox_Job0_REFERENCEY;
    // cognex.wizard.controldeclarations.end
    ///////////////////////// END WIZARD GENERATED

    private System.Windows.Forms.Label label_AcquisitionResults_Overruns;
    private System.Windows.Forms.TextBox textBox_JobN_TotalAcquisitionErrors;
    private System.Windows.Forms.TextBox textBox_JobN_TotalAcquisitionOverruns;
    private System.Windows.Forms.GroupBox groupBox_AcquisitionResults;
    private System.Windows.Forms.Label label_AcquisitionResults_Errors;
    private System.Windows.Forms.TextBox textBox_JobN_TotalAcquisitions;
    private System.Windows.Forms.Label label_AcquisitionResults_TotalAcquisitions;
    private System.Windows.Forms.Label label_JobThroughput_persec;
    private System.Windows.Forms.GroupBox groupBox_JobThroughput;
    private System.Windows.Forms.TextBox textBox_JobN_MaxThroughput;
    private System.Windows.Forms.TextBox textBox_JobN_MinThroughput;
    private System.Windows.Forms.Label label_JobThroughput_Max;
    private System.Windows.Forms.Label label_JobThroughput_Min;
    private System.Windows.Forms.TextBox textBox_JobN_Throughput;
    private System.Windows.Forms.Label label_JobThroughput_TotalThroughput;
    private System.Windows.Forms.TabControl tabControlJobTabs;
    private System.Windows.Forms.TabPage tabPage_JobN_JobStatistics;
    private System.Windows.Forms.Button button_ResetStatistics;
    private System.Windows.Forms.Button button_ResetStatisticsForAllJobs;
    private System.Windows.Forms.GroupBox groupBox_JobResults;
    private System.Windows.Forms.Label label_JobResults_Percent;
    private System.Windows.Forms.TextBox textBox_JobN_TotalAccept_Percent;
    private System.Windows.Forms.TextBox textBox_JobN_TotalWarning;
    private System.Windows.Forms.TextBox textBox_JobN_TotalError;
    private System.Windows.Forms.Label label_JobResults_Error;
    private System.Windows.Forms.Label label_JobResults_Warning;
    private System.Windows.Forms.TextBox textBox_JobN_TotalReject;
    private System.Windows.Forms.Label label_JobResults_Reject;
    private System.Windows.Forms.TextBox textBox_JobN_TotalAccept;
    private System.Windows.Forms.Label label_JobResults_Accept;
    private System.Windows.Forms.TextBox textBox_JobN_TotalIterations;
    private System.Windows.Forms.Label label_JobResults_TotalIterations;
    private System.Windows.Forms.Label label_ResultBar;
    private System.Windows.Forms.ErrorProvider applicationErrorProvider;
    private System.Windows.Forms.Label label_controlErrorMessage;
        private Button btnRun;
        private Button button_SaveSettings;
        private CheckBox checkBox_LiveDisplay;
        private Button button_Configuration;
        private TableLayoutPanel tableLayoutPanel5;
        private TableLayoutPanel tableLayoutPanel4;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private SplitContainer splitContainerCompositeImageProcessControl;
        private GroupBox groupBox_RESULT;
        private TableLayoutPanel tableLayoutPanel6;
        private Label label_Job0_CODE;
        private TextBox textBox_Job0_MESSAGE;
        private Label label_Job0_DECODEDATA;
        private TextBox textBox_Job0_REMAINEDPPM;
        private Label label_Job0_REMAINEDPPM;
        private TextBox textBox_Job0_BARCODEPPM;
        private Label label_Job0_BARCODEPPM;
        private TextBox textBox_Job0_QRCOUNT;
        private Label label_Job0_QRCOUNT;
        private TextBox textBox_Job0_REMAINED;
        private Label label_Job0_REMAINED;
        private TextBox textBox_Job0_RMSERRORVALUE;
        private Label label_Job0_RMSERRORValue;
        private TextBox textBox_Job0_SCALING;
        private Label label_Job0_SCALING;
        private TextBox textBox_Job0_RADIUS;
        private Label label_Job0_RADIUS;
        private TextBox textBox_Job0_SLOTEMPTY;
        private Label label_Job0_SLOTEMPTY;
        private TextBox textBox_Job0_SCORE;
        private Label label_Job0_SCORE;
        private TextBox textBox_Job0_CENTERY;
        private TextBox textBox_Job0_CENTERX;
        private Label label_Job0_CENTERX;
        private Label label_Job0_CENTERY;
        private TextBox textBox_Job0_CARTTYPE;
        private Label label_Job0_CARTTYPE;
        private TextBox textBox_Job0_TOTALTIME;
        private Label label_Job0_TOTALTIME;
        private TextBox textBox_Job0_PSTIME;
        private Label label_Job0_PSTIME;
        private TextBox textBox_Job0_STATUS;
        private TextBox textBox_Job0_CODE;
        private Label label_Job0_STATUS;
        private TextBox textBox_Job0_DECODEDATA;
        private TableLayoutPanel tableLayoutPanel7;
        private Button buttonOpenLight;
        private GroupBox groupBox_RESULT_1;
        private Label label_Job0_VERIFIED;
        private TextBox textBox_Job0_VERIFIED;
        private Label label_Job0_FOUND1st;
        private TextBox textBox_Job0_FOUND1st;
        private Label label_Job0_FOUND2nd;
        private TextBox textBox_Job0_FOUND2nd;
        private Label label_Job0_1stX;
        private TextBox textBox_Job0_1stX;
        private Label label_Job0_1stY;
        private TextBox textBox_Job0_1stY;
        private Label label_Job0_1stAngle;
        private TextBox textBox_Job0_1stAngle;
        private Label label_Job0_2ndX;
        private TextBox textBox_Job0_2ndX;
        private Label label_Job0_2ndY;
        private TextBox textBox_Job0_2ndY;
        private Label label_Job0_2ndAngle;
        private TextBox textBox_Job0_2ndAngle;
        private Label label_Job0_DISX;
        private TextBox textBox_Job0_DISX;
        private Label label_Job0_DISY;
        private TextBox textBox_Job0_DISY;
        private Label label_Job0_DISAngle;
        private TextBox textBox_Job0_DISAngle;
        private TableLayoutPanel tableLayoutPanel9;
        private TableLayoutPanel tableLayoutPanel8;
        private TabPage tabPage_Job0_GUIDEPOINT;
        private GroupBox groupBox_GuidePoint_Result;
        private TableLayoutPanel tableLayoutPanel10;
        private TextBox textBox_Job0_2ndY_1;
        private TextBox textBox_Job0_2ndX_1;
        private Label label_Job0_2ndX_1;
        private Label label_Job0_2ndY_1;
        private TextBox textBox_Job0_3rdY;
        private TextBox textBox_Job0_3rdX;
        private TextBox textBox_Job0_1stY_1;
        private TextBox textBox_Job0_1stX_1;
        private TextBox textBox_Job0_VERIFIED_1;
        private Label label_Job0_VERIFIED_1;
        private Label label_Job0_1stX_1;
        private Label label_Job0_1stY_1;
        private Label label_Job0_3rdX;
        private Label label_Job0_3rdY;
        private GroupBox groupBox_GuidePoint_Parameters;
        private TableLayoutPanel tableLayoutPanel11;
        private Label label_Job0_PITCHX;
        private Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox textBox_Job0_PITCHX;
        private Label label_Job0_PITCHY;
        private Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox textBox_Job0_PITCHY;
        private TableLayoutPanel tableLayoutPanel12;
        private Button buttonBasePointRun;
        private TableLayoutPanel tableLayoutPanel13;
        private Button buttonGuidePointRun;
        private TextBox textBox_Job0_3rdAngle;
        private Label label_Job0_3rdAngle;
        private Label label_Job0_1stAngle_1;
        private Label label_Job0_2ndAngle_1;
        private TextBox textBox_Job0_1stAngle_1;
        private TextBox textBox_Job0_2ndAngle_1;
        private Cognex.VisionPro.CogRecordsDisplay cogRecordsDisplay1;
        private Cognex.VisionPro.QuickBuild.Implementation.Internal.CogApplicationWizardNumberBox textBox_Job0_GUIDEPOINTMODE;
        private Label label_Job0_GUIDEPOINTMODE;
    }
}
