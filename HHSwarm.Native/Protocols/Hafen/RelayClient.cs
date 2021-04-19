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
    /// <summary>
    /// В дополнение к тому, что умеет <see cref="RelayProtocol"/>, восстанавливает изначальное сообщение из фрагментов <see cref="RMSG_FRAGMENT"/>.
    /// </summary>
    public class RelayClient : RelayProtocol
    {
        public RelayClient(CreateReaderDelegate createReader, CreateWriterDelegate createWriter, IMSG_REL_Hub session) :
            base(createReader, createWriter, session)
        {

        }

        #region RMSG_FRAGMENT
        /// <summary>
        /// На практике, сервер передаёт только одно фрагментированное сообщение за раз, поэтому достаточно одного накопителя.
        /// </summary>
        FragmentedMessage IncomingFragments = null;

        /// <summary>
        /// Накапливает фрагменты сообщения в накопителе <see cref="IncomingFragments"/> до тех пор, пока не прибудут все части.
        /// После этого - восстанавливает (склеивает и распаковывает) изначальное сообщение и вызывает соответствующее событие, как будто оно пришло целиком. 
        /// </summary>
        /// <param name="message">Фрагмент сообщения</param>
        /// <returns></returns>
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
