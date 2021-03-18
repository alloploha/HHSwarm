using HHSwarm.Native.Protocols.v17;
using HHSwarm.TestClient.Properties;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;

namespace HHSwarm.TestClient
{
    class PersistedToIsolatedStorageCredentials
    {
        private IsolatedStorageFile Store = IsolatedStorageFile.GetUserStoreForAssembly();

        private void EnsureDirectoryExists(string filePath)
        {
            string directory_path = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory_path))
            {
                Store.CreateDirectory(directory_path);
            }
        }

        private string FilePath(string name)
        {
            return System.IO.Path.Combine(nameof(PersistedToIsolatedStorageCredentials), Settings.Default.PlayerLoginName, name);
        }

        public async Task<byte[]> GetCookieAsync()
        {
            string file_name = FilePath("Cookie");

            byte[] result = null;

            if (Store.FileExists(file_name))
            {
                using (var file = Store.OpenFile(file_name, System.IO.FileMode.Open))
                {
                    BinaryReader reader = new BinaryReader(file);
                    result = reader.ReadBytes((int)file.Length);
                    file.Close();
                    await Task.CompletedTask;
                }
            }

            return result;
        }

        public async Task<byte[]> GetTokenAsync()
        {
            string file_name = FilePath("Token");

            byte[] result = null;

            if (Store.FileExists(file_name))
            {
                using (var file = Store.OpenFile(file_name, System.IO.FileMode.Open))
                {
                    BinaryReader reader = new BinaryReader(file);
                    result = reader.ReadBytes((int)file.Length);
                    file.Close();
                    await Task.CompletedTask;
                }
            }

            return result;
        }

        public async Task<string> GetTokenNameAsync()
        {
            string file_name = FilePath("TokenName");

            string result = null;

            if (Store.FileExists(file_name))
            {
                using (var file = Store.OpenFile(file_name, System.IO.FileMode.Open))
                {
                    StreamReader reader = new StreamReader(file);
                    result = await reader.ReadToEndAsync();
                    file.Close();
                }
            }

            return result;
        }

        public async Task<string> GetAccountNameAsync()
        {
            string file_name = FilePath("AccountName");

            string result = null;

            if (Store.FileExists(file_name))
            {
                using (var file = Store.OpenFile(file_name, System.IO.FileMode.Open))
                {
                    StreamReader reader = new StreamReader(file);
                    result = await reader.ReadToEndAsync();
                    file.Close();
                }
            }

            return result;
        }

        public async Task SaveAccountNameAsync(string accountName)
        {
            string file_name = FilePath("AccountName");

            if (string.IsNullOrEmpty(accountName) && Store.FileExists(file_name))
                Store.DeleteFile(file_name);
            else
            {
                EnsureDirectoryExists(file_name);
                using (var file = Store.OpenFile(file_name, System.IO.FileMode.OpenOrCreate))
                {
                    file.SetLength(0);
                    StreamWriter writer = new StreamWriter(file);
                    await writer.WriteAsync(accountName);
                    await writer.FlushAsync();
                    file.Close();
                }
            }
        }

        public async Task SaveCookieAsync(byte[] cookie)
        {
            string file_name = FilePath("Cookie");

            if (cookie == null && Store.FileExists(file_name))
                Store.DeleteFile(file_name);
            else
            {
                EnsureDirectoryExists(file_name);
                using (var file = Store.OpenFile(file_name, System.IO.FileMode.OpenOrCreate))
                {
                    file.SetLength(0);
                    BinaryWriter writer = new BinaryWriter(file);
                    writer.Write(cookie);
                    writer.Flush();
                    file.Close();
                    await Task.CompletedTask;
                }
            }
        }

        public async Task SaveTokenAsync(byte[] token)
        {
            string file_name = FilePath("Token");

            if (token == null && Store.FileExists(file_name))
                Store.DeleteFile(file_name);
            else
            {
                EnsureDirectoryExists(file_name);
                using (var file = Store.OpenFile(file_name, System.IO.FileMode.OpenOrCreate))
                {
                    file.SetLength(0);
                    BinaryWriter writer = new BinaryWriter(file);
                    writer.Write(token);
                    writer.Flush();
                    file.Close();
                    await Task.CompletedTask;
                }
            }
        }

        public async Task SaveTokenNameAsync(string tokenName)
        {
            string file_name = FilePath("TokenName");

            if (string.IsNullOrEmpty(tokenName) && Store.FileExists(file_name))
                Store.DeleteFile(file_name);
            else
            {
                EnsureDirectoryExists(file_name);
                using (var file = Store.OpenFile(file_name, System.IO.FileMode.OpenOrCreate))
                {
                    file.SetLength(0);
                    StreamWriter writer = new StreamWriter(file);
                    await writer.WriteAsync(tokenName);
                    await writer.FlushAsync();
                    file.Close();
                }
            }
        }
    }
}
