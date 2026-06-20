using NAudio.Wave;
using sb_explorer.Classes;
using sb_explorer.Services.Audio;
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
        private bool isSeeking;

        //-------------------------------------------------------------------------------------------
        //  MAIN FORM
        //-------------------------------------------------------------------------------------------
        public FormMediaPlayer()
        {
            InitializeComponent();
            trackBarPosition.Maximum = 10000;
            trackBarPosition.SmallChange = 1;
            trackBarPosition.LargeChange = 100;
            trackBarPosition.TickFrequency = 500;
            trackBarPosition.MouseDown += (sender, args) => isSeeking = true;
            trackBarPosition.MouseUp += (sender, args) =>
            {
                SeekToTrackBarPosition();
                isSeeking = false;
            };
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
            trackBarPosition.Value = 0;
            labelCurrentTime.Text = GetDurationText(TimeSpan.Zero);
            labelTotalTime.Text = GetDurationText(GetSoundDuration());

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
                            EuroSoundWaveWriter.WriteSampleProvider16(
                                filePath,
                                audioFunctions.CreateStereoWav(ref providerLeft, ref providerRight, soundToPlay.PcmData, soundToPlay).ToSampleProvider(),
                                EuroSoundWaveWriter.CreateLoopInfo(soundToPlay.isLooped, soundToPlay.loopStartPoint, Math.Min(soundToPlay.PcmData[0].Length, soundToPlay.PcmData[1].Length) * 2L, 2));
                        }
                        else
                        {
                            EuroSoundWaveWriter.WriteSampleProvider16(
                                filePath,
                                audioFunctions.CreateMonoWav(ref providerLeft, soundToPlay.PcmData[0], soundToPlay).ToSampleProvider(),
                                EuroSoundWaveWriter.CreateLoopInfo(soundToPlay.isLooped, soundToPlay.loopStartPoint, soundToPlay.PcmData[0].Length, 1));
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
            SeekToTrackBarPosition();
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
                    labelTotalTime.Text = GetDurationText(providerLeft.TotalTime);
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
                if (!isSeeking && providerLeft.TotalTime.TotalMilliseconds > 0)
                {
                    trackBarPosition.Value = Math.Min(trackBarPosition.Maximum, (int)(trackBarPosition.Maximum * currentTime.TotalMilliseconds / providerLeft.TotalTime.TotalMilliseconds));
                }
                labelCurrentTime.Text = GetDurationText(currentTime);
            }
            else
            {
                trackBarPosition.Value = 0;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void SeekToTrackBarPosition()
        {
            if (providerLeft == null || providerLeft.TotalTime.TotalMilliseconds <= 0)
            {
                return;
            }

            TimeSpan streamPos = TimeSpan.FromMilliseconds(providerLeft.TotalTime.TotalMilliseconds * trackBarPosition.Value / trackBarPosition.Maximum);
            if (soundToPlay.channels > 1 && providerRight != null)
            {
                providerRight.CurrentTime = streamPos;
            }
            providerLeft.CurrentTime = streamPos;
            labelCurrentTime.Text = GetDurationText(streamPos);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private TimeSpan GetSoundDuration()
        {
            if (soundToPlay == null || soundToPlay.PcmData == null || soundToPlay.PcmData.Length == 0 || soundToPlay.PcmData[0] == null || soundToPlay.sampleRate == 0)
            {
                return TimeSpan.Zero;
            }

            double samples = soundToPlay.PcmData[0].Length / 2.0;
            return TimeSpan.FromSeconds(samples / soundToPlay.sampleRate);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static string GetDurationText(TimeSpan value)
        {
            return string.Format("{0:00}:{1:00}.{2:000}", (int)value.TotalMinutes, value.Seconds, value.Milliseconds);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
