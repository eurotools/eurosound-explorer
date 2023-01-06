using MusX;
using MusX.Objects;
using MusX.Readers;
using sb_explorer.Classes;
using System;
using System.Collections.Generic;
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

        //SoundBanks
        public SfxHeaderData soundBankHeaderData = new SfxHeaderData();
        public readonly SortedDictionary<uint, Sample> sfxSamples = new SortedDictionary<uint, Sample>();
        public readonly List<SampleData> sfxStoredData = new List<SampleData>();

        //Streams
        public SfxHeaderData streamBankHeaderData = new SfxHeaderData();
        public readonly List<StreamSample> streamSamples = new List<StreamSample>();

        //Musics
        public SfxHeaderData musicBankHeaderData = new SfxHeaderData();
        public MusicSample musicData = new MusicSample();

        //SBI
        public SfxHeaderData sbiBankHeaderData = new SfxHeaderData();
        public SbiFile sbiFileData = new SbiFile();

        //Project Details
        public SfxHeaderData projDetailsHeaderData = new SfxHeaderData();
        public ProjectDetails projDetailsData = new ProjectDetails();

        //Sound Details
        public SfxHeaderData soundDetailsHeaderData = new SfxHeaderData();
        public SoundDetails soundDetails = new SoundDetails();

        //Music Details
        public SfxHeaderData musicDetailsHeaderData = new SfxHeaderData();
        public MusicDetails musicDetails = new MusicDetails();

        //-------------------------------------------------------------------------------------------
        //  MAIN FORM
        //-------------------------------------------------------------------------------------------
        public FormSoundBankFiles()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void LoadData()
        {
            FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);
            HashcodeParser hashTable = parentForm.hashTable;
            int selectedVersion = parentForm.configuration.FileVersion;
            string folder = parentForm.configuration.ProjectFolder;
            Title selectedTitle = parentForm.configuration.TitleSelected;

            if (Directory.Exists(folder))
            {
                if (btnListView.Checked)
                {
                    FillListView(folder, selectedVersion, selectedTitle, hashTable);
                    txtTotal.Text = lvwFiles.Items.Count.ToString();
                }
                else
                {
                    FillTreeView(folder, selectedVersion, selectedTitle);
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
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).hashTable.LoadHashTable();
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
                string parentFolder = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.ProjectFolder;
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
                string parentFolder = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.ProjectFolder;
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
            string parentFolder = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.ProjectFolder;

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

        //-------------------------------------------------------------------------------------------
        //  FUNCTIONS
        //-------------------------------------------------------------------------------------------
        private void LoadSelectedSfx(string filePath)
        {
            ClearLoadedData(FileType.SoundbankFile);

            //Load data
            soundBankHeaderData = reader.ReadSfxHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected.ToString());
            reader.ReadSoundBank(filePath, soundBankHeaderData, sfxSamples, sfxStoredData);

            //Display Data
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSbHashCodes.SetHashCodesToListView();
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlWavHeaderData.ShowWavesList();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void LoadSelectedStream(string filePath)
        {
            ClearLoadedData(FileType.StreamFile);

            //Load data
            streamBankHeaderData = streamReader.ReadSfxHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected.ToString());
            streamReader.ReadStreamBank(filePath, streamBankHeaderData, streamSamples);

            //Display Data
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlStreamData.ShowStreamData();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void LoadSelectedMusic(string filePath)
        {
            ClearLoadedData(FileType.MusicFile);

            //Load data
            musicBankHeaderData = musicReader.ReadSfxHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected.ToString());
            musicData = musicReader.ReadMusicBank(filePath, musicBankHeaderData);

            //Display Data
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlMusicData.ShowMusicData();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void LoadSelectedSbi(string filePath)
        {
            ClearLoadedData(FileType.SBI);

            //Load data
            sbiBankHeaderData = sbiReader.ReadSfxHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected.ToString());
            sbiFileData = sbiReader.ReadStreamFile(filePath, sbiBankHeaderData);

            //Display Data
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSbiSoundbanks.DisplayHashCodes();
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSbiMusicbanks.DisplayHashCodes();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void LoadSelectedProjectDetails(string filePath)
        {
            ClearLoadedData(FileType.ProjectDetails);

            //Load data
            projDetailsHeaderData = projDetailsReader.ReadSfxHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected.ToString());
            projDetailsData = projDetailsReader.ReadProjectFile(filePath, projDetailsHeaderData);

            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlProjDetailsMemSlots.ShowData();
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlProjDetailsSoundBanks.ShowData();
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlProjDetailsData.ShowData();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void LoadSelectedSoundDetails(string filePath)
        {
            ClearLoadedData(FileType.SoundDetailsFile);

            //Load data
            soundDetailsHeaderData = soundDetailsReader.ReadSfxHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected.ToString());
            soundDetails = soundDetailsReader.ReadSoundDetailsFile(filePath, projDetailsHeaderData);

            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundDetailsData.ShowData();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void LoadSelectedMusicDetails(string filePath)
        {
            ClearLoadedData(FileType.MusicDetails);

            //Load data
            musicDetailsHeaderData = musicDetailsReader.ReadSfxHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected.ToString());
            musicDetails = musicDetailsReader.ReadSoundDetailsFile(filePath, projDetailsHeaderData);

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
                    soundBankHeaderData = new SfxHeaderData();
                    sfxSamples.Clear();
                    sfxStoredData.Clear();

                    //Clear UI
                    mainForm.pnlSbHashCodes.listView1.Items.Clear();
                    mainForm.pnlWavHeaderData.listView1.Items.Clear();
                    mainForm.pnlSbSamplePool.listView1.Items.Clear();
                    break;
                case FileType.StreamFile:
                    streamBankHeaderData = new SfxHeaderData();
                    streamSamples.Clear();

                    //Clear UI
                    mainForm.pnlMarkers.lvwMarkers.Items.Clear();
                    mainForm.pnlStartMarkers.lvwStartMarkers.Items.Clear();
                    mainForm.pnlStreamData.lvwStreamData.Items.Clear();
                    break;
                case FileType.MusicFile:
                    musicBankHeaderData = new SfxHeaderData();
                    musicData = new MusicSample();

                    //Clear UI
                    mainForm.pnlMusicData.propertyGrid1.SelectedObject = null;
                    break;
                case FileType.SBI:
                    sbiBankHeaderData = new SfxHeaderData();
                    sbiFileData = new SbiFile();

                    //Clear UI
                    mainForm.pnlSbiMusicbanks.textBoxSoundBanksCount.Text = "0";
                    mainForm.pnlSbiMusicbanks.listView_ColumnSortingClick1.Items.Clear();
                    mainForm.pnlSbiSoundbanks.textBoxSoundBanksCount.Text = "0";
                    mainForm.pnlSbiSoundbanks.listView_ColumnSortingClick1.Items.Clear();
                    break;
                case FileType.ProjectDetails:
                    projDetailsHeaderData = new SfxHeaderData();
                    projDetailsData = new ProjectDetails();

                    //Clear UI
                    mainForm.pnlProjDetailsSoundBanks.textboxCount.Text = "0";
                    mainForm.pnlProjDetailsSoundBanks.listView_ColumnSortingClick1.Items.Clear();
                    mainForm.pnlProjDetailsMemSlots.textboxCount.Text = "0";
                    mainForm.pnlProjDetailsMemSlots.listView_ColumnSortingClick1.Items.Clear();
                    mainForm.pnlProjDetailsData.ClearData();
                    break;
                case FileType.SoundDetailsFile:
                    soundDetailsHeaderData = new SfxHeaderData();
                    soundDetails = new SoundDetails();

                    //Clear UI
                    mainForm.pnlSoundDetailsData.ClearData();
                    break;
                case FileType.MusicDetails:
                    musicDetailsHeaderData = new SfxHeaderData();
                    musicDetails = new MusicDetails();

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
                    SfxHeaderData sbData = reader.ReadSfxHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected.ToString());
                    total = reader.GetNumberOfSFXs(filePath, sbData);
                    break;
                case FileType.StreamFile:
                    sbData = streamReader.ReadSfxHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected.ToString());
                    total = (int)(sbData.FileLength1 / 4);
                    break;
                case FileType.MusicFile:
                    total = 1;
                    break;
            }
            return total;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void FillListView(string folder, int selectedVersion, Title selectedTitle, HashcodeParser hashTable)
        {
            string[] files = Directory.GetFiles(folder, "*.sfx", SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                lvwFiles.BeginUpdate();
                lvwFiles.Items.Clear();
                for (int i = 0; i < files.Length; i++)
                {
                    int hashCode = reader.GetFileHashCode(files[i]);

                    //Get version of MusX Files
                    FileType fileType = GenericMethods.GetFileType(hashCode, selectedVersion, files[i], selectedTitle);

                    //Create item
                    ListViewItem itemToAdd = new ListViewItem(new string[]
                    {
                            string.Format("0x{0:X8}", hashCode),
                            hashTable.GetHashCodeLabel((uint)GenericMethods.GetHashCodeWithSection(fileType, hashCode, selectedVersion, selectedTitle)),
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
        private void FillTreeView(string folder, int selectedVersion, Title selectedTitle)
        {
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();
            DirectoryInfo rootDirectoryInfo = new DirectoryInfo(folder);
            treeView1.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo, selectedVersion, selectedTitle));
            if (treeView1.Nodes.Count > 0 && treeView1.Nodes[0].Nodes.Count > 0)
            {
                treeView1.Nodes[0].Expand();
            }
            treeView1.EndUpdate();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo, int selectedVersion, Title selectedTitle)
        {
            TreeNode directoryNode = new TreeNode(directoryInfo.Name);
            foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
            {
                //Create node and set image
                TreeNode nodeData = CreateDirectoryNode(directory, selectedVersion, selectedTitle);
                nodeData.ImageIndex = 0;
                nodeData.SelectedImageIndex = 0;
                //Add node
                directoryNode.Nodes.Add(nodeData);
            }
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (Path.GetExtension(file.Name).Equals(".sfx", StringComparison.OrdinalIgnoreCase))
                {
                    int hashCode = reader.GetFileHashCode(file.FullName);

                    //Get version of MusX Files
                    FileType fileType = GenericMethods.GetFileType(hashCode, selectedVersion, file.FullName, selectedTitle);

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
