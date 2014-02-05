using System;

namespace RippleClientGtk
{
	public class RipplePublicGeneratorAddress : RippleIdentifier
	{
		public RipplePublicGeneratorAddress (byte[] payloadBytes) : base (payloadBytes, 41)
		{
		}
	}
}

