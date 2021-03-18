using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.v17.Messages
{
    /// <summary>
    /// RMSG_DSTWDG = 2
    /// </summary>
    public class RMSG_DSTWDG : RMSG_WDG, ISerializableRelayMessage
    {
        public void CallSerializer(IRelayMessageSerializer serializer, BinaryWriter writer)
        {
            serializer.Serialize(this, writer);
        }

        public delegate void Callback(RMSG_DSTWDG message);
    }
}
