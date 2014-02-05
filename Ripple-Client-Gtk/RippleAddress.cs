using System;

namespace RippleClientGtk
{
	public class RippleAddress : RippleIdentifier
	{
		public static readonly RippleAddress RIPPLE_ROOT_ACCOUNT=new RippleAddress("rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh");
 		public static readonly RippleAddress RIPPLE_ADDRESS_ZERO=new RippleAddress("rrrrrrrrrrrrrrrrrrrrrhoLvTp");
		public static readonly RippleAddress RIPPLE_ADDRESS_ONE=new RippleAddress("rrrrrrrrrrrrrrrrrrrrBZbvji");
		public static readonly RippleAddress RIPPLE_ADDRESS_NEUTRAL=RIPPLE_ADDRESS_ONE;
		public static readonly RippleAddress RIPPLE_ADDRESS_NAN=new RippleAddress("rrrrrrrrrrrrrrrrrrrn5RM1rHd");
		public static readonly RippleAddress RIPPLE_ADDRESS_BITSTAMP = new RippleAddress("rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B");
		public static readonly RippleAddress RIPPLE_ADDRESS_JRIPPLEAPI=new RippleAddress("r32fLio1qkmYqFFYkwdnsaVN7cxBwkW4cT");
		public static readonly RippleAddress RIPPLE_ADDRESS_PMARCHES=new RippleAddress("rEQQNvhuLt1KTYmDWmw12mPvmJD4KCtxmS");
		public static readonly RippleAddress RIPPLE_ADDRESS_JARGOMAN=new RippleAddress("rn3KLJY2AfHP5mjfHfn5QXNUn9VvFqihLK");
		public static readonly RippleAddress RIPPLE_ADDRESS_ICE_ISSUER=new RippleAddress("r4H3F9dDaYPFwbrUsusvNAHLz2sEZk4wE5");

		public RippleAddress (byte[] payloadBytes) : base (payloadBytes, 0)
		{
			
		}

		public RippleAddress (String stringID) : base (stringID)
		{

		}
	}
}

