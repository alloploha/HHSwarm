import haven.ItemInfo.Owner;
import haven.ItemInfo.Tip;
import haven.Text;
import haven.Text.Line;
import java.awt.image.BufferedImage;

public class Wear
  extends ItemInfo.Tip
{
  public final int d;
  public final int m;
  
  public Wear(ItemInfo.Owner paramOwner, int paramInt1, int paramInt2)
  {
    super(paramOwner);
    this.d = paramInt1;
    this.m = paramInt2;
  }
  
  public BufferedImage tipimg()
  {
    return Text.render(String.format("Wear: %,d/%,d", new Object[] { Integer.valueOf(this.d), Integer.valueOf(this.m) })).img;
  }
}
