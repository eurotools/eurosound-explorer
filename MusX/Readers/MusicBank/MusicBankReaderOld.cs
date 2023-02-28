using MusX.Objects;
using System;
using System.IO;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class MusicBankReaderOld
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public MusicSample ReadMusicFile(string filePath, StreambankHeader headerData, int interleave_block_size)
        {
            MusicSample musicDat;
            using (BinaryReader binaryReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                //Seek Position Section 1
                binaryReader.BaseStream.Seek(headerData.FileStart1, SeekOrigin.Begin);

                //Stream marker header data 
                musicDat = new MusicSample
                {
                    //Properties
                    StartMarkersCount = BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                    MarkersCount = BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                    StartMarkerOffset = BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                    MarkerOffset = BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                    BaseVolume = BinaryFunctions.FlipData(binaryReader.ReadUInt32(), headerData.IsBigEndian)
                };

                //Read Start Markers
                musicDat.StartMarkers = new StartMarker[musicDat.StartMarkersCount];
                for (int j = 0; j < musicDat.StartMarkersCount; j++)
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
                        StartMarker.Position /= 4;
                        StartMarker.LoopStart /= 4;
                    }
                    else if (headerData.Platform.IndexOf("PS2", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        StartMarker.Position = CalculusLoopOffsets.SonyVagToSamples(StartMarker.Position, 2);
                        StartMarker.LoopStart = CalculusLoopOffsets.SonyVagToSamples(StartMarker.LoopStart, 2);
                    }
                    else if (headerData.Platform.IndexOf("XB", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        StartMarker.Position = CalculusLoopOffsets.XboxAdpcmToSamples(StartMarker.Position, 2);
                        StartMarker.LoopStart = CalculusLoopOffsets.XboxAdpcmToSamples(StartMarker.LoopStart, 2);
                    }

                    //Add marker
                    musicDat.StartMarkers[j] = StartMarker;
                }

                //Read Markers
                musicDat.Markers = new Marker[musicDat.MarkersCount];
                for (int k = 0; k < musicDat.MarkersCount; k++)
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
                    if (headerData.Platform.IndexOf("PC", StringComparison.OrdinalIgnoreCase) >= 0 || headerData.Platform.IndexOf("Ga", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        DataMarker.Position /= 4;
                        DataMarker.LoopStart /= 4;
                    }
                    else if (headerData.Platform.IndexOf("PS2", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        DataMarker.Position = CalculusLoopOffsets.SonyVagToSamples(DataMarker.Position, 2);
                        DataMarker.LoopStart = CalculusLoopOffsets.SonyVagToSamples(DataMarker.LoopStart, 2);
                    }
                    else if (headerData.Platform.IndexOf("XB", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        DataMarker.Position = CalculusLoopOffsets.XboxAdpcmToSamples(DataMarker.Position, 2);
                        DataMarker.LoopStart = CalculusLoopOffsets.XboxAdpcmToSamples(DataMarker.LoopStart, 2);
                    }

                    //Add marker
                    musicDat.Markers[k] = DataMarker;
                }

                //Read Section 2
                uint TracksLength = headerData.FileLength2 / 2;
                bool InterleavedStereo = true;
                int IndexLC = 0, IndexRC = 0;

                //Seek Position
                binaryReader.BaseStream.Seek(headerData.FileStart2, SeekOrigin.Begin);

                //Save offset
                musicDat.AudioOffset = (uint)binaryReader.BaseStream.Position;
                musicDat.AudioSize = headerData.FileLength2;

                //Init arrays
                musicDat.EncodedData[0] = new byte[TracksLength];
                musicDat.EncodedData[1] = new byte[TracksLength];

                //Read Stereo interleaving
                while (binaryReader.BaseStream.Position < (headerData.FileStart2 + headerData.FileLength2))
                {
                    if (InterleavedStereo)
                    {
                        Buffer.BlockCopy(binaryReader.ReadBytes(interleave_block_size), 0, musicDat.EncodedData[0], IndexLC, interleave_block_size);
                        IndexLC += interleave_block_size;
                    }
                    else
                    {
                        Buffer.BlockCopy(binaryReader.ReadBytes(interleave_block_size), 0, musicDat.EncodedData[1], IndexRC, interleave_block_size);
                        IndexRC += interleave_block_size;
                    }
                    InterleavedStereo = !InterleavedStereo;
                }
            }

            return musicDat;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
