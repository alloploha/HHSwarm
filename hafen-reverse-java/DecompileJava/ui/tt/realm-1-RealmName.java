import haven.ItemInfo;
import haven.ItemInfo.InfoFactory;
import haven.ItemInfo.Owner;

public class RealmName
  implements ItemInfo.InfoFactory
{
  public ItemInfo build(ItemInfo.Owner paramOwner, Object... paramVarArgs)
  {
    return new RealmName.1(this, paramOwner, "In " + (String)paramVarArgs[1]);
  }
}
