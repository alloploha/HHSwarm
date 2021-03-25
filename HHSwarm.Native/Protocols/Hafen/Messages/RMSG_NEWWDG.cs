using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.Messages
{
    /// <summary>
    /// RMSG_NEWWDG = 0
    /// </summary>
    public class RMSG_NEWWDG : RMSG_WDG, ISerializableRelayMessage
    {
        public string Type;

        /// <summary>
        /// parent
        /// </summary>
        public ushort ParentID;

        /// <summary>
        /// pargs
        /// </summary>
        public object[] AddChildArguments;

        /// <summary>
        /// cargs
        /// </summary>
        public object[] CreateArguments;

        public void CallSerializer(IRelayMessageSerializer serializer, BinaryWriter writer)
        {
            serializer.Serialize(this, writer);
        }

        public delegate void Callback(RMSG_NEWWDG message);
    }
}
