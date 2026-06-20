using MusX.Objects;
using sb_explorer.Classes;
using sb_explorer.UI.Formatting;
using System;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace sb_explorer
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FormMarkers : DockContent
    {
        private StreamSample currentStreamSample;
        private MusicSample currentMusicSample;
        private bool suppressDisplayPreferenceEvents;

        //-------------------------------------------------------------------------------------------------------------------------------
        public FormMarkers()
        {
            InitializeComponent();
            InitializeDisplayMenuState();
            AddExtendedMarkerColumns();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowMarkers(StreamSample sampleToDisplay)
        {
            currentStreamSample = sampleToDisplay;
            currentMusicSample = null;

            //Print markers
            lvwMarkers.BeginUpdate();
            lvwMarkers.Items.Clear();
            if (sampleToDisplay.Markers.Length >= 0 && sampleToDisplay.Markers.Length <= 20)
            {
                txtMarkerCount.Text = sampleToDisplay.Markers.Length.ToString();
                txtMarkerCount.ForeColor = SystemColors.ControlText;

                for (int i = 0; i < sampleToDisplay.Markers.Length; i++)
                {
                    Marker musicMarkerStartData = sampleToDisplay.Markers[i];
                    PrintMarkers(musicMarkerStartData, ref i, null, sampleToDisplay);
                }
            }
            else
            {
                txtMarkerCount.ForeColor = Color.Red;
            }
            lvwMarkers.EndUpdate();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowMarkers(MusicSample sampleToDisplay)
        {
            currentMusicSample = sampleToDisplay;
            currentStreamSample = null;

            //Print markers
            lvwMarkers.BeginUpdate();
            lvwMarkers.Items.Clear();
            if (sampleToDisplay.Markers.Length >= 0 && sampleToDisplay.Markers.Length <= 20)
            {
                txtMarkerCount.Text = sampleToDisplay.Markers.Length.ToString();
                txtMarkerCount.ForeColor = SystemColors.ControlText;

                for (int i = 0; i < sampleToDisplay.Markers.Length; i++)
                {
                    Marker musicMarkerStartData = sampleToDisplay.Markers[i];
                    PrintMarkers(musicMarkerStartData, ref i, sampleToDisplay, null);
                }
            }
            else
            {
                txtMarkerCount.ForeColor = Color.Red;
            }
            lvwMarkers.EndUpdate();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void PrintMarkers(Marker musicMarkerStartData, ref int i, MusicSample musicObj, StreamSample sampleObj)
        {
            //Create listview item
            ListViewItem listViewItem = new ListViewItem(new string[] { "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A" })
            {
                UseItemStyleForSubItems = false
            };

            ushort errors = 0;

            //Update UI
            listViewItem.Text = i.ToString();
            listViewItem.SubItems[1].Text = musicMarkerStartData.Index.ToString();
            listViewItem.SubItems[2].Text = FormatMarkerOffset(musicMarkerStartData.Position, musicMarkerStartData.OriginalPosition);
            switch (musicMarkerStartData.Type)
            {
                case 10:
                    listViewItem.SubItems[3].Text = "Start";
                    break;
                case 9:
                    listViewItem.SubItems[3].Text = "End";
                    break;
                case 7:
                    listViewItem.SubItems[3].Text = "Goto";
                    break;
                case 6:
                    listViewItem.SubItems[3].Text = "Loop";
                    break;
                case 5:
                    listViewItem.SubItems[3].Text = "Pause";
                    break;
                case 0:
                    listViewItem.SubItems[3].Text = "Jump";
                    break;
                default:
                    listViewItem.SubItems[3].Text = "Error";
                    break;
            }
            listViewItem.SubItems[4].Text = FormatMarkerOffset(musicMarkerStartData.LoopStart, musicMarkerStartData.OriginalLoopStart);
            listViewItem.SubItems[5].Text = musicMarkerStartData.LoopMarkerCount.ToString();
            listViewItem.SubItems[6].Text = musicMarkerStartData.HasExtendedFields ? musicMarkerStartData.Flags.ToString() : "N/A";
            listViewItem.SubItems[7].Text = musicMarkerStartData.HasExtendedFields ? musicMarkerStartData.Extra.ToString() : "N/A";
            listViewItem.SubItems[8].Text = musicMarkerStartData.HasExtendedFields ? musicMarkerStartData.MarkerCount.ToString() : "N/A";

            //Check for errors
            if (musicObj != null)
            {
                int streamLenght = (int)musicObj.AudioSize * 4;
                if (musicMarkerStartData.Position > streamLenght)
                {
                    errors |= (1 << 2);
                }
                if (musicMarkerStartData.LoopStart > streamLenght)
                {
                    errors |= (1 << 4);
                }
            }
            else
            {
                //Check for errors
                int streamLenght = (int)sampleObj.AudioSize * 4;
                if (musicMarkerStartData.Position > streamLenght)
                {
                    errors |= (1 << 2);
                }
                if (musicMarkerStartData.LoopStart > streamLenght)
                {
                    errors |= (1 << 4);
                }
            }

            //Change color if we have errors
            for (int j = 0; j < listViewItem.SubItems.Count; j++)
            {
                if (Convert.ToBoolean((errors >> j) & 1))
                {
                    listViewItem.SubItems[j].ForeColor = Color.Red;
                }
            }
            lvwMarkers.Items.Add(listViewItem);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static string FormatMarkerOffset(uint samplesValue, uint binaryValue)
        {
            if (Properties.Settings.Default.ExplorerMarkerOffsetsAsSamples)
            {
                return samplesValue.ToString();
            }

            return DisplayValueFormatter.FormatOffset(binaryValue, Properties.Settings.Default.ExplorerOffsetsHex);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void InitializeDisplayMenuState()
        {
            suppressDisplayPreferenceEvents = true;
            MenuItem_MarkerOffsetsAsSamples.Checked = Properties.Settings.Default.ExplorerMarkerOffsetsAsSamples;
            MenuItem_BinaryOffsetsHex.Checked = Properties.Settings.Default.ExplorerOffsetsHex;
            suppressDisplayPreferenceEvents = false;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_MarkerOffsetsAsSamples_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressDisplayPreferenceEvents)
            {
                return;
            }

            Properties.Settings.Default.ExplorerMarkerOffsetsAsSamples = MenuItem_MarkerOffsetsAsSamples.Checked;
            Properties.Settings.Default.Save();
            RefreshCurrentMarkers();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_BinaryOffsetsHex_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressDisplayPreferenceEvents)
            {
                return;
            }

            Properties.Settings.Default.ExplorerOffsetsHex = MenuItem_BinaryOffsetsHex.Checked;
            Properties.Settings.Default.Save();
            RefreshCurrentMarkers();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void AddExtendedMarkerColumns()
        {
            lvwMarkers.Columns.Add("Flags", 55);
            lvwMarkers.Columns.Add("Extra", 55);
            lvwMarkers.Columns.Add("Marker Count", 85);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void RefreshCurrentMarkers()
        {
            if (currentStreamSample != null)
            {
                ShowMarkers(currentStreamSample);
            }
            else if (currentMusicSample != null)
            {
                ShowMarkers(currentMusicSample);
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
