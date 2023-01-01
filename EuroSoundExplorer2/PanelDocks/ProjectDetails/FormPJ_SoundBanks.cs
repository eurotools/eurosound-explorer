using MusX.Objects;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace EuroSoundExplorer2
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FormPJ_SoundBanks : DockContent
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public FormPJ_SoundBanks()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowData()
        {
            FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);
            ProjectDetails dictToShow = parentForm.pnlSoundBankFiles.projDetailsData;

            //Update textbox count
            textboxCount.Text = dictToShow.soundBanksData.Count.ToString();

            //Add all items
            listView_ColumnSortingClick1.BeginUpdate();
            foreach (ProjectSoundBank soundBank in dictToShow.soundBanksData)
            {
                ListViewItem itemToAdd = new ListViewItem(new string[] { "0x" + soundBank.HashCode.ToString("X8"), soundBank.SlotNumber.ToString() });
                listView_ColumnSortingClick1.Items.Add(itemToAdd);
            }
            listView_ColumnSortingClick1.EndUpdate();
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
