using HHSwarm.Native.Protocols.Hafen.Messages;
using HHSwarm.Native.WorldModel;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static HHSwarm.Native.Protocols.TransportProtocol;

namespace HHSwarm.Native.Protocols.Hafen
{
    public abstract class RelayProtocol : IRelayMessagesReceiverAsync
    {
        protected TraceSource Trace = new TraceSource("HHSwarm.Relay");

        private IMSG_REL_Hub Session;
        private ushort NextSequenceNumber = 0;
        protected RelayMessageFormatter Formatter = new RelayMessageFormatter();

        private readonly CreateReaderDelegate CreateReader;
        private readonly CreateWriterDelegate CreateWriter;

        public RelayProtocol(CreateReaderDelegate createReader, CreateWriterDelegate createWriter, IMSG_REL_Hub session)
        {
            this.CreateReader = createReader;//?? throw new ArgumentNullException();
            this.CreateWriter = createWriter;//?? throw new ArgumentNullException();
            this.Session = session;//?? throw new ArgumentNullException();

            Session.MSG_REL_Received += Session_MSG_REL_Received;
        }

        private async void Session_MSG_REL_Received(MSG_REL message)
        {
            using (MemoryStream mem = new MemoryStream(message.RelayedData))
            {
                await Formatter.DeserializeAsync(CreateReader(mem, false), this);

                if (mem.Position < mem.Length) throw new Exception($"Not all data has beed read from stream and deserialized! Length of data taken is {mem.Position} bytes, but message size was {mem.Length} bytes. Check corresponding {nameof(Formatter.Deserialize)} code.");
            }
        }


        #region RMSG_NEWWDG
        public async virtual Task SendAsync(RMSG_NEWWDG message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Session.SendAsync(PackMessageToRelay(message, RMSG.TYPE.RMSG_NEWWDG));
        }

        public event RMSG_NEWWDG.Callback RMSG_NEWWDG_Received;

        public async virtual Task ReceiveAsync(RMSG_NEWWDG message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (RMSG_NEWWDG_Received != null)
                await Task.Factory.FromAsync(RMSG_NEWWDG_Received.BeginInvoke, RMSG_NEWWDG_Received.EndInvoke, message, null);
        }
        #endregion

        #region RMSG_WDGMSG
        public async virtual Task SendAsync(RMSG_WDGMSG message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Session.SendAsync(PackMessageToRelay(message, RMSG.TYPE.RMSG_WDGMSG));
        }

        public event RMSG_WDGMSG.Callback RMSG_WDGMSG_Received;

        public async virtual Task ReceiveAsync(RMSG_WDGMSG message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (RMSG_WDGMSG_Received != null)
                await Task.Factory.FromAsync(RMSG_WDGMSG_Received.BeginInvoke, RMSG_WDGMSG_Received.EndInvoke, message, null);
        }
        #endregion

        #region RMSG_DSTWDG
        public async virtual Task SendAsync(RMSG_DSTWDG message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Session.SendAsync(PackMessageToRelay(message, RMSG.TYPE.RMSG_DSTWDG));
        }

        public event RMSG_DSTWDG.Callback RMSG_DSTWDG_Received;

        public async virtual Task ReceiveAsync(RMSG_DSTWDG message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (RMSG_DSTWDG_Received != null)
                await Task.Factory.FromAsync(RMSG_DSTWDG_Received.BeginInvoke, RMSG_DSTWDG_Received.EndInvoke, message, null);
        }
        #endregion

        #region RMSG_MAPIV
        public async virtual Task SendAsync(RMSG_MAPIV message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Session.SendAsync(PackMessageToRelay(message, RMSG.TYPE.RMSG_MAPIV));
        }

        public event RMSG_MAPIV.Callback RMSG_MAPIV_Received;

        public async virtual Task ReceiveAsync(RMSG_MAPIV message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (RMSG_MAPIV_Received != null)
                await Task.Factory.FromAsync(RMSG_MAPIV_Received.BeginInvoke, RMSG_MAPIV_Received.EndInvoke, message, null);
        }
        #endregion

        #region RMSG_GLOBLOB
        public async virtual Task SendAsync(RMSG_GLOBLOB message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Session.SendAsync(PackMessageToRelay(message, RMSG.TYPE.RMSG_GLOBLOB));
        }

        public event RMSG_GLOBLOB.Callback RMSG_GLOBLOB_Received;

        public async virtual Task ReceiveAsync(RMSG_GLOBLOB message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (RMSG_GLOBLOB_Received != null)
                await Task.Factory.FromAsync(RMSG_GLOBLOB_Received.BeginInvoke, RMSG_GLOBLOB_Received.EndInvoke, message, null);
        }
        #endregion

        #region RMSG_RESID
        public async virtual Task SendAsync(RMSG_RESID message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Session.SendAsync(PackMessageToRelay(message, RMSG.TYPE.RMSG_RESID));
        }

        public event RMSG_RESID.Callback RMSG_RESID_Received;

        public async virtual Task ReceiveAsync(RMSG_RESID message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (RMSG_RESID_Received != null)
                await Task.Factory.FromAsync(RMSG_RESID_Received.BeginInvoke, RMSG_RESID_Received.EndInvoke, message, null);
        }
        #endregion

        #region RMSG_PARTY
        public async virtual Task SendAsync(RMSG_PARTY message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Session.SendAsync(PackMessageToRelay(message, RMSG.TYPE.RMSG_PARTY));
        }

        public event RMSG_PARTY.Callback RMSG_PARTY_Received;

        public async virtual Task ReceiveAsync(RMSG_PARTY message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (RMSG_PARTY_Received != null)
                await Task.Factory.FromAsync(RMSG_PARTY_Received.BeginInvoke, RMSG_PARTY_Received.EndInvoke, message, null);
        }
        #endregion

        #region RMSG_SFX
        public async virtual Task SendAsync(RMSG_SFX message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Session.SendAsync(PackMessageToRelay(message, RMSG.TYPE.RMSG_SFX));
        }

        public event RMSG_SFX.Callback RMSG_SFX_Received;

        public async virtual Task ReceiveAsync(RMSG_SFX message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (RMSG_SFX_Received != null)
                await Task.Factory.FromAsync(RMSG_SFX_Received.BeginInvoke, RMSG_SFX_Received.EndInvoke, message, null);
        }
        #endregion

        #region RMSG_CATTR
        public async virtual Task SendAsync(RMSG_CATTR message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Session.SendAsync(PackMessageToRelay(message, RMSG.TYPE.RMSG_CATTR));
        }

        public event RMSG_CATTR.Callback RMSG_CATTR_Received;

        public async virtual Task ReceiveAsync(RMSG_CATTR message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (RMSG_CATTR_Received != null)
                await Task.Factory.FromAsync(RMSG_CATTR_Received.BeginInvoke, RMSG_CATTR_Received.EndInvoke, message, null);
        }
        #endregion

        #region RMSG_MUSIC
        public async virtual Task SendAsync(RMSG_MUSIC message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Session.SendAsync(PackMessageToRelay(message, RMSG.TYPE.RMSG_MUSIC));
        }

        public event RMSG_MUSIC.Callback RMSG_MUSIC_Received;

        public async virtual Task ReceiveAsync(RMSG_MUSIC message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (RMSG_MUSIC_Received != null)
                await Task.Factory.FromAsync(RMSG_MUSIC_Received.BeginInvoke, RMSG_MUSIC_Received.EndInvoke, message, null);
        }
        #endregion

        #region RMSG_TILES
        public async virtual Task SendAsync(RMSG_TILES message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Session.SendAsync(PackMessageToRelay(message, RMSG.TYPE.RMSG_TILES));
        }

        public event RMSG_TILES.Callback RMSG_TILES_Received;

        public async virtual Task ReceiveAsync(RMSG_TILES message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (RMSG_TILES_Received != null)
                await Task.Factory.FromAsync(RMSG_TILES_Received.BeginInvoke, RMSG_TILES_Received.EndInvoke, message, null);
        }
        #endregion

        #region RMSG_SESSKEY
        public async virtual Task SendAsync(RMSG_SESSKEY message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Session.SendAsync(PackMessageToRelay(message, RMSG.TYPE.RMSG_SESSKEY));
        }

        public event RMSG_SESSKEY.Callback RMSG_SESSKEY_Received;

        public async virtual Task ReceiveAsync(RMSG_SESSKEY message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (RMSG_SESSKEY_Received != null)
                await Task.Factory.FromAsync(RMSG_SESSKEY_Received.BeginInvoke, RMSG_SESSKEY_Received.EndInvoke, message, null);
        }
        #endregion

        #region RMSG_FRAGMENT
        public async virtual Task SendAsync(RMSG_FRAGMENT message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Session.SendAsync(PackMessageToRelay(message, RMSG.TYPE.RMSG_FRAGMENT));
        }

        public event RMSG_FRAGMENT.Callback RMSG_FRAGMENT_Received;

        public async virtual Task ReceiveAsync(RMSG_FRAGMENT message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (RMSG_FRAGMENT_Received != null)
                await Task.Factory.FromAsync(RMSG_FRAGMENT_Received.BeginInvoke, RMSG_FRAGMENT_Received.EndInvoke, message, null);
        }
        #endregion

        #region RMSG_ADDWDG
        public async virtual Task SendAsync(RMSG_ADDWDG message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Session.SendAsync(PackMessageToRelay(message, RMSG.TYPE.RMSG_ADDWDG));
        }

        public event RMSG_ADDWDG.Callback RMSG_ADDWDG_Received;

        public async virtual Task ReceiveAsync(RMSG_ADDWDG message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (RMSG_ADDWDG_Received != null)
                await Task.Factory.FromAsync(RMSG_ADDWDG_Received.BeginInvoke, RMSG_ADDWDG_Received.EndInvoke, message, null);
        }
        #endregion

        private MSG_REL PackMessageToRelay(object message, RMSG.TYPE type)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                BinaryWriter writer = CreateWriter(mem, true);

                Formatter.Serialize(message, writer);
                writer.Flush();

                return new MSG_REL()
                {
                    SequenceNumber = NextSequenceNumber++,
                    RelayedData = mem.ToArray()
                };
            }
        }
    }
}
