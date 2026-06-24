using AudioDecoders;
using MusX;
using MusX.Objects;
using sb_explorer.Classes;
using sb_explorer.Services.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static MusX.Readers.SfxFunctions;
using static sb_explorer.Enumerations;
using static System.Windows.Forms.ListViewItem;

namespace sb_explorer
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    internal static class GenericMethods
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        internal static string GetFileSize(string filename)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = new FileInfo(filename).Length;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            return string.Format("{0:0.##} {1}", len, sizes[order]);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal static byte[] DecodeSfxSample(SampleData selectedSample, AudioFunctions audioFunctions, SoundbankHeader headerData, Platform selectedPlatform)
        {
            EuroSoundAudioCodec codec = EuroSoundCodecMatrix.GetCodec(headerData.FileVersion, headerData.Platform, EuroSoundBankType.SoundBank);
            return EuroSoundAudioDecoder.Decode(codec, selectedSample.EncodedData, audioFunctions, selectedSample.DspCoeffs, selectedSample);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal static byte[] DecodeStreamSample(StreamSample selectedSample, AudioFunctions audioFunctions, StreambankHeader headerData, Platform selectedPlatform)
        {
            EuroSoundAudioCodec codec = EuroSoundCodecMatrix.GetCodec(headerData.FileVersion, headerData.Platform, EuroSoundBankType.StreamBank);
            return EuroSoundAudioDecoder.Decode(codec, selectedSample.EncodedData, audioFunctions, null, null);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal static byte[] DecodeMusicChannel(byte[] encodedData, AudioFunctions audioFunctions, StreambankHeader headerData)
        {
            EuroSoundAudioCodec codec = EuroSoundCodecMatrix.GetCodec(headerData.FileVersion, headerData.Platform, EuroSoundBankType.MusicBank);
            return EuroSoundAudioDecoder.Decode(codec, encodedData, audioFunctions, null, null);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal static int GetHashCodeWithSection(FileType fileType, int hashCode, int selectedVersion, Title selectedTitle)
        {
            if (selectedVersion == 201 && (selectedTitle == Title.Sphinx || selectedTitle == Title.Buffy))
            {
                if (fileType == FileType.MusicFile)
                {
                    hashCode |= 0x1BE00000;
                }
            }
            else if ((selectedVersion == 1 || selectedVersion == 4 || selectedVersion == 5 || selectedVersion == 6) && (selectedTitle == Title.Athens || selectedTitle == Title.Spyro || selectedTitle == Title.Robots || selectedTitle == Title.Predator || selectedTitle == Title.BatmanBegins))
            {
                if (fileType == FileType.MusicFile)
                {
                    hashCode = 0x1B000000 | (0x00000FFF & hashCode);
                }
                else if (fileType == FileType.SoundbankFile)
                {
                    hashCode = 0x1AE00000 | (0x00000FFF & hashCode);
                }
            }

            return hashCode;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal static FileType GetFileType(int hashCode, int selectedVersion, string filePath, Title selectedTitle)
        {
            if (selectedVersion == 6 && Path.GetFileName(filePath).StartsWith("__musicmarkers", StringComparison.OrdinalIgnoreCase))
            {
                return FileType.MusicMarkers;
            }

            if (selectedVersion == 201)
            {
                int sectionHashCode = (hashCode & 0x00F00000) >> 20;
                if (sectionHashCode == 0xE)
                {
                    return FileType.MusicFile;
                }
                else if (filePath.IndexOf("stream") >= 0 || hashCode == 0x0000FFFF)
                {
                    return FileType.StreamFile;
                }
                else if (hashCode == 0x00FFFFFF)
                {
                    return FileType.SBI;
                }
                else
                {
                    return FileType.SoundbankFile;
                }
            }
            else if ((selectedVersion == 1 || selectedVersion == 4 || selectedVersion == 5 || selectedVersion == 6) &&
                selectedTitle != Title.IceAge2 &&
                selectedTitle != Title.DemoX)
            {
                int sectionHashCode = (hashCode & 0x0000F000) >> 12;
                switch (sectionHashCode)
                {
                    case 0xA:
                        return FileType.MusicDetails;
                    case 0xB:
                        return FileType.SoundDetailsFile;
                    case 0xC:
                        return FileType.ProjectDetails;
                    case 0xD:
                        return FileType.StreamFile;
                    case 0xE:
                        return FileType.SoundbankFile;
                    case 0xF:
                        return FileType.MusicFile;
                }
            }
            else if (selectedVersion >= 6)
            {
                int sectionHashCode = (hashCode & 0x00F00000) >> 20;
                if (hashCode == 0x2D600000)
                {
                    return FileType.MusicDetails;
                }
                else
                {
                    switch (sectionHashCode)
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
                    }
                }
            }
            else
            {

            }
            return FileType.Unknown;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal static string GetFinalPath(string outputFilePath)
        {
            int i = 1;
            string finalPath = outputFilePath;
            while (File.Exists(finalPath))
            {
                finalPath = Path.Combine(Path.GetDirectoryName(outputFilePath), string.Format("{0} ({1}){2}", Path.GetFileNameWithoutExtension(outputFilePath), i, Path.GetExtension(outputFilePath)));
                i++;
            }

            return finalPath;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal static void FilterListView(string filterText, ListView lvwFiles)
        {
            lvwFiles.BeginUpdate();
            foreach (ListViewItem itemToCheck in lvwFiles.Items)
            {
                bool RemoveSubSFX = true;

                //Look for matches in the subitems
                foreach (ListViewSubItem subItem in itemToCheck.SubItems)
                {
                    if (subItem.Text.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        RemoveSubSFX = false;
                        break;
                    }
                }

                //Check if we have to remove this item
                if (RemoveSubSFX)
                {
                    itemToCheck.Remove();
                }
            }
            lvwFiles.EndUpdate();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal static void FilterTree(string filterText, TreeView treeView1)
        {
            //.Reverse() is required here to iterate backward because the collections
            //are modified when removing nodes. You can call .ToList() instead to 
            //iterate forward.
            treeView1.BeginUpdate();
            foreach (TreeNode node in GetAllNodes(treeView1.Nodes).Reverse())
            {
                if (node.Tag != null && node.Text.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    if (node.Parent is null)
                    {
                        treeView1.Nodes.Remove(node);
                    }
                    else
                    {
                        node.Parent.Nodes.Remove(node);
                    }
                }
            }
            treeView1.EndUpdate();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static IEnumerable<TreeNode> GetAllNodes(TreeNodeCollection Nodes)
        {
            foreach (TreeNode tn in Nodes)
            {
                yield return tn;
                foreach (TreeNode child in GetAllNodes(tn.Nodes))
                {
                    yield return child;
                }
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
