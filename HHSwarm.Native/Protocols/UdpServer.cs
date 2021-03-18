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
    public class UdpServer : UdpClient, IDisposable
    {
        private System.Net.Sockets.UdpClient Server;

        protected UdpServer(CreateReaderDelegate createReader, CreateWriterDelegate createWriter) :
            base(createReader, createWriter)
        {
        }

        public static UdpServer Create(CreateReaderDelegate createReader, CreateWriterDelegate createWriter, int listenAtPort, AddressFamily networkLayerProtocol = AddressFamily.InterNetwork)
        {
            UdpServer @this = new UdpServer(createReader, createWriter);
            Initialize(@this, listenAtPort, networkLayerProtocol);
            return @this;
        }

        protected static void Initialize<T>(T @this, int listenAtPort, AddressFamily networkLayerProtocol) where T : UdpServer
        {
            if (!SupportedNetworkProtocols.Contains(networkLayerProtocol)) throw new ArgumentOutOfRangeException();

            @this.Server = new System.Net.Sockets.UdpClient(listenAtPort, networkLayerProtocol);
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
                    while (!cancel.IsCancellationRequested)
                    {
                        var accept_client_task = Server.ReceiveAsync()
                            .ContinueWith(async (Task<UdpReceiveResult> t) =>
                            {
                                System.Net.Sockets.UdpClient client = Server;
                                IPEndPoint remoteEndPoint = t.Result.RemoteEndPoint;
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
