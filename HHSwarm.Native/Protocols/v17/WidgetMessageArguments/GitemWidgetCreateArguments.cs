using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.v17.WidgetMessageArguments
{
    /// <summary>
    /// '@RName("item")', 'class GItem'
    /// </summary>
    class GitemWidgetCreateArguments
    {
        /// <summary>
        /// 'res'
        /// </summary>
        public ushort ResourceID;

        /// <summary>
        /// 'sdt'
        /// </summary>
        public byte[] Data = Array.Empty<byte>();
    }
}
