using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HHSwarm.Native.Common
{
    class AwaitableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> m_Dictionary = new Dictionary<TKey, TValue>();
        private ReaderWriterLockSlim DictionalyLock = new ReaderWriterLockSlim();

        private List<Tuple<TKey, TaskCompletionSource<TValue>>> Tasks = new List<Tuple<TKey, TaskCompletionSource<TValue>>>();
        private object TasksLock = new object();

        public TValue this[TKey key]
        {
            get
            {
                return GetItemAsync(key).Result;
            }
            set
            {
                this.Add(key, value);
            }
        }

        public async Task<TValue> GetItemAsync(TKey key)
        {
            if (ContainsKey(key))
            {
                return await Task.FromResult(GetValue(key));
            }
            else
            {
                TaskCompletionSource<TValue> task_source = new TaskCompletionSource<TValue>();

                lock (TasksLock)
                {
                    Tasks.Add(new Tuple<TKey, TaskCompletionSource<TValue>>(key, task_source));
                }

                return await task_source.Task;
            }
        }

        private void ReleaseTasks(TKey key)
        {
            lock (TasksLock)
            {
                Tasks.ForEach(tt =>
                {
                    if (object.Equals(tt.Item1, key)) tt.Item2.SetResult(GetValue(key));
                });

                Tasks.RemoveAll(tt => object.Equals(tt.Item1, key));
            }
        }

        public void Add(TKey key, TValue value)
        {
            try
            {
                DictionalyLock.EnterWriteLock();
                m_Dictionary.Add(key, value);
            }
            finally
            {
                DictionalyLock.ExitWriteLock();
            }

            ReleaseTasks(key);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            try
            {
                DictionalyLock.EnterWriteLock();
                ((IDictionary<TKey, TValue>)m_Dictionary).Add(item);
            }
            finally
            {
                DictionalyLock.ExitWriteLock();
            }

            ReleaseTasks(item.Key);
        }

        public ICollection<TKey> Keys
        {
            get
            {
                try
                {
                    DictionalyLock.EnterReadLock();
                    return m_Dictionary.Keys;
                }
                finally
                {
                    DictionalyLock.ExitReadLock();
                }
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                try
                {
                    DictionalyLock.EnterReadLock();
                    return m_Dictionary.Values;
                }
                finally
                {
                    DictionalyLock.ExitReadLock();
                }
            }
        }

        public int Count
        {
            get
            {
                try
                {
                    DictionalyLock.EnterReadLock();
                    return m_Dictionary.Count;
                }
                finally
                {
                    DictionalyLock.ExitReadLock();
                }
            }
        }

        public bool IsReadOnly => false;

        public void Clear()
        {
            try
            {
                DictionalyLock.EnterWriteLock();
                m_Dictionary.Clear();
            }
            finally
            {
                DictionalyLock.ExitWriteLock();
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            try
            {
                DictionalyLock.EnterReadLock();
                return m_Dictionary.Contains(item);
            }
            finally
            {
                DictionalyLock.ExitReadLock();
            }
        }

        public bool ContainsKey(TKey key)
        {
            try
            {
                DictionalyLock.EnterReadLock();
                return m_Dictionary.ContainsKey(key);
            }
            finally
            {
                DictionalyLock.ExitReadLock();
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            try
            {
                DictionalyLock.EnterReadLock();
                ((IDictionary<TKey, TValue>)m_Dictionary).CopyTo(array, arrayIndex);
            }
            finally
            {
                DictionalyLock.ExitReadLock();
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            try
            {
                DictionalyLock.EnterReadLock();
                return m_Dictionary.GetEnumerator();
            }
            finally
            {
                DictionalyLock.ExitReadLock();
            }
        }

        public bool Remove(TKey key)
        {
            try
            {
                DictionalyLock.EnterWriteLock();
                return m_Dictionary.Remove(key);
            }
            finally
            {
                DictionalyLock.ExitWriteLock();
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            try
            {
                DictionalyLock.EnterWriteLock();
                return ((IDictionary<TKey, TValue>)m_Dictionary).Remove(item);
            }
            finally
            {
                DictionalyLock.ExitWriteLock();
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            try
            {
                DictionalyLock.EnterReadLock();
                return m_Dictionary.TryGetValue(key, out value);
            }
            finally
            {
                DictionalyLock.ExitReadLock();
            }
        }

        public TValue GetValue(TKey key)
        {
            try
            {
                DictionalyLock.EnterReadLock();
                return m_Dictionary[key];
            }
            finally
            {
                DictionalyLock.ExitReadLock();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
