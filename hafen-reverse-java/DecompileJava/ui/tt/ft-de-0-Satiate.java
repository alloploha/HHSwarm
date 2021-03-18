import haven.Indir;
import haven.ItemInfo;
import haven.ItemInfo.InfoFactory;
import haven.ItemInfo.Owner;
import haven.Resource.Resolver;

public class Satiate
  implements ItemInfo.InfoFactory
{
  public ItemInfo build(ItemInfo.Owner paramOwner, Object... paramVarArgs)
  {
    Indir localIndir = ((Resource.Resolver)paramOwner.context(Resource.Resolver.class)).getres(((Integer)paramVarArgs[1]).intValue());
    double d = ((Number)paramVarArgs[2]).doubleValue();
    return new Satiate.1(this, paramOwner, localIndir, d);
  }
}
