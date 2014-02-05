using System;
using System.Security.Cryptography;
using System.Text;

namespace RippleClientGtk
{
	public class Hash
	{
		/*
		public Hash ()
		{
		}
		*/

		public static byte[] getHash(String str) {
			byte[] hash;
			byte[] data = Encoding.UTF8.GetBytes(str);
			using (SHA512 sh = new SHA512Managed()) {
				hash = sh.ComputeHash(data);
			}
			return hash;
		}
	}
}

