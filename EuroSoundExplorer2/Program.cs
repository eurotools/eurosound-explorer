using System;
using System.Diagnostics;
using System.Threading;
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
            FrmMain mainForm;
            using (FrmSplash splash = new FrmSplash())
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                splash.Show();
                Application.DoEvents();

                mainForm = new FrmMain();
                Application.DoEvents();

                while (stopwatch.ElapsedMilliseconds < 2000)
                {
                    int progress = 55 + (int)((stopwatch.ElapsedMilliseconds / 2000.0) * 45);
                    Application.DoEvents();
                    Thread.Sleep(40);
                }

                Application.DoEvents();
                splash.Close();
            }

            Application.Run(mainForm);
        }
    }
}
