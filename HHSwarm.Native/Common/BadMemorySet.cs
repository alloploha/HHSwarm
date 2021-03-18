using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Common
{
    class BadMemorySet<T>
    {
        private int KeepAtLeast;
        private int TrimIfMoreThan;

        private HashSet<T> Items = new HashSet<T>();
        private Queue<T> Memory = new Queue<T>();

        public BadMemorySet(int keepNoLessThan, int trimIfMoreThan)
        {
            if (keepNoLessThan < 0) throw new ArgumentOutOfRangeException();
            if (trimIfMoreThan < 0) throw new ArgumentOutOfRangeException();
            if (keepNoLessThan > trimIfMoreThan) throw new ArgumentOutOfRangeException();

            this.KeepAtLeast = keepNoLessThan;
            this.TrimIfMoreThan = trimIfMoreThan;
        }

        public bool Contains(T item)
        {
            return Items.Contains(item);
        }

        private static object Lock = new object();

        public bool Add(T item)
        {
            bool result = false;

            lock (Lock)
            {
                result = Items.Add(item);
                Memory.Enqueue(item);
                TrimExcess();
            }

            return result;
        }

        public int Count => Items.Count;

        private void TrimExcess()
        {
            if(Items.Count > TrimIfMoreThan || (Items.Count > KeepAtLeast && Memory.Count > TrimIfMoreThan) || Memory.Count > TrimIfMoreThan * 2)
            {
                while(Items.Count > KeepAtLeast)
                {
                    Items.Remove(Memory.Dequeue());
                }

                Items.TrimExcess();
                Memory.TrimExcess();
            }

            Debug.Assert(Items.Count <= Memory.Count);
        }
    }
}
