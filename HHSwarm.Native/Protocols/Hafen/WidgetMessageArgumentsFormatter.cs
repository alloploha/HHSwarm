using HHSwarm.Native.Common;
using HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments;
using HHSwarm.Native.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HHSwarm.Native.Protocols.Hafen
{
    class WidgetMessageArgumentsFormatter : IAddChildArgumentsDeserializer
    {
        public void Deserialize(ArgumentsReader reader, out CharactersListWidgetAddArguments result)
        {
            result = new CharactersListWidgetAddArguments()
            {
                CharacterName = reader.ReadString() // [0]
            };

            if (reader.Length - reader.Index >= 2)
            {
                CompositedDesc desc;
                Deserialize(reader.ReadReader(), out desc); // [1]
                result.Desc = desc;

                Dictionary<ushort, ushort> map;
                Deserialize(reader.ReadReader(), out map); // [2]
                result.Map = map;
            }
            //Debugger.Break();
        }

        public void Deserialize(ArgumentsReader reader, out CharactersListWidgetAvaArguments result)
        {
            result = new CharactersListWidgetAvaArguments()
            {
                /* cnm */
                CharacterName = reader.ReadString() // [0]
            };

            CompositedDesc desc;
            Deserialize(reader.ReadReader(), out desc); // [1]
            result.Desc = desc;

            Dictionary<ushort, ushort> map;
            Deserialize(reader.ReadReader(), out map); // [2]
            result.Map = map;

            //Debugger.Break();
        }

        public void Deserialize(ArgumentsReader reader, out CompositedDesc result)
        {
            result = new CompositedDesc()
            {
                Base = reader.ReadUInt16() // [0]
            };

            ArgumentsReader ma = reader.ReadReader(); // [1]

            while (!ma.ReachedEnd)
            {
                var md = new CompositedDesc.MD()
                {
                    ModelResourceID = ma.ReadUInt16()
                };

                ArgumentsReader ta = ma.ReadReader();

                while (!ta.ReachedEnd)
                {
                    ResData rd = new ResData
                    (
                        resourceId: ta.ReadUInt16(),
                        resourceData: ta.ReadBytes_ifType()
                    );

                    md.tex.Add(rd);
                }

                result.Mod.Add(md);
            }


            /*
            object[] ea = reader.ReadObjects(); // [2]


            for (int i = 0; i < ea.Length; i++)
            {
                CompositedDesc.ED ed = new CompositedDesc.ED()
                {
                    res = new ResData()
                };

                object[] qa = (object[])ea[i];
                ed.t = (byte)qa[0];
                ed.at = (string)qa[1];

                ed.res.trResourceID = Convert.ToUInt16(qa[2]);
                ed.res.sdt = null;
                if (qa[3] is byte[])
                    ed.res.sdt = (byte[])qa[3];

                int k = ed.res.sdt == null ? -1 : 0;

                ed.off = new Point3F(x: (float)qa[4 + k], y: (float)qa[5 + k], z: (float)qa[6 + k]);

                result.Equ.Add(ed);
            }
            */

            ArgumentsReader ea = reader.ReadReader(); // [2]

            while (!ea.ReachedEnd)
            {
                ArgumentsReader qa = ea.ReadReader();

                CompositedDesc.ED ed = new CompositedDesc.ED()
                {
                    t = qa.ReadByte(),
                    at = qa.ReadString(),
                    Resource = new ResData
                    (
                        resourceId: qa.ReadUInt16(),
                        resourceData: qa.ReadBytes_ifType()
                    ),
                    Offset = new Coord3f
                    (
                        x: qa.ReadSingle(),
                        y: qa.ReadSingle(),
                        z: qa.ReadSingle()
                    )
                };

                result.Equ.Add(ed);
            }
        }

        public void Deserialize(ArgumentsReader reader, out Dictionary<ushort, ushort> result)
        {
            result = new Dictionary<ushort, ushort>();

            while (!reader.ReachedEnd)
            {
                result.Add
                (
                    reader.ReadUInt16(),
                    reader.ReadUInt16()
                );
            }
        }

        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/394a9d64bc732ed8c2eb6e5df1b57dd08b97c4d8/src/haven/IButton.java#L41
        /// </remarks>
        public void Deserialize(ArgumentsReader reader, out ButtonWidgetCreateArguments result)
        {
            if (reader.Length != 2) throw new ArgumentException($"Number of create widget arguments for {nameof(ButtonWidgetCreateArguments)} is {reader.Length} while expected number is 2!");

            result = new ButtonWidgetCreateArguments();

            result[ButtonWidgetCreateArguments.RESOURCE_NAME.UpImage] = reader.ReadString(); // [0]
            result[ButtonWidgetCreateArguments.RESOURCE_NAME.DownImage] = reader.ReadString(); // [1]
        }

        public void Deserialize(ArgumentsReader reader, out CharactersListWidgetCreateArguments result)
        {
            if (reader.Length != 1) throw new ArgumentException($"Number of create widget arguments for {nameof(CharactersListWidgetCreateArguments)} is {reader.Length} while expected number is 1!");

            result = new CharactersListWidgetCreateArguments()
            {
                Height = reader.ReadInt32()
            };

            result[CharactersListWidgetCreateArguments.RESOURCE_NAME.BackgroundImage] = "gfx/hud/avakort";
            result[CharactersListWidgetCreateArguments.RESOURCE_NAME.ScrollUpButtonUpImage] = "gfx/hud/buttons/csa" + "u" + "u";
            result[CharactersListWidgetCreateArguments.RESOURCE_NAME.ScrollUpButtonDownImage] = "gfx/hud/buttons/csa" + "u" + "d";
            result[CharactersListWidgetCreateArguments.RESOURCE_NAME.ScrollUpButtonHoverImage] = "gfx/hud/buttons/csa" + "u" + "o";
            result[CharactersListWidgetCreateArguments.RESOURCE_NAME.ScrollDownButtonUpImage] = "gfx/hud/buttons/csa" + "d" + "u";
            result[CharactersListWidgetCreateArguments.RESOURCE_NAME.ScrollDownButtonDownImage] = "gfx/hud/buttons/csa" + "d" + "d";
            result[CharactersListWidgetCreateArguments.RESOURCE_NAME.ScrollDownButtonHoverImage] = "gfx/hud/buttons/csa" + "d" + "o";
        }

        public void Deserialize(ArgumentsReader reader, out ImageWidgetCreateArguments result)
        {
            if (!new int[] { 1, 2, 3 }.Contains(reader.Length)) throw new ArgumentException($"Number of create widget arguments for {nameof(ImageWidgetCreateArguments)} is {reader.Length} while expected number is 1, 2 or 3!");

            result = new ImageWidgetCreateArguments()
            {
                ImageResourceName = reader.ReadString_ifType() // [0]
            };

            if (!String.IsNullOrEmpty(result.ImageResourceName))
            {
                if (reader) result.ImageResourceVersion = reader.ReadUInt16(); // [1]
            }
            else
            {
                result.ImageResourceID = reader.ReadUInt16(); // [0]
            }

            if (reader) result.AcceptsUserInput = reader.ReadInt32() != 0; // [2] or [1]
        }

        public void Deserialize(ArgumentsReader reader, out CreateEmptyCenteredWidgetCreateArguments result)
        {
            if (reader.Length != 1) throw new ArgumentException($"Number of create widget arguments for {nameof(CreateEmptyCenteredWidgetCreateArguments)} is {reader.Length} while expected number is 1!");

            result = new CreateEmptyCenteredWidgetCreateArguments()
            {
                Size = reader.ReadCoord2i()
            };
        }

        public void Deserialize(ArgumentsReader reader, out GameUserInterfaceWidgetCreateArguments result)
        {
            if (!new int[] { 2, 3 }.Contains(reader.Length)) throw new ArgumentException($"Number of create widget arguments for {nameof(GameUserInterfaceWidgetCreateArguments)} is {reader.Length} while expected number is 2 or 3!");

            result = new GameUserInterfaceWidgetCreateArguments()
            {
                CharacterID = reader.ReadString(), // [0]
                plid = reader.ReadInt32(), // [1]
                genus = String.Empty
            };

            if (reader) result.genus = reader.ReadString(); // [2]
        }

        public void Deserialize(ArgumentsReader reader, out WidgetCreateAddChildArguments result)
        {
            Coord2i? ci = reader.ReadCoord2i_ifType();

            if (ci.HasValue)
            {
                result = new WidgetCreateAddChildArguments(ci.Value);
            }
            else
            {
                Coord2f? cf = reader.ReadCoord2f_ifType();

                if (cf.HasValue)
                {
                    result = new WidgetCreateAddChildArguments(cf.Value);
                }
                else
                {
                    Coord2d? cd = reader.ReadCoord2d_ifType();

                    if (cd.HasValue)
                    {
                        result = new WidgetCreateAddChildArguments(cd.Value);
                    }
                    else
                    {
                        string cs = reader.ReadString_ifType();

                        if (cs != null)
                        {
                            result = new WidgetCreateAddChildArguments
                            (
                                specification: cs,
                                arguments: reader.ReadRemainder()
                            );
                        }
                        else
                        {
                            Debugger.Break();
                            throw new NotImplementedException($"Unexpected 1st argument type for {nameof(WidgetCreateAddChildArguments)}!");
                        }
                    }
                }
            }
        }

        public void Deserialize(ArgumentsReader reader, out MapViewWidgetCreateArguments result)
        {
            result = new MapViewWidgetCreateArguments()
            {
                Size = reader.ReadCoord2i(),
                mc = reader.ReadCoord2i()
            };

            if (reader) result.pgob = reader.ReadInt32();
        }

        public void Deserialize(ArgumentsReader reader, out GitemWidgetCreateArguments result)
        {
            result = new GitemWidgetCreateArguments()
            {
                ResourceID = reader.ReadUInt16()
            };

            if (reader)
                result.Data = reader.ReadBytes();
        }

        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/974366a68c0e61ee175a678d574f863705eac352/src/haven/Equipory.java#L86-L91
        /// </remarks>
        public void Deserialize(ArgumentsReader reader, out EquiporyWidgetCreateArguments result)
        {
            result = new EquiporyWidgetCreateArguments();

            if (reader)
            {
                object value = reader.ReadObject();
                if (value != null)
                    result.ObjectID = (int)value;
            }
        }

        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/c90a0aea7a84f7ab738c1d7c40556ae50decf03f/src/haven/Inventory.java#L61
        /// </remarks>
        public void Deserialize(ArgumentsReader reader, out InventoryWidgetCreateArguments result)
        {
            result = new InventoryWidgetCreateArguments()
            {
                Size = reader.ReadCoord2i()
            };
        }

        private Coord2i CalculateRelativePosition(RelativePositionComputer computer, WidgetCreateAddChildArguments widgetCreateAddChildArguments, ushort parentWidgetID, ushort childWidgetID)
        {
            computer.Reset();
            computer.Run(widgetCreateAddChildArguments.Specification, widgetCreateAddChildArguments.Arguments, parentWidgetID, childWidgetID);
            return (Coord2i)computer.Result;
        }

        private Coord2i DeserializeWidgetRelativePosition(RelativePositionComputer computer, object[] addChildArguments, ushort parentWidgetID, ushort widgetID)
        {
            WidgetCreateAddChildArguments add_child_arguments;
            Deserialize(new ArgumentsReader(addChildArguments), out add_child_arguments);
            return CalculateRelativePosition(computer, add_child_arguments, parentWidgetID, widgetID);
        }

        public Coord2i DeserializeCharactersListWidgetPosition(RelativePositionComputer computer, object[] addChildArguments, ushort parentWidgetID, ushort widgetID)
        {
            return DeserializeWidgetRelativePosition(computer, addChildArguments, parentWidgetID, widgetID);
        }

        public void Deserialize(ArgumentsReader reader, out CharacterSheetWidgetCreateArguments result)
        {
            result = new CharacterSheetWidgetCreateArguments();

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1862
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.BaseAttributesImage] = "gfx/hud/chr/tips/base";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1865
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L434
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.BaseAttributesStrengthImage] = "gfx/hud/chr/" + "str";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1866
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L434
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.BaseAttributesAgilityImage] = "gfx/hud/chr/" + "agi";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1867
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L434
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.BaseAttributesIntelligenceImage] = "gfx/hud/chr/" + "int";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1868
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L434
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.BaseAttributesConstitutionImage] = "gfx/hud/chr/" + "con";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1869
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L434
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.BaseAttributesPerceptionImage] = "gfx/hud/chr/" + "prc";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1870
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L434
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.BaseAttributesCharismaImage] = "gfx/hud/chr/" + "csm";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1871
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L434
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.BaseAttributesDexterityImage] = "gfx/hud/chr/" + "dex";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1872
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L434
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.BaseAttributesWillImage] = "gfx/hud/chr/" + "wil";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1873
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L434
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.BaseAttributesPsycheImage] = "gfx/hud/chr/" + "psy";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2106
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2083
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.BaseAttributesButtonUpImage] = "gfx/hud/chr/" + "battr" + "u";
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.BaseAttributesButtonDownImage] = "gfx/hud/chr/" + "battr" + "d";


            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1875
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.FoodEventPointsImage] = "gfx/hud/chr/tips/fep";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1876
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L76
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.FoodMeterImage] = "gfx/hud/chr/foodm";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1878
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.FoodSatuationsImage] = "gfx/hud/chr/tips/constip";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1881
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.HungerLevelImage] = "gfx/hud/chr/tips/hunger";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1882
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L246
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.GlutMeterImage] = "gfx/hud/chr/glutm";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1888
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.AbilitiesImage] = "gfx/hud/chr/tips/sattr";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1891
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.AbilitiesUnarmedImage] = "gfx/hud/chr/" + "unarmed";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1892
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.AbilitiesMeleeImage] = "gfx/hud/chr/" + "melee";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1893
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.AbilitiesRangedImage] = "gfx/hud/chr/" + "ranged";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1894
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.AbilitiesExploreImage] = "gfx/hud/chr/" + "explore";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1895
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.AbilitiesStealthImage] = "gfx/hud/chr/" + "stealth";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1896
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.AbilitiesSewingImage] = "gfx/hud/chr/" + "sewing";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1897
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.AbilitiesSmithingImage] = "gfx/hud/chr/" + "smithing";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1898
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.AbilitiesMasonryImage] = "gfx/hud/chr/" + "masonry";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1899
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.AbilitiesCarpentryImage] = "gfx/hud/chr/" + "carpentry";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1900
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.AbilitiesCookingImage] = "gfx/hud/chr/" + "cooking";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1901
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.AbilitiesFarmingImage] = "gfx/hud/chr/" + "farming";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1902
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.AbilitiesSurviveImage] = "gfx/hud/chr/" + "survive";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1903
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.AbilitiesLoreImage] = "gfx/hud/chr/" + "lore";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L497
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L500
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.AbilitiesButtonAddUpImage] = "gfx/hud/buttons/add" + "u";
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.AbilitiesButtonAddDownImage] = "gfx/hud/buttons/add" + "d";
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.AbilitiesButtonSubtractUpImage] = "gfx/hud/buttons/sub" + "u";
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.AbilitiesButtonSubtractDownImage] = "gfx/hud/buttons/sub" + "d";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2107
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2083
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.AbilitiesButtonUpImage] = "gfx/hud/chr/" + "sattr" + "u";
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.AbilitiesButtonDownImage] = "gfx/hud/chr/" + "sattr" + "d";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1906
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.StudyReportImage] = "gfx/hud/chr/tips/study";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1936
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.LoreAndSkillsImage] = "gfx/hud/chr/tips/skills";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2108
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2083
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.LoreAndSkillsButtonUpImage] = "gfx/hud/chr/" + "skill" + "u";
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.LoreAndSkillsButtonDownImage] = "gfx/hud/chr/" + "skill" + "d";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2109
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2083
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.MartialArtsAndCombatSchoolsButtonUpImage] = "gfx/hud/chr/" + "fgt" + "u";
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.MartialArtsAndCombatSchoolsButtonDownImage] = "gfx/hud/chr/" + "fgt" + "d";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2019
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.HealthAndWoundsImage] = "gfx/hud/chr/tips/wounds";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2110
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2083
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.HealthAndWoundsButtonUpImage] = "gfx/hud/chr/" + "wound" + "u";
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.HealthAndWoundsButtonDownImage] = "gfx/hud/chr/" + "wound" + "d";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2043
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.QuestLogImage] = "gfx/hud/chr/tips/quests";

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2111
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2083
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.QuestLogButtonUpImage] = "gfx/hud/chr/" + "quest" + "u";
            result[CharacterSheetWidgetCreateArguments.RESOURCE_NAME.QuestLogButtonDownImage] = "gfx/hud/chr/" + "quest" + "d";
        }

        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/FightWnd.java#L696
        /// </remarks>
        public void Deserialize(ArgumentsReader reader, out FightWindowWidgetCreateArguments result)
        {
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/FightWnd.java#L725
            result = new FightWindowWidgetCreateArguments()
            {
                nsave = reader.ReadInt32(),
                nact = reader.ReadInt32(),
                max = reader.ReadInt32()
            };
        }

        public void Deserialize(ArgumentsReader reader, out FightViewWidgetCreateArguments result)
        {
            result = new FightViewWidgetCreateArguments();

            // https://github.com/dolda2000/hafen-client/blob/394a9d64bc732ed8c2eb6e5df1b57dd08b97c4d8/src/haven/Fightview.java#L33
            result[FightViewWidgetCreateArguments.RESOURCE_NAME.BackgroundImage] = "gfx/hud/bosq";
        }

        public void Deserialize(ArgumentsReader reader, out BuddyWindowWidgetCreateArguments result)
        {
            result = new BuddyWindowWidgetCreateArguments();

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/BuddyWnd.java#L47-L48
            result[BuddyWindowWidgetCreateArguments.RESOURCE_NAME.OnlineImage] = "gfx/hud/online";
            result[BuddyWindowWidgetCreateArguments.RESOURCE_NAME.OfflineImage] = "gfx/hud/offline";
        }

        public void Deserialize(ArgumentsReader reader, out ProgressBarWidgetCreateArguments result)
        {
            result = new ProgressBarWidgetCreateArguments()
            {
                // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/IMeter.java#L42
                BackgroundImageResourceID = reader.ReadUInt16() // Integer?
            };

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/IMeter.java#L43
            result.Meters = new ProgressBarWidgetCreateArguments.Meter[1];

            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/IMeter.java#L84
            for (int i = 0; i < result.Meters.Length; i++)
            {
                result.Meters[i] = new ProgressBarWidgetCreateArguments.Meter()
                {
                    Color = reader.ReadColor(),
                    Value = reader.ReadDouble()
                };
            }
        }

        public void Deserialize(ArgumentsReader reader, out MultiChatWidgetCreateArguments result)
        {
            // https://github.com/dolda2000/hafen-client/blob/9dc7c1e7af3f1e3d49a7e2b42b6a11ec8c463af8/src/haven/ChatUI.java#L855-L857
            result = new MultiChatWidgetCreateArguments()
            {
                Closable = false,
                Name = reader.ReadString(),
                Urgency = reader.ReadInt32()
            };
        }

        public void Deserialize(ArgumentsReader reader, out GearboxWidgetCreateArguments result)
        {
            // https://github.com/dolda2000/hafen-client/blob/f85b82305e06f850c924d3309de68eedbd9209dd/src/haven/Speedget.java#L37-L61
            result = new GearboxWidgetCreateArguments()
            {
                CurrentSpeed = reader.ReadInt32(),
                MaximalSpeed = reader.ReadInt32()
            };

            string ShiftName(GearboxWidgetCreateArguments.RESOURCE_NAME shift)
            {
                switch (shift)
                {
                    case GearboxWidgetCreateArguments.RESOURCE_NAME.Crawl:
                        return "crawl";
                    case GearboxWidgetCreateArguments.RESOURCE_NAME.Walk:
                        return "walk";
                    case GearboxWidgetCreateArguments.RESOURCE_NAME.Run:
                        return "run";
                    case GearboxWidgetCreateArguments.RESOURCE_NAME.Sprint:
                        return "sprint";
                    default:
                        throw new ArgumentOutOfRangeException(nameof(shift));
                }
            }

            string StateName(GearboxWidgetCreateArguments.RESOURCE_NAME state)
            {
                switch (state)
                {
                    case GearboxWidgetCreateArguments.RESOURCE_NAME.Disabled:
                        return "dis";
                    case GearboxWidgetCreateArguments.RESOURCE_NAME.On:
                        return "off";
                    case GearboxWidgetCreateArguments.RESOURCE_NAME.Off:
                        return "on";
                    default:
                        throw new ArgumentOutOfRangeException(nameof(state));
                }
            }

            foreach (var shift in new[] { GearboxWidgetCreateArguments.RESOURCE_NAME.Crawl, GearboxWidgetCreateArguments.RESOURCE_NAME.Walk, GearboxWidgetCreateArguments.RESOURCE_NAME.Run, GearboxWidgetCreateArguments.RESOURCE_NAME.Sprint })
            {
                foreach (var state in new[] { GearboxWidgetCreateArguments.RESOURCE_NAME.Disabled, GearboxWidgetCreateArguments.RESOURCE_NAME.Off, GearboxWidgetCreateArguments.RESOURCE_NAME.On })
                {
                    // https://github.com/dolda2000/hafen-client/blob/f85b82305e06f850c924d3309de68eedbd9209dd/src/haven/Speedget.java#L44
                    result[shift & state & GearboxWidgetCreateArguments.RESOURCE_NAME.Image] = String.Format("gfx/hud/meter/rmeter/{0}-{1}", ShiftName(shift), StateName(state));
                }

                // https://github.com/dolda2000/hafen-client/blob/f85b82305e06f850c924d3309de68eedbd9209dd/src/haven/Speedget.java#L50
                result[shift & GearboxWidgetCreateArguments.RESOURCE_NAME.Tooltip] = String.Format("gfx/hud/meter/rmeter/{0}-on", ShiftName(shift));
            }
        }

        public void Deserialize(ArgumentsReader reader, out CommandsMenuWidgetCreateArguments result)
        {
            result = new CommandsMenuWidgetCreateArguments();

            // https://github.com/dolda2000/hafen-client/blob/25968122bbebc26462e0046ae4b8a0cb480dc65d/src/haven/MenuGrid.java#L133
            result[CommandsMenuWidgetCreateArguments.RESOURCE_NAME.NextButtonImage] = "gfx/hud/sc-next";

            // https://github.com/dolda2000/hafen-client/blob/25968122bbebc26462e0046ae4b8a0cb480dc65d/src/haven/MenuGrid.java#L148
            result[CommandsMenuWidgetCreateArguments.RESOURCE_NAME.BackButtonImage] = "gfx/hud/sc-back";
        }

        public void Deserialize(ArgumentsReader reader, out PartyViewWidgetCreateArguments result)
        {
            result = new PartyViewWidgetCreateArguments()
            {
                // https://github.com/dolda2000/hafen-client/blob/d61d2a6ff4a832f84e55069794578002d37b0ce1/src/haven/Partyview.java#L45
                ign = reader.ReadInt32()
            };
        }

        public void Deserialize(ArgumentsReader reader, out SimpleChatWidgetCreateArguments result)
        {
            // https://github.com/dolda2000/hafen-client/blob/9dc7c1e7af3f1e3d49a7e2b42b6a11ec8c463af8/src/haven/ChatUI.java#L848-L849
            result = new SimpleChatWidgetCreateArguments()
            {
                Closable = false,
                Name = reader.ReadString()
            };
        }

        public void Deserialize(ArgumentsReader reader, out QuestBoxWidgetCreateArguments result)
        {
            // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1386-L1388
            result = new QuestBoxWidgetCreateArguments()
            {
                QuestID = reader.ReadInt32(),
                ResourceID = reader.ReadUInt16(),
                Title = null
            };

            if (!reader.ReachedEnd)
                result.Title = reader.ReadString();
        }
    }
}
