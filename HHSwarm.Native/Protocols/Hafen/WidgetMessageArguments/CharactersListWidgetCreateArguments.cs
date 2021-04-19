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
    class CharactersListWidgetCreateArguments : ResourceNamesWidgetCreateArguments<CharactersListWidgetCreateArguments.RESOURCE_NAME>
    {
        /// <summary>
        /// 'height'
        /// </summary>
        public int Height;

        public enum RESOURCE_NAME
        {
            BackgroundImage,
            ScrollUpButtonUpImage,
            ScrollUpButtonDownImage,
            ScrollUpButtonHoverImage,
            ScrollDownButtonUpImage,
            ScrollDownButtonDownImage,
            ScrollDownButtonHoverImage
        }
    }
}
