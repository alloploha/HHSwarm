using HHSwarm.Native.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    /// <summary>
    /// Параметры создания виджета Рюкзак (Inventory)
    /// </summary>
    /// <remarks>
    /// @RName("inv")
    /// class Inventory
    /// </remarks>
    class InventoryWidgetCreateArguments
    {
        /// <summary>
        /// 'sz'
        /// </summary>
        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/c90a0aea7a84f7ab738c1d7c40556ae50decf03f/src/haven/Inventory.java#L75
        /// </remarks>
        public Coord2i Size;
    }
}
