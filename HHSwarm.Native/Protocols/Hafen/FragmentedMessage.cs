using HHSwarm.Native.Protocols.Hafen.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen
{
    /// <summary>
    /// Умеет собирать/разбирать сообщение <see cref="RMSG"/> из/на фрагменты <see cref="RMSG_FRAGMENT"/>.
    /// </summary>
    class FragmentedMessage
    {
        private List<RMSG_FRAGMENT> Fragments = new List<RMSG_FRAGMENT>();

        /// <summary>
        /// Изначальный тип сообщения.
        /// </summary>
        public RMSG.TYPE? Type => Fragments.Count > 0 ? Fragments[0].MessageType : (RMSG.TYPE?)null;

        /// <summary>
        /// Разбивает большое сообщение на фрагменты для последующей передачи.
        /// </summary>
        /// <typeparam name="T">Тип собщения, определённый в <see cref="RMSG.TYPE"/></typeparam>
        /// <param name="message">Сообщение RMSG_*</param>
        /// <param name="maxFragmentSize">Размер фрагмента</param>
        /// <param name="formatter">Умеет упаковывать сообщения в двоичный формат</param>
        /// <returns></returns>
        public static FragmentedMessage Create<T>(T message, ushort maxFragmentSize, SessionMessageFormatter formatter)
        {
            RMSG.TYPE type = RMSG.TYPES[typeof(T)];
            FragmentedMessage result = new FragmentedMessage();

            using (MemoryStream mem = new MemoryStream())
            {
                MessageBinaryWriter writer = new MessageBinaryWriter(mem, Encoding.UTF8, true);
                formatter.Serialize(message, writer);
                writer.Flush();
                mem.Position = 0;
                byte[] data = new byte[maxFragmentSize];
                int n;
                while ((n = mem.Read(data, 0, maxFragmentSize)) > 0)
                {
                    result.Push(type, data, n);
                }
                result.Seal();
            }

            return result;
        }

        public bool IsSealed => Fragments.TrueForAll(f => !f.IsFinal);
        

        private void Seal()
        {
            if (Fragments.Count < 2) throw new InvalidOperationException();

            Fragments.Last().IsFinal = true;
        }

        private void Push(RMSG.TYPE type, byte[] buffer, int length)
        {
            if (IsSealed) throw new InvalidOperationException();

            RMSG_FRAGMENT fragment = new RMSG_FRAGMENT()
            {
                MessageType = type,
                Data = new byte[length]
            };

            if (Fragments.Count == 0)
                fragment.IsFirst = true;
            else
                fragment.IsNext = true;

            Array.Copy(buffer, fragment.Data, length);

            Fragments.Add(fragment);
        }

        /// <summary>
        /// Запоминает фрагмент сообщения. Этот метод предназначен для приёма сообщений.
        /// </summary>
        /// <param name="fragment"></param>
        public void Push(RMSG_FRAGMENT fragment)
        {
            // Предохранитель на случай если последовательность фрагментов нарушена.
            if (IsSealed) throw new InvalidOperationException();

            int fragments_count = Fragments.Count;

            if(fragments_count == 0)
            {
                // Предохранитель на случай если последовательность фрагментов нарушена.
                if (!fragment.IsFirst) throw new ArgumentOutOfRangeException();
            }
            else if (fragments_count > 0)
            {
                // Предохранитель на случай если в этот накопитель начнут складывать фрагменты не связанные друг с другом.
                if (fragment.MessageType != Type.Value) throw new ArgumentOutOfRangeException();
            }

            Fragments.Add(fragment);
        }

        public byte[] Data
        {
            get
            {
                using (MemoryStream data = new MemoryStream())
                {
                    foreach (RMSG_FRAGMENT fragment in Fragments)
                    {
                        data.Write(fragment.Data, 0, fragment.Data.Length);
                    }

                    return data.ToArray();
                }
            }
        }
    }
}
