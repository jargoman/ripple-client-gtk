/*
 *	License : Le Ice Sense 
 */

using System;
using System.Threading;
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
			if (Debug.SendIOU) {
				Logging.write("SendIOU : const");
			}

			this.Build ();

			if (Debug.SendIOU) {
				Logging.write("SendIOU : Build complete");
			}

			// lol all these didn't work :/
			//this.currencycomboboxentry.Changed += new EventHandler (this.OnCurrencycomboboxentryChanged);
			//this.currencycomboboxentry.EditingDone += new EventHandler (this.OnCurrencycomboboxentryChanged);
			//this.currencycomboboxentry.SelectionReceived += //this.OnCurrencycomboboxentryChanged;
			this.currencycomboboxentry.Entry.Activated += new EventHandler(this.OnCurrencycomboboxentryChanged);
			this.currencycomboboxentry.SelectionReceived += new Gtk.SelectionReceivedHandler (this.OnCurrencycomboboxentryReceived);


			//this.currencycomboboxentry.PropertyNotifyEvent += new Gtk.PropertyNotifyEventHandler (this.OnCurrencycomboboxentryChanged);
			//this.currencycomboboxentry.SelectionNotifyEvent += OnCurrencycomboboxentryChanged;



			this.issuerentry.Entry.Activated += new EventHandler (this.OnIssuerentryActivated);

			this.issuerentry.SelectionReceived += new Gtk.SelectionReceivedHandler (this.OnSelectionReceivedEvent);
			//this.issuerentry.Changed
			this.amountentry.Activated += new EventHandler (this.OnAmountentryActivated);
			this.sendMaxEntry.Activated += new EventHandler (this.OnSendMaxEntryActivated);
			this.destinationentry.Activated += new EventHandler (this.OnDestinationentryActivated);

			this.sendIOUButton.Clicked += new EventHandler (this.OnSendIOUButtonClicked);
			//this.ChooseButton.Clicked += new EventHandler(this.OnChooseButtonClicked);

			//this.issuerentry.

			currentInstance = this;

		}



		public static SendIOU currentInstance = null;
		//private double highestLedger = 0;

		 
		public static void sendIOUPayment ( String account, String destination, decimal amount, String secret, String currency, String issuer, decimal sendmax, decimal fee ) {
			if (Debug.SendIOU) {
				Logging.write("SendIOU : sendIOUPayment" );

			}

			RippleSeedAddress seed = null;
			RippleAddress payee = null;
			RippleAddress issu = null;
			RippleAddress payer = null;

			try {
				seed = new RippleSeedAddress(secret);


			} catch (Exception exc) {
				MessageDialog.showMessage("Invalid Secret\n" + exc.Message);
				return;
			}

			try {
				payee = new RippleAddress(destination);

			} catch (Exception exc) {
				MessageDialog.showMessage("Invalid destination address\n" + exc.Message);
				return;
			}

			try {

				issu = new RippleAddress(issuer);

			} catch (Exception exc) {
				MessageDialog.showMessage("Invalid currency issuer\n" + exc.Message);
				return;
			}

			try {

				payer = new RippleAddress(account);
				if (Debug.SendIOU) {
					Logging.write(payer.ToString());
				}


			} catch (Exception exc) {
				MessageDialog.showMessage("Invalid account address\n" + exc.Message);
				return;
			}

			if (payer!=seed.getPublicRippleAddress()) {
				// TODO make an appropriate message and debug
				MessageDialog.showMessage("Account and secret don't match. Bug?");
				return;
			}

			if (Debug.SendIOU) {
				Logging.write("SendIOU : sendIOUPayment : preparing to send payment");
			}

			DenominatedIssuedCurrency amnt = new DenominatedIssuedCurrency(amount,issu,currency);
			DenominatedIssuedCurrency dafee = new DenominatedIssuedCurrency(fee);

			DenominatedIssuedCurrency sndmx = null; //new DenominatedIssuedCurrency(sendmax,issu,currency);



			if (Debug.SendIOU) {
				Logging.write("amnt = " + amnt.ToString() + ", dafee = " + dafee.ToString() + ", sndmx = " + ((sndmx == null) ? "null" : sndmx.ToString()));
			}

			RipplePaymentTransaction tx = new RipplePaymentTransaction(seed.getPublicRippleAddress(),payee,amnt,dafee, MainWindow.currentInstance.sequence,sndmx); // Todo implement sequemce number. int 23 is an arbatrary number for testing 
			if (part) { 
				tx.flags |= tx.tfPartialPayment;
			}
			//RippleBinaryObject rbo = tx.getBinaryObject().getObjectSorted();
			//rbo = new RippleSigner(seed.getPrivateKey(0)).sign(rbo);

			//byte[] signedTXBytes = new BinarySerializer().writeBinaryObject(rbo).ToArray();

			//String blob = Base58.ByteArrayToHexString(signedTXBytes);

			//object ob = new {
			//	command = "submit",
			//	tx_blob = blob
			//};

			tx.sign(seed);

			tx.submit();

		}


		public void sendIOU ()
		{
			if (Debug.SendIOU) {
				Logging.write("SendIOU : method sendIOU begin");
			}

			if (MainWindow.currentInstance==null) {
				return;
			}

			RippleWallet rw = MainWindow.currentInstance.getRippleWallet();

			if (rw == null || rw.seed == null) {
				// todo debug
				return;
			}

			String issuer = this.issuerentry.ActiveText;

			//this.issuerentry.ActiveText;

			String account = MainWindow.currentInstance.getRippleWallet().getStoredReceiveAddress(); //.getReceiveAddress ();
			String destination = this.destinationentry.Text;

			String amount = this.amountentry.Text;
			String secret = rw.seed.ToString();

			String currency = currencycomboboxentry.ActiveText;

			String sendmax = this.sendMaxEntry.Text;



			if (account == null || account.Equals("")) {
				// no need to show warning dialog because MainWindow.currentInstance.getReceiveAddress (); does so, simply return
				return;
			}

			account = account.Trim();

			if (secret == null) {
				return;
			}

			secret = secret.Trim();

			if (destination == null || destination.Trim().Equals("")) {
				MessageDialog.showMessage("Please enter a destination address");
				return;
			}
			destination = destination.Trim();

			if (currency == null || currency.Trim().Equals("")) {
				MessageDialog.showMessage("Please choose a currency to send");
				return;
			}
			currency = currency.Trim();

			if (amount == null || amount.Trim().Equals("")) {
				MessageDialog.showMessage("Please enter an amount of " + currency + " to send");
			}
			amount = amount.Trim();

			/* // Maybe I should allow self payments??
			if (account.Equals(destination)) {
				MessageDialog.showMessage ("You're trying to send to yourself?");
				return;
			}
			*/ 

			if (Debug.SendIOU) {
				Logging.write(
					"SendIOU : method sendIOU()\n\taccount = " + account +
					"\n\tsecret = " + (Debug.allowInsecureDebugging ? secret : "--masked--") +
					"\n\tdestination = " + destination +
					"\n\tamount = " + amount +
					"\n\tcurrency = " + currency +
					"\n\tissuer = " + issuer +
					"\n\tsendmax = " + ((sendmax == null) ? "null" : sendmax)+ "\n"
					);
			}

			Thread th = new Thread( new ParameterizedThreadStart(sendIOUThread));

			threadParam par = new threadParam(amount,destination,account,secret,currency,issuer,sendmax);

			th.Start(par);

		}

		private static void sendIOUThread (object param)
		{
			if (Debug.SendIOU) {
				Logging.write ("SendIOU : sendIOUThread begin");
			}


			threadParam tp = param as threadParam;
			Decimal max = 0;
			Decimal amountd = 0;

			if (tp == null) {
				throw new InvalidCastException ("Unable to cast object to type threadParam");
			}

			if (Debug.SendIOU) {

				//Logging.write("Units = " + tp.currency);

				Logging.write ("Send IOU : requesting Server Info\n");
			}
				
			ServerInfo.refresh_blocking ();

			if (Debug.SendIOU) {
				Logging.write ("Send IOU : refresh_blocking returned, ServerInfo.transaction_fee = " + ServerInfo.transaction_fee.ToString ());
			}

				
			//amountd = 
				

			Decimal? dee = DenominatedIssuedCurrency.parseDecimal (tp.amount, "Send Amount");  // send amount is not a valid string ect
			if (dee != null) {
				amountd = (decimal)dee;
			} else {
				return;
			}
				
			if (amountd < 0) {
				MessageDialog.showMessage("Sending negative amounts is not supported. Please enter a valid amount");
				return;
			}

			if (tp.sendmax == null) {
				tp.sendmax = "";
				if (Debug.SendIOU) {
					Logging.write("Setting sendmax to blank value because it was null");
				}
			}

			if (!("".Equals (tp.sendmax.Trim ()))) { // if sendmax is not blank

					
				Decimal? m = DenominatedIssuedCurrency.parseDecimal (tp.sendmax.Trim(), "SendMax");  // and is a valid number
					
				if (m == null) {
					return;
				}

				max = (Decimal)m;

			} else {
				max = amountd;
			}

			sendIOUPayment(tp.account,tp.destination,amountd,tp.secret,tp.currency,tp.issuer,max,new decimal(ServerInfo.transaction_fee));

		}

		class threadParam 
		{
			public threadParam(String amount, String destination, String account, String secret, String currency, String issuer, String sendmax) {
				this.amount = amount;
				this.destination = destination;
				this.account = account;
				this.secret = secret;
				this.currency = currency;
				this.sendmax = sendmax;
				this.issuer = issuer;
			}

			public String amount;
			public String destination;
			public String account;
			public String secret;
			public String currency;
			public String sendmax;
			public String issuer;
		}


		protected void OnSendIOUButtonClicked (object sender, EventArgs e)
		{

			sendIOU ();
		}

		/*
		protected void OnChooseButtonClicked (object sender, EventArgs e)
		{

		}
		*/

		public void updateBalance () {

			Gtk.Application.Invoke (
				delegate {

				Dictionary<String, Decimal> cash = AccountLines.cash;

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
					Decimal dud;

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

		private void updateCurrencyIssuers ()
		{
			Gtk.Application.Invoke ( delegate {

		
				String cur = this.currencycomboboxentry.ActiveText;

				List<String>  lis = AccountLines.getIssuersForCurrency(cur);

				ListStore store = new ListStore(typeof(string));

				foreach (String s in lis) {
					store.AppendValues(s);
				}

				this.issuerentry.Model = store;
			});

		}

		// no idea why this won't fire ????
		void OnCurrencycomboboxentryReceived (object sender, EventArgs e)
		{
			this.updateBalance ();
			//this.issuerentry.GrabFocus();

			this.updateCurrencyIssuers();
			this.issuerentry.Entry.GrabFocus();

		}

		protected void OnCurrencycomboboxentryChanged (object sender, EventArgs e)
		{
			this.updateBalance ();
			//this.issuerentry.GrabFocus();

			this.updateCurrencyIssuers();
			this.issuerentry.Entry.GrabFocus();
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

				//foreach (String s in currencies) {
				//	store.AppendValues(s);
				//}
				if (currencies!=null) {
					store.AppendValues(currencies);
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


		void OnSelectionReceivedEvent (object sender, EventArgs e)
		{
			if (this.amountentry == null) {
				return;
			}

			this.amountentry.GrabFocus ();

			//throw new NotImplementedException ();
		}

		protected void currencychanged (object sender, EventArgs e)
		{
			this.updateBalance ();
			//this.issuerentry.GrabFocus();

			this.updateCurrencyIssuers();
			this.issuerentry.Entry.GrabFocus();
		}



		static bool part = true;

		protected void partialtoggled (object sender, EventArgs e)
		{
			CheckButton c = sender as CheckButton;
			if (c!=null) {
				part = c.Active;
			}
		}

	}
}

