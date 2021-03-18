using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <summary>
    /// '@Resource.LayerName("vbuf")', 'class Legacy'
    /// </summary>
    [Serializable]
    public class VertexBufferResourceLayer1 : ResourceLayer
    {
        [Flags]
        public enum FLAGS : byte
        {
        }

        public enum ArrayType : byte
        {
            /// <summary>
            /// 'VertexArray', 'pos'
            /// </summary>
            Vertex = 0,

            /// <summary>
            /// 'NormalArray', 'nrm'
            /// </summary>
            Normal = 1,

            /// <summary>
            /// 'TexelArray', 'tex'
            /// </summary>
            Texel = 2,

            /// <summary>
            /// 'BoneArray + WeightArray', 'bones'
            /// </summary>
            Bone = 3
        }

        public float[,] Vertexes;
        public float[,] Normals;
        public float[,] Texels;

        public VertexBufferResourceLayer_Bone[] Bones = Array.Empty<VertexBufferResourceLayer_Bone>();
    }
}
