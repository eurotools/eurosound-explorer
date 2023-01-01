using MusX.Objects;
using System;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace EuroSoundExplorer2
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FormMarkers : DockContent
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public FormMarkers()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowMarkers(StreamSample sampleToDisplay, int frequency)
        {
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
                    PrintMarkers(musicMarkerStartData, ref i, frequency, null, sampleToDisplay);
                }
            }
            else
            {
                txtMarkerCount.ForeColor = Color.Red;
            }
            lvwMarkers.EndUpdate();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowMarkers(MusicSample sampleToDisplay, int frequency)
        {
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
                    PrintMarkers(musicMarkerStartData, ref i, frequency, sampleToDisplay, null);
                }
            }
            else
            {
                txtMarkerCount.ForeColor = Color.Red;
            }
            lvwMarkers.EndUpdate();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void PrintMarkers(Marker musicMarkerStartData, ref int i, int frequency, MusicSample musicObj, StreamSample sampleObj)
        {
            //Create listview item
            ListViewItem listViewItem = new ListViewItem(new string[] { "N/A", "N/A", "N/A", "N/A", "N/A", "N/A" })
            {
                UseItemStyleForSubItems = false
            };

            ushort errors = 0;

            //Check Conversion
            decimal freqDiv = decimal.Divide(frequency, 1000);

            //Update UI
            listViewItem.Text = i.ToString();
            listViewItem.SubItems[1].Text = musicMarkerStartData.Index.ToString();
            listViewItem.SubItems[2].Text = string.Format("{0:G7} ms", musicMarkerStartData.Position / freqDiv);
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
            listViewItem.SubItems[4].Text = string.Format("{0:G7} ms", musicMarkerStartData.LoopStart / freqDiv);
            listViewItem.SubItems[5].Text = musicMarkerStartData.LoopMarkerCount.ToString();

            //Check for errors
            if (musicObj != null)
            {
                int streamLenght = (musicObj.EncodedData[0].Length + musicObj.EncodedData[1].Length) * 4;
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
                int streamLenght = sampleObj.EncodedData.Length * 4;
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
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
