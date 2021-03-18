using HHSwarm.Native.Protocols.v17;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    public class CachedGameResources : IGameResources
    {
        protected TraceSource Trace = new TraceSource("HHSwarm.Resources");

        private static MemoryCache Cache = new MemoryCache(nameof(CachedGameResources));

        private IsolatedStorageFile Store = IsolatedStorageFile.GetUserStoreForAssembly();
        private static SemaphoreSlim StoreLock = new SemaphoreSlim(1, 1); // IsolatedStorage is not thread-safe. Deadlocks are very possible.

        private static Regex InvalidFilePathCharsRegex;

        static CachedGameResources()
        {
            List<char> chars = new List<char>();
            chars.Add(Path.DirectorySeparatorChar);
            chars.Add(Path.AltDirectorySeparatorChar);
            chars.AddRange(Path.GetInvalidFileNameChars());
            chars.AddRange(Path.GetInvalidPathChars());
            chars.Add('.');

            InvalidFilePathCharsRegex = new Regex("[" + chars.Distinct().ToArray().Select(c => Regex.Escape(c.ToString())) + "]", RegexOptions.Compiled | RegexOptions.Singleline);
        }

        private string Key(string resourceName)
        {
            return String.Format("{0}", resourceName.Trim());
        }

        private string FileNameFromResourceName(string resourceName)
        {
            return InvalidFilePathCharsRegex.Replace(resourceName, "!").Trim();
        }

        private string FilePath(string resourceName)
        {
            Type type = typeof(CachedGameResources);
            Version version = type.Assembly.GetName().Version;
            return Path.Combine($"{nameof(CachedGameResources)}-{version.Major}.{version.Minor}.{version.Build}.{version.Revision}", String.Format("{0}.bin", FileNameFromResourceName(resourceName)));
        }

        public bool Contains(string resourceName)
        {
            HavenResource1 resource = (HavenResource1)Cache[Key(resourceName)];

            bool is_in_memory = resource != null;

            bool is_in_file = false;
            long file_size = 0;

            if (!is_in_memory)
            {
                string file_path = FilePath(resourceName);

                is_in_file = Store.FileExists(file_path);

                if (is_in_file)
                {
                    using (var file = Store.OpenFile(file_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        file_size = file.Length;
                        file.Close();
                    }
                }
            }

            return is_in_memory || (is_in_file && file_size > 0);
        }

        public async Task AddAsync(string resourceName, HavenResource1 resource)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(mem, resource);

                string file_path = FilePath(resourceName);

                try
                {
                    await StoreLock.WaitAsync();

                    EnsureDirectoryExists(file_path);

                    using (var file = Store.CreateFile(file_path))
                    {
                        await file.WriteAsync(mem.ToArray(), 0, (int)mem.Length);
                        file.Close();
                    }
                }
                finally
                {
                    StoreLock.Release();
                }
            }

            CacheItemPolicy cache_policy = new CacheItemPolicy();
            Cache.Set(Key(resourceName), resource, cache_policy);
        }

        public async Task<HavenResource1> GetAsync(string resourceName, ushort? resourceVersion = null)
        {
            string cache_key = Key(resourceName);

            bool is_in_memory = Cache.Contains(cache_key);

            if (!is_in_memory)
            {
                string file_path = FilePath(resourceName);

                bool is_in_file_possibly = Store.FileExists(FilePath(resourceName));

                if (is_in_file_possibly)
                {
                    using (var file = Store.OpenFile(file_path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        byte[] data = new byte[file.Length];
                        await file.ReadAsync(data, 0, data.Length);

                        file.Close();

                        BinaryFormatter serializer = new BinaryFormatter();

                        using (MemoryStream mem = new MemoryStream(data))
                        {
                            HavenResource1 resource_from_file = (HavenResource1)serializer.Deserialize(mem);
                            bool resource_found = true;

                            if (resourceVersion.HasValue)
                            {
                                resource_found = resource_from_file.Version == resourceVersion.Value;
                            }

                            if (resource_found)
                            {
                                CacheItemPolicy cache_policy = new CacheItemPolicy();
                                Cache.Set(cache_key, resource_from_file, cache_policy);
                            }
                        }
                    }
                }
            }

            return (HavenResource1)Cache[cache_key];
        }

        private void EnsureDirectoryExists(string filePath)
        {
            string directory_path = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory_path))
            {
                Store.CreateDirectory(directory_path);
            }
        }
    }
}
