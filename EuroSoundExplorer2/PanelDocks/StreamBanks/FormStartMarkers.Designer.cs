
namespace EuroSoundExplorer2
{
    partial class FormStartMarkers
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.Textbox_StartMarkers_Count = new System.Windows.Forms.ToolStripTextBox();
            this.lvwStartMarkers = new EuroSoundExplorer2.CustomControls.ListView_ColumnSortingClick();
            this.colNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPosition = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMarkerType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLoopStart = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLoopMarkerIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMarkerPosition = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.Textbox_StartMarkers_Count});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(613, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(43, 22);
            this.toolStripLabel1.Text = "Count:";
            // 
            // Textbox_StartMarkers_Count
            // 
            this.Textbox_StartMarkers_Count.BackColor = System.Drawing.SystemColors.Window;
            this.Textbox_StartMarkers_Count.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Textbox_StartMarkers_Count.Name = "Textbox_StartMarkers_Count";
            this.Textbox_StartMarkers_Count.ReadOnly = true;
            this.Textbox_StartMarkers_Count.Size = new System.Drawing.Size(35, 25);
            this.Textbox_StartMarkers_Count.Text = "0";
            // 
            // lvwStartMarkers
            // 
            this.lvwStartMarkers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwStartMarkers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colNo,
            this.colIndex,
            this.colPosition,
            this.colMarkerType,
            this.colLoopStart,
            this.colLoopMarkerIndex,
            this.colMarkerPosition});
            this.lvwStartMarkers.FullRowSelect = true;
            this.lvwStartMarkers.GridLines = true;
            this.lvwStartMarkers.HideSelection = false;
            this.lvwStartMarkers.Location = new System.Drawing.Point(0, 26);
            this.lvwStartMarkers.Name = "lvwStartMarkers";
            this.lvwStartMarkers.Size = new System.Drawing.Size(613, 458);
            this.lvwStartMarkers.TabIndex = 4;
            this.lvwStartMarkers.UseCompatibleStateImageBehavior = false;
            this.lvwStartMarkers.View = System.Windows.Forms.View.Details;
            // 
            // colNo
            // 
            this.colNo.Text = "No.";
            this.colNo.Width = 30;
            // 
            // colIndex
            // 
            this.colIndex.Text = "Index";
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
            // colMarkerPosition
            // 
            this.colMarkerPosition.Text = "Marker Position";
            this.colMarkerPosition.Width = 98;
            // 
            // FormStartMarkers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 484);
            this.Controls.Add(this.lvwStartMarkers);
            this.Controls.Add(this.toolStrip1);
            this.HideOnClose = true;
            this.Name = "FormStartMarkers";
            this.TabText = "Start Markers";
            this.Text = "Start Markers";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox Textbox_StartMarkers_Count;
        private CustomControls.ListView_ColumnSortingClick lvwStartMarkers;
        private System.Windows.Forms.ColumnHeader colNo;
        private System.Windows.Forms.ColumnHeader colIndex;
        private System.Windows.Forms.ColumnHeader colPosition;
        private System.Windows.Forms.ColumnHeader colMarkerType;
        private System.Windows.Forms.ColumnHeader colLoopStart;
        private System.Windows.Forms.ColumnHeader colLoopMarkerIndex;
        private System.Windows.Forms.ColumnHeader colMarkerPosition;
    }
}