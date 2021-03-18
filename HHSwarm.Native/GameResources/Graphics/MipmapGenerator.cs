using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources.Graphics
{
    public abstract class MipmapGenerator
    {
        public enum TYPE : byte
        {
            Default = 0,
            Average = 1,
            Random = 2,
            Cnt = 3,
            Dav = 4
        }
    }
}
