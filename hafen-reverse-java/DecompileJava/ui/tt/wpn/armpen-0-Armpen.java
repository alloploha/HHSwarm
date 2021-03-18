import haven.ItemInfo.Owner;
import haven.res.ui.tt.wpn.info.WeaponInfo;

public class Armpen
  extends WeaponInfo
{
  public final double deg;
  
  public Armpen(ItemInfo.Owner paramOwner, double paramDouble)
  {
    super(paramOwner);
    this.deg = paramDouble;
  }
  
  public static Armpen mkinfo(ItemInfo.Owner paramOwner, Object... paramVarArgs)
  {
    return new Armpen(paramOwner, ((Number)paramVarArgs[1]).doubleValue() * 0.01D);
  }
  
  public String wpntips()
  {
    return String.format("Armor penetration: %.1f%%", new Object[] { Double.valueOf(this.deg * 100.0D) });
  }
  
  public int order()
  {
    return 100;
  }
}
