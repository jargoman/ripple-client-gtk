using System;
//using System.Data.Linq;
using System.Security.Cryptography;
using Org.BouncyCastle;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Crypto.Signers;

// done

namespace RippleClientGtk
{
	public class RippleIdentifier
	{
		String humanReadableIdentifier = null;
		protected byte[] payloadBytes = null;
		int identifierType; // I suppose this is the 4 byte checksum? If so keep endianness in mind. 

		public RippleIdentifier (byte[] payloadBytes, int identifierType)
		{
			this.payloadBytes = payloadBytes;
			this.identifierType = identifierType;
		}


		public RippleIdentifier (String humanreadable)
		{
			this.humanReadableIdentifier = humanreadable;
			byte[] stridBytes = Base58.decode (humanreadable);

			byte[] checksumArray = doubleSha256 (stridBytes, 0, stridBytes.Length - 4);

			if (checksumArray [0] != stridBytes [stridBytes.Length - 4]
				|| checksumArray [1] != stridBytes [stridBytes.Length - 3]
				|| checksumArray [2] != stridBytes [stridBytes.Length - 2]
				|| checksumArray [3] != stridBytes [stridBytes.Length - 1]) {

				throw new CryptographicException("Checksum failed on identifier "+ humanreadable);

			}



		}


		private void init ()
		{

		}

		//@Override

		new public String ToString() 
		{
			if (humanReadableIdentifier==null) {
				byte[] versionPayloadChecksumBytes=new byte[1+payloadBytes.Length+4];
				versionPayloadChecksumBytes[0]=(byte) this.identifierType;
				Array.Copy(payloadBytes,0,versionPayloadChecksumBytes,1,payloadBytes.LongLength);
				try {
					SHA256 sha = new SHA256Managed ();
					//sha.CanReuseTransform = true;

					byte[] firsthash = sha.ComputeHash(versionPayloadChecksumBytes,0,1+payloadBytes.Length);

					sha.Clear();
					Array.Copy (sha.ComputeHash(firsthash),0,versionPayloadChecksumBytes,1+payloadBytes.LongLength, 4);
					humanReadableIdentifier=Base58.encode(versionPayloadChecksumBytes);

				} catch (Exception e) {
					Logging.write("RippleIdentifier : toString ");

				}


			}



			return humanReadableIdentifier;

		}

		protected byte[] doubleSha256 (byte[] bytesToDoubleHash, int offset, int length)
		{

			Sha256Digest digest = new Sha256Digest();
			digest.BlockUpdate(bytesToDoubleHash, offset, length);

			byte[] firstrun = new byte[digest.GetDigestSize()];
			digest.DoFinal(firstrun,0);

			digest.Reset();

			digest.BlockUpdate(firstrun,0,firstrun.Length);
			byte[] result = new byte[digest.GetDigestSize()];
			digest.DoFinal(result,0);

			return result;
		}

		public byte[] getBytes() {
			return payloadBytes;
		}

		/*
		public int hashCode ()
		{
			int prime = 31;
			int result = 1;

		}
		*/

		//@Override
		public Boolean equals (Object obj)
		{
			if (this == obj)
				return true;
			if (obj == null)
				return false;
			if (GetType() != obj.GetType ())
				return false;

			if (!(obj is RippleIdentifier))
				throw new FieldAccessException("Object is not a RippleIdentifier. This is a bug");

			RippleIdentifier other = (RippleIdentifier) obj;


			byte[] otherArray = other.payloadBytes;

			if (this.payloadBytes.Length != otherArray.Length) 
			{
				return false;
			}

			for ( int i = 0; i < this.payloadBytes.Length; i++) {

				if (this.payloadBytes[i] != otherArray[i])
					return false;

			}

			return true;
		}
	}
}

