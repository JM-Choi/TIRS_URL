namespace ProcessWatcher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.groupBoxEquipmentStatus = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelAlarmCodeValue = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelEquipmentStatusValue = new System.Windows.Forms.Label();
            this.labelAlarmMessageValue = new System.Windows.Forms.Label();
            this.groupBoxHostInterfaceStatus = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelHostInterface = new System.Windows.Forms.TableLayoutPanel();
            this.labelHostConnection = new System.Windows.Forms.Label();
            this.labelHostAddressValue = new System.Windows.Forms.Label();
            this.checkBoxHostConnectionValue = new System.Windows.Forms.CheckBox();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemShow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemHide = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBoxEquipmentStatus.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxHostInterfaceStatus.SuspendLayout();
            this.tableLayoutPanelHostInterface.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxEquipmentStatus
            // 
            this.groupBoxEquipmentStatus.Controls.Add(this.tableLayoutPanel1);
            this.groupBoxEquipmentStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxEquipmentStatus.Location = new System.Drawing.Point(7, 77);
            this.groupBoxEquipmentStatus.Name = "groupBoxEquipmentStatus";
            this.groupBoxEquipmentStatus.Padding = new System.Windows.Forms.Padding(7);
            this.groupBoxEquipmentStatus.Size = new System.Drawing.Size(466, 113);
            this.groupBoxEquipmentStatus.TabIndex = 0;
            this.groupBoxEquipmentStatus.TabStop = false;
            this.groupBoxEquipmentStatus.Text = " EQUIPMENT ";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.labelAlarmCodeValue, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelStatus, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelEquipmentStatusValue, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelAlarmMessageValue, 2, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(7, 21);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(452, 85);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // labelAlarmCodeValue
            // 
            this.labelAlarmCodeValue.AutoSize = true;
            this.labelAlarmCodeValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelAlarmCodeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAlarmCodeValue.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAlarmCodeValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelAlarmCodeValue.Location = new System.Drawing.Point(0, 44);
            this.labelAlarmCodeValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelAlarmCodeValue.Name = "labelAlarmCodeValue";
            this.labelAlarmCodeValue.Size = new System.Drawing.Size(100, 41);
            this.labelAlarmCodeValue.TabIndex = 0;
            this.labelAlarmCodeValue.Text = "ALARM CODE";
            this.labelAlarmCodeValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.ForeColor = System.Drawing.SystemColors.Window;
            this.labelStatus.Location = new System.Drawing.Point(0, 0);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(100, 41);
            this.labelStatus.TabIndex = 0;
            this.labelStatus.Text = "STATUS";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelEquipmentStatusValue
            // 
            this.labelEquipmentStatusValue.AutoSize = true;
            this.labelEquipmentStatusValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelEquipmentStatusValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.labelEquipmentStatusValue, 3);
            this.labelEquipmentStatusValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEquipmentStatusValue.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEquipmentStatusValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelEquipmentStatusValue.Location = new System.Drawing.Point(103, 0);
            this.labelEquipmentStatusValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelEquipmentStatusValue.Name = "labelEquipmentStatusValue";
            this.labelEquipmentStatusValue.Size = new System.Drawing.Size(349, 41);
            this.labelEquipmentStatusValue.TabIndex = 0;
            this.labelEquipmentStatusValue.Text = "IDLE";
            this.labelEquipmentStatusValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelAlarmMessageValue
            // 
            this.labelAlarmMessageValue.AutoSize = true;
            this.labelAlarmMessageValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelAlarmMessageValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.labelAlarmMessageValue, 3);
            this.labelAlarmMessageValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAlarmMessageValue.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAlarmMessageValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelAlarmMessageValue.Location = new System.Drawing.Point(103, 44);
            this.labelAlarmMessageValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelAlarmMessageValue.Name = "labelAlarmMessageValue";
            this.labelAlarmMessageValue.Size = new System.Drawing.Size(349, 41);
            this.labelAlarmMessageValue.TabIndex = 0;
            this.labelAlarmMessageValue.Text = "ALARM MESSAGE";
            this.labelAlarmMessageValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxHostInterfaceStatus
            // 
            this.groupBoxHostInterfaceStatus.Controls.Add(this.tableLayoutPanelHostInterface);
            this.groupBoxHostInterfaceStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxHostInterfaceStatus.Location = new System.Drawing.Point(7, 7);
            this.groupBoxHostInterfaceStatus.Name = "groupBoxHostInterfaceStatus";
            this.groupBoxHostInterfaceStatus.Padding = new System.Windows.Forms.Padding(7);
            this.groupBoxHostInterfaceStatus.Size = new System.Drawing.Size(466, 70);
            this.groupBoxHostInterfaceStatus.TabIndex = 0;
            this.groupBoxHostInterfaceStatus.TabStop = false;
            this.groupBoxHostInterfaceStatus.Text = " HOST INTERFACE ";
            // 
            // tableLayoutPanelHostInterface
            // 
            this.tableLayoutPanelHostInterface.ColumnCount = 5;
            this.tableLayoutPanelHostInterface.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelHostInterface.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanelHostInterface.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelHostInterface.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanelHostInterface.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanelHostInterface.Controls.Add(this.labelHostConnection, 0, 0);
            this.tableLayoutPanelHostInterface.Controls.Add(this.labelHostAddressValue, 2, 0);
            this.tableLayoutPanelHostInterface.Controls.Add(this.checkBoxHostConnectionValue, 4, 0);
            this.tableLayoutPanelHostInterface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelHostInterface.Location = new System.Drawing.Point(7, 21);
            this.tableLayoutPanelHostInterface.Name = "tableLayoutPanelHostInterface";
            this.tableLayoutPanelHostInterface.RowCount = 1;
            this.tableLayoutPanelHostInterface.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelHostInterface.Size = new System.Drawing.Size(452, 42);
            this.tableLayoutPanelHostInterface.TabIndex = 0;
            this.tableLayoutPanelHostInterface.UseWaitCursor = true;
            // 
            // labelHostConnection
            // 
            this.labelHostConnection.AutoSize = true;
            this.labelHostConnection.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelHostConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHostConnection.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHostConnection.ForeColor = System.Drawing.SystemColors.Window;
            this.labelHostConnection.Location = new System.Drawing.Point(0, 0);
            this.labelHostConnection.Margin = new System.Windows.Forms.Padding(0);
            this.labelHostConnection.Name = "labelHostConnection";
            this.labelHostConnection.Size = new System.Drawing.Size(100, 42);
            this.labelHostConnection.TabIndex = 0;
            this.labelHostConnection.Text = "HOST";
            this.labelHostConnection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelHostConnection.UseWaitCursor = true;
            // 
            // labelHostAddressValue
            // 
            this.labelHostAddressValue.AutoSize = true;
            this.labelHostAddressValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelHostAddressValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHostAddressValue.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHostAddressValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelHostAddressValue.Location = new System.Drawing.Point(103, 0);
            this.labelHostAddressValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelHostAddressValue.Name = "labelHostAddressValue";
            this.labelHostAddressValue.Size = new System.Drawing.Size(226, 42);
            this.labelHostAddressValue.TabIndex = 0;
            this.labelHostAddressValue.Text = "HOST ADDRESS";
            this.labelHostAddressValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelHostAddressValue.UseWaitCursor = true;
            // 
            // checkBoxHostConnectionValue
            // 
            this.checkBoxHostConnectionValue.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxHostConnectionValue.AutoCheck = false;
            this.checkBoxHostConnectionValue.AutoSize = true;
            this.checkBoxHostConnectionValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxHostConnectionValue.Location = new System.Drawing.Point(332, 0);
            this.checkBoxHostConnectionValue.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxHostConnectionValue.Name = "checkBoxHostConnectionValue";
            this.checkBoxHostConnectionValue.Size = new System.Drawing.Size(120, 42);
            this.checkBoxHostConnectionValue.TabIndex = 0;
            this.checkBoxHostConnectionValue.TabStop = false;
            this.checkBoxHostConnectionValue.Text = "DISCONNECTED";
            this.checkBoxHostConnectionValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxHostConnectionValue.UseVisualStyleBackColor = true;
            this.checkBoxHostConnectionValue.UseWaitCursor = true;
            this.checkBoxHostConnectionValue.CheckedChanged += new System.EventHandler(this.OnCheckedChangedCheckBoxHostConnectionValue);
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipText = "The manager is running in minimized state.";
            this.notifyIcon.BalloonTipTitle = "TechFloor Process Watcher Service Manager";
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "TechFloor Process Watcher Service Manager";
            this.notifyIcon.Visible = true;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemShow,
            this.toolStripMenuItemHide,
            this.toolStripSeparator1,
            this.toolStripMenuItemConfig,
            this.toolStripSeparator2,
            this.toolStripMenuItemExit});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(111, 104);
            // 
            // toolStripMenuItemShow
            // 
            this.toolStripMenuItemShow.Name = "toolStripMenuItemShow";
            this.toolStripMenuItemShow.Size = new System.Drawing.Size(110, 22);
            this.toolStripMenuItemShow.Text = "Show";
            this.toolStripMenuItemShow.Click += new System.EventHandler(this.OnDoubleClicked);
            // 
            // toolStripMenuItemHide
            // 
            this.toolStripMenuItemHide.Name = "toolStripMenuItemHide";
            this.toolStripMenuItemHide.Size = new System.Drawing.Size(110, 22);
            this.toolStripMenuItemHide.Text = "Hide";
            this.toolStripMenuItemHide.Click += new System.EventHandler(this.OnHide);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(107, 6);
            // 
            // toolStripMenuItemConfig
            // 
            this.toolStripMenuItemConfig.Name = "toolStripMenuItemConfig";
            this.toolStripMenuItemConfig.Size = new System.Drawing.Size(110, 22);
            this.toolStripMenuItemConfig.Text = "Config";
            this.toolStripMenuItemConfig.Click += new System.EventHandler(this.OnConfig);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(107, 6);
            // 
            // toolStripMenuItemExit
            // 
            this.toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            this.toolStripMenuItemExit.Size = new System.Drawing.Size(110, 22);
            this.toolStripMenuItemExit.Text = "Exit";
            this.toolStripMenuItemExit.Click += new System.EventHandler(this.OnExitApplication);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // FormMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(480, 197);
            this.Controls.Add(this.groupBoxEquipmentStatus);
            this.Controls.Add(this.groupBoxHostInterfaceStatus);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 240);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 240);
            this.Name = "FormMain";
            this.Padding = new System.Windows.Forms.Padding(7);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProcessWatcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.Shown += new System.EventHandler(this.OnFormShown);
            this.groupBoxEquipmentStatus.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBoxHostInterfaceStatus.ResumeLayout(false);
            this.tableLayoutPanelHostInterface.ResumeLayout(false);
            this.tableLayoutPanelHostInterface.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        protected System.Windows.Forms.GroupBox groupBoxEquipmentStatus;
        protected System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        protected System.Windows.Forms.Label labelAlarmCodeValue;
        protected System.Windows.Forms.Label labelStatus;
        protected System.Windows.Forms.Label labelEquipmentStatusValue;
        protected System.Windows.Forms.Label labelAlarmMessageValue;
        protected System.Windows.Forms.GroupBox groupBoxHostInterfaceStatus;
        protected System.Windows.Forms.TableLayoutPanel tableLayoutPanelHostInterface;
        protected System.Windows.Forms.Label labelHostConnection;
        protected System.Windows.Forms.Label labelHostAddressValue;
        protected System.Windows.Forms.CheckBox checkBoxHostConnectionValue;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        protected System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShow;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemHide;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemConfig;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

