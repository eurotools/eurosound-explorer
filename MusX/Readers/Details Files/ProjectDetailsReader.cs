using MusX.Objects;
using System.IO;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class ProjectDetailsReader : SfxFunctions
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public ProjectDetailsHeader ReadProjectFileHeader(string filePath, string platform)
        {
            SfxCommonHeader commonHeader = ReadCommonHeader(filePath, platform);
            ProjectDetailsHeader headerData = new ProjectDetailsHeader(commonHeader);

            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                BReader.BaseStream.Seek(headerData.EndOffset, SeekOrigin.Begin);
                //Get the start offset where memmory slots start.
                headerData.MemoryStart = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                //Size of the first section, in bytes
                headerData.MemoryLength = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
            }

            return headerData;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public ProjectDetails ReadProjectFile(string filePath, ProjectDetailsHeader headerData)
        {
            ProjectDetails projectData = new ProjectDetails();
            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                //Read Offsets and count
                BReader.BaseStream.Seek(headerData.MemoryStart, SeekOrigin.Begin);
                projectData.MemmorySlotsCount = BytesFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian);
                projectData.MemorySlotsOffset = BytesFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian);
                projectData.SoundBanksCount = BytesFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian);
                projectData.SoundBanksOffset = BytesFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian);
                long flagsPos = BReader.BaseStream.Position;

                //Read Project Slots 
                BReader.BaseStream.Seek(headerData.MemoryStart + projectData.MemorySlotsOffset, SeekOrigin.Begin);
                for (int i = 0; i < projectData.MemmorySlotsCount; i++)
                {
                    ProjectSlots projSlots = new ProjectSlots
                    {
                        SlotNumber = BytesFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian),
                        MemorySize = BytesFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian),
                        Quantity = BytesFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian)
                    };
                    projectData.memorySlotsData.Add(projSlots);
                }

                //Read Soundbanks Section
                BReader.BaseStream.Seek(headerData.MemoryStart + projectData.SoundBanksOffset, SeekOrigin.Begin);
                for (int i = 0; i < projectData.SoundBanksCount; i++)
                {
                    ProjectSoundBank soundbankData = new ProjectSoundBank
                    {
                        HashCode = BytesFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian),
                        SlotNumber = BytesFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian)
                    };
                    projectData.soundBanksData.Add(soundbankData);
                }

                //Read Flags Data
                BReader.BaseStream.Seek(flagsPos + 16, SeekOrigin.Begin);
                projectData.StereoStreamCount = BytesFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian);
                projectData.MonoStreamCount = BytesFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian);
                projectData.ProjectCode = BytesFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian);
                for (int i = 0; i < 10; i++)
                {
                    projectData.flagsValues[i] = BytesFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian);
                }
            }
            return projectData;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
