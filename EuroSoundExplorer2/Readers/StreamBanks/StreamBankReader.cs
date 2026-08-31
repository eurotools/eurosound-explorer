using MusX.Objects;
using System;
using System.Collections.Generic;
using System.IO;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class StreamBankReader : SfxFunctions
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public StreambankHeader ReadStreamBankHeader(string filePath, string platform)
        {
            SfxCommonHeader commonHeader = ReadCommonHeader(filePath, platform);
            StreambankHeader headerData = new StreambankHeader(commonHeader);

            if (headerData.FileVersion == 10)
            {
                using (EuroSoundBinaryReader reader = new EuroSoundBinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read), false))
                {
                    reader.Seek(0x40, SeekOrigin.Begin);
                    string descriptor = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(4));
                    if (descriptor == "DAT8")
                    {
                        uint audioSize = reader.ReadUInt32();
                        uint datCodec = reader.ReadUInt32();
                        uint frequency = reader.ReadUInt32();
                        uint channels = reader.ReadUInt32();
                        uint flags = reader.ReadUInt32();
                        headerData.LoopStartByteOffset = reader.ReadUInt32();
                        headerData.LoopEndByteOffset = reader.ReadUInt32();
                        headerData.SampleCount = reader.ReadUInt32();
                        headerData.LoopStartSample = reader.ReadUInt32();
                        headerData.FileStart1 = datCodec;
                        headerData.FileLength1 = flags;
                        headerData.CodecType = datCodec;
                        headerData.StreamFlags = flags;
                        headerData.Channels = Math.Max(1u, channels);
                        headerData.Frequency = frequency;
                        headerData.FileStart2 = 0x800;
                        headerData.FileLength2 = Math.Min(audioSize, (uint)Math.Max(0, reader.BaseStream.Length - 0x800));
                        return headerData;
                    }

                    if (descriptor == "DAT5")
                    {
                        // Spider-Man keeps the outer MusX version at 10 and the
                        // SBNK version at 21. Its standalone streams carry the
                        // v21 DAT5 descriptor, including their real rate and
                        // channel count, despite not having FileVersion 21.
                        uint audioSize = reader.ReadUInt32();
                        uint channels = reader.ReadUInt32();
                        uint frequency = reader.ReadUInt32();
                        uint streamCodec = reader.ReadUInt32();
                        uint flags = reader.ReadUInt32();
                        headerData.LoopStartByteOffset = reader.ReadUInt32();
                        headerData.LoopEndByteOffset = reader.ReadUInt32();
                        headerData.SampleCount = reader.ReadUInt32();
                        headerData.LoopStartSample = reader.ReadUInt32();
                        headerData.FileStart1 = streamCodec;
                        headerData.FileLength1 = flags;
                        headerData.CodecType = streamCodec;
                        headerData.StreamFlags = flags;
                        headerData.Channels = Math.Max(1u, channels);
                        headerData.Frequency = frequency;
                        headerData.FileStart2 = 0x800;
                        headerData.FileLength2 = Math.Min(audioSize, (uint)Math.Max(0, reader.BaseStream.Length - 0x800));
                        return headerData;
                    }

                    // G-Force keeps the MusX container version at 10, while its
                    // stream descriptor uses the earlier EngineXT v18 layout.
                    // It has no DAT8 tag: the descriptor starts directly at 0x30.
                    // Do this structural check before falling back to the older
                    // Pirates MusX 10 defaults (notably its fixed 22050 Hz rate).
                    if (TryReadEngineXtV18StreamDescriptor(reader, headerData))
                        return headerData;
                }
                EuroSoundAudioCodec codec = EuroSoundCodecMatrix.GetCodec(10, headerData.Platform, EuroSoundBankType.StreamBank);

                if (headerData.UsesAdpcm == 21)
                {
                    // Some large Spider-Man streams omit DAT5 and mirror the
                    // beginning of their encoded payload in the header sector.
                    // The common MusX header still identifies SBNK v21; leave
                    // rate/channels/sample count unresolved so the associated
                    // SBNK WAV record remains authoritative.
                    headerData.FileStart1 = GetCodecType(codec);
                    headerData.CodecType = headerData.FileStart1;
                    headerData.Channels = 1;
                    headerData.Frequency = 0;
                    headerData.FileStart2 = 0x800;
                    headerData.FileLength2 = GetMusX10AudioLength(filePath, headerData.FileStart2, codec, 1);
                    headerData.SampleCount = 0;
                    headerData.LoopStartByteOffset = uint.MaxValue;
                    headerData.LoopEndByteOffset = uint.MaxValue;
                    headerData.LoopStartSample = uint.MaxValue;
                    return headerData;
                }

                bool isMusicEffect = IsMusX10MusicEffect(headerData.FileHashCode);
                headerData.FileStart1 = GetCodecType(codec);
                headerData.CodecType = headerData.FileStart1;
                headerData.Channels = isMusicEffect ? 2u : 1u;
                headerData.Frequency = 22050;
                bool hasVagPrefix = codec == EuroSoundAudioCodec.SonyVagAdpcm && !isMusicEffect;
                headerData.FileStart2 = hasVagPrefix ? 0x810u : 0x800u;
                headerData.FileLength2 = GetMusX10AudioLength(filePath, headerData.FileStart2, codec, checked((int)headerData.Channels));
                headerData.SampleCount = EuroSoundCodecMatrix.EncodedByteCountToSamples(codec, headerData.FileLength2, checked((int)headerData.Channels));
                headerData.LoopStartByteOffset = uint.MaxValue;
                headerData.LoopEndByteOffset = uint.MaxValue;
                headerData.LoopStartSample = uint.MaxValue;
                return headerData;
            }

            if (headerData.FileVersion == 21)
            {
                // EngineXT v21 DAT5 descriptors are little-endian even on Wii.
                using (EuroSoundBinaryReader reader = new EuroSoundBinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read), false))
                {
                    reader.Seek(0x40, SeekOrigin.Begin);
                    if (System.Text.Encoding.ASCII.GetString(reader.ReadBytes(4)) != "DAT5")
                    {
                        // Some shipped v21 files have the large-file payload encrypted, so the
                        // DAT5 descriptor is not visible. Keep them browsable as one opaque stream;
                        // decoding remains unavailable until its codec metadata can be decrypted.
                        headerData.CodecType = 0;
                        headerData.StreamFlags = 0;
                        headerData.Channels = 1;
                        headerData.Frequency = 0;
                        headerData.FileStart1 = 0;
                        headerData.FileLength1 = 0;
                        headerData.FileStart2 = 0x800;
                        headerData.FileLength2 = (uint)Math.Min(uint.MaxValue, Math.Max(0, reader.BaseStream.Length - 0x800));
                        return headerData;
                    }
                    uint audioSize = reader.ReadUInt32();
                    uint channels = reader.ReadUInt32();
                    uint frequency = reader.ReadUInt32();
                    uint codec = reader.ReadUInt32();
                    uint flags = reader.ReadUInt32();
                    // DAT5 follows AudioCode's stream layout: encoded loop start,
                    // encoded end, logical sample count, exact decoded loop sample.
                    headerData.LoopStartByteOffset = reader.ReadUInt32();
                    headerData.LoopEndByteOffset = reader.ReadUInt32();
                    headerData.SampleCount = reader.ReadUInt32();
                    headerData.LoopStartSample = reader.ReadUInt32();
                    headerData.FileStart1 = codec;
                    headerData.FileLength1 = flags;
                    headerData.CodecType = codec;
                    headerData.StreamFlags = flags;
                    headerData.Channels = channels;
                    headerData.Frequency = frequency;
                    headerData.FileStart2 = 0x800;
                    headerData.FileLength2 = Math.Min(audioSize, (uint)Math.Max(0, reader.BaseStream.Length - 0x800));
                }
                return headerData;
            }

            if (headerData.FileVersion == 15 || headerData.FileVersion == 18)
            {
                // Unlike ESPD/SBNK, the v18 stream metadata remains little-endian on XE__.
                using (EuroSoundBinaryReader reader = new EuroSoundBinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read), false))
                {
                    reader.Seek(0x30, SeekOrigin.Begin);
                    uint codec = reader.ReadUInt32();
                    uint flags = reader.ReadUInt32();
                    headerData.CodecType = codec;
                    headerData.StreamFlags = flags;
                    headerData.LoopStartByteOffset = reader.ReadUInt32();
                    headerData.LoopEndByteOffset = reader.ReadUInt32();
                    headerData.SampleCount = reader.ReadUInt32();
                    headerData.LoopStartSample = reader.ReadUInt32();
                    uint audioSize = reader.ReadUInt32();
                    uint loopStartCopy = reader.ReadUInt32();
                    if (headerData.LoopStartByteOffset == uint.MaxValue) headerData.LoopStartByteOffset = loopStartCopy;
                    headerData.FileStart1 = codec;
                    headerData.FileLength1 = flags;
                    headerData.FileStart2 = 0x800;
                    headerData.FileLength2 = Math.Min(audioSize, (uint)Math.Max(0, reader.BaseStream.Length - 0x800));
                }
                return headerData;
            }

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
        private static bool TryReadEngineXtV18StreamDescriptor(EuroSoundBinaryReader reader, StreambankHeader headerData)
        {
            const uint AudioStart = 0x800;
            if (reader.BaseStream.Length < AudioStart || reader.BaseStream.Length < 0x50)
                return false;

            reader.Seek(0x30, SeekOrigin.Begin);
            uint codec = reader.ReadUInt32();
            uint flags = reader.ReadUInt32();
            uint loopStart = reader.ReadUInt32();
            uint loopEnd = reader.ReadUInt32();
            uint sampleCount = reader.ReadUInt32();
            uint loopStartSample = reader.ReadUInt32();
            uint audioSize = reader.ReadUInt32();
            uint loopStartCopy = reader.ReadUInt32();

            long available = reader.BaseStream.Length - AudioStart;
            int alignment = codec == 6 ? 0x800 : codec == 1 ? 32 : codec == 4 ? 2 : 16;
            bool loopStartValid = loopStart == uint.MaxValue || loopStart <= audioSize;
            bool loopEndValid = loopEnd == uint.MaxValue || loopEnd <= audioSize;
            bool copyValid = loopStartCopy == uint.MaxValue || loopStartCopy <= audioSize;
            if (codec < 1 || codec > 6 || audioSize == 0 || audioSize > available ||
                sampleCount == 0 || (audioSize % alignment) != 0 ||
                !loopStartValid || !loopEndValid || !copyValid)
                return false;

            headerData.CodecType = codec;
            headerData.StreamFlags = flags;
            headerData.LoopStartByteOffset = loopStart == uint.MaxValue ? loopStartCopy : loopStart;
            headerData.LoopEndByteOffset = loopEnd;
            headerData.SampleCount = sampleCount;
            headerData.LoopStartSample = loopStartSample;
            headerData.FileStart1 = codec;
            headerData.FileLength1 = flags;
            headerData.FileStart2 = AudioStart;
            headerData.FileLength2 = audioSize;
            // Frequency and channel count live in the associated SBNK v18 WAV
            // record and are resolved by StreamBankReaderNew.
            headerData.Frequency = 0;
            headerData.Channels = 1;
            return true;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public void ReadStreamBank(string filePath, StreambankHeader headerData, List<StreamSample> streamedSamples)
        {
            if (headerData.FileVersion == 10)
            {
                StreamBankReaderNew.ReadStreamFileV18(filePath, headerData, streamedSamples);
                return;
            }
            if (headerData.FileVersion == 15 || headerData.FileVersion == 18 || headerData.FileVersion == 21)
            {
                StreamBankReaderNew.ReadStreamFileV18(filePath, headerData, streamedSamples);
                return;
            }
            if (headerData.FileVersion == 201 || headerData.FileVersion == 1)
            {
                StreamBankReaderOld oldReader = new StreamBankReaderOld();
                oldReader.ReadStreamFile(filePath, headerData, streamedSamples);
            }
            else
            {
                StreamBankReaderNew newReader = new StreamBankReaderNew();
                newReader.ReadStreamFile(filePath, headerData, streamedSamples);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal static uint GetCodecType(EuroSoundAudioCodec codec)
        {
            switch (codec)
            {
                case EuroSoundAudioCodec.EurocomImaAdpcm: return 1;
                case EuroSoundAudioCodec.SonyVagAdpcm: return 2;
                case EuroSoundAudioCodec.DspAdpcmLegacy:
                case EuroSoundAudioCodec.DspAdpcmNgca: return 3;
                case EuroSoundAudioCodec.Pcm16: return 4;
                case EuroSoundAudioCodec.Vorbis: return 5;
                case EuroSoundAudioCodec.Xma: return 6;
                default: return 0;
            }
        }

        private static bool IsMusX10MusicEffect(uint hashCode)
        {
            // EngineX compares hashes without the platform-specific top byte.
            // Section 6 contains the stereo _mus_mfx assets.
            return (hashCode & 0x00F00000u) == 0x00600000u;
        }

        internal static uint GetMusX10AudioLength(string filePath, uint audioOffset, EuroSoundAudioCodec codec, int channels)
        {
            int blockSize = codec == EuroSoundAudioCodec.EurocomImaAdpcm ? 32 : 16;
            int blockSetSize = blockSize * Math.Max(1, channels);
            using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                long available = Math.Max(0, stream.Length - audioOffset);
                int tailLength = (int)Math.Min(0x1000, available);
                byte[] tail = new byte[tailLength];
                stream.Position = stream.Length - tailLength;
                int read = 0;
                while (read < tail.Length)
                {
                    int count = stream.Read(tail, read, tail.Length - read);
                    if (count == 0) break;
                    read += count;
                }

                int padding = 0;
                for (int index = read - 1; index >= 0 && tail[index] == 0xAB; index--)
                {
                    padding++;
                }

                long payloadLength = Math.Max(0, available - padding);
                payloadLength -= payloadLength % blockSetSize;
                return payloadLength <= 0 ? 0u : (uint)Math.Min(uint.MaxValue, payloadLength);
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
