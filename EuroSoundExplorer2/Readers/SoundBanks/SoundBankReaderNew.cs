using MusX.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    internal class SoundBankReaderNew : SoundBankReader
    {
        internal static void ReadSoundbankHeaderV10(string filePath, SoundbankHeader headerData)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                V10Sections sections = ReadV10Sections(reader, headerData.IsBigEndian);
                headerData.SFXStart = checked((uint)sections.SfxStart);
                headerData.SFXLenght = checked((uint)Math.Max(0, sections.SfxEnd - sections.SfxStart));
                headerData.SampleInfoStart = checked((uint)sections.SampleHeaderStart);
                headerData.SampleInfoLenght = checked((uint)Math.Max(0, sections.SampleHeaderEnd - sections.SampleHeaderStart));
                headerData.SampleDataStart = checked((uint)sections.AudioStart);
                headerData.SampleDataLength = checked((uint)Math.Max(0, sections.AudioEnd - sections.AudioStart));
            }
        }

        internal static void ReadSoundbankV10(string filePath, SoundbankHeader headerData, SortedDictionary<uint, Sample> samplesDictionary, List<SampleData> wavesList, List<uint> duplicatedHashCodes)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                V10Sections sections = ReadV10Sections(reader, headerData.IsBigEndian);
                Dictionary<uint, short> waveIndices = ReadV10SampleHeaders(reader, filePath, headerData, sections, wavesList);

                long position = sections.SfxStart;
                while (position + 12 <= sections.SfxEnd)
                {
                    reader.BaseStream.Position = position;
                    if (ReadFourCC(reader) != "FORM") break;
                    uint formSize = ReadV10UInt32(reader, headerData.IsBigEndian);
                    long formEnd = Math.Min(sections.SfxEnd, position + 8L + formSize);
                    if (ReadFourCC(reader) != "PARA" || formEnd < reader.BaseStream.Position) break;

                    Sample sample = null;
                    long child = reader.BaseStream.Position;
                    while (child + 8 <= formEnd)
                    {
                        reader.BaseStream.Position = child;
                        string chunkId = ReadFourCC(reader);
                        uint chunkSize = ReadV10UInt32(reader, headerData.IsBigEndian);
                        long chunkData = reader.BaseStream.Position;
                        long chunkEnd = Math.Min(formEnd, chunkData + chunkSize);

                        if (chunkId == "DATA" && chunkSize >= 28)
                        {
                            sample = ReadV10Parameters(reader, headerData.IsBigEndian);
                        }
                        else if (chunkId == "FORM" && chunkSize >= 4 && ReadFourCC(reader) == "POOL")
                        {
                            if (sample == null) sample = new Sample();
                            ReadV10Pool(reader, chunkEnd, sample, waveIndices, headerData.IsBigEndian);
                        }

                        if (chunkEnd <= child) break;
                        child = chunkEnd;
                    }

                    if (sample != null)
                    {
                        if (samplesDictionary.ContainsKey(sample.HashCodeNumber)) duplicatedHashCodes.Add(sample.HashCodeNumber);
                        else samplesDictionary.Add(sample.HashCodeNumber, sample);
                    }

                    if (formEnd <= position) break;
                    position = formEnd;
                }
            }
        }

        private static Sample ReadV10Parameters(BinaryReader reader, bool bigEndian)
        {
            Sample sample = new Sample();
            sample.HashCodeNumber = NormalizeV10EngineXHashCode(ReadV10UInt32(reader, bigEndian));
            long parameterDataStart = reader.BaseStream.Position;
            sample.V10RawParameterData = reader.ReadBytes(24);
            reader.BaseStream.Position = parameterDataStart;
            sample.V10Flags = ReadV10UInt32(reader, bigEndian);
            sample.Flags = unchecked((ushort)(sample.V10Flags & 0xffff));
            sample.DuckerLenght = ReadV10Int16(reader, bigEndian);
            sample.MinDelay = ReadV10Int16(reader, bigEndian);
            sample.MaxDelay = ReadV10Int16(reader, bigEndian);
            sample.GroupHashCode = unchecked((short)ReadV10UInt16(reader, bigEndian));
            sample.ReverbSend = reader.ReadSByte();
            sample.MaxVoices = reader.ReadSByte();
            sample.Priority = reader.ReadSByte();
            sample.Ducker = reader.ReadSByte();
            sample.MasterVolume = reader.ReadSByte();
            sample.GroupMaxChannels = unchecked((sbyte)reader.ReadByte());
            sample.PlayType = reader.ReadByte();
            sample.DopplerValue = reader.ReadSByte();
            sample.SFXDucker = reader.ReadSByte();
            reader.ReadBytes(3);
            return sample;
        }

        private static uint NormalizeV10EngineXHashCode(uint hashCode)
        {
            // Some console builds serialize the runtime 0xED section byte even
            // though EngineX source hashes and sound.h use 0x2D. The engine's
            // CompareHashcodes masks this byte, so expose the canonical form.
            return (hashCode & 0xFF000000u) == 0xED000000u
                ? 0x2D000000u | (hashCode & 0x00FFFFFFu)
                : hashCode;
        }

        private static void ReadV10Pool(BinaryReader reader, long poolEnd, Sample sample, Dictionary<uint, short> waveIndices, bool bigEndian)
        {
            while (reader.BaseStream.Position + 8 <= poolEnd)
            {
                long chunkStart = reader.BaseStream.Position;
                string id = ReadFourCC(reader);
                uint size = ReadV10UInt32(reader, bigEndian);
                long chunkEnd = Math.Min(poolEnd, reader.BaseStream.Position + size);
                if (id == "ELMT" && size >= 20)
                {
                    uint referenceHash = ReadV10UInt32(reader, bigEndian);
                    SampleInfo item = new SampleInfo();
                    item.ReferenceHashCode = referenceHash;
                    item.Pitch = reader.ReadSByte() * 0.2f;
                    item.PitchOffset = reader.ReadByte() * 0.1f;
                    item.Volume = reader.ReadSByte();
                    item.VolumeOffset = reader.ReadSByte();
                    item.Pan = reader.ReadSByte();
                    item.PanOffset = reader.ReadSByte();
                    item.MinDelay = ReadV10Int16(reader, bigEndian);
                    item.MaxDelay = ReadV10Int16(reader, bigEndian);
                    item.DelayType = reader.ReadByte();
                    item.IsReleaseElement = reader.ReadByte();
                    item.Spare = reader.ReadByte();
                    reader.ReadBytes(3);

                    short waveIndex;
                    if (waveIndices.TryGetValue(referenceHash, out waveIndex)) item.FileRef = waveIndex;
                    else item.FileRef = unchecked((short)(referenceHash & 0xffff));
                    sample.samplesList.Add(item);
                }

                if (chunkEnd <= chunkStart) break;
                reader.BaseStream.Position = chunkEnd;
            }
        }

        private static Dictionary<uint, short> ReadV10SampleHeaders(BinaryReader reader, string soundbankPath, SoundbankHeader headerData, V10Sections sections, List<SampleData> waves)
        {
            Dictionary<uint, short> indices = new Dictionary<uint, short>();
            Dictionary<uint, string> externalFiles = null;
            long position = sections.SampleHeaderStart;
            while (position + 40 <= sections.SampleHeaderEnd)
            {
                reader.BaseStream.Position = position;
                string id = ReadFourCC(reader);
                uint size = ReadV10UInt32(reader, headerData.IsBigEndian);
                if ((id != "WAV_" && id != "STRM") || size < 32) break;

                uint hash = ReadV10UInt32(reader, headerData.IsBigEndian);
                uint frequency = ReadV10UInt32(reader, headerData.IsBigEndian);
                uint sampleCount = ReadV10UInt32(reader, headerData.IsBigEndian);
                uint channels = ReadV10UInt32(reader, headerData.IsBigEndian);
                uint dataAddress = ReadV10UInt32(reader, headerData.IsBigEndian);
                uint dataSize = ReadV10UInt32(reader, headerData.IsBigEndian);
                uint loopOffset = ReadV10UInt32(reader, headerData.IsBigEndian);
                byte loopFlag = reader.ReadByte();
                reader.ReadBytes(3);

                WavType storage = id == "WAV_" ? WavType.Memory : WavType.Stream;
                string audioPath = soundbankPath;
                uint audioOffset = checked((uint)(sections.AudioStart + dataAddress));
                uint audioSize = dataSize;
                EuroSoundAudioCodec codec = EuroSoundCodecMatrix.GetCodec(10, headerData.Platform, EuroSoundBankType.SoundBank);
                // Pirates Wii keeps resident samples in NGCA/DSP, but streamed
                // samples use Eurocom IMA blocks in the external STR files.
                if (storage == WavType.Stream && EuroSoundCodecMatrix.IsGameCubePlatform(headerData.Platform))
                {
                    codec = EuroSoundAudioCodec.EurocomImaAdpcm;
                }
                if (storage == WavType.Stream)
                {
                    if (externalFiles == null) externalFiles = BuildV10AudioFileMap(soundbankPath);
                    externalFiles.TryGetValue(hash, out audioPath);
                    audioOffset = EuroSoundCodecMatrix.IsGameCubePlatform(headerData.Platform) ? 0x380u : 0x800u;
                    if (!string.IsNullOrEmpty(audioPath))
                    {
                        long available = Math.Max(0, new FileInfo(audioPath).Length - audioOffset);
                        audioSize = (uint)Math.Min(dataSize, Math.Min(uint.MaxValue, available));
                        if (codec == EuroSoundAudioCodec.EurocomImaAdpcm)
                        {
                            audioSize = TrimV10ExternalImaPadding(audioPath, audioOffset, audioSize, checked((int)channels));
                        }
                    }
                }

                uint encodedLoopOffset = loopOffset;
                if (storage == WavType.Stream && encodedLoopOffset >= audioOffset) encodedLoopOffset -= audioOffset;
                // MUSX 10 follows the legacy EuroSound convention: loop offsets are
                // byte positions in decoded 16-bit PCM, not Sony ADPCM byte positions.
                uint loopSample = loopFlag == 0 ? 0 : codec == EuroSoundAudioCodec.SonyVagAdpcm
                    ? CalculusLoopOffsets.Pcm16BytesToSamples(encodedLoopOffset, Math.Max(1, (int)channels))
                    : EuroSoundCodecMatrix.SoundBankLoopOffsetToSamples(codec, encodedLoopOffset, Math.Max(1, (int)channels));
                SampleData wave = new SampleData
                {
                    WavHashCode = hash,
                    StorageType = storage,
                    Flags = loopFlag == 0 ? 0u : 1u,
                    Address = audioOffset,
                    MemorySize = dataSize,
                    SampleSize = audioSize,
                    Frequency = frequency,
                    Channels = channels,
                    TotalSamples = sampleCount,
                    OriginalLoopOffset = loopFlag == 0 || loopOffset == uint.MaxValue ? 0u : loopOffset,
                    LoopStartOffset = loopSample,
                    LoopStartSample = loopSample,
                    AudioReference = string.IsNullOrEmpty(audioPath) ? null : new AudioDataReference
                    {
                        FilePath = audioPath,
                        Offset = audioOffset,
                        Size = audioSize,
                        Codec = codec,
                        Frequency = frequency,
                        Channels = checked((int)channels)
                    }
                };
                indices[hash] = checked((short)waves.Count);
                waves.Add(wave);
                position += 8L + size;
            }
            return indices;
        }

        private static uint TrimV10ExternalImaPadding(string filePath, uint offset, uint size, int channels)
        {
            int blockSetSize = 32 * Math.Max(1, channels);
            if (size < blockSetSize)
            {
                return size;
            }

            const int MaximumSectorPadding = 0x1000;
            int tailSize = (int)Math.Min(size, MaximumSectorPadding);
            byte[] tail = new byte[tailSize];
            using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                stream.Position = offset + size - (uint)tailSize;
                int read = 0;
                while (read < tail.Length)
                {
                    int count = stream.Read(tail, read, tail.Length - read);
                    if (count == 0) break;
                    read += count;
                }
            }

            int trailingPadding = 0;
            for (int i = tail.Length - 1; i >= 0 && tail[i] == 0xAB; i--)
            {
                trailingPadding++;
            }

            if (trailingPadding == 0)
            {
                return size - (size % (uint)blockSetSize);
            }

            uint withoutPadding = size - (uint)trailingPadding;
            return withoutPadding - (withoutPadding % (uint)blockSetSize);
        }

        private static Dictionary<uint, string> BuildV10AudioFileMap(string soundbankPath)
        {
            Dictionary<uint, string> result = new Dictionary<uint, string>();
            DirectoryInfo directory = new FileInfo(soundbankPath).Directory;
            if (directory != null && directory.Parent != null) directory = directory.Parent;
            if (directory == null) return result;
            foreach (string path in Directory.GetFiles(directory.FullName, "*.sfx", SearchOption.AllDirectories))
            {
                try
                {
                    using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                    {
                        if (reader.BaseStream.Length < 12 || ReadFourCC(reader) != "MUSX") continue;
                        uint hash = reader.ReadUInt32();
                        if (!result.ContainsKey(hash)) result.Add(hash, path);
                    }
                }
                catch (IOException) { }
            }
            return result;
        }

        private sealed class V10Sections
        {
            internal long SfxStart;
            internal long SfxEnd;
            internal long SampleHeaderStart;
            internal long SampleHeaderEnd;
            internal long AudioStart;
            internal long AudioEnd;
        }

        private static V10Sections ReadV10Sections(BinaryReader reader, bool bigEndian)
        {
            reader.BaseStream.Position = 0x800;
            if (ReadFourCC(reader) != "FORM") throw new InvalidDataException("MUSX v10 soundbank has no root FORM.");
            uint rootSize = ReadV10UInt32(reader, bigEndian);
            long rootEnd = Math.Min(reader.BaseStream.Length, 0x808L + rootSize);
            if (ReadFourCC(reader) != "SBNK") throw new InvalidDataException("MUSX v10 FORM is not an SBNK soundbank.");
            V10Sections sections = new V10Sections();
            while (reader.BaseStream.Position + 8 <= rootEnd)
            {
                long chunkStart = reader.BaseStream.Position;
                string id = ReadFourCC(reader);
                uint size = ReadV10UInt32(reader, bigEndian);
                long dataStart = reader.BaseStream.Position;
                long chunkEnd = Math.Min(rootEnd, dataStart + size);
                if (id == "FORM" && size >= 4)
                {
                    string type = ReadFourCC(reader);
                    if (type == "SFXP") { sections.SfxStart = reader.BaseStream.Position; sections.SfxEnd = chunkEnd; }
                    else if (type == "SHDA") { sections.SampleHeaderStart = reader.BaseStream.Position; sections.SampleHeaderEnd = chunkEnd; }
                }
                else if (id == "AUDD")
                {
                    sections.AudioStart = dataStart;
                    sections.AudioEnd = chunkEnd;
                }
                if (chunkEnd <= chunkStart) break;
                reader.BaseStream.Position = chunkEnd;
            }
            if (sections.SfxStart == 0 || sections.SampleHeaderStart == 0 || sections.AudioStart == 0)
                throw new InvalidDataException("MUSX v10 SBNK is missing SFXP, SHDA or AUDD.");
            return sections;
        }

        private static string ReadFourCC(BinaryReader reader)
        {
            byte[] value = reader.ReadBytes(4);
            return value.Length == 4 ? Encoding.ASCII.GetString(value) : string.Empty;
        }

        private static uint ReadV10UInt32(BinaryReader reader, bool bigEndian) { return BytesFunctions.FlipData(reader.ReadUInt32(), bigEndian); }
        private static ushort ReadV10UInt16(BinaryReader reader, bool bigEndian) { return BytesFunctions.FlipData(reader.ReadUInt16(), bigEndian); }
        private static short ReadV10Int16(BinaryReader reader, bool bigEndian) { return BytesFunctions.FlipData(reader.ReadInt16(), bigEndian); }

        internal static void ReadSoundbankV18(string filePath, SoundbankHeader headerData, SortedDictionary<uint, Sample> samplesDictionary, List<SampleData> wavesList, List<uint> duplicatedHashCodes)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                reader.BaseStream.Position = 0x800;
                if (new string(reader.ReadChars(4)) != "SBNK") throw new InvalidDataException("Invalid EngineXT SBNK descriptor.");
                bool bigEndian = headerData.IsBigEndian;
                uint dataVersion = ReadV18UInt32(reader, bigEndian);
                if (dataVersion != 18 && dataVersion != 21 && dataVersion != 39) throw new InvalidDataException("The EngineXT reader currently supports SBNK data versions 18, 21 and 39.");
                ReadV18UInt32(reader, bigEndian); // language
                ReadV18UInt32(reader, bigEndian); // soundbank hash
                if (dataVersion >= 21) ReadV18UInt32(reader, bigEndian); // memory-slot hash added in v21

                uint sfxCount = ReadV18UInt32(reader, bigEndian); long sfxOffsetField = reader.BaseStream.Position; long sfxTable = ResolveRelative(sfxOffsetField, ReadV18Int32(reader, bigEndian));
                ReadV18UInt32(reader, bigEndian); ReadV18Int32(reader, bigEndian);
                ReadV18UInt32(reader, bigEndian); ReadV18Int32(reader, bigEndian);
                ReadV18UInt32(reader, bigEndian); ReadV18Int32(reader, bigEndian);
                uint memoryCount = ReadV18UInt32(reader, bigEndian); long memoryOffsetField = reader.BaseStream.Position; int memoryOffset = ReadV18Int32(reader, bigEndian);
                uint streamCount = ReadV18UInt32(reader, bigEndian); long streamOffsetField = reader.BaseStream.Position; int streamOffset = ReadV18Int32(reader, bigEndian);
                uint instantCount = ReadV18UInt32(reader, bigEndian); long instantOffsetField = reader.BaseStream.Position; int instantOffset = ReadV18Int32(reader, bigEndian);
                uint wavDataSize = ReadV18UInt32(reader, bigEndian);
                long wavDataStart = ReadV18Int32(reader, bigEndian); // physical file offset, unlike GAFRO pointers

                long memoryTable = ResolveRelative(memoryOffsetField, memoryOffset);
                long streamTable = ResolveRelative(streamOffsetField, streamOffset);
                long instantTable = ResolveRelative(instantOffsetField, instantOffset);

                Dictionary<long, short> waveIndices = new Dictionary<long, short>();
                if (dataVersion == 39)
                {
                    ReadV39WaveTable(reader, filePath, memoryTable, memoryCount, WavType.Memory, wavDataStart, wavDataSize, wavesList, waveIndices, bigEndian);
                    // The v39 stream and instant-stream records have additional
                    // variable layouts which are not the v18/v21 structures.
                    // Do not expose guessed rows as valid WAV metadata; the
                    // standalone DAT8 files remain available as stream banks.
                }
                else
                {
                    ReadV18WaveTable(reader, filePath, memoryTable, memoryCount, WavType.Memory, wavDataStart, wavDataSize, wavesList, waveIndices, bigEndian, dataVersion);
                    ReadV18WaveTable(reader, filePath, streamTable, streamCount, WavType.Stream, 0, 0, wavesList, waveIndices, bigEndian, dataVersion);
                    ReadV18WaveTable(reader, filePath, instantTable, instantCount, WavType.InstantStream, wavDataStart, wavDataSize, wavesList, waveIndices, bigEndian, dataVersion);
                }

                for (int i = 0; i < sfxCount; i++)
                {
                    int sfxInfoSize = dataVersion == 39 ? 20 : 16;
                    long entry = sfxTable + i * (long)sfxInfoSize;
                    if (!CanRead(reader, entry, sfxInfoSize)) break;
                    reader.BaseStream.Position = entry;
                    uint hash = ReadV18UInt32(reader, bigEndian);
                    long parameterField = reader.BaseStream.Position; long parameter = ResolveRelative(parameterField, ReadV18Int32(reader, bigEndian));
                    long poolField = reader.BaseStream.Position; long pool = ResolveRelative(poolField, ReadV18Int32(reader, bigEndian));
                    if (dataVersion == 39) ReadV18UInt32(reader, bigEndian); // persistent runtime id
                    int elementCount = reader.ReadByte();
                    byte runtimeStatus = reader.ReadByte();
                    byte infoFlags = reader.ReadByte();
                    reader.ReadByte();

                    Sample sample = new Sample { HashCodeNumber = hash, IsV18 = true, V18RuntimeStatus = runtimeStatus, V18InfoFlags = infoFlags, V18ParameterAddress = parameter, V18PoolAddress = pool, V18ElementCount = (byte)elementCount };
                    int parameterSize = dataVersion >= 21 ? 36 : 32;
                    if (CanRead(reader, parameter, parameterSize))
                    {
                        reader.BaseStream.Position = parameter;
                        uint flags = ReadV18UInt32(reader, bigEndian);
                        sample.V18Flags = flags;
                        sample.Flags = (ushort)(flags & 0xffff);
                        sample.V18AttackTime = ReadV18UInt16(reader, bigEndian);
                        sample.V18ReleaseTime = ReadV18UInt16(reader, bigEndian);
                        sample.V18MixGroup = ReadV18UInt16(reader, bigEndian);
                        sample.V18Ducker = ReadV18UInt16(reader, bigEndian);
                        sample.V18CullingGroup = ReadV18UInt16(reader, bigEndian);
                        sample.V18Oscillator = ReadV18UInt16(reader, bigEndian);
                        if (dataVersion >= 21) sample.V18Controller = ReadV18UInt16(reader, bigEndian);
                        sample.V18DuckerOffset = ReadV18Int16(reader, bigEndian);
                        sample.V18ReverbSend = reader.ReadByte();
                        sample.V18MultiTapSend = reader.ReadByte();
                        sample.V18PingPongSend = reader.ReadByte();
                        sample.V18LowPass = reader.ReadByte();
                        if (dataVersion >= 21) { reader.ReadByte(); reader.ReadByte(); } // high-pass and amplitude modulation
                        sample.V18VolumeRolloff = reader.ReadSByte();
                        sample.V18MaxItems = reader.ReadByte();
                        sample.V18Priority = reader.ReadByte();
                        sample.V18MasterVolume = reader.ReadByte();
                        sample.V18PlayAndCull = reader.ReadByte();
                        sample.V18Doppler = reader.ReadByte();
                        sample.V18TriggerChance = reader.ReadByte();
                        sample.V18ChorusSend = reader.ReadByte();
                        if (dataVersion >= 21) { reader.ReadByte(); reader.ReadByte(); } // noise level and padding
                        else sample.V18Controller = ReadV18UInt16(reader, bigEndian);

                        // Legacy projection used by list columns and older auxiliary forms.
                        sample.GroupHashCode = unchecked((short)sample.V18MixGroup);
                        sample.DuckerLenght = sample.V18DuckerOffset;
                        sample.ReverbSend = unchecked((sbyte)sample.V18ReverbSend);
                        sample.MaxVoices = unchecked((sbyte)sample.V18MaxItems);
                        sample.Priority = unchecked((sbyte)sample.V18Priority);
                        sample.MasterVolume = unchecked((sbyte)sample.V18MasterVolume);
                        sample.DopplerValue = unchecked((sbyte)sample.V18Doppler);
                    }

                    for (int elementIndex = 0; elementIndex < elementCount; elementIndex++)
                    {
                        long poolEntry = pool + elementIndex * 8L;
                        if (!CanRead(reader, poolEntry, 8)) break;
                        reader.BaseStream.Position = poolEntry;
                        long elementField = reader.BaseStream.Position;
                        long element = ResolveRelative(elementField, ReadV18Int32(reader, bigEndian) & ~1);
                        long sampleField = reader.BaseStream.Position;
                        long referencedWave = ResolveRelative(sampleField, ReadV18Int32(reader, bigEndian) & ~1);
                        short waveIndex;
                        if (!waveIndices.TryGetValue(referencedWave, out waveIndex)) waveIndex = -1;
                        SampleInfo item = new SampleInfo { FileRef = waveIndex };
                        if (CanRead(reader, element, 16))
                        {
                            reader.BaseStream.Position = element;
                            short preMin = ReadV18Int16(reader, bigEndian);
                            short preMax = ReadV18Int16(reader, bigEndian);
                            sample.MinDelay = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, preMin * 10));
                            sample.MaxDelay = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, preMax * 10));
                            reader.BaseStream.Position += 4;
                            reader.ReadByte(); reader.ReadByte();
                            item.Pitch = reader.ReadSByte() * 0.2f;
                            item.PitchOffset = reader.ReadByte() * 0.1f;
                            int usedChannels = reader.ReadByte();
                            reader.ReadByte(); reader.ReadBytes(2);
                            if (usedChannels > 0 && CanRead(reader, reader.BaseStream.Position, 4))
                            {
                                item.Volume = reader.ReadSByte();
                                item.VolumeOffset = reader.ReadSByte();
                                byte angle = reader.ReadByte();
                                item.Pan = angle + (angle >> 1);
                                item.PanOffset = reader.ReadByte();
                            }
                        }
                        sample.samplesList.Add(item);
                    }

                    if (samplesDictionary.ContainsKey(hash)) duplicatedHashCodes.Add(hash);
                    else samplesDictionary.Add(hash, sample);
                }
            }
        }

        private static void ReadV18WaveTable(BinaryReader reader, string filePath, long table, uint count, WavType wavType, long wavDataStart, uint wavDataSize, List<SampleData> waves, Dictionary<long, short> indices, bool bigEndian, uint dataVersion)
        {
            int entrySize = wavType == WavType.Stream ? 24 : wavType == WavType.InstantStream ? 32 : 28;
            for (int i = 0; i < count; i++)
            {
                long entry = table + i * (long)entrySize;
                if (!CanRead(reader, entry, entrySize)) break;
                reader.BaseStream.Position = entry;
                uint wavHash = ReadV18UInt32(reader, bigEndian);
                uint sampleCount = ReadV18UInt32(reader, bigEndian);
                ReadV18Int32(reader, bigEndian);
                ushort frequency = ReadV18UInt16(reader, bigEndian);
                byte flags = reader.ReadByte();
                byte channels = reader.ReadByte();
                WavType encodedWavType = (WavType)((flags >> 4) & 3);
                if (encodedWavType != wavType)
                    throw new InvalidDataException(string.Format("SBNK WAV 0x{0:X8} has WavType {1}, but is stored in the {2} table.", wavHash, (int)encodedWavType, wavType));
                uint dataOffset;
                uint dataSize;
                uint loopOffset;
                if (wavType == WavType.Memory)
                {
                    dataOffset = (uint)Math.Max(0, wavDataStart + ReadV18Int32(reader, bigEndian));
                    dataSize = ReadV18UInt32(reader, bigEndian);
                    loopOffset = ReadV18UInt32(reader, bigEndian);
                }
                else if (wavType == WavType.Stream)
                {
                    uint fileEnd = ReadV18UInt32(reader, bigEndian);
                    loopOffset = ReadV18UInt32(reader, bigEndian);
                    dataOffset = 0x800;
                    dataSize = fileEnd > 0x800 ? fileEnd - 0x800 : 0;
                }
                else
                {
                    uint fileEnd = ReadV18UInt32(reader, bigEndian);
                    loopOffset = ReadV18UInt32(reader, bigEndian);
                    dataOffset = (uint)Math.Max(0, wavDataStart + ReadV18Int32(reader, bigEndian));
                    dataSize = ReadV18UInt32(reader, bigEndian);
                    if (dataSize == 0 && fileEnd > dataOffset) dataSize = fileEnd - dataOffset;
                }

                EuroSoundAudioCodec codec = CodecFromV18Value(flags & 7, dataVersion);
                string audioPath = wavType == WavType.Memory ? filePath : FindV18StreamFile(filePath, wavHash);
                uint referenceOffset = dataOffset;
                uint referenceSize = dataSize;
                uint loopStartSample = loopOffset;
                uint loopEndByteOffset = 0;
                if (wavType != WavType.Memory && !string.IsNullOrEmpty(audioPath))
                {
                    referenceOffset = 0x800;
                    long available = Math.Max(0, new FileInfo(audioPath).Length - referenceOffset);
                    // Shipped v18 files use the physical stream file as authority. Some publishers
                    // leave a source-WAV-derived end offset in SBNK, which must not include padding
                    // or run beyond the individual MUSX stream.
                    referenceSize = (uint)Math.Min(uint.MaxValue, available);
                    try
                    {
                        StreambankHeader streamHeader = new StreamBankReader().ReadStreamBankHeader(audioPath, string.Empty);
                        if (streamHeader.FileVersion == 18 || streamHeader.FileVersion == 21)
                        {
                            if (streamHeader.FileLength2 <= referenceSize) referenceSize = streamHeader.FileLength2;
                            if (streamHeader.LoopStartSample != uint.MaxValue) loopStartSample = streamHeader.LoopStartSample;
                            loopEndByteOffset = streamHeader.LoopEndByteOffset;
                        }
                    }
                    catch (InvalidDataException) { }
                }
                if (wavType == WavType.Stream)
                {
                    dataOffset = referenceOffset;
                    dataSize = referenceSize;
                }
                if ((flags & 7) == 3 && !string.IsNullOrEmpty(audioPath))
                    codec = DetectDspContainerCodec(audioPath, referenceOffset, codec);
                AudioDataReference audioReference = string.IsNullOrEmpty(audioPath) ? null : new AudioDataReference { FilePath = audioPath, Offset = referenceOffset, Size = referenceSize, Codec = codec, Frequency = frequency, Channels = channels };
                SampleData wave = new SampleData
                {
                    WavHashCode = wavHash,
                    StorageType = wavType,
                    Flags = (uint)(((flags >> 3) & 1) != 0 ? 1 : 0),
                    Address = dataOffset,
                    SampleSize = dataSize,
                    MemorySize = dataSize,
                    Frequency = frequency,
                    Channels = channels,
                    TotalSamples = sampleCount,
                    LoopStartOffset = wavType != WavType.Memory ? loopStartSample : V18SfxLoopOffsetToSamples(codec, loopOffset, channels),
                    // EngineXT writes 0xFFFFFFFF when the WAV does not loop. Keep that
                    // implementation sentinel out of Wav Header Data and expose 0 instead.
                    OriginalLoopOffset = ((flags >> 3) & 1) != 0 && loopOffset != uint.MaxValue ? loopOffset : 0,
                    LoopStartSample = wavType != WavType.Memory ? loopStartSample : V18SfxLoopOffsetToSamples(codec, loopOffset, channels),
                    LoopEndByteOffset = loopEndByteOffset,
                    AudioReference = audioReference
                };
                indices[entry] = (short)waves.Count;
                waves.Add(wave);
            }
        }

        private static void ReadV39WaveTable(BinaryReader reader, string filePath, long table, uint count, WavType wavType, long wavDataStart, uint wavDataSize, List<SampleData> waves, Dictionary<long, short> indices, bool bigEndian)
        {
            long entry = table;
            for (uint index = 0; index < count; index++)
            {
                long recordStart = entry;
                int minimumSize = 16;
                if (!CanRead(reader, entry, minimumSize)) break;
                reader.BaseStream.Position = entry;
                uint wavHash = ReadV18UInt32(reader, bigEndian);
                uint sampleCount = ReadV18UInt32(reader, bigEndian);
                ushort frequency = ReadV18UInt16(reader, bigEndian);
                byte flags = reader.ReadByte();
                byte channelFlags = reader.ReadByte();
                int channels = Math.Max(1, channelFlags & 7);
                bool looped = (channelFlags & 8) != 0;
                uint dataOffset = 0;
                uint dataSize;
                uint loopStart = uint.MaxValue;

                if (wavType == WavType.Memory)
                {
                    bool extended = (flags & 7) == 6;
                    if (extended)
                    {
                        if (!CanRead(reader, recordStart, 24)) break;
                        loopStart = ReadV18UInt32(reader, bigEndian);
                        ReadV18UInt32(reader, bigEndian); // encoded loop/end value
                    }
                    long dataInfoField = reader.BaseStream.Position;
                    long dataInfo = ResolveRelative(dataInfoField, ReadV18Int32(reader, bigEndian));
                    entry += extended ? 24 : 16;
                    if (CanRead(reader, dataInfo, 8))
                    {
                        reader.BaseStream.Position = dataInfo;
                        dataOffset = (uint)Math.Max(0, wavDataStart + ReadV18UInt32(reader, bigEndian));
                        dataSize = ReadV18UInt32(reader, bigEndian);
                    }
                    else
                    {
                        dataOffset = (uint)Math.Max(0, wavDataStart);
                        dataSize = 0;
                    }
                }
                else if (wavType == WavType.Stream)
                {
                    dataSize = ReadV18UInt32(reader, bigEndian);
                    entry += 16;
                    if (looped && CanRead(reader, entry, 8))
                    {
                        reader.BaseStream.Position = entry;
                        loopStart = ReadV18UInt32(reader, bigEndian);
                        ReadV18UInt32(reader, bigEndian); // encoded loop/end value
                        entry += 8;
                    }
                }
                else
                {
                    if (!CanRead(reader, recordStart, 28)) break;
                    ReadV18UInt32(reader, bigEndian); // physical/end value
                    loopStart = ReadV18UInt32(reader, bigEndian);
                    dataOffset = (uint)Math.Max(0, wavDataStart + ReadV18Int32(reader, bigEndian));
                    dataSize = ReadV18UInt32(reader, bigEndian);
                    entry += 28;
                }

                EuroSoundAudioCodec codec = CodecFromV18Value(flags & 7, 39);
                if (wavType != WavType.Stream)
                {
                    // Disney's Xenon v39 uses value 6 for its big-endian
                    // Eurocom IMA blocks, not for XMA as v18/v21 do.
                    codec = LooksLikeEurocomIma(filePath, dataOffset, dataSize)
                        ? EuroSoundAudioCodec.EurocomImaAdpcm
                        : codec;
                }
                string audioPath = wavType == WavType.Stream ? FindV18StreamFile(filePath, wavHash) : filePath;
                uint referenceOffset = wavType == WavType.Stream ? 0x800u : dataOffset;
                uint referenceSize = dataSize;
                if (wavType == WavType.Stream && !string.IsNullOrEmpty(audioPath))
                {
                    long available = Math.Max(0, new FileInfo(audioPath).Length - referenceOffset);
                    referenceSize = (uint)Math.Min(uint.MaxValue, available);
                }

                SampleData wave = new SampleData
                {
                    WavHashCode = wavHash,
                    StorageType = wavType,
                    Flags = looped ? 1u : 0u,
                    Address = referenceOffset,
                    SampleSize = referenceSize,
                    MemorySize = referenceSize,
                    Frequency = frequency,
                    Channels = (uint)channels,
                    TotalSamples = sampleCount,
                    Duration = frequency == 0 ? 0 : (uint)Math.Min(uint.MaxValue, sampleCount * 1000UL / frequency),
                    OriginalLoopOffset = looped && loopStart != uint.MaxValue ? loopStart : 0,
                    LoopStartSample = looped && loopStart != uint.MaxValue ? loopStart : 0,
                    LoopStartOffset = looped && loopStart != uint.MaxValue ? loopStart : 0,
                    AudioReference = string.IsNullOrEmpty(audioPath) ? null : new AudioDataReference { FilePath = audioPath, Offset = referenceOffset, Size = referenceSize, Codec = codec, Frequency = frequency, Channels = channels }
                };
                // Pool references point at the beginning of the WAV record.
                indices[recordStart] = (short)waves.Count;
                waves.Add(wave);
            }
        }

        private static uint V18SfxLoopOffsetToSamples(EuroSoundAudioCodec codec, uint offset, int channels)
        {
            if (offset == uint.MaxValue) return 0;
            channels = Math.Max(1, channels);
            switch (codec)
            {
                case EuroSoundAudioCodec.EurocomImaAdpcm:
                    return CalculusLoopOffsets.EurocomImaToSamples(checked(offset * 2), channels);
                case EuroSoundAudioCodec.SonyVagAdpcm:
                    return CalculusLoopOffsets.SonyVagToSamples(offset, channels);
                case EuroSoundAudioCodec.DspAdpcm:
                case EuroSoundAudioCodec.DspAdpcmLegacy:
                case EuroSoundAudioCodec.DspAdpcmNgca:
                    return CalculusLoopOffsets.DspAdpcmToSamples(offset, channels);
                case EuroSoundAudioCodec.Pcm16:
                    return CalculusLoopOffsets.Pcm16BytesToSamples(offset, channels);
                default:
                    return 0;
            }
        }

        private static bool LooksLikeEurocomIma(string filePath, uint offset, uint size)
        {
            if (size == 0 || (size & 31) != 0) return false;
            try
            {
                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    if (offset > stream.Length || size > stream.Length - offset) return false;
                    stream.Position = offset;
                    int blocks = (int)Math.Min(64u, size / 32u);
                    byte[] header = new byte[4];
                    for (int block = 0; block < blocks; block++)
                    {
                        if (stream.Read(header, 0, header.Length) != header.Length)
                            return false;
                        bool guard = header[0] == 0xAB && header[1] == 0xAB && header[2] == 0xAB && header[3] == 0xAB;
                        // Byte 2 is the IMA step index. Xenon v39 uses byte 3
                        // for block state, whereas older little-endian banks
                        // normally leave it at zero. Allocator guard blocks are
                        // removed by the decoder and do not disqualify the WAV.
                        if (!guard && header[2] > 88) return false;
                        stream.Position += 28;
                    }
                    return blocks != 0;
                }
            }
            catch (IOException) { return false; }
        }

        private static EuroSoundAudioCodec CodecFromV18Value(int value, uint dataVersion)
        {
            switch (value) { case 1: return EuroSoundAudioCodec.EurocomImaAdpcm; case 2: return EuroSoundAudioCodec.SonyVagAdpcm; case 3: return dataVersion >= 21 ? EuroSoundAudioCodec.DspAdpcmNgca : EuroSoundAudioCodec.DspAdpcmLegacy; case 4: return EuroSoundAudioCodec.Pcm16; case 5: return EuroSoundAudioCodec.Vorbis; case 6: return EuroSoundAudioCodec.Xma; default: return EuroSoundAudioCodec.Unknown; }
        }

        private static EuroSoundAudioCodec DetectDspContainerCodec(string filePath, uint offset, EuroSoundAudioCodec fallback)
        {
            try
            {
                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    if (offset > stream.Length - 4) return fallback;
                    stream.Position = offset;
                    return stream.ReadByte() == 'N' && stream.ReadByte() == 'G' && stream.ReadByte() == 'C' && stream.ReadByte() == 'A'
                        ? EuroSoundAudioCodec.DspAdpcmNgca
                        : EuroSoundAudioCodec.DspAdpcmLegacy;
                }
            }
            catch (IOException) { return fallback; }
        }

        private static string FindV18StreamFile(string soundbankPath, uint wavHash)
        {
            string[] tokens =
            {
                wavHash.ToString("X8"),
                ((wavHash & 0xff000000u) | 0x00400000u | (wavHash & 0x000fffffu)).ToString("X8")
            };
            DirectoryInfo directory = new FileInfo(soundbankPath).Directory;
            for (int level = 0; directory != null && level < 3; level++, directory = directory.Parent)
            {
                for (int tokenIndex = 0; tokenIndex < tokens.Length; tokenIndex++)
                {
                    string[] direct = Directory.GetFiles(directory.FullName, "*" + tokens[tokenIndex] + "*.sfx", SearchOption.TopDirectoryOnly);
                    for (int i = 0; i < direct.Length; i++) if (direct[i].IndexOf("_STR_", StringComparison.OrdinalIgnoreCase) >= 0) return direct[i];
                    if (level == 1)
                    {
                        string[] recursive = Directory.GetFiles(directory.FullName, "*" + tokens[tokenIndex] + "*.sfx", SearchOption.AllDirectories);
                        for (int i = 0; i < recursive.Length; i++) if (recursive[i].IndexOf("_STR_", StringComparison.OrdinalIgnoreCase) >= 0) return recursive[i];
                    }
                }
            }
            return null;
        }

        private static long ResolveRelative(long fieldAddress, int relative) { return fieldAddress + relative; }
        private static bool CanRead(BinaryReader reader, long offset, int size) { return offset >= 0 && size >= 0 && offset <= reader.BaseStream.Length - size; }
        private static uint ReadV18UInt32(BinaryReader reader, bool bigEndian) { return BytesFunctions.FlipData(reader.ReadUInt32(), bigEndian); }
        private static int ReadV18Int32(BinaryReader reader, bool bigEndian) { return BytesFunctions.FlipData(reader.ReadInt32(), bigEndian); }
        private static ushort ReadV18UInt16(BinaryReader reader, bool bigEndian) { return BytesFunctions.FlipData(reader.ReadUInt16(), bigEndian); }
        private static short ReadV18Int16(BinaryReader reader, bool bigEndian) { return BytesFunctions.FlipData(reader.ReadInt16(), bigEndian); }

        //-------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------
        internal void ReadSoundbank(string filePath, SoundbankHeader headerData, SortedDictionary<uint, Sample> samplesDictionary, List<SampleData> wavesList, List<uint> duplicatedHashCodes)
        {
            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                //Read SFX Start
                BReader.BaseStream.Seek(headerData.SFXStart, SeekOrigin.Begin);
                uint sfxCount = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);

                for (int i = 0; i < sfxCount; i++)
                {
                    uint hashcode;
                    switch (headerData.FileVersion)
                    {
                        case 201:
                            hashcode = 0x1A000000 | BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                            break;

                        case 6:
                            hashcode = 0x2D700000 | BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                            break;

                        default:
                            hashcode = 0x1AF00000 | BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                            break;
                    }

                    uint curSfxPos = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                    long prevPos = BReader.BaseStream.Position;

                    //Goto SFX Data
                    BReader.BaseStream.Seek(headerData.SFXStart + curSfxPos, SeekOrigin.Begin);

                    //Read SFX data
                    Sample sample = new Sample
                    {
                        HashCodeNumber = hashcode,
                        DuckerLenght = BytesFunctions.FlipData(BReader.ReadInt16(), headerData.IsBigEndian),
                        MinDelay = BytesFunctions.FlipData(BReader.ReadInt16(), headerData.IsBigEndian),
                        MaxDelay = BytesFunctions.FlipData(BReader.ReadInt16(), headerData.IsBigEndian),
                        ReverbSend = BReader.ReadSByte(),
                        TrackingType = BReader.ReadByte(),
                        MaxVoices = BReader.ReadSByte(),
                        Priority = BReader.ReadSByte(),
                        Ducker = BReader.ReadSByte(),
                        MasterVolume = BReader.ReadSByte()
                    };

                    //Read flags and sample pool
                    if (headerData.FileVersion == 4)
                    {
                        // Version 4 stores this compact block as four bytes in a
                        // fixed order on every platform. It is not endian-sensitive.
                        sample.GroupHashCode = BReader.ReadByte();
                        sample.GroupMaxChannels = (sbyte)(BReader.ReadByte() >> 4);
                        byte flagsLow = BReader.ReadByte();
                        byte flagsHigh = BReader.ReadByte();
                        sample.Flags = (ushort)(flagsLow | (flagsHigh << 8));
                    }
                    else if (headerData.Platform.Contains("PS2"))
                    {
                        short groupHashCode = (short)BReader.ReadUInt16();
                        sample.GroupHashCode = (short)((groupHashCode & 0xFFF0) >> 4);
                        sample.GroupMaxChannels = (sbyte)(groupHashCode & 0xF);

                        //Read Flags
                        sample.Flags = BReader.ReadUInt16();

                        //Read UserFlags
                        if (headerData.FileVersion > 4)
                        {
                            sample.UserFlags = BytesFunctions.FlipData(BReader.ReadUInt16(), headerData.IsBigEndian);
                            sample.DopplerValue = BReader.ReadSByte();
                            sample.UserValue = BReader.ReadSByte();
                        }
                    }
                    else
                    {
                        sample.GroupHashCode = (short)BytesFunctions.FlipData(BReader.ReadUInt16(), headerData.IsBigEndian);
                        sample.GroupMaxChannels = BReader.ReadSByte();
                        BReader.ReadSByte();

                        //Read Flags
                        for (int j = 0; j < 16; j++)
                        {
                            sbyte flagState = BReader.ReadSByte();

                            if (flagState == 1)
                            {
                                sample.Flags = (ushort)(sample.Flags | (flagState << j));
                            }
                        }

                        //Read User Flags
                        if (headerData.FileVersion > 4)
                        {
                            for (int j = 0; j < 16; j++)
                            {
                                sbyte flagState = BReader.ReadSByte();

                                if (flagState == 1)
                                {
                                    sample.UserFlags = (ushort)(sample.UserFlags | (flagState << j));
                                }
                            }

                            sample.DopplerValue = BReader.ReadSByte();
                            sample.UserValue = BReader.ReadSByte();
                        }
                    }

                    if (headerData.FileVersion > 5)
                    {
                        sample.SFXDucker = BReader.ReadSByte();
                        sample.Spare = BReader.ReadSByte();
                    }

                    //Read Sample Pool
                    ushort samplesCount = BytesFunctions.FlipData(BReader.ReadUInt16(), headerData.IsBigEndian);

                    for (int j = 0; j < samplesCount; j++)
                    {
                        SampleInfo samplePoolItem = new SampleInfo
                        {
                            FileRef = BytesFunctions.FlipData(BReader.ReadInt16(), headerData.IsBigEndian),
                            Pitch = BReader.ReadSByte() * 0.2f,
                            PitchOffset = BReader.ReadSByte() * 0.1f,
                            Volume = BReader.ReadSByte(),
                            VolumeOffset = BReader.ReadSByte(),
                            Pan = BReader.ReadSByte(),
                            PanOffset = BReader.ReadSByte()
                        };

                        sample.samplesList.Add(samplePoolItem);
                    }

                    //Save in dictionary
                    if (samplesDictionary.ContainsKey(hashcode))
                    {
                        duplicatedHashCodes.Add(hashcode);
                    }
                    else
                    {
                        samplesDictionary.Add(hashcode, sample);
                    }

                    //Read data to show in the Hex viewer
                    BReader.BaseStream.Seek(curSfxPos + headerData.SFXStart, SeekOrigin.Begin);

                    //Return to previous position
                    BReader.BaseStream.Seek(prevPos, SeekOrigin.Begin);
                }

                //Read Sample info
                BReader.BaseStream.Seek(headerData.SampleInfoStart, SeekOrigin.Begin);
                uint waveCount = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);

                for (int i = 0; i < waveCount; i++)
                {
                    SampleData wavHeaderData = new SampleData
                    {
                        Flags = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        Address = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        MemorySize = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        Frequency = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        SampleSize = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        PsiSampleHeader = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        LoopStartOffset = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        Duration = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian)
                    };

                    wavHeaderData.OriginalLoopOffset = wavHeaderData.LoopStartOffset;

                    EuroSoundAudioCodec codec = EuroSoundCodecMatrix.GetCodec(headerData.FileVersion, headerData.Platform, EuroSoundBankType.SoundBank);
                    wavHeaderData.TotalSamples = EuroSoundCodecMatrix.EncodedByteCountToSamples(codec, wavHeaderData.SampleSize, 1);
                    wavHeaderData.LoopStartOffset = EuroSoundCodecMatrix.SoundBankLoopOffsetToSamples(codec, wavHeaderData.LoopStartOffset, 1);
                    wavHeaderData.LoopStartSample = wavHeaderData.LoopStartOffset;
                    wavHeaderData.AudioReference = new AudioDataReference
                    {
                        FilePath = filePath,
                        Offset = headerData.SampleDataStart + wavHeaderData.Address,
                        Size = wavHeaderData.SampleSize,
                        Codec = codec,
                        Frequency = wavHeaderData.Frequency,
                        Channels = 1
                    };

                    if (!wavHeaderData.IsLooped)
                    {
                        wavHeaderData.LoopStartOffset = 0;
                        wavHeaderData.LoopStartSample = 0;
                    }

                    //Store current position
                    long prevPos = BReader.BaseStream.Position;

                    //Read coefficients
                    if (headerData.SpecialSampleInfoLength > 0)
                    {
                        BReader.BaseStream.Seek(
                            headerData.SpecialSampleInfoStart + wavHeaderData.PsiSampleHeader,
                            SeekOrigin.Begin);

                        BReader.BaseStream.Seek(28, SeekOrigin.Current);

                        wavHeaderData.DspCoeffs = new short[16];

                        for (int j = 0; j < wavHeaderData.DspCoeffs.Length; j++)
                        {
                            wavHeaderData.DspCoeffs[j] =
                                BytesFunctions.FlipData(BReader.ReadInt16(), headerData.IsBigEndian);
                        }
                    }

                    //Store data
                    wavesList.Add(wavHeaderData);

                    //Return to previous position
                    BReader.BaseStream.Seek(prevPos, SeekOrigin.Begin);
                }
            }
        }

    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
