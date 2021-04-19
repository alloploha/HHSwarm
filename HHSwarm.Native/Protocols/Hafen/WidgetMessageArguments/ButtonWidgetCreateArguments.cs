using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    /// <remarks>
    /// @RName("ibtn")
    /// https://github.com/dolda2000/hafen-client/blob/394a9d64bc732ed8c2eb6e5df1b57dd08b97c4d8/src/haven/IButton.java#L38
    /// </remarks>
    class ButtonWidgetCreateArguments : ResourceNamesWidgetCreateArguments<ButtonWidgetCreateArguments.RESOURCE_NAME>
    {
        public enum RESOURCE_NAME
        {
            UpImage,
            DownImage
        }
    }
}
