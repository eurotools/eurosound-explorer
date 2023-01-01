
namespace EuroSoundExplorer2
{
    partial class FormSBiMusicBanks
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.labelSoundBanks = new System.Windows.Forms.ToolStripLabel();
            this.textBoxSoundBanksCount = new System.Windows.Forms.ToolStripTextBox();
            this.listView_ColumnSortingClick1 = new EuroSoundExplorer2.CustomControls.ListView_ColumnSortingClick();
            this.colHashCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHashCodeLabel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelSoundBanks,
            this.textBoxSoundBanksCount});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(392, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // labelSoundBanks
            // 
            this.labelSoundBanks.Name = "labelSoundBanks";
            this.labelSoundBanks.Size = new System.Drawing.Size(75, 22);
            this.labelSoundBanks.Text = "SoundBanks:";
            // 
            // textBoxSoundBanksCount
            // 
            this.textBoxSoundBanksCount.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxSoundBanksCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxSoundBanksCount.Name = "textBoxSoundBanksCount";
            this.textBoxSoundBanksCount.ReadOnly = true;
            this.textBoxSoundBanksCount.Size = new System.Drawing.Size(35, 25);
            this.textBoxSoundBanksCount.Text = "0";
            // 
            // listView_ColumnSortingClick1
            // 
            this.listView_ColumnSortingClick1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_ColumnSortingClick1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHashCode,
            this.colHashCodeLabel});
            this.listView_ColumnSortingClick1.FullRowSelect = true;
            this.listView_ColumnSortingClick1.GridLines = true;
            this.listView_ColumnSortingClick1.HideSelection = false;
            this.listView_ColumnSortingClick1.Location = new System.Drawing.Point(0, 25);
            this.listView_ColumnSortingClick1.Name = "listView_ColumnSortingClick1";
            this.listView_ColumnSortingClick1.Size = new System.Drawing.Size(392, 321);
            this.listView_ColumnSortingClick1.TabIndex = 1;
            this.listView_ColumnSortingClick1.UseCompatibleStateImageBehavior = false;
            this.listView_ColumnSortingClick1.View = System.Windows.Forms.View.Details;
            // 
            // colHashCode
            // 
            this.colHashCode.Text = "HashCode";
            this.colHashCode.Width = 100;
            // 
            // colHashCodeLabel
            // 
            this.colHashCodeLabel.Text = "HashCode Label";
            this.colHashCodeLabel.Width = 200;
            // 
            // FormSBiMusicBanks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 346);
            this.Controls.Add(this.listView_ColumnSortingClick1);
            this.Controls.Add(this.toolStrip1);
            this.HideOnClose = true;
            this.Name = "FormSBiMusicBanks";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TabText = "SBInfo MusicBanks";
            this.Text = "SBInfo MusicBanks";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel labelSoundBanks;
        private System.Windows.Forms.ToolStripTextBox textBoxSoundBanksCount;
        private CustomControls.ListView_ColumnSortingClick listView_ColumnSortingClick1;
        private System.Windows.Forms.ColumnHeader colHashCode;
        private System.Windows.Forms.ColumnHeader colHashCodeLabel;
    }
}