using HHSwarm.Native.Protocols.Hafen.Fakes;
using HHSwarm.Native.Protocols.Hafen.Messages;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.Tests
{
    [TestClass]
    public class SessionClientTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public async Task LoginToLiveAuthenticationServer()
        {
            using (ShimsContext.Create())
            {
                UdpClient session_transport = UdpClient.Create
                (
                    createReader: (s, o) => new MessageBinaryReader(s, Encoding.ASCII, o, false),
                    createWriter: (s, o) => new MessageBinaryWriter(s, Encoding.ASCII, o, false),
                    connectToHostname: (string)TestContext.Properties["GameServerAddress"],
                    connectToPort: int.Parse((string)TestContext.Properties["GameServerPort"])
                );

                SecureTcpClient auth_transport = SecureTcpClient.Create
                (
                    createReader: (s, o) => new MessageBinaryReader(s, Encoding.UTF8, o, true),
                    createWriter: (s, o) => new MessageBinaryWriter(s, Encoding.UTF8, o, true),
                    connectToHostname: (string)TestContext.Properties["AuthenticationServerAddress"],
                    connectToPort: int.Parse((string)TestContext.Properties["AuthenticationServerPort"]),
                    enableSslProtocols: (SslProtocols)Enum.Parse(typeof(SslProtocols), (string)TestContext.Properties["AuthenticationServerSslProtocol"], true),
                    explicitlyTrustedCertHashes: ((string)TestContext.Properties["AuthenticationServerCertHashes"]).Split(',', ';')
                );

                string tokenName = null, accountName = null;
                byte[] token = null, cookie = null;

                ISourceOfCredentials creds = new StubISourceOfCredentials()
                {
                    GetLoginNameAsync = () => Task.FromResult((string)TestContext.Properties["PlayerLoginName"]),
                    GetPasswordAsync = () => Task.FromResult((string)TestContext.Properties["PlayerPassword"]),
                    GetTokenNameAsync = () => Task.FromResult<string>(tokenName),
                    GetTokenAsync = () => Task.FromResult<byte[]>(token),
                    GetAccountNameAsync = () => Task.FromResult<string>(accountName),
                    GetCookieAsync = () => Task.FromResult<byte[]>(cookie),
                    SaveAccountNameAsyncString = (an) =>
                    {
                        accountName = an;
                        return Task.CompletedTask;
                    },
                    SaveCookieAsyncByteArray = (c) =>
                    {
                        cookie = c;
                        return Task.CompletedTask;
                    },
                    SaveTokenNameAsyncString = (tn) =>
                    {
                        tokenName = tn;
                        return Task.CompletedTask;
                    },
                    SaveTokenAsyncByteArray = (t) =>
                    {
                        token = t;
                        return Task.CompletedTask;
                    }
                };

                SessionClient session = new SessionClient(session_transport, new AuthenticationClient(auth_transport), TimeSpan.FromSeconds(5));

                bool? connected = null;
                session.MSG_SESS_RESPONSE_Received += async (msg) =>
                {
                    Trace.WriteLine($"{nameof(msg.ErrorCode)}: {msg.ErrorCode}", nameof(session.MSG_SESS_RESPONSE_Received));

                    connected = msg.ErrorCode == MSG_SESS.RESPONSE.ERROR_CODE.SUCCESS;

                    await session.DisconnectAsync();
                };

                session.MSG_CLOSE_Received += async (msg) =>
                {
                    Trace.WriteLine(String.Empty, nameof(session.MSG_CLOSE_Received));

                    await session.DisconnectAsync();
                };

                Task session_listen = session_transport.ListenAsync(200);
                Task auth_listen = auth_transport.ListenAsync(200);

                Trace.WriteLine("BEFORE", nameof(session.ConnectAsync));
                await session.ConnectAsync(creds);
                Trace.WriteLine("AFTER", nameof(session.ConnectAsync));

                await Task.Delay(3000); // if not to makedelay, MSG_SESS_RESPONSE can be processed later and 'connected' will be 'false'.

                Assert.IsTrue(connected == true, "Was not able to establish connection! Check debug output for details.");

                Trace.WriteLine("BEFORE", nameof(session.DisconnectAsync));
                await session.DisconnectAsync();
                Trace.WriteLine("AFTER", nameof(session.DisconnectAsync));

                await Task.Delay(5);
                await Task.WhenAll(auth_transport.CloseAsync(), session_transport.CloseAsync());

                try { await auth_listen; } catch (TaskCanceledException) { }
                try { await session_listen; } catch (TaskCanceledException) { }

                Assert.IsTrue(connected.HasValue, "MSG_SESS_RESPONSE has never been received!");
                Assert.IsTrue(connected.Value, "Server didn't confirm successful login!");
            }
        }
    }
}
