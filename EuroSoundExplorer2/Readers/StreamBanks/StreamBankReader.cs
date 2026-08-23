using MusX.Objects;
using System;
using System.Collections.Generic;
using System.IO;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class StreamBankReader : SfxFunctions
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public StreambankHeader ReadStreamBankHeader(string filePath, string platform)
        {
            SfxCommonHeader commonHeader = ReadCommonHeader(filePath, platform);
            StreambankHeader headerData = new StreambankHeader(commonHeader);

            if (headerData.FileVersion == 21)
            {
                // EngineXT v21 DAT5 descriptors are little-endian even on Wii.
                using (EuroSoundBinaryReader reader = new EuroSoundBinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read), false))
                {
                    reader.Seek(0x40, SeekOrigin.Begin);
                    if (System.Text.Encoding.ASCII.GetString(reader.ReadBytes(4)) != "DAT5")
                    {
                        // Some shipped v21 files have the large-file payload encrypted, so the
                        // DAT5 descriptor is not visible. Keep them browsable as one opaque stream;
                        // decoding remains unavailable until its codec metadata can be decrypted.
                        headerData.CodecType = 0;
                        headerData.StreamFlags = 0;
                        headerData.Channels = 1;
                        headerData.Frequency = 0;
                        headerData.FileStart1 = 0;
                        headerData.FileLength1 = 0;
                        headerData.FileStart2 = 0x800;
                        headerData.FileLength2 = (uint)Math.Min(uint.MaxValue, Math.Max(0, reader.BaseStream.Length - 0x800));
                        return headerData;
                    }
                    uint audioSize = reader.ReadUInt32();
                    uint channels = reader.ReadUInt32();
                    uint frequency = reader.ReadUInt32();
                    uint codec = reader.ReadUInt32();
                    uint flags = reader.ReadUInt32();
                    // DAT5 follows AudioCode's stream layout: encoded loop start,
                    // encoded end, logical sample count, exact decoded loop sample.
                    headerData.LoopStartByteOffset = reader.ReadUInt32();
                    headerData.LoopEndByteOffset = reader.ReadUInt32();
                    headerData.SampleCount = reader.ReadUInt32();
                    headerData.LoopStartSample = reader.ReadUInt32();
                    headerData.FileStart1 = codec;
                    headerData.FileLength1 = flags;
                    headerData.CodecType = codec;
                    headerData.StreamFlags = flags;
                    headerData.Channels = channels;
                    headerData.Frequency = frequency;
                    headerData.FileStart2 = 0x800;
                    headerData.FileLength2 = Math.Min(audioSize, (uint)Math.Max(0, reader.BaseStream.Length - 0x800));
                }
                return headerData;
            }

            if (headerData.FileVersion == 15 || headerData.FileVersion == 18)
            {
                // Unlike ESPD/SBNK, the v18 stream metadata remains little-endian on XE__.
                using (EuroSoundBinaryReader reader = new EuroSoundBinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read), false))
                {
                    reader.Seek(0x30, SeekOrigin.Begin);
                    uint codec = reader.ReadUInt32();
                    uint flags = reader.ReadUInt32();
                    headerData.CodecType = codec;
                    headerData.StreamFlags = flags;
                    headerData.LoopStartByteOffset = reader.ReadUInt32();
                    headerData.LoopEndByteOffset = reader.ReadUInt32();
                    headerData.SampleCount = reader.ReadUInt32();
                    headerData.LoopStartSample = reader.ReadUInt32();
                    uint audioSize = reader.ReadUInt32();
                    uint loopStartCopy = reader.ReadUInt32();
                    if (headerData.LoopStartByteOffset == uint.MaxValue) headerData.LoopStartByteOffset = loopStartCopy;
                    headerData.FileStart1 = codec;
                    headerData.FileLength1 = flags;
                    headerData.FileStart2 = 0x800;
                    headerData.FileLength2 = Math.Min(audioSize, (uint)Math.Max(0, reader.BaseStream.Length - 0x800));
                }
                return headerData;
            }

            using (EuroSoundBinaryReader BReader = new EuroSoundBinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read), headerData.IsBigEndian))
            {
                BReader.Seek(headerData.EndOffset, SeekOrigin.Begin);

                //Points to the stream look-up file details
                headerData.FileStart1 = BReader.ReadUInt32();
                //Size of the first section, in bytes. 
                headerData.FileLength1 = BReader.ReadUInt32();

                //Offset to the second section with the sample data. 
                headerData.FileStart2 = BReader.ReadUInt32();
                //Size of the second section, in bytes. 
                headerData.FileLength2 = BReader.ReadUInt32();

                if (headerData.FileVersion == 201 || headerData.FileVersion == 1)
                {
                    //Unused offset. Set to zero.
                    headerData.FileStart3 = BReader.ReadUInt32();
                    //Unused. Set to zero.
                    headerData.FileLength3 = BReader.ReadUInt32();
                }
            }

            return headerData;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ReadStreamBank(string filePath, StreambankHeader headerData, List<StreamSample> streamedSamples)
        {
            if (headerData.FileVersion == 15 || headerData.FileVersion == 18 || headerData.FileVersion == 21)
            {
                StreamBankReaderNew.ReadStreamFileV18(filePath, headerData, streamedSamples);
                return;
            }
            if (headerData.FileVersion == 201 || headerData.FileVersion == 1)
            {
                StreamBankReaderOld oldReader = new StreamBankReaderOld();
                oldReader.ReadStreamFile(filePath, headerData, streamedSamples);
            }
            else
            {
                StreamBankReaderNew newReader = new StreamBankReaderNew();
                newReader.ReadStreamFile(filePath, headerData, streamedSamples);
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
