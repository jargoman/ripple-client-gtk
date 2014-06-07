using System;

namespace RippleClientGtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class CurrencyWidget : Gtk.Bin
	{
		public CurrencyWidget ()
		{
			this.Build ();
		}

		/* note : there is subtle differences between the the 
		 * various set functions
		 * 
		 * 
		 */


		public void set (DenominatedIssuedCurrency dic)
		{
			this.set(dic.currency, dic.amount.ToString());
		}


		public void set (String currency, String amount) {
				Gtk.Application.Invoke (
				delegate {

				this.label3.Text = currency;
				this.label4.Text = amount;

			});

		}

		public void set (String currency) {


			this.set(currency, Base58.truncateTrailingZerosFromString(AccountLines.getCurrencyTotal(currency).ToString()));
		}

		// 
		public void setAsUnset (String currency) {
			this.set(currency, "  --  Unsynced-- ");
		}
	}
}

