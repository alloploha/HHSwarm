using System.Threading.Tasks;
using HHSwarm.Native.Protocols.Hafen.Messages;

namespace HHSwarm.Native.Protocols.Hafen
{
    public interface IMSG_REL_Hub
    {
        event MSG_REL.Callback MSG_REL_Received;

        Task ReceiveAsync(MSG_REL message);
        Task SendAsync(MSG_REL message);
    }
}