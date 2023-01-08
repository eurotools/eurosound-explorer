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
        [Category("From v6 and above")]
        public sbyte DopplerValue { get; set; }

        [DisplayName("User Value")]
        [Category("From v6 and above")]
        public sbyte UserValue { get; set; }

        [DisplayName("SFX Ducker")]
        [Category("From v6 and above")]
        public sbyte SFXDucker { get; set; }

        [DisplayName("Spare")]
        [Category("From v6 and above")]
        public sbyte Spare { get; set; }

        [DisplayName("Inner Radius")]
        [Category("v201 and v1 ONLY")]
        public short InnerRadius { get; set; }

        [DisplayName("Outer Radius")]
        [Category("v201 and v1 ONLY")]
        public short OuterRadius { get; set; }

        [DisplayName("Flags")]
        [Category("All Versions")]
        public ushort Flags { get; set; }
    }
}
