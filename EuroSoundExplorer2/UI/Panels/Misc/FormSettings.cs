using System;
using MusX;
using System.IO;
using System.Linq;
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
        private DataGridView gridProjects;
        private Button buttonAddProject;
        private Button buttonRemoveProject;
        private Button buttonApplyProject;
        private ComboBox comboProjects;
        private ComboBox comboPlatforms;
        private bool loadingProfiles;
        private bool profilesDirty;
        private static readonly Platform[] ProfilePlatforms =
        {
            Platform.PC, Platform.PS2, Platform.PS3, Platform.GameCube,
            Platform.Wii, Platform.Xbox, Platform.Xbox360
        };

        //-------------------------------------------------------------------------------------------
        //  MAIN FORM
        //-------------------------------------------------------------------------------------------
        public FormSettings()
        {
            InitializeComponent();
            InitializeProjectSelector();
            InitializeProjectsTab();
            PropGridSettings.PropertyValueChanged += delegate(object sender, PropertyValueChangedEventArgs e)
            {
                FrmMain main = (FrmMain)Application.OpenForms[nameof(FrmMain)];
                if (e.ChangedItem != null && e.ChangedItem.PropertyDescriptor != null &&
                    e.ChangedItem.PropertyDescriptor.Name == nameof(AppConfig.PlatformSelected) &&
                    Directory.Exists(main.Configuration.ProjectFolder))
                {
                    main.pnlSoundBankFiles.LoadData();
                }
            };
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
            if (profilesDirty) SaveProfileGrid();

            using (StreamWriter sw = new StreamWriter(File.Open(SettingsFilePath, FileMode.Create, FileAccess.Write, FileShare.Read)))
            {
                sw.WriteLine("SoundhFile={0}", ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.SoundhFile);
                sw.WriteLine("FilesFolder={0}", ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.ProjectFolder);
                sw.WriteLine("Platform={0}", ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.PlatformSelected);
                sw.WriteLine("Title={0}", ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.TitleSelected);
                sw.WriteLine("ProjectTitle={0}", ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.ProjectTitle);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void LoadSettings()
        {
            FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);
            parentForm.AppState.ProjectProfiles.Load(ProjectsFilePath);
            LoadProfileGrid();
            string filePath = GetSettingsFileToLoad();
            if (File.Exists(filePath))
            {
                string projectFolder = null;
                string soundhFile = null;
                string projectTitle = null;
                Platform? platform = null;
                Title? title = null;

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
                                    projectFolder = lineData[1];
                                    break;
                                case "Platform":
                                    if (Enum.TryParse(lineData[1], out Platform selectedPlatform))
                                    {
                                        platform = selectedPlatform;
                                    }
                                    break;
                                case "SoundhFile":
                                    soundhFile = lineData[1];
                                    break;
                                case "Title":
                                    if (Enum.TryParse(lineData[1], out Title selectedTitle))
                                    {
                                        title = selectedTitle;
                                    }
                                    break;
                                case "ProjectTitle":
                                    projectTitle = lineData[1];
                                    break;
                            }
                        }
                    }
                }

                if (title.HasValue)
                    parentForm.Configuration.TitleSelected = title.Value;
                if (projectTitle != null)
                    parentForm.Configuration.ProjectTitle = projectTitle;
                if (platform.HasValue)
                    parentForm.Configuration.PlatformSelected = platform.Value;
                if (soundhFile != null)
                    parentForm.Configuration.SoundhFile = soundhFile;

                ProjectProfile namedProfile = parentForm.AppState.ProjectProfiles.FindByName(projectTitle);
                if (namedProfile == null && title.HasValue && title.Value != Title.None)
                    namedProfile = parentForm.AppState.ProjectProfiles.FindByName(title.Value.ToString());

                // General Settings.ini only remembers the last selection. If that old selection
                // points at a broad game root, use the platform-specific folder from Projects.ini;
                // never import that broad path over an existing named profile.
                if (namedProfile != null)
                {
                    Platform selectedPlatform = platform ?? Platform.None;
                    string selectedFolder = namedProfile.GetFolder(selectedPlatform);
                    if (string.IsNullOrWhiteSpace(selectedFolder))
                    {
                        Platform[] defined = namedProfile.DefinedPlatforms.ToArray();
                        if (defined.Length == 1)
                        {
                            selectedPlatform = defined[0];
                            selectedFolder = namedProfile.GetFolder(selectedPlatform);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(selectedFolder))
                    {
                        projectFolder = selectedFolder;
                        platform = selectedPlatform;
                    }
                    soundhFile = namedProfile.HashTable;
                    title = namedProfile.CompatibilityTitle;
                }

                // Migrate a genuinely unknown previous project only. A project with the same
                // name is never rewritten from General Settings.ini.
                if (namedProfile == null && !string.IsNullOrWhiteSpace(projectFolder) && parentForm.AppState.ProjectProfiles.FindForFolder(projectFolder) == null)
                {
                    parentForm.AppState.ProjectProfiles.Profiles.Add(new ProjectProfile
                    {
                        Name = !string.IsNullOrWhiteSpace(projectTitle) ? projectTitle :
                            (title.HasValue && title.Value != Title.None ? title.Value.ToString() : new DirectoryInfo(projectFolder).Name),
                        HashTable = soundhFile,
                        Platform = platform ?? Platform.None,
                        CompatibilityTitle = title ?? Title.None
                    });
                    ProjectProfile migrated = parentForm.AppState.ProjectProfiles.Profiles[parentForm.AppState.ProjectProfiles.Profiles.Count - 1];
                    if (platform.HasValue && platform.Value != Platform.None) migrated.SetFolder(platform.Value, projectFolder);
                    parentForm.AppState.ProjectProfiles.Save(ProjectsFilePath);
                    LoadProfileGrid();
                }

                // ProjectFolder raises LoadData, so apply it only after every other setting is ready.
                if (projectFolder != null)
                    parentForm.Configuration.ProjectFolder = projectFolder;
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

        private string ProjectsFilePath
        {
            get { return Path.Combine(SettingsDirectory, "Projects.ini"); }
        }

        public void RefreshSettings()
        {
            if (PropGridSettings.SelectedObject != null) PropGridSettings.Refresh();
            RefreshProjectSelector();
        }

        private void InitializeProjectsTab()
        {
            TabPage page = new TabPage("Projects");
            gridProjects = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                RowHeadersVisible = false
            };
            gridProjects.Columns.Add("Name", "Title");
            gridProjects.Columns.Add(new DataGridViewTextBoxColumn { Name = "LegacyFolder", Visible = false });
            AddPlatformFolderColumn("PCFolder", "PC Folder");
            AddPlatformFolderColumn("PS2Folder", "PS2 Folder");
            AddPlatformFolderColumn("PS3Folder", "PS3 Folder");
            AddPlatformFolderColumn("GCFolder", "GC Folder");
            AddPlatformFolderColumn("WiiFolder", "Wii Folder");
            AddPlatformFolderColumn("XboxFolder", "Xbox Folder");
            AddPlatformFolderColumn("Xbox360Folder", "Xbox 360 Folder");
            gridProjects.Columns.Add("HashTable", "Hash Table");
            gridProjects.Columns.Add(new DataGridViewComboBoxColumn { Name = "CompatibilityTitle", HeaderText = "Compatibility", DataSource = Enum.GetValues(typeof(Title)) });
            gridProjects.CellValueChanged += delegate { if (!loadingProfiles) profilesDirty = true; };
            gridProjects.RowsRemoved += delegate { if (!loadingProfiles) profilesDirty = true; };

            FlowLayoutPanel buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 34, FlowDirection = FlowDirection.LeftToRight };
            buttonAddProject = new Button { Text = "Add current", AutoSize = true };
            buttonRemoveProject = new Button { Text = "Remove", AutoSize = true };
            buttonApplyProject = new Button { Text = "Apply selected", AutoSize = true };
            buttonAddProject.Click += delegate
            {
                FrmMain main = (FrmMain)Application.OpenForms[nameof(FrmMain)];
                int rowIndex = gridProjects.Rows.Add();
                DataGridViewRow row = gridProjects.Rows[rowIndex];
                row.Cells["Name"].Value = main.Configuration.ProjectTitle;
                if (main.Configuration.PlatformSelected != Platform.None)
                    row.Cells[FolderColumn(main.Configuration.PlatformSelected)].Value = main.Configuration.ProjectFolder;
                row.Cells["HashTable"].Value = main.Configuration.SoundhFile;
                row.Cells["CompatibilityTitle"].Value = main.Configuration.TitleSelected;
                profilesDirty = true;
            };
            buttonRemoveProject.Click += delegate
            {
                foreach (DataGridViewRow row in gridProjects.SelectedRows) if (!row.IsNewRow) gridProjects.Rows.Remove(row);
                profilesDirty = true;
            };
            buttonApplyProject.Click += delegate
            {
                if (gridProjects.CurrentRow == null) return;
                SaveProfileGrid();
                FrmMain main = (FrmMain)Application.OpenForms[nameof(FrmMain)];
                ProjectProfile profile = main.AppState.ProjectProfiles.Profiles.FirstOrDefault(p => p.Name == Convert.ToString(gridProjects.CurrentRow.Cells["Name"].Value));
                if (profile != null) ApplyProfile(profile);
            };
            buttons.Controls.Add(buttonAddProject);
            buttons.Controls.Add(buttonRemoveProject);
            buttons.Controls.Add(buttonApplyProject);
            page.Controls.Add(gridProjects);
            page.Controls.Add(buttons);
            tabControlSettings.Controls.Add(page);
        }

        private void AddPlatformFolderColumn(string name, string header)
        {
            gridProjects.Columns.Add(new DataGridViewTextBoxColumn { Name = name, HeaderText = header, Width = 180 });
        }

        private void LoadProfileGrid()
        {
            if (gridProjects == null) return;
            loadingProfiles = true;
            gridProjects.Rows.Clear();
            FrmMain main = (FrmMain)Application.OpenForms[nameof(FrmMain)];
            foreach (ProjectProfile profile in main.AppState.ProjectProfiles.Profiles)
            {
                int index = gridProjects.Rows.Add();
                DataGridViewRow row = gridProjects.Rows[index];
                row.Cells["Name"].Value = profile.Name;
                row.Cells["LegacyFolder"].Value = profile.LegacyFolder;
                foreach (Platform platform in profile.DefinedPlatforms) row.Cells[FolderColumn(platform)].Value = profile.GetFolder(platform);
                row.Cells["HashTable"].Value = profile.HashTable;
                row.Cells["CompatibilityTitle"].Value = profile.CompatibilityTitle;
            }
            loadingProfiles = false;
            profilesDirty = false;
            RefreshProjectSelector();
        }

        private void SaveProfileGrid()
        {
            if (gridProjects == null) return;
            gridProjects.EndEdit();
            FrmMain main = (FrmMain)Application.OpenForms[nameof(FrmMain)];
            main.AppState.ProjectProfiles.Profiles.Clear();
            foreach (DataGridViewRow row in gridProjects.Rows)
            {
                if (row.IsNewRow || string.IsNullOrWhiteSpace(Convert.ToString(row.Cells["Name"].Value))) continue;
                Title compatibility; Enum.TryParse(Convert.ToString(row.Cells["CompatibilityTitle"].Value), out compatibility);
                ProjectProfile profile = new ProjectProfile
                {
                    Name = Convert.ToString(row.Cells["Name"].Value),
                    LegacyFolder = Convert.ToString(row.Cells["LegacyFolder"].Value),
                    HashTable = Convert.ToString(row.Cells["HashTable"].Value),
                    CompatibilityTitle = compatibility
                };
                foreach (Platform platform in ProfilePlatforms) profile.SetFolder(platform, Convert.ToString(row.Cells[FolderColumn(platform)].Value));
                main.AppState.ProjectProfiles.Profiles.Add(profile);
            }
            main.AppState.ProjectProfiles.Save(ProjectsFilePath);
            profilesDirty = false;
            RefreshProjectSelector();
        }

        private void InitializeProjectSelector()
        {
            Panel panel = new Panel { Dock = DockStyle.Fill };
            Label label = new Label { Text = "Project:", AutoSize = true, Left = 4, Top = 8 };
            comboProjects = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Left = 58, Top = 4, Width = 220 };
            Label platformLabel = new Label { Text = "Platform:", AutoSize = true, Left = 288, Top = 8 };
            comboPlatforms = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Left = 350, Top = 4, Width = 130 };
            comboProjects.SelectedIndexChanged += delegate
            {
                if (loadingProfiles || comboProjects.SelectedIndex < 0) return;
                FrmMain main = (FrmMain)Application.OpenForms[nameof(FrmMain)];
                if (comboProjects.SelectedIndex >= main.AppState.ProjectProfiles.Profiles.Count) return;
                ProjectProfile profile = main.AppState.ProjectProfiles.Profiles[comboProjects.SelectedIndex];
                PopulatePlatformSelector(profile, true);
            };
            comboPlatforms.SelectedIndexChanged += delegate
            {
                if (loadingProfiles || comboPlatforms.SelectedItem == null) return;
                FrmMain main = (FrmMain)Application.OpenForms[nameof(FrmMain)];
                if (comboProjects.SelectedIndex >= 0 && comboProjects.SelectedIndex < main.AppState.ProjectProfiles.Profiles.Count)
                    ApplyProfilePlatform(main.AppState.ProjectProfiles.Profiles[comboProjects.SelectedIndex], (Platform)comboPlatforms.SelectedItem);
                else
                {
                    main.Configuration.PlatformSelected = (Platform)comboPlatforms.SelectedItem;
                    if (Directory.Exists(main.Configuration.ProjectFolder)) main.pnlSoundBankFiles.LoadData();
                    PropGridSettings.Refresh();
                }
            };
            panel.Controls.Add(label);
            panel.Controls.Add(comboProjects);
            panel.Controls.Add(platformLabel);
            panel.Controls.Add(comboPlatforms);
            tabPageGeneral.Controls.Remove(PropGridSettings);
            TableLayoutPanel layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Margin = Padding.Empty, Padding = Padding.Empty };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.Controls.Add(panel, 0, 0);
            layout.Controls.Add(PropGridSettings, 0, 1);
            tabPageGeneral.Controls.Add(layout);
        }

        private void RefreshProjectSelector()
        {
            if (comboProjects == null) return;
            FrmMain main = (FrmMain)Application.OpenForms[nameof(FrmMain)];
            loadingProfiles = true;
            comboProjects.Items.Clear();
            int selected = -1;
            for (int i = 0; i < main.AppState.ProjectProfiles.Profiles.Count; i++)
            {
                ProjectProfile profile = main.AppState.ProjectProfiles.Profiles[i];
                comboProjects.Items.Add(profile.Name);
                if (main.AppState.ProjectProfiles.FindForFolder(main.Configuration.ProjectFolder) == profile) selected = i;
            }
            comboProjects.SelectedIndex = selected;
            loadingProfiles = false;
            if (selected >= 0) PopulatePlatformSelector(main.AppState.ProjectProfiles.Profiles[selected], false);
            else PopulateManualPlatformSelector();
        }

        private void PopulateManualPlatformSelector()
        {
            FrmMain main = (FrmMain)Application.OpenForms[nameof(FrmMain)];
            bool previousLoading = loadingProfiles;
            loadingProfiles = true;
            comboPlatforms.Items.Clear();
            foreach (Platform platform in ProfilePlatforms) comboPlatforms.Items.Add(platform);
            comboPlatforms.SelectedIndex = comboPlatforms.Items.IndexOf(main.Configuration.PlatformSelected);
            loadingProfiles = previousLoading;
        }

        private void PopulatePlatformSelector(ProjectProfile profile, bool applySelection)
        {
            FrmMain main = (FrmMain)Application.OpenForms[nameof(FrmMain)];
            bool previousLoading = loadingProfiles;
            loadingProfiles = true;
            comboPlatforms.Items.Clear();
            Platform current = main.AppState.ProjectProfiles.FindPlatformForFolder(profile, main.Configuration.ProjectFolder);
            foreach (Platform platform in profile.DefinedPlatforms) comboPlatforms.Items.Add(platform);
            if (current == Platform.None && comboPlatforms.Items.Contains(main.Configuration.PlatformSelected)) current = main.Configuration.PlatformSelected;
            int selected = current == Platform.None ? (comboPlatforms.Items.Count > 0 ? 0 : -1) : comboPlatforms.Items.IndexOf(current);
            comboPlatforms.SelectedIndex = selected;
            loadingProfiles = previousLoading;
            if (applySelection && selected >= 0) ApplyProfilePlatform(profile, (Platform)comboPlatforms.Items[selected]);
        }

        private void ApplyProfile(ProjectProfile profile)
        {
            PopulatePlatformSelector(profile, true);
        }

        private void ApplyProfilePlatform(ProjectProfile profile, Platform platform)
        {
            string folder = profile.GetFolder(platform);
            if (string.IsNullOrWhiteSpace(folder)) return;
            FrmMain main = (FrmMain)Application.OpenForms[nameof(FrmMain)];
            main.Configuration.ProjectTitle = profile.Name;
            main.Configuration.TitleSelected = profile.CompatibilityTitle;
            main.Configuration.SoundhFile = profile.HashTable;
            main.Configuration.PlatformSelected = platform;
            if (!string.Equals(main.Configuration.ProjectFolder, folder, StringComparison.OrdinalIgnoreCase)) main.Configuration.ProjectFolder = folder;
            else main.pnlSoundBankFiles.LoadData();
            PropGridSettings.Refresh();
        }

        private static string FolderColumn(Platform platform)
        {
            switch (platform)
            {
                case Platform.GameCube: return "GCFolder";
                case Platform.Xbox360: return "Xbox360Folder";
                default: return platform + "Folder";
            }
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
            AddCodecRows(1, "MusX 1");
            AddCodecRows(201, "MusX 201");
            AddCodecRows(4, "MusX 4");
            AddCodecRows(5, "MusX 5");
            AddCodecRows(6, "MusX 6");
            AddCodecRow(6, "MusX 6", "Wii");
            AddCodecRow(6, "MusX 6", "XB2_");
            AddCodecRow(15, "MusX 10 / Stream 15", "XB2_");
            AddCodecRow(18, "MusX 10 / SBNK 18", "PC");
            AddCodecRow(18, "MusX 10 / SBNK 18", "PS2");
            AddCodecRow(18, "MusX 10 / SBNK 18", "PS3");
            AddCodecRow(18, "MusX 10 / SBNK 18", "Wii");
            AddCodecRow(18, "MusX 10 / SBNK 18", "XB2_");
            AddCodecRow(21, "MusX 10 / SBNK 21", "PC");
            AddCodecRow(21, "MusX 10 / SBNK 21", "PS2");
            AddCodecRow(21, "MusX 10 / SBNK 21", "PS3");
            AddCodecRow(21, "MusX 10 / SBNK 21", "Wii");
            AddCodecRow(21, "MusX 10 / SBNK 21", "XB2_");
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
            string soundBankCodec = EuroSoundCodecMatrix.GetCodec(version, platform, EuroSoundBankType.SoundBank).ToString();
            if (soundBankCodec == EuroSoundAudioCodec.DspAdpcmNgca.ToString())
                soundBankCodec = "Nintendo DSP ADPCM (NGCA)";
            else if (soundBankCodec == EuroSoundAudioCodec.DspAdpcmLegacy.ToString())
                soundBankCodec = version == 15 || version == 18
                    ? "Nintendo DSP ADPCM (Legacy/NGCA; per sample)"
                    : "Nintendo DSP ADPCM (Legacy)";
            else if (soundBankCodec == EuroSoundAudioCodec.DspAdpcm.ToString())
            {
                soundBankCodec = "Nintendo DSP ADPCM (external coefficients)";
            }
            gridCodecMatrix.Rows.Add(
                versionLabel,
                platform,
                soundBankCodec,
                EuroSoundCodecMatrix.GetCodec(version, platform, EuroSoundBankType.StreamBank).ToString(),
                EuroSoundCodecMatrix.GetCodec(version, platform, EuroSoundBankType.MusicBank).ToString());
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
