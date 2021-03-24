using Microsoft.VisualStudio.TestTools.UnitTesting;
using HHSwarm.Native.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;

namespace HHSwarm.Native.Protocols.Tests
{
    [TestClass]
    public class UdpClientTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public async Task ListenAsync_CancelSlowPoll_Test()
        {
            int port = Convert.ToInt32(TestContext.Properties["LocalListenerPort"]);

            UdpClient p = UdpClient.Create
            (
                (s, o) => new BinaryReader(s, Encoding.ASCII, o),
                (s, o) => new BinaryWriter(s, Encoding.ASCII, o),
                IPAddress.Loopback.MapToIPv4().ToString(),
                port
            );

            Task t = p.ListenAsync(30000);
            Task c = p.CloseAsync();
            await Task.WhenAll(t, c);

            p.Dispose();

            Assert.IsFalse(t.IsFaulted, "ListenAsync not IsFaulted");
            Assert.IsFalse(c.IsFaulted, "CloseAsync not IsFaulted");
        }

        [TestMethod]
        public async Task ListenAsync_CancelFastPoll_Test()
        {
            int port = Convert.ToInt32(TestContext.Properties["LocalListenerPort"]);

            UdpClient p = UdpClient.Create
            (
                (s, o) => new BinaryReader(s, Encoding.ASCII, o),
                (s, o) => new BinaryWriter(s, Encoding.ASCII, o),
                IPAddress.Loopback.MapToIPv4().ToString(),
                port
            );

            Task t = p.ListenAsync(0);

            Task d = Task.Delay(TimeSpan.FromSeconds(20));
            await d;

            Task c = p.CloseAsync();
            await Task.WhenAll(t, c);

            p.Dispose();

            Assert.IsFalse(t.IsFaulted, "ListenAsync not IsFaulted");
            Assert.IsFalse(c.IsFaulted, "CloseAsync not IsFaulted");
        }

        [TestMethod]
        public async Task SendAsync_Byte_Test()
        {
            int port = Convert.ToInt32(TestContext.Properties["LocalListenerPort"]);

            System.Net.Sockets.UdpClient server = new System.Net.Sockets.UdpClient(port, System.Net.Sockets.AddressFamily.InterNetwork);

            UdpClient client = UdpClient.Create
            (
                (s, o) => new BinaryReader(s, Encoding.ASCII, o),
                (s, o) => new BinaryWriter(s, Encoding.ASCII, o),
                IPAddress.Loopback.MapToIPv4().ToString(),
                port,
                System.Net.Sockets.AddressFamily.InterNetwork
            );

            byte[] server_received_bytes = null;
            ManualResetEvent server_bytes_arrived = new ManualResetEvent(false);

            Task receive = server.ReceiveAsync().ContinueWith(t =>
            {
                server_received_bytes = t.Result.Buffer.ToArray();
                server_bytes_arrived.Set();
            }, TaskContinuationOptions.NotOnFaulted);

            byte[] client_sent_bytes = new byte[] { (byte)'*' };

            await client.SendAsync(writer => writer.Write(client_sent_bytes));

            bool server_bytes_arrived_signalled = server_bytes_arrived.WaitOne(10000);

            Assert.IsTrue(server_bytes_arrived_signalled, "Server timedout");

            await client.CloseAsync();
            server.Close();

            CollectionAssert.AreEqual(client_sent_bytes, server_received_bytes, "Recevied bytes are not the same as sent ones!");

            client.Dispose();
            server.Dispose();
        }

        [TestMethod]
        public async Task SendAsync_MaxSize_Test()
        {
            int port = Convert.ToInt32(TestContext.Properties["LocalListenerPort"]);

            System.Net.Sockets.UdpClient server = new System.Net.Sockets.UdpClient(port, System.Net.Sockets.AddressFamily.InterNetwork);

            UdpClient client = UdpClient.Create
            (
                (s, o) => new BinaryReader(s, Encoding.ASCII, o),
                (s, o) => new BinaryWriter(s, Encoding.ASCII, o),
                IPAddress.Loopback.MapToIPv4().ToString(),
                port,
                System.Net.Sockets.AddressFamily.InterNetwork
            );

            byte[] server_received_bytes = null;
            ManualResetEvent server_bytes_arrived = new ManualResetEvent(false);

            Task receive = server.ReceiveAsync().ContinueWith(t =>
            {
                server_received_bytes = t.Result.Buffer.ToArray();
                server_bytes_arrived.Set();
            }, TaskContinuationOptions.NotOnFaulted);

            int max_size = UInt16.MaxValue - 20 /* IP header */ - 8 /* UDP header */;
            byte[] client_sent_bytes = new byte[max_size];
            Random rnd = new Random();
            rnd.NextBytes(client_sent_bytes);

            await client.SendAsync(writer => writer.Write(client_sent_bytes));

            bool server_bytes_arrived_signalled = server_bytes_arrived.WaitOne(10000);

            Assert.IsTrue(server_bytes_arrived_signalled, "Server timedout");

            await client.CloseAsync();
            server.Close();

            CollectionAssert.AreEqual(client_sent_bytes, server_received_bytes, "Recevied bytes are not the same as sent ones!");

            client.Dispose();
            server.Dispose();
        }

        [TestMethod]
        public async Task ListenAsyncTest()
        {
            int port = Convert.ToInt32(TestContext.Properties["LocalListenerPort"]);

            System.Net.Sockets.UdpClient server = new System.Net.Sockets.UdpClient(port, System.Net.Sockets.AddressFamily.InterNetwork);

            UdpClient thisClient = UdpClient.Create
            (
                (s, o) => new BinaryReader(s, Encoding.ASCII, o),
                (s, o) => new BinaryWriter(s, Encoding.ASCII, o),
                IPAddress.Loopback.MapToIPv4().ToString(),
                port,
                System.Net.Sockets.AddressFamily.InterNetwork
            );

            string receivedString = null;
            ManualResetEvent string_arrived_to_client = new ManualResetEvent(false);

            thisClient.DataReceived += (reader, remoteEndPoint) =>
            {
                receivedString = reader.ReadString();
                string_arrived_to_client.Set();
            };

            Task listenTask = thisClient.ListenAsync();

            string sentString = "abcdefg";

            using (MemoryStream mem = new MemoryStream())
            {
                BinaryWriter writer = new BinaryWriter(mem, Encoding.ASCII, true);
                writer.Write(sentString);
                writer.Flush();
                byte[] sentData = mem.ToArray();

                await thisClient.SendAsync(w => w.Write(0x00) );
                var remoteClient = await server.ReceiveAsync();

                await server.SendAsync(sentData, sentData.Length, remoteClient.RemoteEndPoint);
            }

            bool string_arrived_to_client_signalled = string_arrived_to_client.WaitOne(10000);

            Assert.IsTrue(string_arrived_to_client_signalled, "Server timedout");

            await thisClient.CloseAsync();
            server.Close();

            if (!listenTask.IsCanceled)
                await listenTask;


            Assert.IsNotNull(receivedString, "Client hasn't received data!");
            Assert.AreEqual(sentString, receivedString);
        }
    }
}