using MusX.Objects;
using System;
using System.IO;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class MusicDetailsReader : SfxFunctions
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public MusicDetails ReadMusicDetailsFile(string filePath, SfxCommonHeader sfxHeaderData)
        {
            //Read file
            MusicDetails projectData = new MusicDetails();
            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                BReader.BaseStream.Seek(0x20, SeekOrigin.Begin);
                projectData.MinHashCode = BReader.ReadUInt32();
                projectData.MaxHashCode = BReader.ReadUInt32();

                uint hashCodePrefix = (0xFFFF0000 & projectData.MinHashCode);

                //Read each stored SFX
                projectData.musicItems = new MusicDetailsData[(int)(0x0000FFFF & projectData.MaxHashCode)];
                for (int i = 0; i < projectData.musicItems.Length; i++)
                {
                    MusicDetailsData sfxItem = new MusicDetailsData
                    {
                        HashCode = (int)(hashCodePrefix | BReader.ReadUInt32()),
                        Duration = BinaryFunctions.FlipData(BReader.ReadSingle(), sfxHeaderData.IsBigEndian),
                        MusicLooping = Convert.ToBoolean(BReader.ReadUInt32()),
                        UserValue = BReader.ReadUInt32(),
                    };
                    projectData.musicItems[i] = sfxItem;
                }
            }

            return projectData;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
