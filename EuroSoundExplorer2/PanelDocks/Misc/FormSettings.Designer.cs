
namespace EuroSoundExplorer2
{
    partial class FormSettings
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
            this.PropGridSettings = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // PropGridSettings
            // 
            this.PropGridSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropGridSettings.HelpVisible = false;
            this.PropGridSettings.Location = new System.Drawing.Point(0, 0);
            this.PropGridSettings.Name = "PropGridSettings";
            this.PropGridSettings.Size = new System.Drawing.Size(456, 307);
            this.PropGridSettings.TabIndex = 0;
            this.PropGridSettings.ToolbarVisible = false;
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 307);
            this.Controls.Add(this.PropGridSettings);
            this.HideOnClose = true;
            this.Name = "FormSettings";
            this.TabText = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid PropGridSettings;
    }
}