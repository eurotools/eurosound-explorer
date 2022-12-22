using AudioDecoders;
using EuroSoundExplorer2.Classes;
using MusX;
using MusX.Objects;
using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace EuroSoundExplorer2
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FormMusicData : DockContent
    {
        private readonly AudioFunctions audioFunctions = new AudioFunctions();

        //-------------------------------------------------------------------------------------------------------------------------------
        public FormMusicData()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ShowMusicData()
        {
            MusicSample musicData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.musicData;
            propertyGrid1.SelectedObject = musicData;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonMediaPlayer_Click(object sender, EventArgs e)
        {
            byte[] decodedDataL = null;
            byte[] decodedDataR = null;
            int frequency = 32000;

            MusicSample musicData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.musicData;
            SfxHeaderData headerFileData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.musicBankHeaderData;
            if (headerFileData.FileVersion == 201 || headerFileData.FileVersion == 1)
            {
                if (headerFileData.Platform.Equals("PC") || headerFileData.Platform.Contains("GC"))
                {
                    ImaAdpcm eurocomDAT = new ImaAdpcm();
                    decodedDataL = audioFunctions.ShortArrayToByteArray(eurocomDAT.Decode(musicData.EncodedData[0], musicData.EncodedData[0].Length * 2));
                    decodedDataR = audioFunctions.ShortArrayToByteArray(eurocomDAT.Decode(musicData.EncodedData[1], musicData.EncodedData[1].Length * 2));
                }
                else if (headerFileData.Platform.Equals("PS2"))
                {
                    SonyAdpcm vagDecoder = new SonyAdpcm();
                    decodedDataL = vagDecoder.Decode(musicData.EncodedData[0]);
                    decodedDataR = vagDecoder.Decode(musicData.EncodedData[1]);
                }
                else if (headerFileData.Platform.Equals("XB"))
                {
                    frequency = 44100;
                    XboxAdpcm xboxDecoder = new XboxAdpcm();
                    decodedDataL = audioFunctions.ShortArrayToByteArray(xboxDecoder.Decode(musicData.EncodedData[0]));
                    decodedDataR = audioFunctions.ShortArrayToByteArray(xboxDecoder.Decode(musicData.EncodedData[1]));
                }
            }
            else
            {
                if (headerFileData.Platform.Equals("PC__") || headerFileData.Platform.Contains("GC__"))
                {
                    Eurocom_ImaAdpcm eurocomDAT = new Eurocom_ImaAdpcm();
                    decodedDataL = audioFunctions.ShortArrayToByteArray(eurocomDAT.Decode(musicData.EncodedData[0]));
                    decodedDataR = audioFunctions.ShortArrayToByteArray(eurocomDAT.Decode(musicData.EncodedData[1]));
                }
                else if (headerFileData.Platform.Equals("PS2_"))
                {
                    SonyAdpcm vagDecoder = new SonyAdpcm();
                    decodedDataL = vagDecoder.Decode(musicData.EncodedData[0]);
                    decodedDataR = vagDecoder.Decode(musicData.EncodedData[1]);
                }
                else if (headerFileData.Platform.Equals("XB__") || headerFileData.Platform.Equals("XB1_"))
                {
                    frequency = 44100;
                    Eurocom_ImaAdpcm xboxDecoder = new Eurocom_ImaAdpcm();
                    decodedDataL = audioFunctions.ShortArrayToByteArray(xboxDecoder.Decode(musicData.EncodedData[0]));
                    decodedDataR = audioFunctions.ShortArrayToByteArray(xboxDecoder.Decode(musicData.EncodedData[1]));
                }
            }

            //Show data
            if (decodedDataL != null && decodedDataR != null)
            {
                SoundFile soundToPlay = new SoundFile
                {
                    PcmData = new byte[2][] { decodedDataL, decodedDataR },
                    volume = musicData.BaseVolume / 100.0f,
                    sampleRate = frequency,
                    channels = 2
                };

                ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlMediaPlayer.LoadSoundData(soundToPlay);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonDisplayMarkers_Click(object sender, EventArgs e)
        {
            MusicSample musicData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.musicData;
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlMarkers.ShowMarkers(musicData);
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlStartMarkers.ShowMarkers(musicData);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
