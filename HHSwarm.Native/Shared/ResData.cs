using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Shared
{
    /// <summary>
    /// 'ResData'
    /// </summary>
    public sealed class ResData
    {
        /// <summary>
        /// 'resid', 'tr'
        /// </summary>
        public ushort ResourceID { get; }

        /// <summary>
        /// 'sdt'
        /// </summary>
        public byte[] ResourceData { get; }

        public ResData(ushort resourceId, byte[] resourceData = null)
        {
            this.ResourceID = resourceId;
            this.ResourceData = resourceData;
        }
    }
}
