using MusX.Objects;
using NAudio.Wave;
using MusX;
using sb_explorer.Classes;
using sb_explorer.Services.Audio;
using sb_explorer.UI.Formatting;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace sb_explorer
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FormSB_WavHeaderData : DockContent
    {
        private readonly AudioFunctions audioFunctions = new AudioFunctions();
        private bool suppressDisplayPreferenceEvents;

        //-------------------------------------------------------------------------------------------------------------------------------
        public FormSB_WavHeaderData()
        {
            InitializeComponent();
            InitializeDisplayMenuState();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowWavesList()
        {
            List<SampleData> wavesList = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.SfxStoredData;

            listView1.BeginUpdate();
            listView1.Items.Clear();
            if (wavesList.Count > 0)
            {
                short index = 0;

                foreach (SampleData waveData in wavesList)
                {
                    bool hexOffsets = Properties.Settings.Default.ExplorerOffsetsHex;
                    bool sizesWithUnits = Properties.Settings.Default.ExplorerSizesHumanReadable;
                    bool loopsAsSamples = Properties.Settings.Default.ExplorerSoundBankLoopsAsSamples;
                    string addressOffset = DisplayValueFormatter.FormatOffset(waveData.Address, hexOffsets);
                    string memorySize = DisplayValueFormatter.FormatSize(waveData.MemorySize, sizesWithUnits);
                    string sampleSize = DisplayValueFormatter.FormatSize(waveData.SampleSize, sizesWithUnits);
                    string binaryLoopOffset = waveData.IsLooped
                        ? DisplayValueFormatter.FormatOffset(waveData.OriginalLoopOffset, hexOffsets)
                        : "None";
                    string loopSample = waveData.IsLooped
                        ? loopsAsSamples
                            ? waveData.LoopStartSample.ToString()
                            : DisplayValueFormatter.FormatOffset(waveData.OriginalLoopOffset, hexOffsets)
                        : "None";

                    //Create item and add it to list
                    string codec = waveData.AudioReference == null ? "Unknown" : waveData.AudioReference.Codec.ToString();
                    ListViewItem listViewItem2 = new ListViewItem(new string[] { (index).ToString(), waveData.Flags.ToString(), addressOffset, memorySize, sampleSize, waveData.Frequency.ToString(), binaryLoopOffset, loopSample, waveData.Duration.ToString(), codec, waveData.Channels.ToString(), waveData.TotalSamples.ToString(), string.Format("0x{0:X8}", waveData.WavHashCode), waveData.StorageType.ToString() })
                    {
                        ImageIndex = 0,
                        Tag = index
                    };
                    if (waveData.IsLooped && waveData.LoopStartOffset > waveData.TotalSamples)
                    {
                        listViewItem2.UseItemStyleForSubItems = false;
                        listViewItem2.SubItems[7].ForeColor = Color.Red;
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

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonHexView_Click(object sender, System.EventArgs e)
        {
            SetHexDisplayPreference(ButtonHexView.Checked);
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
                    List<SampleData> wavesList = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.SfxStoredData;

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
                    FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);
                    List<SampleData> wavesList = parentForm.pnlSoundBankFiles.SfxStoredData;

                    //Start output
                    foreach (ListViewItem selectedItem in listView1.SelectedItems)
                    {
                        SampleData selectedSample = wavesList[(short)selectedItem.Tag];

                        //Create object music
                        DecodedAudio decodedAudio = GenericMethods.DecodeSfxSampleChannels(selectedSample, audioFunctions, parentForm.pnlSoundBankFiles.SoundBankHeaderData);
                        if (decodedAudio != null && decodedAudio.Channels.Length > 0)
                        {
                            SoundFile soundToPlay = new SoundFile();
                            soundToPlay.PcmData = decodedAudio.Channels;
                            soundToPlay.sampleRate = decodedAudio.SampleRate;
                            soundToPlay.channels = (uint)decodedAudio.Channels.Length;
                            ApplyLoopMetadata(soundToPlay, selectedSample, decodedAudio, parentForm.pnlSoundBankFiles.SoundBankHeaderData.FileVersion);

                            //Create Wav File
                            int channelBytes = soundToPlay.PcmData.Min(channel => channel.Length);
                            WavLoopInfo loopInfo = soundToPlay.isLooped && soundToPlay.loopEndPoint > 0
                                ? new WavLoopInfo(soundToPlay.loopStartPoint, (uint)(soundToPlay.loopEndPoint - 1))
                                : EuroSoundWaveWriter.CreateLoopInfo(soundToPlay.isLooped, soundToPlay.loopStartPoint,
                                    (long)channelBytes * soundToPlay.PcmData.Length, soundToPlay.PcmData.Length);
                            EuroSoundWaveWriter.WriteChannelsPcm16(
                                GenericMethods.GetFinalPath(Path.Combine(folderBrowserDialog1.SelectedPath, (short)selectedItem.Tag + ".wav")),
                                soundToPlay.PcmData, (int)soundToPlay.sampleRate, loopInfo);
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
                FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);
                List<SampleData> wavesList = parentForm.pnlSoundBankFiles.SfxStoredData;
                SampleData selectedSample = wavesList[(short)listView1.SelectedItems[0].Tag];

                //Create object music
                DecodedAudio decodedAudio;
                try
                {
                    decodedAudio = GenericMethods.DecodeSfxSampleChannels(selectedSample, audioFunctions, parentForm.pnlSoundBankFiles.SoundBankHeaderData);
                }
                catch (System.Exception exception) when (exception is InvalidDataException || exception is System.InvalidOperationException)
                {
                    MessageBox.Show(this,
                        "This sample could not be decoded with the currently supported codec layout." + System.Environment.NewLine + System.Environment.NewLine + exception.Message,
                        "Audio decoding failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (decodedAudio != null && decodedAudio.Channels.Length > 0)
                {
                    SoundFile soundToPlay = new SoundFile();
                    soundToPlay.PcmData = decodedAudio.Channels;
                    soundToPlay.sampleRate = decodedAudio.SampleRate;
                    soundToPlay.channels = (uint)decodedAudio.Channels.Length;
                    ApplyLoopMetadata(soundToPlay, selectedSample, decodedAudio, parentForm.pnlSoundBankFiles.SoundBankHeaderData.FileVersion);

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
                SortedDictionary<uint, Sample> samplesDisct = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.SfxSamples;
                using (FrmFileRefUsage itemUsage = new FrmFileRefUsage((short)listView1.SelectedItems[0].Tag, null, samplesDisct))
                {
                    itemUsage.ShowDialog();
                }
            }
        }

        private static void ApplyLoopMetadata(SoundFile sound, SampleData sample, DecodedAudio decodedAudio, int fileVersion)
        {
            if (fileVersion == 18 || fileVersion == 21)
            {
                int decodedSamples = decodedAudio.Channels.Min(channel => channel.Length) / 2;
                sound.isLooped = EuroSoundStreamLoopResolver.TryResolveV18(sample, decodedSamples, out uint loopStart, out uint loopEnd);
                sound.loopStartPoint = loopStart;
                sound.loopEndPoint = loopEnd > int.MaxValue ? int.MaxValue : (int)loopEnd;
                return;
            }

            sound.loopStartPoint = sample.LoopStartOffset;
            sound.isLooped = sample.IsLooped;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void InitializeDisplayMenuState()
        {
            suppressDisplayPreferenceEvents = true;
            ButtonHexView.Checked = Properties.Settings.Default.ExplorerOffsetsHex;
            MenuItem_OffsetsHex.Checked = Properties.Settings.Default.ExplorerOffsetsHex;
            MenuItem_SizesWithUnits.Checked = Properties.Settings.Default.ExplorerSizesHumanReadable;
            MenuItem_LoopOffsetsAsSamples.Checked = Properties.Settings.Default.ExplorerSoundBankLoopsAsSamples;
            suppressDisplayPreferenceEvents = false;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_OffsetsHex_CheckedChanged(object sender, System.EventArgs e)
        {
            if (suppressDisplayPreferenceEvents)
            {
                return;
            }

            SetHexDisplayPreference(MenuItem_OffsetsHex.Checked);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_SizesWithUnits_CheckedChanged(object sender, System.EventArgs e)
        {
            if (suppressDisplayPreferenceEvents)
            {
                return;
            }

            Properties.Settings.Default.ExplorerSizesHumanReadable = MenuItem_SizesWithUnits.Checked;
            Properties.Settings.Default.Save();
            ShowWavesList();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_LoopOffsetsAsSamples_CheckedChanged(object sender, System.EventArgs e)
        {
            if (suppressDisplayPreferenceEvents)
            {
                return;
            }

            Properties.Settings.Default.ExplorerSoundBankLoopsAsSamples = MenuItem_LoopOffsetsAsSamples.Checked;
            Properties.Settings.Default.Save();
            ShowWavesList();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void SetHexDisplayPreference(bool checkedState)
        {
            ButtonHexView.Checked = checkedState;
            MenuItem_OffsetsHex.Checked = checkedState;
            Properties.Settings.Default.ExplorerOffsetsHex = checkedState;
            Properties.Settings.Default.Save();
            ShowWavesList();
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
