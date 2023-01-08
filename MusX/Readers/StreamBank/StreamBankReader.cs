using MusX.Objects;
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

            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                BReader.BaseStream.Seek(headerData.EndOffset, SeekOrigin.Begin);

                //Points to the stream look-up file details
                headerData.FileStart1 = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                //Size of the first section, in bytes. 
                headerData.FileLength1 = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);

                //Offset to the second section with the sample data. 
                headerData.FileStart2 = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                //Size of the second section, in bytes. 
                headerData.FileLength2 = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);

                if (headerData.FileVersion == 201 || headerData.FileVersion == 1)
                {
                    //Unused offset. Set to zero.
                    headerData.FileStart3 = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                    //Unused. Set to zero.
                    headerData.FileLength3 = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                }
            }

            return headerData;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ReadStreamBank(string filePath, StreambankHeader headerData, List<StreamSample> streamedSamples)
        {
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
