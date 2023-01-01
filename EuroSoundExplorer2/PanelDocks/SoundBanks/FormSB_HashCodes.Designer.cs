﻿
using EuroSoundExplorer2.CustomControls;

namespace EuroSoundExplorer2
{
    partial class FormSB_HashCodes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSB_HashCodes));
            this.listView1 = new EuroSoundExplorer2.CustomControls.ListView_ColumnSortingClick();
            this.Col_HashCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_HashCode_Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_HashCode_Label = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ButtonSendToSamplePool = new System.Windows.Forms.ToolStripButton();
            this.ButtonListProperties = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Col_HashCode,
            this.Col_HashCode_Status,
            this.Col_HashCode_Label});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 31);
            this.listView1.Margin = new System.Windows.Forms.Padding(0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(599, 393);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.ListView1_SelectedIndexChanged);
            // 
            // Col_HashCode
            // 
            this.Col_HashCode.Text = "HashCode";
            this.Col_HashCode.Width = 96;
            // 
            // Col_HashCode_Status
            // 
            this.Col_HashCode_Status.Text = "Status";
            this.Col_HashCode_Status.Width = 70;
            // 
            // Col_HashCode_Label
            // 
            this.Col_HashCode_Label.Text = "Label";
            this.Col_HashCode_Label.Width = 250;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList1.Images.SetKeyName(0, "SfxItem.png");
            this.imageList1.Images.SetKeyName(1, "SatusError.png");
            this.imageList1.Images.SetKeyName(2, "SatusOK.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonSendToSamplePool,
            this.ButtonListProperties});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(599, 31);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ButtonSendToSamplePool
            // 
            this.ButtonSendToSamplePool.Checked = true;
            this.ButtonSendToSamplePool.CheckOnClick = true;
            this.ButtonSendToSamplePool.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ButtonSendToSamplePool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonSendToSamplePool.Image = ((System.Drawing.Image)(resources.GetObject("ButtonSendToSamplePool.Image")));
            this.ButtonSendToSamplePool.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonSendToSamplePool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonSendToSamplePool.Name = "ButtonSendToSamplePool";
            this.ButtonSendToSamplePool.Size = new System.Drawing.Size(28, 28);
            this.ButtonSendToSamplePool.Text = "List samples in the sample pool";
            // 
            // ButtonListProperties
            // 
            this.ButtonListProperties.Checked = true;
            this.ButtonListProperties.CheckOnClick = true;
            this.ButtonListProperties.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ButtonListProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonListProperties.Image = ((System.Drawing.Image)(resources.GetObject("ButtonListProperties.Image")));
            this.ButtonListProperties.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonListProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonListProperties.Name = "ButtonListProperties";
            this.ButtonListProperties.Size = new System.Drawing.Size(28, 28);
            this.ButtonListProperties.Text = "List selected item properties";
            this.ButtonListProperties.ToolTipText = "List selected item properties";
            // 
            // FormSB_HashCodes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 424);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.listView1);
            this.HideOnClose = true;
            this.Name = "FormSB_HashCodes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TabText = "SoundBank HashCodes";
            this.Text = "SoundBank HashCodes";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ColumnHeader Col_HashCode;
        private System.Windows.Forms.ColumnHeader Col_HashCode_Status;
        private System.Windows.Forms.ColumnHeader Col_HashCode_Label;
        private System.Windows.Forms.ImageList imageList1;
        protected internal ListView_ColumnSortingClick listView1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ButtonSendToSamplePool;
        private System.Windows.Forms.ToolStripButton ButtonListProperties;
    }
}