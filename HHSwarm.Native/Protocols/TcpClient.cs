using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols
{
    public class TcpClient : TcpProtocol, IDisposable
    {
        protected TcpClient(CreateReaderDelegate createReader, CreateWriterDelegate createWriter) : base(createReader, createWriter)
        {
        }

        public static TcpClient Create(CreateReaderDelegate createReader, CreateWriterDelegate createWriter, string connectToHostname, int connectToPort, AddressFamily networkLayerProtocol = AddressFamily.InterNetwork)
        {
            TcpClient @this = new TcpClient(createReader, createWriter);
            Initialize(@this, connectToHostname, connectToPort, networkLayerProtocol);
            return @this;
        }

        protected static void Initialize<T>(T @this, string connectToHostname, int connectToPort, AddressFamily networkLayerProtocol) where T : TcpClient
        {
            if (!SupportedNetworkProtocols.Contains(networkLayerProtocol)) throw new ArgumentOutOfRangeException();

            var client = new System.Net.Sockets.TcpClient(networkLayerProtocol);
            client.Connect(connectToHostname, connectToPort);
            @this.Clients.Add((IPEndPoint)client.Client.LocalEndPoint, client);
        }

        private SemaphoreSlim ListenCompleted = new SemaphoreSlim(1, 1); // Use semaphore because ManualEvent does not support await.        

        protected async override Task ListenAsync(CancellationToken cancel, TimeSpan cancellationPollDelay)
        {
            System.Net.Sockets.TcpClient client = Clients.Single().Value;
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
