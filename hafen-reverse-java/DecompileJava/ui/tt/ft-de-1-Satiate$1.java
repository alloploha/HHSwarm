import haven.CharWnd.Constipations;
import haven.Coord;
import haven.Indir;
import haven.ItemInfo.Owner;
import haven.ItemInfo.Tip;
import haven.PUtils;
import haven.Resource;
import haven.Resource.Image;
import haven.Resource.Tooltip;
import haven.RichText;
import haven.Text;
import haven.Text.Line;
import java.awt.image.BufferedImage;

class Satiate$1
  extends ItemInfo.Tip
{
  Satiate$1(Satiate paramSatiate, ItemInfo.Owner paramOwner, Indir paramIndir, double paramDouble)
  {
    super(paramOwner);
  }
  
  public BufferedImage tipimg()
  {
    BufferedImage localBufferedImage1 = Text.render("Satiate ").img;
    int i = localBufferedImage1.getHeight();
    BufferedImage localBufferedImage2 = PUtils.convolvedown(((Resource.Image)((Resource)this.val$res.get()).layer(Resource.imgc)).img, new Coord(i, i), CharWnd.Constipations.tflt);
    BufferedImage localBufferedImage3 = RichText.render(String.format("%s by $col[255,128,128]{%d%%}", new Object[] { ((Resource.Tooltip)((Resource)this.val$res.get()).layer(Resource.tooltip)).t, Integer.valueOf((int)Math.round((1.0D - this.val$f) * 100.0D)) }), 0, new Object[0]).img;
    return catimgsh(0, new BufferedImage[] { localBufferedImage1, localBufferedImage2, localBufferedImage3 });
  }
}
