using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    public interface ISourceOfGameResources
    {
        Task<HavenResource1> GetResourceAsync(string resourceName);
    }
}
