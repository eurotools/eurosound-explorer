namespace sb_explorer
{
    public enum EuroSoundVersion
    {
        Unknown = 0,
        EuroSound357 = 357,
        EuroSound450 = 450,
        EuroSound510 = 510,
        EuroSound610 = 610
    }

    public static class EuroSoundVersions
    {
        public static readonly int[] SupportedFileVersions =
        {
            1,
            4,
            5,
            6,
            201
        };

        public static EuroSoundVersion FromFileVersion(int fileVersion)
        {
            switch (fileVersion)
            {
                case 201:
                case 1:
                    return EuroSoundVersion.EuroSound357;
                case 4:
                    return EuroSoundVersion.EuroSound450;
                case 5:
                    return EuroSoundVersion.EuroSound510;
                case 6:
                    return EuroSoundVersion.EuroSound610;
                default:
                    return EuroSoundVersion.Unknown;
            }
        }
    }
}
