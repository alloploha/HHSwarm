using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.v17
{
    interface ISerializableAuthenticationMessage
    {
        void CallSerializer(IAuthenticationMessageSerializer serializer, BinaryWriter writer);
    }
}
