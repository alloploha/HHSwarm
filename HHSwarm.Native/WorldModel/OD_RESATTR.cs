using HHSwarm.Native.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.WorldModel
{
    public class OD_RESATTR : IGameObjectDatum
    {
        private ResData Resource;

        public ushort ResourceID => Resource.ResourceID;

        public byte[] ResourceData => Resource.ResourceData;

        public OD_RESATTR(ushort resourceId, byte[] resourceData) : this(new ResData(resourceId, resourceData))
        {
        }

        private OD_RESATTR(ResData resData)
        {
            if (resData == null) throw new ArgumentNullException(nameof(resData));
            this.Resource = resData;
        }

        public class REMOVE : OD_RESATTR
        {
            public REMOVE() : base(0xFFFF, null)
            {
            }
        }
    }
}
