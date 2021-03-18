using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.v17.Messages
{
    public class RMSG_MAPIV : RMSG, ISerializableRelayMessage
    {
        new public enum TYPE : byte
        {
            Point = 0,
            Square = 1,
            Everything = 2
        }

        public void CallSerializer(IRelayMessageSerializer serializer, BinaryWriter writer)
        {
            serializer.Serialize(this, writer);
        }

        public delegate void Callback(RMSG_MAPIV message);

    }
}
