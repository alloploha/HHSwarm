using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <summary>
    /// '@Resource.LayerName("tex")'
    /// <c>java: class TexR</c>
    /// </summary>
    [Serializable]
    public class TextureResourceLayer : ResourceLayer
    {
        public enum DATA_PART : byte
        {
            Image = 0,
            MipmapGeneratorType = 1,
            MagnificationFilter = 2,
            MinificationFilter = 3,
            Mask = 4,

            /// <summary>
            /// "Linear color values, not relevant right now"
            /// </summary>
            /// https://github.com/dolda2000/hafen-client/blob/394aeed6e3ebbfa64d679a7d4fdda364a982d8bb/src/haven/TexR.java#L86
            /// <remarks>
            /// </remarks>
            LinearColorValues = 5
        }

        /// <summary>
        /// <c>java: TexR.id</c>
        /// </summary>
        public short TextureID;

        /// <summary>
        /// <c>java: TexR.off</c>
        /// </summary>
        public Point Offset;

        /// <summary>
        /// <c>java: TexR.sz</c>
        /// </summary>
        public Size Size;

        public byte[] Image;

        public byte[] Mask;

        /// <summary>
        /// <c>java: int ma</c>
        /// </summary>
        public Graphics.MipmapGenerator.TYPE MipmapGeneratorType;

        public Graphics.TextureFilter.TYPE MagFilterType;
        public Graphics.TextureFilter.TYPE MinFilterType;

        public object Texture;
    }
}
