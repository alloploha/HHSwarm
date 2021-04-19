using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHSwarm.Model.Common;
using HHSwarm.Native.Common;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    public interface IAddChildArgumentsDeserializer
    {
        Coord2i DeserializeCharactersListWidgetPosition(RelativePositionComputer computer, object[] addChildArguments, ushort parentWidgetID, ushort widgetID);
    }
}
