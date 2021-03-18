using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.v17
{
    public interface IAuthenticationClientAsync
    {
        Task<Cookie> GetCookieByTokenAsync(string tokenName, byte[] token);
        Task<Token> GetTokenByPasswordAsync(string loginName, string password);
    }
}