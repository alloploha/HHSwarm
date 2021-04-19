using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <remarks>
    /// @LayerName("pagina"), possible: a command button or a "Page" of command buttons
    /// https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1087
    /// use: https://github.com/dolda2000/hafen-client/blob/25968122bbebc26462e0046ae4b8a0cb480dc65d/src/haven/MenuGrid.java#L129
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
