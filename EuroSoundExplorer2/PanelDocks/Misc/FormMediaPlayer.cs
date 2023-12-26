using NAudio.Wave;
using sb_explorer.Classes;
using System;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace sb_explorer
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class FormMediaPlayer : DockContent
    {
        private readonly AudioFunctions audioFunctions = new AudioFunctions();
        private WaveOut _waveOut;
        private SoundFile soundToPlay;
        private RawSourceWaveStream providerLeft, providerRight;

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
                userControl_WaveViewer1.WaveStream = new RawSourceWaveStream(new MemoryStream(soundToPlay.PcmData[0]), new WaveFormat((int)soundToPlay.sampleRate, 16, 1));
                userControl_WaveViewer1.InitControl();

                //Right Channel
                userControl_WaveViewer2.WaveStream = new RawSourceWaveStream(new MemoryStream(soundToPlay.PcmData[1]), new WaveFormat((int)soundToPlay.sampleRate, 16, 1));
                userControl_WaveViewer2.InitControl();
            }
            else
            {
                splitContainer1.Panel2Collapsed = true;
                userControl_WaveViewer1.WaveStream = new RawSourceWaveStream(new MemoryStream(soundToPlay.PcmData[0]), new WaveFormat((int)soundToPlay.sampleRate, 16, 1));
                userControl_WaveViewer1.InitControl();
            }

            //Check if sound has to play
            if (ButtonAutoPlay.Checked)
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
        private void ButtonPause_Click(object sender, EventArgs e)
        {
            if (_waveOut != null)
            {
                if (_waveOut.PlaybackState == PlaybackState.Playing)
                {
                    _waveOut.Pause();
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            //Show dialog
            if (soundToPlay != null)
            {
                DialogResult saveFileDialog = SaveFileDlg_SaveFile.ShowDialog();
                if (saveFileDialog == DialogResult.OK)
                {
                    string filePath = SaveFileDlg_SaveFile.FileName;
                    try
                    {
                        //Stop current sound to avoid bugs.
                        StopSound();

                        //Save Data
                        if (soundToPlay.channels > 1)
                        {

                            WaveFileWriter.CreateWaveFile16(filePath, audioFunctions.CreateStereoWav(ref providerLeft, ref providerRight, soundToPlay.PcmData, soundToPlay).ToSampleProvider());
                        }
                        else
                        {
                            WaveFileWriter.CreateWaveFile16(filePath, audioFunctions.CreateMonoWav(ref providerLeft, soundToPlay.PcmData[0], soundToPlay).ToSampleProvider());
                        }
                        MessageBox.Show("File saved successfully!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------
        //  TRACKBAR
        //-------------------------------------------------------------------------------------------
        private void TrackBarPosition_Scroll(object sender, EventArgs e)
        {
            if (_waveOut != null)
            {
                TimeSpan streamPos = TimeSpan.FromSeconds(providerLeft.TotalTime.TotalSeconds * trackBarPosition.Value / 100.0);
                if (soundToPlay.channels > 1)
                {
                    providerRight.CurrentTime = streamPos;
                }
                providerLeft.CurrentTime = streamPos;
            }
        }

        //-------------------------------------------------------------------------------------------
        //  FUNCTIONS
        //-------------------------------------------------------------------------------------------
        private void PlaySound()
        {
            if (WaveOut.DeviceCount > 0 && soundToPlay != null)
            {
                if (_waveOut != null && _waveOut.PlaybackState == PlaybackState.Paused)
                {
                    _waveOut.Play();
                }
                else
                {
                    //Create a new instance and start playing
                    StopSound();
                    _waveOut = new WaveOut();
                    IWaveProvider waveDataProv;
                    if (soundToPlay.channels > 1)
                    {
                        waveDataProv = audioFunctions.CreateStereoLoopWav(ref providerLeft, ref providerRight, soundToPlay.PcmData, soundToPlay);
                    }
                    else
                    {
                        waveDataProv = audioFunctions.CreateMonoLoopWav(ref providerLeft, soundToPlay.PcmData[0], soundToPlay);
                    }
                    labelTotalTime.Text = string.Format("{0:00}:{1:00}", (int)providerLeft.TotalTime.TotalMinutes, providerLeft.TotalTime.Seconds);
                    _waveOut.Init(waveDataProv);
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

        //-------------------------------------------------------------------------------------------------------------------------------
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (_waveOut != null && providerLeft != null)
            {
                TimeSpan currentTime = (_waveOut.PlaybackState == PlaybackState.Stopped) ? TimeSpan.Zero : providerLeft.CurrentTime;
                trackBarPosition.Value = Math.Min(trackBarPosition.Maximum, (int)(100 * currentTime.TotalSeconds / providerLeft.TotalTime.TotalSeconds));
                labelCurrentTime.Text = string.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);
            }
            else
            {
                trackBarPosition.Value = 0;
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
