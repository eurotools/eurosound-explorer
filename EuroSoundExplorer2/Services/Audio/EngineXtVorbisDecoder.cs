using NVorbis;
using System;
using System.Collections.Generic;
using System.IO;

namespace sb_explorer.Services.Audio
{
    internal static class EngineXtVorbisDecoder
    {
        internal static DecodedAudio Decode(byte[] encodedData, uint declaredSampleCount)
        {
            using (MemoryStream ogg = RebuildOggStream(encodedData))
            using (VorbisReader reader = new VorbisReader(ogg, false))
            {
                int channelCount = reader.Channels;
                List<byte>[] pcm = new List<byte>[channelCount];
                for (int channel = 0; channel < channelCount; channel++) pcm[channel] = new List<byte>();

                float[] buffer = new float[4096 * channelCount];
                int read;
                while ((read = reader.ReadSamples(buffer, 0, buffer.Length)) > 0)
                {
                    int complete = read - read % channelCount;
                    for (int index = 0; index < complete; index++)
                    {
                        short sample = FloatToPcm16(buffer[index]);
                        List<byte> destination = pcm[index % channelCount];
                        destination.Add((byte)sample);
                        destination.Add((byte)(sample >> 8));
                    }
                }

                byte[][] channels = new byte[channelCount][];
                uint decodedSamples = 0;
                for (int channel = 0; channel < channelCount; channel++)
                {
                    channels[channel] = pcm[channel].ToArray();
                    decodedSamples = Math.Max(decodedSamples, (uint)(channels[channel].Length / 2));
                }
                uint sampleCount = declaredSampleCount == 0 ? decodedSamples : Math.Min(declaredSampleCount, decodedSamples);
                return new DecodedAudio { Channels = channels, SampleRate = (uint)reader.SampleRate, SampleCount = sampleCount };
            }
        }

        internal static byte[] DecodeInterleaved(byte[] encodedData)
        {
            DecodedAudio decoded = Decode(encodedData, 0);
            if (decoded.Channels.Length == 0) return new byte[0];
            int samples = decoded.Channels[0].Length / 2;
            byte[] result = new byte[checked(samples * decoded.Channels.Length * 2)];
            for (int sample = 0; sample < samples; sample++)
                for (int channel = 0; channel < decoded.Channels.Length; channel++)
                    Buffer.BlockCopy(decoded.Channels[channel], sample * 2, result, (sample * decoded.Channels.Length + channel) * 2, 2);
            return result;
        }

        internal static MemoryStream RebuildOggStream(byte[] source)
        {
            if (source == null || source.Length < 28) throw new InvalidDataException("EngineXT Vorbis data is truncated.");
            MemoryStream output = new MemoryStream(source.Length);
            int position = 0;
            uint sequence = 0;
            uint serial = ReadUInt32(source, 14);
            while (position <= source.Length - 27)
            {
                int segments = source[position + 26];
                int headerLength = 27 + segments;
                if (position > source.Length - headerLength) throw new InvalidDataException("EngineXT Vorbis page table is truncated.");
                int payloadLength = 0;
                for (int i = 0; i < segments; i++) payloadLength += source[position + 27 + i];
                int pageLength = checked(headerLength + payloadLength);
                if (source[position] != 0x4f || source[position + 1] != headerLength ||
                    ((source[position + 2] << 8) | source[position + 3]) != payloadLength ||
                    ReadUInt32(source, position + 14) != serial || ReadUInt32(source, position + 18) != sequence ||
                    position > source.Length - pageLength)
                    throw new InvalidDataException("EngineXT Vorbis page metadata is invalid.");

                byte[] page = new byte[pageLength];
                Buffer.BlockCopy(source, position, page, 0, pageLength);
                page[0] = (byte)'O'; page[1] = (byte)'g'; page[2] = (byte)'g'; page[3] = (byte)'S';
                page[22] = page[23] = page[24] = page[25] = 0;
                uint checksum = OggChecksum(page);
                page[22] = (byte)checksum; page[23] = (byte)(checksum >> 8); page[24] = (byte)(checksum >> 16); page[25] = (byte)(checksum >> 24);
                output.Write(page, 0, page.Length);
                sequence++;
                if ((page[5] & 4) != 0) break;

                int next = FindNextPage(source, position + pageLength, serial, sequence);
                if (next < 0) throw new InvalidDataException("EngineXT Vorbis stream is missing page " + sequence + ".");
                position = next;
            }
            output.Position = 0;
            return output;
        }

        private static int FindNextPage(byte[] source, int start, uint serial, uint sequence)
        {
            for (int position = start; position <= source.Length - 27; position++)
            {
                int segments = source[position + 26];
                int headerLength = 27 + segments;
                if (source[position] != 0x4f || source[position + 1] != headerLength || source[position + 4] != 0 ||
                    ReadUInt32(source, position + 14) != serial || ReadUInt32(source, position + 18) != sequence) continue;
                int payloadLength = (source[position + 2] << 8) | source[position + 3];
                int lacingLength = 0;
                if (position > source.Length - headerLength) continue;
                for (int i = 0; i < segments; i++) lacingLength += source[position + 27 + i];
                if (payloadLength == lacingLength && position <= source.Length - headerLength - payloadLength) return position;
            }
            return -1;
        }

        private static uint OggChecksum(byte[] page)
        {
            uint checksum = 0;
            for (int i = 0; i < page.Length; i++)
            {
                checksum ^= (uint)page[i] << 24;
                for (int bit = 0; bit < 8; bit++) checksum = (checksum & 0x80000000) != 0 ? (checksum << 1) ^ 0x04c11db7 : checksum << 1;
            }
            return checksum;
        }

        private static uint ReadUInt32(byte[] data, int offset)
        {
            return (uint)(data[offset] | data[offset + 1] << 8 | data[offset + 2] << 16 | data[offset + 3] << 24);
        }

        private static short FloatToPcm16(float value)
        {
            if (value >= 1f) return short.MaxValue;
            if (value <= -1f) return short.MinValue;
            return (short)Math.Round(value * 32767f);
        }
    }
}
