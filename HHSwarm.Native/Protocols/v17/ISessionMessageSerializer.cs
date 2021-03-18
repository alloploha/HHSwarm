using HHSwarm.Native.Protocols.v17.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.v17
{
    public interface ISessionMessageSerializer
    {
        void Serialize(MSG_SESS.REQUEST message, BinaryWriter writer);
        void Serialize(MSG_SESS.RESPONSE message, BinaryWriter writer);
        void Serialize(MSG_REL message, BinaryWriter writer);
        void Serialize(MSG_ACK message, BinaryWriter writer);
        void Serialize(MSG_BEAT message, BinaryWriter writer);
        void Serialize(MSG_MAPREQ message, BinaryWriter writer);
        void Serialize(MSG_MAPDATA message, BinaryWriter writer);
        void Serialize(MSG_OBJDATA message, BinaryWriter writer);
        void Serialize(MSG_OBJACK message, BinaryWriter writer);
        void Serialize(MSG_CLOSE message, BinaryWriter writer);
    }
}
