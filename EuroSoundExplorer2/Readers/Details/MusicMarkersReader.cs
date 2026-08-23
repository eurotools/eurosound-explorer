using MusX.Objects;
using System.IO;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class MusicMarkersReader : SfxFunctions
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public MusicMarkers ReadMusicMarkersFile(string filePath, SfxCommonHeader sfxHeaderData)
        {
            MusicMarkers musicMarkers = new MusicMarkers();

            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                BReader.BaseStream.Seek(0x20, SeekOrigin.Begin);

                musicMarkers.MusicHeadersCount = ReadUInt32(BReader, sfxHeaderData);
                musicMarkers.MusicHeadersPadding = ReadUInt32(BReader, sfxHeaderData);
                musicMarkers.MusicMarkerCountsCount = ReadUInt32(BReader, sfxHeaderData);
                musicMarkers.MusicMarkerCountsPadding = ReadUInt32(BReader, sfxHeaderData);
                musicMarkers.MusicMarkerListsCount = ReadUInt32(BReader, sfxHeaderData);
                musicMarkers.MusicMarkerListsPadding = ReadUInt32(BReader, sfxHeaderData);

                long remainingLength = BReader.BaseStream.Length - BReader.BaseStream.Position;
                long regularLength = checked(
                    ((long)musicMarkers.MusicHeadersCount * 16) +
                    ((long)musicMarkers.MusicMarkerCountsCount * 16) +
                    ((long)musicMarkers.MusicMarkerListsCount * 16));
                long xb2Length = checked(
                    ((long)musicMarkers.MusicHeadersCount * 32) +
                    ((long)musicMarkers.MusicMarkerCountsCount * 12) +
                    ((long)musicMarkers.MusicMarkerListsCount * 4));

                // The late version-6 Xbox 360 format interleaves each music header
                // with its marker counts, then stores 12-byte marker summaries and
                // a final array of marker positions.  Older templates described all
                // three arrays as independent 16-byte records.
                bool isXb2Layout = sfxHeaderData.FileVersion == 6 &&
                    EuroSoundCodecMatrix.IsXbox360Platform(sfxHeaderData.Platform) &&
                    remainingLength == xb2Length;

                if (!isXb2Layout && remainingLength < regularLength)
                {
                    throw new InvalidDataException(string.Format(
                        "MusicMarkers layout is invalid: {0} bytes remain, but {1} are required.",
                        remainingLength,
                        regularLength));
                }

                for (int i = 0; i < musicMarkers.MusicHeadersCount; i++)
                {
                    musicMarkers.MusicHeaders.Add(new MusicMarkerHeader
                    {
                        MusicHashCode = ReadUInt32(BReader, sfxHeaderData),
                        StreamDataOffset = ReadUInt32(BReader, sfxHeaderData),
                        BaseVolume = ReadUInt32(BReader, sfxHeaderData),
                        Padding = ReadUInt32(BReader, sfxHeaderData)
                    });

                    if (isXb2Layout)
                    {
                        musicMarkers.MusicMarkerCounts.Add(new MusicMarkerCounts
                        {
                            StartMarkerCount = ReadUInt32(BReader, sfxHeaderData),
                            MarkerCount = ReadUInt32(BReader, sfxHeaderData),
                            Padding0 = ReadUInt32(BReader, sfxHeaderData),
                            Padding1 = ReadUInt32(BReader, sfxHeaderData)
                        });
                    }
                }

                if (isXb2Layout)
                {
                    for (int i = 0; i < musicMarkers.MusicMarkerCountsCount; i++)
                    {
                        musicMarkers.MusicMarkerLists.Add(new MusicMarkerListEntry
                        {
                            Position = ReadUInt32(BReader, sfxHeaderData),
                            LoopStart = ReadInt32(BReader, sfxHeaderData),
                            Padding0 = ReadUInt32(BReader, sfxHeaderData),
                            Padding1 = 0
                        });
                    }

                    for (int i = 0; i < musicMarkers.MusicMarkerListsCount; i++)
                    {
                        musicMarkers.MarkerPositions.Add(ReadUInt32(BReader, sfxHeaderData));
                    }
                }
                else
                {
                    for (int i = 0; i < musicMarkers.MusicMarkerCountsCount; i++)
                    {
                        musicMarkers.MusicMarkerCounts.Add(new MusicMarkerCounts
                        {
                            StartMarkerCount = ReadUInt32(BReader, sfxHeaderData),
                            MarkerCount = ReadUInt32(BReader, sfxHeaderData),
                            Padding0 = ReadUInt32(BReader, sfxHeaderData),
                            Padding1 = ReadUInt32(BReader, sfxHeaderData)
                        });
                    }

                    for (int i = 0; i < musicMarkers.MusicMarkerListsCount; i++)
                    {
                        musicMarkers.MusicMarkerLists.Add(new MusicMarkerListEntry
                        {
                            Position = ReadUInt32(BReader, sfxHeaderData),
                            LoopStart = ReadInt32(BReader, sfxHeaderData),
                            Padding0 = ReadUInt32(BReader, sfxHeaderData),
                            Padding1 = ReadUInt32(BReader, sfxHeaderData)
                        });
                    }
                }
            }

            return musicMarkers;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static uint ReadUInt32(BinaryReader reader, SfxCommonHeader headerData)
        {
            return BytesFunctions.FlipData(reader.ReadUInt32(), headerData.IsBigEndian);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static int ReadInt32(BinaryReader reader, SfxCommonHeader headerData)
        {
            return BytesFunctions.FlipData(reader.ReadInt32(), headerData.IsBigEndian);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
