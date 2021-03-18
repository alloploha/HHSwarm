using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols
{
    public class UdpClient : UdpProtocol, IDisposable
    {
        protected UdpClient(CreateReaderDelegate createReader, CreateWriterDelegate createWriter) : base(createReader, createWriter)
        {
        }

        public static UdpClient Create(CreateReaderDelegate createReader, CreateWriterDelegate createWriter, string connectToHostname, int connectToPort, AddressFamily networkLayerProtocol = AddressFamily.InterNetwork)
        {
            UdpClient @this = new UdpClient(createReader, createWriter);
            Initialize(@this, connectToHostname, connectToPort, networkLayerProtocol);
            return @this;
        }

        protected static void Initialize<T>(T @this, string connectToHostname, int connectToPort, AddressFamily networkLayerProtocol) where T : UdpClient
        {
            if (!SupportedNetworkProtocols.Contains(networkLayerProtocol)) throw new ArgumentOutOfRangeException();

            var client = new System.Net.Sockets.UdpClient(networkLayerProtocol);
            client.Connect(connectToHostname, connectToPort);
            @this.Clients.Add((IPEndPoint)client.Client.LocalEndPoint, client);
        }

        private SemaphoreSlim ListenCompleted = new SemaphoreSlim(1, 1); // Use semaphore because ManualEvent does not support await.        

        protected async override Task ListenAsync(CancellationToken cancel, TimeSpan cancellationPollDelay)
        {
            System.Net.Sockets.UdpClient client = Clients.Single().Value;
            IPEndPoint remoteEndpoint = (IPEndPoint)client.Client.RemoteEndPoint;

            await ListenCompleted.WaitAsync(cancel);
            try
            {
                if (!cancel.IsCancellationRequested)
                {
                    await ListenClientAsync(client, cancel, cancellationPollDelay);
                }
            }
            finally
            {
                ListenCompleted.Release();
            }
        }

        public async override Task CloseAsync()
        {
            if (!CancellationSource.IsCancellationRequested)
                CancellationSource.Cancel();

            await ListenCompleted.WaitAsync();

            await base.CloseAsync();
        }
    }
}
