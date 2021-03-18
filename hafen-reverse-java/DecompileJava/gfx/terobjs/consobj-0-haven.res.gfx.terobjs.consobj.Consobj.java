package haven.res.gfx.terobjs.consobj;

import haven.Coord;
import haven.Coord2d;
import haven.Coord3f;
import haven.FastMesh;
import haven.Glob;
import haven.Gob;
import haven.Gob.Overlay.CUpd;
import haven.Indir;
import haven.Loading;
import haven.Location;
import haven.MCache;
import haven.Material;
import haven.Material.Res;
import haven.MeshBuf;
import haven.MeshBuf.Face;
import haven.MeshBuf.Tex;
import haven.MeshBuf.Vertex;
import haven.Message;
import haven.MessageBuf;
import haven.RenderList;
import haven.Rendered;
import haven.ResData;
import haven.Resource;
import haven.Resource.Pool;
import haven.Resource.Resolver;
import haven.Sprite;
import haven.Sprite.Owner;

public class Consobj
  extends Sprite
  implements Gob.Overlay.CUpd
{
  public static final Indir<Resource> signres = Resource.classres(Consobj.class).pool.load("gfx/terobjs/sign", 6);
  public static final Indir<Resource> poleres = Resource.classres(Consobj.class).pool.load("gfx/terobjs/arch/conspole", 2);
  private static Material bmat = null;
  public static final float bscale = 0.09090909F;
  public final Coord ul;
  public final Coord br;
  public final ResData built;
  public final boolean dbound;
  public float done;
  final Coord3f cc;
  final Sprite sign;
  final Sprite pole;
  final Location[] poles;
  final MCache map;
  Rendered bound;
  
  Coord3f gnd(float paramFloat1, float paramFloat2)
  {
    double d = -((Gob)this.owner).a;
    float f1 = (float)Math.sin(d);float f2 = (float)Math.cos(d);
    float f3 = paramFloat1 * f2 + paramFloat2 * f1;float f4 = paramFloat2 * f2 - paramFloat1 * f1;
    return new Coord3f(paramFloat1, -paramFloat2, this.map.getcz(f3 + this.cc.x, f4 + this.cc.y) - this.cc.z);
  }
  
  public Consobj(Sprite.Owner paramOwner, Resource paramResource, Message paramMessage)
  {
    super(paramOwner, paramResource);
    this.map = ((Glob)paramOwner.context(Glob.class)).map;
    if (bmat == null) {
      bmat = ((Material.Res)Resource.classres(Consobj.class).layer(Material.Res.class)).get();
    }
    this.ul = new Coord(paramMessage.int8(), paramMessage.int8());
    this.br = new Coord(paramMessage.int8(), paramMessage.int8());
    this.done = (paramMessage.uint8() / 255.0F);
    if (!paramMessage.eom())
    {
      int i = paramMessage.uint16();
      this.built = new ResData(((Resource.Resolver)paramOwner.context(Resource.Resolver.class)).getres(i), new MessageBuf(paramMessage.bytes()));
    }
    else
    {
      this.built = null;
    }
    this.sign = Sprite.create(paramOwner, (Resource)signres.get(), Message.nil);
    this.pole = Sprite.create(paramOwner, (Resource)poleres.get(), Message.nil);
    this.cc = ((Gob)paramOwner).getrc();
    
    this.poles = new Location[] {Location.xlate(gnd(this.ul.x, this.ul.y)), Location.xlate(gnd(this.br.x, this.ul.y)), Location.xlate(gnd(this.br.x, this.br.y)), Location.xlate(gnd(this.ul.x, this.br.y)) };
    
    this.dbound = ((this.br.x - this.ul.x > 22) || (this.br.y - this.ul.y > 22));
  }
  
  void trace(MeshBuf paramMeshBuf, float paramFloat1, float paramFloat2, float paramFloat3, float paramFloat4)
  {
    float f1 = paramFloat3 - paramFloat1;float f2 = paramFloat4 - paramFloat2;float f3 = (float)Math.sqrt(f1 * f1 + f2 * f2);
    float f4 = paramFloat1;float f5 = paramFloat2;
    Coord3f localCoord3f = new Coord3f(f2 / f3, f1 / f3, 0.0F);
    MeshBuf.Tex localTex = (MeshBuf.Tex)paramMeshBuf.layer(MeshBuf.tex); MeshBuf 
      tmp73_72 = paramMeshBuf;tmp73_72.getClass();Object localObject1 = new MeshBuf.Vertex(tmp73_72, gnd(f4, f5), localCoord3f); MeshBuf 
      tmp98_97 = paramMeshBuf;tmp98_97.getClass();Object localObject2 = new MeshBuf.Vertex(tmp98_97, gnd(f4, f5).add(0.0F, 0.0F, 3.0F), localCoord3f);
    localTex.set((MeshBuf.Vertex)localObject1, new Coord3f(0.0F, 1.0F, 0.0F));
    localTex.set((MeshBuf.Vertex)localObject2, new Coord3f(0.0F, 0.0F, 0.0F));
    int i = 0;
    for (;;)
    {
      int j = 1;
      float f6 = 1.0F;
      float f8 = paramFloat3;float f9 = paramFloat4;
      float f10;
      float f7;
      if (f1 != 0.0F)
      {
        if (f1 > 0.0F) {
          f7 = ((f10 = (float)((Math.floor(f4 / MCache.tilesz.x) + 1.0D) * MCache.tilesz.x)) - paramFloat1) / f1;
        } else {
          f7 = ((f10 = (float)((Math.ceil(f4 / MCache.tilesz.x) - 1.0D) * MCache.tilesz.x)) - paramFloat1) / f1;
        }
        if (f7 < f6)
        {
          f8 = f10;f9 = paramFloat2 + f2 * f7;
          f6 = f7;
          j = 0;
        }
      }
      if (f2 != 0.0F)
      {
        if (f2 > 0.0F) {
          f7 = ((f10 = (float)((Math.floor(f5 / MCache.tilesz.y) + 1.0D) * MCache.tilesz.y)) - paramFloat2) / f2;
        } else {
          f7 = ((f10 = (float)((Math.ceil(f5 / MCache.tilesz.y) - 1.0D) * MCache.tilesz.y)) - paramFloat2) / f2;
        }
        if (f7 < f6)
        {
          f8 = paramFloat1 + f1 * f7;f9 = f10;
          f6 = f7;
          j = 0;
        }
      }
      MeshBuf tmp403_402 = paramMeshBuf;tmp403_402.getClass();MeshBuf.Vertex localVertex1 = new MeshBuf.Vertex(tmp403_402, gnd(f8, f9), localCoord3f); MeshBuf 
        tmp428_427 = paramMeshBuf;tmp428_427.getClass();MeshBuf.Vertex localVertex2 = new MeshBuf.Vertex(tmp428_427, gnd(f8, f9).add(0.0F, 0.0F, 3.0F), localCoord3f);
      localTex.set(localVertex1, new Coord3f(f6 * f3 * 0.09090909F, 1.0F, 0.0F));
      localTex.set(localVertex2, new Coord3f(f6 * f3 * 0.09090909F, 0.0F, 0.0F)); MeshBuf 
        tmp508_507 = paramMeshBuf;tmp508_507.getClass();new MeshBuf.Face(tmp508_507, (MeshBuf.Vertex)localObject2, (MeshBuf.Vertex)localObject1, localVertex2); MeshBuf tmp528_527 = paramMeshBuf;tmp528_527.getClass();new MeshBuf.Face(tmp528_527, (MeshBuf.Vertex)localObject1, localVertex1, localVertex2);
      localObject1 = localVertex1;localObject2 = localVertex2;
      f4 = f8;f5 = f9;
      if (j != 0) {
        return;
      }
      if (i++ > 100) {
        throw new RuntimeException("stuck in trace");
      }
    }
  }
  
  Rendered mkbound()
  {
    MeshBuf localMeshBuf = new MeshBuf();
    trace(localMeshBuf, this.ul.x, this.ul.y, this.ul.x, this.br.y);
    trace(localMeshBuf, this.ul.x, this.br.y, this.br.x, this.br.y);
    trace(localMeshBuf, this.br.x, this.br.y, this.br.x, this.ul.y);
    trace(localMeshBuf, this.br.x, this.ul.y, this.ul.x, this.ul.y);
    FastMesh localFastMesh = localMeshBuf.mkmesh();
    return bmat.apply(localFastMesh);
  }
  
  public boolean setup(RenderList paramRenderList)
  {
    paramRenderList.add(this.sign, null);
    if (this.dbound)
    {
      for (Location localLocation : this.poles) {
        paramRenderList.add(this.pole, localLocation);
      }
      if (this.bound == null) {
        try
        {
          this.bound = mkbound();
        }
        catch (Loading localLoading) {}
      }
      if (this.bound != null) {
        paramRenderList.add(this.bound, null);
      }
    }
    return false;
  }
  
  public void update(Message paramMessage)
  {
    for (int i = 0; i < 4; i++) {
      paramMessage.int8();
    }
    this.done = (paramMessage.uint8() / 255.0F);
  }
}
