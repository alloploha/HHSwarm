using HHSwarm.Native.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <summary>
    /// 'hfdec', 'Utils.hfdec((short)buf.int16())'
    /// </summary>
    public static class Message16bitCoord3fExtensions
    {
        /// <summary>
        /// 'hfdec'
        /// </summary>
        public static Coord3f ReadCoord3f16bit(this BinaryReader reader)
        {
            return new Coord3f
            (
                x: reader.ReadSingle16bit(),
                y: reader.ReadSingle16bit(),
                z: reader.ReadSingle16bit()
            );
        }

        public static void Write32bit(this BinaryReader writer, Coord3f value)
        {
            throw new NotImplementedException(nameof(Write32bit));
        }
    }
}
