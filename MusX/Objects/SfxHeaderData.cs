namespace MusX
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class SfxHeaderData
    {
        public bool IsBigEndian;
        public uint FileHashCode;
        public uint FileVersion;
        public uint FileSize;
        public string Platform;
        public uint Timespan;
        public uint EurocomIma;

        //Soundbanks
        public uint SFXStart;
        public uint SFXLenght;

        public uint SampleInfoStart;
        public uint SampleInfoLenght;

        public uint SpecialSampleInfoStart;
        public uint SpecialSampleInfoLength;

        public uint SampleDataStart;
        public uint SampleDataLength;

        //Streambank
        public uint FileStart1;
        public uint FileLength1;

        public uint FileStart2;
        public uint FileLength2;

        public uint FileStart3;
        public uint FileLength3;
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
