using System;
//using System.Security;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Asn1.Sec;

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Generators;

namespace RippleClientGtk
{
	public class RippleSigner
	{
		RipplePrivateKey privateKey = null;

		public RippleSigner (RipplePrivateKey privateKey)
		{
			this.privateKey = privateKey;	


		}

		public RippleBinaryObject sign (RippleBinaryObject serObjToSign)
		{
			if (serObjToSign.getField (BinaryFieldType.TxnSignature) != null) {
				throw new Exception ("Object already signed");
			}

			RippleBinaryObject signedRBO = new RippleBinaryObject(serObjToSign);
			signedRBO.putField (BinaryFieldType.SigningPubKey, privateKey.getPublicKey().getPublicPoint().GetEncoded());

			byte[] hashOfRBOBytes = signedRBO.generateHashFromBinaryObject();
			ECDSASignature signature = signHash(hashOfRBOBytes);

			signedRBO.putField(BinaryFieldType.TxnSignature, signature.encodeToDER());
			return signedRBO;
		}


		private ECDSASignature signHash (byte[] hashOfBytes) {
			if (hashOfBytes.Length!=32) {
				throw new FormatException("can sign only a hash of 32 bytes");
			}

			ECDsaSigner signer = new ECDsaSigner();

			Org.BouncyCastle.Crypto.Parameters.ECPrivateKeyParameters privKey = privateKey.getECPrivateKey();
			signer.Init(true,privKey);

			BigInteger[] RandS = signer.GenerateSignature(hashOfBytes);
			return new ECDSASignature ((BigInteger)RandS.GetValue(0), (BigInteger)RandS.GetValue(1), privateKey.getPublicKey().getPublicPoint());
		}

		public Boolean isSignatureVerified (RippleBinaryObject serObj)
		{
			try {
				byte[] signatureBytes = (byte[]) serObj.getField(BinaryFieldType.TxnSignature);

				if ( signatureBytes==null) {
					throw new Exception ("The specified  has no signature");
				}

				byte[] signingPubKeyBytes = (byte[]) serObj.getField(BinaryFieldType.SigningPubKey);

				if (signingPubKeyBytes==null) {
					throw new Exception("The specified  has no public key associated to the signature");
				}

				RippleBinaryObject unsignedRBO = serObj.getUnsignedCopy();
				byte[] hashToVeryfy = unsignedRBO.generateHashFromBinaryObject();

				ECDsaSigner signer = new ECDsaSigner();
				ECDSASignature signature = new ECDSASignature(signatureBytes, signingPubKeyBytes);
				if (signature.publicSigningKey==null) {
					// shouldn't ever happen
					throw new Exception("ECDSASignature publicSigningKey is null");
				}
				//Org.BouncyCastle.Crypto.Parameters.ECPublicKeyParameters
				signer.Init(false, new Org.BouncyCastle.Crypto.Parameters.ECPublicKeyParameters(signature.publicSigningKey,RippleDeterministicKeyGenerator.SECP256k1_PARAMS));
				
				return signer.VerifySignature(hashToVeryfy,signature.r, signature.s);
			} catch (Exception e) {
				//TODO debug
				throw e;
			}
		}
	}
}

