using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Model.Shared
{
    public class Command : ICommand
    {
        public virtual Action Execute { get; }

        public Command(Action execute)
        {
            this.Execute = execute;
        }
    }
}
