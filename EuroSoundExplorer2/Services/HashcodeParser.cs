using System;
using System.Collections.Generic;
using System.IO;
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

        //-------------------------------------------------------------------------------------------------------------------------------
        public void LoadHashTable(string filePath)
        {
            if (File.Exists(filePath))
            {
                //Clear dictionary before adding a new hashtable
                HashCodes.Clear();
                HashCodeLabels.Clear();

                //Read new hashtable
                using (StreamReader sr = new StreamReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
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

                                List<string> labels;
                                if (!HashCodeLabels.TryGetValue(hashCode, out labels))
                                {
                                    labels = new List<string>();
                                    HashCodeLabels.Add(hashCode, labels);
                                }
                                if (!labels.Contains(hashcodeMatch))
                                {
                                    labels.Add(hashcodeMatch);
                                }

                                if (!HashCodes.ContainsKey(hashCode))
                                {
                                    //Add HashCode
                                    HashCodes.Add(hashCode, hashcodeMatch);
                                }
                            }
                        }
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public string GetHashCodeLabel(uint hashCode)
        {
            string label = "**HashCode Not Found**";
            if (HashCodes.ContainsKey((int)hashCode))
            {
                label = HashCodes[(int)hashCode];
            }

            return label;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public bool HashcodeIsListed(uint hashCode)
        {
            return HashCodes.ContainsKey((int)hashCode);
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
