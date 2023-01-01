namespace MusX
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    internal static class CalculusLoopOffsets
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint ReverseGetMusicLoopOffsetPCandGC(uint loopOffsetGC)
        {
            // 1.378125 comes from 44100Hz / 32000Hz
            double ruleOfThree = loopOffsetGC / 4.0;
            return (uint)(ruleOfThree * 1.378125);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint ReverseGetMusicLoopOffsetPlayStation2(uint loopOffsetPS2)
        {
            uint pcResult = (uint)(loopOffsetPS2 * 3.5);
            return ReverseGetMusicLoopOffsetPCandGC(pcResult);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint ReverseGetMusicLoopOffsetXbox(uint loopOffsetXbox)
        {
            return (uint)(loopOffsetXbox * 0.88888887);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint ReverseGetMusicLoopOffsetXboxNew(uint loopOffsetXbox)
        {
            uint positionAligned = (uint)(loopOffsetXbox * 3.5003);
            return ReverseGetStreamLoopOffsetPCandGC(positionAligned);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint ReverseGetStreamLoopOffsetPCandGC(uint markerPosition)
        {
            uint PositionAligned;

            //Align position
            if ((markerPosition & 1) == 1) //Odd number
            {
                PositionAligned = ((markerPosition + 1) / 4 * 4);
            }
            else //Even number
            {
                PositionAligned = AlignNumber(markerPosition, 4);
            }
            return PositionAligned;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint ReverseGetStreamLoopOffsetPlayStation2(uint loopOffsetPS2)
        {
            uint positionAligned = (uint)(loopOffsetPS2 * 3.5);
            return ReverseGetStreamLoopOffsetPCandGC(positionAligned);
        }

        public static uint ReverseGetStreamLoopOffsetPlayStation2New(uint loopOffsetPS2)
        {
            uint positionAligned = (uint)(loopOffsetPS2 * 8.5);
            return ReverseGetStreamLoopOffsetPCandGC(positionAligned);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint ReverseGetStreamLoopOffsetXbox(uint loopOffsetXbox)
        {
            uint parsedLoopOffset = (uint)(loopOffsetXbox * 1.7777777777);
            uint positionAligned = parsedLoopOffset * 2;
            return ReverseGetStreamLoopOffsetPCandGC(positionAligned);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint ReverseGetXboxAlignedNumber(uint inputValue)
        {
            uint alignedNumber = 0;
            if (inputValue > 31)
            {
                alignedNumber = (((inputValue / 36) - 1) * 64) + 32;
            }
            return alignedNumber;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint AlignNumber(uint valueToAlign, uint blockSize)
        {
            uint PositionAligned = (valueToAlign + (blockSize - 1)) & ~(blockSize - 1);
            return PositionAligned;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
