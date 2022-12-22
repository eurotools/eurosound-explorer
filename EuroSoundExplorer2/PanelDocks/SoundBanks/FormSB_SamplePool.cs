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
    public partial class FormSB_SamplePool : DockContent
    {
        private Sample soundSampleData;
        private readonly AudioFunctions audioFunctions = new AudioFunctions();

        //-------------------------------------------------------------------------------------------------------------------------------
        public FormSB_SamplePool()
        {
            InitializeComponent();
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
                ListViewItem listViewItem = new ListViewItem(new string[] { "", "", "", "", "", "", "" });
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
                if (MusXheaderData.FileVersion == 201 || MusXheaderData.FileVersion == 1)
                {
                    listViewItem.SubItems[3].Text = decimal.Divide(samplePoolItem.Pitch, 1024).ToString();
                    listViewItem.SubItems[4].Text = decimal.Divide(samplePoolItem.PitchOffset, 1024).ToString();
                }
                else
                {
                    float finalPitch = samplePoolItem.Pitch * 0.2f;
                    listViewItem.SubItems[3].Text = finalPitch.ToString();

                    float finalPitchOffset = samplePoolItem.PitchOffset * 0.1f;
                    listViewItem.SubItems[4].Text = finalPitchOffset.ToString();
                }
                listViewItem.SubItems[5].Text = samplePoolItem.Pan.ToString();
                listViewItem.SubItems[6].Text = samplePoolItem.PanOffset.ToString();
                listViewItem.Tag = finalFileRef;

                //Add item to listview
                listView1.Items.Add(listViewItem);
            }
            listView1.EndUpdate();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
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
        private void MenuItem_SendToMediaPlayer_Click(object sender, EventArgs e)
        {
            SendToMediaPlayer();
        }

        //------------------------------------------------------------------------------------------------------------------------------- 
        private void SendToMediaPlayer()
        {
            if (listView1.SelectedItems.Count == 1)
            {
                short fileRef = (short)listView1.SelectedItems[0].Tag;
                byte[] decodedData = null;
                SoundFile soundToPlay = new SoundFile();

                //SoundBanks
                if (fileRef >= 0 && ((soundSampleData.Flags >> 10) & 1) == 0)
                {
                    List<SampleData> wavesList = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.sfxStoredData;
                    SampleData selectedSample = wavesList[fileRef];

                    //Decode Data
                    decodedData = GenericMethods.DecodeSfxSample(selectedSample, audioFunctions);

                    //Set settings
                    soundToPlay.loopOffset = selectedSample.LoopStartOffset;
                    soundToPlay.isLooped = selectedSample.Flags == 1;
                    soundToPlay.sampleRate = selectedSample.Frequency;
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

                        //Set settings
                        soundToPlay.sampleRate = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.StreamsFrequency;
                    }
                    else
                    {
                        MessageBox.Show("Could not decode the selected stream sample. Ensure that the stream bank is loaded.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                //Create music object and play
                if (decodedData != null)
                {
                    soundToPlay.PcmData[0] = decodedData;
                    soundToPlay.volume = float.Parse(listView1.SelectedItems[0].SubItems[1].Text) / 100;
                    soundToPlay.pan = float.Parse(listView1.SelectedItems[0].SubItems[5].Text);
                    soundToPlay.pitch = float.Parse(listView1.SelectedItems[0].SubItems[3].Text);
                    soundToPlay.channels = 1;

                    ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlMediaPlayer.LoadSoundData(soundToPlay);
                }
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
