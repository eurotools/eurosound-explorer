using System;

namespace MusX
{
    //-------------------------------------------------------------------------------------------------------------------------------
    public enum EuroSoundAudioCodec
    {
        Unknown,
        Pcm16,
        ImaAdpcm,
        EurocomImaAdpcm,
        SonyVagAdpcm,
        DspAdpcm,
        XboxAdpcm
    }

    //-------------------------------------------------------------------------------------------------------------------------------
    public enum EuroSoundBankType
    {
        SoundBank,
        StreamBank,
        MusicBank
    }

    //-------------------------------------------------------------------------------------------------------------------------------
    public static class EuroSoundCodecMatrix
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        private enum EuroSoundPlatformGroup
        {
            Any,
            Pc,
            Ps2,
            GameCube,
            Xbox
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private sealed class CodecRule
        {
            public readonly int[] FileVersions;
            public readonly EuroSoundBankType BankType;
            public readonly EuroSoundPlatformGroup Platform;
            public readonly EuroSoundAudioCodec Codec;

            public CodecRule(int[] fileVersions, EuroSoundBankType bankType, EuroSoundPlatformGroup platform, EuroSoundAudioCodec codec)
            {
                FileVersions = fileVersions;
                BankType = bankType;
                Platform = platform;
                Codec = codec;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static readonly int[] Version201 = { 1, 201 };
        private static readonly int[] EurocomVersions = { 4, 5, 6 };

        //-------------------------------------------------------------------------------------------------------------------------------
        private static readonly CodecRule[] Rules =
        {
            // EuroSound 357 / file version 201
            new CodecRule(Version201, EuroSoundBankType.SoundBank, EuroSoundPlatformGroup.Pc, EuroSoundAudioCodec.Pcm16),
            new CodecRule(Version201, EuroSoundBankType.StreamBank, EuroSoundPlatformGroup.Pc, EuroSoundAudioCodec.ImaAdpcm),
            new CodecRule(Version201, EuroSoundBankType.MusicBank, EuroSoundPlatformGroup.Pc, EuroSoundAudioCodec.ImaAdpcm),

            new CodecRule(Version201, EuroSoundBankType.SoundBank, EuroSoundPlatformGroup.Ps2, EuroSoundAudioCodec.SonyVagAdpcm),
            new CodecRule(Version201, EuroSoundBankType.StreamBank, EuroSoundPlatformGroup.Ps2, EuroSoundAudioCodec.SonyVagAdpcm),
            new CodecRule(Version201, EuroSoundBankType.MusicBank, EuroSoundPlatformGroup.Ps2, EuroSoundAudioCodec.SonyVagAdpcm),

            new CodecRule(Version201, EuroSoundBankType.SoundBank, EuroSoundPlatformGroup.GameCube, EuroSoundAudioCodec.DspAdpcm),
            new CodecRule(Version201, EuroSoundBankType.StreamBank, EuroSoundPlatformGroup.GameCube, EuroSoundAudioCodec.ImaAdpcm),
            new CodecRule(Version201, EuroSoundBankType.MusicBank, EuroSoundPlatformGroup.GameCube, EuroSoundAudioCodec.ImaAdpcm),

            new CodecRule(Version201, EuroSoundBankType.SoundBank, EuroSoundPlatformGroup.Xbox, EuroSoundAudioCodec.XboxAdpcm),
            new CodecRule(Version201, EuroSoundBankType.StreamBank, EuroSoundPlatformGroup.Xbox, EuroSoundAudioCodec.XboxAdpcm),
            new CodecRule(Version201, EuroSoundBankType.MusicBank, EuroSoundPlatformGroup.Xbox, EuroSoundAudioCodec.XboxAdpcm),

            // EuroSound 450/510/650 / file versions 4/5/6
            new CodecRule(EurocomVersions, EuroSoundBankType.SoundBank, EuroSoundPlatformGroup.Pc, EuroSoundAudioCodec.EurocomImaAdpcm),
            new CodecRule(EurocomVersions, EuroSoundBankType.StreamBank, EuroSoundPlatformGroup.Pc, EuroSoundAudioCodec.EurocomImaAdpcm),
            new CodecRule(EurocomVersions, EuroSoundBankType.MusicBank, EuroSoundPlatformGroup.Pc, EuroSoundAudioCodec.EurocomImaAdpcm),

            new CodecRule(EurocomVersions, EuroSoundBankType.SoundBank, EuroSoundPlatformGroup.Ps2, EuroSoundAudioCodec.SonyVagAdpcm),
            new CodecRule(EurocomVersions, EuroSoundBankType.StreamBank, EuroSoundPlatformGroup.Ps2, EuroSoundAudioCodec.SonyVagAdpcm),
            new CodecRule(EurocomVersions, EuroSoundBankType.MusicBank, EuroSoundPlatformGroup.Ps2, EuroSoundAudioCodec.SonyVagAdpcm),

            new CodecRule(EurocomVersions, EuroSoundBankType.SoundBank, EuroSoundPlatformGroup.GameCube, EuroSoundAudioCodec.EurocomImaAdpcm),
            new CodecRule(EurocomVersions, EuroSoundBankType.StreamBank, EuroSoundPlatformGroup.GameCube, EuroSoundAudioCodec.EurocomImaAdpcm),
            new CodecRule(EurocomVersions, EuroSoundBankType.MusicBank, EuroSoundPlatformGroup.GameCube, EuroSoundAudioCodec.EurocomImaAdpcm),

            new CodecRule(EurocomVersions, EuroSoundBankType.SoundBank, EuroSoundPlatformGroup.Xbox, EuroSoundAudioCodec.EurocomImaAdpcm),
            new CodecRule(EurocomVersions, EuroSoundBankType.StreamBank, EuroSoundPlatformGroup.Xbox, EuroSoundAudioCodec.EurocomImaAdpcm),
            new CodecRule(EurocomVersions, EuroSoundBankType.MusicBank, EuroSoundPlatformGroup.Xbox, EuroSoundAudioCodec.EurocomImaAdpcm),
        };

        //-------------------------------------------------------------------------------------------------------------------------------
        public static EuroSoundAudioCodec GetCodec(int fileVersion, string platform, EuroSoundBankType bankType)
        {
            EuroSoundPlatformGroup platformGroup = GetPlatformGroup(platform);
            foreach (CodecRule rule in Rules)
            {
                if (rule.BankType == bankType &&
                    rule.Platform == platformGroup &&
                    ContainsVersion(rule.FileVersions, fileVersion))
                {
                    return rule.Codec;
                }
            }

            return EuroSoundAudioCodec.Unknown;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint EncodedByteCountToSamples(EuroSoundAudioCodec codec, uint bytes, int channels)
        {
            switch (codec)
            {
                case EuroSoundAudioCodec.ImaAdpcm:
                    return CalculusLoopOffsets.ImaAdpcmToSamples(bytes, channels);
                case EuroSoundAudioCodec.EurocomImaAdpcm:
                    return CalculusLoopOffsets.EurocomImaToSamples(bytes, channels);
                case EuroSoundAudioCodec.SonyVagAdpcm:
                    return CalculusLoopOffsets.SonyVagToSamples(bytes, channels);
                case EuroSoundAudioCodec.DspAdpcm:
                    return CalculusLoopOffsets.DspAdpcmToSamples(bytes, channels);
                case EuroSoundAudioCodec.XboxAdpcm:
                    return CalculusLoopOffsets.XboxAdpcmToSamples(bytes, channels);
                case EuroSoundAudioCodec.Pcm16:
                    return CalculusLoopOffsets.Pcm16BytesToSamples(bytes, channels);
                default:
                    return 0;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint SoundBankLoopOffsetToSamples(EuroSoundAudioCodec codec, uint offset, int channels)
        {
            switch (codec)
            {
                case EuroSoundAudioCodec.EurocomImaAdpcm:
                    return CalculusLoopOffsets.EurocomImaToSamples(offset, channels);
                case EuroSoundAudioCodec.XboxAdpcm:
                    return CalculusLoopOffsets.XboxAdpcmToSamples(offset, channels);
                case EuroSoundAudioCodec.Pcm16:
                case EuroSoundAudioCodec.SonyVagAdpcm:
                case EuroSoundAudioCodec.DspAdpcm:
                    return CalculusLoopOffsets.Pcm16BytesToSamples(offset, channels);
                case EuroSoundAudioCodec.ImaAdpcm:
                    return CalculusLoopOffsets.Pcm16BytesToSamples(offset, channels);
                default:
                    return 0;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static uint StreamMarkerOffsetToSamples(EuroSoundAudioCodec codec, uint offset, int channels)
        {
            switch (codec)
            {
                case EuroSoundAudioCodec.ImaAdpcm:
                case EuroSoundAudioCodec.Pcm16:
                    return CalculusLoopOffsets.Pcm16BytesToSamples(offset, channels);
                default:
                    return EncodedByteCountToSamples(codec, offset, channels);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static bool IsPcPlatform(string platform)
        {
            return ContainsPlatform(platform, "PC");
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static bool IsPs2Platform(string platform)
        {
            return ContainsPlatform(platform, "PS2");
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static bool IsXboxPlatform(string platform)
        {
            return ContainsPlatform(platform, "XB") || ContainsPlatform(platform, "Xbox");
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public static bool IsGameCubePlatform(string platform)
        {
            return ContainsPlatform(platform, "GC") ||
                ContainsPlatform(platform, "Ga") ||
                ContainsPlatform(platform, "GameCube") ||
                ContainsPlatform(platform, "wii");
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static EuroSoundPlatformGroup GetPlatformGroup(string platform)
        {
            if (IsPcPlatform(platform))
            {
                return EuroSoundPlatformGroup.Pc;
            }

            if (IsPs2Platform(platform))
            {
                return EuroSoundPlatformGroup.Ps2;
            }

            if (IsXboxPlatform(platform))
            {
                return EuroSoundPlatformGroup.Xbox;
            }

            if (IsGameCubePlatform(platform))
            {
                return EuroSoundPlatformGroup.GameCube;
            }

            return EuroSoundPlatformGroup.Any;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static bool ContainsVersion(int[] fileVersions, int fileVersion)
        {
            for (int i = 0; i < fileVersions.Length; i++)
            {
                if (fileVersions[i] == fileVersion)
                {
                    return true;
                }
            }

            return false;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static bool ContainsPlatform(string platform, string value)
        {
            return !string.IsNullOrEmpty(platform) &&
                platform.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
