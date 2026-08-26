using MusX.Objects;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

            if (headerData.FileVersion == 10)
            {
                SoundBankReaderNew.ReadSoundbankHeaderV10(filePath, headerData);
                return headerData;
            }

            if (headerData.FileVersion == 18 || headerData.FileVersion == 21)
            {
                using (EuroSoundBinaryReader reader = new EuroSoundBinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read), headerData.IsBigEndian))
                {
                    reader.Seek(0x800, SeekOrigin.Begin);
                    if (Encoding.ASCII.GetString(reader.ReadBytes(4)) != "SBNK") throw new InvalidDataException("EngineXT v18 soundbank has no SBNK descriptor.");
                    reader.ReadUInt32(); reader.ReadUInt32(); reader.ReadUInt32();
                    uint sfxCount = reader.ReadUInt32(); long field = reader.BaseStream.Position; int rel = reader.ReadInt32();
                    headerData.SFXStart = (uint)(field + rel);
                    headerData.SFXLenght = sfxCount * 16;
                    reader.ReadUInt32(); reader.ReadInt32();
                    reader.ReadUInt32(); reader.ReadInt32();
                    reader.ReadUInt32(); reader.ReadInt32();
                    uint memoryCount = reader.ReadUInt32(); long memoryField = reader.BaseStream.Position; int memoryRel = reader.ReadInt32();
                    headerData.SampleInfoStart = (uint)(memoryField + memoryRel);
                    headerData.SampleInfoLenght = memoryCount * 28;
                    reader.ReadUInt32(); reader.ReadInt32();
                    reader.ReadUInt32(); reader.ReadInt32();
                    headerData.SampleDataLength = reader.ReadUInt32();
                    headerData.SampleDataStart = unchecked((uint)reader.ReadInt32());
                }
                return headerData;
            }

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
            if (headerData.FileVersion == 10)
            {
                SoundBankReaderNew.ReadSoundbankV10(filePath, headerData, samplesDictionary, wavesList, duplicatedHashCodes);
                return;
            }

            if (headerData.FileVersion == 18 || headerData.FileVersion == 21)
            {
                SoundBankReaderNew.ReadSoundbankV18(filePath, headerData, samplesDictionary, wavesList, duplicatedHashCodes);
                return;
            }
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
