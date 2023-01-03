using System.ComponentModel;

namespace MusX.Objects
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class MusicSample
    {
        //Markers
        public StartMarker[] StartMarkers = new StartMarker[0];
        public Marker[] Markers = new Marker[0];

        //Channel info
        public byte[][] EncodedData = new byte[2][];

        //Parameters
        [Category("Marker Header")]
        public uint StartMarkersCount { get; set; }
        [Category("Marker Header")]
        public uint MarkersCount { get; set; }
        [Category("Marker Header")]
        public uint StartMarkerOffset { get; set; }
        [Category("Marker Header")]
        public uint MarkerOffset { get; set; }
        [Category("Marker Header")]
        public uint BaseVolume { get; set; }

        [Category("Header")]
        public uint AudioOffset { get; set; }
        [Category("Header")]
        public uint AudioSize { get; set; }

    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
