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
        public IWaveProvider CreateMonoLoopWav(int startPos, int _startLoop, bool flags, byte[] _pcmData, int _frequency, float _pitch, float _pan, float _volume)
        {
            RawSourceWaveStream provider = new RawSourceWaveStream(new MemoryStream(_pcmData), new WaveFormat(SemitonesToFreq(_frequency, _pitch), 16, 1));
            LoopStream loop = new LoopStream(provider, _startLoop) { EnableLooping = flags, Position = startPos };
            PanningSampleProvider panProvider = new PanningSampleProvider(loop.ToSampleProvider()) { Pan = _pan };
            VolumeSampleProvider volumeProvider = new VolumeSampleProvider(panProvider) { Volume = _volume };

            return volumeProvider.ToWaveProvider();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public IWaveProvider CreateMonoWav(byte[] _pcmData, int _frequency, float _pitch, float _pan, float _volume)
        {
            RawSourceWaveStream provider = new RawSourceWaveStream(new MemoryStream(_pcmData), new WaveFormat(SemitonesToFreq(_frequency, _pitch), 16, 1));
            PanningSampleProvider panProvider = new PanningSampleProvider(provider.ToSampleProvider()) { Pan = _pan };
            VolumeSampleProvider volumeProvider = new VolumeSampleProvider(panProvider) { Volume = _volume };

            return volumeProvider.ToWaveProvider();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public IWaveProvider CreateStereoLoopWav(int startPos, int _startLoop, bool flags, byte[][] _pcmData, int _frequency, float _pitch, float _volume)
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
        public IWaveProvider CreateStereoWav(byte[][] _pcmData, int _frequency, float _pitch, float _volume)
        {
            IWaveProvider providerLeft = new RawSourceWaveStream(new MemoryStream(_pcmData[0]), new WaveFormat(SemitonesToFreq(_frequency, _pitch), 16, 1));
            IWaveProvider providerRight = new RawSourceWaveStream(new MemoryStream(_pcmData[1]), new WaveFormat(SemitonesToFreq(_frequency, _pitch), 16, 1));
            MultiplexingWaveProvider waveProvider = new MultiplexingWaveProvider(new IWaveProvider[] { providerLeft, providerRight }, 2);
            VolumeSampleProvider volumeProvider = new VolumeSampleProvider(waveProvider.ToSampleProvider()) { Volume = _volume };

            return volumeProvider.ToWaveProvider();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public byte[] ShortArrayToByteArray(short[] inputArray)
        {
            byte[] byteArray = new byte[inputArray.Length * 2];
            Buffer.BlockCopy(inputArray, 0, byteArray, 0, byteArray.Length);

            return byteArray;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
