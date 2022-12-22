using MusX.Objects;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class MusicBankReader : SfxFunctions
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public MusicSample ReadMusicBank(string filePath, SfxHeaderData headerData)
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
                    case "GC":
                        musicObj = oldReader.ReadMusicFile(filePath, headerData, 1);
                        break;
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
