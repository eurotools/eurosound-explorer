using MusX.Objects;
using System.Collections.Generic;
using System.IO;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class SoundBankReader : SfxFunctions
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public SoundbankHeader ReadSfxHeader(string filePath, string platform)
        {
            SfxCommonHeader commonHeader = ReadCommonHeader(filePath, platform);
            SoundbankHeader headerData = new SoundbankHeader(commonHeader);

            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                BReader.BaseStream.Seek(headerData.EndOffset, SeekOrigin.Begin);

                //Section where soundbanks are stored
                headerData.SFXStart = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                //Size of the first section, in bytes
                headerData.SFXLenght = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);

                //Section where the sample properties are stored
                headerData.SampleInfoStart = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                //Size of the second section, in bytes. 
                headerData.SampleInfoLenght = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);

                //Section where the ADPCM metadata and parameters for the GameCube DSP are stored
                headerData.SpecialSampleInfoStart = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                //Size of the block, in bytes.
                headerData.SpecialSampleInfoLength = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);

                //Points to the beginning of the PCM data, where sound is actually stored. 
                headerData.SampleDataStart = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                //Size of the block, in bytes. 
                headerData.SampleDataLength = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
            }

            return headerData;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ReadSoundBank(string filePath, SoundbankHeader headerData, SortedDictionary<uint, Sample> samplesDictionary, List<SampleData> wavesList)
        {
            if (headerData.FileVersion == 201 || headerData.FileVersion == 1)
            {
                SoundBankReaderOld oldReader = new SoundBankReaderOld();
                oldReader.ReadSoundbank(filePath, headerData, samplesDictionary, wavesList);
            }
            else
            {
                SoundBankReaderNew newReader = new SoundBankReaderNew();
                newReader.ReadSoundbank(filePath, headerData, samplesDictionary, wavesList);
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
