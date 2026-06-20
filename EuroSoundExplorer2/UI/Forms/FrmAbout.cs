using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace sb_explorer
{
    public partial class FrmAbout : Form
    {
        public FrmAbout()
        {
            InitializeComponent();
        }

        private void FrmAbout_Load(object sender, EventArgs e)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = string.Format("Version {0}.{1}", version.Major, version.Minor);
        }

        private void btnLatestRelease_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/eurotools/eurosound-explorer/releases/latest");
        }
    }
}
