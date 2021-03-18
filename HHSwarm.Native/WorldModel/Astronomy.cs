using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.WorldModel
{
    /// <summary>
    /// 'astro', 'Astronomy'
    /// </summary>
    public class Astronomy
    {
        /// <summary>
        /// 'dt', 24 hours, angle in turns for: 0.25-0.75 at day, 0.75-1.25 at night, but <see cref="IsNight"/> also.
        /// </summary>
        public double DayTimeTurns;

        /// <summary>
        /// 'mp'
        /// </summary>
        public double MoonPhase;

        /// <summary>
        /// 'yt'
        /// </summary>
        public double yt;

        /// <summary>
        /// 'night'
        /// </summary>
        public bool IsNight;

        /// <summary>
        /// 'mc'
        /// </summary>
        public Color MoonColor;
    }
}
