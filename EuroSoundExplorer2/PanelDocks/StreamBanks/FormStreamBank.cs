using EuroSoundExplorer2.Classes;
using MusX;
using MusX.Objects;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
            //Output Selected Files
            if (listView1.SelectedItems.Count > 0)
            {
                //Ask user for an output file
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    int sampleRate = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.StreamsFrequency;
                    List<StreamSample> streamedSamples = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.streamSamples;

                    //Output samples
                    foreach (ListViewItem selectedItem in listView1.SelectedItems)
                    {
                        SoundFile soundToPlay = GetSoundFileFromListViewItem(selectedItem, streamedSamples, sampleRate);
                        if (soundToPlay != null)
                        {
                            //Create Wav File
                            IWaveProvider wavFile = audioFunctions.CreateMonoWav(soundToPlay.PcmData[0], soundToPlay.sampleRate, soundToPlay.pitch, soundToPlay.panning, soundToPlay.volume);
                            WaveFileWriter.CreateWaveFile(GenericMethods.GetFinalPath(Path.Combine(folderBrowserDialog1.SelectedPath, (int)selectedItem.Tag + ".wav")), wavFile);
                        }
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_SendToMediaPlayer_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                int sampleRate = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.StreamsFrequency;
                List<StreamSample> streamedSamples = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.streamSamples;

                //Get Sound data and play
                SoundFile soundToPlay = GetSoundFileFromListViewItem(listView1.SelectedItems[0], streamedSamples, sampleRate);
                if (soundToPlay != null)
                {
                    ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlMediaPlayer.LoadSoundData(soundToPlay);
                }
            }
        }

        //-------------------------------------------------------------------------------------------
        //  LISTVIEW
        //-------------------------------------------------------------------------------------------
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

        //-------------------------------------------------------------------------------------------
        //  BUTTONS 
        //-------------------------------------------------------------------------------------------
        private void ButtonValidateAllStreams_Click(object sender, EventArgs e)
        {
            List<StreamSample> streamedSamples = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.streamSamples;
            if (streamedSamples.Count > 0)
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    //Check files
                    ListViewItem currentItem = listView1.Items[i];
                    byte[] ImaData = streamedSamples[(int)currentItem.Tag].EncodedData;
                    if (ImaData[3] == 65)
                    {
                        currentItem.SubItems[1].Text = "OK";
                        currentItem.ForeColor = SystemColors.ControlText;
                    }
                    else
                    {
                        currentItem.SubItems[1].Text = "INVALID";
                        currentItem.ForeColor = Color.Red;
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------
        //  FUNCTIONS
        //------------------------------------------------------------------------------------------
        private SoundFile GetSoundFileFromListViewItem(ListViewItem selectedItem, List<StreamSample> streamedSamples, int sampleRate)
        {
            StreamSample selectedSample = streamedSamples[(int)selectedItem.Tag];
            SoundFile soundToPlay = null;

            //Create object music
            byte[] decodedData = GenericMethods.DecodeStreamSample(selectedSample, audioFunctions);
            if (decodedData != null)
            {
                soundToPlay = new SoundFile();
                soundToPlay.PcmData[0] = decodedData;
                soundToPlay.volume = selectedSample.BaseVolume / 100;
                soundToPlay.sampleRate = sampleRate;
                soundToPlay.channels = 1;
            }

            return soundToPlay;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
