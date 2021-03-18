package haven.res.ui.tt.attrmod;

import haven.CharWnd;
import haven.Coord;
import haven.ItemInfo.Owner;
import haven.ItemInfo.Tip;
import haven.PUtils;
import haven.Resource;
import haven.Resource.Image;
import haven.Resource.Tooltip;
import haven.RichText;
import java.awt.image.BufferedImage;
import java.util.ArrayList;
import java.util.Collection;

public class AttrMod
  extends ItemInfo.Tip
{
  public AttrMod(ItemInfo.Owner paramOwner, Collection<AttrMod.Mod> paramCollection)
  {
    super(paramOwner);
    this.mods = paramCollection;
  }
  
  private static String debuff = "255,128,128";
  private static String buff = "128,255,128";
  public final Collection<AttrMod.Mod> mods;
  
  public static BufferedImage modimg(Collection<AttrMod.Mod> paramCollection)
  {
    ArrayList localArrayList = new ArrayList(paramCollection.size());
    for (AttrMod.Mod localMod : paramCollection)
    {
      BufferedImage localBufferedImage1 = RichText.render(String.format("%s $col[%s]{%s%d}", new Object[] { ((Resource.Tooltip)localMod.attr.layer(Resource.tooltip)).t, localMod.mod < 0 ? debuff : buff, 
        Character.valueOf(localMod.mod < 0 ? 45 : '+'), Integer.valueOf(Math.abs(localMod.mod)) }), 0, new Object[0]).img;
      
      BufferedImage localBufferedImage2 = PUtils.convolvedown(((Resource.Image)localMod.attr.layer(Resource.imgc)).img, new Coord(localBufferedImage1
        .getHeight(), localBufferedImage1.getHeight()), CharWnd.iconfilter);
      
      localArrayList.add(catimgsh(0, new BufferedImage[] { localBufferedImage2, localBufferedImage1 }));
    }
    return catimgs(0, (BufferedImage[])localArrayList.toArray(new BufferedImage[0]));
  }
  
  public BufferedImage tipimg()
  {
    return modimg(this.mods);
  }
}
