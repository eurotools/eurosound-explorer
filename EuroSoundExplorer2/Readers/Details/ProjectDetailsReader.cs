using MusX.Objects;
using System;
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

            if (headerData.FileVersion == 10 || headerData.FileVersion == 18 || headerData.FileVersion == 21)
            {
                headerData.MemoryStart = 0x800;
                headerData.MemoryLength = headerData.FileSize > 0x800 ? headerData.FileSize - 0x800 : 0;
                return headerData;
            }

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
                if (headerData.FileVersion == 21)
                {
                    ReadProjectFileVersion21(BReader, headerData, projectData);
                    return projectData;
                }

                if (headerData.FileVersion == 18)
                {
                    ReadProjectFileVersion18(BReader, headerData, projectData);
                    return projectData;
                }

                if (headerData.FileVersion == 6 || headerData.FileVersion == 10)
                {
                    ReadProjectFileVersion6(BReader, headerData, projectData);
                    return projectData;
                }

                // Some v1/v201 projects publish only the four-byte empty payload.
                if (headerData.MemoryLength < 16 || headerData.MemoryStart > BReader.BaseStream.Length - 16)
                    return projectData;

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

        private static void ReadProjectFileVersion18(BinaryReader reader, ProjectDetailsHeader header, ProjectDetails project)
        {
            reader.BaseStream.Position = 0x800;
            if (ReadFourCC(reader) != "ESPD") throw new InvalidDataException("EngineXT ProjectDetails has no ESPD descriptor.");
            bool bigEndian = header.IsBigEndian;
            uint version = ReadUInt32(reader, bigEndian);
            if (version != 18) throw new InvalidDataException("The EngineXT reader currently supports ESPD data version 18.");
            uint dataSizeOrPublish = ReadUInt32(reader, bigEndian);
            ReadUInt32(reader, bigEndian);

            uint soundbankCount;
            long soundbankTable;
            long memoryMap;
            uint effectsCount, mixCount, duckCount, cullCount, oscillatorCount;
            long effectsTable, mixTable, duckTable, cullTable, oscillatorTable;
            if (dataSizeOrPublish == 0)
            {
                // Current EngineXT v18 publisher: file size + stream heap, followed by eleven count/offset pairs.
                ReadUInt32(reader, bigEndian);
                project.StereoStreamCount = unchecked((int)ReadUInt32(reader, bigEndian));
                uint memoryMapCount = ReadUInt32(reader, bigEndian);
                long memoryOffsetField = reader.BaseStream.Position;
                memoryMap = memoryOffsetField + ReadInt32(reader, bigEndian);
                effectsCount = ReadUInt32(reader, bigEndian); effectsTable = ReadRelativeTarget(reader, bigEndian);
                mixCount = ReadUInt32(reader, bigEndian); mixTable = ReadRelativeTarget(reader, bigEndian);
                duckCount = ReadUInt32(reader, bigEndian); duckTable = ReadRelativeTarget(reader, bigEndian);
                cullCount = ReadUInt32(reader, bigEndian); cullTable = ReadRelativeTarget(reader, bigEndian);
                soundbankCount = ReadUInt32(reader, bigEndian);
                long soundbankOffsetField = reader.BaseStream.Position;
                soundbankTable = soundbankOffsetField + ReadInt32(reader, bigEndian);
                oscillatorCount = ReadUInt32(reader, bigEndian); oscillatorTable = ReadRelativeTarget(reader, bigEndian);
                project.ProjectCode = (int)memoryMapCount;
            }
            else
            {
                // Original v18 layout used by shipped EngineXT titles.
                ReadUInt32(reader, bigEndian); // output count
                effectsCount = ReadUInt32(reader, bigEndian); effectsTable = ReadRelativeTarget(reader, bigEndian);
                mixCount = ReadUInt32(reader, bigEndian); mixTable = ReadRelativeTarget(reader, bigEndian);
                duckCount = ReadUInt32(reader, bigEndian); duckTable = ReadRelativeTarget(reader, bigEndian);
                cullCount = ReadUInt32(reader, bigEndian); cullTable = ReadRelativeTarget(reader, bigEndian);
                soundbankCount = ReadUInt32(reader, bigEndian);
                long soundbankOffsetField = reader.BaseStream.Position;
                soundbankTable = soundbankOffsetField + ReadInt32(reader, bigEndian);
                oscillatorCount = ReadUInt32(reader, bigEndian); oscillatorTable = ReadRelativeTarget(reader, bigEndian);
                memoryMap = reader.BaseStream.Position;
            }

            project.EffectsCount = (int)effectsCount;
            project.MixGroupsCount = (int)mixCount;
            project.DuckersCount = (int)duckCount;
            project.CullingGroupsCount = (int)cullCount;
            project.OscillatorsCount = (int)oscillatorCount;

            if (memoryMap + 72 <= reader.BaseStream.Length)
            {
                reader.BaseStream.Position = memoryMap;
                ProjectMemoryMap map = new ProjectMemoryMap();
                project.MaximumMemoryMapSize = ReadInt32(reader, bigEndian);
                map.Name = string.Format("0x{0:X8}", ReadUInt32(reader, bigEndian));
                for (int i = 0; i < 8; i++)
                {
                    uint slotHash = ReadUInt32(reader, bigEndian);
                    uint sizeAndHeap = ReadUInt32(reader, bigEndian);
                    int size = (int)(sizeAndHeap & 0x7fffffff);
                    if (slotHash == 0 && size == 0) continue;
                    map.SlotSizes.Add(size);
                    project.memorySlotsData.Add(new ProjectSlots { SlotNumber = unchecked((int)slotHash), MemorySize = size, Quantity = (sizeAndHeap & 0x80000000) != 0 ? 1 : 0 });
                }
                project.memoryMapsData.Add(map);
                project.MemmorySlotsCount = project.memorySlotsData.Count;
            }

            project.SoundBanksCount = (int)soundbankCount;
            project.SoundBanksOffset = (int)soundbankTable;
            if (soundbankTable < 0 || soundbankTable > reader.BaseStream.Length - soundbankCount * 12L) return;
            reader.BaseStream.Position = soundbankTable;
            for (int i = 0; i < soundbankCount; i++)
            {
                int bankHash = ReadInt32(reader, bigEndian);
                ReadInt32(reader, bigEndian);
                int slotHash = ReadInt32(reader, bigEndian);
                project.soundBanksData.Add(new ProjectSoundBank { HashCode = bankHash, SlotNumber = slotHash });
            }


            ReadEffects(reader, project, effectsTable, effectsCount, bigEndian);
            ReadMixGroups(reader, project, mixTable, mixCount, bigEndian);
            ReadDuckers(reader, project, duckTable, duckCount, bigEndian);
            ReadCullingGroups(reader, project, cullTable, cullCount, bigEndian);
            ReadOscillators(reader, project, oscillatorTable, oscillatorCount, bigEndian);
        }

        private static void ReadProjectFileVersion21(BinaryReader reader, ProjectDetailsHeader header, ProjectDetails project)
        {
            reader.BaseStream.Position = 0x800;
            if (ReadFourCC(reader) != "ESPD") throw new InvalidDataException("MusX Project Details has no ESPD descriptor.");
            bool bigEndian = header.IsBigEndian;
            uint version = ReadUInt32(reader, bigEndian);
            if (version != 21) throw new InvalidDataException(string.Format("Expected ESPD 21 but found ESPD {0}.", version));

            project.PublishCount = ReadUInt32(reader, bigEndian);
            project.OutputCount = ReadUInt32(reader, bigEndian);
            uint espdSize = ReadUInt32(reader, bigEndian);
            project.ListenerVelocitySmoothing = ReadSingle(reader, bigEndian);
            project.StreamHeapSize0 = ReadUInt32(reader, bigEndian);
            project.StreamHeapSize1 = ReadUInt32(reader, bigEndian);

            uint memoryCount = ReadUInt32(reader, bigEndian); long memoryTable = ReadRelativeTarget(reader, bigEndian);
            uint effectsCount = ReadUInt32(reader, bigEndian); long effectsTable = ReadRelativeTarget(reader, bigEndian);
            uint mixCount = ReadUInt32(reader, bigEndian); long mixTable = ReadRelativeTarget(reader, bigEndian);
            uint duckerCount = ReadUInt32(reader, bigEndian); long duckerTable = ReadRelativeTarget(reader, bigEndian);
            uint cullingCount = ReadUInt32(reader, bigEndian); long cullingTable = ReadRelativeTarget(reader, bigEndian);
            uint soundbankCount = ReadUInt32(reader, bigEndian); long soundbankTable = ReadRelativeTarget(reader, bigEndian);
            uint oscillatorCount = ReadUInt32(reader, bigEndian); long oscillatorTable = ReadRelativeTarget(reader, bigEndian);
            uint gameVarCount = ReadUInt32(reader, bigEndian); long gameVarTable = ReadRelativeTarget(reader, bigEndian);
            uint controllerCount = ReadUInt32(reader, bigEndian); long controllerTable = ReadRelativeTarget(reader, bigEndian);
            uint eventCount = ReadUInt32(reader, bigEndian); long eventTable = ReadRelativeTarget(reader, bigEndian);
            uint tagCount = ReadUInt32(reader, bigEndian); long tagTable = ReadRelativeTarget(reader, bigEndian);
            uint spreadsheetCount = ReadUInt32(reader, bigEndian); long spreadsheetTable = ReadRelativeTarget(reader, bigEndian);

            long availablePayload = Math.Max(0, reader.BaseStream.Length - 0x800);
            if (espdSize != 0 && espdSize > availablePayload)
                throw new InvalidDataException("ESPD 21 declares a size larger than the available MusX payload.");

            project.EffectsCount = checked((int)effectsCount);
            project.MixGroupsCount = checked((int)mixCount);
            project.DuckersCount = checked((int)duckerCount);
            project.CullingGroupsCount = checked((int)cullingCount);
            project.OscillatorsCount = checked((int)oscillatorCount);
            project.GameVarsCount = checked((int)gameVarCount);
            project.ControllersCount = checked((int)controllerCount);
            project.EventsCount = checked((int)eventCount);
            project.TagsCount = checked((int)tagCount);
            project.SpreadsheetsCount = checked((int)spreadsheetCount);

            ReadMemoryMapsV21(reader, project, memoryTable, memoryCount, bigEndian);
            ReadSoundbankLookups(reader, project, soundbankTable, soundbankCount, bigEndian);
            ReadEffects(reader, project, effectsTable, effectsCount, bigEndian);
            ReadMixGroups(reader, project, mixTable, mixCount, bigEndian);
            ReadDuckers(reader, project, duckerTable, duckerCount, bigEndian);
            ReadCullingGroups(reader, project, cullingTable, cullingCount, bigEndian);
            ReadOscillators(reader, project, oscillatorTable, oscillatorCount, bigEndian);
            ReadFixedRuntimeTable(reader, project, gameVarTable, gameVarCount, 12, "Game Variable", bigEndian);
            ReadControllersV21(reader, project, controllerTable, controllerCount, bigEndian);
            ReadFixedRuntimeTable(reader, project, eventTable, eventCount, 32, "Event", bigEndian);
            ReadFixedRuntimeTable(reader, project, tagTable, tagCount, 4, "Tag", bigEndian);
            ReadFixedRuntimeTable(reader, project, spreadsheetTable, spreadsheetCount, 12, "Spreadsheet", bigEndian);
        }

        private static void ReadMemoryMapsV21(BinaryReader reader, ProjectDetails project, long table, uint count, bool bigEndian)
        {
            const int MapBytes = 68;
            if (!CanRead(reader, table, 4 + count * (long)MapBytes)) return;
            reader.BaseStream.Position = table;
            project.MaximumMemoryMapSize = ReadInt32(reader, bigEndian);
            for (uint mapIndex = 0; mapIndex < count; mapIndex++)
            {
                ProjectMemoryMap map = new ProjectMemoryMap { Name = string.Format("0x{0:X8}", ReadUInt32(reader, bigEndian)) };
                for (int slot = 0; slot < 8; slot++)
                {
                    uint slotHash = ReadUInt32(reader, bigEndian);
                    uint sizeAndHeap = ReadUInt32(reader, bigEndian);
                    int size = checked((int)(sizeAndHeap & 0x7fffffff));
                    if (slotHash == 0 && size == 0) continue;
                    map.SlotSizes.Add(size);
                    project.memorySlotsData.Add(new ProjectSlots { SlotNumber = unchecked((int)slotHash), MemorySize = size, Quantity = (sizeAndHeap & 0x80000000) == 0 ? 1 : 0 });
                }
                project.memoryMapsData.Add(map);
            }
            project.MemmorySlotsCount = project.memorySlotsData.Count;
        }

        private static void ReadSoundbankLookups(BinaryReader reader, ProjectDetails project, long table, uint count, bool bigEndian)
        {
            if (!CanRead(reader, table, count * 12L)) return;
            project.SoundBanksCount = checked((int)count);
            project.SoundBanksOffset = checked((int)table);
            for (uint i = 0; i < count; i++)
            {
                reader.BaseStream.Position = table + i * 12L;
                int bankHash = ReadInt32(reader, bigEndian);
                int mapHash = ReadInt32(reader, bigEndian);
                int slotHash = ReadInt32(reader, bigEndian);
                project.soundBanksData.Add(new ProjectSoundBank { HashCode = bankHash, SlotNumber = slotHash });
                project.runtimeObjects.Add(new ProjectRuntimeObject { Type = "Soundbank Lookup", HashCode = unchecked((uint)bankHash), Details = string.Format("Memory map 0x{0:X8}, slot 0x{1:X8}", mapHash, slotHash) });
            }
        }

        private static void ReadFixedRuntimeTable(BinaryReader reader, ProjectDetails project, long table, uint count, int stride, string type, bool bigEndian)
        {
            if (!CanRead(reader, table, count * (long)stride)) return;
            for (uint i = 0; i < count; i++)
            {
                reader.BaseStream.Position = table + i * (long)stride;
                uint hash = ReadUInt32(reader, bigEndian);
                project.runtimeObjects.Add(new ProjectRuntimeObject { Type = type, HashCode = hash, Details = string.Format("{0}-byte ESPD 21 record", stride) });
            }
        }

        private static void ReadPointerRuntimeTable(BinaryReader reader, ProjectDetails project, long table, uint count, string type, bool bigEndian)
        {
            if (!CanRead(reader, table, count * 8L)) return;
            for (uint i = 0; i < count; i++)
            {
                reader.BaseStream.Position = table + i * 8L;
                uint hash = ReadUInt32(reader, bigEndian);
                long data = ReadRelativeTarget(reader, bigEndian);
                project.runtimeObjects.Add(new ProjectRuntimeObject { Type = type, HashCode = hash, Details = string.Format("Data at 0x{0:X}", data) });
            }
        }

        private static void ReadControllersV21(BinaryReader reader, ProjectDetails project, long table, uint count, bool bigEndian)
        {
            const int ControllerInfoBytes = 12;
            if (!CanRead(reader, table, count * (long)ControllerInfoBytes)) return;
            for (uint i = 0; i < count; i++)
            {
                reader.BaseStream.Position = table + i * (long)ControllerInfoBytes;
                uint hash = ReadUInt32(reader, bigEndian);
                uint elementCount = ReadUInt32(reader, bigEndian);
                long elementTable = ReadRelativeTarget(reader, bigEndian);
                project.runtimeObjects.Add(new ProjectRuntimeObject
                {
                    Type = "Controller",
                    HashCode = hash,
                    Details = string.Format("{0} elements, table at 0x{1:X}", elementCount, elementTable)
                });
            }
        }

        private static long ReadRelativeTarget(BinaryReader reader, bool bigEndian)
        {
            long field = reader.BaseStream.Position;
            int relative = ReadInt32(reader, bigEndian);
            return relative == 0 ? 0 : field + relative;
        }

        private static void ReadEffects(BinaryReader reader, ProjectDetails project, long table, uint count, bool bigEndian)
        {
            if (!CanRead(reader, table, count * 8L)) return;
            for (uint i = 0; i < count; i++)
            {
                reader.BaseStream.Position = table + i * 8L;
                uint hash = ReadUInt32(reader, bigEndian);
                long data = ReadRelativeTarget(reader, bigEndian);
                string details = string.Empty;
                if (CanRead(reader, data, 8))
                {
                    reader.BaseStream.Position = data;
                    uint dataHash = ReadUInt32(reader, bigEndian);
                    uint type = ReadUInt32(reader, bigEndian);
                    int parameterCount = GetEffectParameterCount(type);
                    System.Text.StringBuilder values = new System.Text.StringBuilder();
                    for (int parameter = 0; parameter < parameterCount && CanRead(reader, reader.BaseStream.Position, 4); parameter++)
                    {
                        if (parameter > 0) values.Append(", ");
                        if (type == 1 && parameter == 0) values.Append(ReadInt32(reader, bigEndian));
                        else values.Append(ReadSingle(reader, bigEndian).ToString("0.###", System.Globalization.CultureInfo.InvariantCulture));
                    }
                    details = string.Format("Type {0}, data hash 0x{1:X8}, parameters [{2}]", type, dataHash, values);
                }
                project.runtimeObjects.Add(new ProjectRuntimeObject { Type = "Effect", HashCode = hash, Details = details });
            }
        }

        private static void ReadMixGroups(BinaryReader reader, ProjectDetails project, long table, uint count, bool bigEndian)
        {
            if (!CanRead(reader, table, count * 12L)) return;
            for (uint i = 0; i < count; i++)
            {
                reader.BaseStream.Position = table + i * 12L;
                uint hash = ReadUInt32(reader, bigEndian);
                float volume = ReadSingle(reader, bigEndian);
                uint parent = ReadUInt32(reader, bigEndian);
                project.runtimeObjects.Add(new ProjectRuntimeObject { Type = "Mix Group", HashCode = hash, Details = string.Format("Volume {0:0.###}, parent 0x{1:X8}", volume, parent) });
            }
        }

        private static void ReadDuckers(BinaryReader reader, ProjectDetails project, long table, uint count, bool bigEndian)
        {
            if (!CanRead(reader, table, count * 8L)) return;
            for (uint i = 0; i < count; i++)
            {
                reader.BaseStream.Position = table + i * 8L;
                uint hash = ReadUInt32(reader, bigEndian);
                long data = ReadRelativeTarget(reader, bigEndian);
                string details = "0 inputs";
                if (CanRead(reader, data, 4))
                {
                    reader.BaseStream.Position = data;
                    uint inputs = ReadUInt32(reader, bigEndian);
                    System.Text.StringBuilder values = new System.Text.StringBuilder();
                    for (uint input = 0; input < inputs && CanRead(reader, reader.BaseStream.Position, 4); input++)
                    {
                        if (input > 0) values.Append(", ");
                        ushort inputHash = ReadUInt16(reader, bigEndian);
                        byte volume = reader.ReadByte(); reader.ReadByte();
                        values.AppendFormat("0x{0:X4}: {1}%", inputHash, volume);
                    }
                    details = inputs + " inputs [" + values + "]";
                }
                project.runtimeObjects.Add(new ProjectRuntimeObject { Type = "Ducker", HashCode = hash, Details = details });
            }
        }

        private static void ReadCullingGroups(BinaryReader reader, ProjectDetails project, long table, uint count, bool bigEndian)
        {
            if (!CanRead(reader, table, count * 12L)) return;
            for (uint i = 0; i < count; i++)
            {
                reader.BaseStream.Position = table + i * 12L;
                uint hash = ReadUInt32(reader, bigEndian);
                uint maximum = ReadUInt32(reader, bigEndian);
                uint action = ReadUInt32(reader, bigEndian);
                project.runtimeObjects.Add(new ProjectRuntimeObject { Type = "Culling Group", HashCode = hash, Details = string.Format("Maximum {0}, action {1}", maximum, action) });
            }
        }

        private static void ReadOscillators(BinaryReader reader, ProjectDetails project, long table, uint count, bool bigEndian)
        {
            bool version21 = project.FormatVersion >= 21;
            int stride = version21 ? 40 : 36;
            if (!CanRead(reader, table, count * (long)stride)) return;
            string[] names = version21
                ? new[] { "Pitch", "Volume", "LowPass", "HighPass", "Morph", "Angle" }
                : new[] { "Pitch", "Volume", "LowPass", "Morph", "Angle" };
            for (uint i = 0; i < count; i++)
            {
                reader.BaseStream.Position = table + i * (long)stride;
                uint hash = ReadUInt32(reader, bigEndian);
                System.Text.StringBuilder details = new System.Text.StringBuilder();
                for (int component = 0; component < names.Length; component++)
                {
                    if (component > 0) details.Append("; ");
                    byte waveType = reader.ReadByte();
                    byte amplitude = reader.ReadByte();
                    ushort rate = ReadUInt16(reader, bigEndian);
                    ushort release = ReadUInt16(reader, bigEndian);
                    details.AppendFormat("{0}: wave {1}, amp {2}, rate {3}ms, release {4}ms", names[component], waveType, amplitude, rate, release);
                }
                if (!version21) ReadUInt16(reader, bigEndian);
                project.runtimeObjects.Add(new ProjectRuntimeObject { Type = "Oscillator", HashCode = hash, Details = details.ToString() });
            }
        }

        private static int GetEffectParameterCount(uint type)
        {
            switch (type)
            {
                case 0: return 6;
                case 1: return 28;
                case 2: return 6;
                case 3: return 5;
                case 4: return 4;
                case 5: return 3;
                case 6:
                case 7:
                case 8: return 2;
                default: return 0;
            }
        }

        private static float ReadSingle(BinaryReader reader, bool bigEndian)
        {
            byte[] bytes = reader.ReadBytes(4);
            if (bigEndian) System.Array.Reverse(bytes);
            return System.BitConverter.ToSingle(bytes, 0);
        }

        private static bool CanRead(BinaryReader reader, long position, long size)
        {
            return position >= 0 && size >= 0 && position <= reader.BaseStream.Length - size;
        }

        private static uint ReadUInt32(BinaryReader reader, bool bigEndian) { return BytesFunctions.FlipData(reader.ReadUInt32(), bigEndian); }
        private static int ReadInt32(BinaryReader reader, bool bigEndian) { return BytesFunctions.FlipData(reader.ReadInt32(), bigEndian); }
        private static ushort ReadUInt16(BinaryReader reader, bool bigEndian) { return BytesFunctions.FlipData(reader.ReadUInt16(), bigEndian); }

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
                else if (chunkId == "NAME" || chunkId == "MMAP")
                {
                    uint memoryMapHashCode = 0;
                    uint memoryMapNameSize = chunkSize;
                    if (chunkId == "MMAP" && chunkSize >= 4)
                    {
                        memoryMapHashCode = BReader.ReadUInt32();
                        memoryMapNameSize -= 4;
                    }
                    ProjectMemoryMap memoryMap = new ProjectMemoryMap
                    {
                        Name = ReadChunkString(BReader, memoryMapNameSize)
                    };
                    BReader.BaseStream.Seek(GetChunkEnd(chunkStart, chunkSize, formEnd), SeekOrigin.Begin);

                    if (BReader.BaseStream.Position + 8 <= formEnd)
                    {
                        long slotChunkStart = BReader.BaseStream.Position;
                        string slotChunkId = ReadFourCC(BReader);
                        uint slotChunkSize = BReader.ReadUInt32();

                        if (slotChunkId == "SLOT")
                        {
                            if (chunkId == "MMAP")
                                ReadVersion10SlotSizes(BReader, slotChunkSize, memoryMap, projectData);
                            else
                                ReadSlotSizes(BReader, slotChunkSize, memoryMap);
                            BReader.BaseStream.Seek(GetChunkEnd(slotChunkStart, slotChunkSize, formEnd), SeekOrigin.Begin);
                        }
                        else
                        {
                            BReader.BaseStream.Seek(slotChunkStart, SeekOrigin.Begin);
                        }
                    }

                    AddMemoryMap(projectData, memoryMap);
                    if (memoryMapHashCode != 0)
                    {
                        projectData.runtimeObjects.Add(new ProjectRuntimeObject
                        {
                            Type = "MemoryMap",
                            HashCode = memoryMapHashCode,
                            Details = memoryMap.Name
                        });
                    }
                }
                else
                {
                    BReader.BaseStream.Seek(GetChunkEnd(chunkStart, chunkSize, formEnd), SeekOrigin.Begin);
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static void ReadVersion10SlotSizes(BinaryReader reader, uint chunkSize, ProjectMemoryMap memoryMap, ProjectDetails projectData)
        {
            uint pairCount = chunkSize / 8;
            for (int i = 0; i < pairCount; i++)
            {
                uint slotHashCode = reader.ReadUInt32();
                int slotSize = reader.ReadInt32();
                memoryMap.SlotSizes.Add(slotSize);
                projectData.runtimeObjects.Add(new ProjectRuntimeObject
                {
                    Type = "MemorySlot",
                    HashCode = slotHashCode,
                    Details = slotSize.ToString()
                });
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
