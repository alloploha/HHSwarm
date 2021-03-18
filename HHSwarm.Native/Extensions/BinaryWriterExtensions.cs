using HHSwarm.Native.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO
{
    public static class BinaryWriterExtensions
    {
        public static void WriteBigEndian(this BinaryWriter @this, ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            @this.Write(bytes);
        }

        public static void WriteBigEndian(this BinaryWriter @this, short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            @this.Write(bytes);
        }

        public static void WriteBigEndian(this BinaryWriter @this, uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            @this.Write(bytes);
        }

        public static void WriteBigEndian(this BinaryWriter @this, int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            @this.Write(bytes);
        }

        public static void WriteBigEndian(this BinaryWriter @this, ulong value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            @this.Write(bytes);
        }

        public static void WriteBigEndian(this BinaryWriter @this, long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            @this.Write(bytes);
        }

        public static void Write(this BinaryWriter @this, Color value)
        {
            @this.Write(value.R);
            @this.Write(value.G);
            @this.Write(value.B);
            @this.Write(value.A);
        }

        public static void Write(this BinaryWriter @this, Coord2i value)
        {
            @this.Write(value.X);
            @this.Write(value.Y);
        }

        public static void Write(this BinaryWriter @this, Coord2f value)
        {
            @this.Write(value.X);
            @this.Write(value.Y);
        }

        public static void Write(this BinaryWriter @this, Coord2d value)
        {
            @this.Write(value.X);
            @this.Write(value.Y);
        }

        public static void Write(this BinaryWriter @this, Coord3i value)
        {
            @this.Write(value.X);
            @this.Write(value.Y);
            @this.Write(value.Z);
        }

        public static void Write(this BinaryWriter @this, Coord3f value)
        {
            @this.Write(value.X);
            @this.Write(value.Y);
            @this.Write(value.Z);
        }

        public static void Write(this BinaryWriter @this, Coord3d value)
        {
            @this.Write(value.X);
            @this.Write(value.Y);
            @this.Write(value.Z);
        }
    }
}
