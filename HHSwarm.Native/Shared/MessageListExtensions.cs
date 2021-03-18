using HHSwarm.Native.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Shared
{
    public static class MessageListExtensions
    {
        enum TYPE : byte
        {
            END = 0,
            INT = 1,
            STR = 2,
            COORD = 3,
            UINT8 = 4,
            UINT16 = 5,
            COLOR = 6,
            TTOL = 8,
            INT8 = 9,
            INT16 = 10,
            NIL = 12,
            UID = 13,
            BYTES = 14,
            FLOAT32 = 15,
            FLOAT64 = 16,
            FCOORD32 = 18,
            FCOORD64 = 19
        }

        public static IEnumerable<object> ReadList(this BinaryReader reader)
        {
            List<object> list = new List<object>();

            if (reader.BaseStream.Position < reader.BaseStream.Length - sizeof(byte))
            {
                bool list_end_marker_reached = false;

                do
                {
                    TYPE type = (TYPE)reader.ReadByte();

                    switch (type)
                    {
                        case TYPE.END:
                            list_end_marker_reached = true;
                            break;
                        case TYPE.INT:
                            list.Add(reader.ReadInt32());
                            break;
                        case TYPE.STR:
                            list.Add(reader.ReadString());
                            break;
                        case TYPE.COORD:
                            list.Add(reader.ReadCoord2i());
                            break;
                        case TYPE.UINT8:
                            list.Add(reader.ReadByte());
                            break;
                        case TYPE.UINT16:
                            list.Add(reader.ReadUInt16());
                            break;
                        case TYPE.COLOR:
                            list.Add(reader.ReadColor());
                            break;
                        case TYPE.TTOL:
                            list.Add(reader.ReadList().ToArray());
                            break;
                        case TYPE.INT8:
                            list.Add(reader.ReadSByte());
                            break;
                        case TYPE.INT16:
                            list.Add(reader.ReadInt16());
                            break;
                        case TYPE.NIL:
                            list.Add(null);
                            break;
                        case TYPE.UID:
                            list.Add(reader.ReadInt64());
                            break;
                        case TYPE.BYTES:
                            {
                                int count = reader.ReadByte();
                                if ((count & 0x80) == 0x80)
                                    count = reader.ReadInt32();
                                list.Add(reader.ReadBytes(count));
                            }
                            break;
                        case TYPE.FLOAT32:
                            list.Add(reader.ReadSingle());
                            break;
                        case TYPE.FLOAT64:
                            list.Add(reader.ReadDouble());
                            break;
                        case TYPE.FCOORD32:
                            list.Add(reader.ReadCoord2f());
                            break;
                        case TYPE.FCOORD64:
                            list.Add(reader.ReadCoord2d());
                            break;
                        default:
                            throw new NotImplementedException($"List object type {type}!");
                    }
                } while (!list_end_marker_reached && reader.BaseStream.Position < reader.BaseStream.Length);
            }

            return list;
        }

        public static void Write(this BinaryWriter writer, IEnumerable<object> list)
        {
            if (list == null) list = Array.Empty<object>();

            foreach (object item in list)
            {
                if(item is int)
                {
                    int value = (int)item;
                    writer.Write((byte)TYPE.INT);
                    writer.Write(value);
                }
                else if(item is string)
                {
                    string value = (string)item;
                    writer.Write((byte)TYPE.STR);
                    writer.Write(value);
                }
                else if (item is Coord2i)
                {
                    Coord2i value = (Coord2i)item;
                    writer.Write((byte)TYPE.COORD);
                    writer.Write(value);
                }
                else if (item is byte)
                {
                    byte value = (byte)item;
                    writer.Write((byte)TYPE.UINT8);
                    writer.Write(value);
                }
                else if (item is sbyte)
                {
                    sbyte value = (sbyte)item;
                    writer.Write((byte)TYPE.INT8);
                    writer.Write(value);
                }
                else if (item is short)
                {
                    short value = (short)item;
                    writer.Write((byte)TYPE.INT16);
                    writer.Write(value);
                }
                else if (item is ushort)
                {
                    ushort value = (ushort)item;
                    writer.Write((byte)TYPE.UINT16);
                    writer.Write(value);
                }
                else if (item is long)
                {
                    long value = (long)item;
                    writer.Write((byte)TYPE.UID);
                    writer.Write(value);
                }
                else if (item is Color)
                {
                    Color value = (Color)item;
                    writer.Write((byte)TYPE.COLOR);
                    writer.Write(value);
                }
                else if (item is IEnumerable<byte>)
                {
                    IEnumerable<byte> value = (IEnumerable<byte>)item;
                    if (value == null) value = Array.Empty<byte>();
                    writer.Write((byte)TYPE.BYTES);

                    int length = value.Count();

                    if (length < 0x80)
                        writer.Write((byte)length);
                    else
                    {
                        writer.Write((byte)0x80);
                        writer.Write(length);
                    }

                    writer.Write(value.ToArray());
                }
                else if (item is IEnumerable<object>)
                {
                    IEnumerable<object> value = (IEnumerable<object>)item;
                    if (value == null) value = Array.Empty<object>();
                    writer.Write((byte)TYPE.TTOL);
                    Write(writer, value);
                }
                else if (item is float)
                {
                    float value = (float)item;
                    writer.Write((byte)TYPE.FLOAT32);
                    writer.Write(value);
                }
                else if (item is double)
                {
                    double value = (double)item;
                    writer.Write((byte)TYPE.FLOAT64);
                    writer.Write(value);
                }
                else if (item is Coord2f)
                {
                    Coord2f value = (Coord2f)item;
                    writer.Write((byte)TYPE.FCOORD32);
                    writer.Write(value);
                }
                else if (item is Coord2d)
                {
                    Coord2d value = (Coord2d)item;
                    writer.Write((byte)TYPE.FCOORD64);
                    writer.Write(value);
                }
                else if (item == null)
                {
                    writer.Write((byte)TYPE.NIL);
                }
            }

            writer.Write((byte)TYPE.END);
        }
    }
}
