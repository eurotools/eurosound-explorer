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

            provider = new RawSourceWaveStream(new MemoryStream(_pcmData), new WaveFormat(SemitonesToFreq(_soundToPlay.sampleRate, GetPitch(_soundToPlay)), 16, 1));
            LoopStream loop = new LoopStream(provider, _soundToPlay.loopStartPoint * 2) { EnableLooping = _soundToPlay.isLooped, Position = _soundToPlay.startPos };
            PanningSampleProvider panProvider = new PanningSampleProvider(loop.ToSampleProvider()) { Pan = GetPan(_soundToPlay) };
            VolumeSampleProvider volumeProvider = new VolumeSampleProvider(panProvider) { Volume = GetVolume(_soundToPlay) };

            return volumeProvider.ToWaveProvider();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal IWaveProvider CreateMonoWav(ref RawSourceWaveStream provider, byte[] _pcmData, SoundFile _soundToPlay)
        {
            provider = new RawSourceWaveStream(new MemoryStream(_pcmData), new WaveFormat(SemitonesToFreq(_soundToPlay.sampleRate, GetPitch(_soundToPlay)), 16, 1));
            PanningSampleProvider panProvider = new PanningSampleProvider(provider.ToSampleProvider()) { Pan = GetPan(_soundToPlay) };
            VolumeSampleProvider volumeProvider = new VolumeSampleProvider(panProvider) { Volume = GetVolume(_soundToPlay) };

            return volumeProvider.ToWaveProvider();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal IWaveProvider CreateStereoLoopWav(ref RawSourceWaveStream providerLeft, ref RawSourceWaveStream providerRight, byte[][] _pcmData, SoundFile _soundToPlay)
        {
            if (_soundToPlay.loopEndPoint > 0)
            {
                Array.Resize(ref _pcmData[0], Math.Min(_soundToPlay.loopEndPoint * 4, _pcmData[0].Length));
                Array.Resize(ref _pcmData[1], Math.Min(_soundToPlay.loopEndPoint * 4, _pcmData[0].Length));
            }

            providerLeft = new RawSourceWaveStream(new MemoryStream(_pcmData[0]), new WaveFormat(SemitonesToFreq(_soundToPlay.sampleRate, GetPitch(_soundToPlay)), 16, 1));
            LoopStream loopLeft = new LoopStream(providerLeft, _soundToPlay.loopStartPoint * 4) { EnableLooping = _soundToPlay.isLooped, Position = _soundToPlay.startPos };
            providerRight = new RawSourceWaveStream(new MemoryStream(_pcmData[1]), new WaveFormat(SemitonesToFreq(_soundToPlay.sampleRate, GetPitch(_soundToPlay)), 16, 1));
            LoopStream loopRight = new LoopStream(providerRight, _soundToPlay.loopStartPoint * 4) { EnableLooping = _soundToPlay.isLooped, Position = _soundToPlay.startPos };
            MultiplexingWaveProvider waveProvider = new MultiplexingWaveProvider(new IWaveProvider[] { loopLeft, loopRight }, 2);
            VolumeSampleProvider volumeProvider = new VolumeSampleProvider(waveProvider.ToSampleProvider()) { Volume = GetVolume(_soundToPlay) };

            return volumeProvider.ToWaveProvider();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal IWaveProvider CreateStereoWav(ref RawSourceWaveStream providerLeft, ref RawSourceWaveStream providerRight, byte[][] _pcmData, SoundFile _soundToPlay)
        {
            providerLeft = new RawSourceWaveStream(new MemoryStream(_pcmData[0]), new WaveFormat(SemitonesToFreq(_soundToPlay.sampleRate, GetPitch(_soundToPlay)), 16, 1));
            providerRight = new RawSourceWaveStream(new MemoryStream(_pcmData[1]), new WaveFormat(SemitonesToFreq(_soundToPlay.sampleRate, GetPitch(_soundToPlay)), 16, 1));
            MultiplexingWaveProvider waveProvider = new MultiplexingWaveProvider(new IWaveProvider[] { providerLeft, providerRight }, 2);
            VolumeSampleProvider volumeProvider = new VolumeSampleProvider(waveProvider.ToSampleProvider()) { Volume = GetVolume(_soundToPlay) };

            return volumeProvider.ToWaveProvider();
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
        internal bool CheckIfEurocomImaIsInvalid(byte[] ImaData)
        {
            bool invalidData = false;
            byte chunckId = 65;
            for (int j = 0; j < ImaData.Length; j += 32)
            {
                byte[] chunckData = new byte[32];
                Buffer.BlockCopy(ImaData, j, chunckData, 0, chunckData.Length);
                if (j == 0 && chunckData[3] != 65)
                {
                    invalidData = true;
                    break;
                }
                else if (chunckData[3] != chunckId)
                {
                    invalidData = true;
                    break;
                }
                chunckId++;
                if (chunckId > 90)
                {
                    chunckId = 65;
                }
            }

            return invalidData;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
