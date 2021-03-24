using HHSwarm.Native.Common;
using HHSwarm.Native.Protocols.v17.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using static HHSwarm.Native.Protocols.TransportProtocol;

namespace HHSwarm.Native.Protocols.v17
{
    public class SessionClient : SessionProtocol
    {
        private IAuthenticationClientAsync Authentication;

        private System.Timers.Timer MSG_BEAT_Timer;

        /*
         * CMD_PW(LoginName + Password) => TokenName, MKTOKEN() => Token
         * Token, COOKIE() => Cookie
         * TOKEN(TokenName, Token) => AccountName
         * MSG_SESS(0, AccountName + Cookie) => Session
         */

        public SessionClient(TransportProtocol transport, IAuthenticationClientAsync authentication, TimeSpan heartBeatInterval):
            base(transport, SessionContext.Client)
        {
            this.Authentication = authentication;//?? throw new ArgumentNullException();

            this.Connected += SessionClient_Connected;
            this.ConnectionFailed += SessionClient_ConnectionFailed;
            this.ClientIsTooOld += SessionClient_ConnectionFailed;

            this.InvalidToken += RenewAuthenticationToken;
            this.TokenExpired += RenewAuthenticationToken;

            MSG_BEAT_Timer = new System.Timers.Timer(heartBeatInterval.TotalMilliseconds)
            {
                AutoReset = true
            };

            MSG_BEAT_Timer.Elapsed += MSG_BEAT_Timer_Elapsed;
            this.Connected += () => MSG_BEAT_Timer.Start();
        }

        #region MSG_REL
        private BadMemorySet<ushort> MSG_REL_Received_SequenceNumbers = new BadMemorySet<ushort>(128, 512);

        public override async Task ReceiveAsync(MSG_REL raw_message)
        {
            ushort sequnce_number = raw_message.SequenceNumber;
            var splitted_messages = Split(raw_message, ref sequnce_number);

            List<Task> tasks = new List<Task>();

            foreach (MSG_REL m in splitted_messages.OrderByDescending(m => m.SequenceNumber))
            {
                tasks.Add(SendAsync(new MSG_ACK()
                {
                    RelayMessageSequenceNumber = m.SequenceNumber
                }));
            }

            foreach (MSG_REL m in splitted_messages.OrderBy(m => m.SequenceNumber))
            {
                bool is_new_number = MSG_REL_Received_SequenceNumbers.Add(m.SequenceNumber);

                if (is_new_number)
                {
                    tasks.Add(base.ReceiveAsync(m));
                }
            }

            await Task.WhenAll(tasks);
        }

        private IEnumerable<MSG_REL> Split(MSG_REL message, ref ushort impliedSequenceNumber)
        {
            List<MSG_REL> result = new List<MSG_REL>();

            using (MemoryStream mem = new MemoryStream(message.RelayedData))
            {
                BinaryReader reader = Transport.CreateReader(mem, false);

                do
                {
                    long position_before_header = mem.Position;

                    byte header = reader.ReadByte();

                    const byte TYPE_SPECIFIED_FLAG_MASK = 0x80;

                    if ((header & TYPE_SPECIFIED_FLAG_MASK) == TYPE_SPECIFIED_FLAG_MASK)
                    {
                        byte type = (byte)(header ^ TYPE_SPECIFIED_FLAG_MASK);
                        ushort data_length = reader.ReadUInt16();

                        byte[] data = new byte[1 + data_length];
                        data[0] = type;

                        Array.Copy(reader.ReadBytes(data_length), 0, data, 1, data_length);

                        result.AddRange(Split
                        (
                            new MSG_REL()
                            {
                                SequenceNumber = impliedSequenceNumber,
                                RelayedData = data
                            }, 
                            ref impliedSequenceNumber
                        ));
                    }
                    else
                    {
                        mem.Position = position_before_header;

                        result.Add(new MSG_REL()
                        {
                            SequenceNumber = impliedSequenceNumber,
                            RelayedData = reader.ReadBytes((int)mem.Length)
                        });

                        impliedSequenceNumber++;
                    }
                } while (mem.Position < mem.Length);
            }

            return result;
        }
        #endregion

        private async void MSG_BEAT_Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs args)
        {
            try
            {
                await SendAsync(new MSG_BEAT());
            }
            catch (InvalidOperationException e)
            {
                Trace.TraceInformation(e.Message);
                ((System.Timers.Timer)sender).Stop();
            }
        }

        public delegate void RenewTokenDelegate();
        public event RenewTokenDelegate RenewToken;

        private void RenewAuthenticationToken()
        {
            RenewToken?.Invoke();
        }


        public delegate void AuthenticationFailed_Delegate(string serverErrorMessage);

        public event AuthenticationFailed_Delegate AuthenticationFailed;

        public override async Task ReceiveAsync(MSG_SESS.RESPONSE message)
        {
            switch (message.ErrorCode)
            {
                case MSG_SESS.RESPONSE.ERROR_CODE.SUCCESS:
                    Connected?.Invoke();
                    break;
                case MSG_SESS.RESPONSE.ERROR_CODE.SESSERR_AUTH:
                    InvalidToken?.Invoke();
                    break;
                case MSG_SESS.RESPONSE.ERROR_CODE.SESSERR_BUSY:
                    AlreadyLoggedIn?.Invoke();
                    break;
                case MSG_SESS.RESPONSE.ERROR_CODE.SESSERR_CONN:
                    ConnectionFailed?.Invoke();
                    break;
                case MSG_SESS.RESPONSE.ERROR_CODE.SESSERR_PVER:
                    ClientIsTooOld?.Invoke();
                    break;
                case MSG_SESS.RESPONSE.ERROR_CODE.SESSERR_EXPR:
                    TokenExpired?.Invoke();
                    break;
                default:
                    throw new NotImplementedException($"{nameof(message.ErrorCode)}: {message.ErrorCode}");
            }

            await base.ReceiveAsync(message);
        }


        private delegate void MSG_SESS_RESPONSE_Delegate();

        private event MSG_SESS_RESPONSE_Delegate Connected;
        private event MSG_SESS_RESPONSE_Delegate InvalidToken;
        private event MSG_SESS_RESPONSE_Delegate AlreadyLoggedIn;
        private event MSG_SESS_RESPONSE_Delegate ConnectionFailed;
        private event MSG_SESS_RESPONSE_Delegate ClientIsTooOld;
        private event MSG_SESS_RESPONSE_Delegate TokenExpired;

        /// <summary>
        /// Returns after session connection established and confirmed by <see cref="MSG_SESS.RESPONSE.ErrorCode"/>.
        /// </summary>
        public async Task ConnectAsync(ISourceOfCredentials credentialsStore)
        {
            string accountName = await credentialsStore.GetAccountNameAsync();
            byte[] cookie = await credentialsStore.GetCookieAsync();

            await ConnectAsync(accountName, cookie, credentialsStore);
        }


        Queue<TaskCompletionSource<bool>> MSG_SESS_Requests = new Queue<TaskCompletionSource<bool>>();

        private void SessionClient_Connected()
        {
            var tsc = MSG_SESS_Requests.Dequeue();
            tsc.SetResult(true);
        }

        private bool KeepTryingToConnect = true;

        private void SessionClient_ConnectionFailed()
        {
            var tsc = MSG_SESS_Requests.Dequeue();
            tsc.SetResult(false);

            KeepTryingToConnect = false;
        }

        private async Task ConnectAsync(string accountName, byte[] cookieData, ISourceOfCredentials credentialsStore)
        {
            if (cookieData != null && cookieData.Length > 0 && !String.IsNullOrEmpty(accountName))
            {
                TaskCompletionSource<bool> msg_sess_tcs = new TaskCompletionSource<bool>();
                MSG_SESS_Requests.Enqueue(msg_sess_tcs);

                try
                {
                    await SendAsync(new MSG_SESS.REQUEST()
                    {
                        AccountName = accountName,
                        Cookie = cookieData
                    });
                }
                catch (AuthenticationException e)
                {
                    MSG_SESS_Requests.Dequeue().SetException(e);
                }

                bool connected = false;

                try
                {
                    connected = await msg_sess_tcs.Task;
                }
                catch (AuthenticationException e)
                {
                    Trace.TraceInformation(e.Message);
                    await credentialsStore.SaveAccountNameAsync(null);
                    await credentialsStore.SaveCookieAsync(null);
                }

                if (!connected && KeepTryingToConnect)
                {
                    await ConnectAsync(credentialsStore);
                }
            }
            else
            {
                string tokenName = await credentialsStore.GetTokenNameAsync();
                byte[] tokenData = await credentialsStore.GetTokenAsync();

                if (tokenData != null && tokenData.Length > 0 && !String.IsNullOrEmpty(tokenName))
                {
                    Cookie cookie = null;

                    try
                    {
                        cookie = await Authentication.GetCookieByTokenAsync(tokenName, tokenData);
                    }
                    catch (AuthenticationException e)
                    {
                        Trace.TraceInformation(e.Message);
                        await credentialsStore.SaveTokenNameAsync(null);
                        await credentialsStore.SaveTokenAsync(null);
                    }

                    if (cookie != null)
                    {
                        await credentialsStore.SaveAccountNameAsync(cookie.AccountName);
                        await credentialsStore.SaveCookieAsync(cookie.Data);
                    }

                    await ConnectAsync(credentialsStore);
                }
                else
                {
                    string loginName = await credentialsStore.GetLoginNameAsync();
                    string password = await credentialsStore.GetPasswordAsync();

                    Token token = null;

                    try
                    {
                        token = await Authentication.GetTokenByPasswordAsync(loginName, password);
                    }
                    catch (AuthenticationException e)
                    {
                        Trace.TraceInformation(e.Message);
                        AuthenticationFailed?.Invoke(e.Message);
                    }

                    if (token != null)
                    {
                        await credentialsStore.SaveTokenNameAsync(token.Name);
                        await credentialsStore.SaveTokenAsync(token.Data);

                        await ConnectAsync(credentialsStore);
                    }
                }
            }
        }

        public async Task DisconnectAsync()
        {
            MSG_BEAT_Timer.Stop();
            await SendAsync(new MSG_CLOSE());
        }

        public override async Task ReceiveAsync(MSG_OBJDATA message)
        {
            /* Send confirmation to server. */

            MSG_OBJACK msg_objack = new MSG_OBJACK();

            msg_objack.DataFrames.AddRange(message.DataFrames.Select(_ => new MSG_OBJACK.GameObjectDataFrame()
            {
                FrameID = _.FrameID,
                ObjectDataID = _.ObjectDataID
            }));
            
            await Task.WhenAll
            (
                SendAsync(msg_objack),
                base.ReceiveAsync(message)
            );
        }
    }
}
