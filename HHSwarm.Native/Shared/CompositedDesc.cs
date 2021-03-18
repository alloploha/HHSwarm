using HHSwarm.Native.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Shared
{
    /// <summary>
    /// 'Composited.Desc'
    /// </summary>
    public class CompositedDesc
    {
        /// <summary>
        /// 'base'
        /// </summary>
        public ushort Base;

        /// <summary>
        /// 'mod'
        /// </summary>
        public List<MD> Mod = new List<MD>();
        public List<ED> Equ = new List<ED>();

        /// <summary>
        /// 'MD'
        /// </summary>
        public class MD
        {
            /// <summary>
            /// 'id'
            /// </summary>
            public int? id;

            /// <summary>
            /// 'mod', 'MD.mod', 'modid -> modr'
            /// </summary>
            public ushort ModelResourceID;

            /// <summary>
            /// 'tex'
            /// </summary>
            public List<ResData> tex = new List<ResData>();
        }

        public class ED
        {
            /// <summary>
            /// 'id'
            /// </summary>
            public int? id;

            /// <summary>
            /// 't', 'et'
            /// </summary>
            public byte? t;

            /// <summary>
            /// 'at'
            /// </summary>
            public string at;

            /// <summary>
            /// 'res'
            /// </summary>
            public ResData Resource;

            /// <summary>
            /// 'off'
            /// </summary>
            public Coord3f Offset;
        }
    }
}
