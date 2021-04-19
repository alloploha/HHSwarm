using HHSwarm.Native.Protocols.Hafen.Messages;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen
{
    /// <summary>
    /// OSI Session Layer (#5)
    /// </summary>
    public abstract class AuthenticationProtocol : IAuthenticationMessagesReceiverAsync
    {
        protected TraceSource Trace = new TraceSource("HHSwarm.Authentication");

        public enum AuthenticationContext
        {
            Unknown,
            Token,
            Pw,
            Mktoken,
            Cookie
        }

        TransportProtocol Transport;
        AuthenticationMessageFormatter Formatter = new AuthenticationMessageFormatter();

        public AuthenticationProtocol(TransportProtocol transport)
        {
            this.Transport = transport;//?? throw new ArgumentNullException();
            Transport.DataReceived += Transport_DataReceived;
        }

        private async void Transport_DataReceived(BinaryReader reader, IPEndPoint remoteEndPoint)
        {
            long dataAvailableSize = reader.BaseStream.Length;

            MessageBinaryReader header_reader = new MessageBinaryReader(reader.BaseStream, Encoding.UTF8, true);

            while (dataAvailableSize > sizeof(ushort))
            {
                long reader_position = reader.BaseStream.Position;
                reader.BaseStream.Position = 0;

                ushort message_size = header_reader.ReadUInt16();

                if (message_size <= dataAvailableSize)
                {
                    try
                    {
                        await Formatter.DeserializeAsync(reader, this);
                        byte[] deserialized_bytes = reader.BaseStream.Dequeue();
                        dataAvailableSize = reader.BaseStream.Length;
                        if (deserialized_bytes.Length != sizeof(ushort) + message_size) throw new Exception($"Not all or excess of data has beed read from stream and deserialized! Length of data taken is {deserialized_bytes.Length} bytes, but message size was {message_size} bytes. Check corresponding {nameof(Formatter.Deserialize)} code.");
                    }
                    catch
                    {
                        reader.BaseStream.Position = reader_position;
                        throw;
                    }
                }
                else
                {
                    reader.BaseStream.Position = reader_position;
                    break;
                }
            }
        }

        private async Task SendMessageByTransportAsync(Action<BinaryWriter> SerializeMessage)
        {
            using (MemoryStream message_mem = new MemoryStream())
            {
                BinaryWriter memory_writer = Transport.CreateWriter(message_mem, false);
                SerializeMessage(memory_writer);

                using (MemoryStream header_mem = new MemoryStream())
                {
                    MessageBinaryWriter header_writer = new MessageBinaryWriter(header_mem, Encoding.UTF8, true);
                    header_writer.Write((ushort)message_mem.Length);

                    await Transport.SendAsync(transport_writer =>
                    {
                        transport_writer.Write(header_mem.ToArray());
                        transport_writer.Write(message_mem.ToArray());
                    });
                }
            }
        }

        #region CMD_PW.REQUEST
        public async virtual Task SendAsync(CMD_PW.REQUEST message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await SendMessageByTransportAsync(writer => Formatter.Serialize(message, writer));
        }

        public event CMD_PW.REQUEST.Callback CMD_PW_REQUEST_Received;

        public async virtual Task ReceiveAsync(CMD_PW.REQUEST message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (CMD_PW_REQUEST_Received != null)
                await Task.Factory.FromAsync(CMD_PW_REQUEST_Received.BeginInvoke, CMD_PW_REQUEST_Received.EndInvoke, message, null);
        }
        #endregion

        #region CMD_PW.RESPONSE_OK
        public async virtual Task SendAsync(CMD_PW.RESPONSE_OK message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await SendMessageByTransportAsync(writer => Formatter.Serialize(message, writer));
        }

        public event CMD_PW.RESPONSE_OK.Callback CMD_PW_RESPONSE_OK_Received;

        public async virtual Task ReceiveAsync(CMD_PW.RESPONSE_OK message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (CMD_PW_RESPONSE_OK_Received != null)
                await Task.Factory.FromAsync(CMD_PW_RESPONSE_OK_Received.BeginInvoke, CMD_PW_RESPONSE_OK_Received.EndInvoke, message, null);
        }
        #endregion

        #region CMD_PW.RESPONSE_NO
        public async virtual Task SendAsync(CMD_PW.RESPONSE_NO message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await SendMessageByTransportAsync(writer => Formatter.Serialize(message, writer));
        }

        public event CMD_PW.RESPONSE_NO.Callback CMD_PW_RESPONSE_NO_Received;

        public async virtual Task ReceiveAsync(CMD_PW.RESPONSE_NO message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (CMD_PW_RESPONSE_NO_Received != null)
                await Task.Factory.FromAsync(CMD_PW_RESPONSE_NO_Received.BeginInvoke, CMD_PW_RESPONSE_NO_Received.EndInvoke, message, null);
        }
        #endregion

        #region CMD_TOKEN.REQUEST
        public async virtual Task SendAsync(CMD_TOKEN.REQUEST message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await SendMessageByTransportAsync(writer => Formatter.Serialize(message, writer));
        }

        public event CMD_TOKEN.REQUEST.Callback CMD_TOKEN_REQUEST_Received;

        public async virtual Task ReceiveAsync(CMD_TOKEN.REQUEST message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (CMD_TOKEN_REQUEST_Received != null)
                await Task.Factory.FromAsync(CMD_TOKEN_REQUEST_Received.BeginInvoke, CMD_TOKEN_REQUEST_Received.EndInvoke, message, null);
        }
        #endregion

        #region CMD_TOKEN.RESPONSE_OK
        public async virtual Task SendAsync(CMD_TOKEN.RESPONSE_OK message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await SendMessageByTransportAsync(writer => Formatter.Serialize(message, writer));
        }

        public event CMD_TOKEN.RESPONSE_OK.Callback CMD_TOKEN_RESPONSE_OK_Received;

        public async virtual Task ReceiveAsync(CMD_TOKEN.RESPONSE_OK message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (CMD_TOKEN_RESPONSE_OK_Received != null)
                await Task.Factory.FromAsync(CMD_TOKEN_RESPONSE_OK_Received.BeginInvoke, CMD_TOKEN_RESPONSE_OK_Received.EndInvoke, message, null);
        }
        #endregion

        #region CMD_TOKEN.RESPONSE_NO
        public async virtual Task SendAsync(CMD_TOKEN.RESPONSE_NO message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await SendMessageByTransportAsync(writer => Formatter.Serialize(message, writer));
        }

        public event CMD_TOKEN.RESPONSE_NO.Callback CMD_TOKEN_RESPONSE_NO_Received;

        public async virtual Task ReceiveAsync(CMD_TOKEN.RESPONSE_NO message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (CMD_TOKEN_RESPONSE_NO_Received != null)
                await Task.Factory.FromAsync(CMD_TOKEN_RESPONSE_NO_Received.BeginInvoke, CMD_TOKEN_RESPONSE_NO_Received.EndInvoke, message, null);
        }
        #endregion

        #region CMD_MKTOKEN.REQUEST
        public async virtual Task SendAsync(CMD_MKTOKEN.REQUEST message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await SendMessageByTransportAsync(writer => Formatter.Serialize(message, writer));
        }

        public event CMD_MKTOKEN.REQUEST.Callback CMD_MKTOKEN_REQUEST_Received;

        public async virtual Task ReceiveAsync(CMD_MKTOKEN.REQUEST message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (CMD_MKTOKEN_REQUEST_Received != null)
                await Task.Factory.FromAsync(CMD_MKTOKEN_REQUEST_Received.BeginInvoke, CMD_MKTOKEN_REQUEST_Received.EndInvoke, message, null);
        }
        #endregion

        #region CMD_MKTOKEN.RESPONSE_OK
        public async virtual Task SendAsync(CMD_MKTOKEN.RESPONSE_OK message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await SendMessageByTransportAsync(writer => Formatter.Serialize(message, writer));
        }

        public event CMD_MKTOKEN.RESPONSE_OK.Callback CMD_MKTOKEN_RESPONSE_OK_Received;

        public async virtual Task ReceiveAsync(CMD_MKTOKEN.RESPONSE_OK message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (CMD_MKTOKEN_RESPONSE_OK_Received != null)
                await Task.Factory.FromAsync(CMD_MKTOKEN_RESPONSE_OK_Received.BeginInvoke, CMD_MKTOKEN_RESPONSE_OK_Received.EndInvoke, message, null);
        }
        #endregion

        #region CMD_MKTOKEN.RESPONSE_NO
        public async virtual Task SendAsync(CMD_MKTOKEN.RESPONSE_NO message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await SendMessageByTransportAsync(writer => Formatter.Serialize(message, writer));
        }

        public event CMD_MKTOKEN.RESPONSE_NO.Callback CMD_MKTOKEN_RESPONSE_NO_Received;

        public async virtual Task ReceiveAsync(CMD_MKTOKEN.RESPONSE_NO message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (CMD_MKTOKEN_RESPONSE_NO_Received != null)
                await Task.Factory.FromAsync(CMD_MKTOKEN_RESPONSE_NO_Received.BeginInvoke, CMD_MKTOKEN_RESPONSE_NO_Received.EndInvoke, message, null);
        }
        #endregion

        #region CMD_COOKIE.REQUEST
        public async virtual Task SendAsync(CMD_COOKIE.REQUEST message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await SendMessageByTransportAsync(writer => Formatter.Serialize(message, writer));
        }

        public event CMD_COOKIE.REQUEST.Callback CMD_COOKIE_Received;

        public async virtual Task ReceiveAsync(CMD_COOKIE.REQUEST message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (CMD_COOKIE_Received != null)
                await Task.Factory.FromAsync(CMD_COOKIE_Received.BeginInvoke, CMD_COOKIE_Received.EndInvoke, message, null);
        }
        #endregion

        #region CMD_COOKIE.RESPONSE_OK
        public async virtual Task SendAsync(CMD_COOKIE.RESPONSE_OK message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await SendMessageByTransportAsync(writer => Formatter.Serialize(message, writer));
        }

        public event CMD_COOKIE.RESPONSE_OK.Callback CMD_COOKIE_RESPONSE_OK_Received;

        public async virtual Task ReceiveAsync(CMD_COOKIE.RESPONSE_OK message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (CMD_COOKIE_RESPONSE_OK_Received != null)
                await Task.Factory.FromAsync(CMD_COOKIE_RESPONSE_OK_Received.BeginInvoke, CMD_COOKIE_RESPONSE_OK_Received.EndInvoke, message, null);
        }
        #endregion

        #region CMD_COOKIE.RESPONSE_NO
        public async virtual Task SendAsync(CMD_COOKIE.RESPONSE_NO message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(SendAsync), message);
            await SendMessageByTransportAsync(writer => Formatter.Serialize(message, writer));
        }

        public event CMD_COOKIE.RESPONSE_NO.Callback CMD_COOKIE_RESPONSE_NO_Received;

        public async virtual Task ReceiveAsync(CMD_COOKIE.RESPONSE_NO message)
        {
            Trace.Dump(TraceEventType.Verbose, nameof(ReceiveAsync), message);
            if (CMD_COOKIE_RESPONSE_NO_Received != null)
                await Task.Factory.FromAsync(CMD_COOKIE_RESPONSE_NO_Received.BeginInvoke, CMD_COOKIE_RESPONSE_NO_Received.EndInvoke, message, null);
        }
        #endregion
    }
}
