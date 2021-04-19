using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.Tests
{
    [TestClass]
    public class AuthenticationClientTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public async Task LiveGetTokenByPasswordAsyncTest()
        {
            SecureTcpClient tcp = SecureTcpClient.Create
            (
                createReader: (s, o) => new MessageBinaryReader(s, Encoding.UTF8, o),
                createWriter: (s, o) => new MessageBinaryWriter(s, Encoding.UTF8, o),
                connectToHostname: (string)TestContext.Properties["AuthenticationServerAddress"],
                connectToPort: Convert.ToInt32(TestContext.Properties["AuthenticationServerPort"]),
                enableSslProtocols: (SslProtocols)Enum.Parse(typeof(SslProtocols), (string)TestContext.Properties["AuthenticationServerSslProtocol"], true),
                networkLayerProtocol: AddressFamily.InterNetwork,
                explicitlyTrustedCertHashes: ((string)TestContext.Properties["AuthenticationServerCertHashes"]).Split(',', ';')
            );
            AuthenticationClient client = new AuthenticationClient(tcp);

            Task listen = tcp.ListenAsync();

            string loginName = (string)TestContext.Properties["PlayerLoginName"];
            string password = (string)TestContext.Properties["PlayerPassword"];

            Token token = await client.GetTokenByPasswordAsync(loginName, password);

            await Task.WhenAll(tcp.CloseAsync(), listen);

            Assert.IsNotNull(token.Name, "Token Name is null!");
            Assert.IsNotNull(token.Data, "Token Data is null!");
            Assert.AreEqual(loginName, token.Name, "Token Name is not the same as Login Name! (it may not be a bug; check server protocol)");
            Assert.AreEqual(32, token.Data.Length, "Token length in bytes.");
        }

        [TestMethod]
        public async Task LiveGetCookieByTokenAsyncTest()
        {
            SecureTcpClient tcp = SecureTcpClient.Create
            (
                createReader: (s, o) => new MessageBinaryReader(s, Encoding.UTF8, o),
                createWriter: (s, o) => new MessageBinaryWriter(s, Encoding.UTF8, o),
                connectToHostname: (string)TestContext.Properties["AuthenticationServerAddress"],
                connectToPort: Convert.ToInt32(TestContext.Properties["AuthenticationServerPort"]),
                enableSslProtocols: (SslProtocols)Enum.Parse(typeof(SslProtocols), (string)TestContext.Properties["AuthenticationServerSslProtocol"], true),
                networkLayerProtocol: AddressFamily.InterNetwork,
                explicitlyTrustedCertHashes: ((string)TestContext.Properties["AuthenticationServerCertHashes"]).Split(',', ';')
            );
            AuthenticationClient client = new AuthenticationClient(tcp);

            Task listen = tcp.ListenAsync();

            string loginName = (string)TestContext.Properties["PlayerLoginName"];
            string password = (string)TestContext.Properties["PlayerPassword"];

            Token token = await client.GetTokenByPasswordAsync(loginName, password);

            Cookie cookie = await client.GetCookieByTokenAsync(token.Name, token.Data);

            await Task.WhenAll(tcp.CloseAsync(), listen);

            Assert.IsNotNull(cookie.AccountName, "Account Name is null!");
            Assert.IsNotNull(cookie.Data, "Cookie Data is null!");
            Assert.AreEqual(loginName, cookie.AccountName, "Account Name is not the same as Login Name! (it may not be a bug; check server protocol)");
            Assert.AreEqual(32, cookie.Data.Length, "Token length in bytes.");
        }

        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public async Task LiveGetTokenByPasswordAsyncBadPasswordTest()
        {
            SecureTcpClient tcp = SecureTcpClient.Create
            (
                createReader: (s, o) => new MessageBinaryReader(s, Encoding.UTF8, o),
                createWriter: (s, o) => new MessageBinaryWriter(s, Encoding.UTF8, o),
                connectToHostname: (string)TestContext.Properties["AuthenticationServerAddress"],
                connectToPort: Convert.ToInt32(TestContext.Properties["AuthenticationServerPort"]),
                enableSslProtocols: (SslProtocols)Enum.Parse(typeof(SslProtocols), (string)TestContext.Properties["AuthenticationServerSslProtocol"], true),
                networkLayerProtocol: AddressFamily.InterNetwork,
                explicitlyTrustedCertHashes: ((string)TestContext.Properties["AuthenticationServerCertHashes"]).Split(',', ';')
            );
            AuthenticationClient client = new AuthenticationClient(tcp);

            Task listen = tcp.ListenAsync();

            string loginName = (string)TestContext.Properties["PlayerLoginName"];
            string password = (string)TestContext.Properties["PlayerPassword"];

            try
            {
                Token token = await client.GetTokenByPasswordAsync(loginName, password + password);
            }
            finally
            {
                await Task.WhenAll(tcp.CloseAsync(), listen);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public async Task LiveGetTokenByPasswordAsyncBadLoginNameTest()
        {
            SecureTcpClient tcp = SecureTcpClient.Create
            (
                createReader: (s, o) => new MessageBinaryReader(s, Encoding.UTF8, o),
                createWriter: (s, o) => new MessageBinaryWriter(s, Encoding.UTF8, o),
                connectToHostname: (string)TestContext.Properties["AuthenticationServerAddress"],
                connectToPort: Convert.ToInt32(TestContext.Properties["AuthenticationServerPort"]),
                enableSslProtocols: (SslProtocols)Enum.Parse(typeof(SslProtocols), (string)TestContext.Properties["AuthenticationServerSslProtocol"], true),
                networkLayerProtocol: AddressFamily.InterNetwork,
                explicitlyTrustedCertHashes: ((string)TestContext.Properties["AuthenticationServerCertHashes"]).Split(',', ';')
            );
            AuthenticationClient client = new AuthenticationClient(tcp);

            Task listen = tcp.ListenAsync();

            string loginName = (string)TestContext.Properties["PlayerLoginName"];
            string password = (string)TestContext.Properties["PlayerPassword"];

            try
            {
                Token token = await client.GetTokenByPasswordAsync(loginName + loginName, password);
            }
            finally
            {
                await Task.WhenAll(tcp.CloseAsync(), listen);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public async Task LiveGetCookieByTokenAsyncBadTokenTest()
        {
            SecureTcpClient tcp = SecureTcpClient.Create
            (
                createReader: (s, o) => new MessageBinaryReader(s, Encoding.UTF8, o),
                createWriter: (s, o) => new MessageBinaryWriter(s, Encoding.UTF8, o),
                connectToHostname: (string)TestContext.Properties["AuthenticationServerAddress"],
                connectToPort: Convert.ToInt32(TestContext.Properties["AuthenticationServerPort"]),
                enableSslProtocols: (SslProtocols)Enum.Parse(typeof(SslProtocols), (string)TestContext.Properties["AuthenticationServerSslProtocol"], true),
                networkLayerProtocol: AddressFamily.InterNetwork,
                explicitlyTrustedCertHashes: ((string)TestContext.Properties["AuthenticationServerCertHashes"]).Split(',', ';')
            );
            AuthenticationClient client = new AuthenticationClient(tcp);

            Task listen = tcp.ListenAsync();

            string loginName = (string)TestContext.Properties["PlayerLoginName"];

            try
            {
                Cookie cookie = await client.GetCookieByTokenAsync(loginName + loginName, new byte[32]);
            }
            finally
            {
                await Task.WhenAll(tcp.CloseAsync(), listen);
            }
        }
    }
}
