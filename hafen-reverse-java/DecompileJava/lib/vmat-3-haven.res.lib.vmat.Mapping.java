package haven.res.lib.vmat;

import haven.FastMesh.MeshRes;
import haven.Gob.ResAttr;
import haven.Material;
import haven.Material.Res;
import haven.Rendered;
import haven.Resource;
import java.util.Collection;
import java.util.LinkedList;
import java.util.Map;

public abstract class Mapping
  extends Gob.ResAttr
{
  public abstract Material mergemat(Material paramMaterial, int paramInt);
  
  public Rendered[] apply(Resource paramResource)
  {
    LinkedList localLinkedList = new LinkedList();
    for (FastMesh.MeshRes localMeshRes : paramResource.layers(FastMesh.MeshRes.class))
    {
      String str = (String)localMeshRes.rdat.get("vm");
      int i = str == null ? -1 : Integer.parseInt(str);
      if (i >= 0) {
        localLinkedList.add(mergemat(localMeshRes.mat.get(), i).apply(localMeshRes.m));
      } else if (localMeshRes.mat != null) {
        localLinkedList.add(localMeshRes.mat.get().apply(localMeshRes.m));
      }
    }
    return (Rendered[])localLinkedList.toArray(new Rendered[0]);
  }
  
  public static final Mapping empty = new Mapping.1();
}
