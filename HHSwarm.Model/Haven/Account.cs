using HHSwarm.Model.Shared;
using HHSwarm.Model.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Model.Haven
{
    public class Account
    {
        public string Name { get; }

        public CharactersList Characters { get; }

        public Account(string name, Task<CharactersListWidget> charactersListWidget, ICommandAsync login)
        {
            this.Name = name;

            this.Characters = new CharactersList(charactersListWidget);

            this.Login = new LoginCommand(login, (sender) =>
            {
                this.IsAuthorized = true;
                Authorized?.Invoke(sender, this);
            });
        }

        public bool IsAuthorized { get; private set; }
        public event EventHandler<Account> Authorized;

        public class LoginCommand : ICommandAsync
        {
            private ICommandAsync Command;
            private Action<object> OnAuthorized;

            public Func<Task> ExecuteAsync => () => Command.ExecuteAsync().ContinueWith(t => OnAuthorized(this), TaskContinuationOptions.OnlyOnRanToCompletion);

            public LoginCommand(ICommandAsync command, Action<object> onAuthorized)
            {
                this.Command = command;
                this.OnAuthorized = onAuthorized;
            }
        }

        public ICommandAsync Login { get; }
    }
}
