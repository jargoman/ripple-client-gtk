using System;

namespace RippleClientGtk
{
	public class RipplePaymentTransaction
	{
		public RippleAddress payer; 
		public RippleAddress payee;

		public DenominatedIssuedCurrency amount;
		public DenominatedIssuedCurrency fee;

		public String signedTransactionBlob;

		public UInt32 sequenceNumber;

		public String txHash;
		String signature;

		public String publicKeyUsedToSign;

		public UInt32 flags;

		public RipplePaymentTransaction (RippleAddress payer, RippleAddress payee, DenominatedIssuedCurrency amount, DenominatedIssuedCurrency fee,UInt32 sequencenumber)
		{
			this.payer = payer;
			this.payee = payee;
			this.amount = amount;
			this.sequenceNumber = sequencenumber;
			this.fee = fee;
		}

		public RipplePaymentTransaction (RippleBinaryObject serObj)
		{
			if (serObj.getTransactionType ().uint16Value != TransactionType.PAYMENT) {
				throw new Exception("The RippleBinaryObject is not a payment transaction, but a " + serObj.getTransactionType().ToString());
			}

			payer = (RippleAddress) serObj.getField(BinaryFieldType.Account);
			payee = (RippleAddress) serObj.getField(BinaryFieldType.Destination);
			amount = (DenominatedIssuedCurrency) serObj.getField(BinaryFieldType.Amount);
			sequenceNumber = (UInt32)serObj.getField(BinaryFieldType.Sequence);
			fee = (DenominatedIssuedCurrency) serObj.getField(BinaryFieldType.Fee);
			flags = (UInt32) serObj.getField(BinaryFieldType.Flags);
		}

		public RippleBinaryObject getBinaryObject ()
		{
			RippleBinaryObject rbo = new RippleBinaryObject();
			rbo.putField(BinaryFieldType.TransactionType, TransactionType.PAYMENT);
			rbo.putField(BinaryFieldType.Account, this.payer);
			rbo.putField(BinaryFieldType.Destination, this.payee);
			rbo.putField(BinaryFieldType.Amount, this.amount);
			rbo.putField(BinaryFieldType.Sequence, this.sequenceNumber);
			rbo.putField(BinaryFieldType.Fee, this.fee);
			rbo.putField(BinaryFieldType.Flags, this.flags);

			return rbo;
		}

		public String getSignedTxBlob() {
			return signedTransactionBlob;
		}
	}
}

