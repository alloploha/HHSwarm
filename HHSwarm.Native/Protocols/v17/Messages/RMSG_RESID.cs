using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.v17.Messages
{
    public class RMSG_RESID : RMSG, ISerializableRelayMessage
    {
        /// <summary>
        /// resid
        /// </summary>
        public ushort ResourceID;

        /// <summary>
        /// resname
        /// </summary>
        public string ResourceName;

        /// <summary>
        /// resver
        /// </summary>
        public ushort ResourceVersion;

        public void CallSerializer(IRelayMessageSerializer serializer, BinaryWriter writer)
        {
            serializer.Serialize(this, writer);
        }

        public delegate void Callback(RMSG_RESID message);

    }
}
