using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Model.Shared
{
    public interface ICommand
    {
        Action Execute { get; }
    }
}
