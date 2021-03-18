package haven.res.lib.vmat;

import haven.GLState;
import haven.Gob;
import haven.Indir;
import haven.IntMap;
import haven.Material;
import haven.Material.Res;
import haven.Message;
import haven.Resource;
import haven.Resource.Resolver;
import haven.resutil.OverTex;
import java.util.Collections;
import java.util.Map;

public class Materials
  extends Mapping
{
  public static final Map<Integer, Material> empty = ;
  public final Map<Integer, Material> mats;
  
  public static Map<Integer, Material> decode(Resource.Resolver paramResolver, Message paramMessage)
  {
    IntMap localIntMap = new IntMap();
    int i = 0;
    while (!paramMessage.eom())
    {
      Indir localIndir = paramResolver.getres(paramMessage.uint16());
      int j = paramMessage.int8();
      Material.Res localRes;
      if (j >= 0) {
        localRes = (Material.Res)((Resource)localIndir.get()).layer(Material.Res.class, Integer.valueOf(j));
      } else {
        localRes = (Material.Res)((Resource)localIndir.get()).layer(Material.Res.class);
      }
      localIntMap.put(Integer.valueOf(i++), localRes.get());
    }
    return localIntMap;
  }
  
  public static Material stdmerge(Material paramMaterial1, Material paramMaterial2)
  {
    OverTex localOverTex = null;
    for (GLState localGLState : paramMaterial1.states) {
      if ((localGLState instanceof OverTex))
      {
        localOverTex = (OverTex)localGLState;
        break;
      }
    }
    if (localOverTex == null) {
      return paramMaterial2;
    }
    return new Material(new GLState[] { paramMaterial2, localOverTex });
  }
  
  public Material mergemat(Material paramMaterial, int paramInt)
  {
    if (!this.mats.containsKey(Integer.valueOf(paramInt))) {
      return paramMaterial;
    }
    Material localMaterial = (Material)this.mats.get(Integer.valueOf(paramInt));
    return stdmerge(paramMaterial, localMaterial);
  }
  
  public Materials(Map<Integer, Material> paramMap)
  {
    this.mats = paramMap;
  }
  
  public Materials(Gob paramGob, Message paramMessage)
  {
    this.mats = decode((Resource.Resolver)paramGob.context(Resource.Resolver.class), paramMessage);
  }
}
