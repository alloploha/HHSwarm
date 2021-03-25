using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.Messages
{
    public class RMSG_SFX : RMSG, ISerializableRelayMessage
    {
        public ushort ResourceID;

        /// <summary>
        /// Convert to double and divide by 256 before use.
        /// </summary>
        public ushort Volume;

        /// <summary>
        /// Convert to double and divide by 256 before use.
        /// </summary>
        public ushort Speed;

        public void CallSerializer(IRelayMessageSerializer serializer, BinaryWriter writer)
        {
            serializer.Serialize(this, writer);
        }

        public delegate void Callback(RMSG_SFX message);

    }
}
