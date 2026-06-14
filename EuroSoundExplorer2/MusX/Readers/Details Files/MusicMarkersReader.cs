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

                for (int i = 0; i < musicMarkers.MusicHeadersCount; i++)
                {
                    musicMarkers.MusicHeaders.Add(new MusicMarkerHeader
                    {
                        MusicHashCode = ReadUInt32(BReader, sfxHeaderData),
                        StreamDataOffset = ReadUInt32(BReader, sfxHeaderData),
                        BaseVolume = ReadUInt32(BReader, sfxHeaderData),
                        Padding = ReadUInt32(BReader, sfxHeaderData)
                    });
                }

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
