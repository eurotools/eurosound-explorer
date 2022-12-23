using MusX.Objects;
using System;
using System.Collections.Generic;
using System.IO;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class StreamBankReaderOld
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        internal void ReadStreamFile(string filePath, SfxHeaderData headerData, List<StreamSample> StreamFileDictionaryData)
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
                    storedElements[i] = BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian);
                }

                //Read Section 2
                for (int i = 0; i < storedElements.Length; i++)
                {
                    binaryReader.BaseStream.Seek(headerData.FileStart2 + storedElements[i], SeekOrigin.Begin);
                    StreamSample StreamSoundToAdd = new StreamSample
                    {
                        BlockPosition = storedElements[i],
                        MarkerSize = BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        AudioOffset = BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        AudioSize = BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        StartMarkersCount = BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        MarkersCount = BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        StartMarkerOffset = BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        MarkerOffset = BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        BaseVolume = BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian)
                    };

                    //Stream marker start data
                    StreamSoundToAdd.StartMarkers = new StartMarker[StreamSoundToAdd.StartMarkersCount];
                    for (int j = 0; j < StreamSoundToAdd.StartMarkersCount; j++)
                    {
                        StartMarker StartMarker = new StartMarker
                        {
                            Index = BinaryFunctions.FlipInt32(binaryReader.ReadInt32(), headerData.IsBigEndian),
                            Position = BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            Type = (byte)BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            Flags = (byte)BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            Extra = (byte)BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            LoopStart = BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            MarkerCount = (int)BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            LoopMarkerCount = (int)BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),

                            //StartMarker
                            MarkerPos = (int)BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            IsInstant = Convert.ToBoolean(BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian)),
                            InstantBuffer = Convert.ToBoolean(BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian)),
                            StateA = BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            StateB = BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian)
                        };

                        //Add marker
                        StreamSoundToAdd.StartMarkers[j] = StartMarker;
                    }

                    //Stream marker data 
                    StreamSoundToAdd.Markers = new Marker[StreamSoundToAdd.MarkersCount];
                    for (int k = 0; k < StreamSoundToAdd.MarkersCount; k++)
                    {
                        Marker DataMarker = new Marker
                        {
                            Index = BinaryFunctions.FlipInt32(binaryReader.ReadInt32(), headerData.IsBigEndian),
                            Position = BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            Type = (byte)BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            Flags = (byte)BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            Extra = (byte)BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            LoopStart = BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            MarkerCount = (int)BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            LoopMarkerCount = (int)BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        };

                        //Add marker
                        StreamSoundToAdd.Markers[k] = DataMarker;
                    }

                    //Read Audio Data
                    binaryReader.BaseStream.Seek(headerData.FileStart2 + StreamSoundToAdd.AudioOffset, SeekOrigin.Begin);
                    StreamSoundToAdd.EncodedData = binaryReader.ReadBytes((int)StreamSoundToAdd.AudioSize);

                    //Add Sound to Dictionary
                    StreamFileDictionaryData.Add(StreamSoundToAdd);
                }
                binaryReader.Close();
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
