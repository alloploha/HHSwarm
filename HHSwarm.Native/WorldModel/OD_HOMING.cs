using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.WorldModel
{
    public class OD_HOMING : IGameObjectDatum
    {
        /// <summary>
        /// 'tgtc'
        /// </summary>
        public Point TargetCoordinates;

        /// <summary>
        /// 'v' - Speed
        /// </summary>
        public double Velocity;

        /// <summary>
        /// 'homostop'
        /// </summary>
        public class REMOVE : OD_HOMING
        {
        }
    }
}
