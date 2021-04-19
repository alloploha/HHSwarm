using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <remarks>
    /// @LayerName("anim"), class Anim
    /// </remarks>
    [Serializable]
    public class AnimationResourceLayer : ResourceLayer
    {
        // 'id'
        // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1065
        public short ID;

        // 'd'
        // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1066
        public ushort D;

        // 'ids'
        // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1067
        public short[] ImageID;
    }
}
