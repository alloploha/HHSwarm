using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <summary>
    /// @LayerName("codeentry")
    /// </summary>
    [Serializable]
    public class JavaClassEntryResourceLayer : ResourceLayer
    {
        public enum TYPE : byte
        {
            ByClassFactoryName = 1,
            ClassNameAndVersion = 2
        }

        [Serializable]
        public class ClassFactory
        {
            /// <summary>
            /// <code>@Resource.PublishedCode(name = "mat")</code>
            /// </summary>
            public string BaseClassFactoryPublishedName;
            public string ClassFactoryName;
        }

        [Serializable]
        public class LoadName
        {
            public string Name;
            public ushort Version;
        }

        public readonly IList<ClassFactory> PublishedEntries = new List<ClassFactory>();

        public readonly IList<LoadName> ToLoad = new List<LoadName>();
    }
}
