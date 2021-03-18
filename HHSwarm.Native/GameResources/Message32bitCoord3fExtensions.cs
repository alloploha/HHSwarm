using HHSwarm.Native.Common;
using System;
using System.IO;

namespace HHSwarm.Native.GameResources
{
    public static class Message32bitCoord3fExtensions
    {
        /// <summary>
        /// 'float9995d'
        /// </summary>
        public static Coord3f ReadCoord3f32bit(this BinaryReader reader)
        {
            int word = reader.ReadInt32();

            int
                xb = (word & 0x7f800000) >> 23, xs = ((word & 0x80000000) == 0x80000000) ? 1 : 0,
                yb = (word & 0x003fc000) >> 14, ys = ((word & 0x00400000) == 0x00400000) ? 1 : 0,
                zb = (word & 0x00001fd0) >> 05, zs = ((word & 0x00002000) == 0x00002000) ? 1 : 0;

            int me = (word & 0x1f) - 15;

            int
                xe = xb.LeadingZerosCount() - 24,
                ye = yb.LeadingZerosCount() - 24,
                ze = zb.LeadingZerosCount() - 24;

            return new Coord3f
            (
                x: (xe == 32) ? 0 : ((int)((xs << 31) | ((me - xe + 127) << 23) | ((xb << (xe + 16)) & 0x007fffff))).BitsToFloat(),
                y: (ye == 32) ? 0 : ((int)((ys << 31) | ((me - ye + 127) << 23) | ((yb << (ye + 16)) & 0x007fffff))).BitsToFloat(),
                z: (ze == 32) ? 0 : ((int)((zs << 31) | ((me - ze + 127) << 23) | ((zb << (ze + 16)) & 0x007fffff))).BitsToFloat()
            );
        }

        public static void Write32bit(this BinaryReader writer, Coord3f value)
        {
            throw new NotImplementedException(nameof(Write32bit));
        }
    }
}
