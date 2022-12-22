
namespace EuroSoundExplorer2
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
            this.ButtonMediaPlayer = new System.Windows.Forms.Button();
            this.buttonDisplayMarkers = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // ButtonMediaPlayer
            // 
            this.ButtonMediaPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonMediaPlayer.Location = new System.Drawing.Point(274, 12);
            this.ButtonMediaPlayer.Name = "ButtonMediaPlayer";
            this.ButtonMediaPlayer.Size = new System.Drawing.Size(124, 23);
            this.ButtonMediaPlayer.TabIndex = 4;
            this.ButtonMediaPlayer.Text = "Send to Media Player";
            this.ButtonMediaPlayer.UseVisualStyleBackColor = true;
            this.ButtonMediaPlayer.Click += new System.EventHandler(this.ButtonMediaPlayer_Click);
            // 
            // buttonDisplayMarkers
            // 
            this.buttonDisplayMarkers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDisplayMarkers.Location = new System.Drawing.Point(274, 41);
            this.buttonDisplayMarkers.Name = "buttonDisplayMarkers";
            this.buttonDisplayMarkers.Size = new System.Drawing.Size(124, 23);
            this.buttonDisplayMarkers.TabIndex = 5;
            this.buttonDisplayMarkers.Text = "Display Markers";
            this.buttonDisplayMarkers.UseVisualStyleBackColor = true;
            this.buttonDisplayMarkers.Click += new System.EventHandler(this.ButtonDisplayMarkers_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(268, 340);
            this.propertyGrid1.TabIndex = 6;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // FormMusicData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 340);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.buttonDisplayMarkers);
            this.Controls.Add(this.ButtonMediaPlayer);
            this.HideOnClose = true;
            this.Name = "FormMusicData";
            this.TabText = "Music Data";
            this.Text = "Music Data";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button ButtonMediaPlayer;
        private System.Windows.Forms.Button buttonDisplayMarkers;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
    }
}