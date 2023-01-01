using EuroSoundExplorer2.Classes;
using MusX.Objects;
using NAudio.Wave;
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
    public partial class FormSB_WavHeaderData : DockContent
    {
        private RawSourceWaveStream rawLeftChannel;
        private readonly AudioFunctions audioFunctions = new AudioFunctions();

        //-------------------------------------------------------------------------------------------------------------------------------
        public FormSB_WavHeaderData()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowWavesList()
        {
            List<SampleData> wavesList = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.sfxStoredData;

            listView1.BeginUpdate();
            listView1.Items.Clear();
            if (wavesList.Count > 0)
            {
                short index = 0;

                foreach (SampleData waveData in wavesList)
                {
                    //Create item and add it to list
                    ListViewItem listViewItem2 = new ListViewItem(new string[] { (index).ToString(), waveData.Flags.ToString(), waveData.Address.ToString(), waveData.MemorySize.ToString(), waveData.SampleSize.ToString(), waveData.Frequency.ToString(), waveData.LoopStartOffset.ToString(), waveData.Duration.ToString() })
                    {
                        ImageIndex = 0,
                        Tag = index
                    };
                    if (waveData.LoopStartOffset > waveData.MemorySize)
                    {
                        listViewItem2.UseItemStyleForSubItems = false;
                        listViewItem2.SubItems[6].ForeColor = Color.Red;
                    }
                    listView1.Items.Add(listViewItem2);

                    //Increase index
                    index++;
                }
            }
            listView1.EndUpdate();
        }

        //-------------------------------------------------------------------------------------------
        //  LISTVIEW
        //-------------------------------------------------------------------------------------------
        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SendToMediaPlayer();
        }

        //-------------------------------------------------------------------------------------------
        //  TOOLBAR BUTTONS
        //-------------------------------------------------------------------------------------------
        private void ButtonSaveRawData_Click(object sender, System.EventArgs e)
        {
            MenuItem_SaveRaw_Click(sender, e);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonSaveAudio_Click(object sender, System.EventArgs e)
        {
            MenuItem_Save_Click(sender, e);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonSaveToMediaPlayer_Click(object sender, System.EventArgs e)
        {
            MenuItem_Play_Click(sender, e);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonItemUsage_Click(object sender, System.EventArgs e)
        {
            MenuItem_ItemUsage_Click(sender, e);
        }

        //-------------------------------------------------------------------------------------------
        //  CONTEXT MENU
        //-------------------------------------------------------------------------------------------
        private void MenuItem_SaveRaw_Click(object sender, System.EventArgs e)
        {
            //Output selected samples
            if (listView1.SelectedItems.Count > 0)
            {
                //Ask user for an output path
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    List<SampleData> wavesList = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.sfxStoredData;

                    //Start output
                    foreach (ListViewItem selectedItem in listView1.SelectedItems)
                    {
                        SampleData selectedSample = wavesList[(short)selectedItem.Tag];

                        //Save raw data
                        File.WriteAllBytes(GenericMethods.GetFinalPath(Path.Combine(folderBrowserDialog1.SelectedPath, (short)selectedItem.Tag + ".raw")), selectedSample.EncodedData);
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_Save_Click(object sender, System.EventArgs e)
        {
            //Output selected samples
            if (listView1.SelectedItems.Count > 0)
            {
                //Ask user for an output path
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    List<SampleData> wavesList = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.sfxStoredData;

                    //Start output
                    foreach (ListViewItem selectedItem in listView1.SelectedItems)
                    {
                        SampleData selectedSample = wavesList[(short)selectedItem.Tag];

                        //Create object music
                        byte[] decodedData = GenericMethods.DecodeSfxSample(selectedSample, audioFunctions);
                        if (decodedData != null)
                        {
                            SoundFile soundToPlay = new SoundFile();
                            soundToPlay.PcmData[0] = decodedData;
                            soundToPlay.loopOffset = selectedSample.LoopStartOffset;
                            soundToPlay.isLooped = selectedSample.Flags == 1;
                            soundToPlay.sampleRate = selectedSample.Frequency;
                            soundToPlay.channels = selectedSample.Channels;

                            //Create Wav File
                            IWaveProvider wavFile = audioFunctions.CreateMonoWav(ref rawLeftChannel, soundToPlay.PcmData[0], soundToPlay);
                            WaveFileWriter.CreateWaveFile16(GenericMethods.GetFinalPath(Path.Combine(folderBrowserDialog1.SelectedPath, (short)selectedItem.Tag + ".wav")), wavFile.ToSampleProvider());
                        }
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_Play_Click(object sender, System.EventArgs e)
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
                List<SampleData> wavesList = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.sfxStoredData;
                SampleData selectedSample = wavesList[(short)listView1.SelectedItems[0].Tag];

                //Create object music
                byte[] decodedData = GenericMethods.DecodeSfxSample(selectedSample, audioFunctions);
                if (decodedData != null)
                {
                    SoundFile soundToPlay = new SoundFile();
                    soundToPlay.PcmData[0] = decodedData;
                    soundToPlay.loopOffset = selectedSample.LoopStartOffset;
                    soundToPlay.isLooped = selectedSample.Flags == 1;
                    soundToPlay.sampleRate = selectedSample.Frequency;
                    soundToPlay.channels = selectedSample.Channels;

                    //Send to Media Player
                    ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlMediaPlayer.LoadSoundData(soundToPlay);
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_ItemUsage_Click(object sender, System.EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                SortedDictionary<uint, Sample> samplesDisct = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.sfxSamples;
                using (FrmFileRefUsage itemUsage = new FrmFileRefUsage((short)listView1.SelectedItems[0].Tag, null, samplesDisct))
                {
                    itemUsage.ShowDialog();
                }
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
