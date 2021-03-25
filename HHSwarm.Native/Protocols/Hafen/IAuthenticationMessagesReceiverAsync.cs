using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHSwarm.Native.Protocols.Hafen.Messages;

namespace HHSwarm.Native.Protocols.Hafen
{
    public interface IAuthenticationMessagesReceiverAsync
    {
        Task ReceiveAsync(CMD_TOKEN.REQUEST message);
        Task ReceiveAsync(CMD_PW.REQUEST message);
        Task ReceiveAsync(CMD_MKTOKEN.REQUEST message);
        Task ReceiveAsync(CMD_COOKIE.REQUEST message);
        Task ReceiveAsync(CMD_TOKEN.RESPONSE_OK message);
        Task ReceiveAsync(CMD_PW.RESPONSE_OK message);
        Task ReceiveAsync(CMD_MKTOKEN.RESPONSE_OK message);
        Task ReceiveAsync(CMD_MKTOKEN.RESPONSE_NO message);
        Task ReceiveAsync(CMD_COOKIE.RESPONSE_OK message);
        Task ReceiveAsync(CMD_COOKIE.RESPONSE_NO message);
        Task ReceiveAsync(CMD_TOKEN.RESPONSE_NO message);
        Task ReceiveAsync(CMD_PW.RESPONSE_NO message);
    }
}
