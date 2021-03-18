using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.v17.Messages
{
    public class CMD_PW : CMD
    {
        public class REQUEST : CMD_PW, ISerializableAuthenticationMessage
        {
            /// <summary>
            /// a.k.a. UserName
            /// </summary>
            public string LoginName;
            public byte[] PasswordHash;

            public void CallSerializer(IAuthenticationMessageSerializer serializer, BinaryWriter writer)
            {
                serializer.Serialize(this, writer);
            }

            public delegate void Callback(REQUEST message);
        }

        public class RESPONSE_OK : CMD_PW, ISerializableAuthenticationMessage
        {
            public string TokenName;

            public void CallSerializer(IAuthenticationMessageSerializer serializer, BinaryWriter writer)
            {
                serializer.Serialize(this, writer);
            }

            public delegate void Callback(RESPONSE_OK message);
        }

        public class RESPONSE_NO : CMD_PW, ISerializableAuthenticationMessage
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
