using Microsoft.VisualStudio.TestTools.UnitTesting;
using HHSwarm.Native.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HHSwarm.Native.Protocols.Tests
{
    [TestClass()]
    public class SecureTcpClientTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod()]
        public async Task LiveServerConnectionTest()
        {
            string address = (string)TestContext.Properties["LiveSSLTestHostAddress"];
            int port = Convert.ToInt32(TestContext.Properties["LiveSSLTestHostPort"]);

            SecureTcpClient client = SecureTcpClient.Create
            (
                (s, o) => new BinaryReader(s, Encoding.ASCII, o), 
                (s, o) => new BinaryWriter(s, Encoding.ASCII, o), 
                address, 
                port
            );

            byte[] responseBytes = null;

            client.DataReceived += (reader, remoteEndPoint) =>
            {
                responseBytes = reader.ReadBytes(ushort.MaxValue);
            };

            await client.SendAsync(writer => writer.Write("This is a !@#$%"));
            Task listenTask =  client.ListenAsync(200);
            await client.CloseAsync();
            await listenTask;

            string resposeText = Encoding.ASCII.GetString(responseBytes);
            StringAssert.Contains(resposeText, "400 Bad Request");
        }
    }
}