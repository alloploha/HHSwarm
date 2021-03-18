using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HHSwarm.Native.GameResources.MaterialResourceLayer2;

namespace HHSwarm.Native.GameResources
{
    public interface IMaterialResourceLayer2PartsReceiver
    {
        void Receive(ColorsPart part);
        void Receive(TexPalPart part);
        void Receive(LightPart part);
        void Receive(OrderPart part);
        void Receive(TexPart part);
        void Receive(CelShadePart part);
        void Receive(MaterialLink part);
        void Receive(OverTex part);
        void Receive(ColorState part);
        void Receive(TexAnim part);
    }
}
