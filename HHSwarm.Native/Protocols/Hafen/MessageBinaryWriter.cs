using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen
{
    public class MessageBinaryWriter : BinaryWriter, IDisposable
    {
        private bool BigEndian = false;

        private void Initialize(bool bigEndian)
        {
            this.BigEndian = bigEndian;
        }

        public MessageBinaryWriter(Stream output, bool bigEndian = false) : base(output)
        {
            Initialize(bigEndian);
        }

        public MessageBinaryWriter(Stream output, Encoding encoding, bool bigEndian = false) : base(output, encoding)
        {
            Initialize(bigEndian);
        }

        public MessageBinaryWriter(Stream output, Encoding encoding, bool leaveOpen, bool bigEndian = false) : base(output, encoding, leaveOpen)
        {
            Initialize(bigEndian);
        }

        public override void Write(string value)
        {
            base.Write(value.ToCharArray());
            base.Write('\0');
        }

        public override void Write(ushort value)
        {
            if (BigEndian)
                this.WriteBigEndian(value);
            else
                base.Write(value);
        }

        public override void Write(short value)
        {
            if (BigEndian)
                this.WriteBigEndian(value);
            else
                base.Write(value);
        }

        public override void Write(int value)
        {
            if (BigEndian)
                this.WriteBigEndian(value);
            else
                base.Write(value);
        }

        public override void Write(uint value)
        {
            if (BigEndian)
                this.WriteBigEndian(value);
            else
                base.Write(value);
        }

        public override void Write(long value)
        {
            if (BigEndian)
                this.WriteBigEndian(value);
            else
                base.Write(value);
        }

        public override void Write(ulong value)
        {
            if (BigEndian)
                this.WriteBigEndian(value);
            else
                base.Write(value);
        }
    }
}
