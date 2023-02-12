using MusX;
using MusX.Objects;
using MusX.Readers;
using sb_explorer.Classes.PropertyGridHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static sb_explorer.Enumerations;

namespace sb_explorer
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class SoundhGenerator : Form
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------
        public class ExporterSettings
        {
            [DisplayName("SFX Files Folder")]
            [Category("Output Settings")]
            [Editor(typeof(CustomFolderBrowser), typeof(UITypeEditor))]
            public string FolderToCheck { get; set; }

            [DisplayName("Sound.h Output Directory")]
            [Category("Output Settings")]
            [Editor(typeof(CustomFolderBrowser), typeof(UITypeEditor))]
            public string SoundhOutputFolder { get; set; }

            [DisplayName("SFX Files Platform (Required for v201 and v1)")]
            [Category("Output Settings")]
            public Platform SfxPlatform { get; set; }

            [DisplayName("Game")]
            [Category("Output Settings")]
            public Title Game { get; set; }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------
        public class HcData
        {
            public string HashCodeLabel;
            public uint HashCodeNumber;
            public HashSet<string> HashCodeUsage = new HashSet<string>();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public ExporterSettings userSettings = new ExporterSettings();

        //-------------------------------------------------------------------------------------------------------------------------------
        public SoundhGenerator()
        {
            InitializeComponent();
            titlePropertyGrid1.propsGrid.SelectedObject = userSettings;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void BtnCancel_Click(object sender, System.EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
            Close();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void BtnStart_Click(object sender, System.EventArgs e)
        {
            if (!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync();
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Directory.Exists(userSettings.FolderToCheck))
            {
                SfxFunctions sfxReader = new SfxFunctions();
                Dictionary<uint, HcData> SFXs = new Dictionary<uint, HcData>();
                Dictionary<string, HcData> SoundBanks = new Dictionary<string, HcData>();
                Dictionary<string, HcData> MusicBanks = new Dictionary<string, HcData>();
                List<string> jumpHashCodes = new List<string>();

                //Get all files inside this folder
                string[] filesToInspect = Directory.GetFiles(userSettings.FolderToCheck, "*.sfx", SearchOption.AllDirectories);

                //Read the Music Details file to get the hashcode prefix
                uint musicHashCodesPrefix = 0;
                for (int i = 0; i < filesToInspect.Length; i++)
                {
                    if (filesToInspect[i].IndexOf("musicdetails.sfx", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        MusicDetailsReader msDetailsReader = new MusicDetailsReader();
                        var headerData = msDetailsReader.ReadCommonHeader(filesToInspect[i], userSettings.SfxPlatform.ToString());
                        MusicDetails musicFileData = msDetailsReader.ReadMusicDetailsFile(filesToInspect[i], headerData);
                        musicHashCodesPrefix = 0xFFF00000 & musicFileData.MinHashCode;
                        break;
                    }
                }

                //Inspect each file
                for (int i = 0; i < filesToInspect.Length; i++)
                {
                    if (backgroundWorker.CancellationPending)
                    {
                        break;
                    }
                    else
                    {
                        backgroundWorker.ReportProgress((int)decimal.Divide(i * 100, filesToInspect.Length));
                        SfxCommonHeader headerData = sfxReader.ReadCommonHeader(filesToInspect[i], userSettings.SfxPlatform.ToString());
                        if (headerData.Platform.IndexOf(userSettings.SfxPlatform.ToString()) >= 0)
                        {
                            SfxFunctions.FileType sfxFileType = GenericMethods.GetFileType((int)headerData.FileHashCode, headerData.FileVersion, filesToInspect[i], userSettings.Game);
                            switch (sfxFileType)
                            {
                                case SfxFunctions.FileType.MusicFile:
                                    GetMusicBanksSFXs(filesToInspect[i], headerData, MusicBanks, musicHashCodesPrefix, jumpHashCodes);
                                    break;
                                case SfxFunctions.FileType.SoundbankFile:
                                    GetSoundBanksSFXs(filesToInspect[i], headerData, SFXs, SoundBanks);
                                    break;
                                case SfxFunctions.FileType.StreamFile:
                                    break;
                            }
                        }
                    }
                }

                using (StreamWriter sw = new StreamWriter(File.Open(Path.Combine(userSettings.SoundhOutputFolder, "Sound.h"), FileMode.Create, FileAccess.Write)))
                {
                    sw.WriteLine("/* HT_Sound */");
                    sw.WriteLine("// SFX Misc defines");
                    sw.WriteLine(string.Empty);
                    sw.WriteLine("// SFX HashCodes");
                    foreach (KeyValuePair<uint, HcData> hashCode in SFXs)
                    {
                        sw.WriteLine(WriteHashCode(hashCode.Value.HashCodeLabel, (int)hashCode.Value.HashCodeNumber, hashCode.Value.HashCodeUsage));
                    }
                    sw.WriteLine(string.Empty);
                    sw.WriteLine("// SFX SoundBank HashCodes");
                    foreach (KeyValuePair<string, HcData> hashCode in SoundBanks)
                    {
                        sw.WriteLine(WriteHashCode(hashCode.Value.HashCodeLabel, (int)hashCode.Value.HashCodeNumber, null));
                    }
                    sw.WriteLine(string.Empty);
                    sw.WriteLine("// Music HashCodes");
                    foreach (KeyValuePair<string, HcData> hashCode in MusicBanks)
                    {
                        sw.WriteLine(WriteHashCode(hashCode.Value.HashCodeLabel, (int)hashCode.Value.HashCodeNumber, null));
                    }
                    sw.WriteLine("#define MFX_MaximumDefined\t\t{0}", MusicBanks.Count);
                    foreach (var jumpHashcode in jumpHashCodes)
                    {
                        sw.WriteLine(jumpHashcode);
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void BackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            prgCurrentProgress.Value = e.ProgressPercentage;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Output Cancelled by the user", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Sound.h created sucessfully!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Close();
        }

        //-------------------------------------------------------------------------------------------
        //  Binary Files Functions
        //-------------------------------------------------------------------------------------------
        private void GetSoundBanksSFXs(string filePath, SfxCommonHeader headerData, Dictionary<uint, HcData> SFXs, Dictionary<string, HcData> SoundBanks)
        {
            SoundBankReader sbReader = new SoundBankReader();
            SoundbankHeader sbHeaderData = sbReader.ReadSfxHeader(filePath, headerData.Platform);
            List<uint> DuplicatedHashCodes = new List<uint>();

            //Read SoundBank Data
            SortedDictionary<uint, Sample> samplesData = new SortedDictionary<uint, Sample>();
            List<SampleData> wavData = new List<SampleData>();
            sbReader.ReadSoundBank(filePath, sbHeaderData, samplesData, wavData, DuplicatedHashCodes);

            //Get Soundbank HashCode Label
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string musicLabel = fileName;
            if (fileName.IndexOf("_sb_", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                musicLabel = InitCapHashCodes(fileName.Substring(4));
            }

            //Get SFXs data
            string soundBankHashCode = "HT_Sound_" + musicLabel;
            if (!SoundBanks.ContainsKey(soundBankHashCode))
            {
                HcData soundBankData = new HcData
                {
                    HashCodeNumber = 0x00000FFF & sbHeaderData.FileHashCode,
                    HashCodeLabel = soundBankHashCode
                };
                SoundBanks.Add(soundBankHashCode, soundBankData);
            }

            //Get SFX hashcode
            foreach (KeyValuePair<uint, Sample> sample in samplesData)
            {
                if (SFXs.ContainsKey(sample.Key))
                {
                    SFXs[sample.Key].HashCodeUsage.Add(soundBankHashCode);
                }
                else
                {
                    HcData sfxData = new HcData
                    {
                        HashCodeNumber = sample.Key,
                        HashCodeLabel = string.Format("HT_Sound_SFX_Unknown_{0:X8}", sample.Key)
                    };
                    sfxData.HashCodeUsage.Add(soundBankHashCode);
                    SFXs.Add(sample.Key, sfxData);
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void GetMusicBanksSFXs(string filePath, SfxCommonHeader headerData, Dictionary<string, HcData> MusicBanks, uint prefix, List<string> jumpHashCodes)
        {
            MusicBankReader sbReader = new MusicBankReader();
            StreambankHeader sbHeaderData = sbReader.ReadMusicHeader(filePath, headerData.Platform);
            MusicSample musicData = sbReader.ReadMusicBank(filePath, sbHeaderData);

            //Get Soundbank HashCode Label
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string musicLabel = fileName;
            if (fileName.StartsWith("_mus", StringComparison.OrdinalIgnoreCase))
            {
                musicLabel = InitCapHashCodes(fileName.Substring(9));
            }

            //Get Markers data
            string soundBankHashCode = "HT_Sound_MFX_" + musicLabel;
            if (!MusicBanks.ContainsKey(soundBankHashCode))
            {
                uint musicHashCode = 0x00000FFF & sbHeaderData.FileHashCode;
                HcData soundBankData = new HcData
                {
                    HashCodeNumber = prefix | musicHashCode,
                    HashCodeLabel = soundBankHashCode
                };
                MusicBanks.Add(soundBankHashCode, soundBankData);

                //Get markers
                jumpHashCodes.Add(string.Empty);
                jumpHashCodes.Add(string.Format("// Music Jump Codes For Level MFX_{0}", musicLabel));
                bool nextIsLoopEnd = false;
                bool addEndMarker = true;
                for (int i = 0, index = 0; i < musicData.Markers.Length; i++)
                {
                    Marker currentMarker = musicData.Markers[i];
                    string marker = "HT_Sound_JMP_" + musicLabel;

                    if (currentMarker.Type == 9 && !addEndMarker)
                    {
                        continue;
                    }
                    else
                    {
                        //Skip start marker from the loop start marker
                        if (i + 1 < musicData.Markers.Length && currentMarker.Index == musicData.Markers[i + 1].Index)
                        {
                            if (currentMarker.Type != 7 && musicData.Markers[i + 1].Type != 7)
                            {
                                continue;
                            }
                        }

                        //Get hashcode
                        uint hashPrefix = (0xFFF00000 & prefix) >> 20;
                        long hashCode = ((hashPrefix & 0xFFF) << 20) | (((short)index & 0xFF) << 12) | ((musicHashCode & 0xFF) << 0);

                        //Get marker type string
                        switch (currentMarker.Type)
                        {
                            case 10:
                                if (nextIsLoopEnd)
                                {
                                    marker += "_Loopend";
                                    nextIsLoopEnd = false;
                                }
                                else
                                {
                                    marker += "_Start";
                                }
                                break;
                            case 9:
                                marker += "_End";
                                break;
                            case 7:
                                marker += "_Goto";
                                addEndMarker = false;
                                break;
                            case 6:
                                marker += "_Loop";
                                nextIsLoopEnd = true;
                                break;
                        }

                        //Add hashcode to the list
                        jumpHashCodes.Add(string.Format("#define {0} 0x{1:X8}", marker, hashCode));
                        index++;
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------
        //  String Functions
        //-------------------------------------------------------------------------------------------
        private string WriteHashCode(string hashCodeLabel, int hashCodeNumber, HashSet<string> usage)
        {
            string stringLength = GetColumnSize(hashCodeLabel.Length, 4);
            string formattedString;
            if (usage == null)
            {
                formattedString = string.Format("#define {0}{1}0x{2,8}", hashCodeLabel, stringLength, hashCodeNumber.ToString("X8"));
            }
            else
            {
                formattedString = string.Format("#define {0}{1}0x{2,8}\t// {3}", hashCodeLabel, stringLength, hashCodeNumber.ToString("X8"), string.Join(", ", usage.ToArray()));
            }

            return formattedString;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private string GetColumnSize(int stringLength, int blocksSize = 14)
        {
            int numTabs = (stringLength + 1) / blocksSize / 5;
            string tabs = new string('\t', numTabs + 1);

            return tabs;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private string InitCapHashCodes(string hashcodeLabel)
        {
            var data = hashcodeLabel.Split('_');
            for (int i = 0; i < data.Length; i++)
            {
                if (i == 0)
                {
                    data[i] = data[i].ToUpper();
                }
                else
                {
                    data[i] = char.ToUpper(data[i][0]) + data[i].Substring(1);
                }
            }

            return string.Join("_", data);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
