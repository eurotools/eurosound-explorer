using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EuroSoundExplorer2.Classes
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class HashcodeParser
    {
        private readonly Dictionary<int, string> HashCodes = new Dictionary<int, string>();

        //-------------------------------------------------------------------------------------------------------------------------------
        public void LoadHashTable()
        {
            string filePath = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.SoundhFile;
            if (File.Exists(filePath))
            {
                //Clear dictionary before adding a new hashtable
                HashCodes.Clear();

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
                                if (!HashCodes.ContainsKey(hashCode))
                                {
                                    //Remove HT_Sound prefix
                                    string hashcodeMatch = Regex.Match(line, "([\\w]+)").ToString().Replace("HT_Sound_", string.Empty);

                                    //Add HashCode
                                    HashCodes.Add(hashCode, hashcodeMatch.Trim());
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
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
