
namespace sb_explorer
{
    partial class FormPJ_SoundBanks
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
            this.labelCount = new System.Windows.Forms.ToolStripLabel();
            this.textboxCount = new System.Windows.Forms.ToolStripTextBox();
            this.listView_ColumnSortingClick1 = new sb_explorer.CustomControls.ListView_ColumnSortingClick();
            this.colHashCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSlotNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelCount,
            this.textboxCount});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(463, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // labelCount
            // 
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(40, 22);
            this.labelCount.Text = "Count";
            // 
            // textboxCount
            // 
            this.textboxCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textboxCount.Name = "textboxCount";
            this.textboxCount.Size = new System.Drawing.Size(100, 25);
            this.textboxCount.Text = "0";
            // 
            // listView_ColumnSortingClick1
            // 
            this.listView_ColumnSortingClick1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_ColumnSortingClick1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHashCode,
            this.colSlotNumber});
            this.listView_ColumnSortingClick1.FullRowSelect = true;
            this.listView_ColumnSortingClick1.GridLines = true;
            this.listView_ColumnSortingClick1.HideSelection = false;
            this.listView_ColumnSortingClick1.Location = new System.Drawing.Point(0, 26);
            this.listView_ColumnSortingClick1.Name = "listView_ColumnSortingClick1";
            this.listView_ColumnSortingClick1.Size = new System.Drawing.Size(463, 388);
            this.listView_ColumnSortingClick1.TabIndex = 2;
            this.listView_ColumnSortingClick1.UseCompatibleStateImageBehavior = false;
            this.listView_ColumnSortingClick1.View = System.Windows.Forms.View.Details;
            // 
            // colHashCode
            // 
            this.colHashCode.Text = "HashCode";
            this.colHashCode.Width = 200;
            // 
            // colSlotNumber
            // 
            this.colSlotNumber.Text = "Slot Number";
            this.colSlotNumber.Width = 200;
            // 
            // FormPJ_SoundBanks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 414);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.listView_ColumnSortingClick1);
            this.HideOnClose = true;
            this.Name = "FormPJ_SoundBanks";
            this.TabText = "SoundBanks";
            this.Text = "SoundBanks";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel labelCount;
        private System.Windows.Forms.ColumnHeader colHashCode;
        private System.Windows.Forms.ColumnHeader colSlotNumber;
        protected internal CustomControls.ListView_ColumnSortingClick listView_ColumnSortingClick1;
        protected internal System.Windows.Forms.ToolStripTextBox textboxCount;
    }
}