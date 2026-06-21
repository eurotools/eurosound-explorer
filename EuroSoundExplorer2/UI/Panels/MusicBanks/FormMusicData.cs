using AudioDecoders;
using MusX;
using MusX.Objects;
using sb_explorer.Classes;
using sb_explorer.Services.Audio;
using System;
using System.Drawing;
using System.IO;
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
            StreambankHeader headerFileData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.MusicBankHeaderData;
            MusicSample musicData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.MusicData;
            propertyGrid1.SelectedObject = musicData;

            //ADPCM Validate
            EuroSoundAudioCodec musicCodec = EuroSoundCodecMatrix.GetCodec(headerFileData.FileVersion, headerFileData.Platform, EuroSoundBankType.MusicBank);
            if (musicCodec == EuroSoundAudioCodec.EurocomImaAdpcm)
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

            //Check if we have to automatically send markers
            if (ButtonAutoSendMarkers.Checked)
            {
                ButtonDisplayMusicMarkers_Click(null, EventArgs.Empty);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonSendMediaPlayer_Click(object sender, EventArgs e)
        {
            MusicSample musicData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.MusicData;
            StreambankHeader headerFileData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.MusicBankHeaderData;
            uint frequency = GetMusicFrequency(headerFileData);
            byte[] decodedDataL = GenericMethods.DecodeMusicChannel(musicData.EncodedData[0], audioFunctions, headerFileData);
            byte[] decodedDataR = GenericMethods.DecodeMusicChannel(musicData.EncodedData[1], audioFunctions, headerFileData);

            //Show data
            if (decodedDataL != null && decodedDataR != null)
            {
                SoundFile soundToPlay = new SoundFile
                {
                    PcmData = new byte[2][] { decodedDataL, decodedDataR },
                    volume = musicData.BaseVolume / 100.0f,
                    sampleRate = frequency,
                    channels = 2,
                    isLooped = EuroSoundMarkerLoopResolver.IsLooped(musicData.Markers, MarkerLoopMode.RequireLoopMarker),
                    startPos = (int)EuroSoundMarkerLoopResolver.GetStartPosition(musicData.Markers),
                    loopStartPoint = EuroSoundMarkerLoopResolver.GetLoopStart(musicData.Markers),
                    loopEndPoint = (int)EuroSoundMarkerLoopResolver.GetLoopEnd(musicData.Markers),
                };

                ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlMediaPlayer.LoadSoundData(soundToPlay);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonSaveWav_Click(object sender, EventArgs e)
        {
            MusicSample musicData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.MusicData;
            StreambankHeader headerFileData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.MusicBankHeaderData;
            if (musicData == null || headerFileData == null)
            {
                return;
            }

            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            byte[] decodedDataL = GenericMethods.DecodeMusicChannel(musicData.EncodedData[0], audioFunctions, headerFileData);
            byte[] decodedDataR = GenericMethods.DecodeMusicChannel(musicData.EncodedData[1], audioFunctions, headerFileData);
            if (decodedDataL == null || decodedDataR == null)
            {
                return;
            }

            string filePath = GenericMethods.GetFinalPath(Path.Combine(folderBrowserDialog1.SelectedPath, "music.wav"));
            EuroSoundWaveWriter.WriteStereoPcm16(
                filePath,
                decodedDataL,
                decodedDataR,
                (int)GetMusicFrequency(headerFileData),
                EuroSoundMarkerLoopResolver.CreateLoopInfo(
                    musicData.Markers,
                    Math.Min(decodedDataL.Length, decodedDataR.Length) / 2,
                    MarkerLoopMode.RequireLoopMarker));
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonDisplayMusicMarkers_Click(object sender, EventArgs e)
        {
            MusicSample musicData = ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlSoundBankFiles.MusicData;
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlMarkers.ShowMarkers(musicData);
            ((FrmMain)Application.OpenForms[nameof(FrmMain)]).pnlStartMarkers.ShowMarkers(musicData);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static uint GetMusicFrequency(StreambankHeader headerFileData)
        {
            return EuroSoundCodecMatrix.IsXboxPlatform(headerFileData.Platform) ||
                EuroSoundCodecMatrix.IsXbox360Platform(headerFileData.Platform) ? 44100u : 32000u;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
