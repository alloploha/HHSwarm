using HHSwarm.Native.Protocols.v17;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    public class WebSourceOfGameResources : ISourceOfGameResources
    {
        private TraceSource Trace = new TraceSource("HHSwarm.Resources");

        private string UserAgent;
        private string ResourcesBaseUri;

        public WebSourceOfGameResources(string resourceBaseUri, string userAgent = @"Haven/1.0")
        {
            this.ResourcesBaseUri = resourceBaseUri;
            if (ResourcesBaseUri.Last() != '/') ResourcesBaseUri += '/';
            this.UserAgent = userAgent;
        }

        public async Task<HavenResource1> GetResourceAsync(string resourceName)
        {
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.UserAgent, UserAgent);

            Trace.TraceInformation($"Downloading resource '{resourceName}' from network...");

            byte[] data = await client.DownloadDataTaskAsync(ResourcesBaseUri + $"{resourceName}.res");

            Trace.TraceInformation($"Downloaded resource '{resourceName}', data length {data.Length} bytes.");


            HavenResourceFormatter serializer = new HavenResource1Formatter();
            HavenResource1 result = new HavenResource1();
            IHavenResourceReceiver receiver = result;

#if DEBUG
            string trace_dump_message = $"Deserialized game resource layer from '{resourceName}'";
            receiver = new HavenResourceTraceDump(trace_dump_message, result);
#endif

            using (MemoryStream mem = new MemoryStream(data))
            {
                serializer.Deserialize(new MessageBinaryReader(mem, Encoding.UTF8), receiver);
            }

            return result;
        }
    }
}
