namespace sb_explorer.Services.Audio
{
    internal sealed class DecodedAudio
    {
        internal byte[][] Channels { get; set; }
        internal uint SampleRate { get; set; }
        internal uint SampleCount { get; set; }

        internal DecodedAudio()
        {
            Channels = new byte[0][];
        }
    }
}
