using MusX.Objects;
using sb_explorer.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace sb_explorer.Services
{
    public sealed class EuroSoundGroupExportResult
    {
        public int RecoveredGroupCount { get; set; }
        public int UpdatedGroupCount { get; set; }
        public List<string> UnmatchedGroups { get; private set; }
        public List<string> AmbiguousGroups { get; private set; }
        public List<string> ConflictingGroups { get; private set; }
        public List<string> MissingSfxNames { get; private set; }

        public EuroSoundGroupExportResult()
        {
            UnmatchedGroups = new List<string>();
            AmbiguousGroups = new List<string>();
            ConflictingGroups = new List<string>();
            MissingSfxNames = new List<string>();
        }
    }

    public static class EuroSoundGroupExporter
    {
        private const string DependenciesSection = "#DEPENDENCIES";
        private const string SfxParametersSection = "#SFXParameters";
        private const string SectionEnd = "#END";

        public static EuroSoundGroupExportResult Export(IEnumerable<Sample> samples, HashcodeParser hashcodes, string groupsFolder)
        {
            if (samples == null)
            {
                throw new ArgumentNullException("samples");
            }

            if (hashcodes == null)
            {
                throw new ArgumentNullException("hashcodes");
            }

            if (string.IsNullOrWhiteSpace(groupsFolder) || !Directory.Exists(groupsFolder))
            {
                throw new DirectoryNotFoundException("The EuroSound groups folder does not exist.");
            }

            EuroSoundGroupExportResult result = new EuroSoundGroupExportResult();
            Dictionary<int, RecoveredGroup> recoveredGroups = RecoverGroups(samples, hashcodes, result);
            List<GroupTextFile> groupFiles = LoadGroupFiles(groupsFolder);

            foreach (GroupTextFile groupFile in groupFiles)
            {
                EnsureGroupHeader(groupFile.FilePath);
            }

            result.RecoveredGroupCount = recoveredGroups.Count;

            foreach (RecoveredGroup recoveredGroup in recoveredGroups.Values.OrderBy(group => group.GroupHashCode))
            {
                if (recoveredGroup.HasConflict)
                {
                    result.ConflictingGroups.Add(FormatRecoveredGroup(recoveredGroup));
                    continue;
                }

                List<GroupTextFile> matches = groupFiles
                    .Where(groupFile => groupFile.Dependencies.SetEquals(recoveredGroup.SfxNames))
                    .ToList();

                if (matches.Count == 0)
                {
                    result.UnmatchedGroups.Add(FormatRecoveredGroup(recoveredGroup));
                    continue;
                }

                if (matches.Count > 1)
                {
                    result.AmbiguousGroups.Add(FormatRecoveredGroup(recoveredGroup) + " -> " + string.Join(", ", matches.Select(match => Path.GetFileName(match.FilePath)).ToArray()));
                    continue;
                }

                UpdateGroupParameters(matches[0].FilePath, recoveredGroup);
                result.UpdatedGroupCount++;
            }

            return result;
        }

        private static Dictionary<int, RecoveredGroup> RecoverGroups(IEnumerable<Sample> samples, HashcodeParser hashcodes, EuroSoundGroupExportResult result)
        {
            Dictionary<int, RecoveredGroup> recoveredGroups = new Dictionary<int, RecoveredGroup>();
            HashSet<uint> missingHashCodes = new HashSet<uint>();

            foreach (Sample sample in samples)
            {
                int groupHashCode = sample.GroupHashCode;
                if (groupHashCode == 0)
                {
                    continue;
                }

                string sfxName = hashcodes.GetHashCodeLabel(sample.HashCodeNumber);
                if (string.IsNullOrWhiteSpace(sfxName) || sfxName.StartsWith("**", StringComparison.Ordinal))
                {
                    if (missingHashCodes.Add(sample.HashCodeNumber))
                    {
                        result.MissingSfxNames.Add("0x" + sample.HashCodeNumber.ToString("X8", CultureInfo.InvariantCulture));
                    }
                    continue;
                }

                RecoveredGroup recoveredGroup;
                if (!recoveredGroups.TryGetValue(groupHashCode, out recoveredGroup))
                {
                    recoveredGroup = new RecoveredGroup
                    {
                        GroupHashCode = groupHashCode,
                        Action1 = Flag(sample.Flags, 14),
                        MaxVoices = sample.GroupMaxChannels < 0 ? 0 : sample.GroupMaxChannels
                    };
                    recoveredGroups.Add(groupHashCode, recoveredGroup);
                }
                else if (recoveredGroup.Action1 != Flag(sample.Flags, 14) || recoveredGroup.MaxVoices != (sample.GroupMaxChannels < 0 ? 0 : sample.GroupMaxChannels))
                {
                    recoveredGroup.HasConflict = true;
                }

                recoveredGroup.SfxNames.Add(NormalizeSfxName(sfxName));
            }

            return recoveredGroups;
        }

        private static List<GroupTextFile> LoadGroupFiles(string groupsFolder)
        {
            List<GroupTextFile> groupFiles = new List<GroupTextFile>();

            foreach (string filePath in Directory.GetFiles(groupsFolder, "*.txt", SearchOption.TopDirectoryOnly))
            {
                List<string> lines = new List<string>(File.ReadAllLines(filePath));
                SortedSet<string> dependencies = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (string dependency in ReadSection(lines, DependenciesSection))
                {
                    string normalized = NormalizeSfxName(dependency);
                    if (normalized.Length > 0)
                    {
                        dependencies.Add(normalized);
                    }
                }

                if (dependencies.Count > 0)
                {
                    groupFiles.Add(new GroupTextFile
                    {
                        FilePath = filePath,
                        Dependencies = dependencies
                    });
                }
            }

            return groupFiles;
        }

        private static IEnumerable<string> ReadSection(List<string> lines, string sectionMarker)
        {
            int startIndex = FindSectionStart(lines, sectionMarker);
            if (startIndex < 0)
            {
                yield break;
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

                yield return line;
            }
        }

        private static void UpdateGroupParameters(string filePath, RecoveredGroup group)
        {
            List<string> lines = new List<string>(File.ReadAllLines(filePath));
            EnsureGroupHeader(lines);
            ReplaceSection(lines, SfxParametersSection, new List<string>
            {
                Line("Action1", group.Action1),
                Line("MaxVoices", group.MaxVoices),
                Line("Priority", 0),
                Line("UseDistCheck", 0)
            });

            File.WriteAllLines(filePath, lines.ToArray(), Encoding.UTF8);
        }

        private static void EnsureGroupHeader(string filePath)
        {
            List<string> lines = new List<string>(File.ReadAllLines(filePath));
            if (EnsureGroupHeader(lines))
            {
                File.WriteAllLines(filePath, lines.ToArray(), Encoding.UTF8);
            }
        }

        private static bool EnsureGroupHeader(List<string> lines)
        {
            const string Header = "## EuroSound Group File";

            if (lines.Count == 0)
            {
                lines.Add(Header);
                return true;
            }

            if (string.Equals(lines[0].Trim('\uFEFF'), Header, StringComparison.Ordinal))
            {
                return false;
            }

            lines[0] = Header;
            return true;
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

        private static string NormalizeSfxName(string value)
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

        private static string FormatRecoveredGroup(RecoveredGroup group)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "Group {0} ({1} SFX, Action1={2}, MaxVoices={3})",
                group.GroupHashCode,
                group.SfxNames.Count,
                group.Action1,
                group.MaxVoices);
        }

        private static string Line(string key, int value)
        {
            return key + "  " + value.ToString(CultureInfo.InvariantCulture);
        }

        private static int Flag(ushort flags, int index)
        {
            return ((flags >> index) & 1) == 1 ? 1 : 0;
        }

        private sealed class RecoveredGroup
        {
            public int GroupHashCode { get; set; }
            public int Action1 { get; set; }
            public int MaxVoices { get; set; }
            public bool HasConflict { get; set; }
            public SortedSet<string> SfxNames { get; private set; }

            public RecoveredGroup()
            {
                SfxNames = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
            }
        }

        private sealed class GroupTextFile
        {
            public string FilePath { get; set; }
            public SortedSet<string> Dependencies { get; set; }
        }
    }
}
