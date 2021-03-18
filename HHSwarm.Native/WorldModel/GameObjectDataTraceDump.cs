using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.WorldModel
{
    class GameObjectDataTraceDump : IGameObjectDataReceiver
    {
        private TraceSource Trace = new TraceSource("HHSwarm.GameObjects");

        private IGameObjectDataReceiver Receiver;
        private string Message;

        public GameObjectDataTraceDump(string message, IGameObjectDataReceiver receiver)
        {
            this.Message = message;
            this.Receiver = receiver;
        }

        public void Receive(OD_REM objectData)
        {
            Trace.Dump(TraceEventType.Information, Message, objectData);
            Receiver.Receive(objectData);
        }

        public void Receive(OD_MOVE objectData)
        {
            Trace.Dump(TraceEventType.Information, Message, objectData);
            Receiver.Receive(objectData);
        }

        public void Receive(OD_RES objectData)
        {
            Trace.Dump(TraceEventType.Information, Message, objectData);
            Receiver.Receive(objectData);
        }

        public void Receive(OD_LINBEG objectData)
        {
            Trace.Dump(TraceEventType.Information, Message, objectData);
            Receiver.Receive(objectData);
        }

        public void Receive(OD_LINSTEP objectData)
        {
            Trace.Dump(TraceEventType.Information, Message, objectData);
            Receiver.Receive(objectData);
        }

        public void Receive(OD_SPEECH objectData)
        {
            Trace.Dump(TraceEventType.Information, Message, objectData);
            Receiver.Receive(objectData);
        }

        public void Receive(OD_COMPOSE objectData)
        {
            Trace.Dump(TraceEventType.Information, Message, objectData);
            Receiver.Receive(objectData);
        }

        public void Receive(OD_ZOFF objectData)
        {
            Trace.Dump(TraceEventType.Information, Message, objectData);
            Receiver.Receive(objectData);
        }

        public void Receive(OD_LUMIN objectData)
        {
            Trace.Dump(TraceEventType.Information, Message, objectData);
            Receiver.Receive(objectData);
        }

        public void Receive(OD_AVATAR objectData)
        {
            Trace.Dump(TraceEventType.Information, Message, objectData);
            Receiver.Receive(objectData);
        }

        public void Receive(OD_FOLLOW objectData)
        {
            Trace.Dump(TraceEventType.Information, Message, objectData);
            Receiver.Receive(objectData);
        }

        public void Receive(OD_HOMING objectData)
        {
            Trace.Dump(TraceEventType.Information, Message, objectData);
            Receiver.Receive(objectData);
        }

        public void Receive(OD_OVERLAY objectData)
        {
            Trace.Dump(TraceEventType.Information, Message, objectData);
            Receiver.Receive(objectData);
        }

        public void Receive(OD_HEALTH objectData)
        {
            Trace.Dump(TraceEventType.Information, Message, objectData);
            Receiver.Receive(objectData);
        }

        public void Receive(OD_BUDDY objectData)
        {
            Trace.Dump(TraceEventType.Information, Message, objectData);
            Receiver.Receive(objectData);
        }

        public void Receive(OD_CMPPOSE objectData)
        {
            Trace.Dump(TraceEventType.Information, Message, objectData);
            Receiver.Receive(objectData);
        }

        public void Receive(OD_CMPMOD objectData)
        {
            Trace.Dump(TraceEventType.Information, Message, objectData);
            Receiver.Receive(objectData);
        }

        public void Receive(OD_CMPEQU objectData)
        {
            Trace.Dump(TraceEventType.Information, Message, objectData);
            Receiver.Receive(objectData);
        }

        public void Receive(OD_ICON objectData)
        {
            Trace.Dump(TraceEventType.Information, Message, objectData);
            Receiver.Receive(objectData);
        }

        public void Receive(OD_RESATTR objectData)
        {
            Trace.Dump(TraceEventType.Information, Message, objectData);
            Receiver.Receive(objectData);
        }
    }
}
