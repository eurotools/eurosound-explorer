using AudioDecoders;
using MusX;
using MusX.Objects;
using sb_explorer.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace sb_explorer
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
            StreambankHeader headerFileData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.musicBankHeaderData;
            MusicSample musicData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.musicData;
            propertyGrid1.SelectedObject = musicData;

            //ADPCM Validate
            if (headerFileData.FileVersion > 3 && headerFileData.FileVersion < 10)
            {
                if (headerFileData.Platform.Contains("PC") || headerFileData.Platform.Contains("GC") || headerFileData.Platform.Equals("XB__") || headerFileData.Platform.Equals("XB1_"))
                {
                    bool leftChannelStatus = audioFunctions.CheckIfEurocomImaIsInvalid(musicData.EncodedData[0]);
                    bool rightChannelStatus = audioFunctions.CheckIfEurocomImaIsInvalid(musicData.EncodedData[1]);

                    if (leftChannelStatus || rightChannelStatus)
                    {
                        textboxAdpcmStatus.Text = "ADPCM data is *INVALID*";
                        textboxAdpcmStatus.ForeColor = Color.Red;
                    }
                    else
                    {
                        textboxAdpcmStatus.Text = "ADPCM data is Valid";
                        textboxAdpcmStatus.ForeColor = SystemColors.ControlText;
                    }
                }
                else
                {
                    textboxAdpcmStatus.Text = "Cannot validate " + headerFileData.Platform.Trim('_') + " adpcm... The file format does not seem to be Eurocom ADPCM codec.";
                    textboxAdpcmStatus.ForeColor = SystemColors.ControlText;
                }
            }
            else
            {
                textboxAdpcmStatus.Text = "Cannot validate adpcm data for this version.";
                textboxAdpcmStatus.ForeColor = SystemColors.ControlText;
            }

            //Check if we have to automatically send markers
            if (ButtonAutoSendMarkers.Checked)
            {
                ButtonDisplayMusicMarkers_Click(null, EventArgs.Empty);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonSendMediaPlayer_Click(object sender, EventArgs e)
        {
            byte[] decodedDataL = null;
            byte[] decodedDataR = null;
            uint frequency = 32000;

            MusicSample musicData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.musicData;
            StreambankHeader headerFileData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.musicBankHeaderData;
            if (headerFileData.FileVersion == 201 || headerFileData.FileVersion == 1)
            {
                if (headerFileData.Platform.Equals("PC") || headerFileData.Platform.Contains("GC") || headerFileData.Platform.Contains("GameCube"))
                {
                    ImaAdpcm imaDecoder = new ImaAdpcm();
                    decodedDataL = audioFunctions.ShortArrayToByteArray(imaDecoder.Decode(musicData.EncodedData[0], musicData.EncodedData[0].Length * 2));
                    decodedDataR = audioFunctions.ShortArrayToByteArray(imaDecoder.Decode(musicData.EncodedData[1], musicData.EncodedData[1].Length * 2));
                }
                else if (headerFileData.Platform.Equals("PS2"))
                {
                    uint test = 0;
                    SonyAdpcm vagDecoder = new SonyAdpcm();
                    decodedDataL = vagDecoder.Decode(musicData.EncodedData[0], ref test);
                    decodedDataR = vagDecoder.Decode(musicData.EncodedData[1], ref test);
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
                    uint test = 0;
                    SonyAdpcm vagDecoder = new SonyAdpcm();
                    decodedDataL = vagDecoder.Decode(musicData.EncodedData[0], ref test);
                    decodedDataR = vagDecoder.Decode(musicData.EncodedData[1], ref test);
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
                    channels = 2,
                    isLooped = true,
                    startPos = (int)GetStartPosition(musicData.Markers),
                    loopStartPoint = GetStartLoopPos(musicData.Markers),
                    loopEndPoint = (int)GetEndLoopPos(musicData.Markers),
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

        //-------------------------------------------------------------------------------------------------------------------------------
        private uint GetStartPosition(Marker[] startMarkers)
        {
            uint startPosition = 0;
            for (int i = 0; i < startMarkers.Length; i++)
            {
                if (startMarkers[i].Type == 10)
                {
                    startPosition = startMarkers[i].Position / 2;
                    break;
                }
            }

            return startPosition;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private uint GetStartLoopPos(Marker[] startMarkers)
        {
            uint startPosition = 0;
            for (int i = 0; i < startMarkers.Length; i++)
            {
                if (startMarkers[i].Type == 7 || startMarkers[i].Type == 6)
                {
                    startPosition = startMarkers[i].LoopStart / 2;
                    break;
                }
            }

            return startPosition;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private uint GetEndLoopPos(Marker[] startMarkers)
        {
            uint startPosition = 0;
            for (int i = 0; i < startMarkers.Length; i++)
            {
                if (startMarkers[i].Type == 7 || startMarkers[i].Type == 6)
                {
                    startPosition = startMarkers[i].Position / 2;
                    break;
                }
            }

            return startPosition;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
