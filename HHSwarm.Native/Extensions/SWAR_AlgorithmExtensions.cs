using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// A SWAR architecture is one which includes instructions explicitly intended to perform parallel operations across data that is stored in the independent subwords or fields of a register.
    /// http://aggregate.org/MAGIC/
    /// </summary>
    public static class SWAR_AlgorithmExtensions
    {
        private const byte WORDBITS = sizeof(int) * 8;

        /// <summary>
        /// 'Integer.numberOfLeadingZeros(...)'
        /// http://aggregate.org/MAGIC/#Leading%20Zero%20Count
        /// </summary>
        public static byte LeadingZerosCount(this int x)
        {
            x |= (x >> 1);
            x |= (x >> 2);
            x |= (x >> 4);
            x |= (x >> 8);
            x |= (x >> 16);
            return (byte)(WORDBITS - x.OnesCount());
        }

        /// <summary>
        /// http://aggregate.org/MAGIC/#Population%20Count%20(Ones%20Count)
        /// </summary>
        public static byte OnesCount(this int x)
        {
            x -= ((x >> 1) & 0x55555555);
            x = (((x >> 2) & 0x33333333) + (x & 0x33333333));
            x = (((x >> 4) + x) & 0x0f0f0f0f);
            x += (x >> 8);
            x += (x >> 16);
            return (byte)(x & 0x0000003f);
        }

        /// <summary>
        /// 'Float.intBitsToFloat(...)'
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float BitsToFloat(this int value) => BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
    }
}
