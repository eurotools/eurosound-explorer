namespace MusX.Objects
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class SampleInfo
    {
        public string FilePath;
        public short FileRef;
        public float Pitch;
        public float PitchOffset;
        public float Volume;
        public float VolumeOffset;
        public float Pan;
        public float PanOffset;

        // MUSX 10 stores a complete hashcode in each POOL/ELMT reference.
        // FileRef remains populated for compatibility with the existing UI.
        public uint ReferenceHashCode;
        public short MinDelay;
        public short MaxDelay;
        public byte DelayType;
        public byte IsReleaseElement;
        public byte Spare;
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
