using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    /// <summary>
    /// '@RName("epry")', 'class Equipory'
    /// </summary>
    class EquiporyWidgetCreateArguments
    {
        /// <summary>
        /// 'gobid'
        /// </summary>
        public int? ObjectID;

        /// <summary>
        /// 'Tex bg = Resource.loadtex("gfx/hud/equip/bg")'
        /// </summary>
        public string BackgroundImageResourceName;
    }
}
