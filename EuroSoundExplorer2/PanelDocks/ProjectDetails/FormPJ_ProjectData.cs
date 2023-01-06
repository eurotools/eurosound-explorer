using MusX.Objects;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace sb_explorer
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FormPJ_ProjectData : DockContent
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public FormPJ_ProjectData()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowData()
        {
            FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);
            ProjectDetails projDataObj = parentForm.pnlSoundBankFiles.projDetailsData;

            propGrid_ProjData.SelectedObject = projDataObj;

            //Add all items
            lvwFlags.BeginUpdate();
            for (int i = 0; i < projDataObj.flagsValues.Length; i++)
            {
                ListViewItem itemToAdd = new ListViewItem(new string[] { i.ToString(), projDataObj.flagsValues[i].ToString() });
                lvwFlags.Items.Add(itemToAdd);
            }
            lvwFlags.EndUpdate();
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
