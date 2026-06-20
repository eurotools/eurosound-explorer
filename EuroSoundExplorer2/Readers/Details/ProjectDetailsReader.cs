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
        public ProjectDetailsHeader ReadProjectFileHeader(string filePath, string platform)
        {
            SfxCommonHeader commonHeader = ReadCommonHeader(filePath, platform);
            ProjectDetailsHeader headerData = new ProjectDetailsHeader(commonHeader);

            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                BReader.BaseStream.Seek(headerData.EndOffset, SeekOrigin.Begin);
                bool descriptorIsBigEndian = headerData.IsBigEndian && headerData.FileVersion != 6;
                //Get the start offset where memmory slots start.
                headerData.MemoryStart = BytesFunctions.FlipData(BReader.ReadUInt32(), descriptorIsBigEndian);
                //Size of the first section, in bytes
                headerData.MemoryLength = BytesFunctions.FlipData(BReader.ReadUInt32(), descriptorIsBigEndian);
            }

            return headerData;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public ProjectDetails ReadProjectFile(string filePath, ProjectDetailsHeader headerData)
        {
            ProjectDetails projectData = new ProjectDetails
            {
                FormatVersion = headerData.FileVersion
            };

            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                if (headerData.FileVersion == 6)
                {
                    ReadProjectFileVersion6(BReader, headerData, projectData);
                    return projectData;
                }

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

        //-------------------------------------------------------------------------------------------------------------------------------
        private static void ReadProjectFileVersion6(BinaryReader BReader, ProjectDetailsHeader headerData, ProjectDetails projectData)
        {
            BReader.BaseStream.Seek(headerData.MemoryStart, SeekOrigin.Begin);

            long payloadEnd = headerData.MemoryStart + headerData.MemoryLength;
            if (payloadEnd > BReader.BaseStream.Length || payloadEnd <= headerData.MemoryStart)
            {
                payloadEnd = BReader.BaseStream.Length;
            }

            if (ReadFourCC(BReader) != "FORM")
            {
                return;
            }

            uint projectFormSize = BReader.ReadUInt32();
            string projectFormType = ReadFourCC(BReader);
            long projectFormEnd = GetFormEnd(BReader.BaseStream.Position - 12, projectFormSize, payloadEnd);
            if (projectFormType != "ES2P")
            {
                BReader.BaseStream.Seek(projectFormEnd, SeekOrigin.Begin);
                return;
            }

            while (BReader.BaseStream.Position + 8 <= projectFormEnd)
            {
                long chunkStart = BReader.BaseStream.Position;
                string chunkId = ReadFourCC(BReader);
                uint chunkSize = BReader.ReadUInt32();

                if (chunkId == "FORM" && BReader.BaseStream.Position + 4 <= projectFormEnd)
                {
                    string formType = ReadFourCC(BReader);
                    long formEnd = GetFormEnd(chunkStart, chunkSize, projectFormEnd);
                    if (formType == "STYP")
                    {
                        ReadMemoryMapsForm(BReader, formEnd, projectData);
                    }
                    BReader.BaseStream.Seek(formEnd, SeekOrigin.Begin);
                }
                else if (chunkId == "USRV")
                {
                    ReadUserValuesChunk(BReader, chunkSize, projectData);
                    BReader.BaseStream.Seek(GetChunkEnd(chunkStart, chunkSize, projectFormEnd), SeekOrigin.Begin);
                }
                else
                {
                    BReader.BaseStream.Seek(GetChunkEnd(chunkStart, chunkSize, projectFormEnd), SeekOrigin.Begin);
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static void ReadMemoryMapsForm(BinaryReader BReader, long formEnd, ProjectDetails projectData)
        {
            while (BReader.BaseStream.Position + 8 <= formEnd)
            {
                long chunkStart = BReader.BaseStream.Position;
                string chunkId = ReadFourCC(BReader);
                uint chunkSize = BReader.ReadUInt32();

                if (chunkId == "SMEM")
                {
                    if (chunkSize >= 4 && BReader.BaseStream.Position + 4 <= formEnd)
                    {
                        projectData.MaximumMemoryMapSize = BReader.ReadInt32();
                    }
                    BReader.BaseStream.Seek(GetChunkEnd(chunkStart, chunkSize, formEnd), SeekOrigin.Begin);
                }
                else if (chunkId == "NAME")
                {
                    ProjectMemoryMap memoryMap = new ProjectMemoryMap
                    {
                        Name = ReadChunkString(BReader, chunkSize)
                    };
                    BReader.BaseStream.Seek(GetChunkEnd(chunkStart, chunkSize, formEnd), SeekOrigin.Begin);

                    if (BReader.BaseStream.Position + 8 <= formEnd)
                    {
                        long slotChunkStart = BReader.BaseStream.Position;
                        string slotChunkId = ReadFourCC(BReader);
                        uint slotChunkSize = BReader.ReadUInt32();

                        if (slotChunkId == "SLOT")
                        {
                            ReadSlotSizes(BReader, slotChunkSize, memoryMap);
                            BReader.BaseStream.Seek(GetChunkEnd(slotChunkStart, slotChunkSize, formEnd), SeekOrigin.Begin);
                        }
                        else
                        {
                            BReader.BaseStream.Seek(slotChunkStart, SeekOrigin.Begin);
                        }
                    }

                    AddMemoryMap(projectData, memoryMap);
                }
                else
                {
                    BReader.BaseStream.Seek(GetChunkEnd(chunkStart, chunkSize, formEnd), SeekOrigin.Begin);
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static void AddMemoryMap(ProjectDetails projectData, ProjectMemoryMap memoryMap)
        {
            projectData.memoryMapsData.Add(memoryMap);

            for (int i = 0; i < memoryMap.SlotSizes.Count; i++)
            {
                projectData.memorySlotsData.Add(new ProjectSlots
                {
                    SlotNumber = i,
                    MemorySize = memoryMap.SlotSizes[i],
                    Quantity = projectData.memoryMapsData.Count
                });
            }

            projectData.MemmorySlotsCount = projectData.memorySlotsData.Count;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static void ReadUserValuesChunk(BinaryReader BReader, uint chunkSize, ProjectDetails projectData)
        {
            uint valueCount = chunkSize / 4;
            for (int i = 0; i < valueCount; i++)
            {
                int value = BReader.ReadInt32();
                projectData.userValues.Add(value);
                if (i < projectData.flagsValues.Length)
                {
                    projectData.flagsValues[i] = value;
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static void ReadSlotSizes(BinaryReader BReader, uint chunkSize, ProjectMemoryMap memoryMap)
        {
            uint slotCount = chunkSize / 4;
            for (int i = 0; i < slotCount; i++)
            {
                memoryMap.SlotSizes.Add(BReader.ReadInt32());
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static string ReadChunkString(BinaryReader BReader, uint chunkSize)
        {
            byte[] data = BReader.ReadBytes((int)chunkSize);
            int textLength = data.Length;
            while (textLength > 0 && data[textLength - 1] == 0)
            {
                textLength--;
            }

            return Encoding.ASCII.GetString(data, 0, textLength);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static string ReadFourCC(BinaryReader BReader)
        {
            return Encoding.ASCII.GetString(BReader.ReadBytes(4));
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static long GetFormEnd(long formStart, uint formSize, long limit)
        {
            long formEnd = formStart + 8 + formSize;
            return formEnd > limit ? limit : formEnd;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static long GetChunkEnd(long chunkStart, uint chunkSize, long limit)
        {
            long chunkEnd = chunkStart + 8 + chunkSize;
            if ((chunkSize & 1) != 0)
            {
                chunkEnd++;
            }

            return chunkEnd > limit ? limit : chunkEnd;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
