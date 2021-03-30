using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHSwarm.Native.Common;
using HHSwarm.Native.Shared;
using HHSwarm.Native.GameResources;

namespace HHSwarm.Native.Protocols.Hafen
{
    public class MessageBinaryReader : BinaryReader, IDisposable, IMessageBinaryReader
    {
        public long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

        public long Length => BaseStream.Length;

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

        /// <summary>
        /// Считывает 'C-style' строку из потока.
        /// (это когда длина не указывается в начале, а отмечается ноликом '\0')
        /// Так же останавливается если достиг указанный в <paramref name="maxLength"/> лимит длины. Такое надо если строковый ресурс был последним в потоке - завершающий ноль не всегда присутствует.
        /// Рекомендуется использовать этот метод вместо <see cref="ReadString()"/> во избежание проблем с неправильно заполненным потоком.
        /// </summary>
        /// <param name="maxLength">Максимальная длина данных строки, в байтах.</param>
        /// <returns></returns>
        public string ReadString(int maxLength)
        {
            maxLength = Math.Min(maxLength, (int)(Length - Position)); // коррекция на случай если была указана большая константа "с запасом"
            long maxPosition = Position + maxLength; // неизвестно в какой кодировке (длины) символы, поэтому учитываем только байты

            StringBuilder str = new StringBuilder();
            char c;
            while (Position < maxPosition && '\0' != (c = base.ReadChar())) str.Append(c);
            return str.ToString();
        }

        /// <inheritdoc/>
        public override string ReadString()
        {            
            return ReadString((int)(Length - Position)); // предохранитель на крайний случай если завершающий ноль '\0' отсутствует
        }

        public override byte[] ReadBytes(int count)
        {
            return base.ReadBytes(Math.Min(count, (int)(Length - Position)));
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

        public ushort ReadUInt16BigEndian()
        {
            return ((BinaryReader)this).ReadUInt16BigEndian();
        }

        public short ReadInt16BigEndian()
        {
            return ((BinaryReader)this).ReadInt16BigEndian();
        }

        public uint ReadUInt32BigEndian()
        {
            return ((BinaryReader)this).ReadUInt32BigEndian();
        }

        public int ReadInt32BigEndian()
        {
            return ((BinaryReader)this).ReadInt32BigEndian();
        }

        public ulong ReadUInt64BigEndian()
        {
            return ((BinaryReader)this).ReadUInt64BigEndian();
        }

        public long ReadInt64BigEndian()
        {
            return ((BinaryReader)this).ReadInt64BigEndian();
        }

        public Color ReadColor()
        {
            return ((BinaryReader)this).ReadColor();
        }

        public Coord2i ReadCoord2i()
        {
            return ((BinaryReader)this).ReadCoord2i();
        }

        public Coord2f ReadCoord2f()
        {
            return ((BinaryReader)this).ReadCoord2f();
        }

        public Coord2d ReadCoord2d()
        {
            return ((BinaryReader)this).ReadCoord2d();
        }

        public Coord3i ReadCoord3i()
        {
            return ((BinaryReader)this).ReadCoord3i();
        }

        public Coord3f ReadCoord3f()
        {
            return ((BinaryReader)this).ReadCoord3f();
        }

        public Coord3d ReadCoord3d()
        {
            return ((BinaryReader)this).ReadCoord3d();
        }

        public double ReadDouble20bit()
        {
            return ((BinaryReader)this).ReadDouble20bit();
        }

        public float ReadSingle16bit()
        {
            return ((BinaryReader)this).ReadSingle16bit();
        }

        public IEnumerable<object> ReadList()
        {
            return ((BinaryReader)this).ReadList();
        }

        public Coord3f ReadCoord3f32bit()
        {
            return ((BinaryReader)this).ReadCoord3f32bit();
        }

        public Coord3f ReadCoord3f20bit()
        {
            return ((BinaryReader)this).ReadCoord3f20bit();
        }

        public Color ReadColor20bit()
        {
            return ((BinaryReader)this).ReadColor20bit();
        }

        public Coord3f ReadCoord3f16bit()
        {
            return ((BinaryReader)this).ReadCoord3f16bit();
        }
    }
}
