
using sb_explorer.CustomControls;

namespace sb_explorer
{
    partial class FrmDuplicatedHashCodes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDuplicatedHashCodes));
            this.lvwDuplicatedHashCodes = new sb_explorer.CustomControls.ListView_ColumnSortingClick();
            this.Col_HashCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_HashCode_Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_HashCode_Label = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuListView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItem_CopyHashCode = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_CopyLabel = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuListView.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvwDuplicatedHashCodes
            // 
            this.lvwDuplicatedHashCodes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Col_HashCode,
            this.Col_HashCode_Status,
            this.Col_HashCode_Label});
            this.lvwDuplicatedHashCodes.ContextMenuStrip = this.contextMenuListView;
            this.lvwDuplicatedHashCodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwDuplicatedHashCodes.FullRowSelect = true;
            this.lvwDuplicatedHashCodes.GridLines = true;
            this.lvwDuplicatedHashCodes.HideSelection = false;
            this.lvwDuplicatedHashCodes.Location = new System.Drawing.Point(0, 0);
            this.lvwDuplicatedHashCodes.Margin = new System.Windows.Forms.Padding(0);
            this.lvwDuplicatedHashCodes.Name = "lvwDuplicatedHashCodes";
            this.lvwDuplicatedHashCodes.Size = new System.Drawing.Size(591, 466);
            this.lvwDuplicatedHashCodes.SmallImageList = this.imageList1;
            this.lvwDuplicatedHashCodes.TabIndex = 0;
            this.lvwDuplicatedHashCodes.UseCompatibleStateImageBehavior = false;
            this.lvwDuplicatedHashCodes.View = System.Windows.Forms.View.Details;
            // 
            // Col_HashCode
            // 
            this.Col_HashCode.Text = "HashCode";
            this.Col_HashCode.Width = 96;
            // 
            // Col_HashCode_Status
            // 
            this.Col_HashCode_Status.Text = "Status";
            this.Col_HashCode_Status.Width = 70;
            // 
            // Col_HashCode_Label
            // 
            this.Col_HashCode_Label.Text = "Label";
            this.Col_HashCode_Label.Width = 250;
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
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList1.Images.SetKeyName(0, "SfxItem.png");
            this.imageList1.Images.SetKeyName(1, "SatusError.png");
            this.imageList1.Images.SetKeyName(2, "SatusOK.png");
            // 
            // FrmDuplicatedHashCodes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 466);
            this.Controls.Add(this.lvwDuplicatedHashCodes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDuplicatedHashCodes";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Duplicated HashCodes";
            this.Load += new System.EventHandler(this.FrmDuplicatedHashCodes_Load);
            this.contextMenuListView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected internal CustomControls.ListView_ColumnSortingClick lvwDuplicatedHashCodes;
        private System.Windows.Forms.ColumnHeader Col_HashCode;
        private System.Windows.Forms.ColumnHeader Col_HashCode_Status;
        private System.Windows.Forms.ColumnHeader Col_HashCode_Label;
        private System.Windows.Forms.ContextMenuStrip contextMenuListView;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_CopyHashCode;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_CopyLabel;
        private System.Windows.Forms.ImageList imageList1;
    }
}