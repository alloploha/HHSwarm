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
    public abstract class UdpProtocol : TransportProtocol, IDisposable
    {
        protected Dictionary<IPEndPoint, System.Net.Sockets.UdpClient> Clients = new Dictionary<IPEndPoint, System.Net.Sockets.UdpClient>();

        protected static AddressFamily[] SupportedNetworkProtocols = new AddressFamily[] { AddressFamily.InterNetwork, AddressFamily.InterNetworkV6 };

        public UdpProtocol(CreateReaderDelegate createReader, CreateWriterDelegate createWriter) : base(createReader, createWriter)
        {
#if DEBUG
            DataReceived += UdpProtocol_TraceDataReceived;
#endif
        }

        private void UdpProtocol_TraceDataReceived(BinaryReader reader, IPEndPoint remoteEndPoint)
        {
            MemoryStream mem = new MemoryStream();
            long position = reader.BaseStream.Position;
            reader.BaseStream.CopyTo(mem);
            reader.BaseStream.Position = position;
            Trace.Dump(TraceEventType.Verbose, $"{nameof(UdpProtocol)}.{nameof(DataReceived)}", mem.ToArray());
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
                    if (c.Value.Client != null && c.Value.Client.Connected)
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

        private async Task SendAsync(Action<BinaryWriter> write, System.Net.Sockets.UdpClient client, CancellationToken cancel)
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

                        Trace.Dump(TraceEventType.Verbose, $"{nameof(UdpProtocol)}.{nameof(SendAsync)}", data);

                        await client.SendAsync(data, data.Length);
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

        protected async Task ListenClientAsync(System.Net.Sockets.UdpClient client, CancellationToken cancel, TimeSpan cancellationPollDelay)
        {
            IPEndPoint remoteEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;

            using (MemoryStream mem = new MemoryStream(client.Available))
            {
                BinaryReader reader = CreateReader(mem, true);

                try
                {
                    do
                    {
                        if (client.Available > 0)
                        {
                            UdpReceiveResult udp_result = await client.ReceiveAsync();

                            if (udp_result != null && udp_result.Buffer?.Length > 0)
                            {
                                long position = mem.Position;
                                mem.Seek(0, SeekOrigin.End);
                                mem.Write(udp_result.Buffer, 0, udp_result.Buffer.Length);
                                mem.Position = position;

                                OnDataReceived(reader, remoteEndPoint);
                            }
                        }
                        else
                        {
                            try
                            {
                                await Task.Delay(cancellationPollDelay, cancel);
                            }
                            catch(TaskCanceledException e)
                            {
                                Debug.WriteLine(e.Message, Trace.Name);
                            }
                        }
                    } while (client.Available > 0 || (client.Client.Connected && !cancel.IsCancellationRequested));
                }
                catch (TaskCanceledException e)
                {
                    Trace.TraceInformation($"Stopped listening UDP port because: {e.Message}");
                }
                catch (ObjectDisposedException e)
                {
                    // https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.udpclient.available?view=netframework-4.7.2
                    if (!"System.Net.Sockets.Socket".Equals(e.ObjectName))
                    {
                        Trace.TraceInformation($"Stopped listening UDP port because: The Socket has been closed ({e.Message})");
                        throw;
                    }
                }
                catch (SocketException e)
                {
                    // https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.udpclient.available?view=netframework-4.7.2
                    Trace.TraceInformation($"Stopped listening UDP port because: Remote host shut down or closed connection ({e.Message})");
                }

                if (mem.Length > 0)
                    OnDataReceived(reader, remoteEndPoint);
            }
        }
    }
}
