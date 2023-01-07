
namespace sb_explorer
{
    partial class FormSB_SampleProps
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
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox2 = new System.Windows.Forms.CheckedListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_Properties = new System.Windows.Forms.TabPage();
            this.tabPage_Flags = new System.Windows.Forms.TabPage();
            this.tabPage_UserFlags = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage_Properties.SuspendLayout();
            this.tabPage_Flags.SuspendLayout();
            this.tabPage_UserFlags.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(515, 391);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox1.ForeColor = System.Drawing.Color.Blue;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "UserFlags1",
            "UserFlags2",
            "UserFlags3",
            "UserFlags4",
            "UserFlags5",
            "UserFlags6",
            "UserFlags7",
            "UserFlags8",
            "UserFlags9",
            "UserFlags10",
            "UserFlags11",
            "UserFlags12",
            "UserFlags13",
            "UserFlags14",
            "UserFlags15",
            "UserFlags16"});
            this.checkedListBox1.Location = new System.Drawing.Point(3, 3);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.checkedListBox1.Size = new System.Drawing.Size(515, 391);
            this.checkedListBox1.TabIndex = 1;
            // 
            // checkedListBox2
            // 
            this.checkedListBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox2.ForeColor = System.Drawing.Color.Purple;
            this.checkedListBox2.Items.AddRange(new object[] {
            "UserFlags1",
            "UserFlags2",
            "UserFlags3",
            "UserFlags4",
            "UserFlags5",
            "UserFlags6",
            "UserFlags7",
            "UserFlags8",
            "UserFlags9",
            "UserFlags10",
            "UserFlags11",
            "UserFlags12",
            "UserFlags13",
            "UserFlags14",
            "UserFlags15",
            "UserFlags16"});
            this.checkedListBox2.Location = new System.Drawing.Point(3, 3);
            this.checkedListBox2.Name = "checkedListBox2";
            this.checkedListBox2.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.checkedListBox2.Size = new System.Drawing.Size(515, 391);
            this.checkedListBox2.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_Properties);
            this.tabControl1.Controls.Add(this.tabPage_Flags);
            this.tabControl1.Controls.Add(this.tabPage_UserFlags);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(529, 423);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage_Properties
            // 
            this.tabPage_Properties.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_Properties.Controls.Add(this.propertyGrid1);
            this.tabPage_Properties.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Properties.Name = "tabPage_Properties";
            this.tabPage_Properties.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Properties.Size = new System.Drawing.Size(521, 397);
            this.tabPage_Properties.TabIndex = 0;
            this.tabPage_Properties.Text = "Settings";
            // 
            // tabPage_Flags
            // 
            this.tabPage_Flags.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_Flags.Controls.Add(this.checkedListBox1);
            this.tabPage_Flags.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Flags.Name = "tabPage_Flags";
            this.tabPage_Flags.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Flags.Size = new System.Drawing.Size(521, 397);
            this.tabPage_Flags.TabIndex = 1;
            this.tabPage_Flags.Text = "Flags";
            // 
            // tabPage_UserFlags
            // 
            this.tabPage_UserFlags.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_UserFlags.Controls.Add(this.checkedListBox2);
            this.tabPage_UserFlags.Location = new System.Drawing.Point(4, 22);
            this.tabPage_UserFlags.Name = "tabPage_UserFlags";
            this.tabPage_UserFlags.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_UserFlags.Size = new System.Drawing.Size(521, 397);
            this.tabPage_UserFlags.TabIndex = 2;
            this.tabPage_UserFlags.Text = "User Flags";
            // 
            // FormSB_SampleProps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 423);
            this.Controls.Add(this.tabControl1);
            this.HideOnClose = true;
            this.Name = "FormSB_SampleProps";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TabText = "SFX Properties";
            this.Text = "SFX Properties";
            this.tabControl1.ResumeLayout(false);
            this.tabPage_Properties.ResumeLayout(false);
            this.tabPage_Flags.ResumeLayout(false);
            this.tabPage_UserFlags.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        protected internal System.Windows.Forms.CheckedListBox checkedListBox2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_Properties;
        private System.Windows.Forms.TabPage tabPage_Flags;
        private System.Windows.Forms.TabPage tabPage_UserFlags;
    }
}