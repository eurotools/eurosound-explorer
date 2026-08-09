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

                if (sfxHeaderData.FileVersion == 18)
                {
                    uint count = BytesFunctions.FlipData(BReader.ReadUInt32(), sfxHeaderData.IsBigEndian);
                    long available = Math.Max(0, BReader.BaseStream.Length - BReader.BaseStream.Position);
                    count = Math.Min(count, (uint)(available / 16));
                    projectData.sfxItems = new SoundDetailsData[count];
                    for (int i = 0; i < projectData.sfxItems.Length; i++)
                    {
                        uint packed;
                        SoundDetailsData item = new SoundDetailsData
                        {
                            HashCode = unchecked((int)BytesFunctions.FlipData(BReader.ReadUInt32(), sfxHeaderData.IsBigEndian)),
                            InnerRadius = BytesFunctions.FlipData(BReader.ReadSingle(), sfxHeaderData.IsBigEndian),
                            OuterRadius = BytesFunctions.FlipData(BReader.ReadSingle(), sfxHeaderData.IsBigEndian)
                        };
                        packed = BytesFunctions.FlipData(BReader.ReadUInt32(), sfxHeaderData.IsBigEndian);
                        item.Duration = packed & 0x00ffffff;
                        item.Looping = (packed & (1u << 24)) != 0;
                        item.SampleStreamed = (packed & (1u << 25)) != 0;
                        item.Is3D = (packed & (1u << 26)) != 0;
                        item.Tracking3D = (sbyte)(((packed & (1u << 27)) == 0) ? 1 : 0);
                        item.KillOnNodeDelete = (packed & (1u << 28)) != 0;
                        item.IsMusic = (packed & (1u << 29)) != 0;
                        projectData.sfxItems[i] = item;
                    }
                    return projectData;
                }

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
