using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace sb_explorer
{
    public partial class FrmProgress : Form
    {
        private readonly Func<BackgroundWorker, object> work;

        public object Result { get; private set; }
        public Exception Error { get; private set; }

        public FrmProgress(string title, string initialStatus, Func<BackgroundWorker, object> work)
        {
            InitializeComponent();
            Text = title;
            labelStatus.Text = initialStatus;
            this.work = work;
        }

        private void FrmProgress_Shown(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            buttonCancel.Enabled = false;
            labelStatus.Text = "Cancelling...";
            backgroundWorker.CancelAsync();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                e.Result = work((BackgroundWorker)sender);
            }
            catch (Exception ex)
            {
                Error = ex;
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int value = Math.Max(progressBar.Minimum, Math.Min(progressBar.Maximum, e.ProgressPercentage));
            progressBar.Value = value;
            if (e.UserState != null)
            {
                labelStatus.Text = e.UserState.ToString();
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (Error == null)
            {
                Result = e.Result;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
