using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <summary>
    /// '@Resource.LayerName("mesh")', 'class MeshRes'
    /// </summary>
    [Serializable]
    public class MeshResourceLayer : ResourceLayer
    {
        [Flags]
        public enum FLAGS : byte
        {
            ID = 2,
            Ref = 4,
            Rdat = 8
        }

        public short ID;
        public short Ref;
        public int MaterialID;
        public IDictionary<string, string> Rdat = new Dictionary<string, string>();

        /// <summary>
        /// ind, indb
        /// </summary>
        public short[,] Items;
    }
}
