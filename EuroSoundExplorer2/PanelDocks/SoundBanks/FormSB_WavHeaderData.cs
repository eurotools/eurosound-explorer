using EuroSoundExplorer2.Classes;
using MusX.Objects;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace EuroSoundExplorer2
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FormSB_WavHeaderData : DockContent
    {
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
        //  CONTEXT MENU
        //-------------------------------------------------------------------------------------------
        private void MenuItem_Save_Click(object sender, System.EventArgs e)
        {

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
