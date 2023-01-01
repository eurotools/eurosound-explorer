
using EuroSoundExplorer2.CustomControls;

namespace EuroSoundExplorer2
{
    partial class FormSB_WavHeaderData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSB_WavHeaderData));
            this.listView1 = new EuroSoundExplorer2.CustomControls.ListView_ColumnSortingClick();
            this.Col_No = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_Flags = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_Address = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_MemorySize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_SampleSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_Frequency = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_LoopStartOffset = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_Duration = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItem_SaveRaw = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItem_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Play = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItem_ItemUsage = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Col_No,
            this.Col_Flags,
            this.Col_Address,
            this.Col_MemorySize,
            this.col_SampleSize,
            this.col_Frequency,
            this.col_LoopStartOffset,
            this.col_Duration});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(503, 497);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListView1_MouseDoubleClick);
            // 
            // Col_No
            // 
            this.Col_No.Text = "No.";
            this.Col_No.Width = 34;
            // 
            // Col_Flags
            // 
            this.Col_Flags.Text = "Flags";
            this.Col_Flags.Width = 40;
            // 
            // Col_Address
            // 
            this.Col_Address.Text = "Adress";
            // 
            // Col_MemorySize
            // 
            this.Col_MemorySize.Text = "Memory Size";
            this.Col_MemorySize.Width = 75;
            // 
            // col_SampleSize
            // 
            this.col_SampleSize.Text = "Sample Size";
            this.col_SampleSize.Width = 73;
            // 
            // col_Frequency
            // 
            this.col_Frequency.Text = "Freq.";
            this.col_Frequency.Width = 42;
            // 
            // col_LoopStartOffset
            // 
            this.col_LoopStartOffset.Text = "LoopStartOffset";
            this.col_LoopStartOffset.Width = 89;
            // 
            // col_Duration
            // 
            this.col_Duration.Text = "Duration (ms)";
            this.col_Duration.Width = 80;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_SaveRaw,
            this.toolStripSeparator2,
            this.MenuItem_Save,
            this.MenuItem_Play,
            this.toolStripSeparator1,
            this.MenuItem_ItemUsage});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip1.Size = new System.Drawing.Size(187, 126);
            // 
            // MenuItem_SaveRaw
            // 
            this.MenuItem_SaveRaw.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_SaveRaw.Image")));
            this.MenuItem_SaveRaw.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_SaveRaw.Name = "MenuItem_SaveRaw";
            this.MenuItem_SaveRaw.Size = new System.Drawing.Size(186, 22);
            this.MenuItem_SaveRaw.Text = "Save Raw Data";
            this.MenuItem_SaveRaw.Click += new System.EventHandler(this.MenuItem_SaveRaw_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(183, 6);
            // 
            // MenuItem_Save
            // 
            this.MenuItem_Save.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_Save.Image")));
            this.MenuItem_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_Save.Name = "MenuItem_Save";
            this.MenuItem_Save.Size = new System.Drawing.Size(186, 22);
            this.MenuItem_Save.Text = "Save";
            this.MenuItem_Save.Click += new System.EventHandler(this.MenuItem_Save_Click);
            // 
            // MenuItem_Play
            // 
            this.MenuItem_Play.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_Play.Image")));
            this.MenuItem_Play.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_Play.Name = "MenuItem_Play";
            this.MenuItem_Play.Size = new System.Drawing.Size(186, 22);
            this.MenuItem_Play.Text = "Send To Media Player";
            this.MenuItem_Play.Click += new System.EventHandler(this.MenuItem_Play_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(183, 6);
            // 
            // MenuItem_ItemUsage
            // 
            this.MenuItem_ItemUsage.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_ItemUsage.Image")));
            this.MenuItem_ItemUsage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_ItemUsage.Name = "MenuItem_ItemUsage";
            this.MenuItem_ItemUsage.Size = new System.Drawing.Size(186, 22);
            this.MenuItem_ItemUsage.Text = "Item Usage";
            this.MenuItem_ItemUsage.Click += new System.EventHandler(this.MenuItem_ItemUsage_Click);
            // 
            // FormSB_WavHeaderData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 497);
            this.Controls.Add(this.listView1);
            this.HideOnClose = true;
            this.Name = "FormSB_WavHeaderData";
            this.TabText = "Wav Header Data";
            this.Text = "Wav Header Data";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ListView_ColumnSortingClick listView1;
        private System.Windows.Forms.ColumnHeader Col_No;
        private System.Windows.Forms.ColumnHeader Col_Flags;
        private System.Windows.Forms.ColumnHeader Col_Address;
        private System.Windows.Forms.ColumnHeader Col_MemorySize;
        private System.Windows.Forms.ColumnHeader col_SampleSize;
        private System.Windows.Forms.ColumnHeader col_Frequency;
        private System.Windows.Forms.ColumnHeader col_LoopStartOffset;
        private System.Windows.Forms.ColumnHeader col_Duration;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Save;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Play;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_ItemUsage;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SaveRaw;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}