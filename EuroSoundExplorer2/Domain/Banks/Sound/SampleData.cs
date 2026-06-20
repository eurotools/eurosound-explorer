namespace MusX.Objects
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class SampleData
    {
        private byte[] encodedData;

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
        public uint LoopStartSample;
        public uint TotalSamples;
        public uint Duration;
        public AudioDataReference AudioReference;
        public byte[] EncodedData
        {
            get
            {
                if (encodedData == null && AudioReference != null)
                {
                    encodedData = EuroSoundAudioDataReader.Read(AudioReference);
                }

                return encodedData;
            }
            set { encodedData = value; }
        }
        public short[] DspCoeffs;

        public bool IsLooped
        {
            get { return (Flags & 1) != 0; }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
