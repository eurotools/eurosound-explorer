using MusX.Objects;

namespace sb_explorer.Services.Audio
{
    internal enum MarkerLoopMode
    {
        RequireLoopMarker,
        LoopUnlessEndMarker
    }

    internal static class EuroSoundMarkerLoopResolver
    {
        public static uint GetStartPosition(Marker[] markers)
        {
            Marker marker = FindStartMarker(markers);
            return marker == null ? 0 : marker.Position;
        }

        public static uint GetLoopStart(Marker[] markers)
        {
            Marker marker = FindLoopMarker(markers);
            return marker == null ? 0 : marker.LoopStart;
        }

        public static uint GetLoopEnd(Marker[] markers)
        {
            Marker marker = FindLoopMarker(markers);
            return marker == null ? 0 : marker.Position;
        }

        public static bool IsLooped(Marker[] markers, MarkerLoopMode mode)
        {
            if (markers == null || markers.Length == 0)
            {
                return false;
            }

            if (mode == MarkerLoopMode.RequireLoopMarker)
            {
                return FindLoopMarker(markers) != null;
            }

            return !HasEndMarker(markers);
        }

        public static WavLoopInfo CreateLoopInfo(Marker[] markers, int totalSamples, MarkerLoopMode mode)
        {
            if (!IsLooped(markers, mode) || totalSamples <= 0)
            {
                return null;
            }

            uint loopStart = 0;
            uint loopEnd = (uint)(totalSamples - 1);
            Marker loopMarker = FindLoopMarker(markers);
            if (loopMarker != null)
            {
                loopStart = loopMarker.LoopStart;
                if (loopMarker.Position > 0 && loopMarker.Position < loopEnd)
                {
                    loopEnd = loopMarker.Position;
                }
            }

            return loopStart < loopEnd ? new WavLoopInfo(loopStart, loopEnd) : null;
        }

        private static Marker FindStartMarker(Marker[] markers)
        {
            if (markers == null)
            {
                return null;
            }

            for (int i = 0; i < markers.Length; i++)
            {
                if (markers[i].Type == 10)
                {
                    return markers[i];
                }
            }

            return null;
        }

        private static Marker FindLoopMarker(Marker[] markers)
        {
            if (markers == null)
            {
                return null;
            }

            for (int i = 0; i < markers.Length; i++)
            {
                if (markers[i].Type == 7 || markers[i].Type == 6)
                {
                    return markers[i];
                }
            }

            return null;
        }

        private static bool HasEndMarker(Marker[] markers)
        {
            if (markers == null)
            {
                return false;
            }

            for (int i = 0; i < markers.Length; i++)
            {
                if (markers[i].Type == 9)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
