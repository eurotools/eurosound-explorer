
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMediaPlayer));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.userControl_WaveViewer1 = new EuroSoundExplorer2.CustomControls.UserControl_WaveViewer();
            this.userControl_WaveViewer2 = new EuroSoundExplorer2.CustomControls.UserControl_WaveViewer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ButtonSave = new System.Windows.Forms.ToolStripButton();
            this.ButtonAutoPlay = new System.Windows.Forms.ToolStripButton();
            this.ButtonPlay = new System.Windows.Forms.ToolStripButton();
            this.ButtonPause = new System.Windows.Forms.ToolStripButton();
            this.ButtonStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.labelCurrentTime = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.labelTotalTime = new System.Windows.Forms.ToolStripLabel();
            this.SaveFileDlg_SaveFile = new System.Windows.Forms.SaveFileDialog();
            this.trackBarPosition = new System.Windows.Forms.TrackBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPosition)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 30);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
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
            this.splitContainer1.Size = new System.Drawing.Size(486, 259);
            this.splitContainer1.SplitterDistance = 123;
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
            this.userControl_WaveViewer1.Size = new System.Drawing.Size(486, 123);
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
            this.userControl_WaveViewer2.Size = new System.Drawing.Size(486, 132);
            this.userControl_WaveViewer2.StartPosition = ((long)(0));
            this.userControl_WaveViewer2.TabIndex = 1;
            this.userControl_WaveViewer2.WaveStream = null;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonSave,
            this.ButtonAutoPlay,
            this.ButtonPlay,
            this.ButtonPause,
            this.ButtonStop,
            this.toolStripLabel1,
            this.labelCurrentTime,
            this.toolStripLabel3,
            this.labelTotalTime});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(486, 30);
            this.toolStrip1.TabIndex = 15;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ButtonSave
            // 
            this.ButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("ButtonSave.Image")));
            this.ButtonSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonSave.Name = "ButtonSave";
            this.ButtonSave.Size = new System.Drawing.Size(25, 27);
            this.ButtonSave.Text = "Save";
            this.ButtonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // ButtonAutoPlay
            // 
            this.ButtonAutoPlay.CheckOnClick = true;
            this.ButtonAutoPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonAutoPlay.Image = ((System.Drawing.Image)(resources.GetObject("ButtonAutoPlay.Image")));
            this.ButtonAutoPlay.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonAutoPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonAutoPlay.Name = "ButtonAutoPlay";
            this.ButtonAutoPlay.Size = new System.Drawing.Size(33, 27);
            this.ButtonAutoPlay.Text = "Auto Play";
            // 
            // ButtonPlay
            // 
            this.ButtonPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonPlay.Image = ((System.Drawing.Image)(resources.GetObject("ButtonPlay.Image")));
            this.ButtonPlay.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonPlay.Name = "ButtonPlay";
            this.ButtonPlay.Size = new System.Drawing.Size(23, 27);
            this.ButtonPlay.Text = "Play";
            this.ButtonPlay.Click += new System.EventHandler(this.ButtonPlay_Click);
            // 
            // ButtonPause
            // 
            this.ButtonPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonPause.Image = ((System.Drawing.Image)(resources.GetObject("ButtonPause.Image")));
            this.ButtonPause.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonPause.Name = "ButtonPause";
            this.ButtonPause.Size = new System.Drawing.Size(23, 27);
            this.ButtonPause.Text = "Pause";
            this.ButtonPause.Click += new System.EventHandler(this.ButtonPause_Click);
            // 
            // ButtonStop
            // 
            this.ButtonStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonStop.Image = ((System.Drawing.Image)(resources.GetObject("ButtonStop.Image")));
            this.ButtonStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.Size = new System.Drawing.Size(23, 27);
            this.ButtonStop.Text = "Stop";
            this.ButtonStop.Click += new System.EventHandler(this.ButtonStop_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(79, 27);
            this.toolStripLabel1.Text = "Current Time:";
            // 
            // labelCurrentTime
            // 
            this.labelCurrentTime.Name = "labelCurrentTime";
            this.labelCurrentTime.Size = new System.Drawing.Size(34, 27);
            this.labelCurrentTime.Text = "00:00";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(64, 27);
            this.toolStripLabel3.Text = "Total Time:";
            // 
            // labelTotalTime
            // 
            this.labelTotalTime.Name = "labelTotalTime";
            this.labelTotalTime.Size = new System.Drawing.Size(34, 27);
            this.labelTotalTime.Text = "00:00";
            // 
            // SaveFileDlg_SaveFile
            // 
            this.SaveFileDlg_SaveFile.FileName = "output.wav";
            this.SaveFileDlg_SaveFile.Filter = "Wave Audio File (*.wav)|*.wav";
            // 
            // trackBarPosition
            // 
            this.trackBarPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarPosition.LargeChange = 10;
            this.trackBarPosition.Location = new System.Drawing.Point(0, 292);
            this.trackBarPosition.Maximum = 100;
            this.trackBarPosition.Name = "trackBarPosition";
            this.trackBarPosition.Size = new System.Drawing.Size(486, 45);
            this.trackBarPosition.TabIndex = 17;
            this.trackBarPosition.TickFrequency = 2;
            this.trackBarPosition.Scroll += new System.EventHandler(this.TrackBarPosition_Scroll);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // FormMediaPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 327);
            this.Controls.Add(this.trackBarPosition);
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
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPosition)).EndInit();
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
        private System.Windows.Forms.ToolStripButton ButtonPause;
        private System.Windows.Forms.TrackBar trackBarPosition;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel labelCurrentTime;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripLabel labelTotalTime;
        private System.Windows.Forms.ToolStripButton ButtonAutoPlay;
    }
}