using EuroSoundExplorer2.Classes;
using MusX;
using MusX.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace EuroSoundExplorer2
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FormStreamBank : DockContent
    {
        private readonly AudioFunctions audioFunctions = new AudioFunctions();

        //-------------------------------------------------------------------------------------------
        //  MAIN FORM
        //-------------------------------------------------------------------------------------------
        public FormStreamBank()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowStreamData()
        {
            List<StreamSample> streamedSamples = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.streamSamples;

            //Print Samples Info
            listView1.BeginUpdate();
            listView1.Items.Clear();
            for (int i = 0; i < streamedSamples.Count; i++)
            {
                StreamSample currentSample = streamedSamples[i];
                ushort errors = 0;

                //Create item and add it to list
                ListViewItem listViewItem2 = new ListViewItem(new string[] { i.ToString(), "??", currentSample.BlockPosition.ToString(), currentSample.MarkerSize.ToString(), currentSample.AudioOffset.ToString(), currentSample.AudioSize.ToString(), currentSample.BaseVolume.ToString() })
                {
                    Tag = i
                };

                //Check for errors
                if (currentSample.MarkerSize < 0)
                {
                    errors |= (1 << 3);
                }
                if (currentSample.AudioOffset < 0)
                {
                    errors |= (1 << 4);
                }
                if (currentSample.AudioSize < 0)
                {
                    errors |= (1 << 5);
                }
                if (currentSample.BaseVolume < 0 || currentSample.BaseVolume > 100)
                {
                    errors |= (1 << 6);
                }

                //Red fields
                for (int j = 0; j < 7; j++)
                {
                    if (Convert.ToBoolean((errors >> j) & 1))
                    {
                        listViewItem2.SubItems[j].ForeColor = Color.Red;
                    }
                }
                listView1.Items.Add(listViewItem2);
            }
            listView1.EndUpdate();

            //Enable or disable button
            SfxHeaderData streamHeader = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.streamBankHeaderData;
            if (streamHeader.FileVersion == 201 || streamHeader.FileVersion == 1)
            {
                ButtonValidateAllStreams.Enabled = false;
            }
            else
            {
                //Enable validation tool - Only For Custom EuroCom ADPCM
                if (streamHeader.Platform.Contains("PC"))
                {
                    ButtonValidateAllStreams.Enabled = true;
                }
                else
                {
                    ButtonValidateAllStreams.Enabled = false;
                }
            }
        }

        //-------------------------------------------------------------------------------------------
        //  LISTVIEW
        //-------------------------------------------------------------------------------------------
        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MenuItem_SendToMediaPlayer_Click(sender, e);
        }

        //-------------------------------------------------------------------------------------------
        //  CONTEXT MENU
        //-------------------------------------------------------------------------------------------
        private void MenuItem_Save_Click(object sender, EventArgs e)
        {

        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_SendToMediaPlayer_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                int sampleRate = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.StreamsFrequency;
                List<StreamSample> streamedSamples = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.streamSamples;
                StreamSample selectedSample = streamedSamples[listView1.SelectedIndices[0]];

                //Create object music
                byte[] decodedData = GenericMethods.DecodeStreamSample(selectedSample, audioFunctions);
                if (decodedData != null)
                {
                    SoundFile soundToPlay = new SoundFile();
                    soundToPlay.PcmData[0] = decodedData;
                    soundToPlay.volume = selectedSample.BaseVolume / 100;
                    soundToPlay.sampleRate = sampleRate;
                    soundToPlay.channels = 1;

                    ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlMediaPlayer.LoadSoundData(soundToPlay);
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                //Get sample
                List<StreamSample> streamedSamples = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.streamSamples;
                int selectedItemIndex = (int)listView1.SelectedItems[0].Tag;

                //Display Data
                if (selectedItemIndex <= streamedSamples.Count)
                {
                    StreamSample sampleToDisplay = streamedSamples[selectedItemIndex];
                    ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlMarkers.ShowMarkers(sampleToDisplay);
                    ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlStartMarkers.ShowMarkers(sampleToDisplay);
                }
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
