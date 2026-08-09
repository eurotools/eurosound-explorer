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
        internal IWaveProvider CreateMonoLoopWav(ref RawSourceWaveStream provider, byte[] _pcmData, SoundFile _soundToPlay)
        {
            if (_soundToPlay.loopEndPoint > 0)
            {
                Array.Resize(ref _pcmData, Math.Min(_soundToPlay.loopEndPoint * 2, _pcmData.Length));
            }

            provider = new RawSourceWaveStream(new MemoryStream(_pcmData), new WaveFormat(SemitonesToFreq((int)_soundToPlay.sampleRate, GetPitch(_soundToPlay)), 16, 1));
            LoopStream loop = new LoopStream(provider, (int)(_soundToPlay.loopStartPoint * 2)) { EnableLooping = _soundToPlay.isLooped, Position = _soundToPlay.startPos * 2 };
            PanningSampleProvider panProvider = new PanningSampleProvider(loop.ToSampleProvider()) { Pan = GetPan(_soundToPlay) };
            VolumeSampleProvider volumeProvider = new VolumeSampleProvider(panProvider) { Volume = GetVolume(_soundToPlay) };

            return volumeProvider.ToWaveProvider();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal IWaveProvider CreateMonoWav(ref RawSourceWaveStream provider, byte[] _pcmData, SoundFile _soundToPlay)
        {
            provider = new RawSourceWaveStream(new MemoryStream(_pcmData), new WaveFormat(SemitonesToFreq((int)_soundToPlay.sampleRate, GetPitch(_soundToPlay)), 16, 1));
            PanningSampleProvider panProvider = new PanningSampleProvider(provider.ToSampleProvider()) { Pan = GetPan(_soundToPlay) };
            VolumeSampleProvider volumeProvider = new VolumeSampleProvider(panProvider) { Volume = GetVolume(_soundToPlay) };

            return volumeProvider.ToWaveProvider();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal IWaveProvider CreateStereoLoopWav(ref RawSourceWaveStream providerLeft, ref RawSourceWaveStream providerRight, byte[][] _pcmData, SoundFile _soundToPlay)
        {
            if (_soundToPlay.loopEndPoint > 0)
            {
                Array.Resize(ref _pcmData[0], Math.Min(_soundToPlay.loopEndPoint * 2, _pcmData[0].Length));
                Array.Resize(ref _pcmData[1], Math.Min(_soundToPlay.loopEndPoint * 2, _pcmData[1].Length));
            }

            providerLeft = new RawSourceWaveStream(new MemoryStream(_pcmData[0]), new WaveFormat(SemitonesToFreq((int)_soundToPlay.sampleRate, GetPitch(_soundToPlay)), 16, 1));
            LoopStream loopLeft = new LoopStream(providerLeft, (int)(_soundToPlay.loopStartPoint * 2)) { EnableLooping = _soundToPlay.isLooped, Position = _soundToPlay.startPos * 2 };
            providerRight = new RawSourceWaveStream(new MemoryStream(_pcmData[1]), new WaveFormat(SemitonesToFreq((int)_soundToPlay.sampleRate, GetPitch(_soundToPlay)), 16, 1));
            LoopStream loopRight = new LoopStream(providerRight, (int)(_soundToPlay.loopStartPoint * 2)) { EnableLooping = _soundToPlay.isLooped, Position = _soundToPlay.startPos * 2 };
            MultiplexingWaveProvider waveProvider = new MultiplexingWaveProvider(new IWaveProvider[] { loopLeft, loopRight }, 2);
            VolumeSampleProvider volumeProvider = new VolumeSampleProvider(waveProvider.ToSampleProvider()) { Volume = GetVolume(_soundToPlay) };

            return volumeProvider.ToWaveProvider();
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
        internal IWaveProvider CreateMultiChannelWav(out RawSourceWaveStream[] providers, byte[][] pcmData, SoundFile sound, bool loop)
        {
            if (pcmData == null || pcmData.Length == 0) throw new ArgumentException("PCM channel data is empty.", "pcmData");
            int channelCount = Math.Min(8, pcmData.Length);
            providers = new RawSourceWaveStream[channelCount];
            IWaveProvider[] inputs = new IWaveProvider[channelCount];
            int frequency = SemitonesToFreq((int)sound.sampleRate, GetPitch(sound));
            for (int channel = 0; channel < channelCount; channel++)
            {
                byte[] data = pcmData[channel] ?? new byte[0];
                if (sound.loopEndPoint > 0 && sound.loopEndPoint <= int.MaxValue / 2)
                    Array.Resize(ref data, Math.Min(sound.loopEndPoint * 2, data.Length));
                providers[channel] = new RawSourceWaveStream(new MemoryStream(data), new WaveFormat(frequency, 16, 1));
                inputs[channel] = loop
                    ? (IWaveProvider)new LoopStream(providers[channel], checked((int)Math.Min(int.MaxValue, sound.loopStartPoint * 2L))) { EnableLooping = sound.isLooped, Position = sound.startPos * 2L }
                    : providers[channel];
            }
            MultiplexingWaveProvider multiplexed = new MultiplexingWaveProvider(inputs, channelCount);
            return new VolumeSampleProvider(multiplexed.ToSampleProvider()) { Volume = GetVolume(sound) }.ToWaveProvider();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal float GetPitch(SoundFile sampleInfo)
        {
            switch (random.Next(0, 3))
            {
                case 0:
                    return sampleInfo.pitch + sampleInfo.pitchOffset;
                case 1:
                    return sampleInfo.pitch + (sampleInfo.pitchOffset * -1);
                default:
                    return sampleInfo.pitch;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal float GetPan(SoundFile sampleInfo)
        {
            switch (random.Next(0, 3))
            {
                case 0:
                    return sampleInfo.panning + sampleInfo.panningOffset;
                case 1:
                    return sampleInfo.panning + (sampleInfo.panningOffset * -1);
                default:
                    return sampleInfo.panning;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal float GetVolume(SoundFile sampleInfo)
        {
            switch (random.Next(0, 3))
            {
                case 0:
                    return sampleInfo.volume + sampleInfo.volumeOffset;
                case 1:
                    return sampleInfo.volume + (sampleInfo.volumeOffset * -1);
                default:
                    return sampleInfo.volume;
            }
        }

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
