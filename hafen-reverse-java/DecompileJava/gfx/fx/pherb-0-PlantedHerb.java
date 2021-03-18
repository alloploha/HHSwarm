import haven.FastMesh.MeshRes;
import haven.Indir;
import haven.Material;
import haven.Material.Res;
import haven.Message;
import haven.Resource;
import haven.Resource.Resolver;
import haven.Sprite;
import haven.Sprite.Factory;
import haven.Sprite.Owner;
import haven.StaticSprite;
import haven.res.lib.plants.GaussianPlant;

public class PlantedHerb
  implements Sprite.Factory
{
  public Sprite create(Sprite.Owner paramOwner, Resource paramResource, Message paramMessage)
  {
    Resource localResource = (Resource)((Resource.Resolver)paramOwner.context(Resource.Resolver.class)).getres(paramMessage.uint16()).get();
    Sprite.Factory localFactory = (Sprite.Factory)localResource.getcode(Sprite.Factory.class, false);
    if ((localFactory instanceof GaussianPlant))
    {
      FastMesh.MeshRes localMeshRes = (FastMesh.MeshRes)localResource.layer(FastMesh.MeshRes.class, Integer.valueOf(0));
      return new StaticSprite(paramOwner, localResource, localMeshRes.mat.get().apply(localMeshRes.m));
    }
    return Sprite.create(paramOwner, localResource, Message.nil);
  }
}
