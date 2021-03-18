using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    public class HavenResource1Formatter : HavenResourceFormatter
    {
        public HavenResource1Formatter() : base(Encoding.ASCII.GetBytes("Haven Resource 1"))
        {

        }

        protected override void Deserialize(BinaryReader reader, string resourceType, IHavenResourceReceiver receiver)
        {
            string trace_dump_message = $"Deserialized game resource of type {resourceType}";

            switch (resourceType)
            {
                case "code":
                    // @LayerName("code")
                    {
                        JavaClassResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "codeentry":
                    // @LayerName("codeentry")
                    {
                        JavaClassEntryResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "tex":
                    // @Resource.LayerName("tex")
                    {
                        TextureResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "mat2":
                    // @Resource.LayerName("mat2")
                    {
                        MaterialResourceLayer2 resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "boneoff":
                    // @Resource.LayerName("boneoff")
                    {
                        BoneOffsetResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "mesh":
                    // @Resource.LayerName("mesh")
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
                    {
                        VertexBufferResourceLayer2 resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "skel":
                    // @Resource.LayerName("skel")
                    {
                        SkeletonResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "neg":
                    // @LayerName("neg")
                    {
                        NegResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "image":
                    // @LayerName("image")
                    {
                        ImageResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "rlink":
                    // @Resource.LayerName("rlink")
                    {
                        RenderLinkResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "skan":
                    // @Resource.LayerName("skan"), class ResPose
                    {
                        SkeletonAnimationResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "obst":
                    // 
                    {
                        // TODO: Implement next! - "obst"
                        //byte[] data = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
                        int length = reader.ReadInt32();
                        byte[] data = reader.ReadBytes(length);
                        File.WriteAllBytes(@"C:\Temp\HHSwarm-ExtractedResources\-" + nameof(HavenResource1Formatter) + "-" + resourceType + DateTime.Now.Ticks + ".bin", data);

                        //Debugger.Break();
                    }
                    break;
                case "manim":
                    // @Resource.LayerName("manim"), MeshAnim
                    {
                        MeshAnimationResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "plparts":
                    {
                        // TODO: Implement next! - "plparts"
                        int length = reader.ReadInt32();
                        byte[] data = reader.ReadBytes(length);
                        File.WriteAllBytes(@"C:\Temp\HHSwarm-ExtractedResources\-" + nameof(HavenResource1Formatter) + "-" + resourceType + DateTime.Now.Ticks + ".bin", data);
                    }
                    break;
                case "tooltip":
                    // @LayerName("tooltip")
                    {
                        TooltipResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                case "light":
                    // @Resource.LayerName("light")
                    {
                        LightResourceLayer resource;
                        Deserialize(reader, out resource);
                        receiver.Receive(resource);
                    }
                    break;
                default:
                    {
#if DEBUG
                        Debugger.Break();
                        byte[] remainder = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
#endif
                        throw new NotImplementedException($"Unexpected resource type '{resourceType}'!");
                    }
            }
        }
    }
}
