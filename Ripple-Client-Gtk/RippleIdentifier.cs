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
		byte identifierType; // the first byte is the identifier

		public RippleIdentifier (byte[] payloadBytes, byte identifierType)
		{
			this.payloadBytes = payloadBytes;
			this.identifierType = identifierType;
		}


		public RippleIdentifier (String humanreadable)
		{
			if (Debug.RippleIdentifier) {
				Logging.write("RippleIdentifier.const (string humanreadable = " + humanreadable + " );\n");
			}

			//this.humanReadableIdentifier = humanreadable;
			byte[] stridBytes = Base58.decode (humanreadable);

			byte[] checksumArray = doubleSha256 (stridBytes, 0, stridBytes.Length - 4);


			Logging.write(
					"checksumArray = " +
					checksumArray[0].ToString() + " " +
					checksumArray[1].ToString() + " " +
					checksumArray[2].ToString() + " " +
					checksumArray[3].ToString() + "\n" +
					"last four bytes = " +
					stridBytes [stridBytes.Length - 4].ToString() + " " +
					stridBytes [stridBytes.Length - 3].ToString() + " " +
					stridBytes [stridBytes.Length - 2].ToString() + " " +
					stridBytes [stridBytes.Length - 1].ToString() + "\n"
			);


			if (checksumArray [0] != stridBytes [stridBytes.Length - 4]
				|| checksumArray [1] != stridBytes [stridBytes.Length - 3]
				|| checksumArray [2] != stridBytes [stridBytes.Length - 2]
				|| checksumArray [3] != stridBytes [stridBytes.Length - 1]) {

				throw new CryptographicException("Checksum failed on identifier "+ humanreadable);

			}

			payloadBytes = new byte[stridBytes.Length - 5];
			System.Array.Copy(stridBytes,1,payloadBytes,0,payloadBytes.Length);
			identifierType = stridBytes[0];

		}

		//@Override

		new public String ToString() 
		{
			if (humanReadableIdentifier==null) {
				byte[] versionPayloadChecksumBytes=new byte[1+payloadBytes.Length+4];
				versionPayloadChecksumBytes[0]= this.identifierType;
				Array.Copy(payloadBytes,0,versionPayloadChecksumBytes,1,payloadBytes.LongLength);

				byte[] hashBytes = doubleSha256(versionPayloadChecksumBytes, 0, 1 + payloadBytes.Length);

				System.Array.Copy(hashBytes, 0, versionPayloadChecksumBytes, 1 + payloadBytes.Length, 4);

				humanReadableIdentifier=Base58.encode(versionPayloadChecksumBytes);



			}



			return humanReadableIdentifier;

		}

		protected byte[] doubleSha256 (byte[] bytesToDoubleHash, int offset, int length)
		{

			Sha256Digest digest = new Sha256Digest();
			byte[] firstrun = new byte[digest.GetDigestSize()];
			digest.BlockUpdate(bytesToDoubleHash, offset, length);
			digest.DoFinal(firstrun,0);

			digest.Reset();

			byte[] result = new byte[digest.GetDigestSize()];
			digest.BlockUpdate(firstrun,0,firstrun.Length);

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
		public override Boolean Equals (Object obj)
		{
			if (Object.ReferenceEquals(this,obj))
				return true;
			if (obj == null)
				return false;
			if (GetType() != obj.GetType ())
				return false;

			if (!(obj is RippleIdentifier))
				throw new FieldAccessException("Object is not a RippleIdentifier. This is a bug");

			RippleIdentifier other = (RippleIdentifier) obj;


			byte[] otherArray = other.payloadBytes;

			/*
			if (this.payloadBytes.Length != otherArray.Length) 
			{
				return false;
			}

			for ( int i = 0; i < this.payloadBytes.Length; i++) {

				if (this.payloadBytes[i] != otherArray[i])
					return false;

			}
			*/

			// the bouncycastle array equals prevents timing attacks
			return Org.BouncyCastle.Utilities.Arrays.ConstantTimeAreEqual(otherArray, this.payloadBytes);

			//return true;
		}
	}
}

