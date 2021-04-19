using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.Messages
{
    public class MSG_OBJACK : MSG, ISerializableSessionMessage
    {
        public class GameObjectDataFrame
        {
            /// <summary>
            /// 'id'
            /// </summary>
            public uint ObjectDataID;

            /// <summary>
            /// 'frame'
            /// </summary>
            public int FrameID;
        }

        public List<GameObjectDataFrame> DataFrames = new List<GameObjectDataFrame>();

        public void CallSerializer(ISessionMessageSerializer serializer, BinaryWriter writer)
        {
            serializer.Serialize(this, writer);
        }

        public delegate void Callback(MSG_OBJACK message);
    }
}
