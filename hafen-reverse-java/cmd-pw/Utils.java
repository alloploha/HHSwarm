
import java.io.*;
import java.nio.*;
import java.util.*;


public class Utils 
{
    public static final java.nio.charset.Charset utf8 = java.nio.charset.Charset.forName("UTF-8");
    public static final java.nio.charset.Charset ascii = java.nio.charset.Charset.forName("US-ASCII");


    public static int ub(byte b) 
   {
	return(((int)b) & 0xff);
    }

    public static byte sb(int b) {
	return((byte)b);
    }

    public static long uint32(int n) {
	return(n & 0xffffffffl);
    }

    public static int uint16d(byte[] buf, int off) 
    {
	return(ub(buf[off]) | (ub(buf[off + 1]) << 8));
    }
	
    public static int int16d(byte[] buf, int off) 
    {
	return((int)(short)uint16d(buf, off));
    }
	
    public static long uint32d(byte[] buf, int off) {
	return((long)ub(buf[off]) | ((long)ub(buf[off + 1]) << 8) | ((long)ub(buf[off + 2]) << 16) | ((long)ub(buf[off + 3]) << 24));
    }
	
    public static void uint32e(long num, byte[] buf, int off) {
	buf[off] = (byte)(num & 0xff);
	buf[off + 1] = (byte)((num & 0x0000ff00) >> 8);
	buf[off + 2] = (byte)((num & 0x00ff0000) >> 16);
	buf[off + 3] = (byte)((num & 0xff000000) >> 24);
    }
	
    public static int int32d(byte[] buf, int off) {
	return((int)uint32d(buf, off));
    }
    
    public static long int64d(byte[] buf, int off) {
	long b = 0;
	for(int i = 0; i < 8; i++)
	    b |= ((long)ub(buf[off + i])) << (i * 8);
	return(b);
    }

    public static void int64e(long num, byte[] buf, int off) {
	for(int i = 0; i < 8; i++) {
	    buf[off++] = (byte)(num & 0xff);
	    num >>>= 8;
	}
    }

    public static void int32e(int num, byte[] buf, int off) {
	uint32e(((long)num) & 0xffffffff, buf, off);
    }
	
    public static void uint16e(int num, byte[] buf, int off) {
	buf[off] = sb(num & 0xff);
	buf[off + 1] = sb((num & 0xff00) >> 8);
    }
	
    public static String strd(byte[] buf, int[] off) {
	int i;
	for(i = off[0]; buf[i] != 0; i++);
	String ret;
	try {
	    ret = new String(buf, off[0], i - off[0], "utf-8");
	} catch(UnsupportedEncodingException e) {
	    throw(new IllegalArgumentException(e));
	}
	off[0] = i + 1;
	return(ret);
    }
    
    public static double floatd(byte[] buf, int off) {
	int e = buf[off];
	long t = uint32d(buf, off + 1);
	int m = (int)(t & 0x7fffffffL);
	boolean s = (t & 0x80000000L) != 0;
	if(e == -128) {
	    if(m == 0)
		return(0.0);
	    throw(new RuntimeException("Invalid special float encoded (" + m + ")"));
	}
	double v = (((double)m) / 2147483648.0) + 1.0;
	if(s)
	    v = -v;
	return(Math.pow(2.0, e) * v);
    }

    public static float float32d(byte[] buf, int off) {
	return(Float.intBitsToFloat(int32d(buf, off)));
    }

    public static double float64d(byte[] buf, int off) {
	return(Double.longBitsToDouble(int64d(buf, off)));
    }

    public static void float32e(float num, byte[] buf, int off) {
	int32e(Float.floatToIntBits(num), buf, off);
    }

    public static void float64e(double num, byte[] buf, int off) {
	int64e(Double.doubleToLongBits(num), buf, off);
    }

    public static void float9995d(int word, float[] ret) {
	int xb = (word & 0x7f800000) >> 23, xs = ((word & 0x80000000) >> 31) & 1,
	    yb = (word & 0x003fc000) >> 14, ys = ((word & 0x00400000) >> 22) & 1,
	    zb = (word & 0x00001fd0) >>  5, zs = ((word & 0x00002000) >> 13) & 1;
	int me = (word & 0x1f) - 15;
	int xe = Integer.numberOfLeadingZeros(xb) - 24,
	    ye = Integer.numberOfLeadingZeros(yb) - 24,
	    ze = Integer.numberOfLeadingZeros(zb) - 24;
	if(xe == 32) ret[0] = 0; else ret[0] = Float.intBitsToFloat((xs << 31) | ((me - xe + 127) << 23) | ((xb << (xe + 16)) & 0x007fffff));
	if(ye == 32) ret[1] = 0; else ret[1] = Float.intBitsToFloat((ys << 31) | ((me - ye + 127) << 23) | ((yb << (ye + 16)) & 0x007fffff));
	if(ze == 32) ret[2] = 0; else ret[2] = Float.intBitsToFloat((zs << 31) | ((me - ze + 127) << 23) | ((zb << (ze + 16)) & 0x007fffff));
    }

    public static float hfdec(short bits) {
	int b = ((int)bits) & 0xffff;
	int e = (b & 0x7c00) >> 10;
	int m = b & 0x03ff;
	int ee;
	if(e == 0) {
	    if(m == 0) {
		ee = 0;
	    } else {
		int n = Integer.numberOfLeadingZeros(m) - 22;
		ee = (-15 - n) + 127;
		m = (m << (n + 1)) & 0x03ff;
	    }
	} else if(e == 0x1f) {
	    ee = 0xff;
	} else {
	    ee = e - 15 + 127;
	}
	int f32 = ((b & 0x8000) << 16) |
	    (ee << 23) |
	    (m << 13);
	return(Float.intBitsToFloat(f32));
    }

    public static short hfenc(float f) {
	int b = Float.floatToIntBits(f);
	int e = (b & 0x7f800000) >> 23;
	int m = b & 0x007fffff;
	int ee;
	if(e == 0) {
	    ee = 0;
	    m = 0;
	} else if(e == 0xff) {
	    ee = 0x1f;
	} else if(e < 127 - 14) {
	    ee = 0;
	    m = (m | 0x00800000) >> ((127 - 14) - e);
	} else if(e > 127 + 15) {
	    return(((b & 0x80000000) == 0)?((short)0x7c00):((short)0xfc00));
	} else {
	    ee = e - 127 + 15;
	}
	int f16 = ((b >> 16) & 0x8000) |
	    (ee << 10) |
	    (m >> 13);
	return((short)f16);
    }

    public static float mfdec(byte bits) {
	int b = ((int)bits) & 0xff;
	int e = (b & 0x78) >> 3;
	int m = b & 0x07;
	int ee;
	if(e == 0) {
	    if(m == 0) {
		ee = 0;
	    } else {
		int n = Integer.numberOfLeadingZeros(m) - 29;
		ee = (-7 - n) + 127;
		m = (m << (n + 1)) & 0x07;
	    }
	} else if(e == 0x0f) {
	    ee = 0xff;
	} else {
	    ee = e - 7 + 127;
	}
	int f32 = ((b & 0x80) << 24) |
	    (ee << 23) |
	    (m << 20);
	return(Float.intBitsToFloat(f32));
    }

    public static byte mfenc(float f) {
	int b = Float.floatToIntBits(f);
	int e = (b & 0x7f800000) >> 23;
	int m = b & 0x007fffff;
	int ee;
	if(e == 0) {
	    ee = 0;
	    m = 0;
	} else if(e == 0xff) {
	    ee = 0x0f;
	} else if(e < 127 - 6) {
	    ee = 0;
	    m = (m | 0x00800000) >> ((127 - 6) - e);
	} else if(e > 127 + 7) {
	    return(((b & 0x80000000) == 0)?((byte)0x78):((byte)0xf8));
	} else {
	    ee = e - 127 + 7;
	}
	int f8 = ((b >> 24) & 0x80) |
	    (ee << 3) |
	    (m >> 20);
	return((byte)f8);
    }

    public static void uvec2oct(float[] buf, float x, float y, float z) {
	float m = 1.0f / (Math.abs(x) + Math.abs(y) + Math.abs(z));
	float hx = x * m, hy = y * m;
	if(z >= 0) {
	    buf[0] = hx;
	    buf[1] = hy;
	} else {
	    buf[0] = (1 - Math.abs(hy)) * Math.copySign(1, hx);
	    buf[1] = (1 - Math.abs(hx)) * Math.copySign(1, hy);
	}
    }

    public static void oct2uvec(float[] buf, float x, float y) {
	float z = 1 - (Math.abs(x) + Math.abs(y));
	if(z < 0) {
	    float xc = x, yc = y;
	    x = (1 - Math.abs(yc)) * Math.copySign(1, xc);
	    y = (1 - Math.abs(xc)) * Math.copySign(1, yc);
	}
	float f = 1 / (float)Math.sqrt((x * x) + (y * y) + (z * z));
	buf[0] = x * f;
	buf[1] = y * f;
	buf[2] = z * f;
    }

    static char num2hex(int num) {
	if(num < 10)
	    return((char)('0' + num));
	else
	    return((char)('A' + num - 10));
    }
	
    static int hex2num(char hex) {
	if((hex >= '0') && (hex <= '9'))
	    return(hex - '0');
	else if((hex >= 'a') && (hex <= 'f'))
	    return(hex - 'a' + 10);
	else if((hex >= 'A') && (hex <= 'F'))
	    return(hex - 'A' + 10);
	else
	    throw(new IllegalArgumentException());
    }

    public static String byte2hex(byte[] in) {
	StringBuilder buf = new StringBuilder();
	for(byte b : in) {
	    buf.append(num2hex((b & 0xf0) >> 4));
	    buf.append(num2hex(b & 0x0f));
	}
	return(buf.toString());
    }

    public static byte[] hex2byte(String hex) {
	if(hex.length() % 2 != 0)
	    throw(new IllegalArgumentException("Invalid hex-encoded string"));
	byte[] ret = new byte[hex.length() / 2];
	for(int i = 0, o = 0; i < hex.length(); i += 2, o++)
	    ret[o] = (byte)((hex2num(hex.charAt(i)) << 4) | hex2num(hex.charAt(i + 1)));
	return(ret);
    }
    
    private final static String base64set = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
    private final static int[] base64rev;
    static {
	int[] rev = new int[128];
	for(int i = 0; i < 128; rev[i++] = -1);
	for(int i = 0; i < base64set.length(); i++)
	    rev[base64set.charAt(i)] = i;
	base64rev = rev;
    }
    public static String base64enc(byte[] in) {
	StringBuilder buf = new StringBuilder();
	int p = 0;
	while(in.length - p >= 3) {
	    buf.append(base64set.charAt( (in[p + 0] & 0xfc) >> 2));
	    buf.append(base64set.charAt(((in[p + 0] & 0x03) << 4) | ((in[p + 1] & 0xf0) >> 4)));
	    buf.append(base64set.charAt(((in[p + 1] & 0x0f) << 2) | ((in[p + 2] & 0xc0) >> 6)));
	    buf.append(base64set.charAt(  in[p + 2] & 0x3f));
	    p += 3;
	}
	if(in.length == p + 1) {
	    buf.append(base64set.charAt( (in[p + 0] & 0xfc) >> 2));
	    buf.append(base64set.charAt( (in[p + 0] & 0x03) << 4));
	    buf.append("==");
	} else if(in.length == p + 2) {
	    buf.append(base64set.charAt( (in[p + 0] & 0xfc) >> 2));
	    buf.append(base64set.charAt(((in[p + 0] & 0x03) << 4) | ((in[p + 1] & 0xf0) >> 4)));
	    buf.append(base64set.charAt( (in[p + 1] & 0x0f) << 2));
	    buf.append("=");
	}
	return(buf.toString());
    }
    public static byte[] base64dec(String in) {
	ByteArrayOutputStream buf = new ByteArrayOutputStream();
	int cur = 0, b = 8;
	for(int i = 0; i < in.length(); i++) {
	    char c = in.charAt(i);
	    if(c >= 128)
		throw(new IllegalArgumentException());
	    if(c == '=')
		break;
	    int d = base64rev[c];
	    if(d == -1)
		throw(new IllegalArgumentException());
	    b -= 6;
	    if(b <= 0) {
		cur |= d >> -b;
		buf.write(cur);
		b += 8;
		cur = 0;
	    }
	    cur |= d << b;
	}
	return(buf.toByteArray());
    }
	
    public static String[] splitwords(String text) {
	ArrayList<String> words = new ArrayList<String>();
	StringBuilder buf = new StringBuilder();
	String st = "ws";
	int i = 0;
	while(i < text.length()) {
	    char c = text.charAt(i);
	    if(st == "ws") {
		if(!Character.isWhitespace(c))
		    st = "word";
		else
		    i++;
	    } else if(st == "word") {
		if(c == '"') {
		    st = "quote";
		    i++;
		} else if(c == '\\') {
		    st = "squote";
		    i++;
		} else if(Character.isWhitespace(c)) {
		    words.add(buf.toString());
		    buf = new StringBuilder();
		    st = "ws";
		} else {
		    buf.append(c);
		    i++;
		}
	    } else if(st == "quote") {
		if(c == '"') {
		    st = "word";
		    i++;
		} else if(c == '\\') {
		    st = "sqquote";
		    i++;
		} else {
		    buf.append(c);
		    i++;
		}
	    } else if(st == "squote") {
		buf.append(c);
		i++;
		st = "word";
	    } else if(st == "sqquote") {
		buf.append(c);
		i++;
		st = "quote";
	    }
	}
	if(st == "word")
	    words.add(buf.toString());
	if((st != "ws") && (st != "word"))
	    return(null);
	return(words.toArray(new String[0]));
    }
	
    public static String[] splitlines(String text) {
	ArrayList<String> ret = new ArrayList<String>();
	int p = 0;
	while(true) {
	    int p2 = text.indexOf('\n', p);
	    if(p2 < 0) {
		ret.add(text.substring(p));
		break;
	    }
	    ret.add(text.substring(p, p2));
	    p = p2 + 1;
	}
	return(ret.toArray(new String[0]));
    }

    static int atoi(String a) {
	try {
	    return(Integer.parseInt(a));
	} catch(NumberFormatException e) {
	    return(0);
	}
    }
    
    static void readtileof(InputStream in) throws IOException {
        byte[] buf = new byte[4096];
        while(true) {
            if(in.read(buf, 0, buf.length) < 0)
                return;
        }
    }
    
    static byte[] readall(InputStream in) throws IOException {
	byte[] buf = new byte[4096];
	int off = 0;
	while(true) {
	    if(off == buf.length) {
		byte[] n = new byte[buf.length * 2];
		System.arraycopy(buf, 0, n, 0, buf.length);
		buf = n;
	    }
	    int ret = in.read(buf, off, buf.length - off);
	    if(ret < 0) {
		byte[] n = new byte[off];
		System.arraycopy(buf, 0, n, 0, off);
		return(n);
	    }
	    off += ret;
	}
    }
    


    public static String titlecase(String str) {
	return(Character.toTitleCase(str.charAt(0)) + str.substring(1));
    }
    

    public static int floordiv(int a, int b) {
	if(a < 0)
	    return(((a + 1) / b) - 1);
	else
	    return(a / b);
    }
    
    public static int floormod(int a, int b) {
	int r = a % b;
	if(r < 0)
	    r += b;
	return(r);
    }

    /* XXX: These are not actually correct, since an exact integer
     * will round downwards, but I don't actually expect that to be a
     * problem given how I use these, and it turns out that
     * java.lang.Math.floor is actually surprisingly slow (it
     * delegates to StrictMath.floor for some reason). */
    public static int floordiv(float a, float b) {
	float q = a / b;
	return((q < 0)?(((int)q) - 1):((int)q));
    }
    public static int floordiv(double a, double b) {
	double q = a / b;
	return((q < 0)?(((int)q) - 1):((int)q));
    }
    
    public static float floormod(float a, float b) {
	float r = a % b;
	return((a < 0)?(r + b):r);
    }
    public static double floormod(double a, double b) {
	double r = a % b;
	return((a < 0)?(r + b):r);
    }

    public static double cangle(double a) {
	while(a > Math.PI)
	    a -= Math.PI * 2;
	while(a < -Math.PI)
	    a += Math.PI * 2;
	return(a);
    }

    public static double cangle2(double a) {
	while(a > Math.PI * 2)
	    a -= Math.PI * 2;
	while(a < 0)
	    a += Math.PI * 2;
	return(a);
    }

    public static double clip(double d, double min, double max) {
	if(d < min)
	    return(min);
	if(d > max)
	    return(max);
	return(d);
    }
    
    public static float clip(float d, float min, float max) {
	if(d < min)
	    return(min);
	if(d > max)
	    return(max);
	return(d);
    }
    
    public static int clip(int i, int min, int max) {
	if(i < min)
	    return(min);
	if(i > max)
	    return(max);
	return(i);
    }

    public static double clipnorm(double d, double min, double max) {
	if(d < min)
	    return(0.0);
	if(d > max)
	    return(1.0);
	return((d - min) / (max - min));
    }

    public static double smoothstep(double d) {
	return(d * d * (3 - (2 * d)));
    }


    public static void serialize(Object obj, OutputStream out) throws IOException {
	ObjectOutputStream oout = new ObjectOutputStream(out);
	oout.writeObject(obj);
	oout.flush();
    }
    
    public static byte[] serialize(Object obj) {
	ByteArrayOutputStream out = new ByteArrayOutputStream();
	try {
	    serialize(obj, out);
	} catch(IOException e) {
	    throw(new RuntimeException(e));
	}
	return(out.toByteArray());
    }
    
    public static Object deserialize(InputStream in) throws IOException {
	ObjectInputStream oin = new ObjectInputStream(in);
	try {
	    return(oin.readObject());
	} catch(ClassNotFoundException e) {
	    return(null);
	}
    }
    
    public static Object deserialize(byte[] buf) {
	if(buf == null)
	    return(null);
	InputStream in = new ByteArrayInputStream(buf);
	try {
	    return(deserialize(in));
	} catch(IOException e) {
	    return(null);
	}
    }
    
    public static boolean parsebool(String s) {
	if(s == null)
	    throw(new IllegalArgumentException(s));
	else if(s.equalsIgnoreCase("1") || s.equalsIgnoreCase("on") || s.equalsIgnoreCase("true") || s.equalsIgnoreCase("yes"))
	    return(true);
	else if(s.equalsIgnoreCase("0") || s.equalsIgnoreCase("off") || s.equalsIgnoreCase("false") || s.equalsIgnoreCase("no"))
	    return(false);
	throw(new IllegalArgumentException(s));
    }

    public static boolean eq(Object a, Object b) {
	return((a == b) || ((a != null) && a.equals(b)));
    }

    public static boolean parsebool(String s, boolean def) {
	try {
	    return(parsebool(s));
	} catch(IllegalArgumentException e) {
	    return(def);
	}
    }
    
    /* Just in case anyone doubted that Java is stupid. :-/ */
    public static FloatBuffer bufcp(float[] a) {
	FloatBuffer b = mkfbuf(a.length);
	b.put(a);
	b.rewind();
	return(b);
    }
    public static ShortBuffer bufcp(short[] a) {
	ShortBuffer b = mksbuf(a.length);
	b.put(a);
	b.rewind();
	return(b);
    }
    public static FloatBuffer bufcp(FloatBuffer a) {
	a.rewind();
	FloatBuffer ret = mkfbuf(a.remaining());
	ret.put(a).rewind();
	return(ret);
    }
    public static IntBuffer bufcp(IntBuffer a) {
	a.rewind();
	IntBuffer ret = mkibuf(a.remaining());
	ret.put(a).rewind();
	return(ret);
    }
    public static ByteBuffer mkbbuf(int n) {
	try {
	    return(ByteBuffer.allocateDirect(n).order(ByteOrder.nativeOrder()));
	} catch(OutOfMemoryError e) {
	    /* At least Sun's class library doesn't try to collect
	     * garbage if it's out of direct memory, which is pretty
	     * stupid. So do it for it, then. */
	    System.gc();
	    return(ByteBuffer.allocateDirect(n).order(ByteOrder.nativeOrder()));
	}
    }
    public static FloatBuffer mkfbuf(int n) {
	return(mkbbuf(n * 4).asFloatBuffer());
    }
    public static ShortBuffer mksbuf(int n) {
	return(mkbbuf(n * 2).asShortBuffer());
    }
    public static IntBuffer mkibuf(int n) {
	return(mkbbuf(n * 4).asIntBuffer());
    }

    /*
    public static ByteBuffer wbbuf(int n) {
	return(mkbbuf(n));
    }
    public static IntBuffer wibuf(int n) {
	return(mkibuf(n));
    }
    public static FloatBuffer wfbuf(int n) {
	return(mkfbuf(n));
    }
    public static ShortBuffer wsbuf(int n) {
	return(mksbuf(n));
    }
    */
    public static ByteBuffer wbbuf(int n) {
	return(ByteBuffer.wrap(new byte[n]));
    }
    public static IntBuffer wibuf(int n) {
	return(IntBuffer.wrap(new int[n]));
    }
    public static FloatBuffer wfbuf(int n) {
	return(FloatBuffer.wrap(new float[n]));
    }
    public static ShortBuffer wsbuf(int n) {
	return(ShortBuffer.wrap(new short[n]));
    }
    public static FloatBuffer wbufcp(FloatBuffer a) {
	a.rewind();
	FloatBuffer ret = wfbuf(a.remaining());
	ret.put(a).rewind();
	return(ret);
    }
    public static IntBuffer wbufcp(IntBuffer a) {
	a.rewind();
	IntBuffer ret = wibuf(a.remaining());
	ret.put(a).rewind();
	return(ret);
    }

    


    


}
