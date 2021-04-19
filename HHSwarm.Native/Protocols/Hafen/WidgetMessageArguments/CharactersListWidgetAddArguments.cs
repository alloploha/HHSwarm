using HHSwarm.Native.Shared;
using HHSwarm.Native.WorldModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    class CharactersListWidgetAddArguments
    {
        public string CharacterName;
        public CompositedDesc Desc;
        public Dictionary<ushort, ushort> Map;
    }
}
