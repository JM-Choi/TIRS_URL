namespace TechFloor.Forms
{
    partial class FormLight
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
            this.groupBoxVisionLight = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelVisionLightChannel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelVisionLightChannel1 = new System.Windows.Forms.Label();
            this.buttonLightOffChannel1 = new System.Windows.Forms.Button();
            this.buttonLightOnChannel1 = new System.Windows.Forms.Button();
            this.trackBarVisionLightChannel1 = new System.Windows.Forms.TrackBar();
            this.numericUpDownVisionLightChannel1 = new System.Windows.Forms.NumericUpDown();
            this.buttonSaveLightSetting = new System.Windows.Forms.Button();
            this.comboBoxVisionLightChannel1 = new System.Windows.Forms.ComboBox();
            this.groupBoxVisionLight.SuspendLayout();
            this.tableLayoutPanelVisionLightChannel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVisionLightChannel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVisionLightChannel1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxVisionLight
            // 
            this.groupBoxVisionLight.Controls.Add(this.tableLayoutPanelVisionLightChannel1);
            this.groupBoxVisionLight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.groupBoxVisionLight.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxVisionLight.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxVisionLight.Location = new System.Drawing.Point(8, 8);
            this.groupBoxVisionLight.Name = "groupBoxVisionLight";
            this.groupBoxVisionLight.Padding = new System.Windows.Forms.Padding(8);
            this.groupBoxVisionLight.Size = new System.Drawing.Size(448, 206);
            this.groupBoxVisionLight.TabIndex = 0;
            this.groupBoxVisionLight.TabStop = false;
            this.groupBoxVisionLight.Text = " CATEGORY ";
            // 
            // tableLayoutPanelVisionLightChannel1
            // 
            this.tableLayoutPanelVisionLightChannel1.ColumnCount = 2;
            this.tableLayoutPanelVisionLightChannel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelVisionLightChannel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelVisionLightChannel1.Controls.Add(this.labelVisionLightChannel1, 0, 0);
            this.tableLayoutPanelVisionLightChannel1.Controls.Add(this.buttonLightOffChannel1, 1, 3);
            this.tableLayoutPanelVisionLightChannel1.Controls.Add(this.buttonLightOnChannel1, 0, 3);
            this.tableLayoutPanelVisionLightChannel1.Controls.Add(this.trackBarVisionLightChannel1, 0, 1);
            this.tableLayoutPanelVisionLightChannel1.Controls.Add(this.numericUpDownVisionLightChannel1, 0, 2);
            this.tableLayoutPanelVisionLightChannel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelVisionLightChannel1.Location = new System.Drawing.Point(8, 27);
            this.tableLayoutPanelVisionLightChannel1.Name = "tableLayoutPanelVisionLightChannel1";
            this.tableLayoutPanelVisionLightChannel1.RowCount = 4;
            this.tableLayoutPanelVisionLightChannel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanelVisionLightChannel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelVisionLightChannel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelVisionLightChannel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanelVisionLightChannel1.Size = new System.Drawing.Size(432, 171);
            this.tableLayoutPanelVisionLightChannel1.TabIndex = 1;
            // 
            // labelVisionLightChannel1
            // 
            this.labelVisionLightChannel1.BackColor = System.Drawing.SystemColors.HotTrack;
            this.tableLayoutPanelVisionLightChannel1.SetColumnSpan(this.labelVisionLightChannel1, 2);
            this.labelVisionLightChannel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelVisionLightChannel1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVisionLightChannel1.ForeColor = System.Drawing.SystemColors.Window;
            this.labelVisionLightChannel1.Location = new System.Drawing.Point(3, 3);
            this.labelVisionLightChannel1.Margin = new System.Windows.Forms.Padding(3);
            this.labelVisionLightChannel1.Name = "labelVisionLightChannel1";
            this.labelVisionLightChannel1.Size = new System.Drawing.Size(426, 40);
            this.labelVisionLightChannel1.TabIndex = 221;
            this.labelVisionLightChannel1.Text = "CHANNEL 1";
            this.labelVisionLightChannel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonLightOffChannel1
            // 
            this.buttonLightOffChannel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLightOffChannel1.Location = new System.Drawing.Point(219, 129);
            this.buttonLightOffChannel1.Name = "buttonLightOffChannel1";
            this.buttonLightOffChannel1.Size = new System.Drawing.Size(210, 40);
            this.buttonLightOffChannel1.TabIndex = 219;
            this.buttonLightOffChannel1.Text = "LIGHT OFF";
            this.buttonLightOffChannel1.UseVisualStyleBackColor = true;
            this.buttonLightOffChannel1.Click += new System.EventHandler(this.OnClickVisionLightOffChannel1);
            // 
            // buttonLightOnChannel1
            // 
            this.buttonLightOnChannel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLightOnChannel1.Location = new System.Drawing.Point(3, 129);
            this.buttonLightOnChannel1.Name = "buttonLightOnChannel1";
            this.buttonLightOnChannel1.Size = new System.Drawing.Size(210, 40);
            this.buttonLightOnChannel1.TabIndex = 220;
            this.buttonLightOnChannel1.Text = "LIGHT ON";
            this.buttonLightOnChannel1.UseVisualStyleBackColor = true;
            this.buttonLightOnChannel1.Click += new System.EventHandler(this.OnClickButtonVisionLightOnChannel1);
            // 
            // trackBarVisionLightChannel1
            // 
            this.tableLayoutPanelVisionLightChannel1.SetColumnSpan(this.trackBarVisionLightChannel1, 2);
            this.trackBarVisionLightChannel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarVisionLightChannel1.Location = new System.Drawing.Point(3, 49);
            this.trackBarVisionLightChannel1.Maximum = 255;
            this.trackBarVisionLightChannel1.Name = "trackBarVisionLightChannel1";
            this.trackBarVisionLightChannel1.Size = new System.Drawing.Size(426, 34);
            this.trackBarVisionLightChannel1.TabIndex = 222;
            this.trackBarVisionLightChannel1.TickFrequency = 50;
            this.trackBarVisionLightChannel1.ValueChanged += new System.EventHandler(this.OnValueChangedTrackBarVisionLightChannel1);
            // 
            // numericUpDownVisionLightChannel1
            // 
            this.tableLayoutPanelVisionLightChannel1.SetColumnSpan(this.numericUpDownVisionLightChannel1, 2);
            this.numericUpDownVisionLightChannel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownVisionLightChannel1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownVisionLightChannel1.Location = new System.Drawing.Point(3, 89);
            this.numericUpDownVisionLightChannel1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownVisionLightChannel1.Name = "numericUpDownVisionLightChannel1";
            this.numericUpDownVisionLightChannel1.Size = new System.Drawing.Size(426, 32);
            this.numericUpDownVisionLightChannel1.TabIndex = 223;
            this.numericUpDownVisionLightChannel1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownVisionLightChannel1.ValueChanged += new System.EventHandler(this.OnValueChangedNumericUpDownVisionLightChannel1);
            // 
            // buttonSaveLightSetting
            // 
            this.buttonSaveLightSetting.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSaveLightSetting.Location = new System.Drawing.Point(235, 220);
            this.buttonSaveLightSetting.Name = "buttonSaveLightSetting";
            this.buttonSaveLightSetting.Size = new System.Drawing.Size(210, 40);
            this.buttonSaveLightSetting.TabIndex = 7;
            this.buttonSaveLightSetting.Text = "SAVE";
            this.buttonSaveLightSetting.UseVisualStyleBackColor = true;
            this.buttonSaveLightSetting.Click += new System.EventHandler(this.OnClickSave);
            // 
            // comboBoxVisionLightChannel1
            // 
            this.comboBoxVisionLightChannel1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.comboBoxVisionLightChannel1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVisionLightChannel1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxVisionLightChannel1.FormattingEnabled = true;
            this.comboBoxVisionLightChannel1.Location = new System.Drawing.Point(123, 4);
            this.comboBoxVisionLightChannel1.Name = "comboBoxVisionLightChannel1";
            this.comboBoxVisionLightChannel1.Size = new System.Drawing.Size(322, 27);
            this.comboBoxVisionLightChannel1.TabIndex = 8;
            this.comboBoxVisionLightChannel1.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChangedLight);
            // 
            // FormLight
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(464, 270);
            this.Controls.Add(this.comboBoxVisionLightChannel1);
            this.Controls.Add(this.buttonSaveLightSetting);
            this.Controls.Add(this.groupBoxVisionLight);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLight";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vision Light Control";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.Shown += new System.EventHandler(this.OnFormShown);
            this.groupBoxVisionLight.ResumeLayout(false);
            this.tableLayoutPanelVisionLightChannel1.ResumeLayout(false);
            this.tableLayoutPanelVisionLightChannel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVisionLightChannel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVisionLightChannel1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxVisionLight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelVisionLightChannel1;
        public System.Windows.Forms.Label labelVisionLightChannel1;
        private System.Windows.Forms.Button buttonLightOffChannel1;
        private System.Windows.Forms.Button buttonLightOnChannel1;
        private System.Windows.Forms.TrackBar trackBarVisionLightChannel1;
        private System.Windows.Forms.NumericUpDown numericUpDownVisionLightChannel1;
        private System.Windows.Forms.Button buttonSaveLightSetting;
        private System.Windows.Forms.ComboBox comboBoxVisionLightChannel1;
    }
}