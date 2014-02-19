using System;
//using System.Numerics;

namespace RippleClientGtk
{
	public class DenominatedIssuedCurrency
	{
		public Decimal amount;


		public RippleAddress issuer = null;
		public String currency = null;

		public static readonly int MIN_SCALE = -96;
		public static readonly int MAX_SCALE = 80;

		public DenominatedIssuedCurrency ()
		{
		}

		public DenominatedIssuedCurrency (Decimal amount, RippleAddress issuer, String currencyStr)
		{
			this.amount = amount;
			this.issuer = issuer;
			this.currency = currencyStr;
		}

		public DenominatedIssuedCurrency (Decimal xrpAmount)
		{
			this.amount = xrpAmount;

		}

		public DenominatedIssuedCurrency (int xrpAmount)
		{
			this.amount = new decimal(xrpAmount);
		}

		public DenominatedIssuedCurrency (long xrpAmount)
		{
			this.amount = new decimal ( xrpAmount );
		}

		public Boolean isNative ()
		{
			// TODO is this sufficient?
			return /*issuer==null && */ this.currency == null; 
		}

		public Boolean isNegative ()
		{
			if (amount < Decimal.Zero) {
				return true;
			} else {
				return false;
			}
		}


		public override String ToString () {
			if ( /*issuer==null || */ currency==null) {
				return amount.ToString() + " drops";
			}

			return amount.ToString() + " " + currency + "/" + issuer.ToString();
		}




	}
}

