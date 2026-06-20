using MusX;
using MusX.Objects;
using NAudio.Wave;
using sb_explorer.Classes;
using sb_explorer.Services.Audio;
using sb_explorer.CustomControls;
using sb_explorer.UI.Formatting;
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
        private bool suppressDisplayPreferenceEvents;

        //-------------------------------------------------------------------------------------------
        //  MAIN FORM
        //-------------------------------------------------------------------------------------------
        public FormStreamBank()
        {
            InitializeComponent();
            InitializeDisplayMenuState();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowStreamData()
        {
            FrmMain parentForm = (FrmMain)Application.OpenForms[nameof(FrmMain)];
            List<StreamSample> streamedSamples = GetDisplayedStreamSamples(parentForm);
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
                    bool hexOffsets = Properties.Settings.Default.ExplorerOffsetsHex;
                    bool sizesWithUnits = Properties.Settings.Default.ExplorerSizesHumanReadable;
                    string blockPos = DisplayValueFormatter.FormatOffset(currentSample.BlockPosition, hexOffsets);
                    string audioOff = DisplayValueFormatter.FormatOffset(currentSample.AudioOffset, hexOffsets);
                    string markerSize = DisplayValueFormatter.FormatSize(currentSample.MarkerSize, sizesWithUnits);
                    string audioSize = DisplayValueFormatter.FormatSize(currentSample.AudioSize, sizesWithUnits);

                    //Create item
                    ListViewItem listViewItem2 = new ListViewItem(new string[] { (i + 1).ToString(), "??", blockPos, markerSize, audioOff, audioSize, currentSample.BaseVolume.ToString() })
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
                StreambankHeader streamHeader = GetDisplayedStreamHeader(parentForm);
                if (streamHeader.FileVersion == 201 || streamHeader.FileVersion == 1)
                {
                    ButtonValidateAllStreams.Enabled = false;
                }
                else
                {
                    //Enable validation tool - Only For Custom EuroCom ADPCM
                    if (streamHeader.Platform.Equals("PC__") || streamHeader.Platform.Equals("XB__") || EuroSoundCodecMatrix.IsGameCubePlatform(streamHeader.Platform))
                    {
                        ButtonValidateAllStreams.Enabled = true;
                    }
                    else
                    {
                        ButtonValidateAllStreams.Enabled = false;
                    }
                }
            }
            else
            {
                lvwStreamData.Items.Clear();
                ButtonValidateAllStreams.Enabled = false;
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
                        FrmMain parentForm = (FrmMain)Application.OpenForms[nameof(FrmMain)];

                        //Get Sample data 
                        List<StreamSample> streamedSamples = GetDisplayedStreamSamples(parentForm);
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
                    FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);
                    uint sampleRate = parentForm.Configuration.StreamsFrequency;
                    List<StreamSample> streamedSamples = GetDisplayedStreamSamples(parentForm);
                    StreambankHeader headerData = GetDisplayedStreamHeader(parentForm);

                    //Output samples
                    foreach (ListViewItem selectedItem in lvwStreamData.SelectedItems)
                    {
                        SoundFile soundToPlay = GetSoundFileFromListViewItem(selectedItem, streamedSamples, sampleRate, headerData, parentForm.Configuration.PlatformSelected);
                        if (soundToPlay != null)
                        {
                            //Create Wav File
                            IWaveProvider wavFile = audioFunctions.CreateMonoWav(ref rawLeftChannel, soundToPlay.PcmData[0], soundToPlay);
                            EuroSoundWaveWriter.WriteSampleProvider16(
                                GenericMethods.GetFinalPath(Path.Combine(folderBrowserDialog1.SelectedPath, (int)selectedItem.Tag + ".wav")),
                                wavFile.ToSampleProvider(),
                                EuroSoundWaveWriter.CreateLoopInfo(soundToPlay.isLooped, soundToPlay.loopStartPoint, soundToPlay.PcmData[0].Length, 1));
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
                FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);
                uint sampleRate = parentForm.Configuration.StreamsFrequency;
                List<StreamSample> streamedSamples = GetDisplayedStreamSamples(parentForm);
                StreambankHeader headerData = GetDisplayedStreamHeader(parentForm);

                //Get Sound data and play
                SoundFile soundToPlay = GetSoundFileFromListViewItem(lvwStreamData.SelectedItems[0], streamedSamples, sampleRate, headerData, parentForm.Configuration.PlatformSelected);
                if (soundToPlay != null)
                {
                    parentForm.pnlMediaPlayer.LoadSoundData(soundToPlay);
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
                    FrmMain parentForm = (FrmMain)Application.OpenForms[nameof(FrmMain)];

                    //Get sample
                    List<StreamSample> streamedSamples = GetDisplayedStreamSamples(parentForm);
                    int selectedItemIndex = (int)lvwStreamData.SelectedItems[0].Tag;

                    //Display Data
                    if (selectedItemIndex <= streamedSamples.Count)
                    {
                        StreamSample sampleToDisplay = streamedSamples[selectedItemIndex];
                        parentForm.pnlMarkers.ShowMarkers(sampleToDisplay);
                        parentForm.pnlStartMarkers.ShowMarkers(sampleToDisplay);
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------
        //  BUTTONS 
        //-------------------------------------------------------------------------------------------
        private void ButtonValidateAllStreams_Click(object sender, EventArgs e)
        {
            FrmMain parentForm = (FrmMain)Application.OpenForms[nameof(FrmMain)];
            List<StreamSample> streamedSamples = GetDisplayedStreamSamples(parentForm);
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
            SetHexDisplayPreference(ButtonHexView.Checked);
        }

        //-------------------------------------------------------------------------------------------
        //  FUNCTIONS
        //------------------------------------------------------------------------------------------
        private List<StreamSample> GetDisplayedStreamSamples(FrmMain parentForm)
        {
            return parentForm.pnlSoundBankFiles.ActiveStreamBankIsCommon
                ? parentForm.pnlSoundBankFiles.CommonStreamSamples
                : parentForm.pnlSoundBankFiles.StreamSamples;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private StreambankHeader GetDisplayedStreamHeader(FrmMain parentForm)
        {
            return parentForm.pnlSoundBankFiles.ActiveStreamBankIsCommon
                ? parentForm.pnlSoundBankFiles.CommonStreamBankHeaderData
                : parentForm.pnlSoundBankFiles.StreamBankHeaderData;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private SoundFile GetSoundFileFromListViewItem(ListViewItem selectedItem, List<StreamSample> streamedSamples, uint sampleRate, StreambankHeader headerData, Enumerations.Platform selectedPlatform)
        {
            StreamSample selectedSample = streamedSamples[(int)selectedItem.Tag];
            SoundFile soundToPlay = null;

            //Create object music
            byte[] decodedData = GenericMethods.DecodeStreamSample(selectedSample, audioFunctions, headerData, selectedPlatform);
            if (decodedData != null)
            {
                soundToPlay = new SoundFile();
                soundToPlay.PcmData[0] = decodedData;
                soundToPlay.volume = selectedSample.BaseVolume / 100;
                soundToPlay.sampleRate = sampleRate;
                soundToPlay.channels = 1;
                soundToPlay.isLooped = EuroSoundMarkerLoopResolver.IsLooped(selectedSample.Markers, MarkerLoopMode.LoopUnlessEndMarker);
                soundToPlay.startPos = (int)EuroSoundMarkerLoopResolver.GetStartPosition(selectedSample.Markers);
                soundToPlay.loopStartPoint = EuroSoundMarkerLoopResolver.GetLoopStart(selectedSample.Markers);
                soundToPlay.loopEndPoint = (int)EuroSoundMarkerLoopResolver.GetLoopEnd(selectedSample.Markers);
            }

            return soundToPlay;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void InitializeDisplayMenuState()
        {
            suppressDisplayPreferenceEvents = true;
            ButtonHexView.Checked = Properties.Settings.Default.ExplorerOffsetsHex;
            MenuItem_OffsetsHex.Checked = Properties.Settings.Default.ExplorerOffsetsHex;
            MenuItem_SizesWithUnits.Checked = Properties.Settings.Default.ExplorerSizesHumanReadable;
            suppressDisplayPreferenceEvents = false;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_OffsetsHex_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressDisplayPreferenceEvents)
            {
                return;
            }

            SetHexDisplayPreference(MenuItem_OffsetsHex.Checked);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_SizesWithUnits_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressDisplayPreferenceEvents)
            {
                return;
            }

            Properties.Settings.Default.ExplorerSizesHumanReadable = MenuItem_SizesWithUnits.Checked;
            Properties.Settings.Default.Save();
            ShowStreamData();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void SetHexDisplayPreference(bool checkedState)
        {
            ButtonHexView.Checked = checkedState;
            MenuItem_OffsetsHex.Checked = checkedState;
            Properties.Settings.Default.ExplorerOffsetsHex = checkedState;
            Properties.Settings.Default.Save();
            ShowStreamData();
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
