namespace ProcessWatcher
{
    partial class FormConfig
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
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfig));
            this.groupBoxHostSetting = new System.Windows.Forms.GroupBox();
            this.checkBoxSimulationMode = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanelConfiguration = new System.Windows.Forms.TableLayoutPanel();
            this.labelLindeCode = new System.Windows.Forms.Label();
            this.labelProcessCode = new System.Windows.Forms.Label();
            this.labelEquipmentId = new System.Windows.Forms.Label();
            this.textBoxLineCodeValue = new System.Windows.Forms.TextBox();
            this.textBoxProcessCodeValue = new System.Windows.Forms.TextBox();
            this.textBoxEquipmentIdValue = new System.Windows.Forms.TextBox();
            this.groupBoxProcessSetting = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxListenerAddressValue = new System.Windows.Forms.TextBox();
            this.labelListener = new System.Windows.Forms.Label();
            this.listViewAlarmList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAlarmCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSeverity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderEnabled = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAlarmMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonAlarmListPath = new System.Windows.Forms.Button();
            this.buttonProcessPath = new System.Windows.Forms.Button();
            this.labelAlarmListValue = new System.Windows.Forms.Label();
            this.comboBoxAlarmMessageCultureValue = new System.Windows.Forms.ComboBox();
            this.labelProcessPathValue = new System.Windows.Forms.Label();
            this.labelAlarmMessageCulture = new System.Windows.Forms.Label();
            this.numericUpDownListenerValue = new System.Windows.Forms.NumericUpDown();
            this.buttonSaveConfig = new System.Windows.Forms.Button();
            this.groupBoxHostSetting.SuspendLayout();
            this.tableLayoutPanelConfiguration.SuspendLayout();
            this.groupBoxProcessSetting.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownListenerValue)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxHostSetting
            // 
            this.groupBoxHostSetting.Controls.Add(this.checkBoxSimulationMode);
            this.groupBoxHostSetting.Controls.Add(this.tableLayoutPanelConfiguration);
            this.groupBoxHostSetting.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxHostSetting.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxHostSetting.Location = new System.Drawing.Point(7, 10);
            this.groupBoxHostSetting.Margin = new System.Windows.Forms.Padding(3, 20, 3, 20);
            this.groupBoxHostSetting.Name = "groupBoxHostSetting";
            this.groupBoxHostSetting.Padding = new System.Windows.Forms.Padding(7);
            this.groupBoxHostSetting.Size = new System.Drawing.Size(610, 90);
            this.groupBoxHostSetting.TabIndex = 0;
            this.groupBoxHostSetting.TabStop = false;
            this.groupBoxHostSetting.Text = " HOST SETTING ";
            // 
            // checkBoxSimulationMode
            // 
            this.checkBoxSimulationMode.AutoSize = true;
            this.checkBoxSimulationMode.Location = new System.Drawing.Point(448, -2);
            this.checkBoxSimulationMode.Name = "checkBoxSimulationMode";
            this.checkBoxSimulationMode.Size = new System.Drawing.Size(111, 21);
            this.checkBoxSimulationMode.TabIndex = 1;
            this.checkBoxSimulationMode.Text = "SIMULATION";
            this.checkBoxSimulationMode.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelConfiguration
            // 
            this.tableLayoutPanelConfiguration.ColumnCount = 5;
            this.tableLayoutPanelConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanelConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanelConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelConfiguration.Controls.Add(this.labelLindeCode, 0, 0);
            this.tableLayoutPanelConfiguration.Controls.Add(this.labelProcessCode, 2, 0);
            this.tableLayoutPanelConfiguration.Controls.Add(this.labelEquipmentId, 4, 0);
            this.tableLayoutPanelConfiguration.Controls.Add(this.textBoxLineCodeValue, 0, 2);
            this.tableLayoutPanelConfiguration.Controls.Add(this.textBoxProcessCodeValue, 2, 2);
            this.tableLayoutPanelConfiguration.Controls.Add(this.textBoxEquipmentIdValue, 4, 2);
            this.tableLayoutPanelConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelConfiguration.Location = new System.Drawing.Point(7, 25);
            this.tableLayoutPanelConfiguration.Name = "tableLayoutPanelConfiguration";
            this.tableLayoutPanelConfiguration.RowCount = 3;
            this.tableLayoutPanelConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tableLayoutPanelConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelConfiguration.Size = new System.Drawing.Size(596, 58);
            this.tableLayoutPanelConfiguration.TabIndex = 0;
            // 
            // labelLindeCode
            // 
            this.labelLindeCode.AutoSize = true;
            this.labelLindeCode.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelLindeCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLindeCode.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLindeCode.ForeColor = System.Drawing.SystemColors.Window;
            this.labelLindeCode.Location = new System.Drawing.Point(0, 0);
            this.labelLindeCode.Margin = new System.Windows.Forms.Padding(0);
            this.labelLindeCode.Name = "labelLindeCode";
            this.labelLindeCode.Size = new System.Drawing.Size(196, 27);
            this.labelLindeCode.TabIndex = 2;
            this.labelLindeCode.Text = "LINE CODE";
            this.labelLindeCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelLindeCode.UseWaitCursor = true;
            // 
            // labelProcessCode
            // 
            this.labelProcessCode.AutoSize = true;
            this.labelProcessCode.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelProcessCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProcessCode.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProcessCode.ForeColor = System.Drawing.SystemColors.Window;
            this.labelProcessCode.Location = new System.Drawing.Point(199, 0);
            this.labelProcessCode.Margin = new System.Windows.Forms.Padding(0);
            this.labelProcessCode.Name = "labelProcessCode";
            this.labelProcessCode.Size = new System.Drawing.Size(196, 27);
            this.labelProcessCode.TabIndex = 3;
            this.labelProcessCode.Text = "PROCESS CODE";
            this.labelProcessCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelProcessCode.UseWaitCursor = true;
            // 
            // labelEquipmentId
            // 
            this.labelEquipmentId.AutoSize = true;
            this.labelEquipmentId.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelEquipmentId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEquipmentId.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEquipmentId.ForeColor = System.Drawing.SystemColors.Window;
            this.labelEquipmentId.Location = new System.Drawing.Point(398, 0);
            this.labelEquipmentId.Margin = new System.Windows.Forms.Padding(0);
            this.labelEquipmentId.Name = "labelEquipmentId";
            this.labelEquipmentId.Size = new System.Drawing.Size(198, 27);
            this.labelEquipmentId.TabIndex = 4;
            this.labelEquipmentId.Text = "EQUIPMENT ID";
            this.labelEquipmentId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelEquipmentId.UseWaitCursor = true;
            // 
            // textBoxLineCodeValue
            // 
            this.textBoxLineCodeValue.BackColor = System.Drawing.SystemColors.Info;
            this.textBoxLineCodeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLineCodeValue.Location = new System.Drawing.Point(0, 31);
            this.textBoxLineCodeValue.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxLineCodeValue.Name = "textBoxLineCodeValue";
            this.textBoxLineCodeValue.Size = new System.Drawing.Size(196, 25);
            this.textBoxLineCodeValue.TabIndex = 5;
            this.textBoxLineCodeValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxProcessCodeValue
            // 
            this.textBoxProcessCodeValue.BackColor = System.Drawing.SystemColors.Info;
            this.textBoxProcessCodeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProcessCodeValue.Location = new System.Drawing.Point(199, 31);
            this.textBoxProcessCodeValue.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxProcessCodeValue.Name = "textBoxProcessCodeValue";
            this.textBoxProcessCodeValue.Size = new System.Drawing.Size(196, 25);
            this.textBoxProcessCodeValue.TabIndex = 6;
            this.textBoxProcessCodeValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxEquipmentIdValue
            // 
            this.textBoxEquipmentIdValue.BackColor = System.Drawing.SystemColors.Info;
            this.textBoxEquipmentIdValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEquipmentIdValue.Location = new System.Drawing.Point(398, 31);
            this.textBoxEquipmentIdValue.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxEquipmentIdValue.Name = "textBoxEquipmentIdValue";
            this.textBoxEquipmentIdValue.Size = new System.Drawing.Size(198, 25);
            this.textBoxEquipmentIdValue.TabIndex = 7;
            this.textBoxEquipmentIdValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBoxProcessSetting
            // 
            this.groupBoxProcessSetting.Controls.Add(this.tableLayoutPanel2);
            this.groupBoxProcessSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxProcessSetting.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxProcessSetting.Location = new System.Drawing.Point(7, 100);
            this.groupBoxProcessSetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxProcessSetting.Name = "groupBoxProcessSetting";
            this.groupBoxProcessSetting.Padding = new System.Windows.Forms.Padding(7);
            this.groupBoxProcessSetting.Size = new System.Drawing.Size(610, 334);
            this.groupBoxProcessSetting.TabIndex = 1;
            this.groupBoxProcessSetting.TabStop = false;
            this.groupBoxProcessSetting.Text = " PROCESS INFORMATION ";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.Controls.Add(this.textBoxListenerAddressValue, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelListener, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.listViewAlarmList, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.buttonAlarmListPath, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.buttonProcessPath, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.labelAlarmListValue, 2, 6);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxAlarmMessageCultureValue, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.labelProcessPathValue, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.labelAlarmMessageCulture, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.numericUpDownListenerValue, 4, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(7, 25);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 9;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(596, 302);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // textBoxListenerAddressValue
            // 
            this.textBoxListenerAddressValue.BackColor = System.Drawing.SystemColors.Info;
            this.textBoxListenerAddressValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxListenerAddressValue.Location = new System.Drawing.Point(123, 0);
            this.textBoxListenerAddressValue.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxListenerAddressValue.Name = "textBoxListenerAddressValue";
            this.textBoxListenerAddressValue.Size = new System.Drawing.Size(370, 25);
            this.textBoxListenerAddressValue.TabIndex = 6;
            this.textBoxListenerAddressValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelListener
            // 
            this.labelListener.AutoSize = true;
            this.labelListener.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelListener.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelListener.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelListener.ForeColor = System.Drawing.SystemColors.Window;
            this.labelListener.Location = new System.Drawing.Point(0, 0);
            this.labelListener.Margin = new System.Windows.Forms.Padding(0);
            this.labelListener.Name = "labelListener";
            this.labelListener.Size = new System.Drawing.Size(120, 28);
            this.labelListener.TabIndex = 7;
            this.labelListener.Text = "EVENT LISTENER";
            this.labelListener.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelListener.UseWaitCursor = true;
            // 
            // listViewAlarmList
            // 
            this.listViewAlarmList.BackColor = System.Drawing.SystemColors.Desktop;
            this.listViewAlarmList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeaderAlarmCode,
            this.columnHeaderSeverity,
            this.columnHeaderEnabled,
            this.columnHeaderAlarmMessage});
            this.tableLayoutPanel2.SetColumnSpan(this.listViewAlarmList, 5);
            this.listViewAlarmList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewAlarmList.ForeColor = System.Drawing.SystemColors.Window;
            this.listViewAlarmList.FullRowSelect = true;
            this.listViewAlarmList.GridLines = true;
            this.listViewAlarmList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewAlarmList.HideSelection = false;
            this.listViewAlarmList.Location = new System.Drawing.Point(0, 136);
            this.listViewAlarmList.Margin = new System.Windows.Forms.Padding(0);
            this.listViewAlarmList.MultiSelect = false;
            this.listViewAlarmList.Name = "listViewAlarmList";
            this.listViewAlarmList.Size = new System.Drawing.Size(596, 166);
            this.listViewAlarmList.TabIndex = 3;
            this.listViewAlarmList.UseCompatibleStateImageBehavior = false;
            this.listViewAlarmList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "NO";
            this.columnHeader1.Width = 0;
            // 
            // columnHeaderAlarmCode
            // 
            this.columnHeaderAlarmCode.Text = "CODE";
            this.columnHeaderAlarmCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeaderAlarmCode.Width = 100;
            // 
            // columnHeaderSeverity
            // 
            this.columnHeaderSeverity.Text = "SEVERITY";
            this.columnHeaderSeverity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeaderSeverity.Width = 100;
            // 
            // columnHeaderEnabled
            // 
            this.columnHeaderEnabled.Text = "ENABLED";
            this.columnHeaderEnabled.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeaderEnabled.Width = 100;
            // 
            // columnHeaderAlarmMessage
            // 
            this.columnHeaderAlarmMessage.Text = "MESSAGE";
            this.columnHeaderAlarmMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeaderAlarmMessage.Width = 300;
            // 
            // buttonAlarmListPath
            // 
            this.buttonAlarmListPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAlarmListPath.Location = new System.Drawing.Point(0, 101);
            this.buttonAlarmListPath.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAlarmListPath.Name = "buttonAlarmListPath";
            this.buttonAlarmListPath.Size = new System.Drawing.Size(120, 32);
            this.buttonAlarmListPath.TabIndex = 1;
            this.buttonAlarmListPath.Text = "ALARM LIST";
            this.buttonAlarmListPath.UseVisualStyleBackColor = true;
            this.buttonAlarmListPath.Click += new System.EventHandler(this.OnClickButtonAlarmListPath);
            // 
            // buttonProcessPath
            // 
            this.buttonProcessPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonProcessPath.Location = new System.Drawing.Point(0, 66);
            this.buttonProcessPath.Margin = new System.Windows.Forms.Padding(0);
            this.buttonProcessPath.Name = "buttonProcessPath";
            this.buttonProcessPath.Size = new System.Drawing.Size(120, 32);
            this.buttonProcessPath.TabIndex = 1;
            this.buttonProcessPath.Text = "PATH";
            this.buttonProcessPath.UseVisualStyleBackColor = true;
            this.buttonProcessPath.Click += new System.EventHandler(this.OnClickButtonProcessPath);
            // 
            // labelAlarmListValue
            // 
            this.labelAlarmListValue.AutoSize = true;
            this.labelAlarmListValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.tableLayoutPanel2.SetColumnSpan(this.labelAlarmListValue, 3);
            this.labelAlarmListValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAlarmListValue.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAlarmListValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelAlarmListValue.Location = new System.Drawing.Point(123, 101);
            this.labelAlarmListValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelAlarmListValue.Name = "labelAlarmListValue";
            this.labelAlarmListValue.Size = new System.Drawing.Size(473, 32);
            this.labelAlarmListValue.TabIndex = 0;
            this.labelAlarmListValue.Text = "...";
            this.labelAlarmListValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelAlarmListValue.UseWaitCursor = true;
            // 
            // comboBoxAlarmMessageCultureValue
            // 
            this.comboBoxAlarmMessageCultureValue.BackColor = System.Drawing.SystemColors.Info;
            this.tableLayoutPanel2.SetColumnSpan(this.comboBoxAlarmMessageCultureValue, 3);
            this.comboBoxAlarmMessageCultureValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxAlarmMessageCultureValue.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxAlarmMessageCultureValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAlarmMessageCultureValue.FormattingEnabled = true;
            this.comboBoxAlarmMessageCultureValue.Items.AddRange(new object[] {
            "English (en-US)",
            "Korean (ko-KR)"});
            this.comboBoxAlarmMessageCultureValue.Location = new System.Drawing.Point(123, 34);
            this.comboBoxAlarmMessageCultureValue.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.comboBoxAlarmMessageCultureValue.Name = "comboBoxAlarmMessageCultureValue";
            this.comboBoxAlarmMessageCultureValue.Size = new System.Drawing.Size(473, 26);
            this.comboBoxAlarmMessageCultureValue.TabIndex = 7;
            this.comboBoxAlarmMessageCultureValue.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnComboBoxDrawItem);
            // 
            // labelProcessPathValue
            // 
            this.labelProcessPathValue.AutoSize = true;
            this.labelProcessPathValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.tableLayoutPanel2.SetColumnSpan(this.labelProcessPathValue, 3);
            this.labelProcessPathValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProcessPathValue.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProcessPathValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelProcessPathValue.Location = new System.Drawing.Point(123, 66);
            this.labelProcessPathValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelProcessPathValue.Name = "labelProcessPathValue";
            this.labelProcessPathValue.Size = new System.Drawing.Size(473, 32);
            this.labelProcessPathValue.TabIndex = 1;
            this.labelProcessPathValue.Text = "...";
            this.labelProcessPathValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelProcessPathValue.UseWaitCursor = true;
            // 
            // labelAlarmMessageCulture
            // 
            this.labelAlarmMessageCulture.AutoSize = true;
            this.labelAlarmMessageCulture.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelAlarmMessageCulture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAlarmMessageCulture.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAlarmMessageCulture.ForeColor = System.Drawing.SystemColors.Window;
            this.labelAlarmMessageCulture.Location = new System.Drawing.Point(0, 31);
            this.labelAlarmMessageCulture.Margin = new System.Windows.Forms.Padding(0);
            this.labelAlarmMessageCulture.Name = "labelAlarmMessageCulture";
            this.labelAlarmMessageCulture.Size = new System.Drawing.Size(120, 32);
            this.labelAlarmMessageCulture.TabIndex = 6;
            this.labelAlarmMessageCulture.Text = "LANGUAGE";
            this.labelAlarmMessageCulture.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelAlarmMessageCulture.UseWaitCursor = true;
            // 
            // numericUpDownListenerValue
            // 
            this.numericUpDownListenerValue.BackColor = System.Drawing.SystemColors.Info;
            this.numericUpDownListenerValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownListenerValue.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownListenerValue.Location = new System.Drawing.Point(496, 0);
            this.numericUpDownListenerValue.Margin = new System.Windows.Forms.Padding(0);
            this.numericUpDownListenerValue.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownListenerValue.Name = "numericUpDownListenerValue";
            this.numericUpDownListenerValue.Size = new System.Drawing.Size(100, 25);
            this.numericUpDownListenerValue.TabIndex = 8;
            this.numericUpDownListenerValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonSaveConfig
            // 
            this.buttonSaveConfig.BackgroundImage = global::ProcessWatcher.Properties.Resources.icon_save_2;
            this.buttonSaveConfig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonSaveConfig.Location = new System.Drawing.Point(570, 4);
            this.buttonSaveConfig.Name = "buttonSaveConfig";
            this.buttonSaveConfig.Size = new System.Drawing.Size(39, 28);
            this.buttonSaveConfig.TabIndex = 1;
            this.buttonSaveConfig.UseVisualStyleBackColor = true;
            this.buttonSaveConfig.Click += new System.EventHandler(this.OnClickButtonSaveConfig);
            // 
            // FormConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.buttonSaveConfig);
            this.Controls.Add(this.groupBoxProcessSetting);
            this.Controls.Add(this.groupBoxHostSetting);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1280, 1024);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "FormConfig";
            this.Padding = new System.Windows.Forms.Padding(7, 10, 7, 7);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuration";
            this.Activated += new System.EventHandler(this.OnFormActivated);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.Shown += new System.EventHandler(this.OnFormShown);
            this.groupBoxHostSetting.ResumeLayout(false);
            this.groupBoxHostSetting.PerformLayout();
            this.tableLayoutPanelConfiguration.ResumeLayout(false);
            this.tableLayoutPanelConfiguration.PerformLayout();
            this.groupBoxProcessSetting.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownListenerValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        protected System.Windows.Forms.TableLayoutPanel tableLayoutPanelConfiguration;
        protected System.Windows.Forms.GroupBox groupBoxProcessSetting;
        protected System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        protected System.Windows.Forms.Button buttonAlarmListPath;
        protected System.Windows.Forms.Label labelProcessPathValue;
        protected System.Windows.Forms.Button buttonProcessPath;
        protected System.Windows.Forms.Label labelLindeCode;
        protected System.Windows.Forms.Label labelProcessCode;
        protected System.Windows.Forms.Label labelEquipmentId;
        protected System.Windows.Forms.TextBox textBoxLineCodeValue;
        protected System.Windows.Forms.TextBox textBoxProcessCodeValue;
        protected System.Windows.Forms.TextBox textBoxEquipmentIdValue;
        protected System.Windows.Forms.GroupBox groupBoxHostSetting;
        protected System.Windows.Forms.Label labelAlarmListValue;
        protected System.Windows.Forms.ListView listViewAlarmList;
        protected System.Windows.Forms.ColumnHeader columnHeader1;
        protected System.Windows.Forms.ColumnHeader columnHeaderAlarmCode;
        protected System.Windows.Forms.ColumnHeader columnHeaderAlarmMessage;
        protected System.Windows.Forms.ComboBox comboBoxAlarmMessageCultureValue;
        private System.Windows.Forms.ColumnHeader columnHeaderSeverity;
        private System.Windows.Forms.ColumnHeader columnHeaderEnabled;
        private System.Windows.Forms.Button buttonSaveConfig;
        protected System.Windows.Forms.TextBox textBoxListenerAddressValue;
        protected System.Windows.Forms.Label labelListener;
        protected System.Windows.Forms.Label labelAlarmMessageCulture;
        protected System.Windows.Forms.NumericUpDown numericUpDownListenerValue;
        private System.Windows.Forms.CheckBox checkBoxSimulationMode;
    }
}