namespace MusX.Objects
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class StreamSample
    {
        //Markers
        public StartMarker[] StartMarkers = new StartMarker[0];
        public Marker[] Markers = new Marker[0];

        //Audio Data
        public byte[] EncodedData = new byte[0];

        //Parameters
        public uint BlockPosition;
        public uint MarkerOffset;
        public uint MarkerSize;
        public uint AudioOffset;
        public uint AudioSize;
        public uint StartMarkerOffset;
        public uint BaseVolume;
        public uint StartMarkersCount;
        public uint MarkersCount;
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
