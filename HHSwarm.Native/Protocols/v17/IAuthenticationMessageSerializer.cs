using HHSwarm.Native.Protocols.v17.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.v17
{
    public interface IAuthenticationMessageSerializer
    {
        void Serialize(CMD_COOKIE.REQUEST message, BinaryWriter writer);
        void Serialize(CMD_COOKIE.RESPONSE_OK message, BinaryWriter writer);
        void Serialize(CMD_COOKIE.RESPONSE_NO message, BinaryWriter writer);
        void Serialize(CMD_PW.REQUEST message, BinaryWriter writer);
        void Serialize(CMD_PW.RESPONSE_OK message, BinaryWriter writer);
        void Serialize(CMD_PW.RESPONSE_NO message, BinaryWriter writer);
        void Serialize(CMD_MKTOKEN.REQUEST message, BinaryWriter writer);
        void Serialize(CMD_MKTOKEN.RESPONSE_OK message, BinaryWriter writer);
        void Serialize(CMD_MKTOKEN.RESPONSE_NO message, BinaryWriter writer);
        void Serialize(CMD_TOKEN.REQUEST message, BinaryWriter writer);
        void Serialize(CMD_TOKEN.RESPONSE_OK message, BinaryWriter writer);
        void Serialize(CMD_TOKEN.RESPONSE_NO message, BinaryWriter writer);
    }
}
