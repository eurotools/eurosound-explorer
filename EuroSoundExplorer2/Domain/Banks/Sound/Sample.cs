using System.Collections.Generic;

namespace MusX.Objects
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class Sample
    {
        //Parameters
        public uint HashCodeNumber;
        public short DuckerLenght;
        public short MinDelay;
        public short MaxDelay;
        public sbyte ReverbSend;
        public byte TrackingType;
        public sbyte MaxVoices;
        public sbyte Priority;
        public sbyte Ducker;
        public sbyte MasterVolume;
        public short GroupHashCode;
        public sbyte GroupMaxChannels;
        public sbyte DopplerValue;
        public sbyte UserValue;
        public sbyte SFXDucker;
        public sbyte Spare;
        public short InnerRadius;
        public short OuterRadius;

        // MUSX 10 replaces the legacy 16-bit flags plus adjacent legacy fields
        // with one 32-bit flag word.
        public uint V10Flags;
        public byte PlayType;

        //Flags
        public ushort Flags;
        public ushort UserFlags;

        // EngineXT v18 SFXInfo + SFXParameters (kept separately from the legacy projection).
        public bool IsV18;
        public byte V18RuntimeStatus;
        public byte V18InfoFlags;
        public long V18ParameterAddress;
        public long V18PoolAddress;
        public byte V18ElementCount;
        public uint V18Flags;
        public ushort V18AttackTime;
        public ushort V18ReleaseTime;
        public ushort V18MixGroup;
        public ushort V18Ducker;
        public ushort V18CullingGroup;
        public ushort V18Oscillator;
        public short V18DuckerOffset;
        public byte V18ReverbSend;
        public byte V18MultiTapSend;
        public byte V18PingPongSend;
        public byte V18LowPass;
        public sbyte V18VolumeRolloff;
        public byte V18MaxItems;
        public byte V18Priority;
        public byte V18MasterVolume;
        public byte V18PlayAndCull;
        public byte V18Doppler;
        public byte V18TriggerChance;
        public byte V18ChorusSend;
        public ushort V18Controller;

        //Samples
        public List<SampleInfo> samplesList = new List<SampleInfo>();
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
