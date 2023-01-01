
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItem_SaveRaw = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItem_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Play = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItem_ItemUsage = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ButtonSaveRawData = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonSaveAudio = new System.Windows.Forms.ToolStripButton();
            this.ButtonSaveToMediaPlayer = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonItemUsage = new System.Windows.Forms.ToolStripButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.listView1 = new EuroSoundExplorer2.CustomControls.ListView_ColumnSortingClick();
            this.Col_No = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_Flags = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_Address = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_MemorySize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_SampleSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_Frequency = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_LoopStartOffset = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_Duration = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.contextMenuStrip1.Size = new System.Drawing.Size(187, 104);
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
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonSaveRawData,
            this.toolStripSeparator3,
            this.ButtonSaveAudio,
            this.ButtonSaveToMediaPlayer,
            this.toolStripSeparator4,
            this.ButtonItemUsage});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(503, 35);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ButtonSaveRawData
            // 
            this.ButtonSaveRawData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonSaveRawData.Image = ((System.Drawing.Image)(resources.GetObject("ButtonSaveRawData.Image")));
            this.ButtonSaveRawData.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonSaveRawData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonSaveRawData.Name = "ButtonSaveRawData";
            this.ButtonSaveRawData.Size = new System.Drawing.Size(26, 32);
            this.ButtonSaveRawData.Text = "Save Raw Data";
            this.ButtonSaveRawData.Click += new System.EventHandler(this.ButtonSaveRawData_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 35);
            // 
            // ButtonSaveAudio
            // 
            this.ButtonSaveAudio.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonSaveAudio.Image = ((System.Drawing.Image)(resources.GetObject("ButtonSaveAudio.Image")));
            this.ButtonSaveAudio.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonSaveAudio.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonSaveAudio.Name = "ButtonSaveAudio";
            this.ButtonSaveAudio.Size = new System.Drawing.Size(25, 32);
            this.ButtonSaveAudio.Text = "Decode and Save";
            this.ButtonSaveAudio.Click += new System.EventHandler(this.ButtonSaveAudio_Click);
            // 
            // ButtonSaveToMediaPlayer
            // 
            this.ButtonSaveToMediaPlayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonSaveToMediaPlayer.Image = ((System.Drawing.Image)(resources.GetObject("ButtonSaveToMediaPlayer.Image")));
            this.ButtonSaveToMediaPlayer.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonSaveToMediaPlayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonSaveToMediaPlayer.Name = "ButtonSaveToMediaPlayer";
            this.ButtonSaveToMediaPlayer.Size = new System.Drawing.Size(34, 32);
            this.ButtonSaveToMediaPlayer.Text = "Send To Media Player";
            this.ButtonSaveToMediaPlayer.Click += new System.EventHandler(this.ButtonSaveToMediaPlayer_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 35);
            // 
            // ButtonItemUsage
            // 
            this.ButtonItemUsage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonItemUsage.Image = ((System.Drawing.Image)(resources.GetObject("ButtonItemUsage.Image")));
            this.ButtonItemUsage.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonItemUsage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonItemUsage.Name = "ButtonItemUsage";
            this.ButtonItemUsage.Size = new System.Drawing.Size(28, 32);
            this.ButtonItemUsage.Text = "Item Usage";
            this.ButtonItemUsage.Click += new System.EventHandler(this.ButtonItemUsage_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList1.Images.SetKeyName(0, "MixedSample.png");
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 35);
            this.listView1.Margin = new System.Windows.Forms.Padding(0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(503, 462);
            this.listView1.SmallImageList = this.imageList1;
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
            // FormSB_WavHeaderData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 497);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.listView1);
            this.HideOnClose = true;
            this.Name = "FormSB_WavHeaderData";
            this.TabText = "Wav Header Data";
            this.Text = "Wav Header Data";
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
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
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ButtonSaveRawData;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton ButtonSaveAudio;
        private System.Windows.Forms.ToolStripButton ButtonSaveToMediaPlayer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton ButtonItemUsage;
        private System.Windows.Forms.ImageList imageList1;
        protected internal ListView_ColumnSortingClick listView1;
    }
}