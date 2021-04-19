using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    /// <remarks>
    /// @RName("epry")
    /// </remarks>
    class EquiporyWidgetCreateArguments
    {
        /// <summary>
        /// 'gobid'
        /// </summary>
        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/974366a68c0e61ee175a678d574f863705eac352/src/haven/Equipory.java#L85
        /// </remarks>
        public int? ObjectID;

        /// <summary>
        /// <code>Tex bg = Resource.loadtex("gfx/hud/equip/bg")</code>
        /// </summary>
        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/974366a68c0e61ee175a678d574f863705eac352/src/haven/Equipory.java#L33
        /// </remarks>
        public string BackgroundImageResourceName;
    }
}
