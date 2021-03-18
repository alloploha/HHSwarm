package haven.res.lib.vmat;

import haven.GLState;
import haven.GOut;
import haven.RenderList;
import haven.Rendered;

public class Wrapping
  implements Rendered
{
  public final Rendered r;
  public final GLState st;
  public final int mid;
  
  public Wrapping(Rendered paramRendered, GLState paramGLState, int paramInt)
  {
    this.r = paramRendered;
    this.st = paramGLState;
    this.mid = paramInt;
  }
  
  public void draw(GOut paramGOut) {}
  
  public boolean setup(RenderList paramRenderList)
  {
    paramRenderList.add(this.r, this.st);
    return false;
  }
  
  public String toString()
  {
    return String.format("#<vmat %s %s>", new Object[] { Integer.valueOf(this.mid), this.st });
  }
}
