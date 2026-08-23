using System;

namespace AudioDecoders
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class DspAdpcm
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        private readonly int SamplesPerFrame = 14, NibblesPerFrame = 16;
        private readonly sbyte[] SignedNibbles = { 0, 1, 2, 3, 4, 5, 6, 7, -8, -7, -6, -5, -4, -3, -2, -1 };

        //-------------------------------------------------------------------------------------------------------------------------------
        private class GcAdpcmParameters
        {
            public int SampleCount { get; set; } = -1;
            public short History1 { get; set; }
            public short History2 { get; set; }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public short[] Decode(byte[] adpcm, short[] coefficients)
        {
            if (adpcm == null) throw new ArgumentNullException("adpcm");
            if (coefficients == null) throw new ArgumentNullException("coefficients");
            if (coefficients.Length < 16) throw new System.IO.InvalidDataException("DSP ADPCM requires 16 coefficients.");
            GcAdpcmParameters config = new GcAdpcmParameters
            {
                SampleCount = NibbleCountToSampleCount(adpcm.Length * 2)
            };
            short[] pcm = new short[config.SampleCount];

            if (config.SampleCount > 0)
            {
                int frameCount = (int)Math.Ceiling((double)config.SampleCount / SamplesPerFrame);
                int currentSample = 0, outIndex = 0, inIndex = 0;
                short hist1 = config.History1;
                short hist2 = config.History2;

                for (int i = 0; i < frameCount; i++)
                {
                    byte predictorScale = adpcm[inIndex++];
                    int scale = (1 << (byte)(predictorScale & 0xF)) * 2048;
                    int predictor = (byte)((predictorScale >> 4) & 0xF);
                    if (predictor >= 8) throw new System.IO.InvalidDataException("Invalid DSP ADPCM predictor index: " + predictor + ".");
                    short coef1 = coefficients[predictor * 2];
                    short coef2 = coefficients[predictor * 2 + 1];

                    int samplesToRead = Math.Min(SamplesPerFrame, config.SampleCount - currentSample);
                    for (int s = 0; s < samplesToRead; s++)
                    {
                        int adpcmSample;
                        if (s % 2 == 0)
                        {
                            adpcmSample = GetHighNibbleSigned(adpcm[inIndex]);
                        }
                        else
                        {
                            adpcmSample = GetLowNibbleSigned(adpcm[inIndex++]);
                        }

                        int distance = scale * adpcmSample;
                        int predictedSample = coef1 * hist1 + coef2 * hist2;
                        int correctedSample = predictedSample + distance;
                        int scaledSample = (correctedSample + 1024) >> 11;
                        short clampedSample = Clamp16(scaledSample);

                        hist2 = hist1;
                        hist1 = clampedSample;

                        pcm[outIndex++] = clampedSample;
                        currentSample++;
                    }
                }
            }
            return pcm;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private sbyte GetHighNibbleSigned(byte value)
        {
            return SignedNibbles[(value >> 4) & 0xF];
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private sbyte GetLowNibbleSigned(byte value)
        {
            return SignedNibbles[value & 0xF];
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private short Clamp16(int value)
        {
            int clampedVal = value;
            if (value > short.MaxValue)
            {
                clampedVal = short.MaxValue;
            }

            if (value < short.MinValue)
            {
                clampedVal = short.MinValue;
            }

            return (short)clampedVal;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private int NibbleCountToSampleCount(int nibbleCount)
        {
            int frames = nibbleCount / NibblesPerFrame;
            int extraNibbles = nibbleCount % NibblesPerFrame;
            int extraSamples = extraNibbles < 2 ? 0 : extraNibbles - 2;

            return SamplesPerFrame * frames + extraSamples;
        }
    }

    // EngineXT v15/v18 stores one 0x60-byte big-endian DSP header per channel.
    public sealed class LegacyDspAdpcm
    {
        public const int HeaderSize = 0x60;

        public short[] Decode(byte[] container)
        {
            return Decode(container, 0, container == null ? 0 : container.Length);
        }

        public short[] Decode(byte[] container, int offset, int length)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (offset < 0 || length < HeaderSize || offset > container.Length - length)
                throw new System.IO.InvalidDataException("Legacy DSP ADPCM requires a 0x60-byte channel header.");
            short[] coefficients = ReadBigEndianCoefficients(container, offset + 0x1c);
            byte[] payload = new byte[length - HeaderSize];
            Buffer.BlockCopy(container, offset + HeaderSize, payload, 0, payload.Length);
            return new DspAdpcm().Decode(payload, coefficients);
        }

        internal static short[] ReadBigEndianCoefficients(byte[] source, int offset)
        {
            if (offset < 0 || offset > source.Length - 32)
                throw new System.IO.InvalidDataException("DSP ADPCM coefficient table is truncated.");
            short[] result = new short[16];
            for (int i = 0; i < result.Length; i++)
                result[i] = unchecked((short)((source[offset + i * 2] << 8) | source[offset + i * 2 + 1]));
            return result;
        }
    }

    // EngineXT v21 uses the compact 0x40-byte NGCA channel header.
    public sealed class NgcaDspAdpcm
    {
        public const int HeaderSize = 0x40;

        public short[] Decode(byte[] container)
        {
            return Decode(container, 0, container == null ? 0 : container.Length);
        }

        public short[] Decode(byte[] container, int offset, int length)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (offset < 0 || length < HeaderSize || offset > container.Length - length ||
                container[offset] != (byte)'N' || container[offset + 1] != (byte)'G' ||
                container[offset + 2] != (byte)'C' || container[offset + 3] != (byte)'A')
                throw new System.IO.InvalidDataException("NGCA DSP ADPCM requires a valid 0x40-byte NGCA channel header.");
            short[] coefficients = LegacyDspAdpcm.ReadBigEndianCoefficients(container, offset + 0x0c);
            byte[] payload = new byte[length - HeaderSize];
            Buffer.BlockCopy(container, offset + HeaderSize, payload, 0, payload.Length);
            return new DspAdpcm().Decode(payload, coefficients);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
