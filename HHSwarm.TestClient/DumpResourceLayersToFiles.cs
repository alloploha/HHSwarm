using HHSwarm.Native.GameResources;
using System;
using System.IO;
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
            string resource_file_name = resourceName.Replace(Path.DirectorySeparatorChar, '!').Replace(Path.AltDirectorySeparatorChar, '!');

            // images
            {
                string directory_name = Path.Combine(DirectoryPath, "images");

                Lock.Wait();
                try
                {
                    Directory.CreateDirectory(directory_name);
                }
                finally
                {
                    Lock.Release();
                }

                for (int i = 0; i < resource.Images.Count; i++)
                {
                    var item = resource.Images[i];
                    string file_name = $"{resource_file_name}-[{i + 1}]({resource.Images.Count})#{item.ID}.png";
                    File.WriteAllBytes(Path.Combine(directory_name, file_name), item.Image);
                    Console.WriteLine($"Extracted image: {file_name}");
                }
            }

            // Java classes
            {
                string directory_name = Path.Combine(DirectoryPath, "classes");

                Lock.Wait();
                try
                {
                    Directory.CreateDirectory(directory_name);
                }
                finally
                {
                    Lock.Release();
                }
                for (int i = 0; i < resource.JavaClasses.Count; i++)
                {
                    var item = resource.JavaClasses[i];
                    string file_name = $"{resource_file_name}-[{i + 1}]({resource.JavaClasses.Count})-{item.Name}.class";
                    File.WriteAllBytes(Path.Combine(directory_name, file_name), item.Code);
                    Console.WriteLine($"Extracted Java class: {file_name}");
                }
            }

            // Java source code
            {
                string directory_name = Path.Combine(DirectoryPath, "java");

                Lock.Wait();
                try
                {
                    Directory.CreateDirectory(directory_name);
                }
                finally
                {
                    Lock.Release();
                }
                for (int i = 0; i < resource.JavaSourceCode.Count; i++)
                {
                    var item = resource.JavaSourceCode[i];
                    string file_name = $"{resource_file_name}-[{i + 1}]({resource.JavaSourceCode.Count}){item.FileName}";
                    File.WriteAllBytes(Path.Combine(directory_name, file_name), item.Text);
                    Console.WriteLine($"Extracted Java source code: {file_name}");
                }
            }
        }
    }
}
