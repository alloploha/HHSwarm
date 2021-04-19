using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    class QuestBoxWidgetCreateArguments
    {
        /// <summary>
        /// <c>java: Box.id</c>
        /// </summary>
        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1059
        /// </remarks>
        public int QuestID;

        /// <summary>
        /// <c>java: Box.res</c>
        /// </summary>
        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1060
        /// </remarks>
        public ushort ResourceID;

        /// <summary>
        /// <c>java: Box.title</c>
        /// </summary>
        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1061
        /// </remarks>
        public string Title;
    }
}
