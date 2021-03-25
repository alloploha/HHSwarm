using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.Messages
{
    public class MSG_CLOSE : MSG, ISerializableSessionMessage
    {
        public void CallSerializer(ISessionMessageSerializer serializer, BinaryWriter writer)
        {
            serializer.Serialize(this, writer);
        }

        public delegate void Callback(MSG_CLOSE message);
    }
}
