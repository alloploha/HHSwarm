using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen
{
    public interface ISourceOfCredentials
    {
        Task<string> GetLoginNameAsync();
        Task<string> GetPasswordAsync();
        Task<string> GetAccountNameAsync();
        Task SaveAccountNameAsync(string accountName);
        Task<string> GetTokenNameAsync();
        Task SaveTokenNameAsync(string tokenName);
        Task<byte[]> GetTokenAsync();
        Task SaveTokenAsync(byte[] token);
        Task<byte[]> GetCookieAsync();
        Task SaveCookieAsync(byte[] cookie);
    }
}
