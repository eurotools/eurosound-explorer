using System;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using static EuroSoundExplorer2.AppConfig;

namespace EuroSoundExplorer2
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FormSettings : DockContent
    {
        //-------------------------------------------------------------------------------------------
        //  MAIN FORM
        //-------------------------------------------------------------------------------------------
        public FormSettings()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void FormSettings_Load(object sender, EventArgs e)
        {
            PropGridSettings.SelectedObject = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration;
            LoadSettings();
        }

        //-------------------------------------------------------------------------------------------
        //  FUNCTIONS
        //-------------------------------------------------------------------------------------------
        public void SaveSettings()
        {
            using (StreamWriter sw = new StreamWriter(File.Open("ESEx\\General Settings.ini", FileMode.Create, FileAccess.Write, FileShare.Read)))
            {
                sw.WriteLine("FilesFolder={0}", ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.ProjectFolder);
                sw.WriteLine("Platform={0}", ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected);
                sw.WriteLine("SoundhFile={0}", ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.SoundhFile);
                sw.WriteLine("Title={0}", ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.TitleSelected);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void LoadSettings()
        {
            string filePath = "ESEx\\General Settings.ini";
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] lineData = line.Split('=');
                        if (lineData.Length == 2)
                        {
                            switch (lineData[0])
                            {
                                case "FilesFolder":
                                    ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.ProjectFolder = lineData[1];
                                    break;
                                case "Platform":
                                    if (Enum.TryParse(lineData[1], out Platform selectedPlatform))
                                    {
                                        ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected = selectedPlatform;
                                    }
                                    break;
                                case "SoundhFile":
                                    ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.SoundhFile = lineData[1];
                                    break;
                                case "Title":
                                    if (Enum.TryParse(lineData[1], out Title selectedTitle))
                                    {
                                        ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.TitleSelected = selectedTitle;
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
