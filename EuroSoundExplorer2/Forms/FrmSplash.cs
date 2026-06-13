using System;
using System.Reflection;
using System.Windows.Forms;

namespace sb_explorer
{
    public partial class FrmSplash : Form
    {
        public FrmSplash()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetStatus("Starting EuroSound Explorer...");
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal void SetStatus(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                Text = text;
            }
            Update();
            Application.DoEvents();
        }
    }
}
