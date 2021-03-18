using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO
{
    public static class Message16bitFloatExtensions
    {
        /// <summary>
        /// 'hfdec', 'float hfdec(short bits)'
        /// </summary>
        public static float ReadSingle16bit(this BinaryReader reader)
        {
            short bits = reader.ReadInt16();

            int b = ((int)bits) & 0xffff;
            int e = (b & 0x7c00) >> 10;
            int m = b & 0x03ff;
            int ee;

            if (e == 0)
            {
                if (m == 0)
                {
                    ee = 0;
                }
                else
                {
                    int n = m.LeadingZerosCount() - 22;
                    ee = (-15 - n) + 127;
                    m = (m << (n + 1)) & 0x03ff;
                }
            }
            else if (e == 0x1f)
            {
                ee = 0xff;
            }
            else
            {
                ee = e - 15 + 127;
            }

            int f32 = 
                ((b & 0x8000) << 16) |
                (ee << 23) |
                (m << 13);

            return f32.BitsToFloat();
        }

        /// <summary>
        /// 'hfenc', 'short hfenc(float f)'
        /// </summary>
        public static void Write16bit(this BinaryWriter writer, float value)
        {
            throw new NotImplementedException(nameof(Write16bit));
        }


    }
}
