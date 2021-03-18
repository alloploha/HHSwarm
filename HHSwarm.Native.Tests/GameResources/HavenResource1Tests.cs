using Microsoft.VisualStudio.TestTools.UnitTesting;
using HHSwarm.Native.GameResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HHSwarm.Native.Protocols.v17;
using System.Runtime.Serialization.Formatters.Binary;

namespace HHSwarm.Native.GameResources.Tests
{
    [TestClass()]
    public class HavenResource1Tests
    {
        public TestContext TestContext { get; set; }


        private List<Tuple<string, byte[]>> Samples = new List<Tuple<string, byte[]>>();

        [TestInitialize]
        public void PreLoadResources()
        {
            Samples.Clear();

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"GameResources\Samples");
            foreach(string file_name in  Directory.EnumerateFiles(path, "*.bin", SearchOption.TopDirectoryOnly))
            {
                Samples.Add(new Tuple<string, byte[]>(Path.GetFileNameWithoutExtension(file_name), File.ReadAllBytes(file_name)));
            }
        }

        [TestMethod()]
        public void HavenResource1Test()
        {
            foreach (var sample in Samples)
            {
                string name = sample.Item1;
                byte[] data = sample.Item2;

                HavenResourceFormatter serializer = new HavenResource1Formatter();
                HavenResource1 res = new HavenResource1();

                using (MemoryStream mem = new MemoryStream(data))
                {
                    serializer.Deserialize(new MessageBinaryReader(mem, Encoding.UTF8), res);
                }

                Assert.IsTrue(res.Version > 0, "No version found!");

                // To check .NET binary serialization and compare sizes.
                using (MemoryStream ssx = new MemoryStream())
                {
                    new BinaryFormatter().Serialize(ssx, res);
                    TestContext.WriteLine("Resource '{0}' original size = {1}, .net bin size = {2}", name, data.Length, ssx.Length);
                }

                //foreach (var r in res.JavaClasses)
                //{
                //    int i = res.JavaClasses.IndexOf(r);
                //    File.WriteAllBytes($@"c:\temp\{name}-{i}-{r.Name}.class", r.Code);
                //}

                //foreach (var r in res.Images)
                //{
                //    int i = res.JavaClasses.IndexOf(r);
                //    File.WriteAllBytes($@"c:\temp\{name}-{i}-{layer.Type}.{r.Name}.image", r.Image);
                //}
            }
        }
    }
}