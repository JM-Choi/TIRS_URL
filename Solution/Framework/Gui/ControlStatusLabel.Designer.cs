namespace TechFloor.Gui
{
    partial class ControlStatusLabel
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelValue = new System.Windows.Forms.Label();
            this.tableLayoutPanelFrame = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelFrame.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelStatus
            // 
            this.labelStatus.BackColor = System.Drawing.SystemColors.Highlight;
            this.labelStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStatus.ForeColor = System.Drawing.SystemColors.Window;
            this.labelStatus.Location = new System.Drawing.Point(0, 0);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(120, 56);
            this.labelStatus.TabIndex = 225;
            this.labelStatus.Text = "REEL SIZE CHECK";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelValue
            // 
            this.labelValue.BackColor = System.Drawing.SystemColors.Desktop;
            this.labelValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelValue.ForeColor = System.Drawing.SystemColors.Window;
            this.labelValue.Location = new System.Drawing.Point(0, 56);
            this.labelValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(120, 24);
            this.labelValue.TabIndex = 226;
            this.labelValue.Text = "REEL 13";
            this.labelValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanelFrame
            // 
            this.tableLayoutPanelFrame.ColumnCount = 1;
            this.tableLayoutPanelFrame.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelFrame.Controls.Add(this.labelStatus, 0, 0);
            this.tableLayoutPanelFrame.Controls.Add(this.labelValue, 0, 1);
            this.tableLayoutPanelFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelFrame.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelFrame.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanelFrame.Name = "tableLayoutPanelFrame";
            this.tableLayoutPanelFrame.RowCount = 2;
            this.tableLayoutPanelFrame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanelFrame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelFrame.Size = new System.Drawing.Size(120, 80);
            this.tableLayoutPanelFrame.TabIndex = 227;
            // 
            // ControlStatusLabel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanelFrame);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ControlStatusLabel";
            this.Size = new System.Drawing.Size(120, 80);
            this.tableLayoutPanelFrame.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelValue;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelFrame;
    }
}
