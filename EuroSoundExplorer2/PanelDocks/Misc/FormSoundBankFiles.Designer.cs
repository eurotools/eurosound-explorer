
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
            this.lvwFiles = new EuroSoundExplorer2.CustomControls.ListView_ColumnSortingClick();
            this.Col_HashCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_HashCode_Label = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_FileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_FileSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SFXsCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItem_Load = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Reload = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Unload = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Separator = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItem_DataViewer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.labelTotal = new System.Windows.Forms.ToolStripLabel();
            this.txtTotal = new System.Windows.Forms.ToolStripTextBox();
            this.btnReloadList = new System.Windows.Forms.ToolStripButton();
            this.btnReloadHashCodes = new System.Windows.Forms.ToolStripButton();
            this.Col_Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.lvwFiles.Location = new System.Drawing.Point(0, 25);
            this.lvwFiles.Name = "lvwFiles";
            this.lvwFiles.Size = new System.Drawing.Size(438, 301);
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
            this.MenuItem_Load.Name = "MenuItem_Load";
            this.MenuItem_Load.Size = new System.Drawing.Size(136, 22);
            this.MenuItem_Load.Text = "Load";
            this.MenuItem_Load.Click += new System.EventHandler(this.MenuItem_Load_Click);
            // 
            // MenuItem_Reload
            // 
            this.MenuItem_Reload.Name = "MenuItem_Reload";
            this.MenuItem_Reload.Size = new System.Drawing.Size(136, 22);
            this.MenuItem_Reload.Text = "Reload";
            this.MenuItem_Reload.Click += new System.EventHandler(this.MenuItem_Reload_Click);
            // 
            // MenuItem_Unload
            // 
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
            this.MenuItem_DataViewer.Name = "MenuItem_DataViewer";
            this.MenuItem_DataViewer.Size = new System.Drawing.Size(136, 22);
            this.MenuItem_DataViewer.Text = "Data Viewer";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelTotal,
            this.txtTotal,
            this.btnReloadList,
            this.btnReloadHashCodes});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(438, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // labelTotal
            // 
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(32, 22);
            this.labelTotal.Text = "Total";
            // 
            // txtTotal
            // 
            this.txtTotal.BackColor = System.Drawing.SystemColors.Window;
            this.txtTotal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.ReadOnly = true;
            this.txtTotal.Size = new System.Drawing.Size(100, 25);
            this.txtTotal.Text = "0";
            // 
            // btnReloadList
            // 
            this.btnReloadList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnReloadList.Image = ((System.Drawing.Image)(resources.GetObject("btnReloadList.Image")));
            this.btnReloadList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReloadList.Name = "btnReloadList";
            this.btnReloadList.Size = new System.Drawing.Size(70, 22);
            this.btnReloadList.Text = "Update List";
            this.btnReloadList.Click += new System.EventHandler(this.BtnReloadList_Click);
            // 
            // btnReloadHashCodes
            // 
            this.btnReloadHashCodes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnReloadHashCodes.Image = ((System.Drawing.Image)(resources.GetObject("btnReloadHashCodes.Image")));
            this.btnReloadHashCodes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReloadHashCodes.Name = "btnReloadHashCodes";
            this.btnReloadHashCodes.Size = new System.Drawing.Size(94, 22);
            this.btnReloadHashCodes.Text = "Reload Sound.h";
            this.btnReloadHashCodes.Click += new System.EventHandler(this.BtnReloadHashCodes_Click);
            // 
            // Col_Type
            // 
            this.Col_Type.Text = "Type";
            // 
            // FormSoundBankFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 326);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lvwFiles);
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
    }
}