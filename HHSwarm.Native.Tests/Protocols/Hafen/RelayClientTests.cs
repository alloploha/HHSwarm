using Microsoft.VisualStudio.TestTools.UnitTesting;
using HHSwarm.Native.Protocols.Hafen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHSwarm.Native.Protocols.Hafen.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using HHSwarm.Native.Protocols.Hafen.Messages;
using System.IO;
using HHSwarm.Native.Protocols.Fakes;

namespace HHSwarm.Native.Protocols.Hafen.Tests
{
    [TestClass()]
    public class RelayClientTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod()]
        public async Task RelayClient_NEWWDG_Test()
        {
            using (ShimsContext.Create())
            {
                StubTransportProtocol session_transport = new StubTransportProtocol
                (
                    (s, o) => new MessageBinaryReader(s, Encoding.UTF8, o), 
                    (s, o) => new MessageBinaryWriter(s, Encoding.UTF8, o)
                );

                session_transport.SendAsyncActionOfBinaryWriter = (w) => Task.CompletedTask;

                IAuthenticationClientAsync auth = new StubIAuthenticationClientAsync();
                ISourceOfCredentials creds = new StubISourceOfCredentials();
                SessionClient session = new SessionClient(session_transport, auth, TimeSpan.FromSeconds(5));

                //StubIMSG_REL_Hub session = new StubIMSG_REL_Hub();

                RelayClient relay = new RelayClient
                (
                    (s, o) => new MessageBinaryReader(s, Encoding.UTF8, o, false),
                    (s, o) => new MessageBinaryWriter(s, Encoding.UTF8, o, false),
                    session
                );

                SessionMessageFormatter smf = new SessionMessageFormatter(SessionProtocol.SessionContext.Client);

                MemoryStream mem = new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Protocols\Hafen\Samples\MSG_REL_NEWWDG.bin")));
                MSG_REL msg_rel;
                mem.Position = 1; // skip "(byte)MSG.TYPE.MSG_REL"
                smf.Deserialize(new MessageBinaryReader(mem, Encoding.UTF8, false, false), out msg_rel);
                mem.Dispose();

                await session.ReceiveAsync(msg_rel);
            }
        }
    }
}