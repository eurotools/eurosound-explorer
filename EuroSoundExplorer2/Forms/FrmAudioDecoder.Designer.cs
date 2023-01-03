
namespace EuroSoundExplorer2
{
    partial class FrmAudioDecoder
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
            this.btnBrowseFiles = new System.Windows.Forms.Button();
            this.lbxFormats = new System.Windows.Forms.ListBox();
            this.lblFrequency = new System.Windows.Forms.Label();
            this.nudFrequency = new System.Windows.Forms.NumericUpDown();
            this.nudChannels = new System.Windows.Forms.NumericUpDown();
            this.lblChannels = new System.Windows.Forms.Label();
            this.nudHeaderBytes = new System.Windows.Forms.NumericUpDown();
            this.lblHeaderBytes = new System.Windows.Forms.Label();
            this.btnConvert = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.nudFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudChannels)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeaderBytes)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBrowseFiles
            // 
            this.btnBrowseFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseFiles.Location = new System.Drawing.Point(12, 12);
            this.btnBrowseFiles.Name = "btnBrowseFiles";
            this.btnBrowseFiles.Size = new System.Drawing.Size(191, 23);
            this.btnBrowseFiles.TabIndex = 0;
            this.btnBrowseFiles.Text = "Browse for Files";
            this.btnBrowseFiles.UseVisualStyleBackColor = true;
            this.btnBrowseFiles.Click += new System.EventHandler(this.BtnBrowseFiles_Click);
            // 
            // lbxFormats
            // 
            this.lbxFormats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbxFormats.FormattingEnabled = true;
            this.lbxFormats.Items.AddRange(new object[] {
            "Sony ADPCM",
            "Software ADPCM",
            "Nintendo ADPCM",
            "Eurocom ADPCM",
            "Xbox ADPCM"});
            this.lbxFormats.Location = new System.Drawing.Point(12, 41);
            this.lbxFormats.Name = "lbxFormats";
            this.lbxFormats.Size = new System.Drawing.Size(191, 69);
            this.lbxFormats.TabIndex = 1;
            // 
            // lblFrequency
            // 
            this.lblFrequency.AutoSize = true;
            this.lblFrequency.Location = new System.Drawing.Point(12, 118);
            this.lblFrequency.Name = "lblFrequency";
            this.lblFrequency.Size = new System.Drawing.Size(57, 13);
            this.lblFrequency.TabIndex = 2;
            this.lblFrequency.Text = "Frequency";
            // 
            // nudFrequency
            // 
            this.nudFrequency.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nudFrequency.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudFrequency.Location = new System.Drawing.Point(89, 116);
            this.nudFrequency.Maximum = new decimal(new int[] {
            44100,
            0,
            0,
            0});
            this.nudFrequency.Minimum = new decimal(new int[] {
            11025,
            0,
            0,
            0});
            this.nudFrequency.Name = "nudFrequency";
            this.nudFrequency.Size = new System.Drawing.Size(77, 20);
            this.nudFrequency.TabIndex = 3;
            this.nudFrequency.Value = new decimal(new int[] {
            22050,
            0,
            0,
            0});
            // 
            // nudChannels
            // 
            this.nudChannels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nudChannels.Enabled = false;
            this.nudChannels.Location = new System.Drawing.Point(89, 142);
            this.nudChannels.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudChannels.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudChannels.Name = "nudChannels";
            this.nudChannels.Size = new System.Drawing.Size(77, 20);
            this.nudChannels.TabIndex = 5;
            this.nudChannels.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblChannels
            // 
            this.lblChannels.AutoSize = true;
            this.lblChannels.Location = new System.Drawing.Point(12, 144);
            this.lblChannels.Name = "lblChannels";
            this.lblChannels.Size = new System.Drawing.Size(51, 13);
            this.lblChannels.TabIndex = 4;
            this.lblChannels.Text = "Channels";
            // 
            // nudHeaderBytes
            // 
            this.nudHeaderBytes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nudHeaderBytes.Location = new System.Drawing.Point(89, 168);
            this.nudHeaderBytes.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.nudHeaderBytes.Name = "nudHeaderBytes";
            this.nudHeaderBytes.Size = new System.Drawing.Size(77, 20);
            this.nudHeaderBytes.TabIndex = 7;
            // 
            // lblHeaderBytes
            // 
            this.lblHeaderBytes.AutoSize = true;
            this.lblHeaderBytes.Location = new System.Drawing.Point(12, 170);
            this.lblHeaderBytes.Name = "lblHeaderBytes";
            this.lblHeaderBytes.Size = new System.Drawing.Size(71, 13);
            this.lblHeaderBytes.TabIndex = 6;
            this.lblHeaderBytes.Text = "Header Bytes";
            // 
            // btnConvert
            // 
            this.btnConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnConvert.Enabled = false;
            this.btnConvert.Location = new System.Drawing.Point(12, 207);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 8;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.BtnConvert_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(128, 207);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Multiselect = true;
            // 
            // FrmAudioDecoder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(215, 242);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.nudHeaderBytes);
            this.Controls.Add(this.lblHeaderBytes);
            this.Controls.Add(this.nudChannels);
            this.Controls.Add(this.lblChannels);
            this.Controls.Add(this.nudFrequency);
            this.Controls.Add(this.lblFrequency);
            this.Controls.Add(this.lbxFormats);
            this.Controls.Add(this.btnBrowseFiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAudioDecoder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Decode Audio File";
            ((System.ComponentModel.ISupportInitialize)(this.nudFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudChannels)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeaderBytes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowseFiles;
        private System.Windows.Forms.ListBox lbxFormats;
        private System.Windows.Forms.Label lblFrequency;
        private System.Windows.Forms.NumericUpDown nudFrequency;
        private System.Windows.Forms.NumericUpDown nudChannels;
        private System.Windows.Forms.Label lblChannels;
        private System.Windows.Forms.NumericUpDown nudHeaderBytes;
        private System.Windows.Forms.Label lblHeaderBytes;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}