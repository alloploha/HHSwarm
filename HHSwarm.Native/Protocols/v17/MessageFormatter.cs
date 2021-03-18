using System;
using System.IO;

namespace HHSwarm.Native.Protocols.v17
{
    public abstract class MessageFormatter<TContext>
    {
        public TContext MessageContext { get; protected set; }

        public MessageFormatter(TContext messageContext)
        {
            this.MessageContext = messageContext;
        }

        protected abstract void CallSerializerFromMessage(object message, BinaryWriter writer);

        public void Serialize(object graph, BinaryWriter writer)
        {
            CallSerializerFromMessage(graph, writer);
            writer.Flush();
        }

        protected T Deserialize<T>(BinaryReader reader, Func<BinaryReader, T> CallSpecificDeserializer)
        {
            return CallSpecificDeserializer(reader);
        }
    }
}
