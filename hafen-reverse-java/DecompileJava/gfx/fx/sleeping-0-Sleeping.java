import haven.Composited;
import haven.Composited.ED;
import haven.Composited.MD;
import haven.Composited.Poses;
import haven.Coord3f;
import haven.Indir;
import haven.Message;
import haven.MessageBuf;
import haven.RenderList;
import haven.ResData;
import haven.Resource;
import haven.Resource.Named;
import haven.Resource.Pool;
import haven.Resource.Resolver;
import haven.Skeleton;
import haven.Skeleton.ModOwner;
import haven.Skeleton.PoseMod;
import haven.Skeleton.Res;
import haven.Sprite;
import haven.Sprite.Owner;
import java.util.Arrays;

public class Sleeping
  extends Sprite
{
  public final Composited comp;
  
  public Sleeping(Sprite.Owner paramOwner, Resource paramResource, Message paramMessage)
  {
    super(paramOwner, paramResource);
    Indir localIndir = ((Resource.Resolver)paramOwner.context(Resource.Resolver.class)).getres(paramMessage.uint16());
    Skeleton localSkeleton = ((Skeleton.Res)((Resource)Resource.classres(Sleeping.class).pool.load("gfx/borka/body", 23).get()).layer(Skeleton.Res.class)).s;
    this.comp = new Composited(localSkeleton);
    Skeleton.PoseMod localPoseMod = this.comp.skel.mkposemod(Skeleton.ModOwner.nil, (Resource)Resource.classres(Sleeping.class).pool.load("gfx/borka/sleeping", 3).get(), new MessageBuf(new byte[] { -1 }));
    
    localPoseMod.age(); Composited 
      tmp137_134 = this.comp;tmp137_134.getClass();new Composited.Poses(tmp137_134, Arrays.asList(new Skeleton.PoseMod[] { localPoseMod })).set(0.0F);
    this.comp.chmod(Arrays.asList(new Composited.MD[] { new Composited.MD(localIndir, Arrays.asList(new ResData[] { new ResData(localIndir, Message.nil) })) }));
    this.comp.chequ(Arrays.asList(new Composited.ED[] { new Composited.ED(0, "h", new ResData(Resource.classres(Sleeping.class).pool.load("gfx/terobjs/items/nightcap", 1), Message.nil), Coord3f.o) }));
  }
  
  public boolean setup(RenderList paramRenderList)
  {
    paramRenderList.add(this.comp, null);
    return false;
  }
}
