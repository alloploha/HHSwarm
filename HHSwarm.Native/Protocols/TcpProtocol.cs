using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols
{
    /// <summary>
    /// OSI Transport Level (#4)
    /// </summary>
    public abstract class TcpProtocol : TransportProtocol, IDisposable
    {
        protected Dictionary<IPEndPoint, System.Net.Sockets.TcpClient> Clients = new Dictionary<IPEndPoint, System.Net.Sockets.TcpClient>();

        protected static AddressFamily[] SupportedNetworkProtocols = new AddressFamily[] { AddressFamily.InterNetwork, AddressFamily.InterNetworkV6 };

        public TcpProtocol(CreateReaderDelegate createReader, CreateWriterDelegate createWriter) : base(createReader, createWriter)
        {
#if DEBUG
            DataReceived += TcpProtocol_TraceDataReceived;
#endif
        }

        private void TcpProtocol_TraceDataReceived(BinaryReader reader, IPEndPoint remoteEndPoint)
        {
            MemoryStream mem = new MemoryStream();
            long position = reader.BaseStream.Position;
            reader.BaseStream.CopyTo(mem);
            reader.BaseStream.Position = position;
            Trace.Dump(TraceEventType.Verbose, $"{nameof(TcpProtocol)}.{nameof(DataReceived)}", mem.ToArray());
        }

        /// <summary>
        /// https://social.msdn.microsoft.com/Forums/vstudio/en-US/aa49f92c-01a8-4901-9846-91bc1587f3ae/countdownevent-initialcount-of-zero?forum=parallelextensions
        /// </summary>
        protected CountdownEvent SendCompleted = new CountdownEvent(1);

        public async override Task CloseAsync()
        {
            SendCompleted.Signal();

            if (!CancellationSource.IsCancellationRequested)
                CancellationSource.Cancel();

            await Task.Run(() =>
            {
                if (!SendCompleted.IsSet)
                    SendCompleted.Wait();

                foreach (var c in Clients)
                {
                    if (c.Value.Connected)
                        c.Value.Close();
                }
            });
        }

        public override async Task SendAsync(Action<BinaryWriter> write)
        {
            await Task.WhenAll(Clients.Select(c => SendAsync(write, c.Value, CancellationSource.Token)));
        }

        public override async Task SendAsync(Action<BinaryWriter> write, IPEndPoint to)
        {
            await SendAsync(write, to, CancellationSource.Token);
        }

        private async Task SendAsync(Action<BinaryWriter> write, IPEndPoint to, CancellationToken cancel)
        {
            await SendAsync(write, Clients[to], cancel);
        }

        private async Task SendAsync(Action<BinaryWriter> write, System.Net.Sockets.TcpClient client, CancellationToken cancel)
        {
            try
            {
                SendCompleted.AddCount();
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException("Underlying connection has been closed!", e);
            }

            try
            {
                if (!cancel.IsCancellationRequested)
                {
                    using (MemoryStream mem = new MemoryStream())
                    {
                        BinaryWriter writer = CreateWriter(mem, keepOpen: true);

                        write(writer);
                        writer.Flush();

                        byte[] data = mem.ToArray();

                        Trace.Dump(TraceEventType.Verbose, $"{nameof(TcpProtocol)}.{nameof(SendAsync)}", data);

                        Stream net = GetStream(client);
                        await net.WriteAsync(data, 0, data.Length, cancel);
                    }
                }
            }
            finally
            {
                SendCompleted.Signal();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var c in Clients)
                {
                    c.Value.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        protected virtual Stream GetStream(System.Net.Sockets.TcpClient client)
        {
            return client.GetStream();
        }

        protected async Task ListenClientAsync(System.Net.Sockets.TcpClient client, CancellationToken cancel, TimeSpan cancellationPollDelay)
        {
            IPEndPoint remoteEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;

            Stream net = GetStream(client);

            using (MemoryStream mem = new MemoryStream(client.Available))
            {
                BinaryReader reader = CreateReader(mem, true);
                byte[] buffer = new byte[client.ReceiveBufferSize];

                do
                {
                    int bytes_read = await net.ReadAsync(buffer, 0, buffer.Length, cancel);

                    if (bytes_read > 0)
                    {
                        long position = mem.Position;
                        mem.Seek(0, SeekOrigin.End);
                        mem.Write(buffer, 0, bytes_read);
                        mem.Position = position;

                        OnDataReceived(reader, remoteEndPoint);
                    }
                    else
                    {
                        try
                        {
                            await Task.Delay(cancellationPollDelay, cancel); // This happens to SslStream only. Regular NetworkStream always waits in ReadSync.
                        }
                        catch (TaskCanceledException e)
                        {
                            Debug.WriteLine(e.Message, Trace.Name);
                        }
                    }
                } while (client.Available > 0 || (client.Connected && !cancel.IsCancellationRequested));

                if (mem.Length > 0)
                    OnDataReceived(reader, remoteEndPoint);
            }
        }
    }
}
