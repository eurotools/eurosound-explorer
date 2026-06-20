using MusX.Objects;
using System;
using System.Collections.Generic;
using System.IO;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class StreamBankReaderOld : StreamBankReader
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        internal void ReadStreamFile(string filePath, StreambankHeader headerData, List<StreamSample> StreamFileDictionaryData)
        {
            using (BinaryReader binaryReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                //Read Section 1
                uint[] storedElements = new uint[headerData.FileLength1 / 4];

                //Go to section
                binaryReader.BaseStream.Seek(headerData.FileStart1, SeekOrigin.Begin);

                //Read Offsets
                for (int i = 0; i < storedElements.Length; i++)
                {
                    storedElements[i] = BytesFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian);
                }

                //Read Section 2
                for (int i = 0; i < storedElements.Length; i++)
                {
                    binaryReader.BaseStream.Seek(headerData.FileStart2 + storedElements[i], SeekOrigin.Begin);
                    StreamSample StreamSoundToAdd = new StreamSample
                    {
                        BlockPosition = storedElements[i],
                        MarkerSize = BytesFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        AudioOffset = BytesFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        AudioSize = BytesFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        StartMarkersCount = BytesFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        MarkersCount = BytesFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        StartMarkerOffset = BytesFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        MarkerOffset = BytesFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        BaseVolume = BytesFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian)
                    };

                    //Stream marker start data
                    StreamSoundToAdd.StartMarkers = new StartMarker[StreamSoundToAdd.StartMarkersCount];
                    for (int j = 0; j < StreamSoundToAdd.StartMarkersCount; j++)
                    {
                        StartMarker StartMarker = EuroSoundMarkerReader.ReadOldStartMarker(binaryReader, headerData);
                        EuroSoundMarkerReader.ConvertMarkerOffsets(StartMarker, headerData, EuroSoundBankType.StreamBank, 1);
                        //Add marker
                        StreamSoundToAdd.StartMarkers[j] = StartMarker;
                    }

                    //Stream marker data 
                    StreamSoundToAdd.Markers = new Marker[StreamSoundToAdd.MarkersCount];
                    for (int k = 0; k < StreamSoundToAdd.MarkersCount; k++)
                    {
                        Marker DataMarker = EuroSoundMarkerReader.ReadOldMarker(binaryReader, headerData);
                        EuroSoundMarkerReader.ConvertMarkerOffsets(DataMarker, headerData, EuroSoundBankType.StreamBank, 1);
                        //Add marker
                        StreamSoundToAdd.Markers[k] = DataMarker;
                    }

                    //Read Audio Data
                    EuroSoundAudioCodec codec = EuroSoundCodecMatrix.GetCodec(headerData.FileVersion, headerData.Platform, EuroSoundBankType.StreamBank);
                    StreamSoundToAdd.AudioReference = new AudioDataReference
                    {
                        FilePath = filePath,
                        Offset = headerData.FileStart2 + StreamSoundToAdd.AudioOffset,
                        Size = StreamSoundToAdd.AudioSize,
                        Codec = codec,
                        Frequency = 0,
                        Channels = 1
                    };

                    //Add Sound to Dictionary
                    StreamFileDictionaryData.Add(StreamSoundToAdd);
                }
                binaryReader.Close();
            }
        }

    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
