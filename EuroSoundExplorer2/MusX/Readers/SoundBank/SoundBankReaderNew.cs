using MusX.Objects;
using System.Collections.Generic;
using System.IO;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    internal class SoundBankReaderNew : SoundBankReader
    {
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

                    //Save position
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
                    if (headerData.Platform.Contains("PS2") || (headerData.Platform.Contains("XB") && headerData.FileVersion < 5) || (headerData.Platform.Contains("GC") && headerData.FileVersion < 5))
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

                        //Flags
                        for (int j = 0; j < 16; j++)
                        {
                            sbyte flagState = BReader.ReadSByte();
                            if (flagState == 1)
                            {
                                sample.Flags = (ushort)(sample.Flags | (flagState << j));
                            }
                        }

                        //User Flags
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

                    //return 
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

                    //Parse Xbox Offset
                    if (headerData.Platform.IndexOf("PC", System.StringComparison.OrdinalIgnoreCase) >= 0 || headerData.Platform.IndexOf("XB", System.StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        wavHeaderData.TotalSamples = CalculusLoopOffsets.EurocomImaToSamples((uint)wavHeaderData.SampleSize, 1);
                        wavHeaderData.LoopStartOffset = CalculusLoopOffsets.EurocomImaToSamples((uint)wavHeaderData.LoopStartOffset, 1);
                    }
                    else
                    {
                        wavHeaderData.TotalSamples = wavHeaderData.SampleSize / 2;
                        wavHeaderData.LoopStartOffset /= 2;
                    }

                    //Store current position
                    long prevPos = BReader.BaseStream.Position;

                    //Read audio pcm data
                    BReader.BaseStream.Seek(headerData.SampleDataStart + wavHeaderData.Address, SeekOrigin.Begin);
                    wavHeaderData.EncodedData = BReader.ReadBytes((int)wavHeaderData.SampleSize);

                    //Read coeffs
                    if (headerData.SpecialSampleInfoLength > 0)
                    {
                        BReader.BaseStream.Seek(headerData.SpecialSampleInfoStart + wavHeaderData.PsiSampleHeader, SeekOrigin.Begin);
                        BReader.BaseStream.Seek(28, SeekOrigin.Current);
                        wavHeaderData.DspCoeffs = new short[16];
                        for (int j = 0; j < wavHeaderData.DspCoeffs.Length; j++)
                        {
                            wavHeaderData.DspCoeffs[j] = BytesFunctions.FlipData(BReader.ReadInt16(), headerData.IsBigEndian);
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
