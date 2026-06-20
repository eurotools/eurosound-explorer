namespace MusX.Objects
{
    public class AudioDataReference
    {
        public string FilePath;
        public uint Offset;
        public uint Size;
        public EuroSoundAudioCodec Codec;
        public uint Frequency;
        public int Channels;
        public int InterleaveBlockSize;
    }
}
