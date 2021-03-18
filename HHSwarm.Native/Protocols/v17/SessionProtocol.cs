using HHSwarm.Native.Protocols.v17.Messages;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.v17
{
    /// <summary>
    /// OSI Session Layer (#5)
    /// </summary>
    public abstract class SessionProtocol : ISessionMessagesReceiverAsync, IMSG_REL_Hub
    {
        protected TraceSource Trace = new TraceSource("HHSwarm.Session");

        public enum SessionContext
        {
            Client,
            Server
        }

        protected TransportProtocol Transport;
        private SessionMessageFormatter Formatter;

        public SessionProtocol(TransportProtocol transport, SessionContext context)
        {
            this.Transport = transport;//?? throw new ArgumentNullException();
            this.Formatter = new SessionMessageFormatter(context);
            Transport.DataReceived += Transport_DataReceived;
        }

        private async void Transport_DataReceived(BinaryReader reader, IPEndPoint remoteEndPoint)
        {
            await Formatter.DeserializeAsync(reader, this);
        }

        //private Dictionary<int, Queue<MSG_MAPDATA>> AccumulatedMapSectionFragments = new Dictionary<int, Queue<MSG_MAPDATA>>();

        //private void ProcessMapSectionFragment(object sender, MSG_MAPDATA fragment)
        //{
        //    if (!AccumulatedMapSectionFragments.ContainsKey(fragment.pktid))
        //        AccumulatedMapSectionFragments.Add(fragment.pktid, new Queue<MSG_MAPDATA>());

        //    Queue<MSG_MAPDATA> fragments = AccumulatedMapSectionFragments[fragment.pktid];
        //    fragments.Enqueue(fragment);
        //}

        //private Dictionary<ushort, MSG_REL> IncomingRelayedMessagesQueue = new Dictionary<ushort, MSG_REL>();
        //private ushort LastIncomingRelayedMessageSequenceNumber = 0;

        //private void DelayedRelayMessageProcessing(object sender, MSG_REL relayMessage)
        //{
        //    IncomingRelayedMessagesQueue[relayMessage.Sequence] = relayMessage;

        //    for
        //    (
        //        ushort nextSequenceNumber = (ushort)(LastIncomingRelayedMessageSequenceNumber + 1);
        //        IncomingRelayedMessagesQueue.ContainsKey(nextSequenceNumber);
        //        nextSequenceNumber++
        //    )
        //    {
        //        RelayMessageReceived?.Invoke(this, IncomingRelayedMessagesQueue[nextSequenceNumber]);
        //        IncomingRelayedMessagesQueue.Remove(nextSequenceNumber);
        //        LastIncomingRelayedMessageSequenceNumber = nextSequenceNumber;
        //    }
        //}



        //private void UnwrapRelayedInnerMessage(object sender, MSG_REL relayMessage)
        //{
        //    DispatchRelayedInnerMessage(relayMessage.DataType, relayMessage.Data);
        //}

        //private FragmentedMessage FragmentedMessage = new FragmentedMessage();

        //private void DispatchArrivedMessageToEventHandlers(object sender, UdpReceiveResult e)
        //{
        //    using (MemoryStream mem = new MemoryStream(e.Buffer))
        //    {
        //        object graph = Formatter.Deserialize(mem);

        //        if (graph is MSG_SESS_ClientRequest)
        //        {
        //            MSG_SESS_ClientRequest msg = (MSG_SESS_ClientRequest)graph;
        //            ConnectionRequested?.Invoke(this, msg);
        //        }
        //        else if (graph is MSG_SESS_ServerResponse)
        //        {
        //            MSG_SESS_ServerResponse msg = (MSG_SESS_ServerResponse)graph;
        //            if (msg.ErrorCode == MSG_SESS_ServerErrorCode.SUCCESS)
        //                Connected?.Invoke(this, EventArgs.Empty);
        //            else
        //                ConnectionFailed?.Invoke(this, msg.ErrorCode);
        //        }
        //        else if (graph is MSG_REL)
        //        {
        //            MSG_REL msg = (MSG_REL)graph;
        //            if (msg.DataType == MSG_REL.TYPE.RMSG_FRAGMENT)
        //            {
        //                RMSG_FRAGMENT fragment = (RMSG_FRAGMENT)Formatter.Deserialize(msg.DataType, msg.Data);

        //                FragmentedMessage.Push(fragment);

        //                if (FragmentedMessage.IsSealed)
        //                {
        //                    if (!FragmentedMessage.Type.HasValue) throw new InvalidDataException();

        //                    Received?.Invoke(this, (RMSG)Formatter.Deserialize(FragmentedMessage.Type.Value, FragmentedMessage.Data));
        //                    FragmentedMessage = new FragmentedMessage();
        //                }
        //            }
        //            else
        //            {
        //                Received?.Invoke(this, (RMSG)Formatter.Deserialize(msg.DataType, msg.Data));
        //            }
        //        }
        //        //else if (graph is MSG_MAPDATA)
        //        //{
        //        //    MSG_MAPDATA msg = (MSG_MAPDATA)graph;
        //        //    MapDataReceived?.Invoke(this, msg);
        //        //}
        //        //else if (graph is MSG_OBJDATA)
        //        //{
        //        //    MSG_OBJDATA msg = (MSG_OBJDATA)graph;
        //        //    ObjectDataReceived?.Invoke(this, msg);
        //        //}
        //        else if (graph is MSG_CLOSE)
        //        {
        //            MSG_CLOSE msg = (MSG_CLOSE)graph;
        //            DisconnectionRequested?.Invoke(this, msg);
        //        }
        //        else
        //            throw new NotImplementedException();
        //    }
        //}

        //public event EventHandler<MSG_SESS_ClientRequest> ConnectionRequested;
        //public event EventHandler Connected;
        //public event EventHandler<MSG_SESS_ServerErrorCode> ConnectionFailed;
        //public event EventHandler<MSG_REL> RelayMessageReceiving;
        //public event EventHandler<MSG_REL> RelayMessageReceived;


        //public async Task ConnectAsync()
        //{
        //    using (MemoryStream mem = new MemoryStream())
        //    {
        //        Formatter.Serialize(mem, new MSG_SESS_ClientRequest());
        //        await Transport.SendAsync(mem.ToArray());
        //    }
        //}

        //public async Task CloseAsync()
        //{
        //    await Transport.CloseAsync();
        //}

        #region MSG_REL

        public virtual async Task SendAsync(MSG_REL message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Transport.SendAsync(writer => Formatter.Serialize(message, writer));
        }

        public event MSG_REL.Callback MSG_REL_Received;

        public virtual async Task ReceiveAsync(MSG_REL message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (MSG_REL_Received != null)
                await Task.Factory.FromAsync(MSG_REL_Received.BeginInvoke, MSG_REL_Received.EndInvoke, message, null);
        }

        #endregion

        #region MSG_SESS.REQUEST
        public virtual async Task SendAsync(MSG_SESS.REQUEST message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Transport.SendAsync(writer => Formatter.Serialize(message, writer));
        }

        public event MSG_SESS.REQUEST.Callback MSG_SESS_REQUEST_Received;

        public virtual async Task ReceiveAsync(MSG_SESS.REQUEST message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (MSG_SESS_REQUEST_Received != null)
                await Task.Factory.FromAsync(MSG_SESS_REQUEST_Received.BeginInvoke, MSG_SESS_REQUEST_Received.EndInvoke, message, null);
        }
        #endregion

        #region MSG_SESS.RESPONSE
        public virtual async Task SendAsync(MSG_SESS.RESPONSE message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Transport.SendAsync(writer => Formatter.Serialize(message, writer));
        }

        public event MSG_SESS.RESPONSE.Callback MSG_SESS_RESPONSE_Received;

        public virtual async Task ReceiveAsync(MSG_SESS.RESPONSE message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (MSG_SESS_RESPONSE_Received != null)
                await Task.Factory.FromAsync(MSG_SESS_RESPONSE_Received.BeginInvoke, MSG_SESS_RESPONSE_Received.EndInvoke, message, null);
        }
        #endregion

        #region MSG_ACK
        public virtual async Task SendAsync(MSG_ACK message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Transport.SendAsync(writer => Formatter.Serialize(message, writer));
        }

        public event MSG_ACK.Callback MSG_ACK_Received;

        public virtual async Task ReceiveAsync(MSG_ACK message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (MSG_ACK_Received != null)
                await Task.Factory.FromAsync(MSG_ACK_Received.BeginInvoke, MSG_ACK_Received.EndInvoke, message, null);
        }
        #endregion

        #region MSG_BEAT
        public virtual async Task SendAsync(MSG_BEAT message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Transport.SendAsync(writer => Formatter.Serialize(message, writer));
        }

        public event MSG_BEAT.Callback MSG_BEAT_Received;

        public virtual async Task ReceiveAsync(MSG_BEAT message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (MSG_BEAT_Received != null)
                await Task.Factory.FromAsync(MSG_BEAT_Received.BeginInvoke, MSG_BEAT_Received.EndInvoke, message, null);
        }
        #endregion

        #region MSG_MAPREQ
        public virtual async Task SendAsync(MSG_MAPREQ message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Transport.SendAsync(writer => Formatter.Serialize(message, writer));
        }

        public event MSG_MAPREQ.Callback MSG_MAPREQ_Received;

        public virtual async Task ReceiveAsync(MSG_MAPREQ message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (MSG_MAPREQ_Received != null)
                await Task.Factory.FromAsync(MSG_MAPREQ_Received.BeginInvoke, MSG_MAPREQ_Received.EndInvoke, message, null);
        }
        #endregion

        #region MSG_MAPDATA
        public virtual async Task SendAsync(MSG_MAPDATA message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Transport.SendAsync(writer => Formatter.Serialize(message, writer));
        }

        public event MSG_MAPDATA.Callback MSG_MAPDATA_Received;

        public virtual async Task ReceiveAsync(MSG_MAPDATA message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (MSG_MAPDATA_Received != null)
                await Task.Factory.FromAsync(MSG_MAPDATA_Received.BeginInvoke, MSG_MAPDATA_Received.EndInvoke, message, null);
        }
        #endregion

        #region MSG_OBJDATA
        public virtual async Task SendAsync(MSG_OBJDATA message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Transport.SendAsync(writer => Formatter.Serialize(message, writer));
        }

        public event MSG_OBJDATA.Callback MSG_OBJDATA_Received;

        public virtual async Task ReceiveAsync(MSG_OBJDATA message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (MSG_OBJDATA_Received != null)
                await Task.Factory.FromAsync(MSG_OBJDATA_Received.BeginInvoke, MSG_OBJDATA_Received.EndInvoke, message, null);
        }
        #endregion

        #region MSG_OBJACK
        public virtual async Task SendAsync(MSG_OBJACK message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Transport.SendAsync(writer => Formatter.Serialize(message, writer));
        }

        public event MSG_OBJACK.Callback MSG_OBJACK_Received;

        public virtual async Task ReceiveAsync(MSG_OBJACK message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (MSG_OBJACK_Received != null)
                await Task.Factory.FromAsync(MSG_OBJACK_Received.BeginInvoke, MSG_OBJACK_Received.EndInvoke, message, null);
        }
        #endregion

        #region MSG_CLOSE
        public virtual async Task SendAsync(MSG_CLOSE message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await Transport.SendAsync(writer => Formatter.Serialize(message, writer));
        }

        public event MSG_CLOSE.Callback MSG_CLOSE_Received;

        public virtual async Task ReceiveAsync(MSG_CLOSE message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (MSG_CLOSE_Received != null)
                await Task.Factory.FromAsync(MSG_CLOSE_Received.BeginInvoke, MSG_CLOSE_Received.EndInvoke, message, null);
        }
        #endregion

    }
}
