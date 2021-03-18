using HHSwarm.Native.WorldModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.v17.Messages
{
    public class RMSG_GLOBLOB : RMSG, ISerializableRelayMessage
    {
        public void CallSerializer(IRelayMessageSerializer serializer, BinaryWriter writer)
        {
            serializer.Serialize(this, writer);
        }

        public delegate void Callback(RMSG_GLOBLOB message);

        public GlobalObjects Objects;
    }
}
