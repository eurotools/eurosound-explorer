namespace MusX
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class StreambankHeader : SfxCommonHeader
    {
        public uint FileStart1;
        public uint FileLength1;

        public uint FileStart2;
        public uint FileLength2;

        public uint FileStart3;
        public uint FileLength3;

        public uint CodecType;
        public uint StreamFlags;
        public uint LoopStartByteOffset;
        public uint LoopEndByteOffset;
        public uint SampleCount;
        public uint LoopStartSample;

        //-------------------------------------------------------------------------------------------------------------------------------
        public StreambankHeader(SfxCommonHeader commonHeader = null)
        {
            if (commonHeader != null)
            {
                IsBigEndian = commonHeader.IsBigEndian;
                FileHashCode = commonHeader.FileHashCode;
                FileVersion = commonHeader.FileVersion;
                FileSize = commonHeader.FileSize;
                Platform = commonHeader.Platform;
                Timespan = commonHeader.Timespan;
                UsesAdpcm = commonHeader.UsesAdpcm;
                EndOffset = commonHeader.EndOffset;
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
