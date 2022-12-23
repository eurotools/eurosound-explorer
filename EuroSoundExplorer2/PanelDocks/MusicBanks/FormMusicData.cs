using AudioDecoders;
using EuroSoundExplorer2.Classes;
using MusX;
using MusX.Objects;
using System;
using System.Drawing;
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
            SfxHeaderData headerFileData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.musicBankHeaderData;
            MusicSample musicData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.musicData;
            propertyGrid1.SelectedObject = musicData;

            //ADPCM Validate
            if ((headerFileData.Platform.Contains("PC") || headerFileData.Platform.Contains("GC")) && headerFileData.FileVersion > 3 && headerFileData.FileVersion < 10)
            {
                byte[] ImaDataLeft = musicData.EncodedData[0];
                byte[] ImaDataRight = musicData.EncodedData[1];
                if (ImaDataLeft[3] == 65 && ImaDataRight[3] == 65)
                {
                    textboxAdpcmStatus.Text = "ADPCM data is Valid";
                    textboxAdpcmStatus.ForeColor = SystemColors.ControlText;
                }
                else
                {
                    textboxAdpcmStatus.Text = "ADPCM data is *INVALID*";
                    textboxAdpcmStatus.ForeColor = Color.Red;
                }
            }
            else
            {
                textboxAdpcmStatus.Text = "Cannot validate " + headerFileData.Platform.Trim('_') + " adpcm... The file format does not seem to be Eurocom ADPCM codec.";
                textboxAdpcmStatus.ForeColor = SystemColors.ControlText;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonSendMediaPlayer_Click(object sender, EventArgs e)
        {
            byte[] decodedDataL = null;
            byte[] decodedDataR = null;
            int frequency = 32000;

            MusicSample musicData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.musicData;
            SfxHeaderData headerFileData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.musicBankHeaderData;
            if (headerFileData.FileVersion == 201 || headerFileData.FileVersion == 1)
            {
                if (headerFileData.Platform.Equals("PC") || headerFileData.Platform.Contains("GC") || headerFileData.Platform.Contains("GameCube"))
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
                else if (headerFileData.Platform.Equals("XB") || headerFileData.Platform.Equals("Xbox"))
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
        private void ButtonDisplayMusicMarkers_Click(object sender, EventArgs e)
        {
            MusicSample musicData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.musicData;
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlMarkers.ShowMarkers(musicData);
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlStartMarkers.ShowMarkers(musicData);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
