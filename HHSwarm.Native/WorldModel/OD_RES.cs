using HHSwarm.Native.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.WorldModel
{
    public class OD_RES : IGameObjectDatum
    {
        private ResData Resource;

        public ushort ResourceID => Resource.ResourceID;

        public byte[] ResourceData => Resource.ResourceData;

        public OD_RES(ushort resourceId, byte[] resourceData = null) : this(new ResData(resourceId, resourceData))
        {
        }

        internal OD_RES(ResData resData)
        {
            if (resData == null) throw new ArgumentNullException(nameof(resData));
            this.Resource = resData;
        }
    }
}
