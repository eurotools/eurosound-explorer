namespace sb_explorer
{
    partial class FrmAbout
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnLatestRelease = new System.Windows.Forms.Button();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.grpAbout = new System.Windows.Forms.GroupBox();
            this.grpAbout.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(77, 22);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(162, 21);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "EuroSound Explorer";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(124, 61);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(42, 13);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "Version";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(83, 81);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(172, 13);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "Check the latest version on GitHub";
            // 
            // btnLatestRelease
            // 
            this.btnLatestRelease.Location = new System.Drawing.Point(110, 101);
            this.btnLatestRelease.Name = "btnLatestRelease";
            this.btnLatestRelease.Size = new System.Drawing.Size(96, 23);
            this.btnLatestRelease.TabIndex = 3;
            this.btnLatestRelease.Text = "Latest Release";
            this.btnLatestRelease.UseVisualStyleBackColor = true;
            this.btnLatestRelease.Click += new System.EventHandler(this.btnLatestRelease_Click);
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Location = new System.Drawing.Point(106, 150);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(94, 13);
            this.lblCopyright.TabIndex = 4;
            this.lblCopyright.Text = "© EuroTools 2026";
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(121, 185);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // grpAbout
            // 
            this.grpAbout.Controls.Add(this.lblTitle);
            this.grpAbout.Controls.Add(this.lblVersion);
            this.grpAbout.Controls.Add(this.lblDescription);
            this.grpAbout.Controls.Add(this.btnLatestRelease);
            this.grpAbout.Controls.Add(this.lblCopyright);
            this.grpAbout.Controls.Add(this.btnOk);
            this.grpAbout.Location = new System.Drawing.Point(13, 12);
            this.grpAbout.Name = "grpAbout";
            this.grpAbout.Size = new System.Drawing.Size(316, 226);
            this.grpAbout.TabIndex = 0;
            this.grpAbout.TabStop = false;
            // 
            // FrmAbout
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOk;
            this.ClientSize = new System.Drawing.Size(342, 251);
            this.Controls.Add(this.grpAbout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAbout";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EuroSound Help";
            this.Load += new System.EventHandler(this.FrmAbout_Load);
            this.grpAbout.ResumeLayout(false);
            this.grpAbout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnLatestRelease;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox grpAbout;
    }
}
