using MusX.Objects;
using System;
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

            if (sfxHeaderData.FileVersion == 10)
            {
                ReadMusicMarkersFileVersion10(filePath, musicMarkers);
                return musicMarkers;
            }

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
        private static void ReadMusicMarkersFileVersion10(string filePath, MusicMarkers musicMarkers)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                reader.BaseStream.Position = 0x800;
                if (ReadFourCC(reader) != "FORM") throw new InvalidDataException("MUSX v10 MusicMarkers has no FORM container.");
                uint rootSize = reader.ReadUInt32();
                if (ReadFourCC(reader) != "MRKF") throw new InvalidDataException("MUSX v10 MusicMarkers has no MRKF form.");
                long rootEnd = Math.Min(reader.BaseStream.Length, 0x800L + 8L + rootSize);

                while (reader.BaseStream.Position + 12 <= rootEnd)
                {
                    long formStart = reader.BaseStream.Position;
                    if (ReadFourCC(reader) != "FORM") break;
                    uint formSize = reader.ReadUInt32();
                    long formEnd = Math.Min(rootEnd, formStart + 8L + formSize);
                    if (ReadFourCC(reader) != "AMUS")
                    {
                        reader.BaseStream.Position = formEnd;
                        continue;
                    }

                    uint hashCode = 0;
                    uint baseVolume = 0;
                    uint position = 0;
                    int loopStart = -1;
                    uint streamOffset = 0;

                    while (reader.BaseStream.Position + 8 <= formEnd)
                    {
                        long chunkStart = reader.BaseStream.Position;
                        string chunkId = ReadFourCC(reader);
                        uint chunkSize = reader.ReadUInt32();
                        long chunkEnd = Math.Min(formEnd, chunkStart + 8L + chunkSize);

                        if (chunkId == "MHDR" && chunkSize >= 8)
                        {
                            hashCode = reader.ReadUInt32();
                            baseVolume = reader.ReadUInt32();
                        }
                        else if (chunkId == "MKLS" && chunkSize >= 8)
                        {
                            position = reader.ReadUInt32();
                            loopStart = reader.ReadInt32();
                        }
                        else if (chunkId == "STLS" && chunkSize >= 4)
                        {
                            streamOffset = reader.ReadUInt32();
                        }

                        reader.BaseStream.Position = chunkEnd + (chunkSize & 1);
                    }

                    musicMarkers.MusicHeaders.Add(new MusicMarkerHeader
                    {
                        MusicHashCode = hashCode,
                        StreamDataOffset = streamOffset,
                        BaseVolume = baseVolume,
                        Padding = 0
                    });
                    musicMarkers.MusicMarkerCounts.Add(new MusicMarkerCounts
                    {
                        StartMarkerCount = loopStart >= 0 ? 1u : 0u,
                        MarkerCount = 1,
                        Padding0 = 0,
                        Padding1 = 0
                    });
                    musicMarkers.MusicMarkerLists.Add(new MusicMarkerListEntry
                    {
                        Position = position,
                        LoopStart = loopStart,
                        Padding0 = 0,
                        Padding1 = 0
                    });

                    reader.BaseStream.Position = formEnd + (formSize & 1);
                }

                musicMarkers.MusicHeadersCount = (uint)musicMarkers.MusicHeaders.Count;
                musicMarkers.MusicMarkerCountsCount = (uint)musicMarkers.MusicMarkerCounts.Count;
                musicMarkers.MusicMarkerListsCount = (uint)musicMarkers.MusicMarkerLists.Count;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static string ReadFourCC(BinaryReader reader)
        {
            return System.Text.Encoding.ASCII.GetString(reader.ReadBytes(4));
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
