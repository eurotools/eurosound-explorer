using MusX;
using MusX.Objects;
using MusX.Readers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using static MusX.Readers.SfxFunctions;
using static sb_explorer.Enumerations;

namespace sb_explorer.Services
{
    public sealed class EuroSoundProjectSoundBankLoadOptions
    {
        public string ProjectFolder { get; set; }
        public string Platform { get; set; }
        public Title SelectedTitle { get; set; }
    }

    public sealed class EuroSoundProjectSoundBank
    {
        public string FilePath { get; set; }
        public SoundbankHeader Header { get; set; }
        public SortedDictionary<uint, Sample> Samples { get; private set; }
        public List<SampleData> StoredData { get; private set; }
        public List<uint> DuplicatedHashCodes { get; private set; }

        public EuroSoundProjectSoundBank()
        {
            Samples = new SortedDictionary<uint, Sample>();
            StoredData = new List<SampleData>();
            DuplicatedHashCodes = new List<uint>();
        }
    }

    public sealed class EuroSoundProjectSoundBankLoadResult
    {
        public List<EuroSoundProjectSoundBank> SoundBanks { get; private set; }
        public List<string> FailedFiles { get; private set; }
        public bool Cancelled { get; set; }

        public int SoundbankCount { get { return SoundBanks.Count; } }
        public int FailedCount { get { return FailedFiles.Count; } }

        public EuroSoundProjectSoundBankLoadResult()
        {
            SoundBanks = new List<EuroSoundProjectSoundBank>();
            FailedFiles = new List<string>();
        }
    }

    public static class EuroSoundProjectSoundBankLoader
    {
        public static string[] GetProjectSfxFiles(string projectFolder)
        {
            if (string.IsNullOrWhiteSpace(projectFolder) || !Directory.Exists(projectFolder))
            {
                return new string[0];
            }

            return Directory.GetFiles(projectFolder, "*.sfx", SearchOption.AllDirectories);
        }

        public static EuroSoundProjectSoundBankLoadResult LoadSoundBanks(
            EuroSoundProjectSoundBankLoadOptions options,
            BackgroundWorker worker,
            int maxProgress,
            string progressAction)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            EuroSoundProjectSoundBankLoadResult result = new EuroSoundProjectSoundBankLoadResult();
            SoundBankReader soundBankReader = new SoundBankReader();
            string[] files = GetProjectSfxFiles(options.ProjectFolder);

            for (int i = 0; i < files.Length; i++)
            {
                if (worker != null && worker.CancellationPending)
                {
                    result.Cancelled = true;
                    return result;
                }

                string filePath = files[i];
                int progress = files.Length == 0 ? maxProgress : (int)(((i + 1) * (double)maxProgress) / files.Length);
                ReportProgress(worker, progress, progressAction + " " + Path.GetFileName(filePath));

                try
                {
                    SfxCommonHeader commonHeader = soundBankReader.ReadCommonHeader(filePath, options.Platform);
                    FileType fileType = GenericMethods.GetFileType((int)commonHeader.FileHashCode, commonHeader.FileVersion, filePath, options.SelectedTitle);
                    if (fileType != FileType.SoundbankFile)
                    {
                        continue;
                    }

                    EuroSoundProjectSoundBank soundBank = new EuroSoundProjectSoundBank
                    {
                        FilePath = filePath,
                        Header = soundBankReader.ReadSfxHeader(filePath, options.Platform)
                    };

                    soundBankReader.ReadSoundBank(filePath, soundBank.Header, soundBank.Samples, soundBank.StoredData, soundBank.DuplicatedHashCodes);
                    result.SoundBanks.Add(soundBank);
                }
                catch (Exception ex)
                {
                    result.FailedFiles.Add(Path.GetFileName(filePath) + ": " + ex.Message);
                }
            }

            return result;
        }

        private static void ReportProgress(BackgroundWorker worker, int progress, string message)
        {
            if (worker != null)
            {
                worker.ReportProgress(progress, message);
            }
        }
    }
}
