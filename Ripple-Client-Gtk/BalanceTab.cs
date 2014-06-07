using System;

namespace RippleClientGtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class BalanceTab : Gtk.Bin
	{
		public BalanceTab ()
		{

			this.Build ();

			wedgies = new CurrencyWidget[] { this.currencywidget1, this.currencywidget2, this.currencywidget3, this.currencywidget4, this.currencywidget5, this.currencywidget6, this.currencywidget7, this.currencywidget8 };


			BalanceTab.currentInstance = this;

		}

		CurrencyWidget[] wedgies = null;


		public void set ()
		{
			if (BalanceTabOptionsWidget.actual_values == null) {
				BalanceTabOptionsWidget.actual_values = BalanceTabOptionsWidget.default_values;
			}

			set (BalanceTabOptionsWidget.actual_values);
		}

		public void set (String[] str)
		{
			// this sets the balance as well. 
			Gtk.Application.Invoke( delegate {

		

			if (this.currencywidget1 != null) {
				this.currencywidget1.set ( "XRP", Base58.truncateTrailingZerosFromString(MainWindow.currentInstance.xrpBalance.ToString () ));
			} else {
				return;
			}

			if (this.currencywidget2 != null) {
				this.currencywidget2.set ("ICE");
			} else {
				return;
			}

			for (int i = 0; i < 6; i++) {
				int x = i + 2;  // only wedgies get the x 
				if (wedgies[x]!=null) {
					if (str!=null && str.Length > i) {
						try {
							wedgies[x].set(str[i]);
						}
						catch (Exception e) {
							wedgies[x].set(BalanceTabOptionsWidget.default_values[i]);
						}
					}

					else {
						wedgies[x].set(BalanceTabOptionsWidget.default_values[i]);
					}
				}

				else {
					break; // the ui isn't built
				}
			}


			}); // end delegate


		} // end public set() (str[] str) 


		public static BalanceTab currentInstance = null;

	}
}

