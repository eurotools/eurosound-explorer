using AudioDecoders;
using EuroSoundExplorer2.Classes;
using MusX;
using MusX.Objects;
using System.IO;
using System.Windows.Forms;
using static EuroSoundExplorer2.AppConfig;
using static MusX.Readers.SfxFunctions;

namespace EuroSoundExplorer2
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
        internal static byte[] DecodeSfxSample(SampleData selectedSample, AudioFunctions audioFunctions)
        {
            SfxHeaderData headerData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.soundBankHeaderData;

            byte[] decodedData = null;
            if (headerData.FileVersion == 201 || headerData.FileVersion == 1)
            {
                Platform selectedPlatform = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected;
                switch (selectedPlatform)
                {
                    case Platform.PC:
                        decodedData = selectedSample.EncodedData;
                        break;
                    case Platform.GameCube:
                        DspAdpcm gcDecoder = new DspAdpcm();
                        decodedData = audioFunctions.ShortArrayToByteArray(gcDecoder.Decode(selectedSample.EncodedData, selectedSample.DspCoeffs));
                        break;
                    case Platform.PS2:
                        SonyAdpcm vagDecoder = new SonyAdpcm();
                        decodedData = vagDecoder.Decode(selectedSample.EncodedData);
                        break;
                    case Platform.Xbox:
                        XboxAdpcm xboxDecoder = new XboxAdpcm();
                        decodedData = audioFunctions.ShortArrayToByteArray(xboxDecoder.Decode(selectedSample.EncodedData));
                        break;
                }
            }
            else
            {
                if (headerData.Platform.Equals("PC__") || headerData.Platform.Equals("XB__") || headerData.Platform.Equals("XB1_"))
                {
                    Eurocom_ImaAdpcm eurocomDAT = new Eurocom_ImaAdpcm();
                    decodedData = audioFunctions.ShortArrayToByteArray(eurocomDAT.Decode(selectedSample.EncodedData));
                }
                else if (headerData.Platform.Equals("GC__"))
                {
                    DspAdpcm gameCubeDecoder = new DspAdpcm();
                    decodedData = audioFunctions.ShortArrayToByteArray(gameCubeDecoder.Decode(selectedSample.EncodedData, selectedSample.DspCoeffs));
                }
                else if (headerData.Platform.Equals("PS2_"))
                {
                    SonyAdpcm vagDecoder = new SonyAdpcm();
                    decodedData = vagDecoder.Decode(selectedSample.EncodedData);
                }
            }

            return decodedData;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal static byte[] DecodeStreamSample(StreamSample selectedSample, AudioFunctions audioFunctions)
        {
            SfxHeaderData headerData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.streamBankHeaderData;

            byte[] decodedData = null;
            if (headerData.FileVersion == 201 || headerData.FileVersion == 1)
            {
                Platform selectedPlatform = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected;

                if (selectedPlatform == Platform.PC || selectedPlatform == Platform.GameCube)
                {
                    ImaAdpcm imaFile = new ImaAdpcm();
                    decodedData = audioFunctions.ShortArrayToByteArray(imaFile.Decode(selectedSample.EncodedData, selectedSample.EncodedData.Length * 2));
                }
                else if (selectedPlatform == Platform.PS2)
                {
                    SonyAdpcm vagDecoder = new SonyAdpcm();
                    decodedData = vagDecoder.Decode(selectedSample.EncodedData);
                }
                else if (selectedPlatform == Platform.Xbox)
                {
                    XboxAdpcm xboxDecoder = new XboxAdpcm();
                    decodedData = audioFunctions.ShortArrayToByteArray(xboxDecoder.Decode(selectedSample.EncodedData));
                }
            }
            else
            {
                if (headerData.Platform.Equals("PC__") || headerData.Platform.Equals("XB__"))
                {
                    Eurocom_ImaAdpcm eurocomDAT = new Eurocom_ImaAdpcm();
                    decodedData = audioFunctions.ShortArrayToByteArray(eurocomDAT.Decode(selectedSample.EncodedData));
                }
                if (headerData.Platform.Equals("GC__"))
                {
                    Eurocom_ImaAdpcm eurocomDAT = new Eurocom_ImaAdpcm();
                    decodedData = audioFunctions.ShortArrayToByteArray(eurocomDAT.Decode(selectedSample.EncodedData));
                }
                else if (headerData.Platform.Equals("PS2_"))
                {
                    SonyAdpcm vagDecoder = new SonyAdpcm();
                    decodedData = vagDecoder.Decode(selectedSample.EncodedData);
                }
            }

            return decodedData;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal static int GetHashCodeWithSection(FileType fileType, int hashCode, int selectedVersion, Title selectedTitle)
        {
            if (selectedVersion == 201 && (selectedTitle == Title.Sphinx || selectedTitle == Title.Buffy))
            {
                if (fileType == FileType.Music)
                {
                    hashCode |= 0x1BE00000;
                }
            }
            else if ((selectedVersion == 1 || selectedVersion == 4 || selectedVersion == 5 || selectedVersion == 6) && (selectedTitle == Title.Athens || selectedTitle == Title.Spyro || selectedTitle == Title.Robots || selectedTitle == Title.Predator || selectedTitle == Title.BatmanBegins))
            {
                if (fileType == FileType.Music)
                {
                    hashCode = 0x1B000000 | (0x00000FFF & hashCode);
                }
                else if (fileType == FileType.SoundBank)
                {
                    hashCode = 0x1AE00000 | (0x00000FFF & hashCode);
                }
            }

            return hashCode;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal static FileType GetFileType(int hashCode, int selectedVersion, string filePath, Title selectedTitle)
        {
            if (selectedVersion == 201 && (selectedTitle == Title.Sphinx || selectedTitle == Title.Buffy))
            {
                int sectionHashCode = (hashCode & 0x00F00000) >> 20;
                if (sectionHashCode == 0xE)
                {
                    return FileType.Music;
                }
                else if (filePath.IndexOf("stream") >= 0 || hashCode == 0x0000FFFF)
                {
                    return FileType.Stream;
                }
                else if (hashCode == 0x00FFFFFF)
                {
                    return FileType.SBI;
                }
                else
                {
                    return FileType.SoundBank;
                }
            }
            else if ((selectedVersion == 1 || selectedVersion == 4 || selectedVersion == 5 || selectedVersion == 6) && (selectedTitle == Title.Athens || selectedTitle == Title.Spyro || selectedTitle == Title.Robots || selectedTitle == Title.Predator || selectedTitle == Title.BatmanBegins))
            {
                int sectionHashCode = (hashCode & 0x0000F000) >> 12;
                switch (sectionHashCode)
                {
                    case 0xF:
                        return FileType.Music;
                    case 0xE:
                        return FileType.SoundBank;
                    case 0xD:
                        return FileType.Stream;
                    case 0xB:
                        return FileType.SoundDetails;
                    case 0xA:
                        return FileType.MusicDetails;
                    case 0xC:
                        return FileType.ProjectDetails;
                }
            }
            else if (selectedVersion == 6 && selectedTitle == Title.IceAge2)
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
                        case 0x6:
                            return FileType.Music;
                        case 0x5:
                            return FileType.SoundBank;
                        case 0x4:
                            return FileType.Stream;
                        case 0x2:
                            return FileType.SoundDetails;
                        case 0x3:
                            return FileType.ProjectDetails;
                    }
                }
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
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
