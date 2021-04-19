using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    /// <summary>
    /// Controls speed: walk..run etc.
    /// <c>java: class Speedget</c>
    /// </summary>
    /// <remarks>
    /// https://github.com/dolda2000/hafen-client/blob/f85b82305e06f850c924d3309de68eedbd9209dd/src/haven/Speedget.java#L56
    /// </remarks>
    class GearboxWidgetCreateArguments : ResourceNamesWidgetCreateArguments<GearboxWidgetCreateArguments.RESOURCE_NAME>
    {
        [Flags]
        public enum RESOURCE_NAME
        {
            Crawl = 0x001,
            Walk = 0x002,
            Run = 0x004,
            Sprint = 0x008,
            Disabled = 0x000,
            On = 0x010,
            Off = 0x020,
            Image = 0x000,
            Tooltip = 0x100
        }

        /// <summary>
        /// <c>java: Speedget.cur</c>
        /// </summary>
        public int CurrentSpeed;

        /// <summary>
        /// <c>java: Speedget.max</c>
        /// </summary>
        public int MaximalSpeed;
    }
}
