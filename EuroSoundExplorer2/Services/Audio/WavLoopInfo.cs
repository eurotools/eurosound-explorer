namespace sb_explorer.Services.Audio
{
    internal sealed class WavLoopInfo
    {
        public uint StartSample { get; private set; }
        public uint EndSample { get; private set; }

        public WavLoopInfo(uint startSample, uint endSample)
        {
            StartSample = startSample;
            EndSample = endSample;
        }

        public bool IsValid
        {
            get { return EndSample > StartSample; }
        }
    }
}
