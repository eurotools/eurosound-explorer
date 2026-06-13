namespace sb_explorer
{
    partial class FrmSplash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSplash));
            this.picSplashImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picSplashImage)).BeginInit();
            this.SuspendLayout();
            // 
            // picSplashImage
            // 
            this.picSplashImage.Image = ((System.Drawing.Image)(resources.GetObject("picSplashImage.Image")));
            this.picSplashImage.Location = new System.Drawing.Point(0, 0);
            this.picSplashImage.Name = "picSplashImage";
            this.picSplashImage.Size = new System.Drawing.Size(680, 520);
            this.picSplashImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSplashImage.TabIndex = 0;
            this.picSplashImage.TabStop = false;
            // 
            // FrmSplash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 520);
            this.ControlBox = false;
            this.Controls.Add(this.picSplashImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmSplash";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EuroSound Explorer";
            ((System.ComponentModel.ISupportInitialize)(this.picSplashImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox picSplashImage;
    }
}
