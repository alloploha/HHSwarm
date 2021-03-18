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
    /// 'Glob', 'globlob'
    /// </summary>
    class GlobalObjectsFormatter
    {
        public void Deserialize(BinaryReader reader, IGlobalObjectsReceiver receiver)
        {
            bool inc = reader.ReadByte() != 0; // 'inc'
            receiver.ReceiveIncrementFlag(inc);

            while(reader.BaseStream.Position < reader.BaseStream.Length)
            {
                string type = reader.ReadString(); // 't'

                if (String.IsNullOrEmpty(type) && reader.BaseStream.Position == reader.BaseStream.Length) break;

                IEnumerable<object> list = reader.ReadList();
                ArgumentsReader list_reader = new ArgumentsReader(list.ToArray());

                switch (type)
                {
                    case "tm":
                        {
                            WorldTime globlob;
                            Deserialize(list_reader, out globlob);
                            receiver.Receive(globlob);
                        }
                        break;
                    case "astro":
                        {
                            Astronomy globlob;
                            Deserialize(list_reader, out globlob);
                            receiver.Receive(globlob);
                        }
                        break;
                    case "light":
                        {
                            Light globlob;
                            Deserialize(list_reader, out globlob);
                            receiver.Receive(globlob);
                        }
                        break;
                    case "sky":
                        {
                            Sky globlob;
                            Deserialize(list_reader, out globlob);
                            receiver.Receive(globlob);
                        }
                        break;
                    case "wth":
                        {
                            Weather globlob;
                            Deserialize(list_reader, out globlob);
                            receiver.Receive(globlob);
                        }
                        break;
                    default:
                        throw new NotImplementedException($"Unexpected {nameof(GlobalObjectsFormatter)} type name '{type}'!");
                }
            }
        }

        /// <summary>
        /// 'static final long rtimeoff = System.nanoTime()'
        /// </summary>
        private static long rtimeoff = Stopwatch.GetTimestamp();

        private void Deserialize(ArgumentsReader reader, out WorldTime result)
        {
            result = new WorldTime()
            {
                Time = reader.ReadDouble(),
                Epoch = (Stopwatch.GetTimestamp() - rtimeoff) * 1000 / Stopwatch.Frequency
            };
        }

        private void Deserialize(ArgumentsReader reader, out Astronomy result)
        {
            result = new Astronomy()
            {
                DayTimeTurns = reader.ReadDouble(),
                MoonPhase = reader.ReadDouble(),
                yt = reader.ReadDouble(),
                IsNight = reader.ReadInt32() != 0, 
                MoonColor = reader.ReadColor()
            };
        }

        private void Deserialize(ArgumentsReader reader, out Light result)
        {
            result = new Light()
            {
                Ambient = reader.ReadColor(),
                Diffuse = reader.ReadColor(),
                Specular = reader.ReadColor(),
                Angle = reader.ReadDouble(),
                Elevation = reader.ReadDouble()
            };
        }

        private void Deserialize(ArgumentsReader reader, out Sky result)
        {
            result = new Sky();

            if(reader.Length >= 1)
            {
                // TODO: check resource id type for both

                result.ResourceID1 = reader.ReadUInt16();

                if(reader.Length >= 2)
                {
                    result.ResourceID2 = reader.ReadUInt16();
                    result.Blend = reader.ReadDouble();
                }
            }
        }

        private void Deserialize(ArgumentsReader reader, out Weather result)
        {
            result = new Weather();

            // TODO: check resource id type

            while (!reader.ReachedEnd)
            {
                result.Map.Add(new Weather.WeatherMap()
                {
                    ResourceID = reader.ReadUInt16(),
                    Arguments = reader.ReadObjects()
                });
            }
        }
    }
}
