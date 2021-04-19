using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.Messages
{
    public abstract class RMSG_WDG : RMSG
    {
        /// <summary>
        /// RemoteUI.rcvmsg(id, ...)
        /// </summary>
        public ushort WidgetID;
    }
}
