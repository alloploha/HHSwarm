using HHSwarm.Native.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    public static class Message20bitCoord3fExtensions
    {
        public static Coord3f ReadCoord3f20bit(this BinaryReader reader)
        {
            return new Coord3f
            (
                x: (float)reader.ReadDouble20bit(),
                y: (float)reader.ReadDouble20bit(),
                z: (float)reader.ReadDouble20bit()
            );
        }

        public static void Write20bit(this BinaryReader writer, Coord3f value)
        {
            throw new NotImplementedException(nameof(Write20bit));
        }
    }
}
