using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.WorldModel
{
    public class OD_ICON : IGameObjectDatum
    {
        public ushort ResourceID;

        public class REMOVE : OD_ICON
        {
        }
    }
}
