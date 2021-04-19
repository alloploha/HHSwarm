using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources.Graphics
{
    public abstract class TextureFilter
    {
        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/394aeed6e3ebbfa64d679a7d4fdda364a982d8bb/src/haven/TexR.java#L74-L80
        /// </remarks>
        public enum TYPE : byte
        {
            NEAREST = 0,
            LINEAR = 1,
            NEAREST_MIPMAP_NEAREST = 2,
            NEAREST_MIPMAP_LINEAR = 3,
            LINEAR_MIPMAP_NEAREST = 4,
            LINEAR_MIPMAP_LINEAR = 5,
        }
    }
}
