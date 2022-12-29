﻿namespace EuroSoundExplorer2
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class SoundFile
    {
        public byte[][] PcmData = new byte[1][];
        public int startPos;
        public int loopOffset;
        public bool isLooped;
        public int sampleRate;
        public int channels;
        public float volume = 1;
        public float pitch;
        public float panning;
        public float volumeOffset;
        public float pitchOffset;
        public float panningOffset;
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
