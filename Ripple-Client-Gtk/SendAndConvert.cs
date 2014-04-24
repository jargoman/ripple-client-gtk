/*
 *	License : Le Ice Sense 
 */

using System;
using Microsoft.CSharp.RuntimeBinder;
using Gtk;
using System.Collections.Generic;
using Codeplex.Data;

namespace RippleClientGtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class SendAndConvert : Gtk.Bin
	{
		public SendAndConvert ()
		{
			this.Build ();

			//this.issuerentry.enActivated += new EventHandler (this.OnIssuerEntryActivated);

			this.issuerentry.Entry.Activated += new EventHandler (this.OnIssuerEntryActivated);
			this.issuerentry.SelectionReceived += new Gtk.SelectionReceivedHandler(this.onIssuerSelection);


			this.destinationentry.Activated += new EventHandler (this.OnDestinationEntryActivated);
			this.sendmaxentry.Activated += new EventHandler (this.OnSendMaxEntryActivated);
			this.receiveamountentry.Activated += new EventHandler (this.OnReceiveAmountEntryActivated);

			this.sendbutton.Clicked += new EventHandler (this.OnSendButtonClicked);

			currentInstance = this;
		}

		public void setCurrencies (String[] currencies) {
			Gtk.Application.Invoke (delegate {

				ListStore store = new ListStore(typeof(string));

				foreach (String s in currencies) {
					store.AppendValues(s);
				}


				comboboxentry.Model = store;


			});
		}

		public static SendAndConvert currentInstance = null;


		public void updateBalance () {

			Gtk.Application.Invoke (
				delegate {

				Dictionary<String, Decimal> cash = AccountLines.cash;

				if (cash == null) {
					// 
					MessageDialog.showMessage ("what to say :P"); // need to sync
					return;
				}

				if (this.comboboxentry==null) {
					// TODO bug

					return;
				}

				String cur = this.comboboxentry.ActiveText;

				if (cash.ContainsKey (cur)) {
					decimal dud;

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


		protected void OnComboboxentryChanged (object sender, EventArgs e)
		{
			this.updateBalance ();
			this.updateCurrencyIssuers();
		}

		protected void OnIssuerEntryActivated (object sender, EventArgs e)
		{
			// issuer entry
			if (this.destinationentry == null) {
				// TODO debug
				return;
			}

			this.destinationentry.GrabFocus ();
		}

		protected void onIssuerSelection (object o, SelectionReceivedArgs args)
		{
			if (this.destinationentry == null) {
				// TODO debug
				return;
			}

			this.destinationentry.GrabFocus ();
		}
		protected void OnDestinationEntryActivated (object sender, EventArgs e)
		{

			// Destination
			if (sendmaxentry == null) {
				//TODO debug

				return;
			}

			this.sendmaxentry.GrabFocus ();
		}

		protected void OnSendMaxEntryActivated (object sender, EventArgs e)
		{
			if (this.receiveamountentry == null){
				// TODO degub
				return;
			}

			this.receiveamountentry.GrabFocus ();
		}

		protected void OnReceiveAmountEntryActivated (object sender, EventArgs e)
		{
			//throw new NotImplementedException ();
			if (this.sendbutton==null) {
				// TODO debug
				return;
			}

			this.sendbutton.GrabFocus ();
		}

		protected void OnSendButtonClicked (object sender, EventArgs e)
		{
			send ();
		}

		private void send () {

			String issuer = this.issuerentry.Entry.Text;

			String account = MainWindow.currentInstance.getReceiveAddress ();
			String destination = this.destinationentry.Text;

			String amount = this.receiveamountentry.Text; 
			String secret = MainWindow.currentInstance.getSecret ();

			String currency = this.comboboxentry2.ActiveText;
			String sendmax = this.sendmaxentry.Text; 

			String receiveCurrency = this.comboboxentry.ActiveText;

			if (account == null) {
				// no need to show warning dialog because MainWindow.currentInstance.getReceiveAddress (); does so, simply return

				return;
			}

			if (secret == null) {
				return;
			}

			/* // Maybe I should allow self payments?? // yes allow self payments for self converting payments/. 
			if (account.Equals(destination)) {
				MessageDialog.showMessage ("You're trying to send to yourself?");
				return;
			}
			*/ 



			try {

				decimal amountd = Convert.ToDecimal(amount);

				if (amountd < 0) {
					MessageDialog.showMessage("Sending negative amounts is not supported. Please enter a valid amount");
					return;
				}

				decimal max = 0;

				if (!("".Equals (sendmax.Trim ()))) { // if sendmax is not blank

					try {
						max = Convert.ToDecimal( sendmax ); //Convert.ToDouble (sendmax);  // and is a valid number
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




				this.sendConvertPayment(account, destination, amountd, secret, currency, issuer, max, receiveCurrency);

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

		protected void sendConvertPayment ( String account, String destination, decimal amount, String secret, String currency, String issuer, decimal sendmax, String destcurrency ) {
			DenominatedIssuedCurrency Amount = null;
			DenominatedIssuedCurrency SendMax = null;
			DenominatedIssuedCurrency Fee = null;


				if ("XRP".Equals (currency)) {

					if ("XRP".Equals(destcurrency)) {
						MessageDialog.showMessage ("Request denied. To send xrp to xrp just use the send ripples tab.");
						return;
					}

					decimal amnt = amount * 1000000m;

					decimal floored = Math.Floor( amnt );

					if ( !(amnt.Equals( floored ))) {
						// User entered too many decimals. 
						MessageDialog.showMessage ("Warning, you entered too many decimals. Amount " + amnt.ToString() + " XRP will truncated to " + floored.ToString());
						amnt = floored;
					}


					Amount = new DenominatedIssuedCurrency(amnt);

				} else {

					Amount = new DenominatedIssuedCurrency (amount, new RippleAddress( issuer), currency);
				}


				if ("XRP".Equals(destcurrency)) {
					decimal snd = sendmax * 1000000m;
					decimal floor = Math.Floor (snd);

					if (!(snd.Equals(floor))) {
						MessageDialog.showMessage ("Warning, you entered too many decimals. SendMax " + snd.ToString() + " XRP will truncated to " + floor.ToString());
						snd = floor;
					}


					SendMax = new DenominatedIssuedCurrency (snd);

				} else {


					SendMax = new DenominatedIssuedCurrency (sendmax, new RippleAddress(account), destcurrency );
				}
			

				if (Amount == null || SendMax == null) {
					Logging.write ("SendAndConvert : Error : Either Amount or SendMax is null\n");
					return;
				}

				RipplePaymentTransaction tx = new RipplePaymentTransaction(
					new RippleAddress(account),
					new RippleAddress (destination),
					Amount,
					Fee,
					MainWindow.currentInstance.sequence,
					SendMax
				);

				RippleSeedAddress seed = new RippleSeedAddress (secret );
				tx.sign(seed);
				tx.submit();


			
		} // end sendConvertPayment

		private void updateCurrencyIssuers ()
		{
			Gtk.Application.Invoke ( delegate {

		
				String cur = this.comboboxentry.ActiveText;

				List<String>  lis = AccountLines.getIssuersForCurrency(cur);

				ListStore store = new ListStore(typeof(string));

				foreach (String s in lis) {
					store.AppendValues(s);
				}

				this.issuerentry.Model = store;
			});

		}

	} // end class
} // end namespace

