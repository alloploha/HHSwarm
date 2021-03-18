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
    /// '@Resource.LayerName("light")'
    /// </summary>
    [Serializable]
    public class LightResourceLayer : ResourceLayer
    {
        /// <summary>
        /// 'id'
        /// </summary>
        public short ID;

        /// <summary>
        /// 'amb'
        /// </summary>
        public Color Ambient;

        /// <summary>
        /// 'dif'
        /// </summary>
        public Color Diffuse;

        /// <summary>
        /// 'spc'
        /// </summary>
        public Color Specular;

        /// <summary>
        /// 'dir', GL_SPOT_DIRECTION
        /// </summary>
        public Coord3f SpotDirection;

        [Serializable]
        public struct LightAttenuation
        {
            /// <summary>
            /// 'ac', GL_CONSTANT_ATTENUATION
            /// </summary>
            public float Constant;

            /// <summary>
            /// 'al', GL_LINEAR_ATTENUATION
            /// </summary>
            public float Linear;

            /// <summary>
            /// 'aq', GL_QUADRATIC_ATTENUATION
            /// </summary>
            public float Quadratic;
        }

        public LightAttenuation? Attenuation;

        /// <summary>
        /// 'hexp' + 'exp', GL_SPOT_EXPONENT
        /// </summary>
        public float? SpotExponent;

        public enum DataType : byte
        {
            Attenuation = 1,
            Direction = 2,
            Exponent = 3
        }
    }
}
