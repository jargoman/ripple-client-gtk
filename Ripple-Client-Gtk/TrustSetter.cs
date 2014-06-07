using System;

namespace RippleClientGtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class TrustSetter : Gtk.Bin
	{
		public TrustSetter ()
		{
			if (Debug.TrustSetter) {
				Logging.write(clsstr + "new");
			}
			this.Build ();
			if (Debug.TrustSetter) {
				Logging.write(clsstr + "build complete");
			}

			this.button2.Clicked += delegate(object sender, EventArgs e) {
				if (Debug.TrustSetter) {
					Logging.write("Set Trust Button clicked");
				}
				setTrust();
			};
		}

		private String clsstr = "TrustSetter : ";
		public void setTrust ()
		{
			String method_sig = clsstr + "setTrust : ";
			if (Debug.TrustSetter) {
				Logging.write (method_sig + "begin");
			}
			DenominatedIssuedCurrency cur = getDenominated ();

			if (cur == null) {
				if (Debug.TrustSetter) {
					Logging.write(method_sig + "Denominated Currency equals null");
				}
				return;
			}

			String i = comboboxentry4.ActiveText;
			UInt32? qin = processQuality(i,"in");

			String o = comboboxentry4.ActiveText;
			UInt32? qout = processQuality(o,"out");

			if (MainWindow.currentInstance==null) {
				// todo debug
				return;
			}

			RippleWallet rw = MainWindow.currentInstance.getRippleWallet();
			if (rw == null) {
				// todo debug
				return;
			}

			RippleSeedAddress seed = rw.seed;

			if (seed == null) {
				if (Debug.TrustSetter) {
					Logging.write(method_sig + "seed == null");
				}
				return;
			}





			// todo fee
			//RippleTrustSetTransaction trustx = new RippleTrustSetTransaction(seed.getPublicRippleAddress(),cur, qin,qout,MainWindow.currentInstance.sequence, new decimal(ServerInfo.transaction_fee ));
			RippleTrustSetTransaction trustx = new RippleTrustSetTransaction( seed.getPublicRippleAddress(),cur, qin, qout, MainWindow.currentInstance.sequence, new DenominatedIssuedCurrency(new Decimal(ServerInfo.transaction_fee)));
			trustx.sign(seed);

			trustx.submit();



		}

		private UInt32? processQuality (String value, String inout)
		{
			String method_sig = clsstr + "processQuality( value=" + (String)(value == null ? "null" : value) + ", inout="  + (String)(inout == null ? "null" : inout) + ") : ";
			if (Debug.TrustSetter) {
				Logging.write(method_sig + "begin"); 
			}
			//String i = comboboxentry4.ActiveText;
			UInt32? q = DenominatedIssuedCurrency.parseUInt32(value, "Quality " + inout);
			if (q == null) {
				// todo alert
				if (Debug.TrustSetter) {
					Logging.write(method_sig + "q == null, returning null");
				}
				return null;
			}

			if ( q > 1999999999) {
				// todo alert user
				if (Debug.TrustSetter) {
					Logging.write(method_sig + "q > 1999999999");
				}
				return null;
			}

			if (Debug.TrustSetter) {
				Logging.write(method_sig + "Quality " + inout + " =" + q.ToString());
			}

			return q;


		}

		public DenominatedIssuedCurrency getDenominated ()
		{
			String method_sig = clsstr + "getDenominated() : ";
			if (Debug.TrustSetter) {
				Logging.write(method_sig + "begin");
			}
			RippleAddress issuerAddress;

			String currency = comboboxentry1.ActiveText;
			if (currency == null) {
				// todo debug
				if (Debug.TrustSetter) {
					Logging.write(method_sig + "currency == null");
				}

				return null;
			}

			currency = currency.Trim();

			if (currency.Equals("")) {
				// todo debug
				if (Debug.TrustSetter) {
					Logging.write(method_sig + "currency is empty");
				}
				return null;
			}

			String issuer = comboboxentry2.ActiveText;
			if (issuer == null) {
				 // todo debug
				if (Debug.TrustSetter) {
					Logging.write(method_sig + "issuer == null");
				}
				return null;
			}

			issuer = issuer.Trim();

			try {
				issuerAddress = new RippleAddress(issuer);
			}

			catch (FormatException e) {
				Logging.write(method_sig + e.Message);

				MessageDialog.showMessage("Issuer address error : " + e.Message);
				return null;
			}


			String amount = comboboxentry3.ActiveText;
			if (amount == null) {
				if (Debug.TrustSetter) {
					Logging.write(method_sig + "amount == null");
				}
				return null;
			}

			amount = amount.Trim();

			if (amount.Equals("")) {
				// todo debug
				if (Debug.TrustSetter) {
					Logging.write(method_sig + "amount is empty");
				}
				return null;
			}

			Decimal? d = DenominatedIssuedCurrency.parseDecimal(amount, "Amount");
			if (d==null) {
				// todo
				if (Debug.TrustSetter) {
					Logging.write(method_sig + "Unable to parse amount");
				}
				return null;
			}

			try {
				DenominatedIssuedCurrency denar = new DenominatedIssuedCurrency((Decimal)d,issuerAddress,currency);

				if (Debug.TrustSetter) {
					Logging.write("erase me");
				}
				return denar;
			}

			catch (Exception e) {
				Logging.write(e.Message);
				return null;
			}

		}

	}
}

