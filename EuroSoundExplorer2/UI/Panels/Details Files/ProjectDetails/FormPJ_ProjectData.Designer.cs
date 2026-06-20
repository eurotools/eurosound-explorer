
namespace sb_explorer
{
    partial class FormPJ_ProjectData
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
            this.propGrid_ProjData = new System.Windows.Forms.PropertyGrid();
            this.lvwFlags = new sb_explorer.CustomControls.ListView_ColumnSortingClick();
            this.colUserFlag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // propGrid_ProjData
            // 
            this.propGrid_ProjData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propGrid_ProjData.HelpVisible = false;
            this.propGrid_ProjData.Location = new System.Drawing.Point(132, 0);
            this.propGrid_ProjData.Name = "propGrid_ProjData";
            this.propGrid_ProjData.Size = new System.Drawing.Size(263, 349);
            this.propGrid_ProjData.TabIndex = 0;
            this.propGrid_ProjData.ToolbarVisible = false;
            // 
            // lvwFlags
            // 
            this.lvwFlags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvwFlags.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colUserFlag,
            this.colValue});
            this.lvwFlags.FullRowSelect = true;
            this.lvwFlags.GridLines = true;
            this.lvwFlags.HideSelection = false;
            this.lvwFlags.Location = new System.Drawing.Point(0, 0);
            this.lvwFlags.Name = "lvwFlags";
            this.lvwFlags.Size = new System.Drawing.Size(126, 349);
            this.lvwFlags.TabIndex = 1;
            this.lvwFlags.UseCompatibleStateImageBehavior = false;
            this.lvwFlags.View = System.Windows.Forms.View.Details;
            // 
            // colUserFlag
            // 
            this.colUserFlag.Text = "User Flag";
            // 
            // colValue
            // 
            this.colValue.Text = "Value";
            // 
            // FormPJ_ProjectData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 349);
            this.Controls.Add(this.lvwFlags);
            this.Controls.Add(this.propGrid_ProjData);
            this.HideOnClose = true;
            this.Name = "FormPJ_ProjectData";
            this.TabText = "Project Data";
            this.Text = "Project Data";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propGrid_ProjData;
        private CustomControls.ListView_ColumnSortingClick lvwFlags;
        private System.Windows.Forms.ColumnHeader colUserFlag;
        private System.Windows.Forms.ColumnHeader colValue;
    }
}