using MusX.Objects;
using System.IO;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class SbiBankReader : SfxFunctions
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public SoundbankInfoHeader ReadSoundbankInfoHeader(string filePath, string platform)
        {
            SfxCommonHeader commonHeader = ReadCommonHeader(filePath, platform);
            SoundbankInfoHeader headerData = new SoundbankInfoHeader(commonHeader);

            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                BReader.BaseStream.Seek(headerData.EndOffset, SeekOrigin.Begin);

                //Points to the stream look-up file details
                headerData.FileStart1 = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                //Size of the first section, in bytes. 
                headerData.FileLength1 = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);

                //Offset to the second section with the sample data. 
                headerData.FileStart2 = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                //Size of the second section, in bytes. 
                headerData.FileLength2 = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
            }

            return headerData;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public SbiFile ReadSoundbankInfoFile(string filePath, SoundbankInfoHeader headerData)
        {
            SbiFile sbiFileObj = new SbiFile();

            using (BinaryReader binaryReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                //Read Project SoundBanks
                binaryReader.BaseStream.Seek(headerData.FileStart1, SeekOrigin.Begin);
                for (int i = 0; i < sbiFileObj.projectSoundBanks.Length; i++)
                {
                    sbiFileObj.projectSoundBanks[i] = BinaryFunctions.FlipData(binaryReader.ReadInt32(), headerData.IsBigEndian);
                }

                //Read Project MusicBanks
                binaryReader.BaseStream.Seek(headerData.FileStart2, SeekOrigin.Begin);
                for (int i = 0; i < sbiFileObj.projectMusicBanks.Length; i++)
                {
                    sbiFileObj.projectMusicBanks[i] = BinaryFunctions.FlipData(binaryReader.ReadInt32(), headerData.IsBigEndian);
                }
            }

            return sbiFileObj;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
