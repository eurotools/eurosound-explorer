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

            if (headerData.FileVersion == 18)
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
            if (headerData.FileVersion == 18)
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
