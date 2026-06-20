using MusX.Objects;
using System;
using System.IO;

namespace MusX
{
    public static class EuroSoundAudioDataReader
    {
        public static byte[] Read(AudioDataReference audioReference)
        {
            if (audioReference == null || string.IsNullOrEmpty(audioReference.FilePath) || audioReference.Size == 0)
            {
                return new byte[0];
            }

            using (FileStream stream = File.Open(audioReference.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                stream.Seek(audioReference.Offset, SeekOrigin.Begin);
                byte[] data = new byte[audioReference.Size];
                int bytesRead = 0;
                while (bytesRead < data.Length)
                {
                    int read = stream.Read(data, bytesRead, data.Length - bytesRead);
                    if (read == 0)
                    {
                        break;
                    }

                    bytesRead += read;
                }

                if (bytesRead == data.Length)
                {
                    return data;
                }

                byte[] trimmed = new byte[bytesRead];
                Buffer.BlockCopy(data, 0, trimmed, 0, bytesRead);
                return trimmed;
            }
        }

        public static byte[][] ReadInterleavedStereo(AudioDataReference audioReference)
        {
            byte[] interleavedData = Read(audioReference);
            byte[][] channels = new byte[2][];
            channels[0] = new byte[interleavedData.Length / 2];
            channels[1] = new byte[interleavedData.Length / 2];

            int blockSize = audioReference == null || audioReference.InterleaveBlockSize <= 0
                ? 1
                : audioReference.InterleaveBlockSize;
            int leftIndex = 0;
            int rightIndex = 0;
            bool leftChannel = true;

            for (int offset = 0; offset < interleavedData.Length;)
            {
                int bytesToCopy = Math.Min(blockSize, interleavedData.Length - offset);
                int bytesReadFromSource = bytesToCopy;
                if (leftChannel)
                {
                    bytesToCopy = Math.Min(bytesToCopy, channels[0].Length - leftIndex);
                    Buffer.BlockCopy(interleavedData, offset, channels[0], leftIndex, bytesToCopy);
                    leftIndex += bytesToCopy;
                }
                else
                {
                    bytesToCopy = Math.Min(bytesToCopy, channels[1].Length - rightIndex);
                    Buffer.BlockCopy(interleavedData, offset, channels[1], rightIndex, bytesToCopy);
                    rightIndex += bytesToCopy;
                }

                offset += bytesReadFromSource;
                leftChannel = !leftChannel;
            }

            return channels;
        }
    }
}
