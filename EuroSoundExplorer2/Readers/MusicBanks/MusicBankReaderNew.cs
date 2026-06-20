using MusX.Objects;
using System;
using System.IO;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class MusicBankReaderNew
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        internal MusicSample ReadMusicFile(string filePath, StreambankHeader headerData, int interleave_block_size)
        {
            MusicSample musicDat = null;

            using (BinaryReader binaryReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                //Go to File Start 1
                binaryReader.BaseStream.Seek(headerData.FileStart1, SeekOrigin.Begin);

                //Stream marker header data 
                musicDat = new MusicSample
                {
                    //Properties
                    StartMarkersCount = BytesFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                    MarkersCount = BytesFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                    StartMarkerOffset = BytesFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                    MarkerOffset = BytesFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                    BaseVolume = BytesFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian)
                };

                //Read Start Markers
                musicDat.StartMarkers = new StartMarker[musicDat.StartMarkersCount];
                for (int j = 0; j < musicDat.StartMarkersCount; j++)
                {
                    StartMarker StartMarker = EuroSoundMarkerReader.ReadNewStartMarker(binaryReader, headerData);
                    EuroSoundMarkerReader.ConvertMarkerOffsets(StartMarker, headerData, EuroSoundBankType.MusicBank, 2);

                    //Add marker
                    musicDat.StartMarkers[j] = StartMarker;
                }

                //Read Markers
                musicDat.Markers = new Marker[musicDat.MarkersCount];
                for (int k = 0; k < musicDat.MarkersCount; k++)
                {
                    Marker DataMarker = EuroSoundMarkerReader.ReadNewMarker(binaryReader, headerData);
                    EuroSoundMarkerReader.ConvertMarkerOffsets(DataMarker, headerData, EuroSoundBankType.MusicBank, 2);

                    //Add marker
                    musicDat.Markers[k] = DataMarker;
                }

                //Read Section 2
                //Seek Position
                binaryReader.BaseStream.Seek(headerData.FileStart2, SeekOrigin.Begin);

                //Save offset
                musicDat.AudioOffset = (uint)binaryReader.BaseStream.Position;
                musicDat.AudioSize = headerData.FileLength2;
                EuroSoundAudioCodec codec = EuroSoundCodecMatrix.GetCodec(headerData.FileVersion, headerData.Platform, EuroSoundBankType.MusicBank);
                AudioDataReference musicAudioReference = new AudioDataReference
                {
                    FilePath = filePath,
                    Offset = musicDat.AudioOffset,
                    Size = musicDat.AudioSize,
                    Codec = codec,
                    Frequency = 0,
                    Channels = 2,
                    InterleaveBlockSize = interleave_block_size
                };
                musicDat.AudioReferences[0] = musicAudioReference;
                musicDat.AudioReferences[1] = musicAudioReference;
            }

            return musicDat;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
    }
}
