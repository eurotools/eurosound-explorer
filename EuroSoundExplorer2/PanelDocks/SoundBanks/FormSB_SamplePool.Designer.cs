
using EuroSoundExplorer2.CustomControls;

namespace EuroSoundExplorer2
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
            this.listView1 = new EuroSoundExplorer2.CustomControls.ListView_ColumnSortingClick();
            this.Col_SamplePool_HashCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SamplePool_Vol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SamplePool_VolRnd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SamplePool_Pitch = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SamplePool_PitchRnd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SamplePool_Pan = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SamplePool_PanRnd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItem_SaveRaw = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItem_SaveSound = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_SendToMediaPlayer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItem_Usage = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Col_SamplePool_HashCode,
            this.Col_SamplePool_Vol,
            this.Col_SamplePool_VolRnd,
            this.Col_SamplePool_Pitch,
            this.Col_SamplePool_PitchRnd,
            this.Col_SamplePool_Pan,
            this.Col_SamplePool_PanRnd});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(425, 317);
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
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_SaveRaw,
            this.toolStripSeparator2,
            this.MenuItem_SaveSound,
            this.MenuItem_SendToMediaPlayer,
            this.toolStripSeparator1,
            this.MenuItem_Usage});
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
            // MenuItem_SaveSound
            // 
            this.MenuItem_SaveSound.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_SaveSound.Image")));
            this.MenuItem_SaveSound.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_SaveSound.Name = "MenuItem_SaveSound";
            this.MenuItem_SaveSound.Size = new System.Drawing.Size(186, 22);
            this.MenuItem_SaveSound.Text = "Save";
            this.MenuItem_SaveSound.Click += new System.EventHandler(this.MenuItem_SaveSound_Click);
            // 
            // MenuItem_SendToMediaPlayer
            // 
            this.MenuItem_SendToMediaPlayer.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_SendToMediaPlayer.Image")));
            this.MenuItem_SendToMediaPlayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_SendToMediaPlayer.Name = "MenuItem_SendToMediaPlayer";
            this.MenuItem_SendToMediaPlayer.Size = new System.Drawing.Size(186, 22);
            this.MenuItem_SendToMediaPlayer.Text = "Send To Media Player";
            this.MenuItem_SendToMediaPlayer.Click += new System.EventHandler(this.MenuItem_SendToMediaPlayer_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(183, 6);
            // 
            // MenuItem_Usage
            // 
            this.MenuItem_Usage.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_Usage.Image")));
            this.MenuItem_Usage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuItem_Usage.Name = "MenuItem_Usage";
            this.MenuItem_Usage.Size = new System.Drawing.Size(186, 22);
            this.MenuItem_Usage.Text = "Item Usage";
            this.MenuItem_Usage.Click += new System.EventHandler(this.MenuItem_Usage_Click);
            // 
            // FormSB_SamplePool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 317);
            this.Controls.Add(this.listView1);
            this.HideOnClose = true;
            this.Name = "FormSB_SamplePool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TabText = "Sample Pool";
            this.Text = "Sample Pool";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ListView_ColumnSortingClick listView1;
        private System.Windows.Forms.ColumnHeader Col_SamplePool_HashCode;
        private System.Windows.Forms.ColumnHeader Col_SamplePool_Vol;
        private System.Windows.Forms.ColumnHeader Col_SamplePool_VolRnd;
        private System.Windows.Forms.ColumnHeader Col_SamplePool_Pitch;
        private System.Windows.Forms.ColumnHeader Col_SamplePool_PitchRnd;
        private System.Windows.Forms.ColumnHeader Col_SamplePool_Pan;
        private System.Windows.Forms.ColumnHeader Col_SamplePool_PanRnd;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Usage;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SendToMediaPlayer;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SaveSound;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SaveRaw;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}