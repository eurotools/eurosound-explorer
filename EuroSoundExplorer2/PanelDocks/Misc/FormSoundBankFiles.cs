﻿using EuroSoundExplorer2.Classes;
using MusX;
using MusX.Objects;
using MusX.Readers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using static EuroSoundExplorer2.AppConfig;
using static MusX.Readers.SfxFunctions;

namespace EuroSoundExplorer2
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
        private readonly SbiReader sbiReader = new SbiReader();

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

        //-------------------------------------------------------------------------------------------
        //  MAIN FORM
        //-------------------------------------------------------------------------------------------
        public FormSoundBankFiles()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------
        //  FORM BUTTONS
        //-------------------------------------------------------------------------------------------
        private void BtnReloadList_Click(object sender, EventArgs e)
        {
            FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);
            HashcodeParser hashTable = parentForm.hashTable;
            int selectedVersion = parentForm.configuration.FileVersion;
            string folder = parentForm.configuration.ProjectFolder;
            Title selectedTitle = parentForm.configuration.TitleSelected;

            if (Directory.Exists(folder))
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
                        { UseItemStyleForSubItems = false, Tag = fileType };

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
                txtTotal.Text = lvwFiles.Items.Count.ToString();
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void BtnReloadHashCodes_Click(object sender, EventArgs e)
        {
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).hashTable.LoadHashTable();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void LvwFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MenuItem_Load_Click(sender, e);
        }

        //-------------------------------------------------------------------------------------------
        //  CONTEXT MENU
        //-------------------------------------------------------------------------------------------
        private void MenuItem_Load_Click(object sender, EventArgs e)
        {
            if (lvwFiles.SelectedItems.Count == 1)
            {
                //Get folder path
                string parentFolder = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.ProjectFolder;
                string filePath = Path.Combine(parentFolder, lvwFiles.SelectedItems[0].SubItems[2].Text.TrimStart('\\'));

                //Get type of file
                FileType fileType = (FileType)lvwFiles.SelectedItems[0].Tag;
                switch (fileType)
                {
                    case FileType.SoundBank:
                        LoadSelectedSfx(filePath);
                        lvwFiles.SelectedItems[0].SubItems[3].Text = "Loaded";
                        break;
                    case FileType.Stream:
                        LoadSelectedStream(filePath);
                        lvwFiles.SelectedItems[0].SubItems[3].Text = "Loaded";
                        break;
                    case FileType.Music:
                        LoadSelectedMusic(filePath);
                        lvwFiles.SelectedItems[0].SubItems[3].Text = "Loaded";
                        break;
                    case FileType.SBI:
                        LoadSelectedSbi(filePath);
                        lvwFiles.SelectedItems[0].SubItems[3].Text = "Loaded";
                        break;
                }                
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_Reload_Click(object sender, EventArgs e)
        {
            if (lvwFiles.SelectedItems.Count == 1)
            {
                MenuItem_Load_Click(sender, e);
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
        }

        //-------------------------------------------------------------------------------------------
        //  FUNCTIONS
        //-------------------------------------------------------------------------------------------
        private void LoadSelectedSfx(string filePath)
        {
            ClearLoadedData(FileType.SoundBank);

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
            ClearLoadedData(FileType.Stream);

            //Load data
            streamBankHeaderData = streamReader.ReadSfxHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected.ToString());
            streamReader.ReadStreamBank(filePath, streamBankHeaderData, streamSamples);

            //Display Data
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlStreamData.ShowStreamData();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void LoadSelectedMusic(string filePath)
        {
            ClearLoadedData(FileType.Music);

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
        private void ClearLoadedData(FileType fileType)
        {
            //Clear loaded data
            switch (fileType)
            {
                case FileType.SoundBank:
                    soundBankHeaderData = new SfxHeaderData();
                    sfxSamples.Clear();
                    sfxStoredData.Clear();
                    break;
                case FileType.Stream:
                    streamBankHeaderData = new SfxHeaderData();
                    streamSamples.Clear();
                    break;
                case FileType.Music:
                    musicBankHeaderData = new SfxHeaderData();
                    musicData = new MusicSample();
                    break;
                case FileType.SBI:
                    sbiBankHeaderData = new SfxHeaderData();
                    sbiFileData = new SbiFile();
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
                case FileType.SoundBank:
                    SfxHeaderData sbData = reader.ReadSfxHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected.ToString());
                    total = reader.GetNumberOfSFXs(filePath, sbData);
                    break;
                case FileType.Stream:
                    sbData = streamReader.ReadSfxHeader(filePath, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected.ToString());
                    total = (int)(sbData.FileLength1 / 4);
                    break;
                case FileType.Music:
                    total = 1;
                    break;
            }
            return total;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}