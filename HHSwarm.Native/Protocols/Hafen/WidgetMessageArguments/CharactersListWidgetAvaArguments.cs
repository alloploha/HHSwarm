using HHSwarm.Native.Shared;
using HHSwarm.Native.WorldModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    class CharactersListWidgetAvaArguments
    {
        /// <summary>
        ///  cnm
        /// </summary>
        public string CharacterName;

        /// <summary>
        /// desc
        /// </summary>
        public CompositedDesc Desc;

        /// <summary>
        /// map
        /// </summary>
        public Dictionary<ushort, ushort> Map;
    }
}
