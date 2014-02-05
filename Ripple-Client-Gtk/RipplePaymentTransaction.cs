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

		public long flags;

		public RipplePaymentTransaction (RippleAddress payer, RippleAddress payee, DenominatedIssuedCurrency amount, long sequencenumber)
		{
			this.payer = payer;
			this.payee = payee;
			this.amount = amount;
			this.sequenceNumber = sequenceNumber;
			this.fee = fee;
		}



	}
}

