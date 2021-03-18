using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <summary>
    /// '@LayerName("tooltip")', 'class Tooltip'
    /// </summary>
    [Serializable]
    public class TooltipResourceLayer : ResourceLayer
    {
        /// <summary>
        /// 't'
        /// </summary>
        public string Text;
    }
}
