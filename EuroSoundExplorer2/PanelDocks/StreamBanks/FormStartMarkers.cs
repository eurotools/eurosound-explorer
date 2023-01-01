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
    public partial class FormStartMarkers : DockContent
    {
        //private readonly CalculusLoopOffsets loopOffsets = new CalculusLoopOffsets();

        //-------------------------------------------------------------------------------------------------------------------------------
        public FormStartMarkers()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowMarkers(StreamSample sampleToDisplay, int frequency)
        {
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
                    PrintMarkers(sampleToDisplay.StartMarkers[i], ref i, frequency, null, sampleToDisplay);
                }
            }
            else
            {
                Textbox_StartMarkers_Count.ForeColor = Color.Red;
            }
            lvwStartMarkers.EndUpdate();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowMarkers(MusicSample sampleToDisplay, int frequency)
        {
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
                    PrintMarkers(sampleToDisplay.StartMarkers[i], ref i, frequency, sampleToDisplay, null);
                }
            }
            else
            {
                Textbox_StartMarkers_Count.ForeColor = Color.Red;
            }
            lvwStartMarkers.EndUpdate();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void PrintMarkers(StartMarker startMarker, ref int i, int frequency, MusicSample musicObj, StreamSample sampleObj)
        {
            //Create listview item
            ListViewItem listViewItem = new ListViewItem(new string[] { "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A" })
            {
                UseItemStyleForSubItems = false
            };

            ushort errors = 0;


            //Check Conversion
            decimal freqDiv = decimal.Divide(frequency, 1000);

            //Update UI
            listViewItem.Text = i.ToString();
            listViewItem.SubItems[1].Text = startMarker.Index.ToString();
            listViewItem.SubItems[2].Text = string.Format("{0:G7} ms", startMarker.Position / freqDiv);
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
            listViewItem.SubItems[4].Text = string.Format("{0:G7} ms", startMarker.LoopStart / freqDiv);
            listViewItem.SubItems[5].Text = startMarker.LoopMarkerCount.ToString();
            listViewItem.SubItems[6].Text = string.Format("{0:G7} ms", startMarker.MarkerPos / freqDiv);

            //Check for errors
            if (musicObj != null)
            {
                int streamLenght = (musicObj.EncodedData[0].Length + musicObj.EncodedData[1].Length) * 4;
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
                int streamLenght = sampleObj.EncodedData.Length * 4;
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
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
