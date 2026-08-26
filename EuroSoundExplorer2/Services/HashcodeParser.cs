using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace sb_explorer.Classes
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class HashcodeParser
    {
        private readonly Dictionary<int, string> HashCodes = new Dictionary<int, string>();
        private readonly Dictionary<int, List<string>> HashCodeLabels = new Dictionary<int, List<string>>();
        public int Count { get { return HashCodes.Count; } }
        public string LastLoadedFile { get; private set; }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void LoadHashTable(string filePath)
        {
            HashCodes.Clear();
            HashCodeLabels.Clear();
            LastLoadedFile = null;
            foreach (string sourcePath in ResolveHashTableFiles(filePath))
            {
                if (LastLoadedFile == null) LastLoadedFile = sourcePath;
                using (StreamReader sr = new StreamReader(File.Open(sourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Match aftEntry = Regex.Match(line, "AFTEntry\\s*\\(\\s*(0x[\\da-fA-F]{1,8})\\s*,\\s*\"((?:\\\\.|[^\"])*)\"");
                        if (aftEntry.Success)
                        {
                            uint unsignedHash = Convert.ToUInt32(aftEntry.Groups[1].Value, 16);
                            string label = aftEntry.Groups[2].Value.Replace("\\\\", "\\").Replace("\\\"", "\"");
                            AddHashCode(unchecked((int)unsignedHash), label);
                            continue;
                        }

                        string pattern = "#define([\\s])+([\\w]+)([\\s])+(0x[\\da-fA-F]{8,8})";
                        MatchCollection matchCollection = Regex.Matches(line, pattern);
                        if (matchCollection.Count > 0)
                        {
                            for (int i = 0; i < matchCollection.Count; i++)
                            {
                                line = matchCollection[i].ToString().Replace("#define", string.Empty);
                                Match match2 = Regex.Match(line, "(0x[\\da-fA-F]{8,8})");
                                int hashCode = Convert.ToInt32(match2.ToString().Trim(), 16);
                                //Remove HT_Sound prefix
                                string hashcodeMatch = Regex.Match(line, "([\\w]+)").ToString().Replace("HT_Sound_", string.Empty).Trim();

                                AddHashCode(hashCode, hashcodeMatch);
                            }
                        }
                    }
                }
            }
        }

        private static IEnumerable<string> ResolveHashTableFiles(string filePath)
        {
            List<string> files = new List<string>();
            if (File.Exists(filePath)) files.Add(Path.GetFullPath(filePath));

            string directory = string.IsNullOrWhiteSpace(filePath) ? null : Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && Directory.Exists(directory))
            {
                string name = Path.GetFileName(filePath) ?? string.Empty;
                if (!File.Exists(filePath) || name.Equals("SFX_Defines.h", StringComparison.OrdinalIgnoreCase) ||
                    name.Equals("MFX_Defines.h", StringComparison.OrdinalIgnoreCase))
                {
                    string sfx = Path.Combine(directory, "SFX_Defines.h");
                    string mfx = Path.Combine(directory, "MFX_Defines.h");
                    if (File.Exists(sfx)) files.Add(Path.GetFullPath(sfx));
                    if (File.Exists(mfx)) files.Add(Path.GetFullPath(mfx));
                }
            }
            return files.Distinct(StringComparer.OrdinalIgnoreCase);
        }

        private void AddHashCode(int hashCode, string label)
        {
            if (string.IsNullOrWhiteSpace(label)) return;
            List<string> labels;
            if (!HashCodeLabels.TryGetValue(hashCode, out labels))
            {
                labels = new List<string>();
                HashCodeLabels.Add(hashCode, labels);
            }
            if (!labels.Contains(label)) labels.Add(label);
            if (!HashCodes.ContainsKey(hashCode)) HashCodes.Add(hashCode, label);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public string GetHashCodeLabel(uint hashCode)
        {
            string label = "**HashCode Not Found**";
            if (HashCodes.ContainsKey((int)hashCode))
            {
                label = HashCodes[(int)hashCode];
            }
            else
            {
                uint engineXHashCode = 0x2D000000u | (hashCode & 0x00FFFFFFu);
                if (HashCodes.ContainsKey(unchecked((int)engineXHashCode)))
                {
                    label = HashCodes[unchecked((int)engineXHashCode)];
                }
            }

            return label;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public bool HashcodeIsListed(uint hashCode)
        {
            if (HashCodes.ContainsKey((int)hashCode)) return true;
            uint engineXHashCode = 0x2D000000u | (hashCode & 0x00FFFFFFu);
            return HashCodes.ContainsKey(unchecked((int)engineXHashCode));
        }

        public string GetMusicHashCodeLabel(uint hashCode)
        {
            List<string> labels;
            if (!HashCodeLabels.TryGetValue((int)hashCode, out labels))
            {
                return "**HashCode Not Found**";
            }

            foreach (string label in labels)
            {
                if (label.StartsWith("MFX_", StringComparison.OrdinalIgnoreCase) ||
                    label.StartsWith("_mus_mfx_", StringComparison.OrdinalIgnoreCase) ||
                    label.StartsWith("mus_mfx_", StringComparison.OrdinalIgnoreCase))
                {
                    return label;
                }
            }

            return "**HashCode Not Found**";
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
