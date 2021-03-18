import haven.ItemInfo;
import haven.ItemInfo.Owner;
import haven.ItemInfo.Tip;
import haven.Text;
import haven.Text.Line;
import java.awt.image.BufferedImage;

public class Armor
  extends ItemInfo.Tip
{
  public final int hard;
  public final int soft;
  
  public Armor(ItemInfo.Owner paramOwner, int paramInt1, int paramInt2)
  {
    super(paramOwner);
    this.hard = paramInt1;
    this.soft = paramInt2;
  }
  
  public static ItemInfo mkinfo(ItemInfo.Owner paramOwner, Object... paramVarArgs)
  {
    return new Armor(paramOwner, ((Integer)paramVarArgs[1]).intValue(), ((Integer)paramVarArgs[2]).intValue());
  }
  
  public BufferedImage tipimg()
  {
    return Text.render(String.format("Armor class: %,d/%,d", new Object[] { Integer.valueOf(this.hard), Integer.valueOf(this.soft) })).img;
  }
}
