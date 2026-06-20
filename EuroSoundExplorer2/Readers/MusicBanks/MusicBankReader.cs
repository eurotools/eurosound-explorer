using MusX.Objects;
using System.IO;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class MusicBankReader : SfxFunctions
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public StreambankHeader ReadMusicHeader(string filePath, string platform)
        {
            SfxCommonHeader commonHeader = ReadCommonHeader(filePath, platform);
            StreambankHeader headerData = new StreambankHeader(commonHeader);

            using (EuroSoundBinaryReader BReader = new EuroSoundBinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read), headerData.IsBigEndian))
            {
                BReader.Seek(headerData.EndOffset, SeekOrigin.Begin);

                //Points to the stream look-up file details
                headerData.FileStart1 = BReader.ReadUInt32();
                //Size of the first section, in bytes. 
                headerData.FileLength1 = BReader.ReadUInt32();

                //Offset to the second section with the sample data. 
                headerData.FileStart2 = BReader.ReadUInt32();
                //Size of the second section, in bytes. 
                headerData.FileLength2 = BReader.ReadUInt32();

                if (headerData.FileVersion == 201 || headerData.FileVersion == 1)
                {
                    //Unused offset. Set to zero.
                    headerData.FileStart3 = BReader.ReadUInt32();
                    //Unused. Set to zero.
                    headerData.FileLength3 = BReader.ReadUInt32();
                }
            }

            return headerData;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public MusicSample ReadMusicBank(string filePath, StreambankHeader headerData)
        {
            MusicSample musicObj = null;
            if (headerData.FileVersion == 201 || headerData.FileVersion == 1)
            {
                MusicBankReaderOld oldReader = new MusicBankReaderOld();
                switch (headerData.Platform)
                {
                    case "PC":
                        musicObj = oldReader.ReadMusicFile(filePath, headerData, 1);
                        break;
                    case "PS2":
                        musicObj = oldReader.ReadMusicFile(filePath, headerData, 128);
                        break;
                    case "GameCube":
                    case "GC":
                    case "Wii":
                        musicObj = oldReader.ReadMusicFile(filePath, headerData, 1);
                        break;
                    case "Xbox":
                    case "XB":
                        musicObj = oldReader.ReadMusicFile(filePath, headerData, 4);
                        break;
                }
            }
            else
            {
                MusicBankReaderNew newReader = new MusicBankReaderNew();
                switch (headerData.Platform)
                {
                    case "PC__":
                        musicObj = newReader.ReadMusicFile(filePath, headerData, 32);
                        break;
                    case "PS2_":
                        musicObj = newReader.ReadMusicFile(filePath, headerData, 128);
                        break;
                    case "GC__":
                    case "WII_":
                    case "Wii_":
                        musicObj = newReader.ReadMusicFile(filePath, headerData, 32);
                        break;
                    case "XB__":
                        musicObj = newReader.ReadMusicFile(filePath, headerData, 32);
                        break;
                    case "XB1_":
                        musicObj = newReader.ReadMusicFile(filePath, headerData, 32);
                        break;
                }
            }

            return musicObj;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
