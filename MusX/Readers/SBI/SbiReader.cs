using MusX.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class SbiReader : SfxFunctions
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public SbiFile ReadStreamFile(string filePath, SfxHeaderData headerData)
        {
            SbiFile sbiFileObj = new SbiFile();

            using (BinaryReader binaryReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                //Read Project SoundBanks
                binaryReader.BaseStream.Seek(headerData.FileStart1, SeekOrigin.Begin);
                for(int i = 0; i < sbiFileObj.projectSoundBanks.Length; i++)
                {
                        sbiFileObj.projectSoundBanks[i] = BinaryFunctions.FlipInt32(binaryReader.ReadInt32(), headerData.IsBigEndian);
                }

                //Read Project MusicBanks
                binaryReader.BaseStream.Seek(headerData.FileStart2, SeekOrigin.Begin);
                for (int i = 0; i < sbiFileObj.projectMusicBanks.Length; i++)
                {
                        sbiFileObj.projectMusicBanks[i] = BinaryFunctions.FlipInt32(binaryReader.ReadInt32(), headerData.IsBigEndian);
                }
            }

            return sbiFileObj;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
