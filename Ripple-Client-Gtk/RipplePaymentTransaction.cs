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

		public long sequenceNumber;

		public String txHash;
		String signature;

		public String publicKeyUsedToSign;

		public long flags;

		public RipplePaymentTransaction (RippleAddress payer, RippleAddress payee, DenominatedIssuedCurrency amount, long sequencenumber)
		{
			this.payer = payer;
			this.payee = payee;
			this.amount = amount;
			this.sequenceNumber = sequenceNumber;
			this.fee = fee;
		}

		public RipplePaymentTransaction (RippleBinaryObject serObj)
		{
			if (serObj.getTransactionType ().byteValue != TransactionType.PAYMENT) {
				throw new Exception("The RippleBinaryObject is not a payment transaction, but a "+serObj.getTransactionType());
			}

			payer = (RippleAddress) serObj.getField(BinaryFieldType.Account);
			payee = (RippleAddress) serObj.getField(BinaryFieldType.Destination);
			amount = (DenominatedIssuedCurrency) serObj.getField(BinaryFieldType.Amount);
			sequenceNumber = (long) serObj.getField(BinaryFieldType.Sequence);
			fee = (DenominatedIssuedCurrency) serObj.getField(BinaryFieldType.Fee);
			flags = (long) serObj.getField(BinaryFieldType.Flags);
		}

		public RippleBinaryObject getBinaryObject ()
		{
			RippleBinaryObject rbo = new RippleBinaryObject();
			rbo.putField(BinaryFieldType.TransactionType, (int) TransactionType.PAYMENT);
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

