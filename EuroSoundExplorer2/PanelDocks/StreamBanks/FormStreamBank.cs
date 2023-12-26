using MusX;
using MusX.Objects;
using NAudio.Wave;
using sb_explorer.Classes;
using sb_explorer.CustomControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace sb_explorer
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FormStreamBank : DockContent
    {
        private RawSourceWaveStream rawLeftChannel;
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
            if (streamedSamples.Count > 0)
            {
                //Print Samples Info
                lvwStreamData.BeginUpdate();
                lvwStreamData.Items.Clear();
                for (int i = 0; i < streamedSamples.Count; i++)
                {
                    StreamSample currentSample = streamedSamples[i];
                    ushort errors = 0;

                    //Create item and add it to list
                    string blockPos = currentSample.BlockPosition.ToString();
                    string audioOff = currentSample.AudioOffset.ToString();

                    //Enable hex
                    if (ButtonHexView.Checked)
                    {
                        blockPos = "0x" + currentSample.BlockPosition.ToString("X8");
                        audioOff = "0x" + currentSample.AudioOffset.ToString("X8");
                    }

                    //Create item
                    ListViewItem listViewItem2 = new ListViewItem(new string[] { (i + 1).ToString(), "??", blockPos, currentSample.MarkerSize.ToString(), audioOff, currentSample.AudioSize.ToString(), currentSample.BaseVolume.ToString() })
                    {
                        ImageIndex = 0,
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
                    lvwStreamData.Items.Add(listViewItem2);
                }
                lvwStreamData.EndUpdate();

                //Enable or disable button
                StreambankHeader streamHeader = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.streamBankHeaderData;
                if (streamHeader.FileVersion == 201 || streamHeader.FileVersion == 1)
                {
                    ButtonValidateAllStreams.Enabled = false;
                }
                else
                {
                    //Enable validation tool - Only For Custom EuroCom ADPCM
                    if (streamHeader.Platform.Equals("PC__") || streamHeader.Platform.Equals("XB__") || streamHeader.Platform.Equals("GC__"))
                    {
                        ButtonValidateAllStreams.Enabled = true;
                    }
                    else
                    {
                        ButtonValidateAllStreams.Enabled = false;
                    }
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
        private void MenuItem_SaveRaw_Click(object sender, EventArgs e)
        {
            if (lvwStreamData.SelectedItems.Count > 0)
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    foreach (ListViewItem selectedItem in lvwStreamData.SelectedItems)
                    {
                        //Get Sample data 
                        List<StreamSample> streamedSamples = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.streamSamples;
                        StreamSample selectedSample = streamedSamples[(int)selectedItem.Tag];

                        //Write RAW file
                        File.WriteAllBytes(GenericMethods.GetFinalPath(Path.Combine(folderBrowserDialog1.SelectedPath, (int)selectedItem.Tag + ".raw")), selectedSample.EncodedData);
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_Save_Click(object sender, EventArgs e)
        {
            //Output Selected Files
            if (lvwStreamData.SelectedItems.Count > 0)
            {
                //Ask user for an output file
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    uint sampleRate = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.StreamsFrequency;
                    List<StreamSample> streamedSamples = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.streamSamples;

                    //Output samples
                    foreach (ListViewItem selectedItem in lvwStreamData.SelectedItems)
                    {
                        SoundFile soundToPlay = GetSoundFileFromListViewItem(selectedItem, streamedSamples, sampleRate);
                        if (soundToPlay != null)
                        {
                            //Create Wav File
                            IWaveProvider wavFile = audioFunctions.CreateMonoWav(ref rawLeftChannel, soundToPlay.PcmData[0], soundToPlay);
                            WaveFileWriter.CreateWaveFile16(GenericMethods.GetFinalPath(Path.Combine(folderBrowserDialog1.SelectedPath, (int)selectedItem.Tag + ".wav")), wavFile.ToSampleProvider());
                        }
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_SendToMediaPlayer_Click(object sender, EventArgs e)
        {
            if (lvwStreamData.SelectedItems.Count == 1)
            {
                uint sampleRate = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.StreamsFrequency;
                List<StreamSample> streamedSamples = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.streamSamples;

                //Get Sound data and play
                SoundFile soundToPlay = GetSoundFileFromListViewItem(lvwStreamData.SelectedItems[0], streamedSamples, sampleRate);
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
            //If the flag is on execute this method
            if (ButtonDisplayMarkers.Checked)
            {
                if (lvwStreamData.SelectedItems.Count == 1)
                {
                    //Get sample
                    List<StreamSample> streamedSamples = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.streamSamples;
                    int selectedItemIndex = (int)lvwStreamData.SelectedItems[0].Tag;

                    //Display Data
                    if (selectedItemIndex <= streamedSamples.Count)
                    {
                        StreamSample sampleToDisplay = streamedSamples[selectedItemIndex];
                        uint sampleRate = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.StreamsFrequency;
                        ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlMarkers.ShowMarkers(sampleToDisplay);
                        ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlStartMarkers.ShowMarkers(sampleToDisplay);
                    }
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
                for (int i = 0; i < lvwStreamData.Items.Count; i++)
                {
                    //Get item and IMA Data
                    ListViewItem currentItem = lvwStreamData.Items[i];
                    byte[] ImaData = streamedSamples[(int)currentItem.Tag].EncodedData;

                    //Show Results
                    if (audioFunctions.CheckIfEurocomImaIsInvalid(ImaData))
                    {
                        currentItem.SubItems[1].Text = "INVALID";
                        currentItem.ForeColor = Color.Red;
                        ListView_ColumnSortingClick.AddImageToSubItem(currentItem, 1, 2, lvwStreamData.Handle);
                    }
                    else
                    {
                        currentItem.SubItems[1].Text = "OK";
                        currentItem.ForeColor = SystemColors.ControlText;
                        ListView_ColumnSortingClick.AddImageToSubItem(currentItem, 1, 1, lvwStreamData.Handle);

                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonSaveRawData_Click(object sender, EventArgs e)
        {
            MenuItem_SaveRaw_Click(sender, e);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonSaveFile_Click(object sender, EventArgs e)
        {
            MenuItem_Save_Click(sender, e);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonSendToMediaPlayer_Click(object sender, EventArgs e)
        {
            MenuItem_SendToMediaPlayer_Click(sender, e);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonHexView_Click(object sender, EventArgs e)
        {
            ShowStreamData();
        }

        //-------------------------------------------------------------------------------------------
        //  FUNCTIONS
        //------------------------------------------------------------------------------------------
        private SoundFile GetSoundFileFromListViewItem(ListViewItem selectedItem, List<StreamSample> streamedSamples, uint sampleRate)
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
                soundToPlay.isLooped = SoundIsLooped(selectedSample.Markers);
                soundToPlay.startPos = (int)GetStartPosition(selectedSample.Markers);
                soundToPlay.loopStartPoint = GetStartLoopPos(selectedSample.Markers);
                soundToPlay.loopEndPoint = (int)GetEndLoopPos(selectedSample.Markers);
            }

            return soundToPlay;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private uint GetStartPosition(Marker[] startMarkers)
        {
            uint startPosition = 0;
            for (int i = 0; i < startMarkers.Length; i++)
            {
                if (startMarkers[i].Type == 10)
                {
                    startPosition = startMarkers[i].Position;
                    break;
                }
            }

            return startPosition;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private uint GetStartLoopPos(Marker[] startMarkers)
        {
            uint startPosition = 0;
            for (int i = 0; i < startMarkers.Length; i++)
            {
                if (startMarkers[i].Type == 7 || startMarkers[i].Type == 6)
                {
                    startPosition = startMarkers[i].LoopStart;
                    break;
                }
            }

            return startPosition;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private uint GetEndLoopPos(Marker[] startMarkers)
        {
            uint startPosition = 0;
            for (int i = 0; i < startMarkers.Length; i++)
            {
                if (startMarkers[i].Type == 7 || startMarkers[i].Type == 6)
                {
                    startPosition = startMarkers[i].Position;
                    break;
                }
            }

            return startPosition;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private bool SoundIsLooped(Marker[] startMarkers)
        {
            bool isLooped = true;
            for (int i = 0; i < startMarkers.Length; i++)
            {
                if (startMarkers[i].Type == 9)
                {
                    isLooped = false;
                    break;
                }
            }

            return isLooped;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
