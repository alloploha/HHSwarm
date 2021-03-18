using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.v17
{
    public class MessageBinaryReader : BinaryReader, IDisposable
    {
        private bool BigEndian = false;

        private void Initialize(bool bigEndian)
        {
            this.BigEndian = bigEndian;
        }

        public MessageBinaryReader(Stream input, bool bigEndian = false) : base(input)
        {
            Initialize(bigEndian);
        }

        public MessageBinaryReader(Stream input, Encoding encoding, bool bigEndian = false) : base(input, encoding)
        {
            Initialize(bigEndian);
        }

        public MessageBinaryReader(Stream input, Encoding encoding, bool leaveOpen, bool bigEndian = false) : base(input, encoding, leaveOpen)
        {
            Initialize(bigEndian);
        }

        public override string ReadString()
        {
            List<char> str = new List<char>();
            char c;
            while ('\0' != (c = base.ReadChar())) str.Add(c);
            return new string(str.ToArray());
        }

        public override byte[] ReadBytes(int count)
        {
            return base.ReadBytes(Math.Min(count, (int)(base.BaseStream.Length - base.BaseStream.Position)));
        }

        public override ushort ReadUInt16()
        {
            return BigEndian ? this.ReadUInt16BigEndian() : base.ReadUInt16();
        }

        public override short ReadInt16()
        {
            return BigEndian ? this.ReadInt16BigEndian() : base.ReadInt16();
        }

        public override uint ReadUInt32()
        {
            return BigEndian ? this.ReadUInt32BigEndian() : base.ReadUInt32();
        }

        public override int ReadInt32()
        {
            return BigEndian ? this.ReadInt32BigEndian() : base.ReadInt32();
        }

        public override ulong ReadUInt64()
        {
            return BigEndian ? this.ReadUInt64BigEndian() : base.ReadUInt64();
        }

        public override long ReadInt64()
        {
            return BigEndian ? this.ReadInt64BigEndian() : base.ReadInt64();
        }
    }
}
