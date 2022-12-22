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
                    { UseItemStyleForSubItems = false, Tag = itemToShow.Key };

                    //Check if we need to highlight this item
                    if (itemToAdd.SubItems[0].Text.StartsWith("**"))
                    {
                        itemToAdd.ForeColor = Color.Red;
                        itemToAdd.SubItems[1].Text = "Not Found";
                    }

                    //Add item to listview
                    listView1.Items.Add(itemToAdd);
                }
                listView1.EndUpdate();
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ListView1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            SortedDictionary<uint, Sample> dictToShow = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.sfxSamples;

            if (listView1.SelectedItems.Count == 1)
            {
                uint hashCode = (uint)listView1.SelectedItems[0].Tag;
                if (dictToShow.ContainsKey(hashCode))
                {
                    Sample sampleData = dictToShow[hashCode];
                    ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSbSamplePool.ShowSampleData(sampleData);
                    ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSbSampleProps.ShowSampleData(sampleData);
                }
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
