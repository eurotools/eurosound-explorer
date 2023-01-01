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
    public partial class FormSB_SamplePool : DockContent
    {
        private RawSourceWaveStream rawLeftChannel;
        private Sample soundSampleData;
        private readonly AudioFunctions audioFunctions = new AudioFunctions();

        //-------------------------------------------------------------------------------------------------------------------------------
        public FormSB_SamplePool()
        {
            InitializeComponent();
        }
        //-------------------------------------------------------------------------------------------
        //  TOOLBAR BUTTONS
        //-------------------------------------------------------------------------------------------
        private void ButtonSaveRawData_Click(object sender, EventArgs e)
        {
            MenuItem_SaveRaw_Click(sender, e);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonSaveAudio_Click(object sender, EventArgs e)
        {
            MenuItem_SaveSound_Click(sender, e);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonPlayWithoutEffects_Click(object sender, EventArgs e)
        {
            MenuItem_SendToMediaPlayer_Click(sender, e);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonItemUsage_Click(object sender, EventArgs e)
        {
            MenuItem_Usage_Click(sender, e);
        }

        //-------------------------------------------------------------------------------------------
        //  CONTEXT MENU
        //-------------------------------------------------------------------------------------------
        private void MenuItem_SaveRaw_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    foreach (ListViewItem selectedItem in listView1.SelectedItems)
                    {
                        short fileRef = (short)selectedItem.Tag;

                        //Get Sample data 
                        List<SampleData> wavesList = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.sfxStoredData;
                        SampleData selectedSample = wavesList[fileRef];

                        //Write RAW file
                        File.WriteAllBytes(GenericMethods.GetFinalPath(Path.Combine(folderBrowserDialog1.SelectedPath, fileRef + ".raw")), selectedSample.EncodedData);
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_SaveSound_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    foreach (ListViewItem selectedItem in listView1.SelectedItems)
                    {
                        short fileRef = (short)selectedItem.Tag;
                        SoundFile soundToPlay = GetSoundFileFromListViewItem(selectedItem);
                        if (soundToPlay != null)
                        {
                            IWaveProvider wavFile = audioFunctions.CreateMonoWav(ref rawLeftChannel, soundToPlay.PcmData[0], soundToPlay);
                            WaveFileWriter.CreateWaveFile16(GenericMethods.GetFinalPath(Path.Combine(folderBrowserDialog1.SelectedPath, fileRef + ".wav")), wavFile.ToSampleProvider());
                        }
                    }
                }
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------- 
        private void MenuItem_SendToMediaPlayer_Click(object sender, EventArgs e)
        {
            SendToMediaPlayer();
        }

        //------------------------------------------------------------------------------------------------------------------------------- 
        private void MenuItem_Usage_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                SortedDictionary<uint, Sample> samplesDisct = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.sfxSamples;
                using (FrmFileRefUsage formUsage = new FrmFileRefUsage((short)listView1.SelectedItems[0].Tag, soundSampleData, samplesDisct))
                {
                    formUsage.ShowDialog();
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowSampleData(Sample sampleData)
        {
            soundSampleData = sampleData;

            SfxHeaderData MusXheaderData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.soundBankHeaderData;
            List<SampleData> wavHeaderData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.sfxStoredData;
            HashcodeParser Hashcodes = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).hashTable;

            listView1.BeginUpdate();
            listView1.Items.Clear();
            foreach (SampleInfo samplePoolItem in sampleData.samplesList)
            {
                ListViewItem listViewItem = new ListViewItem(new string[] { "", "", "", "", "", "", "" })
                {
                    ImageIndex = 0
                };
                short finalFileRef = samplePoolItem.FileRef;

                //Check for SubSFX
                if (Convert.ToBoolean((sampleData.Flags >> 10) & 1))
                {
                    if (finalFileRef < 0)
                    {
                        //Apply 0x3FFF and turn it into negative to be detected by the tool as a stream file.
                        if (MusXheaderData.FileVersion > 5 && MusXheaderData.FileVersion < 15)
                        {
                            finalFileRef = (short)((samplePoolItem.FileRef & 0x3FFF) * -1);
                        }
                        listViewItem.Text = "Stream: " + finalFileRef;
                    }
                    else if (Hashcodes == null)
                    {
                        listViewItem.Text = finalFileRef.ToString();
                    }
                    else
                    {
                        //Find HashCode Section
                        uint hashcodeToCheck;
                        switch (MusXheaderData.FileVersion)
                        {
                            case 201:
                                hashcodeToCheck = (uint)(0x1A000000 | (ushort)finalFileRef);
                                break;
                            case 6:
                                hashcodeToCheck = (uint)(0x2D700000 | (ushort)finalFileRef);
                                break;
                            default:
                                hashcodeToCheck = (uint)(0x1AF00000 | (ushort)finalFileRef);
                                break;
                        }

                        //Print HashCode
                        if (Hashcodes.HashcodeIsListed(hashcodeToCheck))
                        {
                            listViewItem.Text = Hashcodes.GetHashCodeLabel(hashcodeToCheck);
                            listViewItem.SubItems[0].ForeColor = SystemColors.ControlText;
                        }
                        else
                        {
                            listViewItem.Text = "0x" + hashcodeToCheck.ToString("X8");
                            listViewItem.UseItemStyleForSubItems = false;
                            listViewItem.SubItems[0].ForeColor = Color.Red;
                        }
                    }
                }
                else if (finalFileRef < 0)
                {
                    //Apply 0x3FFF and turn it into negative to be detected by the tool as a stream file.
                    if (MusXheaderData.FileVersion > 5 && MusXheaderData.FileVersion < 15)
                    {
                        finalFileRef = (short)((samplePoolItem.FileRef & 0x3FFF) * -1);
                    }
                    listViewItem.Text = "Stream: " + finalFileRef;
                }
                else
                {
                    if (finalFileRef > wavHeaderData.Count)
                    {
                        listViewItem.UseItemStyleForSubItems = false;
                        listViewItem.SubItems[0].ForeColor = Color.Red;
                    }
                    listViewItem.Text = "Wav: " + finalFileRef;
                }
                listViewItem.SubItems[1].Text = samplePoolItem.Volume.ToString();
                listViewItem.SubItems[2].Text = samplePoolItem.VolumeOffset.ToString();
                listViewItem.SubItems[3].Text = samplePoolItem.Pitch.ToString();
                listViewItem.SubItems[4].Text = samplePoolItem.PitchOffset.ToString();
                listViewItem.SubItems[5].Text = samplePoolItem.Pan.ToString();
                listViewItem.SubItems[6].Text = samplePoolItem.PanOffset.ToString();
                listViewItem.Tag = finalFileRef;

                //Add item to listview
                listView1.Items.Add(listViewItem);
            }
            listView1.EndUpdate();
        }

        //-------------------------------------------------------------------------------------------
        //  LISTVIEW FUNCTIONS
        //-------------------------------------------------------------------------------------------
        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SendToMediaPlayer();
        }

        //-------------------------------------------------------------------------------------------
        //  FUNCTIONS
        //-------------------------------------------------------------------------------------------
        private void SendToMediaPlayer()
        {
            if (listView1.SelectedItems.Count == 1)
            {
                SoundFile soundToPlay = GetSoundFileFromListViewItem(listView1.SelectedItems[0]);
                if (soundToPlay != null)
                {
                    ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlMediaPlayer.LoadSoundData(soundToPlay);
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private SoundFile GetSoundFileFromListViewItem(ListViewItem selectedItem)
        {
            short fileRef = (short)selectedItem.Tag;
            byte[] decodedData = null;
            SoundFile soundToPlay = null;

            //SoundBanks
            if (fileRef >= 0 && ((soundSampleData.Flags >> 10) & 1) == 0)
            {
                List<SampleData> wavesList = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.sfxStoredData;
                SampleData selectedSample = wavesList[fileRef];

                //Decode Data
                decodedData = GenericMethods.DecodeSfxSample(selectedSample, audioFunctions);
                if (decodedData != null)
                {
                    //Set settings
                    soundToPlay = new SoundFile();
                    soundToPlay.PcmData[0] = decodedData;
                    soundToPlay.sampleRate = selectedSample.Frequency;
                    if (ButtonApplyEffects.Checked)
                    {
                        soundToPlay.volume = float.Parse(selectedItem.SubItems[1].Text) / 100;
                        soundToPlay.volumeOffset = float.Parse(selectedItem.SubItems[2].Text) / 100;
                        soundToPlay.pitch = float.Parse(selectedItem.SubItems[3].Text);
                        soundToPlay.pitchOffset = float.Parse(selectedItem.SubItems[4].Text);
                        soundToPlay.panning = float.Parse(selectedItem.SubItems[5].Text) / 100;
                        soundToPlay.panningOffset = float.Parse(selectedItem.SubItems[6].Text) / 100;
                    }
                    soundToPlay.channels = 1;
                    soundToPlay.loopStartPoint = selectedSample.LoopStartOffset;
                    soundToPlay.isLooped = selectedSample.Flags == 1;
                }
            }
            else if (fileRef < 0 && ((soundSampleData.Flags >> 10) & 1) == 0) //Streambanks
            {
                fileRef = (short)(Math.Abs(fileRef) - 1);
                List<StreamSample> streamedSamples = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.streamSamples;

                if ((fileRef >= 0) && (fileRef < streamedSamples.Count))
                {
                    StreamSample selectedSample = streamedSamples[fileRef];

                    //Decode Data
                    decodedData = GenericMethods.DecodeStreamSample(selectedSample, audioFunctions);
                    if (decodedData != null)
                    {
                        soundToPlay = new SoundFile();
                        soundToPlay.PcmData[0] = decodedData;
                        soundToPlay.sampleRate = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.StreamsFrequency;
                        if (ButtonApplyEffects.Checked)
                        {
                            soundToPlay.volume = float.Parse(selectedItem.SubItems[1].Text) / 100;
                            soundToPlay.volumeOffset = float.Parse(selectedItem.SubItems[2].Text) / 100;
                            soundToPlay.pitch = float.Parse(selectedItem.SubItems[3].Text);
                            soundToPlay.pitchOffset = float.Parse(selectedItem.SubItems[4].Text);
                            soundToPlay.panning = float.Parse(selectedItem.SubItems[5].Text) / 100;
                            soundToPlay.panningOffset = float.Parse(selectedItem.SubItems[6].Text) / 100;
                        }
                        soundToPlay.channels = 1;
                    }
                }
                else
                {
                    MessageBox.Show("Could not decode the selected stream sample. Ensure that the stream bank is loaded.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return soundToPlay;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
