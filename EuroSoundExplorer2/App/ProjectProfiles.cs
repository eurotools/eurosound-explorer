using MusX;
using MusX.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static sb_explorer.Enumerations;

namespace sb_explorer
{
    public sealed class ProjectProfile
    {
        private readonly Dictionary<Platform, string> platformFolders = new Dictionary<Platform, string>();

        public string Name { get; set; }
        public string HashTable { get; set; }
        // Kept only while importing old Projects.ini files.
        public string LegacyFolder { get; set; }
        public Platform Platform { get; set; }
        public Title CompatibilityTitle { get; set; }

        public IDictionary<Platform, string> PlatformFolders { get { return platformFolders; } }

        public string GetFolder(Platform platform)
        {
            string folder;
            return platformFolders.TryGetValue(platform, out folder) ? folder : null;
        }

        public void SetFolder(Platform platform, string folder)
        {
            if (platform == Platform.None) return;
            if (string.IsNullOrWhiteSpace(folder)) platformFolders.Remove(platform);
            else platformFolders[platform] = folder;
        }

        public IEnumerable<Platform> DefinedPlatforms
        {
            get { return platformFolders.Where(pair => !string.IsNullOrWhiteSpace(pair.Value)).Select(pair => pair.Key).OrderBy(PlatformOrder); }
        }

        private static int PlatformOrder(Platform platform)
        {
            switch (platform)
            {
                case Platform.PC: return 0;
                case Platform.PS2: return 1;
                case Platform.PS3: return 2;
                case Platform.GameCube: return 3;
                case Platform.Wii: return 4;
                case Platform.Xbox: return 5;
                case Platform.Xbox360: return 6;
                default: return 99;
            }
        }
    }

    public sealed class ProjectProfileStore
    {
        private readonly List<ProjectProfile> profiles = new List<ProjectProfile>();
        public IList<ProjectProfile> Profiles { get { return profiles; } }

        public void Load(string path)
        {
            profiles.Clear();
            if (!File.Exists(path)) return;
            ProjectProfile current = null;
            foreach (string rawLine in File.ReadAllLines(path))
            {
                string line = rawLine.Trim();
                if (line.Length == 0 || line.StartsWith(";")) continue;
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    current = new ProjectProfile { Name = line.Substring(1, line.Length - 2), Platform = Platform.None };
                    profiles.Add(current);
                    continue;
                }
                if (current == null) continue;
                string[] pair = line.Split(new[] { '=' }, 2);
                if (pair.Length != 2) continue;
                Platform folderPlatform;
                if (TryParseFolderKey(pair[0], out folderPlatform))
                {
                    current.SetFolder(folderPlatform, DecodePath(pair[1], path));
                    continue;
                }
                switch (pair[0])
                {
                    case "Folder": current.LegacyFolder = DecodePath(pair[1], path); break;
                    case "HashTable": current.HashTable = DecodePath(pair[1], path); break;
                    case "Platform": Platform platform; if (Enum.TryParse(pair[1], true, out platform)) current.Platform = platform; break;
                    case "CompatibilityTitle": Title title; if (Enum.TryParse(pair[1], true, out title)) current.CompatibilityTitle = title; break;
                }
            }
            foreach (ProjectProfile profile in profiles) MigrateLegacyFolder(profile);
        }

        public void Save(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.WriteLine("; Editable EuroSound Explorer project profiles");
                foreach (ProjectProfile profile in profiles.Where(p => !string.IsNullOrWhiteSpace(p.Name)))
                {
                    writer.WriteLine();
                    writer.WriteLine("[{0}]", profile.Name.Replace("]", string.Empty));
                    Platform[] defined = profile.DefinedPlatforms.ToArray();
                    foreach (Platform platform in defined)
                        writer.WriteLine("{0}Folder={1}", PlatformKey(platform), EncodePath(profile.GetFolder(platform), path));
                    if (defined.Length == 0 && !string.IsNullOrWhiteSpace(profile.LegacyFolder))
                        writer.WriteLine("Folder={0}", EncodePath(profile.LegacyFolder, path));
                    writer.WriteLine("HashTable={0}", EncodePath(profile.HashTable, path));
                    writer.WriteLine("CompatibilityTitle={0}", profile.CompatibilityTitle);
                }
            }
        }

        public ProjectProfile FindForFolder(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder)) return null;
            string target = Normalize(folder);
            return profiles
                .SelectMany(p => p.PlatformFolders.Values.Where(f => !string.IsNullOrWhiteSpace(f)).Select(f => new { Profile = p, Folder = f }))
                .Where(p => IsInside(target, Normalize(p.Folder)))
                .OrderByDescending(p => Normalize(p.Folder).Length)
                .Select(p => p.Profile)
                .FirstOrDefault();
        }

        public ProjectProfile FindByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            return profiles.FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public Platform FindPlatformForFolder(ProjectProfile profile, string folder)
        {
            if (profile == null || string.IsNullOrWhiteSpace(folder)) return Platform.None;
            string target = Normalize(folder);
            return profile.PlatformFolders
                .Where(pair => !string.IsNullOrWhiteSpace(pair.Value) && IsInside(target, Normalize(pair.Value)))
                .OrderByDescending(pair => Normalize(pair.Value).Length)
                .Select(pair => pair.Key)
                .FirstOrDefault();
        }

        private static void MigrateLegacyFolder(ProjectProfile profile)
        {
            if (string.IsNullOrWhiteSpace(profile.LegacyFolder)) return;
            Platform platform = profile.Platform;
            if (platform == Platform.None) platform = DetectPlatformFromFolder(profile.LegacyFolder);
            if (platform != Platform.None && string.IsNullOrWhiteSpace(profile.GetFolder(platform))) profile.SetFolder(platform, profile.LegacyFolder);
        }

        private static Platform DetectPlatformFromFolder(string folder)
        {
            if (!Directory.Exists(folder)) return Platform.None;
            string file = Directory.GetFiles(folder, "*.sfx", SearchOption.AllDirectories).FirstOrDefault() ??
                Directory.GetFiles(folder, "*.musx", SearchOption.AllDirectories).FirstOrDefault();
            if (file == null) return Platform.None;
            try { return ProjectConfigurationDetector.ParsePlatform(new SfxFunctions().ReadCommonHeader(file, string.Empty).Platform); }
            catch (InvalidDataException) { return Platform.None; }
        }

        private static bool TryParseFolderKey(string key, out Platform platform)
        {
            platform = Platform.None;
            string value = key.EndsWith("Folder", StringComparison.OrdinalIgnoreCase) ? key.Substring(0, key.Length - 6) : key;
            if (value.Equals("GC", StringComparison.OrdinalIgnoreCase)) platform = Platform.GameCube;
            else if (value.Equals("XB", StringComparison.OrdinalIgnoreCase) || value.Equals("Xbox", StringComparison.OrdinalIgnoreCase) || key.Equals("Xboxolder", StringComparison.OrdinalIgnoreCase)) platform = Platform.Xbox;
            else if (value.Equals("XB2", StringComparison.OrdinalIgnoreCase) || value.Equals("X360", StringComparison.OrdinalIgnoreCase) || value.Equals("Xbox360", StringComparison.OrdinalIgnoreCase)) platform = Platform.Xbox360;
            else Enum.TryParse(value, true, out platform);
            return platform != Platform.None && key.EndsWith("Folder", StringComparison.OrdinalIgnoreCase) || key.Equals("Xboxolder", StringComparison.OrdinalIgnoreCase);
        }

        private static string PlatformKey(Platform platform)
        {
            if (platform == Platform.GameCube) return "GC";
            return platform.ToString();
        }

        private static bool IsInside(string target, string root)
        {
            return target.Equals(root, StringComparison.OrdinalIgnoreCase) ||
                target.StartsWith(root.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
        }

        private static string Normalize(string path) { return Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar); }
        private static string DecodePath(string value, string iniPath) { return string.IsNullOrWhiteSpace(value) || Path.IsPathRooted(value) ? value : Path.GetFullPath(Path.Combine(Path.GetDirectoryName(iniPath), value)); }
        private static string EncodePath(string value, string iniPath)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;
            string basePath = Path.GetDirectoryName(iniPath).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            string full = Path.GetFullPath(value);
            return full.StartsWith(basePath, StringComparison.OrdinalIgnoreCase) ? full.Substring(basePath.Length) : full;
        }
    }

    internal static class ProjectConfigurationDetector
    {
        internal static void Apply(AppConfig config, ProjectProfileStore store)
        {
            ProjectProfile profile = store.FindForFolder(config.ProjectFolder);
            if (profile != null)
            {
                config.ProjectTitle = profile.Name;
                config.TitleSelected = profile.CompatibilityTitle;
                // None is meaningful: it prevents a v201 project from inheriting the
                // platform selected for the previously opened project.
                Platform configured = store.FindPlatformForFolder(profile, config.ProjectFolder);
                config.PlatformSelected = configured != Platform.None ? configured : profile.Platform;
                if (!string.IsNullOrWhiteSpace(profile.HashTable)) config.SoundhFile = profile.HashTable;
            }
            else
            {
                config.ProjectTitle = Directory.Exists(config.ProjectFolder) ? new DirectoryInfo(config.ProjectFolder).Name : string.Empty;
                config.TitleSelected = Title.None;
                config.PlatformSelected = Platform.None;
                config.SoundhFile = string.Empty;
            }

            string file = FindMusX(config.ProjectFolder);
            if (file == null) return;
            SfxCommonHeader header = new SfxFunctions().ReadCommonHeader(file, config.PlatformSelected.ToString());
            config.FileVersion = header.FileVersion;
            Platform detected = ParsePlatform(header.Platform);
            if (detected != Platform.None) config.PlatformSelected = detected;
            if (string.IsNullOrWhiteSpace(config.SoundhFile)) config.SoundhFile = FindHashTable(config.ProjectFolder);
        }

        private static string FindMusX(string folder)
        {
            if (!Directory.Exists(folder)) return null;
            return Directory.GetFiles(folder, "*.sfx", SearchOption.AllDirectories).FirstOrDefault() ??
                Directory.GetFiles(folder, "*.musx", SearchOption.AllDirectories).FirstOrDefault();
        }

        private static string FindHashTable(string folder)
        {
            if (!Directory.Exists(folder)) return null;
            return Directory.GetFiles(folder, "Sound.h", SearchOption.AllDirectories).FirstOrDefault() ??
                Directory.GetFiles(folder, "AudioFileTable.h", SearchOption.AllDirectories).FirstOrDefault() ??
                Directory.GetFiles(folder, "SFX_Defines.h", SearchOption.AllDirectories).FirstOrDefault();
        }

        internal static Platform ParsePlatform(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return Platform.None;
            if (EuroSoundCodecMatrix.IsPs2Platform(value)) return Platform.PS2;
            if (EuroSoundCodecMatrix.IsPs3Platform(value)) return Platform.PS3;
            if (EuroSoundCodecMatrix.IsXbox360Platform(value)) return Platform.Xbox360;
            if (EuroSoundCodecMatrix.IsXboxPlatform(value)) return Platform.Xbox;
            if (value.IndexOf("WII", StringComparison.OrdinalIgnoreCase) >= 0) return Platform.Wii;
            if (EuroSoundCodecMatrix.IsGameCubePlatform(value)) return Platform.GameCube;
            if (EuroSoundCodecMatrix.IsPcPlatform(value)) return Platform.PC;
            return Platform.None;
        }
    }
}
