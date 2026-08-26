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

            if (fileVersion == 18 || sampleData.IsV18)
            {
                short innerRadius = sampleData.InnerRadius;
                short outerRadius = sampleData.OuterRadius;
                if (parentForm.pnlSoundBankFiles.TryGetSoundDetailsRadius(sampleData.HashCodeNumber, out EuroSoundSfxRadiusData radiusData))
                {
                    innerRadius = radiusData.InnerRadius;
                    outerRadius = radiusData.OuterRadius;
                }

                propertyGrid1.Title = "SFX Parameters";
                propertyGrid1.propsGrid.SelectedObject = new SampleV18ForPropGrid
                {
                    Guid = "0x" + sampleData.HashCodeNumber.ToString("X8"),
                    ParameterAddress = "0x" + sampleData.V18ParameterAddress.ToString("X"),
                    PoolAddress = "0x" + sampleData.V18PoolAddress.ToString("X"),
                    ElementCount = sampleData.V18ElementCount,
                    RuntimeStatus = sampleData.V18RuntimeStatus,
                    InfoFlags = "0x" + sampleData.V18InfoFlags.ToString("X2"),
                    AttackTime = sampleData.V18AttackTime,
                    ReleaseTime = sampleData.V18ReleaseTime,
                    DuckerOffset = sampleData.V18DuckerOffset,
                    MixGroup = FormatObjectId(sampleData.V18MixGroup),
                    Ducker = FormatObjectId(sampleData.V18Ducker),
                    CullingGroup = FormatObjectId(sampleData.V18CullingGroup),
                    Oscillator = FormatObjectId(sampleData.V18Oscillator),
                    Controller = FormatObjectId(sampleData.V18Controller),
                    ReverbSend = sampleData.V18ReverbSend,
                    MultiTapSend = sampleData.V18MultiTapSend,
                    PingPongSend = sampleData.V18PingPongSend,
                    ChorusSend = sampleData.V18ChorusSend,
                    Doppler = sampleData.V18Doppler,
                    LowPassType = (byte)(sampleData.V18LowPass >> 6),
                    LowPassValue = (byte)(sampleData.V18LowPass & 0x3f),
                    VolumeRolloff = sampleData.V18VolumeRolloff,
                    MaxItems = sampleData.V18MaxItems,
                    Priority = sampleData.V18Priority,
                    MasterVolume = sampleData.V18MasterVolume,
                    PlayType = GetPlayTypeDescription((byte)(sampleData.V18PlayAndCull >> 3)),
                    CullingAction = GetCullingActionDescription((byte)(sampleData.V18PlayAndCull & 7)),
                    TriggerChance = sampleData.V18TriggerChance,
                    RawFlags = "0x" + sampleData.V18Flags.ToString("X8"),
                    InnerRadius = innerRadius,
                    OuterRadius = outerRadius
                };

                string[] v18Flags =
                {
                    "IsMusic", "DisableCentreSpeaker", "UnusedBit2", "PoolLoop", "UnusedBit4", "InstantPause", "ScaleDelayWithPitch", "UnusedBit7",
                    "DisableDistanceCull", "OneInstancePerFrame", "UnusedBit10", "UnusedBit11", "UseGlobalAuxSpeakers", "MorphAffectsPitch",
                    "GameVarCount bit 0", "GameVarCount bit 1", "GameVarCount bit 2", "GameVarCount bit 3", "GameVarCount bit 4", "GameVarCount bit 5", "GameVarCount bit 6", "GameVarCount bit 7",
                    "StreamCatchUp", "StreamRememberPosition", "TrackingFadeDistance", "TrackingFixedPan", "UnusedBit26", "TrackingRandomPosition", "TrackingUpdatePanAndDistance", "UnusedBit29", "UnusedBit30", "UnusedBit31"
                };
                checkedListBox1.Items.Clear();
                checkedListBox1.Items.AddRange(v18Flags);
                for (int i = 0; i < v18Flags.Length; i++) checkedListBox1.SetItemChecked(i, ((sampleData.V18Flags >> i) & 1) != 0);
                checkedListBox2.Items.Clear();
                return;
            }

            propertyGrid1.Title = "SFX Parameters";
            if (checkedListBox2.Items.Count == 0)
            {
                for (int i = 1; i <= 16; i++) checkedListBox2.Items.Add("UserFlags" + i);
            }

            //Clone Values
            SampleForPropGrid gridObj = new SampleForPropGrid
            {
                FileVersion = fileVersion,
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
                Flags = sampleData.Flags,
                PlayType = GetPlayTypeDescription(sampleData.PlayType),
                V10Flags = "0x" + sampleData.V10Flags.ToString("X8"),
                InstanceCulling = GetLegacyInstanceCullingDescription(sampleData),
                GroupCulling = GetLegacyGroupCullingDescription(sampleData)
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
            else if (fileVersion == 10)
            {
                checkedListBox1.Items.Clear();
                checkedListBox1.Items.AddRange(new string[]
                {
                    "MaxReject",
                    "UnPausable",
                    "IgnoreMasterVolume",
                    "Legacy MultiSample (superseded by PlayType)",
                    "Legacy RandomPick (superseded by PlayType)",
                    "Legacy Shuffled (superseded by PlayType)",
                    "Loop",
                    "Legacy Polyphonic (superseded by PlayType)",
                    "UnderWater",
                    "PauseInstant",
                    "HasSubSfx",
                    "StealOnLouder",
                    "TreatLikeMusic",
                    "KillMeOwnGroup",
                    "GroupStealReject",
                    "OneInstancePerFrame",
                    "Extended flag 16",
                    "Extended flag 17",
                    "Extended flag 18",
                    "Extended flag 19",
                    "Extended flag 20",
                    "Extended flag 21",
                    "Extended flag 22",
                    "Extended flag 23",
                    "Extended flag 24",
                    "Extended flag 25",
                    "Extended flag 26",
                    "Extended flag 27",
                    "Extended flag 28",
                    "Extended flag 29",
                    "Extended flag 30",
                    "Extended flag 31"
                });
            }
            else
            {
                if (fileVersion == 10)
                {
                    gridObj.TrackingType = "Stored in MUSX 10 flags (not the legacy TrackingType enum)";
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

            uint displayedFlags = fileVersion == 10 ? sampleData.V10Flags : sampleData.Flags;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, Convert.ToBoolean((displayedFlags >> i) & 1));
            }
            for (int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                checkedListBox2.SetItemChecked(i, Convert.ToBoolean((sampleData.UserFlags >> i) & 1));
            }
        }

        public void ClearData()
        {
            propertyGrid1.propsGrid.SelectedObject = null;
            propertyGrid1.Title = "SFX Parameters";
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

        private static string FormatObjectId(ushort id)
        {
            return id == 0 ? "(none)" : "0x" + id.ToString("X4") + " (" + id + ")";
        }

        private static string GetLegacyInstanceCullingDescription(Sample sample)
        {
            if (sample.MaxVoices <= 0)
            {
                return "None";
            }

            if ((sample.Flags & (1 << 0)) != 0)
            {
                return "Stop new";
            }

            if ((sample.Flags & (1 << 11)) != 0)
            {
                return "Steal quietest";
            }

            return "Stop oldest";
        }

        private static string GetLegacyGroupCullingDescription(Sample sample)
        {
            if (sample.GroupMaxChannels <= 0)
            {
                return "None";
            }

            if ((sample.Flags & (1 << 14)) != 0)
            {
                return "Stop new";
            }

            if ((sample.Flags & (1 << 13)) != 0)
            {
                return "Stop group";
            }

            return "Stop oldest";
        }

        private static string GetPlayTypeDescription(byte playType)
        {
            string[] descriptions =
            {
                "Single cycled",
                "Single random",
                "Single shuffled",
                "Sentence sequence",
                "Sentence shuffled",
                "Polyphonic",
                "Morph",
                "Morph radii",
                "Polyphonic wait"
            };

            return playType < descriptions.Length
                ? descriptions[playType] + " (" + playType + ")"
                : "Unknown (" + playType + ")";
        }

        private static string GetCullingActionDescription(byte cullingAction)
        {
            string[] descriptions =
            {
                "None",
                "Stop oldest",
                "Stop new",
                "Steal quietest",
                "Stop group",
                "Stop lowest priority",
                "Stop quietest"
            };

            return cullingAction < descriptions.Length
                ? descriptions[cullingAction] + " (" + cullingAction + ")"
                : "Unknown (" + cullingAction + ")";
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
