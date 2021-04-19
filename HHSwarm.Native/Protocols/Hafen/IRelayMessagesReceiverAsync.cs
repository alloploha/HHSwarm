using HHSwarm.Native.Protocols.Hafen.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen
{
    public interface IRelayMessagesReceiverAsync
    {
        Task ReceiveAsync(RMSG_NEWWDG message);
        Task ReceiveAsync(RMSG_WDGMSG message);
        Task ReceiveAsync(RMSG_DSTWDG message);
        Task ReceiveAsync(RMSG_MAPIV message);
        Task ReceiveAsync(RMSG_GLOBLOB message);
        Task ReceiveAsync(RMSG_RESID message);
        Task ReceiveAsync(RMSG_PARTY message);
        Task ReceiveAsync(RMSG_SFX message);
        Task ReceiveAsync(RMSG_CATTR message);
        Task ReceiveAsync(RMSG_MUSIC message);
        Task ReceiveAsync(RMSG_TILES message);
        Task ReceiveAsync(RMSG_SESSKEY message);
        Task ReceiveAsync(RMSG_FRAGMENT message);
        Task ReceiveAsync(RMSG_ADDWDG message);
    }
}
