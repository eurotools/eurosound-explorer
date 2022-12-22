using AudioDecoders;
using EuroSoundExplorer2.Classes;
using MusX;
using MusX.Objects;
using System.IO;
using System.Windows.Forms;

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
                AppConfig.Platform selectedPlatform = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected;
                switch (selectedPlatform)
                {
                    case AppConfig.Platform.PC:
                        decodedData = selectedSample.EncodedData;
                        break;
                    case AppConfig.Platform.GameCube:
                        DspAdpcm gcDecoder = new DspAdpcm();
                        decodedData = audioFunctions.ShortArrayToByteArray(gcDecoder.Decode(selectedSample.EncodedData, selectedSample.DspCoeffs));
                        break;
                    case AppConfig.Platform.PS2:
                        SonyAdpcm vagDecoder = new SonyAdpcm();
                        decodedData = vagDecoder.Decode(selectedSample.EncodedData);
                        break;
                    case AppConfig.Platform.Xbox:
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
                AppConfig.Platform selectedPlatform = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).configuration.PlatformSelected;

                if (selectedPlatform == AppConfig.Platform.PC || selectedPlatform == AppConfig.Platform.GameCube)
                {
                    ImaAdpcm imaFile = new ImaAdpcm();
                    decodedData = audioFunctions.ShortArrayToByteArray(imaFile.Decode(selectedSample.EncodedData, selectedSample.EncodedData.Length * 2));
                }
                else if (selectedPlatform == AppConfig.Platform.PS2)
                {
                    SonyAdpcm vagDecoder = new SonyAdpcm();
                    decodedData = vagDecoder.Decode(selectedSample.EncodedData);
                }
                else if (selectedPlatform == AppConfig.Platform.Xbox)
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
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
