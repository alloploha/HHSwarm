using HHSwarm.Model.Haven;
using HHSwarm.Model.Shared;
using HHSwarm.Native;
using HHSwarm.Native.GameResources;
using HHSwarm.Native.Protocols;
using HHSwarm.Native.Protocols.v17;
using HHSwarm.Native.Protocols.v17.Messages;
using HHSwarm.Native.WorldModel;
using HHSwarm.TestClient.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HHSwarm.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cancellation = new CancellationTokenSource();

            Task main_task = MainAsync(args, cancellation.Token, () =>
            {
                cancellation.Cancel();
            });

            do
            {
                Thread.Sleep(166);

                if (Console.KeyAvailable)
                {
                    char key = Console.ReadKey(true).KeyChar;

                    switch (key)
                    {
                        case 'p':
                            Program.Play?.Invoke();
                            break;
                        case 'q':
                            Console.WriteLine("Exiting...");
                            Exiting();
                            cancellation.Cancel();
                            break;
                    }
                }
            } while (!cancellation.IsCancellationRequested);

            main_task.Wait();
        }

        static Action Exiting;
        static Action Play;


        static async Task MainAsync(string[] args, CancellationToken cancel, Action SessionCloseRequested)
        {
            CreateReaderDelegate create_reader_little_endian = (s, o) => new MessageBinaryReader(s, Encoding.UTF8, o, false);
            CreateWriterDelegate create_writer_little_endian = (s, o) => new MessageBinaryWriter(s, Encoding.UTF8, o, false);

            UdpClient session_transport = UdpClient.Create
            (
                create_reader_little_endian,
                create_writer_little_endian,
                Settings.Default.GameServerAddress,
                Settings.Default.GameServerPort
            );

            CreateReaderDelegate create_reader_big_endian = (s, o) => new MessageBinaryReader(s, Encoding.UTF8, o, true);
            CreateWriterDelegate create_writer_big_endian = (s, o) => new MessageBinaryWriter(s, Encoding.UTF8, o, true);

            SecureTcpClient authentication_transport = SecureTcpClient.Create
            (
                create_reader_big_endian,
                create_writer_big_endian,
                Settings.Default.AuthenticationServerAddress,
                Settings.Default.AuthenticationServerPort,
                System.Net.Sockets.AddressFamily.InterNetwork,
                (SslProtocols)Enum.Parse(typeof(SslProtocols), Settings.Default.AuthenticationServerSslProtocol, true),
                Settings.Default.AuthenticationServerCertHashes.Cast<string>()
            );


            AuthenticationClient authentication = new AuthenticationClient(authentication_transport);

            PersistedToIsolatedStorageCredentials credentials = new PersistedToIsolatedStorageCredentials();

            SessionClient session = new SessionClient(session_transport, authentication, Settings.Default.HeartBeatInterval);
            session.RenewToken += async () =>
            {
                await Task.WhenAll
                (
                    credentials.SaveCookieAsync(null),
                    credentials.SaveAccountNameAsync(null),
                    credentials.SaveTokenAsync(null),
                    credentials.SaveTokenNameAsync(null)
                );

                await session.ConnectAsync(new Credentials(Settings.Default.PlayerLoginName, Settings.Default.PlayerPassword))
                .ContinueWith(t =>
                {
                    t.Exception.Handle((e) =>
                    {
                        if (e is AuthenticationException)
                        {
                            Console.WriteLine(e.Message);
                            return true;
                        }
                        return false;
                    });
                }, TaskContinuationOptions.OnlyOnFaulted);
            };

            Program.Exiting = async () => await session.SendAsync(new MSG_CLOSE());

            RelayClient relay = new RelayClient(create_reader_little_endian, create_writer_little_endian, session);

            #region GLOBLOB

            GlobalObjects globalObjects = new GlobalObjects();

            relay.RMSG_GLOBLOB_Received += (msg) =>
            {
                globalObjects.Receive(msg.Objects);
            };

            #endregion

            #region OBJDATA

            session.MSG_OBJDATA_Received += (msg) =>
            {

            };

            #endregion

            ISourceOfGameResources remote_resources = new WebSourceOfGameResources(Settings.Default.ResourceServerBaseUri);

            if (!String.IsNullOrWhiteSpace(Settings.Default.ExtractResourcesTo))
            {
                remote_resources = new DumpResourceLayersToFiles(remote_resources, Settings.Default.ExtractResourcesTo);
            }

            ResourcesBindingClient client = new ResourcesBindingClient(relay, new CachedGameResources(), remote_resources);

            /* WIRES */

            session.MSG_SESS_RESPONSE_Received += async (msg) =>
            {
                if (msg.ErrorCode == MSG_SESS.RESPONSE.ERROR_CODE.SUCCESS)
                {
                    try
                    {
                        await authentication_transport.CloseAsync();
                    }
                    catch (Exception)
                    {
                        Debugger.Break();
                    }
                }
            };

            session.MSG_CLOSE_Received += async (msg) =>
            {
                try
                {
                    await session_transport.CloseAsync();
                    SessionCloseRequested();
                }
                catch (Exception)
                {
                    Debugger.Break();
                }
            };

            /* GO LIVE */

            Task authentication_listen = authentication_transport.ListenAsync();
            Task session_listen = session_transport.ListenAsync(cancel);

            ModelFactory_v17 factory = new ModelFactory_v17(relay, session, client);

            Account account = factory.ConstructAccount
            (
                GetCredentialsStore: (accountCredentials) =>
                {
                    return new Credentials(accountCredentials.LoginName, accountCredentials.Password);
                },
                GetAccountCredentialsAsync: () =>
                {
                    return Task.FromResult(new AccountCredentials(Settings.Default.PlayerLoginName, Settings.Default.PlayerPassword));
                }
            );

            account.Authorized += (s, a) => Console.WriteLine("Connected as " + a.Name);
            account.Characters.WidgetInfoIsReady += (widget) => Console.WriteLine($"Widget ID: {widget.WidgetInfo.ID}, Height = {widget.WidgetInfo.Capacity}");

            account.Characters.CharacterAdded += (list, character) =>
            {
                Console.WriteLine($"Can play as {character.Name} now.");
                Program.Play = async () =>
                {
                    Console.WriteLine($"Play as {character.Name}...");
                    await character.Play.ExecuteAsync();
                };
            };

            await account.Login.ExecuteAsync();

            await session_listen;
        }
    }

    class Credentials : PersistedToIsolatedStorageCredentials, ISourceOfCredentials
    {
        private string LoginName;
        private string Password;

        public Credentials(string loginName, string password)
        {
            this.LoginName = loginName;
            this.Password = password;
        }

        public Task<string> GetLoginNameAsync()
        {
            return Task.FromResult(LoginName);
        }

        public Task<string> GetPasswordAsync()
        {
            return Task.FromResult(Password);
        }
    }
}
