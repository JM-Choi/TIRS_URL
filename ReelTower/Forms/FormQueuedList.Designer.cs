namespace Marcus.Solution.TechFloor.Forms
{
    partial class FormQueuedList
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
            this.groupBoxPendingList = new System.Windows.Forms.GroupBox();
            this.listViewQueuedPickingList = new System.Windows.Forms.ListView();
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnReels = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnRegistered = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonRemoveAll = new System.Windows.Forms.Button();
            this.groupBoxPendingList.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxPendingList
            // 
            this.groupBoxPendingList.Controls.Add(this.listViewQueuedPickingList);
            this.groupBoxPendingList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.groupBoxPendingList.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxPendingList.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxPendingList.Location = new System.Drawing.Point(8, 8);
            this.groupBoxPendingList.Name = "groupBoxPendingList";
            this.groupBoxPendingList.Padding = new System.Windows.Forms.Padding(8);
            this.groupBoxPendingList.Size = new System.Drawing.Size(446, 516);
            this.groupBoxPendingList.TabIndex = 0;
            this.groupBoxPendingList.TabStop = false;
            this.groupBoxPendingList.Text = " PENDING  ";
            // 
            // listViewQueuedPickingList
            // 
            this.listViewQueuedPickingList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnReels,
            this.columnRegistered});
            this.listViewQueuedPickingList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewQueuedPickingList.Location = new System.Drawing.Point(8, 27);
            this.listViewQueuedPickingList.Name = "listViewQueuedPickingList";
            this.listViewQueuedPickingList.Size = new System.Drawing.Size(430, 481);
            this.listViewQueuedPickingList.TabIndex = 0;
            this.listViewQueuedPickingList.UseCompatibleStateImageBehavior = false;
            this.listViewQueuedPickingList.View = System.Windows.Forms.View.Details;
            // 
            // columnName
            // 
            this.columnName.Text = "NAME";
            this.columnName.Width = 180;
            // 
            // columnReels
            // 
            this.columnReels.Text = "REELS";
            this.columnReels.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnReels.Width = 80;
            // 
            // columnRegistered
            // 
            this.columnRegistered.Text = "DATE TIME";
            this.columnRegistered.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnRegistered.Width = 166;
            // 
            // buttonRemoveAll
            // 
            this.buttonRemoveAll.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRemoveAll.Location = new System.Drawing.Point(244, 530);
            this.buttonRemoveAll.Name = "buttonRemoveAll";
            this.buttonRemoveAll.Size = new System.Drawing.Size(210, 40);
            this.buttonRemoveAll.TabIndex = 7;
            this.buttonRemoveAll.Text = "REMOVE ALL";
            this.buttonRemoveAll.UseVisualStyleBackColor = true;
            this.buttonRemoveAll.Click += new System.EventHandler(this.OnClickRemoveAll);
            // 
            // FormQueuedList
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(462, 581);
            this.Controls.Add(this.buttonRemoveAll);
            this.Controls.Add(this.groupBoxPendingList);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormQueuedList";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Queued picking list";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.Shown += new System.EventHandler(this.OnFormShown);
            this.groupBoxPendingList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxPendingList;
        private System.Windows.Forms.ListView listViewQueuedPickingList;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnReels;
        private System.Windows.Forms.ColumnHeader columnRegistered;
        private System.Windows.Forms.Button buttonRemoveAll;
    }
}