using System.ComponentModel;

namespace MusX.Objects
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class MusicSample
    {
        private byte[][] encodedData = new byte[2][];

        //Markers
        public StartMarker[] StartMarkers = new StartMarker[0];
        public Marker[] Markers = new Marker[0];

        //Channel info
        public AudioDataReference[] AudioReferences = new AudioDataReference[2];
        public byte[][] EncodedData
        {
            get
            {
                if ((encodedData[0] == null || encodedData[1] == null) && AudioReferences[0] != null)
                {
                    encodedData = EuroSoundAudioDataReader.ReadInterleavedStereo(AudioReferences[0]);
                }

                return encodedData;
            }
            set { encodedData = value; }
        }

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
