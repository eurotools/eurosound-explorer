
namespace EuroSoundExplorer2
{
    partial class FormMarkers
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
            this.lvwMarkers = new EuroSoundExplorer2.CustomControls.ListView_ColumnSortingClick();
            this.colNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStartMarkersIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPosition = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMarkerType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLoopStart = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLoopMarkerIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtMarkerCount = new System.Windows.Forms.ToolStripTextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvwMarkers
            // 
            this.lvwMarkers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwMarkers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colNo,
            this.colStartMarkersIndex,
            this.colPosition,
            this.colMarkerType,
            this.colLoopStart,
            this.colLoopMarkerIndex});
            this.lvwMarkers.FullRowSelect = true;
            this.lvwMarkers.GridLines = true;
            this.lvwMarkers.HideSelection = false;
            this.lvwMarkers.Location = new System.Drawing.Point(0, 26);
            this.lvwMarkers.Name = "lvwMarkers";
            this.lvwMarkers.Size = new System.Drawing.Size(613, 458);
            this.lvwMarkers.TabIndex = 0;
            this.lvwMarkers.UseCompatibleStateImageBehavior = false;
            this.lvwMarkers.View = System.Windows.Forms.View.Details;
            // 
            // colNo
            // 
            this.colNo.Text = "No.";
            this.colNo.Width = 30;
            // 
            // colStartMarkersIndex
            // 
            this.colStartMarkersIndex.Text = "Start Marker Index";
            this.colStartMarkersIndex.Width = 103;
            // 
            // colPosition
            // 
            this.colPosition.Text = "Position";
            // 
            // colMarkerType
            // 
            this.colMarkerType.Text = "Type";
            // 
            // colLoopStart
            // 
            this.colLoopStart.Text = "Loop Start";
            this.colLoopStart.Width = 80;
            // 
            // colLoopMarkerIndex
            // 
            this.colLoopMarkerIndex.Text = "Loop Marker Index";
            this.colLoopMarkerIndex.Width = 120;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.txtMarkerCount});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(613, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(43, 22);
            this.toolStripLabel1.Text = "Count:";
            // 
            // txtMarkerCount
            // 
            this.txtMarkerCount.BackColor = System.Drawing.SystemColors.Window;
            this.txtMarkerCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMarkerCount.Name = "txtMarkerCount";
            this.txtMarkerCount.ReadOnly = true;
            this.txtMarkerCount.Size = new System.Drawing.Size(35, 25);
            this.txtMarkerCount.Text = "0";
            // 
            // FormMarkers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 484);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lvwMarkers);
            this.HideOnClose = true;
            this.Name = "FormMarkers";
            this.TabText = "Markers";
            this.Text = "Markers";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomControls.ListView_ColumnSortingClick lvwMarkers;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox txtMarkerCount;
        private System.Windows.Forms.ColumnHeader colNo;
        private System.Windows.Forms.ColumnHeader colStartMarkersIndex;
        private System.Windows.Forms.ColumnHeader colPosition;
        private System.Windows.Forms.ColumnHeader colMarkerType;
        private System.Windows.Forms.ColumnHeader colLoopStart;
        private System.Windows.Forms.ColumnHeader colLoopMarkerIndex;
    }
}