using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.IO;

namespace sb_explorer.Services.Audio
{
    internal static class EuroSoundWaveWriter
    {
        public static void WriteMonoPcm16(string filePath, byte[] pcmData, int sampleRate, WavLoopInfo loopInfo)
        {
            if (pcmData == null)
            {
                throw new ArgumentNullException("pcmData");
            }

            WritePcm16(filePath, pcmData, sampleRate, 1, loopInfo);
        }

        public static void WriteStereoPcm16(string filePath, byte[] leftPcmData, byte[] rightPcmData, int sampleRate, WavLoopInfo loopInfo)
        {
            if (leftPcmData == null)
            {
                throw new ArgumentNullException("leftPcmData");
            }
            if (rightPcmData == null)
            {
                throw new ArgumentNullException("rightPcmData");
            }

            int stereoBytes = Math.Min(leftPcmData.Length, rightPcmData.Length) * 2;
            byte[] interleavedData = new byte[stereoBytes];
            int outputOffset = 0;
            int channelBytes = stereoBytes / 2;
            for (int inputOffset = 0; inputOffset + 1 < channelBytes; inputOffset += 2)
            {
                interleavedData[outputOffset++] = leftPcmData[inputOffset];
                interleavedData[outputOffset++] = leftPcmData[inputOffset + 1];
                interleavedData[outputOffset++] = rightPcmData[inputOffset];
                interleavedData[outputOffset++] = rightPcmData[inputOffset + 1];
            }

            WritePcm16(filePath, interleavedData, sampleRate, 2, loopInfo);
        }

        public static void WriteSampleProvider16(string filePath, ISampleProvider sampleProvider, WavLoopInfo loopInfo)
        {
            if (sampleProvider == null)
            {
                throw new ArgumentNullException("sampleProvider");
            }

            byte[] pcmData = RenderSampleProviderToPcm16(sampleProvider);
            WritePcm16(filePath, pcmData, sampleProvider.WaveFormat.SampleRate, sampleProvider.WaveFormat.Channels, loopInfo);
        }

        public static WavLoopInfo CreateLoopInfo(bool isLooped, uint loopStartSample, long pcmByteLength, int channels)
        {
            if (!isLooped || channels <= 0)
            {
                return null;
            }

            uint totalSamples = (uint)(pcmByteLength / (channels * 2));
            if (totalSamples == 0 || loopStartSample >= totalSamples - 1)
            {
                return null;
            }

            return new WavLoopInfo(loopStartSample, totalSamples - 1);
        }

        private static byte[] RenderSampleProviderToPcm16(ISampleProvider sampleProvider)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                float[] sampleBuffer = new float[sampleProvider.WaveFormat.SampleRate * sampleProvider.WaveFormat.Channels];
                byte[] byteBuffer = new byte[sampleBuffer.Length * 2];
                int samplesRead;
                while ((samplesRead = sampleProvider.Read(sampleBuffer, 0, sampleBuffer.Length)) > 0)
                {
                    for (int i = 0; i < samplesRead; i++)
                    {
                        float sample = Math.Max(-1.0f, Math.Min(1.0f, sampleBuffer[i]));
                        short pcmSample = (short)(sample < 0 ? sample * 32768.0f : sample * 32767.0f);
                        byteBuffer[i * 2] = (byte)(pcmSample & 0xFF);
                        byteBuffer[(i * 2) + 1] = (byte)((pcmSample >> 8) & 0xFF);
                    }

                    memoryStream.Write(byteBuffer, 0, samplesRead * 2);
                }

                return memoryStream.ToArray();
            }
        }

        private static void WritePcm16(string filePath, byte[] pcmData, int sampleRate, int channels, WavLoopInfo loopInfo)
        {
            if (sampleRate <= 0)
            {
                throw new ArgumentOutOfRangeException("sampleRate");
            }
            if (channels <= 0)
            {
                throw new ArgumentOutOfRangeException("channels");
            }

            bool writeLoop = loopInfo != null && loopInfo.IsValid;
            int smplChunkSize = writeLoop ? 60 : 0;
            int riffDataSize = 4 + (8 + 16) + (8 + pcmData.Length) + (writeLoop ? 8 + smplChunkSize : 0);

            using (BinaryWriter writer = new BinaryWriter(File.Create(filePath)))
            {
                writer.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });
                writer.Write(riffDataSize);
                writer.Write(new byte[] { 0x57, 0x41, 0x56, 0x45 });

                writer.Write(new byte[] { 0x66, 0x6D, 0x74, 0x20 });
                writer.Write(16);
                writer.Write((short)1);
                writer.Write((short)channels);
                writer.Write(sampleRate);
                writer.Write(sampleRate * channels * 2);
                writer.Write((short)(channels * 2));
                writer.Write((short)16);

                writer.Write(new byte[] { 0x64, 0x61, 0x74, 0x61 });
                writer.Write(pcmData.Length);
                writer.Write(pcmData);

                if (writeLoop)
                {
                    WriteSmplChunk(writer, sampleRate, loopInfo);
                }
            }
        }

        private static void WriteSmplChunk(BinaryWriter writer, int sampleRate, WavLoopInfo loopInfo)
        {
            writer.Write(new byte[] { 0x73, 0x6D, 0x70, 0x6C });
            writer.Write(60);
            writer.Write(0u);
            writer.Write(0u);
            writer.Write((uint)(1000000000.0 / sampleRate));
            writer.Write(60u);
            writer.Write(0u);
            writer.Write(0u);
            writer.Write(0u);
            writer.Write(1u);
            writer.Write(0u);

            writer.Write(0u);
            writer.Write(0u);
            writer.Write(loopInfo.StartSample);
            writer.Write(loopInfo.EndSample);
            writer.Write(0u);
            writer.Write(0u);
        }
    }
}
