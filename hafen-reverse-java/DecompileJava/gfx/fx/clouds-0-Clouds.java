import haven.CloudShadow;
import haven.Coord3f;
import haven.Glob;
import haven.Glob.Weather;
import haven.RenderList;
import haven.Resource;
import haven.TexGL;
import haven.TexR;

public class Clouds
  implements Glob.Weather
{
  public static final TexGL clouds = ((TexR)Resource.classres(Clouds.class).layer(TexR.class)).tex();
  float scale;
  float cmin;
  float cmax;
  float rmin;
  float rmax;
  float nscale;
  float ncmin;
  float ncmax;
  float nrmin;
  float nrmax;
  float oscale;
  float ocmin;
  float ocmax;
  float ormin;
  float ormax;
  float xv;
  float yv;
  float ia = -1.0F;
  CloudShadow cur;
  
  public Clouds(Object... paramVarArgs)
  {
    update(paramVarArgs);
    this.scale = this.nscale;
    this.cmin = this.ncmin;
    this.cmax = this.ncmax;
    this.rmin = this.nrmin;
    this.rmax = this.nrmax;
    this.ia = -1.0F;
  }
  
  public void gsetup(RenderList paramRenderList)
  {
    Coord3f localCoord3f = new Coord3f(this.xv, this.yv, 0.0F);
    if (this.cur == null)
    {
      this.cur = new CloudShadow(clouds, Glob.amblight(paramRenderList), localCoord3f, this.scale);
    }
    else
    {
      this.cur.light = Glob.amblight(paramRenderList);
      this.cur.vel = localCoord3f;
      this.cur.scale = this.scale;
    }
    this.cur.cmin = this.cmin;this.cur.cmax = this.cmax;this.cur.rmin = this.rmin;this.cur.rmax = this.rmax;
    paramRenderList.prepc(this.cur);
  }
  
  public void update(Object... paramVarArgs)
  {
    int i = 0;
    this.oscale = this.scale;
    this.ocmin = this.cmin;
    this.ocmax = this.cmax;
    this.ormin = this.rmin;
    this.ormax = this.rmax;
    this.nscale = (1.0F / ((Integer)paramVarArgs[(i++)]).intValue());
    this.ncmin = (((Number)paramVarArgs[(i++)]).floatValue() / 100.0F);
    this.ncmax = (((Number)paramVarArgs[(i++)]).floatValue() / 100.0F);
    this.nrmin = (((Number)paramVarArgs[(i++)]).floatValue() / 100.0F);
    this.nrmax = (((Number)paramVarArgs[(i++)]).floatValue() / 100.0F);
    if (paramVarArgs.length > i)
    {
      this.xv = ((Number)paramVarArgs[(i++)]).floatValue();
      this.yv = ((Number)paramVarArgs[(i++)]).floatValue();
    }
    else
    {
      this.xv = 0.001F;
      this.yv = 0.002F;
    }
    this.ia = 0.0F;
  }
  
  public boolean tick(int paramInt)
  {
    if (this.ia != -1.0F)
    {
      float f1 = paramInt / 1000.0F;
      this.ia += f1;
      if (this.ia >= 2.0F)
      {
        this.scale = this.nscale;
        this.cmin = this.ncmin;
        this.cmax = this.ncmax;
        this.rmin = this.nrmin;
        this.rmax = this.nrmax;
        this.ia = -1.0F;
      }
      else
      {
        float f2 = this.ia / 2.0F;float f3 = 1.0F - f2;
        this.scale = (f2 * this.nscale + f3 * this.oscale);
        this.cmin = (f2 * this.ncmin + f3 * this.ocmin);
        this.cmax = (f2 * this.ncmax + f3 * this.ocmax);
        this.rmin = (f2 * this.nrmin + f3 * this.ormin);
        this.rmax = (f2 * this.nrmax + f3 * this.ormax);
      }
    }
    return false;
  }
}
