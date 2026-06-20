using MusX.Objects;
using System;
using System.IO;

namespace MusX.Readers
{
    internal static class EuroSoundMarkerReader
    {
        public static StartMarker ReadOldStartMarker(BinaryReader reader, StreambankHeader headerData)
        {
            StartMarker marker = new StartMarker
            {
                Index = ReadInt32(reader, headerData),
                Position = ReadUInt32(reader, headerData),
                Type = (byte)ReadUInt32(reader, headerData),
                Flags = (byte)ReadUInt32(reader, headerData),
                Extra = (byte)ReadUInt32(reader, headerData),
                LoopStart = ReadUInt32(reader, headerData),
                MarkerCount = (int)ReadUInt32(reader, headerData),
                LoopMarkerCount = (int)ReadUInt32(reader, headerData),
                MarkerPos = (int)ReadUInt32(reader, headerData),
                IsInstant = Convert.ToBoolean(ReadUInt32(reader, headerData)),
                InstantBuffer = Convert.ToBoolean(ReadUInt32(reader, headerData)),
                StateA = ReadUInt32(reader, headerData),
                StateB = ReadUInt32(reader, headerData),
                HasExtendedFields = true
            };
            SetOriginalOffsets(marker);
            return marker;
        }

        public static Marker ReadOldMarker(BinaryReader reader, StreambankHeader headerData)
        {
            Marker marker = new Marker
            {
                Index = ReadInt32(reader, headerData),
                Position = ReadUInt32(reader, headerData),
                Type = (byte)ReadUInt32(reader, headerData),
                Flags = (byte)ReadUInt32(reader, headerData),
                Extra = (byte)ReadUInt32(reader, headerData),
                LoopStart = ReadUInt32(reader, headerData),
                MarkerCount = (int)ReadUInt32(reader, headerData),
                LoopMarkerCount = (int)ReadUInt32(reader, headerData),
                HasExtendedFields = true
            };
            SetOriginalOffsets(marker);
            return marker;
        }

        public static StartMarker ReadNewStartMarker(BinaryReader reader, StreambankHeader headerData)
        {
            StartMarker marker = new StartMarker
            {
                Index = ReadInt32(reader, headerData),
                Position = ReadUInt32(reader, headerData),
                Type = (byte)ReadUInt32(reader, headerData),
                LoopStart = ReadUInt32(reader, headerData),
                LoopMarkerCount = ReadInt32(reader, headerData),
                MarkerPos = ReadInt32(reader, headerData),
            };
            SetOriginalOffsets(marker);
            return marker;
        }

        public static Marker ReadNewMarker(BinaryReader reader, StreambankHeader headerData)
        {
            Marker marker = new Marker
            {
                Index = ReadInt32(reader, headerData),
                Position = ReadUInt32(reader, headerData),
                Type = (byte)ReadUInt32(reader, headerData),
                LoopStart = ReadUInt32(reader, headerData),
                LoopMarkerCount = ReadInt32(reader, headerData),
            };
            SetOriginalOffsets(marker);
            return marker;
        }

        public static void ConvertMarkerOffsets(Marker marker, StreambankHeader headerData, EuroSoundBankType bankType, int channels)
        {
            EuroSoundAudioCodec codec = EuroSoundCodecMatrix.GetCodec(headerData.FileVersion, headerData.Platform, bankType);
            marker.Position = EuroSoundCodecMatrix.StreamMarkerOffsetToSamples(codec, marker.Position, channels);
            marker.LoopStart = EuroSoundCodecMatrix.StreamMarkerOffsetToSamples(codec, marker.LoopStart, channels);
        }

        private static void SetOriginalOffsets(Marker marker)
        {
            marker.OriginalPosition = marker.Position;
            marker.OriginalLoopStart = marker.LoopStart;
        }

        private static uint ReadUInt32(BinaryReader reader, StreambankHeader headerData)
        {
            return BytesFunctions.FlipData(reader.ReadUInt32(), headerData.IsBigEndian);
        }

        private static int ReadInt32(BinaryReader reader, StreambankHeader headerData)
        {
            return BytesFunctions.FlipData(reader.ReadInt32(), headerData.IsBigEndian);
        }
    }
}
