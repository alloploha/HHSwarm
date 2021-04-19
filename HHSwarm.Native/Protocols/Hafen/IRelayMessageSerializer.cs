using HHSwarm.Native.Protocols.Hafen.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen
{
    public interface IRelayMessageSerializer
    {
        void Serialize(RMSG_ADDWDG message, BinaryWriter writer);
        void Serialize(RMSG_CATTR message, BinaryWriter writer);
        void Serialize(RMSG_DSTWDG message, BinaryWriter writer);
        void Serialize(RMSG_FRAGMENT message, BinaryWriter writer);
        void Serialize(RMSG_GLOBLOB message, BinaryWriter writer);
        void Serialize(RMSG_MAPIV message, BinaryWriter writer);
        void Serialize(RMSG_MUSIC message, BinaryWriter writer);
        void Serialize(RMSG_NEWWDG message, BinaryWriter writer);
        void Serialize(RMSG_PARTY message, BinaryWriter writer);
        void Serialize(RMSG_RESID message, BinaryWriter writer);
        void Serialize(RMSG_SESSKEY message, BinaryWriter writer);
        void Serialize(RMSG_SFX message, BinaryWriter writer);
        void Serialize(RMSG_TILES message, BinaryWriter writer);
        void Serialize(RMSG_WDGMSG message, BinaryWriter writer);
    }
}
