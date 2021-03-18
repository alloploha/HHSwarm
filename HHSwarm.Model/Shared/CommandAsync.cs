using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Model.Shared
{
    public class CommandAsync : ICommandAsync
    {
        public virtual Func<Task> ExecuteAsync { get; }

        public CommandAsync(Func<Task> executeAsync)
        {
            this.ExecuteAsync = executeAsync;
        }
    }
}
