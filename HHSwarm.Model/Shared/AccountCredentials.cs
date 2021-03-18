using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Model.Shared
{
    public class AccountCredentials
    {
        public readonly string LoginName;
        public readonly string Password;

        public AccountCredentials(string loginName, string password)
        {
            this.LoginName = loginName;
            this.Password = password;
        }
    }
}
