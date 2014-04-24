using System;

namespace RippleClientGtk
{
	public class IOUPaymentTransaction
	{
		public IOUPaymentTransaction ()
		{
		}

		RippleAddress payer = null;
		RippleAddress payee = null;
		DenominatedIssuedCurrency amount = null;
		DenominatedIssuedCurrency fee = null;
		UInt32 sequenceNumber = 0;

		DenominatedIssuedCurrency sendmax = null;

		public IOUPaymentTransaction (RippleAddress payer, RippleAddress payee, DenominatedIssuedCurrency amount, DenominatedIssuedCurrency fee,UInt32 sequencenumber, DenominatedIssuedCurrency sendmax)
		{
			this.payer = payer;
			this.payee = payee;
			this.amount = amount;
			this.sequenceNumber = sequencenumber;
			this.fee = fee;

			this.sendmax = sendmax;
		}
	}
}

