using Microsoft.VisualStudio.TestTools.UnitTesting;
using HHSwarm.Native.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace HHSwarm.Native.Protocols.Tests
{
    [TestClass()]
    public class TcpServerTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod()]
        public async Task ListenAsyncTest()
        {
            int port = Convert.ToInt32(TestContext.Properties["AuthenticationServerPort"]);
            TcpProtocol thisServer = TcpServer.Create((s, o) => new BinaryReader(s, Encoding.ASCII, o), (s, o) => new BinaryWriter(s, Encoding.ASCII, o), port);

            string receivedString = null;

            thisServer.DataReceived += (reader, remoteEndPoint) =>
            {
                receivedString = reader.ReadString();
            };

            Task listenTask = thisServer.ListenAsync();

            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient(AddressFamily.InterNetwork);
            await client.ConnectAsync(IPAddress.Loopback.MapToIPv4().ToString(), port);

            string sentString = "123456";
            NetworkStream net = client.GetStream();

            using (MemoryStream mem = new MemoryStream())
            {
                BinaryWriter writer = new BinaryWriter(mem, Encoding.ASCII, true);
                writer.Write(sentString);
                writer.Flush();
                byte[] sentData = mem.ToArray();
                net.Write(sentData, 0, sentData.Length);
                net.Flush();
            }

            client.Close();

            Task.WaitAll(thisServer.CloseAsync(), listenTask);

            Assert.AreEqual(sentString, receivedString);
        }
    }
}