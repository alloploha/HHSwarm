using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.WorldModel
{
    interface IGlobalObjectsReceiver
    {
        void ReceiveIncrementFlag(bool value);
        void Receive(WorldTime globlob);
        void Receive(Astronomy globlob);
        void Receive(Light globlob);
        void Receive(Sky globlob);
        void Receive(Weather globlob);
    }
}
