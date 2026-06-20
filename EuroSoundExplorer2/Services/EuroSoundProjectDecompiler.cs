using MusX;
using MusX.Objects;
using MusX.Readers;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using sb_explorer.Classes;
using sb_explorer.Services.Audio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using static MusX.Readers.SfxFunctions;
using static sb_explorer.Enumerations;

namespace sb_explorer.Services
{
    public enum EuroSoundProjectDecompilerMode
    {
        Create,
        ReplaceSections
    }

    public sealed class EuroSoundProjectDecompilerOptions
    {
        public string CompiledFolder { get; set; }
        public string EuroSoundProjectFolder { get; set; }
        public string OutputFolder { get; set; }
        public EuroSoundProjectDecompilerMode Mode { get; set; }
        public Title SelectedTitle { get; set; }
        public HashcodeParser Hashcodes { get; set; }
        public bool ExportSoundBanks { get; set; }
        public bool ExportGroups { get; set; }
        public bool ExportDuckerGroups { get; set; }
        public bool ExportSfx { get; set; }
        public bool ExportMemoryMaps { get; set; }
        public bool RewriteSamplesOnly { get; set; }
        public bool ExportPlatformSpecificSamplePoolModes { get; set; }
        public bool ReplaceSfxParameters { get; set; }
        public bool ReplaceSfxSamplePoolFiles { get; set; }
        public bool ReplaceSfxSamplePoolModes { get; set; }
        public bool ReplaceSfxSamplePoolControl { get; set; }
        public bool ReplaceGroupDependencies { get; set; }
        public bool ReplaceGroupParameters { get; set; }
        public bool ReplaceDuckerDependencies { get; set; }
        public bool ReplaceDuckerParameters { get; set; }
        public bool ReplaceSoundBankDependencies { get; set; }
    }

    public sealed class EuroSoundProjectDecompilerResult
    {
        public int SoundbankFilesRead { get; set; }
        public int SoundbanksWritten { get; set; }
        public int GroupsWritten { get; set; }
        public int DuckerGroupsWritten { get; set; }
        public int SfxWritten { get; set; }
        public int SamplesWritten { get; set; }
        public int StreambanksRead { get; set; }
        public int MusicbanksRead { get; set; }
        public int MusicsWritten { get; set; }
        public int MemoryMapsWritten { get; set; }
        public int FailedFiles { get; set; }
        public bool Cancelled { get; set; }
        public string ReportPath { get; set; }
        public List<string> Warnings { get; private set; }
        public List<string> FailedFileMessages { get; private set; }

        public EuroSoundProjectDecompilerResult()
        {
            Warnings = new List<string>();
            FailedFileMessages = new List<string>();
        }
    }

    public static class EuroSoundProjectDecompiler
    {
        private const int MasterSampleRate = 44100;
        private const string DependenciesSection = "#DEPENDENCIES";
        private const string SfxParametersSection = "#SFXParameters";
        private const string HashCodeSection = "#HASHCODE";
        private const string SectionEnd = "#END";
        private const string SfxMissingName = "**HashCode Not Found**";
        private static readonly LanguageName[] LanguageNames =
        {
            new LanguageName("English", "_Eng"),
            new LanguageName("American", "_Usa"),
            new LanguageName("Japanese", "_Jap"),
            new LanguageName("Danish", "_Dan"),
            new LanguageName("Dutch", "_Dut"),
            new LanguageName("Finnish", "_Fin"),
            new LanguageName("French", "_Fre"),
            new LanguageName("German", "_Ger"),
            new LanguageName("Italian", "_Ita"),
            new LanguageName("Norwegian", "_Nor"),
            new LanguageName("Portuguese", "_Por"),
            new LanguageName("Spanish", "_Spa"),
            new LanguageName("Swedish", "_Swe")
        };

        public static EuroSoundProjectDecompilerResult Decompile(EuroSoundProjectDecompilerOptions options, BackgroundWorker worker)
        {
            ValidateOptions(options);

            EuroSoundProjectDecompilerResult result = new EuroSoundProjectDecompilerResult();
            ProjectFolders folders = ResolveFolders(options);
            ProjectReference reference = ProjectReference.Load(options.Mode == EuroSoundProjectDecompilerMode.ReplaceSections ? options.EuroSoundProjectFolder : folders.RootFolder);
            List<SoundBankReadResult> soundBanks = ReadSoundBanks(options, worker, result);
            List<StreamBankReadResult> streamBanks = options.RewriteSamplesOnly
                ? new List<StreamBankReadResult>()
                : ReadStreamBanks(options, result);
            List<MusicBankReadResult> musicBanks = options.RewriteSamplesOnly
                ? new List<MusicBankReadResult>()
                : ReadMusicBanks(options, result);
            List<ProjectDetailsReadResult> projectDetails = options.ExportMemoryMaps && !options.RewriteSamplesOnly
                ? ReadProjectDetails(options, result)
                : new List<ProjectDetailsReadResult>();
            ProjectSampleCatalog sampleCatalog = new ProjectSampleCatalog();

            if (result.Cancelled)
            {
                return result;
            }

            List<SoundBankReadResult> baseSoundBanks = soundBanks
                .GroupBy(item => GetSoundBankGroupKey(item))
                .Select(group => group.OrderBy(item => PlatformRank(item.Header.Platform)).First())
                .OrderBy(item => GetSoundBankName(item, options.Hashcodes, reference))
                .ToList();

            Dictionary<uint, EuroSoundSfxRadiusData> soundDetailsRadii = (options.ExportSfx || options.ExportDuckerGroups)
                ? BuildSoundDetailsRadiusLookup(options)
                : new Dictionary<uint, EuroSoundSfxRadiusData>();

            if (options.RewriteSamplesOnly)
            {
                worker.ReportProgress(92, "Rebuilding Samples.txt...");
                sampleCatalog.AddExistingMasterFiles(folders.MasterFolder);
                PopulateSampleCatalog(soundBanks, streamBanks, folders, reference, options, sampleCatalog, false);
                result.SamplesWritten = WriteSystemFiles(folders, soundBanks, sampleCatalog, new List<ProjectDetailsReadResult>(), result);
                WriteDiagnosticReport(options, folders, soundBanks, streamBanks, musicBanks, result);
                worker.ReportProgress(100, "Done");
                return result;
            }

            if (options.ExportSfx)
            {
                worker.ReportProgress(92, "Writing SFX files...");
                result.SfxWritten = WriteSfxFiles(options.ExportPlatformSpecificSamplePoolModes ? soundBanks : baseSoundBanks, soundBanks, streamBanks, folders, reference, options, soundDetailsRadii, sampleCatalog);
            }

            List<RecoveredGroup> recoveredGroups = RecoverGroups(baseSoundBanks, options.Hashcodes, reference, result);
            if (options.ExportGroups)
            {
                worker.ReportProgress(95, "Writing group files...");
                result.GroupsWritten = WriteGroupFiles(recoveredGroups, folders, reference, options);
            }

            List<RecoveredDuckerGroup> recoveredDuckerGroups = RecoverDuckerGroups(baseSoundBanks, options.Hashcodes, reference, result, soundDetailsRadii);
            if (options.ExportDuckerGroups)
            {
                worker.ReportProgress(96, "Writing Ducker group files...");
                result.DuckerGroupsWritten = WriteDuckerGroupFiles(recoveredDuckerGroups, folders, reference, options);
            }

            worker.ReportProgress(97, "Writing music WAV files...");
            result.MusicsWritten = WriteMusicFiles(musicBanks, folders, reference, options);

            if (options.ExportSoundBanks)
            {
                worker.ReportProgress(98, "Writing SoundBank files...");
                result.SoundbanksWritten = WriteSoundBankFiles(baseSoundBanks, recoveredGroups, folders, reference, options);
            }

            worker.ReportProgress(99, "Writing system files...");
            result.SamplesWritten = WriteSystemFiles(folders, soundBanks, sampleCatalog, projectDetails, result);

            WriteDiagnosticReport(options, folders, soundBanks, streamBanks, musicBanks, result);
            worker.ReportProgress(100, "Done");
            return result;
        }

        private static void ValidateOptions(EuroSoundProjectDecompilerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            if (string.IsNullOrWhiteSpace(options.CompiledFolder) || !Directory.Exists(options.CompiledFolder))
            {
                throw new DirectoryNotFoundException("The compiled files folder does not exist.");
            }

            if (options.Hashcodes == null)
            {
                throw new ArgumentNullException("Hashcodes");
            }

            if (options.Mode == EuroSoundProjectDecompilerMode.ReplaceSections)
            {
                if (string.IsNullOrWhiteSpace(options.EuroSoundProjectFolder) || !Directory.Exists(options.EuroSoundProjectFolder))
                {
                    throw new DirectoryNotFoundException("The EuroSound project folder is required in replace mode.");
                }
            }
            else if (string.IsNullOrWhiteSpace(options.OutputFolder))
            {
                throw new DirectoryNotFoundException("The output folder is required in create mode.");
            }
        }

        private static ProjectFolders ResolveFolders(EuroSoundProjectDecompilerOptions options)
        {
            string rootFolder = options.Mode == EuroSoundProjectDecompilerMode.ReplaceSections
                ? options.EuroSoundProjectFolder
                : options.OutputFolder;

            EnsureDirectoryExists(rootFolder);

            return new ProjectFolders
            {
                RootFolder = rootFolder,
                SfxFolder = GetOrCreateSubfolder(rootFolder, "SFXs"),
                GroupsFolder = GetOrCreateSubfolder(rootFolder, "Groups"),
                DuckersFolder = GetOrCreateSubfolder(rootFolder, "Duckers"),
                SoundBanksFolder = GetOrCreateSubfolder(rootFolder, "SoundBanks"),
                MusicsFolder = GetOrCreateSubfolder(rootFolder, "Musics"),
                SystemFolder = GetOrCreateSubfolder(rootFolder, "System"),
                MasterFolder = GetOrCreateSubfolder(rootFolder, "Master")
            };
        }

        private static List<SoundBankReadResult> ReadSoundBanks(EuroSoundProjectDecompilerOptions options, BackgroundWorker worker, EuroSoundProjectDecompilerResult result)
        {
            SoundBankReader reader = new SoundBankReader();
            List<CompiledFileCandidate> files = FindCompiledFileCandidates(options.CompiledFolder, options.SelectedTitle)
                .Where(file => file.FileType == FileType.SoundbankFile && IsSupportedVersion(file.CommonHeader.FileVersion))
                .ToList();
            List<SoundBankReadResult> soundBanks = new List<SoundBankReadResult>();
            if (files.Count == 0)
            {
                result.Warnings.Add("No supported SoundBank files were found under the selected compiled folder.");
            }

            for (int i = 0; i < files.Count; i++)
            {
                if (worker.CancellationPending)
                {
                    result.Cancelled = true;
                    return soundBanks;
                }

                string filePath = files[i].FilePath;
                int progress = files.Count == 0 ? 90 : (int)(((i + 1) * 90.0) / files.Count);
                worker.ReportProgress(progress, "Reading " + Path.GetFileName(filePath));

                try
                {
                    SoundbankHeader header = reader.ReadSfxHeader(filePath, files[i].CommonHeader.Platform);
                    SortedDictionary<uint, Sample> samples = new SortedDictionary<uint, Sample>();
                    List<SampleData> storedData = new List<SampleData>();
                    List<uint> duplicates = new List<uint>();
                    reader.ReadSoundBank(filePath, header, samples, storedData, duplicates);

                    soundBanks.Add(new SoundBankReadResult
                    {
                        FilePath = filePath,
                        Header = header,
                        Samples = samples,
                        StoredData = storedData,
                        LanguageName = InferLanguageNameFromPath(filePath)
                    });
                    result.SoundbankFilesRead++;
                }
                catch (Exception ex)
                {
                    result.FailedFiles++;
                    result.FailedFileMessages.Add(Path.GetFileName(filePath) + ": " + ex.Message);
                }
            }

            if (files.Count > 0 && soundBanks.Count == 0)
            {
                result.Warnings.Add("Supported SoundBank files were found, but none could be read.");
            }

            return soundBanks;
        }

        private static List<StreamBankReadResult> ReadStreamBanks(EuroSoundProjectDecompilerOptions options, EuroSoundProjectDecompilerResult result)
        {
            StreamBankReader reader = new StreamBankReader();
            List<CompiledFileCandidate> files = FindCompiledFileCandidates(options.CompiledFolder, options.SelectedTitle)
                .Where(file => file.FileType == FileType.StreamFile && IsSupportedVersion(file.CommonHeader.FileVersion))
                .ToList();
            List<StreamBankReadResult> streamBanks = new List<StreamBankReadResult>();

            foreach (CompiledFileCandidate file in files)
            {
                try
                {
                    StreambankHeader header = reader.ReadStreamBankHeader(file.FilePath, file.Platform);
                    List<StreamSample> streams = new List<StreamSample>();
                    reader.ReadStreamBank(file.FilePath, header, streams);
                    bool isCommon = (file.CommonHeader.FileHashCode & 0x0000FFFF) == 1 ||
                        Path.GetFileNameWithoutExtension(file.FilePath).IndexOf("common", StringComparison.OrdinalIgnoreCase) >= 0;
                    streamBanks.Add(new StreamBankReadResult
                    {
                        FilePath = file.FilePath,
                        Header = header,
                        Streams = streams,
                        IsCommon = isCommon,
                        LanguageName = isCommon ? string.Empty : InferLanguageNameFromPath(file.FilePath)
                    });
                    result.StreambanksRead++;
                }
                catch (Exception ex)
                {
                    result.FailedFiles++;
                    result.FailedFileMessages.Add(Path.GetFileName(file.FilePath) + ": " + ex.Message);
                }
            }

            return streamBanks;
        }

        private static List<MusicBankReadResult> ReadMusicBanks(EuroSoundProjectDecompilerOptions options, EuroSoundProjectDecompilerResult result)
        {
            MusicBankReader reader = new MusicBankReader();
            List<CompiledFileCandidate> files = FindCompiledFileCandidates(options.CompiledFolder, options.SelectedTitle)
                .Where(file => file.FileType == FileType.MusicFile && IsSupportedVersion(file.CommonHeader.FileVersion))
                .ToList();
            List<MusicBankReadResult> musicBanks = new List<MusicBankReadResult>();

            foreach (CompiledFileCandidate file in files)
            {
                try
                {
                    StreambankHeader header = reader.ReadMusicHeader(file.FilePath, file.Platform);
                    MusicSample music = reader.ReadMusicBank(file.FilePath, header);
                    if (music == null)
                    {
                        continue;
                    }

                    musicBanks.Add(new MusicBankReadResult
                    {
                        FilePath = file.FilePath,
                        Header = header,
                        Music = music,
                        HashCode = file.CommonHeader.FileHashCode
                    });
                    result.MusicbanksRead++;
                }
                catch (Exception ex)
                {
                    result.FailedFiles++;
                    result.FailedFileMessages.Add(Path.GetFileName(file.FilePath) + ": " + ex.Message);
                }
            }

            return musicBanks;
        }

        private static List<ProjectDetailsReadResult> ReadProjectDetails(EuroSoundProjectDecompilerOptions options, EuroSoundProjectDecompilerResult result)
        {
            ProjectDetailsReader reader = new ProjectDetailsReader();
            List<CompiledFileCandidate> files = FindCompiledFileCandidates(options.CompiledFolder, options.SelectedTitle)
                .Where(file => file.FileType == FileType.ProjectDetails && IsSupportedVersion(file.CommonHeader.FileVersion))
                .ToList();
            List<ProjectDetailsReadResult> projectDetails = new List<ProjectDetailsReadResult>();

            foreach (CompiledFileCandidate file in files)
            {
                try
                {
                    ProjectDetailsHeader header = reader.ReadProjectFileHeader(file.FilePath, file.Platform);
                    ProjectDetails details = reader.ReadProjectFile(file.FilePath, header);
                    projectDetails.Add(new ProjectDetailsReadResult
                    {
                        FilePath = file.FilePath,
                        Header = header,
                        Details = details
                    });
                }
                catch (Exception ex)
                {
                    result.FailedFiles++;
                    result.FailedFileMessages.Add(Path.GetFileName(file.FilePath) + ": " + ex.Message);
                }
            }

            if (files.Count > 0 && projectDetails.Count == 0)
            {
                result.Warnings.Add("ProjectDetails files were found, but none could be read.");
            }
            else if (files.Count == 0)
            {
                result.Warnings.Add("No ProjectDetails file was found for Memory Maps.");
            }

            return projectDetails;
        }

        private static int WriteSfxFiles(List<SoundBankReadResult> soundBanks, List<SoundBankReadResult> allSoundBanks, List<StreamBankReadResult> streamBanks, ProjectFolders folders, ProjectReference reference, EuroSoundProjectDecompilerOptions options, IDictionary<uint, EuroSoundSfxRadiusData> soundDetailsRadii, ProjectSampleCatalog sampleCatalog)
        {
            int written = 0;
            HashSet<string> exported = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            List<EuroSoundSfxTextSection> sections = GetSfxSections(options);
            SamplePoolFileNameResolver sampleNameResolver = new SamplePoolFileNameResolver(allSoundBanks, streamBanks, folders.MasterFolder, reference, options.Hashcodes, sampleCatalog);

            foreach (SoundBankReadResult soundBank in soundBanks)
            {
                if (sections.Count == 0)
                {
                    break;
                }

                foreach (Sample sample in soundBank.Samples.Values)
                {
                    string exportKey = options.ExportPlatformSpecificSamplePoolModes
                        ? sample.HashCodeNumber.ToString("X8", CultureInfo.InvariantCulture) + "|" + GetPlatformSuffix(soundBank.Header.Platform)
                        : sample.HashCodeNumber.ToString("X8", CultureInfo.InvariantCulture);
                    if (!exported.Add(exportKey))
                    {
                        continue;
                    }

                    string filePath = GetSfxFilePath(sample.HashCodeNumber, folders.SfxFolder, reference, options.Hashcodes, options.ExportPlatformSpecificSamplePoolModes ? soundBank.Header.Platform : null);
                    string sfxName = Path.GetFileNameWithoutExtension(filePath);
                    List<string> samplePoolFileNames = sampleNameResolver.GetNames(sample, soundBank, sfxName, filePath, options.Mode);
                    EnsureDirectoryExists(Path.GetDirectoryName(filePath));
                    EuroSoundSfxTextExporter.ExportToFile(
                        sample,
                        soundBank.Header.FileVersion,
                        filePath,
                        sections,
                        soundDetailsRadii,
                        null,
                        samplePoolFileNames,
                        options.Mode == EuroSoundProjectDecompilerMode.ReplaceSections);
                    written++;
                }
            }

            return written;
        }

        private static Dictionary<uint, EuroSoundSfxRadiusData> BuildSoundDetailsRadiusLookup(EuroSoundProjectDecompilerOptions options)
        {
            SoundDetailsReader detailsReader = new SoundDetailsReader();
            Dictionary<uint, EuroSoundSfxRadiusData> radii = new Dictionary<uint, EuroSoundSfxRadiusData>();

            foreach (CompiledFileCandidate file in FindCompiledFileCandidates(options.CompiledFolder, options.SelectedTitle))
            {
                try
                {
                    if (file.FileType != FileType.SoundDetailsFile || !IsSupportedVersion(file.CommonHeader.FileVersion))
                    {
                        continue;
                    }

                    SoundDetails details = detailsReader.ReadSoundDetailsFile(file.FilePath, file.CommonHeader);
                    AddSoundDetailsRadii(radii, details);
                }
                catch (Exception)
                {
                    // SoundDetails files are optional; unreadable entries should not stop project decompile.
                }
            }

            return radii;
        }

        private static void AddSoundDetailsRadii(Dictionary<uint, EuroSoundSfxRadiusData> radii, SoundDetails details)
        {
            if (details == null || details.sfxItems == null)
            {
                return;
            }

            foreach (SoundDetailsData item in details.sfxItems)
            {
                uint hashCode = unchecked((uint)item.HashCode);
                EuroSoundSfxRadiusData radiusData = new EuroSoundSfxRadiusData
                {
                    InnerRadius = unchecked((short)item.InnerRadius),
                    OuterRadius = unchecked((short)item.OuterRadius),
                    DurationCentiseconds = (int)Math.Floor((decimal)item.Duration * 100m + 0.5m)
                };

                radii[hashCode] = radiusData;
                radii[StripSection(hashCode)] = radiusData;
            }
        }

        private static EuroSoundSfxRadiusData GetSoundDetailsData(uint hashCode, IDictionary<uint, EuroSoundSfxRadiusData> soundDetailsRadii)
        {
            if (soundDetailsRadii == null)
            {
                return null;
            }

            if (soundDetailsRadii.TryGetValue(hashCode, out EuroSoundSfxRadiusData details))
            {
                return details;
            }

            return soundDetailsRadii.TryGetValue(StripSection(hashCode), out details) ? details : null;
        }

        private static List<RecoveredGroup> RecoverGroups(List<SoundBankReadResult> soundBanks, HashcodeParser hashcodes, ProjectReference reference, EuroSoundProjectDecompilerResult result)
        {
            Dictionary<int, RecoveredGroup> groups = new Dictionary<int, RecoveredGroup>();

            foreach (SoundBankReadResult soundBank in soundBanks)
            {
                foreach (Sample sample in soundBank.Samples.Values)
                {
                    int groupHashCode = sample.GroupHashCode;
                    if (groupHashCode == 0)
                    {
                        continue;
                    }

                    if (!groups.TryGetValue(groupHashCode, out RecoveredGroup group))
                    {
                        group = new RecoveredGroup
                        {
                            GroupHashCode = groupHashCode,
                            Action1 = Flag(sample.Flags, 14),
                            MaxVoices = Math.Max(0, (int)sample.GroupMaxChannels)
                        };
                        groups.Add(groupHashCode, group);
                    }
                    else if (group.Action1 != Flag(sample.Flags, 14) || group.MaxVoices != Math.Max(0, (int)sample.GroupMaxChannels))
                    {
                        group.HasConflict = true;
                    }

                    string sfxName = GetSfxDependencyName(sample.HashCodeNumber, hashcodes, reference);
                    group.SfxNames.Add(sfxName);
                }
            }

            foreach (RecoveredGroup group in groups.Values)
            {
                group.Name = reference.FindGroupByDependencies(group.SfxNames) ?? "Group_" + group.GroupHashCode.ToString("000", CultureInfo.InvariantCulture);
                if (group.HasConflict)
                {
                    result.Warnings.Add("Group " + group.GroupHashCode.ToString(CultureInfo.InvariantCulture) + " has conflicting recovered properties; first values were used.");
                }
            }

            return groups.Values.OrderBy(group => group.GroupHashCode).ToList();
        }

        private static int WriteGroupFiles(List<RecoveredGroup> groups, ProjectFolders folders, ProjectReference reference, EuroSoundProjectDecompilerOptions options)
        {
            int written = 0;

            foreach (RecoveredGroup group in groups)
            {
                string groupName = SanitizeFileName(group.Name);
                string filePath = reference.GetGroupPath(groupName) ?? Path.Combine(folders.GroupsFolder, groupName + ".txt");
                List<string> lines = File.Exists(filePath)
                    ? new List<string>(File.ReadAllLines(filePath))
                    : CreateGroupFileLines();

                EnsureHeader(lines, "## EuroSound Group File");

                if (options.Mode == EuroSoundProjectDecompilerMode.Create || options.ReplaceGroupDependencies)
                {
                    ReplaceSection(lines, DependenciesSection, group.SfxNames.OrderBy(name => name, StringComparer.OrdinalIgnoreCase).ToList());
                }

                if (options.Mode == EuroSoundProjectDecompilerMode.Create || options.ReplaceGroupParameters)
                {
                    ReplaceSection(lines, SfxParametersSection, new List<string>
                    {
                        Line("Action1", group.Action1),
                        Line("MaxVoices", group.MaxVoices),
                        Line("Priority", 0),
                        Line("UseDistCheck", 0)
                    });
                }

                EnsureDirectoryExists(Path.GetDirectoryName(filePath));
                File.WriteAllLines(filePath, lines.ToArray(), Encoding.UTF8);
                written++;
            }

            return written;
        }

        private static List<RecoveredDuckerGroup> RecoverDuckerGroups(List<SoundBankReadResult> soundBanks, HashcodeParser hashcodes, ProjectReference reference, EuroSoundProjectDecompilerResult result, IDictionary<uint, EuroSoundSfxRadiusData> soundDetailsRadii)
        {
            Dictionary<int, RecoveredDuckerGroup> duckers = new Dictionary<int, RecoveredDuckerGroup>();

            foreach (SoundBankReadResult soundBank in soundBanks)
            {
                if (soundBank.Header.FileVersion != 6)
                {
                    continue;
                }

                foreach (Sample sample in soundBank.Samples.Values)
                {
                    int duckerHashCode = sample.SFXDucker;
                    if (duckerHashCode == 0)
                    {
                        continue;
                    }

                    if (!duckers.TryGetValue(duckerHashCode, out RecoveredDuckerGroup ducker))
                    {
                        ducker = new RecoveredDuckerGroup
                        {
                            DuckerHashCode = duckerHashCode,
                            Ducker = Math.Max(0, (int)sample.Ducker),
                            DuckerLenght = Math.Max(0, GetRecoveredDuckerLength(sample, soundDetailsRadii))
                        };
                        duckers.Add(duckerHashCode, ducker);
                    }
                    else if (ducker.Ducker != Math.Max(0, (int)sample.Ducker) || ducker.DuckerLenght != Math.Max(0, GetRecoveredDuckerLength(sample, soundDetailsRadii)))
                    {
                        ducker.HasConflict = true;
                    }

                    string sfxName = GetSfxDependencyName(sample.HashCodeNumber, hashcodes, reference);
                    ducker.SfxNames.Add(sfxName);
                }
            }

            foreach (RecoveredDuckerGroup ducker in duckers.Values)
            {
                ducker.Name = reference.FindDuckerByDependencies(ducker.SfxNames) ?? "Ducker_" + ducker.DuckerHashCode.ToString("000", CultureInfo.InvariantCulture);
                if (ducker.HasConflict)
                {
                    result.Warnings.Add("Ducker group " + ducker.DuckerHashCode.ToString(CultureInfo.InvariantCulture) + " has conflicting recovered properties; first values were used.");
                }
            }

            return duckers.Values.OrderBy(ducker => ducker.DuckerHashCode).ToList();
        }

        private static int GetRecoveredDuckerLength(Sample sample, IDictionary<uint, EuroSoundSfxRadiusData> soundDetailsRadii)
        {
            if (sample.Ducker <= 0)
            {
                return sample.DuckerLenght;
            }

            EuroSoundSfxRadiusData details = GetSoundDetailsData(sample.HashCodeNumber, soundDetailsRadii);
            if (details == null || details.DurationCentiseconds <= 0)
            {
                return sample.DuckerLenght;
            }

            return sample.DuckerLenght - details.DurationCentiseconds;
        }

        private static int WriteDuckerGroupFiles(List<RecoveredDuckerGroup> duckers, ProjectFolders folders, ProjectReference reference, EuroSoundProjectDecompilerOptions options)
        {
            int written = 0;

            foreach (RecoveredDuckerGroup ducker in duckers)
            {
                string duckerName = SanitizeFileName(ducker.Name);
                string filePath = reference.GetDuckerPath(duckerName) ?? Path.Combine(folders.DuckersFolder, duckerName + ".txt");
                List<string> lines = File.Exists(filePath)
                    ? new List<string>(File.ReadAllLines(filePath))
                    : CreateDuckerGroupFileLines();

                EnsureHeader(lines, "## EuroSound Ducker Group File");

                if (options.Mode == EuroSoundProjectDecompilerMode.Create || options.ReplaceDuckerDependencies)
                {
                    ReplaceSection(lines, DependenciesSection, ducker.SfxNames.OrderBy(name => name, StringComparer.OrdinalIgnoreCase).ToList());
                }

                if (options.Mode == EuroSoundProjectDecompilerMode.Create || options.ReplaceDuckerParameters)
                {
                    ReplaceSection(lines, "#DuckerParameters", new List<string>
                    {
                        Line("Ducker", ducker.Ducker),
                        Line("DuckerLenght", ducker.DuckerLenght)
                    });
                }

                EnsureDirectoryExists(Path.GetDirectoryName(filePath));
                File.WriteAllLines(filePath, lines.ToArray(), Encoding.UTF8);
                written++;
            }

            return written;
        }

        private static int WriteSoundBankFiles(List<SoundBankReadResult> soundBanks, List<RecoveredGroup> groups, ProjectFolders folders, ProjectReference reference, EuroSoundProjectDecompilerOptions options)
        {
            int written = 0;
            Dictionary<int, string> groupNames = groups.ToDictionary(group => group.GroupHashCode, group => group.Name);

            foreach (SoundBankReadResult soundBank in soundBanks)
            {
                string soundBankName = GetSoundBankName(soundBank, options.Hashcodes, reference);
                string filePath = reference.GetSoundBankPath(soundBank.Header.FileHashCode) ?? Path.Combine(folders.SoundBanksFolder, SanitizeFileName(soundBankName) + ".txt");
                List<string> dependencies = soundBank.Samples.Values
                    .Select(sample => sample.GroupHashCode != 0 && groupNames.ContainsKey(sample.GroupHashCode)
                        ? groupNames[sample.GroupHashCode]
                        : GetSfxDependencyName(sample.HashCodeNumber, options.Hashcodes, reference))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                List<string> lines = File.Exists(filePath)
                    ? new List<string>(File.ReadAllLines(filePath))
                    : CreateSoundBankFileLines(soundBank.Header.FileHashCode);

                EnsureHeader(lines, "## EuroSound Soundbank File");

                if (options.Mode == EuroSoundProjectDecompilerMode.Create || options.ReplaceSoundBankDependencies)
                {
                    ReplaceSection(lines, DependenciesSection, dependencies);
                }

                ReplaceSection(lines, HashCodeSection, new List<string>
                {
                    Line("HashCodeNumber", StripSection(soundBank.Header.FileHashCode))
                });

                EnsureDirectoryExists(Path.GetDirectoryName(filePath));
                File.WriteAllLines(filePath, lines.ToArray(), Encoding.UTF8);
                written++;
            }

            return written;
        }

        private static void PopulateSampleCatalog(List<SoundBankReadResult> soundBanks, List<StreamBankReadResult> streamBanks, ProjectFolders folders, ProjectReference reference, EuroSoundProjectDecompilerOptions options, ProjectSampleCatalog sampleCatalog, bool writeWavs)
        {
            SamplePoolFileNameResolver sampleNameResolver = new SamplePoolFileNameResolver(soundBanks, streamBanks, folders.MasterFolder, reference, options.Hashcodes, sampleCatalog, writeWavs);

            foreach (SoundBankReadResult soundBank in soundBanks)
            {
                foreach (Sample sample in soundBank.Samples.Values)
                {
                    string filePath = GetSfxFilePath(sample.HashCodeNumber, folders.SfxFolder, reference, options.Hashcodes, null);
                    string sfxName = Path.GetFileNameWithoutExtension(filePath);
                    sampleNameResolver.GetNames(sample, soundBank, sfxName, filePath, options.Mode);
                }
            }
        }

        private static int WriteMusicFiles(List<MusicBankReadResult> musicBanks, ProjectFolders folders, ProjectReference reference, EuroSoundProjectDecompilerOptions options)
        {
            if (musicBanks == null || musicBanks.Count == 0)
            {
                return 0;
            }

            int written = 0;
            foreach (IGrouping<uint, MusicBankReadResult> group in musicBanks.GroupBy(item => item.HashCode))
            {
                MusicBankReadResult bestMusic = group
                    .OrderByDescending(item => GetMusicSourceSampleRate(item.Header))
                    .ThenBy(item => PlatformRank(item.Header.Platform))
                    .FirstOrDefault();
                if (bestMusic == null || bestMusic.Music == null)
                {
                    continue;
                }

                string musicName = GetMusicName(group.Key, bestMusic.FilePath, reference, options.Hashcodes);
                string wavPath = GenericMethods.GetFinalPath(Path.Combine(folders.MusicsFolder, musicName + ".wav"));
                string markerPath = Path.ChangeExtension(wavPath, ".mkr");

                byte[] leftPcm;
                byte[] rightPcm;
                if (!TryDecodeMusic(bestMusic, out leftPcm, out rightPcm))
                {
                    continue;
                }

                WriteMusicMasterWav(wavPath, bestMusic, leftPcm, rightPcm);
                WriteMusicMarkerFile(markerPath, bestMusic);
                written++;
            }

            return written;
        }

        private static bool TryDecodeMusic(MusicBankReadResult musicBank, out byte[] leftPcm, out byte[] rightPcm)
        {
            leftPcm = null;
            rightPcm = null;
            if (musicBank == null || musicBank.Music == null || musicBank.Music.EncodedData == null || musicBank.Music.EncodedData.Length < 2)
            {
                return false;
            }

            AudioFunctions audioFunctions = new AudioFunctions();
            leftPcm = GenericMethods.DecodeMusicChannel(musicBank.Music.EncodedData[0], audioFunctions, musicBank.Header);
            rightPcm = GenericMethods.DecodeMusicChannel(musicBank.Music.EncodedData[1], audioFunctions, musicBank.Header);
            return leftPcm != null && leftPcm.Length > 0 && rightPcm != null && rightPcm.Length > 0;
        }

        private static void WriteMusicMasterWav(string wavPath, MusicBankReadResult musicBank, byte[] leftPcm, byte[] rightPcm)
        {
            int sourceSampleRate = GetMusicSourceSampleRate(musicBank.Header);
            using (RawSourceWaveStream leftStream = new RawSourceWaveStream(new MemoryStream(leftPcm), new WaveFormat(sourceSampleRate, 16, 1)))
            using (RawSourceWaveStream rightStream = new RawSourceWaveStream(new MemoryStream(rightPcm), new WaveFormat(sourceSampleRate, 16, 1)))
            {
                IWaveProvider stereoProvider = new MultiplexingWaveProvider(new IWaveProvider[] { leftStream, rightStream }, 2);
                ISampleProvider sampleProvider = stereoProvider.ToSampleProvider();
                if (sourceSampleRate != MasterSampleRate)
                {
                    sampleProvider = new WdlResamplingSampleProvider(sampleProvider, MasterSampleRate);
                }

                EuroSoundWaveWriter.WriteSampleProvider16(
                    wavPath,
                    sampleProvider,
                    ScaleLoopInfo(CreateMusicLoopInfo(musicBank.Music.Markers, Math.Min(leftPcm.Length, rightPcm.Length) / 2), sourceSampleRate, MasterSampleRate));
            }
        }

        private static void WriteMusicMarkerFile(string markerPath, MusicBankReadResult musicBank)
        {
            if (musicBank == null || musicBank.Header == null || musicBank.Header.FileLength1 == 0)
            {
                return;
            }

            using (FileStream input = File.Open(musicBank.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                input.Seek(musicBank.Header.FileStart1, SeekOrigin.Begin);
                byte[] markerData = new byte[(int)musicBank.Header.FileLength1];
                int read = input.Read(markerData, 0, markerData.Length);
                if (read <= 0)
                {
                    return;
                }

                if (read != markerData.Length)
                {
                    Array.Resize(ref markerData, read);
                }

                File.WriteAllBytes(markerPath, markerData);
            }
        }

        private static void WriteDiagnosticReport(EuroSoundProjectDecompilerOptions options, ProjectFolders folders, List<SoundBankReadResult> soundBanks, List<StreamBankReadResult> streamBanks, List<MusicBankReadResult> musicBanks, EuroSoundProjectDecompilerResult result)
        {
            if (folders == null || string.IsNullOrEmpty(folders.RootFolder) || result == null)
            {
                return;
            }

            string reportPath = Path.Combine(folders.RootFolder, "decompile_report.txt");
            List<string> lines = new List<string>();
            lines.Add("EuroSound Explorer Decompile Report");
            lines.Add("Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
            lines.Add("Mode: " + (options == null ? string.Empty : options.Mode.ToString()));
            lines.Add("Compiled folder: " + (options == null ? string.Empty : options.CompiledFolder));
            lines.Add("Output folder: " + folders.RootFolder);
            lines.Add(string.Empty);
            lines.Add("Summary");
            lines.Add("SoundBanks read: " + result.SoundbankFilesRead.ToString(CultureInfo.InvariantCulture));
            lines.Add("StreamBanks read: " + result.StreambanksRead.ToString(CultureInfo.InvariantCulture));
            lines.Add("MusicBanks read: " + result.MusicbanksRead.ToString(CultureInfo.InvariantCulture));
            lines.Add("SoundBanks written: " + result.SoundbanksWritten.ToString(CultureInfo.InvariantCulture));
            lines.Add("SFX files written: " + result.SfxWritten.ToString(CultureInfo.InvariantCulture));
            lines.Add("Sample entries written: " + result.SamplesWritten.ToString(CultureInfo.InvariantCulture));
            lines.Add("Music WAVs written: " + result.MusicsWritten.ToString(CultureInfo.InvariantCulture));
            lines.Add("Groups written: " + result.GroupsWritten.ToString(CultureInfo.InvariantCulture));
            lines.Add("Ducker groups written: " + result.DuckerGroupsWritten.ToString(CultureInfo.InvariantCulture));
            lines.Add("Memory maps written: " + result.MemoryMapsWritten.ToString(CultureInfo.InvariantCulture));
            lines.Add("Failed files: " + result.FailedFiles.ToString(CultureInfo.InvariantCulture));
            lines.Add(string.Empty);
            lines.Add("Audio and loop summary");
            lines.Add("SoundBank SFX entries: " + CountSoundBankSamples(soundBanks).ToString(CultureInfo.InvariantCulture));
            lines.Add("SoundBank looped samples: " + CountSoundBankLoops(soundBanks).ToString(CultureInfo.InvariantCulture));
            lines.Add("Stream entries: " + CountStreamEntries(streamBanks).ToString(CultureInfo.InvariantCulture));
            lines.Add("Stream entries with loop: " + CountStreamLoops(streamBanks).ToString(CultureInfo.InvariantCulture));
            lines.Add("Music entries: " + (musicBanks == null ? 0 : musicBanks.Count).ToString(CultureInfo.InvariantCulture));
            lines.Add("Music entries with loop: " + CountMusicLoops(musicBanks).ToString(CultureInfo.InvariantCulture));
            lines.Add("Stream markers: " + CountStreamMarkers(streamBanks).ToString(CultureInfo.InvariantCulture));
            lines.Add("Music markers: " + CountMusicMarkers(musicBanks).ToString(CultureInfo.InvariantCulture));
            lines.Add(string.Empty);
            AppendCodecSummary(lines, soundBanks, streamBanks, musicBanks);
            AppendMessages(lines, "Warnings", result.Warnings);
            AppendMessages(lines, "Failed files", result.FailedFileMessages);

            File.WriteAllLines(reportPath, lines.ToArray(), Encoding.UTF8);
            result.ReportPath = reportPath;
        }

        private static void AppendCodecSummary(List<string> lines, List<SoundBankReadResult> soundBanks, List<StreamBankReadResult> streamBanks, List<MusicBankReadResult> musicBanks)
        {
            lines.Add("Codec summary");
            if (soundBanks != null)
            {
                foreach (SoundBankReadResult soundBank in soundBanks)
                {
                    lines.Add(FormatCodecLine(Path.GetFileName(soundBank.FilePath), soundBank.Header.FileVersion, soundBank.Header.Platform, EuroSoundBankType.SoundBank));
                }
            }

            if (streamBanks != null)
            {
                foreach (StreamBankReadResult streamBank in streamBanks)
                {
                    lines.Add(FormatCodecLine(Path.GetFileName(streamBank.FilePath), streamBank.Header.FileVersion, streamBank.Header.Platform, EuroSoundBankType.StreamBank));
                }
            }

            if (musicBanks != null)
            {
                foreach (MusicBankReadResult musicBank in musicBanks)
                {
                    lines.Add(FormatCodecLine(Path.GetFileName(musicBank.FilePath), musicBank.Header.FileVersion, musicBank.Header.Platform, EuroSoundBankType.MusicBank));
                }
            }

            lines.Add(string.Empty);
        }

        private static string FormatCodecLine(string fileName, int fileVersion, string platform, EuroSoundBankType bankType)
        {
            EuroSoundAudioCodec codec = EuroSoundCodecMatrix.GetCodec(fileVersion, platform, bankType);
            return string.Format(CultureInfo.InvariantCulture, "{0}: version {1}, platform {2}, {3}, codec {4}", fileName, fileVersion, platform, bankType, codec);
        }

        private static void AppendMessages(List<string> lines, string title, List<string> messages)
        {
            lines.Add(title);
            if (messages == null || messages.Count == 0)
            {
                lines.Add("None");
                lines.Add(string.Empty);
                return;
            }

            foreach (string message in messages)
            {
                lines.Add("- " + message);
            }

            lines.Add(string.Empty);
        }

        private static int CountSoundBankSamples(List<SoundBankReadResult> soundBanks)
        {
            return soundBanks == null ? 0 : soundBanks.Sum(soundBank => soundBank.Samples == null ? 0 : soundBank.Samples.Count);
        }

        private static int CountSoundBankLoops(List<SoundBankReadResult> soundBanks)
        {
            return soundBanks == null ? 0 : soundBanks.Sum(soundBank => soundBank.StoredData == null ? 0 : soundBank.StoredData.Count(sample => sample.IsLooped));
        }

        private static int CountStreamEntries(List<StreamBankReadResult> streamBanks)
        {
            return streamBanks == null ? 0 : streamBanks.Sum(streamBank => streamBank.Streams == null ? 0 : streamBank.Streams.Count);
        }

        private static int CountStreamLoops(List<StreamBankReadResult> streamBanks)
        {
            return streamBanks == null ? 0 : streamBanks.Sum(streamBank => streamBank.Streams == null ? 0 : streamBank.Streams.Count(stream => EuroSoundMarkerLoopResolver.IsLooped(stream.Markers, MarkerLoopMode.LoopUnlessEndMarker)));
        }

        private static int CountMusicLoops(List<MusicBankReadResult> musicBanks)
        {
            return musicBanks == null ? 0 : musicBanks.Count(musicBank => musicBank.Music != null && EuroSoundMarkerLoopResolver.IsLooped(musicBank.Music.Markers, MarkerLoopMode.RequireLoopMarker));
        }

        private static int CountStreamMarkers(List<StreamBankReadResult> streamBanks)
        {
            return streamBanks == null ? 0 : streamBanks.Sum(streamBank => streamBank.Streams == null ? 0 : streamBank.Streams.Sum(stream => stream.Markers == null ? 0 : stream.Markers.Length));
        }

        private static int CountMusicMarkers(List<MusicBankReadResult> musicBanks)
        {
            return musicBanks == null ? 0 : musicBanks.Sum(musicBank => musicBank.Music == null || musicBank.Music.Markers == null ? 0 : musicBank.Music.Markers.Length);
        }

        private static string GetMusicName(uint hashCode, string filePath, ProjectReference reference, HashcodeParser hashcodes)
        {
            string label = GetHashCodeLabel(hashcodes, hashCode);
            if (string.IsNullOrWhiteSpace(label) || label.StartsWith("**", StringComparison.Ordinal))
            {
                label = Path.GetFileNameWithoutExtension(filePath);
            }

            if (string.IsNullOrWhiteSpace(label))
            {
                label = "Music_" + StripSection(hashCode).ToString("X4", CultureInfo.InvariantCulture);
            }

            return SanitizeFileName(label);
        }

        private static int GetMusicSourceSampleRate(StreambankHeader header)
        {
            return header != null && EuroSoundCodecMatrix.IsXboxPlatform(header.Platform) ? 44100 : 32000;
        }

        private static WavLoopInfo CreateMusicLoopInfo(Marker[] markers, int totalSamples)
        {
            return EuroSoundMarkerLoopResolver.CreateLoopInfo(markers, totalSamples, MarkerLoopMode.RequireLoopMarker);
        }

        private static WavLoopInfo ScaleLoopInfo(WavLoopInfo loopInfo, int sourceSampleRate, int destinationSampleRate)
        {
            if (loopInfo == null || !loopInfo.IsValid || sourceSampleRate <= 0 || destinationSampleRate <= 0)
            {
                return null;
            }

            if (sourceSampleRate == destinationSampleRate)
            {
                return loopInfo;
            }

            double scale = destinationSampleRate / (double)sourceSampleRate;
            return new WavLoopInfo((uint)Math.Round(loopInfo.StartSample * scale), (uint)Math.Round(loopInfo.EndSample * scale));
        }

        private static int WriteSystemFiles(ProjectFolders folders, List<SoundBankReadResult> soundBanks, ProjectSampleCatalog sampleCatalog, List<ProjectDetailsReadResult> projectDetails, EuroSoundProjectDecompilerResult result)
        {
            List<string> platforms = GetProjectPlatforms(folders, soundBanks);
            List<SampleRateProfile> sampleRateProfiles = BuildSampleRateProfiles(platforms, sampleCatalog.Samples);

            WritePropertiesFile(folders, platforms, sampleRateProfiles, projectDetails, result);
            return WriteSamplesFile(folders, platforms.Count, sampleRateProfiles, sampleCatalog);
        }

        private static int WriteSamplesFile(ProjectFolders folders, int platformCount, List<SampleRateProfile> sampleRateProfiles, ProjectSampleCatalog sampleCatalog)
        {
            string filePath = Path.Combine(folders.SystemFolder, "Samples.txt");
            List<ProjectSampleEntry> samples = sampleCatalog.Samples
                .OrderBy(sample => NormalizeSampleRelativePath(sample.RelativePath), StringComparer.InvariantCultureIgnoreCase)
                .ToList();
            List<string> lines = CreateEuroSoundHeader("## EuroSound Samples File");
            List<string> values = new List<string>
            {
                samples.Count.ToString(CultureInfo.InvariantCulture)
            };

            values.AddRange(samples.Select(sample => NormalizeSampleRelativePath(sample.RelativePath)));
            values.AddRange(samples.Select(sample => GetSampleRateProfileLabel(sampleRateProfiles, sample)));
            values.AddRange(samples.Select(sample => sample.FileSizeBytes.ToString(CultureInfo.InvariantCulture)));
            values.AddRange(samples.Select(sample => sample.ModifiedDate.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
            values.AddRange(samples.Select(sample => "True"));
            values.AddRange(samples.Select(sample => sample.IsStream ? "True" : "False"));

            for (int i = 0; i < platformCount; i++)
            {
                values.AddRange(samples.Select(sample => "True"));
            }

            lines.Add("#AvailableSamples");
            lines.AddRange(values);
            lines.Add(SectionEnd);
            File.WriteAllLines(filePath, lines, Encoding.Default);
            return samples.Count;
        }

        private static void WritePropertiesFile(ProjectFolders folders, List<string> platforms, List<SampleRateProfile> sampleRateProfiles, List<ProjectDetailsReadResult> projectDetails, EuroSoundProjectDecompilerResult result)
        {
            string filePath = Path.Combine(folders.SystemFolder, "Properties.txt");
            List<string> lines = File.Exists(filePath)
                ? File.ReadAllLines(filePath, Encoding.Default).ToList()
                : CreateEuroSoundHeader("## EuroSound Properties File");

            if (ReadSection(lines, "#AvailableFormats").Count == 0)
            {
                List<string> availableFormats = new List<string>
                {
                    platforms.Count.ToString(CultureInfo.InvariantCulture)
                };
                availableFormats.AddRange(platforms);
                availableFormats.AddRange(platforms.Select(platform => Path.Combine(folders.RootFolder, "_ESOutput")));
                availableFormats.AddRange(platforms.Select(platform => "On"));
                RewriteDocumentSection(lines, "#AvailableFormats", availableFormats);
            }
            else
            {
                platforms = ReadProjectPlatforms(lines);
            }

            List<SampleRateProfile> sampleRates = sampleRateProfiles.ToList();

            List<string> sampleRateLabels = new List<string> { "Default" };
            sampleRateLabels.AddRange(sampleRates.Select(profile => profile.Label));
            RewriteDocumentSection(lines, "#AvailableReSampleRates", sampleRateLabels);
            for (int i = 0; i < platforms.Count; i++)
            {
                string platform = platforms[i];
                List<string> platformRates = new List<string>
                {
                    GetDefaultSampleRate(platform).ToString(CultureInfo.InvariantCulture)
                };
                platformRates.AddRange(sampleRates.Select(profile => profile.GetFrequency(platform).ToString(CultureInfo.InvariantCulture)));
                RewriteDocumentSection(lines, "#ReSampleRates" + i.ToString(CultureInfo.InvariantCulture), platformRates);
            }

            result.MemoryMapsWritten = WriteMemoryMapSections(lines, platforms, projectDetails);

            SetDocumentSectionValue(lines, "#MiscProperites", "DefaultRate", "0");
            SetDocumentSectionValue(lines, "#MiscProperites", "SampleFileFolder", folders.RootFolder);

            File.WriteAllLines(filePath, lines, Encoding.Default);
        }

        private static int WriteMemoryMapSections(List<string> lines, List<string> platforms, List<ProjectDetailsReadResult> projectDetails)
        {
            if (projectDetails == null || projectDetails.Count == 0)
            {
                return 0;
            }

            bool usesVersion6Maps = projectDetails.Any(item => item.Details != null && item.Details.memoryMapsData.Count > 0);
            return usesVersion6Maps
                ? WriteVersion6MemoryMapSections(lines, platforms, projectDetails)
                : WriteVersion45MemoryMapSections(lines, platforms, projectDetails);
        }

        private static int WriteVersion45MemoryMapSections(List<string> lines, List<string> platforms, List<ProjectDetailsReadResult> projectDetails)
        {
            RemoveDocumentSection(lines, "#AvailableMemoryMapSlots");
            RemoveDocumentSectionsByPrefix(lines, "#MemoryMapSlotSizes");

            int mapCount = projectDetails
                .SelectMany(item => item.Details.memorySlotsData)
                .Where(slot => slot.SlotNumber >= 0)
                .Select(slot => slot.SlotNumber + 1)
                .DefaultIfEmpty(0)
                .Max();

            if (mapCount == 0)
            {
                return 0;
            }

            List<string> memoryMaps = Enumerable.Range(0, mapCount)
                .Select(index => "MemoryMap_" + index.ToString("000", CultureInfo.InvariantCulture))
                .ToList();
            RewriteDocumentSection(lines, "#AvailableMemoryMaps", memoryMaps);

            Dictionary<string, ProjectDetailsReadResult> detailsByPlatform = BuildProjectDetailsByPlatform(projectDetails);
            for (int platformIndex = 0; platformIndex < platforms.Count; platformIndex++)
            {
                ProjectDetailsReadResult details = GetProjectDetailsForPlatform(detailsByPlatform, platforms[platformIndex]);
                int[] sizes = new int[mapCount];
                if (details != null)
                {
                    foreach (ProjectSlots slot in details.Details.memorySlotsData)
                    {
                        if (slot.SlotNumber >= 0 && slot.SlotNumber < sizes.Length)
                        {
                            sizes[slot.SlotNumber] = Math.Max(0, slot.MemorySize);
                        }
                    }
                }

                RewriteDocumentSection(lines, "#MemoryMaps" + platformIndex.ToString(CultureInfo.InvariantCulture), sizes.Select(size => size.ToString(CultureInfo.InvariantCulture)).ToList());
            }

            SetDocumentSectionValue(lines, "#MiscProperites", "DefaultMap", "0");
            return mapCount;
        }

        private static int WriteVersion6MemoryMapSections(List<string> lines, List<string> platforms, List<ProjectDetailsReadResult> projectDetails)
        {
            Dictionary<string, List<string>> existingSlotNames = ReadExistingMemorySlotNames(lines);
            RemoveDocumentSection(lines, "#AvailableMemoryMaps");
            RemoveDocumentSectionsByPrefix(lines, "#MemoryMaps");

            List<ProjectMemoryMap> memoryMaps = projectDetails
                .OrderByDescending(item => item.Details.memoryMapsData.Count)
                .SelectMany(item => item.Details.memoryMapsData)
                .GroupBy(map => string.IsNullOrWhiteSpace(map.Name) ? "MemoryMap" : map.Name, StringComparer.OrdinalIgnoreCase)
                .Select(group => group.OrderByDescending(map => map.SlotSizes.Count).First())
                .ToList();

            if (memoryMaps.Count == 0)
            {
                return 0;
            }

            List<string> slotLines = new List<string>();
            foreach (ProjectMemoryMap memoryMap in memoryMaps)
            {
                string mapName = GetMemoryMapName(memoryMap);
                for (int slotIndex = 0; slotIndex < memoryMap.SlotSizes.Count; slotIndex++)
                {
                    slotLines.Add(string.Join("\t",
                        mapName,
                        GetMemorySlotName(existingSlotNames, mapName, slotIndex),
                        slotIndex.ToString(CultureInfo.InvariantCulture)));
                }
            }
            RewriteDocumentSection(lines, "#AvailableMemoryMapSlots", slotLines);

            Dictionary<string, ProjectDetailsReadResult> detailsByPlatform = BuildProjectDetailsByPlatform(projectDetails);
            for (int platformIndex = 0; platformIndex < platforms.Count; platformIndex++)
            {
                ProjectDetailsReadResult details = GetProjectDetailsForPlatform(detailsByPlatform, platforms[platformIndex]);
                List<string> sizeLines = new List<string>();
                foreach (ProjectMemoryMap memoryMap in memoryMaps)
                {
                    string mapName = GetMemoryMapName(memoryMap);
                    ProjectMemoryMap platformMap = details?.Details.memoryMapsData.FirstOrDefault(map => string.Equals(GetMemoryMapName(map), mapName, StringComparison.OrdinalIgnoreCase));
                    for (int slotIndex = 0; slotIndex < memoryMap.SlotSizes.Count; slotIndex++)
                    {
                        int size = platformMap != null && slotIndex < platformMap.SlotSizes.Count
                            ? Math.Max(0, platformMap.SlotSizes[slotIndex])
                            : 0;
                        sizeLines.Add(string.Join("\t",
                            mapName,
                            GetMemorySlotName(existingSlotNames, mapName, slotIndex),
                            size.ToString(CultureInfo.InvariantCulture),
                            slotIndex.ToString(CultureInfo.InvariantCulture)));
                    }
                }

                RewriteDocumentSection(lines, "#MemoryMapSlotSizes" + platformIndex.ToString(CultureInfo.InvariantCulture), sizeLines);
            }

            if (memoryMaps[0].SlotSizes.Count > 0)
            {
                string mapName = GetMemoryMapName(memoryMaps[0]);
                SetDocumentSectionValue(lines, "#MiscProperites", "DefaultSlot", GetMemorySlotName(existingSlotNames, mapName, 0));
            }

            return memoryMaps.Count;
        }

        private static Dictionary<string, List<string>> ReadExistingMemorySlotNames(List<string> lines)
        {
            Dictionary<string, List<string>> slotsByMap = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            foreach (string line in ReadSection(lines, "#AvailableMemoryMapSlots"))
            {
                string[] parts = line.Split(new[] { '\t', '|' }, StringSplitOptions.None);
                if (parts.Length < 2)
                {
                    continue;
                }

                string mapName = parts[0].Trim();
                string slotName = parts[1].Trim();
                if (string.IsNullOrWhiteSpace(mapName) || string.IsNullOrWhiteSpace(slotName))
                {
                    continue;
                }

                List<string> slotNames;
                if (!slotsByMap.TryGetValue(mapName, out slotNames))
                {
                    slotNames = new List<string>();
                    slotsByMap[mapName] = slotNames;
                }

                slotNames.Add(slotName);
            }

            return slotsByMap;
        }

        private static Dictionary<string, ProjectDetailsReadResult> BuildProjectDetailsByPlatform(List<ProjectDetailsReadResult> projectDetails)
        {
            return projectDetails
                .GroupBy(item => GetProjectPlatformName(item.Header.Platform), StringComparer.OrdinalIgnoreCase)
                .ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase);
        }

        private static ProjectDetailsReadResult GetProjectDetailsForPlatform(Dictionary<string, ProjectDetailsReadResult> detailsByPlatform, string platform)
        {
            ProjectDetailsReadResult details;
            if (detailsByPlatform.TryGetValue(GetProjectPlatformName(platform), out details))
            {
                return details;
            }

            return detailsByPlatform.Values.FirstOrDefault();
        }

        private static string GetMemoryMapName(ProjectMemoryMap memoryMap)
        {
            return string.IsNullOrWhiteSpace(memoryMap.Name) ? "MemoryMap" : memoryMap.Name.Trim();
        }

        private static string GetMemorySlotName(Dictionary<string, List<string>> existingSlotNames, string mapName, int slotIndex)
        {
            List<string> slotNames;
            if (existingSlotNames != null &&
                existingSlotNames.TryGetValue(mapName, out slotNames) &&
                slotIndex >= 0 &&
                slotIndex < slotNames.Count &&
                !string.IsNullOrWhiteSpace(slotNames[slotIndex]))
            {
                return slotNames[slotIndex];
            }

            return "Slot_" + slotIndex.ToString("000", CultureInfo.InvariantCulture);
        }

        private static List<EuroSoundSfxTextSection> GetSfxSections(EuroSoundProjectDecompilerOptions options)
        {
            if (options.Mode == EuroSoundProjectDecompilerMode.Create)
            {
                return new List<EuroSoundSfxTextSection>
                {
                    EuroSoundSfxTextSection.Parameters,
                    EuroSoundSfxTextSection.SamplePoolFiles,
                    EuroSoundSfxTextSection.SamplePoolModes,
                    EuroSoundSfxTextSection.SamplePoolControl
                };
            }

            List<EuroSoundSfxTextSection> sections = new List<EuroSoundSfxTextSection>();
            if (options.ReplaceSfxParameters)
            {
                sections.Add(EuroSoundSfxTextSection.Parameters);
            }
            if (options.ReplaceSfxSamplePoolFiles)
            {
                sections.Add(EuroSoundSfxTextSection.SamplePoolFiles);
            }
            if (options.ReplaceSfxSamplePoolModes)
            {
                sections.Add(EuroSoundSfxTextSection.SamplePoolModes);
            }
            if (options.ReplaceSfxSamplePoolControl)
            {
                sections.Add(EuroSoundSfxTextSection.SamplePoolControl);
            }

            return sections;
        }

        private static string GetSfxFilePath(uint hashCode, string sfxFolder, ProjectReference reference, HashcodeParser hashcodes, string platform)
        {
            string referencePath = reference.GetSfxPath(hashCode);
            if (!string.IsNullOrWhiteSpace(referencePath) && string.IsNullOrWhiteSpace(platform))
            {
                return referencePath;
            }

            string name = GetSfxDependencyName(hashCode, hashcodes, reference);
            if (!string.IsNullOrWhiteSpace(platform))
            {
                name += "_" + GetPlatformSuffix(platform);
            }

            return Path.Combine(sfxFolder, name + ".txt");
        }

        private static string GetSfxDependencyName(uint hashCode, HashcodeParser hashcodes, ProjectReference reference)
        {
            string referenceName = reference.GetSfxName(hashCode);
            if (!string.IsNullOrWhiteSpace(referenceName))
            {
                return referenceName;
            }

            string label = GetHashCodeLabel(hashcodes, hashCode);
            if (string.IsNullOrWhiteSpace(label) || string.Equals(label, SfxMissingName, StringComparison.Ordinal))
            {
                label = "SFX_" + hashCode.ToString("X8", CultureInfo.InvariantCulture);
            }

            return SanitizeFileName(label);
        }

        private static string GetSoundBankName(SoundBankReadResult soundBank, HashcodeParser hashcodes, ProjectReference reference)
        {
            string referenceName = reference.GetSoundBankName(soundBank.Header.FileHashCode);
            if (!string.IsNullOrWhiteSpace(referenceName))
            {
                return referenceName;
            }

            string label = GetHashCodeLabel(hashcodes, soundBank.Header.FileHashCode);
            if (string.IsNullOrWhiteSpace(label) || label.StartsWith("**", StringComparison.Ordinal))
            {
                label = "SoundBank_" + soundBank.Header.FileHashCode.ToString("X8", CultureInfo.InvariantCulture);
            }

            return SanitizeFileName(label);
        }

        private static string GetSoundBankGroupKey(SoundBankReadResult soundBank)
        {
            return soundBank.Header.FileVersion.ToString(CultureInfo.InvariantCulture) + "|" + soundBank.Header.FileHashCode.ToString("X8", CultureInfo.InvariantCulture);
        }

        private static List<string> CreateGroupFileLines()
        {
            List<string> lines = CreateHeaderLines("## EuroSound Group File");
            lines.Add("");
            lines.Add(DependenciesSection);
            lines.Add(SectionEnd);
            lines.Add("");
            lines.Add(SfxParametersSection);
            lines.Add("Action1  0");
            lines.Add("MaxVoices  2");
            lines.Add("Priority  50");
            lines.Add("UseDistCheck  0");
            lines.Add(SectionEnd);
            return lines;
        }

        private static List<string> CreateDuckerGroupFileLines()
        {
            List<string> lines = CreateHeaderLines("## EuroSound Ducker Group File");
            lines.Add("");
            lines.Add(DependenciesSection);
            lines.Add(SectionEnd);
            lines.Add("");
            lines.Add("#DuckerParameters");
            lines.Add("Ducker  0");
            lines.Add("DuckerLenght  0");
            lines.Add(SectionEnd);
            return lines;
        }

        private static List<string> CreateSoundBankFileLines(uint hashCode)
        {
            List<string> lines = CreateHeaderLines("## EuroSound Soundbank File");
            lines.Add("");
            lines.Add(DependenciesSection);
            lines.Add(SectionEnd);
            lines.Add("");
            lines.Add(HashCodeSection);
            lines.Add(Line("HashCodeNumber", StripSection(hashCode)));
            lines.Add(SectionEnd);
            return lines;
        }

        private static List<string> CreateHeaderLines(string title)
        {
            string timestamp = DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            string userName = string.IsNullOrWhiteSpace(Environment.UserName) ? "Unknown" : Environment.UserName;

            return new List<string>
            {
                title,
                "## First Created ... " + timestamp,
                "## Created By ... " + userName,
                "## Last Modified ... " + timestamp,
                "## Last Modified By ... " + userName
            };
        }

        private static void EnsureHeader(List<string> lines, string header)
        {
            if (lines.Count == 0)
            {
                lines.Add(header);
                return;
            }

            lines[0] = header;
        }

        private static void ReplaceSection(List<string> lines, string sectionMarker, List<string> replacementLines)
        {
            int startIndex = FindSectionStart(lines, sectionMarker);
            if (startIndex < 0)
            {
                if (lines.Count > 0 && lines[lines.Count - 1].Length != 0)
                {
                    lines.Add("");
                }
                lines.Add(sectionMarker);
                lines.AddRange(replacementLines);
                lines.Add(SectionEnd);
                return;
            }

            int endIndex = FindSectionEnd(lines, startIndex + 1);
            if (endIndex < 0)
            {
                endIndex = startIndex;
            }

            lines.RemoveRange(startIndex + 1, endIndex - startIndex);
            lines.InsertRange(startIndex + 1, replacementLines);
            lines.Insert(startIndex + 1 + replacementLines.Count, SectionEnd);
        }

        private static List<string> ReadSection(List<string> lines, string sectionMarker)
        {
            List<string> values = new List<string>();
            int startIndex = FindSectionStart(lines, sectionMarker);
            if (startIndex < 0)
            {
                return values;
            }

            int endIndex = FindSectionEnd(lines, startIndex + 1);
            if (endIndex < 0)
            {
                endIndex = lines.Count;
            }

            for (int i = startIndex + 1; i < endIndex; i++)
            {
                string line = lines[i].Trim();
                if (line.Length == 0 || line.StartsWith("#", StringComparison.Ordinal))
                {
                    continue;
                }

                values.Add(line);
            }

            return values;
        }

        private static int FindSectionStart(List<string> lines, string sectionMarker)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (string.Equals(lines[i].Trim(), sectionMarker, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }

        private static int FindSectionEnd(List<string> lines, int startIndex)
        {
            for (int i = startIndex; i < lines.Count; i++)
            {
                if (string.Equals(lines[i].Trim(), SectionEnd, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }

        private static uint ReadHashCode(List<string> lines)
        {
            foreach (string line in ReadSection(lines, HashCodeSection))
            {
                string[] parts = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2 && string.Equals(parts[0], "HashCodeNumber", StringComparison.OrdinalIgnoreCase))
                {
                    if (uint.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out uint value))
                    {
                        return value;
                    }
                }
            }

            return 0;
        }

        private static List<string> CreateEuroSoundHeader(string title)
        {
            string now = DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            string user = Environment.UserName ?? string.Empty;
            return new List<string>
            {
                title,
                "## First Created ... " + now,
                "## Created By ... " + user,
                "## Last Modified ... " + now,
                "## Last Modified By ... " + user,
                string.Empty
            };
        }

        private static void RewriteDocumentSection(List<string> lines, string sectionMarker, List<string> values)
        {
            int sectionStart = FindSectionStart(lines, sectionMarker);
            if (sectionStart < 0)
            {
                if (lines.Count > 0 && lines[lines.Count - 1].Length != 0)
                {
                    lines.Add(string.Empty);
                }

                lines.Add(sectionMarker);
                lines.AddRange(values);
                lines.Add(SectionEnd);
                lines.Add(string.Empty);
                return;
            }

            int sectionEnd = FindSectionEnd(lines, sectionStart + 1);
            if (sectionEnd < 0)
            {
                sectionEnd = sectionStart;
                lines.Insert(sectionStart + 1, SectionEnd);
            }

            lines.RemoveRange(sectionStart + 1, sectionEnd - sectionStart - 1);
            lines.InsertRange(sectionStart + 1, values);
        }

        private static void RemoveDocumentSection(List<string> lines, string sectionMarker)
        {
            int sectionStart = FindSectionStart(lines, sectionMarker);
            if (sectionStart < 0)
            {
                return;
            }

            int sectionEnd = FindSectionEnd(lines, sectionStart + 1);
            if (sectionEnd < 0)
            {
                sectionEnd = sectionStart;
            }

            lines.RemoveRange(sectionStart, sectionEnd - sectionStart + 1);
        }

        private static void RemoveDocumentSectionsByPrefix(List<string> lines, string sectionPrefix)
        {
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                string line = lines[i].Trim();
                if (!line.StartsWith(sectionPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                int sectionEnd = FindSectionEnd(lines, i + 1);
                if (sectionEnd < 0)
                {
                    sectionEnd = i;
                }

                lines.RemoveRange(i, sectionEnd - i + 1);
            }
        }

        private static void SetDocumentSectionValue(List<string> lines, string sectionMarker, string key, string value)
        {
            int sectionStart = FindSectionStart(lines, sectionMarker);
            if (sectionStart < 0)
            {
                RewriteDocumentSection(lines, sectionMarker, new List<string> { key + " " + value });
                return;
            }

            int sectionEnd = FindSectionEnd(lines, sectionStart + 1);
            if (sectionEnd < 0)
            {
                sectionEnd = lines.Count;
                lines.Add(SectionEnd);
            }

            for (int i = sectionStart + 1; i < sectionEnd; i++)
            {
                string line = lines[i].Trim();
                if (line.StartsWith(key + " ", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(line, key, StringComparison.OrdinalIgnoreCase))
                {
                    lines[i] = key + " " + value;
                    return;
                }
            }

            lines.Insert(sectionEnd, key + " " + value);
        }

        private static List<string> GetProjectPlatforms(ProjectFolders folders, List<SoundBankReadResult> soundBanks)
        {
            string propertiesPath = Path.Combine(folders.SystemFolder, "Properties.txt");
            if (File.Exists(propertiesPath))
            {
                List<string> existingPlatforms = ReadProjectPlatforms(File.ReadAllLines(propertiesPath, Encoding.Default).ToList());
                if (existingPlatforms.Count > 0)
                {
                    return existingPlatforms;
                }
            }

            return GetSoundBankPlatforms(soundBanks);
        }

        private static List<string> GetSoundBankPlatforms(List<SoundBankReadResult> soundBanks)
        {
            List<string> platforms = soundBanks
                .Select(soundBank => GetProjectPlatformName(soundBank.Header.Platform))
                .Where(platform => !string.IsNullOrWhiteSpace(platform))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(PlatformProjectOrder)
                .ToList();

            if (platforms.Count == 0)
            {
                platforms.Add("PC");
            }

            return platforms;
        }

        private static List<string> ReadProjectPlatforms(List<string> lines)
        {
            List<string> values = ReadSection(lines, "#AvailableFormats");
            if (values.Count == 0 || !int.TryParse(values[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int count))
            {
                return new List<string> { "PC" };
            }

            return values
                .Skip(1)
                .Take(count)
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .ToList();
        }

        private static int PlatformProjectOrder(string platform)
        {
            if (platform.Equals("PlayStation2", StringComparison.OrdinalIgnoreCase))
            {
                return 0;
            }
            if (platform.Equals("Xbox", StringComparison.OrdinalIgnoreCase))
            {
                return 1;
            }
            if (platform.Equals("GameCube", StringComparison.OrdinalIgnoreCase) ||
                platform.Equals("Wii", StringComparison.OrdinalIgnoreCase))
            {
                return 2;
            }
            if (platform.Equals("PC", StringComparison.OrdinalIgnoreCase))
            {
                return 3;
            }

            return 99;
        }

        private static string GetProjectPlatformName(string platform)
        {
            if (string.IsNullOrWhiteSpace(platform))
            {
                return "PC";
            }
            if (platform.IndexOf("PS2", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "PlayStation2";
            }
            if (platform.IndexOf("XB", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "Xbox";
            }
            if (platform.IndexOf("X Box", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "Xbox";
            }
            if (EuroSoundCodecMatrix.IsGameCubePlatform(platform))
            {
                return platform.IndexOf("Wii", StringComparison.OrdinalIgnoreCase) >= 0 ? "Wii" : "GameCube";
            }

            return "PC";
        }

        private static List<SampleRateProfile> BuildSampleRateProfiles(List<string> platforms, IEnumerable<ProjectSampleEntry> samples)
        {
            List<ProjectSampleEntry> sampleList = samples.ToList();
            Dictionary<string, List<int>> frequenciesByPlatform = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);
            foreach (string platform in platforms)
            {
                List<int> platformFrequencies = sampleList
                    .Select(sample => sample.GetFrequency(platform))
                    .Where(frequency => frequency > 0)
                    .Distinct()
                    .OrderBy(frequency => frequency)
                    .ToList();

                if (platformFrequencies.Count == 0)
                {
                    platformFrequencies.Add(44100);
                }

                frequenciesByPlatform[platform] = platformFrequencies;
            }

            string referencePlatform = platforms
                .OrderByDescending(platform => frequenciesByPlatform[platform].Count)
                .ThenBy(PlatformProjectOrder)
                .FirstOrDefault() ?? "PC";
            List<int> referenceFrequencies = frequenciesByPlatform[referencePlatform];

            List<SampleRateProfile> profiles = new List<SampleRateProfile>();
            for (int i = 0; i < referenceFrequencies.Count; i++)
            {
                int referenceFrequency = referenceFrequencies[i];
                Dictionary<string, int> frequencies = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                foreach (string platform in platforms)
                {
                    frequencies[platform] = string.Equals(platform, referencePlatform, StringComparison.OrdinalIgnoreCase)
                        ? referenceFrequency
                        : GetMappedSampleRate(sampleList, referencePlatform, referenceFrequency, platform, frequenciesByPlatform[platform]);
                }

                profiles.Add(new SampleRateProfile
                {
                    Label = GetSampleRateProfileName(i, referenceFrequencies.Count, referenceFrequency),
                    Frequencies = frequencies,
                    ReferencePlatform = referencePlatform
                });
            }

            return profiles;
        }

        private static string GetSampleRateProfileLabel(List<SampleRateProfile> profiles, ProjectSampleEntry sample)
        {
            string referencePlatform = profiles
                .Select(profile => profile.ReferencePlatform)
                .FirstOrDefault(platform => !string.IsNullOrWhiteSpace(platform));

            if (!string.IsNullOrWhiteSpace(referencePlatform) && sample.PlatformFrequencies.ContainsKey(referencePlatform))
            {
                int referenceFrequency = sample.GetFrequency(referencePlatform);
                SampleRateProfile referenceMatch = profiles.FirstOrDefault(profile => profile.GetFrequency(referencePlatform) == referenceFrequency);
                if (referenceMatch != null)
                {
                    return referenceMatch.Label;
                }
            }

            foreach (string platform in sample.PlatformFrequencies.Keys.OrderBy(PlatformProjectOrder))
            {
                int frequency = sample.GetFrequency(platform);
                List<SampleRateProfile> matches = profiles
                    .Where(profile => profile.GetFrequency(platform) == frequency)
                    .ToList();
                if (matches.Count == 1)
                {
                    return matches[0].Label;
                }
            }

            foreach (string platform in sample.PlatformFrequencies.Keys.OrderBy(PlatformProjectOrder))
            {
                int frequency = sample.GetFrequency(platform);
                SampleRateProfile match = profiles.FirstOrDefault(profile => profile.GetFrequency(platform) == frequency);
                if (match != null)
                {
                    return match.Label;
                }
            }

            int fallbackFrequency = sample.Frequency > 0 ? sample.Frequency : 44100;
            SampleRateProfile fallbackMatch = profiles.FirstOrDefault(profile => profile.Frequencies.Values.Contains(fallbackFrequency));
            return fallbackMatch != null
                ? fallbackMatch.Label
                : (profiles.Count > 0 ? profiles[0].Label : "Q_Medium");
        }

        private static int GetMappedSampleRate(List<ProjectSampleEntry> samples, string referencePlatform, int referenceFrequency, string platform, List<int> platformFrequencies)
        {
            List<int> matchingFrequencies = samples
                .Where(sample => sample.PlatformFrequencies.ContainsKey(referencePlatform) && sample.GetFrequency(referencePlatform) == referenceFrequency && sample.PlatformFrequencies.ContainsKey(platform))
                .Select(sample => sample.GetFrequency(platform))
                .Where(frequency => frequency > 0)
                .ToList();

            if (matchingFrequencies.Count > 0)
            {
                return matchingFrequencies
                    .GroupBy(frequency => frequency)
                    .OrderByDescending(group => group.Count())
                    .ThenBy(group => Math.Abs(group.Key - referenceFrequency))
                    .First()
                    .Key;
            }

            return platformFrequencies
                .OrderBy(frequency => Math.Abs(frequency - referenceFrequency))
                .ThenBy(frequency => frequency)
                .FirstOrDefault();
        }

        private static string GetSampleRateProfileName(int index, int count, int frequency)
        {
            string[] names;
            if (count == 1)
            {
                names = new[] { "Q_Med" };
            }
            else if (count == 2)
            {
                names = new[] { "Q_Low", "Q_High" };
            }
            else if (count == 3)
            {
                names = new[] { "Q_Low", "Q_Med", "Q_High" };
            }
            else if (count == 4)
            {
                names = new[] { "Q_Low", "Q_MedL", "Q_MedH", "Q_High" };
            }
            else
            {
                names = new[]
                {
                    "Q_VVVLow",
                    "Q_VVLow",
                    "Q_VLow",
                    "Q_MLow",
                    "Q_Low",
                    "Q_MedL",
                    "Q_Med",
                    "Q_MedH",
                    "Q_High",
                    "Q_VHigh",
                    "Q_Max"
                };
            }

            string baseName = index < names.Length
                ? names[index]
                : "Q_Max" + (index - names.Length + 2).ToString(CultureInfo.InvariantCulture);

            return baseName + "_(" + FormatSampleRateName(frequency) + ")";
        }

        private static string FormatSampleRateName(int frequency)
        {
            int roundedKhz = (int)Math.Round(frequency / 1000m, MidpointRounding.AwayFromZero);
            return roundedKhz.ToString(CultureInfo.InvariantCulture) + "k";
        }

        private static int GetDefaultSampleRate(string platform)
        {
            if (platform.Equals("PlayStation2", StringComparison.OrdinalIgnoreCase))
            {
                return 15000;
            }
            if (platform.Equals("Xbox", StringComparison.OrdinalIgnoreCase))
            {
                return 44100;
            }
            if (platform.Equals("GameCube", StringComparison.OrdinalIgnoreCase) ||
                platform.Equals("Wii", StringComparison.OrdinalIgnoreCase) ||
                platform.Equals("PC", StringComparison.OrdinalIgnoreCase))
            {
                return 22050;
            }

            return 22050;
        }

        private static string NormalizeSampleRelativePath(string relativePath)
        {
            string clean = (relativePath ?? string.Empty).Trim().TrimStart('\\', '/').Replace('/', '\\');
            return "\\" + clean;
        }

        private static string GetOrCreateSubfolder(string rootFolder, string name)
        {
            string existing = Directory.GetDirectories(rootFolder)
                .FirstOrDefault(path => string.Equals(Path.GetFileName(path), name, StringComparison.OrdinalIgnoreCase));
            string folder = existing ?? Path.Combine(rootFolder, name);
            EnsureDirectoryExists(folder);
            return folder;
        }

        private static void EnsureDirectoryExists(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                return;
            }

            if (Directory.Exists(folder))
            {
                return;
            }

            try
            {
                Directory.CreateDirectory(folder);
            }
            catch
            {
                // The exporter may run over an existing project tree. If Windows says the
                // folder already exists, keep going and let the actual file write decide.
                return;
            }
        }

        private static bool IsSupportedVersion(int version)
        {
            return version == 4 || version == 5 || version == 6;
        }

        private static int PlatformRank(string platform)
        {
            if (platform == null)
            {
                return 99;
            }
            if (platform.IndexOf("PC", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return 0;
            }
            if (platform.IndexOf("PS2", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return 1;
            }
            if (platform.IndexOf("XB", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return 2;
            }
            if (EuroSoundCodecMatrix.IsGameCubePlatform(platform))
            {
                return 3;
            }

            return 50;
        }

        private static List<CompiledFileCandidate> FindCompiledFileCandidates(string compiledFolder, Title selectedTitle)
        {
            List<string> searchFolders = FindCompiledAudioFolders(compiledFolder);
            Dictionary<string, CompiledFileCandidate> candidates = new Dictionary<string, CompiledFileCandidate>(StringComparer.OrdinalIgnoreCase);
            SoundBankReader reader = new SoundBankReader();

            foreach (string searchFolder in searchFolders)
            {
                foreach (string filePath in Directory.GetFiles(searchFolder, "*", SearchOption.AllDirectories))
                {
                    if (!IsCompiledAudioFile(filePath))
                    {
                        continue;
                    }

                    if (!candidates.ContainsKey(filePath))
                    {
                        SfxCommonHeader commonHeader;
                        FileType fileType;
                        try
                        {
                            string platform = InferPlatformFromPath(filePath);
                            commonHeader = reader.ReadCommonHeader(filePath, platform);
                            fileType = IsSupportedVersion(commonHeader.FileVersion)
                                ? GetCompiledFileType(commonHeader, filePath, selectedTitle)
                                : FileType.Unknown;
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                        candidates.Add(filePath, new CompiledFileCandidate
                        {
                            FilePath = filePath,
                            Platform = commonHeader.Platform,
                            CommonHeader = commonHeader,
                            FileType = fileType
                        });
                    }
                }
            }

            return candidates.Values
                .OrderBy(item => PlatformRank(item.Platform))
                .ThenBy(item => item.FilePath, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private static List<string> FindCompiledAudioFolders(string compiledFolder)
        {
            List<string> folders = new List<string>();
            if (string.Equals(Path.GetFileName(compiledFolder), "audio", StringComparison.OrdinalIgnoreCase))
            {
                folders.Add(compiledFolder);
            }

            foreach (string folder in Directory.GetDirectories(compiledFolder, "audio", SearchOption.AllDirectories))
            {
                string normalized = folder.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                if (normalized.IndexOf(Path.DirectorySeparatorChar + "_bin_", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    folders.Add(folder);
                }
            }

            folders.Add(compiledFolder);

            return folders.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        }

        private static FileType GetCompiledFileType(SfxCommonHeader commonHeader, string filePath, Title selectedTitle)
        {
            FileType fileType = GenericMethods.GetFileType((int)commonHeader.FileHashCode, commonHeader.FileVersion, filePath, selectedTitle);
            if (fileType != FileType.Unknown || commonHeader.FileVersion < 6)
            {
                return fileType;
            }

            if (commonHeader.FileHashCode == 0x2D600000)
            {
                return FileType.MusicDetails;
            }

            switch ((commonHeader.FileHashCode & 0x00F00000) >> 20)
            {
                case 0x2:
                    return FileType.SoundDetailsFile;
                case 0x3:
                    return FileType.ProjectDetails;
                case 0x4:
                    return FileType.StreamFile;
                case 0x5:
                    return FileType.SoundbankFile;
                case 0x6:
                    return FileType.MusicFile;
                default:
                    return FileType.Unknown;
            }
        }

        private static bool IsCompiledAudioFile(string filePath)
        {
            if (string.Equals(Path.GetExtension(filePath), ".sfx", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            try
            {
                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    if (stream.Length < 4)
                    {
                        return false;
                    }

                    byte[] magic = new byte[4];
                    return stream.Read(magic, 0, magic.Length) == magic.Length
                        && magic[0] == (byte)'M'
                        && magic[1] == (byte)'U'
                        && magic[2] == (byte)'S'
                        && magic[3] == (byte)'X';
                }
            }
            catch (IOException)
            {
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        private static string InferPlatformFromPath(string filePath)
        {
            string value = filePath ?? string.Empty;
            if (value.IndexOf("_bin_ps2", StringComparison.OrdinalIgnoreCase) >= 0 || value.IndexOf("PS2", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "PS2";
            }
            if (value.IndexOf("_bin_wii", StringComparison.OrdinalIgnoreCase) >= 0 || value.IndexOf("Wii", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "Wii";
            }
            if (value.IndexOf("_bin_gc", StringComparison.OrdinalIgnoreCase) >= 0 || value.IndexOf("GC", StringComparison.OrdinalIgnoreCase) >= 0 || value.IndexOf("GameCube", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "GC";
            }
            if (value.IndexOf("_bin_xb", StringComparison.OrdinalIgnoreCase) >= 0 || value.IndexOf("XB", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "XB";
            }
            if (value.IndexOf("_bin_pc", StringComparison.OrdinalIgnoreCase) >= 0 || value.IndexOf("PC", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "PC";
            }

            return "PC";
        }

        private static string InferLanguageNameFromPath(string filePath)
        {
            string value = (filePath ?? string.Empty).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            foreach (LanguageName language in LanguageNames)
            {
                if (value.IndexOf(language.Name, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    value.IndexOf(language.ShortName, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    value.IndexOf(language.ShortName.TrimStart('_'), StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return language.Name;
                }
            }

            return "English";
        }

        private static string NormalizeName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            string normalized = value.Trim().Trim('"').Replace('\\', '/');
            int slashIndex = normalized.LastIndexOf('/');
            if (slashIndex >= 0)
            {
                normalized = normalized.Substring(slashIndex + 1);
            }
            if (normalized.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                normalized = normalized.Substring(0, normalized.Length - 4);
            }

            return normalized.Trim();
        }

        private static string SanitizeFileName(string value)
        {
            string name = NormalizeName(value);
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(invalidChar, '_');
            }

            return string.IsNullOrWhiteSpace(name) ? "Unnamed" : name;
        }

        private static string GetPlatformSuffix(string platform)
        {
            if (string.IsNullOrWhiteSpace(platform))
            {
                return "Unknown";
            }

            string value = platform.Trim();
            if (value.IndexOf("PS2", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "PS2";
            }
            if (value.IndexOf("XB", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "XB";
            }
            if (value.IndexOf("Wii", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "Wii";
            }
            if (EuroSoundCodecMatrix.IsGameCubePlatform(value))
            {
                return "GC";
            }
            if (value.IndexOf("PC", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "PC";
            }

            return SanitizeFileName(value);
        }

        private static string Line(string key, int value)
        {
            return key + "  " + value.ToString(CultureInfo.InvariantCulture);
        }

        private static string Line(string key, uint value)
        {
            return key + "  " + value.ToString(CultureInfo.InvariantCulture);
        }

        private static uint StripSection(uint hashCode)
        {
            return hashCode & 0x00FFFFFF;
        }

        private static IEnumerable<uint> GetHashLookupKeys(uint hashCode)
        {
            yield return hashCode;

            uint stripped24 = hashCode & 0x00FFFFFF;
            if (stripped24 != hashCode)
            {
                yield return stripped24;
            }

            uint stripped16 = hashCode & 0x0000FFFF;
            if (stripped16 != hashCode && stripped16 != stripped24)
            {
                yield return stripped16;
            }

            uint stripped12 = hashCode & 0x00000FFF;
            if (stripped12 != hashCode && stripped12 != stripped24 && stripped12 != stripped16)
            {
                yield return stripped12;
            }

            foreach (uint stripped in new[] { stripped16, stripped12 })
            {
                uint sfxV6Hash = 0x1AF00000u | stripped;
                if (sfxV6Hash != hashCode)
                {
                    yield return sfxV6Hash;
                }

                uint sfxV4Hash = 0x1A000000u | stripped;
                if (sfxV4Hash != hashCode)
                {
                    yield return sfxV4Hash;
                }

                uint sfxV5Hash = 0x2D700000u | stripped;
                if (sfxV5Hash != hashCode)
                {
                    yield return sfxV5Hash;
                }

                uint soundBankHash = 0x1AE00000u | stripped;
                if (soundBankHash != hashCode)
                {
                    yield return soundBankHash;
                }
            }
        }

        private static string GetHashCodeLabel(HashcodeParser hashcodes, uint hashCode)
        {
            foreach (uint lookupKey in GetHashLookupKeys(hashCode))
            {
                string label = hashcodes.GetHashCodeLabel(lookupKey);
                if (!string.IsNullOrWhiteSpace(label) && !label.StartsWith("**", StringComparison.Ordinal))
                {
                    return label;
                }
            }

            return SfxMissingName;
        }

        private static int Flag(ushort flags, int index)
        {
            return ((flags >> index) & 1) == 1 ? 1 : 0;
        }

        private sealed class ProjectFolders
        {
            public string RootFolder { get; set; }
            public string SfxFolder { get; set; }
            public string GroupsFolder { get; set; }
            public string DuckersFolder { get; set; }
            public string SoundBanksFolder { get; set; }
            public string MusicsFolder { get; set; }
            public string SystemFolder { get; set; }
            public string MasterFolder { get; set; }
        }

        private sealed class LanguageName
        {
            public string Name { get; private set; }
            public string ShortName { get; private set; }

            public LanguageName(string name, string shortName)
            {
                Name = name;
                ShortName = shortName;
            }
        }

        private sealed class SoundBankReadResult
        {
            public string FilePath { get; set; }
            public SoundbankHeader Header { get; set; }
            public SortedDictionary<uint, Sample> Samples { get; set; }
            public List<SampleData> StoredData { get; set; }
            public string LanguageName { get; set; }
        }

        private sealed class StreamBankReadResult
        {
            public string FilePath { get; set; }
            public StreambankHeader Header { get; set; }
            public List<StreamSample> Streams { get; set; }
            public bool IsCommon { get; set; }
            public string LanguageName { get; set; }
        }

        private sealed class MusicBankReadResult
        {
            public string FilePath { get; set; }
            public StreambankHeader Header { get; set; }
            public MusicSample Music { get; set; }
            public uint HashCode { get; set; }
        }

        private sealed class ProjectDetailsReadResult
        {
            public string FilePath { get; set; }
            public ProjectDetailsHeader Header { get; set; }
            public ProjectDetails Details { get; set; }
        }

        private sealed class CompiledFileCandidate
        {
            public string FilePath { get; set; }
            public string Platform { get; set; }
            public SfxCommonHeader CommonHeader { get; set; }
            public FileType FileType { get; set; }
        }

        private sealed class ProjectSampleCatalog
        {
            private readonly Dictionary<string, ProjectSampleEntry> samples = new Dictionary<string, ProjectSampleEntry>(StringComparer.OrdinalIgnoreCase);

            public IEnumerable<ProjectSampleEntry> Samples
            {
                get { return samples.Values; }
            }

            public void Add(string relativePath, int frequency, bool isStream, string fullPath)
            {
                Add(relativePath, frequency, null, isStream, fullPath);
            }

            public void Add(string relativePath, int frequency, string platform, bool isStream, string fullPath)
            {
                if (string.IsNullOrWhiteSpace(relativePath))
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(fullPath) || !File.Exists(fullPath))
                {
                    return;
                }

                string key = relativePath.TrimStart('\\', '/').Replace('/', '\\');
                FileInfo fileInfo = new FileInfo(fullPath);

                ProjectSampleEntry entry;
                if (!samples.TryGetValue(key, out entry))
                {
                    entry = new ProjectSampleEntry
                    {
                        RelativePath = key
                    };
                    samples[key] = entry;
                }

                entry.Frequency = frequency > 0 ? frequency : 44100;
                if (!string.IsNullOrWhiteSpace(platform))
                {
                    entry.PlatformFrequencies[GetProjectPlatformName(platform)] = entry.Frequency;
                }
                entry.IsStream = entry.IsStream || isStream;
                entry.FileSizeBytes = (int)Math.Min(fileInfo.Length, int.MaxValue);
                entry.ModifiedDate = TruncateToSeconds(fileInfo.LastWriteTime);
            }

            public void AddExistingMasterFiles(string masterFolder)
            {
                if (string.IsNullOrWhiteSpace(masterFolder) || !Directory.Exists(masterFolder))
                {
                    return;
                }

                foreach (string filePath in Directory.EnumerateFiles(masterFolder, "*.wav", SearchOption.AllDirectories))
                {
                    string relativePath = filePath.Substring(masterFolder.TrimEnd('\\', '/').Length).TrimStart('\\', '/');
                    bool isStream = relativePath.StartsWith("Speech" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) ||
                        relativePath.StartsWith("Streams" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
                    Add(relativePath, 44100, isStream, filePath);
                }
            }

            private static DateTime TruncateToSeconds(DateTime value)
            {
                return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Kind);
            }
        }

        private sealed class ProjectSampleEntry
        {
            public ProjectSampleEntry()
            {
                PlatformFrequencies = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            }

            public string RelativePath { get; set; }
            public int Frequency { get; set; }
            public bool IsStream { get; set; }
            public int FileSizeBytes { get; set; }
            public DateTime ModifiedDate { get; set; }
            public Dictionary<string, int> PlatformFrequencies { get; private set; }

            public int GetFrequency(string platform)
            {
                int frequency;
                return !string.IsNullOrWhiteSpace(platform) && PlatformFrequencies.TryGetValue(platform, out frequency)
                    ? frequency
                    : (Frequency > 0 ? Frequency : 44100);
            }
        }

        private sealed class SampleRateProfile
        {
            public string Label { get; set; }
            public Dictionary<string, int> Frequencies { get; set; }
            public string ReferencePlatform { get; set; }

            public int GetFrequency(string platform)
            {
                int frequency;
                return Frequencies != null && Frequencies.TryGetValue(platform, out frequency)
                    ? frequency
                    : 44100;
            }
        }

        private sealed class SamplePoolFileNameResolver
        {
            private const int MasterSampleRate = 44100;
            private const int DefaultStreamSampleRate = 22050;
            private readonly Dictionary<string, BestSampleData> bestSamplesBySfxAndIndex = new Dictionary<string, BestSampleData>(StringComparer.OrdinalIgnoreCase);
            private readonly Dictionary<string, Dictionary<string, int>> platformFrequenciesBySfxAndRef = new Dictionary<string, Dictionary<string, int>>(StringComparer.OrdinalIgnoreCase);
            private readonly HashSet<string> writtenWavs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            private readonly List<StreamBankReadResult> streamBanks;
            private readonly string masterFolder;
            private readonly ProjectReference reference;
            private readonly HashcodeParser hashcodes;
            private readonly ProjectSampleCatalog sampleCatalog;
            private readonly bool writeWavs;

            public SamplePoolFileNameResolver(List<SoundBankReadResult> soundBanks, List<StreamBankReadResult> streamBanks, string masterFolder, ProjectReference reference, HashcodeParser hashcodes, ProjectSampleCatalog sampleCatalog, bool writeWavs = true)
            {
                this.streamBanks = streamBanks ?? new List<StreamBankReadResult>();
                this.masterFolder = masterFolder;
                this.reference = reference;
                this.hashcodes = hashcodes;
                this.sampleCatalog = sampleCatalog;
                this.writeWavs = writeWavs;

                IndexBestSamples(soundBanks);
            }

            public List<string> GetNames(Sample sample, SoundBankReadResult soundBank, string sfxName, string filePath, EuroSoundProjectDecompilerMode mode)
            {
                List<string> names = new List<string>();
                bool subSfx = Flag(sample.Flags, 10) == 1;

                for (int i = 0; i < sample.samplesList.Count; i++)
                {
                    SampleInfo sampleInfo = sample.samplesList[i];
                    if (subSfx)
                    {
                        names.Add(GetSubSfxName(sampleInfo));
                        continue;
                    }

                    string relativeWavPath = GetSampleWavName(sfxName, sampleInfo.FileRef, i);
                    if (IsStreamReference(sampleInfo.FileRef))
                    {
                        relativeWavPath = GetStreamWavName(sampleInfo.FileRef);
                        TryWriteStreamWavs(sampleInfo.FileRef, soundBank, relativeWavPath);
                        names.Add(relativeWavPath);
                        continue;
                    }

                    TryWriteBestWav(sample, i, soundBank, relativeWavPath);
                    names.Add(relativeWavPath);
                }

                return names;
            }

            private void IndexBestSamples(List<SoundBankReadResult> soundBanks)
            {
                foreach (SoundBankReadResult soundBank in soundBanks)
                {
                    foreach (Sample sample in soundBank.Samples.Values)
                    {
                        if (Flag(sample.Flags, 10) == 1)
                        {
                            continue;
                        }

                        for (int i = 0; i < sample.samplesList.Count; i++)
                        {
                            SampleInfo sampleInfo = sample.samplesList[i];
                            SampleData sampleData = GetSampleData(soundBank, sampleInfo);
                            if (sampleData == null)
                            {
                                continue;
                            }

                            string key = GetSfxSampleIndexKey(sample.HashCodeNumber, sampleInfo.FileRef);
                            AddPlatformFrequency(key, soundBank.Header.Platform, (int)sampleData.Frequency);
                            BestSampleData existingBest;
                            if (!bestSamplesBySfxAndIndex.TryGetValue(key, out existingBest) ||
                                sampleData.Frequency > existingBest.SampleData.Frequency)
                            {
                                bestSamplesBySfxAndIndex[key] = new BestSampleData
                                {
                                    SoundBank = soundBank,
                                    SampleData = sampleData
                                };
                            }
                        }
                    }
                }
            }

            private void AddPlatformFrequency(string key, string platform, int frequency)
            {
                Dictionary<string, int> frequencies;
                if (!platformFrequenciesBySfxAndRef.TryGetValue(key, out frequencies))
                {
                    frequencies = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                    platformFrequenciesBySfxAndRef[key] = frequencies;
                }

                string platformName = GetProjectPlatformName(platform);
                int existing;
                if (!frequencies.TryGetValue(platformName, out existing) || frequency > existing)
                {
                    frequencies[platformName] = frequency;
                }
            }

            private void TryWriteBestWav(Sample sample, int sampleIndex, SoundBankReadResult fallbackSoundBank, string relativeWavPath)
            {
                if (string.IsNullOrWhiteSpace(masterFolder))
                {
                    return;
                }

                string wavPath = Path.Combine(masterFolder, relativeWavPath.Replace('/', Path.DirectorySeparatorChar));
                if (!writtenWavs.Add(wavPath))
                {
                    return;
                }

                BestSampleData bestSample;
                short fileRef = sample.samplesList[sampleIndex].FileRef;
                string sampleKey = GetSfxSampleIndexKey(sample.HashCodeNumber, fileRef);
                if (!bestSamplesBySfxAndIndex.TryGetValue(sampleKey, out bestSample))
                {
                    bestSample = new BestSampleData
                    {
                        SoundBank = fallbackSoundBank,
                        SampleData = GetSampleData(fallbackSoundBank, sample.samplesList[sampleIndex])
                    };
                }

                if (bestSample.SampleData == null || bestSample.SampleData.EncodedData == null)
                {
                    return;
                }

                if (!writeWavs)
                {
                    AddSampleCatalogEntry(relativeWavPath, bestSample, sampleKey, false, wavPath);
                    return;
                }

                byte[] decodedData = DecodeSample(bestSample.SampleData, bestSample.SoundBank.Header);
                if (decodedData == null || decodedData.Length == 0)
                {
                    return;
                }

                EnsureDirectoryExists(Path.GetDirectoryName(wavPath));
                WriteMasterWav(
                    wavPath,
                    decodedData,
                    Math.Max(1, (int)bestSample.SampleData.Frequency),
                    CreateSampleLoopInfo(bestSample.SampleData, decodedData));
                AddSampleCatalogEntry(relativeWavPath, bestSample, sampleKey, false, wavPath);
            }

            private void AddSampleCatalogEntry(string relativeWavPath, BestSampleData bestSample, string sampleKey, bool isStream, string wavPath)
            {
                int frequency = Math.Max(1, (int)bestSample.SampleData.Frequency);
                sampleCatalog?.Add(relativeWavPath, frequency, bestSample.SoundBank.Header.Platform, isStream, wavPath);

                Dictionary<string, int> platformFrequencies;
                if (!platformFrequenciesBySfxAndRef.TryGetValue(sampleKey, out platformFrequencies))
                {
                    return;
                }

                foreach (KeyValuePair<string, int> platformFrequency in platformFrequencies)
                {
                    sampleCatalog?.Add(relativeWavPath, platformFrequency.Value, platformFrequency.Key, isStream, wavPath);
                }
            }

            private static string GetSampleWavName(string sfxName, short fileRef, int sampleIndex)
            {
                int sampleNumber = fileRef >= 0 ? fileRef + 1 : sampleIndex + 1;
                return SanitizeFileName(sfxName) + "_Sample" + sampleNumber.ToString(CultureInfo.InvariantCulture) + ".WAV";
            }

            private void TryWriteStreamWavs(short fileRef, SoundBankReadResult soundBank, string relativeWavPath)
            {
                if (string.IsNullOrWhiteSpace(masterFolder))
                {
                    return;
                }

                if (!writeWavs)
                {
                    sampleCatalog?.Add(relativeWavPath, DefaultStreamSampleRate, true, Path.Combine(masterFolder, relativeWavPath.Replace('/', Path.DirectorySeparatorChar)));
                    return;
                }

                if (!IsCommonStreamReference(fileRef))
                {
                    List<StreamBankReadResult> languageStreamBanks = GetLanguageStreamBanks(soundBank?.Header?.Platform, soundBank?.LanguageName);
                    foreach (StreamBankReadResult streamBank in languageStreamBanks)
                    {
                        string languagePath = GetLanguageStreamWavName(fileRef, streamBank.LanguageName);
                        TryWriteSingleStreamWav(fileRef, streamBank, languagePath);
                    }

                    if (!languageStreamBanks.Any(item => string.Equals(item.LanguageName, "English", StringComparison.OrdinalIgnoreCase)))
                    {
                        TryWriteSingleStreamWav(fileRef, languageStreamBanks.FirstOrDefault() ?? FindStreamBank(soundBank?.Header?.Platform, false, soundBank?.LanguageName), relativeWavPath);
                    }
                    else
                    {
                        sampleCatalog?.Add(relativeWavPath, DefaultStreamSampleRate, true, Path.Combine(masterFolder, relativeWavPath.Replace('/', Path.DirectorySeparatorChar)));
                    }

                    return;
                }

                StreamBankReadResult commonStreamBank = FindStreamBank(soundBank?.Header?.Platform, true, string.Empty);
                TryWriteSingleStreamWav(fileRef, commonStreamBank, relativeWavPath);
            }

            private void TryWriteSingleStreamWav(short fileRef, StreamBankReadResult streamBank, string relativeWavPath)
            {
                string wavPath = Path.Combine(masterFolder, relativeWavPath.Replace('/', Path.DirectorySeparatorChar));
                if (!writtenWavs.Add(wavPath))
                {
                    sampleCatalog?.Add(relativeWavPath, DefaultStreamSampleRate, true, wavPath);
                    return;
                }

                StreamSample streamSample = GetStreamSample(streamBank, GetStreamReferenceIndex(fileRef));
                if (streamSample == null || streamSample.EncodedData == null || streamSample.EncodedData.Length == 0)
                {
                    sampleCatalog?.Add(relativeWavPath, DefaultStreamSampleRate, true, wavPath);
                    return;
                }

                byte[] decodedData = DecodeStreamSample(streamSample, streamBank.Header);
                if (decodedData == null || decodedData.Length == 0)
                {
                    sampleCatalog?.Add(relativeWavPath, DefaultStreamSampleRate, true, wavPath);
                    return;
                }

                EnsureDirectoryExists(Path.GetDirectoryName(wavPath));
                WriteMasterWav(wavPath, decodedData, DefaultStreamSampleRate, CreateStreamLoopInfo(streamSample, decodedData));
                sampleCatalog?.Add(relativeWavPath, DefaultStreamSampleRate, true, wavPath);
            }

            private string GetSubSfxName(SampleInfo sampleInfo)
            {
                uint hashCode = unchecked((ushort)sampleInfo.FileRef);
                return GetSfxDependencyName(hashCode, hashcodes, reference);
            }

            private static SampleData GetSampleData(SoundBankReadResult soundBank, SampleInfo sampleInfo)
            {
                int dataIndex = sampleInfo.FileRef;
                return soundBank != null &&
                    soundBank.StoredData != null &&
                    dataIndex >= 0 &&
                    dataIndex < soundBank.StoredData.Count
                    ? soundBank.StoredData[dataIndex]
                    : null;
            }

            private StreamBankReadResult FindStreamBank(string platform, bool common, string languageName)
            {
                StreamBankReadResult matchingPlatform = streamBanks
                    .Where(item => item.IsCommon == common)
                    .Where(item => common || string.Equals(item.LanguageName, languageName, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault(item => string.Equals(GetPlatformSuffix(item.Header.Platform), GetPlatformSuffix(platform), StringComparison.OrdinalIgnoreCase));

                return matchingPlatform ??
                    streamBanks.FirstOrDefault(item => item.IsCommon == common && (common || string.Equals(item.LanguageName, languageName, StringComparison.OrdinalIgnoreCase))) ??
                    streamBanks.FirstOrDefault(item => item.IsCommon == common) ??
                    streamBanks.FirstOrDefault();
            }

            private List<StreamBankReadResult> GetLanguageStreamBanks(string platform, string preferredLanguageName)
            {
                List<StreamBankReadResult> matchingPlatform = streamBanks
                    .Where(item => !item.IsCommon)
                    .Where(item => string.Equals(GetPlatformSuffix(item.Header.Platform), GetPlatformSuffix(platform), StringComparison.OrdinalIgnoreCase))
                    .GroupBy(item => string.IsNullOrWhiteSpace(item.LanguageName) ? "English" : item.LanguageName, StringComparer.OrdinalIgnoreCase)
                    .Select(group => group.OrderByDescending(item => string.Equals(item.LanguageName, preferredLanguageName, StringComparison.OrdinalIgnoreCase)).First())
                    .ToList();

                if (matchingPlatform.Count > 0)
                {
                    return matchingPlatform;
                }

                return streamBanks
                    .Where(item => !item.IsCommon)
                    .GroupBy(item => string.IsNullOrWhiteSpace(item.LanguageName) ? "English" : item.LanguageName, StringComparer.OrdinalIgnoreCase)
                    .Select(group => group.OrderByDescending(item => string.Equals(item.LanguageName, preferredLanguageName, StringComparison.OrdinalIgnoreCase)).First())
                    .ToList();
            }

            private static StreamSample GetStreamSample(StreamBankReadResult streamBank, int index)
            {
                return streamBank != null &&
                    streamBank.Streams != null &&
                    index >= 0 &&
                    index < streamBank.Streams.Count
                    ? streamBank.Streams[index]
                    : null;
            }

            private static string GetStreamWavName(short fileRef)
            {
                string index = GetStreamReferenceIndex(fileRef).ToString("0000", CultureInfo.InvariantCulture);
                if (IsCommonStreamReference(fileRef))
                {
                    return Path.Combine("Streams", "Stream_Common_" + index + ".WAV");
                }

                return Path.Combine("Speech", "English", "Stream_" + index + ".WAV");
            }

            private static string GetLanguageStreamWavName(short fileRef, string languageName)
            {
                string index = GetStreamReferenceIndex(fileRef).ToString("0000", CultureInfo.InvariantCulture);
                string language = string.IsNullOrWhiteSpace(languageName) ? "English" : languageName.Trim();
                return Path.Combine("Speech", language, "Stream_" + index + ".WAV");
            }

            private static bool IsStreamReference(short fileRef)
            {
                return (((ushort)fileRef) & 0x8000) != 0;
            }

            private static bool IsCommonStreamReference(short fileRef)
            {
                return ((((ushort)fileRef) & 0x4000) == 0);
            }

            private static int GetStreamReferenceIndex(short fileRef)
            {
                return ((ushort)fileRef) & 0x3FFF;
            }

            private static string GetSfxSampleIndexKey(uint sfxHashCode, int sampleIndex)
            {
                return (sfxHashCode & 0x0000FFFF).ToString("X4", CultureInfo.InvariantCulture) + "|" + sampleIndex.ToString(CultureInfo.InvariantCulture);
            }

            private static byte[] DecodeSample(SampleData sampleData, SoundbankHeader header)
            {
                SampleData decodeData = CloneSampleData(sampleData);
                AudioFunctions audioFunctions = new AudioFunctions();
                return GenericMethods.DecodeSfxSample(decodeData, audioFunctions, header, GetPlatform(header.Platform));
            }

            private static byte[] DecodeStreamSample(StreamSample streamSample, StreambankHeader header)
            {
                AudioFunctions audioFunctions = new AudioFunctions();
                return GenericMethods.DecodeStreamSample(streamSample, audioFunctions, header, GetPlatform(header.Platform));
            }

            private static SampleData CloneSampleData(SampleData sampleData)
            {
                return new SampleData
                {
                    Flags = sampleData.Flags,
                    Address = sampleData.Address,
                    MemorySize = sampleData.MemorySize,
                    Frequency = sampleData.Frequency,
                    SampleSize = sampleData.SampleSize,
                    PsiSampleHeader = sampleData.PsiSampleHeader,
                    Channels = sampleData.Channels,
                    Bits = sampleData.Bits,
                    LoopStartOffset = sampleData.LoopStartOffset,
                    OriginalLoopOffset = sampleData.OriginalLoopOffset,
                    TotalSamples = sampleData.TotalSamples,
                    Duration = sampleData.Duration,
                    EncodedData = sampleData.EncodedData,
                    DspCoeffs = sampleData.DspCoeffs
                };
            }

            private static Platform GetPlatform(string platform)
            {
                string value = platform ?? string.Empty;
                if (EuroSoundCodecMatrix.IsGameCubePlatform(value))
                {
                    return Platform.GameCube;
                }
                if (value.IndexOf("PS2", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return Platform.PS2;
                }
                if (value.IndexOf("XB", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return Platform.Xbox;
                }

                return Platform.PC;
            }

            private static void WriteMasterWav(string wavPath, byte[] pcmData, int sourceSampleRate, WavLoopInfo sourceLoopInfo)
            {
                using (RawSourceWaveStream sourceStream = new RawSourceWaveStream(new MemoryStream(pcmData), new WaveFormat(sourceSampleRate, 16, 1)))
                {
                    ISampleProvider sampleProvider = sourceStream.ToSampleProvider();
                    if (sourceSampleRate != MasterSampleRate)
                    {
                        sampleProvider = new WdlResamplingSampleProvider(sampleProvider, MasterSampleRate);
                    }

                    EuroSoundWaveWriter.WriteSampleProvider16(wavPath, sampleProvider, ScaleLoopInfo(sourceLoopInfo, sourceSampleRate, MasterSampleRate));
                }
            }

            private static WavLoopInfo CreateSampleLoopInfo(SampleData sampleData, byte[] pcmData)
            {
                if (sampleData == null || !sampleData.IsLooped)
                {
                    return null;
                }

                return EuroSoundWaveWriter.CreateLoopInfo(true, sampleData.LoopStartOffset, pcmData == null ? 0 : pcmData.Length, 1);
            }

            private static WavLoopInfo CreateStreamLoopInfo(StreamSample streamSample, byte[] pcmData)
            {
                return EuroSoundMarkerLoopResolver.CreateLoopInfo(
                    streamSample == null ? null : streamSample.Markers,
                    (pcmData == null ? 0 : pcmData.Length) / 2,
                    MarkerLoopMode.LoopUnlessEndMarker);
            }

            private static WavLoopInfo ScaleLoopInfo(WavLoopInfo loopInfo, int sourceSampleRate, int destinationSampleRate)
            {
                if (loopInfo == null || !loopInfo.IsValid || sourceSampleRate <= 0 || destinationSampleRate <= 0)
                {
                    return null;
                }

                if (sourceSampleRate == destinationSampleRate)
                {
                    return loopInfo;
                }

                double scale = destinationSampleRate / (double)sourceSampleRate;
                return new WavLoopInfo((uint)Math.Round(loopInfo.StartSample * scale), (uint)Math.Round(loopInfo.EndSample * scale));
            }

            private sealed class BestSampleData
            {
                public SoundBankReadResult SoundBank { get; set; }
                public SampleData SampleData { get; set; }
            }
        }

        private sealed class RecoveredGroup
        {
            public int GroupHashCode { get; set; }
            public string Name { get; set; }
            public int Action1 { get; set; }
            public int MaxVoices { get; set; }
            public bool HasConflict { get; set; }
            public SortedSet<string> SfxNames { get; private set; }

            public RecoveredGroup()
            {
                SfxNames = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
            }
        }

        private sealed class RecoveredDuckerGroup
        {
            public int DuckerHashCode { get; set; }
            public string Name { get; set; }
            public int Ducker { get; set; }
            public int DuckerLenght { get; set; }
            public bool HasConflict { get; set; }
            public SortedSet<string> SfxNames { get; private set; }

            public RecoveredDuckerGroup()
            {
                SfxNames = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
            }
        }

        private sealed class ProjectReference
        {
            private readonly Dictionary<uint, string> sfxNameByHash = new Dictionary<uint, string>();
            private readonly Dictionary<uint, string> sfxPathByHash = new Dictionary<uint, string>();
            private readonly Dictionary<uint, string> soundBankNameByHash = new Dictionary<uint, string>();
            private readonly Dictionary<uint, string> soundBankPathByHash = new Dictionary<uint, string>();
            private readonly Dictionary<string, string> groupPathByName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            private readonly Dictionary<string, string> duckerPathByName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            private readonly List<GroupReference> groups = new List<GroupReference>();
            private readonly List<GroupReference> duckers = new List<GroupReference>();

            public static ProjectReference Load(string projectFolder)
            {
                ProjectReference reference = new ProjectReference();
                if (string.IsNullOrWhiteSpace(projectFolder) || !Directory.Exists(projectFolder))
                {
                    return reference;
                }

                reference.LoadSfx(projectFolder);
                reference.LoadGroups(projectFolder);
                reference.LoadDuckers(projectFolder);
                reference.LoadSoundBanks(projectFolder);
                return reference;
            }

            public string GetSfxName(uint hashCode)
            {
                return TryGetHashValue(sfxNameByHash, hashCode);
            }

            public string GetSfxPath(uint hashCode)
            {
                return TryGetHashValue(sfxPathByHash, hashCode);
            }

            public string GetGroupPath(string groupName)
            {
                string path;
                return groupPathByName.TryGetValue(groupName, out path) ? path : null;
            }

            public string GetDuckerPath(string duckerName)
            {
                return duckerPathByName.TryGetValue(duckerName, out string path) ? path : null;
            }

            public string FindGroupByDependencies(SortedSet<string> dependencies)
            {
                GroupReference match = groups.FirstOrDefault(group => group.Dependencies.SetEquals(dependencies));
                return match?.Name;
            }

            public string FindDuckerByDependencies(SortedSet<string> dependencies)
            {
                GroupReference match = duckers.FirstOrDefault(ducker => ducker.Dependencies.SetEquals(dependencies));
                return match?.Name;
            }

            public string GetSoundBankName(uint hashCode)
            {
                return TryGetHashValue(soundBankNameByHash, hashCode);
            }

            public string GetSoundBankPath(uint hashCode)
            {
                return TryGetHashValue(soundBankPathByHash, hashCode);
            }

            private void LoadSfx(string projectFolder)
            {
                string sfxFolder = FindFolder(projectFolder, "SFXs");
                if (sfxFolder == null)
                {
                    return;
                }

                foreach (string filePath in Directory.GetFiles(sfxFolder, "*.txt", SearchOption.AllDirectories))
                {
                    List<string> lines = new List<string>(File.ReadAllLines(filePath));
                    uint hashCode = ReadHashCode(lines);
                    if (hashCode == 0)
                    {
                        continue;
                    }

                    string name = NormalizeName(Path.GetFileNameWithoutExtension(filePath));
                    AddHashValue(sfxNameByHash, hashCode, name);
                    AddHashValue(sfxPathByHash, hashCode, filePath);
                }
            }

            private void LoadGroups(string projectFolder)
            {
                string groupsFolder = FindFolder(projectFolder, "Groups");
                if (groupsFolder == null)
                {
                    return;
                }

                foreach (string filePath in Directory.GetFiles(groupsFolder, "*.txt", SearchOption.AllDirectories))
                {
                    List<string> lines = new List<string>(File.ReadAllLines(filePath));
                    string name = NormalizeName(Path.GetFileNameWithoutExtension(filePath));
                    SortedSet<string> dependencies = new SortedSet<string>(ReadSection(lines, DependenciesSection).Select(NormalizeName), StringComparer.OrdinalIgnoreCase);

                    groupPathByName[name] = filePath;
                    if (dependencies.Count > 0)
                    {
                        groups.Add(new GroupReference
                        {
                            Name = name,
                            Dependencies = dependencies
                        });
                    }
                }
            }

            private void LoadDuckers(string projectFolder)
            {
                string duckersFolder = FindFolder(projectFolder, "Duckers");
                if (duckersFolder == null)
                {
                    return;
                }

                foreach (string filePath in Directory.GetFiles(duckersFolder, "*.txt", SearchOption.AllDirectories))
                {
                    List<string> lines = new List<string>(File.ReadAllLines(filePath));
                    string name = NormalizeName(Path.GetFileNameWithoutExtension(filePath));
                    SortedSet<string> dependencies = new SortedSet<string>(ReadSection(lines, DependenciesSection).Select(NormalizeName), StringComparer.OrdinalIgnoreCase);

                    duckerPathByName[name] = filePath;
                    if (dependencies.Count > 0)
                    {
                        duckers.Add(new GroupReference
                        {
                            Name = name,
                            Dependencies = dependencies
                        });
                    }
                }
            }

            private void LoadSoundBanks(string projectFolder)
            {
                string soundBanksFolder = FindFolder(projectFolder, "SoundBanks");
                if (soundBanksFolder == null)
                {
                    return;
                }

                foreach (string filePath in Directory.GetFiles(soundBanksFolder, "*.txt", SearchOption.AllDirectories))
                {
                    List<string> lines = new List<string>(File.ReadAllLines(filePath));
                    uint hashCode = ReadHashCode(lines);
                    if (hashCode == 0)
                    {
                        continue;
                    }

                    string name = NormalizeName(Path.GetFileNameWithoutExtension(filePath));
                    AddHashValue(soundBankNameByHash, hashCode, name);
                    AddHashValue(soundBankPathByHash, hashCode, filePath);
                }
            }

            private static void AddHashValue(Dictionary<uint, string> valuesByHash, uint hashCode, string value)
            {
                foreach (uint lookupKey in GetHashLookupKeys(hashCode))
                {
                    valuesByHash[lookupKey] = value;
                }
            }

            private static string TryGetHashValue(Dictionary<uint, string> valuesByHash, uint hashCode)
            {
                foreach (uint lookupKey in GetHashLookupKeys(hashCode))
                {
                    string value;
                    if (valuesByHash.TryGetValue(lookupKey, out value))
                    {
                        return value;
                    }
                }

                return null;
            }

            private static string FindFolder(string rootFolder, string name)
            {
                return Directory.GetDirectories(rootFolder)
                    .FirstOrDefault(path => string.Equals(Path.GetFileName(path), name, StringComparison.OrdinalIgnoreCase));
            }
        }

        private sealed class GroupReference
        {
            public string Name { get; set; }
            public SortedSet<string> Dependencies { get; set; }
        }
    }
}
