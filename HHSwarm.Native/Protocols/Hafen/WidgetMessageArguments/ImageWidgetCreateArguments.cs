using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    /// <summary>
    /// '@RName("img")'
    /// </summary>
    class ImageWidgetCreateArguments
    {
        /// <summary>
        /// 'hit'
        /// </summary>
        public bool AcceptsUserInput;

        public string ImageResourceName;
        public ushort? ImageResourceVersion;
        public ushort? ImageResourceID;
    }
}
