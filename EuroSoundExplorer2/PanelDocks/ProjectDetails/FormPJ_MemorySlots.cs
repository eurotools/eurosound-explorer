using MusX.Objects;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace EuroSoundExplorer2
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
            ProjectDetails dictToShow = parentForm.pnlSoundBankFiles.projDetailsData;

            //Update textbox count
            textboxCount.Text = dictToShow.memorySlotsData.Count.ToString();

            //Add all items
            listView_ColumnSortingClick1.BeginUpdate();
            foreach (ProjectSlots memSlot in dictToShow.memorySlotsData)
            {
                ListViewItem itemToAdd = new ListViewItem(new string[] { memSlot.SlotNumber.ToString(), memSlot.MemorySize.ToString(), memSlot.Quantity.ToString() });
                listView_ColumnSortingClick1.Items.Add(itemToAdd);
            }
            listView_ColumnSortingClick1.EndUpdate();
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
