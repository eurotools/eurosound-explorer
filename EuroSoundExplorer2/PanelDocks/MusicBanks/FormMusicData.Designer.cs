
namespace sb_explorer
{
    partial class FormMusicData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMusicData));
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ButtonSendMediaPlayer = new System.Windows.Forms.ToolStripButton();
            this.ButtonDisplayMusicMarkers = new System.Windows.Forms.ToolStripButton();
            this.ButtonAutoSendMarkers = new System.Windows.Forms.ToolStripButton();
            this.labelAdpcmStatus = new System.Windows.Forms.ToolStripLabel();
            this.textboxAdpcmStatus = new System.Windows.Forms.ToolStripTextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 35);
            this.propertyGrid1.Margin = new System.Windows.Forms.Padding(0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(410, 305);
            this.propertyGrid1.TabIndex = 6;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonSendMediaPlayer,
            this.ButtonDisplayMusicMarkers,
            this.ButtonAutoSendMarkers,
            this.labelAdpcmStatus,
            this.textboxAdpcmStatus});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(410, 35);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ButtonSendMediaPlayer
            // 
            this.ButtonSendMediaPlayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonSendMediaPlayer.Image = ((System.Drawing.Image)(resources.GetObject("ButtonSendMediaPlayer.Image")));
            this.ButtonSendMediaPlayer.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonSendMediaPlayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonSendMediaPlayer.Name = "ButtonSendMediaPlayer";
            this.ButtonSendMediaPlayer.Size = new System.Drawing.Size(34, 32);
            this.ButtonSendMediaPlayer.Text = "Send to media player";
            this.ButtonSendMediaPlayer.Click += new System.EventHandler(this.ButtonSendMediaPlayer_Click);
            // 
            // ButtonDisplayMusicMarkers
            // 
            this.ButtonDisplayMusicMarkers.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonDisplayMusicMarkers.Image = ((System.Drawing.Image)(resources.GetObject("ButtonDisplayMusicMarkers.Image")));
            this.ButtonDisplayMusicMarkers.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonDisplayMusicMarkers.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonDisplayMusicMarkers.Name = "ButtonDisplayMusicMarkers";
            this.ButtonDisplayMusicMarkers.Size = new System.Drawing.Size(35, 32);
            this.ButtonDisplayMusicMarkers.Text = "Display Markers";
            this.ButtonDisplayMusicMarkers.Click += new System.EventHandler(this.ButtonDisplayMusicMarkers_Click);
            // 
            // ButtonAutoSendMarkers
            // 
            this.ButtonAutoSendMarkers.Checked = true;
            this.ButtonAutoSendMarkers.CheckOnClick = true;
            this.ButtonAutoSendMarkers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ButtonAutoSendMarkers.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonAutoSendMarkers.Image = ((System.Drawing.Image)(resources.GetObject("ButtonAutoSendMarkers.Image")));
            this.ButtonAutoSendMarkers.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonAutoSendMarkers.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonAutoSendMarkers.Name = "ButtonAutoSendMarkers";
            this.ButtonAutoSendMarkers.Size = new System.Drawing.Size(39, 32);
            this.ButtonAutoSendMarkers.Text = "toolStripButton1";
            this.ButtonAutoSendMarkers.ToolTipText = "Automatically Show Markers";
            // 
            // labelAdpcmStatus
            // 
            this.labelAdpcmStatus.Name = "labelAdpcmStatus";
            this.labelAdpcmStatus.Size = new System.Drawing.Size(87, 32);
            this.labelAdpcmStatus.Text = "ADPCM Status:";
            // 
            // textboxAdpcmStatus
            // 
            this.textboxAdpcmStatus.BackColor = System.Drawing.SystemColors.Window;
            this.textboxAdpcmStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textboxAdpcmStatus.Name = "textboxAdpcmStatus";
            this.textboxAdpcmStatus.ReadOnly = true;
            this.textboxAdpcmStatus.Size = new System.Drawing.Size(100, 35);
            // 
            // FormMusicData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 340);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.propertyGrid1);
            this.HideOnClose = true;
            this.Name = "FormMusicData";
            this.TabText = "Music Data";
            this.Text = "Music Data";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ButtonSendMediaPlayer;
        private System.Windows.Forms.ToolStripButton ButtonDisplayMusicMarkers;
        private System.Windows.Forms.ToolStripLabel labelAdpcmStatus;
        private System.Windows.Forms.ToolStripTextBox textboxAdpcmStatus;
        private System.Windows.Forms.ToolStripButton ButtonAutoSendMarkers;
        protected internal System.Windows.Forms.PropertyGrid propertyGrid1;
    }
}