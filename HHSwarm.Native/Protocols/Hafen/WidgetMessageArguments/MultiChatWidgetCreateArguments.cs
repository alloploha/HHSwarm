using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    /// <summary>
    /// <c>java: class MultiChat</c>
    /// </summary>
    /// <remarks>
    /// https://github.com/dolda2000/hafen-client/blob/9dc7c1e7af3f1e3d49a7e2b42b6a11ec8c463af8/src/haven/ChatUI.java#L675
    /// </remarks>
    class MultiChatWidgetCreateArguments
    {
        public bool Closable;

        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/9dc7c1e7af3f1e3d49a7e2b42b6a11ec8c463af8/src/haven/ChatUI.java#L855
        /// </remarks>
        public string Name;

        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/9dc7c1e7af3f1e3d49a7e2b42b6a11ec8c463af8/src/haven/ChatUI.java#L856
        /// </remarks>
        public int Urgency;
    }
}
