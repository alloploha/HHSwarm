using HHSwarm.Native.Protocols.Hafen.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen
{
    public interface IWidgetMessageSerializer
    {
        void Serialize(RMSG_NEWWDG message, BinaryWriter writer);
        void Serialize(RMSG_WDGMSG message, BinaryWriter writer);
    }
}
