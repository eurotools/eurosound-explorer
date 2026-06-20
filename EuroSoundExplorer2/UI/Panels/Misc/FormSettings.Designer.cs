
namespace sb_explorer
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
            this.tabControlSettings = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.tabPageCodecs = new System.Windows.Forms.TabPage();
            this.gridCodecMatrix = new System.Windows.Forms.DataGridView();
            this.ColumnVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPlatform = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSoundBank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStreamBank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMusicBank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControlSettings.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tabPageCodecs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCodecMatrix)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlSettings
            // 
            this.tabControlSettings.Controls.Add(this.tabPageGeneral);
            this.tabControlSettings.Controls.Add(this.tabPageCodecs);
            this.tabControlSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlSettings.Location = new System.Drawing.Point(0, 0);
            this.tabControlSettings.Name = "tabControlSettings";
            this.tabControlSettings.SelectedIndex = 0;
            this.tabControlSettings.Size = new System.Drawing.Size(456, 307);
            this.tabControlSettings.TabIndex = 0;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.PropGridSettings);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(448, 281);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // tabPageCodecs
            // 
            this.tabPageCodecs.Controls.Add(this.gridCodecMatrix);
            this.tabPageCodecs.Location = new System.Drawing.Point(4, 22);
            this.tabPageCodecs.Name = "tabPageCodecs";
            this.tabPageCodecs.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCodecs.Size = new System.Drawing.Size(448, 281);
            this.tabPageCodecs.TabIndex = 1;
            this.tabPageCodecs.Text = "Codecs";
            this.tabPageCodecs.UseVisualStyleBackColor = true;
            // 
            // PropGridSettings
            // 
            this.PropGridSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropGridSettings.HelpVisible = false;
            this.PropGridSettings.Location = new System.Drawing.Point(3, 3);
            this.PropGridSettings.Name = "PropGridSettings";
            this.PropGridSettings.Size = new System.Drawing.Size(442, 275);
            this.PropGridSettings.TabIndex = 0;
            this.PropGridSettings.ToolbarVisible = false;
            // 
            // gridCodecMatrix
            // 
            this.gridCodecMatrix.AllowUserToAddRows = false;
            this.gridCodecMatrix.AllowUserToDeleteRows = false;
            this.gridCodecMatrix.AllowUserToResizeRows = false;
            this.gridCodecMatrix.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridCodecMatrix.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCodecMatrix.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnVersion,
            this.ColumnPlatform,
            this.ColumnSoundBank,
            this.ColumnStreamBank,
            this.ColumnMusicBank});
            this.gridCodecMatrix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCodecMatrix.Location = new System.Drawing.Point(3, 3);
            this.gridCodecMatrix.MultiSelect = false;
            this.gridCodecMatrix.Name = "gridCodecMatrix";
            this.gridCodecMatrix.ReadOnly = true;
            this.gridCodecMatrix.RowHeadersVisible = false;
            this.gridCodecMatrix.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCodecMatrix.Size = new System.Drawing.Size(442, 275);
            this.gridCodecMatrix.TabIndex = 0;
            // 
            // ColumnVersion
            // 
            this.ColumnVersion.HeaderText = "Version";
            this.ColumnVersion.Name = "ColumnVersion";
            this.ColumnVersion.ReadOnly = true;
            // 
            // ColumnPlatform
            // 
            this.ColumnPlatform.HeaderText = "Platform";
            this.ColumnPlatform.Name = "ColumnPlatform";
            this.ColumnPlatform.ReadOnly = true;
            // 
            // ColumnSoundBank
            // 
            this.ColumnSoundBank.HeaderText = "SoundBank";
            this.ColumnSoundBank.Name = "ColumnSoundBank";
            this.ColumnSoundBank.ReadOnly = true;
            // 
            // ColumnStreamBank
            // 
            this.ColumnStreamBank.HeaderText = "StreamBank";
            this.ColumnStreamBank.Name = "ColumnStreamBank";
            this.ColumnStreamBank.ReadOnly = true;
            // 
            // ColumnMusicBank
            // 
            this.ColumnMusicBank.HeaderText = "MusicBank";
            this.ColumnMusicBank.Name = "ColumnMusicBank";
            this.ColumnMusicBank.ReadOnly = true;
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 307);
            this.Controls.Add(this.tabControlSettings);
            this.HideOnClose = true;
            this.Name = "FormSettings";
            this.TabText = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.tabControlSettings.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageCodecs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCodecMatrix)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid PropGridSettings;
        private System.Windows.Forms.TabControl tabControlSettings;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageCodecs;
        private System.Windows.Forms.DataGridView gridCodecMatrix;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPlatform;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSoundBank;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStreamBank;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMusicBank;
    }
}
