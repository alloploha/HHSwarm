using HHSwarm.Native.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <summary>
    /// '@Resource.LayerName("skel")'
    /// </summary>
    [Serializable]
    public class SkeletonResourceLayer : ResourceLayer
    {
        [Serializable]
        public class Bone
        {
            public string Name;
            public Coord3d Position;
            public Coord3d RotationAxis;
            public double RotationAngle;
            public string ParentBoneName;
        }

        public readonly IList<Bone> Bones = new List<Bone>();
    }
}
