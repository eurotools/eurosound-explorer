using MusX.Objects;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace sb_explorer
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FrmFileRefUsage : Form
    {
        public short fileRef;
        public SortedDictionary<uint, Sample> samplesDictionary;
        public Sample SampleCaller;

        //-------------------------------------------------------------------------------------------------------------------------------
        public FrmFileRefUsage(short fileRefToCheck, Sample sourceSample, SortedDictionary<uint, Sample> samplesDict)
        {
            InitializeComponent();
            fileRef = fileRefToCheck;
            SampleCaller = sourceSample;
            samplesDictionary = samplesDict;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void FrmFileRefUsage_Shown(object sender, EventArgs e)
        {
            FrmMain parentForm = ((FrmMain)Application.OpenForms[nameof(FrmMain)]);

            //If source is not null means that this call comes from the sample pool, need to check flags.
            if (SampleCaller != null)
            {
                foreach (KeyValuePair<uint, Sample> sampleData in samplesDictionary)
                {
                    //Check Sub SFX flag
                    if (((SampleCaller.Flags >> 10) & 1) == 0)
                    {
                        if (((sampleData.Value.Flags >> 10) & 1) == 0)
                        {
                            foreach (SampleInfo sampleInfo in sampleData.Value.samplesList)
                            {
                                if (sampleInfo.FileRef == fileRef)
                                {
                                    if (parentForm.hashTable.HashcodeIsListed(sampleData.Key))
                                    {
                                        listViewItemUsage.Items.Add(new ListViewItem(new string[] { fileRef.ToString(), parentForm.hashTable.GetHashCodeLabel(sampleData.Key) }) { ImageIndex = 0 });
                                    }
                                    else
                                    {
                                        listViewItemUsage.Items.Add(new ListViewItem(new string[] { fileRef.ToString(), string.Format("0x{0:X8}", sampleData.Key) }) { ImageIndex = 0 });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<uint, Sample> sampleData in samplesDictionary)
                {
                    if (((sampleData.Value.Flags >> 10) & 1) == 0)
                    {
                        //Check Sub SFX flag
                        foreach (SampleInfo sampleInfo in sampleData.Value.samplesList)
                        {
                            if (sampleInfo.FileRef == fileRef)
                            {
                                if (parentForm.hashTable.HashcodeIsListed(sampleData.Key))
                                {
                                    listViewItemUsage.Items.Add(new ListViewItem(new string[] { fileRef.ToString(), parentForm.hashTable.GetHashCodeLabel(sampleData.Key) }) { ImageIndex = 0 });
                                }
                                else
                                {
                                    listViewItemUsage.Items.Add(new ListViewItem(new string[] { fileRef.ToString(), string.Format("0x{0:X8}", sampleData.Key) }) { ImageIndex = 0 });
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
