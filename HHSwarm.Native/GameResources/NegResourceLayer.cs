using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <summary>
    /// '@LayerName("neg")'
    /// </summary>
    [Serializable]
    public class NegResourceLayer : ResourceLayer
    {
        public Point CC;
        public Point[][] EP;
    }
}
