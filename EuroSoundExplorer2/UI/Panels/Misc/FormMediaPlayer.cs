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
        private RawSourceWaveStream[] channelProviders;
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

                        if (soundToPlay.PcmData.Length == 1)
                        {
                            RawSourceWaveStream left = null;
                            EuroSoundWaveWriter.WriteSampleProvider16(filePath,
                                audioFunctions.CreateMonoWav(ref left, soundToPlay.PcmData[0], soundToPlay).ToSampleProvider(),
                                EuroSoundWaveWriter.CreateLoopInfo(soundToPlay.isLooped, soundToPlay.loopStartPoint, soundToPlay.PcmData[0].Length, 1));
                            left.Dispose();
                        }
                        else if (soundToPlay.PcmData.Length == 2)
                        {
                            RawSourceWaveStream left = null, right = null;
                            EuroSoundWaveWriter.WriteSampleProvider16(filePath,
                                audioFunctions.CreateStereoWav(ref left, ref right, soundToPlay.PcmData, soundToPlay).ToSampleProvider(),
                                EuroSoundWaveWriter.CreateLoopInfo(soundToPlay.isLooped, soundToPlay.loopStartPoint,
                                    Math.Min(soundToPlay.PcmData[0].Length, soundToPlay.PcmData[1].Length) * 2L, 2));
                            left.Dispose(); right.Dispose();
                        }
                        else
                        {
                            int channels = Math.Min(8, soundToPlay.PcmData.Length);
                            int channelBytes = int.MaxValue;
                            for (int channel = 0; channel < channels; channel++) channelBytes = Math.Min(channelBytes, soundToPlay.PcmData[channel].Length);
                            EuroSoundWaveWriter.WriteChannelsPcm16(filePath, soundToPlay.PcmData, (int)soundToPlay.sampleRate,
                                EuroSoundWaveWriter.CreateLoopInfo(soundToPlay.isLooped, soundToPlay.loopStartPoint, (long)channelBytes * channels, channels));
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

        private void TrackBarPosition_MouseDown(object sender, MouseEventArgs e)
        {
            isSeeking = true;
        }

        private void TrackBarPosition_MouseUp(object sender, MouseEventArgs e)
        {
            SeekToTrackBarPosition();
            isSeeking = false;
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
                    if (soundToPlay.PcmData.Length == 1)
                    {
                        RawSourceWaveStream left = null;
                        waveDataProv = audioFunctions.CreateMonoLoopWav(ref left, soundToPlay.PcmData[0], soundToPlay);
                        channelProviders = new[] { left };
                    }
                    else if (soundToPlay.PcmData.Length == 2)
                    {
                        RawSourceWaveStream left = null, right = null;
                        waveDataProv = audioFunctions.CreateStereoLoopWav(ref left, ref right, soundToPlay.PcmData, soundToPlay);
                        channelProviders = new[] { left, right };
                    }
                    else
                    {
                        waveDataProv = audioFunctions.CreateMultiChannelWav(out channelProviders, soundToPlay.PcmData, soundToPlay, true);
                    }
                    labelTotalTime.Text = GetDurationText(channelProviders[0].TotalTime);
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
            RawSourceWaveStream timeline = GetTimelineProvider();
            if (_waveOut != null && timeline != null)
            {
                TimeSpan currentTime = (_waveOut.PlaybackState == PlaybackState.Stopped) ? TimeSpan.Zero : timeline.CurrentTime;
                if (!isSeeking && timeline.TotalTime.TotalMilliseconds > 0)
                {
                    trackBarPosition.Value = Math.Min(trackBarPosition.Maximum, (int)(trackBarPosition.Maximum * currentTime.TotalMilliseconds / timeline.TotalTime.TotalMilliseconds));
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
            RawSourceWaveStream timeline = GetTimelineProvider();
            if (timeline == null || timeline.TotalTime.TotalMilliseconds <= 0)
            {
                return;
            }

            TimeSpan streamPos = TimeSpan.FromMilliseconds(timeline.TotalTime.TotalMilliseconds * trackBarPosition.Value / trackBarPosition.Maximum);
            for (int channel = 0; channelProviders != null && channel < channelProviders.Length; channel++)
            {
                channelProviders[channel].CurrentTime = streamPos;
            }
            labelCurrentTime.Text = GetDurationText(streamPos);
        }

        private RawSourceWaveStream GetTimelineProvider()
        {
            return channelProviders != null && channelProviders.Length > 0 ? channelProviders[0] : null;
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
