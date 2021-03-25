using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    /// <summary>
    /// '@RName("charlist")'
    /// </summary>
    class CharactersListWidgetCreateArguments
    {
        /// <summary>
        /// 'height'
        /// </summary>
        public int Height;

        public string BackgroundImageResourceName;
        public string ScrollUpButtonUpImageResourceName;
        public string ScrollUpButtonDownImageResourceName;
        public string ScrollUpButtonHoverImageResourceName;
        public string ScrollDownButtonUpImageResourceName;
        public string ScrollDownButtonDownImageResourceName;
        public string ScrollDownButtonHoverImageResourceName;
    }
}
