namespace MusX.Objects
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class StreamSample
    {
        private byte[] encodedData;

        //Markers
        public StartMarker[] StartMarkers = new StartMarker[0];
        public Marker[] Markers = new Marker[0];

        //Audio Data
        public AudioDataReference AudioReference;
        public byte[] EncodedData
        {
            get
            {
                if (encodedData == null && AudioReference != null)
                {
                    encodedData = EuroSoundAudioDataReader.Read(AudioReference);
                }

                return encodedData ?? new byte[0];
            }
            set { encodedData = value; }
        }

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
        public uint CodecType;
        public uint Flags;
        public uint SampleCount;
        public uint Frequency;
        public uint Channels = 1;
        public uint LoopStartSample;
        public uint LoopStartByteOffset;
        public uint LoopEndByteOffset;
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
