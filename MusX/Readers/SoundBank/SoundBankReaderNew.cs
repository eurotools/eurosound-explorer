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
        internal void ReadSoundbank(string filePath, SoundbankHeader headerData, SortedDictionary<uint, Sample> samplesDictionary, List<SampleData> wavesList)
        {
            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                //Read SFX Start
                BReader.BaseStream.Seek(headerData.SFXStart, SeekOrigin.Begin);
                uint sfxCount = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                for (int i = 0; i < sfxCount; i++)
                {
                    uint hashcode;
                    switch (headerData.FileVersion)
                    {
                        case 201:
                            hashcode = 0x1A000000 | BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                            break;
                        case 6:
                            hashcode = 0x2D700000 | BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                            break;
                        default:
                            hashcode = 0x1AF00000 | BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                            break;
                    }

                    uint curSfxPos = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                    long prevPos = BReader.BaseStream.Position;

                    //Goto SFX Data
                    BReader.BaseStream.Seek(headerData.SFXStart + curSfxPos, SeekOrigin.Begin);

                    //Save position
                    Sample sample = new Sample
                    {
                        DuckerLenght = BinaryFunctions.FlipData(BReader.ReadInt16(), headerData.IsBigEndian),
                        MinDelay = BinaryFunctions.FlipData(BReader.ReadInt16(), headerData.IsBigEndian),
                        MaxDelay = BinaryFunctions.FlipData(BReader.ReadInt16(), headerData.IsBigEndian),
                        ReverbSend = BReader.ReadSByte(),
                        TrackingType = BReader.ReadSByte(),
                        MaxVoices = BReader.ReadSByte(),
                        Priority = BReader.ReadSByte(),
                        Ducker = BReader.ReadSByte(),
                        MasterVolume = BReader.ReadSByte()
                    };

                    //Read flags and sample pool
                    if (headerData.Platform.Contains("PS2") || (headerData.Platform.Contains("XB") && headerData.FileVersion < 5) || (headerData.Platform.Contains("GC") && headerData.FileVersion < 5))
                    {
                        sample.GroupHashCode = BReader.ReadInt16();
                        sample.GroupMaxChannels = (sbyte)(sample.GroupHashCode & 15);
                        sample.GroupHashCode >>= 4;

                        //Read Flags
                        sample.Flags = BReader.ReadUInt16();

                        //Read UserFlags
                        if (headerData.FileVersion > 4)
                        {
                            sample.UserFlags = BinaryFunctions.FlipData(BReader.ReadUInt16(), headerData.IsBigEndian);
                            sample.DopplerValue = BReader.ReadSByte();
                            sample.UserValue = BReader.ReadSByte();
                        }
                    }
                    else
                    {
                        sample.GroupHashCode = BinaryFunctions.FlipData(BReader.ReadInt16(), headerData.IsBigEndian);
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
                    ushort samplesCount = BinaryFunctions.FlipData(BReader.ReadUInt16(), headerData.IsBigEndian);
                    for (int j = 0; j < samplesCount; j++)
                    {
                        SampleInfo samplePoolItem = new SampleInfo
                        {
                            FileRef = BinaryFunctions.FlipData(BReader.ReadInt16(), headerData.IsBigEndian),
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
                    if (!samplesDictionary.ContainsKey(hashcode))
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
                uint waveCount = BinaryFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                for (int i = 0; i < waveCount; i++)
                {
                    SampleData wavHeaderData = new SampleData
                    {
                        Flags = BinaryFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian),
                        Address = BinaryFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian),
                        MemorySize = BinaryFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian),
                        Frequency = BinaryFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian),
                        SampleSize = BinaryFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian),
                        PsiSampleHeader = BinaryFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian),
                        LoopStartOffset = BinaryFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian),
                        Duration = BinaryFunctions.FlipData(BReader.ReadInt32(), headerData.IsBigEndian)
                    };

                    //Parse Xbox Offset
                    if (headerData.Platform.IndexOf("PC", System.StringComparison.OrdinalIgnoreCase) >= 0 || headerData.Platform.IndexOf("XB", System.StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        wavHeaderData.LoopStartOffset = (int)CalculusLoopOffsets.GetSoundBankEurocomImaLoopOffset((uint)wavHeaderData.LoopStartOffset);
                    }

                    //Store current position
                    long prevPos = BReader.BaseStream.Position;

                    //Read audio pcm data
                    BReader.BaseStream.Seek(headerData.SampleDataStart + wavHeaderData.Address, SeekOrigin.Begin);
                    wavHeaderData.EncodedData = BReader.ReadBytes(wavHeaderData.SampleSize);

                    //Read coeffs
                    if (headerData.SpecialSampleInfoLength > 0)
                    {
                        BReader.BaseStream.Seek(headerData.SpecialSampleInfoStart + wavHeaderData.PsiSampleHeader, SeekOrigin.Begin);
                        BReader.BaseStream.Seek(28, SeekOrigin.Current);
                        wavHeaderData.DspCoeffs = new short[16];
                        for (int j = 0; j < wavHeaderData.DspCoeffs.Length; j++)
                        {
                            wavHeaderData.DspCoeffs[j] = BinaryFunctions.FlipData(BReader.ReadInt16(), headerData.IsBigEndian);
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
