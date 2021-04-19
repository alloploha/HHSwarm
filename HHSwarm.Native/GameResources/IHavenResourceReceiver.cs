using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    public interface IHavenResourceReceiver
    {
        void Receive(JavaClassResourceLayer resource);
        void Receive(JavaClassEntryResourceLayer resource);
        void Receive(TextureResourceLayer resource);
        void Receive(MaterialResourceLayer2 resource);
        void Receive(BoneOffsetResourceLayer resource);
        void Receive(MeshResourceLayer resource);
        void Receive(VertexBufferResourceLayer2 resource);
        void Receive(SkeletonResourceLayer resource);
        void Receive(NegResourceLayer resource);
        void Receive(ImageResourceLayer resource);
        void ResourceStreamSignature(byte[] signature);
        void ResourceStreamVersion(ushort version);
        void Receive(RenderLinkResourceLayer resource);
        void Receive(SkeletonAnimationResourceLayer resource);
        void Receive(MeshAnimationResourceLayer resource);
        void Receive(VertexBufferResourceLayer1 resource);
        void Receive(TooltipResourceLayer resource);
        void Receive(LightResourceLayer resource);
        void Receive(PaginaResourceLayer resource);
        void Receive(AnimationResourceLayer resource);
        void Receive(ActionButtonResourceLayer resource);
        void Receive(JavaSourceCodeResourceLayer resource);
    }
}
