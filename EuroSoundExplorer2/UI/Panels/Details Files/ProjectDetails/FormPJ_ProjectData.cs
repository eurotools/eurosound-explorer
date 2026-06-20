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
            ProjectDetails projDataObj = parentForm.pnlSoundBankFiles.ProjDetailsData;

            propGrid_ProjData.SelectedObject = projDataObj;

            //Add all items
            lvwFlags.BeginUpdate();
            lvwFlags.Items.Clear();
            if (projDataObj.userValues.Count > 0)
            {
                for (int i = 0; i < projDataObj.userValues.Count; i++)
                {
                    ListViewItem itemToAdd = new ListViewItem(new string[] { i.ToString(), projDataObj.userValues[i].ToString() });
                    lvwFlags.Items.Add(itemToAdd);
                }
            }
            else
            {
                for (int i = 0; i < projDataObj.flagsValues.Length; i++)
                {
                    ListViewItem itemToAdd = new ListViewItem(new string[] { i.ToString(), projDataObj.flagsValues[i].ToString() });
                    lvwFlags.Items.Add(itemToAdd);
                }
            }
            lvwFlags.EndUpdate();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ClearData()
        {
            propGrid_ProjData.SelectedObject = null;
            lvwFlags.BeginUpdate();
            lvwFlags.Items.Clear();
            lvwFlags.EndUpdate();
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
