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
        //-------------------------------------------------------------------------------------------------------------------------------
        public override SfxHeaderData ReadSfxHeader(string filePath, string platform)
        {
            SfxHeaderData headerData = new SfxHeaderData
            {
                Platform = platform
            };

            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                //Magic value MUSX
                string Magic = Encoding.ASCII.GetString(BReader.ReadBytes(4));
                if (Magic.Equals("MUSX"))
                {
                    //Hashcode for the current soundbank 
                    headerData.FileHashCode = BReader.ReadUInt32();
                    //Current version of the file
                    headerData.FileVersion = BReader.ReadUInt32();
                    //Size of the whole file, in bytes
                    headerData.FileSize = BReader.ReadUInt32();

                    //Fields in the new versions
                    if (headerData.FileVersion > 3 && headerData.FileVersion < 10)
                    {
                        //Platform PS2_ PC__ GC__ XB__
                        headerData.Platform = Encoding.ASCII.GetString(BReader.ReadBytes(4));
                        //Seconds from 1/1/2000, 1:00:00 (946684800)
                        headerData.Timespan = BReader.ReadUInt32();
                        //Seems padding but when the platform is PC__ or GC__ is set to 1
                        headerData.EurocomIma = BReader.ReadUInt32();
                        //Padding??
                        BReader.ReadUInt32();
                    }

                    //Big endian
                    if (headerData.Platform.Contains("GC"))
                    {
                        headerData.IsBigEndian = true;
                    }

                    //Section where soundbanks are stored
                    headerData.SFXStart = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian);
                    //Size of the first section, in bytes
                    headerData.SFXLenght = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian);

                    //Section where the sample properties are stored
                    headerData.SampleInfoStart = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian);
                    //Size of the second section, in bytes. 
                    headerData.SampleInfoLenght = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian);

                    //Section where the ADPCM metadata and parameters for the GameCube DSP are stored
                    headerData.SpecialSampleInfoStart = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian);
                    //Size of the block, in bytes.
                    headerData.SpecialSampleInfoLength = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian);

                    //Points to the beginning of the PCM data, where sound is actually stored. 
                    headerData.SampleDataStart = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian);
                    //Size of the block, in bytes. 
                    headerData.SampleDataLength = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian);
                }

                //Close
                BReader.Close();
            }

            return headerData;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ReadSoundBank(string filePath, SfxHeaderData headerData, SortedDictionary<uint, Sample> samplesDictionary, List<SampleData> wavesList)
        {
            if (headerData.FileVersion == 201 || headerData.FileVersion == 1)
            {
                SoundBankReaderOld oldReader = new SoundBankReaderOld();
                oldReader.ReadSoundbank(filePath, headerData, samplesDictionary, wavesList);
            }
            else
            {
                SoundBankReaderNew oldReader = new SoundBankReaderNew();
                oldReader.ReadSoundbank(filePath, headerData, samplesDictionary, wavesList);
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
