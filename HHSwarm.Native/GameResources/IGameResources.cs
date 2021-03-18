using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    public interface IGameResources
    {
        bool Contains(string resourceName);
        Task AddAsync(string resourceName, HavenResource1 resource);
        Task<HavenResource1> GetAsync(string resourceName, ushort? resourceVersion = null);
    }
}
