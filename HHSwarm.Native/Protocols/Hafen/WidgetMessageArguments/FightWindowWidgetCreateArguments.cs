using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    /// <remarks>
    /// @RName("fmg")
    /// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/FightWnd.java#L725
    /// </remarks>
    class FightWindowWidgetCreateArguments
    {
        /// <summary>
        /// 'nsave'
        /// </summary>
        public int nsave;

        /// <summary>
        /// 'nact'
        /// </summary>
        public int nact;

        /// <summary>
        /// 'max'
        /// </summary>
        public int max;
    }
}
