using HHSwarm.Native.WorldModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.v17.Messages
{
    public class MSG_OBJDATA : MSG, ISerializableSessionMessage
    {
        public new enum TYPE : byte
        {
            OD_REM = 0,
            OD_MOVE = 1,
            OD_RES = 2,
            OD_LINBEG = 3,
            OD_LINSTEP = 4,
            OD_SPEECH = 5,
            OD_COMPOSE = 6,
            OD_ZOFF = 7,
            OD_LUMIN = 8,
            OD_AVATAR = 9,
            OD_FOLLOW = 10,
            OD_HOMING = 11,
            OD_OVERLAY = 12,
            [Obsolete]
            OD_AUTH = 13,
            OD_HEALTH = 14,
            OD_BUDDY = 15,
            OD_CMPPOSE = 16,
            OD_CMPMOD = 17,
            OD_CMPEQU = 18,
            OD_ICON = 19,
            OD_RESATTR = 20,
            OD_END = 255
        }

        [Flags]
        public enum FLAGS : byte
        {
            RemoveFromCache = 0x01,
            ObjectIsVirtual = 0x02
        }

        public class GameObjectDataFrame
        {
            public uint ObjectDataID;
            public int FrameID;
            public FLAGS Flags;
            public GameObjectData ObjectsData;
        }

        public List<GameObjectDataFrame> DataFrames = new List<GameObjectDataFrame>();

        public void CallSerializer(ISessionMessageSerializer serializer, BinaryWriter writer)
        {
            serializer.Serialize(this, writer);
        }

        public delegate void Callback(MSG_OBJDATA message);
    }
}
