using MusX.Objects;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace sb_explorer
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FormSD_SoundDetails : DockContent
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public FormSD_SoundDetails()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowData()
        {
            FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);
            SoundDetails fileData = parentForm.pnlSoundBankFiles.soundDetails;

            int m_ErrorCount = 0;
            lstvSfxItems.BeginUpdate();
            lstvSfxItems.Items.Clear();
            foreach (SoundDetailsData itemToadd in fileData.sfxItems)
            {
                ListViewItem itemToAdd = new ListViewItem(new string[]
                {
                    string.Format("0x{0:X8}", itemToadd.HashCode),
                    string.Empty,
                    itemToadd.InnerRadius.ToString(),
                    itemToadd.OuterRadius.ToString(),
                    itemToadd.Duration.ToString(),
                    itemToadd.Looping.ToString(),
                    itemToadd.Tracking3D.ToString(),
                    itemToadd.SampleStreamed.ToString(),
                    itemToadd.Is3D.ToString()
                });

                //Check that is not an empty block
                if ((itemToadd.InnerRadius != 0 && itemToadd.OuterRadius != 0) || itemToadd.Duration != 0)
                {
                    itemToAdd.SubItems[1].Text = parentForm.hashTable.GetHashCodeLabel((uint)itemToadd.HashCode);

                    //Check for errors
                    if (itemToadd.InnerRadius < 0.0 || itemToadd.InnerRadius > 30000.0)
                    {
                        ++m_ErrorCount;
                        itemToAdd.ForeColor = Color.Red;
                    }
                    if (itemToadd.OuterRadius <= 0.0 || itemToadd.OuterRadius > 30000.0)
                    {
                        ++m_ErrorCount;
                        itemToAdd.ForeColor = Color.Red;
                    }
                    if (itemToadd.InnerRadius > (double)itemToadd.OuterRadius)
                    {
                        ++m_ErrorCount;
                        itemToAdd.ForeColor = Color.Red;
                    }
                    if (itemToadd.Duration < 0 || itemToadd.Duration > 600000)
                    {
                        ++m_ErrorCount;
                        itemToAdd.ForeColor = Color.Red;
                    }
                    if (!itemToadd.Looping && itemToadd.SampleStreamed && itemToadd.Duration <= 0)
                    {
                        ++m_ErrorCount;
                        itemToAdd.ForeColor = Color.Red;
                    }
                }

                //Add item
                lstvSfxItems.Items.Add(itemToAdd);
            }
            lstvSfxItems.EndUpdate();

            //Update labels
            txtHashCodeMax.Text = string.Format("0x{0:X8}", fileData.MaxHashCode);
            txtHashCodeMin.Text = string.Format("0x{0:X8}", fileData.MinHashCode);
            txtItemsCount.Text = fileData.sfxItems.Length.ToString();
            txtErrorsCount.Text = m_ErrorCount.ToString();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ClearData()
        {
            lstvSfxItems.BeginUpdate();
            lstvSfxItems.Items.Clear();
            lstvSfxItems.EndUpdate();
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
                GenericMethods.FilterListView(txtBoxSearch.Text, lstvSfxItems);
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
            if (lstvSfxItems.SelectedItems.Count > 0)
            {
                Clipboard.SetText(lstvSfxItems.SelectedItems[0].SubItems[0].Text);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_CopyLabel_Click(object sender, System.EventArgs e)
        {
            if (lstvSfxItems.SelectedItems.Count > 0)
            {
                Clipboard.SetText(lstvSfxItems.SelectedItems[0].SubItems[1].Text);
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
