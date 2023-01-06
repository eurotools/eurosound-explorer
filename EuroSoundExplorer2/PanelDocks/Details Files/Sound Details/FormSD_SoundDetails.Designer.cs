
namespace sb_explorer
{
    partial class FormSD_SoundDetails
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSD_SoundDetails));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lblTotalItems = new System.Windows.Forms.ToolStripLabel();
            this.txtItemsCount = new System.Windows.Forms.ToolStripTextBox();
            this.lblHashCodeMin = new System.Windows.Forms.ToolStripLabel();
            this.txtHashCodeMin = new System.Windows.Forms.ToolStripTextBox();
            this.lblHashCodeMax = new System.Windows.Forms.ToolStripLabel();
            this.txtHashCodeMax = new System.Windows.Forms.ToolStripTextBox();
            this.lblErrors = new System.Windows.Forms.ToolStripLabel();
            this.txtErrorsCount = new System.Windows.Forms.ToolStripTextBox();
            this.ButtonApplyFilter = new System.Windows.Forms.ToolStripButton();
            this.txtBoxSearch = new System.Windows.Forms.ToolStripTextBox();
            this.contextMenuListView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItem_CopyHashCode = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_CopyLabel = new System.Windows.Forms.ToolStripMenuItem();
            this.lstvSfxItems = new sb_explorer.CustomControls.ListView_ColumnSortingClick();
            this.Col_HashCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_HashCodeLabel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_InnerRadius = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_OuterRadius = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_Duration = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_Looping = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_Tracking3D = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SampleStreamed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_OutOfWater = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1.SuspendLayout();
            this.contextMenuListView.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblTotalItems,
            this.txtItemsCount,
            this.lblHashCodeMin,
            this.txtHashCodeMin,
            this.lblHashCodeMax,
            this.txtHashCodeMax,
            this.lblErrors,
            this.txtErrorsCount,
            this.ButtonApplyFilter,
            this.txtBoxSearch});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(568, 35);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lblTotalItems
            // 
            this.lblTotalItems.Name = "lblTotalItems";
            this.lblTotalItems.Size = new System.Drawing.Size(43, 32);
            this.lblTotalItems.Text = "Count:";
            // 
            // txtItemsCount
            // 
            this.txtItemsCount.BackColor = System.Drawing.SystemColors.Window;
            this.txtItemsCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtItemsCount.Name = "txtItemsCount";
            this.txtItemsCount.ReadOnly = true;
            this.txtItemsCount.Size = new System.Drawing.Size(35, 35);
            this.txtItemsCount.Text = "0";
            // 
            // lblHashCodeMin
            // 
            this.lblHashCodeMin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.lblHashCodeMin.Image = ((System.Drawing.Image)(resources.GetObject("lblHashCodeMin.Image")));
            this.lblHashCodeMin.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.lblHashCodeMin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lblHashCodeMin.Name = "lblHashCodeMin";
            this.lblHashCodeMin.Size = new System.Drawing.Size(32, 32);
            this.lblHashCodeMin.Text = "toolStripLabel2";
            // 
            // txtHashCodeMin
            // 
            this.txtHashCodeMin.BackColor = System.Drawing.SystemColors.Window;
            this.txtHashCodeMin.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtHashCodeMin.Name = "txtHashCodeMin";
            this.txtHashCodeMin.ReadOnly = true;
            this.txtHashCodeMin.Size = new System.Drawing.Size(72, 35);
            this.txtHashCodeMin.Text = "0";
            // 
            // lblHashCodeMax
            // 
            this.lblHashCodeMax.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.lblHashCodeMax.Image = ((System.Drawing.Image)(resources.GetObject("lblHashCodeMax.Image")));
            this.lblHashCodeMax.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.lblHashCodeMax.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lblHashCodeMax.Name = "lblHashCodeMax";
            this.lblHashCodeMax.Size = new System.Drawing.Size(32, 32);
            this.lblHashCodeMax.Text = "toolStripLabel1";
            // 
            // txtHashCodeMax
            // 
            this.txtHashCodeMax.BackColor = System.Drawing.SystemColors.Window;
            this.txtHashCodeMax.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtHashCodeMax.Name = "txtHashCodeMax";
            this.txtHashCodeMax.ReadOnly = true;
            this.txtHashCodeMax.Size = new System.Drawing.Size(72, 35);
            this.txtHashCodeMax.Text = "0";
            // 
            // lblErrors
            // 
            this.lblErrors.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.lblErrors.Image = ((System.Drawing.Image)(resources.GetObject("lblErrors.Image")));
            this.lblErrors.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.lblErrors.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lblErrors.Name = "lblErrors";
            this.lblErrors.Size = new System.Drawing.Size(23, 32);
            this.lblErrors.Text = "Errors:";
            // 
            // txtErrorsCount
            // 
            this.txtErrorsCount.BackColor = System.Drawing.SystemColors.Window;
            this.txtErrorsCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtErrorsCount.Name = "txtErrorsCount";
            this.txtErrorsCount.ReadOnly = true;
            this.txtErrorsCount.Size = new System.Drawing.Size(35, 35);
            this.txtErrorsCount.Text = "0";
            // 
            // ButtonApplyFilter
            // 
            this.ButtonApplyFilter.CheckOnClick = true;
            this.ButtonApplyFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonApplyFilter.Image = ((System.Drawing.Image)(resources.GetObject("ButtonApplyFilter.Image")));
            this.ButtonApplyFilter.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonApplyFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonApplyFilter.Name = "ButtonApplyFilter";
            this.ButtonApplyFilter.Size = new System.Drawing.Size(23, 32);
            this.ButtonApplyFilter.Text = "Apply Filter";
            this.ButtonApplyFilter.Click += new System.EventHandler(this.ButtonApplyFilter_Click);
            // 
            // txtBoxSearch
            // 
            this.txtBoxSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtBoxSearch.Name = "txtBoxSearch";
            this.txtBoxSearch.Size = new System.Drawing.Size(100, 35);
            // 
            // contextMenuListView
            // 
            this.contextMenuListView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_CopyHashCode,
            this.MenuItem_CopyLabel});
            this.contextMenuListView.Name = "contextMenuStrip1";
            this.contextMenuListView.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuListView.Size = new System.Drawing.Size(161, 48);
            // 
            // MenuItem_CopyHashCode
            // 
            this.MenuItem_CopyHashCode.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_CopyHashCode.Image")));
            this.MenuItem_CopyHashCode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_CopyHashCode.Name = "MenuItem_CopyHashCode";
            this.MenuItem_CopyHashCode.Size = new System.Drawing.Size(160, 22);
            this.MenuItem_CopyHashCode.Text = "Copy HashCode";
            this.MenuItem_CopyHashCode.Click += new System.EventHandler(this.MenuItem_CopyHashCode_Click);
            // 
            // MenuItem_CopyLabel
            // 
            this.MenuItem_CopyLabel.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_CopyLabel.Image")));
            this.MenuItem_CopyLabel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_CopyLabel.Name = "MenuItem_CopyLabel";
            this.MenuItem_CopyLabel.Size = new System.Drawing.Size(160, 22);
            this.MenuItem_CopyLabel.Text = "Copy Label";
            this.MenuItem_CopyLabel.Click += new System.EventHandler(this.MenuItem_CopyLabel_Click);
            // 
            // lstvSfxItems
            // 
            this.lstvSfxItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstvSfxItems.BackColor = System.Drawing.Color.White;
            this.lstvSfxItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Col_HashCode,
            this.Col_HashCodeLabel,
            this.Col_InnerRadius,
            this.Col_OuterRadius,
            this.Col_Duration,
            this.Col_Looping,
            this.Col_Tracking3D,
            this.Col_SampleStreamed,
            this.Col_OutOfWater});
            this.lstvSfxItems.ContextMenuStrip = this.contextMenuListView;
            this.lstvSfxItems.ForeColor = System.Drawing.Color.Black;
            this.lstvSfxItems.FullRowSelect = true;
            this.lstvSfxItems.HideSelection = false;
            this.lstvSfxItems.Location = new System.Drawing.Point(0, 38);
            this.lstvSfxItems.Name = "lstvSfxItems";
            this.lstvSfxItems.Size = new System.Drawing.Size(568, 382);
            this.lstvSfxItems.TabIndex = 4;
            this.lstvSfxItems.UseCompatibleStateImageBehavior = false;
            this.lstvSfxItems.View = System.Windows.Forms.View.Details;
            // 
            // Col_HashCode
            // 
            this.Col_HashCode.Text = "SFX Hashcode";
            this.Col_HashCode.Width = 90;
            // 
            // Col_HashCodeLabel
            // 
            this.Col_HashCodeLabel.Text = "Label";
            // 
            // Col_InnerRadius
            // 
            this.Col_InnerRadius.Text = "Inner Radius";
            this.Col_InnerRadius.Width = 90;
            // 
            // Col_OuterRadius
            // 
            this.Col_OuterRadius.Text = "Outer Radius";
            this.Col_OuterRadius.Width = 90;
            // 
            // Col_Duration
            // 
            this.Col_Duration.Text = "Duration";
            this.Col_Duration.Width = 90;
            // 
            // Col_Looping
            // 
            this.Col_Looping.Text = "Looping";
            this.Col_Looping.Width = 90;
            // 
            // Col_Tracking3D
            // 
            this.Col_Tracking3D.Text = "Tracking 3D";
            this.Col_Tracking3D.Width = 90;
            // 
            // Col_SampleStreamed
            // 
            this.Col_SampleStreamed.Text = "Sample Streamed";
            this.Col_SampleStreamed.Width = 90;
            // 
            // Col_OutOfWater
            // 
            this.Col_OutOfWater.Text = "Is 3D?";
            this.Col_OutOfWater.Width = 90;
            // 
            // FormSD_SoundDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 420);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lstvSfxItems);
            this.HideOnClose = true;
            this.Name = "FormSD_SoundDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TabText = "Sound Details File";
            this.Text = "Sound Details File";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuListView.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ColumnHeader Col_InnerRadius;
        private System.Windows.Forms.ColumnHeader Col_OuterRadius;
        private System.Windows.Forms.ColumnHeader Col_Duration;
        private System.Windows.Forms.ColumnHeader Col_Looping;
        private System.Windows.Forms.ColumnHeader Col_Tracking3D;
        private System.Windows.Forms.ColumnHeader Col_SampleStreamed;
        private System.Windows.Forms.ColumnHeader Col_OutOfWater;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel lblTotalItems;
        private System.Windows.Forms.ColumnHeader Col_HashCode;
        private System.Windows.Forms.ColumnHeader Col_HashCodeLabel;
        private System.Windows.Forms.ToolStripLabel lblErrors;
        private System.Windows.Forms.ToolStripTextBox txtErrorsCount;
        private System.Windows.Forms.ToolStripTextBox txtItemsCount;
        private System.Windows.Forms.ToolStripLabel lblHashCodeMin;
        private System.Windows.Forms.ToolStripTextBox txtHashCodeMin;
        private System.Windows.Forms.ToolStripLabel lblHashCodeMax;
        private System.Windows.Forms.ToolStripTextBox txtHashCodeMax;
        private System.Windows.Forms.ToolStripButton ButtonApplyFilter;
        private System.Windows.Forms.ToolStripTextBox txtBoxSearch;
        private System.Windows.Forms.ContextMenuStrip contextMenuListView;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_CopyHashCode;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_CopyLabel;
        protected internal CustomControls.ListView_ColumnSortingClick lstvSfxItems;
    }
}