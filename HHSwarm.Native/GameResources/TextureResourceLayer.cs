using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <summary>
    /// '@Resource.LayerName("tex")', 'class TexR'
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
            Mask = 4
        }

        public short TextureID;
        public Point Offset;
        public Size Size;
        public byte[] Image;
        public byte[] Mask;
        public Graphics.MipmapGenerator.TYPE MipmapGeneratorType;
        public Graphics.TextureFilter.TYPE MagFilterType;
        public Graphics.TextureFilter.TYPE MinFilterType;

        public object Texture;
    }
}
