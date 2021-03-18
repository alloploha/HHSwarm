using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols
{
    public abstract class TransportProtocol : IDisposable
    {
        protected TraceSource Trace = new TraceSource("HHSwarm.Transport");

        public delegate void DataReceivedDelegate(BinaryReader reader, IPEndPoint remoteEndPoint);

        public event DataReceivedDelegate DataReceived;

        protected void OnDataReceived(BinaryReader reader, IPEndPoint remoteEndPoint)
        {
            // Invoke event handlers while any of them reads at least 1 byte, or end of data is reached.
            for
            (
                long prev_pos = -1, curr_pos = reader.BaseStream.Position;
                prev_pos < curr_pos && curr_pos < reader.BaseStream.Length;
                prev_pos = curr_pos, curr_pos = reader.BaseStream.Position
            )
                DataReceived?.Invoke(reader, remoteEndPoint);
        }

        internal readonly CreateReaderDelegate CreateReader;
        internal readonly CreateWriterDelegate CreateWriter;

        public TransportProtocol(CreateReaderDelegate createReader, CreateWriterDelegate createWriter)
        {
            this.CreateReader = createReader;
            this.CreateWriter = createWriter;
        }

        protected CancellationTokenSource CancellationSource = new CancellationTokenSource();

        public const int CANCELLATION_POLL_MILLISECONDS_DELAY_DEAFULT = 1000;

        public async Task ListenAsync(int cancellationPollMillisecondsDelay = CANCELLATION_POLL_MILLISECONDS_DELAY_DEAFULT)
        {
            await ListenAsync(CancellationSource.Token, cancellationPollMillisecondsDelay);
        }

        public async Task ListenAsync(CancellationToken cancel, int cancellationPollMillisecondsDelay = CANCELLATION_POLL_MILLISECONDS_DELAY_DEAFULT)
        {
            if (cancellationPollMillisecondsDelay <= 0) cancellationPollMillisecondsDelay = 1;
            await ListenAsync(cancel, TimeSpan.FromMilliseconds(cancellationPollMillisecondsDelay));
        }

        protected abstract Task ListenAsync(CancellationToken cancel, TimeSpan cancellationPollDelay);

        public abstract Task SendAsync(Action<BinaryWriter> write);

        public abstract Task SendAsync(Action<BinaryWriter> write, IPEndPoint to);

        public abstract Task CloseAsync();

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
