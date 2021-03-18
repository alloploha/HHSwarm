using HHSwarm.Native.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.WorldModel
{
    public class Light
    {
        /// <summary>
        /// 'lightamb'
        /// </summary>
        public Color Ambient;

        /// <summary>
        /// 'lightdif'
        /// </summary>
        public Color Diffuse;

        /// <summary>
        /// 'lightspc'
        /// </summary>
        public Color Specular;

        /// <summary>
        /// 'lightang'
        /// </summary>
        public double Angle;

        /// <summary>
        /// 'lightelev'
        /// </summary>
        public double Elevation;
    }
}
