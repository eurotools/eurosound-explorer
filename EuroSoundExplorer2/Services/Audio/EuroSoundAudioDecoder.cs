using AudioDecoders;
using MusX;
using MusX.Objects;
using sb_explorer.Classes;

namespace sb_explorer.Services.Audio
{
    internal static class EuroSoundAudioDecoder
    {
        public static byte[] Decode(EuroSoundAudioCodec codec, byte[] encodedData, AudioFunctions audioFunctions, short[] dspCoeffs, SampleData selectedSample)
        {
            if (encodedData == null)
            {
                return null;
            }

            switch (codec)
            {
                case EuroSoundAudioCodec.Pcm16:
                    return encodedData;

                case EuroSoundAudioCodec.ImaAdpcm:
                    ImaAdpcm imaFile = new ImaAdpcm();
                    return audioFunctions.ShortArrayToByteArray(imaFile.Decode(encodedData, encodedData.Length * 2));

                case EuroSoundAudioCodec.EurocomImaAdpcm:
                    Eurocom_ImaAdpcm eurocomDAT = new Eurocom_ImaAdpcm();
                    return audioFunctions.ShortArrayToByteArray(eurocomDAT.Decode(encodedData));

                case EuroSoundAudioCodec.SonyVagAdpcm:
                    SonyAdpcm vagDecoder = new SonyAdpcm();
                    uint vagLoopStartOffset = selectedSample == null ? 0 : uint.MaxValue;
                    byte[] decodedData = vagDecoder.Decode(encodedData, ref vagLoopStartOffset);
                    if (selectedSample != null && selectedSample.IsLooped && vagLoopStartOffset != uint.MaxValue)
                    {
                        selectedSample.LoopStartOffset = vagLoopStartOffset / 2;
                        selectedSample.LoopStartSample = selectedSample.LoopStartOffset;
                    }
                    return decodedData;

                case EuroSoundAudioCodec.DspAdpcm:
                    DspAdpcm gcDecoder = new DspAdpcm();
                    return audioFunctions.ShortArrayToByteArray(gcDecoder.Decode(encodedData, dspCoeffs));

                case EuroSoundAudioCodec.XboxAdpcm:
                    XboxAdpcm xboxDecoder = new XboxAdpcm();
                    return audioFunctions.ShortArrayToByteArray(xboxDecoder.Decode(encodedData));

                default:
                    return null;
            }
        }
    }
}
