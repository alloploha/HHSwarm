using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.Messages
{
    public class RMSG_PARTY : RMSG, ISerializableRelayMessage
    {
        public class Member
        {
            /// <summary>
            /// 'gobid'
            /// </summary>
            public int GameObjectID;

            /// <summary>
            /// 'c', 'msg.coord()'
            /// </summary>
            public Point? Coordinates;

            /// <summary>
            /// 'vis'
            /// </summary>
            public bool Visible;

            /// <summary>
            /// 'col'
            /// </summary>
            public Color? Color;
        }


        public new enum TYPE : byte
        {
            PD_LIST = 0,
            PD_LEADER = 1,
            PD_MEMBER = 2
        }

        /// <summary>
        /// 'lid', 'leader.gobid'
        /// </summary>
        public int LeaderGameObjectID;

        public List<Member> Members = new List<Member>();

        public void CallSerializer(IRelayMessageSerializer serializer, BinaryWriter writer)
        {
            serializer.Serialize(this, writer);
        }

        public delegate void Callback(RMSG_PARTY message);

    }
}
