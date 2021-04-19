using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    class CommandsMenuWidgetCreateArguments : ResourceNamesWidgetCreateArguments<CommandsMenuWidgetCreateArguments.RESOURCE_NAME>
    {
        public enum RESOURCE_NAME
        {
            NextButtonImage,
            BackButtonImage
        }
    }
}
