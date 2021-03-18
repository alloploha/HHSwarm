using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.WorldModel
{
    /// <summary>
    /// 'OCache'
    /// </summary>
    public class GameObjectData : IGameObjectDataReceiver
    {
        private List<IGameObjectDatum> Data = new List<IGameObjectDatum>();

        public void Receive(OD_LINBEG objectData)
        {
            Data.Add(objectData);
        }

        public void Receive(OD_SPEECH objectData)
        {
            Data.Add(objectData);
        }

        public void Receive(OD_ZOFF objectData)
        {
            Data.Add(objectData);
        }

        public void Receive(OD_AVATAR objectData)
        {
            Data.Add(objectData);
        }

        public void Receive(OD_HOMING objectData)
        {
            Data.Add(objectData);
        }

        public void Receive(OD_HEALTH objectData)
        {
            Data.Add(objectData);
        }

        public void Receive(OD_CMPPOSE objectData)
        {
            Data.Add(objectData);
        }

        public void Receive(OD_CMPMOD objectData)
        {
            Data.Add(objectData);
        }

        public void Receive(OD_BUDDY objectData)
        {
            Data.Add(objectData);
        }

        public void Receive(OD_OVERLAY objectData)
        {
            Data.Add(objectData);
        }

        public void Receive(OD_FOLLOW objectData)
        {
            Data.Add(objectData);
        }

        public void Receive(OD_LUMIN objectData)
        {
            Data.Add(objectData);
        }

        public void Receive(OD_COMPOSE objectData)
        {
            Data.Add(objectData);
        }

        public void Receive(OD_LINSTEP objectData)
        {
            Data.Add(objectData);
        }

        public void Receive(OD_RES objectData)
        {
            Data.Add(objectData);
        }

        public void Receive(OD_MOVE objectData)
        {
            Data.Add(objectData);
        }

        public void Receive(OD_REM objectData)
        {
            Data.Add(objectData);
        }

        public void Receive(OD_CMPEQU objectData)
        {
            Data.Add(objectData);
        }

        public void Receive(OD_ICON objectData)
        {
            Data.Add(objectData);
        }

        public void Receive(OD_RESATTR objectData)
        {
            Data.Add(objectData);
        }
    }
}
