using HHSwarm.Native.WorldModel;
using HHSwarm.Native.Protocols.Hafen.Messages;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen
{
    public class SessionMessageFormatter : MessageFormatter<SessionProtocol.SessionContext>, ISessionMessageSerializer
    {
        private TraceSource Trace = new TraceSource("HHSwarm.Session");
        private const int MAX_MESSAGE_SIZE = ushort.MaxValue * 8;
        private GameObjectDataFormatter ObjectsDataFormatter = new GameObjectDataFormatter();

        public SessionMessageFormatter(SessionProtocol.SessionContext messageContext) : base(messageContext)
        {
        }

        protected override void CallSerializerFromMessage(object message, BinaryWriter writer)
        {
            (message as ISerializableSessionMessage)?.CallSerializer(this, writer);
        }

        #region MSG_SESS.REQUEST
        public void Serialize(MSG_SESS.REQUEST message, BinaryWriter writer)
        {
            writer.Write((byte)MSG.TYPE.MSG_SESS);
            writer.Write(message.Reserved);
            writer.Write(message.ProtocolName);
            writer.Write(message.ProtocolVersion);
            writer.Write(message.AccountName);
            writer.Write((ushort)message.Cookie.Length);
            writer.Write(message.Cookie);
        }

        public void Deserialize(BinaryReader reader, out MSG_SESS.REQUEST message)
        {
            message = new MSG_SESS.REQUEST()
            {
                Reserved = reader.ReadUInt16(),
                ProtocolName = reader.ReadString(),
                ProtocolVersion = reader.ReadUInt16(),
                AccountName = reader.ReadString(),
                Cookie = reader.ReadBytes(reader.ReadUInt16())
            };
        }
        #endregion

        #region MSG_SESS.RESPONSE
        public void Serialize(MSG_SESS.RESPONSE message, BinaryWriter writer)
        {
            writer.Write((byte)MSG.TYPE.MSG_SESS);
            writer.Write((byte)message.ErrorCode);
        }

        public void Deserialize(BinaryReader reader, out MSG_SESS.RESPONSE message)
        {
            message = new MSG_SESS.RESPONSE()
            {
                ErrorCode = (MSG_SESS.RESPONSE.ERROR_CODE)reader.ReadByte()
            };
        }
        #endregion

        #region MSG_REL
        public void Serialize(MSG_REL message, BinaryWriter writer)
        {
            writer.Write((byte)MSG.TYPE.MSG_REL);
            writer.Write(message.SequenceNumber);
            writer.Write(message.RelayedData);
        }

        public void Deserialize(BinaryReader reader, out MSG_REL message)
        {
            message = new MSG_REL()
            {
                SequenceNumber = reader.ReadUInt16(),
                RelayedData = reader.ReadBytes(MAX_MESSAGE_SIZE)
            };
        }
        #endregion

        #region MSG_ACK
        public void Serialize(MSG_ACK message, BinaryWriter writer)
        {
            writer.Write((byte)MSG.TYPE.MSG_ACK);
            writer.Write(message.RelayMessageSequenceNumber);
        }

        public void Deserialize(BinaryReader reader, out MSG_ACK message)
        {
            message = new MSG_ACK()
            {
                RelayMessageSequenceNumber = reader.ReadUInt16()
            };
        }
        #endregion

        #region MSG_BEAT
        public void Serialize(MSG_BEAT message, BinaryWriter writer)
        {
            writer.Write((byte)MSG.TYPE.MSG_BEAT); // Send it every 10 sec.
        }

        public void Deserialize(BinaryReader reader, out MSG_BEAT message)
        {
            message = new MSG_BEAT()
            {
            };
        }
        #endregion

        #region MSG_MAPREQ
        public void Serialize(MSG_MAPREQ message, BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(BinaryReader reader, out MSG_MAPREQ message)
        {
            message = new MSG_MAPREQ()
            {
            };
        }
        #endregion

        #region MSG_MAPDATA
        public void Serialize(MSG_MAPDATA message, BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(BinaryReader reader, out MSG_MAPDATA message)
        {
            message = new MSG_MAPDATA()
            {
            };
        }
        #endregion

        #region MSG_OBJDATA
        public void Serialize(MSG_OBJDATA message, BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 'Session::getobjdata(Message msg)'
        /// </summary>
        public void Deserialize(BinaryReader reader, out MSG_OBJDATA message)
        {
            message = new MSG_OBJDATA();

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                MSG_OBJDATA.GameObjectDataFrame data_frame = new MSG_OBJDATA.GameObjectDataFrame()
                {
                    Flags = (MSG_OBJDATA.FLAGS)reader.ReadByte(),
                    ObjectDataID = reader.ReadUInt32(),
                    FrameID = reader.ReadInt32(),
                    ObjectsData = new GameObjectData()
                };

                if (data_frame.Flags.HasFlag(MSG_OBJDATA.FLAGS.RemoveFromCache))
                    data_frame.ObjectsData.Receive(new OD_REM.PREVIOUS());

                IGameObjectDataReceiver data_receiver = data_frame.ObjectsData;
#if DEBUG
                data_receiver = new GameObjectDataTraceDump($"{nameof(data_frame.ObjectDataID)}: {data_frame.ObjectDataID}, {nameof(data_frame.FrameID)}: {data_frame.FrameID}", data_receiver);
#endif
                ObjectsDataFormatter.Deserialize(reader, data_receiver);

                message.DataFrames.Add(data_frame);
            }
        }
        #endregion

        #region MSG_OBJACK
        public void Serialize(MSG_OBJACK message, BinaryWriter writer)
        {
            writer.Write((byte)MSG.TYPE.MSG_OBJACK);

            foreach (var frame in message.DataFrames)
            {
                writer.Write(frame.ObjectDataID);
                writer.Write(frame.FrameID);
            }
        }

        public void Deserialize(BinaryReader reader, out MSG_OBJACK message)
        {
            message = new MSG_OBJACK();

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var frame = new MSG_OBJACK.GameObjectDataFrame()
                {
                    ObjectDataID = reader.ReadUInt32(),
                    FrameID = reader.ReadInt32()
                };

                message.DataFrames.Add(frame);
            }
        }
        #endregion

        #region MSG_CLOSE
        public void Serialize(MSG_CLOSE message, BinaryWriter writer)
        {
            writer.Write((byte)MSG.TYPE.MSG_ACK);
        }

        public void Deserialize(BinaryReader reader, out MSG_CLOSE message)
        {
            message = new MSG_CLOSE()
            {
            };
        }
        #endregion

        public async Task DeserializeAsync(BinaryReader reader, ISessionMessagesReceiverAsync receiver)
        {
            MSG.TYPE type = (MSG.TYPE)reader.ReadByte();

            switch (type)
            {
                case MSG.TYPE.MSG_SESS:
                    switch (MessageContext)
                    {
                        case SessionProtocol.SessionContext.Client:
                            {
                                MSG_SESS.RESPONSE message;
                                Deserialize(reader, out message);
                                await receiver.ReceiveAsync(message);
                            }
                            break;
                        case SessionProtocol.SessionContext.Server:
                            {
                                MSG_SESS.REQUEST message;
                                Deserialize(reader, out message);
                                await receiver.ReceiveAsync(message);
                            }
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                    break;
                case MSG.TYPE.MSG_REL:
                    {
                        MSG_REL message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case MSG.TYPE.MSG_ACK:
                    {
                        MSG_ACK message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case MSG.TYPE.MSG_BEAT:
                    {
                        MSG_BEAT message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case MSG.TYPE.MSG_MAPREQ:
                    {
                        MSG_MAPREQ message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case MSG.TYPE.MSG_MAPDATA:
                    {
                        MSG_MAPDATA message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case MSG.TYPE.MSG_OBJDATA:
                    {
                        MSG_OBJDATA message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case MSG.TYPE.MSG_OBJACK:
                    {
                        MSG_OBJACK message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case MSG.TYPE.MSG_CLOSE:
                    {
                        MSG_CLOSE message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                default:
                    throw new NotImplementedException(type.ToString());
            }
        }

    }
}
