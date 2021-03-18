import haven.ItemInfo;
import haven.ItemInfo.InfoFactory;
import haven.ItemInfo.Name;
import haven.ItemInfo.Owner;

public class CustName
  implements ItemInfo.InfoFactory
{
  public ItemInfo build(ItemInfo.Owner paramOwner, Object... paramVarArgs)
  {
    return new ItemInfo.Name(paramOwner, (String)paramVarArgs[1]);
  }
}
