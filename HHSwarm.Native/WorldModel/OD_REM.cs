using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.WorldModel
{
    public abstract class OD_REM : IGameObjectDatum
    {
        public class PREVIOUS : OD_REM
        {
        }

        public class CURRENT : OD_REM
        {
        }
    }
}
