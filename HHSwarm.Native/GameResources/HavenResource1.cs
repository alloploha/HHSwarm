using HHSwarm.Native.GameResources.Graphics;
using HHSwarm.Native.Protocols.Hafen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    [Serializable]
    public class HavenResource1 : IHavenResourceReceiver
    {
        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Resource.java#L1528
        /// </remarks>
        internal static readonly byte[] FILE_SIGNATURE = Encoding.ASCII.GetBytes("Haven Resource 1");


        public readonly IList<JavaClassResourceLayer> JavaClasses = new List<JavaClassResourceLayer>();

        public void Receive(JavaClassResourceLayer resource)
        {
            JavaClasses.Add(resource);
        }

        public readonly IList<JavaClassEntryResourceLayer> JavaClassEntries = new List<JavaClassEntryResourceLayer>();

        public void Receive(JavaClassEntryResourceLayer resource)
        {
            JavaClassEntries.Add(resource);
        }

        public readonly IList<TextureResourceLayer> Textures = new List<TextureResourceLayer>();

        public void Receive(TextureResourceLayer resource)
        {
            Textures.Add(resource);
        }

        public readonly IList<MaterialResourceLayer2> Materials = new List<MaterialResourceLayer2>();

        public void Receive(MaterialResourceLayer2 resource)
        {
            Materials.Add(resource);
        }

        public readonly IList<BoneOffsetResourceLayer> BoneOffs = new List<BoneOffsetResourceLayer>();

        public void Receive(BoneOffsetResourceLayer resource)
        {
            BoneOffs.Add(resource);
        }

        public readonly IList<MeshResourceLayer> Meshes = new List<MeshResourceLayer>();

        public void Receive(MeshResourceLayer resource)
        {
            Meshes.Add(resource);
        }

        public readonly IList<VertexBufferResourceLayer1> VBuf1s = new List<VertexBufferResourceLayer1>();

        public void Receive(VertexBufferResourceLayer1 resource)
        {
            VBuf1s.Add(resource);
        }

        public readonly IList<VertexBufferResourceLayer2> VBuf2s = new List<VertexBufferResourceLayer2>();

        public void Receive(VertexBufferResourceLayer2 resource)
        {
            VBuf2s.Add(resource);
        }

        public readonly IList<SkeletonResourceLayer> Skels = new List<SkeletonResourceLayer>();

        public void Receive(SkeletonResourceLayer resource)
        {
            Skels.Add(resource);
        }

        public readonly IList<NegResourceLayer> Negs = new List<NegResourceLayer>();

        public void Receive(NegResourceLayer resource)
        {
            Negs.Add(resource);
        }

        public readonly IList<ImageResourceLayer> Images = new List<ImageResourceLayer>();

        public void Receive(ImageResourceLayer resource)
        {
            Images.Add(resource);
        }

        public byte[] Signature { get; private set; }

        public void ResourceStreamSignature(byte[] signature)
        {
            this.Signature = signature;
        }

        public ushort Version { get; private set; }

        public void ResourceStreamVersion(ushort version)
        {
            this.Version = version;
        }

        public readonly IList<RenderLinkResourceLayer> RenderLinks = new List<RenderLinkResourceLayer>();

        public void Receive(RenderLinkResourceLayer resource)
        {
            RenderLinks.Add(resource);
        }

        public readonly IList<SkeletonAnimationResourceLayer> SkeletonAnimations = new List<SkeletonAnimationResourceLayer>();

        public void Receive(SkeletonAnimationResourceLayer resource)
        {
            SkeletonAnimations.Add(resource);
        }

        public readonly IList<MeshAnimationResourceLayer> MeshAnimations = new List<MeshAnimationResourceLayer>();

        public void Receive(MeshAnimationResourceLayer resource)
        {
            MeshAnimations.Add(resource);
        }

        public readonly IList<TooltipResourceLayer> Tooltips = new List<TooltipResourceLayer>();

        public void Receive(TooltipResourceLayer resource)
        {
            Tooltips.Add(resource);
        }

        public readonly IList<LightResourceLayer> Lights = new List<LightResourceLayer>();

        public void Receive(LightResourceLayer resource)
        {
            Lights.Add(resource);
        }
    }
}
