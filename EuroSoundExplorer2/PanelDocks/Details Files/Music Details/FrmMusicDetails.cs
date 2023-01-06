using MusX.Objects;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace sb_explorer
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FrmMusicDetails : DockContent
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public FrmMusicDetails()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowData()
        {
            FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);
            MusicDetails fileData = parentForm.pnlSoundBankFiles.musicDetails;

            int m_ErrorCount = 0;
            lstvMfxItems.BeginUpdate();
            lstvMfxItems.Items.Clear();
            foreach (MusicDetailsData itemToadd in fileData.musicItems)
            {
                ListViewItem itemToAdd = new ListViewItem(new string[]
                {
                    string.Format("0x{0:X8}", itemToadd.HashCode),
                    parentForm.hashTable.GetHashCodeLabel((uint)itemToadd.HashCode),
                    itemToadd.Duration.ToString(),
                    itemToadd.MusicLooping.ToString(),
                    itemToadd.UserValue.ToString()
                });
                lstvMfxItems.Items.Add(itemToAdd);
            }
            lstvMfxItems.EndUpdate();

            //Update labels
            txtHashCodeMax.Text = string.Format("0x{0:X8}", fileData.MaxHashCode);
            txtHashCodeMin.Text = string.Format("0x{0:X8}", fileData.MinHashCode);
            txtItemsCount.Text = fileData.musicItems.Length.ToString();
            txtErrorsCount.Text = m_ErrorCount.ToString();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ClearData()
        {
            lstvMfxItems.BeginUpdate();
            lstvMfxItems.Items.Clear();
            lstvMfxItems.EndUpdate();
            txtHashCodeMax.Text = string.Format("0x{0:X8}", 0);
            txtHashCodeMin.Text = string.Format("0x{0:X8}", 0);
            txtItemsCount.Text = "0";
            txtErrorsCount.Text = "0";
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonApplyFilter_Click(object sender, System.EventArgs e)
        {
            if (ButtonApplyFilter.Checked)
            {
                //Iterate through all list items
                GenericMethods.FilterListView(txtBoxSearch.Text, lstvMfxItems);
            }
            else
            {
                ShowData();
            }
        }

        //-------------------------------------------------------------------------------------------
        //  Context Menu
        //-------------------------------------------------------------------------------------------
        private void MenuItem_CopyHashCode_Click(object sender, System.EventArgs e)
        {
            if (lstvMfxItems.SelectedItems.Count > 0)
            {
                Clipboard.SetText(lstvMfxItems.SelectedItems[0].SubItems[0].Text);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_CopyLabel_Click(object sender, System.EventArgs e)
        {
            if (lstvMfxItems.SelectedItems.Count > 0)
            {
                Clipboard.SetText(lstvMfxItems.SelectedItems[0].SubItems[1].Text);
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
