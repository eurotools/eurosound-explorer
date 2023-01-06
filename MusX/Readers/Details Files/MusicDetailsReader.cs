using MusX.Objects;
using System;
using System.IO;
using System.Text;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class MusicDetailsReader : SfxFunctions
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public override SfxHeaderData ReadSfxHeader(string filePath, string platform)
        {
            SfxHeaderData headerData = new SfxHeaderData
            {
                Platform = platform
            };

            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                //Magic value MUSX
                string Magic = Encoding.ASCII.GetString(BReader.ReadBytes(4));
                if (Magic.Equals("MUSX"))
                {
                    //Hashcode for the current soundbank 
                    headerData.FileHashCode = BReader.ReadUInt32();
                    //Current version of the file
                    headerData.FileVersion = BReader.ReadUInt32();
                    if (headerData.FileVersion < 7 || headerData.FileVersion == 201)
                    {
                        //Size of the whole file, in bytes
                        headerData.FileSize = BReader.ReadUInt32();

                        //Fields in the new versions
                        if (headerData.FileVersion > 3 && headerData.FileVersion < 10)
                        {
                            //Platform PS2_ PC__ GC__ XB__
                            headerData.Platform = Encoding.ASCII.GetString(BReader.ReadBytes(4));
                            //Seconds from 1/1/2000, 1:00:00 (946684800)
                            headerData.Timespan = BReader.ReadUInt32();
                            //Seems padding but when the platform is PC__ or GC__ is set to 1
                            headerData.UsesAdpcm = BReader.ReadUInt32();
                            //Padding??
                            BReader.ReadUInt32();
                        }

                        //Big endian
                        if (headerData.Platform.Contains("GC") || headerData.Platform.Contains("GameCube"))
                        {
                            headerData.IsBigEndian = true;
                        }
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
        public MusicDetails ReadSoundDetailsFile(string filePath, SfxHeaderData sfxHeaderData)
        {
            MusicDetails projectData = new MusicDetails();
            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                BReader.BaseStream.Seek(0x20, SeekOrigin.Begin);
                projectData.MinHashCode = BReader.ReadUInt32();
                projectData.MaxHashCode = BReader.ReadUInt32();

                uint hashCodePrefix = (0xFFFF0000 & projectData.MinHashCode);

                //Read each stored SFX
                projectData.sfxItems = new MusicDetailsData[(int)(0x0000FFFF & projectData.MaxHashCode)];
                for (int i = 0; i < projectData.sfxItems.Length; i++)
                {
                    MusicDetailsData sfxItem = new MusicDetailsData
                    {
                        HashCode = (int)(hashCodePrefix | BReader.ReadUInt32()),
                        Duration = BinaryFunctions.FlipData(BReader.ReadSingle(), sfxHeaderData.IsBigEndian),
                        MusicLooping = Convert.ToBoolean(BReader.ReadUInt32()),
                        UserValue = BReader.ReadUInt32(),
                    };
                    projectData.sfxItems[i] = sfxItem;
                }
            }

            return projectData;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
