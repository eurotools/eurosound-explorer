
using EuroSoundExplorer2.CustomControls;

namespace EuroSoundExplorer2
{
    partial class FrmFileRefUsage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmFileRefUsage));
            this.listViewItemUsage = new EuroSoundExplorer2.CustomControls.ListView_ColumnSortingClick();
            this.Col_Item = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_Usage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ImageList_ListView = new System.Windows.Forms.ImageList(this.components);
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewItemUsage
            // 
            this.listViewItemUsage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewItemUsage.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Col_Item,
            this.Col_Usage});
            this.listViewItemUsage.FullRowSelect = true;
            this.listViewItemUsage.GridLines = true;
            this.listViewItemUsage.HideSelection = false;
            this.listViewItemUsage.Location = new System.Drawing.Point(12, 12);
            this.listViewItemUsage.Name = "listViewItemUsage";
            this.listViewItemUsage.Size = new System.Drawing.Size(384, 296);
            this.listViewItemUsage.SmallImageList = this.ImageList_ListView;
            this.listViewItemUsage.TabIndex = 0;
            this.listViewItemUsage.UseCompatibleStateImageBehavior = false;
            this.listViewItemUsage.View = System.Windows.Forms.View.Details;
            // 
            // Col_Item
            // 
            this.Col_Item.Text = "File Ref";
            this.Col_Item.Width = 75;
            // 
            // Col_Usage
            // 
            this.Col_Usage.Text = "Usage";
            this.Col_Usage.Width = 268;
            // 
            // ImageList_ListView
            // 
            this.ImageList_ListView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList_ListView.ImageStream")));
            this.ImageList_ListView.TransparentColor = System.Drawing.Color.Magenta;
            this.ImageList_ListView.Images.SetKeyName(0, "iconUsage.png");
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(321, 314);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // FrmFileRefUsage
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 349);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.listViewItemUsage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmFileRefUsage";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Item Usage";
            this.Shown += new System.EventHandler(this.FrmFileRefUsage_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private ListView_ColumnSortingClick listViewItemUsage;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ColumnHeader Col_Item;
        private System.Windows.Forms.ColumnHeader Col_Usage;
        private System.Windows.Forms.ImageList ImageList_ListView;
    }
}