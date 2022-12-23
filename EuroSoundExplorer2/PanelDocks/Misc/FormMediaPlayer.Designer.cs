
namespace EuroSoundExplorer2
{
    partial class FormMediaPlayer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMediaPlayer));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.userControl_WaveViewer1 = new EuroSoundExplorer2.CustomControls.UserControl_WaveViewer();
            this.userControl_WaveViewer2 = new EuroSoundExplorer2.CustomControls.UserControl_WaveViewer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ButtonSave = new System.Windows.Forms.ToolStripButton();
            this.ButtonPlay = new System.Windows.Forms.ToolStripButton();
            this.ButtonStop = new System.Windows.Forms.ToolStripButton();
            this.SaveFileDlg_SaveFile = new System.Windows.Forms.SaveFileDialog();
            this.chbxAutoPlay = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.userControl_WaveViewer1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.userControl_WaveViewer2);
            this.splitContainer1.Size = new System.Drawing.Size(565, 260);
            this.splitContainer1.SplitterDistance = 125;
            this.splitContainer1.TabIndex = 14;
            // 
            // userControl_WaveViewer1
            // 
            this.userControl_WaveViewer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.userControl_WaveViewer1.CurrentWaveImage = null;
            this.userControl_WaveViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControl_WaveViewer1.Location = new System.Drawing.Point(0, 0);
            this.userControl_WaveViewer1.MaximumSize = new System.Drawing.Size(0, 150);
            this.userControl_WaveViewer1.Name = "userControl_WaveViewer1";
            this.userControl_WaveViewer1.PenWidth = 1F;
            this.userControl_WaveViewer1.SamplesPerPixel = 128;
            this.userControl_WaveViewer1.Size = new System.Drawing.Size(565, 125);
            this.userControl_WaveViewer1.StartPosition = ((long)(0));
            this.userControl_WaveViewer1.TabIndex = 0;
            this.userControl_WaveViewer1.WaveStream = null;
            // 
            // userControl_WaveViewer2
            // 
            this.userControl_WaveViewer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.userControl_WaveViewer2.CurrentWaveImage = null;
            this.userControl_WaveViewer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControl_WaveViewer2.Location = new System.Drawing.Point(0, 0);
            this.userControl_WaveViewer2.MaximumSize = new System.Drawing.Size(0, 150);
            this.userControl_WaveViewer2.Name = "userControl_WaveViewer2";
            this.userControl_WaveViewer2.PenWidth = 1F;
            this.userControl_WaveViewer2.SamplesPerPixel = 128;
            this.userControl_WaveViewer2.Size = new System.Drawing.Size(565, 131);
            this.userControl_WaveViewer2.StartPosition = ((long)(0));
            this.userControl_WaveViewer2.TabIndex = 1;
            this.userControl_WaveViewer2.WaveStream = null;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonSave,
            this.ButtonPlay,
            this.ButtonStop});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(565, 25);
            this.toolStrip1.TabIndex = 15;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ButtonSave
            // 
            this.ButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("ButtonSave.Image")));
            this.ButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonSave.Name = "ButtonSave";
            this.ButtonSave.Size = new System.Drawing.Size(35, 22);
            this.ButtonSave.Text = "Save";
            this.ButtonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // ButtonPlay
            // 
            this.ButtonPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ButtonPlay.Image = ((System.Drawing.Image)(resources.GetObject("ButtonPlay.Image")));
            this.ButtonPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonPlay.Name = "ButtonPlay";
            this.ButtonPlay.Size = new System.Drawing.Size(33, 22);
            this.ButtonPlay.Text = "Play";
            this.ButtonPlay.Click += new System.EventHandler(this.ButtonPlay_Click);
            // 
            // ButtonStop
            // 
            this.ButtonStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ButtonStop.Image = ((System.Drawing.Image)(resources.GetObject("ButtonStop.Image")));
            this.ButtonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.Size = new System.Drawing.Size(35, 22);
            this.ButtonStop.Text = "Stop";
            this.ButtonStop.Click += new System.EventHandler(this.ButtonStop_Click);
            // 
            // SaveFileDlg_SaveFile
            // 
            this.SaveFileDlg_SaveFile.FileName = "output.wav";
            this.SaveFileDlg_SaveFile.Filter = "Wave Audio File (*.wav)|*.wav";
            // 
            // chbxAutoPlay
            // 
            this.chbxAutoPlay.AutoSize = true;
            this.chbxAutoPlay.Location = new System.Drawing.Point(101, 5);
            this.chbxAutoPlay.Name = "chbxAutoPlay";
            this.chbxAutoPlay.Size = new System.Drawing.Size(130, 17);
            this.chbxAutoPlay.TabIndex = 16;
            this.chbxAutoPlay.Text = "Auto-Play When Load";
            this.chbxAutoPlay.UseVisualStyleBackColor = true;
            // 
            // FormMediaPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 288);
            this.Controls.Add(this.chbxAutoPlay);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.splitContainer1);
            this.HideOnClose = true;
            this.MaximumSize = new System.Drawing.Size(1200, 500);
            this.Name = "FormMediaPlayer";
            this.TabText = "Media Player";
            this.Text = "Media Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMediaPlayer_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomControls.UserControl_WaveViewer userControl_WaveViewer1;
        private CustomControls.UserControl_WaveViewer userControl_WaveViewer2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ButtonSave;
        private System.Windows.Forms.ToolStripButton ButtonPlay;
        private System.Windows.Forms.ToolStripButton ButtonStop;
        private System.Windows.Forms.SaveFileDialog SaveFileDlg_SaveFile;
        private System.Windows.Forms.CheckBox chbxAutoPlay;
    }
}