﻿using MusX.Readers;
using System;
using System.IO;
using System.Windows.Forms;
using static EuroSoundExplorer2.AppConfig;
using static MusX.Readers.SfxFunctions;

namespace EuroSoundExplorer2
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FrmDataViewer : Form
    {
        private readonly SoundBankReader sbReader = new SoundBankReader();
        private readonly StreamBankReader strReader = new StreamBankReader();
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

            //Update form title
            Text = string.Format("Data Viewer - {0}", Path.GetFileName(sfxFilePath));

            //Get selected info from the mainform
            int selectedVersion = parentForm.configuration.FileVersion;
            Title selectedTitle = parentForm.configuration.TitleSelected;

            //Get version of MusX Files
            int hashCode = sbReader.GetFileHashCode(sfxFilePath);
            FileType fileType = GenericMethods.GetFileType(hashCode, selectedVersion, sfxFilePath, selectedTitle);
            switch (fileType)
            {
                case FileType.SoundBank:
                    ShowSoundBank(sfxFilePath);
                    break;
                case FileType.Stream:
                    ShowStreamBank(sfxFilePath);
                    break;
                case FileType.Music:
                    //LoadSelectedMusic(filePath);
                    //lvwFiles.SelectedItems[0].SubItems[3].Text = "Loaded";
                    break;
                case FileType.SBI:
                    //LoadSelectedSbi(filePath);
                    //lvwFiles.SelectedItems[0].SubItems[3].Text = "Loaded";
                    break;
                case FileType.ProjectDetails:
                    //LoadSelectedProjectDetails(filePath);
                    //lvwFiles.SelectedItems[0].SubItems[3].Text = "Loaded";
                    break;
            }

            //Expand TreeView
            if (treeView1.Nodes.Count > 0)
            {
                treeView1.Nodes[0].Expand();
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

        private void TreeAdd(TreeNode parent, string name, bool value)
        {
            TreeNode node = new TreeNode(string.Format("bool {0} = {1}", name, value));
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
        private void MenuItem_SetFont_Click(object sender, EventArgs e)
        {
            if (fntDialog.ShowDialog() == DialogResult.OK)
            {
                treeView1.Font = fntDialog.Font;
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
