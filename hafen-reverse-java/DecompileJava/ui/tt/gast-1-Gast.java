import haven.ItemInfo.Owner;
import haven.ItemInfo.Tip;
import haven.RichText;
import haven.Utils;
import java.awt.image.BufferedImage;

public class Gast
  extends ItemInfo.Tip
{
  public final double glut;
  public final double fev;
  
  public Gast(ItemInfo.Owner paramOwner, double paramDouble1, double paramDouble2)
  {
    super(paramOwner);
    this.glut = paramDouble1;
    this.fev = paramDouble2;
  }
  
  public BufferedImage tipimg()
  {
    StringBuilder localStringBuilder = new StringBuilder();
    if (this.glut != 1.0D) {
      localStringBuilder.append(String.format("Hunger reduction: %s%%\n", new Object[] { Utils.odformat2(100.0D * this.glut, 1) }));
    }
    if (this.fev != 1.0D) {
      localStringBuilder.append(String.format("Food event bonus: %s%%\n", new Object[] { Utils.odformat2(100.0D * this.fev, 1) }));
    }
    return RichText.render(localStringBuilder.toString(), 0, new Object[0]).img;
  }
}
