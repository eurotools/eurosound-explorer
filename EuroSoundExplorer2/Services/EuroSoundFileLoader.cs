using MusX;
using MusX.Objects;
using MusX.Readers;
using System;
using System.Collections.Generic;
using System.IO;

namespace sb_explorer.Services
{
    public sealed class EuroSoundFileLoadOptions
    {
        public string FilePath { get; set; }
        public string ProjectFolder { get; set; }
        public string Platform { get; set; }
    }

    public sealed class EuroSoundFileLoadResult
    {
        public List<string> LoadedFilePaths { get; private set; }

        public EuroSoundFileLoadResult()
        {
            LoadedFilePaths = new List<string>();
        }
    }

    public static class EuroSoundFileLoader
    {
        public static EuroSoundFileLoadResult LoadSoundBank(EuroSoundFileLoadOptions options, LoadedProjectData data)
        {
            Validate(options, data);

            SoundBankReader reader = new SoundBankReader();
            data.SoundBankHeaderData = reader.ReadSfxHeader(options.FilePath, options.Platform);
            reader.ReadSoundBank(options.FilePath, data.SoundBankHeaderData, data.SfxSamples, data.SfxStoredData, data.DuplicatedHashCodes);

            return Loaded(options.FilePath);
        }

        public static EuroSoundFileLoadResult LoadStreamBank(EuroSoundFileLoadOptions options, LoadedProjectData data)
        {
            Validate(options, data);

            StreamBankReader streamReader = new StreamBankReader();
            StreambankHeader headerData = streamReader.ReadStreamBankHeader(options.FilePath, options.Platform);
            EuroSoundFileLoadResult result = new EuroSoundFileLoadResult();

            data.ActiveStreamBankIsCommon = IsCommonVersion6Stream(headerData);
            LoadStreamBank(options.FilePath, headerData, data, streamReader);
            result.LoadedFilePaths.Add(options.FilePath);

            TryLoadVersion6StreamCompanion(options, headerData, data, streamReader, result);
            return result;
        }

        public static EuroSoundFileLoadResult LoadMusicBank(EuroSoundFileLoadOptions options, LoadedProjectData data)
        {
            Validate(options, data);

            MusicBankReader reader = new MusicBankReader();
            data.MusicBankHeaderData = reader.ReadMusicHeader(options.FilePath, options.Platform);
            data.MusicData = reader.ReadMusicBank(options.FilePath, data.MusicBankHeaderData);

            return Loaded(options.FilePath);
        }

        public static EuroSoundFileLoadResult LoadSbi(EuroSoundFileLoadOptions options, LoadedProjectData data)
        {
            Validate(options, data);

            SbiBankReader reader = new SbiBankReader();
            data.SbiBankHeaderData = reader.ReadSoundbankInfoHeader(options.FilePath, options.Platform);
            data.SbiFileData = reader.ReadSoundbankInfoFile(options.FilePath, data.SbiBankHeaderData);

            return Loaded(options.FilePath);
        }

        public static EuroSoundFileLoadResult LoadProjectDetails(EuroSoundFileLoadOptions options, LoadedProjectData data)
        {
            Validate(options, data);

            ProjectDetailsReader reader = new ProjectDetailsReader();
            data.ProjectDetailsHeaderData = reader.ReadProjectFileHeader(options.FilePath, options.Platform);
            data.ProjectDetailsData = reader.ReadProjectFile(options.FilePath, data.ProjectDetailsHeaderData);

            return Loaded(options.FilePath);
        }

        public static EuroSoundFileLoadResult LoadSoundDetails(EuroSoundFileLoadOptions options, LoadedProjectData data)
        {
            Validate(options, data);

            SoundDetailsReader reader = new SoundDetailsReader();
            data.SoundDetailsHeaderData = reader.ReadCommonHeader(options.FilePath, options.Platform);
            data.SoundDetails = reader.ReadSoundDetailsFile(options.FilePath, data.SoundDetailsHeaderData);

            return Loaded(options.FilePath);
        }

        public static EuroSoundFileLoadResult LoadMusicDetails(EuroSoundFileLoadOptions options, LoadedProjectData data)
        {
            Validate(options, data);

            MusicDetailsReader reader = new MusicDetailsReader();
            data.MusicDetailsHeaderData = reader.ReadCommonHeader(options.FilePath, options.Platform);
            data.MusicDetails = reader.ReadMusicDetailsFile(options.FilePath, data.MusicDetailsHeaderData);

            return Loaded(options.FilePath);
        }

        public static EuroSoundFileLoadResult LoadMusicMarkers(EuroSoundFileLoadOptions options, LoadedProjectData data)
        {
            Validate(options, data);

            MusicMarkersReader reader = new MusicMarkersReader();
            data.MusicMarkersHeaderData = reader.ReadCommonHeader(options.FilePath, options.Platform);
            data.MusicMarkers = reader.ReadMusicMarkersFile(options.FilePath, data.MusicMarkersHeaderData);

            return Loaded(options.FilePath);
        }

        private static void LoadStreamBank(string filePath, StreambankHeader headerData, LoadedProjectData data, StreamBankReader streamReader)
        {
            if (IsCommonVersion6Stream(headerData))
            {
                data.CommonStreamBankHeaderData = headerData;
                data.CommonStreamSamples.Clear();
                streamReader.ReadStreamBank(filePath, data.CommonStreamBankHeaderData, data.CommonStreamSamples);
                return;
            }

            data.StreamBankHeaderData = headerData;
            data.StreamSamples.Clear();
            streamReader.ReadStreamBank(filePath, data.StreamBankHeaderData, data.StreamSamples);
        }

        private static void TryLoadVersion6StreamCompanion(
            EuroSoundFileLoadOptions options,
            StreambankHeader headerData,
            LoadedProjectData data,
            StreamBankReader streamReader,
            EuroSoundFileLoadResult result)
        {
            if (headerData.FileVersion != 6 ||
                (headerData.FileHashCode != 0x2D400000 && headerData.FileHashCode != 0x2D400001))
            {
                return;
            }

            uint companionHashCode = headerData.FileHashCode == 0x2D400001 ? 0x2D400000u : 0x2D400001u;
            if ((companionHashCode == 0x2D400000 && data.StreamSamples.Count > 0) ||
                (companionHashCode == 0x2D400001 && data.CommonStreamSamples.Count > 0))
            {
                return;
            }

            if (!Directory.Exists(options.ProjectFolder))
            {
                return;
            }

            SoundBankReader reader = new SoundBankReader();
            foreach (string candidatePath in Directory.GetFiles(options.ProjectFolder, "*.sfx", SearchOption.AllDirectories))
            {
                if (string.Equals(candidatePath, options.FilePath, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                SfxCommonHeader commonHeader = reader.ReadCommonHeader(candidatePath, options.Platform);
                if (commonHeader.FileVersion == 6 && commonHeader.FileHashCode == companionHashCode)
                {
                    StreambankHeader companionHeader = streamReader.ReadStreamBankHeader(candidatePath, options.Platform);
                    LoadStreamBank(candidatePath, companionHeader, data, streamReader);
                    result.LoadedFilePaths.Add(candidatePath);
                    return;
                }
            }
        }

        private static bool IsCommonVersion6Stream(StreambankHeader headerData)
        {
            return headerData.FileVersion == 6 && headerData.FileHashCode == 0x2D400001;
        }

        private static EuroSoundFileLoadResult Loaded(string filePath)
        {
            EuroSoundFileLoadResult result = new EuroSoundFileLoadResult();
            result.LoadedFilePaths.Add(filePath);
            return result;
        }

        private static void Validate(EuroSoundFileLoadOptions options, LoadedProjectData data)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
        }
    }
}
