using MusX.Objects;
using System;

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

        public static bool TryResolvePlayback(Marker[] markers, int totalSamples, MarkerLoopMode mode,
            out int startPosition, out uint loopStart, out int loopEndExclusive)
        {
            startPosition = 0;
            loopStart = 0;
            loopEndExclusive = 0;
            if (totalSamples <= 0) return false;

            uint available = (uint)totalSamples;
            startPosition = (int)Math.Min(GetStartPosition(markers), available - 1);
            if (!IsLooped(markers, mode)) return false;

            Marker loopMarker = FindLoopMarker(markers);
            loopStart = loopMarker == null ? 0 : Math.Min(loopMarker.LoopStart, available);
            uint end = loopMarker == null || loopMarker.Position == 0
                ? available
                : Math.Min(available, loopMarker.Position == uint.MaxValue ? uint.MaxValue : loopMarker.Position + 1);
            loopEndExclusive = end > int.MaxValue ? int.MaxValue : (int)end;
            return loopStart < end;
        }

        public static WavLoopInfo CreateLoopInfo(Marker[] markers, int totalSamples, MarkerLoopMode mode)
        {
            if (!IsLooped(markers, mode) || totalSamples <= 0)
            {
                return null;
            }

            if (!TryResolvePlayback(markers, totalSamples, mode, out int ignoredStart, out uint loopStart, out int loopEndExclusive))
                return null;
            return new WavLoopInfo(loopStart, (uint)(loopEndExclusive - 1));
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
