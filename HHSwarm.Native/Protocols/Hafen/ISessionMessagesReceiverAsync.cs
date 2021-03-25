using HHSwarm.Native.Protocols.Hafen.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen
{
    public interface ISessionMessagesReceiverAsync
    {
        Task ReceiveAsync(MSG_SESS.REQUEST message);
        Task ReceiveAsync(MSG_SESS.RESPONSE message);
        Task ReceiveAsync(MSG_REL message);
        Task ReceiveAsync(MSG_ACK message);
        Task ReceiveAsync(MSG_BEAT message);
        Task ReceiveAsync(MSG_MAPREQ message);
        Task ReceiveAsync(MSG_MAPDATA message);
        Task ReceiveAsync(MSG_OBJDATA message);
        Task ReceiveAsync(MSG_OBJACK message);
        Task ReceiveAsync(MSG_CLOSE message);
    }
}
