namespace MusX.Objects
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class Marker
    {
        public int Index;
        public uint Position;
        public uint OriginalPosition;
        public byte Type;
        public byte Flags;
        public byte Extra;
        public uint LoopStart;
        public uint OriginalLoopStart;
        public int LoopMarkerCount;
        public int MarkerCount;
        public bool HasExtendedFields;
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
