using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.v17.Messages
{
    /// <summary>
    /// Character's Attributes ("health points", "strength", "will", "masonry" etc.
    /// </summary>
    public class RMSG_CATTR : RMSG, ISerializableRelayMessage
    {
        /// <summary>
        /// 'CAttr', 'a'
        /// </summary>
        public class AttributeInfo
        {
            /// <summary>
            /// 'nm'
            /// </summary>
            public string Name;

            /// <summary>
            /// 'base'
            /// </summary>
            public int Base;

            /// <summary>
            /// 'comp'
            /// </summary>
            public int Comp;
        }

        public List<AttributeInfo> Attributes = new List<AttributeInfo>();

        public void CallSerializer(IRelayMessageSerializer serializer, BinaryWriter writer)
        {
            serializer.Serialize(this, writer);
        }

        public delegate void Callback(RMSG_CATTR message);
    }
}
