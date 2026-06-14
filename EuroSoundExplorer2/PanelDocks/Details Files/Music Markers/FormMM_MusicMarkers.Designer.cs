namespace sb_explorer
{
    partial class FormMM_MusicMarkers
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; false otherwise.</param>
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
        /// Required method for Designer support.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lblMusicHeaders = new System.Windows.Forms.ToolStripLabel();
            this.txtMusicHeadersCount = new System.Windows.Forms.ToolStripTextBox();
            this.lblMarkerCounts = new System.Windows.Forms.ToolStripLabel();
            this.txtMarkerCountsCount = new System.Windows.Forms.ToolStripTextBox();
            this.lblMarkerLists = new System.Windows.Forms.ToolStripLabel();
            this.txtMarkerListsCount = new System.Windows.Forms.ToolStripTextBox();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.grpMusicHeaders = new System.Windows.Forms.GroupBox();
            this.lvwMusicHeaders = new sb_explorer.CustomControls.ListView_ColumnSortingClick();
            this.colMusicHashCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMusicLabel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStreamDataOffset = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colBaseVolume = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHeaderPadding = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainerBottom = new System.Windows.Forms.SplitContainer();
            this.grpMarkerCounts = new System.Windows.Forms.GroupBox();
            this.lvwMarkerCounts = new sb_explorer.CustomControls.ListView_ColumnSortingClick();
            this.colStartMarkerCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMarkerCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCountsPadding0 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCountsPadding1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.grpMarkerLists = new System.Windows.Forms.GroupBox();
            this.lvwMarkerLists = new sb_explorer.CustomControls.ListView_ColumnSortingClick();
            this.colPosition = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLoopStart = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colListPadding0 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colListPadding1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.grpMusicHeaders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerBottom)).BeginInit();
            this.splitContainerBottom.Panel1.SuspendLayout();
            this.splitContainerBottom.Panel2.SuspendLayout();
            this.splitContainerBottom.SuspendLayout();
            this.grpMarkerCounts.SuspendLayout();
            this.grpMarkerLists.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblMusicHeaders,
            this.txtMusicHeadersCount,
            this.lblMarkerCounts,
            this.txtMarkerCountsCount,
            this.lblMarkerLists,
            this.txtMarkerListsCount});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(784, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lblMusicHeaders
            // 
            this.lblMusicHeaders.Name = "lblMusicHeaders";
            this.lblMusicHeaders.Size = new System.Drawing.Size(88, 22);
            this.lblMusicHeaders.Text = "Music Headers";
            // 
            // txtMusicHeadersCount
            // 
            this.txtMusicHeadersCount.BackColor = System.Drawing.SystemColors.Window;
            this.txtMusicHeadersCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMusicHeadersCount.Name = "txtMusicHeadersCount";
            this.txtMusicHeadersCount.ReadOnly = true;
            this.txtMusicHeadersCount.Size = new System.Drawing.Size(45, 25);
            this.txtMusicHeadersCount.Text = "0";
            // 
            // lblMarkerCounts
            // 
            this.lblMarkerCounts.Name = "lblMarkerCounts";
            this.lblMarkerCounts.Size = new System.Drawing.Size(84, 22);
            this.lblMarkerCounts.Text = "Marker Counts";
            // 
            // txtMarkerCountsCount
            // 
            this.txtMarkerCountsCount.BackColor = System.Drawing.SystemColors.Window;
            this.txtMarkerCountsCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMarkerCountsCount.Name = "txtMarkerCountsCount";
            this.txtMarkerCountsCount.ReadOnly = true;
            this.txtMarkerCountsCount.Size = new System.Drawing.Size(45, 25);
            this.txtMarkerCountsCount.Text = "0";
            // 
            // lblMarkerLists
            // 
            this.lblMarkerLists.Name = "lblMarkerLists";
            this.lblMarkerLists.Size = new System.Drawing.Size(69, 22);
            this.lblMarkerLists.Text = "Marker Lists";
            // 
            // txtMarkerListsCount
            // 
            this.txtMarkerListsCount.BackColor = System.Drawing.SystemColors.Window;
            this.txtMarkerListsCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMarkerListsCount.Name = "txtMarkerListsCount";
            this.txtMarkerListsCount.ReadOnly = true;
            this.txtMarkerListsCount.Size = new System.Drawing.Size(45, 25);
            this.txtMarkerListsCount.Text = "0";
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerMain.Location = new System.Drawing.Point(0, 28);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.grpMusicHeaders);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerBottom);
            this.splitContainerMain.Size = new System.Drawing.Size(784, 433);
            this.splitContainerMain.SplitterDistance = 171;
            this.splitContainerMain.TabIndex = 1;
            // 
            // grpMusicHeaders
            // 
            this.grpMusicHeaders.Controls.Add(this.lvwMusicHeaders);
            this.grpMusicHeaders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpMusicHeaders.Location = new System.Drawing.Point(0, 0);
            this.grpMusicHeaders.Name = "grpMusicHeaders";
            this.grpMusicHeaders.Size = new System.Drawing.Size(784, 171);
            this.grpMusicHeaders.TabIndex = 0;
            this.grpMusicHeaders.TabStop = false;
            this.grpMusicHeaders.Text = "Music Headers";
            // 
            // lvwMusicHeaders
            // 
            this.lvwMusicHeaders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colMusicHashCode,
            this.colMusicLabel,
            this.colStreamDataOffset,
            this.colBaseVolume,
            this.colHeaderPadding});
            this.lvwMusicHeaders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwMusicHeaders.FullRowSelect = true;
            this.lvwMusicHeaders.GridLines = true;
            this.lvwMusicHeaders.HideSelection = false;
            this.lvwMusicHeaders.Location = new System.Drawing.Point(3, 16);
            this.lvwMusicHeaders.Name = "lvwMusicHeaders";
            this.lvwMusicHeaders.Size = new System.Drawing.Size(778, 152);
            this.lvwMusicHeaders.TabIndex = 0;
            this.lvwMusicHeaders.UseCompatibleStateImageBehavior = false;
            this.lvwMusicHeaders.View = System.Windows.Forms.View.Details;
            // 
            // colMusicHashCode
            // 
            this.colMusicHashCode.Text = "Music Hashcode";
            this.colMusicHashCode.Width = 105;
            // 
            // colMusicLabel
            // 
            this.colMusicLabel.Text = "Label";
            this.colMusicLabel.Width = 180;
            // 
            // colStreamDataOffset
            // 
            this.colStreamDataOffset.Text = "Stream Data Offset";
            this.colStreamDataOffset.Width = 120;
            // 
            // colBaseVolume
            // 
            this.colBaseVolume.Text = "Base Volume";
            this.colBaseVolume.Width = 90;
            // 
            // colHeaderPadding
            // 
            this.colHeaderPadding.Text = "Padding";
            this.colHeaderPadding.Width = 80;
            // 
            // splitContainerBottom
            // 
            this.splitContainerBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerBottom.Location = new System.Drawing.Point(0, 0);
            this.splitContainerBottom.Name = "splitContainerBottom";
            // 
            // splitContainerBottom.Panel1
            // 
            this.splitContainerBottom.Panel1.Controls.Add(this.grpMarkerCounts);
            // 
            // splitContainerBottom.Panel2
            // 
            this.splitContainerBottom.Panel2.Controls.Add(this.grpMarkerLists);
            this.splitContainerBottom.Size = new System.Drawing.Size(784, 258);
            this.splitContainerBottom.SplitterDistance = 381;
            this.splitContainerBottom.TabIndex = 0;
            // 
            // grpMarkerCounts
            // 
            this.grpMarkerCounts.Controls.Add(this.lvwMarkerCounts);
            this.grpMarkerCounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpMarkerCounts.Location = new System.Drawing.Point(0, 0);
            this.grpMarkerCounts.Name = "grpMarkerCounts";
            this.grpMarkerCounts.Size = new System.Drawing.Size(381, 258);
            this.grpMarkerCounts.TabIndex = 0;
            this.grpMarkerCounts.TabStop = false;
            this.grpMarkerCounts.Text = "Marker Counts";
            // 
            // lvwMarkerCounts
            // 
            this.lvwMarkerCounts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colStartMarkerCount,
            this.colMarkerCount,
            this.colCountsPadding0,
            this.colCountsPadding1});
            this.lvwMarkerCounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwMarkerCounts.FullRowSelect = true;
            this.lvwMarkerCounts.GridLines = true;
            this.lvwMarkerCounts.HideSelection = false;
            this.lvwMarkerCounts.Location = new System.Drawing.Point(3, 16);
            this.lvwMarkerCounts.Name = "lvwMarkerCounts";
            this.lvwMarkerCounts.Size = new System.Drawing.Size(375, 239);
            this.lvwMarkerCounts.TabIndex = 0;
            this.lvwMarkerCounts.UseCompatibleStateImageBehavior = false;
            this.lvwMarkerCounts.View = System.Windows.Forms.View.Details;
            // 
            // colStartMarkerCount
            // 
            this.colStartMarkerCount.Text = "Start Markers";
            this.colStartMarkerCount.Width = 90;
            // 
            // colMarkerCount
            // 
            this.colMarkerCount.Text = "Markers";
            this.colMarkerCount.Width = 80;
            // 
            // colCountsPadding0
            // 
            this.colCountsPadding0.Text = "Padding 0";
            this.colCountsPadding0.Width = 80;
            // 
            // colCountsPadding1
            // 
            this.colCountsPadding1.Text = "Padding 1";
            this.colCountsPadding1.Width = 80;
            // 
            // grpMarkerLists
            // 
            this.grpMarkerLists.Controls.Add(this.lvwMarkerLists);
            this.grpMarkerLists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpMarkerLists.Location = new System.Drawing.Point(0, 0);
            this.grpMarkerLists.Name = "grpMarkerLists";
            this.grpMarkerLists.Size = new System.Drawing.Size(399, 258);
            this.grpMarkerLists.TabIndex = 0;
            this.grpMarkerLists.TabStop = false;
            this.grpMarkerLists.Text = "Marker Lists";
            // 
            // lvwMarkerLists
            // 
            this.lvwMarkerLists.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colPosition,
            this.colLoopStart,
            this.colListPadding0,
            this.colListPadding1});
            this.lvwMarkerLists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwMarkerLists.FullRowSelect = true;
            this.lvwMarkerLists.GridLines = true;
            this.lvwMarkerLists.HideSelection = false;
            this.lvwMarkerLists.Location = new System.Drawing.Point(3, 16);
            this.lvwMarkerLists.Name = "lvwMarkerLists";
            this.lvwMarkerLists.Size = new System.Drawing.Size(393, 239);
            this.lvwMarkerLists.TabIndex = 0;
            this.lvwMarkerLists.UseCompatibleStateImageBehavior = false;
            this.lvwMarkerLists.View = System.Windows.Forms.View.Details;
            // 
            // colPosition
            // 
            this.colPosition.Text = "Position";
            this.colPosition.Width = 90;
            // 
            // colLoopStart
            // 
            this.colLoopStart.Text = "Loop Start";
            this.colLoopStart.Width = 90;
            // 
            // colListPadding0
            // 
            this.colListPadding0.Text = "Padding 0";
            this.colListPadding0.Width = 80;
            // 
            // colListPadding1
            // 
            this.colListPadding1.Text = "Padding 1";
            this.colListPadding1.Width = 80;
            // 
            // FormMM_MusicMarkers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.splitContainerMain);
            this.HideOnClose = true;
            this.Name = "FormMM_MusicMarkers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TabText = "Music Markers File";
            this.Text = "Music Markers File";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.grpMusicHeaders.ResumeLayout(false);
            this.splitContainerBottom.Panel1.ResumeLayout(false);
            this.splitContainerBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerBottom)).EndInit();
            this.splitContainerBottom.ResumeLayout(false);
            this.grpMarkerCounts.ResumeLayout(false);
            this.grpMarkerLists.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel lblMusicHeaders;
        private System.Windows.Forms.ToolStripTextBox txtMusicHeadersCount;
        private System.Windows.Forms.ToolStripLabel lblMarkerCounts;
        private System.Windows.Forms.ToolStripTextBox txtMarkerCountsCount;
        private System.Windows.Forms.ToolStripLabel lblMarkerLists;
        private System.Windows.Forms.ToolStripTextBox txtMarkerListsCount;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.GroupBox grpMusicHeaders;
        protected internal CustomControls.ListView_ColumnSortingClick lvwMusicHeaders;
        private System.Windows.Forms.ColumnHeader colMusicHashCode;
        private System.Windows.Forms.ColumnHeader colMusicLabel;
        private System.Windows.Forms.ColumnHeader colStreamDataOffset;
        private System.Windows.Forms.ColumnHeader colBaseVolume;
        private System.Windows.Forms.ColumnHeader colHeaderPadding;
        private System.Windows.Forms.SplitContainer splitContainerBottom;
        private System.Windows.Forms.GroupBox grpMarkerCounts;
        protected internal CustomControls.ListView_ColumnSortingClick lvwMarkerCounts;
        private System.Windows.Forms.ColumnHeader colStartMarkerCount;
        private System.Windows.Forms.ColumnHeader colMarkerCount;
        private System.Windows.Forms.ColumnHeader colCountsPadding0;
        private System.Windows.Forms.ColumnHeader colCountsPadding1;
        private System.Windows.Forms.GroupBox grpMarkerLists;
        protected internal CustomControls.ListView_ColumnSortingClick lvwMarkerLists;
        private System.Windows.Forms.ColumnHeader colPosition;
        private System.Windows.Forms.ColumnHeader colLoopStart;
        private System.Windows.Forms.ColumnHeader colListPadding0;
        private System.Windows.Forms.ColumnHeader colListPadding1;
    }
}
