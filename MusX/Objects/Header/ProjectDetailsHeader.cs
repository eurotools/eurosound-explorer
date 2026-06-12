namespace MusX
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class ProjectDetailsHeader : SfxCommonHeader
    {
        public uint MemoryStart;
        public uint MemoryLength;

        //-------------------------------------------------------------------------------------------------------------------------------
        public ProjectDetailsHeader(SfxCommonHeader commonHeader = null)
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
