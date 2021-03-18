using HHSwarm.Native.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.WorldModel
{
    public class OD_CMPPOSE : IGameObjectDatum
    {
        /// <summary>
        /// 'seq'
        /// </summary>
        public byte Seq;

        /// <summary>
        /// 'interp'
        /// </summary>
        public bool Interp;

        /// <summary>
        /// 'ttime'
        /// </summary>
        public float Ttime;

        [Flags]
        internal enum FLAGS : byte
        {
            /// <summary>
            /// 'interp'
            /// </summary>
            Interp = 0x01,
            Poses = 0x02,
            Tposes = 0x04
        }

        /// <summary>
        /// 'poses'
        /// </summary>
        public List<ResData> Poses = new List<ResData>();

        /// <summary>
        /// 'tposes'
        /// </summary>
        public List<ResData> Tposes = new List<ResData>();
    }
}
