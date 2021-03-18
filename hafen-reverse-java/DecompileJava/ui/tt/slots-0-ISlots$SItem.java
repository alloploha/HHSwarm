import haven.CharWnd;
import haven.CompImage;
import haven.Coord;
import haven.GSprite;
import haven.GSprite.ImageSprite;
import haven.Indir;
import haven.ItemInfo;
import haven.ItemInfo.Layout;
import haven.PUtils;
import haven.ResData;
import haven.Resource;
import haven.Resource.Image;
import haven.Text;
import haven.Text.Line;
import haven.Utils;
import haven.res.lib.tspec.Spec;
import java.awt.image.BufferedImage;
import java.util.List;

public class ISlots$SItem
{
  public final Resource res;
  public final GSprite spr;
  public final List<ItemInfo> info;
  public final String name;
  
  public ISlots$SItem(ISlots paramISlots, ResData paramResData, Object[] paramArrayOfObject)
  {
    this.res = ((Resource)paramResData.res.get());
    Spec localSpec1 = new Spec(paramResData, paramISlots.owner, Utils.extend(new Object[] { ISlots.defn }, paramArrayOfObject));
    this.spr = localSpec1.spr();
    this.name = localSpec1.name();
    Spec localSpec2 = new Spec(paramResData, paramISlots.owner, paramArrayOfObject);
    this.info = localSpec2.info();
  }
  
  private BufferedImage img()
  {
    if ((this.spr instanceof GSprite.ImageSprite)) {
      return ((GSprite.ImageSprite)this.spr).image();
    }
    return ((Resource.Image)this.res.layer(Resource.imgc)).img;
  }
  
  public void layout(ItemInfo.Layout paramLayout)
  {
    BufferedImage localBufferedImage1 = PUtils.convolvedown(img(), new Coord(16, 16), CharWnd.iconfilter);
    BufferedImage localBufferedImage2 = Text.render(this.name).img;
    BufferedImage localBufferedImage3 = ItemInfo.longtip(this.info);
    int i = 10;int j = paramLayout.cmp.sz.y;
    paramLayout.cmp.add(localBufferedImage1, new Coord(i, j));
    paramLayout.cmp.add(localBufferedImage2, new Coord(i + 16 + 3, j + (16 - localBufferedImage2.getHeight()) / 2));
    if (localBufferedImage3 != null) {
      paramLayout.cmp.add(localBufferedImage3, new Coord(i + 16, j + 16));
    }
  }
}
