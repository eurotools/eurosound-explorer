using MusX.Objects;
using System;
using System.Collections.Generic;
using System.IO;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    internal class SoundBankReaderNew : SoundBankReader
    {
        internal static void ReadSoundbankV18(string filePath, SoundbankHeader headerData, SortedDictionary<uint, Sample> samplesDictionary, List<SampleData> wavesList, List<uint> duplicatedHashCodes)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                reader.BaseStream.Position = 0x800;
                if (new string(reader.ReadChars(4)) != "SBNK") throw new InvalidDataException("Invalid EngineXT SBNK descriptor.");
                bool bigEndian = headerData.IsBigEndian;
                uint dataVersion = ReadV18UInt32(reader, bigEndian);
                if (dataVersion != 18 && dataVersion != 21) throw new InvalidDataException("The EngineXT reader currently supports SBNK data versions 18 and 21.");
                ReadV18UInt32(reader, bigEndian); // language
                ReadV18UInt32(reader, bigEndian); // soundbank hash
                if (dataVersion >= 21) ReadV18UInt32(reader, bigEndian); // memory-slot hash added in v21

                uint sfxCount = ReadV18UInt32(reader, bigEndian); long sfxOffsetField = reader.BaseStream.Position; long sfxTable = ResolveRelative(sfxOffsetField, ReadV18Int32(reader, bigEndian));
                ReadV18UInt32(reader, bigEndian); ReadV18Int32(reader, bigEndian);
                ReadV18UInt32(reader, bigEndian); ReadV18Int32(reader, bigEndian);
                ReadV18UInt32(reader, bigEndian); ReadV18Int32(reader, bigEndian);
                uint memoryCount = ReadV18UInt32(reader, bigEndian); long memoryOffsetField = reader.BaseStream.Position; long memoryTable = ResolveRelative(memoryOffsetField, ReadV18Int32(reader, bigEndian));
                uint streamCount = ReadV18UInt32(reader, bigEndian); long streamOffsetField = reader.BaseStream.Position; long streamTable = ResolveRelative(streamOffsetField, ReadV18Int32(reader, bigEndian));
                uint instantCount = ReadV18UInt32(reader, bigEndian); long instantOffsetField = reader.BaseStream.Position; long instantTable = ResolveRelative(instantOffsetField, ReadV18Int32(reader, bigEndian));
                uint wavDataSize = ReadV18UInt32(reader, bigEndian);
                long wavDataStart = ReadV18Int32(reader, bigEndian); // physical file offset, unlike GAFRO pointers

                Dictionary<long, short> waveIndices = new Dictionary<long, short>();
                ReadV18WaveTable(reader, filePath, memoryTable, memoryCount, WavType.Memory, wavDataStart, wavDataSize, wavesList, waveIndices, bigEndian, dataVersion);
                ReadV18WaveTable(reader, filePath, streamTable, streamCount, WavType.Stream, 0, 0, wavesList, waveIndices, bigEndian, dataVersion);
                ReadV18WaveTable(reader, filePath, instantTable, instantCount, WavType.InstantStream, wavDataStart, wavDataSize, wavesList, waveIndices, bigEndian, dataVersion);

                for (int i = 0; i < sfxCount; i++)
                {
                    const int SfxInfoSizeV18 = 16;
                    long entry = sfxTable + i * (long)SfxInfoSizeV18;
                    if (!CanRead(reader, entry, SfxInfoSizeV18)) break;
                    reader.BaseStream.Position = entry;
                    uint hash = ReadV18UInt32(reader, bigEndian);
                    long parameterField = reader.BaseStream.Position; long parameter = ResolveRelative(parameterField, ReadV18Int32(reader, bigEndian));
                    long poolField = reader.BaseStream.Position; long pool = ResolveRelative(poolField, ReadV18Int32(reader, bigEndian));
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

        private static EuroSoundAudioCodec CodecFromV18Value(int value, uint dataVersion)
        {
            switch (value) { case 1: return EuroSoundAudioCodec.EurocomImaAdpcm; case 2: return EuroSoundAudioCodec.SonyVagAdpcm; case 3: return dataVersion >= 21 ? EuroSoundAudioCodec.DspAdpcmNgca : EuroSoundAudioCodec.DspAdpcmLegacy; case 4: return EuroSoundAudioCodec.Pcm16; case 6: return EuroSoundAudioCodec.Xma; default: return EuroSoundAudioCodec.Unknown; }
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
                    if (headerData.FileVersion == 4 &&
                        !headerData.Platform.Contains("PS2") &&
                        !headerData.Platform.Contains("XB") &&
                        !EuroSoundCodecMatrix.IsGameCubePlatform(headerData.Platform))
                    {
                        sample.GroupMaxChannels = BReader.ReadSByte();
                        sample.GroupHashCode = (short)BytesFunctions.FlipData(BReader.ReadUInt16(), headerData.IsBigEndian);
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
                    }
                    else if (headerData.Platform.Contains("PS2") ||
                        (headerData.Platform.Contains("XB") && headerData.FileVersion < 5) ||
                        (EuroSoundCodecMatrix.IsGameCubePlatform(headerData.Platform) && headerData.FileVersion < 5))
                    {
                        short groupHashCode = (short)BReader.ReadUInt16();

                        if (headerData.FileVersion == 4)
                        {
                            sample.GroupHashCode = (short)(groupHashCode & 0x0FFF);
                            sample.GroupMaxChannels = (sbyte)((groupHashCode & 0xF000) >> 12);
                        }
                        else
                        {
                            sample.GroupHashCode = (short)((groupHashCode & 0xFFF0) >> 4);
                            sample.GroupMaxChannels = (sbyte)(groupHashCode & 0xF);
                        }

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
