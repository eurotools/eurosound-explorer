using MusX.Readers;
using System;
using System.IO;
using System.Windows.Forms;
using static sb_explorer.Enumerations;
using static MusX.Readers.SfxFunctions;

namespace sb_explorer
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FrmDataViewer : Form
    {
        private readonly SoundBankReader sbReader = new SoundBankReader();
        private readonly StreamBankReader strReader = new StreamBankReader();
        private readonly MusicBankReader musReader = new MusicBankReader();
        private readonly SbiBankReader sbiReader = new SbiBankReader();
        private readonly ProjectDetailsReader projDetReader = new ProjectDetailsReader();
        private readonly int m_ErrorCount = 0;
        private string sfxFilePath = string.Empty;

        //-------------------------------------------------------------------------------------------------------------------------------
        public FrmDataViewer(string fileToLoad = "")
        {
            InitializeComponent();
            sfxFilePath = fileToLoad;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void FrmDataViewer_Load(object sender, EventArgs e)
        {
            FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);

            //If the file path has not been passed as an argument ask user 
            if (string.IsNullOrEmpty(sfxFilePath))
            {
                openFileDialog1.InitialDirectory = parentForm.configuration.ProjectFolder;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    sfxFilePath = openFileDialog1.FileName;
                }
                else
                {
                    Close();
                }
            }

            //Check that the string is not null and show the data
            if (!string.IsNullOrEmpty(sfxFilePath))
            {
                //Update form title
                Text = string.Format("Data Viewer - {0}", Path.GetFileName(sfxFilePath));

                //Get selected info from the mainform
                Title selectedTitle = parentForm.configuration.TitleSelected;

                //Get version of MusX Files
                int hashCode = sbReader.GetFileHashCode(sfxFilePath);
                int fileVersion = sbiReader.GetFileVersion(sfxFilePath);
                try
                {
                    FileType fileType = GenericMethods.GetFileType(hashCode, fileVersion, sfxFilePath, selectedTitle);
                    switch (fileType)
                    {
                        case FileType.SoundbankFile:
                            ShowSoundBank(sfxFilePath);
                            break;
                        case FileType.StreamFile:
                            ShowStreamBank(sfxFilePath);
                            break;
                        case FileType.MusicFile:
                            ShowMusicBank(sfxFilePath);
                            break;
                        case FileType.SBI:
                            ShowSbiBank(sfxFilePath);
                            break;
                        case FileType.ProjectDetails:
                            ShowProjectDetails(sfxFilePath);
                            break;
                        default:
                            MessageBox.Show("Could not load this file, probably is from an unsupported version or game.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            Close();
                            break;
                    }

                    //Expand TreeView
                    if (treeView1.Nodes.Count > 0)
                    {
                        treeView1.Nodes[0].Expand();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
            }
        }

        //-------------------------------------------------------------------------------------------
        //  TOOLBAR FUNCTIONS
        //-------------------------------------------------------------------------------------------
        private void ButtonFind_Click(object sender, EventArgs e)
        {
            if (treeView1.Nodes.Count > 0 && textboxFind.Text.Length > 0)
            {
                TreeNode n = treeView1.SelectedNode;
                if (n != null)
                {
                    n = NextNode(n, false);
                }
                if (n == null)
                {
                    n = treeView1.Nodes[0];
                }

                string upper = textboxFind.Text.ToUpper();
                for (; n != null; n = NextNode(n, false))
                {
                    if (n.Text.ToUpper().Contains(upper))
                    {
                        treeView1.SelectedNode = n;
                        treeView1.Focus();
                        break;
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------
        //  CONTEXT MENU
        //-------------------------------------------------------------------------------------------
        private void MenuItem_ExpandNode_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Nodes.Count > 0)
            {
                treeView1.SelectedNode.Expand();
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_CollapseNode_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Nodes.Count > 0)
            {
                treeView1.SelectedNode.Collapse();
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void MenuItem_SetFont_Click(object sender, EventArgs e)
        {
            if (fntDialog.ShowDialog() == DialogResult.OK)
            {
                treeView1.Font = fntDialog.Font;
            }
        }

        //-------------------------------------------------------------------------------------------
        //  TREE VIEW FUNCTIONS
        //-------------------------------------------------------------------------------------------
        private void TreeAdd(TreeNode parent, string name, uint value)
        {
            TreeNode node = new TreeNode(string.Format("u32 {0} = {1} (0x{1:X8})", name, value));
            parent.Nodes.Add(node);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void TreeAdd(TreeNode parent, string name, int value)
        {
            TreeNode node = new TreeNode(string.Format("s32 {0} = {1} (0x{1:X8})", name, value));
            parent.Nodes.Add(node);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void TreeAdd(TreeNode parent, string name, ushort value)
        {
            TreeNode node = new TreeNode(string.Format("u16 {0} = {1} (0x{1:X4})", name, value));
            parent.Nodes.Add(node);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void TreeAdd(TreeNode parent, string name, short value)
        {
            TreeNode node = new TreeNode(string.Format("s16 {0} = {1} (0x{1:X4})", name, value));
            parent.Nodes.Add(node);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void TreeAdd(TreeNode parent, string name, byte value)
        {
            TreeNode node = new TreeNode(string.Format("u8 {0} = {1} (0x{1:X2})", name, value));
            parent.Nodes.Add(node);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void TreeAdd(TreeNode parent, string name, sbyte value)
        {
            TreeNode node = new TreeNode(string.Format("s8 {0} = {1} (0x{1:X2})", name, value));
            parent.Nodes.Add(node);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void TreeAdd(TreeNode parent, string name, float value)
        {
            TreeNode node = new TreeNode(string.Format("float {0} = {1}", name, value));
            parent.Nodes.Add(node);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void TreeAdd(TreeNode parent, string name, bool value)
        {
            TreeNode node = new TreeNode(string.Format("bool {0} = {1}", name, value));
            parent.Nodes.Add(node);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void TreeAdd(TreeNode parent, string name, string value)
        {
            TreeNode node = new TreeNode(string.Format("string {0} = {1}", name, value));
            parent.Nodes.Add(node);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private TreeNode NextNode(TreeNode n, bool Returning)
        {
            if (n == null)
            {
                return null;
            }
            if (!Returning && n.FirstNode != null)
            {
                return n.FirstNode;
            }
            return n.NextNode ?? NextNode(n.Parent, true);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void TreeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
