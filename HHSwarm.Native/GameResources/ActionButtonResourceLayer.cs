using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <remarks>
    /// @LayerName("action"), AButton
    /// https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1097
    /// </remarks>
    [Serializable]
    public class ActionButtonResourceLayer : ResourceLayer
    {
        /// <summary>
        /// <c>java: pr</c>
        /// </summary>
        public string ParentResourceName;

        /// <summary>
        /// <c>java: pver</c>
        /// </summary>
        public ushort ParentResourceVersion;

        /// <summary>
        /// <c>java: AButton.name</c>
        /// </summary>
        public string Name;

        public string PrerequisiteSkill;

        /// <summary>
        /// <c>java: AButton.hk</c>
        /// </summary>
        public char HotKey;

        /// <summary>
        /// <c>java: AButton.ad</c>
        /// </summary>
        public string[] ad;
    }
}
