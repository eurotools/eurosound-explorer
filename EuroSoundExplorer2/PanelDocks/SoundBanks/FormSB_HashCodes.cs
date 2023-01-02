using EuroSoundExplorer2.CustomControls;
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
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
