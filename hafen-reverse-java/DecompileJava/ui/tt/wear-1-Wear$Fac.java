import haven.ItemInfo;
import haven.ItemInfo.InfoFactory;
import haven.ItemInfo.Owner;

public class Wear$Fac
  implements ItemInfo.InfoFactory
{
  public ItemInfo build(ItemInfo.Owner paramOwner, Object... paramVarArgs)
  {
    return new Wear(paramOwner, ((Integer)paramVarArgs[1]).intValue(), ((Integer)paramVarArgs[2]).intValue());
  }
}
