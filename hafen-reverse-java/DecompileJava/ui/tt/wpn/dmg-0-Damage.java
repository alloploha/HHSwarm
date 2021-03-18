import haven.ItemInfo.Owner;
import haven.res.ui.tt.wpn.info.WeaponInfo;

public class Damage
  extends WeaponInfo
{
  public final int dmg;
  
  public Damage(ItemInfo.Owner paramOwner, int paramInt)
  {
    super(paramOwner);
    this.dmg = paramInt;
  }
  
  public static Damage mkinfo(ItemInfo.Owner paramOwner, Object... paramVarArgs)
  {
    return new Damage(paramOwner, ((Number)paramVarArgs[1]).intValue());
  }
  
  public String wpntips()
  {
    return "Damage: " + this.dmg;
  }
  
  public int order()
  {
    return 50;
  }
}
