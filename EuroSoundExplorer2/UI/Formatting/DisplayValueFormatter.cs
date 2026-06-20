namespace sb_explorer.UI.Formatting
{
    internal static class DisplayValueFormatter
    {
        public static string FormatOffset(uint value, bool asHex)
        {
            return asHex ? "0x" + value.ToString("X8") : value.ToString();
        }

        public static string FormatSize(uint bytes, bool humanReadable)
        {
            if (!humanReadable)
            {
                return bytes.ToString();
            }

            if (bytes >= 1024 * 1024)
            {
                return (bytes / 1048576.0).ToString("0.##") + " MB";
            }

            if (bytes >= 1024)
            {
                return (bytes / 1024.0).ToString("0.##") + " KB";
            }

            return bytes + " B";
        }
    }
}
