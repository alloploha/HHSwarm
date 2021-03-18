using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources.Graphics
{
    public abstract class TextureFilter
    {
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
