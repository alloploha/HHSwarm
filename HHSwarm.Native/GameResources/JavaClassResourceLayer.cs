using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <summary>
    /// '@LayerName("code")'
    /// </summary>
    [Serializable]
    public class JavaClassResourceLayer : ResourceLayer
    {
        public string Name;
        public byte[] Code;
    }
}
