//-------------------------------------------------------------------------------------------------------------------------------
//  ______                                           _ 
// |  ____|                                         | |
// | |__   _   _ _ __ ___  ___  ___  _   _ _ __   __| |
// |  __| | | | | '__/ _ \/ __|/ _ \| | | | '_ \ / _` |
// | |____| |_| | | | (_) \__ \ (_) | |_| | | | | (_| |
// |______|\__,_|_|  \___/|___/\___/ \__,_|_| |_|\__,_|
//
//-------------------------------------------------------------------------------------------------------------------------------
// CALCULUS LOOP OFFSETS (To Samples)
//-------------------------------------------------------------------------------------------------------------------------------

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
            uint BlockBytes = 32;
            uint BlockSamples = 56;

            return BlockBytesToSamples(bytes, channels, BlockBytes, BlockSamples);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint XboxAdpcmToSamples(uint bytes, int channels)
        {
            uint BlockBytes = 36;
            uint BlockSamples = 64;

            return BlockBytesToSamples(bytes, channels, BlockBytes, BlockSamples);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint ImaAdpcmToSamples(uint bytes, int channels)
        {
            return (bytes * 2) / (uint)channels;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint SonyVagToSamples(uint bytes, int channels)
        {
            uint BlockBytes = 16;
            uint BlockSamples = 28;

            return BlockBytesToSamples(bytes, channels, BlockBytes, BlockSamples);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint DspAdpcmToSamples(uint bytes, int channels)
        {
            uint nibbles = bytes * 2;
            uint frames = nibbles / 16;
            uint extraNibbles = nibbles % 16;
            uint extraSamples = extraNibbles < 2 ? 0 : extraNibbles - 2;

            return ((frames * 14) + extraSamples) / (uint)channels;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint Pcm16BytesToSamples(uint bytes, int channels)
        {
            return (bytes / 2) / (uint)channels;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static uint BlockBytesToSamples(uint bytes, int channels, uint blockBytes, uint blockSamples)
        {
            return (uint)(((ulong)bytes * blockSamples / blockBytes) / (uint)channels);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
