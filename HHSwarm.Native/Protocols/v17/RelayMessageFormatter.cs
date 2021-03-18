using HHSwarm.Native.Protocols.v17.Messages;
using HHSwarm.Native.Shared;
using HHSwarm.Native.WorldModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.v17
{
    public class RelayMessageFormatter : MessageFormatter<object>, IRelayMessageSerializer
    {
        private TraceSource Trace = new TraceSource("HHSwarm.Relay");
        private GlobalObjectsFormatter GlobalObjectsFormatter = new GlobalObjectsFormatter();

        public RelayMessageFormatter() : base(null)
        {
        }

        protected override void CallSerializerFromMessage(object message, BinaryWriter writer)
        {
            (message as ISerializableRelayMessage)?.CallSerializer(this, writer);
        }

        #region RMSG_NEWWDG

        /// <summary>
        /// <code>
        /// int id = msg.uint16();
        /// String type = msg.string ();
        /// int parent = msg.uint16();
        /// Object[] pargs = msg.list();
        /// Object[] cargs = msg.list();
        /// ui.newwidget(id, type, parent, pargs, cargs);
        /// </code>
        /// </summary>

        public void Serialize(RMSG_NEWWDG message, BinaryWriter writer)
        {
            writer.Write((byte)RMSG.TYPE.RMSG_NEWWDG);
            writer.Write(message.WidgetID);
            writer.Write(message.Type);
            writer.Write(message.ParentID);
            writer.Write(message.AddChildArguments);
            writer.Write(message.CreateArguments);
        }

        public void Deserialize(BinaryReader reader, out RMSG_NEWWDG message)
        {
            long position_before = reader.BaseStream.Position;

            message = new RMSG_NEWWDG()
            {
                WidgetID = reader.ReadUInt16(),
                Type = reader.ReadString(),
                ParentID = reader.ReadUInt16(),
                AddChildArguments = reader.ReadList().ToArray(),
                CreateArguments = reader.ReadList().ToArray()
            };

            if (message.Type == "item")
            {
                long position_after = reader.BaseStream.Position;
                int length = (int)reader.BaseStream.Length;
                reader.BaseStream.Position = 0;
                byte[] data = reader.ReadBytes((int)reader.BaseStream.Length);
                File.WriteAllBytes(@"C:\Temp\HHSwarm-ExtractedResources\" + nameof(RMSG_NEWWDG) + @"\" + DateTime.Now.Ticks + String.Format(@"-pb{0:X}-", position_before) + ".bin", data);
                reader.BaseStream.Position = position_after;
            }
        }
        #endregion

        #region RMSG_WDGMSG
        public void Serialize(RMSG_WDGMSG message, BinaryWriter writer)
        {
            writer.Write((byte)RMSG.TYPE.RMSG_WDGMSG);
            writer.Write(message.WidgetID);
            writer.Write(message.MessageName);
            writer.Write(message.MessageArguments);
        }

        public void Deserialize(BinaryReader reader, out RMSG_WDGMSG message)
        {
            message = new RMSG_WDGMSG()
            {
                WidgetID = reader.ReadUInt16(),
                MessageName = reader.ReadString(),
                MessageArguments = reader.ReadList().ToArray()
            };
        }
        #endregion

        #region RMSG_DSTWDG
        public void Serialize(RMSG_DSTWDG message, BinaryWriter writer)
        {
            writer.Write((byte)RMSG.TYPE.RMSG_DSTWDG);
            writer.Write(message.WidgetID);
        }

        public void Deserialize(BinaryReader reader, out RMSG_DSTWDG message)
        {
            message = new RMSG_DSTWDG()
            {
                WidgetID = reader.ReadUInt16()
            };
        }
        #endregion

        #region RMSG_ADDWDG
        public void Serialize(RMSG_ADDWDG message, BinaryWriter writer)
        {
            writer.Write((byte)RMSG.TYPE.RMSG_ADDWDG);
            writer.Write(message.WidgetID);
            writer.Write(message.ParentID);
            writer.Write(message.AddChildArguments);
        }

        public void Deserialize(BinaryReader reader, out RMSG_ADDWDG message)
        {
            message = new RMSG_ADDWDG()
            {
                WidgetID = reader.ReadUInt16(),
                ParentID = reader.ReadUInt16(),
                AddChildArguments = reader.ReadList().ToArray()
            };
        }
        #endregion

        #region RMSG_MAPIV
        public void Serialize(RMSG_MAPIV message, BinaryWriter writer)
        {
            writer.Write((byte)RMSG.TYPE.RMSG_MAPIV);
            throw new NotImplementedException();
        }

        public void Deserialize(BinaryReader reader, out RMSG_MAPIV message)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region RMSG_GLOBLOB
        public void Serialize(RMSG_GLOBLOB message, BinaryWriter writer)
        {
            writer.Write((byte)RMSG.TYPE.RMSG_GLOBLOB);
            throw new NotImplementedException();
        }

        public void Deserialize(BinaryReader reader, out RMSG_GLOBLOB message)
        {
            message = new RMSG_GLOBLOB()
            {
                Objects = new GlobalObjects()
            };

            GlobalObjectsFormatter.Deserialize(reader, message.Objects);
        }
        #endregion

        #region RMSG_RESID
        public void Serialize(RMSG_RESID message, BinaryWriter writer)
        {
            writer.Write((byte)RMSG.TYPE.RMSG_RESID);
            writer.Write(message.ResourceID);
            writer.Write(message.ResourceName);
            writer.Write(message.ResourceVersion);
        }

        public void Deserialize(BinaryReader reader, out RMSG_RESID message)
        {
            message = new RMSG_RESID()
            {
                ResourceID = reader.ReadUInt16(),
                ResourceName = reader.ReadString(),
                ResourceVersion = reader.ReadUInt16()
            };
        }
        #endregion

        #region RMSG_PARTY
        public void Serialize(RMSG_PARTY message, BinaryWriter writer)
        {
            writer.Write((byte)RMSG.TYPE.RMSG_PARTY);
            throw new NotImplementedException();
        }

        public void Deserialize(BinaryReader reader, out RMSG_PARTY message)
        {
            message = new RMSG_PARTY();

            Dictionary<int, RMSG_PARTY.Member> list = new Dictionary<int, RMSG_PARTY.Member>();

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                RMSG_PARTY.TYPE type = (RMSG_PARTY.TYPE)reader.ReadByte();

                switch (type)
                {
                    case RMSG_PARTY.TYPE.PD_LIST:
                        {
                            for (int id = reader.ReadInt32(); id >= 0; id = reader.ReadInt32())
                            {
                                if (!list.ContainsKey(id))
                                    list[id] = new RMSG_PARTY.Member() { GameObjectID = id };
                            }
                        }
                        break;
                    case RMSG_PARTY.TYPE.PD_LEADER:
                        {
                            message.LeaderGameObjectID = reader.ReadInt32();
                        }
                        break;
                    case RMSG_PARTY.TYPE.PD_MEMBER:
                        {
                            int id = reader.ReadInt32();

                            RMSG_PARTY.Member member = list[id];

                            if (member == null)
                            {
                                member = new RMSG_PARTY.Member()
                                {
                                    GameObjectID = id
                                };

                                list[id] = member;
                            }

                            member.Visible = reader.ReadByte() == 1; // vis

                            if (member.Visible)
                                member.Coordinates = reader.ReadCoord2i();

                            member.Color = reader.ReadColor();
                        }


                        break;
                    default:
                        throw new NotImplementedException($"Unexpected {nameof(RMSG_PARTY)} type {type}!");
                }
            }

            message.Members.AddRange(list.Values);
        }
        #endregion

        #region RMSG_SFX
        public void Serialize(RMSG_SFX message, BinaryWriter writer)
        {
            writer.Write((byte)RMSG.TYPE.RMSG_SFX);
            writer.Write(message.ResourceID);
            writer.Write(message.Volume);
            writer.Write(message.Speed);
        }

        public void Deserialize(BinaryReader reader, out RMSG_SFX message)
        {
            message = new RMSG_SFX()
            {
                ResourceID = reader.ReadUInt16(),
                Volume = reader.ReadUInt16(),
                Speed = reader.ReadUInt16()
            };
        }
        #endregion

        #region RMSG_CATTR
        public void Serialize(RMSG_CATTR message, BinaryWriter writer)
        {
            writer.Write((byte)RMSG.TYPE.RMSG_CATTR);

            foreach (var a in message.Attributes)
            {
                writer.Write(a.Name);
                writer.Write(a.Base);
                writer.Write(a.Comp);
            }
        }

        public void Deserialize(BinaryReader reader, out RMSG_CATTR message)
        {
            message = new RMSG_CATTR();

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                RMSG_CATTR.AttributeInfo a = new RMSG_CATTR.AttributeInfo()
                {
                    Name = reader.ReadString(),
                    Base = reader.ReadInt32(),
                    Comp = reader.ReadInt32()
                };

                message.Attributes.Add(a);
            }
        }
        #endregion

        #region RMSG_MUSIC
        public void Serialize(RMSG_MUSIC message, BinaryWriter writer)
        {
            writer.Write((byte)RMSG.TYPE.RMSG_MUSIC);
            throw new NotImplementedException();
        }

        public void Deserialize(BinaryReader reader, out RMSG_MUSIC message)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region RMSG_TILES
        public void Serialize(RMSG_TILES message, BinaryWriter writer)
        {
            writer.Write((byte)RMSG.TYPE.RMSG_TILES);
            throw new NotImplementedException();
        }

        public void Deserialize(BinaryReader reader, out RMSG_TILES message)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region RMSG_SESSKEY
        public void Serialize(RMSG_SESSKEY message, BinaryWriter writer)
        {
            writer.Write((byte)RMSG.TYPE.RMSG_SESSKEY);
            writer.Write(message.SessionKey);
        }

        public void Deserialize(BinaryReader reader, out RMSG_SESSKEY message)
        {
            message = new RMSG_SESSKEY()
            {
                SessionKey = reader.ReadBytes((int)reader.BaseStream.Length)
            };
        }
        #endregion

        #region RMSG_FRAGMENT
        public void Serialize(RMSG_FRAGMENT message, BinaryWriter writer)
        {
            writer.Write((byte)RMSG.TYPE.RMSG_FRAGMENT);
            throw new NotImplementedException();
        }

        public void Deserialize(BinaryReader reader, out RMSG_FRAGMENT message)
        {
            throw new NotImplementedException();
        }
        #endregion

        public async Task DeserializeAsync(BinaryReader reader, IRelayMessagesReceiverAsync receiver)
        {
            RMSG.TYPE type = (RMSG.TYPE)reader.ReadByte();

            switch (type)
            {
                case RMSG.TYPE.RMSG_NEWWDG:
                    {
                        RMSG_NEWWDG message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case RMSG.TYPE.RMSG_WDGMSG:
                    {
                        RMSG_WDGMSG message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case RMSG.TYPE.RMSG_DSTWDG:
                    {
                        RMSG_DSTWDG message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case RMSG.TYPE.RMSG_MAPIV:
                    {
                        RMSG_MAPIV message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case RMSG.TYPE.RMSG_GLOBLOB:
                    {
                        RMSG_GLOBLOB message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case RMSG.TYPE.RMSG_RESID:
                    {
                        RMSG_RESID message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case RMSG.TYPE.RMSG_PARTY:
                    {
                        RMSG_PARTY message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case RMSG.TYPE.RMSG_SFX:
                    {
                        RMSG_SFX message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case RMSG.TYPE.RMSG_CATTR:
                    {
                        RMSG_CATTR message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case RMSG.TYPE.RMSG_MUSIC:
                    {
                        RMSG_MUSIC message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case RMSG.TYPE.RMSG_TILES:
                    {
                        RMSG_TILES message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case RMSG.TYPE.RMSG_SESSKEY:
                    {
                        RMSG_SESSKEY message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case RMSG.TYPE.RMSG_FRAGMENT:
                    {
                        RMSG_FRAGMENT message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case RMSG.TYPE.RMSG_ADDWDG:
                    {
                        RMSG_ADDWDG message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                default:
                    throw new NotImplementedException($"Relay Message Type {type}!");
            }
        }
    }
}
