using System.IO;
using System.Text;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class SfxFunctions
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public enum FileType
        {
            MusicFile = 1,
            StreamFile = 2,
            SoundbankFile = 3,
            SoundDetailsFile = 4,
            ProjectDetails = 5,
            MusicDetails = 6,
            SBI = 7,
            TestSFX = 8,
            MusicMarkers = 9,
            Unknown = 10
        }

        //-------------------------------------------------------------------------------------------
        //  GET TYPE OF FILE
        //-------------------------------------------------------------------------------------------
        public int GetFileHashCode(string filePath)
        {
            int hashCode = -1;
            using (BinaryReader br = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                string Magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (Magic.Equals("MUSX"))
                {
                    hashCode = br.ReadInt32();
                }
            }

            return hashCode;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public int GetFileVersion(string filePath)
        {
            int fileVersion = -1;
            using (BinaryReader br = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                string Magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (Magic.Equals("MUSX"))
                {
                    br.BaseStream.Seek(4, SeekOrigin.Current);
                    fileVersion = br.ReadInt32();
                    if (fileVersion == 10 && br.BaseStream.Length >= 0x1c)
                    {
                        br.BaseStream.Seek(0x18, SeekOrigin.Begin);
                        fileVersion = br.ReadInt32();
                    }
                }
            }

            return fileVersion;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public SfxCommonHeader ReadCommonHeader(string filePath, string platform)
        {
            SfxCommonHeader headerData = new SfxCommonHeader
            {
                Platform = platform
            };

            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                //Magic value MUSX
                string Magic = Encoding.ASCII.GetString(BReader.ReadBytes(4));
                if (Magic.Equals("MUSX"))
                {
                    //Big endian
                    if (EuroSoundCodecMatrix.IsBigEndianPlatform(headerData.Platform))
                    {
                        headerData.IsBigEndian = true;
                    }

                    //Hashcode for the current soundbank 
                    headerData.FileHashCode = BReader.ReadUInt32();
                    //Current version of the file
                    int containerVersion = BReader.ReadInt32();
                    headerData.FileVersion = containerVersion;
                    if (containerVersion == 10)
                    {
                        headerData.FileSize = BReader.ReadUInt32();
                        headerData.Platform = Encoding.ASCII.GetString(BReader.ReadBytes(4));
                        headerData.IsBigEndian = EuroSoundCodecMatrix.IsBigEndianPlatform(headerData.Platform);
                        headerData.Timespan = BReader.ReadUInt32();
                        headerData.FileVersion = BReader.ReadInt32();
                        BReader.ReadUInt32();
                        headerData.EndOffset = 0x800;
                    }
                    else if (headerData.FileVersion < 7 || headerData.FileVersion == 201)
                    {
                        //Size of the whole file, in bytes
                        headerData.FileSize = BReader.ReadUInt32();

                        //Fields in the new versions
                        if (headerData.FileVersion > 3 && headerData.FileVersion < 10)
                        {
                            //Platform PS2_ PC__ GC__ XB__
                            headerData.Platform = Encoding.ASCII.GetString(BReader.ReadBytes(4));
                            headerData.IsBigEndian = EuroSoundCodecMatrix.IsBigEndianPlatform(headerData.Platform);
                            //Seconds from 1/1/2000, 1:00:00 (946684800)
                            headerData.Timespan = BReader.ReadUInt32();
                            //Seems that when the data is encoded in adpcm is set to 1.
                            headerData.UsesAdpcm = BReader.ReadUInt32();
                            //Padding??
                            BReader.ReadUInt32();
                        }

                        // EngineXT keeps the engine-visible descriptor at the next 0x800-byte sector.
                        headerData.EndOffset = BReader.BaseStream.Position;
                    }
                    else
                    {
                        throw new InvalidDataException(string.Format("This file version ({0}) is unsupported by this version of the EuroSound Explorer", headerData.FileVersion));
                    }
                }

                //Close
                BReader.Close();
            }

            return headerData;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public int GetNumberOfSFXs(string filePath, SoundbankHeader sbData)
        {
            // In EngineXT the SBNK descriptor already supplies the count and
            // ReadSfxHeader exposes its 16-byte pointer-table span here. The
            // first value at SFXStart is an SFX hash, not a legacy count.
            if (sbData.FileVersion == 18)
            {
                return checked((int)(sbData.SFXLenght / 16));
            }

            int totalSfx = -1;
            using (BinaryReader br = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                if (sbData.SFXStart < br.BaseStream.Length)
                {
                    br.BaseStream.Seek(sbData.SFXStart, SeekOrigin.Begin);
                    totalSfx = (int)BytesFunctions.FlipData(br.ReadUInt32(), sbData.IsBigEndian);
                }
            }
            return totalSfx;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
