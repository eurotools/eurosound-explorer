using EuroSoundExplorer2.Classes;
using NAudio.Wave;
using System;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace EuroSoundExplorer2
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FormMediaPlayer : DockContent
    {
        private readonly AudioFunctions audioFunctions = new AudioFunctions();
        private WaveOut _waveOut;
        private SoundFile soundToPlay;

        //-------------------------------------------------------------------------------------------
        //  MAIN FORM
        //-------------------------------------------------------------------------------------------
        public FormMediaPlayer()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void FormMediaPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopSound();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void LoadSoundData(SoundFile soundData)
        {
            //Stop previous instances
            StopSound();

            //Assign Values
            soundToPlay = soundData;

            //Draw waves
            if (soundToPlay.channels > 1)
            {
                splitContainer1.Panel2Collapsed = false;

                //Left Channel
                userControl_WaveViewer1.WaveStream = new RawSourceWaveStream(new MemoryStream(soundToPlay.PcmData[0]), new WaveFormat(soundToPlay.sampleRate, 16, 1));
                userControl_WaveViewer1.InitControl();

                //Right Channel
                userControl_WaveViewer2.WaveStream = new RawSourceWaveStream(new MemoryStream(soundToPlay.PcmData[1]), new WaveFormat(soundToPlay.sampleRate, 16, 1));
                userControl_WaveViewer2.InitControl();
            }
            else
            {
                splitContainer1.Panel2Collapsed = true;
                userControl_WaveViewer1.WaveStream = new RawSourceWaveStream(new MemoryStream(soundToPlay.PcmData[0]), new WaveFormat(soundToPlay.sampleRate, 16, 1));
                userControl_WaveViewer1.InitControl();
            }

            //Check if sound has to play
            if (chbxAutoPlay.Checked)
            {
                PlaySound();
            }
        }

        //-------------------------------------------------------------------------------------------
        //  TOOLBAR
        //-------------------------------------------------------------------------------------------
        private void ButtonPlay_Click(object sender, EventArgs e)
        {
            PlaySound();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonStop_Click(object sender, EventArgs e)
        {
            StopSound();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            //Show dialog
            DialogResult saveFileDialog = SaveFileDlg_SaveFile.ShowDialog();
            if (saveFileDialog == DialogResult.OK)
            {
                string filePath = SaveFileDlg_SaveFile.FileName;
                try
                {
                    if (soundToPlay.channels > 1)
                    {
                        WaveFileWriter.CreateWaveFile16(filePath, audioFunctions.CreateStereoWav(soundToPlay.PcmData, soundToPlay).ToSampleProvider());
                    }
                    else
                    {
                        WaveFileWriter.CreateWaveFile16(filePath, audioFunctions.CreateMonoWav(soundToPlay.PcmData[0], soundToPlay).ToSampleProvider());
                    }
                    MessageBox.Show("File saved successfully!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //-------------------------------------------------------------------------------------------
        //  FUNCTIONS
        //-------------------------------------------------------------------------------------------
        private void PlaySound()
        {
            if (WaveOut.DeviceCount > 0)
            {
                //Create a new instance and start playing
                StopSound();
                _waveOut = new WaveOut();
                if (soundToPlay.channels > 1)
                {
                    _waveOut.Init(audioFunctions.CreateStereoLoopWav(soundToPlay.PcmData, soundToPlay));
                    _waveOut.Play();
                }
                else
                {
                    _waveOut.Init(audioFunctions.CreateMonoLoopWav(soundToPlay.PcmData[0], soundToPlay));
                    _waveOut.Play();
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void StopSound()
        {
            if (_waveOut != null)
            {
                _waveOut.Stop();
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
