using MusX;
using MusX.Objects;
using MusX.Readers;
using sb_explorer.Classes;
using sb_explorer.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using static MusX.Readers.SfxFunctions;
using static sb_explorer.Enumerations;

namespace sb_explorer
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FormSoundBankFiles : DockContent
    {
        //Readers
        private readonly SoundBankReader reader = new SoundBankReader();
        private readonly StreamBankReader streamReader = new StreamBankReader();
        private readonly MusicBankReader musicReader = new MusicBankReader();
        private readonly SbiBankReader sbiReader = new SbiBankReader();
        private readonly ProjectDetailsReader projDetailsReader = new ProjectDetailsReader();
        private readonly SoundDetailsReader soundDetailsReader = new SoundDetailsReader();
        private readonly MusicDetailsReader musicDetailsReader = new MusicDetailsReader();
        private Dictionary<uint, EuroSoundSfxRadiusData> soundDetailsRadiusLookupCache;
        private string soundDetailsRadiusLookupCacheKey = string.Empty;

        public LoadedProjectData LoadedData { get; set; }

        //SoundBanks
        public SoundbankHeader SoundBankHeaderData { get { return LoadedData.SoundBankHeaderData; } set { LoadedData.SoundBankHeaderData = value; } }
        public SortedDictionary<uint, Sample> SfxSamples { get { return LoadedData.SfxSamples; } }
        public List<SampleData> SfxStoredData { get { return LoadedData.SfxStoredData; } }
        public List<uint> DuplicatedHashCodes { get { return LoadedData.DuplicatedHashCodes; } }

        //Streams
        public StreambankHeader StreamBankHeaderData { get { return LoadedData.StreamBankHeaderData; } set { LoadedData.StreamBankHeaderData = value; } }
        public List<StreamSample> StreamSamples { get { return LoadedData.StreamSamples; } }

        //Musics
        public StreambankHeader MusicBankHeaderData { get { return LoadedData.MusicBankHeaderData; } set { LoadedData.MusicBankHeaderData = value; } }
        public MusicSample MusicData { get { return LoadedData.MusicData; } set { LoadedData.MusicData = value; } }

        //SBI
        public SoundbankInfoHeader SbiBankHeaderData { get { return LoadedData.SbiBankHeaderData; } set { LoadedData.SbiBankHeaderData = value; } }
        public SbiFile SbiFileData { get { return LoadedData.SbiFileData; } set { LoadedData.SbiFileData = value; } }

        //Project Details
        public ProjectDetailsHeader ProjDetailsHeaderData { get { return LoadedData.ProjectDetailsHeaderData; } set { LoadedData.ProjectDetailsHeaderData = value; } }
        public ProjectDetails ProjDetailsData { get { return LoadedData.ProjectDetailsData; } set { LoadedData.ProjectDetailsData = value; } }

        //Sound Details
        public SfxCommonHeader SoundDetailsHeaderData { get { return LoadedData.SoundDetailsHeaderData; } set { LoadedData.SoundDetailsHeaderData = value; } }
        public SoundDetails SoundDetails { get { return LoadedData.SoundDetails; } set { LoadedData.SoundDetails = value; } }

        //Music Details
        public SfxCommonHeader MusicDetailsHeaderData { get { return LoadedData.MusicDetailsHeaderData; } set { LoadedData.MusicDetailsHeaderData = value; } }
        public MusicDetails MusicDetails { get { return LoadedData.MusicDetails; } set { LoadedData.MusicDetails = value; } }

        //-------------------------------------------------------------------------------------------
        //  MAIN FORM
        //-------------------------------------------------------------------------------------------
        public FormSoundBankFiles()
        {
            LoadedData = new LoadedProjectData();
            InitializeComponent();
        }

        private sealed class ProjectSfxExportOptions
        {
            public string ProjectFolder { get; set; }
            public string OutputFolder { get; set; }
            public string Platform { get; set; }
            public Title SelectedTitle { get; set; }
            public HashcodeParser Hashcodes { get; set; }
            public List<EuroSoundSfxTextSection> Sections { get; set; }
        }

        private sealed class ProjectSfxExportResult
        {
            public EuroSoundSfxTextExportResult ExportResult { get; set; }
            public int SoundbankCount { get; set; }
            public int FailedCount { get; set; }
            public bool Cancelled { get; set; }
            public List<string> FailedFiles { get; set; }

            public ProjectSfxExportResult()
            {
                ExportResult = new EuroSoundSfxTextExportResult();
                FailedFiles = new List<string>();
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void LoadData()
        {
            FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);
            HashcodeParser hashTable = parentForm.HashTable;
            Platform selectedPlatform = parentForm.Configuration.PlatformSelected;
            Title selectedTitle = parentForm.Configuration.TitleSelected;
            string folder = parentForm.Configuration.ProjectFolder;

            if (Directory.Exists(folder))
            {
                if (btnListView.Checked)
                {
                    FillListView(folder, selectedPlatform, selectedTitle, hashTable);
                    txtTotal.Text = lvwFiles.Items.Count.ToString();
                }
                else
                {
                    FillTreeView(folder, selectedPlatform, selectedTitle);
                    txtTotal.Text = treeView1.GetNodeCount(true).ToString();
                }
            }
        }

        //-------------------------------------------------------------------------------------------
        //  FORM BUTTONS
        //-------------------------------------------------------------------------------------------
        private void BtnReloadList_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void BtnTreeView_Click(object sender, EventArgs e)
        {
            if (btnListView.Checked)
            {
                btnListView.Checked = false;
                lvwFiles.Visible = false;
                MenuItem_CopyHashcode.Visible = false;
                MenuItem_CopyLabel.Visible = false;
                MenuItem_Separator1.Visible = false;
                lvwFiles.Items.Clear();
                treeView1.Visible = true;
                BtnReloadList_Click(sender, e);
            }
            else
            {
                btnTreeView.Checked = true;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void BtnListView_Click(object sender, EventArgs e)
        {
            if (btnTreeView.Checked)
            {
                btnTreeView.Checked = false;
                treeView1.Visible = false;
                treeView1.Nodes.Clear();
                lvwFiles.Visible = true;
                MenuItem_CopyHashcode.Visible = true;
                MenuItem_CopyLabel.Visible = true;
                MenuItem_Separator1.Visible = true;
                BtnReloadList_Click(sender, e);
            }
            else
            {
                btnListView.Checked = true;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void BtnApplyFilter_Click(object sender, EventArgs e)
        {
            if (btnApplyFilter.Checked)
            {
                if (!string.IsNullOrEmpty(txtFilter.Text))
                {
                    if (btnListView.Checked)
                    {
                        //Iterate through all list items
                        GenericMethods.FilterListView(txtFilter.Text, lvwFiles);

                        //Update Count
                        txtTotal.Text = lvwFiles.Items.Count.ToString();
                    }
                    else
                    {
                        //Iterate through all list items
                        GenericMethods.FilterTree(txtFilter.Text, treeView1);

                        //Update Count
                        txtTotal.Text = treeView1.GetNodeCount(true).ToString();
                    }
                }
            }
            else
            {
                LoadData();
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void BtnReloadHashCodes_Click(object sender, EventArgs e)
        {
            FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);
            parentForm.HashTable.LoadHashTable(parentForm.Configuration.SoundhFile);
        }

        //-------------------------------------------------------------------------------------------
        //  LISTVIEW EVENTS
        //-------------------------------------------------------------------------------------------
        private void LvwFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MenuItem_Load_Click(sender, e);
        }

        //-------------------------------------------------------------------------------------------
        //  TREEVIEW EVENTS
        //-------------------------------------------------------------------------------------------
        private void TreeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node != null && e.Node.Tag == null)
            {
                e.Node.ImageIndex = 1;
                e.Node.SelectedImageIndex = 1;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void TreeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node != null && e.Node.Tag == null)
            {
                e.Node.ImageIndex = 0;
                e.Node.SelectedImageIndex = 0;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void TreeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag != null)
            {
                //Get folder path
                string parentFolder = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.ProjectFolder;
                string filePath = Path.Combine(parentFolder, treeView1.SelectedNode.Name);

                //Get type of file
                FileType fileType = (FileType)treeView1.SelectedNode.Tag;
                switch (fileType)
                {
                    case FileType.SoundbankFile:
                        LoadSelectedSfx(filePath);
                        break;
                    case FileType.StreamFile:
                        LoadSelectedStream(filePath);
                        break;
                    case FileType.MusicFile:
                        LoadSelectedMusic(filePath);
                        break;
                    case FileType.SBI:
                        LoadSelectedSbi(filePath);
                        break;
                    case FileType.ProjectDetails:
                        LoadSelectedProjectDetails(filePath);
                        break;
                    case FileType.SoundDetailsFile:
                        LoadSelectedSoundDetails(filePath);
                        break;
                    case FileType.MusicDetails:
                        LoadSelectedMusicDetails(filePath);
                        break;
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void TreeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);
            }
        }

        //-------------------------------------------------------------------------------------------
        //  CONTEXT MENU
        //-------------------------------------------------------------------------------------------
        private void MenuItem_CopyHashcode_Click(object sender, EventArgs e)
        {
            if (btnListView.Checked && lvwFiles.SelectedItems.Count == 1)
            {
                Clipboard.SetText(lvwFiles.SelectedItems[0].SubItems[0].Text);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_CopyLabel_Click(object sender, EventArgs e)
        {
            if (btnListView.Checked && lvwFiles.SelectedItems.Count == 1)
            {
                Clipboard.SetText(lvwFiles.SelectedItems[0].SubItems[1].Text);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_Load_Click(object sender, EventArgs e)
        {
            if (btnListView.Checked && lvwFiles.SelectedItems.Count == 1)
            {
                //Get folder path
                string parentFolder = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.ProjectFolder;
                string filePath = Path.Combine(parentFolder, lvwFiles.SelectedItems[0].SubItems[2].Text.TrimStart('\\'));

                //This can crash if the user has not correctly selected the platform & title
                try
                {
                    //Get type of file
                    FileType fileType = (FileType)lvwFiles.SelectedItems[0].Tag;
                    switch (fileType)
                    {
                        case FileType.SoundbankFile:
                            LoadSelectedSfx(filePath);
                            lvwFiles.SelectedItems[0].SubItems[3].Text = "Loaded";
                            break;
                        case FileType.StreamFile:
                            LoadSelectedStream(filePath);
                            lvwFiles.SelectedItems[0].SubItems[3].Text = "Loaded";
                            break;
                        case FileType.MusicFile:
                            LoadSelectedMusic(filePath);
                            lvwFiles.SelectedItems[0].SubItems[3].Text = "Loaded";
                            break;
                        case FileType.SBI:
                            LoadSelectedSbi(filePath);
                            lvwFiles.SelectedItems[0].SubItems[3].Text = "Loaded";
                            break;
                        case FileType.ProjectDetails:
                            LoadSelectedProjectDetails(filePath);
                            lvwFiles.SelectedItems[0].SubItems[3].Text = "Loaded";
                            break;
                        case FileType.SoundDetailsFile:
                            LoadSelectedSoundDetails(filePath);
                            lvwFiles.SelectedItems[0].SubItems[3].Text = "Loaded";
                            break;
                        case FileType.MusicDetails:
                            LoadSelectedMusicDetails(filePath);
                            lvwFiles.SelectedItems[0].SubItems[3].Text = "Loaded";
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (treeView1.SelectedNode != null)
            {
                TreeView1_MouseDoubleClick(sender, null);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_Reload_Click(object sender, EventArgs e)
        {
            if (btnListView.Checked && lvwFiles.SelectedItems.Count == 1)
            {
                MenuItem_Load_Click(sender, e);
            }
            else if (treeView1.SelectedNode != null)
            {
                MenuItem_Load_Click(sender, null);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_Unload_Click(object sender, EventArgs e)
        {
            if (lvwFiles.SelectedItems.Count > 0)
            {
                foreach (ListViewItem itemToUnload in lvwFiles.SelectedItems)
                {
                    ClearLoadedData((FileType)itemToUnload.Tag);
                }
            }
            else if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag != null)
            {
                ClearLoadedData((FileType)treeView1.SelectedNode.Tag);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_DataViewer_Click(object sender, EventArgs e)
        {
            string parentFolder = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.ProjectFolder;

            //Get the selected file path
            string filePath = string.Empty;
            if (btnListView.Checked && lvwFiles.SelectedItems.Count == 1)
            {
                filePath = Path.Combine(parentFolder, lvwFiles.SelectedItems[0].SubItems[2].Text.TrimStart('\\'));
            }
            else if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag != null)
            {
                filePath = Path.Combine(parentFolder, treeView1.SelectedNode.Name);
            }

            //Show the selected file if exists
            if (File.Exists(filePath))
            {
                using (FrmDataViewer dataViewer = new FrmDataViewer(filePath))
                {
                    dataViewer.ShowDialog();
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_ExportSfxToEuroSoundFile_Click(object sender, EventArgs e)
        {
            ExportSfxSamplesToEuroSoundFile(SfxSamples.Values);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_ExportProjectSfxToEuroSoundFiles_Click(object sender, EventArgs e)
        {
            ExportProjectSfxSamplesToEuroSoundFiles();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ExportSfxSamplesToEuroSoundFile(IEnumerable<Sample> samples)
        {
            List<Sample> samplesToExport = new List<Sample>(samples);
            if (samplesToExport.Count == 0)
            {
                MessageBox.Show("Load a soundbank or select at least one SFX first.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!TrySelectEuroSoundSections(out List<EuroSoundSfxTextSection> sections))
            {
                return;
            }

            if (!TrySelectEuroSoundOutputFolder(out string outputFolder))
            {
                return;
            }

            try
            {
                FrmMain parentForm = (FrmMain)Application.OpenForms[nameof(FrmMain)];
                int fileVersion = SoundBankHeaderData.FileVersion != 0 ? SoundBankHeaderData.FileVersion : parentForm.Configuration.FileVersion;
                Dictionary<uint, EuroSoundSfxRadiusData> radii = BuildSoundDetailsRadiusLookup(SoundDetails);
                EuroSoundSfxTextExportResult result = EuroSoundSfxTextExporter.Export(samplesToExport, fileVersion, parentForm.HashTable, outputFolder, sections, radii);

                ShowEuroSoundExportResult(result, 1, 0, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ExportProjectSfxSamplesToEuroSoundFiles()
        {
            FrmMain parentForm = (FrmMain)Application.OpenForms[nameof(FrmMain)];
            string projectFolder = parentForm.Configuration.ProjectFolder;
            if (!Directory.Exists(projectFolder))
            {
                MessageBox.Show("Select a valid project folder first.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!TrySelectEuroSoundSections(out List<EuroSoundSfxTextSection> sections))
            {
                return;
            }

            if (!TrySelectEuroSoundOutputFolder(out string outputFolder))
            {
                return;
            }

            ProjectSfxExportOptions options = new ProjectSfxExportOptions
            {
                ProjectFolder = projectFolder,
                OutputFolder = outputFolder,
                Platform = parentForm.Configuration.PlatformSelected.ToString(),
                SelectedTitle = parentForm.Configuration.TitleSelected,
                Hashcodes = parentForm.HashTable,
                Sections = sections
            };

            using (FrmProgress progressForm = new FrmProgress(
                "Export Project SFXs to EuroSound Files",
                "Preparing project export...",
                worker => ExportProjectSfxSamplesWorker(options, worker)))
            {
                progressForm.ShowDialog(this);

                if (progressForm.Error != null)
                {
                    MessageBox.Show(progressForm.Error.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ProjectSfxExportResult result = (ProjectSfxExportResult)progressForm.Result;
                if (result == null)
                {
                    return;
                }

                if (result.Cancelled)
                {
                    MessageBox.Show("Project export was cancelled.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (result.SoundbankCount == 0 && result.FailedCount == 0)
                {
                    MessageBox.Show("No SoundBank files were found in the project folder.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ShowEuroSoundExportResult(result.ExportResult, result.SoundbankCount, result.FailedCount, result.FailedFiles);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private ProjectSfxExportResult ExportProjectSfxSamplesWorker(ProjectSfxExportOptions options, BackgroundWorker worker)
        {
            ProjectSfxExportResult totalResult = new ProjectSfxExportResult();
            SoundBankReader soundBankReader = new SoundBankReader();
            SoundDetailsReader detailsReader = new SoundDetailsReader();
            string[] files = Directory.GetFiles(options.ProjectFolder, "*.sfx", SearchOption.AllDirectories);
            Dictionary<uint, EuroSoundSfxRadiusData> radii = BuildProjectSoundDetailsRadiusLookup(files, options, soundBankReader, detailsReader);

            for (int i = 0; i < files.Length; i++)
            {
                if (worker.CancellationPending)
                {
                    totalResult.Cancelled = true;
                    return totalResult;
                }

                string filePath = files[i];
                int progress = files.Length == 0 ? 100 : (int)(((i + 1) * 100.0) / files.Length);
                worker.ReportProgress(progress, "Inspecting " + Path.GetFileName(filePath));

                try
                {
                    SfxCommonHeader commonHeader = soundBankReader.ReadCommonHeader(filePath, options.Platform);
                    FileType fileType = GenericMethods.GetFileType((int)commonHeader.FileHashCode, commonHeader.FileVersion, filePath, options.SelectedTitle);
                    if (fileType != FileType.SoundbankFile)
                    {
                        continue;
                    }

                    worker.ReportProgress(progress, "Exporting " + Path.GetFileName(filePath));

                    SoundbankHeader header = soundBankReader.ReadSfxHeader(filePath, options.Platform);
                    SortedDictionary<uint, Sample> samples = new SortedDictionary<uint, Sample>();
                    List<SampleData> storedData = new List<SampleData>();
                    List<uint> duplicates = new List<uint>();

                    soundBankReader.ReadSoundBank(filePath, header, samples, storedData, duplicates);
                    EuroSoundSfxTextExportResult fileResult = EuroSoundSfxTextExporter.Export(samples.Values, header.FileVersion, options.Hashcodes, options.OutputFolder, options.Sections, radii);

                    totalResult.ExportResult.ExportedCount += fileResult.ExportedCount;
                    totalResult.ExportResult.CreatedCount += fileResult.CreatedCount;
                    totalResult.ExportResult.UpdatedCount += fileResult.UpdatedCount;
                    totalResult.SoundbankCount++;
                }
                catch (Exception ex)
                {
                    totalResult.FailedFiles.Add(Path.GetFileName(filePath) + ": " + ex.Message);
                    totalResult.FailedCount++;
                }
            }

            worker.ReportProgress(100, "Done");
            return totalResult;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private Dictionary<uint, EuroSoundSfxRadiusData> BuildProjectSoundDetailsRadiusLookup(string[] files, ProjectSfxExportOptions options, SoundBankReader soundBankReader, SoundDetailsReader detailsReader)
        {
            Dictionary<uint, EuroSoundSfxRadiusData> radii = new Dictionary<uint, EuroSoundSfxRadiusData>();

            foreach (string filePath in files)
            {
                try
                {
                    SfxCommonHeader commonHeader = soundBankReader.ReadCommonHeader(filePath, options.Platform);
                    FileType fileType = GenericMethods.GetFileType((int)commonHeader.FileHashCode, commonHeader.FileVersion, filePath, options.SelectedTitle);
                    if (fileType != FileType.SoundDetailsFile)
                    {
                        continue;
                    }

                    SoundDetails details = detailsReader.ReadSoundDetailsFile(filePath, commonHeader);
                    AddSoundDetailsRadii(radii, details);
                }
                catch (Exception)
                {
                    // SoundDetails files are optional for export; unreadable files are ignored here and handled later if they are SoundBanks.
                }
            }

            return radii;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private Dictionary<uint, EuroSoundSfxRadiusData> BuildSoundDetailsRadiusLookup(SoundDetails details)
        {
            Dictionary<uint, EuroSoundSfxRadiusData> radii = new Dictionary<uint, EuroSoundSfxRadiusData>();
            AddSoundDetailsRadii(radii, details);
            return radii;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void AddSoundDetailsRadii(Dictionary<uint, EuroSoundSfxRadiusData> radii, SoundDetails details)
        {
            if (details == null || details.sfxItems == null)
            {
                return;
            }

            foreach (SoundDetailsData item in details.sfxItems)
            {
                uint hashCode = unchecked((uint)item.HashCode);
                EuroSoundSfxRadiusData radiusData = new EuroSoundSfxRadiusData
                {
                    InnerRadius = unchecked((short)item.InnerRadius),
                    OuterRadius = unchecked((short)item.OuterRadius)
                };

                radii[hashCode] = radiusData;
                radii[hashCode & 0x00FFFFFF] = radiusData;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public bool TryGetSoundDetailsRadius(uint hashCode, out EuroSoundSfxRadiusData radiusData)
        {
            radiusData = null;

            Dictionary<uint, EuroSoundSfxRadiusData> loadedRadii = BuildSoundDetailsRadiusLookup(SoundDetails);
            if (TryGetSoundDetailsRadius(loadedRadii, hashCode, out radiusData))
            {
                return true;
            }

            FrmMain parentForm = (FrmMain)Application.OpenForms[nameof(FrmMain)];
            string projectFolder = parentForm.Configuration.ProjectFolder;
            if (!Directory.Exists(projectFolder))
            {
                return false;
            }

            string cacheKey = projectFolder + "|" + parentForm.Configuration.PlatformSelected + "|" + parentForm.Configuration.TitleSelected;
            if (soundDetailsRadiusLookupCache == null || soundDetailsRadiusLookupCacheKey != cacheKey)
            {
                ProjectSfxExportOptions options = new ProjectSfxExportOptions
                {
                    ProjectFolder = projectFolder,
                    Platform = parentForm.Configuration.PlatformSelected.ToString(),
                    SelectedTitle = parentForm.Configuration.TitleSelected
                };

                string[] files = Directory.GetFiles(projectFolder, "*.sfx", SearchOption.AllDirectories);
                soundDetailsRadiusLookupCache = BuildProjectSoundDetailsRadiusLookup(files, options, reader, soundDetailsReader);
                soundDetailsRadiusLookupCacheKey = cacheKey;
            }

            return TryGetSoundDetailsRadius(soundDetailsRadiusLookupCache, hashCode, out radiusData);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private bool TryGetSoundDetailsRadius(Dictionary<uint, EuroSoundSfxRadiusData> radii, uint hashCode, out EuroSoundSfxRadiusData radiusData)
        {
            radiusData = null;
            if (radii == null)
            {
                return false;
            }

            return radii.TryGetValue(hashCode, out radiusData) || radii.TryGetValue(hashCode & 0x00FFFFFF, out radiusData);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private bool TrySelectEuroSoundSections(out List<EuroSoundSfxTextSection> sections)
        {
            sections = new List<EuroSoundSfxTextSection>();

            using (FrmEuroSoundSfxExportSection form = new FrmEuroSoundSfxExportSection())
            {
                if (form.ShowDialog(this) != DialogResult.OK)
                {
                    return false;
                }

                sections = form.SelectedSections;
                return true;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private bool TrySelectEuroSoundOutputFolder(out string outputFolder)
        {
            outputFolder = string.Empty;

            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Select the folder where EuroSound TXT files will be created or updated.";
                if (folderBrowserDialog.ShowDialog(this) != DialogResult.OK)
                {
                    return false;
                }

                outputFolder = folderBrowserDialog.SelectedPath;
                return true;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ShowEuroSoundExportResult(EuroSoundSfxTextExportResult result, int soundbankCount, int failedCount, List<string> failedFiles)
        {
            string message = string.Format(
                "Exported {0} SFX files.\r\nCreated: {1}\r\nUpdated: {2}\r\nSoundBanks: {3}",
                result.ExportedCount,
                result.CreatedCount,
                result.UpdatedCount,
                soundbankCount);

            if (failedCount > 0)
            {
                message += string.Format("\r\nFailed files: {0}", failedCount);
                int maxFailuresToShow = Math.Min(failedFiles.Count, 5);
                for (int i = 0; i < maxFailuresToShow; i++)
                {
                    message += "\r\n" + failedFiles[i];
                }
            }

            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, failedCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
        }

        //-------------------------------------------------------------------------------------------
        //  FUNCTIONS
        //-------------------------------------------------------------------------------------------
        private void LoadSelectedSfx(string filePath)
        {
            ClearLoadedData(FileType.SoundbankFile);

            //Load data
            SoundBankHeaderData = reader.ReadSfxHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.PlatformSelected.ToString());
            reader.ReadSoundBank(filePath, SoundBankHeaderData, SfxSamples, SfxStoredData, DuplicatedHashCodes);

            //Display Data
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSbHashCodes.SetHashCodesToListView();
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlWavHeaderData.ShowWavesList();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void LoadSelectedStream(string filePath)
        {
            ClearLoadedData(FileType.StreamFile);

            //Load data
            StreamBankHeaderData = streamReader.ReadStreamBankHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.PlatformSelected.ToString());
            streamReader.ReadStreamBank(filePath, StreamBankHeaderData, StreamSamples);

            //Display Data
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlStreamData.ShowStreamData();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void LoadSelectedMusic(string filePath)
        {
            ClearLoadedData(FileType.MusicFile);

            //Load data
            MusicBankHeaderData = musicReader.ReadMusicHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.PlatformSelected.ToString());
            MusicData = musicReader.ReadMusicBank(filePath, MusicBankHeaderData);

            //Display Data
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlMusicData.ShowMusicData();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void LoadSelectedSbi(string filePath)
        {
            ClearLoadedData(FileType.SBI);

            //Load data
            SbiBankHeaderData = sbiReader.ReadSoundbankInfoHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.PlatformSelected.ToString());
            SbiFileData = sbiReader.ReadSoundbankInfoFile(filePath, SbiBankHeaderData);

            //Display Data
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSbiSoundbanks.DisplayHashCodes();
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSbiMusicbanks.DisplayHashCodes();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void LoadSelectedProjectDetails(string filePath)
        {
            ClearLoadedData(FileType.ProjectDetails);

            //Load data
            ProjDetailsHeaderData = projDetailsReader.ReadProjectFileHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.PlatformSelected.ToString());
            ProjDetailsData = projDetailsReader.ReadProjectFile(filePath, ProjDetailsHeaderData);

            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlProjDetailsMemSlots.ShowData();
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlProjDetailsSoundBanks.ShowData();
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlProjDetailsData.ShowData();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void LoadSelectedSoundDetails(string filePath)
        {
            ClearLoadedData(FileType.SoundDetailsFile);

            //Load data
            SoundDetailsHeaderData = soundDetailsReader.ReadCommonHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.PlatformSelected.ToString());
            SoundDetails = soundDetailsReader.ReadSoundDetailsFile(filePath, ProjDetailsHeaderData);

            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundDetailsData.ShowData();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void LoadSelectedMusicDetails(string filePath)
        {
            ClearLoadedData(FileType.MusicDetails);

            //Load data
            MusicDetailsHeaderData = musicDetailsReader.ReadCommonHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.PlatformSelected.ToString());
            MusicDetails = musicDetailsReader.ReadMusicDetailsFile(filePath, ProjDetailsHeaderData);

            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlMusicDetailsData.ShowData();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ClearLoadedData(FileType fileType)
        {
            FrmMain mainForm = (FrmMain)Application.OpenForms[nameof(FrmMain)];

            //Clear loaded data and UI
            switch (fileType)
            {
                case FileType.SoundbankFile:
                    SoundBankHeaderData = new SoundbankHeader();
                    SfxSamples.Clear();
                    SfxStoredData.Clear();
                    DuplicatedHashCodes.Clear();

                    //Clear UI
                    mainForm.pnlSbHashCodes.listView1.Items.Clear();
                    mainForm.pnlWavHeaderData.listView1.Items.Clear();
                    mainForm.pnlSbSamplePool.listView1.Items.Clear();
                    break;
                case FileType.StreamFile:
                    StreamBankHeaderData = new StreambankHeader();
                    StreamSamples.Clear();

                    //Clear UI
                    mainForm.pnlMarkers.lvwMarkers.Items.Clear();
                    mainForm.pnlStartMarkers.lvwStartMarkers.Items.Clear();
                    mainForm.pnlStreamData.lvwStreamData.Items.Clear();
                    break;
                case FileType.MusicFile:
                    MusicBankHeaderData = new StreambankHeader();
                    MusicData = new MusicSample();

                    //Clear UI
                    mainForm.pnlMusicData.propertyGrid1.SelectedObject = null;
                    break;
                case FileType.SBI:
                    SbiBankHeaderData = new SoundbankInfoHeader();
                    SbiFileData = new SbiFile();

                    //Clear UI
                    mainForm.pnlSbiMusicbanks.textBoxSoundBanksCount.Text = "0";
                    mainForm.pnlSbiMusicbanks.listView_ColumnSortingClick1.Items.Clear();
                    mainForm.pnlSbiSoundbanks.textBoxSoundBanksCount.Text = "0";
                    mainForm.pnlSbiSoundbanks.listView_ColumnSortingClick1.Items.Clear();
                    break;
                case FileType.ProjectDetails:
                    ProjDetailsHeaderData = new ProjectDetailsHeader();
                    ProjDetailsData = new ProjectDetails();

                    //Clear UI
                    mainForm.pnlProjDetailsSoundBanks.textboxCount.Text = "0";
                    mainForm.pnlProjDetailsSoundBanks.listView_ColumnSortingClick1.Items.Clear();
                    mainForm.pnlProjDetailsMemSlots.textboxCount.Text = "0";
                    mainForm.pnlProjDetailsMemSlots.listView_ColumnSortingClick1.Items.Clear();
                    mainForm.pnlProjDetailsData.ClearData();
                    break;
                case FileType.SoundDetailsFile:
                    SoundDetailsHeaderData = new SoundbankHeader();
                    SoundDetails = new SoundDetails();

                    //Clear UI
                    mainForm.pnlSoundDetailsData.ClearData();
                    break;
                case FileType.MusicDetails:
                    MusicDetailsHeaderData = new StreambankHeader();
                    MusicDetails = new MusicDetails();

                    //Clear UI
                    mainForm.pnlMusicDetailsData.ClearData();
                    break;
            }

            //Update ListView
            lvwFiles.BeginUpdate();
            foreach (ListViewItem itemToModify in lvwFiles.Items)
            {
                if ((FileType)itemToModify.Tag == fileType)
                {
                    itemToModify.SubItems[3].Text = "Unloaded";
                }
            }
            lvwFiles.EndUpdate();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private int GetNumberOfSFXs(string filePath, FileType fileType)
        {
            int total = 0;
            switch (fileType)
            {
                case FileType.SoundbankFile:
                    SoundbankHeader sbData = reader.ReadSfxHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.PlatformSelected.ToString());
                    total = reader.GetNumberOfSFXs(filePath, sbData);
                    break;
                case FileType.StreamFile:
                    StreambankHeader strData = streamReader.ReadStreamBankHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).Configuration.PlatformSelected.ToString());
                    total = (int)(strData.FileLength1 / 4);
                    break;
                case FileType.MusicFile:
                    total = 1;
                    break;
            }
            return total;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void FillListView(string folder, Platform selectedPlatform, Title selectedTitle, HashcodeParser hashTable)
        {
            string[] files = Directory.GetFiles(folder, "*.sfx", SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                lvwFiles.BeginUpdate();
                lvwFiles.Items.Clear();
                for (int i = 0; i < files.Length; i++)
                {
                    SfxCommonHeader headerDat = reader.ReadCommonHeader(files[i], selectedPlatform.ToString());

                    //Get version of MusX Files
                    FileType fileType = GenericMethods.GetFileType((int)headerDat.FileHashCode, headerDat.FileVersion, files[i], selectedTitle);

                    //Create item
                    ListViewItem itemToAdd = new ListViewItem(new string[]
                    {
                        string.Format("0x{0:X8}", headerDat.FileHashCode),
                        hashTable.GetHashCodeLabel((uint)GenericMethods.GetHashCodeWithSection(fileType, (int)headerDat.FileHashCode, headerDat.FileVersion, selectedTitle)),
                        files[i].Substring(folder.Length),
                        "Unloaded",
                        GenericMethods.GetFileSize(files[i]),
                        GetNumberOfSFXs(files[i], fileType).ToString(),
                        fileType.ToString()
                    })
                    {
                        UseItemStyleForSubItems = false,
                        Tag = fileType,
                        ImageIndex = 2
                    };

                    //Check if we need to highlight this item
                    if (itemToAdd.SubItems[1].Text.StartsWith("**"))
                    {
                        itemToAdd.SubItems[1].ForeColor = Color.Red;
                    }

                    //Add item to listview
                    lvwFiles.Items.Add(itemToAdd);
                }
                lvwFiles.EndUpdate();
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void FillTreeView(string folder, Platform selectedPlatform, Title selectedTitle)
        {
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();
            DirectoryInfo rootDirectoryInfo = new DirectoryInfo(folder);
            treeView1.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo, selectedPlatform, selectedTitle));
            if (treeView1.Nodes.Count > 0 && treeView1.Nodes[0].Nodes.Count > 0)
            {
                treeView1.Nodes[0].Expand();
            }
            treeView1.EndUpdate();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo, Platform selectedPlatform, Title selectedTitle)
        {
            TreeNode directoryNode = new TreeNode(directoryInfo.Name);
            foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
            {
                //Create node and set image
                TreeNode nodeData = CreateDirectoryNode(directory, selectedPlatform, selectedTitle);
                nodeData.ImageIndex = 0;
                nodeData.SelectedImageIndex = 0;
                //Add node
                directoryNode.Nodes.Add(nodeData);
            }
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (Path.GetExtension(file.Name).Equals(".sfx", StringComparison.OrdinalIgnoreCase))
                {
                    SfxCommonHeader headerData = reader.ReadCommonHeader(file.FullName, selectedPlatform.ToString());

                    //Get version of MusX Files
                    FileType fileType = GenericMethods.GetFileType((int)headerData.FileHashCode, headerData.FileVersion, file.FullName, selectedTitle);

                    //Create Node
                    TreeNode fileNode = new TreeNode(file.Name)
                    {
                        Name = file.FullName,
                        Text = file.Name,
                        Tag = fileType,
                        ImageIndex = 2,
                        SelectedImageIndex = 2
                    };
                    directoryNode.Nodes.Add(fileNode);
                }
            }
            return directoryNode;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
