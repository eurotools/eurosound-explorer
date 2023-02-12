using sb_explorer.CustomControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace sb_explorer
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FrmDuplicatedHashCodes : Form
    {
        private readonly List<uint> hashCodesList;

        //-------------------------------------------------------------------------------------------------------------------------------
        public FrmDuplicatedHashCodes(List<uint> _hashcodesList)
        {
            InitializeComponent();
            hashCodesList = _hashcodesList;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void FrmDuplicatedHashCodes_Load(object sender, EventArgs e)
        {
            FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);

            lvwDuplicatedHashCodes.BeginUpdate();
            lvwDuplicatedHashCodes.Items.Clear();
            foreach (uint itemToShow in hashCodesList)
            {
                ListViewItem itemToAdd = new ListViewItem(new string[]
                {
                        string.Format("0x{0:X8}", itemToShow),
                        "OK",
                        parentForm.hashTable.GetHashCodeLabel(itemToShow),
                })
                { UseItemStyleForSubItems = false, Tag = itemToShow, ImageIndex = 0 };

                //Check if we need to highlight this item
                if (itemToAdd.SubItems[0].Text.StartsWith("**"))
                {
                    itemToAdd.ForeColor = Color.Red;
                    itemToAdd.SubItems[1].Text = "Not Found";
                }
                //Add item to listview
                lvwDuplicatedHashCodes.Items.Add(itemToAdd);

                //Add another imageIndex
                ListView_ColumnSortingClick.AddImageToSubItem(itemToAdd, 1, 2, lvwDuplicatedHashCodes.Handle);
            }
            lvwDuplicatedHashCodes.EndUpdate();
        }

        //-------------------------------------------------------------------------------------------
        //  CONTEXT MENU
        //-------------------------------------------------------------------------------------------
        private void MenuItem_CopyHashCode_Click(object sender, System.EventArgs e)
        {
            if (lvwDuplicatedHashCodes.SelectedItems.Count > 0)
            {
                Clipboard.SetText(lvwDuplicatedHashCodes.SelectedItems[0].SubItems[0].Text);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_CopyLabel_Click(object sender, System.EventArgs e)
        {
            if (lvwDuplicatedHashCodes.SelectedItems.Count > 0)
            {
                Clipboard.SetText(lvwDuplicatedHashCodes.SelectedItems[0].SubItems[2].Text);
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
