using System;
using Gtk;
using System.Collections.Generic;
using Codeplex.Data;

namespace RippleClientGtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class SendIOU : Gtk.Bin
	{
		public SendIOU ()
		{
			this.Build ();


			this.currencycomboboxentry.Changed += new EventHandler (this.OnCurrencycomboboxentryChanged);
			this.issuerentry.Activated += new EventHandler (this.OnIssuerentryActivated);
			this.amountentry.Activated += new EventHandler (this.OnAmountentryActivated);
			this.sendMaxEntry.Activated += new EventHandler (this.OnSendMaxEntryActivated);
			this.destinationentry.Activated += new EventHandler (this.OnDestinationentryActivated);

			this.sendIOUButton.Clicked += new EventHandler (this.OnSendIOUButtonClicked);

			currentInstance = this;

		}
		public static SendIOU currentInstance = null;
		//private double highestLedger = 0;

		protected void sendIOUPayment ( String account, String destination, double amount, String secret, String currency, String issuer, double sendmax ) {

			object obj = new 
			{
				command = "submit",
				tx_json = new
				{
					TransactionType = "Payment",
					Account = account,
					Destination = destination,
					Amount = new 
					{
						currency = currency,
						value = amount.ToString(),
						issuer = issuer
					},
					SendMax = new
					{
						currency = currency,
						value = sendmax.ToString(),

					}
				},
				secret = secret
			};

			String json = DynamicJson.Serialize (obj);

			Logging.write ("Sending IOU payment: " + json + "\n");

			if (NetworkInterface.currentInstance != null) {

				AreYouSure ays = new AreYouSure ("You are about to send " + amount.ToString () + " " + currency + " to address " + destination + " spending a maximum of " + sendmax.ToString () + " " + currency);

				int resp = ays.Run ();
				ays.Destroy ();

				if (resp == (int)ResponseType.Ok) {
					NetworkInterface.currentInstance.sendToServer (json);

				} else {
					// user canseled
					return;
				}

			} else {
				MessageDialog.showMessage ("To send an IOU you need to be connected to a server. Please go to network settings and connect");
			}
		}


		public void sendIOU () {
			String issuer = this.issuerentry.Text;

			String account = MainWindow.currentInstance.getReceiveAddress ();
			String destination = this.destinationentry.Text;

			String amount = this.amountentry.Text;
			String secret = MainWindow.currentInstance.getSecret ();

			String currency = currencycomboboxentry.ActiveText;

			String sendmax = this.sendMaxEntry.Text;



			if (account == null) {
				// no need to show warning dialog because MainWindow.currentInstance.getReceiveAddress (); does so, simply return

				return;
			}

			if (secret == null) {
				return;
			}

			/* // Maybe I should allow self payments??
			if (account.Equals(destination)) {
				MessageDialog.showMessage ("You're trying to send to yourself?");
				return;
			}
			*/ 

		

				try {

					double amountd = Convert.ToDouble(amount);

					if (amountd < 0) {
						MessageDialog.showMessage("Sending negative amounts is not supported. Please enter a valid amount");
						return;
					}

				Double max = 0;

				if (!("".Equals (sendmax.Trim ()))) { // if sendmax is not blank

					try {
						max = Convert.ToDouble (sendmax);  // and is a valid number
					} catch (FormatException ex) {

						MessageDialog.showMessage ("SendMax is fomated incorrectly for sending an IOU. It must be a valid decimal number or left blank");
						return;

					} catch (OverflowException ex) {
						MessageDialog.showMessage ("SendMax is greater than a double? No one's got that much money");
						return;
					} catch (Exception ex) {
						MessageDialog.showMessage ("Amount is fomated incorrectly for sending an IOU. It must be a valid decimal number or left blank");
						return;
					}

				} else {
					max = amountd;
				}



					
					this.sendIOUPayment(account, destination, amountd, secret, currency, issuer, max);

				}

				catch (FormatException ex) {

					MessageDialog.showMessage ("Amount is fomated incorrectly for sending an IOU.\n It must be a valid decimal number\n");
					return;

				}

				catch (OverflowException ex) {
					MessageDialog.showMessage ("Send amount is greater than a double? No one's got that much money\n");
					return;
				}

				catch (Exception ex) {
					MessageDialog.showMessage ("Amount is fomated incorrectly for sending an IOU.\n It must be a valid decimal number\n");
					return;
				}


		}


		protected void OnSendIOUButtonClicked (object sender, EventArgs e)
		{

			sendIOU ();
		}

		public void updateBalance () {

			Gtk.Application.Invoke (
				delegate {

				Dictionary<String, Double> cash = AccountLines.cash;

				if (cash == null) {
					// 
					MessageDialog.showMessage ("You need to sync an account with the network to access this functionality\n"); // need to sync
					return;
				}

				if (this.currencycomboboxentry==null) {
					// TODO bug

					return;
				}

				String cur = this.currencycomboboxentry.ActiveText;

				if (cash.ContainsKey (cur)) {
					double dud;

					if (cash.TryGetValue (cur, out dud)) {

						this.balancelabel.Text = dud.ToString();

					} else {
						// TODO debug
					}


				} // end if cash contains combobox text 
					
				else {
					// TODO debug/alert user
				}

			} // end delegate
			);  // end invoke


		} // end public void updateBalance

		protected void OnCurrencycomboboxentryChanged (object sender, EventArgs e)
		{
			this.updateBalance ();
		}

		protected void OnIssuerentryActivated (object sender, EventArgs e)
		{
			if (this.amountentry == null) {
				return;
			}

			this.amountentry.GrabFocus ();
		}

		protected void OnAmountentryActivated (object sender, EventArgs e)
		{
			if (sendMaxEntry==null) {
				// TODO
				return;
			}

			this.sendMaxEntry.GrabFocus ();
		}

		protected void OnDestinationentryActivated (object sender, EventArgs e)
		{
			if (this.sendIOUButton == null) {
				return;
			}

			this.sendIOUButton.GrabFocus ();
		}

		public void setCurrencies (String[] currencies) {
			Gtk.Application.Invoke (delegate {

				ListStore store = new ListStore(typeof(string));

				foreach (String s in currencies) {
					store.AppendValues(s);
				}

				this.currencycomboboxentry.Model = store;


			});
		}

		protected void OnSendMaxEntryActivated (object sender, EventArgs e)
		{
			if (this.destinationentry==null) {
				return;
			}

			this.destinationentry.GrabFocus ();
		}
	}
}

