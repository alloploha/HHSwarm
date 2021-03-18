import haven.Drawable;
import haven.GLState;
import haven.Glob;
import haven.Gob;
import haven.Indir;
import haven.Message;
import haven.MessageBuf;
import haven.RenderList;
import haven.Resource;
import haven.Session;
import haven.Skeleton.BoneOffset;
import haven.Sprite;
import haven.Sprite.Owner;

public class Equed
  extends Sprite
{
  private final Sprite espr;
  private final GLState ep;
  
  public Equed(Sprite.Owner paramOwner, Resource paramResource, Message paramMessage)
  {
    super(paramOwner, paramResource);
    Indir localIndir = ((Gob)paramOwner).glob.sess.getres(paramMessage.uint16());
    int i = paramMessage.uint8();
    String str = paramMessage.string();
    Resource localResource = (i & 0x1) == 0 ? paramOwner.getres() : (Resource)localIndir.get();
    Object localObject = Message.nil;
    if ((i & 0x2) != 0) {
      localObject = new MessageBuf(paramMessage.bytes(paramMessage.uint8()));
    }
    Skeleton.BoneOffset localBoneOffset = (Skeleton.BoneOffset)localResource.layer(Skeleton.BoneOffset.class, str);
    if (localBoneOffset == null) {
      throw new RuntimeException("No such bone-offset in " + localResource.name + ": " + str);
    }
    Drawable localDrawable = (Drawable)((Gob)paramOwner).getattr(Drawable.class);
    this.ep = localBoneOffset.forpose(localDrawable == null ? null : localDrawable.getpose());
    this.espr = Sprite.create(paramOwner, (Resource)localIndir.get(), (Message)localObject);
  }
  
  public boolean setup(RenderList paramRenderList)
  {
    paramRenderList.add(this.espr, this.ep);
    return false;
  }
  
  public boolean tick(int paramInt)
  {
    this.espr.tick(paramInt);
    return false;
  }
}
