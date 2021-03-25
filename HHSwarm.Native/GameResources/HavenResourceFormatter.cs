using HHSwarm.Native.Common;
using HHSwarm.Native.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace HHSwarm.Native.GameResources
{
    public abstract class HavenResourceFormatter
    {
        protected TraceSource Trace = new TraceSource("HHSwarm.Resources");

        private readonly byte[] FILE_SIGNATURE;

        public HavenResourceFormatter(byte[] fileSignature)
        {
            this.FILE_SIGNATURE = fileSignature;
        }

        /// <summary>
        /// Разбирает двоичный файл ресурсов, в объекты классов.
        /// </summary>
        /// <param name="reader">Источник двоичных данных в формате файла ресурсов.</param>
        /// <param name="receiver">Объект, методы которого будут вызываться каждый раз когда очередной ресурс преобразован в объект класса.</param>
        public void Deserialize(BinaryReader reader, IHavenResourceReceiver receiver)
        {
            byte[] signature = reader.ReadBytes(FILE_SIGNATURE.Length);

            if (!Enumerable.SequenceEqual(FILE_SIGNATURE, signature))
                throw new ArgumentException();

            receiver.ResourceStreamSignature(signature);

            ushort version = reader.ReadUInt16();

            receiver.ResourceStreamVersion(version);

            do
            {
                string type = reader.ReadString();

#if DEBUG
                if (receiver is ITraceMessageCanBeChanged)
                {
                    string trace_dump_message = $"Deserialized game resource of type {type}";
                    (receiver as ITraceMessageCanBeChanged).Message = trace_dump_message;
                }
#endif

                Deserialize(reader, type, receiver);
            } while (reader.BaseStream.Position < reader.BaseStream.Length);
        }

        protected abstract void Deserialize(BinaryReader reader, string resourceType, IHavenResourceReceiver receiver);

        /// <param name="Extract">
        /// 1st argument is stream end position. Do not read outside this boundary.
        /// </param>
        protected TResource ExtractResourceFromLayer<TResource>(BinaryReader reader, Func<long, TResource> Extract) where TResource : ResourceLayer
        {
            int resource_data_length = reader.ReadInt32();
            long position_before_layer = reader.BaseStream.Position;

            try
            {
                TResource resource = Extract(position_before_layer + resource_data_length);

                Debug.Assert(resource != null, "Resource Extract function must always return resource object!");

                if (reader.BaseStream.Position != position_before_layer + resource_data_length)
                    throw new Exception($"Not all data has beed read from stream and deserialized for {resource.GetType().Name}! Length of data taken is {reader.BaseStream.Position - position_before_layer} bytes, but message size was {resource_data_length} bytes. Check corresponding {nameof(HavenResourceFormatter.Deserialize)} code.");

                return resource;
            }
            finally
            {
                reader.BaseStream.Position = position_before_layer + resource_data_length;
            }
        }
    }
}
