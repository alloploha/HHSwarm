using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources.Graphics
{
    public abstract class MipmapGenerator
    {
        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/394aeed6e3ebbfa64d679a7d4fdda364a982d8bb/src/haven/TexR.java#L60
        /// </remarks>
        public enum TYPE : byte
        {
            /// <summary>
            /// <c>java: Mipmapper.avg</c>
            /// </summary>
            Default = 0,

            /// <summary>
            /// <c>java: Mipmapper.avg</c>
            /// </summary>
            Average = 1,

            /// <summary>
            /// <c>java: Mipmapper.rnd</c>
            /// </summary>
            Random = 2,

            /// <summary>
            /// <c>java: Mipmapper.cnt</c>
            /// </summary>
            Cnt = 3,

            /// <summary>
            /// <c>java: Mipmapper.dav</c>
            /// </summary>
            Dav = 4
        }
    }
}
