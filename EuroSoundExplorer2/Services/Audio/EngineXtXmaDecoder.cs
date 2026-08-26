using MusX.Objects;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace sb_explorer.Services.Audio
{
    internal static class EngineXtXmaDecoder
    {
        private const int XmaPacketSize = 2048;

        //-------------------------------------------------------------------------------------------------------------------------------
        internal static DecodedAudio Decode(byte[] encodedData, int channelCount, uint sampleRate, uint sampleCount)
        {
            if (encodedData == null) { throw new ArgumentNullException(nameof(encodedData)); }
            if (channelCount < 1 || channelCount > 8) { throw new InvalidDataException("XMA channel count is invalid: " + channelCount); }
            if (sampleRate == 0) { throw new InvalidDataException("XMA sample rate is missing."); }

            string ffmpegPath = FindFfmpeg();

            string temporaryFolder = Path.Combine(Path.GetTempPath(), "EuroSoundExplorer", Guid.NewGuid().ToString("N"));
            string inputPath = Path.Combine(temporaryFolder, "engine_xt.xma.wav");
            string outputPath = Path.Combine(temporaryFolder, "decoded.pcm");

            Directory.CreateDirectory(temporaryFolder);

            try
            {
                WriteXmaWave(inputPath, encodedData, channelCount, sampleRate, sampleCount);
                RunFfmpeg(ffmpegPath, inputPath, outputPath);

                byte[] interleavedPcm = File.ReadAllBytes(outputPath);
                byte[][] channels = DeinterleavePcm16(interleavedPcm, channelCount, sampleCount);

                uint decodedSampleCount = channels.Length == 0
                    ? 0
                    : checked((uint)(channels[0].Length / sizeof(short)));

                return new DecodedAudio
                {
                    Channels = channels,
                    SampleRate = sampleRate,
                    SampleCount = sampleCount == 0 ? decodedSampleCount : Math.Min(sampleCount, decodedSampleCount)
                };
            }
            finally
            {
                TryDeleteFile(inputPath);
                TryDeleteFile(outputPath);
                TryDeleteDirectory(temporaryFolder);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static void WriteXmaWave(string outputPath, byte[] encodedData, int channelCount, uint sampleRate, uint sampleCount)
        {
            int packetCount = (encodedData.Length + XmaPacketSize - 1) / XmaPacketSize;
            int paddedDataLength = checked(packetCount * XmaPacketSize);
            const int FormatChunkSize = 52;
            int riffSize = checked(4 + 8 + FormatChunkSize + 8 + paddedDataLength);

            using (FileStream stream = File.Create(outputPath))
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII))
            {
                writer.Write(Encoding.ASCII.GetBytes("RIFF"));
                writer.Write(riffSize);
                writer.Write(Encoding.ASCII.GetBytes("WAVE"));
                writer.Write(Encoding.ASCII.GetBytes("fmt "));
                writer.Write(FormatChunkSize);

                writer.Write((ushort)0x0166);
                writer.Write((ushort)1);
                writer.Write(sampleRate);
                writer.Write(CalculateAverageBytesPerSecond(sampleRate, channelCount));
                writer.Write((ushort)XmaPacketSize);
                writer.Write((ushort)16);
                writer.Write((ushort)34);

                writer.Write((ushort)channelCount);
                writer.Write(BuildChannelMask(channelCount));
                writer.Write(sampleCount);
                writer.Write((uint)XmaPacketSize);
                writer.Write(0u);
                writer.Write(sampleCount);
                writer.Write(0u);
                writer.Write(0u);
                writer.Write((byte)0);
                writer.Write((byte)4);
                writer.Write((ushort)packetCount);

                writer.Write(Encoding.ASCII.GetBytes("data"));
                writer.Write(paddedDataLength);
                byte[] normalizedData = NormalizeEngineXtPackets(encodedData);
                writer.Write(normalizedData);

                for (int index = encodedData.Length; index < paddedDataLength; index++)
                {
                    writer.Write((byte)0);
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static byte[] NormalizeEngineXtPackets(byte[] encodedData)
        {
            byte[] normalizedData = (byte[])encodedData.Clone();

            for (int packetOffset = 0; packetOffset + 3 < normalizedData.Length; packetOffset += XmaPacketSize)
            {
                // EngineXT stores its own first/last-packet flags in these bits.
                // The Xbox runtime clears them before submitting a packet to the
                // hardware XMA decoder, so the desktop decoder must do the same.
                normalizedData[packetOffset + 2] &= 0xF9;
                normalizedData[packetOffset + 3] = 0;
            }

            return normalizedData;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static uint CalculateAverageBytesPerSecond(uint sampleRate, int channelCount)
        {
            ulong value = (ulong)sampleRate * (uint)channelCount * XmaPacketSize;
            return checked((uint)Math.Max(1UL, value / 512UL));
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static uint BuildChannelMask(int channelCount)
        {
            if (channelCount >= 32) { return uint.MaxValue; }

            return (1u << channelCount) - 1u;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static void RunFfmpeg(string executable, string inputPath, string outputPath)
        {
            string arguments = "-y -v error -i " + Quote(inputPath) + " -f s16le -acodec pcm_s16le " + Quote(outputPath);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = executable,
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            try
            {
                using (Process process = Process.Start(startInfo))
                {
                    string errorText = process.StandardError.ReadToEnd();
                    process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode != 0 || !File.Exists(outputPath))
                    {
                        throw new InvalidDataException("FFmpeg could not decode the EngineXT XMA stream." + Environment.NewLine + errorText.Trim());
                    }
                }
            }
            catch (System.ComponentModel.Win32Exception exception)
            {
                throw new InvalidOperationException("XMA decoding requires ffmpeg.exe in the application folder or in PATH.", exception);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static string FindFfmpeg()
        {
            string applicationFolder = AppDomain.CurrentDomain.BaseDirectory;
            string[] bundledCandidates =
            {
                Path.Combine(applicationFolder, "ffmpeg.exe"),
                Path.Combine(applicationFolder, "SystemFiles", "ffmpeg.exe")
            };

            foreach (string candidate in bundledCandidates)
            {
                if (File.Exists(candidate)) { return candidate; }
            }

            string pathValue = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;

            foreach (string folder in pathValue.Split(Path.PathSeparator))
            {
                if (string.IsNullOrWhiteSpace(folder)) { continue; }

                string candidate;

                try
                {
                    candidate = Path.Combine(folder.Trim(), "ffmpeg.exe");
                }
                catch (ArgumentException)
                {
                    continue;
                }

                if (File.Exists(candidate)) { return candidate; }
            }

            throw new InvalidOperationException(
                "FFmpeg was not found, so this XMA audio cannot be decoded." + Environment.NewLine + Environment.NewLine +
                "Place ffmpeg.exe next to sb_explorer.exe, inside its SystemFiles folder, or add FFmpeg to PATH.");
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static byte[][] DeinterleavePcm16(byte[] interleaved, int channelCount, uint requestedSampleCount)
        {
            int frameCount = interleaved.Length / (sizeof(short) * channelCount);

            if (requestedSampleCount != 0 && requestedSampleCount < frameCount)
            {
                frameCount = checked((int)requestedSampleCount);
            }

            byte[][] channels = new byte[channelCount][];

            for (int channelIndex = 0; channelIndex < channelCount; channelIndex++)
            {
                channels[channelIndex] = new byte[frameCount * sizeof(short)];
            }

            for (int frameIndex = 0; frameIndex < frameCount; frameIndex++)
            {
                int sourceFrameOffset = frameIndex * channelCount * sizeof(short);
                int destinationOffset = frameIndex * sizeof(short);

                for (int channelIndex = 0; channelIndex < channelCount; channelIndex++)
                {
                    Buffer.BlockCopy(interleaved, sourceFrameOffset + channelIndex * sizeof(short), channels[channelIndex], destinationOffset, sizeof(short));
                }
            }

            return channels;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static string Quote(string value)
        {
            return "\"" + value.Replace("\"", "\\\"") + "\"";
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static void TryDeleteFile(string path)
        {
            try
            {
                if (File.Exists(path)) { File.Delete(path); }
            }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private static void TryDeleteDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path)) { Directory.Delete(path, false); }
            }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
        }
    }
}
