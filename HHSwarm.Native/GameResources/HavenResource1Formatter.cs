using HHSwarm.Native.Common;
using HHSwarm.Native.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace HHSwarm.Native.GameResources
{
    public class HavenResource1Formatter : HavenResourceFormatter
    {
        public HavenResource1Formatter() : this(HavenResource1.FILE_SIGNATURE)
        {

        }

        protected HavenResource1Formatter(byte[] fileSignature) : base(fileSignature)
        {

        }

        /// <summary>
        /// Декодирует очередной ресурс указанного типа из потока.
        /// </summary>
        /// <remarks>
        /// Вызывает декодировщик для указанного типа. Фактически - занимается только диспетчеризацией; непосредственное декодирование производится в отдельных методах.
        /// </remarks>
        /// <param name="reader">Поток данных файла ресурсов</param>
        /// <param name="resourceType">Тип ресурса</param>
        /// <param name="receiver">После декодирования, будет вызван метод, соответствующий типу ресурса</param>
        protected override void Deserialize(IMessageBinaryReader reader, string resourceType, IHavenResourceReceiver receiver)
        {
            string trace_dump_message = $"Deserialized game resource of type {resourceType}";

            switch (resourceType)
            {
                case "code":
                    // @LayerName("code")
                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1162
                    {
                        JavaClassResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "codeentry":
                    // @LayerName("codeentry")
                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1239
                    {
                        JavaClassEntryResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "tex":
                    // @Resource.LayerName("tex")
                    // https://github.com/dolda2000/hafen-client/blob/394aeed6e3ebbfa64d679a7d4fdda364a982d8bb/src/haven/TexR.java#L38
                    {
                        TextureResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "mat2":
                    // @Resource.LayerName("mat2")
                    // https://github.com/dolda2000/hafen-client/blob/f85b82305e06f850c924d3309de68eedbd9209dd/src/haven/Material.java#L349
                    {
                        MaterialResourceLayer2 resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "boneoff":
                    // @Resource.LayerName("boneoff")
                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Skeleton.java#L1040
                    {
                        BoneOffsetResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "mesh":
                    // @Resource.LayerName("mesh")
                    // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/FastMesh.java#L220
                    {
                        MeshResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "vbuf":
                    // @Resource.LayerName("vbuf")
                    {
                        VertexBufferResourceLayer1 resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "vbuf2":
                    // @Resource.LayerName("vbuf2")
                    // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/VertexBuf.java#L412
                    {
                        VertexBufferResourceLayer2 resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "skel":
                    // @Resource.LayerName("skel")
                    // https://github.com/dolda2000/hafen-client/blob/f85b82305e06f850c924d3309de68eedbd9209dd/src/haven/Skeleton.java#L552
                    {
                        SkeletonResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "skan":
                    // @Resource.LayerName("skan")
                    // https://github.com/dolda2000/hafen-client/blob/f85b82305e06f850c924d3309de68eedbd9209dd/src/haven/Skeleton.java#L886
                    {
                        SkeletonAnimationResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "neg":
                    // @LayerName("neg")
                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1036
                    {
                        NegResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "image":
                    // @LayerName("image")
                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L902
                    {
                        ImageResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "rlink":
                    // @Resource.LayerName("rlink")
                    // https://github.com/dolda2000/hafen-client/blob/f96bf3fd3ebe4391f794448def33f04289c93137/src/haven/RenderLink.java#L203
                    {
                        RenderLinkResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "anim":
                    // @LayerName("anim")
                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1058
                    {
                        AnimationResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "pagina":
                    // @LayerName("pagina"), A "page" of Command Buttons at the right bottom corner of the screen.
                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1086
                    {
                        PaginaResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "action":
                    // @LayerName("action"), Action Button
                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1097
                    {
                        ActionButtonResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "font":
                    // @LayerName("font")
                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1454
                    // TODO: @LayerName("font")
                    throw new NotImplementedException($"@LayerName(\"{resourceType}\")");
                    break;
                case "audio":
                    // @LayerName("audio")
                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1389
                    // TODO: @LayerName("audio")
                    throw new NotImplementedException($"@LayerName(\"{resourceType}\")");
                    break;
                case "audio2":
                    // @LayerName("audio2")
                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1419
                    // TODO: @LayerName("audio2")
                    throw new NotImplementedException($"@LayerName(\"{resourceType}\")");
                    break;
                case "midi":
                    // @LayerName("midi")
                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1437
                    // TODO: @LayerName("midi")
                    throw new NotImplementedException($"@LayerName(\"{resourceType}\")");
                    break;
                case "manim":
                    // @Resource.LayerName("manim")
                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/MeshAnim.java#L378
                    {
                        MeshAnimationResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "tooltip":
                    // @LayerName("tooltip")
                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1025
                    {
                        TooltipResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "light":
                    // @Resource.LayerName("light")
                    // https://github.com/dolda2000/hafen-client/blob/f85b82305e06f850c924d3309de68eedbd9209dd/src/haven/Light.java#L237
                    {
                        LightResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "tileset":
                    // @Resource.LayerName("tileset")
                    // https://github.com/dolda2000/hafen-client/blob/f85b82305e06f850c924d3309de68eedbd9209dd/src/haven/Tileset.java#L235
                    // TODO: @Resource.LayerName("tileset")
                    throw new NotImplementedException($"@Resource.LayerName(\"{resourceType}\")");
                    break;
                case "tileset2":
                    // @Resource.LayerName("tileset2")
                    // https://github.com/dolda2000/hafen-client/blob/f85b82305e06f850c924d3309de68eedbd9209dd/src/haven/Tileset.java#L35
                    // TODO: @Resource.LayerName("tileset2")
                    throw new NotImplementedException($"@Resource.LayerName(\"{resourceType}\")");
                    break;
                case "tile":
                    // @Resource.LayerName("tile")
                    // https://github.com/dolda2000/hafen-client/blob/f85b82305e06f850c924d3309de68eedbd9209dd/src/haven/Tileset.java#L47
                    // TODO: @Resource.LayerName("tile")
                    throw new NotImplementedException($"@Resource.LayerName(\"{resourceType}\")");
                    break;
                case "slink":
                    // @Resource.LayerName("slink")
                    // https://github.com/dolda2000/hafen-client/blob/f85b82305e06f850c924d3309de68eedbd9209dd/src/haven/SpriteLink.java#L33
                    // TODO: @Resource.LayerName("slink")
                    throw new NotImplementedException($"@Resource.LayerName(\"{resourceType}\")");
                    break;
                case "clamb":
                    // @Resource.LayerName("clamb")
                    // https://github.com/dolda2000/hafen-client/blob/f85b82305e06f850c924d3309de68eedbd9209dd/src/haven/ClipAmbiance.java#L257
                    // TODO: @Resource.LayerName("clamb")
                    throw new NotImplementedException($"@Resource.LayerName(\"{resourceType}\")");
                    break;
                case "overlay":
                    // @Resource.LayerName("overlay")
                    // https://github.com/dolda2000/hafen-client/blob/c254d17c9b05363eff99c9e9baa529b2be779f4b/src/haven/MCache.java#L101
                    // TODO: @Resource.LayerName("overlay")
                    throw new NotImplementedException($"@Resource.LayerName(\"{resourceType}\")");
                    break;
                case "foodev":
                    // @Resource.LayerName("foodev")
                    // Food Event
                    // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L85
                    // TODO: @Resource.LayerName("foodev")
                    throw new NotImplementedException($"@Resource.LayerName(\"{resourceType}\")");
                    break;
                case "obst":
                    // TODO: ?Obstacles?
                    {
                        int size = reader.ReadInt32();
                        byte[] remainder = reader.ReadBytes(size);
                        Trace.TraceWarning("Resource layer '{0}' (length: {1}) received. No original client code exists for such object. Deprecated?", resourceType, reader.Length - reader.Position);
#if DEBUG
                        //File.WriteAllBytes(String.Format(@"C:\Temp\HHSwarm-ExtractedResources\obst-{0}", DateTime.Now.Millisecond), remainder);
#endif
                    }
                    break;
                case "src":
                    // ?Preprocessed source code?
                    {
                        Trace.TraceWarning("Resource layer '{0}' (length: {1}) received. No original client code exists for such object. Deprecated?", resourceType, reader.Length - reader.Position);

                        JavaSourceCodeResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                default:
                    {
#if DEBUG
                        Debugger.Break();
                        byte[] remainder = reader.ReadBytes((int)(reader.Length - reader.Position));
#endif
                        throw new NotImplementedException($"Unexpected resource type '{resourceType}'!");
                    }
            }
        }

        private void Deserialize(IMessageBinaryReader reader, out JavaSourceCodeResourceLayer resource)
        {
            resource = ExtractResourceFromLayer(reader, (nextLayerPosition) =>
            {
                JavaSourceCodeResourceLayer result = new JavaSourceCodeResourceLayer()
                {
                    N = reader.ReadByte(),
                    FileName = reader.ReadString((int)(nextLayerPosition - reader.Position)),
                    Text = reader.ReadBytes((int)(nextLayerPosition - reader.Position))
                };

                return result;
            });
        }

        private void Deserialize(IMessageBinaryReader reader, out ActionButtonResourceLayer resource)
        {
            resource = ExtractResourceFromLayer(reader, (nextLayerPosition) =>
            {
                // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1104-L1122
                ActionButtonResourceLayer result = new ActionButtonResourceLayer()
                {
                    ParentResourceName = reader.ReadString((int)(nextLayerPosition - reader.Position)),
                    ParentResourceVersion = reader.ReadUInt16(),
                    Name = reader.ReadString((int)(nextLayerPosition - reader.Position)),
                    PrerequisiteSkill = reader.ReadString((int)(nextLayerPosition - reader.Position)),
                    HotKey = Convert.ToChar(reader.ReadUInt16()),
                    ad = new string[reader.ReadUInt16()]
                };

                for (int i = 0; i < result.ad.Length; i++)
                {
                    result.ad[i] = reader.ReadString((int)(nextLayerPosition - reader.Position));
                }

                return result;
            });
        }

        private void Deserialize(IMessageBinaryReader reader, out AnimationResourceLayer resource)
        {
            resource = ExtractResourceFromLayer(reader, (nextLayerPosition) =>
            {
                // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1064-L1070
                AnimationResourceLayer result = new AnimationResourceLayer()
                {
                    ID = reader.ReadInt16(),
                    D = reader.ReadUInt16()
                };

                result.ImageID = new short[reader.ReadUInt16()];

                for (int i = 0; i < result.ImageID.Length; i++)
                {
                    result.ImageID[i] = reader.ReadInt16();
                }

                return result;
            });
        }

        protected void Deserialize(IMessageBinaryReader reader, out JavaClassResourceLayer resource)
        {
            resource = ExtractResourceFromLayer(reader, (nextLayerPosition) =>
            {
                JavaClassResourceLayer result = new JavaClassResourceLayer()
                {
                    Name = reader.ReadString((int)(nextLayerPosition - reader.Position)),
                    Code = reader.ReadBytes((int)(nextLayerPosition - reader.Position))
                };

                return result;
            });
        }

        protected void Deserialize(IMessageBinaryReader reader, out JavaClassEntryResourceLayer resource)
        {
            resource = ExtractResourceFromLayer(reader, (nextLayerPosition) =>
            {
                JavaClassEntryResourceLayer result = new JavaClassEntryResourceLayer();

                do
                {
                    JavaClassEntryResourceLayer.TYPE type = (JavaClassEntryResourceLayer.TYPE)reader.ReadByte();

                    switch (type)
                    {
                        case JavaClassEntryResourceLayer.TYPE.ByClassFactoryName:
                            {
                                bool stop = false;
                                do
                                {
                                    var item = new JavaClassEntryResourceLayer.ClassFactory()
                                    {
                                        BaseClassFactoryPublishedName = reader.ReadString((int)(nextLayerPosition - reader.Position)),
                                        ClassFactoryName = reader.ReadString((int)(nextLayerPosition - reader.Position))
                                    };

                                    stop = String.IsNullOrEmpty(item.BaseClassFactoryPublishedName);

                                    if (!stop)
                                    {
                                        result.PublishedEntries.Add(item);
                                    }
                                } while (!stop && reader.Position < nextLayerPosition);
                            }
                            break;
                        case JavaClassEntryResourceLayer.TYPE.ClassNameAndVersion:
                            {
                                bool stop = false;
                                do
                                {
                                    var item = new JavaClassEntryResourceLayer.LoadName()
                                    {
                                        Name = reader.ReadString((int)(nextLayerPosition - reader.Position))
                                    };

                                    stop = String.IsNullOrEmpty(item.Name);

                                    if (!stop)
                                    {
                                        item.Version = reader.ReadUInt16();

                                        result.ToLoad.Add(item);
                                    }

                                } while (!stop && reader.Position < nextLayerPosition);
                            }
                            break;
                        default:
                            throw new NotImplementedException($"Unexpected Class Entry Point type {type}!");
                    }
                } while (reader.Position < nextLayerPosition);

                return result;
            });
        }

        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/394aeed6e3ebbfa64d679a7d4fdda364a982d8bb/src/haven/TexR.java#L45
        /// </remarks>
        protected void Deserialize(IMessageBinaryReader reader, out TextureResourceLayer resource)
        {
            resource = ExtractResourceFromLayer(reader, (nextLayerPosition) =>
            {
                TextureResourceLayer result = new TextureResourceLayer()
                {
                    TextureID = reader.ReadInt16(),

                    Offset = new System.Drawing.Point
                    (
                        reader.ReadUInt16(),
                        reader.ReadUInt16()
                    ),

                    Size = new System.Drawing.Size
                    (
                        reader.ReadUInt16(),
                        reader.ReadUInt16()
                    ),

                    MagFilterType = Graphics.TextureFilter.TYPE.LINEAR,
                    MinFilterType = Graphics.TextureFilter.TYPE.LINEAR
                };

                // https://github.com/dolda2000/hafen-client/blob/394aeed6e3ebbfa64d679a7d4fdda364a982d8bb/src/haven/TexR.java#L52
                do
                {
                    TextureResourceLayer.DATA_PART type = (TextureResourceLayer.DATA_PART)reader.ReadByte();

                    switch (type)
                    {
                        case TextureResourceLayer.DATA_PART.Image:
                            // https://github.com/dolda2000/hafen-client/blob/394aeed6e3ebbfa64d679a7d4fdda364a982d8bb/src/haven/TexR.java#L55
                            {
                                int length = reader.ReadInt32();
                                result.Image = reader.ReadBytes(length);
                            }
                            break;
                        case TextureResourceLayer.DATA_PART.MipmapGeneratorType:
                            // https://github.com/dolda2000/hafen-client/blob/394aeed6e3ebbfa64d679a7d4fdda364a982d8bb/src/haven/TexR.java#L58
                            {
                                result.MipmapGeneratorType = (Graphics.MipmapGenerator.TYPE)reader.ReadByte();
                                if (result.MipmapGeneratorType == Graphics.MipmapGenerator.TYPE.Default) result.MipmapGeneratorType = Graphics.MipmapGenerator.TYPE.Average;
                            }
                            break;
                        case TextureResourceLayer.DATA_PART.MagnificationFilter:
                            // https://github.com/dolda2000/hafen-client/blob/394aeed6e3ebbfa64d679a7d4fdda364a982d8bb/src/haven/TexR.java#L70
                            {
                                result.MagFilterType = (Graphics.TextureFilter.TYPE)reader.ReadByte();
                            }
                            break;
                        case TextureResourceLayer.DATA_PART.MinificationFilter:
                            // https://github.com/dolda2000/hafen-client/blob/394aeed6e3ebbfa64d679a7d4fdda364a982d8bb/src/haven/TexR.java#L74-L80
                            {
                                result.MinFilterType = (Graphics.TextureFilter.TYPE)reader.ReadByte();
                            }
                            break;
                        case TextureResourceLayer.DATA_PART.Mask:
                            // https://github.com/dolda2000/hafen-client/blob/394aeed6e3ebbfa64d679a7d4fdda364a982d8bb/src/haven/TexR.java#L84
                            {
                                int size = reader.ReadInt32();
                                result.Mask = reader.ReadBytes(size);
                            }
                            break;
                        case TextureResourceLayer.DATA_PART.LinearColorValues:
                            /* Linear color values, not relevant right now */
                            break;
                        default:
                            // https://github.com/dolda2000/hafen-client/blob/394aeed6e3ebbfa64d679a7d4fdda364a982d8bb/src/haven/TexR.java#L90
                            throw new NotImplementedException($"Unexpected Texture Resource record type {type}!");
                    }

                } while (reader.Position < nextLayerPosition);

                if (result.MipmapGeneratorType == Graphics.MipmapGenerator.TYPE.Default)
                    result.MipmapGeneratorType = Graphics.MipmapGenerator.TYPE.Average;

                return result;
            });
        }

        protected void Deserialize(IMessageBinaryReader reader, out MaterialResourceLayer2 resource)
        {
            resource = ExtractResourceFromLayer(reader, (nextLayerPosition) =>
            {
                MaterialResourceLayer2 result = new MaterialResourceLayer2()
                {
                    MaterialID = reader.ReadUInt16()
                };

                IMaterialResourceLayer2PartsReceiver parts = result;

                do
                {
                    string material_part_name = reader.ReadString((int)(nextLayerPosition - reader.Position));

#if DEBUG
                    string trace_dump_message = $"Deserialized material (ID: {result.MaterialID}) part of type {material_part_name}";
                    parts = new MaterialResourceLayer2PartsTraceDump(trace_dump_message, result);
#endif

                    var list = reader.ReadList().ToArray();

                    if (!String.IsNullOrEmpty(material_part_name))
                    {
                        ArgumentsReader list_reader = new ArgumentsReader(list);

                        switch (material_part_name)
                        {
                            case "linear":
                                result.Linear = true;
                                break;
                            case "mipmap":
                                result.Mipmap = true;
                                break;
                            case "col":
                                // @ResName("col")
                                {
                                    MaterialResourceLayer2.ColorsPart part;
                                    DeserializeMaterialPart(list_reader, out part);
                                    parts.Receive(part);
                                }
                                break;
                            case "pal":
                                // @Material.ResName("pal")
                                {
                                    MaterialResourceLayer2.TexPalPart part;
                                    DeserializeMaterialPart(list_reader, out part);
                                    parts.Receive(part);
                                }
                                break;
                            case "light":
                                // @Material.ResName("light")
                                {
                                    MaterialResourceLayer2.LightPart part;
                                    DeserializeMaterialPart(list_reader, out part);
                                    parts.Receive(part);
                                }
                                break;
                            case "order":
                                // @ResName("order")
                                {
                                    MaterialResourceLayer2.OrderPart part;
                                    DeserializeMaterialPart(list_reader, out part);
                                    parts.Receive(part);
                                }
                                break;
                            case "tex":
                                // @Material.ResName("tex")
                                {
                                    MaterialResourceLayer2.TexPart part;
                                    DeserializeMaterialPart(list_reader, out part);
                                    parts.Receive(part);
                                }
                                break;
                            case "cel":
                                // @Material.ResName("cel")
                                {
                                    MaterialResourceLayer2.CelShadePart part;
                                    DeserializeMaterialPart(list_reader, out part);
                                    parts.Receive(part);
                                }
                                break;
                            case "mlink":
                                // @ResName("mlink")
                                {
                                    MaterialResourceLayer2.MaterialLink part;
                                    DeserializeMaterialPart(list_reader, out part);
                                    parts.Receive(part);
                                }
                                break;
                            case "nofacecull":
                                // @ResName("nofacecull")
                                result.FaceCulling = true;
                                break;
                            case "otex":
                                // @Material.ResName("otex")
                                {
                                    MaterialResourceLayer2.OverTex part;
                                    DeserializeMaterialPart(list_reader, out part);
                                    parts.Receive(part);
                                }
                                break;
                            case "vcol":
                                // @Material.ResName("vcol")
                                {
                                    MaterialResourceLayer2.ColorState part;
                                    DeserializeMaterialPart(list_reader, out part);
                                    parts.Receive(part);
                                }
                                break;
                            case "texrot":
                                // @Material.ResName("texrot")
                                {
                                    MaterialResourceLayer2.TexAnim part;
                                    DeserializeMaterialPart(list_reader, out part);
                                    parts.Receive(part);
                                }
                                break;
                            case "maskcol":
                                // @Material.ResName("maskcol")
                                result.MaskColor = MaterialResourceLayer2.MaskColorPart.RGBA;
                                break;
                            default:
                                {
#if DEBUG
                                    Debugger.Break();
#endif
                                    throw new NotImplementedException($"Material resource part type '{material_part_name}' has not been implemented!");
                                }
                        }
                    }
                } while (reader.Position < nextLayerPosition);

                return result;
            });
        }

        private void DeserializeMaterialPart(ArgumentsReader reader, out MaterialResourceLayer2.ColorsPart part)
        {
            if (reader.Length != 5) throw new InvalidDataException($"For material part '{nameof(MaterialResourceLayer2.ColorsPart)}' 5 items expected but {reader.Length} arrived!");

            part = new MaterialResourceLayer2.ColorsPart()
            {
                Ambient = reader.ReadColor(),
                Diffuse = reader.ReadColor(),
                Specular = reader.ReadColor(),
                Emission = reader.ReadColor(),
                Shine = reader.ReadSingle()
            };
        }

        private void DeserializeMaterialPart(ArgumentsReader reader, out MaterialResourceLayer2.TexPalPart part)
        {
            if (!new int[] { 1, 3 }.Contains(reader.Length)) throw new InvalidDataException($"For material part '{nameof(MaterialResourceLayer2.TexPalPart)}' 1 or 3 items expected but {reader.Length} arrived!");

            if (reader.Length == 3)
            {
                part = new MaterialResourceLayer2.TexPalPart()
                {
                    ResourceName = reader.ReadString(),
                    ResourceVersion = reader.ReadUInt16(),
                    TextureID = reader.ReadUInt16()
                };
            }
            else if (reader.Length == 1)
            {
                part = new MaterialResourceLayer2.TexPalPart()
                {
                    TextureID = reader.ReadUInt16()
                };
            }
            else
                throw new NotImplementedException();
        }

        private void DeserializeMaterialPart(ArgumentsReader reader, out MaterialResourceLayer2.LightPart part)
        {
            if (reader.Length != 1) throw new InvalidDataException($"For material part '{nameof(MaterialResourceLayer2.LightPart)}' 1 item expected but {reader.Length} arrived!");

            part = new MaterialResourceLayer2.LightPart();

            string name = reader.ReadString();

            switch (name)
            {
                case "def":
                    part.Algorithm = MaterialResourceLayer2.LightPart.ShaderAlgorithm.Default;
                    break;
                case "pv":
                    part.Algorithm = MaterialResourceLayer2.LightPart.ShaderAlgorithm.PhongVertex;
                    break;
                case "pp":
                    part.Algorithm = MaterialResourceLayer2.LightPart.ShaderAlgorithm.PhongFragment;
                    break;
                case "n":
                    part.Algorithm = MaterialResourceLayer2.LightPart.ShaderAlgorithm.None;
                    break;
                default:
                    throw new NotImplementedException($"Unexpected Material light type name {name}!");
            }
        }

        private void DeserializeMaterialPart(ArgumentsReader reader, out MaterialResourceLayer2.OrderPart part)
        {
            if (reader.Length != 1) throw new InvalidDataException($"For material part '{nameof(MaterialResourceLayer2.OrderPart)}' 1 item expected but {reader.Length} arrived!");

            part = new MaterialResourceLayer2.OrderPart();
            string name = reader.ReadString();

            switch (name)
            {
                case "first":
                    part.DrawOrder = MaterialResourceLayer2.OrderPart.DrawOrderType.First;
                    break;
                case "last":
                    part.DrawOrder = MaterialResourceLayer2.OrderPart.DrawOrderType.Last;
                    break;
                case "pfx":
                    part.DrawOrder = MaterialResourceLayer2.OrderPart.DrawOrderType.PostPfx;
                    break;
                case "eye":
                    part.DrawOrder = MaterialResourceLayer2.OrderPart.DrawOrderType.EyeSort;
                    break;
                case "earlyeye":
                    part.DrawOrder = MaterialResourceLayer2.OrderPart.DrawOrderType.EarlyEyeSort;
                    break;
                case "premap":
                    part.DrawOrder = MaterialResourceLayer2.OrderPart.DrawOrderType.PreMap;
                    break;
                case "postmap":
                    part.DrawOrder = MaterialResourceLayer2.OrderPart.DrawOrderType.PostMap;
                    break;
                default:
                    throw new NotImplementedException($"Unexpected Material draw order name {name}!");
            }
        }

        private void DeserializeMaterialPart(ArgumentsReader reader, out MaterialResourceLayer2.TexPart part)
        {
            if (reader.Length < 1) throw new InvalidDataException($"For material part '{nameof(MaterialResourceLayer2.TexPart)}' 1 or more items expected but {reader.Length} arrived!");

            part = new MaterialResourceLayer2.TexPart();

            string name = reader.ReadString_ifType();

            if (!String.IsNullOrEmpty(name) && reader.Length >= 3)
            {
                part.ResourceName = name;
                part.ResourceVersion = reader.ReadUInt16();
                part.TextureID = reader.ReadUInt16();
            }
            else
            {
                part.TextureID = reader.ReadUInt16();
            }

            while (!reader.ReachedEnd && !part.Clip)
            {
                string value = reader.ReadString_ifType();
                part.Clip = "a".Equals(value);
            }
        }

        private void DeserializeMaterialPart(ArgumentsReader reader, out MaterialResourceLayer2.CelShadePart part)
        {
            if (reader.Length > 1) throw new InvalidDataException($"For material part '{nameof(MaterialResourceLayer2.CelShadePart)}' 1 or nothing expected but {reader.Length} arrived!");

            if (reader.Length == 0)
            {
                part = new MaterialResourceLayer2.CelShadePart()
                {
                    Diffuse = true,
                    Specular = false
                };
            }
            else if (reader.Length == 1)
            {
                part = new MaterialResourceLayer2.CelShadePart();

                string text = reader.ReadString();

                part.Diffuse = text.Contains("d");
                part.Specular = text.Contains("s");
            }
            else
                throw new NotImplementedException();
        }

        private void DeserializeMaterialPart(ArgumentsReader reader, out MaterialResourceLayer2.MaterialLink part)
        {
            if (reader.Length < 2) throw new InvalidDataException($"For material part '{nameof(MaterialResourceLayer2.MaterialLink)}' 2 or more items expected but {reader.Length} arrived!");

            part = new MaterialResourceLayer2.MaterialLink()
            {
                ResourceName = reader.ReadString(),
                ResourceVersion = reader.ReadUInt16()
            };

            if (reader.Length > 2)
            {
                part.ResourceLayerID = reader.ReadUInt16();
            }
        }

        private void DeserializeMaterialPart(ArgumentsReader reader, out MaterialResourceLayer2.OverTex part)
        {
            if (reader.Length < 1) throw new InvalidDataException($"For material part '{nameof(MaterialResourceLayer2.OverTex)}' 1 or more items expected but {reader.Length} arrived!");

            part = new MaterialResourceLayer2.OverTex()
            {
                Blend = MaterialResourceLayer2.OverTex.BlendMode.cpblend
            };

            string name = reader.ReadString_ifType();

            if (!String.IsNullOrEmpty(name))
            {
                part.ResourceName = name;
                part.ResourceVersion = reader.ReadUInt16();
            }

            part.TextureID = reader.ReadUInt16();

            if (!reader.ReachedEnd)
            {
                string blend_mode_name = reader.ReadString(); // 'nm'

                switch (blend_mode_name)
                {
                    case "cp":
                        part.Blend = MaterialResourceLayer2.OverTex.BlendMode.cpblend;
                        break;
                    case "ol":
                        part.Blend = MaterialResourceLayer2.OverTex.BlendMode.olblend;
                        break;
                    case "a":
                        part.Blend = MaterialResourceLayer2.OverTex.BlendMode.colblend;
                        break;
                    default:
                        throw new NotImplementedException($"Unexpected overtex blend mode {blend_mode_name}!");
                }
            }
        }

        private void DeserializeMaterialPart(ArgumentsReader reader, out MaterialResourceLayer2.ColorState part)
        {
            part = new MaterialResourceLayer2.ColorState()
            {
                Color = reader.ReadColor()
            };
        }

        private void DeserializeMaterialPart(ArgumentsReader reader, out MaterialResourceLayer2.TexAnim part)
        {
            part = new MaterialResourceLayer2.TexAnim()
            {
                ax = new Coord3f
                (
                    x: reader.ReadSingle(),
                    y: reader.ReadSingle(),
                    z: 0
                )
            };
        }

        protected void Deserialize(IMessageBinaryReader reader, out BoneOffsetResourceLayer resource)
        {
            resource = ExtractResourceFromLayer(reader, (nextLayerPosition) =>
            {
                BoneOffsetResourceLayer result = new BoneOffsetResourceLayer()
                {
                    Name = reader.ReadString((int)(nextLayerPosition - reader.Position))
                };

                do
                {
                    BoneOffsetResourceLayer.COMMAND_TYPE command_type = (BoneOffsetResourceLayer.COMMAND_TYPE)reader.ReadByte();

                    string trace_dump_message = $"Deserialized Bone Offset {result.Name} command of type {command_type}";

                    switch (command_type)
                    {
                        case BoneOffsetResourceLayer.COMMAND_TYPE.Translate:
                            {
                                var command = new BoneOffsetResourceLayer.Xlate()
                                {
                                    X = reader.ReadDouble20bit(),
                                    Y = reader.ReadDouble20bit(),
                                    Z = reader.ReadDouble20bit()
                                };
                                Trace.Dump(TraceEventType.Verbose, trace_dump_message, command);
                                result.Commands.Add(command);
                            }
                            break;
                        case BoneOffsetResourceLayer.COMMAND_TYPE.Rotate:
                            {
                                var command = new BoneOffsetResourceLayer.Rot()
                                {
                                    Angle = reader.ReadDouble20bit(),
                                    AxisX = reader.ReadDouble20bit(),
                                    AxisY = reader.ReadDouble20bit(),
                                    AxisZ = reader.ReadDouble20bit()
                                };
                                Trace.Dump(TraceEventType.Verbose, trace_dump_message, command);
                                result.Commands.Add(command);
                            }
                            break;
                        case BoneOffsetResourceLayer.COMMAND_TYPE.TranslateBoneByName:
                            {
                                var command = new BoneOffsetResourceLayer.Bonetrans()
                                {
                                    BoneName = reader.ReadString((int)(nextLayerPosition - reader.Position))
                                };
                                Trace.Dump(TraceEventType.Verbose, trace_dump_message, command);
                                result.Commands.Add(command);
                            }
                            break;
                        case BoneOffsetResourceLayer.COMMAND_TYPE.AlignBone:
                            {
                                var command = new BoneOffsetResourceLayer.BoneAlign()
                                {
                                    RefX = reader.ReadDouble20bit(),
                                    RefY = reader.ReadDouble20bit(),
                                    RefZ = reader.ReadDouble20bit(),
                                    OriginBoneName = reader.ReadString((int)(nextLayerPosition - reader.Position)),
                                    TargetBoneName = reader.ReadString((int)(nextLayerPosition - reader.Position))
                                };
                                Trace.Dump(TraceEventType.Verbose, trace_dump_message, command);
                                result.Commands.Add(command);
                            }
                            break;
                        default:
                            throw new NotImplementedException($"Unexpected Bone Offset command {command_type}!");
                    }

                } while (reader.Position < nextLayerPosition);

                return result;
            });
        }

        protected void Deserialize(IMessageBinaryReader reader, out MeshResourceLayer resource)
        {
            resource = ExtractResourceFromLayer(reader, (nextLayerPosition) =>
            {
                MeshResourceLayer.FLAGS flags = (MeshResourceLayer.FLAGS)reader.ReadByte();
                ushort number_of_items = reader.ReadUInt16();

                MeshResourceLayer result = new MeshResourceLayer()
                {
                    MaterialID = reader.ReadInt16(),
                    ID = -1,
                    Ref = -1,
                    Items = new short[number_of_items, 3]
                };

                if (flags.HasFlag(MeshResourceLayer.FLAGS.ID))
                {
                    result.ID = reader.ReadInt16();
                }

                if (flags.HasFlag(MeshResourceLayer.FLAGS.Ref))
                {
                    result.Ref = reader.ReadInt16();
                }

                if (flags.HasFlag(MeshResourceLayer.FLAGS.Rdat))
                {
                    bool stop = false;
                    do
                    {
                        string key = reader.ReadString((int)(nextLayerPosition - reader.Position));
                        stop = String.IsNullOrEmpty(key);

                        if (!stop)
                        {
                            string value = reader.ReadString((int)(nextLayerPosition - reader.Position));
                            result.Rdat.Add(key, value);
                        }
                    } while (!stop && reader.Position < nextLayerPosition);
                }

                for (int i = 0; i < number_of_items; i++)
                {
                    for (int n = 0; n < 3; n++)
                    {
                        result.Items[i, n] = reader.ReadInt16();
                    }
                }

                return result;
            });
        }

        [Obsolete]
        protected void Deserialize(IMessageBinaryReader reader, out VertexBufferResourceLayer1 resource)
        {
            resource = ExtractResourceFromLayer(reader, (nextLayerPosition) =>
            {
                VertexBufferResourceLayer1.FLAGS flags = (VertexBufferResourceLayer1.FLAGS)reader.ReadByte(); // 'fl'

                ushort number_of_items = reader.ReadUInt16(); // 'num'

                VertexBufferResourceLayer1 result = new VertexBufferResourceLayer1()
                {
                };

                while (reader.Position < nextLayerPosition)
                {
                    VertexBufferResourceLayer1.ArrayType array_type = (VertexBufferResourceLayer1.ArrayType)reader.ReadByte(); // 'id'

                    switch (array_type)
                    {
                        case VertexBufferResourceLayer1.ArrayType.Vertex:
                            {
                                Debug.Assert(result.Vertexes == null, $"Array Type '{array_type}' appeared more than once in the same resource!");
                                result.Vertexes = ReadVertexBufferResource1ArrayOfSingle(reader, 3, number_of_items);
                            }
                            break;
                        case VertexBufferResourceLayer1.ArrayType.Normal:
                            {
                                Debug.Assert(result.Normals == null, $"Array Type '{array_type}' appeared more than once in the same resource!");
                                result.Normals = ReadVertexBufferResource1ArrayOfSingle(reader, 3, number_of_items);
                            }
                            break;
                        case VertexBufferResourceLayer1.ArrayType.Texel:
                            {
                                Debug.Assert(result.Texels == null, $"Array Type '{array_type}' appeared more than once in the same resource!");
                                result.Texels = ReadVertexBufferResource1ArrayOfSingle(reader, 2, number_of_items);
                            }
                            break;
                        case VertexBufferResourceLayer1.ArrayType.Bone:
                            {
                                Debug.Assert(result.Bones == null || result.Bones.Length == 0, $"Array Type '{array_type}' appeared more than once in the same resource!");
                                result.Bones = ReadVertexBufferResource1ArrayOfBones(reader, number_of_items);
                            }
                            break;
                        default:
                            throw new NotImplementedException($"Unexpected Vertex Buffer array type {array_type}!");
                    }
                }

                return result;
            });
        }

        protected void Deserialize(IMessageBinaryReader reader, out VertexBufferResourceLayer2 resource)
        {
            resource = ExtractResourceFromLayer(reader, (nextLayerPosition) =>
            {
                byte header = reader.ReadByte();
                VertexBufferResourceLayer2.FLAGS flags = (VertexBufferResourceLayer2.FLAGS)header; // 'fl'

                VertexBufferResourceLayer2 result = new VertexBufferResourceLayer2()
                {
                    Version = (byte)(header & 0x0F),
                    ID = 0
                };

                if (result.Version >= 2) throw new NotImplementedException($"Uknown {nameof(VertexBufferResourceLayer2)} version {result.Version}!");

                if (result.Version >= 1)
                    result.ID = reader.ReadInt16();

                ushort number_of_items = reader.ReadUInt16(); // 'num'

                while (reader.Position < nextLayerPosition)
                {
                    string array_name = reader.ReadString((int)(nextLayerPosition - reader.Position)); // 'nm'

                    int? array_data_length = null;

                    if (result.Version >= 1)
                        array_data_length = reader.ReadInt32();

                    long position_before_array = reader.Position;

                    Debug.WriteLine($"Deserializing vertex array '{array_name}', number of items to extract = {number_of_items}...", Trace.Name);

                    switch (array_name)
                    {
                        case "pos":
                            // @ResName("pos"), class VertexDecode
                            {
                                Debug.Assert(result.Vertexes == null, $"Array Name '{array_name}' appeared more than once in the same resource!");
                                result.Vertexes = ReadVertexBufferResource2ArrayOfSingle(reader, 3, number_of_items);
                            }
                            break;
                        case "nrm":
                            // @ResName("nrm"), class NormalDecode
                            {
                                Debug.Assert(result.Normals == null, $"Array Name '{array_name}' appeared more than once in the same resource!");
                                result.Normals = ReadVertexBufferResource2ArrayOfSingle(reader, 3, number_of_items);
                            }
                            break;
                        case "col":
                            // @ResName("col"), class ColorDecode
                            {
                                Debug.Assert(result.Colors == null, $"Array Name '{array_name}' appeared more than once in the same resource!");
                                result.Colors = ReadVertexBufferResource2ArrayOfSingle(reader, 4, number_of_items);
                            }
                            break;
                        case "tex":
                            // @ResName("tex"), class TexelDecode
                            {
                                Debug.Assert(result.Texels == null, $"Array Name '{array_name}' appeared more than once in the same resource!");
                                result.Texels = ReadVertexBufferResource2ArrayOfSingle(reader, 2, number_of_items);
                            }
                            break;
                        case "bones":
                            // @VertexBuf.ResName("bones"), class $Res
                            {
                                Debug.Assert(result.Bones == null || result.Bones.Length == 0, $"Array Name '{array_name}' appeared more than once in the same resource!");
                                result.Bones = ReadVertexBufferResource2ArrayOfBones(reader, number_of_items);
                            }
                            break;
                        case "otex":
                            // @VertexBuf.ResName("otex"), class CDecode
                            {
                                Debug.Assert(result.OverTexes == null, $"Array Name '{array_name}' appeared more than once in the same resource!");
                                result.OverTexes = ReadVertexBufferResource2ArrayOfSingle(reader, 2, number_of_items);
                            }
                            break;
                        case "tan":
                            // @VertexBuf.ResName("tan"), class TanDecode
                            {
                                Debug.Assert(result.Tangents == null, $"Array Name '{array_name}' appeared more than once in the same resource!");
                                result.Tangents = ReadVertexBufferResource2ArrayOfSingle(reader, 3, number_of_items);
                            }
                            break;
                        case "bit":
                            // @VertexBuf.ResName("bit"), class BitDecode
                            {
                                Debug.Assert(result.Bitangents == null, $"Array Name '{array_name}' appeared more than once in the same resource!");
                                result.Bitangents = ReadVertexBufferResource2ArrayOfSingle(reader, 3, number_of_items);
                            }
                            break;
                        default:
                            throw new NotImplementedException($"Unexpected Vertex Buffer array name {array_name}!");
                    }

                    if (array_data_length.HasValue)
                    {
                        if (reader.Position > position_before_array + array_data_length.Value)
                            throw new Exception($"Excess data has beed read from stream and deserialized {nameof(VertexBufferResourceLayer2)} vertex array name {array_name}! Length of data taken is {reader.Position - position_before_array} bytes, but declared array size was {array_data_length} bytes. Check corresponding {nameof(HavenResourceFormatter.Deserialize)} code.");

                        if (reader.Position < position_before_array + array_data_length.Value)
                        {
                            Trace.TraceError($"Not all data has beed read from stream and deserialized for {nameof(VertexBufferResourceLayer2)} vertex array name {array_name}! Length of data taken is {reader.Position - position_before_array} bytes, but declared array size was {array_data_length} bytes. Check corresponding {nameof(HavenResourceFormatter.Deserialize)} code.");
                            Debugger.Break();
                            reader.Position = position_before_array + array_data_length.Value;
                        }
                    }
                }

                return result;
            });
        }

        /// <param name="numberOfItemsInArray">'num', 'nv'</param>
        private float[,] ReadVertexBufferResource1ArrayOfSingle(IMessageBinaryReader reader, byte numberOfArrayDimensions, ushort numberOfItemsInArray)
        {
            return ReadVertexBufferArray<float>(reader, numberOfArrayDimensions, numberOfItemsInArray, (r) => (float)r.ReadDouble20bit());
        }

        /// <param name="numberOfItemsInArray">'num', 'nv'</param>
        private float[,] ReadVertexBufferResource2ArrayOfSingle(IMessageBinaryReader reader, byte numberOfArrayDimensions, ushort numberOfItemsInArray)
        {
            return ReadVertexBufferArray<float>(reader, numberOfArrayDimensions, numberOfItemsInArray, (r) => r.ReadSingle());
        }

        private T[,] ReadVertexBufferArray<T>(IMessageBinaryReader reader, byte numberOfArrayDimensions, ushort numberOfItemsInArray, Func<IMessageBinaryReader, T> ReadValue)
        {
            T[,] result = new T[numberOfItemsInArray, numberOfArrayDimensions];

            for (int i = 0; i < numberOfItemsInArray; i++)
            {
                for (int dimension = 0; dimension < numberOfArrayDimensions; dimension++)
                {
                    result[i, dimension] = ReadValue(reader);
                }
            }

            return result;
        }

        /// <param name="numberOfItemsInArray">'num', 'nv'</param>
        private VertexBufferResourceLayer_Bone[] ReadVertexBufferResource1ArrayOfBones(IMessageBinaryReader reader, ushort numberOfItemsInArray)
        {
            return ReadVertexBufferBones(reader, numberOfItemsInArray, (r) => (float)r.ReadDouble20bit());
        }

        /// <param name="numberOfItemsInArray">'num', 'nv'</param>
        private VertexBufferResourceLayer_Bone[] ReadVertexBufferResource2ArrayOfBones(IMessageBinaryReader reader, ushort numberOfItemsInArray)
        {
            return ReadVertexBufferBones(reader, numberOfItemsInArray, (r) => (float)r.ReadSingle());
        }

        /// <param name="num">numberOfItemsInArray</param>
        private VertexBufferResourceLayer_Bone[] ReadVertexBufferBones(IMessageBinaryReader reader, ushort num, Func<IMessageBinaryReader, float> ReadWeightValue)
        {
            List<VertexBufferResourceLayer_Bone> result = new List<VertexBufferResourceLayer_Bone>();

            ushort nv = num; // 'nv' = Number of Vertexes

            byte mba = reader.ReadByte(); // 'mba' = Maximum Bones Array (to restrict bone-to-vertex nodes for processing, performance)

            /*
            int[] ba = new int[nv * mba]; // 'ba' = Bones Array

            float[] bw = new float[nv * mba]; // 'bw' = Bone Weights
            byte[] na = new byte[nv]; // array to keep count times vertexes mentioned, to restrict processing by 'mba' if needed.
            */

            // bone_index
            int bidx = 0; // 'bidx' = a Bone InDeX

            for (string bone_name = reader.ReadString((int)(reader.Length - reader.Position)); !String.IsNullOrEmpty(bone_name); bone_name = reader.ReadString((int)(reader.Length - reader.Position)), bidx++) // read bone name, stop when bone name is empty, compute bone index
            {
                VertexBufferResourceLayer_Bone bone = new VertexBufferResourceLayer_Bone()
                {
                    BoneIndex = bidx,
                    BoneName = bone_name,
                    mba = mba
                };

                List<VertexBufferResourceLayer_Bone.VertexWeight[]> runs = new List<VertexBufferResourceLayer_Bone.VertexWeight[]>();

                // 'run' = a sequence if vertex-weight records, vertex indexes are sequential within a 'run', length if all runs may be shorten indirectly by 'mba'.
                // 'vn' = 1st in the 'run' Vertex Number
                for (ushort run = reader.ReadUInt16(), vn = reader.ReadUInt16(); run != 0; run = reader.ReadUInt16(), vn = reader.ReadUInt16())
                {
                    List<VertexBufferResourceLayer_Bone.VertexWeight> this_run = new List<VertexBufferResourceLayer_Bone.VertexWeight>();

                    for (ushort i = 0; i < run; i++, vn++)
                    {
                        float weight = ReadWeightValue(reader); // 'w' = bone-vertex Weight

                        /*
                        int cna = na[vn]++; // cna = na[vn]++
                        if (cna > mba) // 'cna >= mba'
                            continue;

                        int vertex_index = vn * mba + cna; // 'vn', 'vn * mba + cna'

                        bw[vertex_index] = weight;
                        ba[vertex_index] = bidx;
                        */

                        VertexBufferResourceLayer_Bone.VertexWeight vertex_weight = new VertexBufferResourceLayer_Bone.VertexWeight()
                        {
                            VertexIndex = vn,
                            Weight = weight
                        };

                        this_run.Add(vertex_weight);
                    }

                    runs.Add(this_run.ToArray());
                }

                bone.Runs = runs.ToArray();

                result.Add(bone);
            }

            return result.ToArray();
        }

        protected void Deserialize(IMessageBinaryReader reader, out SkeletonResourceLayer resource)
        {
            resource = ExtractResourceFromLayer(reader, (nextLayerPosition) =>
            {
                SkeletonResourceLayer result = new SkeletonResourceLayer();

                string trace_dump_message = @"Deserialized skeleton bone:";

                do
                {
                    SkeletonResourceLayer.Bone bone = new SkeletonResourceLayer.Bone()
                    {
                        Name = reader.ReadString((int)(nextLayerPosition - reader.Position)),
                        Position = new Coord3d(x: reader.ReadDouble20bit(), y: reader.ReadDouble20bit(), z: reader.ReadDouble20bit()),
                        RotationAxis = new Coord3d(x: reader.ReadDouble20bit(), y: reader.ReadDouble20bit(), z: reader.ReadDouble20bit()),
                        RotationAngle = reader.ReadDouble20bit(),
                        ParentBoneName = reader.ReadString((int)(nextLayerPosition - reader.Position))
                    };
                    Trace.Dump(TraceEventType.Verbose, trace_dump_message, bone);
                    result.Bones.Add(bone);
                } while (reader.Position < nextLayerPosition);

                return result;
            });
        }

        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1041
        /// </remarks>
        protected void Deserialize(IMessageBinaryReader reader, out NegResourceLayer resource)
        {
            resource = ExtractResourceFromLayer(reader, (nextLayerPosition) =>
            {
                NegResourceLayer result = new NegResourceLayer()
                {
                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L794
                    CC = new Point(x: reader.ReadInt16(), y: reader.ReadInt16()),

                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1044
                    EP = new Point[8][]
                };

                // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1043
                byte[] skipped_bytes = reader.ReadBytes(12);

                byte en = reader.ReadByte();

                for (int i = 0; i < en; i++)
                {
                    byte epid = reader.ReadByte();
                    ushort cn = reader.ReadUInt16();
                    result.EP[epid] = new Point[cn];

                    for (int o = 0; o < cn; o++)
                    {
                        result.EP[epid][o] = new Point(x: reader.ReadInt16(), y: reader.ReadInt16());
                    }
                }

                return result;
            });
        }

        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L915
        /// </remarks>
        protected void Deserialize(IMessageBinaryReader reader, out ImageResourceLayer resource)
        {
            resource = ExtractResourceFromLayer(reader, (nextLayerPosition) =>
            {
                ImageResourceLayer result = new ImageResourceLayer()
                {
                    Z = reader.ReadInt16(),
                    SubZ = reader.ReadInt16(),
                    Flags = (ImageResourceLayer.FLAGS)reader.ReadByte(),
                    ID = reader.ReadInt16(),
                    O = new Point(x: reader.ReadInt16(), y: reader.ReadInt16()),

                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L925-L943
                    // TODO: *** NEXT
                    Image = reader.ReadBytes((int)(nextLayerPosition - reader.Position))
                };

                return result;
            });
        }

        protected void Deserialize(IMessageBinaryReader reader, out RenderLinkResourceLayer resource)
        {
            resource = ExtractResourceFromLayer(reader, (nextLayerPosition) =>
            {
                byte header = reader.ReadByte(); // 'lver'
                short? layer_id = null; // 'id'

                RenderLinkResourceLayer.TYPE type = (RenderLinkResourceLayer.TYPE)header;

                if (header == 3)
                {
                    layer_id = reader.ReadInt16();
                    type = (RenderLinkResourceLayer.TYPE)reader.ReadByte();
                }
                else
                    throw new NotImplementedException($"Unexpected {nameof(RenderLinkResourceLayer)} version {header}!");

                RenderLinkResourceLayer result;

                switch (type)
                {
                    case RenderLinkResourceLayer.TYPE.MeshAndMaterial:
                        {
                            result = new RenderLinkResourceLayer.MeshAndMaterial()
                            {
                                LayerID = layer_id,
                                MeshName = reader.ReadString((int)(nextLayerPosition - reader.Position)),
                                MeshVersion = reader.ReadUInt16(),
                                MeshID = reader.ReadInt16(),
                                MaterialName = reader.ReadString((int)(nextLayerPosition - reader.Position)),
                                MaterialVersion = reader.ReadUInt16(),
                                MaterialID = reader.ReadInt16()
                            };
                        }
                        break;
                    case RenderLinkResourceLayer.TYPE.AmbienceAudio:
                        {
                            result = new RenderLinkResourceLayer.AmbienceAudio()
                            {
                                LayerID = layer_id,
                                Name = reader.ReadString((int)(nextLayerPosition - reader.Position)),
                                Version = reader.ReadUInt16()
                            };
                        }
                        break;
                    case RenderLinkResourceLayer.TYPE.Mesh:
                        {
                            result = new RenderLinkResourceLayer.Mesh()
                            {
                                LayerID = layer_id,
                                Name = reader.ReadString((int)(nextLayerPosition - reader.Position)),
                                Version = reader.ReadUInt16(),
                                MeshID = reader.ReadInt16()
                            };
                        }
                        break;
                    default:
                        throw new NotImplementedException($"Unexpected {nameof(RenderLinkResourceLayer)} type {type}!");
                }

                return result;
            });
        }

        protected void Deserialize(IMessageBinaryReader reader, out PaginaResourceLayer resource)
        {
            resource = ExtractResourceFromLayer(reader, (nextLayerPosition) =>
            {
                PaginaResourceLayer result = new PaginaResourceLayer()
                {
                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1091
                    Text = reader.ReadString((int)(nextLayerPosition - reader.Position))
                };

                return result;
            });
        }

        protected void Deserialize(IMessageBinaryReader reader, out SkeletonAnimationResourceLayer resource)
        {
            resource = ExtractResourceFromLayer(reader, (nextLayerPosition) =>
            {
                SkeletonAnimationResourceLayer result = new SkeletonAnimationResourceLayer()
                {
                    ID = reader.ReadInt16()
                };

                SkeletonAnimationResourceLayer.FLAGS flags = (SkeletonAnimationResourceLayer.FLAGS)reader.ReadByte();

                result.Mode = (SkeletonAnimationResourceLayer.AnimationMode)reader.ReadByte();
                result.Diration = reader.ReadDouble20bit();

                if (flags.HasFlag(SkeletonAnimationResourceLayer.FLAGS.SpeedSpecified))
                {
                    result.Speed = reader.ReadDouble20bit();
                }

                while (reader.Position < nextLayerPosition)
                {
                    string bone_name = reader.ReadString((int)(nextLayerPosition - reader.Position)); // 'bnm'

                    if (@"{ctl}".Equals(bone_name))
                    {
                        ushort length = reader.ReadUInt16();
                        List<SkeletonAnimationResourceLayer.Effect.Event> events = new List<SkeletonAnimationResourceLayer.Effect.Event>(length);

                        for (int i = 0; i < length; i++)
                        {
                            double time = reader.ReadDouble20bit();
                            SkeletonAnimationResourceLayer.Effect.Event.TYPE type = (SkeletonAnimationResourceLayer.Effect.Event.TYPE)reader.ReadByte();

                            switch (type)
                            {
                                case SkeletonAnimationResourceLayer.Effect.Event.TYPE.SpawnSprite:
                                    {
                                        var sprite = new SkeletonAnimationResourceLayer.Effect.Event.SpawnSprite()
                                        {
                                            Time = time,
                                            ResourceName = reader.ReadString((int)(nextLayerPosition - reader.Position)),
                                            ResourceVersion = reader.ReadUInt16(),
                                            Data = reader.ReadBytes(reader.ReadByte())
                                        };

                                        events.Add(sprite);
                                    }
                                    break;
                                case SkeletonAnimationResourceLayer.Effect.Event.TYPE.Trigger:
                                    {
                                        var trigger = new SkeletonAnimationResourceLayer.Effect.Event.Trigger()
                                        {
                                            Time = time,
                                            ID = reader.ReadString((int)(nextLayerPosition - reader.Position))
                                        };

                                        events.Add(trigger);
                                    }
                                    break;
                                default:
                                    throw new NotImplementedException($"Unexpected {nameof(SkeletonAnimationResourceLayer)} event {nameof(SkeletonAnimationResourceLayer.Effect)} type {type}!");
                            }
                        }

                        result.Effects.Add(new SkeletonAnimationResourceLayer.Effect()
                        {
                            Events = events.ToArray()
                        });
                    }
                    else
                    {
                        ushort length = reader.ReadUInt16();
                        List<SkeletonAnimationResourceLayer.Track.Frame> frames = new List<SkeletonAnimationResourceLayer.Track.Frame>(length);

                        for (int i = 0; i < length; i++)
                        {
                            SkeletonAnimationResourceLayer.Track.Frame frame = new SkeletonAnimationResourceLayer.Track.Frame()
                            {
                                Time = reader.ReadDouble20bit(),
                                trans = new Coord3d
                                (
                                    reader.ReadDouble20bit(),
                                    reader.ReadDouble20bit(),
                                    reader.ReadDouble20bit()
                                ),
                                RotationAngle = Angle.FromRadians(reader.ReadDouble20bit()),
                                RotationAxis = new Coord3d
                                (
                                    reader.ReadDouble20bit(),
                                    reader.ReadDouble20bit(),
                                    reader.ReadDouble20bit()
                                )
                            };

                            frames.Add(frame);
                        }

                        result.Tracks.Add(new SkeletonAnimationResourceLayer.Track()
                        {
                            BoneName = bone_name,
                            Frames = frames.ToArray()
                        });
                    }
                }

                return result;
            });
        }

        protected void Deserialize(IMessageBinaryReader reader, out MeshAnimationResourceLayer resource)
        {
            resource = ExtractResourceFromLayer(reader, (nextLayerPosition) =>
            {
                byte version = reader.ReadByte(); // 'ver'

                MeshAnimationResourceLayer result = new MeshAnimationResourceLayer();

                switch (version)
                {
                    case 1:
                        {
                            result.ID = reader.ReadInt16();
                            result.rnd = reader.ReadByte() != 0;

                            float len = reader.ReadSingle(); // 'len'

                            for (byte frame_format = reader.ReadByte(); frame_format != 0; frame_format = reader.ReadByte()) // 't'
                            {
                                MeshAnimationResourceLayer.Frame frame = new MeshAnimationResourceLayer.Frame()
                                {
                                    Time = reader.ReadSingle()
                                };

                                ushort n = reader.ReadUInt16();

                                for (int i = 0; i < n;)
                                {
                                    ushort st = reader.ReadUInt16();
                                    ushort run = reader.ReadUInt16();

                                    for (int o = 0; o < run; o++, i++)
                                    {
                                        frame.idx[i] = st + o;

                                        switch (frame_format)
                                        {
                                            case 1:
                                                {
                                                    frame.Runs.Add(new MeshAnimationResourceLayer.Frame.Run
                                                    {
                                                        pos = reader.ReadCoord3f(),
                                                        nrm = reader.ReadCoord3f()
                                                    });
                                                }
                                                break;
                                            case 2:
                                                {
                                                    frame.Runs.Add(new MeshAnimationResourceLayer.Frame.Run
                                                    {
                                                        pos = reader.ReadCoord3f32bit(),
                                                        nrm = new Coord3f(0, 0, 0)
                                                    });
                                                }
                                                break;
                                            case 3:
                                                {
                                                    frame.Runs.Add(new MeshAnimationResourceLayer.Frame.Run
                                                    {
                                                        pos = reader.ReadCoord3f16bit(),
                                                        nrm = new Coord3f(0, 0, 0)
                                                    });
                                                }
                                                break;
                                            default:
                                                throw new NotImplementedException($"Unexpected {nameof(MeshAnimationResourceLayer)} frame format!");
                                        }
                                    }
                                }

                                result.Frames.Add(frame);
                            }
                        }
                        break;
                    default:
                        throw new NotImplementedException($"Unexpected {nameof(MeshAnimationResourceLayer)} version {version}!");
                }

                return result;
            });
        }

        protected void Deserialize(IMessageBinaryReader reader, out TooltipResourceLayer resource)
        {
            resource = ExtractResourceFromLayer(reader, (nextLayerPosition) =>
            {
                TooltipResourceLayer result = new TooltipResourceLayer()
                {
                    Text = reader.ReadString((int)(nextLayerPosition - reader.Position))
                };

                return result;
            });
        }

        protected void Deserialize(IMessageBinaryReader reader, out LightResourceLayer resource)
        {
            resource = ExtractResourceFromLayer(reader, (nextLayerPosition) =>
            {
                LightResourceLayer result = new LightResourceLayer()
                {
                    ID = reader.ReadInt16(),
                    Ambient = reader.ReadColor20bit(),
                    Diffuse = reader.ReadColor20bit(),
                    Specular = reader.ReadColor20bit()
                };

                while (reader.Position < nextLayerPosition)
                {
                    LightResourceLayer.DataType type = (LightResourceLayer.DataType)reader.ReadByte(); // 't'

                    switch (type)
                    {
                        case LightResourceLayer.DataType.Attenuation:
                            result.Attenuation = new LightResourceLayer.LightAttenuation()
                            {
                                Constant = (float)reader.ReadDouble20bit(),
                                Linear = (float)reader.ReadDouble20bit(),
                                Quadratic = (float)reader.ReadDouble20bit()
                            };
                            break;
                        case LightResourceLayer.DataType.Direction:
                            result.SpotDirection = reader.ReadCoord3f20bit();
                            break;
                        case LightResourceLayer.DataType.Exponent:
                            result.SpotExponent = (float)reader.ReadDouble20bit();
                            break;
                        default:
                            throw new NotImplementedException($"Unexpected light data type '{type}'!");
                    }
                }

                return result;
            });
        }
    }
}
