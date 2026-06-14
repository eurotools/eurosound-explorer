using System.Collections.Generic;

namespace MusX.Objects
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public class MusicMarkers
    {
        public uint MusicHeadersCount;
        public uint MusicHeadersPadding;
        public uint MusicMarkerCountsCount;
        public uint MusicMarkerCountsPadding;
        public uint MusicMarkerListsCount;
        public uint MusicMarkerListsPadding;

        public List<MusicMarkerHeader> MusicHeaders = new List<MusicMarkerHeader>();
        public List<MusicMarkerCounts> MusicMarkerCounts = new List<MusicMarkerCounts>();
        public List<MusicMarkerListEntry> MusicMarkerLists = new List<MusicMarkerListEntry>();
    }

    //-------------------------------------------------------------------------------------------------------------------------------
    public class MusicMarkerHeader
    {
        public uint MusicHashCode;
        public uint StreamDataOffset;
        public uint BaseVolume;
        public uint Padding;
    }

    //-------------------------------------------------------------------------------------------------------------------------------
    public class MusicMarkerCounts
    {
        public uint StartMarkerCount;
        public uint MarkerCount;
        public uint Padding0;
        public uint Padding1;
    }

    //-------------------------------------------------------------------------------------------------------------------------------
    public class MusicMarkerListEntry
    {
        public uint Position;
        public int LoopStart;
        public uint Padding0;
        public uint Padding1;
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
