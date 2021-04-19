using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    /// <remarks>
    /// https://github.com/dolda2000/hafen-client/blob/d61d2a6ff4a832f84e55069794578002d37b0ce1/src/haven/Partyview.java#L42
    /// </remarks>
    class PartyViewWidgetCreateArguments
    {
        /// <summary>
        /// ?Ignore?,
        /// same type as <c>java: Member.gobid</c>
        /// </summary>
        /// <remarks>
        /// <c>java: Partyvew.ign</c>
        /// https://github.com/dolda2000/hafen-client/blob/d61d2a6ff4a832f84e55069794578002d37b0ce1/src/haven/Partyview.java#L62-L64
        /// </remarks>
        public long ign;
    }
}
