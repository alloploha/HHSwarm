import haven.GLState;
import haven.Indir;
import haven.Material;
import haven.Material.Factory;
import haven.Material.Owner;
import haven.Message;
import haven.Resource;
import haven.Resource.Resolver;
import haven.TexGL;
import haven.TexR;

public class PalTex
  implements Material.Factory
{
  public Material create(Material.Owner paramOwner, Resource paramResource, Message paramMessage)
  {
    Resource.Resolver localResolver = (Resource.Resolver)paramOwner.context(Resource.Resolver.class);
    Resource localResource1 = (Resource)localResolver.getres(paramMessage.uint16()).get();
    Resource localResource2 = (Resource)localResolver.getres(paramMessage.uint16()).get();
    Material localMaterial = Material.fromres(paramOwner, localResource1, Message.nil);
    TexGL localTexGL = ((TexR)localResource2.layer(TexR.class)).tex();
    return new Material(new GLState[] { localMaterial, localTexGL.draw() });
  }
}
