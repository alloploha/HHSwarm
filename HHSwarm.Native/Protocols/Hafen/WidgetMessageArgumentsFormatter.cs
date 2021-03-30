using HHSwarm.Native.Common;
using HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments;
using HHSwarm.Native.Shared;
using HHSwarm.Native.WorldModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using HHSwarm.Model.Common;

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

            result = new ButtonWidgetCreateArguments()
            {
                UpImageResourceName = reader.ReadString(), // [0]
                DownImageResourceName = reader.ReadString() // [1]
            };
        }

        public void Deserialize(ArgumentsReader reader, out CharactersListWidgetCreateArguments result)
        {
            if (reader.Length != 1) throw new ArgumentException($"Number of create widget arguments for {nameof(CharactersListWidgetCreateArguments)} is {reader.Length} while expected number is 1!");

            result = new CharactersListWidgetCreateArguments()
            {
                Height = reader.ReadInt32(),
                BackgroundImageResourceName = "gfx/hud/avakort",
                ScrollUpButtonUpImageResourceName = "gfx/hud/buttons/csa" + "u" + "u",
                ScrollUpButtonDownImageResourceName = "gfx/hud/buttons/csa" + "u" + "d",
                ScrollUpButtonHoverImageResourceName = "gfx/hud/buttons/csa" + "u" + "o",
                ScrollDownButtonUpImageResourceName = "gfx/hud/buttons/csa" + "d" + "u",
                ScrollDownButtonDownImageResourceName = "gfx/hud/buttons/csa" + "d" + "d",
                ScrollDownButtonHoverImageResourceName = "gfx/hud/buttons/csa" + "d" + "o"
            };
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
            result.ResourceName[CharacterSheetWidgetCreateArguments.RESOURCE.BaseAttributesImage] = "gfx/hud/chr/tips/base";

            {
                // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1862
                //BaseAttributesImageResourceName = "gfx/hud/chr/tips/base",
                //ResourceName[CharacterSheetWidgetCreateArguments.RESOURCE.BaseAttributesImage] = "gfx/hud/chr/tips/base"//,

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1865
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L434
                //BaseAttributesStrengthImageResourceName = "gfx/hud/chr/" + "str",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1866
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L434
                //BaseAttributesAgilityImageResourceName = "gfx/hud/chr/" + "agi",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1867
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L434
                //BaseAttributesIntelligenceImageResourceName = "gfx/hud/chr/" + "int",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1868
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L434
                //BaseAttributesConstitutionImageResourceName = "gfx/hud/chr/" + "con",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1869
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L434
                //BaseAttributesPerceptionImageResourceName = "gfx/hud/chr/" + "prc",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1870
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L434
                //BaseAttributesCharismaImageResourceName = "gfx/hud/chr/" + "csm",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1871
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L434
                //BaseAttributesDexterityImageResourceName = "gfx/hud/chr/" + "dex",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1872
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L434
                //BaseAttributesWillImageResourceName = "gfx/hud/chr/" + "wil",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1873
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L434
                //BaseAttributesPsycheImageResourceName = "gfx/hud/chr/" + "psy",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2106
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2083
                //BaseAttributesButtonUpImageResourceName = "gfx/hud/chr/" + "battr" + "u",
                //BaseAttributesButtonDownImageResourceName = "gfx/hud/chr/" + "battr" + "d",


                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1875
                //FoodEventPointsImageResourceName = "gfx/hud/chr/tips/fep",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1876
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L76
                //FoodMeterImageResourceName = "gfx/hud/chr/foodm",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1878
                //FoodSatuationsImageResourceName = "gfx/hud/chr/tips/constip",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1881
                //HungerLevelImageResourceName = "gfx/hud/chr/tips/hunger",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1882
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L246
                //GlutMeterImageResourceName = "gfx/hud/chr/glutm",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1888
                //AbilitiesImageResourceName = "gfx/hud/chr/tips/sattr",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1891
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
                //AbilitiesUnarmedImageResouceName = "gfx/hud/chr/" + "unarmed",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1892
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
                //AbilitiesMeleeImageResouceName = "gfx/hud/chr/" + "melee",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1893
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
                //AbilitiesRangedImageResouceName = "gfx/hud/chr/" + "ranged",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1894
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
                //AbilitiesExploreImageResouceName = "gfx/hud/chr/" + "explore",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1895
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
                //AbilitiesStealthImageResouceName = "gfx/hud/chr/" + "stealth",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1896
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
                //AbilitiesSewingImageResouceName = "gfx/hud/chr/" + "sewing",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1897
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
                //AbilitiesSmithingImageResouceName = "gfx/hud/chr/" + "smithing",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1898
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
                //AbilitiesMasonryImageResouceName = "gfx/hud/chr/" + "masonry",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1899
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
                //AbilitiesCarpentryImageResouceName = "gfx/hud/chr/" + "carpentry",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1900
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
                //AbilitiesCookingImageResouceName = "gfx/hud/chr/" + "cooking",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1901
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
                //AbilitiesFarmingImageResouceName = "gfx/hud/chr/" + "farming",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1902
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
                //AbilitiesSurviveImageResouceName = "gfx/hud/chr/" + "survive",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1903
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L491
                //AbilitiesLoreImageResouceName = "gfx/hud/chr/" + "lore",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L497
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L500
                //AbilitiesButtonAddImageResourceName = "gfx/hud/buttons/add",
                //AbilitiesButtonSubtractImageResourceName = "gfx/hud/buttons/sub",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2107
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2083
                //AbilitiesButtonUpImageResourceName = "gfx/hud/chr/" + "sattr" + "u",
                //AbilitiesButtonDownImageResourceName = "gfx/hud/chr/" + "sattr" + "d",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1906
                //StudyReportImageResourceName = "gfx/hud/chr/tips/study",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1936
                //LoreAndSkillsImageResourceName = "gfx/hud/chr/tips/skills",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2108
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2083
                //LoreAndSkillsButtonUpImageResourceName = "gfx/hud/chr/" + "skill" + "u",
                //LoreAndSkillsButtonDownImageResourceName = "gfx/hud/chr/" + "skill" + "d",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2109
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2083
                //MartialArtsAndCombatSchoolsButtonUpImageResourceName = "gfx/hud/chr/" + "fgt" + "u",
                //MartialArtsAndCombatSchoolsButtonDownImageResourceName = "gfx/hud/chr/" + "fgt" + "d",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2019
                //HealthAndWoundsImageResourceName = "gfx/hud/chr/tips/wounds",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2110
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2083
                //HealthAndWoundsButtonUpImageResourceName = "gfx/hud/chr/" + "wound" + "u",
                //HealthAndWoundsButtonDownImageResourceName = "gfx/hud/chr/" + "wound" + "d",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2043
                //QuestLogImageResourceName = "gfx/hud/chr/tips/quests",

                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2111
                //// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L2083
                //QuestLogButtonUpImageResourceName = "gfx/hud/chr/" + "quest" + "u",
                //QuestLogButtonDownImageResourceName = "gfx/hud/chr/" + "quest" + "d"
            };
        }
    }
}
