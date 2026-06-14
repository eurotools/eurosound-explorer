using MusX.Objects;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace sb_explorer
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FormMM_MusicMarkers : DockContent
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public FormMM_MusicMarkers()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowData()
        {
            FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);
            MusicMarkers fileData = parentForm.pnlSoundBankFiles.MusicMarkers;

            txtMusicHeadersCount.Text = fileData.MusicHeaders.Count.ToString();
            txtMarkerCountsCount.Text = fileData.MusicMarkerCounts.Count.ToString();
            txtMarkerListsCount.Text = fileData.MusicMarkerLists.Count.ToString();

            lvwMusicHeaders.BeginUpdate();
            lvwMusicHeaders.Items.Clear();
            foreach (MusicMarkerHeader item in fileData.MusicHeaders)
            {
                ListViewItem itemToAdd = new ListViewItem(new string[]
                {
                    string.Format("0x{0:X8}", item.MusicHashCode),
                    parentForm.HashTable.GetHashCodeLabel(item.MusicHashCode),
                    string.Format("0x{0:X8}", item.StreamDataOffset),
                    item.BaseVolume.ToString(),
                    item.Padding.ToString()
                });
                lvwMusicHeaders.Items.Add(itemToAdd);
            }
            lvwMusicHeaders.EndUpdate();

            lvwMarkerCounts.BeginUpdate();
            lvwMarkerCounts.Items.Clear();
            foreach (MusicMarkerCounts item in fileData.MusicMarkerCounts)
            {
                ListViewItem itemToAdd = new ListViewItem(new string[]
                {
                    item.StartMarkerCount.ToString(),
                    item.MarkerCount.ToString(),
                    item.Padding0.ToString(),
                    item.Padding1.ToString()
                });
                lvwMarkerCounts.Items.Add(itemToAdd);
            }
            lvwMarkerCounts.EndUpdate();

            lvwMarkerLists.BeginUpdate();
            lvwMarkerLists.Items.Clear();
            foreach (MusicMarkerListEntry item in fileData.MusicMarkerLists)
            {
                ListViewItem itemToAdd = new ListViewItem(new string[]
                {
                    item.Position.ToString(),
                    item.LoopStart.ToString(),
                    item.Padding0.ToString(),
                    item.Padding1.ToString()
                });
                lvwMarkerLists.Items.Add(itemToAdd);
            }
            lvwMarkerLists.EndUpdate();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ClearData()
        {
            txtMusicHeadersCount.Text = "0";
            txtMarkerCountsCount.Text = "0";
            txtMarkerListsCount.Text = "0";

            lvwMusicHeaders.BeginUpdate();
            lvwMusicHeaders.Items.Clear();
            lvwMusicHeaders.EndUpdate();

            lvwMarkerCounts.BeginUpdate();
            lvwMarkerCounts.Items.Clear();
            lvwMarkerCounts.EndUpdate();

            lvwMarkerLists.BeginUpdate();
            lvwMarkerLists.Items.Clear();
            lvwMarkerLists.EndUpdate();
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
