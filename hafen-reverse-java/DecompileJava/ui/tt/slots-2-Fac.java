import haven.Indir;
import haven.ItemInfo;
import haven.ItemInfo.InfoFactory;
import haven.ItemInfo.Owner;
import haven.ItemInfo.Raw;
import haven.Message;
import haven.MessageBuf;
import haven.ResData;
import haven.Resource;
import haven.Resource.Resolver;
import java.util.Collection;
import java.util.LinkedList;
import java.util.List;

public class Fac
  implements ItemInfo.InfoFactory
{
  public ItemInfo build(ItemInfo.Owner paramOwner, ItemInfo.Raw paramRaw, Object... paramVarArgs)
  {
    Resource.Resolver localResolver = (Resource.Resolver)paramOwner.context(Resource.Resolver.class);
    int i = 1;
    double d1 = ((Number)paramVarArgs[(i++)]).doubleValue();
    double d2 = ((Number)paramVarArgs[(i++)]).doubleValue();
    LinkedList localLinkedList = new LinkedList();
    while (paramVarArgs[i] != null) {
      localLinkedList.add(localResolver.getres(((Integer)paramVarArgs[(i++)]).intValue()).get());
    }
    i++;
    int j = ((Integer)paramVarArgs[(i++)]).intValue();
    ISlots localISlots = new ISlots(paramOwner, j, d1, d2, (Resource[])localLinkedList.toArray(new Resource[0]));
    Indir localIndir;
    Object localObject;
    Object[] arrayOfObject;
    for (; i < paramVarArgs.length; localISlots.s.add(new ISlots.SItem(tmp238_236, new ResData(localIndir, (Message)localObject), arrayOfObject)))
    {
      localIndir = localResolver.getres(((Integer)paramVarArgs[(i++)]).intValue());
      localObject = Message.nil;
      if ((paramVarArgs[i] instanceof byte[])) {
        localObject = new MessageBuf((byte[])paramVarArgs[(i++)]);
      }
      arrayOfObject = (Object[])paramVarArgs[(i++)];
      localISlots.getClass();
    }
    return localISlots;
  }
}
