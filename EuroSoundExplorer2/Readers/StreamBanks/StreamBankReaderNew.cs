using MusX.Objects;
using System.Collections.Generic;
using System.IO;
using System;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class StreamBankReaderNew : StreamBankReader
    {
        internal static void ReadStreamFileV18(string filePath, StreambankHeader headerData, List<StreamSample> streamedSamples)
        {
            EuroSoundAudioCodec codec;
            switch (headerData.FileStart1)
            {
                case 1: codec = EuroSoundAudioCodec.EurocomImaAdpcm; break;
                case 2: codec = EuroSoundAudioCodec.SonyVagAdpcm; break;
                case 3: codec = headerData.FileVersion >= 21 ? EuroSoundAudioCodec.DspAdpcmNgca : EuroSoundAudioCodec.DspAdpcmLegacy; break;
                case 4: codec = EuroSoundAudioCodec.Pcm16; break;
                case 6: codec = EuroSoundAudioCodec.Xma; break;
                default: codec = EuroSoundAudioCodec.Unknown; break;
            }
            if (headerData.FileStart1 == 3)
                codec = DetectDspContainerCodec(filePath, headerData.FileStart2, codec);
            StreamSample sample = new StreamSample
            {
                AudioOffset = headerData.FileStart2,
                AudioSize = headerData.FileLength2,
                BlockPosition = 0,
                CodecType = headerData.CodecType,
                Flags = headerData.StreamFlags,
                SampleCount = headerData.SampleCount,
                LoopStartSample = headerData.LoopStartSample,
                LoopStartByteOffset = headerData.LoopStartByteOffset,
                LoopEndByteOffset = headerData.LoopEndByteOffset,
                Frequency = headerData.Frequency,
                Channels = Math.Max(1u, headerData.Channels),
                AudioReference = new AudioDataReference { FilePath = filePath, Offset = headerData.FileStart2, Size = headerData.FileLength2, Codec = codec, Frequency = headerData.Frequency, Channels = (int)Math.Max(1u, headerData.Channels) }
            };
            ResolveV18Metadata(filePath, sample, headerData.Platform);
            streamedSamples.Add(sample);
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

        private static void ResolveV18Metadata(string streamPath, StreamSample sample, string platform)
        {
            string directory = Path.GetDirectoryName(streamPath);
            if (string.IsNullOrEmpty(directory)) return;
            string target = Path.GetFullPath(streamPath);
            foreach (string candidate in FindCandidateSoundBanks(directory))
            {
                if (string.Equals(Path.GetFullPath(candidate), target, StringComparison.OrdinalIgnoreCase) || !HasSbnkPayload(candidate)) continue;
                try
                {
                    SoundBankReader soundBankReader = new SoundBankReader();
                    SoundbankHeader soundBankHeader = soundBankReader.ReadSfxHeader(candidate, platform);
                    SortedDictionary<uint, Sample> samples = new SortedDictionary<uint, Sample>();
                    List<SampleData> waves = new List<SampleData>();
                    List<uint> duplicates = new List<uint>();
                    soundBankReader.ReadSoundBank(candidate, soundBankHeader, samples, waves, duplicates);
                    foreach (SampleData wave in waves)
                    {
                        if (wave.AudioReference == null || string.IsNullOrEmpty(wave.AudioReference.FilePath) ||
                            !string.Equals(Path.GetFullPath(wave.AudioReference.FilePath), target, StringComparison.OrdinalIgnoreCase)) continue;
                        sample.Frequency = wave.Frequency;
                        sample.Channels = Math.Max(1u, wave.Channels);
                        if (wave.TotalSamples != 0) sample.SampleCount = wave.TotalSamples;
                        if (wave.IsLooped && wave.LoopStartSample != uint.MaxValue) sample.LoopStartSample = wave.LoopStartSample;
                        sample.AudioReference.Frequency = sample.Frequency;
                        sample.AudioReference.Channels = (int)sample.Channels;
                        return;
                    }
                }
                catch (InvalidDataException) { }
                catch (EndOfStreamException) { }
            }
        }

        private static IEnumerable<string> FindCandidateSoundBanks(string streamDirectory)
        {
            HashSet<string> yielded = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string path in Directory.GetFiles(streamDirectory, "*.sfx", SearchOption.TopDirectoryOnly))
                if (yielded.Add(path)) yield return path;
            DirectoryInfo parent = Directory.GetParent(streamDirectory);
            if (parent == null) yield break;
            foreach (string path in Directory.GetFiles(parent.FullName, "*.sfx", SearchOption.AllDirectories))
                if (yielded.Add(path)) yield return path;
        }

        private static bool HasSbnkPayload(string path)
        {
            using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (stream.Length < 0x804) return false;
                stream.Position = 0x800;
                byte[] magic = new byte[4];
                return stream.Read(magic, 0, 4) == 4 && magic[0] == (byte)'S' && magic[1] == (byte)'B' && magic[2] == (byte)'N' && magic[3] == (byte)'K';
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        internal void ReadStreamFile(string filePath, StreambankHeader headerData, List<StreamSample> streamedSamples)
        {
            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                //Go to File Start 1
                BReader.BaseStream.Seek(headerData.FileStart1, SeekOrigin.Begin);

                //Get count of the stored elements
                uint[] storedElements = new uint[headerData.FileLength1 / 4];

                //Read Offsets
                for (int i = 0; i < storedElements.Length; i++)
                {
                    storedElements[i] = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                }

                //Read File Section 2
                for (int i = 0; i < storedElements.Length; i++)
                {
                    BReader.BaseStream.Seek(headerData.FileStart2 + storedElements[i], SeekOrigin.Begin);

                    StreamSample streamSample = new StreamSample
                    {
                        BlockPosition = storedElements[i],
                        MarkerSize = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        AudioOffset = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        AudioSize = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        StartMarkersCount = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        MarkersCount = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        StartMarkerOffset = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        MarkerOffset = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        BaseVolume = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian)
                    };

                    //Stream marker start data
                    streamSample.StartMarkers = new StartMarker[streamSample.StartMarkersCount];
                    for (int j = 0; j < streamSample.StartMarkersCount; j++)
                    {
                        StartMarker startMarker = EuroSoundMarkerReader.ReadNewStartMarker(BReader, headerData);
                        EuroSoundMarkerReader.ConvertMarkerOffsets(startMarker, headerData, EuroSoundBankType.StreamBank, 1);

                        //Add marker
                        streamSample.StartMarkers[j] = startMarker;
                    }

                    //Stream marker data 
                    streamSample.Markers = new Marker[streamSample.MarkersCount];
                    for (int j = 0; j < streamSample.MarkersCount; j++)
                    {
                        Marker DataMarker = EuroSoundMarkerReader.ReadNewMarker(BReader, headerData);
                        EuroSoundMarkerReader.ConvertMarkerOffsets(DataMarker, headerData, EuroSoundBankType.StreamBank, 1);

                        //Add marker
                        streamSample.Markers[j] = DataMarker;
                    }

                    //Read Audio Data
                    EuroSoundAudioCodec codec = EuroSoundCodecMatrix.GetCodec(headerData.FileVersion, headerData.Platform, EuroSoundBankType.StreamBank);
                    streamSample.AudioReference = new AudioDataReference
                    {
                        FilePath = filePath,
                        Offset = headerData.FileStart2 + streamSample.AudioOffset,
                        Size = streamSample.AudioSize,
                        Codec = codec,
                        Frequency = 0,
                        Channels = 1
                    };

                    //Add audio to list
                    streamedSamples.Add(streamSample);
                }

                BReader.Close();
            }
        }

    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
