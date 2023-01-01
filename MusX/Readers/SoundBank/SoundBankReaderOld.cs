using MusX.Objects;
using System.Collections.Generic;
using System.IO;

namespace MusX.Readers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    internal class SoundBankReaderOld
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        internal void ReadSoundbank(string filePath, SfxHeaderData headerData, SortedDictionary<uint, Sample> samplesDictionary, List<SampleData> wavesList)
        {
            using (BinaryReader BReader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                //Go to SFX Start
                BReader.BaseStream.Seek(headerData.SFXStart, SeekOrigin.Begin);

                //Loop througt stored elements
                uint sfxCount = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian);
                for (int i = 0; i < sfxCount; i++)
                {
                    uint hashcode = 0x1A000000 | BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian);
                    uint sfxPos = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian);
                    long prevPos = BReader.BaseStream.Position;

                    //go to sound offset
                    BReader.BaseStream.Seek(sfxPos + headerData.SFXStart, SeekOrigin.Begin);

                    //Read sound properties
                    Sample sample = new Sample
                    {
                        DuckerLenght = BinaryFunctions.FlipShort(BReader.ReadInt16(), headerData.IsBigEndian),
                        MinDelay = BinaryFunctions.FlipShort(BReader.ReadInt16(), headerData.IsBigEndian),
                        MaxDelay = BinaryFunctions.FlipShort(BReader.ReadInt16(), headerData.IsBigEndian),
                        InnerRadius = BinaryFunctions.FlipShort(BReader.ReadInt16(), headerData.IsBigEndian),
                        OuterRadius = BinaryFunctions.FlipShort(BReader.ReadInt16(), headerData.IsBigEndian),
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
                    ushort sfxSamplesCount = BinaryFunctions.FlipUShort(BReader.ReadUInt16(), headerData.IsBigEndian);

                    //Loop througt all SFX samples
                    for (int j = 0; j < sfxSamplesCount; j++)
                    {
                        //Read sample properties
                        SampleInfo samplePoolItem = new SampleInfo()
                        {
                            FileRef = BinaryFunctions.FlipShort(BReader.ReadInt16(), headerData.IsBigEndian),
                            Pitch = BinaryFunctions.FlipShort(BReader.ReadInt16(), headerData.IsBigEndian) / 1024.0f,
                            PitchOffset = BinaryFunctions.FlipShort(BReader.ReadInt16(), headerData.IsBigEndian) / 1024.0f,
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
                    if (!samplesDictionary.ContainsKey(hashcode))
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
                uint waveCount = BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian);
                for (int i = 0; i < waveCount; i++)
                {
                    SampleData wavHeaderData = new SampleData
                    {
                        Flags = (ushort)BinaryFunctions.FlipUInt32(BReader.ReadUInt32(), headerData.IsBigEndian),
                        Address = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian),
                        MemorySize = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian),
                        Frequency = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian),
                        SampleSize = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian),
                        Channels = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian),
                        Bits = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian),
                        PsiSampleHeader = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian),
                        LoopStartOffset = BinaryFunctions.FlipInt32(BReader.ReadInt32(), headerData.IsBigEndian),
                        Duration = BReader.ReadInt32(),
                    };

                    //Parse Xbox Offset
                    if (headerData.Platform.IndexOf("XB", System.StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        wavHeaderData.LoopStartOffset = (int)CalculusLoopOffsets.ReverseGetXboxAlignedNumber((uint)wavHeaderData.LoopStartOffset);
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
                            wavHeaderData.DspCoeffs[j] = BinaryFunctions.FlipShort(BReader.ReadInt16(), headerData.IsBigEndian);
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
