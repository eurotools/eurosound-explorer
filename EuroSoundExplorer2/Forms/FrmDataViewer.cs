using MusX;
using MusX.Objects;
using MusX.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace EuroSoundExplorer2
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FrmDataViewer : Form
    {
        private readonly SoundBankReader sfxFileReader = new SoundBankReader();
        private readonly int m_ErrorCount = 0;

        //-------------------------------------------------------------------------------------------------------------------------------
        public FrmDataViewer()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void FrmDataViewer_Load(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Read File Data 
                SfxHeaderData headerData = sfxFileReader.ReadSfxHeader(openFileDialog1.FileName, ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected.ToString());
                SortedDictionary<uint, Sample> SfxSamples = new SortedDictionary<uint, Sample>();
                List<SampleData> waveData = new List<SampleData>();
                sfxFileReader.ReadSoundBank(openFileDialog1.FileName, headerData, SfxSamples, waveData);

                Text = string.Format("Data Viewer - {0}", Path.GetFileName(openFileDialog1.FileName));
                labelPlatformValue.Text = string.Format("{0}", headerData.Platform);
                labelMemoryValue.Text = string.Format("{0} kb", headerData.FileSize / 1024);
                labelErrorsValue.Text = string.Format("{0}", m_ErrorCount);

                //Add data
                TreeNode treeNode1 = new TreeNode("SoundbankInfo");
                treeView1.Nodes.Add(treeNode1);

                //Print Soundbank Info
                TreeAdd(treeNode1, nameof(headerData.FileHashCode), headerData.FileHashCode);
                TreeAdd(treeNode1, nameof(headerData.FileVersion), headerData.FileVersion);
                TreeAdd(treeNode1, nameof(headerData.FileSize), headerData.FileSize);

                if (headerData.FileVersion > 3 && headerData.FileVersion < 10)
                {
                    TreeAdd(treeNode1, nameof(headerData.Timespan), headerData.Timespan);
                    TreeAdd(treeNode1, nameof(headerData.EurocomIma), headerData.EurocomIma);
                }

                TreeAdd(treeNode1, "SFXParametersStart", headerData.SFXStart);
                TreeAdd(treeNode1, "SFXParametersLength", headerData.SFXLenght);

                TreeAdd(treeNode1, nameof(headerData.SampleInfoStart), headerData.SampleInfoStart);
                TreeAdd(treeNode1, nameof(headerData.SampleInfoLenght), headerData.SampleInfoLenght);

                TreeAdd(treeNode1, nameof(headerData.SpecialSampleInfoStart), headerData.SpecialSampleInfoStart);
                TreeAdd(treeNode1, nameof(headerData.SpecialSampleInfoLength), headerData.SpecialSampleInfoLength);

                TreeAdd(treeNode1, nameof(headerData.SampleDataStart), headerData.SampleDataStart);
                TreeAdd(treeNode1, nameof(headerData.SampleDataLength), headerData.SampleDataLength);

                //Print SFX Parameters List
                TreeNode treeNode2 = new TreeNode("SFXParameters " + SfxSamples.Count);
                treeView1.Nodes.Add(treeNode2);

                foreach (KeyValuePair<uint, Sample> item in SfxSamples)
                {
                    TreeNode hashCode = new TreeNode(string.Format("u32 {0} = {1} (0x{1:X8})", "HashCode", item.Key));
                    treeNode2.Nodes.Add(hashCode);

                    TreeAdd(hashCode, nameof(item.Value.DuckerLenght), item.Value.DuckerLenght);
                    TreeAdd(hashCode, nameof(item.Value.MinDelay), item.Value.MinDelay);
                    TreeAdd(hashCode, nameof(item.Value.MaxDelay), item.Value.MaxDelay);
                    TreeAdd(hashCode, nameof(item.Value.InnerRadius), item.Value.InnerRadius);
                    TreeAdd(hashCode, nameof(item.Value.OuterRadius), item.Value.OuterRadius);
                    TreeAdd(hashCode, nameof(item.Value.ReverbSend), item.Value.ReverbSend);
                    TreeAdd(hashCode, nameof(item.Value.TrackingType), item.Value.TrackingType);
                    TreeAdd(hashCode, nameof(item.Value.MaxVoices), item.Value.MaxVoices);
                    TreeAdd(hashCode, nameof(item.Value.Priority), item.Value.Priority);
                    TreeAdd(hashCode, nameof(item.Value.Ducker), item.Value.Ducker);
                    TreeAdd(hashCode, nameof(item.Value.MasterVolume), item.Value.MasterVolume);

                    //Add Samples
                    TreeNode node = new TreeNode("SFXPoolElements " + item.Value.samplesList.Count);
                    hashCode.Nodes.Add(node);
                    foreach (SampleInfo sampleToPrint in item.Value.samplesList)
                    {
                        TreeNode fileRef = new TreeNode(string.Format("u32 {0} = {1} (0x{1:X8})", nameof(sampleToPrint.FileRef), sampleToPrint.FileRef));
                        node.Nodes.Add(fileRef);

                        TreeAdd(fileRef, nameof(sampleToPrint.FileRef), sampleToPrint.FileRef);
                        TreeAdd(fileRef, nameof(sampleToPrint.Pitch), sampleToPrint.Pitch);
                        TreeAdd(fileRef, nameof(sampleToPrint.PitchOffset), sampleToPrint.PitchOffset);
                        TreeAdd(fileRef, nameof(sampleToPrint.Volume), sampleToPrint.Volume);
                        TreeAdd(fileRef, nameof(sampleToPrint.VolumeOffset), sampleToPrint.VolumeOffset);
                        TreeAdd(fileRef, nameof(sampleToPrint.Pan), sampleToPrint.Pan);
                        TreeAdd(fileRef, nameof(sampleToPrint.PanOffset), sampleToPrint.PanOffset);
                    }
                }

                //Print SFX Waves List
                TreeNode treeNode3 = new TreeNode("SampleInfo " + waveData.Count);
                treeView1.Nodes.Add(treeNode3);

                for (int i = 0; i < waveData.Count; i++)
                {
                    TreeNode waveNode = new TreeNode(i.ToString());
                    treeNode3.Nodes.Add(waveNode);
                    TreeAdd(waveNode, "Flags", waveData[i].Flags);
                    TreeAdd(waveNode, "Address", waveData[i].Address);
                    TreeAdd(waveNode, "MemorySize", waveData[i].MemorySize);
                    TreeAdd(waveNode, "Frequency", waveData[i].Frequency);
                    TreeAdd(waveNode, "SampleSize", waveData[i].SampleSize);
                    TreeAdd(waveNode, "Channels", waveData[i].Channels);
                    TreeAdd(waveNode, "Bits", waveData[i].Bits);
                    TreeAdd(waveNode, "PsiSampleHeader", waveData[i].PsiSampleHeader);
                    TreeAdd(waveNode, "SampleLoopOffset", waveData[i].LoopStartOffset);
                    TreeAdd(waveNode, "Duration", waveData[i].Duration);
                }
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
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
