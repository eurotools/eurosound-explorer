using System;
using System.Windows.Forms;

namespace sb_explorer
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (FrmSplash splash = new FrmSplash())
            {
                splash.Show();
                Application.DoEvents();

                FrmMain mainForm = new FrmMain(splash);
                mainForm.Shown += (sender, args) =>
                {
                    if (!splash.IsDisposed)
                    {
                        splash.Close();
                    }
                };

                Application.Run(mainForm);
            }
        }
    }
}
