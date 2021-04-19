using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.Messages
{
    public abstract class MSG
    {
        public enum TYPE : byte
        {
            MSG_SESS = 0,
            MSG_REL = 1,
            MSG_ACK = 2,
            MSG_BEAT = 3,
            MSG_MAPREQ = 4,
            MSG_MAPDATA = 5,
            MSG_OBJDATA = 6,
            MSG_OBJACK = 7,
            MSG_CLOSE = 8
        }

        public static readonly Dictionary<Type, TYPE> TYPES = new Dictionary<Type, TYPE>();

        static MSG()
        {
            TYPES.Add(typeof(MSG_SESS), TYPE.MSG_SESS);
            TYPES.Add(typeof(MSG_REL), TYPE.MSG_REL);
            TYPES.Add(typeof(MSG_ACK), TYPE.MSG_ACK);
            TYPES.Add(typeof(MSG_BEAT), TYPE.MSG_BEAT);
            TYPES.Add(typeof(MSG_MAPREQ), TYPE.MSG_MAPREQ);
            TYPES.Add(typeof(MSG_MAPDATA), TYPE.MSG_MAPDATA);
            TYPES.Add(typeof(MSG_OBJDATA), TYPE.MSG_OBJDATA);
            TYPES.Add(typeof(MSG_OBJACK), TYPE.MSG_OBJACK);
            TYPES.Add(typeof(MSG_CLOSE), TYPE.MSG_CLOSE);
        }
    }
}
