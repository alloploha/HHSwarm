

public class MessageBuf extends Message 
{
    public static final MessageBuf nil = new MessageBuf();
    private final int oh;

    public MessageBuf(byte[] blob, int off, int len) 
	{
		if(blob == null)
	    	throw(new NullPointerException("blob"));

		this.rbuf = blob;
		this.rh = this.oh = off;
		this.rt = off + len;
    }

    public MessageBuf(byte[] blob) 
	{
		this(blob, 0, blob.length);
    }

    public MessageBuf() 
	{
		this.oh = 0;
    }

    public MessageBuf(Message from) 
	{
		if(from instanceof MessageBuf) 
		{
	    	MessageBuf fb = (MessageBuf)from;
		    this.rbuf = fb.rbuf;
		    this.rh = this.oh = fb.rh;
		    this.rt = fb.rt;
		    this.wbuf = fb.wbuf;
		    this.wh = this.wt = fb.wh;
		} 
		else 
		{
	    	this.rbuf = from.bytes();
		    this.rh = this.oh = 0;
		    this.rt = rbuf.length;
		}
    }

    public boolean underflow(int hint) 
	{
		return(false);
    }

    public void overflow(int min) 
	{
		int cl = (wt == 0)?32:wt;

		while(cl - wh < min)
		    cl *= 2;

		byte[] n = new byte[cl];
		System.arraycopy(wbuf, 0, n, 0, wh);
		wbuf = n;
		wt = cl;
    }

    public boolean equals(Object o2) 
	{
		if(!(o2 instanceof MessageBuf))
	    	return(false);

		MessageBuf m2 = (MessageBuf)o2;

		if((m2.rt - m2.oh) != (rt - oh))
		    return(false);

		for(int i = oh, o = m2.oh; i < rt; i++, o++) 
		{
	    	if(m2.rbuf[o] != rbuf[i])
				return(false);
		}

		return(true);
    }

    public int hashCode() 
	{
		int ret = 192581;

		for(int i = oh; i < rt; i++)
		    ret = (ret * 31) + rbuf[i];

		return(ret);
    }

    public int rem() 
	{
		return(rt - rh);
    }

    public void rewind() 
	{
		rh = oh;
    }

    public MessageBuf clone() 
	{
		return(new MessageBuf(rbuf, oh, rt - oh));
    }

    public int size() 
	{
		return(wh);
    }

    public byte[] fin() 
    {
		byte[] ret = new byte[wh];
		System.arraycopy(wbuf, 0, ret, 0, wh);
		return(ret);
    }

    public void fin(byte[] buf, int off) 
	{
		System.arraycopy(wbuf, 0, buf, off, Math.min(wh, buf.length - off));
    }

    public void fin(java.nio.ByteBuffer buf) 
	{
		buf.put(wbuf, 0, wh);
    }

    public String toString() 
	{
		StringBuilder buf = new StringBuilder();

		buf.append("Message(");

		for(int i = oh; i < rt; i++) 
		{
	    	if(i > 0)
				buf.append(' ');

		    if(i == rh)
				buf.append('>');

		    buf.append(String.format("%02x", rbuf[i] & 0xff));
		}

		buf.append(")");

		return(buf.toString());
    }
}
