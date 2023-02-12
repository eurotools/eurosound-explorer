namespace MusX
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    internal static class CalculusLoopOffsets
    {
        //-------------------------------------------------------------------------------------------
        //  SOUNDBANKS - OLD
        //-------------------------------------------------------------------------------------------
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
        public static uint GetSoundBankEurocomImaLoopOffset(uint loopOffsetPS2)
        {
            uint positionAligned = (uint)(loopOffsetPS2 * 3.498389);
            return GetLoopOffsetAligned(positionAligned);
        }

        //-------------------------------------------------------------------------------------------
        //  STREAMBANKS - OLD
        //-------------------------------------------------------------------------------------------
        public static uint GetStreamLoopOffsetPlayStation2(uint loopOffsetPS2)
        {
            uint positionAligned = (uint)(loopOffsetPS2 * 3.5);
            return GetLoopOffsetAligned(positionAligned);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint GetStreamLoopOffsetXbox(uint loopOffsetXbox)
        {
            uint parsedLoopOffset = (uint)(loopOffsetXbox * 1.7777777777);
            uint positionAligned = parsedLoopOffset * 2;
            return GetLoopOffsetAligned(positionAligned);
        }

        //-------------------------------------------------------------------------------------------
        //  MUSICBANKS - NEW
        //-------------------------------------------------------------------------------------------
        public static uint GetMusicLoopOffsetXboxNew(uint loopOffsetXbox)
        {
            uint positionAligned = (uint)(loopOffsetXbox * 3.5003);
            return GetLoopOffsetAligned(positionAligned);
        }

        //-------------------------------------------------------------------------------------------
        //  FUNCTIONS
        //-------------------------------------------------------------------------------------------
        public static uint GetLoopOffsetAligned(uint markerPosition)
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
        public static uint AlignNumber(uint valueToAlign, uint blockSize)
        {
            uint PositionAligned = (valueToAlign + (blockSize - 1)) & ~(blockSize - 1);
            return PositionAligned;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
