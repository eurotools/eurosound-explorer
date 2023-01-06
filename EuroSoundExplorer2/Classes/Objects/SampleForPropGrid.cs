using System.ComponentModel;

namespace sb_explorer.Classes
{
    internal class SampleForPropGrid
    {
        //Parameters
        [DisplayName("Ducker Length")]
        [Category("Sample Settings")]
        public short DuckerLenght { get; set; }

        [DisplayName("Min Delay")]
        [Category("Sample Settings")]
        public short MinDelay { get; set; }

        [DisplayName("Max Delay")]
        [Category("Sample Settings")]
        public short MaxDelay { get; set; }

        [DisplayName("Reverb Send")]
        [Category("Sample Settings")]
        public sbyte ReverbSend { get; set; }

        [DisplayName("Tracking Type")]
        [Category("Sample Settings")]
        public string TrackingType { get; set; }

        [DisplayName("Max Voices")]
        [Category("Sample Settings")]
        public sbyte MaxVoices { get; set; }

        [DisplayName("Priority")]
        [Category("Sample Settings")]
        public sbyte Priority { get; set; }

        [DisplayName("Ducker")]
        [Category("Sample Settings")]
        public sbyte Ducker { get; set; }

        [DisplayName("Master Volume")]
        [Category("Sample Settings")]
        public sbyte MasterVolume { get; set; }

        [DisplayName("Group HashCode")]
        [Category("Sample Settings (V4 and above)")]
        public short GroupHashCode { get; set; }

        [DisplayName("Group Max Channels")]
        [Category("Sample Settings (V4 and above)")]
        public sbyte GroupMaxChannels { get; set; }

        [DisplayName("Doppler Value")]
        [Category("Sample Settings (V6 and above)")]
        public sbyte DopplerValue { get; set; }

        [DisplayName("User Value")]
        [Category("Sample Settings (V6 and above)")]
        public sbyte UserValue { get; set; }

        [DisplayName("SFX Ducker")]
        [Category("Sample Settings (V6 and above)")]
        public sbyte SFXDucker { get; set; }

        [DisplayName("Spare")]
        [Category("Sample Settings (V6 and above)")]
        public sbyte Spare { get; set; }

        [DisplayName("Inner Radius")]
        [Category("Sample Settings (V201 and V1 ONLY)")]
        public short InnerRadius { get; set; }

        [DisplayName("Outer Radius")]
        [Category("Sample Settings (V201 and V1 ONLY)")]
        public short OuterRadius { get; set; }

        [DisplayName("Flags")]
        [Category("Sample Settings")]
        public ushort Flags { get; set; }
    }
}
