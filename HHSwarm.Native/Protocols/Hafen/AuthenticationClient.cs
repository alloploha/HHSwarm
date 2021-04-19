using HHSwarm.Native.Protocols.Hafen.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen
{
    public class AuthenticationClient : AuthenticationProtocol, IAuthenticationClientAsync
    {
        public AuthenticationClient(TransportProtocol transport) : base(transport)
        {
        }

        private byte[] PasswordHash(string password)
        {
            var hashAlgorithm = SHA256.Create();
            return hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        #region GetTokenByPasswordAsync
        Queue<TaskCompletionSource<string>> CMD_PW_Requests = new Queue<TaskCompletionSource<string>>();

        public async override Task ReceiveAsync(CMD_PW.RESPONSE_OK message)
        {
            var tsc = CMD_PW_Requests.Dequeue();
            tsc.SetResult(message.TokenName);
            await base.ReceiveAsync(message);
        }

        public async override Task ReceiveAsync(CMD_PW.RESPONSE_NO message)
        {
            var tsc = CMD_PW_Requests.Dequeue();
            tsc.SetException(new AuthenticationException(message.ErrorMessage));
            await base.ReceiveAsync(message);
        }

        Queue<TaskCompletionSource<byte[]>> CMD_MKTOKEN_Requests = new Queue<TaskCompletionSource<byte[]>>();

        public async override Task ReceiveAsync(CMD_MKTOKEN.RESPONSE_OK message)
        {
            var tsc = CMD_MKTOKEN_Requests.Dequeue();
            tsc.SetResult(message.Token);
            await base.ReceiveAsync(message);
        }

        public async override Task ReceiveAsync(CMD_MKTOKEN.RESPONSE_NO message)
        {
            var tsc = CMD_MKTOKEN_Requests.Dequeue();
            tsc.SetException(new AuthenticationException());
            await base.ReceiveAsync(message);
        }

        /// <summary>
        /// <code>
        /// SecureTcpClient tcp = ...
        /// AuthenticationProtocol auth = new AuthenticationProtocol(tcp);
        /// AuthenticationClient client = new AuthenticationClient(auth);
        /// Task listen = auth.ListenAsync();
        /// Token token = await client.GetTokenByPasswordAsync(loginName, password);
        /// Cookie cookie = await client.GetCookieByTokenAsync(token.Name, token.Data);
        /// await Task.WhenAll(auth.CloseAsync(), listen);
        /// </code>
        /// </summary>
        public async Task<Token> GetTokenByPasswordAsync(string loginName, string password)
        {
            TaskCompletionSource<string> cmd_pw_tsc = new TaskCompletionSource<string>();
            CMD_PW_Requests.Enqueue(cmd_pw_tsc);

            await SendAsync(new CMD_PW.REQUEST()
            {
                LoginName = loginName,
                PasswordHash = PasswordHash(password)
            });

            string tokenName = await cmd_pw_tsc.Task;

            TaskCompletionSource<byte[]> cmd_mktoken_tsc = new TaskCompletionSource<byte[]>();
            CMD_MKTOKEN_Requests.Enqueue(cmd_mktoken_tsc);

            await SendAsync(new CMD_MKTOKEN.REQUEST());

            byte[] token = await cmd_mktoken_tsc.Task;

            return new Token()
            {
                Name = tokenName,
                Data = token
            };
        }
        #endregion

        #region GetCookieByTokenAsync
        Queue<TaskCompletionSource<string>> CMD_TOKEN_Requests = new Queue<TaskCompletionSource<string>>();

        public async override Task ReceiveAsync(CMD_TOKEN.RESPONSE_OK message)
        {
            var tsc = CMD_TOKEN_Requests.Dequeue();
            tsc.SetResult(message.AccountName);
            await base.ReceiveAsync(message);
        }

        public async override Task ReceiveAsync(CMD_TOKEN.RESPONSE_NO message)
        {
            var tsc = CMD_TOKEN_Requests.Dequeue();
            tsc.SetException(new AuthenticationException(message.ErrorMessage));
            await base.ReceiveAsync(message);
        }

        Queue<TaskCompletionSource<byte[]>> CMD_COOKIE_Requests = new Queue<TaskCompletionSource<byte[]>>();

        public async override Task ReceiveAsync(CMD_COOKIE.RESPONSE_OK message)
        {
            var tsc = CMD_COOKIE_Requests.Dequeue();
            tsc.SetResult(message.Cookie);
            await base.ReceiveAsync(message);
        }

        public async override Task ReceiveAsync(CMD_COOKIE.RESPONSE_NO message)
        {
            var tsc = CMD_COOKIE_Requests.Dequeue();
            tsc.SetException(new AuthenticationException());
            await base.ReceiveAsync(message);
        }

        public async Task<Cookie> GetCookieByTokenAsync(string tokenName, byte[] token)
        {
            TaskCompletionSource<string> cmd_token_tsc = new TaskCompletionSource<string>();
            CMD_TOKEN_Requests.Enqueue(cmd_token_tsc);

            await SendAsync(new CMD_TOKEN.REQUEST()
            {
                TokenName = tokenName,
                Token = token
            });

            string accountName = await cmd_token_tsc.Task;

            TaskCompletionSource<byte[]> cmd_cookie_tsc = new TaskCompletionSource<byte[]>();
            CMD_COOKIE_Requests.Enqueue(cmd_cookie_tsc);

            await SendAsync(new CMD_COOKIE.REQUEST());

            byte[] cookie = await cmd_cookie_tsc.Task;

            return new Cookie()
            {
                AccountName = accountName,
                Data = cookie
            };
        }
        #endregion
    }
}
