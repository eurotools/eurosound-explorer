﻿using EuroSoundExplorer2.Classes;
using EuroSoundExplorer2.CustomControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using WeifenLuo.WinFormsUI.Docking;

namespace EuroSoundExplorer2
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FrmMain : Form
    {
        public AppConfig configuration = new AppConfig();
        public HashcodeParser hashTable = new HashcodeParser();

        //Collections
        private readonly List<DockContent> m_DockForms = new List<DockContent>();

        //Forms
        internal FormSB_HashCodes pnlSbHashCodes = new FormSB_HashCodes();
        internal FormSB_SamplePool pnlSbSamplePool = new FormSB_SamplePool();
        internal FormSB_SampleProps pnlSbSampleProps = new FormSB_SampleProps();
        internal FormSettings pnlSettings = new FormSettings();
        internal FormSoundBankFiles pnlSoundBankFiles = new FormSoundBankFiles();
        internal FormSB_WavHeaderData pnlWavHeaderData = new FormSB_WavHeaderData();
        internal FormMediaPlayer pnlMediaPlayer = new FormMediaPlayer();
        internal FormStreamBank pnlStreamData = new FormStreamBank();
        internal FormMarkers pnlMarkers = new FormMarkers();
        internal FormStartMarkers pnlStartMarkers = new FormStartMarkers();
        internal FormMusicData pnlMusicData = new FormMusicData();
        internal FormSBiSoundBanks pnlSbiSoundbanks = new FormSBiSoundBanks();
        internal FormSBiMusicBanks pnlSbiMusicbanks = new FormSBiMusicBanks();

        //-------------------------------------------------------------------------------------------
        //  MAIN FORM
        //-------------------------------------------------------------------------------------------
        public FrmMain()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void FrmMain_Load(object sender, EventArgs e)
        {
            m_DockForms.Add(pnlSbHashCodes);
            m_DockForms.Add(pnlSbSamplePool);
            m_DockForms.Add(pnlSbSampleProps);
            m_DockForms.Add(pnlSettings);
            m_DockForms.Add(pnlSoundBankFiles);
            m_DockForms.Add(pnlWavHeaderData);
            m_DockForms.Add(pnlMediaPlayer);
            m_DockForms.Add(pnlStreamData);
            m_DockForms.Add(pnlMarkers);
            m_DockForms.Add(pnlStartMarkers);
            m_DockForms.Add(pnlMusicData);
            m_DockForms.Add(pnlSbiSoundbanks);
            m_DockForms.Add(pnlSbiMusicbanks);

            //Load Panels State
            if (!File.Exists("ESEx\\Dock Settings.xml"))
            {
                File.Copy("ESEx\\Default Dock Settings.xml", "ESEx\\Dock Settings.xml", true);
                File.SetAttributes("ESEx\\Dock Settings.xml", FileAttributes.Normal);
            }
            mainDockPanel.LoadFromXml("ESEx\\Dock Settings.xml", new DeserializeDockContent(GetContentFromPersistString));

            //Load last state listview
            foreach (Form dockForm in m_DockForms)
            {
                LoadListViewConfig(dockForm);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private DockContent GetContentFromPersistString(string persistString)
        {
            foreach (DockContent dockForm in m_DockForms)
            {
                if (persistString == dockForm.GetType().ToString())
                {
                    return dockForm;
                }
            }
            return null;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save all listviews state
            foreach (Form dockForm in m_DockForms)
            {
                SaveListViewConfig(dockForm);
            }
            pnlSettings.SaveSettings();

            //Save previous config
            mainDockPanel.SaveAsXml("ESEx\\Dock Settings.xml");

            //Close all forms
            foreach (Form dockForm in m_DockForms)
            {
                dockForm.Close();
            }
        }

        //-------------------------------------------------------------------------------------------
        //  File Menu
        //-------------------------------------------------------------------------------------------
        private void MenuItem_File_DropDownOpening(object sender, EventArgs e)
        {
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_File_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_File_Settings_Click(object sender, EventArgs e)
        {
            pnlSettings.Show(mainDockPanel, DockState.Float);
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_File_DataViewer_Click(object sender, EventArgs e)
        {
            using (FrmDataViewer dataViewer = new FrmDataViewer())
            {
                dataViewer.ShowDialog();
            }
        }

        //-------------------------------------------------------------------------------------------
        //  MENU 1
        //-------------------------------------------------------------------------------------------
        private void MenuItem_Menu1_DropDownOpening(object sender, EventArgs e)
        {
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_Files_Click(object sender, EventArgs e)
        {
            pnlSoundBankFiles.Show(mainDockPanel, DockState.Float);
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_MediaPlayer_Click(object sender, EventArgs e)
        {
            pnlMediaPlayer.Show(mainDockPanel, DockState.Float);
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------
        //  Sound Banks Menu
        //-------------------------------------------------------------------------------------------
        private void MenuItem_SB_DropDownOpening(object sender, EventArgs e)
        {
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_SB_HashCodes_Click(object sender, EventArgs e)
        {
            pnlSbHashCodes.Show(mainDockPanel, DockState.Float);
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_SB_SamplePool_Click(object sender, EventArgs e)
        {
            pnlSbSamplePool.Show(mainDockPanel, DockState.Float);
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_SB_SampleProps_Click(object sender, EventArgs e)
        {
            pnlSbSampleProps.Show(mainDockPanel, DockState.Float);
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_SB_WavHeaderData_Click(object sender, EventArgs e)
        {
            pnlWavHeaderData.Show(mainDockPanel, DockState.Float);
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------
        //  Stream Banks Menu
        //-------------------------------------------------------------------------------------------
        private void MenuItem_Stream_DropDownOpening(object sender, EventArgs e)
        {
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_Streams_StreamData_Click(object sender, EventArgs e)
        {
            pnlStreamData.Show(mainDockPanel, DockState.Float);
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_Streams_StartMarkers_Click(object sender, EventArgs e)
        {
            pnlStartMarkers.Show(mainDockPanel, DockState.Float);
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_Streams_Markers_Click(object sender, EventArgs e)
        {
            pnlMarkers.Show(mainDockPanel, DockState.Float);
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------
        //  Music Banks Menu
        //-------------------------------------------------------------------------------------------
        private void MenuItem_Music_DropDownOpening(object sender, EventArgs e)
        {
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_Music_MusicData_Click(object sender, EventArgs e)
        {
            pnlMusicData.Show(mainDockPanel, DockState.Float);
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_Music_StartMarkers_Click(object sender, EventArgs e)
        {
            pnlStartMarkers.Show(mainDockPanel, DockState.Float);
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_Music_Markers_Click(object sender, EventArgs e)
        {
            pnlMarkers.Show(mainDockPanel, DockState.Float);
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------
        //  SBI Banks Menu
        //-------------------------------------------------------------------------------------------
        private void MenuItem_SBI_DropDownOpening(object sender, EventArgs e)
        {
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_SBI_StoredSoundBanks_Click(object sender, EventArgs e)
        {
            pnlSbiSoundbanks.Show(mainDockPanel, DockState.Float);
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_SBI_StoredMusicBanks_Click(object sender, EventArgs e)
        {
            pnlSbiMusicbanks.Show(mainDockPanel, DockState.Float);
            UpdateWindowMenuChecks();
        }

        //-------------------------------------------------------------------------------------------
        //  Functions
        //-------------------------------------------------------------------------------------------
        private void UpdateWindowMenuChecks()
        {
            //File
            MenuItem_File_Settings.Checked = pnlSettings.DockState != DockState.Hidden;

            //SoundBanks
            MenuItem_SB_HashCodes.Checked = pnlSbHashCodes.DockState != DockState.Hidden;
            MenuItem_SB_SamplePool.Checked = pnlSbSamplePool.DockState != DockState.Hidden;
            MenuItem_SB_SampleProps.Checked = pnlSbSampleProps.DockState != DockState.Hidden;
            MenuItem_SB_WavHeaderData.Checked = pnlWavHeaderData.DockState != DockState.Hidden;

            //Streams
            MenuItem_Streams_StreamData.Checked = pnlStreamData.DockState != DockState.Hidden;
            MenuItem_Streams_Markers.Checked = pnlMarkers.DockState != DockState.Hidden;
            MenuItem_Streams_StartMarkers.Checked = pnlStartMarkers.DockState != DockState.Hidden;

            //Music
            MenuItem_Music_MusicData.Checked = pnlMusicData.DockState != DockState.Hidden;
            MenuItem_Music_Markers.Checked = pnlMarkers.DockState != DockState.Hidden;
            MenuItem_Music_StartMarkers.Checked = pnlStartMarkers.DockState != DockState.Hidden;

            //SBI
            MenuItem_SBI_StoredSoundBanks.Checked = pnlSbiSoundbanks.DockState != DockState.Hidden;
            MenuItem_SBI_StoredMusicBanks.Checked = pnlSbiMusicbanks.DockState != DockState.Hidden;

            //Menu 1
            MenuItem_SfxFiles.Checked = pnlSoundBankFiles.DockState != DockState.Hidden;
            MenuItem_MediaPlayer.Checked = pnlMediaPlayer.DockState != DockState.Hidden;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private string GetConfigFile(Form f) => "ESEx\\" + f.Name + ".ini";

        //-------------------------------------------------------------------------------------------------------------------------------
        private void SaveListViewConfig(Form f)
        {
            string contents = "";
            foreach (Control control in (ArrangedElementCollection)f.Controls)
            {
                if (control is ListView_ColumnSortingClick listView1)
                {
                    contents += listView1.Name;
                    for (int i = 0; i < listView1.Columns.Count; ++i)
                    {
                        contents = contents + " " + listView1.Columns[i].Width;
                    }
                    contents += "\n";
                }
            }
            File.WriteAllText(GetConfigFile(f), contents);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void LoadListViewConfig(Form frm)
        {
            string filePath = GetConfigFile(frm);
            if (File.Exists(filePath))
            {
                string[] fileContent = File.ReadAllLines(filePath);
                foreach (string line in fileContent)
                {
                    string[] lineData = line.Split(' ');
                    Control[] formControls = frm.Controls.Find(lineData[0], false);
                    if (formControls.Length == 1)
                    {
                        ListView_ColumnSortingClick listView = (ListView_ColumnSortingClick)formControls[0];
                        if (listView.Columns.Count == lineData.Length - 1)
                        {
                            for (int i = 0; i < listView.Columns.Count; ++i)
                            {
                                listView.Columns[i].Width = Convert.ToInt32(lineData[1 + i]);
                            }
                        }
                    }
                }
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}