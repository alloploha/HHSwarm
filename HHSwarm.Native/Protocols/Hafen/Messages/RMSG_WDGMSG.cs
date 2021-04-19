using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.Messages
{
    /// <summary>
    /// RMSG_WDGMSG = 1
    /// </summary>
    public class RMSG_WDGMSG : RMSG_WDG, ISerializableRelayMessage
    {        
        /// <summary>
        /// RemoteUI.rcvmsg(..., name, ...)
        /// </summary>
        public string MessageName;

        /// <summary>
        /// RemoteUI.rcvmsg(..., ..., args)
        /// </summary>
        public object[] MessageArguments;

        public void CallSerializer(IRelayMessageSerializer serializer, BinaryWriter writer)
        {
            serializer.Serialize(this, writer);
        }

        public delegate void Callback(RMSG_WDGMSG message);

    }
}
