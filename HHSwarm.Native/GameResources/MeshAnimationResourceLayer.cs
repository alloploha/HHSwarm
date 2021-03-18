using HHSwarm.Native.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <summary>
    /// '@Resource.LayerName("manim")'
    /// </summary>
    [Serializable]
    public class MeshAnimationResourceLayer : ResourceLayer
    {
        /// <summary>
        /// 'id'
        /// </summary>
        public short ID;

        /// <summary>
        /// 'rnd'
        /// </summary>
        public bool rnd;

        [Serializable]
        public class Frame
        {
            /// <summary>
            /// 'tm'
            /// </summary>
            public float Time;

            /// <summary>
            /// 'idx'
            /// </summary>
            public Dictionary<int, int> idx = new Dictionary<int, int>();


            [Serializable]
            public class Run
            {
                /// <summary>
                /// 'pos'
                /// </summary>
                public Coord3f pos;

                /// <summary>
                /// 'nrm'
                /// </summary>
                public Coord3f nrm;
            }

            public readonly IList<Run> Runs = new List<Run>();
        }

        public readonly IList<Frame> Frames = new List<Frame>();
    }
}
