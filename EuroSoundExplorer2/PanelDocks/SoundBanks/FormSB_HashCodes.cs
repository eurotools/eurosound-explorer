using MusX.Objects;
using sb_explorer.CustomControls;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace sb_explorer
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FormSB_HashCodes : DockContent
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public FormSB_HashCodes()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonApplyFilter_Click(object sender, System.EventArgs e)
        {
            if (ButtonApplyFilter.Checked)
            {
                //Iterate through all list items
                GenericMethods.FilterListView(txtBoxSearch.Text, listView1);
            }
            else
            {
                SetHashCodesToListView();
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void SetHashCodesToListView()
        {
            SortedDictionary<uint, Sample> dictToShow = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.sfxSamples;
            FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);

            //Print Data
            if (dictToShow.Count > 0)
            {
                txtSfxCount.Text = dictToShow.Count.ToString();

                listView1.BeginUpdate();
                listView1.Items.Clear();
                foreach (KeyValuePair<uint, Sample> itemToShow in dictToShow)
                {
                    ListViewItem itemToAdd = new ListViewItem(new string[]
                    {
                        string.Format("0x{0:X8}", itemToShow.Key),
                        "OK",
                        parentForm.hashTable.GetHashCodeLabel(itemToShow.Key),
                    })
                    { UseItemStyleForSubItems = false, Tag = itemToShow.Key, ImageIndex = 0 };

                    //Check if we need to highlight this item
                    if (itemToAdd.SubItems[0].Text.StartsWith("**"))
                    {
                        itemToAdd.ForeColor = Color.Red;
                        itemToAdd.SubItems[1].Text = "Not Found";
                    }
                    //Add item to listview
                    listView1.Items.Add(itemToAdd);

                    //Add another imageIndex
                    ListView_ColumnSortingClick.AddImageToSubItem(itemToAdd, 1, 2, listView1.Handle);
                }
                listView1.EndUpdate();
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonCheckDuplicated_Click(object sender, System.EventArgs e)
        {
            List<uint> duplicatedHashCodes = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.duplicatedHashCodes;
            if (duplicatedHashCodes.Count > 0)
            {
                using (FrmDuplicatedHashCodes duplHashCodes = new FrmDuplicatedHashCodes(duplicatedHashCodes))
                {
                    duplHashCodes.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("No duplicated hashcodes found", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonSaveList_Click(object sender, System.EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Create text file
                using (StreamWriter writer = new StreamWriter(File.Open(saveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.Read), Encoding.UTF8))
                {
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        writer.WriteLine(listView1.Items[i].SubItems[2].Text);
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ListView1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //If one of these options is checked, execute this method
            if (ButtonListProperties.Checked || ButtonSendToSamplePool.Checked)
            {
                FrmMain parentForm = (FrmMain)Application.OpenForms[nameof(FrmMain)];
                SortedDictionary<uint, Sample> dictToShow = parentForm.pnlSoundBankFiles.sfxSamples;

                //Ensure that we have one item selected in the listview
                if (listView1.SelectedItems.Count == 1)
                {
                    uint hashCode = (uint)listView1.SelectedItems[0].Tag;
                    if (dictToShow.ContainsKey(hashCode))
                    {
                        Sample sampleData = dictToShow[hashCode];

                        //Show Samples
                        if (ButtonSendToSamplePool.Checked)
                        {
                            parentForm.pnlSbSamplePool.ShowSampleData(sampleData);
                        }

                        //Show SFX properties
                        if (ButtonListProperties.Checked)
                        {
                            parentForm.pnlSbSampleProps.ShowSampleData(sampleData);
                        }
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------
        //  CONTEXT MENU
        //-------------------------------------------------------------------------------------------
        private void MenuItem_CopyHashCode_Click(object sender, System.EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                Clipboard.SetText(listView1.SelectedItems[0].SubItems[0].Text);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_CopyLabel_Click(object sender, System.EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                Clipboard.SetText(listView1.SelectedItems[0].SubItems[2].Text);
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
