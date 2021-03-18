package haven.res.ui.tt.attrmod;

import haven.Indir;
import haven.ItemInfo;
import haven.ItemInfo.InfoFactory;
import haven.ItemInfo.Owner;
import haven.Resource;
import haven.Resource.Resolver;
import java.util.ArrayList;
import java.util.Collection;

public class AttrMod$Fac
  implements ItemInfo.InfoFactory
{
  public ItemInfo build(ItemInfo.Owner paramOwner, Object... paramVarArgs)
  {
    Resource.Resolver localResolver = (Resource.Resolver)paramOwner.context(Resource.Resolver.class);
    ArrayList localArrayList = new ArrayList();
    for (int i = 1; i < paramVarArgs.length; i += 2) {
      localArrayList.add(new AttrMod.Mod((Resource)localResolver.getres(((Integer)paramVarArgs[i]).intValue()).get(), ((Integer)paramVarArgs[(i + 1)]).intValue()));
    }
    return new AttrMod(paramOwner, localArrayList);
  }
}
