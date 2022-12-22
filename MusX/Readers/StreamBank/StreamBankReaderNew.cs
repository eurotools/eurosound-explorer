using MusX.Objects;
using System.Collections.Generic;
using System.IO;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class StreamBankReaderNew
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        internal void ReadStreamFile(string filePath, SfxHeaderData headerData, List<StreamSample> streamedSamples)
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
                    storedElements[i] = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian);
                }

                //Read File Section 2
                for (int i = 0; i < storedElements.Length; i++)
                {
                    BReader.BaseStream.Seek(headerData.FileStart2 + storedElements[i], SeekOrigin.Begin);

                    StreamSample streamSample = new StreamSample
                    {
                        BlockPosition = storedElements[i],
                        MarkerSize = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian),
                        AudioOffset = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian),
                        AudioSize = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian),
                        StartMarkersCount = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian),
                        MarkersCount = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian),
                        StartMarkerOffset = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian),
                        MarkerOffset = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian),
                        BaseVolume = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian)
                    };

                    //Stream marker start data
                    streamSample.StartMarkers = new StartMarker[streamSample.StartMarkersCount];
                    for (int j = 0; j < streamSample.StartMarkersCount; j++)
                    {
                        StartMarker startMarker = new StartMarker
                        {
                            Index = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian),
                            Position = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian),
                            Type = (byte)BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian),
                            LoopStart = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian),
                            LoopMarkerCount = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian),
                            MarkerPos = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian),
                        };

                        //Add marker
                        streamSample.StartMarkers[j] = startMarker;
                    }

                    //Stream marker data 
                    streamSample.Markers = new Marker[streamSample.MarkersCount];
                    for (int j = 0; j < streamSample.MarkersCount; j++)
                    {
                        Marker DataMarker = new Marker
                        {
                            Index = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian),
                            Position = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian),
                            Type = (byte)BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian),
                            LoopStart = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian),
                            LoopMarkerCount = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian),
                        };

                        //Add marker
                        streamSample.Markers[j] = DataMarker;
                    }

                    //Read Audio Data
                    BReader.BaseStream.Seek(headerData.FileStart2 + streamSample.AudioOffset, SeekOrigin.Begin);
                    streamSample.EncodedData = BReader.ReadBytes((int)streamSample.AudioSize);

                    //Add audio to list
                    streamedSamples.Add(streamSample);
                }

                BReader.Close();
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
