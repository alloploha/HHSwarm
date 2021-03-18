using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.v17.Messages
{
    public class RMSG_FRAGMENT : RMSG, ISerializableRelayMessage
    {
        /// <summary>
        /// head
        /// 10000001 bit - final fragment
        /// 1000000x bit - next or final fragment
        /// 0xxxxxxx bit - first fragment, xxxxxxx - MSG_REL message type
        /// </summary>
        public byte Header;

        public TYPE MessageType
        {
            get
            {
                return (TYPE)(Header & 0x7F);
            }
            set
            {
                Header = (byte)((byte)value | 0x80);
            }
        }

        public bool IsFinal
        {
            get
            {
                return Header == 0x81;
            }
            set
            {
                Header = 0x81;
            }
        }

        public bool IsNext
        {
            get
            {
                return Header == 0x80;
            }
            set
            {
                Header = 0x80;
            }
        }

        public bool IsFirst
        {
            get
            {
                return (Header & 0x80) == 0x00;
            }
            set
            {
                Header &= 0x7f;
            }
        }

        /// <summary>
        /// Fragment of MSG_REL
        /// </summary>
        public byte[] Data;

        public void CallSerializer(IRelayMessageSerializer serializer, BinaryWriter writer)
        {
            serializer.Serialize(this, writer);
        }

        public delegate void Callback(RMSG_FRAGMENT message);
    }
}
