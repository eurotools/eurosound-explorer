using System;
using MusX;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using static sb_explorer.Enumerations;

namespace sb_explorer
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
            PropGridSettings.SelectedObject = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration;
            LoadSettings();
            LoadCodecMatrix();
        }

        //-------------------------------------------------------------------------------------------
        //  FUNCTIONS
        //-------------------------------------------------------------------------------------------
        public void SaveSettings()
        {
            Directory.CreateDirectory(SettingsDirectory);

            using (StreamWriter sw = new StreamWriter(File.Open(SettingsFilePath, FileMode.Create, FileAccess.Write, FileShare.Read)))
            {
                sw.WriteLine("SoundhFile={0}", ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.SoundhFile);
                sw.WriteLine("FilesFolder={0}", ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.ProjectFolder);
                sw.WriteLine("Platform={0}", ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.PlatformSelected);
                sw.WriteLine("Title={0}", ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.TitleSelected);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void LoadSettings()
        {
            string filePath = GetSettingsFileToLoad();
            if (File.Exists(filePath))
            {
                var parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);
                using (StreamReader sr = new StreamReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] lineData = line.Split(new[] { '=' }, 2);
                        if (lineData.Length == 2)
                        {
                            switch (lineData[0])
                            {
                                case "FilesFolder":
                                    parentForm.Configuration.ProjectFolder = lineData[1];
                                    break;
                                case "Platform":
                                    if (Enum.TryParse(lineData[1], out Platform selectedPlatform))
                                    {
                                        parentForm.Configuration.PlatformSelected = selectedPlatform;
                                    }
                                    break;
                                case "SoundhFile":
                                    parentForm.Configuration.SoundhFile = lineData[1];
                                    break;
                                case "Title":
                                    if (Enum.TryParse(lineData[1], out Title selectedTitle))
                                    {
                                        parentForm.Configuration.TitleSelected = selectedTitle;
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private string SettingsDirectory
        {
            get { return Path.Combine(Application.StartupPath, "ESEx"); }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private string SettingsFilePath
        {
            get { return Path.Combine(SettingsDirectory, "General Settings.ini"); }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private string GetSettingsFileToLoad()
        {
            return SettingsFilePath;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void LoadCodecMatrix()
        {
            gridCodecMatrix.Rows.Clear();
            AddCodecRows(1, "1 (EuroSound 357)");
            AddCodecRows(201, "201 (EuroSound 357)");
            AddCodecRows(4, "4 (EuroSound 450)");
            AddCodecRows(5, "5 (EuroSound 510)");
            AddCodecRows(6, "6 (EuroSound 650)");
            AddCodecRow(6, "6 (EuroSound 650)", "Wii");
            AddCodecRow(6, "6 (EuroSound 650)", "XB2_");
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void AddCodecRows(int version, string versionLabel)
        {
            AddCodecRow(version, versionLabel, "PC");
            AddCodecRow(version, versionLabel, "PS2");
            AddCodecRow(version, versionLabel, "GC");
            AddCodecRow(version, versionLabel, "Xbox");
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void AddCodecRow(int version, string versionLabel, string platform)
        {
            gridCodecMatrix.Rows.Add(
                versionLabel,
                platform,
                EuroSoundCodecMatrix.GetCodec(version, platform, EuroSoundBankType.SoundBank).ToString(),
                EuroSoundCodecMatrix.GetCodec(version, platform, EuroSoundBankType.StreamBank).ToString(),
                EuroSoundCodecMatrix.GetCodec(version, platform, EuroSoundBankType.MusicBank).ToString());
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
