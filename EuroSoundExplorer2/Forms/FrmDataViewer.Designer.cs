
namespace EuroSoundExplorer2
{
    partial class FrmDataViewer
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.labelMemory = new System.Windows.Forms.ToolStripLabel();
            this.labelMemoryValue = new System.Windows.Forms.ToolStripLabel();
            this.separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.labelPlatform = new System.Windows.Forms.ToolStripLabel();
            this.labelPlatformValue = new System.Windows.Forms.ToolStripLabel();
            this.separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.labelErrors = new System.Windows.Forms.ToolStripLabel();
            this.labelErrorsValue = new System.Windows.Forms.ToolStripLabel();
            this.separator3 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonFind = new System.Windows.Forms.ToolStripButton();
            this.textboxFind = new System.Windows.Forms.ToolStripTextBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuTreeview = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItem_SetFont = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.fntDialog = new System.Windows.Forms.FontDialog();
            this.MenuItem_ExpandNode = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_CollapseNode = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Separator = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            this.contextMenuTreeview.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.CanOverflow = false;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelMemory,
            this.labelMemoryValue,
            this.separator1,
            this.labelPlatform,
            this.labelPlatformValue,
            this.separator2,
            this.labelErrors,
            this.labelErrorsValue,
            this.separator3,
            this.buttonFind,
            this.textboxFind});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(540, 25);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // labelMemory
            // 
            this.labelMemory.Name = "labelMemory";
            this.labelMemory.Size = new System.Drawing.Size(35, 22);
            this.labelMemory.Text = "Mem";
            // 
            // labelMemoryValue
            // 
            this.labelMemoryValue.Name = "labelMemoryValue";
            this.labelMemoryValue.Size = new System.Drawing.Size(44, 22);
            this.labelMemoryValue.Text = "1234kb";
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(6, 25);
            // 
            // labelPlatform
            // 
            this.labelPlatform.Name = "labelPlatform";
            this.labelPlatform.Size = new System.Drawing.Size(53, 22);
            this.labelPlatform.Text = "Platform";
            // 
            // labelPlatformValue
            // 
            this.labelPlatformValue.Name = "labelPlatformValue";
            this.labelPlatformValue.Size = new System.Drawing.Size(31, 22);
            this.labelPlatformValue.Text = "XB__";
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            this.separator2.Size = new System.Drawing.Size(6, 25);
            // 
            // labelErrors
            // 
            this.labelErrors.Name = "labelErrors";
            this.labelErrors.Size = new System.Drawing.Size(37, 22);
            this.labelErrors.Text = "Errors";
            // 
            // labelErrorsValue
            // 
            this.labelErrorsValue.Name = "labelErrorsValue";
            this.labelErrorsValue.Size = new System.Drawing.Size(25, 22);
            this.labelErrorsValue.Text = "123";
            // 
            // separator3
            // 
            this.separator3.Name = "separator3";
            this.separator3.Size = new System.Drawing.Size(6, 25);
            // 
            // buttonFind
            // 
            this.buttonFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.buttonFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(34, 22);
            this.buttonFind.Text = "Find";
            this.buttonFind.Click += new System.EventHandler(this.ButtonFind_Click);
            // 
            // textboxFind
            // 
            this.textboxFind.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textboxFind.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textboxFind.Name = "textboxFind";
            this.textboxFind.Size = new System.Drawing.Size(100, 25);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.ContextMenuStrip = this.contextMenuTreeview;
            this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.treeView1.Location = new System.Drawing.Point(0, 25);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(540, 454);
            this.treeView1.TabIndex = 7;
            this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TreeView1_MouseDown);
            // 
            // contextMenuTreeview
            // 
            this.contextMenuTreeview.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_ExpandNode,
            this.MenuItem_CollapseNode,
            this.MenuItem_Separator,
            this.MenuItem_SetFont});
            this.contextMenuTreeview.Name = "contextMenuTreeview";
            this.contextMenuTreeview.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuTreeview.Size = new System.Drawing.Size(120, 76);
            // 
            // MenuItem_SetFont
            // 
            this.MenuItem_SetFont.Name = "MenuItem_SetFont";
            this.MenuItem_SetFont.Size = new System.Drawing.Size(119, 22);
            this.MenuItem_SetFont.Text = "Font...";
            this.MenuItem_SetFont.Click += new System.EventHandler(this.MenuItem_SetFont_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "SFX Files (*.sfx)|*.sfx";
            // 
            // MenuItem_ExpandNode
            // 
            this.MenuItem_ExpandNode.Name = "MenuItem_ExpandNode";
            this.MenuItem_ExpandNode.Size = new System.Drawing.Size(119, 22);
            this.MenuItem_ExpandNode.Text = "Expand";
            this.MenuItem_ExpandNode.Click += new System.EventHandler(this.MenuItem_ExpandNode_Click);
            // 
            // MenuItem_CollapseNode
            // 
            this.MenuItem_CollapseNode.Name = "MenuItem_CollapseNode";
            this.MenuItem_CollapseNode.Size = new System.Drawing.Size(119, 22);
            this.MenuItem_CollapseNode.Text = "Collapse";
            this.MenuItem_CollapseNode.Click += new System.EventHandler(this.MenuItem_CollapseNode_Click);
            // 
            // MenuItem_Separator
            // 
            this.MenuItem_Separator.Name = "MenuItem_Separator";
            this.MenuItem_Separator.Size = new System.Drawing.Size(116, 6);
            // 
            // FrmDataViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 479);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.toolStrip1);
            this.MinimizeBox = false;
            this.Name = "FrmDataViewer";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Data Viewer";
            this.Load += new System.EventHandler(this.FrmDataViewer_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuTreeview.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel labelMemory;
        private System.Windows.Forms.ToolStripLabel labelMemoryValue;
        private System.Windows.Forms.ToolStripSeparator separator1;
        private System.Windows.Forms.ToolStripLabel labelPlatform;
        private System.Windows.Forms.ToolStripLabel labelPlatformValue;
        private System.Windows.Forms.ToolStripSeparator separator2;
        private System.Windows.Forms.ToolStripLabel labelErrors;
        private System.Windows.Forms.ToolStripLabel labelErrorsValue;
        private System.Windows.Forms.ToolStripSeparator separator3;
        private System.Windows.Forms.ToolStripButton buttonFind;
        private System.Windows.Forms.ToolStripTextBox textboxFind;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuTreeview;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SetFont;
        private System.Windows.Forms.FontDialog fntDialog;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_ExpandNode;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_CollapseNode;
        private System.Windows.Forms.ToolStripSeparator MenuItem_Separator;
    }
}