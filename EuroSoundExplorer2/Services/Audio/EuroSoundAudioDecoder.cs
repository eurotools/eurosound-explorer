using AudioDecoders;
using MusX;
using MusX.Objects;
using sb_explorer.Classes;
using System;

namespace sb_explorer.Services.Audio
{
    internal static class EuroSoundAudioDecoder
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public static DecodedAudio DecodeChannels(EuroSoundAudioCodec codec, byte[] encodedData, AudioFunctions audioFunctions,
            short[] dspCoeffs, SampleData selectedSample, int channelCount, uint sampleRate, uint sampleCount, bool engineXt18)
        {
            if (encodedData == null) return null;
            channelCount = Math.Max(1, Math.Min(8, channelCount));

            byte[][] channels;
            switch (codec)
            {
                case EuroSoundAudioCodec.Pcm16:
                    channels = DeinterleavePcm16(encodedData, channelCount);
                    break;
                case EuroSoundAudioCodec.EurocomImaAdpcm:
                    channels = DecodeBlockInterleaved(encodedData, channelCount, 32, delegate(byte[] data)
                    {
                        return audioFunctions.ShortArrayToByteArray(new Eurocom_ImaAdpcm().Decode(data));
                    });
                    break;
                case EuroSoundAudioCodec.SonyVagAdpcm:
                    channels = DecodeBlockInterleaved(encodedData, channelCount, 16, delegate(byte[] data)
                    {
                        // The legacy decoder expects a 16-byte container prefix. EngineXT stores raw VAG blocks.
                        byte[] prefixed = new byte[data.Length + 16];
                        Buffer.BlockCopy(data, 0, prefixed, 16, data.Length);
                        uint loop = uint.MaxValue;
                        return new SonyAdpcm().Decode(prefixed, ref loop);
                    });
                    break;
                case EuroSoundAudioCodec.DspAdpcm:
                    channels = engineXt18
                        ? DecodeEngineXtDsp(encodedData, channelCount, audioFunctions)
                        : new[] { audioFunctions.ShortArrayToByteArray(new DspAdpcm().Decode(encodedData, dspCoeffs)) };
                    break;
                default:
                    byte[] mono = Decode(codec, encodedData, audioFunctions, dspCoeffs, selectedSample);
                    channels = mono == null ? new byte[0][] : new[] { mono };
                    break;
            }

            if (sampleCount != 0)
            {
                int wantedBytes = sampleCount > int.MaxValue / 2 ? int.MaxValue : checked((int)sampleCount * 2);
                for (int i = 0; i < channels.Length; i++)
                    if (channels[i] != null && channels[i].Length > wantedBytes) Array.Resize(ref channels[i], wantedBytes);
            }

            return new DecodedAudio { Channels = channels, SampleRate = sampleRate, SampleCount = sampleCount };
        }

        public static byte[] Decode(EuroSoundAudioCodec codec, byte[] encodedData, AudioFunctions audioFunctions, short[] dspCoeffs, SampleData selectedSample)
        {
            if (encodedData == null)
            {
                return null;
            }

            switch (codec)
            {
                case EuroSoundAudioCodec.Pcm16:
                    return encodedData;

                case EuroSoundAudioCodec.ImaAdpcm:
                    ImaAdpcm imaFile = new ImaAdpcm();
                    return audioFunctions.ShortArrayToByteArray(imaFile.Decode(encodedData, encodedData.Length * 2));

                case EuroSoundAudioCodec.EurocomImaAdpcm:
                    Eurocom_ImaAdpcm eurocomDAT = new Eurocom_ImaAdpcm();
                    return audioFunctions.ShortArrayToByteArray(eurocomDAT.Decode(encodedData));

                case EuroSoundAudioCodec.SonyVagAdpcm:
                    SonyAdpcm vagDecoder = new SonyAdpcm();
                    uint vagLoopStartOffset = selectedSample == null ? 0 : uint.MaxValue;
                    byte[] decodedData = vagDecoder.Decode(encodedData, ref vagLoopStartOffset);
                    if (selectedSample != null && selectedSample.IsLooped && vagLoopStartOffset != uint.MaxValue)
                    {
                        selectedSample.LoopStartOffset = vagLoopStartOffset / 2;
                        selectedSample.LoopStartSample = selectedSample.LoopStartOffset;
                    }
                    return decodedData;

                case EuroSoundAudioCodec.DspAdpcm:
                    DspAdpcm gcDecoder = new DspAdpcm();
                    return audioFunctions.ShortArrayToByteArray(gcDecoder.Decode(encodedData, dspCoeffs));

                case EuroSoundAudioCodec.XboxAdpcm:
                    XboxAdpcm xboxDecoder = new XboxAdpcm();
                    return audioFunctions.ShortArrayToByteArray(xboxDecoder.Decode(encodedData));

                default:
                    return null;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static byte[][] DecodeBlockInterleaved(byte[] source, int channels, int blockBytes, Func<byte[], byte[]> decoder)
        {
            int blockSet = checked(blockBytes * channels);
            int completeLength = source.Length - source.Length % blockSet;
            int bytesPerChannel = completeLength / channels;
            byte[][] encoded = new byte[channels][];
            for (int channel = 0; channel < channels; channel++) encoded[channel] = new byte[bytesPerChannel];
            int[] positions = new int[channels];
            for (int offset = 0; offset < completeLength; offset += blockSet)
            {
                for (int channel = 0; channel < channels; channel++)
                {
                    Buffer.BlockCopy(source, offset + channel * blockBytes, encoded[channel], positions[channel], blockBytes);
                    positions[channel] += blockBytes;
                }
            }
            byte[][] result = new byte[channels][];
            for (int channel = 0; channel < channels; channel++) result[channel] = decoder(encoded[channel]);
            return result;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static byte[][] DeinterleavePcm16(byte[] source, int channels)
        {
            int frameBytes = checked(channels * 2);
            int frames = source.Length / frameBytes;
            byte[][] result = new byte[channels][];
            for (int channel = 0; channel < channels; channel++) result[channel] = new byte[frames * 2];
            for (int frame = 0; frame < frames; frame++)
                for (int channel = 0; channel < channels; channel++)
                    Buffer.BlockCopy(source, frame * frameBytes + channel * 2, result[channel], frame * 2, 2);
            return result;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static byte[][] DecodeEngineXtDsp(byte[] source, int channels, AudioFunctions audioFunctions)
        {
            int regionBytes = source.Length / channels;
            byte[][] result = new byte[channels][];
            for (int channel = 0; channel < channels; channel++)
            {
                int start = channel * regionBytes;
                if (regionBytes < 96) { result[channel] = new byte[0]; continue; }
                short[] coefficients = new short[16];
                for (int i = 0; i < coefficients.Length; i++)
                {
                    int p = start + 0x1c + i * 2;
                    coefficients[i] = unchecked((short)((source[p] << 8) | source[p + 1]));
                }
                byte[] payload = new byte[regionBytes - 96];
                Buffer.BlockCopy(source, start + 96, payload, 0, payload.Length);
                result[channel] = audioFunctions.ShortArrayToByteArray(new DspAdpcm().Decode(payload, coefficients));
            }
            return result;
        }
    }
}
