using MusX.Objects;
using System.Collections.Generic;
using System.IO;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class StreamBankReaderNew : StreamBankReader
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        internal void ReadStreamFile(string filePath, StreambankHeader headerData, List<StreamSample> streamedSamples)
        {
            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                //Go to File Start 1
                BReader.BaseStream.Seek(headerData.FileStart1, SeekOrigin.Begin);

                //Get count of the stored elements
                uint[] storedElements = new uint[headerData.FileLength1 / 4];

                //Read Offsets
                for (int i = 0; i < storedElements.Length; i++)
                {
                    storedElements[i] = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                }

                //Read File Section 2
                for (int i = 0; i < storedElements.Length; i++)
                {
                    BReader.BaseStream.Seek(headerData.FileStart2 + storedElements[i], SeekOrigin.Begin);

                    StreamSample streamSample = new StreamSample
                    {
                        BlockPosition = storedElements[i],
                        MarkerSize = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        AudioOffset = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        AudioSize = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        StartMarkersCount = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        MarkersCount = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        StartMarkerOffset = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        MarkerOffset = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        BaseVolume = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian)
                    };

                    //Stream marker start data
                    streamSample.StartMarkers = new StartMarker[streamSample.StartMarkersCount];
                    for (int j = 0; j < streamSample.StartMarkersCount; j++)
                    {
                        StartMarker startMarker = EuroSoundMarkerReader.ReadNewStartMarker(BReader, headerData);
                        EuroSoundMarkerReader.ConvertMarkerOffsets(startMarker, headerData, EuroSoundBankType.StreamBank, 1);

                        //Add marker
                        streamSample.StartMarkers[j] = startMarker;
                    }

                    //Stream marker data 
                    streamSample.Markers = new Marker[streamSample.MarkersCount];
                    for (int j = 0; j < streamSample.MarkersCount; j++)
                    {
                        Marker DataMarker = EuroSoundMarkerReader.ReadNewMarker(BReader, headerData);
                        EuroSoundMarkerReader.ConvertMarkerOffsets(DataMarker, headerData, EuroSoundBankType.StreamBank, 1);

                        //Add marker
                        streamSample.Markers[j] = DataMarker;
                    }

                    //Read Audio Data
                    EuroSoundAudioCodec codec = EuroSoundCodecMatrix.GetCodec(headerData.FileVersion, headerData.Platform, EuroSoundBankType.StreamBank);
                    streamSample.AudioReference = new AudioDataReference
                    {
                        FilePath = filePath,
                        Offset = headerData.FileStart2 + streamSample.AudioOffset,
                        Size = streamSample.AudioSize,
                        Codec = codec,
                        Frequency = 0,
                        Channels = 1
                    };

                    //Add audio to list
                    streamedSamples.Add(streamSample);
                }

                BReader.Close();
            }
        }

    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
