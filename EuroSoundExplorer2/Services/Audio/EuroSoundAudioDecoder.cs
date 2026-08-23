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
            uint vagLoopStartSample = uint.MaxValue;
            switch (codec)
            {
                case EuroSoundAudioCodec.Pcm16:
                    channels = DeinterleavePcm16(encodedData, channelCount);
                    break;
                case EuroSoundAudioCodec.EurocomImaAdpcm:
                    byte[] imaData = engineXt18
                        ? NormalizeEngineXtIma(encodedData, channelCount, sampleCount)
                        : encodedData;
                    channels = DecodeBlockInterleaved(imaData, channelCount, 32, delegate(byte[] data)
                    {
                        return audioFunctions.ShortArrayToByteArray(new Eurocom_ImaAdpcm().Decode(data));
                    });
                    break;
                case EuroSoundAudioCodec.SonyVagAdpcm:
                    byte[] vagBlocks = encodedData;
                    if (!engineXt18)
                    {
                        if (encodedData.Length < 16) throw new System.IO.InvalidDataException("EuroSound VAG data has no 16-byte prefix.");
                        vagBlocks = new byte[encodedData.Length - 16];
                        Buffer.BlockCopy(encodedData, 16, vagBlocks, 0, vagBlocks.Length);
                    }
                    channels = DecodeBlockInterleaved(vagBlocks, channelCount, 16, delegate(byte[] data)
                    {
                        uint loop = uint.MaxValue;
                        byte[] pcm = new SonyAdpcm().DecodeRaw(data, ref loop);
                        if (vagLoopStartSample == uint.MaxValue && loop != uint.MaxValue) vagLoopStartSample = loop / 2;
                        return pcm;
                    });
                    break;
                case EuroSoundAudioCodec.DspAdpcm:
                    channels = new[] { audioFunctions.ShortArrayToByteArray(new DspAdpcm().Decode(encodedData, dspCoeffs)) };
                    break;
                case EuroSoundAudioCodec.DspAdpcmLegacy:
                    channels = DecodeEngineXtDsp(encodedData, channelCount, audioFunctions, false);
                    break;
                case EuroSoundAudioCodec.DspAdpcmNgca:
                    channels = DecodeEngineXtDsp(encodedData, channelCount, audioFunctions, true);
                    break;
                default:
                    byte[] mono = Decode(codec, encodedData, audioFunctions, dspCoeffs, selectedSample);
                    channels = mono == null ? new byte[0][] : new[] { mono };
                    break;
            }

            if (codec == EuroSoundAudioCodec.SonyVagAdpcm && selectedSample != null && selectedSample.IsLooped && vagLoopStartSample != uint.MaxValue)
            {
                selectedSample.LoopStartOffset = vagLoopStartSample;
                selectedSample.LoopStartSample = vagLoopStartSample;
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

                case EuroSoundAudioCodec.DspAdpcmLegacy:
                    return audioFunctions.ShortArrayToByteArray(new LegacyDspAdpcm().Decode(encodedData));

                case EuroSoundAudioCodec.DspAdpcmNgca:
                    return audioFunctions.ShortArrayToByteArray(new NgcaDspAdpcm().Decode(encodedData));

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
        private static byte[] NormalizeEngineXtIma(byte[] source, int channels, uint sampleCount)
        {
            const int BlockBytes = 32;
            const int SamplesPerBlock = 56;
            channels = Math.Max(1, channels);
            long blocksPerChannel = sampleCount == 0 ? 0 : (sampleCount + SamplesPerBlock - 1L) / SamplesPerBlock;
            long expectedLong = blocksPerChannel * BlockBytes * channels;
            int expectedBytes = expectedLong > int.MaxValue ? source.Length : (int)expectedLong;
            if (expectedBytes <= 0 || expectedBytes > source.Length)
                expectedBytes = source.Length - source.Length % (BlockBytes * channels);

            int alignment = FindImaAlignment(source, expectedBytes, channels);
            if (alignment < 0)
                throw new System.IO.InvalidDataException("EngineXT stream does not contain a valid aligned Eurocom IMA ADPCM block sequence.");

            int available = source.Length - alignment;
            int length = Math.Min(expectedBytes, available);
            length -= length % (BlockBytes * channels);
            byte[] normalized = new byte[length];
            Buffer.BlockCopy(source, alignment, normalized, 0, length);
            return normalized;
        }

        private static int FindImaAlignment(byte[] source, int wantedBytes, int channels)
        {
            int blockSet = 32 * Math.Max(1, channels);
            for (int alignment = 0; alignment < Math.Min(blockSet, source.Length); alignment++)
            {
                int bytes = Math.Min(wantedBytes, source.Length - alignment);
                bytes -= bytes % blockSet;
                bool valid = bytes > 0;
                for (int offset = alignment; valid && offset < alignment + bytes; offset += 32)
                    valid = offset + 2 < source.Length && source[offset + 2] <= 88;
                if (valid) return alignment;
            }
            return -1;
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
        private static byte[][] DecodeEngineXtDsp(byte[] source, int channels, AudioFunctions audioFunctions, bool ngca)
        {
            int regionBytes = source.Length / channels;
            byte[][] result = new byte[channels][];
            for (int channel = 0; channel < channels; channel++)
            {
                int start = channel * regionBytes;
                short[] decoded = ngca
                    ? new NgcaDspAdpcm().Decode(source, start, regionBytes)
                    : new LegacyDspAdpcm().Decode(source, start, regionBytes);
                result[channel] = audioFunctions.ShortArrayToByteArray(decoded);
            }
            return result;
        }
    }
}
