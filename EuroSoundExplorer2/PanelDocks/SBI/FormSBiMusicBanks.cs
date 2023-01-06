using MusX.Objects;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using static sb_explorer.Enumerations;
using static MusX.Readers.SfxFunctions;

namespace sb_explorer
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FormSBiMusicBanks : DockContent
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public FormSBiMusicBanks()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void DisplayHashCodes()
        {
            FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);
            SbiFile dictToShow = parentForm.pnlSoundBankFiles.sbiFileData;

            //Get Game and Version
            int selectedVersion = parentForm.configuration.FileVersion;
            Title selectedTitle = parentForm.configuration.TitleSelected;

            //Print Data
            for (int i = 0; i < dictToShow.projectMusicBanks.Length; i++)
            {
                if (dictToShow.projectMusicBanks[i] == -1)
                {
                    break;
                }
                ListViewItem itemToAdd = new ListViewItem(new string[]
                {
                    string.Format("0x{0:X8}", dictToShow.projectMusicBanks[i]),
                    parentForm.hashTable.GetHashCodeLabel((uint)GenericMethods.GetHashCodeWithSection(FileType.MusicFile, dictToShow.projectMusicBanks[i], selectedVersion, selectedTitle))
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
