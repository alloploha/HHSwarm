package haven.res.lib.vmat;

import haven.FastMesh.MeshRes;
import haven.Gob;
import haven.Gob.ResAttr.Cell;
import haven.Material.Res;
import haven.Message;
import haven.Rendered;
import haven.Resource;
import haven.Sprite.Owner;
import haven.res.lib.uspr.UnivSprite;
import java.util.Collection;
import java.util.LinkedList;
import java.util.Map;

public class VarSprite
  extends UnivSprite
{
  private Gob.ResAttr.Cell<Mapping> aptr;
  private Mapping cmats;
  
  public VarSprite(Sprite.Owner paramOwner, Resource paramResource, Message paramMessage)
  {
    super(paramOwner, paramResource, paramMessage);
    this.aptr = Gob.getrattr(paramOwner, Mapping.class);
  }
  
  public Mapping mats()
  {
    return (this.aptr != null) && (this.aptr.attr != null) ? (Mapping)this.aptr.attr : Mapping.empty;
  }
  
  public Collection<Rendered> iparts(int paramInt)
  {
    LinkedList localLinkedList = new LinkedList();
    Mapping localMapping = mats();
    for (FastMesh.MeshRes localMeshRes : this.res.layers(FastMesh.MeshRes.class))
    {
      String str = (String)localMeshRes.rdat.get("vm");
      int i = str == null ? -1 : Integer.parseInt(str);
      if (((localMeshRes.mat != null) || (i >= 0)) && ((localMeshRes.id < 0) || ((1 << localMeshRes.id & paramInt) != 0))) {
        localLinkedList.add(new Wrapping(animmesh(localMeshRes.m), localMapping.mergemat(localMeshRes.mat.get(), i), i));
      }
    }
    this.cmats = localMapping;
    return localLinkedList;
  }
  
  public boolean tick(int paramInt)
  {
    if (mats() != this.cmats) {
      update();
    }
    return super.tick(paramInt);
  }
}
