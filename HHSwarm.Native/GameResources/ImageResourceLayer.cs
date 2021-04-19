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
        /// <summary>
        /// <c>java: Image.img</c>
        /// </summary>
        public byte[] Image;

        /// <summary>
        /// <c>java: Image.z</c>
        /// </summary>
        public short Z;

        /// <summary>
        /// <c>java: Image.subz</c>
        /// </summary>
        public short SubZ;

        [Flags]
        public enum FLAGS : byte
        {
            /// <remarks>
            /// https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L920
            /// </remarks>
            NoOff = 0x2,

            /// <remarks>
            /// https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L925-L943
            /// </remarks>
            Meatadata = 0x4
        }

        public FLAGS Flags;

        /// <summary>
        /// <c>java: Image.id</c>
        /// </summary>
        public short ID;

        /// <summary>
        /// <c>java: Image.o</c>
        /// </summary>
        public Point O;

        /// <summary>
        /// <c>java: Image.tsz</c>
        /// </summary>
        public Point tsz;

        /// <summary>
        /// <c>java: Image.scale</c>
        /// </summary>
        public float Scale = 1;

        [Serializable]
        public class KVData
        {
            /// <example>
            /// "mm/rot", "mm/z"
            /// </example>
            public string Key;

            public byte[] Data;
        }

        /// <summary>
        /// <c>java: Image.kvdata</c>
        /// </summary>
        public KVData[] kvdata;
    }
}
