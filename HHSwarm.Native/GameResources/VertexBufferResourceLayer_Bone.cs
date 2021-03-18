using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    [DebuggerDisplay(@"Bone:{BoneIndex}='{BoneName}', Runs={Runs.Length}")]
    [Serializable]
    public class VertexBufferResourceLayer_Bone
    {
        public string BoneName;
        public int BoneIndex;

        /// <summary>
        /// 'mba'
        /// </summary>
        public byte mba;

        [DebuggerDisplay(@"{VertexIndex}:{Weight}")]
        [Serializable]
        public struct VertexWeight
        {
            public int VertexIndex;
            public float Weight;
        }

        public VertexWeight[][] Runs;
    }
}
