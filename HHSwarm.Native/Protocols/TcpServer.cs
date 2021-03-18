using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols
{
    public class TcpServer : TcpProtocol, IDisposable
    {
        private TcpListener Server;

        protected TcpServer(CreateReaderDelegate createReader, CreateWriterDelegate createWriter) :
            base(createReader, createWriter)
        {
        }

        public static TcpServer Create(CreateReaderDelegate createReader, CreateWriterDelegate createWriter, int listenAtPort, AddressFamily networkLayerProtocol = AddressFamily.InterNetwork)
        {
            TcpServer @this = new TcpServer(createReader, createWriter);
            Initialize(@this, listenAtPort, networkLayerProtocol);
            return @this;
        }

        protected static void Initialize<T>(T @this, int listenAtPort, AddressFamily networkLayerProtocol) where T : TcpServer
        {
            if (!SupportedNetworkProtocols.Contains(networkLayerProtocol)) throw new ArgumentOutOfRangeException();

            switch (networkLayerProtocol)
            {
                case AddressFamily.InterNetwork:
                    @this.Server = new TcpListener(IPAddress.Any, listenAtPort);
                    break;
                case AddressFamily.InterNetworkV6:
                    @this.Server = new TcpListener(IPAddress.IPv6Any, listenAtPort);
                    break;
                default:
                    throw new NotImplementedException(networkLayerProtocol.ToString());
            }
        }

        private SemaphoreSlim ListenCompleted = new SemaphoreSlim(1, 1); // Use semaphore because ManualEvent does not support await.        

        protected async override Task ListenAsync(CancellationToken cancel, TimeSpan cancellationPollDelay)
        {
            if (cancellationPollDelay.TotalMilliseconds <= 0) cancellationPollDelay = TimeSpan.FromMilliseconds(1);

            await ListenCompleted.WaitAsync(cancel);
            try
            {
                if (!cancel.IsCancellationRequested)
                {
                    Server.Start();

                    while (!cancel.IsCancellationRequested)
                    {
                        var accept_client_task = Server.AcceptTcpClientAsync()
                            .ContinueWith(async (Task<System.Net.Sockets.TcpClient> t) =>
                            {
                                System.Net.Sockets.TcpClient client = t.Result;
                                IPEndPoint remoteEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
                                Clients.Add(remoteEndPoint, client);

                                await ListenClientAsync(client, cancel, cancellationPollDelay);

                                Clients.Remove(remoteEndPoint);
                            }
                            , TaskContinuationOptions.OnlyOnRanToCompletion);

                        Task await_accept_client_task;
                        do
                        {
                            await_accept_client_task = await Task.WhenAny(Task.Delay(cancellationPollDelay), accept_client_task);
                        } while (await_accept_client_task.Id != accept_client_task.Id && !cancel.IsCancellationRequested);
                    }

                    Server.Stop();
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
