package haven.res.ui.tt.defn;

import haven.ItemInfo;
import haven.ItemInfo.InfoFactory;
import haven.ItemInfo.Name;
import haven.ItemInfo.Owner;
import haven.ItemInfo.ResOwner;
import haven.ItemInfo.SpriteOwner;
import haven.Resource;
import haven.Resource.Tooltip;

public class DefName
  implements ItemInfo.InfoFactory
{
  public ItemInfo build(ItemInfo.Owner paramOwner, Object... paramVarArgs)
  {
    if ((paramOwner instanceof ItemInfo.SpriteOwner))
    {
      localObject = ((ItemInfo.SpriteOwner)paramOwner).sprite();
      if ((localObject instanceof DynName)) {
        return new ItemInfo.Name(paramOwner, ((DynName)localObject).name());
      }
    }
    if (!(paramOwner instanceof ItemInfo.ResOwner)) {
      return null;
    }
    Object localObject = ((ItemInfo.ResOwner)paramOwner).resource();
    Resource.Tooltip localTooltip = (Resource.Tooltip)((Resource)localObject).layer(Resource.tooltip);
    if (localTooltip == null) {
      throw new RuntimeException("Item resource " + localObject + " is missing default tooltip");
    }
    return new ItemInfo.Name(paramOwner, localTooltip.t);
  }
}
