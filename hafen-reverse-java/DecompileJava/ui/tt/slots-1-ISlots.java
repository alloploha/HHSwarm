import haven.CharWnd;
import haven.CompImage;
import haven.Coord;
import haven.GItem.NumberInfo;
import haven.ItemInfo.Layout;
import haven.ItemInfo.Owner;
import haven.ItemInfo.Tip;
import haven.Loading;
import haven.PUtils;
import haven.Resource;
import haven.Resource.Image;
import haven.Resource.Pool;
import haven.RichText;
import haven.Text;
import haven.Text.Foundry;
import haven.Text.Line;
import java.awt.Color;
import java.awt.Font;
import java.awt.image.BufferedImage;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Iterator;

public class ISlots
  extends ItemInfo.Tip
  implements GItem.NumberInfo
{
  public static final Text ch = Text.render("Gilding:");
  public static final Text.Foundry progf = new Text.Foundry(Text.dfont.deriveFont(2), new Color(0, 169, 224));
  public final Collection<ISlots.SItem> s = new ArrayList();
  public final int left;
  public final double pmin;
  public final double pmax;
  public final Resource[] attrs;
  public static final String chc = "192,192,255";
  
  public ISlots(ItemInfo.Owner paramOwner, int paramInt, double paramDouble1, double paramDouble2, Resource[] paramArrayOfResource)
  {
    super(paramOwner);
    this.left = paramInt;
    this.pmin = paramDouble1;
    this.pmax = paramDouble2;
    this.attrs = paramArrayOfResource;
  }
  
  public void layout(ItemInfo.Layout paramLayout)
  {
    paramLayout.cmp.add(ch.img, new Coord(0, paramLayout.cmp.sz.y));
    if (this.attrs.length > 0)
    {
      localObject = RichText.render(String.format("Chance: $col[%s]{%d%%} to $col[%s]{%d%%}", new Object[] { "192,192,255", Long.valueOf(Math.round(100.0D * this.pmin)), "192,192,255", Long.valueOf(Math.round(100.0D * this.pmax)) }), 0, new Object[0]).img;
      int i = ((BufferedImage)localObject).getHeight();
      int j = 10;int k = paramLayout.cmp.sz.y;
      paramLayout.cmp.add((BufferedImage)localObject, new Coord(j, k));
      j += ((BufferedImage)localObject).getWidth() + 10;
      for (int m = 0; m < this.attrs.length; m++)
      {
        BufferedImage localBufferedImage = PUtils.convolvedown(((Resource.Image)this.attrs[m].layer(Resource.imgc)).img, new Coord(i, i), CharWnd.iconfilter);
        paramLayout.cmp.add(localBufferedImage, new Coord(j, k));
        j += localBufferedImage.getWidth() + 2;
      }
    }
    else
    {
      localObject = RichText.render(String.format("Chance: $col[%s]{%d%%}", new Object[] { "192,192,255", Integer.valueOf((int)Math.round(100.0D * this.pmin)) }), 0, new Object[0]).img;
      paramLayout.cmp.add((BufferedImage)localObject, new Coord(10, paramLayout.cmp.sz.y));
    }
    for (Object localObject = this.s.iterator(); ((Iterator)localObject).hasNext();)
    {
      ISlots.SItem localSItem = (ISlots.SItem)((Iterator)localObject).next();
      localSItem.layout(paramLayout);
    }
    if (this.left > 0) {
      paramLayout.cmp.add(progf.render(this.left > 1 ? String.format("Gildable ×%d", new Object[] { Integer.valueOf(this.left) }) : "Gildable").img, new Coord(10, paramLayout.cmp.sz.y));
    }
  }
  
  public static final Object[] defn = { Loading.waitfor(Resource.classres(ISlots.class).pool.load("ui/tt/defn", 4)) };
  
  public int order()
  {
    return 200;
  }
  
  public int itemnum()
  {
    return this.s.size();
  }
  
  public static final Color avail = new Color(128, 192, 255);
  
  public Color numcolor()
  {
    return this.left > 0 ? avail : Color.WHITE;
  }
}
