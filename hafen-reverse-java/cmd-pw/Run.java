
class Run 
{
	private static String bytesToHex(byte[] hash) 
	{
    	StringBuffer hexString = new StringBuffer();

    	for (int i = 0; i < hash.length; i++) 
		{
		    String hex = Integer.toHexString(0xff & hash[i]);

		    if(hex.length() == 1) hexString.append('0');

	        hexString.append(hex);
    	}
	    return hexString.toString();
	}


	public static void main(String[] args)
	{    	
		try
		{
			AuthClient test = new AuthClient();

		  	byte[] buf = new AuthClient.NativeCred(args[0], args[1]).tryauth(test);

			System.out.println(bytesToHex(buf));
		}
		catch(Exception e) 
		{
	    	throw(new RuntimeException(e));
		}
  	}
}
