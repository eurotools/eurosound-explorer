using System.ComponentModel;

namespace sb_explorer.Classes
{
    internal class SampleForPropGrid
    {
        //Parameters
        [DisplayName("Ducker Length")]
        [Category("All Versions")]
        public short DuckerLenght { get; set; }

        [DisplayName("Min Delay")]
        [Category("All Versions")]
        public short MinDelay { get; set; }

        [DisplayName("Max Delay")]
        [Category("All Versions")]
        public short MaxDelay { get; set; }

        [DisplayName("Reverb Send")]
        [Category("All Versions")]
        public sbyte ReverbSend { get; set; }

        [DisplayName("Tracking Type")]
        [Category("All Versions")]
        public string TrackingType { get; set; }

        [DisplayName("Max Voices")]
        [Category("All Versions")]
        public sbyte MaxVoices { get; set; }

        [DisplayName("Priority")]
        [Category("All Versions")]
        public sbyte Priority { get; set; }

        [DisplayName("Ducker")]
        [Category("All Versions")]
        public sbyte Ducker { get; set; }

        [DisplayName("Master Volume")]
        [Category("All Versions")]
        public sbyte MasterVolume { get; set; }

        [DisplayName("Group HashCode")]
        [Category("From v4 and above")]
        public short GroupHashCode { get; set; }

        [DisplayName("Group Max Channels")]
        [Category("From v4 and above")]
        public sbyte GroupMaxChannels { get; set; }

        [DisplayName("Doppler Value")]
        [Category("From v5 and above")]
        public sbyte DopplerValue { get; set; }

        [DisplayName("User Value")]
        [Category("From v5 and above")]
        public sbyte UserValue { get; set; }

        [DisplayName("SFX Ducker")]
        [Category("From v6 and above")]
        public sbyte SFXDucker { get; set; }

        [DisplayName("Spare")]
        [Category("From v6 and above")]
        public sbyte Spare { get; set; }

        [DisplayName("Inner Radius")]
        [Category("All Versions")]
        public short InnerRadius { get; set; }

        [DisplayName("Outer Radius")]
        [Category("All Versions")]
        public short OuterRadius { get; set; }

        [DisplayName("Flags")]
        [Category("All Versions")]
        public ushort Flags { get; set; }
    }

    internal sealed class SampleV18ForPropGrid
    {
        [Category("SFXInfo"), DisplayName("GUID")]
        public string Guid { get; set; }
        [Category("SFXInfo"), DisplayName("Parameter address")]
        public string ParameterAddress { get; set; }
        [Category("SFXInfo"), DisplayName("Pool address")]
        public string PoolAddress { get; set; }
        [Category("SFXInfo"), DisplayName("Pool element count")]
        public byte ElementCount { get; set; }
        [Category("SFXInfo"), DisplayName("Runtime status")]
        public byte RuntimeStatus { get; set; }
        [Category("SFXInfo"), DisplayName("Info flags")]
        public string InfoFlags { get; set; }

        [Category("Timing"), DisplayName("Attack time (ms)")]
        public ushort AttackTime { get; set; }
        [Category("Timing"), DisplayName("Release time (ms)")]
        public ushort ReleaseTime { get; set; }
        [Category("Timing"), DisplayName("Ducker offset")]
        public short DuckerOffset { get; set; }

        [Category("Project references"), DisplayName("Mix Group ID")]
        public string MixGroup { get; set; }
        [Category("Project references"), DisplayName("Ducker ID")]
        public string Ducker { get; set; }
        [Category("Project references"), DisplayName("Culling Group ID")]
        public string CullingGroup { get; set; }
        [Category("Project references"), DisplayName("Oscillator ID")]
        public string Oscillator { get; set; }
        [Category("Project references"), DisplayName("Controller ID")]
        public string Controller { get; set; }

        [Category("Effects"), DisplayName("Reverb send (%)")]
        public byte ReverbSend { get; set; }
        [Category("Effects"), DisplayName("MultiTap send (%)")]
        public byte MultiTapSend { get; set; }
        [Category("Effects"), DisplayName("PingPong send (%)")]
        public byte PingPongSend { get; set; }
        [Category("Effects"), DisplayName("Chorus send (%)")]
        public byte ChorusSend { get; set; }
        [Category("Effects"), DisplayName("Doppler (%)")]
        public byte Doppler { get; set; }
        [Category("Effects"), DisplayName("Low-pass type")]
        public byte LowPassType { get; set; }
        [Category("Effects"), DisplayName("Low-pass value")]
        public byte LowPassValue { get; set; }

        [Category("Playback"), DisplayName("Volume rolloff")]
        public sbyte VolumeRolloff { get; set; }
        [Category("Playback"), DisplayName("Maximum instances")]
        public byte MaxItems { get; set; }
        [Category("Playback"), DisplayName("Priority")]
        public byte Priority { get; set; }
        [Category("Playback"), DisplayName("Master volume (%)")]
        public byte MasterVolume { get; set; }
        [Category("Playback"), DisplayName("Play type")]
        public byte PlayType { get; set; }
        [Category("Playback"), DisplayName("Culling action")]
        public byte CullingAction { get; set; }
        [Category("Playback"), DisplayName("Trigger chance (%)")]
        public byte TriggerChance { get; set; }

        [Category("Flags"), DisplayName("Raw flags")]
        public string RawFlags { get; set; }
        [Category("SoundDetails"), DisplayName("Inner radius")]
        public short InnerRadius { get; set; }
        [Category("SoundDetails"), DisplayName("Outer radius")]
        public short OuterRadius { get; set; }
    }
}
