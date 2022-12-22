using MusX.Objects;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace EuroSoundExplorer2
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FormSBiSoundBanks : DockContent
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public FormSBiSoundBanks()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void DisplayHashCodes()
        {
            FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);
            SbiFile dictToShow = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.sbiFileData;
            for (int i = 0; i < dictToShow.projectSoundBanks.Length; i++)
            {
                if (dictToShow.projectSoundBanks[i] == -1)
                {
                    break;
                }
                ListViewItem itemToAdd = new ListViewItem(new string[]
                {
                    string.Format("0x{0:X8}", dictToShow.projectSoundBanks[i]),
                    parentForm.hashTable.GetHashCodeLabel((uint)dictToShow.projectSoundBanks[i])
                })
                {
                    UseItemStyleForSubItems = false
                };

                //Add item to listview
                listView_ColumnSortingClick1.Items.Add(itemToAdd);
            }

            textBoxSoundBanksCount.Text = listView_ColumnSortingClick1.Items.Count.ToString();
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
