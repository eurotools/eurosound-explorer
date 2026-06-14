using MusX.Objects;
using System;
using System.IO;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class SoundDetailsReader : SfxFunctions
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public SoundDetails ReadSoundDetailsFile(string filePath, SfxCommonHeader sfxHeaderData)
        {
            //Read file
            SoundDetails projectData = new SoundDetails();
            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                BReader.BaseStream.Seek(0x20, SeekOrigin.Begin);
                projectData.MinHashCode = BytesFunctions.FlipData(BReader.ReadUInt32(), sfxHeaderData.IsBigEndian);
                projectData.MaxHashCode = BytesFunctions.FlipData(BReader.ReadUInt32(), sfxHeaderData.IsBigEndian);

                //Read each stored SFX
                if (sfxHeaderData.FileVersion == 6)
                {
                    long payloadEnd = sfxHeaderData.FileSize > 0 ? sfxHeaderData.FileSize : BReader.BaseStream.Length;
                    long entriesSize = payloadEnd - BReader.BaseStream.Position;
                    if (entriesSize < 0)
                    {
                        entriesSize = 0;
                    }

                    projectData.sfxItems = new SoundDetailsData[(int)(entriesSize / 16)];
                }
                else
                {
                    projectData.sfxItems = new SoundDetailsData[(int)(0x0000FFFF & projectData.MaxHashCode)];
                }

                int hashCodePrefix = (int)(0xFFFF0000 & projectData.MinHashCode);
                for (int i = 0; i < projectData.sfxItems.Length; i++)
                {
                    int hashCode = sfxHeaderData.FileVersion == 6
                        ? unchecked((int)BytesFunctions.FlipData(BReader.ReadUInt32(), sfxHeaderData.IsBigEndian))
                        : hashCodePrefix | i;

                    SoundDetailsData sfxItem = new SoundDetailsData
                    {
                        HashCode = hashCode,
                        InnerRadius = BytesFunctions.FlipData(BReader.ReadUInt16(), sfxHeaderData.IsBigEndian),
                        OuterRadius = BytesFunctions.FlipData(BReader.ReadUInt16(), sfxHeaderData.IsBigEndian),
                        Duration = BytesFunctions.FlipData(BReader.ReadSingle(), sfxHeaderData.IsBigEndian),
                        Looping = Convert.ToBoolean(BReader.ReadSByte()),
                        Tracking3D = BReader.ReadSByte(),
                        SampleStreamed = Convert.ToBoolean(BReader.ReadSByte()),
                        Is3D = Convert.ToBoolean(BReader.ReadSByte())
                    };
                    projectData.sfxItems[i] = sfxItem;
                }
            }

            return projectData;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
