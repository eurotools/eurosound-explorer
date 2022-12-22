
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
            this.listView1 = new EuroSoundExplorer2.CustomControls.ListView_ColumnSortingClick();
            this.Col_SamplePool_HashCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SamplePool_Vol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SamplePool_VolRnd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SamplePool_Pitch = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SamplePool_PitchRnd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SamplePool_Pan = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_SamplePool_PanRnd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItem_Usage = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_SendToMediaPlayer = new System.Windows.Forms.ToolStripMenuItem();
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
            this.MenuItem_Usage,
            this.MenuItem_SendToMediaPlayer});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip1.Size = new System.Drawing.Size(187, 48);
            // 
            // MenuItem_Usage
            // 
            this.MenuItem_Usage.Name = "MenuItem_Usage";
            this.MenuItem_Usage.Size = new System.Drawing.Size(186, 22);
            this.MenuItem_Usage.Text = "Usage";
            this.MenuItem_Usage.Click += new System.EventHandler(this.MenuItem_Usage_Click);
            // 
            // MenuItem_SendToMediaPlayer
            // 
            this.MenuItem_SendToMediaPlayer.Name = "MenuItem_SendToMediaPlayer";
            this.MenuItem_SendToMediaPlayer.Size = new System.Drawing.Size(186, 22);
            this.MenuItem_SendToMediaPlayer.Text = "Send To Media Player";
            this.MenuItem_SendToMediaPlayer.Click += new System.EventHandler(this.MenuItem_SendToMediaPlayer_Click);
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
    }
}