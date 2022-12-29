using MusX.Objects;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.IO;

namespace EuroSoundExplorer2.Classes
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    internal class AudioFunctions
    {
        private readonly Random random = new Random();

        //-------------------------------------------------------------------------------------------------------------------------------
        private int SemitonesToFreq(int Frequency, float Semitone)
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
        internal IWaveProvider CreateMonoLoopWav(int startPos, int _startLoop, bool flags, byte[] _pcmData, int _frequency, float _pitch, float _pan, float _volume)
        {
            RawSourceWaveStream provider = new RawSourceWaveStream(new MemoryStream(_pcmData), new WaveFormat(SemitonesToFreq(_frequency, _pitch), 16, 1));
            LoopStream loop = new LoopStream(provider, _startLoop) { EnableLooping = flags, Position = startPos };
            PanningSampleProvider panProvider = new PanningSampleProvider(loop.ToSampleProvider()) { Pan = _pan };
            VolumeSampleProvider volumeProvider = new VolumeSampleProvider(panProvider) { Volume = _volume };

            return volumeProvider.ToWaveProvider();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal IWaveProvider CreateMonoWav(byte[] _pcmData, int _frequency, float _pitch, float _pan, float _volume)
        {
            RawSourceWaveStream provider = new RawSourceWaveStream(new MemoryStream(_pcmData), new WaveFormat(SemitonesToFreq(_frequency, _pitch), 16, 1));
            PanningSampleProvider panProvider = new PanningSampleProvider(provider.ToSampleProvider()) { Pan = _pan };
            VolumeSampleProvider volumeProvider = new VolumeSampleProvider(panProvider) { Volume = _volume };

            return volumeProvider.ToWaveProvider();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal IWaveProvider CreateStereoLoopWav(int startPos, int _startLoop, bool flags, byte[][] _pcmData, int _frequency, float _pitch, float _volume)
        {
            RawSourceWaveStream providerLeft = new RawSourceWaveStream(new MemoryStream(_pcmData[0]), new WaveFormat(SemitonesToFreq(_frequency, _pitch), 16, 1));
            LoopStream loopLeft = new LoopStream(providerLeft, _startLoop) { EnableLooping = flags, Position = startPos };
            RawSourceWaveStream providerRight = new RawSourceWaveStream(new MemoryStream(_pcmData[1]), new WaveFormat(SemitonesToFreq(_frequency, _pitch), 16, 1));
            LoopStream loopRight = new LoopStream(providerRight, _startLoop) { EnableLooping = flags, Position = startPos };
            MultiplexingWaveProvider waveProvider = new MultiplexingWaveProvider(new IWaveProvider[] { loopLeft, loopRight }, 2);
            VolumeSampleProvider volumeProvider = new VolumeSampleProvider(waveProvider.ToSampleProvider()) { Volume = _volume };

            return volumeProvider.ToWaveProvider();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal IWaveProvider CreateStereoWav(byte[][] _pcmData, int _frequency, float _pitch, float _volume)
        {
            IWaveProvider providerLeft = new RawSourceWaveStream(new MemoryStream(_pcmData[0]), new WaveFormat(SemitonesToFreq(_frequency, _pitch), 16, 1));
            IWaveProvider providerRight = new RawSourceWaveStream(new MemoryStream(_pcmData[1]), new WaveFormat(SemitonesToFreq(_frequency, _pitch), 16, 1));
            MultiplexingWaveProvider waveProvider = new MultiplexingWaveProvider(new IWaveProvider[] { providerLeft, providerRight }, 2);
            VolumeSampleProvider volumeProvider = new VolumeSampleProvider(waveProvider.ToSampleProvider()) { Volume = _volume };

            return volumeProvider.ToWaveProvider();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal float GetPitch(SampleInfo sampleInfo)
        {
            switch (random.Next(0, 3))
            {
                case 0:
                    return sampleInfo.Pitch + sampleInfo.PitchOffset;
                case 1:
                    return sampleInfo.Pitch + (sampleInfo.PitchOffset * -1);
                default:
                    return sampleInfo.Pitch;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal float GetPan(SampleInfo sampleInfo)
        {
            switch (random.Next(0, 3))
            {
                case 0:
                    return sampleInfo.Pan + sampleInfo.PanOffset;
                case 1:
                    return sampleInfo.Pan + (sampleInfo.PanOffset * -1);
                default:
                    return sampleInfo.Pan;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal float GetVolume(SampleInfo sampleInfo)
        {
            switch (random.Next(0, 3))
            {
                case 0:
                    return sampleInfo.Volume + sampleInfo.VolumeOffset;
                case 1:
                    return sampleInfo.Volume + (sampleInfo.VolumeOffset * -1);
                default:
                    return sampleInfo.Volume;
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
