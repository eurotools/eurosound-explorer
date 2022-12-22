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
        //-------------------------------------------------------------------------------------------------------------------------------
        public FormStartMarkers()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowMarkers(StreamSample sampleToDisplay)
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
                    //Create listview item
                    ListViewItem listViewItem = new ListViewItem(new string[] { "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A" })
                    {
                        UseItemStyleForSubItems = false
                    };

                    ushort errors = 0;

                    StartMarker musicMarkerStartData = sampleToDisplay.StartMarkers[i];
                    listViewItem.Text = i.ToString();
                    listViewItem.SubItems[1].Text = musicMarkerStartData.Index.ToString();
                    listViewItem.SubItems[2].Text = musicMarkerStartData.Position.ToString();
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
                    listViewItem.SubItems[4].Text = musicMarkerStartData.LoopStart.ToString();
                    listViewItem.SubItems[5].Text = musicMarkerStartData.LoopMarkerCount.ToString();
                    listViewItem.SubItems[6].Text = musicMarkerStartData.MarkerPos.ToString();

                    //Check for errors
                    int streamLenght = sampleToDisplay.EncodedData.Length * 4;

                    if (musicMarkerStartData.Position > streamLenght)
                        errors |= (1 << 2);
                    if (musicMarkerStartData.LoopStart > streamLenght)
                        errors |= (1 << 4);

                    //Change color if we have errors
                    for (int j = 0; j < listViewItem.SubItems.Count; j++)
                    {
                        if (Convert.ToBoolean((errors >> j) & 1))
                            listViewItem.SubItems[j].ForeColor = Color.Red;
                    }
                    lvwStartMarkers.Items.Add(listViewItem);
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
                    //Create listview item
                    ListViewItem listViewItem = new ListViewItem(new string[] { "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A" })
                    {
                        UseItemStyleForSubItems = false
                    };

                    ushort errors = 0;

                    StartMarker musicMarkerStartData = sampleToDisplay.StartMarkers[i];
                    listViewItem.Text = i.ToString();
                    listViewItem.SubItems[1].Text = musicMarkerStartData.Index.ToString();
                    listViewItem.SubItems[2].Text = musicMarkerStartData.Position.ToString();
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
                    listViewItem.SubItems[4].Text = musicMarkerStartData.LoopStart.ToString();
                    listViewItem.SubItems[5].Text = musicMarkerStartData.LoopMarkerCount.ToString();
                    listViewItem.SubItems[6].Text = musicMarkerStartData.MarkerPos.ToString();

                    //Check for errors
                    int streamLenght = sampleToDisplay.EncodedData.Length * 4;

                    if (musicMarkerStartData.Position > streamLenght)
                        errors |= (1 << 2);
                    if (musicMarkerStartData.LoopStart > streamLenght)
                        errors |= (1 << 4);

                    //Change color if we have errors
                    for (int j = 0; j < listViewItem.SubItems.Count; j++)
                    {
                        if (Convert.ToBoolean((errors >> j) & 1))
                            listViewItem.SubItems[j].ForeColor = Color.Red;
                    }
                    lvwStartMarkers.Items.Add(listViewItem);
                }
            }
            else
            {
                Textbox_StartMarkers_Count.ForeColor = Color.Red;
            }
            lvwStartMarkers.EndUpdate();
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
