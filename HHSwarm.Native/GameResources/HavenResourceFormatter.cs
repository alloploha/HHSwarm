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
        /// Декодирует двоичный файл ресурсов (в виде <paramref name="reader"/>) и передаёт все найденные в нём и декодированные ресурсы в <paramref name="receiver"/>.
        /// Файл ресурсов состоит из слоёв. Каждый слой содержит один ресурс. Тип ресурса в слое задан строкой в заголовке слоя.
        /// </summary>
        /// <param name="reader">Источник двоичных данных в формате файла ресурсов.</param>
        /// <param name="receiver">Объект, методы которого будут вызываться каждый раз когда очередной ресурс успешно декодирован.</param>
        public void Deserialize(BinaryReader reader, IHavenResourceReceiver receiver)
        {
            byte[] signature = reader.ReadBytes(FILE_SIGNATURE.Length);

            // проверка типа фала по последовательности байт в начале
            if (!Enumerable.SequenceEqual(FILE_SIGNATURE, signature))
                throw new ArgumentException();

            receiver.ResourceStreamSignature(signature);

            // версия содержимого файла; используется для обновления ресурсов в кэше клиента
            // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1531
            ushort version = reader.ReadUInt16();

            receiver.ResourceStreamVersion(version);

            do
            {
                // тип ресурса в рассматриваемом слое
                string type = reader.ReadString();

#if DEBUG
                if (receiver is ITraceMessageCanBeChanged)
                {
                    string trace_dump_message = $"Deserialized game resource of type '{type}'";
                    (receiver as ITraceMessageCanBeChanged).Message = trace_dump_message;
                }
#endif

                Deserialize(reader, type, receiver);
            } while (reader.BaseStream.Position < reader.BaseStream.Length);
        }

        protected abstract void Deserialize(BinaryReader reader, string resourceType, IHavenResourceReceiver receiver);

        /// <summary>
        /// Считывает очередной слой из ресурса (через <paramref name="reader"/>) и передаёт его на раскодирование в <paramref name="Extract"/>.
        /// </summary>
        /// <param name="Extract">
        /// 1st argument is stream end position. Do not read outside this boundary!
        /// </param>
        /// <returns>Декодированный ресурс</returns>
        protected TResource ExtractResourceFromLayer<TResource>(BinaryReader reader, Func<long, TResource> Extract) where TResource : ResourceLayer
        {
            // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1539
            int layer_data_length = reader.ReadInt32();

            long position_before_layer = reader.BaseStream.Position;

            try
            {
                TResource resource = Extract(position_before_layer + layer_data_length);

                Debug.Assert(resource != null, "Resource Extract function must return resource object!");

                if (reader.BaseStream.Position != position_before_layer + layer_data_length)
                    throw new Exception($"Not all data has beed read from stream and deserialized for {resource.GetType().Name}! Length of data taken is {reader.BaseStream.Position - position_before_layer} bytes, but message size was {layer_data_length} bytes. Check corresponding {nameof(HavenResourceFormatter.Deserialize)} code.");

                return resource;
            }
            finally
            {
                // вне зависимости от корректности работы с потоком функции Extract, продвигаем указатель ровно на следующий слой
                // это позволяет избежать лавины ошибок из-за избытка или недостатка извлечённых из потока данных
                reader.BaseStream.Position = position_before_layer + layer_data_length;
            }
        }
    }
}
