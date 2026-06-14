using MusX;
using MusX.Objects;
using System.Collections.Generic;

namespace sb_explorer
{
    public class LoadedProjectData
    {
        public SoundbankHeader SoundBankHeaderData = new SoundbankHeader();
        public readonly SortedDictionary<uint, Sample> SfxSamples = new SortedDictionary<uint, Sample>();
        public readonly List<SampleData> SfxStoredData = new List<SampleData>();
        public readonly List<uint> DuplicatedHashCodes = new List<uint>();

        public StreambankHeader StreamBankHeaderData = new StreambankHeader();
        public readonly List<StreamSample> StreamSamples = new List<StreamSample>();
        public StreambankHeader CommonStreamBankHeaderData = new StreambankHeader();
        public readonly List<StreamSample> CommonStreamSamples = new List<StreamSample>();
        public bool ActiveStreamBankIsCommon;

        public StreambankHeader MusicBankHeaderData = new StreambankHeader();
        public MusicSample MusicData = new MusicSample();

        public SoundbankInfoHeader SbiBankHeaderData = new SoundbankInfoHeader();
        public SbiFile SbiFileData = new SbiFile();

        public ProjectDetailsHeader ProjectDetailsHeaderData = new ProjectDetailsHeader();
        public ProjectDetails ProjectDetailsData = new ProjectDetails();

        public SfxCommonHeader SoundDetailsHeaderData = new SfxCommonHeader();
        public SoundDetails SoundDetails = new SoundDetails();

        public SfxCommonHeader MusicDetailsHeaderData = new SfxCommonHeader();
        public MusicDetails MusicDetails = new MusicDetails();

        public SfxCommonHeader MusicMarkersHeaderData = new SfxCommonHeader();
        public MusicMarkers MusicMarkers = new MusicMarkers();
    }
}
