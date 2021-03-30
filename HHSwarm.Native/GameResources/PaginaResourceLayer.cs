using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <remarks>
    /// @LayerName("pagina")
    /// https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1087
    /// </remarks>
    [Serializable]
    public class PaginaResourceLayer : ResourceLayer
    {
        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1088
        /// </remarks>
        public string Text;
    }
}
