
using sb_explorer.CustomControls;

namespace sb_explorer
{
    partial class FormSB_SamplePool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSB_SamplePool));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItem_SaveRaw = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItem_SaveSound = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_SendToPlayer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItem_Usage = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ButtonSaveRawData = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonSaveAudio = new System.Windows.Forms.ToolStripButton();
            this.ButtonSendToMediaPlayer = new System.Windows.Forms.ToolStripButton();
            this.ButtonApplyEffects = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonItemUsage = new System.Windows.Forms.ToolStripButton();
            this.listView1 = new sb_explorer.CustomControls.ListView_ColumnSortingClick();
            this.Col_SamplePool_HashCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SamplePool_Vol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SamplePool_VolRnd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SamplePool_Pitch = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SamplePool_PitchRnd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SamplePool_Pan = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SamplePool_PanRnd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_SaveRaw,
            this.toolStripSeparator2,
            this.MenuItem_SaveSound,
            this.MenuItem_SendToPlayer,
            this.toolStripSeparator1,
            this.MenuItem_Usage});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip1.Size = new System.Drawing.Size(151, 104);
            // 
            // MenuItem_SaveRaw
            // 
            this.MenuItem_SaveRaw.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_SaveRaw.Image")));
            this.MenuItem_SaveRaw.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_SaveRaw.Name = "MenuItem_SaveRaw";
            this.MenuItem_SaveRaw.Size = new System.Drawing.Size(150, 22);
            this.MenuItem_SaveRaw.Text = "Save Raw Data";
            this.MenuItem_SaveRaw.Click += new System.EventHandler(this.MenuItem_SaveRaw_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(147, 6);
            // 
            // MenuItem_SaveSound
            // 
            this.MenuItem_SaveSound.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_SaveSound.Image")));
            this.MenuItem_SaveSound.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_SaveSound.Name = "MenuItem_SaveSound";
            this.MenuItem_SaveSound.Size = new System.Drawing.Size(150, 22);
            this.MenuItem_SaveSound.Text = "Save";
            this.MenuItem_SaveSound.Click += new System.EventHandler(this.MenuItem_SaveSound_Click);
            // 
            // MenuItem_SendToPlayer
            // 
            this.MenuItem_SendToPlayer.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_SendToPlayer.Image")));
            this.MenuItem_SendToPlayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_SendToPlayer.Name = "MenuItem_SendToPlayer";
            this.MenuItem_SendToPlayer.Size = new System.Drawing.Size(150, 22);
            this.MenuItem_SendToPlayer.Text = "Send to player";
            this.MenuItem_SendToPlayer.Click += new System.EventHandler(this.MenuItem_SendToMediaPlayer_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(147, 6);
            // 
            // MenuItem_Usage
            // 
            this.MenuItem_Usage.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_Usage.Image")));
            this.MenuItem_Usage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_Usage.Name = "MenuItem_Usage";
            this.MenuItem_Usage.Size = new System.Drawing.Size(150, 22);
            this.MenuItem_Usage.Text = "Item Usage";
            this.MenuItem_Usage.Click += new System.EventHandler(this.MenuItem_Usage_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList1.Images.SetKeyName(0, "Sample.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonSaveRawData,
            this.toolStripSeparator3,
            this.ButtonSaveAudio,
            this.ButtonSendToMediaPlayer,
            this.ButtonApplyEffects,
            this.toolStripSeparator4,
            this.ButtonItemUsage});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(425, 35);
            this.toolStrip1.TabIndex = 1;
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
            // ButtonSendToMediaPlayer
            // 
            this.ButtonSendToMediaPlayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonSendToMediaPlayer.Image = ((System.Drawing.Image)(resources.GetObject("ButtonSendToMediaPlayer.Image")));
            this.ButtonSendToMediaPlayer.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonSendToMediaPlayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonSendToMediaPlayer.Name = "ButtonSendToMediaPlayer";
            this.ButtonSendToMediaPlayer.Size = new System.Drawing.Size(34, 32);
            this.ButtonSendToMediaPlayer.Text = "Send to Media Player";
            this.ButtonSendToMediaPlayer.Click += new System.EventHandler(this.ButtonPlayWithoutEffects_Click);
            // 
            // ButtonApplyEffects
            // 
            this.ButtonApplyEffects.Checked = true;
            this.ButtonApplyEffects.CheckOnClick = true;
            this.ButtonApplyEffects.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ButtonApplyEffects.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonApplyEffects.Image = ((System.Drawing.Image)(resources.GetObject("ButtonApplyEffects.Image")));
            this.ButtonApplyEffects.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonApplyEffects.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonApplyEffects.Name = "ButtonApplyEffects";
            this.ButtonApplyEffects.Size = new System.Drawing.Size(28, 32);
            this.ButtonApplyEffects.Text = "Apply sample effects";
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
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Col_SamplePool_HashCode,
            this.Col_SamplePool_Vol,
            this.Col_SamplePool_VolRnd,
            this.Col_SamplePool_Pitch,
            this.Col_SamplePool_PitchRnd,
            this.Col_SamplePool_Pan,
            this.Col_SamplePool_PanRnd});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 35);
            this.listView1.Margin = new System.Windows.Forms.Padding(0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(425, 282);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListView1_MouseDoubleClick);
            // 
            // Col_SamplePool_HashCode
            // 
            this.Col_SamplePool_HashCode.Text = "HashCode";
            this.Col_SamplePool_HashCode.Width = 80;
            // 
            // Col_SamplePool_Vol
            // 
            this.Col_SamplePool_Vol.Text = "Vol";
            this.Col_SamplePool_Vol.Width = 36;
            // 
            // Col_SamplePool_VolRnd
            // 
            this.Col_SamplePool_VolRnd.Text = "Vol +/-";
            this.Col_SamplePool_VolRnd.Width = 47;
            // 
            // Col_SamplePool_Pitch
            // 
            this.Col_SamplePool_Pitch.Text = "Pitch";
            this.Col_SamplePool_Pitch.Width = 42;
            // 
            // Col_SamplePool_PitchRnd
            // 
            this.Col_SamplePool_PitchRnd.Text = "Pitch +/-";
            // 
            // Col_SamplePool_Pan
            // 
            this.Col_SamplePool_Pan.Text = "Pan";
            this.Col_SamplePool_Pan.Width = 47;
            // 
            // Col_SamplePool_PanRnd
            // 
            this.Col_SamplePool_PanRnd.Text = "Pan +/-";
            this.Col_SamplePool_PanRnd.Width = 90;
            // 
            // FormSB_SamplePool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 317);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.listView1);
            this.HideOnClose = true;
            this.Name = "FormSB_SamplePool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TabText = "Sample Pool";
            this.Text = "Sample Pool";
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ColumnHeader Col_SamplePool_HashCode;
        private System.Windows.Forms.ColumnHeader Col_SamplePool_Vol;
        private System.Windows.Forms.ColumnHeader Col_SamplePool_VolRnd;
        private System.Windows.Forms.ColumnHeader Col_SamplePool_Pitch;
        private System.Windows.Forms.ColumnHeader Col_SamplePool_PitchRnd;
        private System.Windows.Forms.ColumnHeader Col_SamplePool_Pan;
        private System.Windows.Forms.ColumnHeader Col_SamplePool_PanRnd;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Usage;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SendToPlayer;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SaveSound;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SaveRaw;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ButtonSaveRawData;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton ButtonSaveAudio;
        private System.Windows.Forms.ToolStripButton ButtonSendToMediaPlayer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton ButtonItemUsage;
        protected internal ListView_ColumnSortingClick listView1;
        private System.Windows.Forms.ToolStripButton ButtonApplyEffects;
    }
}