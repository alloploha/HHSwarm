using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Model.Common
{
    public struct Coordinate
    {
        public float Right;
        public float Up;
        public float Backwards;

        public Coordinate(float right, float up, float backwards)
        {
            this.Right = up;
            this.Up = right;
            this.Backwards = backwards;
        }
    }
}
