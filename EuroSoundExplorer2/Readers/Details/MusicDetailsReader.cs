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
                projectData.MinHashCode = BytesFunctions.FlipData(BReader.ReadUInt32(), sfxHeaderData.IsBigEndian);
                projectData.MaxHashCode = BytesFunctions.FlipData(BReader.ReadUInt32(), sfxHeaderData.IsBigEndian);

                if (sfxHeaderData.FileVersion == 6)
                {
                    projectData.MinHashCode = GetSoundHMusicHashCode(projectData.MinHashCode);
                    projectData.MaxHashCode = GetSoundHMusicHashCode(projectData.MaxHashCode);
                }

                uint hashCodePrefix = 0xFFFF0000 & projectData.MinHashCode;

                //Read each stored SFX
                projectData.musicItems = new MusicDetailsData[(int)(0x0000FFFF & projectData.MaxHashCode)];
                for (int i = 0; i < projectData.musicItems.Length; i++)
                {
                    uint fileHashCode = BytesFunctions.FlipData(BReader.ReadUInt32(), sfxHeaderData.IsBigEndian);
                    uint hashCode = sfxHeaderData.FileVersion == 6
                        ? GetSoundHMusicHashCode(fileHashCode)
                        : hashCodePrefix | fileHashCode;

                    MusicDetailsData sfxItem = new MusicDetailsData
                    {
                        HashCode = unchecked((int)hashCode),
                        Duration = BytesFunctions.FlipData(BReader.ReadSingle(), sfxHeaderData.IsBigEndian),
                        MusicLooping = Convert.ToBoolean(BytesFunctions.FlipData(BReader.ReadUInt32(), sfxHeaderData.IsBigEndian)),
                        UserValue = BytesFunctions.FlipData(BReader.ReadUInt32(), sfxHeaderData.IsBigEndian),
                    };
                    projectData.musicItems[i] = sfxItem;
                }
            }

            return projectData;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static uint GetSoundHMusicHashCode(uint musicDetailsHashCode)
        {
            return 0x2D600000 | (musicDetailsHashCode & 0x0000FFFF);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
