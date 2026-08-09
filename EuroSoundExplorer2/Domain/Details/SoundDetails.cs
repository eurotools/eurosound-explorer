namespace MusX.Objects
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class SoundDetails
    {
        public uint MinHashCode;
        public uint MaxHashCode;
        public SoundDetailsData[] sfxItems;
    }


    //-------------------------------------------------------------------------------------------------------------------------------
    public class SoundDetailsData
    {
        public int HashCode;
        public float InnerRadius;
        public float OuterRadius;
        public float Duration;
        public bool Looping;
        public sbyte Tracking3D;
        public bool SampleStreamed;
        public bool Is3D;
        public bool KillOnNodeDelete;
        public bool IsMusic;
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
