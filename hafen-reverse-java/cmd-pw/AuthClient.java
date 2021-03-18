
import java.io.*;
import java.security.MessageDigest;

public class AuthClient 
{
    private static byte[] digest(byte[] pw)
	{
		MessageDigest dig;

		try
		{
	    	dig = MessageDigest.getInstance("SHA-256");
	    }
		catch(Exception e) 
		{
	    	throw(new RuntimeException(e));
		}

		dig.update(pw);
		return(dig.digest());
    }

	// returns account or null
    public byte[] trypasswd(String user, byte[] phash) throws IOException 
	{
		return cmd("pw", user, phash);
    }


    private byte[] sendmsg(MessageBuf msg) throws IOException 
	{
		if(msg.size() > 65535)
	    	throw(new RuntimeException("Too long message in AuthClient (" + msg.size() + " bytes)"));

		byte[] buf = new byte[msg.size() + 2];
		buf[0] = (byte)((msg.size() & 0xff00) >> 8);
		buf[1] = (byte)(msg.size() & 0x00ff);

		msg.fin(buf, 2);

		return buf;
    }
    
    private byte[] esendmsg(Object... args) throws IOException 
	{
		MessageBuf buf = new MessageBuf();

		for(Object arg : args) 
		{
	    	if(arg instanceof String) 
			{
				buf.addstring((String)arg);
	    	} 
			else if(arg instanceof byte[]) 
			{
				buf.addbytes((byte[])arg);
	    	} 
			else 
			{
				throw(new RuntimeException("Illegal argument to esendmsg: " + arg.getClass()));
	    	}
		}

		return sendmsg(buf);
    }

   
    public byte[] cmd(Object... args) throws IOException 
	{
		return esendmsg(args);
    }
    
    public static abstract class Credentials 
	{
		public abstract byte[] tryauth(AuthClient cl) throws IOException;
		public abstract String name();
		public void discard() {}
	
		protected void finalize() 
		{
	    	discard();
		}
	
		public static class AuthException extends RuntimeException 
		{
	    	public AuthException(String msg) 
			{
				super(msg);
	    	}
		}
    } // class Credentials 

    public static class NativeCred extends Credentials 
	{
		public final String username;
		private byte[] phash;
	
		public NativeCred(String username, byte[] phash) 
		{
	    	this.username = username;

		    if((this.phash = phash).length != 32)
				throw(new IllegalArgumentException("Password hash must be 32 bytes"));
		}
	
		private static byte[] ohdearjava(String a)
		{
	    	try 
			{
				return(digest(a.getBytes("utf-8")));
	    	} 
			catch(UnsupportedEncodingException e) 
			{
				throw(new RuntimeException(e));
		    }
		}

		public NativeCred(String username, String pw) 
		{
	    	this(username, ohdearjava(pw));
		}
	
		public String name() 
		{
	    	return(username);
		}
	
		public byte[] tryauth(AuthClient cl) throws IOException 
		{
	    	return cl.cmd("pw", username, phash);
		}
	
		public void discard() 
		{
	    	if(phash != null) 
			{
				for(int i = 0; i < phash.length; i++)
				    phash[i] = 0;

				phash = null;
		    }
		}
    } // class NativeCred
} // class AuthClient 
