using HHSwarm.Native.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace HHSwarm.Native.Shared
{
    class ArgumentsReader
    {
        private object[] Arguments;
        public int Index { get; private set; }
        public int Length => Arguments != null ? Arguments.Length : 0;

        public ArgumentsReader(object[] arguments)
        {
            this.Arguments = arguments;
        }

        public bool IsNext<T>()
        {
            return !ReachedEnd && Arguments[Index] is T;
        }

        public virtual bool ReadBoolean()
        {
            if (ReachedEnd) return default(bool);
            return Convert.ToBoolean(Arguments[Index++]);
        }

        public virtual object ReadObject()
        {
            if (ReachedEnd) return default(object);
            return Arguments[Index++];
        }

        public virtual object[] ReadObjects()
        {
            if (ReachedEnd) return default(object[]);
            return (object[])Arguments[Index++];
        }

        public virtual byte ReadByte()
        {
            if (ReachedEnd) return default(byte);
            return Convert.ToByte(Arguments[Index++]);
        }

        public virtual byte[] ReadBytes()
        {
            if (ReachedEnd) return default(byte[]);
            return (byte[])Arguments[Index++];
        }

        public virtual char ReadChar()
        {
            if (ReachedEnd) return default(char);
            return Convert.ToChar(Arguments[Index++]);
        }

        public virtual char[] ReadChars()
        {
            if (ReachedEnd) return default(char[]);
            return (char[])Arguments[Index++];
        }

        public virtual decimal ReadDecimal()
        {
            if (ReachedEnd) return default(decimal);
            return Convert.ToDecimal(Arguments[Index++]);
        }

        public virtual double ReadDouble()
        {
            if (ReachedEnd) return default(double);
            return Convert.ToDouble(Arguments[Index++]);
        }

        public virtual ushort ReadUInt16()
        {
            if (ReachedEnd) return default(ushort);
            return Convert.ToUInt16(Arguments[Index++]);
        }

        public virtual short ReadInt16()
        {
            if (ReachedEnd) return default(short);
            return Convert.ToInt16(Arguments[Index++]);
        }

        public virtual uint ReadUInt32()
        {
            if (ReachedEnd) return default(uint);
            return Convert.ToUInt32(Arguments[Index++]);
        }

        public virtual int ReadInt32()
        {
            if (ReachedEnd) return default(int);
            return Convert.ToInt32(Arguments[Index++]);
        }

        public virtual long ReadInt64()
        {
            if (ReachedEnd) return default(long);
            return Convert.ToInt64(Arguments[Index++]);
        }

        public virtual sbyte ReadSByte()
        {
            if (ReachedEnd) return default(sbyte);
            return Convert.ToSByte(Arguments[Index++]);
        }

        public virtual float ReadSingle()
        {
            if (ReachedEnd) return default(float);
            return Convert.ToSingle(Arguments[Index++]);
        }

        public virtual string ReadString()
        {
            if (ReachedEnd) return default(string);
            return Convert.ToString(Arguments[Index++]);
        }

        public virtual ulong ReadUInt64()
        {
            if (ReachedEnd) return default(ulong);
            return Convert.ToUInt64(Arguments[Index++]);
        }

        public ArgumentsReader ReadReader()
        {
            if (ReachedEnd) return new ArgumentsReader(Array.Empty<object>());
            return new ArgumentsReader(ReadObjects());
        }

        public virtual Color ReadColor()
        {
            if (ReachedEnd) return default(Color);
            return (Color)Arguments[Index++];
        }

        public virtual Coord2i ReadCoord2i()
        {
            if (ReachedEnd) return default(Coord2i);
            return (Coord2i)Arguments[Index++];
        }

        public virtual Coord2f ReadCoord2f()
        {
            if (ReachedEnd) return default(Coord2f);
            return (Coord2f)Arguments[Index++];
        }

        public virtual Coord2d ReadCoord2d()
        {
            if (ReachedEnd) return default(Coord2d);
            return (Coord2d)Arguments[Index++];
        }

        public virtual Coord3i ReadCoord3i()
        {
            if (ReachedEnd) return default(Coord3i);
            return (Coord3i)Arguments[Index++];
        }

        public virtual Coord3f ReadCoord3f()
        {
            if (ReachedEnd) return default(Coord3f);
            return (Coord3f)Arguments[Index++];
        }

        public virtual Coord3d ReadCoord3d()
        {
            if (ReachedEnd) return default(Coord3d);
            return (Coord3d)Arguments[Index++];
        }

        public bool ReadBoolean_ifType(bool defaultValue = default(bool))
        {
            return IsNext<bool>() ? ReadBoolean() : defaultValue;
        }

        public byte ReadByte_ifType(byte defaultValue = default(byte))
        {
            return IsNext<byte>() ? ReadByte() : defaultValue;
        }

        public sbyte ReadSByte_ifType(sbyte defaultValue = default(sbyte))
        {
            return IsNext<sbyte>() ? ReadSByte() : defaultValue;
        }

        public byte[] ReadBytes_ifType(byte[] defaultValue = default(byte[]))
        {
            return IsNext<byte[]>() ? ReadBytes() : defaultValue;
        }

        public char ReadChar_ifType(char defaultValue = default(char))
        {
            return IsNext<char>() ? ReadChar() : defaultValue;
        }

        public char[] ReadChars_ifType(char[] defaultValue = default(char[]))
        {
            return IsNext<char[]>() ? ReadChars() : defaultValue;
        }

        public decimal ReadDecimal_ifType(decimal defaultValue = default(decimal))
        {
            return IsNext<decimal>() ? ReadDecimal() : defaultValue;
        }

        public double ReadDouble_ifType(double defaultValue = default(double))
        {
            return IsNext<double>() ? ReadDouble() : defaultValue;
        }

        public float ReadSingle_ifType(float defaultValue = default(float))
        {
            return IsNext<float>() ? ReadSingle() : defaultValue;
        }

        public ushort ReadUInt16_ifType(ushort defaultValue = default(ushort))
        {
            return IsNext<ushort>() ? ReadUInt16() : defaultValue;
        }

        public short ReadInt16_ifType(short defaultValue = default(short))
        {
            return IsNext<short>() ? ReadInt16() : defaultValue;
        }

        public uint ReadUInt32_ifType(uint defaultValue = default(uint))
        {
            return IsNext<uint>() ? ReadUInt32() : defaultValue;
        }

        public int ReadInt32_ifType(int defaultValue = default(int))
        {
            return IsNext<int>() ? ReadInt32() : defaultValue;
        }

        public ulong ReadUInt64_ifType(ulong defaultValue = default(ulong))
        {
            return IsNext<ulong>() ? ReadUInt64() : defaultValue;
        }

        public long ReadInt64_ifType(long defaultValue = default(long))
        {
            return IsNext<long>() ? ReadInt64() : defaultValue;
        }

        public string ReadString_ifType(string defaultValue = default(string))
        {
            return IsNext<string>() ? ReadString() : defaultValue;
        }

        public Color? ReadColor_ifType(Color? defaultValue = default(Color?))
        {
            return IsNext<Color>() ? ReadColor() : defaultValue;
        }

        public Coord2i? ReadCoord2i_ifType(Coord2i? defaultValue = default(Coord2i?))
        {
            return IsNext<Coord2i>() ? ReadCoord2i() : defaultValue;
        }

        public Coord2f? ReadCoord2f_ifType(Coord2f? defaultValue = default(Coord2f?))
        {
            return IsNext<Coord2f>() ? ReadCoord2f() : defaultValue;
        }

        public Coord2d? ReadCoord2d_ifType(Coord2d? defaultValue = default(Coord2d?))
        {
            return IsNext<Coord2d>() ? ReadCoord2d() : defaultValue;
        }

        public Coord3i? ReadCoord3i_ifType(Coord3i? defaultValue = default(Coord3i?))
        {
            return IsNext<Coord3i>() ? ReadCoord3i() : defaultValue;
        }

        public Coord3f? ReadCoord3f_ifType(Coord3f? defaultValue = default(Coord3f?))
        {
            return IsNext<Coord3f>() ? ReadCoord3f() : defaultValue;
        }

        public Coord3d? ReadCoord3d_ifType(Coord3d? defaultValue = default(Coord3d?))
        {
            return IsNext<Coord3d>() ? ReadCoord3d() : defaultValue;
        }        

        public bool ReachedEnd => Index >= Length;

        public static implicit operator bool(ArgumentsReader @this)
        {
            return !@this.ReachedEnd;
        }

        public IEnumerable<object> Remainder
        {
            get
            {
                for (int i = Index; i < Length; i++)
                {
                    yield return Arguments[i];
                }
            }
        }

        public object[] ReadRemainder()
        {
            object[] result = Remainder.ToArray();
            Index = Length;
            return result;
        }
    }
}
