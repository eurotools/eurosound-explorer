using MusX.Objects;
using NAudio.Wave;
using sb_explorer.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace sb_explorer.Services
{
    public enum EuroSoundSfxTextSection
    {
        Parameters,
        SamplePoolFiles,
        SamplePoolModes,
        SamplePoolControl
    }

    public sealed class EuroSoundSfxTextExportResult
    {
        public int ExportedCount { get; set; }
        public int CreatedCount { get; set; }
        public int UpdatedCount { get; set; }
    }

    public sealed class EuroSoundSfxRadiusData
    {
        public short InnerRadius { get; set; }
        public short OuterRadius { get; set; }
        public int DurationCentiseconds { get; set; }
    }

    public static class EuroSoundSfxTextExporter
    {
        private const string ParametersSection = "#SFXParameters";
        private const string SamplePoolFilesSection = "#SFXSamplePoolFiles";
        private const string SamplePoolModesSection = "#SFXSamplePoolModes";
        private const string SamplePoolControlSection = "#SFXSamplePoolControl";
        private const string HashCodeSection = "#HASHCODE";
        private const string SectionEnd = "#END";

        public static EuroSoundSfxTextExportResult Export(
            IEnumerable<Sample> samples,
            int fileVersion,
            HashcodeParser hashcodes,
            string outputFolder,
            IEnumerable<EuroSoundSfxTextSection> sections,
            IDictionary<uint, EuroSoundSfxRadiusData> soundDetailsRadii = null,
            string masterSamplesFolder = null,
            IList<string> samplePoolFileNames = null,
            bool protectSamplePoolModeCount = false)
        {
            EuroSoundVersion version = EuroSoundVersions.FromFileVersion(fileVersion);
            EuroSoundSfxTextExportResult result = new EuroSoundSfxTextExportResult();
            List<EuroSoundSfxTextSection> sectionsToExport = new List<EuroSoundSfxTextSection>(sections);

            foreach (Sample sample in samples)
            {
                string filePath = GetOutputFilePath(outputFolder, hashcodes, sample.HashCodeNumber);
                bool fileExists = File.Exists(filePath);
                List<string> lines = fileExists
                    ? new List<string>(File.ReadAllLines(filePath))
                    : CreateEmptyLines(version, sample.HashCodeNumber);
                List<string> originalLines = fileExists ? new List<string>(lines) : null;

                foreach (EuroSoundSfxTextSection section in sectionsToExport)
                {
                    if (section == EuroSoundSfxTextSection.SamplePoolModes && protectSamplePoolModeCount && originalLines != null && !CanReplaceSamplePoolModes(originalLines, sample))
                    {
                        continue;
                    }

                    ReplaceSection(lines, GetSectionMarker(section), BuildSectionLines(section, version, sample, soundDetailsRadii, masterSamplesFolder, fileExists ? lines : null, samplePoolFileNames));
                }
                if (!fileExists)
                {
                    UpdateModifiedHeader(lines);
                }
                File.WriteAllLines(filePath, lines.ToArray(), Encoding.UTF8);

                result.ExportedCount++;
                if (fileExists)
                {
                    result.UpdatedCount++;
                }
                else
                {
                    result.CreatedCount++;
                }
            }

            return result;
        }

        public static void ExportToFile(
            Sample sample,
            int fileVersion,
            string filePath,
            IEnumerable<EuroSoundSfxTextSection> sections,
            IDictionary<uint, EuroSoundSfxRadiusData> soundDetailsRadii = null,
            string masterSamplesFolder = null,
            IList<string> samplePoolFileNames = null,
            bool protectSamplePoolModeCount = false)
        {
            EuroSoundVersion version = EuroSoundVersions.FromFileVersion(fileVersion);
            bool fileExists = File.Exists(filePath);
            List<string> lines = fileExists
                ? new List<string>(File.ReadAllLines(filePath))
                : CreateEmptyLines(version, sample.HashCodeNumber);
            List<string> originalLines = fileExists ? new List<string>(lines) : null;

            foreach (EuroSoundSfxTextSection section in sections)
            {
                if (section == EuroSoundSfxTextSection.SamplePoolModes && protectSamplePoolModeCount && originalLines != null && !CanReplaceSamplePoolModes(originalLines, sample))
                {
                    continue;
                }

                ReplaceSection(lines, GetSectionMarker(section), BuildSectionLines(section, version, sample, soundDetailsRadii, masterSamplesFolder, fileExists ? lines : null, samplePoolFileNames));
            }

            if (!fileExists)
            {
                UpdateModifiedHeader(lines);
            }
            EnsureDirectoryExists(Path.GetDirectoryName(filePath));
            File.WriteAllLines(filePath, lines.ToArray(), Encoding.UTF8);
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
                return;
            }
        }

        public static string GetSectionMarker(EuroSoundSfxTextSection section)
        {
            switch (section)
            {
                case EuroSoundSfxTextSection.Parameters:
                    return ParametersSection;
                case EuroSoundSfxTextSection.SamplePoolFiles:
                    return SamplePoolFilesSection;
                case EuroSoundSfxTextSection.SamplePoolModes:
                    return SamplePoolModesSection;
                case EuroSoundSfxTextSection.SamplePoolControl:
                    return SamplePoolControlSection;
                default:
                    throw new ArgumentOutOfRangeException("section");
            }
        }

        private static List<string> BuildSectionLines(EuroSoundSfxTextSection section, EuroSoundVersion version, Sample sample, IDictionary<uint, EuroSoundSfxRadiusData> soundDetailsRadii, string masterSamplesFolder, List<string> existingLines, IList<string> samplePoolFileNames)
        {
            switch (section)
            {
                case EuroSoundSfxTextSection.Parameters:
                    return BuildParametersLines(version, sample, soundDetailsRadii, masterSamplesFolder, existingLines);
                case EuroSoundSfxTextSection.SamplePoolFiles:
                    return BuildSamplePoolFilesLines(sample, existingLines, samplePoolFileNames);
                case EuroSoundSfxTextSection.SamplePoolModes:
                    return BuildSamplePoolModesLines(sample);
                case EuroSoundSfxTextSection.SamplePoolControl:
                    return BuildSamplePoolControlLines(sample);
                default:
                    throw new ArgumentOutOfRangeException("section");
            }
        }

        private static List<string> BuildParametersLines(EuroSoundVersion version, Sample sample, IDictionary<uint, EuroSoundSfxRadiusData> soundDetailsRadii, string masterSamplesFolder, List<string> existingLines)
        {
            EuroSoundSfxRadiusData radiusData = version == EuroSoundVersion.EuroSound357 ? null : GetRadiusData(sample.HashCodeNumber, soundDetailsRadii);
            int duckerLength = GetOriginalDuckerLength(sample, radiusData, masterSamplesFolder, existingLines);
            List<string> lines = new List<string>
            {
                Line("ReverbSend", sample.ReverbSend)
            };
            if (version == EuroSoundVersion.EuroSound510 || version == EuroSoundVersion.EuroSound610)
            {
                lines.Add(Line("DopplerSend", sample.DopplerValue));
            }
            lines.Add(Line("TrackingType", sample.TrackingType));
            lines.Add(Line("InnerRadius", radiusData == null ? sample.InnerRadius : radiusData.InnerRadius));
            lines.Add(Line("OuterRadius", radiusData == null ? sample.OuterRadius : radiusData.OuterRadius));
            lines.Add(Line("MaxVoices", sample.MaxVoices));
            lines.Add(Line("Action1", Flag(sample.Flags, 0)));
            lines.Add(Line("Priority", sample.Priority));
            lines.Add(Line("Group", 0));
            lines.Add(Line("Action2", Flag(sample.Flags, version == EuroSoundVersion.EuroSound357 ? 1 : 3)));
            lines.Add(Line("Alertness", sample.UserValue));

            if (version == EuroSoundVersion.EuroSound357)
            {
                lines.Add(Line("IgnoreAge", Flag(sample.Flags, 2)));
                lines.Add(Line("Ducker", sample.Ducker));
                lines.Add(Line("DuckerLenght", duckerLength));
                lines.Add(Line("MasterVolume", sample.MasterVolume));
                lines.Add(Line("Outdoors", Flag(sample.Flags, 8)));
                lines.Add(Line("PauseInNis", Flag(sample.Flags, 9)));
                lines.Add(Line("StealOnAge", Flag(sample.Flags, 11)));
                lines.Add(Line("MusicType", Flag(sample.Flags, 12)));
                lines.Add(Line("Doppler", Flag(sample.Flags, 1)));
                return lines;
            }

            lines.Add(Line("IgnoreMasterVolume", Flag(sample.Flags, 2)));
            lines.Add(Line("Ducker", sample.Ducker));
            lines.Add(Line("DuckerLenght", duckerLength));
            lines.Add(Line("MasterVolume", sample.MasterVolume));
            lines.Add(Line("UnderWater", Flag(sample.Flags, 8)));
            lines.Add(Line("PauseInstant", Flag(sample.Flags, 9)));
            lines.Add(Line("StealOnAge", Flag(sample.Flags, 11)));
            lines.Add(Line("ThreatLikeMusic", Flag(sample.Flags, 12)));
            lines.Add(Line("UnPausable", Flag(sample.Flags, 1)));
            lines.Add(Line("KillMeOwnGroup", Flag(sample.Flags, 13)));
            lines.Add(Line("OneInstancePerFrame", Flag(sample.Flags, 15)));

            if (version == EuroSoundVersion.EuroSound510 || version == EuroSoundVersion.EuroSound610)
            {
                lines.Add(Line("UserFlags", sample.UserFlags));
            }

            return lines;
        }

        private static List<string> BuildSamplePoolFilesLines(Sample sample, List<string> existingLines, IList<string> samplePoolFileNames)
        {
            if (samplePoolFileNames != null)
            {
                return new List<string>(samplePoolFileNames);
            }

            if (existingLines != null)
            {
                List<string> existingNames = GetExistingSamplePoolFileNames(existingLines);
                if (existingNames.Count > 0)
                {
                    return existingNames;
                }
            }

            List<string> lines = new List<string>();
            for (int i = 0; i < sample.samplesList.Count; i++)
            {
                lines.Add("Sample" + (i + 1).ToString(CultureInfo.InvariantCulture));
            }

            return lines;
        }

        private static List<string> BuildSamplePoolModesLines(Sample sample)
        {
            List<string> lines = new List<string>();
            foreach (SampleInfo sampleInfo in sample.samplesList)
            {
                lines.Add(Line("BaseVolume", sampleInfo.Volume));
                lines.Add(Line("PitchOffset", sampleInfo.Pitch));
                lines.Add(Line("RandomPitchOffset", sampleInfo.PitchOffset));
                lines.Add(Line("RandomVolumeOffset", sampleInfo.VolumeOffset));
                lines.Add(Line("Pan", sampleInfo.Pan));
                lines.Add(Line("RandomPan", sampleInfo.PanOffset));
            }

            return lines;
        }

        private static List<string> BuildSamplePoolControlLines(Sample sample)
        {
            return new List<string>
            {
                Line("Action1", Flag(sample.Flags, 3)),
                Line("RandomPick", Flag(sample.Flags, 4)),
                Line("Shuffled", Flag(sample.Flags, 5)),
                Line("Loop", Flag(sample.Flags, 6)),
                Line("Polyphonic", Flag(sample.Flags, 7)),
                Line("MinDelay", sample.MinDelay),
                Line("MaxDelay", sample.MaxDelay),
                Line("EnableSubSFX", Flag(sample.Flags, 10)),
                Line("EnableStereo", 0)
            };
        }

        private static List<string> CreateEmptyLines(EuroSoundVersion version, uint hashCode)
        {
            DateTime now = DateTime.Now;
            string timestamp = now.ToString("MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            string userName = string.IsNullOrEmpty(Environment.UserName) ? "Unknown" : Environment.UserName;
            List<string> lines = new List<string>
            {
                "## EuroSound SFX File",
                "## First Created ... " + timestamp,
                "## Created By ... " + userName,
                "## Last Modified ... " + timestamp,
                "## Last Modified By ... " + userName,
                "",
                ParametersSection
            };

            lines.AddRange(BuildParametersDefaults(version));
            lines.Add(SectionEnd);
            lines.Add("");
            lines.Add(SamplePoolFilesSection);
            lines.Add(SectionEnd);
            lines.Add("");
            lines.Add(SamplePoolModesSection);
            lines.Add(SectionEnd);
            lines.Add("");
            lines.Add(SamplePoolControlSection);
            lines.AddRange(BuildSamplePoolControlDefaults());
            lines.Add(SectionEnd);
            lines.Add("");
            lines.Add(HashCodeSection);
            lines.Add("HashCodeNumber " + StripSection(hashCode).ToString(CultureInfo.InvariantCulture));
            lines.Add(SectionEnd);

            return lines;
        }

        private static List<string> BuildParametersDefaults(EuroSoundVersion version)
        {
            Sample sample = new Sample();
            return BuildParametersLines(version, sample, null, null, null);
        }

        private static List<string> BuildSamplePoolControlDefaults()
        {
            return BuildSamplePoolControlLines(new Sample());
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

        private static bool CanReplaceSamplePoolModes(List<string> existingLines, Sample sample)
        {
            int samplePoolFileCount = GetExistingSamplePoolFileNames(existingLines).Count;
            return samplePoolFileCount == sample.samplesList.Count;
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

        private static void UpdateModifiedHeader(List<string> lines)
        {
            string timestamp = DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            string userName = string.IsNullOrEmpty(Environment.UserName) ? "Unknown" : Environment.UserName;
            ReplaceHeaderLine(lines, "## Last Modified ... ", timestamp);
            ReplaceHeaderLine(lines, "## Last Modified By ... ", userName);
        }

        private static void ReplaceHeaderLine(List<string> lines, string prefix, string value)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    lines[i] = prefix + value;
                    return;
                }
            }
        }

        private static string GetOutputFilePath(string outputFolder, HashcodeParser hashcodes, uint hashCode)
        {
            string label = hashcodes == null ? string.Empty : hashcodes.GetHashCodeLabel(hashCode);
            if (string.IsNullOrWhiteSpace(label) || label.StartsWith("**", StringComparison.Ordinal))
            {
                label = "SFX_" + hashCode.ToString("X8", CultureInfo.InvariantCulture);
            }

            foreach (char invalidChar in Path.GetInvalidFileNameChars())
            {
                label = label.Replace(invalidChar, '_');
            }

            return Path.Combine(outputFolder, label + ".txt");
        }

        private static int Flag(ushort flags, int index)
        {
            return ((flags >> index) & 1) == 1 ? 1 : 0;
        }

        private static uint StripSection(uint hashCode)
        {
            return hashCode & 0x00FFFFFF;
        }

        private static EuroSoundSfxRadiusData GetRadiusData(uint hashCode, IDictionary<uint, EuroSoundSfxRadiusData> soundDetailsRadii)
        {
            if (soundDetailsRadii == null)
            {
                return null;
            }

            EuroSoundSfxRadiusData radiusData;
            if (soundDetailsRadii.TryGetValue(hashCode, out radiusData))
            {
                return radiusData;
            }

            if (soundDetailsRadii.TryGetValue(StripSection(hashCode), out radiusData))
            {
                return radiusData;
            }

            return null;
        }

        private static int GetOriginalDuckerLength(Sample sample, EuroSoundSfxRadiusData radiusData, string masterSamplesFolder, List<string> existingLines)
        {
            if (sample.Ducker <= 0)
            {
                return sample.DuckerLenght;
            }

            if (!string.IsNullOrWhiteSpace(masterSamplesFolder) && existingLines != null)
            {
                List<string> sampleNames = GetExistingSamplePoolFileNames(existingLines);
                if (sampleNames.Count > 0)
                {
                    decimal totalCentiseconds;
                    if (TryGetTotalSampleCentiseconds(masterSamplesFolder, sampleNames, out totalCentiseconds))
                    {
                        return sample.DuckerLenght - (int)totalCentiseconds;
                    }
                }
            }

            if (radiusData != null && radiusData.DurationCentiseconds > 0)
            {
                return sample.DuckerLenght - radiusData.DurationCentiseconds;
            }

            return sample.DuckerLenght;
        }

        private static List<string> GetExistingSamplePoolFileNames(List<string> lines)
        {
            List<string> sampleNames = new List<string>();
            int startIndex = FindSectionStart(lines, SamplePoolFilesSection);
            if (startIndex < 0)
            {
                return sampleNames;
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

                string[] parts = line.Split((char[])null, 2, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0)
                {
                    continue;
                }

                string sampleName = parts.Length == 1 ? parts[0] : parts[1];
                sampleName = sampleName.Trim().Trim('"');
                if (sampleName.Length > 0)
                {
                    sampleNames.Add(sampleName);
                }
            }

            return sampleNames;
        }

        private static bool TryGetTotalSampleCentiseconds(string masterSamplesFolder, List<string> sampleNames, out decimal totalCentiseconds)
        {
            totalCentiseconds = 0;

            foreach (string sampleName in sampleNames)
            {
                string wavPath = GetMasterWavPath(masterSamplesFolder, sampleName);
                if (!File.Exists(wavPath))
                {
                    return false;
                }

                decimal centiseconds;
                if (!TryGetWavCentiseconds(wavPath, out centiseconds))
                {
                    return false;
                }

                totalCentiseconds += centiseconds;
            }

            return true;
        }

        private static string GetMasterWavPath(string masterSamplesFolder, string sampleName)
        {
            string relativePath = sampleName.Replace('/', Path.DirectorySeparatorChar);
            if (string.IsNullOrEmpty(Path.GetExtension(relativePath)))
            {
                relativePath += ".wav";
            }

            return Path.Combine(masterSamplesFolder, relativePath);
        }

        private static bool TryGetWavCentiseconds(string wavPath, out decimal centiseconds)
        {
            centiseconds = 0;

            try
            {
                using (WaveFileReader reader = new WaveFileReader(wavPath))
                {
                    int blockAlign = reader.WaveFormat.BlockAlign;
                    if (reader.WaveFormat.SampleRate <= 0 || blockAlign <= 0)
                    {
                        return false;
                    }

                    decimal totalSeconds = reader.Length / (decimal)(reader.WaveFormat.SampleRate * blockAlign);
                    centiseconds = RoundHalfUp(totalSeconds * 100);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static decimal RoundHalfUp(decimal value)
        {
            return Math.Floor(value + 0.5m);
        }

        private static string Line(string name, object value)
        {
            IFormattable formattable = value as IFormattable;
            string text = formattable == null
                ? Convert.ToString(value, CultureInfo.InvariantCulture)
                : formattable.ToString(null, CultureInfo.InvariantCulture);
            return name + "  " + text;
        }
    }
}
