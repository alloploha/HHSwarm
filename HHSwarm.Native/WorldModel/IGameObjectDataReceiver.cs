using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.WorldModel
{
    public interface IGameObjectDataReceiver
    {
        void Receive(OD_REM objectData);
        void Receive(OD_MOVE objectData);
        void Receive(OD_RES objectData);
        void Receive(OD_LINBEG objectData);
        void Receive(OD_LINSTEP objectData);
        void Receive(OD_SPEECH objectData);
        void Receive(OD_COMPOSE objectData);
        void Receive(OD_ZOFF objectData);
        void Receive(OD_LUMIN objectData);
        void Receive(OD_AVATAR objectData);
        void Receive(OD_FOLLOW objectData);
        void Receive(OD_HOMING objectData);
        void Receive(OD_OVERLAY objectData);
        void Receive(OD_HEALTH objectData);
        void Receive(OD_BUDDY objectData);
        void Receive(OD_CMPPOSE objectData);
        void Receive(OD_CMPMOD objectData);
        void Receive(OD_CMPEQU objectData);
        void Receive(OD_ICON objectData);
        void Receive(OD_RESATTR objectData);
    }
}
