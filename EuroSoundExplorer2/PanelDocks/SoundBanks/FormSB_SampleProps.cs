using MusX.Objects;
using sb_explorer.Classes;
using sb_explorer.Services;
using System;
using System.Collections.Generic;
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
            FrmMain parentForm = (FrmMain)Application.OpenForms[nameof(FrmMain)];
            AppConfig MusXheaderData = parentForm.Configuration;
            int fileVersion = parentForm.pnlSoundBankFiles.SoundBankHeaderData.FileVersion != 0 ? parentForm.pnlSoundBankFiles.SoundBankHeaderData.FileVersion : MusXheaderData.FileVersion;

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
            if (fileVersion >= 4 && fileVersion <= 6)
            {
                if (parentForm.pnlSoundBankFiles.TryGetSoundDetailsRadius(sampleData.HashCodeNumber, out EuroSoundSfxRadiusData radiusData))
                {
                    gridObj.InnerRadius = radiusData.InnerRadius;
                    gridObj.OuterRadius = radiusData.OuterRadius;
                }
            }

            if (fileVersion >= 5 && fileVersion <= 6)
            {
                gridObj.TrackingType = GetTrackingTypeDescription(sampleData.TrackingType);
            }
            else
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
                    default:
                        gridObj.TrackingType = sampleData.TrackingType.ToString();
                        break;
                }
            }

            //Display
            propertyGrid1.propsGrid.SelectedObject = gridObj;

            //Update Flags
            if (fileVersion == 201 || fileVersion == 1)
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

        //-------------------------------------------------------------------------------------------------------------------------------
        private static string GetTrackingTypeDescription(byte trackingType)
        {
            List<string> parts = new List<string>
            {
                (trackingType & 0x01) != 0 ? "3D" : "2D"
            };

            if ((trackingType & 0x02) != 0)
            {
                parts.Add("AMB");
            }

            if ((trackingType & 0x04) != 0)
            {
                parts.Add("RND");
            }

            if ((trackingType & 0x08) != 0)
            {
                parts.Add("NT");
            }

            return string.Join(" ", parts);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
