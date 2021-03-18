import haven.Indir;
import haven.ItemInfo;
import haven.ItemInfo.InfoFactory;
import haven.ItemInfo.Owner;
import haven.Resource;
import haven.Resource.Resolver;
import haven.resutil.FoodInfo;
import haven.resutil.FoodInfo.Effect;
import haven.resutil.FoodInfo.Event;
import java.util.Collection;
import java.util.LinkedList;

public class Fac
  implements ItemInfo.InfoFactory
{
  public ItemInfo build(ItemInfo.Owner paramOwner, Object... paramVarArgs)
  {
    double d1 = ((Float)paramVarArgs[1]).floatValue();
    double d2 = ((Float)paramVarArgs[2]).floatValue();
    Object[] arrayOfObject1 = (Object[])paramVarArgs[3];
    Object[] arrayOfObject2 = (Object[])paramVarArgs[4];
    Object[] arrayOfObject3 = (Object[])paramVarArgs[5];
    
    LinkedList localLinkedList1 = new LinkedList();
    LinkedList localLinkedList2 = new LinkedList();
    Resource.Resolver localResolver = (Resource.Resolver)paramOwner.context(Resource.Resolver.class);
    for (int i = 0; i < arrayOfObject1.length; i += 2) {
      localLinkedList1.add(new FoodInfo.Event((Resource)localResolver.getres(((Integer)arrayOfObject1[i]).intValue()).get(), ((Number)arrayOfObject1[(i + 1)])
        .doubleValue()));
    }
    for (i = 0; i < arrayOfObject2.length; i += 2) {
      localLinkedList2.add(new FoodInfo.Effect(ItemInfo.buildinfo(paramOwner, new Object[] { (Object[])(Object[])arrayOfObject2[i] }), ((Number)arrayOfObject2[(i + 1)])
        .doubleValue()));
    }
    int[] arrayOfInt2 = new int[arrayOfObject3.length * 32];
    int j = 0;int k = 0;
    for (int m = 0; m < arrayOfObject3.length; m++)
    {
      int n = 0;
      for (int i1 = 1; n < 32; k++)
      {
        if ((((Integer)arrayOfObject3[m]).intValue() & i1) != 0) {
          arrayOfInt2[(j++)] = k;
        }
        n++;i1 <<= 1;
      }
    }
    int[] arrayOfInt1 = new int[j];
    for (m = 0; m < j; m++) {
      arrayOfInt1[m] = arrayOfInt2[m];
    }
    return new FoodInfo(paramOwner, d1, d2, (FoodInfo.Event[])localLinkedList1.toArray(new FoodInfo.Event[0]), (FoodInfo.Effect[])localLinkedList2.toArray(new FoodInfo.Effect[0]), arrayOfInt1);
  }
}
