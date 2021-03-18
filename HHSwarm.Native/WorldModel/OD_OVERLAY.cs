using HHSwarm.Native.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.WorldModel
{
    public class OD_OVERLAY : IGameObjectDatum
    {
        /// <summary>
        /// 'olid'
        /// </summary>
        public uint OverlayID;

        /// <summary>
        /// 'prs', 'delign'
        /// </summary>
        public bool Delign;

        /// <summary>
        /// Can be null
        /// </summary>
        public ResData Resource;
    }
}
