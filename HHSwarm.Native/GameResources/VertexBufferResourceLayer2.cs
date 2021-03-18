using System;
using System.Collections.Generic;

namespace HHSwarm.Native.GameResources
{
    /// <summary>
    /// '@Resource.LayerName("vbuf2")', 'class VertexRes'
    /// </summary>
    [Serializable]
    public class VertexBufferResourceLayer2 : ResourceLayer
    {
        [Flags]
        public enum FLAGS : byte
        {
        }

        public byte Version;
        public short ID;

        public float[,] Vertexes;
        public float[,] Normals;
        public float[,] Colors;
        public float[,] Texels;
        public float[,] OverTexes;
        public float[,] Tangents;
        public float[,] Bitangents;

        public VertexBufferResourceLayer_Bone[] Bones = Array.Empty<VertexBufferResourceLayer_Bone>();
    }
}
