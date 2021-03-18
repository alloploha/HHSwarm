using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <summary>
    /// '@LayerName("image")'
    /// </summary>
    [Serializable]
    public class ImageResourceLayer : ResourceLayer
    {
        public byte[] Image;
        public short Z;
        public short SubZ;

        public enum FLAGS : byte
        {
            NoOff = 0x2
        }

        public FLAGS Flags;

        public short ID;
        public Point O;
    }
}
