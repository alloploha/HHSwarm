using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.WorldModel
{
    public class OD_LUMIN : IGameObjectDatum
    {
        /// <summary>
        /// 'Coord off'
        /// </summary>
        public Point Off;

        /// <summary>
        /// 'sz'
        /// </summary>
        public ushort Sz;

        /// <summary>
        /// 'str'
        /// </summary>
        public byte Str;
    }
}
