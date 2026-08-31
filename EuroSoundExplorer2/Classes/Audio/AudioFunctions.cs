using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.IO;

namespace sb_explorer.Classes
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    internal class AudioFunctions
    {
        private readonly Random random = new Random();

        //-------------------------------------------------------------------------------------------------------------------------------
        internal int SemitonesToFreq(int Frequency, float Semitone)
        {
            float mult = 1.0f;
            if (Semitone != 0)
            {
                //In terms of frequencies, a semitone is equal to a frequency ratio of 2^(1/12)
                mult = (float)Math.Pow(2.0f, Semitone * (1.0f / 12.0f));
            }
            return (int)(Frequency * mult);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal IWaveProvider CreateMonoLoopWav(ref RawSourceWaveStream provider, byte[] _pcmData, SoundFile _soundToPlay,
            float pitch, float pan, float volume)
        {
            _pcmData = (byte[])_pcmData.Clone();
            NormalizePlaybackOffsets(_soundToPlay, _pcmData.Length / 2);
            if (_soundToPlay.loopEndPoint > 0)
            {
                Array.Resize(ref _pcmData, Math.Min(_soundToPlay.loopEndPoint * 2, _pcmData.Length));
            }

            provider = new RawSourceWaveStream(new MemoryStream(_pcmData), new WaveFormat(SemitonesToFreq((int)_soundToPlay.sampleRate, pitch), 16, 1));
            LoopStream loop = new LoopStream(provider, (int)(_soundToPlay.loopStartPoint * 2)) { EnableLooping = _soundToPlay.isLooped, Position = _soundToPlay.startPos * 2 };
            PanningSampleProvider panProvider = new PanningSampleProvider(loop.ToSampleProvider()) { Pan = pan };
            VolumeSampleProvider volumeProvider = new VolumeSampleProvider(panProvider) { Volume = volume };

            return volumeProvider.ToWaveProvider();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal IWaveProvider CreateMonoWav(ref RawSourceWaveStream provider, byte[] _pcmData, SoundFile _soundToPlay)
        {
            provider = new RawSourceWaveStream(new MemoryStream(_pcmData), new WaveFormat(SemitonesToFreq((int)_soundToPlay.sampleRate, GetPitch(_soundToPlay)), 16, 1));
            // PanningSampleProvider always produces stereo.  This non-looping path
            // is also used when exporting a mono source, so keep its single channel.
            VolumeSampleProvider volumeProvider = new VolumeSampleProvider(provider.ToSampleProvider()) { Volume = GetVolume(_soundToPlay) };

            return volumeProvider.ToWaveProvider();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal IWaveProvider CreateStereoLoopWav(ref RawSourceWaveStream providerLeft, ref RawSourceWaveStream providerRight,
            byte[][] _pcmData, SoundFile _soundToPlay, float pitch, float pan, float volume)
        {
            byte[] leftData = (byte[])_pcmData[0].Clone();
            byte[] rightData = (byte[])_pcmData[1].Clone();
            NormalizePlaybackOffsets(_soundToPlay, Math.Min(leftData.Length, rightData.Length) / 2);
            if (_soundToPlay.loopEndPoint > 0)
            {
                Array.Resize(ref leftData, Math.Min(_soundToPlay.loopEndPoint * 2, leftData.Length));
                Array.Resize(ref rightData, Math.Min(_soundToPlay.loopEndPoint * 2, rightData.Length));
            }

            int frequency = SemitonesToFreq((int)_soundToPlay.sampleRate, pitch);
            byte[] interleaved = InterleaveStereoPcm16(leftData, rightData, GetLeftPanGain(pan), GetRightPanGain(pan));
            providerLeft = new RawSourceWaveStream(new MemoryStream(interleaved), new WaveFormat(frequency, 16, 2));
            providerRight = null;
            // Keep both channels in one provider so loopStart/loopEnd are crossed
            // on the same PCM frame, matching EngineXT's single stream jump.
            LoopStream loop = new LoopStream(providerLeft, checked((int)(_soundToPlay.loopStartPoint * 4L)))
            {
                EnableLooping = _soundToPlay.isLooped,
                Position = _soundToPlay.startPos * 4L
            };
            VolumeSampleProvider volumeProvider = new VolumeSampleProvider(loop.ToSampleProvider()) { Volume = volume };

            return volumeProvider.ToWaveProvider();
        }

        private static byte[] InterleaveStereoPcm16(byte[] left, byte[] right, float leftGain, float rightGain)
        {
            int frames = Math.Min(left.Length, right.Length) / 2;
            byte[] result = new byte[frames * 4];
            for (int frame = 0; frame < frames; frame++)
            {
                short leftSample = BitConverter.ToInt16(left, frame * 2);
                short rightSample = BitConverter.ToInt16(right, frame * 2);
                short scaledLeft = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, (int)Math.Round(leftSample * leftGain)));
                short scaledRight = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, (int)Math.Round(rightSample * rightGain)));
                int destination = frame * 4;
                result[destination] = (byte)scaledLeft;
                result[destination + 1] = (byte)(scaledLeft >> 8);
                result[destination + 2] = (byte)scaledRight;
                result[destination + 3] = (byte)(scaledRight >> 8);
            }
            return result;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal IWaveProvider CreateStereoWav(ref RawSourceWaveStream providerLeft, ref RawSourceWaveStream providerRight, byte[][] _pcmData, SoundFile _soundToPlay)
        {
            providerLeft = new RawSourceWaveStream(new MemoryStream(_pcmData[0]), new WaveFormat(SemitonesToFreq((int)_soundToPlay.sampleRate, GetPitch(_soundToPlay)), 16, 1));
            providerRight = new RawSourceWaveStream(new MemoryStream(_pcmData[1]), new WaveFormat(SemitonesToFreq((int)_soundToPlay.sampleRate, GetPitch(_soundToPlay)), 16, 1));
            MultiplexingWaveProvider waveProvider = new MultiplexingWaveProvider(new IWaveProvider[] { providerLeft, providerRight }, 2);
            VolumeSampleProvider volumeProvider = new VolumeSampleProvider(waveProvider.ToSampleProvider()) { Volume = GetVolume(_soundToPlay) };

            return volumeProvider.ToWaveProvider();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal IWaveProvider CreateMultiChannelWav(out RawSourceWaveStream[] providers, byte[][] pcmData, SoundFile sound,
            bool loop, float pitch, float pan, float volume)
        {
            if (pcmData == null || pcmData.Length == 0) throw new ArgumentException("PCM channel data is empty.", "pcmData");
            int channelCount = Math.Min(8, pcmData.Length);
            int availableSamples = int.MaxValue;
            for (int channel = 0; channel < channelCount; channel++)
                availableSamples = Math.Min(availableSamples, (pcmData[channel] == null ? 0 : pcmData[channel].Length) / 2);
            NormalizePlaybackOffsets(sound, availableSamples == int.MaxValue ? 0 : availableSamples);
            providers = new RawSourceWaveStream[channelCount];
            IWaveProvider[] inputs = new IWaveProvider[channelCount];
            int frequency = SemitonesToFreq((int)sound.sampleRate, pitch);
            for (int channel = 0; channel < channelCount; channel++)
            {
                byte[] data = pcmData[channel] ?? new byte[0];
                if (sound.loopEndPoint > 0 && sound.loopEndPoint <= int.MaxValue / 2)
                    Array.Resize(ref data, Math.Min(sound.loopEndPoint * 2, data.Length));
                providers[channel] = new RawSourceWaveStream(new MemoryStream(data), new WaveFormat(frequency, 16, 1));
                inputs[channel] = loop
                    ? (IWaveProvider)new LoopStream(providers[channel], checked((int)Math.Min(int.MaxValue, sound.loopStartPoint * 2L))) { EnableLooping = sound.isLooped, Position = sound.startPos * 2L }
                    : providers[channel];
                if (channel == 0)
                    inputs[channel] = new VolumeSampleProvider(inputs[channel].ToSampleProvider()) { Volume = GetLeftPanGain(pan) }.ToWaveProvider();
                else if (channel == 1)
                    inputs[channel] = new VolumeSampleProvider(inputs[channel].ToSampleProvider()) { Volume = GetRightPanGain(pan) }.ToWaveProvider();
            }
            MultiplexingWaveProvider multiplexed = new MultiplexingWaveProvider(inputs, channelCount);
            return new VolumeSampleProvider(multiplexed.ToSampleProvider()) { Volume = volume }.ToWaveProvider();
        }

        private static void NormalizePlaybackOffsets(SoundFile sound, int availableSamples)
        {
            availableSamples = Math.Max(0, availableSamples);
            if (sound.startPos < 0 || sound.startPos >= availableSamples) sound.startPos = 0;
            if (sound.loopEndPoint <= 0 || sound.loopEndPoint > availableSamples) sound.loopEndPoint = availableSamples;
            if (!sound.isLooped) return;
            if (sound.loopStartPoint >= (uint)sound.loopEndPoint)
            {
                sound.isLooped = false;
                sound.loopStartPoint = 0;
                sound.loopEndPoint = 0;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal float GetPitch(SoundFile sampleInfo)
        {
            if (!sampleInfo.applyPoolEffects) return 0;
            return sampleInfo.pitch + RandomRange(Math.Abs(sampleInfo.pitchOffset));
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal float GetPan(SoundFile sampleInfo)
        {
            if (!sampleInfo.applyPoolEffects) return 0;
            float value = sampleInfo.panning + RandomRange(Math.Abs(sampleInfo.panningOffset));
            if (sampleInfo.panningIsAngle)
            {
                value %= 360.0f;
                if (value > 180.0f) value -= 360.0f;
                if (value < -180.0f) value += 360.0f;
                value = (float)Math.Sin(value * (Math.PI / 180.0));
            }
            return Math.Max(-1.0f, Math.Min(1.0f, value));
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal float GetVolume(SoundFile sampleInfo)
        {
            if (!sampleInfo.applyPoolEffects) return 1;
            float linear = sampleInfo.volume + RandomRange(Math.Abs(sampleInfo.volumeOffset));
            linear = Math.Max(0.0f, Math.Min(1.0f, linear));
            return linear * linear;
        }

        private float RandomRange(float extent)
        {
            return extent <= 0 ? 0 : (float)((random.NextDouble() * 2.0 - 1.0) * extent);
        }

        private static float GetLeftPanGain(float pan) { return pan > 0 ? 1.0f - pan : 1.0f; }
        private static float GetRightPanGain(float pan) { return pan < 0 ? 1.0f + pan : 1.0f; }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal byte[] ShortArrayToByteArray(short[] inputArray)
        {
            byte[] byteArray = new byte[inputArray.Length * 2];
            Buffer.BlockCopy(inputArray, 0, byteArray, 0, byteArray.Length);

            return byteArray;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal bool CheckIfEurocomImaIsInvalid(byte[] imaData, int channels = 1)
        {
            if (imaData == null || imaData.Length == 0) return true;
            channels = Math.Max(1, Math.Min(8, channels));
            const int blockSize = 32;
            int blockSetSize = blockSize * channels;
            if ((imaData.Length % blockSetSize) != 0) return true;

            int blockSetIndex = 0;
            for (int offset = 0; offset < imaData.Length; offset += blockSetSize, blockSetIndex++)
            {
                byte expectedMarker = (byte)('A' + (blockSetIndex % 26));
                for (int channel = 0; channel < channels; channel++)
                {
                    int markerOffset = offset + channel * blockSize + 3;
                    if (imaData[markerOffset] != expectedMarker) return true;
                }
            }
            return false;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
