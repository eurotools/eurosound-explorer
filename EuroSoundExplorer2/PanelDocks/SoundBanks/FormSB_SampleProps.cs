using sb_explorer.Classes;
using MusX.Objects;
using System;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace sb_explorer
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FormSB_SampleProps : DockContent
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public FormSB_SampleProps()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowSampleData(Sample sampleData)
        {
            AppConfig MusXheaderData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration;

            //Clone Values
            SampleForPropGrid gridObj = new SampleForPropGrid
            {
                DuckerLenght = sampleData.DuckerLenght,
                MinDelay = sampleData.MinDelay,
                MaxDelay = sampleData.MaxDelay,
                ReverbSend = sampleData.ReverbSend,
                MaxVoices = sampleData.MaxVoices,
                Priority = sampleData.Priority,
                Ducker = sampleData.Ducker,
                MasterVolume = sampleData.MasterVolume,
                GroupHashCode = sampleData.GroupHashCode,
                GroupMaxChannels = sampleData.GroupMaxChannels,
                DopplerValue = sampleData.DopplerValue,
                UserValue = sampleData.UserValue,
                SFXDucker = sampleData.SFXDucker,
                Spare = sampleData.Spare,
                InnerRadius = sampleData.InnerRadius,
                OuterRadius = sampleData.OuterRadius,
                Flags = sampleData.Flags
            };
            if (MusXheaderData.FileVersion == 201 || MusXheaderData.FileVersion == 1)
            {
                switch (sampleData.TrackingType)
                {
                    case 0:
                        gridObj.TrackingType = "2D";
                        break;
                    case 1:
                        gridObj.TrackingType = "2D AMB";
                        break;
                    case 2:
                        gridObj.TrackingType = "3D";
                        break;
                    case 3:
                        gridObj.TrackingType = "3D RND POS";
                        break;
                    case 4:
                        gridObj.TrackingType = "2D PL2";
                        break;
                }
            }
            else
            {
                /*00 = 2D
                01 = 3D
                02 = 2D AMB
                03 = 3D AMB
                04 = 2D RND
                05 = 3D RND
                06 = 2D AMB RND
                07 = 3D AMB RND
                08 = 2D NT
                09 = 3D NT*/
                StringBuilder stringBuilder1 = new StringBuilder();
                if ((sbyte)(sampleData.TrackingType & 1) != 0)
                    stringBuilder1.Append("3D ");
                else
                    stringBuilder1.Append("2D ");
                if ((sbyte)(sampleData.TrackingType & 2) != 0)
                    stringBuilder1.Append("AMB ");
                if ((sbyte)(sampleData.TrackingType & 4) != 0)
                    stringBuilder1.Append("RND ");
                if ((sbyte)(sampleData.TrackingType & 8) != 0)
                    stringBuilder1.Append("NT ");
                gridObj.TrackingType = stringBuilder1.ToString().Trim();
            }

            //Display
            propertyGrid1.SelectedObject = gridObj;

            //Update Flags
            if (MusXheaderData.FileVersion == 201 || MusXheaderData.FileVersion == 1)
            {
                checkedListBox1.Items.Clear();
                checkedListBox1.Items.AddRange(new string[] { "MaxReject", "Doppler", "IgnoreAge", "MultiSample", "RandomPick", "Shuffled", "Loop", "Polyphonic", "UnderWater", "PauseInNis", "HasSubSfx", "StealOnLouder", "TreatLikeMusic", "UserFlags14", "UserFlags15", "UserFlags16" });
            }
            else
            {
                checkedListBox1.Items.Clear();
                checkedListBox1.Items.AddRange(new string[] { "MaxReject", "UnPausable", "IgnoreMasterVolume", "MultiSample", "RandomPick", "Shuffled", "Loop", "Polyphonic", "UnderWater", "PauseInstant", "HasSubSfx", "StealOnLouder", "TreatLikeMusic", "KillMeOwnGroup", "GroupStealReject", "OneInstancePerFrame" });
            }

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, Convert.ToBoolean((sampleData.Flags >> i) & 1));
            }

            for (int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                checkedListBox2.SetItemChecked(i, Convert.ToBoolean((sampleData.UserFlags >> i) & 1));
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
