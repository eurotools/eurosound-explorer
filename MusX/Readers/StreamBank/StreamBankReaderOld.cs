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
                    storedElements[i] = BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian);
                }

                //Read Section 2
                for (int i = 0; i < storedElements.Length; i++)
                {
                    binaryReader.BaseStream.Seek(headerData.FileStart2 + storedElements[i], SeekOrigin.Begin);
                    StreamSample StreamSoundToAdd = new StreamSample
                    {
                        BlockPosition = storedElements[i],
                        MarkerSize = BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        AudioOffset = BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        AudioSize = BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        StartMarkersCount = BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        MarkersCount = BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        StartMarkerOffset = BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        MarkerOffset = BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        BaseVolume = BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian)
                    };

                    //Stream marker start data
                    StreamSoundToAdd.StartMarkers = new StartMarker[StreamSoundToAdd.StartMarkersCount];
                    for (int j = 0; j < StreamSoundToAdd.StartMarkersCount; j++)
                    {
                        StartMarker StartMarker = new StartMarker
                        {
                            Index = BinaryFunctions.FlipData(binaryReader.ReadInt32(), headerData.IsBigEndian),
                            Position = BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            Type = (byte)BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            Flags = (byte)BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            Extra = (byte)BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            LoopStart = BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            MarkerCount = (int)BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            LoopMarkerCount = (int)BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),

                            //StartMarker
                            MarkerPos = (int)BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            IsInstant = Convert.ToBoolean(BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian)),
                            InstantBuffer = Convert.ToBoolean(BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian)),
                            StateA = BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            StateB = BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian)
                        };

                        //Parse loop Offsets
                        if (headerData.Platform.IndexOf("PC", StringComparison.OrdinalIgnoreCase) >= 0 || headerData.Platform.IndexOf("Ga", StringComparison.OrdinalIgnoreCase) >= 0 || headerData.Platform.IndexOf("GC", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            StartMarker.Position /= 2;
                            StartMarker.LoopStart /= 2;
                        }
                        else if (headerData.Platform.IndexOf("PS2", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            StartMarker.Position = CalculusLoopOffsets.SonyVagToSamples(StartMarker.Position, 1);
                            StartMarker.LoopStart = CalculusLoopOffsets.SonyVagToSamples(StartMarker.LoopStart, 1);
                        }
                        else if (headerData.Platform.IndexOf("XB", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            StartMarker.Position = CalculusLoopOffsets.XboxAdpcmToSamples(StartMarker.Position, 1);
                            StartMarker.LoopStart = CalculusLoopOffsets.XboxAdpcmToSamples(StartMarker.LoopStart, 1);
                        }

                        //Add marker
                        StreamSoundToAdd.StartMarkers[j] = StartMarker;
                    }

                    //Stream marker data 
                    StreamSoundToAdd.Markers = new Marker[StreamSoundToAdd.MarkersCount];
                    for (int k = 0; k < StreamSoundToAdd.MarkersCount; k++)
                    {
                        Marker DataMarker = new Marker
                        {
                            Index = BinaryFunctions.FlipData(binaryReader.ReadInt32(), headerData.IsBigEndian),
                            Position = BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            Type = (byte)BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            Flags = (byte)BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            Extra = (byte)BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            LoopStart = BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            MarkerCount = (int)BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                            LoopMarkerCount = (int)BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                        };

                        //Parse loop Offsets
                        if (headerData.Platform.IndexOf("PC", StringComparison.OrdinalIgnoreCase) >= 0 || headerData.Platform.IndexOf("Ga", StringComparison.OrdinalIgnoreCase) >= 0 || headerData.Platform.IndexOf("GC", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            DataMarker.Position /= 2;
                            DataMarker.LoopStart /= 2;
                        }
                        else if (headerData.Platform.IndexOf("PS2", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            DataMarker.Position = CalculusLoopOffsets.SonyVagToSamples(DataMarker.Position, 1);
                            DataMarker.LoopStart = CalculusLoopOffsets.SonyVagToSamples(DataMarker.LoopStart, 1);
                        }
                        else if (headerData.Platform.IndexOf("XB", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            DataMarker.Position = CalculusLoopOffsets.XboxAdpcmToSamples(DataMarker.Position, 1);
                            DataMarker.LoopStart = CalculusLoopOffsets.XboxAdpcmToSamples(DataMarker.LoopStart, 1);
                        }

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
