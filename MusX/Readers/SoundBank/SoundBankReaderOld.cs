using MusX.Objects;
using System.Collections.Generic;
using System.IO;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    internal class SoundBankReaderOld : SoundBankReader
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        internal void ReadSoundbank(string filePath, SoundbankHeader headerData, SortedDictionary<uint, Sample> samplesDictionary, List<SampleData> wavesList, List<uint> duplicatedHashCodes)
        {
            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                //Go to SFX Start
                BReader.BaseStream.Seek(headerData.SFXStart, SeekOrigin.Begin);

                //Loop througt stored elements
                uint sfxCount = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                for (int i = 0; i < sfxCount; i++)
                {
                    uint hashcode = 0x1A000000 | BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                    uint sfxPos = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian);
                    long prevPos = BReader.BaseStream.Position;

                    //go to sound offset
                    BReader.BaseStream.Seek(sfxPos + headerData.SFXStart, SeekOrigin.Begin);

                    //Read sound properties
                    Sample sample = new Sample
                    {
                        HashCodeNumber = hashcode,
                        DuckerLenght = BytesFunctions.FlipData(BReader.ReadInt16(), headerData.IsBigEndian),
                        MinDelay = BytesFunctions.FlipData(BReader.ReadInt16(), headerData.IsBigEndian),
                        MaxDelay = BytesFunctions.FlipData(BReader.ReadInt16(), headerData.IsBigEndian),
                        InnerRadius = BytesFunctions.FlipData(BReader.ReadInt16(), headerData.IsBigEndian),
                        OuterRadius = BytesFunctions.FlipData(BReader.ReadInt16(), headerData.IsBigEndian),
                        ReverbSend = BReader.ReadSByte(),
                        TrackingType = BReader.ReadSByte(),
                        MaxVoices = BReader.ReadSByte(),
                        Priority = BReader.ReadSByte(),
                        Ducker = BReader.ReadSByte(),
                        MasterVolume = BReader.ReadSByte()
                    };

                    //Read flags
                    sample.Flags = BReader.ReadUInt16();

                    //get samples count
                    ushort sfxSamplesCount = BytesFunctions.FlipData(BReader.ReadUInt16(), headerData.IsBigEndian);

                    //Loop througt all SFX samples
                    for (int j = 0; j < sfxSamplesCount; j++)
                    {
                        //Read sample properties
                        SampleInfo samplePoolItem = new SampleInfo()
                        {
                            FileRef = BytesFunctions.FlipData(BReader.ReadInt16(), headerData.IsBigEndian),
                            Pitch = BytesFunctions.FlipData(BReader.ReadInt16(), headerData.IsBigEndian) / 1024.0f,
                            PitchOffset = BytesFunctions.FlipData(BReader.ReadInt16(), headerData.IsBigEndian) / 1024.0f,
                            Volume = BReader.ReadSByte(),
                            VolumeOffset = BReader.ReadSByte(),
                            Pan = BReader.ReadSByte(),
                            PanOffset = BReader.ReadSByte()
                        };
                        sample.samplesList.Add(samplePoolItem);

                        //Padding
                        BReader.BaseStream.Seek(2, SeekOrigin.Current);
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
                    BReader.BaseStream.Seek(sfxPos + headerData.SFXStart, SeekOrigin.Begin);

                    //return 
                    BReader.BaseStream.Seek(prevPos, SeekOrigin.Begin);
                }

                //Go to sample info section
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
                        Channels = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        Bits = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        PsiSampleHeader = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        LoopStartOffset = BytesFunctions.FlipData(BReader.ReadUInt32(), headerData.IsBigEndian),
                        Duration = BReader.ReadUInt32(),
                    };
                    wavHeaderData.OriginalLoopOffset = wavHeaderData.LoopStartOffset;

                    if (wavHeaderData.OriginalLoopOffset > 5700 && wavHeaderData.OriginalLoopOffset < 5800)
                    {
                        var fgwefw = "HIH";
                    }

                    //Parse Xbox Offset
                    if (headerData.Platform.IndexOf("XB", System.StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        wavHeaderData.TotalSamples = CalculusLoopOffsets.XboxAdpcmToSamples((uint)wavHeaderData.SampleSize, 1);
                        wavHeaderData.LoopStartOffset = CalculusLoopOffsets.XboxAdpcmToSamples((uint)wavHeaderData.LoopStartOffset, 1);
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
