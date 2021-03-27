using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <remarks>
    /// @LayerName("image")
    /// https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L915
    /// </remarks>
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
