
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
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonSaveFile = new System.Windows.Forms.ToolStripButton();
            this.ButtonSendToMediaPlayer = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonDisplayMarkers = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItem_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_SendToMediaPlayer = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.lvwStreamData = new EuroSoundExplorer2.CustomControls.ListView_ColumnSortingClick();
            this.Col_No = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_Adpcm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_MarkerOffset = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_MarkerSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_AudioOffset = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_AudioLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_BaseVolume = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonValidateAllStreams,
            this.toolStripSeparator2,
            this.ButtonSaveFile,
            this.ButtonSendToMediaPlayer,
            this.toolStripSeparator1,
            this.ButtonDisplayMarkers});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(590, 35);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ButtonValidateAllStreams
            // 
            this.ButtonValidateAllStreams.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonValidateAllStreams.Enabled = false;
            this.ButtonValidateAllStreams.Image = ((System.Drawing.Image)(resources.GetObject("ButtonValidateAllStreams.Image")));
            this.ButtonValidateAllStreams.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonValidateAllStreams.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonValidateAllStreams.Name = "ButtonValidateAllStreams";
            this.ButtonValidateAllStreams.Size = new System.Drawing.Size(31, 32);
            this.ButtonValidateAllStreams.Text = "Validate All Streams";
            this.ButtonValidateAllStreams.Click += new System.EventHandler(this.ButtonValidateAllStreams_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 35);
            // 
            // ButtonSaveFile
            // 
            this.ButtonSaveFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonSaveFile.Image = ((System.Drawing.Image)(resources.GetObject("ButtonSaveFile.Image")));
            this.ButtonSaveFile.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonSaveFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonSaveFile.Name = "ButtonSaveFile";
            this.ButtonSaveFile.Size = new System.Drawing.Size(25, 32);
            this.ButtonSaveFile.Text = "Save File";
            this.ButtonSaveFile.Click += new System.EventHandler(this.ButtonSaveFile_Click);
            // 
            // ButtonSendToMediaPlayer
            // 
            this.ButtonSendToMediaPlayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonSendToMediaPlayer.Image = ((System.Drawing.Image)(resources.GetObject("ButtonSendToMediaPlayer.Image")));
            this.ButtonSendToMediaPlayer.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonSendToMediaPlayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonSendToMediaPlayer.Name = "ButtonSendToMediaPlayer";
            this.ButtonSendToMediaPlayer.Size = new System.Drawing.Size(34, 32);
            this.ButtonSendToMediaPlayer.Text = "Send To Media Player";
            this.ButtonSendToMediaPlayer.Click += new System.EventHandler(this.ButtonSendToMediaPlayer_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 35);
            // 
            // ButtonDisplayMarkers
            // 
            this.ButtonDisplayMarkers.Checked = true;
            this.ButtonDisplayMarkers.CheckOnClick = true;
            this.ButtonDisplayMarkers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ButtonDisplayMarkers.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonDisplayMarkers.Image = ((System.Drawing.Image)(resources.GetObject("ButtonDisplayMarkers.Image")));
            this.ButtonDisplayMarkers.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonDisplayMarkers.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonDisplayMarkers.Name = "ButtonDisplayMarkers";
            this.ButtonDisplayMarkers.Size = new System.Drawing.Size(35, 32);
            this.ButtonDisplayMarkers.Text = "Show Markers";
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
            this.MenuItem_Save.Image = ((System.Drawing.Image)(resources.GetObject("MenuItem_Save.Image")));
            this.MenuItem_Save.Name = "MenuItem_Save";
            this.MenuItem_Save.Size = new System.Drawing.Size(186, 22);
            this.MenuItem_Save.Text = "Save";
            this.MenuItem_Save.Click += new System.EventHandler(this.MenuItem_Save_Click);
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
            // lvwStreamData
            // 
            this.lvwStreamData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwStreamData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Col_No,
            this.Col_Adpcm,
            this.Col_MarkerOffset,
            this.Col_MarkerSize,
            this.Col_AudioOffset,
            this.Col_AudioLength,
            this.Col_BaseVolume});
            this.lvwStreamData.ContextMenuStrip = this.contextMenuStrip1;
            this.lvwStreamData.FullRowSelect = true;
            this.lvwStreamData.GridLines = true;
            this.lvwStreamData.HideSelection = false;
            this.lvwStreamData.Location = new System.Drawing.Point(0, 35);
            this.lvwStreamData.Margin = new System.Windows.Forms.Padding(0);
            this.lvwStreamData.Name = "lvwStreamData";
            this.lvwStreamData.Size = new System.Drawing.Size(590, 491);
            this.lvwStreamData.SmallImageList = this.imageList1;
            this.lvwStreamData.TabIndex = 0;
            this.lvwStreamData.UseCompatibleStateImageBehavior = false;
            this.lvwStreamData.View = System.Windows.Forms.View.Details;
            this.lvwStreamData.SelectedIndexChanged += new System.EventHandler(this.ListView1_SelectedIndexChanged);
            this.lvwStreamData.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListView1_MouseDoubleClick);
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
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList1.Images.SetKeyName(0, "File.png");
            this.imageList1.Images.SetKeyName(1, "SatusOK.png");
            this.imageList1.Images.SetKeyName(2, "SatusError.png");
            // 
            // FormStreamBank
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 526);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lvwStreamData);
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
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripButton ButtonSaveFile;
        private System.Windows.Forms.ToolStripButton ButtonSendToMediaPlayer;
        protected internal CustomControls.ListView_ColumnSortingClick lvwStreamData;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ButtonDisplayMarkers;
    }
}