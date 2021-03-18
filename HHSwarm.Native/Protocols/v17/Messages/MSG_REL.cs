using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.v17.Messages
{
    /// <summary>
    /// MSG_REL = 1
    /// </summary>
    public class MSG_REL : MSG, ISerializableSessionMessage
    {

        /// <summary>
        /// PMessage.seq
        /// </summary>
        public ushort SequenceNumber;

        /// <summary>
        /// PMessage.type + PMessage.fin()
        /// </summary>
        public byte[] RelayedData;

        public void CallSerializer(ISessionMessageSerializer serializer, BinaryWriter writer)
        {
            serializer.Serialize(this, writer);
        }

        public delegate void Callback(MSG_REL message);
    }
}
