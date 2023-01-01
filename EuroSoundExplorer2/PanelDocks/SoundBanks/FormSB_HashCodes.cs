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
                    { UseItemStyleForSubItems = false, Tag = itemToShow.Key, ImageIndex = 0 };

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

        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawBackground();
            Size sz = listView1.SmallImageList.ImageSize;
            int idx = 1;
            if (e.SubItem.Tag != null) idx = (int)e.SubItem.Tag;
            Bitmap bmp = (Bitmap)listView1.SmallImageList.Images[idx];
            Rectangle rTgt = new Rectangle(e.Bounds.Location, sz);
            bool selected = e.ItemState.HasFlag(ListViewItemStates.Selected);
            // optionally show selection:
            if (selected) e.Graphics.FillRectangle(Brushes.CornflowerBlue, e.Bounds);

            if (bmp != null) e.Graphics.DrawImage(bmp, rTgt);

            // optionally draw text
            e.Graphics.DrawString(e.SubItem.Text, listView1.Font,
                                  selected ? Brushes.White : Brushes.Black,
                                  e.Bounds.X + sz.Width + 2, e.Bounds.Y + 2);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
