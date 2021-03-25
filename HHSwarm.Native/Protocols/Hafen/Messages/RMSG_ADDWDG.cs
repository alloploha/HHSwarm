using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.Messages
{
    /// <summary>
    /// RMSG_ADDWDG = 15 (0x0F)
    /// </summary>
    public class RMSG_ADDWDG : RMSG_WDG, ISerializableRelayMessage
    {
        /// <summary>
        /// parent
        /// </summary>
        public ushort ParentID;

        /// <summary>
        /// pargs
        /// </summary>
        public object[] AddChildArguments;

        public void CallSerializer(IRelayMessageSerializer serializer, BinaryWriter writer)
        {
            serializer.Serialize(this, writer);
        }

        public delegate void Callback(RMSG_ADDWDG message);
    }
}
