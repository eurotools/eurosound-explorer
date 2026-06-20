using MusX.Objects;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace sb_explorer
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FormPJ_MemorySlots : DockContent
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public FormPJ_MemorySlots()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowData()
        {
            FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);
            ProjectDetails dictToShow = parentForm.pnlSoundBankFiles.ProjDetailsData;

            //Update textbox count
            textboxCount.Text = dictToShow.memorySlotsData.Count.ToString();

            //Add all items
            listView_ColumnSortingClick1.BeginUpdate();
            listView_ColumnSortingClick1.Items.Clear();
            if (dictToShow.memoryMapsData.Count > 0)
            {
                colMemSlotNo.Text = "Map";
                colMemSize.Text = "Slot";
                colMemQuant.Text = "Memory Size";

                foreach (ProjectMemoryMap memoryMap in dictToShow.memoryMapsData)
                {
                    for (int i = 0; i < memoryMap.SlotSizes.Count; i++)
                    {
                        ListViewItem itemToAdd = new ListViewItem(new string[] { memoryMap.Name, i.ToString(), memoryMap.SlotSizes[i].ToString() });
                        listView_ColumnSortingClick1.Items.Add(itemToAdd);
                    }
                }
            }
            else
            {
                colMemSlotNo.Text = "No";
                colMemSize.Text = "Memory Size";
                colMemQuant.Text = "Quantity";

                foreach (ProjectSlots memSlot in dictToShow.memorySlotsData)
                {
                    ListViewItem itemToAdd = new ListViewItem(new string[] { memSlot.SlotNumber.ToString(), memSlot.MemorySize.ToString(), memSlot.Quantity.ToString() });
                    listView_ColumnSortingClick1.Items.Add(itemToAdd);
                }
            }
            listView_ColumnSortingClick1.EndUpdate();
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
