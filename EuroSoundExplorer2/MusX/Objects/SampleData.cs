namespace MusX.Objects
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class SampleData
    {
        public uint Flags;
        public uint Address;
        public uint MemorySize;
        public uint Frequency;
        public uint SampleSize;
        public uint PsiSampleHeader;
        public uint Channels;
        public uint Bits;
        public uint LoopStartOffset;
        public uint OriginalLoopOffset;
        public uint TotalSamples;
        public uint Duration;
        public byte[] EncodedData;
        public short[] DspCoeffs;
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
