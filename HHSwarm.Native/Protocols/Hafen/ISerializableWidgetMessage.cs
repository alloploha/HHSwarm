using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen
{
    interface ISerializableWidgetMessage
    {
        void CallSerializer(IWidgetMessageSerializer serializer, BinaryWriter writer);
    }
}
