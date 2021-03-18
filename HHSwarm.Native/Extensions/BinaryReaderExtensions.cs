using HHSwarm.Native.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO
{
    public static class EndiannessExtensions
    {
        public static ushort ReadUInt16BigEndian(this BinaryReader @this)
        {
            byte[] bytes = @this.ReadBytes(2);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return BitConverter.ToUInt16(bytes, 0);
        }

        public static short ReadInt16BigEndian(this BinaryReader @this)
        {
            byte[] bytes = @this.ReadBytes(2);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return BitConverter.ToInt16(bytes, 0);
        }

        public static uint ReadUInt32BigEndian(this BinaryReader @this)
        {
            byte[] bytes = @this.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        public static int ReadInt32BigEndian(this BinaryReader @this)
        {
            byte[] bytes = @this.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public static ulong ReadUInt64BigEndian(this BinaryReader @this)
        {
            byte[] bytes = @this.ReadBytes(8);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return BitConverter.ToUInt64(bytes, 0);
        }

        public static long ReadInt64BigEndian(this BinaryReader @this)
        {
            byte[] bytes = @this.ReadBytes(8);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }

        public static Color ReadColor(this BinaryReader @this)
        {
            byte red = @this.ReadByte();
            byte green = @this.ReadByte();
            byte blue = @this.ReadByte();
            byte alpha = @this.ReadByte();
            return Color.FromArgb(alpha, red, green, blue);
        }

        public static Coord2i ReadCoord2i(this BinaryReader @this)
        {
            return new Coord2i
            (
                x: @this.ReadInt32(),
                y: @this.ReadInt32()
            );
        }

        public static Coord2f ReadCoord2f(this BinaryReader @this)
        {
            return new Coord2f
            (
                x: @this.ReadSingle(),
                y: @this.ReadSingle()
            );
        }

        public static Coord2d ReadCoord2d(this BinaryReader @this)
        {
            return new Coord2d
            (
                x: @this.ReadDouble(),
                y: @this.ReadDouble()
            );
        }

        public static Coord3i ReadCoord3i(this BinaryReader @this)
        {
            return new Coord3i
            (
                x: @this.ReadInt32(),
                y: @this.ReadInt32(),
                z: @this.ReadInt32()
            );
        }

        public static Coord3f ReadCoord3f(this BinaryReader @this)
        {
            return new Coord3f
            (
                x: @this.ReadSingle(),
                y: @this.ReadSingle(),
                z: @this.ReadSingle()
            );
        }

        public static Coord3d ReadCoord3d(this BinaryReader @this)
        {
            return new Coord3d
            (
                x: @this.ReadDouble(),
                y: @this.ReadDouble(),
                z: @this.ReadDouble()
            );
        }
    }
}
