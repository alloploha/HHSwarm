using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <summary>
    /// Выводит содержимое ресурса в поток трассировки. Используется для отладки. Источник: "HHSwarm.Resources".
    /// </summary>
    class HavenResourceTraceDump : IHavenResourceReceiver, ITraceMessageCanBeChanged
    {
        private TraceSource Trace = new TraceSource("HHSwarm.Resources");

        private IHavenResourceReceiver Receiver;
        public string Message { get; set; }

        public HavenResourceTraceDump(string message, IHavenResourceReceiver receiver)
        {
            this.Message = message;
            this.Receiver = receiver;
        }

        public void Receive(JavaClassResourceLayer resource)
        {
            Trace.Dump(TraceEventType.Verbose, Message, resource);
            Receiver.Receive(resource);
        }

        public void Receive(JavaClassEntryResourceLayer resource)
        {
            Trace.Dump(TraceEventType.Verbose, Message, resource);
            Receiver.Receive(resource);
        }

        public void Receive(TextureResourceLayer resource)
        {
            Trace.Dump(TraceEventType.Verbose, Message, resource);
            Receiver.Receive(resource);
        }

        public void Receive(MaterialResourceLayer2 resource)
        {
            Trace.Dump(TraceEventType.Verbose, Message, resource);
            Receiver.Receive(resource);
        }

        public void Receive(BoneOffsetResourceLayer resource)
        {
            Trace.Dump(TraceEventType.Verbose, Message, resource);
            Receiver.Receive(resource);
        }

        public void Receive(MeshResourceLayer resource)
        {
            Trace.Dump(TraceEventType.Verbose, Message, resource);
            Receiver.Receive(resource);
        }

        public void Receive(VertexBufferResourceLayer2 resource)
        {
            Trace.Dump(TraceEventType.Verbose, Message, resource);
            Receiver.Receive(resource);
        }

        public void Receive(SkeletonResourceLayer resource)
        {
            Trace.Dump(TraceEventType.Verbose, Message, resource);
            Receiver.Receive(resource);
        }

        public void Receive(NegResourceLayer resource)
        {
            Trace.Dump(TraceEventType.Verbose, Message, resource);
            Receiver.Receive(resource);
        }

        public void Receive(ImageResourceLayer resource)
        {
            Trace.Dump(TraceEventType.Verbose, Message, resource);
            Receiver.Receive(resource);
        }

        public void ResourceStreamSignature(byte[] signature)
        {
            Trace.Dump(TraceEventType.Verbose, Message + "- Signature", signature);
            Receiver.ResourceStreamSignature(signature);
        }

        public void ResourceStreamVersion(ushort version)
        {
            Trace.Dump(TraceEventType.Verbose, Message + "- Version", version);
            Receiver.ResourceStreamVersion(version);
        }

        public void Receive(RenderLinkResourceLayer resource)
        {
            Trace.Dump(TraceEventType.Verbose, Message, resource);
            Receiver.Receive(resource);
        }

        public void Receive(SkeletonAnimationResourceLayer resource)
        {
            Trace.Dump(TraceEventType.Verbose, Message, resource);
            Receiver.Receive(resource);
        }

        public void Receive(MeshAnimationResourceLayer resource)
        {
            Trace.Dump(TraceEventType.Verbose, Message, resource);
            Receiver.Receive(resource);
        }

        public void Receive(VertexBufferResourceLayer1 resource)
        {
            Trace.Dump(TraceEventType.Verbose, Message, resource);
            Receiver.Receive(resource);
        }

        public void Receive(TooltipResourceLayer resource)
        {
            Trace.Dump(TraceEventType.Verbose, Message, resource);
            Receiver.Receive(resource);
        }

        public void Receive(LightResourceLayer resource)
        {
            Trace.Dump(TraceEventType.Verbose, Message, resource);
            Receiver.Receive(resource);
        }

        public void Receive(PaginaResourceLayer resource)
        {
            Trace.Dump(TraceEventType.Verbose, Message, resource);
            Receiver.Receive(resource);
        }
    }
}
