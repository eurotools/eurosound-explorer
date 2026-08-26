using System;
using System.IO;

namespace AudioDecoders
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class SonyAdpcm
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        private static readonly double[,] VAGLut = new double[,]
        {
            {0.0, 0.0},
            {60.0 / 64.0, 0.0},
            {115.0 / 64.0, -52.0 / 64.0},
            {98.0 / 64.0, -55.0 / 64.0},
            {122.0 / 64.0, -60.0 / 64.0}
        };

        //-------------------------------------------------------------------------------------------------------------------------------
        private struct VAGChunk
        {
            public sbyte shift;
            public sbyte predict; /* swy: reversed nibbles due to little-endian */
            public byte flags;
            public byte[] sample;
        };

        //-------------------------------------------------------------------------------------------------------------------------------
        private enum VAGFlag
        {
            VAGF_NOTHING = 0,         /* Nothing*/
            VAGF_LOOP_LAST_BLOCK = 1, /* Last block to loop */
            VAGF_LOOP_REGION = 2,     /* Loop region*/
            VAGF_LOOP_END = 3,        /* Ending block of the loop */
            VAGF_LOOP_FIRST_BLOCK = 4,/* First block of looped data */
            VAGF_UNK = 5,             /* ?*/
            VAGF_LOOP_START = 6,      /* Starting block of the loop*/
            VAGF_PLAYBACK_END = 7     /* Playback ending position */
        };

        //-------------------------------------------------------------------------------------------------------------------------------
        private static readonly int VAG_SAMPLE_BYTES = 14;
        private readonly int VAG_SAMPLE_NIBBL = VAG_SAMPLE_BYTES * 2;

        //-------------------------------------------------------------------------------------------------------------------------------
        public byte[] Decode(byte[] vagData, ref uint loopOffset)
        {
            return Decode(vagData, 16, ref loopOffset);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public byte[] DecodeRaw(byte[] vagData, ref uint loopOffset)
        {
            return Decode(vagData, 0, ref loopOffset);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private byte[] Decode(byte[] vagData, int dataOffset, ref uint loopOffset)
        {
            if (vagData == null)
            {
                throw new ArgumentNullException("vagData");
            }
            if (dataOffset < 0 || dataOffset > vagData.Length)
            {
                throw new InvalidDataException("Invalid VAG data offset.");
            }
            if ((vagData.Length - dataOffset) % 16 != 0)
            {
                throw new InvalidDataException("VAG data contains a truncated ADPCM block.");
            }

            byte[] pcmData;

            using (BinaryReader VagReader = new BinaryReader(new MemoryStream(vagData, false)))
            using (MemoryStream PCMStream = new MemoryStream())
            using (BinaryWriter PCMWriter = new BinaryWriter(PCMStream))
            {
                double hist_1 = 0.0, hist_2 = 0.0;

                VagReader.BaseStream.Seek(dataOffset, SeekOrigin.Begin);

                //Start decoding
                while (VagReader.BaseStream.Position < VagReader.BaseStream.Length)
                {
                    long blockPosition = VagReader.BaseStream.Position;
                    if (vagData.Length - blockPosition >= 16)
                    {
                        bool isSectorPadding = true;
                        for (int paddingIndex = 0; paddingIndex < 16; paddingIndex++)
                        {
                            if (vagData[blockPosition + paddingIndex] != 0xAB)
                            {
                                isSectorPadding = false;
                                break;
                            }
                        }
                        if (isSectorPadding) break;
                    }

                    //Read chunk data
                    byte DecodingCoefficent = VagReader.ReadByte();
                    VAGChunk vc = new VAGChunk
                    {
                        shift = (sbyte)(DecodingCoefficent & 0xF),
                        predict = (sbyte)((DecodingCoefficent & 0xF0) >> 4),
                        flags = VagReader.ReadByte(),
                        sample = VagReader.ReadBytes(14)
                    };

                    if (vc.predict < 0 || vc.predict >= VAGLut.GetLength(0))
                    {
                        throw new InvalidDataException("Invalid VAG predictor index: " + vc.predict + ".");
                    }
                    if (vc.shift < 0 || vc.shift > 12)
                    {
                        throw new InvalidDataException("Invalid VAG shift value: " + vc.shift + ".");
                    }

                    if (vc.flags == (byte)VAGFlag.VAGF_PLAYBACK_END)
                    {
                        break;
                    }

                    if (vc.flags == (byte)VAGFlag.VAGF_LOOP_START)
                    {
                        loopOffset = (uint)PCMStream.Length;
                    }

                    int[] samples = new int[VAG_SAMPLE_NIBBL];

                    // expand 4bit -> 8bit
                    for (int j = 0; j < VAG_SAMPLE_BYTES; j++)
                    {
                        samples[j * 2] = vc.sample[j] & 0xF;
                        samples[j * 2 + 1] = (vc.sample[j] & 0xF0) >> 4;
                    }

                    //Decode samples
                    for (int j = 0; j < VAG_SAMPLE_NIBBL; j++)
                    {
                        // shift 4 bits to top range of int16_t
                        int s = samples[j] << 12;
                        if ((s & 0x8000) != 0)
                        {
                            s = (int)(s | 0xFFFF0000);
                        }

                        double sample = (s >> vc.shift) + hist_1 * VAGLut[vc.predict, 0] + hist_2 * VAGLut[vc.predict, 1];
                        hist_2 = hist_1;
                        hist_1 = sample;

                        PCMWriter.Write((short)(Math.Min(short.MaxValue, Math.Max(sample, short.MinValue))));
                    }
                }
                pcmData = PCMStream.ToArray();

                PCMWriter.Close();
                PCMStream.Close();
                VagReader.Close();
            }

            return pcmData;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
