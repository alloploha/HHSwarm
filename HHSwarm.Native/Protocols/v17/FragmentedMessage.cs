using HHSwarm.Native.Protocols.v17.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.v17
{
    class FragmentedMessage
    {
        private List<RMSG_FRAGMENT> Fragments = new List<RMSG_FRAGMENT>();

        public RMSG.TYPE? Type => Fragments.Count > 0 ? Fragments[0].MessageType : (RMSG.TYPE?)null;

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

        public void Push(RMSG_FRAGMENT fragment)
        {
            if (IsSealed) throw new InvalidOperationException();
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
