using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.WorldModel
{
    public class Weather
    {
        public class WeatherMap
        {
            /// <summary>
            /// 'res'
            /// </summary>
            public ushort ResourceID;

            /// <summary>
            /// 'args'
            /// </summary>
            public object[] Arguments;
        }

        /// <summary>
        /// 'wmap'
        /// </summary>
        public List<WeatherMap> Map = new List<WeatherMap>();
    }
}
