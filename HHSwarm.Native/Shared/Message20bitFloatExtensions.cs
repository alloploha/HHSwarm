using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO
{
    public static class Message20bitFloatExtensions
    {
        /// <summary>
        /// 'cpfloat', 'floatd'
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static double ReadDouble20bit(this BinaryReader reader)
        {
            int exponent = reader.ReadByte();
            uint t = reader.ReadUInt32();

            int mantissa = (int)(t & 0x7fffffff);
            bool sign_bit = (t & 0x80000000) == 0x80000000;

            if (exponent == -128)
            {
                if (mantissa == 0)
                    return 0;
                else
                    throw (new InvalidDataException($"Invalid special float encoded ({mantissa})"));
            }

            double v = mantissa / 2147483648.0 + 1.0;

            return (Math.Pow(2.0, exponent) * v * (sign_bit ? -1 : 1));
        }

        public static void Write20bit(this BinaryWriter writer, double value)
        {
            throw new NotImplementedException(nameof(Write20bit));
        }
    }
}
