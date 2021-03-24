using HHSwarm.Native.Protocols.v17;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Tests
{
    [TestClass()]
    public class TcpClientTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod()]
        public async Task SendAsyncTest()
        {
            int port = Convert.ToInt32(TestContext.Properties["LocalListenerPort"]);
            TcpListener server = new TcpListener(IPAddress.Loopback, port);
            server.Start();
            TcpClient thisClient = TcpClient.Create((s, o) => new BinaryReader(s, Encoding.ASCII, o), (s, o) => new BinaryWriter(s, Encoding.ASCII, o), IPAddress.Loopback.MapToIPv4().ToString(), port);
            string sentString = "ABCDEF";

            var remoteClient = server.AcceptTcpClient();

            await thisClient.SendAsync(w => w.Write(sentString));

            NetworkStream net = remoteClient.GetStream();
            Assert.IsTrue(remoteClient.Available > 0, "No data received on server side!");
            byte[] receivedData = new byte[remoteClient.Available];

            await thisClient.CloseAsync(); // the following Read operation may stall if 0 bytes transmitted. Thus need to close connection on client side to prevent deadlock.

            net.Read(receivedData, 0, receivedData.Length);

            remoteClient.Close();
            server.Stop();

            BinaryReader reader = new BinaryReader(new MemoryStream(receivedData), Encoding.ASCII, false);
            string receivedString = reader.ReadString();

            Assert.AreEqual(sentString, receivedString);
        }

        [TestMethod]
        public async Task ListenAsyncTest()
        {
            int port = Convert.ToInt32(TestContext.Properties["LocalListenerPort"]);
            TcpListener server = new TcpListener(IPAddress.Loopback, port);
            server.Start();
            TcpClient thisClient = TcpClient.Create((s, o) => new BinaryReader(s, Encoding.ASCII, o), (s, o) => new BinaryWriter(s, Encoding.ASCII, o), IPAddress.Loopback.MapToIPv4().ToString(), port);
            string sentString = "GHIJK";

            var remoteClient = server.AcceptTcpClient();
            NetworkStream net = remoteClient.GetStream();

            using (MemoryStream mem = new MemoryStream())
            {
                BinaryWriter writer = new BinaryWriter(mem, Encoding.ASCII, true);
                writer.Write(sentString);
                writer.Flush();
                byte[] sentData = mem.ToArray();
                net.Write(sentData, 0, sentData.Length);
                net.Flush();
            }

            string receivedString = null;

            thisClient.DataReceived += (reader, remoteEndPoint) =>
            {
                receivedString = reader.ReadString();
            };

            Task listenTask = thisClient.ListenAsync();

            remoteClient.Close();
            await thisClient.CloseAsync();
            server.Stop();

            if (!listenTask.IsCanceled)
                await listenTask;


            Assert.IsNotNull(receivedString, "Client hasn't received data!");
            Assert.AreEqual(sentString, receivedString);
        }

        [TestMethod]
        public async Task ListenAsyncManyTest()
        {
            int port = Convert.ToInt32(TestContext.Properties["LocalListenerPort"]);
            TcpListener server = new TcpListener(IPAddress.Loopback, port);
            server.Start();
            TcpClient thisClient = TcpClient.Create((s, o) => new BinaryReader(s, Encoding.ASCII, o), (s, o) => new BinaryWriter(s, Encoding.ASCII, o), IPAddress.Loopback.MapToIPv4().ToString(), port);
            string[] sentStrings = { "ABC", "-", "123" };

            var remoteClient = server.AcceptTcpClient();
            NetworkStream net = remoteClient.GetStream();

            using (MemoryStream mem = new MemoryStream())
            {
                BinaryWriter writer = new BinaryWriter(mem, Encoding.ASCII, true);

                for (int i = 0; i < sentStrings.Length; i++)
                {
                    mem.SetLength(0);
                    writer.Write(sentStrings[i]);
                    writer.Flush();
                    byte[] sentData = mem.ToArray();
                    net.Write(sentData, 0, sentData.Length);
                    net.Flush();
                }
            }

            List<string> receivedStrings = new List<string>();

            thisClient.DataReceived += (reader, remoteEndPoint) =>
            {
                receivedStrings.Add(reader.ReadString());
            };

            Task listenTask = thisClient.ListenAsync();

            remoteClient.Close();
            await thisClient.CloseAsync();
            server.Stop();

            if (!listenTask.IsCanceled)
                await listenTask;

            Assert.IsTrue(receivedStrings.Count > 0, "Client hasn't received all data!");
            Assert.AreEqual(sentStrings.Length, receivedStrings.Count);

            for (int i = 0; i < receivedStrings.Count; i++)
            {
                Assert.AreEqual(sentStrings[i], receivedStrings[i]);
            }
        }
    }
}