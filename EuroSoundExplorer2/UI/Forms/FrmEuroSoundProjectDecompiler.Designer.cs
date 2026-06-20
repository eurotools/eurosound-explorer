namespace sb_explorer
{
    partial class FrmEuroSoundProjectDecompiler
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
            this.lblCompiledFolder = new System.Windows.Forms.Label();
            this.txtCompiledFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseCompiledFolder = new System.Windows.Forms.Button();
            this.lblMode = new System.Windows.Forms.Label();
            this.cbxMode = new System.Windows.Forms.ComboBox();
            this.lblProjectFolder = new System.Windows.Forms.Label();
            this.txtProjectFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseProjectFolder = new System.Windows.Forms.Button();
            this.lblOutputFolder = new System.Windows.Forms.Label();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseOutputFolder = new System.Windows.Forms.Button();
            this.grpExport = new System.Windows.Forms.GroupBox();
            this.chkExportMemoryMaps = new System.Windows.Forms.CheckBox();
            this.chkRewriteSamplesOnly = new System.Windows.Forms.CheckBox();
            this.chkExportPlatformSamplePools = new System.Windows.Forms.CheckBox();
            this.chkExportDuckerGroups = new System.Windows.Forms.CheckBox();
            this.chkExportSfx = new System.Windows.Forms.CheckBox();
            this.chkExportGroups = new System.Windows.Forms.CheckBox();
            this.chkExportSoundBanks = new System.Windows.Forms.CheckBox();
            this.grpReplaceSections = new System.Windows.Forms.GroupBox();
            this.chkSfxSamplePoolFiles = new System.Windows.Forms.CheckBox();
            this.chkDuckerParameters = new System.Windows.Forms.CheckBox();
            this.chkDuckerDependencies = new System.Windows.Forms.CheckBox();
            this.chkSoundBankDependencies = new System.Windows.Forms.CheckBox();
            this.chkGroupParameters = new System.Windows.Forms.CheckBox();
            this.chkGroupDependencies = new System.Windows.Forms.CheckBox();
            this.chkSfxSamplePoolControl = new System.Windows.Forms.CheckBox();
            this.chkSfxSamplePoolModes = new System.Windows.Forms.CheckBox();
            this.chkSfxParameters = new System.Windows.Forms.CheckBox();
            this.btnDecompile = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpExport.SuspendLayout();
            this.grpReplaceSections.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCompiledFolder
            // 
            this.lblCompiledFolder.AutoSize = true;
            this.lblCompiledFolder.Location = new System.Drawing.Point(12, 17);
            this.lblCompiledFolder.Name = "lblCompiledFolder";
            this.lblCompiledFolder.Size = new System.Drawing.Size(77, 13);
            this.lblCompiledFolder.TabIndex = 0;
            this.lblCompiledFolder.Text = "Compiled files";
            // 
            // txtCompiledFolder
            // 
            this.txtCompiledFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCompiledFolder.Location = new System.Drawing.Point(118, 14);
            this.txtCompiledFolder.Name = "txtCompiledFolder";
            this.txtCompiledFolder.Size = new System.Drawing.Size(352, 20);
            this.txtCompiledFolder.TabIndex = 1;
            // 
            // btnBrowseCompiledFolder
            // 
            this.btnBrowseCompiledFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseCompiledFolder.Location = new System.Drawing.Point(476, 12);
            this.btnBrowseCompiledFolder.Name = "btnBrowseCompiledFolder";
            this.btnBrowseCompiledFolder.Size = new System.Drawing.Size(32, 23);
            this.btnBrowseCompiledFolder.TabIndex = 2;
            this.btnBrowseCompiledFolder.Text = "...";
            this.btnBrowseCompiledFolder.UseVisualStyleBackColor = true;
            this.btnBrowseCompiledFolder.Click += new System.EventHandler(this.BtnBrowseCompiledFolder_Click);
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Location = new System.Drawing.Point(12, 47);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(34, 13);
            this.lblMode.TabIndex = 3;
            this.lblMode.Text = "Mode";
            // 
            // cbxMode
            // 
            this.cbxMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxMode.FormattingEnabled = true;
            this.cbxMode.Items.AddRange(new object[] {
            "Create project files",
            "Replace sections in EuroSound project"});
            this.cbxMode.Location = new System.Drawing.Point(118, 44);
            this.cbxMode.Name = "cbxMode";
            this.cbxMode.Size = new System.Drawing.Size(230, 21);
            this.cbxMode.TabIndex = 4;
            this.cbxMode.SelectedIndexChanged += new System.EventHandler(this.CbxMode_SelectedIndexChanged);
            // 
            // lblProjectFolder
            // 
            this.lblProjectFolder.AutoSize = true;
            this.lblProjectFolder.Location = new System.Drawing.Point(12, 77);
            this.lblProjectFolder.Name = "lblProjectFolder";
            this.lblProjectFolder.Size = new System.Drawing.Size(96, 13);
            this.lblProjectFolder.TabIndex = 5;
            this.lblProjectFolder.Text = "EuroSound project";
            // 
            // txtProjectFolder
            // 
            this.txtProjectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProjectFolder.Location = new System.Drawing.Point(118, 74);
            this.txtProjectFolder.Name = "txtProjectFolder";
            this.txtProjectFolder.Size = new System.Drawing.Size(352, 20);
            this.txtProjectFolder.TabIndex = 6;
            // 
            // btnBrowseProjectFolder
            // 
            this.btnBrowseProjectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseProjectFolder.Location = new System.Drawing.Point(476, 72);
            this.btnBrowseProjectFolder.Name = "btnBrowseProjectFolder";
            this.btnBrowseProjectFolder.Size = new System.Drawing.Size(32, 23);
            this.btnBrowseProjectFolder.TabIndex = 7;
            this.btnBrowseProjectFolder.Text = "...";
            this.btnBrowseProjectFolder.UseVisualStyleBackColor = true;
            this.btnBrowseProjectFolder.Click += new System.EventHandler(this.BtnBrowseProjectFolder_Click);
            // 
            // lblOutputFolder
            // 
            this.lblOutputFolder.AutoSize = true;
            this.lblOutputFolder.Location = new System.Drawing.Point(12, 107);
            this.lblOutputFolder.Name = "lblOutputFolder";
            this.lblOutputFolder.Size = new System.Drawing.Size(69, 13);
            this.lblOutputFolder.TabIndex = 8;
            this.lblOutputFolder.Text = "Output folder";
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFolder.Location = new System.Drawing.Point(118, 104);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(352, 20);
            this.txtOutputFolder.TabIndex = 9;
            // 
            // btnBrowseOutputFolder
            // 
            this.btnBrowseOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseOutputFolder.Location = new System.Drawing.Point(476, 102);
            this.btnBrowseOutputFolder.Name = "btnBrowseOutputFolder";
            this.btnBrowseOutputFolder.Size = new System.Drawing.Size(32, 23);
            this.btnBrowseOutputFolder.TabIndex = 10;
            this.btnBrowseOutputFolder.Text = "...";
            this.btnBrowseOutputFolder.UseVisualStyleBackColor = true;
            this.btnBrowseOutputFolder.Click += new System.EventHandler(this.BtnBrowseOutputFolder_Click);
            // 
            // grpExport
            // 
            this.grpExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpExport.Controls.Add(this.chkExportMemoryMaps);
            this.grpExport.Controls.Add(this.chkRewriteSamplesOnly);
            this.grpExport.Controls.Add(this.chkExportPlatformSamplePools);
            this.grpExport.Controls.Add(this.chkExportDuckerGroups);
            this.grpExport.Controls.Add(this.chkExportSfx);
            this.grpExport.Controls.Add(this.chkExportGroups);
            this.grpExport.Controls.Add(this.chkExportSoundBanks);
            this.grpExport.Location = new System.Drawing.Point(15, 140);
            this.grpExport.Name = "grpExport";
            this.grpExport.Size = new System.Drawing.Size(493, 121);
            this.grpExport.TabIndex = 11;
            this.grpExport.TabStop = false;
            this.grpExport.Text = "Export";
            // 
            // chkExportMemoryMaps
            // 
            this.chkExportMemoryMaps.AutoSize = true;
            this.chkExportMemoryMaps.Checked = true;
            this.chkExportMemoryMaps.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExportMemoryMaps.Location = new System.Drawing.Point(276, 69);
            this.chkExportMemoryMaps.Name = "chkExportMemoryMaps";
            this.chkExportMemoryMaps.Size = new System.Drawing.Size(92, 17);
            this.chkExportMemoryMaps.TabIndex = 6;
            this.chkExportMemoryMaps.Text = "Memory Maps";
            this.chkExportMemoryMaps.UseVisualStyleBackColor = true;
            // 
            // chkRewriteSamplesOnly
            // 
            this.chkRewriteSamplesOnly.AutoSize = true;
            this.chkRewriteSamplesOnly.Location = new System.Drawing.Point(16, 69);
            this.chkRewriteSamplesOnly.Name = "chkRewriteSamplesOnly";
            this.chkRewriteSamplesOnly.Size = new System.Drawing.Size(151, 17);
            this.chkRewriteSamplesOnly.TabIndex = 5;
            this.chkRewriteSamplesOnly.Text = "Only rebuild Samples.txt";
            this.chkRewriteSamplesOnly.UseVisualStyleBackColor = true;
            // 
            // chkExportPlatformSamplePools
            // 
            this.chkExportPlatformSamplePools.AutoSize = true;
            this.chkExportPlatformSamplePools.Location = new System.Drawing.Point(276, 46);
            this.chkExportPlatformSamplePools.Name = "chkExportPlatformSamplePools";
            this.chkExportPlatformSamplePools.Size = new System.Drawing.Size(166, 17);
            this.chkExportPlatformSamplePools.TabIndex = 4;
            this.chkExportPlatformSamplePools.Text = "Platform sample pool variants";
            this.chkExportPlatformSamplePools.UseVisualStyleBackColor = true;
            // 
            // chkExportDuckerGroups
            // 
            this.chkExportDuckerGroups.AutoSize = true;
            this.chkExportDuckerGroups.Checked = true;
            this.chkExportDuckerGroups.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExportDuckerGroups.Location = new System.Drawing.Point(153, 46);
            this.chkExportDuckerGroups.Name = "chkExportDuckerGroups";
            this.chkExportDuckerGroups.Size = new System.Drawing.Size(97, 17);
            this.chkExportDuckerGroups.TabIndex = 3;
            this.chkExportDuckerGroups.Text = "Ducker Groups";
            this.chkExportDuckerGroups.UseVisualStyleBackColor = true;
            // 
            // chkExportSfx
            // 
            this.chkExportSfx.AutoSize = true;
            this.chkExportSfx.Checked = true;
            this.chkExportSfx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExportSfx.Location = new System.Drawing.Point(16, 46);
            this.chkExportSfx.Name = "chkExportSfx";
            this.chkExportSfx.Size = new System.Drawing.Size(48, 17);
            this.chkExportSfx.TabIndex = 2;
            this.chkExportSfx.Text = "SFXs";
            this.chkExportSfx.UseVisualStyleBackColor = true;
            // 
            // chkExportGroups
            // 
            this.chkExportGroups.AutoSize = true;
            this.chkExportGroups.Checked = true;
            this.chkExportGroups.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExportGroups.Location = new System.Drawing.Point(153, 23);
            this.chkExportGroups.Name = "chkExportGroups";
            this.chkExportGroups.Size = new System.Drawing.Size(60, 17);
            this.chkExportGroups.TabIndex = 1;
            this.chkExportGroups.Text = "Groups";
            this.chkExportGroups.UseVisualStyleBackColor = true;
            // 
            // chkExportSoundBanks
            // 
            this.chkExportSoundBanks.AutoSize = true;
            this.chkExportSoundBanks.Checked = true;
            this.chkExportSoundBanks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExportSoundBanks.Location = new System.Drawing.Point(16, 23);
            this.chkExportSoundBanks.Name = "chkExportSoundBanks";
            this.chkExportSoundBanks.Size = new System.Drawing.Size(87, 17);
            this.chkExportSoundBanks.TabIndex = 0;
            this.chkExportSoundBanks.Text = "SoundBanks";
            this.chkExportSoundBanks.UseVisualStyleBackColor = true;
            // 
            // grpReplaceSections
            // 
            this.grpReplaceSections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpReplaceSections.Controls.Add(this.chkSfxSamplePoolFiles);
            this.grpReplaceSections.Controls.Add(this.chkDuckerParameters);
            this.grpReplaceSections.Controls.Add(this.chkDuckerDependencies);
            this.grpReplaceSections.Controls.Add(this.chkSoundBankDependencies);
            this.grpReplaceSections.Controls.Add(this.chkGroupParameters);
            this.grpReplaceSections.Controls.Add(this.chkGroupDependencies);
            this.grpReplaceSections.Controls.Add(this.chkSfxSamplePoolControl);
            this.grpReplaceSections.Controls.Add(this.chkSfxSamplePoolModes);
            this.grpReplaceSections.Controls.Add(this.chkSfxParameters);
            this.grpReplaceSections.Location = new System.Drawing.Point(15, 274);
            this.grpReplaceSections.Name = "grpReplaceSections";
            this.grpReplaceSections.Size = new System.Drawing.Size(493, 139);
            this.grpReplaceSections.TabIndex = 12;
            this.grpReplaceSections.TabStop = false;
            this.grpReplaceSections.Text = "Replace sections";
            // 
            // chkSfxSamplePoolFiles
            // 
            this.chkSfxSamplePoolFiles.AutoSize = true;
            this.chkSfxSamplePoolFiles.Checked = true;
            this.chkSfxSamplePoolFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSfxSamplePoolFiles.Location = new System.Drawing.Point(16, 46);
            this.chkSfxSamplePoolFiles.Name = "chkSfxSamplePoolFiles";
            this.chkSfxSamplePoolFiles.Size = new System.Drawing.Size(126, 17);
            this.chkSfxSamplePoolFiles.TabIndex = 8;
            this.chkSfxSamplePoolFiles.Text = "SFX sample pool files";
            this.chkSfxSamplePoolFiles.UseVisualStyleBackColor = true;
            // 
            // chkDuckerParameters
            // 
            this.chkDuckerParameters.AutoSize = true;
            this.chkDuckerParameters.Checked = true;
            this.chkDuckerParameters.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDuckerParameters.Location = new System.Drawing.Point(244, 115);
            this.chkDuckerParameters.Name = "chkDuckerParameters";
            this.chkDuckerParameters.Size = new System.Drawing.Size(124, 17);
            this.chkDuckerParameters.TabIndex = 7;
            this.chkDuckerParameters.Text = "Ducker parameters";
            this.chkDuckerParameters.UseVisualStyleBackColor = true;
            // 
            // chkDuckerDependencies
            // 
            this.chkDuckerDependencies.AutoSize = true;
            this.chkDuckerDependencies.Checked = true;
            this.chkDuckerDependencies.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDuckerDependencies.Location = new System.Drawing.Point(16, 115);
            this.chkDuckerDependencies.Name = "chkDuckerDependencies";
            this.chkDuckerDependencies.Size = new System.Drawing.Size(137, 17);
            this.chkDuckerDependencies.TabIndex = 6;
            this.chkDuckerDependencies.Text = "Ducker dependencies";
            this.chkDuckerDependencies.UseVisualStyleBackColor = true;
            // 
            // chkSoundBankDependencies
            // 
            this.chkSoundBankDependencies.AutoSize = true;
            this.chkSoundBankDependencies.Checked = true;
            this.chkSoundBankDependencies.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSoundBankDependencies.Location = new System.Drawing.Point(244, 92);
            this.chkSoundBankDependencies.Name = "chkSoundBankDependencies";
            this.chkSoundBankDependencies.Size = new System.Drawing.Size(154, 17);
            this.chkSoundBankDependencies.TabIndex = 5;
            this.chkSoundBankDependencies.Text = "SoundBank dependencies";
            this.chkSoundBankDependencies.UseVisualStyleBackColor = true;
            // 
            // chkGroupParameters
            // 
            this.chkGroupParameters.AutoSize = true;
            this.chkGroupParameters.Checked = true;
            this.chkGroupParameters.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGroupParameters.Location = new System.Drawing.Point(244, 69);
            this.chkGroupParameters.Name = "chkGroupParameters";
            this.chkGroupParameters.Size = new System.Drawing.Size(111, 17);
            this.chkGroupParameters.TabIndex = 4;
            this.chkGroupParameters.Text = "Group parameters";
            this.chkGroupParameters.UseVisualStyleBackColor = true;
            // 
            // chkGroupDependencies
            // 
            this.chkGroupDependencies.AutoSize = true;
            this.chkGroupDependencies.Checked = true;
            this.chkGroupDependencies.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGroupDependencies.Location = new System.Drawing.Point(244, 23);
            this.chkGroupDependencies.Name = "chkGroupDependencies";
            this.chkGroupDependencies.Size = new System.Drawing.Size(124, 17);
            this.chkGroupDependencies.TabIndex = 3;
            this.chkGroupDependencies.Text = "Group dependencies";
            this.chkGroupDependencies.UseVisualStyleBackColor = true;
            // 
            // chkSfxSamplePoolControl
            // 
            this.chkSfxSamplePoolControl.AutoSize = true;
            this.chkSfxSamplePoolControl.Checked = true;
            this.chkSfxSamplePoolControl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSfxSamplePoolControl.Location = new System.Drawing.Point(16, 92);
            this.chkSfxSamplePoolControl.Name = "chkSfxSamplePoolControl";
            this.chkSfxSamplePoolControl.Size = new System.Drawing.Size(141, 17);
            this.chkSfxSamplePoolControl.TabIndex = 2;
            this.chkSfxSamplePoolControl.Text = "SFX sample pool control";
            this.chkSfxSamplePoolControl.UseVisualStyleBackColor = true;
            // 
            // chkSfxSamplePoolModes
            // 
            this.chkSfxSamplePoolModes.AutoSize = true;
            this.chkSfxSamplePoolModes.Checked = true;
            this.chkSfxSamplePoolModes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSfxSamplePoolModes.Location = new System.Drawing.Point(16, 69);
            this.chkSfxSamplePoolModes.Name = "chkSfxSamplePoolModes";
            this.chkSfxSamplePoolModes.Size = new System.Drawing.Size(137, 17);
            this.chkSfxSamplePoolModes.TabIndex = 1;
            this.chkSfxSamplePoolModes.Text = "SFX sample pool modes";
            this.chkSfxSamplePoolModes.UseVisualStyleBackColor = true;
            // 
            // chkSfxParameters
            // 
            this.chkSfxParameters.AutoSize = true;
            this.chkSfxParameters.Checked = true;
            this.chkSfxParameters.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSfxParameters.Location = new System.Drawing.Point(16, 23);
            this.chkSfxParameters.Name = "chkSfxParameters";
            this.chkSfxParameters.Size = new System.Drawing.Size(101, 17);
            this.chkSfxParameters.TabIndex = 0;
            this.chkSfxParameters.Text = "SFX parameters";
            this.chkSfxParameters.UseVisualStyleBackColor = true;
            // 
            // btnDecompile
            // 
            this.btnDecompile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDecompile.Location = new System.Drawing.Point(15, 432);
            this.btnDecompile.Name = "btnDecompile";
            this.btnDecompile.Size = new System.Drawing.Size(75, 23);
            this.btnDecompile.TabIndex = 13;
            this.btnDecompile.Text = "Decompile";
            this.btnDecompile.UseVisualStyleBackColor = true;
            this.btnDecompile.Click += new System.EventHandler(this.BtnDecompile_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(433, 432);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmEuroSoundProjectDecompiler
            // 
            this.AcceptButton = this.btnDecompile;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(520, 467);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDecompile);
            this.Controls.Add(this.grpReplaceSections);
            this.Controls.Add(this.grpExport);
            this.Controls.Add(this.btnBrowseOutputFolder);
            this.Controls.Add(this.txtOutputFolder);
            this.Controls.Add(this.lblOutputFolder);
            this.Controls.Add(this.btnBrowseProjectFolder);
            this.Controls.Add(this.txtProjectFolder);
            this.Controls.Add(this.lblProjectFolder);
            this.Controls.Add(this.cbxMode);
            this.Controls.Add(this.lblMode);
            this.Controls.Add(this.btnBrowseCompiledFolder);
            this.Controls.Add(this.txtCompiledFolder);
            this.Controls.Add(this.lblCompiledFolder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmEuroSoundProjectDecompiler";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Decompile EuroSound Project";
            this.grpExport.ResumeLayout(false);
            this.grpExport.PerformLayout();
            this.grpReplaceSections.ResumeLayout(false);
            this.grpReplaceSections.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblCompiledFolder;
        private System.Windows.Forms.TextBox txtCompiledFolder;
        private System.Windows.Forms.Button btnBrowseCompiledFolder;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.ComboBox cbxMode;
        private System.Windows.Forms.Label lblProjectFolder;
        private System.Windows.Forms.TextBox txtProjectFolder;
        private System.Windows.Forms.Button btnBrowseProjectFolder;
        private System.Windows.Forms.Label lblOutputFolder;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.Button btnBrowseOutputFolder;
        private System.Windows.Forms.GroupBox grpExport;
        private System.Windows.Forms.CheckBox chkExportMemoryMaps;
        private System.Windows.Forms.CheckBox chkRewriteSamplesOnly;
        private System.Windows.Forms.CheckBox chkExportPlatformSamplePools;
        private System.Windows.Forms.CheckBox chkExportDuckerGroups;
        private System.Windows.Forms.CheckBox chkExportSfx;
        private System.Windows.Forms.CheckBox chkExportGroups;
        private System.Windows.Forms.CheckBox chkExportSoundBanks;
        private System.Windows.Forms.GroupBox grpReplaceSections;
        private System.Windows.Forms.CheckBox chkSfxSamplePoolFiles;
        private System.Windows.Forms.CheckBox chkDuckerParameters;
        private System.Windows.Forms.CheckBox chkDuckerDependencies;
        private System.Windows.Forms.CheckBox chkSoundBankDependencies;
        private System.Windows.Forms.CheckBox chkGroupParameters;
        private System.Windows.Forms.CheckBox chkGroupDependencies;
        private System.Windows.Forms.CheckBox chkSfxSamplePoolControl;
        private System.Windows.Forms.CheckBox chkSfxSamplePoolModes;
        private System.Windows.Forms.CheckBox chkSfxParameters;
        private System.Windows.Forms.Button btnDecompile;
        private System.Windows.Forms.Button btnCancel;
    }
}
