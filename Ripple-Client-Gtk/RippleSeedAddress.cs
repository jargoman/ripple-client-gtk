using System;
using System.Text;

namespace RippleClientGtk
{
	public class RippleSeedAddress : RippleIdentifier
	{
		public RippleSeedAddress (byte[] payloadBytes) : base (payloadBytes,33)
		{

		}

		public RippleSeedAddress (String stringID) : base (verifyFormat(stringID))
		{
		}

		public RipplePrivateKey getPrivateKey (int accountNumber) {
			if (Debug.RippleSeedAddress) {
				Logging.write("RippleSeedAddressgetPrivateKey ( accountNumber = " + accountNumber + ")");
			}
			RippleDeterministicKeyGenerator generator = new RippleDeterministicKeyGenerator(this.payloadBytes);
			RipplePrivateKey signingPrivateKey = generator.getAccountPrivateKey(accountNumber);
			return signingPrivateKey;
		}

		public RippleAddress getPublicRippleAddress ()
		{
			if (Debug.RippleSeedAddress) {
				Logging.write("RippleSeedAddress.getPublicRippleAddress ()");
			}
			return getPrivateKey(0).getPublicKey().getAddress();
		}

		/*
		private static string stripLeadingS (String str)
		{
			//shu7fkUGmUQrfztp8jVYQHuMkRXYH
			if (Debug.RippleSeedAddress) {
				Logging.write("RippleSeedAddress stripLeadingS " + str + "\n");
			}

			string ret = str;
			if (str.Length == SEEDLENGTH + 1 && (str [0] == 's' || str [0] == 'S')) {
				ret = str.Remove(0,1);
			}

			if (ret.Length != SEEDLENGTH) {
				throw new FormatException("Invalid seedaddress string " + ret + " of length " + ret.Length + "\n");
			}

			return ret;
		}
		*/

		//private static int SEEDLENGTH = 29;

		private static string verifyFormat (String str)
		{
			if (!str.StartsWith("s") /*|| str.Length != SEEDLENGTH*/) {
				throw new FormatException("Invalid seed address string");
			}

			return str;
		}		

		private String hiddenString = null;
		public string ToHiddenString ()
		{
			if (hiddenString == null) {
				int len = this.ToString().Length;

				StringBuilder bob = new StringBuilder(len);
				for (int i = 0; i < len; i++) {
					bob.Append('*');
				}

				hiddenString = bob.ToString();
			}

			return hiddenString;
		}


	}
}

