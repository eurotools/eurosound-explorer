
using EuroSoundExplorer2.CustomControls;

namespace EuroSoundExplorer2
{
    partial class FormSoundBankFiles
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSoundBankFiles));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItem_Load = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Reload = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Unload = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Separator = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItem_DataViewer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnListView = new System.Windows.Forms.ToolStripButton();
            this.btnTreeView = new System.Windows.Forms.ToolStripButton();
            this.btnReloadList = new System.Windows.Forms.ToolStripButton();
            this.btnReloadHashCodes = new System.Windows.Forms.ToolStripButton();
            this.labelTotal = new System.Windows.Forms.ToolStripLabel();
            this.txtTotal = new System.Windows.Forms.ToolStripTextBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lvwFiles = new EuroSoundExplorer2.CustomControls.ListView_ColumnSortingClick();
            this.Col_HashCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_HashCode_Label = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_FileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_FileSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SFXsCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_Load,
            this.MenuItem_Reload,
            this.MenuItem_Unload,
            this.MenuItem_Separator,
            this.MenuItem_DataViewer});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 98);
            // 
            // MenuItem_Load
            // 
            this.MenuItem_Load.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_Load.Image")));
            this.MenuItem_Load.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_Load.Name = "MenuItem_Load";
            this.MenuItem_Load.Size = new System.Drawing.Size(136, 22);
            this.MenuItem_Load.Text = "Load";
            this.MenuItem_Load.Click += new System.EventHandler(this.MenuItem_Load_Click);
            // 
            // MenuItem_Reload
            // 
            this.MenuItem_Reload.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_Reload.Image")));
            this.MenuItem_Reload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_Reload.Name = "MenuItem_Reload";
            this.MenuItem_Reload.Size = new System.Drawing.Size(136, 22);
            this.MenuItem_Reload.Text = "Reload";
            this.MenuItem_Reload.Click += new System.EventHandler(this.MenuItem_Reload_Click);
            // 
            // MenuItem_Unload
            // 
            this.MenuItem_Unload.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_Unload.Image")));
            this.MenuItem_Unload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_Unload.Name = "MenuItem_Unload";
            this.MenuItem_Unload.Size = new System.Drawing.Size(136, 22);
            this.MenuItem_Unload.Text = "Unload";
            this.MenuItem_Unload.Click += new System.EventHandler(this.MenuItem_Unload_Click);
            // 
            // MenuItem_Separator
            // 
            this.MenuItem_Separator.Name = "MenuItem_Separator";
            this.MenuItem_Separator.Size = new System.Drawing.Size(133, 6);
            // 
            // MenuItem_DataViewer
            // 
            this.MenuItem_DataViewer.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_DataViewer.Image")));
            this.MenuItem_DataViewer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_DataViewer.Name = "MenuItem_DataViewer";
            this.MenuItem_DataViewer.Size = new System.Drawing.Size(136, 22);
            this.MenuItem_DataViewer.Text = "Data Viewer";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnListView,
            this.btnTreeView,
            this.btnReloadList,
            this.btnReloadHashCodes,
            this.labelTotal,
            this.txtTotal});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(437, 30);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnListView
            // 
            this.btnListView.Checked = true;
            this.btnListView.CheckOnClick = true;
            this.btnListView.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnListView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnListView.Image = ((System.Drawing.Image)(resources.GetObject("btnListView.Image")));
            this.btnListView.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnListView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnListView.Name = "btnListView";
            this.btnListView.Size = new System.Drawing.Size(26, 27);
            this.btnListView.Text = "List View";
            this.btnListView.Click += new System.EventHandler(this.BtnListView_Click);
            // 
            // btnTreeView
            // 
            this.btnTreeView.CheckOnClick = true;
            this.btnTreeView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnTreeView.Image = ((System.Drawing.Image)(resources.GetObject("btnTreeView.Image")));
            this.btnTreeView.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnTreeView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTreeView.Name = "btnTreeView";
            this.btnTreeView.Size = new System.Drawing.Size(26, 27);
            this.btnTreeView.Text = "Tree View";
            this.btnTreeView.Click += new System.EventHandler(this.BtnTreeView_Click);
            // 
            // btnReloadList
            // 
            this.btnReloadList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnReloadList.Image = ((System.Drawing.Image)(resources.GetObject("btnReloadList.Image")));
            this.btnReloadList.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnReloadList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReloadList.Name = "btnReloadList";
            this.btnReloadList.Size = new System.Drawing.Size(28, 27);
            this.btnReloadList.Text = "Update List";
            this.btnReloadList.Click += new System.EventHandler(this.BtnReloadList_Click);
            // 
            // btnReloadHashCodes
            // 
            this.btnReloadHashCodes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnReloadHashCodes.Image = ((System.Drawing.Image)(resources.GetObject("btnReloadHashCodes.Image")));
            this.btnReloadHashCodes.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnReloadHashCodes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReloadHashCodes.Name = "btnReloadHashCodes";
            this.btnReloadHashCodes.Size = new System.Drawing.Size(25, 27);
            this.btnReloadHashCodes.Text = "Reload Sound.h";
            this.btnReloadHashCodes.Click += new System.EventHandler(this.BtnReloadHashCodes_Click);
            // 
            // labelTotal
            // 
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(43, 27);
            this.labelTotal.Text = "Count:";
            // 
            // txtTotal
            // 
            this.txtTotal.BackColor = System.Drawing.SystemColors.Window;
            this.txtTotal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.ReadOnly = true;
            this.txtTotal.Size = new System.Drawing.Size(35, 30);
            this.txtTotal.Text = "0";
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 30);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(437, 343);
            this.treeView1.StateImageList = this.imageList1;
            this.treeView1.TabIndex = 2;
            this.treeView1.Visible = false;
            this.treeView1.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeView1_BeforeCollapse);
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeView1_BeforeExpand);
            this.treeView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TreeView1_MouseDoubleClick);
            this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TreeView1_MouseDown);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList1.Images.SetKeyName(0, "folderClosed.png");
            this.imageList1.Images.SetKeyName(1, "folderOpen.png");
            this.imageList1.Images.SetKeyName(2, "SoundBankIcon.png");
            // 
            // lvwFiles
            // 
            this.lvwFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Col_HashCode,
            this.Col_HashCode_Label,
            this.Col_FileName,
            this.Col_Status,
            this.Col_FileSize,
            this.Col_SFXsCount,
            this.Col_Type});
            this.lvwFiles.ContextMenuStrip = this.contextMenuStrip1;
            this.lvwFiles.FullRowSelect = true;
            this.lvwFiles.GridLines = true;
            this.lvwFiles.HideSelection = false;
            this.lvwFiles.Location = new System.Drawing.Point(0, 30);
            this.lvwFiles.Margin = new System.Windows.Forms.Padding(0);
            this.lvwFiles.Name = "lvwFiles";
            this.lvwFiles.Size = new System.Drawing.Size(437, 343);
            this.lvwFiles.SmallImageList = this.imageList1;
            this.lvwFiles.TabIndex = 0;
            this.lvwFiles.UseCompatibleStateImageBehavior = false;
            this.lvwFiles.View = System.Windows.Forms.View.Details;
            this.lvwFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LvwFiles_MouseDoubleClick);
            // 
            // Col_HashCode
            // 
            this.Col_HashCode.Text = "HashCode";
            this.Col_HashCode.Width = 80;
            // 
            // Col_HashCode_Label
            // 
            this.Col_HashCode_Label.Text = "HashCode Label";
            this.Col_HashCode_Label.Width = 170;
            // 
            // Col_FileName
            // 
            this.Col_FileName.Text = "File Name";
            this.Col_FileName.Width = 150;
            // 
            // Col_Status
            // 
            this.Col_Status.Text = "Status";
            this.Col_Status.Width = 70;
            // 
            // Col_FileSize
            // 
            this.Col_FileSize.Text = "Size";
            this.Col_FileSize.Width = 70;
            // 
            // Col_SFXsCount
            // 
            this.Col_SFXsCount.Text = "SFXs";
            this.Col_SFXsCount.Width = 70;
            // 
            // Col_Type
            // 
            this.Col_Type.Text = "Type";
            // 
            // FormSoundBankFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 373);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lvwFiles);
            this.Controls.Add(this.treeView1);
            this.HideOnClose = true;
            this.Name = "FormSoundBankFiles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TabText = "SoundBank Files";
            this.Text = "SoundBank Files";
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListView_ColumnSortingClick lvwFiles;
        private System.Windows.Forms.ColumnHeader Col_HashCode;
        private System.Windows.Forms.ColumnHeader Col_FileName;
        private System.Windows.Forms.ColumnHeader Col_Status;
        private System.Windows.Forms.ColumnHeader Col_FileSize;
        private System.Windows.Forms.ColumnHeader Col_SFXsCount;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel labelTotal;
        private System.Windows.Forms.ToolStripTextBox txtTotal;
        private System.Windows.Forms.ToolStripButton btnReloadList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Load;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Reload;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Unload;
        private System.Windows.Forms.ToolStripSeparator MenuItem_Separator;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_DataViewer;
        private System.Windows.Forms.ColumnHeader Col_HashCode_Label;
        private System.Windows.Forms.ToolStripButton btnReloadHashCodes;
        private System.Windows.Forms.ColumnHeader Col_Type;
        private System.Windows.Forms.ToolStripButton btnTreeView;
        private System.Windows.Forms.ToolStripButton btnListView;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ImageList imageList1;
    }
}