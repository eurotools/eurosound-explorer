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
                projectData.MinHashCode = BReader.ReadUInt32();
                projectData.MaxHashCode = BReader.ReadUInt32();

                int hashCodePrefix = (int)(0xFFFF0000 & projectData.MinHashCode);

                //Read each stored SFX
                projectData.sfxItems = new SoundDetailsData[(int)(0x0000FFFF & projectData.MaxHashCode)];
                for (int i = 0; i < projectData.sfxItems.Length; i++)
                {
                    SoundDetailsData sfxItem = new SoundDetailsData
                    {
                        HashCode = hashCodePrefix | i,
                        InnerRadius = BinaryFunctions.FlipData(BReader.ReadUInt16(), sfxHeaderData.IsBigEndian),
                        OuterRadius = BinaryFunctions.FlipData(BReader.ReadUInt16(), sfxHeaderData.IsBigEndian),
                        Duration = BinaryFunctions.FlipData(BReader.ReadSingle(), sfxHeaderData.IsBigEndian),
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
