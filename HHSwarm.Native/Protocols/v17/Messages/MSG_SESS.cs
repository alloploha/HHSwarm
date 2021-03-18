using System.IO;

namespace HHSwarm.Native.Protocols.v17.Messages
{
    /// <summary>
    /// MSG_SESS = 0
    /// </summary>
    public abstract class MSG_SESS : MSG
    {

        public class REQUEST : MSG_SESS, ISerializableSessionMessage
        {
            public ushort Reserved = 2;
            public string ProtocolName = @"Hafen/default";
            public ushort ProtocolVersion = 17;
            public string AccountName;
            public byte[] Cookie;

            public void CallSerializer(ISessionMessageSerializer serializer, BinaryWriter writer)
            {
                serializer.Serialize(this, writer);
            }

            public delegate void Callback(REQUEST message);
        }

        public class RESPONSE : MSG_SESS, ISerializableSessionMessage
        {
            public enum ERROR_CODE : byte
            {
                SUCCESS = 0,

                /// <summary>
                /// Invalid authentication token.
                /// </summary>
                SESSERR_AUTH = 1,

                /// <summary>
                /// Already logged in.
                /// </summary>
                SESSERR_BUSY = 2,

                /// <summary>
                /// Could not connect to server.
                /// </summary>
                SESSERR_CONN = 3,

                /// <summary>
                /// This client is too old.
                /// </summary>
                SESSERR_PVER = 4,

                /// <summary>
                /// Authentication token expired.
                /// </summary>
                SESSERR_EXPR = 5
            }

            public ERROR_CODE ErrorCode;

            public void CallSerializer(ISessionMessageSerializer serializer, BinaryWriter writer)
            {
                serializer.Serialize(this, writer);
            }

            public delegate void Callback(RESPONSE message);
        }
    }
}
