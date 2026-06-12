//-------------------------------------------------------------------------------------------------------------------------------
//  ______                                           _ 
// |  ____|                                         | |
// | |__   _   _ _ __ ___  ___  ___  _   _ _ __   __| |
// |  __| | | | | '__/ _ \/ __|/ _ \| | | | '_ \ / _` |
// | |____| |_| | | | (_) \__ \ (_) | |_| | | | | (_| |
// |______|\__,_|_|  \___/|___/\___/ \__,_|_| |_|\__,_|
//
//-------------------------------------------------------------------------------------------------------------------------------
// BYTES FUNCTIONS
//-------------------------------------------------------------------------------------------------------------------------------
using System;
using System.IO;

namespace MusX
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    internal static class BytesFunctions
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public static string FormatBytes(long bytes)
        {
            string[] suffix = new string[] { "bytes", "KB", "MB", "GB", "TB" };
            long fileBytes = bytes;

            int i = 0;
            double dblBytes = bytes;
            if (bytes > 1024)
            {
                for (i = 0; (bytes / 1024) > 0 && i < suffix.Length - 1; i++, bytes /= 1024)
                {
                    dblBytes = bytes / 1024.0;
                }
            }

            return string.Format("{0:0.##} {1} ({2:#,##0} bytes)", dblBytes, suffix[i], fileBytes);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal static uint FlipData(uint valueToFlip, bool isBigEndian)
        {
            if (!isBigEndian)
                return valueToFlip;

            return ((valueToFlip & 0xFF000000) >> 24) |
                   ((valueToFlip & 0x00FF0000) >> 8) |
                   ((valueToFlip & 0x0000FF00) << 8) |
                   ((valueToFlip & 0x000000FF) << 24);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal static int FlipData(int valueToFlip, bool isBigEndian)
        {
            if (!isBigEndian)
                return valueToFlip;

            uint value = unchecked((uint)valueToFlip);

            uint flipped = ((value & 0xFF000000) >> 24) |
                           ((value & 0x00FF0000) >> 8) |
                           ((value & 0x0000FF00) << 8) |
                           ((value & 0x000000FF) << 24);

            return unchecked((int)flipped);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal static short FlipData(short valueToFlip, bool isBigEndian)
        {
            if (!isBigEndian)
                return valueToFlip;

            ushort value = unchecked((ushort)valueToFlip);

            ushort flipped = (ushort)(((value & 0xFF00) >> 8) |
                                      ((value & 0x00FF) << 8));

            return unchecked((short)flipped);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal static ushort FlipData(ushort valueToFlip, bool isBigEndian)
        {
            if (!isBigEndian)
                return valueToFlip;

            return (ushort)(((valueToFlip & 0xFF00) >> 8) |
                            ((valueToFlip & 0x00FF) << 8));
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal static float FlipData(float valueToFlip, bool isBigEndian)
        {
            if (!isBigEndian)
                return valueToFlip;

            byte[] data = BitConverter.GetBytes(valueToFlip);
            Array.Reverse(data);
            return BitConverter.ToSingle(data, 0);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint AlignNumber(uint valueToAlign, uint blockSize)
        {
            uint PositionAligned = (valueToAlign + (blockSize - 1)) & ~(blockSize - 1);
            return PositionAligned;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal static void WriteAlignedDecoration(BinaryWriter bw, uint PositionAligned)
        {
            while (bw.BaseStream.Position != PositionAligned)
            {
                bw.Write((byte)0xAB);
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
