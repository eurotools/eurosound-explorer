using System.ComponentModel;

using System;

namespace sb_explorer.Classes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class MinimumLegacyVersionAttribute : Attribute
    {
        public int Version { get; private set; }

        public MinimumLegacyVersionAttribute(int version)
        {
            Version = version;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class ExactLegacyVersionAttribute : Attribute
    {
        public int Version { get; private set; }

        public ExactLegacyVersionAttribute(int version)
        {
            Version = version;
        }
    }

    internal class SampleForPropGrid : ICustomTypeDescriptor
    {
        [Browsable(false)]
        public int FileVersion { get; set; }

        [Category("Timing"), DisplayName("Ducker Length")]
        public short DuckerLenght { get; set; }
        [Category("Timing"), DisplayName("Min Delay")]
        public short MinDelay { get; set; }
        [Category("Timing"), DisplayName("Max Delay")]
        public short MaxDelay { get; set; }
        [Category("Effects"), DisplayName("Reverb Send")]
        public sbyte ReverbSend { get; set; }
        [Category("Spatial"), DisplayName("Tracking Type")]
        public string TrackingType { get; set; }
        [Category("Playback"), DisplayName("Max Voices")]
        public sbyte MaxVoices { get; set; }
        [Category("Playback"), DisplayName("Priority")]
        public sbyte Priority { get; set; }
        [Category("Project references"), DisplayName("Ducker")]
        public sbyte Ducker { get; set; }
        [Category("Playback"), DisplayName("Master Volume")]
        public sbyte MasterVolume { get; set; }
        [Category("Project references"), DisplayName("Group HashCode"), MinimumLegacyVersion(4)]
        public short GroupHashCode { get; set; }
        [Category("Playback"), DisplayName("Group Max Channels"), MinimumLegacyVersion(4)]
        public sbyte GroupMaxChannels { get; set; }
        [Category("Effects"), DisplayName("Doppler Value"), MinimumLegacyVersion(5)]
        public sbyte DopplerValue { get; set; }
        [Category("Miscellaneous"), DisplayName("User Value"), MinimumLegacyVersion(5)]
        public sbyte UserValue { get; set; }
        [Category("Project references"), DisplayName("SFX Ducker"), MinimumLegacyVersion(6)]
        public sbyte SFXDucker { get; set; }
        [Category("Miscellaneous"), DisplayName("Spare"), MinimumLegacyVersion(6)]
        public sbyte Spare { get; set; }
        [Category("Spatial"), DisplayName("Inner Radius")]
        public short InnerRadius { get; set; }
        [Category("Spatial"), DisplayName("Outer Radius")]
        public short OuterRadius { get; set; }
        [Category("Flags"), DisplayName("Flags")]
        public ushort Flags { get; set; }
        [Category("Playback"), DisplayName("Play Type"), ExactLegacyVersion(10)]
        public string PlayType { get; set; }
        [Category("Flags"), DisplayName("MUSX 10 Flags"), ExactLegacyVersion(10)]
        public string V10Flags { get; set; }
        [Category("Playback"), DisplayName("Instance Culling"), ExactLegacyVersion(10)]
        public string InstanceCulling { get; set; }
        [Category("Playback"), DisplayName("Group Culling"), ExactLegacyVersion(10)]
        public string GroupCulling { get; set; }

        AttributeCollection ICustomTypeDescriptor.GetAttributes() { return TypeDescriptor.GetAttributes(this, true); }
        string ICustomTypeDescriptor.GetClassName() { return TypeDescriptor.GetClassName(this, true); }
        string ICustomTypeDescriptor.GetComponentName() { return TypeDescriptor.GetComponentName(this, true); }
        TypeConverter ICustomTypeDescriptor.GetConverter() { return TypeDescriptor.GetConverter(this, true); }
        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent() { return TypeDescriptor.GetDefaultEvent(this, true); }
        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty() { return TypeDescriptor.GetDefaultProperty(this, true); }
        object ICustomTypeDescriptor.GetEditor(Type editorBaseType) { return TypeDescriptor.GetEditor(this, editorBaseType, true); }
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes) { return TypeDescriptor.GetEvents(this, attributes, true); }
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents() { return TypeDescriptor.GetEvents(this, true); }
        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes) { return GetVisibleProperties(attributes); }
        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties() { return GetVisibleProperties(null); }
        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd) { return this; }

        private PropertyDescriptorCollection GetVisibleProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection properties = attributes == null
                ? TypeDescriptor.GetProperties(this, true)
                : TypeDescriptor.GetProperties(this, attributes, true);
            PropertyDescriptor[] visible = new PropertyDescriptor[properties.Count];
            int count = 0;
            foreach (PropertyDescriptor property in properties)
            {
                MinimumLegacyVersionAttribute minimum = (MinimumLegacyVersionAttribute)property.Attributes[typeof(MinimumLegacyVersionAttribute)];
                ExactLegacyVersionAttribute exact = (ExactLegacyVersionAttribute)property.Attributes[typeof(ExactLegacyVersionAttribute)];
                bool minimumVisible = minimum == null || (FileVersion >= minimum.Version && FileVersion <= 6);
                bool exactVisible = exact == null || FileVersion == exact.Version;
                if (property.IsBrowsable && minimumVisible && exactVisible)
                    visible[count++] = property;
            }
            Array.Resize(ref visible, count);
            return new PropertyDescriptorCollection(visible, true);
        }
    }

    internal sealed class SampleV10VerifiedForPropGrid
    {
        [Category("Identity"), DisplayName("HashCode")]
        public string HashCode { get; set; }

        [Category("Probable (comparative evidence)"), DisplayName("Flags word (raw, probable)")]
        public string ProbableFlags { get; set; }
        [Category("Probable (comparative evidence)"), DisplayName("Ducker Length (probable)")]
        public short ProbableDuckerLength { get; set; }
        [Category("Probable (comparative evidence)"), DisplayName("Min Delay (probable)")]
        public short ProbableMinDelay { get; set; }
        [Category("Probable (comparative evidence)"), DisplayName("Max Delay (probable)")]
        public short ProbableMaxDelay { get; set; }
        [Category("Probable (comparative evidence)"), DisplayName("Group HashCode (probable)")]
        public string ProbableGroupHashCode { get; set; }
        [Category("Probable (comparative evidence)"), DisplayName("Reverb Send (probable)")]
        public sbyte ProbableReverbSend { get; set; }
        [Category("Probable (comparative evidence)"), DisplayName("Max Voices (probable)")]
        public sbyte ProbableMaxVoices { get; set; }
        [Category("Probable (comparative evidence)"), DisplayName("Priority (probable)")]
        public sbyte ProbablePriority { get; set; }
        [Category("Probable (comparative evidence)"), DisplayName("Ducker (probable)")]
        public sbyte ProbableDucker { get; set; }
        [Category("Probable (comparative evidence)"), DisplayName("Master Volume (probable)")]
        public sbyte ProbableMasterVolume { get; set; }
        [Category("Probable (comparative evidence)"), DisplayName("Group Max Channels (probable)")]
        public sbyte ProbableGroupMaxChannels { get; set; }
        [Category("Probable (comparative evidence)"), DisplayName("Play Type value (probable, undecoded)")]
        public byte ProbablePlayType { get; set; }
        [Category("Probable (comparative evidence)"), DisplayName("Doppler value (probable)")]
        public sbyte ProbableDoppler { get; set; }
        [Category("Probable (comparative evidence)"), DisplayName("SFX Ducker (probable)")]
        public sbyte ProbableSfxDucker { get; set; }

        [Category("Raw data"), DisplayName("PARA/DATA bytes after HashCode")]
        public string RawParameterData { get; set; }
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
        public string PlayType { get; set; }
        [Category("Playback"), DisplayName("Culling action")]
        public string CullingAction { get; set; }
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
