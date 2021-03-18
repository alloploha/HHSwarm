import haven.CharWnd;
import haven.Coord;
import haven.Indir;
import haven.ItemInfo.Owner;
import haven.PUtils;
import haven.Resource;
import haven.Resource.Image;
import haven.Resource.Resolver;
import haven.Text;
import haven.Text.Line;
import haven.res.ui.tt.wpn.info.WeaponInfo;
import java.awt.image.BufferedImage;

public class Weight
  extends WeaponInfo
{
  public final Resource attr;
  
  public Weight(ItemInfo.Owner paramOwner, Resource paramResource)
  {
    super(paramOwner);
    this.attr = paramResource;
  }
  
  public static Weight mkinfo(ItemInfo.Owner paramOwner, Object... paramVarArgs)
  {
    return new Weight(paramOwner, (Resource)((Resource.Resolver)paramOwner.context(Resource.Resolver.class)).getres(((Integer)paramVarArgs[1]).intValue()).get());
  }
  
  public BufferedImage wpntip()
  {
    BufferedImage localBufferedImage1 = Text.render("Attack weight: ").img;
    BufferedImage localBufferedImage2 = PUtils.convolvedown(((Resource.Image)this.attr.layer(Resource.imgc)).img, new Coord(localBufferedImage1.getHeight(), localBufferedImage1.getHeight()), CharWnd.iconfilter);
    return catimgsh(0, new BufferedImage[] { localBufferedImage1, localBufferedImage2 });
  }
  
  public int order()
  {
    return 75;
  }
}
