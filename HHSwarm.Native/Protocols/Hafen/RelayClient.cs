using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHSwarm.Native.Protocols.Hafen.Messages;
using static HHSwarm.Native.Protocols.TransportProtocol;
using HHSwarm.Native.WorldModel;

namespace HHSwarm.Native.Protocols.Hafen
{
    public class RelayClient : RelayProtocol
    {
        public RelayClient(CreateReaderDelegate createReader, CreateWriterDelegate createWriter, IMSG_REL_Hub session) :
            base(createReader, createWriter, session)
        {

        }

        #region RMSG_FRAGMENT
        FragmentedMessage IncomingFragments = null;

        public async override Task ReceiveAsync(RMSG_FRAGMENT message)
        {
            if (IncomingFragments == null) IncomingFragments = new FragmentedMessage();

            IncomingFragments.Push(message);

            if (IncomingFragments.IsSealed)
            {
                using (MemoryStream mem = new MemoryStream(IncomingFragments.Data))
                {
                    MessageBinaryReader reader = new MessageBinaryReader(mem, Encoding.UTF8, true);
                    await Formatter.DeserializeAsync(reader, this);
                }
                IncomingFragments = null;
            }

            await base.ReceiveAsync(message);
        } 
        #endregion
    }
}
