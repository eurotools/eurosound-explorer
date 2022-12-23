
namespace EuroSoundExplorer2
{
    partial class FormStreamBank
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStreamBank));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ButtonValidateAllStreams = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItem_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_SendToMediaPlayer = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.listView1 = new EuroSoundExplorer2.CustomControls.ListView_ColumnSortingClick();
            this.Col_No = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_Adpcm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_MarkerOffset = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_MarkerSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_AudioOffset = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_AudioLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_BaseVolume = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonValidateAllStreams});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(590, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ButtonValidateAllStreams
            // 
            this.ButtonValidateAllStreams.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ButtonValidateAllStreams.Enabled = false;
            this.ButtonValidateAllStreams.Image = ((System.Drawing.Image)(resources.GetObject("ButtonValidateAllStreams.Image")));
            this.ButtonValidateAllStreams.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonValidateAllStreams.Name = "ButtonValidateAllStreams";
            this.ButtonValidateAllStreams.Size = new System.Drawing.Size(114, 22);
            this.ButtonValidateAllStreams.Text = "Validate All Streams";
            this.ButtonValidateAllStreams.Click += new System.EventHandler(this.ButtonValidateAllStreams_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_Save,
            this.MenuItem_SendToMediaPlayer});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip1.Size = new System.Drawing.Size(187, 48);
            // 
            // MenuItem_Save
            // 
            this.MenuItem_Save.Name = "MenuItem_Save";
            this.MenuItem_Save.Size = new System.Drawing.Size(186, 22);
            this.MenuItem_Save.Text = "Save";
            this.MenuItem_Save.Click += new System.EventHandler(this.MenuItem_Save_Click);
            // 
            // MenuItem_SendToMediaPlayer
            // 
            this.MenuItem_SendToMediaPlayer.Name = "MenuItem_SendToMediaPlayer";
            this.MenuItem_SendToMediaPlayer.Size = new System.Drawing.Size(186, 22);
            this.MenuItem_SendToMediaPlayer.Text = "Send To Media Player";
            this.MenuItem_SendToMediaPlayer.Click += new System.EventHandler(this.MenuItem_SendToMediaPlayer_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Col_No,
            this.Col_Adpcm,
            this.Col_MarkerOffset,
            this.Col_MarkerSize,
            this.Col_AudioOffset,
            this.Col_AudioLength,
            this.Col_BaseVolume});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 26);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(590, 500);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.ListView1_SelectedIndexChanged);
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListView1_MouseDoubleClick);
            // 
            // Col_No
            // 
            this.Col_No.Text = "No";
            this.Col_No.Width = 40;
            // 
            // Col_Adpcm
            // 
            this.Col_Adpcm.Text = "ADPCM";
            // 
            // Col_MarkerOffset
            // 
            this.Col_MarkerOffset.Text = "Marker Offset";
            this.Col_MarkerOffset.Width = 90;
            // 
            // Col_MarkerSize
            // 
            this.Col_MarkerSize.Text = "Marker Size";
            this.Col_MarkerSize.Width = 90;
            // 
            // Col_AudioOffset
            // 
            this.Col_AudioOffset.Text = "Audio Offset";
            this.Col_AudioOffset.Width = 90;
            // 
            // Col_AudioLength
            // 
            this.Col_AudioLength.Text = "Audio Length";
            this.Col_AudioLength.Width = 90;
            // 
            // Col_BaseVolume
            // 
            this.Col_BaseVolume.Text = "Base Volume";
            this.Col_BaseVolume.Width = 90;
            // 
            // FormStreamBank
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 526);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.listView1);
            this.HideOnClose = true;
            this.Name = "FormStreamBank";
            this.TabText = "Stream Data";
            this.Text = "Stream Data";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomControls.ListView_ColumnSortingClick listView1;
        private System.Windows.Forms.ColumnHeader Col_No;
        private System.Windows.Forms.ColumnHeader Col_Adpcm;
        private System.Windows.Forms.ColumnHeader Col_MarkerOffset;
        private System.Windows.Forms.ColumnHeader Col_MarkerSize;
        private System.Windows.Forms.ColumnHeader Col_AudioOffset;
        private System.Windows.Forms.ColumnHeader Col_AudioLength;
        private System.Windows.Forms.ColumnHeader Col_BaseVolume;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ButtonValidateAllStreams;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Save;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SendToMediaPlayer;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}