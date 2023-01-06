using MusX.Objects;
using System.IO;
using System.Text;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class ProjectDetailsReader : SfxFunctions
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

                        //Get the start offset where memmory slots start.
                        headerData.MemoryStart = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian);
                        //Size of the first section, in bytes
                        headerData.MemoryLength = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian);
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
        public ProjectDetails ReadProjectFile(string filePath, SfxHeaderData headerData)
        {
            ProjectDetails projectData = new ProjectDetails();
            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                //Read Offsets and count
                BReader.BaseStream.Seek(headerData.MemoryStart, SeekOrigin.Begin);
                projectData.MemmorySlotsCount = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian);
                projectData.MemorySlotsOffset = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian);
                projectData.SoundBanksCount = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian);
                projectData.SoundBanksOffset = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian);
                long flagsPos = BReader.BaseStream.Position;

                //Read Project Slots 
                BReader.BaseStream.Seek(headerData.MemoryStart + projectData.MemorySlotsOffset, SeekOrigin.Begin);
                for (int i = 0; i < projectData.MemmorySlotsCount; i++)
                {
                    ProjectSlots projSlots = new ProjectSlots
                    {
                        SlotNumber = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian),
                        MemorySize = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian),
                        Quantity = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian)
                    };
                    projectData.memorySlotsData.Add(projSlots);
                }

                //Read Soundbanks Section
                BReader.BaseStream.Seek(headerData.MemoryStart + projectData.SoundBanksOffset, SeekOrigin.Begin);
                for (int i = 0; i < projectData.SoundBanksCount; i++)
                {
                    ProjectSoundBank soundbankData = new ProjectSoundBank
                    {
                        HashCode = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian),
                        SlotNumber = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian)
                    };
                    projectData.soundBanksData.Add(soundbankData);
                }

                //Read Flags Data
                BReader.BaseStream.Seek(flagsPos + 16, SeekOrigin.Begin);
                projectData.StereoStreamCount = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian);
                projectData.MonoStreamCount = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian);
                projectData.ProjectCode = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian);
                for (int i = 0; i < 10; i++)
                {
                    projectData.flagsValues[i] = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian);
                }
            }
            return projectData;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
