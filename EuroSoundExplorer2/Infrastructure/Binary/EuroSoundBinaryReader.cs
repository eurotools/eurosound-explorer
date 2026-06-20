using System;
using System.IO;

namespace MusX
{
    public sealed class EuroSoundBinaryReader : IDisposable
    {
        private readonly BinaryReader reader;
        private readonly bool isBigEndian;

        public EuroSoundBinaryReader(Stream input, bool isBigEndian)
        {
            reader = new BinaryReader(input);
            this.isBigEndian = isBigEndian;
        }

        public Stream BaseStream
        {
            get { return reader.BaseStream; }
        }

        public byte ReadByte()
        {
            return reader.ReadByte();
        }

        public sbyte ReadSByte()
        {
            return reader.ReadSByte();
        }

        public byte[] ReadBytes(int count)
        {
            return reader.ReadBytes(count);
        }

        public short ReadInt16()
        {
            return BytesFunctions.FlipData(reader.ReadInt16(), isBigEndian);
        }

        public ushort ReadUInt16()
        {
            return BytesFunctions.FlipData(reader.ReadUInt16(), isBigEndian);
        }

        public int ReadInt32()
        {
            return BytesFunctions.FlipData(reader.ReadInt32(), isBigEndian);
        }

        public uint ReadUInt32()
        {
            return BytesFunctions.FlipData(reader.ReadUInt32(), isBigEndian);
        }

        public uint ReadRawUInt32()
        {
            return reader.ReadUInt32();
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            reader.BaseStream.Seek(offset, origin);
        }

        public void Dispose()
        {
            reader.Dispose();
        }
    }
}
