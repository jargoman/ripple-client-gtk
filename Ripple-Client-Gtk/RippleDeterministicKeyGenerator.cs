using System;
using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace RippleClientGtk
{
	public class RippleDeterministicKeyGenerator
	{
		public static ECDomainParameters SECP256k1_PARAMS;
		protected byte[] seedBytes;

		static RippleDeterministicKeyGenerator (/*String stringID*/) // STATIC constructor
		{
			if (Debug.RippleDeterministicKeyGenerator) {
				Logging.write ("static RippleDeterministicKeyGenerator : begin \n");

			}

			string curveName = "secp256k1";

			X9ECParameters paramater = SecNamedCurves.GetByName(curveName);
			SECP256k1_PARAMS = new ECDomainParameters (paramater.Curve,paramater.G,paramater.N,paramater.H);

			if (Debug.RippleDeterministicKeyGenerator) {
				testVectors();

			}
		}

		public RippleDeterministicKeyGenerator (RippleSeedAdress secret) : this (secret.getBytes())
		{
			//this.seedBytes = secret.getBytes();

		}

		public RippleDeterministicKeyGenerator (byte[] bytesSeed)
		{
			if (bytesSeed.Length!=16) {
				throw new FormatException ("The seed size should be 128 bit, was " + bytesSeed.Length * 8);
			}

			this.seedBytes = bytesSeed;
		}

		public static byte[] halfSHA512 ( byte[] byteToHash )
		{

			byte[] result = null;
			byte[] ret32 = null;
			try {

				SHA512 sha = new SHA512Managed();
				result = sha.ComputeHash(byteToHash);
				ret32 = new byte[32];
				Array.Copy(result,ret32,32); // copy half the bytes

			} catch (Exception e) {
				Logging.write ("RippleDeterministicKeyGenerator : method halfSHA512 : \n" + e.Message);

				throw e;
			}

			return ret32; // return ret32 not result
		}


		public byte[] getPrivateRootKeyBytes ()
		{

			// TODO portable? endianess? testing?
			for (int seq=0;;seq++) { 

				int offset = this.seedBytes.Length;

				byte[] seqBytes = new byte[offset + 4];
				seqBytes[offset + 0] = (byte)(seq>>24);
				seqBytes[offset + 1] = (byte)(seq>>16);
				seqBytes[offset + 2] = (byte)(seq>>8);
				seqBytes[offset + 3] = (byte) (seq);

				Array.Copy(this.seedBytes,seqBytes,offset); // copy seedbytes infron of the sequence int

				byte[] privateGeneratorBytes = halfSHA512(seedBytes);
				// AAAHHH after so much time coding I think the following line is wrong, System.numeric.biginteger should be replaced with bouncycastle int
				//BigInteger bigin = new BigInteger (BinarySerializer.prepareBigIntegerBytes(privateGeneratorBytes));

				BigInteger bigin = new BigInteger(privateGeneratorBytes);
				if (bigin.CompareTo(SECP256k1_PARAMS.N)==-1) {
					return privateGeneratorBytes;
				}
			}
		}


		public ECPoint getPublicGeneratorPoint ()
		{
			byte[] privateGeneratorBytes = getPrivateRootKeyBytes();
			ECPoint publicGenerator = new RipplePrivateKey(privateGeneratorBytes).getPublicKey();
			return publicGenerator;
		}

		public RipplePrivateKey getAccountPrivateKey (int accountNumber)
		{
			BigInteger privateRootKeyBI = new BigInteger(1,getPrivateRootKeyBytes());
			// TODO factor out the common part with the public key

			ECPoint publicGeneratorPoint = getPublicGeneratorPoint();

			byte[] publicGeneratorBytes = publicGeneratorPoint.GetEncoded();

			/*
			MemoryStream ms = new MemoryStream(4);
			BigEndianWriter bew = new BigEndianWriter(ms);
			bew.Write(accountNumber);
			bew.Flush();
			ms.Flush();

			byte[] accountNumberBytes = ms.ToArray();
			*/
			BigInteger pubGenSeqSubSeqHashBI;

			for (int subSequence = 0; ; subSequence++) {
				MemoryStream mem = new MemoryStream(publicGeneratorBytes.Length + 4 + 4);
				BigEndianWriter ben = new BigEndianWriter(mem);  // luckily I rewrote this. I found a serious bug :O

				ben.Write(publicGeneratorBytes);
				ben.Write(accountNumber);
				ben.Write(subSequence);
				ben.Flush();
				mem.Flush();

				byte[] pubGenAccountSubSeqBytes = mem.ToArray();
				byte[] publicGeneratorAccountSeqHashBytes = halfSHA512(pubGenAccountSubSeqBytes);

				pubGenSeqSubSeqHashBI = new BigInteger(1, publicGeneratorAccountSeqHashBytes);
				if (pubGenSeqSubSeqHashBI.CompareTo(SECP256k1_PARAMS.N) == -1 && !pubGenSeqSubSeqHashBI.Equals(BigInteger.Zero) ) {
					break;
				}

				BigInteger privateKeyForAccount = privateRootKeyBI.Add(pubGenSeqSubSeqHashBI).Mod(SECP256k1_PARAMS.N);
				return new RipplePrivateKey(privateKeyForAccount);
			}
		}


		public RipplePublicKey getAccountPublicKey (int accountNumber)
		{
			ECPoint publicGeneratorPoint = getPublicGeneratorPoint();
			byte[] publicGeneratorBytes = publicGeneratorPoint.GetEncoded();

			byte[] publicGeneratorAccountSeqHashBytes;
			for (int subSequence=0;;subSequence++) {
				MemoryStream ms = new MemoryStream(publicGeneratorBytes.Length + 4 + 4);
				BigEndianWriter ben = new BigEndianWriter(ms);

				ben.Write(publicGeneratorBytes);
				ben.Write(accountNumber);
				ben.Write(subSequence);

				ben.Flush();
				ms.Flush();

				byte[] pubGenAccountSubSeqBytes = ms.ToArray();

				publicGeneratorAccountSeqHashBytes = halfSHA512(pubGenAccountSubSeqBytes);
				BigInteger pubGenSeqSubSeqHashBI = new BigInteger(1, publicGeneratorAccountSeqHashBytes);

				if(pubGenSeqSubSeqHashBI.CompareTo(SECP256k1_PARAMS.N) ==-1){ // TODO should this also test for non zero value like the above function? 
					break;
				}
			}
			ECPoint temporaryPublicPoint = new RipplePrivateKey(publicGeneratorAccountSeqHashBytes).getPublicKey().getPublicPoint();
			ECPoint accountPublicKeyPoint = publicGeneratorPoint.Add(temporaryPublicPoint);
			byte[] publicKeyBytes = accountPublicKeyPoint.GetEncoded();
			return new RipplePublicKey(publicKeyBytes);

		}

		public RipplePublicGeneratorAddress getPublicGeneratorFamily ()
		{
			byte[] publicGeneratorBytes = getPublicGeneratorPoint().GetEncoded();
			return new RipplePublicGeneratorAddress(publicGeneratorBytes);
		}

		public static bool testVectors ()
		{
			Logging.write ("testVectors suite : begin \n");

			byte[] masterseedhex = { 0x71,0xED,0x06,0x41,0x55,0xFF,0xAD,0xFA,0x38,0x78,0x2C,0x5E,0x01,0x58,0xCB,0x26};
			String humanseed = "shHM53KPZ87Gwdqarm1bAmPeXg8Tn";

			RippleSeedAdress seed = new RippleSeedAdress (masterseedhex);

			RippleSeedAdress seed2 = new RippleSeedAdress (humanseed);

			Logging.write ("hex seed = " + masterseedhex.ToString() + "\n human seed = " + seed.ToString());

			//RipplePrivateKey privateGen = new RipplePrivateKey();



			return true;
		}
	}
}

