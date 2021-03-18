using HHSwarm.Native.GameResources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HHSwarm.TestClient
{
    class DumpResourceLayersToFiles : ISourceOfGameResources
    {
        private ISourceOfGameResources Source;
        private string DirectoryPath;

        public DumpResourceLayersToFiles(ISourceOfGameResources source, string directory_path)
        {
            this.Source = source;
            this.DirectoryPath = directory_path;
        }

        public async Task<HavenResource1> GetResourceAsync(string resourceName)
        {
            HavenResource1 result = await Source.GetResourceAsync(resourceName);

            ThreadPool.QueueUserWorkItem((state) => WriteLayersToFiles(result, resourceName));

            return result;
        }

        private static SemaphoreSlim Lock = new SemaphoreSlim(1, 1);

        private void WriteLayersToFiles(HavenResource1 resource, string resourceName)
        {
            string file_name_base = Path.Combine(DirectoryPath, resourceName);

            Lock.Wait();
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(file_name_base));
            }
            finally
            {
                Lock.Release();
            }

            for (int i = 0; i < resource.Images.Count; i++)
            {
                var image = resource.Images[i];
                string file_name = file_name_base + $"-{i}-{image.ID}.jpeg";
                File.WriteAllBytes(file_name, image.Image);
                Console.WriteLine($"Extracted image: {file_name}");
            }

            for (int i = 0; i < resource.JavaClasses.Count; i++)
            {
                var java_class = resource.JavaClasses[i];
                string file_name = file_name_base + $"-{i}-{java_class.Name}.class";
                File.WriteAllBytes(file_name, java_class.Code);
                Console.WriteLine($"Java class: {file_name}");
            }
        }
    }
}
