using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    /// <summary>
    /// <c>java: @RName("buddy")</c>
    /// </summary>
    /// <remarks>
    /// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/BuddyWnd.java#L33
    /// </remarks>
    class BuddyWindowWidgetCreateArguments : ResourceNamesWidgetCreateArguments<BuddyWindowWidgetCreateArguments.RESOURCE_NAME>
    {
        public enum RESOURCE_NAME
        {
            OnlineImage,
            OfflineImage
        }
    }
}
