using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <summary>
    /// @Resource.LayerName("rlink"), 'RenderLink', 'rlink'
    /// </summary>
    [Serializable]
    public abstract class RenderLinkResourceLayer : ResourceLayer
    {
        /// <summary>
        /// 'id', 'layerid'
        /// </summary>
        public short? LayerID;

        /// <summary>
        /// 't'
        /// </summary>
        public enum TYPE : byte
        {
            MeshAndMaterial = 0,
            AmbienceAudio = 1,
            Mesh = 2
        }

        [Serializable]
        public class MeshAndMaterial : RenderLinkResourceLayer
        {
            /// <summary>
            /// 'meshnm'
            /// </summary>
            public string MeshName;

            /// <summary>
            /// 'meshver'
            /// </summary>
            public ushort MeshVersion;

            /// <summary>
            /// 'meshid'
            /// </summary>
            public short MeshID;

            /// <summary>
            /// 'matnm'
            /// </summary>
            public string MaterialName;

            /// <summary>
            /// 'matver'
            /// </summary>
            public ushort MaterialVersion;

            /// <summary>
            /// 'matid'
            /// </summary>
            public short MaterialID;
        }

        [Serializable]
        public class AmbienceAudio : RenderLinkResourceLayer
        {
            /// <summary>
            /// 'nm'
            /// </summary>
            public string Name;

            /// <summary>
            /// 'ver'
            /// </summary>
            public ushort Version;
        }

        [Serializable]
        public class Mesh : RenderLinkResourceLayer
        {
            /// <summary>
            /// 'nm'
            /// </summary>
            public string Name;

            /// <summary>
            /// 'ver'
            /// </summary>
            public ushort Version;

            /// <summary>
            /// 'meshid'
            /// </summary>
            public short MeshID;
        }
    }
}
