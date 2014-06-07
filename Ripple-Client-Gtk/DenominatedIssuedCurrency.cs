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

		public static readonly ulong MAX_MANTISSA = 9999999999999999UL;
		public static readonly ulong MIN_MANTISSA = 1000000000000000UL;

		public DenominatedIssuedCurrency ()
		{
		}

		public DenominatedIssuedCurrency (Decimal amount, RippleAddress issuer, String currencyStr)
		{
			this.amount = amount;
			this.issuer = issuer;

			/*
			if (currencyStr == null || currencyStr.Trim().Equals("")) {
				throw new FormatException("IOU amount must have a currency value");
			}

			if (currencyStr.ToLower().Trim().Equals("xrp")) {
				throw new FormatException("XRP is not a valid Currency value");
			}
			*/

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
			return /*issuer==null && */ this.currency == null || this.currency == ""; 
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

			return amount.ToString() + " " + currency + " " + issuer.ToString();
		}


		public static Decimal? parseDecimal (String str, String messageVar)
		{

			Decimal? dec = null;
			try {
				dec = Convert.ToDecimal(str);
				return dec;
			}
			catch (FormatException ex) {
				MessageDialog.showMessage (messageVar + " is fomated incorrectly./n" + messageVar + "must be a valid decimal number\n");
				// todo debug
				return null;
			}

			catch (OverflowException ex) {
				MessageDialog.showMessage (messageVar + " is greater than a Decimal? No one's got that much money\n");
				// todo debug
				return null;
			}

			catch (Exception ex) {
				// todo debug
				//MessageDialog.showMessage (messageVar + " is fomated incorrectly.\n It must be a valid decimal number\n");
				MessageDialog.showMessage ("Unknown exception parsing " + messageVar + " to decimal number\n" + ex.Message);
				return null;
			}


			return dec;
		}

		public static UInt64? parseUInt64 (String str, String messageVar)
		{
			try {

					UInt64? amountl = Convert.ToUInt64( str );
					return amountl;
				}

				catch (FormatException ex) {

					MessageDialog.showMessage (messageVar + " is fomated incorrectly for sending drops.\n It must be a valid integer\n");
					return null;

				}

				catch (OverflowException ex) {
					MessageDialog.showMessage (messageVar + " is greater than an unsignd long. No one's got that much money\n");
					return null;
				}

				catch (Exception ex) {
					MessageDialog.showMessage (messageVar + "Unknown error formatting " + messageVar + "\n");
					return null;
				}
		}

		public static UInt32? parseUInt32 (String str, String messageVar)
		{
			try {

					UInt32? amountl = Convert.ToUInt32( str );
					return amountl;
				}

				catch (FormatException ex) {

					MessageDialog.showMessage (messageVar + " is fomated incorrectly.\n It must be a valid unsigned 32 bit integer\n");
					return null;

				}

				catch (OverflowException ex) {
					MessageDialog.showMessage (messageVar + " is greater than an unsigned int 32\n");
					return null;
				}

				catch (Exception ex) {
					MessageDialog.showMessage (messageVar + "Unknown error formatting " + messageVar + "\n");
					return null;
				}
		}



	}
}

