using System;

namespace RippleClientGtk
{
	public class RippleSeedAdress : RippleIdentifier
	{
		public RippleSeedAdress (byte[] payloadBytes) : base (payloadBytes,33)
		{

		}

		public RippleSeedAdress (String stringID) : base (stringID)
		{
		}

		public RipplePrivateKey getPrivateKey (int accountNumber) {
			RippleDeterministicKeyGenerator generator = new RippleDeterministicKeyGenerator(this.payloadBytes);
			//RipplePrivateKey signing = generator.
		}
	}
}

