using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.Messages
{
    public abstract class RMSG
    {
        public enum TYPE : byte
        {
            RMSG_NEWWDG = 0,
            RMSG_WDGMSG = 1,
            RMSG_DSTWDG = 2,
            RMSG_MAPIV = 3,
            RMSG_GLOBLOB = 4,
            [Obsolete]
            RMSG_PAGINAE = 5,
            RMSG_RESID = 6,
            RMSG_PARTY = 7,
            RMSG_SFX = 8,
            RMSG_CATTR = 9,
            RMSG_MUSIC = 10,
            RMSG_TILES = 11,
            [Obsolete]
            RMSG_BUFF = 12,
            RMSG_SESSKEY = 13,
            RMSG_FRAGMENT = 14,
            RMSG_ADDWDG = 15
        }

        public static readonly Dictionary<Type, TYPE> TYPES = new Dictionary<Type, TYPE>();

        static RMSG()
        {
            TYPES.Add(typeof(RMSG_NEWWDG), TYPE.RMSG_NEWWDG);
            TYPES.Add(typeof(RMSG_WDGMSG), TYPE.RMSG_WDGMSG);
            TYPES.Add(typeof(RMSG_DSTWDG), TYPE.RMSG_DSTWDG);
            TYPES.Add(typeof(RMSG_MAPIV), TYPE.RMSG_MAPIV);
            TYPES.Add(typeof(RMSG_RESID), TYPE.RMSG_RESID);
            TYPES.Add(typeof(RMSG_PARTY), TYPE.RMSG_PARTY);
            TYPES.Add(typeof(RMSG_SFX), TYPE.RMSG_SFX);
            TYPES.Add(typeof(RMSG_CATTR), TYPE.RMSG_CATTR);
            TYPES.Add(typeof(RMSG_MUSIC), TYPE.RMSG_MUSIC);
            TYPES.Add(typeof(RMSG_TILES), TYPE.RMSG_TILES);
            TYPES.Add(typeof(RMSG_SESSKEY), TYPE.RMSG_SESSKEY);
            TYPES.Add(typeof(RMSG_FRAGMENT), TYPE.RMSG_FRAGMENT);
            TYPES.Add(typeof(RMSG_ADDWDG), TYPE.RMSG_ADDWDG);
        }

    }
}
