using MusX.Objects;
using System;

namespace MusX
{
    internal static class EuroSoundStreamLoopResolver
    {
        internal static bool TryResolveV18(StreamSample sample, int decodedSampleCount, out uint loopStart, out uint loopEndExclusive)
        {
            loopStart = 0;
            loopEndExclusive = 0;
            if (sample == null || (sample.Flags & 1) == 0 || sample.LoopStartSample == uint.MaxValue || decodedSampleCount <= 0)
                return false;

            uint availableSamples = (uint)decodedSampleCount;
            uint logicalSamples = sample.SampleCount == 0 ? availableSamples : Math.Min(sample.SampleCount, availableSamples);
            loopStart = Math.Min(sample.LoopStartSample, logicalSamples);

            int channels = sample.AudioReference == null ? (int)sample.Channels : sample.AudioReference.Channels;
            channels = Math.Max(1, channels);
            EuroSoundAudioCodec codec = sample.AudioReference == null ? EuroSoundAudioCodec.Unknown : sample.AudioReference.Codec;
            uint encodedEnd = sample.LoopEndByteOffset;
            uint convertedEnd = encodedEnd == 0 || encodedEnd == uint.MaxValue
                ? 0
                : EuroSoundCodecMatrix.EncodedByteCountToSamples(codec, encodedEnd, channels);

            // The encoded end is block-aligned and exclusive. SampleCount is the
            // logical (unpadded) length, so it also limits the decoded endpoint.
            loopEndExclusive = convertedEnd == 0 ? logicalSamples : Math.Min(convertedEnd, logicalSamples);
            return loopStart < loopEndExclusive;
        }

        internal static bool TryResolveV18(SampleData sample, int decodedSampleCount, out uint loopStart, out uint loopEndExclusive)
        {
            loopStart = 0;
            loopEndExclusive = 0;
            if (sample == null || !sample.IsLooped || decodedSampleCount <= 0)
                return false;

            uint availableSamples = (uint)decodedSampleCount;
            uint logicalSamples = sample.TotalSamples == 0 ? availableSamples : Math.Min(sample.TotalSamples, availableSamples);
            loopStart = Math.Min(sample.LoopStartSample, logicalSamples);

            int channels = sample.AudioReference == null ? (int)sample.Channels : sample.AudioReference.Channels;
            channels = Math.Max(1, channels);
            EuroSoundAudioCodec codec = sample.AudioReference == null ? EuroSoundAudioCodec.Unknown : sample.AudioReference.Codec;
            uint convertedEnd = sample.LoopEndByteOffset == 0 || sample.LoopEndByteOffset == uint.MaxValue
                ? 0
                : EuroSoundCodecMatrix.EncodedByteCountToSamples(codec, sample.LoopEndByteOffset, channels);
            loopEndExclusive = convertedEnd == 0 ? logicalSamples : Math.Min(convertedEnd, logicalSamples);
            return loopStart < loopEndExclusive;
        }
    }
}
