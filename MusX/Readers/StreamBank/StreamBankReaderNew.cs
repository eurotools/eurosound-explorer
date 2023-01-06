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
                    storedElements[i] = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                }

                //Read File Section 2
                for (int i = 0; i < storedElements.Length; i++)
                {
                    BReader.BaseStream.Seek(headerData.FileStart2 + storedElements[i], SeekOrigin.Begin);

                    StreamSample streamSample = new StreamSample
                    {
                        BlockPosition = storedElements[i],
                        MarkerSize = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        AudioOffset = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        AudioSize = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        StartMarkersCount = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        MarkersCount = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        StartMarkerOffset = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        MarkerOffset = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        BaseVolume = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian)
                    };

                    //Stream marker start data
                    streamSample.StartMarkers = new StartMarker[streamSample.StartMarkersCount];
                    for (int j = 0; j < streamSample.StartMarkersCount; j++)
                    {
                        StartMarker startMarker = new StartMarker
                        {
                            Index = BinaryFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian),
                            Position = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                            Type = (byte)BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                            LoopStart = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                            LoopMarkerCount = BinaryFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian),
                            MarkerPos = BinaryFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian),
                        };

                        //Parse loop Offsets
                        startMarker.Position = CalculusLoopOffsets.GetStreamLoopOffsetPlayStation2(startMarker.Position);
                        startMarker.LoopStart = CalculusLoopOffsets.GetStreamLoopOffsetPlayStation2(startMarker.LoopStart);

                        //Add marker
                        streamSample.StartMarkers[j] = startMarker;
                    }

                    //Stream marker data 
                    streamSample.Markers = new Marker[streamSample.MarkersCount];
                    for (int j = 0; j < streamSample.MarkersCount; j++)
                    {
                        Marker DataMarker = new Marker
                        {
                            Index = BinaryFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian),
                            Position = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                            Type = (byte)BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                            LoopStart = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                            LoopMarkerCount = BinaryFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian),
                        };

                        //Parse loop Offsets
                        DataMarker.Position = CalculusLoopOffsets.GetStreamLoopOffsetPlayStation2(DataMarker.Position);
                        DataMarker.LoopStart = CalculusLoopOffsets.GetStreamLoopOffsetPlayStation2(DataMarker.LoopStart);

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
