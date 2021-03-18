using HHSwarm.Native.Protocols.v17.WidgetMessageArguments;
using HHSwarm.Native.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.WorldModel
{
    public class OD_CMPEQU : IGameObjectDatum
    {
        /// <summary>
        /// 'equ'
        /// </summary>
        public List<CompositedDesc.ED> Equ = new List<CompositedDesc.ED>();
    }
}
