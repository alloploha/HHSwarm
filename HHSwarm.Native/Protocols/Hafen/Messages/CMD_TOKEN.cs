using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.Messages
{
    public class CMD_TOKEN : CMD
    {
        public class REQUEST : CMD_TOKEN, ISerializableAuthenticationMessage
        {
            public string TokenName;
            public byte[] Token;

            public void CallSerializer(IAuthenticationMessageSerializer serializer, BinaryWriter writer)
            {
                serializer.Serialize(this, writer);
            }

            public delegate void Callback(REQUEST message);
        }

        public class RESPONSE_OK : CMD_TOKEN, ISerializableAuthenticationMessage
        {
            public string AccountName;

            public void CallSerializer(IAuthenticationMessageSerializer serializer, BinaryWriter writer)
            {
                serializer.Serialize(this, writer);
            }

            public delegate void Callback(RESPONSE_OK message);
        }

        public class RESPONSE_NO : CMD_TOKEN, ISerializableAuthenticationMessage
        {
            public string ErrorMessage;

            public void CallSerializer(IAuthenticationMessageSerializer serializer, BinaryWriter writer)
            {
                serializer.Serialize(this, writer);
            }

            public delegate void Callback(RESPONSE_NO message);
        }
    }
}
