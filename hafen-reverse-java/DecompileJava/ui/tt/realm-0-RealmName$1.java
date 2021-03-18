import haven.Coord;
import haven.ItemInfo.Name;
import haven.ItemInfo.Owner;
import haven.PUtils;
import haven.TexI;
import haven.Text;
import java.awt.Graphics;
import java.awt.image.BufferedImage;

class RealmName$1
  extends ItemInfo.Name
{
  RealmName$1(RealmName paramRealmName, ItemInfo.Owner paramOwner, String paramString)
  {
    super(paramOwner, paramString);
  }
  
  public BufferedImage tipimg()
  {
    BufferedImage localBufferedImage = TexI.mkbuf(PUtils.imgsz(this.str.img).add(0, 10));
    Graphics localGraphics = localBufferedImage.getGraphics();
    localGraphics.drawImage(this.str.img, 0, 0, null);
    localGraphics.dispose();
    return localBufferedImage;
  }
}
