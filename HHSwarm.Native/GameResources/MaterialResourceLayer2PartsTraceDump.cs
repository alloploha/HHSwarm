using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    class MaterialResourceLayer2PartsTraceDump : IMaterialResourceLayer2PartsReceiver
    {
        private TraceSource Trace = new TraceSource("HHSwarm.Resources");

        private IMaterialResourceLayer2PartsReceiver Receiver;
        private string Message;

        public MaterialResourceLayer2PartsTraceDump(string message, IMaterialResourceLayer2PartsReceiver receiver)
        {
            this.Message = message;
            this.Receiver = receiver;
        }

        public void Receive(MaterialResourceLayer2.ColorsPart part)
        {
            Trace.Dump(TraceEventType.Verbose, Message, part);
            Receiver.Receive(part);
        }

        public void Receive(MaterialResourceLayer2.TexPalPart part)
        {
            Trace.Dump(TraceEventType.Verbose, Message, part);
            Receiver.Receive(part);
        }

        public void Receive(MaterialResourceLayer2.LightPart part)
        {
            Trace.Dump(TraceEventType.Verbose, Message, part);
            Receiver.Receive(part);
        }

        public void Receive(MaterialResourceLayer2.OrderPart part)
        {
            Trace.Dump(TraceEventType.Verbose, Message, part);
            Receiver.Receive(part);
        }

        public void Receive(MaterialResourceLayer2.TexPart part)
        {
            Trace.Dump(TraceEventType.Verbose, Message, part);
            Receiver.Receive(part);
        }

        public void Receive(MaterialResourceLayer2.CelShadePart part)
        {
            Trace.Dump(TraceEventType.Verbose, Message, part);
            Receiver.Receive(part);
        }

        public void Receive(MaterialResourceLayer2.MaterialLink part)
        {
            Trace.Dump(TraceEventType.Verbose, Message, part);
            Receiver.Receive(part);
        }

        public void Receive(MaterialResourceLayer2.OverTex part)
        {
            Trace.Dump(TraceEventType.Verbose, Message, part);
            Receiver.Receive(part);
        }

        public void Receive(MaterialResourceLayer2.ColorState part)
        {
            Trace.Dump(TraceEventType.Verbose, Message, part);
            Receiver.Receive(part);
        }

        public void Receive(MaterialResourceLayer2.TexAnim part)
        {
            Trace.Dump(TraceEventType.Verbose, Message, part);
            Receiver.Receive(part);
        }
    }
}
