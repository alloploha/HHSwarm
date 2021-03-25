using HHSwarm.Native.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    /// <summary>
    /// '@RName("mapview")'
    /// </summary>
    class MapViewWidgetCreateArguments
    {
        /// <summary>
        /// 'sz'
        /// </summary>
        public Coord2i Size;

        /// <summary>
        /// 'mc', 'cc'
        /// </summary>
        public Coord2i mc;

        /// <summary>
        /// 'pgob', 'plgob'
        /// </summary>
        public int? pgob;
    }
}
