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
    public partial class FormStartMarkers : DockContent
    {
        private StreamSample currentStreamSample;
        private MusicSample currentMusicSample;
        private bool suppressDisplayPreferenceEvents;

        //-------------------------------------------------------------------------------------------------------------------------------
        public FormStartMarkers()
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

            //Clear listview
            lvwStartMarkers.BeginUpdate();
            lvwStartMarkers.Items.Clear();

            //Print Start Markers
            if (sampleToDisplay.StartMarkers.Length >= 0 && sampleToDisplay.StartMarkers.Length <= 20)
            {
                Textbox_StartMarkers_Count.Text = sampleToDisplay.StartMarkers.Length.ToString();
                Textbox_StartMarkers_Count.ForeColor = SystemColors.ControlText;

                for (int i = 0; i < sampleToDisplay.StartMarkers.Length; i++)
                {
                    PrintMarkers(sampleToDisplay.StartMarkers[i], ref i, null, sampleToDisplay);
                }
            }
            else
            {
                Textbox_StartMarkers_Count.ForeColor = Color.Red;
            }
            lvwStartMarkers.EndUpdate();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowMarkers(MusicSample sampleToDisplay)
        {
            currentMusicSample = sampleToDisplay;
            currentStreamSample = null;

            //Clear listview
            lvwStartMarkers.BeginUpdate();
            lvwStartMarkers.Items.Clear();

            //Print Start Markers
            if (sampleToDisplay.StartMarkers.Length >= 0 && sampleToDisplay.StartMarkers.Length <= 20)
            {
                Textbox_StartMarkers_Count.Text = sampleToDisplay.StartMarkers.Length.ToString();
                Textbox_StartMarkers_Count.ForeColor = SystemColors.ControlText;

                for (int i = 0; i < sampleToDisplay.StartMarkers.Length; i++)
                {
                    PrintMarkers(sampleToDisplay.StartMarkers[i], ref i, sampleToDisplay, null);
                }
            }
            else
            {
                Textbox_StartMarkers_Count.ForeColor = Color.Red;
            }
            lvwStartMarkers.EndUpdate();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void PrintMarkers(StartMarker startMarker, ref int i, MusicSample musicObj, StreamSample sampleObj)
        {
            //Create listview item
            ListViewItem listViewItem = new ListViewItem(new string[] { "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A" })
            {
                UseItemStyleForSubItems = false
            };

            ushort errors = 0;

            //Update UI
            listViewItem.Text = i.ToString();
            listViewItem.SubItems[1].Text = startMarker.Index.ToString();
            listViewItem.SubItems[2].Text = FormatMarkerOffset(startMarker.Position, startMarker.OriginalPosition);
            switch (startMarker.Type)
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
            listViewItem.SubItems[4].Text = FormatMarkerOffset(startMarker.LoopStart, startMarker.OriginalLoopStart);
            listViewItem.SubItems[5].Text = startMarker.LoopMarkerCount.ToString();
            listViewItem.SubItems[6].Text = startMarker.MarkerPos.ToString();
            listViewItem.SubItems[7].Text = startMarker.HasExtendedFields ? startMarker.Flags.ToString() : "N/A";
            listViewItem.SubItems[8].Text = startMarker.HasExtendedFields ? startMarker.Extra.ToString() : "N/A";
            listViewItem.SubItems[9].Text = startMarker.HasExtendedFields ? startMarker.MarkerCount.ToString() : "N/A";
            listViewItem.SubItems[10].Text = startMarker.HasExtendedFields ? startMarker.IsInstant.ToString() : "N/A";
            listViewItem.SubItems[11].Text = startMarker.HasExtendedFields ? startMarker.InstantBuffer.ToString() : "N/A";
            listViewItem.SubItems[12].Text = startMarker.HasExtendedFields ? DisplayValueFormatter.FormatOffset(startMarker.StateA, Properties.Settings.Default.ExplorerOffsetsHex) : "N/A";
            listViewItem.SubItems[13].Text = startMarker.HasExtendedFields ? DisplayValueFormatter.FormatOffset(startMarker.StateB, Properties.Settings.Default.ExplorerOffsetsHex) : "N/A";

            //Check for errors
            if (musicObj != null)
            {
                int streamLenght = (int)musicObj.AudioSize * 4;
                if (startMarker.Position > streamLenght)
                {
                    errors |= (1 << 2);
                }
                if (startMarker.LoopStart > streamLenght)
                {
                    errors |= (1 << 4);
                }
            }
            else
            {
                //Check for errors
                int streamLenght = (int)sampleObj.AudioSize * 4;
                if (startMarker.Position > streamLenght)
                {
                    errors |= (1 << 2);
                }
                if (startMarker.LoopStart > streamLenght)
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
            lvwStartMarkers.Items.Add(listViewItem);
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
            lvwStartMarkers.Columns.Add("Flags", 55);
            lvwStartMarkers.Columns.Add("Extra", 55);
            lvwStartMarkers.Columns.Add("Marker Count", 85);
            lvwStartMarkers.Columns.Add("Is Instant", 75);
            lvwStartMarkers.Columns.Add("Instant Buffer", 95);
            lvwStartMarkers.Columns.Add("State A", 75);
            lvwStartMarkers.Columns.Add("State B", 75);
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
