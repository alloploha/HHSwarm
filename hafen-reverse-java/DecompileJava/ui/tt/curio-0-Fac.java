import haven.ItemInfo;
import haven.ItemInfo.InfoFactory;
import haven.ItemInfo.Owner;
import haven.resutil.Curiosity;

public class Fac
  implements ItemInfo.InfoFactory
{
  public ItemInfo build(ItemInfo.Owner paramOwner, Object... paramVarArgs)
  {
    int i = ((Number)paramVarArgs[1]).intValue();
    int j = ((Number)paramVarArgs[2]).intValue();
    int k = ((Number)paramVarArgs[3]).intValue();
    int m = ((Number)paramVarArgs[4]).intValue();
    return new Curiosity(paramOwner, i, j, k, m);
  }
}
