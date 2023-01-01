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
        public MusicSample ReadMusicFile(string filePath, SfxHeaderData headerData, int interleave_block_size)
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
                    StartMarkerCount = BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                    MarkerCount = BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                    StartMarkerOffset = BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                    MarkerOffset = BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian),
                    BaseVolume = BinaryFunctions.FlipUInt32(binaryReader.ReadUInt32(), headerData.IsBigEndian)
                };

                //Read Start Markers
                musicDat.StartMarkers = new StartMarker[musicDat.StartMarkerCount];
                for (int j = 0; j < musicDat.StartMarkerCount; j++)
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

                    //Parse loop Offsets
                    if (headerData.Platform.IndexOf("PC", StringComparison.OrdinalIgnoreCase) >= 0 || headerData.Platform.IndexOf("Ga", StringComparison.OrdinalIgnoreCase) >= 0 || headerData.Platform.IndexOf("GC", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        StartMarker.Position = CalculusLoopOffsets.ReverseGetStreamLoopOffsetPCandGC(StartMarker.Position);
                        StartMarker.LoopStart = CalculusLoopOffsets.ReverseGetStreamLoopOffsetPCandGC(StartMarker.LoopStart);
                    }
                    else if (headerData.Platform.IndexOf("PS2", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        StartMarker.Position = CalculusLoopOffsets.ReverseGetStreamLoopOffsetPlayStation2(StartMarker.Position);
                        StartMarker.LoopStart = CalculusLoopOffsets.ReverseGetStreamLoopOffsetPlayStation2(StartMarker.LoopStart);
                    }
                    else if (headerData.Platform.IndexOf("XB", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        StartMarker.Position = CalculusLoopOffsets.ReverseGetStreamLoopOffsetXbox(StartMarker.Position);
                        StartMarker.LoopStart = CalculusLoopOffsets.ReverseGetStreamLoopOffsetXbox(StartMarker.LoopStart);
                    }

                    //Add marker
                    musicDat.StartMarkers[j] = StartMarker;
                }

                //Read Markers
                musicDat.Markers = new Marker[musicDat.MarkerCount];
                for (int k = 0; k < musicDat.MarkerCount; k++)
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

                    //Parse loop Offsets
                    if (headerData.Platform.IndexOf("PC", StringComparison.OrdinalIgnoreCase) >= 0 || headerData.Platform.IndexOf("Ga", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        DataMarker.Position = CalculusLoopOffsets.ReverseGetStreamLoopOffsetPCandGC(DataMarker.Position);
                        DataMarker.LoopStart = CalculusLoopOffsets.ReverseGetStreamLoopOffsetPCandGC(DataMarker.LoopStart);
                    }
                    else if (headerData.Platform.IndexOf("PS2", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        DataMarker.Position = CalculusLoopOffsets.ReverseGetStreamLoopOffsetPlayStation2(DataMarker.Position);
                        DataMarker.LoopStart = CalculusLoopOffsets.ReverseGetStreamLoopOffsetPlayStation2(DataMarker.LoopStart);
                    }
                    else if (headerData.Platform.IndexOf("XB", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        DataMarker.Position = CalculusLoopOffsets.ReverseGetStreamLoopOffsetXbox(DataMarker.Position);
                        DataMarker.LoopStart = CalculusLoopOffsets.ReverseGetStreamLoopOffsetXbox(DataMarker.LoopStart);
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
