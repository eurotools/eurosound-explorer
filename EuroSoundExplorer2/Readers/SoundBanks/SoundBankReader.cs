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
        public enum OldFlags
        {
            MaxReject = 0,
            NextFreeOneToUse = 1,
            IgnoreAge = 2,
            MultiSample = 3,
            RandomPick = 4,
            Shuffled = 5,
            Loop = 6,
            Polyphonic = 7,
            UnderWater = 8,
            PauseInNis = 9,
            HasSubSfx = 10,
            StealOnLouder = 11,
            TreatLikeMusic = 12
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public SoundbankHeader ReadSfxHeader(string filePath, string platform)
        {
            SfxCommonHeader commonHeader = ReadCommonHeader(filePath, platform);
            SoundbankHeader headerData = new SoundbankHeader(commonHeader);

            using (EuroSoundBinaryReader BReader = new EuroSoundBinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read), headerData.IsBigEndian))
            {
                BReader.Seek(headerData.EndOffset, SeekOrigin.Begin);

                //Section where soundbanks are stored
                headerData.SFXStart = BReader.ReadUInt32();
                //Size of the first section, in bytes
                headerData.SFXLenght = BReader.ReadUInt32();

                //Section where the sample properties are stored
                headerData.SampleInfoStart = BReader.ReadUInt32();
                //Size of the second section, in bytes. 
                headerData.SampleInfoLenght = BReader.ReadUInt32();

                //Section where the ADPCM metadata and parameters for the GameCube DSP are stored
                headerData.SpecialSampleInfoStart = BReader.ReadUInt32();
                //Size of the block, in bytes.
                headerData.SpecialSampleInfoLength = BReader.ReadUInt32();

                //Points to the beginning of the PCM data, where sound is actually stored. 
                headerData.SampleDataStart = BReader.ReadUInt32();
                //Size of the block, in bytes. 
                headerData.SampleDataLength = BReader.ReadUInt32();
            }

            return headerData;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ReadSoundBank(string filePath, SoundbankHeader headerData, SortedDictionary<uint, Sample> samplesDictionary, List<SampleData> wavesList, List<uint> duplicatedHashCodes)
        {
            if (headerData.FileVersion == 201 || headerData.FileVersion == 1)
            {
                SoundBankReaderOld oldReader = new SoundBankReaderOld();
                oldReader.ReadSoundbank(filePath, headerData, samplesDictionary, wavesList, duplicatedHashCodes);
            }
            else
            {
                SoundBankReaderNew newReader = new SoundBankReaderNew();
                newReader.ReadSoundbank(filePath, headerData, samplesDictionary, wavesList, duplicatedHashCodes);
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
