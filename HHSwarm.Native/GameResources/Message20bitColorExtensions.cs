using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    public static class Message20bitColorExtensions
    {
        /// <summary>
        /// 'cold(...)'
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Color ReadColor20bit(this BinaryReader reader)
        {
            return Color.FromArgb
            (
                red: (byte)(reader.ReadDouble20bit() * 255),
                green: (byte)(reader.ReadDouble20bit() * 255),
                blue: (byte)(reader.ReadDouble20bit() * 255),
                alpha: (byte)(reader.ReadDouble20bit() * 255)
            );
        }

        public static void Write20bit(this BinaryWriter writer, Color value)
        {
            throw new NotImplementedException(nameof(Write20bit));
        }
    }
}
