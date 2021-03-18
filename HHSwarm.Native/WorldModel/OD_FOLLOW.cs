using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.WorldModel
{
    public class OD_FOLLOW : IGameObjectDatum
    {
        /// <summary>
        /// 'oid'
        /// </summary>
        public uint ObjectID;

        /// <summary>
        /// 'xfres'
        /// </summary>
        public ushort BoneOffsetResourceID;

        /// <summary>
        /// 'xfname'
        /// </summary>
        public string BoneOffsetResourceLayerName;

        public class REMOVE : OD_FOLLOW
        {
        }
    }
}
