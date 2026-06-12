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
                        StartMarker startMarker = new StartMarker
                        {
                            Index = BytesFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian),
                            Position = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                            Type = (byte)BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                            LoopStart = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                            LoopMarkerCount = BytesFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian),
                            MarkerPos = BytesFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian),
                        };

                        //Parse loop Offsets
                        startMarker.Position = CalculusLoopOffsets.EurocomImaToSamples(startMarker.Position, 1);
                        startMarker.LoopStart = CalculusLoopOffsets.EurocomImaToSamples(startMarker.LoopStart, 1);

                        //Add marker
                        streamSample.StartMarkers[j] = startMarker;
                    }

                    //Stream marker data 
                    streamSample.Markers = new Marker[streamSample.MarkersCount];
                    for (int j = 0; j < streamSample.MarkersCount; j++)
                    {
                        Marker DataMarker = new Marker
                        {
                            Index = BytesFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian),
                            Position = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                            Type = (byte)BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                            LoopStart = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                            LoopMarkerCount = BytesFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian),
                        };

                        //Parse loop Offsets
                        DataMarker.Position = CalculusLoopOffsets.EurocomImaToSamples(DataMarker.Position, 1);
                        DataMarker.LoopStart = CalculusLoopOffsets.EurocomImaToSamples(DataMarker.LoopStart, 1);

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
