import haven.ItemInfo;
import haven.ItemInfo.InfoFactory;
import haven.ItemInfo.Owner;

public class Gast$Fac
  implements ItemInfo.InfoFactory
{
  public ItemInfo build(ItemInfo.Owner paramOwner, Object... paramVarArgs)
  {
    return new Gast(paramOwner, ((Number)paramVarArgs[1]).doubleValue(), ((Number)paramVarArgs[2]).doubleValue());
  }
}
