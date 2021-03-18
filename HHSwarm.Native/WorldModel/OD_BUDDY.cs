using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.WorldModel
{
    public class OD_BUDDY : IGameObjectDatum
    {
        /// <summary>
        /// 'name'
        /// </summary>
        public string Name;

        /// <summary>
        /// 'group'
        /// </summary>
        public byte Group;

        /// <summary>
        /// 'btype'
        /// </summary>
        public byte BuddyType;

        public class REMOVE : OD_BUDDY
        {
        }
    }
}
