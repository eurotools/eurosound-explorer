using System.Collections.Generic;
using System.ComponentModel;

namespace MusX.Objects
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class ProjectDetails
    {
        [DisplayName("Format Version")]
        [Category("Project")]
        public int FormatVersion { get; set; }

        [DisplayName("Maximum Memory Map Size")]
        [Category("Memory Slots")]
        public int MaximumMemoryMapSize { get; set; }

        [DisplayName("Memory Map Count")]
        [Category("Memory Slots")]
        public int MemoryMapCount { get { return memoryMapsData.Count; } }

        [DisplayName("Memory Slots Count")]
        [Category("Memory Slots")]
        public int MemmorySlotsCount { get; set; }

        [DisplayName("Memory Slots Offset")]
        [Category("Memory Slots")]
        public int MemorySlotsOffset { get; set; }

        [DisplayName("Soundbanks Count")]
        [Category("SoundBanks")]
        public int SoundBanksCount { get; set; }

        [DisplayName("Soundbanks Offset")]
        [Category("SoundBanks")]
        public int SoundBanksOffset { get; set; }

        [DisplayName("Stereo Streams Offset")]
        [Category("Streams")]
        public int StereoStreamCount { get; set; }

        [DisplayName("Mono Streams Count")]
        [Category("Streams")]
        public int MonoStreamCount { get; set; }

        [DisplayName("Project Code")]
        [Category("Project")]
        public int ProjectCode { get; set; }

        [DisplayName("Effects Count")]
        [Category("EngineXT v18")]
        public int EffectsCount { get; set; }

        [DisplayName("Mix Groups Count")]
        [Category("EngineXT v18")]
        public int MixGroupsCount { get; set; }

        [DisplayName("Duckers Count")]
        [Category("EngineXT v18")]
        public int DuckersCount { get; set; }

        [DisplayName("Culling Groups Count")]
        [Category("EngineXT v18")]
        public int CullingGroupsCount { get; set; }

        [DisplayName("Oscillators Count")]
        [Category("EngineXT v18")]
        public int OscillatorsCount { get; set; }

        public List<ProjectSoundBank> soundBanksData = new List<ProjectSoundBank>();
        public List<ProjectSlots> memorySlotsData = new List<ProjectSlots>();
        public List<ProjectMemoryMap> memoryMapsData = new List<ProjectMemoryMap>();
        public List<ProjectRuntimeObject> runtimeObjects = new List<ProjectRuntimeObject>();
        public List<int> userValues = new List<int>();

        public int[] flagsValues = new int[10];
    }

    public class ProjectRuntimeObject
    {
        public string Type { get; set; }
        public uint HashCode { get; set; }
        public string Details { get; set; }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
    public class ProjectMemoryMap
    {
        public string Name { get; set; }
        public List<int> SlotSizes { get; private set; }

        public ProjectMemoryMap()
        {
            Name = string.Empty;
            SlotSizes = new List<int>();
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
