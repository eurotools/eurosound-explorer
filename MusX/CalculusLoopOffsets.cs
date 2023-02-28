using System;

namespace MusX
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    internal static class CalculusLoopOffsets
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint EurocomImaToSamples(uint bytes, int channels)
        {
            int block_align = 0x20 * channels;
            if (channels <= 0) return 0;

            /* DAT4 IMA blocks have a 4 byte header per channel; 2 samples per byte (2 nibbles) */
            long samples = (bytes / block_align) * (block_align - 4 * channels) * 2 / channels
            + (Convert.ToBoolean(bytes % block_align) ? ((bytes % block_align) - 4 * channels) * 2 / channels : 0); /* unlikely (encoder aligns) */

            return (uint)samples;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint XboxAdpcmToSamples(uint bytes, int channels)
        {
            int mod;
            int block_align = 0x24 * channels;
            if (channels <= 0) return 0;

            mod = (int)(bytes % block_align);
            /* XBOX IMA blocks have a 4 byte header per channel; 2 samples per byte (2 nibbles) */
            long samples = (bytes / block_align) * (block_align - 4 * channels) * 2 / channels
                    + ((mod > 0 && mod > 0x04 * channels) ? (mod - 0x04 * channels) * 2 / channels : 0); /* unlikely (encoder aligns) */

            return (uint)samples;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint SonyVagToSamples(uint bytes, int channels)
        {
            if (channels <= 0) return 0;
            return (uint)(bytes / channels / 0x10 * 28);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
