using HHSwarm.Native.Common;
using HHSwarm.Native.Protocols.v17.Messages;
using HHSwarm.Native.Protocols.v17.WidgetMessageArguments;
using HHSwarm.Native.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.WorldModel
{
    /// <summary>
    /// 'OCache'
    /// </summary>
    class GameObjectDataFormatter
    {
        public void Deserialize(BinaryReader reader, IGameObjectDataReceiver receiver)
        {
            bool stop = false;

            while (!stop && reader.BaseStream.Position < reader.BaseStream.Length)
            {
                MSG_OBJDATA.TYPE type = (MSG_OBJDATA.TYPE)reader.ReadByte();
                stop = type == MSG_OBJDATA.TYPE.OD_END;

                if (!stop)
                {
                    Deserialize(reader, type, receiver);
                }
            }
        }

        protected virtual void Deserialize(BinaryReader reader, MSG_OBJDATA.TYPE objectDataType, IGameObjectDataReceiver receiver)
        {
            switch (objectDataType)
            {
                case MSG_OBJDATA.TYPE.OD_REM:
                    {
                        OD_REM objectData;
                        Deserialize(reader, out objectData);
                        receiver.Receive(objectData);
                    }
                    break;
                case MSG_OBJDATA.TYPE.OD_MOVE:
                    {
                        OD_MOVE objectData;
                        Deserialize(reader, out objectData);
                        receiver.Receive(objectData);
                    }
                    break;
                case MSG_OBJDATA.TYPE.OD_RES:
                    {
                        OD_RES objectData;
                        Deserialize(reader, out objectData);
                        receiver.Receive(objectData);
                    }
                    break;
                case MSG_OBJDATA.TYPE.OD_LINBEG:
                    {
                        OD_LINBEG objectData;
                        Deserialize(reader, out objectData);
                        receiver.Receive(objectData);
                    }
                    break;
                case MSG_OBJDATA.TYPE.OD_LINSTEP:
                    {
                        OD_LINSTEP objectData;
                        Deserialize(reader, out objectData);
                        receiver.Receive(objectData);
                    }
                    break;
                case MSG_OBJDATA.TYPE.OD_SPEECH:
                    {
                        OD_SPEECH objectData;
                        Deserialize(reader, out objectData);
                        receiver.Receive(objectData);
                    }
                    break;
                case MSG_OBJDATA.TYPE.OD_COMPOSE:
                    {
                        OD_COMPOSE objectData;
                        Deserialize(reader, out objectData);
                        receiver.Receive(objectData);
                    }
                    break;
                case MSG_OBJDATA.TYPE.OD_ZOFF:
                    {
                        OD_ZOFF objectData;
                        Deserialize(reader, out objectData);
                        receiver.Receive(objectData);
                    }
                    break;
                case MSG_OBJDATA.TYPE.OD_LUMIN:
                    {
                        OD_LUMIN objectData;
                        Deserialize(reader, out objectData);
                        receiver.Receive(objectData);
                    }
                    break;
                case MSG_OBJDATA.TYPE.OD_AVATAR:
                    {
                        OD_AVATAR objectData;
                        Deserialize(reader, out objectData);
                        receiver.Receive(objectData);
                    }
                    break;
                case MSG_OBJDATA.TYPE.OD_FOLLOW:
                    {
                        OD_FOLLOW objectData;
                        Deserialize(reader, out objectData);
                        receiver.Receive(objectData);
                    }
                    break;
                case MSG_OBJDATA.TYPE.OD_HOMING:
                    {
                        OD_HOMING objectData;
                        Deserialize(reader, out objectData);
                        receiver.Receive(objectData);
                    }
                    break;
                case MSG_OBJDATA.TYPE.OD_OVERLAY:
                    {
                        OD_OVERLAY objectData;
                        Deserialize(reader, out objectData);
                        receiver.Receive(objectData);
                    }
                    break;
                case MSG_OBJDATA.TYPE.OD_HEALTH:
                    {
                        OD_HEALTH objectData;
                        Deserialize(reader, out objectData);
                        receiver.Receive(objectData);
                    }
                    break;
                case MSG_OBJDATA.TYPE.OD_BUDDY:
                    {
                        OD_BUDDY objectData;
                        Deserialize(reader, out objectData);
                        receiver.Receive(objectData);
                    }
                    break;
                case MSG_OBJDATA.TYPE.OD_CMPPOSE:
                    {
                        OD_CMPPOSE objectData;
                        Deserialize(reader, out objectData);
                        receiver.Receive(objectData);
                    }
                    break;
                case MSG_OBJDATA.TYPE.OD_CMPMOD:
                    {
                        OD_CMPMOD objectData;
                        Deserialize(reader, out objectData);
                        receiver.Receive(objectData);
                    }
                    break;
                case MSG_OBJDATA.TYPE.OD_CMPEQU:
                    {
                        OD_CMPEQU objectData;
                        Deserialize(reader, out objectData);
                        receiver.Receive(objectData);
                    }
                    break;
                case MSG_OBJDATA.TYPE.OD_ICON:
                    {
                        OD_ICON objectData;
                        Deserialize(reader, out objectData);
                        receiver.Receive(objectData);
                    }
                    break;
                case MSG_OBJDATA.TYPE.OD_RESATTR:
                    {
                        OD_RESATTR objectData;
                        Deserialize(reader, out objectData);
                        receiver.Receive(objectData);
                    }
                    break;
                case MSG_OBJDATA.TYPE.OD_END:
                    throw new InvalidOperationException(nameof(MSG_OBJDATA.TYPE.OD_END));
                default:
                    {
#if DEBUG
                        Debugger.Break();
#endif
                        throw new NotImplementedException($"Unexpected {nameof(MSG_OBJDATA)} Object Data type {objectDataType}!");
                    }
            }
        }

        private void Deserialize(BinaryReader reader, out OD_REM result)
        {
            result = new OD_REM.CURRENT();
        }

        /// <summary>
        /// 'OCache::move(Gob g, Coord2d c, double a)'
        /// </summary>
        private void Deserialize(BinaryReader reader, out OD_MOVE result)
        {
            result = new OD_MOVE()
            {
                C = reader.ReadCoord2i(),
                A = (reader.ReadUInt16() / 65536.0) * Math.PI * 2
            };
        }

        private void Deserialize(BinaryReader reader, out OD_RES result)
        {
            result = new OD_RES(ExtractResData(reader));
        }

        private void Deserialize(BinaryReader reader, out OD_LINBEG result)
        {
            result = new OD_LINBEG()
            {
                S = reader.ReadCoord2d(),
                V = reader.ReadCoord2d()
            };
        }

        private void Deserialize(BinaryReader reader, out OD_LINSTEP result)
        {
            Debugger.Break(); // check Math.Pow(2, -10);

            int w = reader.ReadInt16();
            double t, e;

            if (w == -1)
            {
                t = e = -1;
            }
            else if ((w & 0x80000000) == 0)
            {
                t = w * Math.Pow(2, -10);
                e = -1;
            }
            else
            {
                t = (w & ~0x80000000) * Math.Pow(2, -10);
                w = reader.ReadInt32();
                e = (w < 0) ? -1 : Math.Pow(2, -10);
            }

            result = new OD_LINSTEP()
            {
                T = t,
                E = e
            };
        }

        private void Deserialize(BinaryReader reader, out OD_SPEECH result)
        {
            result = new OD_SPEECH()
            {
                Zo = reader.ReadInt16() / 100f,
                Text = reader.ReadString()
            };
        }

        private void Deserialize(BinaryReader reader, out OD_COMPOSE result)
        {
            result = new OD_COMPOSE()
            {
                ResourceID = reader.ReadUInt16()
            };
        }

        private void Deserialize(BinaryReader reader, out OD_ZOFF result)
        {
            result = new OD_ZOFF()
            {
                Off = reader.ReadInt16() / 100f
            };
        }

        private void Deserialize(BinaryReader reader, out OD_LUMIN result)
        {
            result = new OD_LUMIN()
            {
                Off = reader.ReadCoord2i(),
                Sz = reader.ReadUInt16(),
                Str = reader.ReadByte()
            };
        }

        private void Deserialize(BinaryReader reader, out OD_AVATAR result)
        {
            List<ushort> layers = new List<ushort>();

            const ushort END_MARK = 0xFFFF;

            for (ushort layer = reader.ReadUInt16(); layer != END_MARK; layer = reader.ReadUInt16())
            {
                layers.Add(layer);
            }

            result = new OD_AVATAR()
            {
                ResourceID = layers.ToArray()
            };
        }

        private void Deserialize(BinaryReader reader, out OD_FOLLOW result)
        {
            uint header = reader.ReadUInt32();

            if (header == 0xFFFFFFFF)
            {
                result = new OD_FOLLOW.REMOVE();
            }
            else
            {
                result = new OD_FOLLOW()
                {
                    ObjectID = header,
                    BoneOffsetResourceID = reader.ReadUInt16(),
                    BoneOffsetResourceLayerName = reader.ReadString()
                };
            }
        }

        private void Deserialize(BinaryReader reader, out OD_HOMING result)
        {
            uint header = reader.ReadUInt32();

            if (header == 0xFFFFFFFF)
            {
                result = new OD_HOMING.REMOVE();
            }
            else
            {
                result = new OD_HOMING()
                {
                    TargetCoordinates = reader.ReadCoord2i(),
                    Velocity = reader.ReadInt32() * Math.Pow(2, -10) * 11
                };
            }
        }

        private void Deserialize(BinaryReader reader, out OD_OVERLAY result)
        {
            uint header = reader.ReadUInt32();

            bool prs = (header & 0x01) == 0x01;
            uint overlay_id = header >> 1;

            result = new OD_OVERLAY()
            {
                OverlayID = overlay_id,
                Delign = prs,
                Resource = ExtractResData(reader)
            };
        }

        protected ResData ExtractResData(BinaryReader reader)
        {
            ResData result = null;

            ushort header = reader.ReadUInt16();

            bool resource_specified = header != 0xFFFF;

            if (resource_specified)
            {
                result = ExtractResData(reader, header);
            }

            return result;
        }

        private ResData ExtractResData(BinaryReader reader, ushort header)
        {
            ResData result;

            const ushort DATA_PRESENCE_FLAG = 0x8000;

            bool contains_data = (header & DATA_PRESENCE_FLAG) == DATA_PRESENCE_FLAG;

            if (contains_data)
            {
                byte data_size = reader.ReadByte();
                byte[] data = reader.ReadBytes(data_size);
                ushort resource_id = (ushort)(header ^ DATA_PRESENCE_FLAG);

                result = new ResData(resource_id, data);

            }
            else
            {
                ushort resource_id = header;

                result = new ResData(resource_id);
            }

            Debug.Assert(result != null);

            return result;
        }

        private void Deserialize(BinaryReader reader, out OD_HEALTH result)
        {
            result = new OD_HEALTH()
            {
                hp = reader.ReadByte()
            };
        }

        private void Deserialize(BinaryReader reader, out OD_BUDDY result)
        {
            string name = reader.ReadString();

            if (String.IsNullOrEmpty(name))
            {
                result = new OD_BUDDY.REMOVE();
            }
            else
            {
                result = new OD_BUDDY()
                {
                    Name = name,
                    Group = reader.ReadByte(),
                    BuddyType = reader.ReadByte()
                };
            }
        }

        private void Deserialize(BinaryReader reader, out OD_CMPPOSE result)
        {
            OD_CMPPOSE.FLAGS flags = (OD_CMPPOSE.FLAGS)reader.ReadByte(); // 'pfl'

            result = new OD_CMPPOSE()
            {
                Seq = reader.ReadByte(),
                Interp = flags.HasFlag(OD_CMPPOSE.FLAGS.Interp),
                Ttime = 0
            };

            if (flags.HasFlag(OD_CMPPOSE.FLAGS.Poses))
            {
                result.Poses.AddRange(ExtractResDataList(reader));
            }

            if (flags.HasFlag(OD_CMPPOSE.FLAGS.Tposes))
            {
                result.Tposes.AddRange(ExtractResDataList(reader));
                result.Ttime = reader.ReadByte() / 10.0f;
            }
        }

        private List<ResData> ExtractResDataList(BinaryReader reader)
        {
            List<ResData> result = new List<ResData>();

            bool stop = false;
            do
            {
                ResData res_data = ExtractResData(reader);

                if (res_data == null)
                    stop = true;
                else
                    result.Add(res_data);
            } while (!stop);

            return result;
        }

        private void Deserialize(BinaryReader reader, out OD_CMPMOD result)
        {
            result = new OD_CMPMOD();
            int model_next_id = 0; // 'mseq'

            for (ushort mod_id = reader.ReadUInt16(); mod_id != 0xFFFF; mod_id = reader.ReadUInt16())
            {
                CompositedDesc.MD md = new CompositedDesc.MD()
                {
                    id = model_next_id++,
                    ModelResourceID = mod_id,
                    tex = ExtractResDataList(reader)
                };

                result.Mod.Add(md);
            }
        }

        private void Deserialize(BinaryReader reader, out OD_CMPEQU result)
        {
            result = new OD_CMPEQU();
            int equipment_next_id = 0; // 'eseq'

            const byte OFFSET_PRESENCE_FLAG = 0x80;

            for (byte header = reader.ReadByte(); header != 0xFF; header = reader.ReadByte())
            {
                bool nonzero_offset = (header & OFFSET_PRESENCE_FLAG) == OFFSET_PRESENCE_FLAG; // 'ef'

                byte et = (byte)(header ^ OFFSET_PRESENCE_FLAG);
                string at = reader.ReadString();
                ResData res = ExtractResData(reader);
                Coord3f off;

                if (nonzero_offset)
                {
                    off = new Coord3f
                    (
                        x: reader.ReadInt16() / 1000f,
                        y: reader.ReadInt16() / 1000f,
                        z: reader.ReadInt16() / 1000f
                    );
                }
                else
                {
                    off = new Coord3f(0, 0, 0);
                }

                CompositedDesc.ED equ = new CompositedDesc.ED()
                {
                    id = equipment_next_id++,
                    t = et,
                    at = at,
                    Resource = res,
                    Offset = off
                };

                result.Equ.Add(equ);
            }
        }

        private void Deserialize(BinaryReader reader, out OD_ICON result)
        {
            ushort resource_id = reader.ReadUInt16(); // 'resid'

            if (resource_id != 0xFFFF)
            {
                byte flags = reader.ReadByte(); // 'ifl' - reserved

                result = new OD_ICON()
                {
                    ResourceID = resource_id
                };
            }
            else
            {
                result = new OD_ICON.REMOVE();
            }
        }

        private void Deserialize(BinaryReader reader, out OD_RESATTR result)
        {
            ushort resource_id = reader.ReadUInt16(); // 'resid'
            byte data_size = reader.ReadByte(); // 'len'

            if (data_size > 0)
            {
                byte[] data = reader.ReadBytes(data_size);
                result = new OD_RESATTR
                (
                   resourceId: resource_id,
                   resourceData: data
                );
            }
            else
            {
                result = new OD_RESATTR.REMOVE();
            }
        }
    }
}
