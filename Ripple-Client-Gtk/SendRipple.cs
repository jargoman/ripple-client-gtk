using System;
using System.Threading;
using Gtk;
using Codeplex.Data;

namespace RippleClientGtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class SendRipple : Gtk.Bin
	{
		public SendRipple ()
		{
			this.Build ();

			this.balanceLabel.Text = ReceiveWidget.UNSYNCED; // unsynced;

			this.unitsSelectBox.Changed += OnUnitsSelectBoxChanged;

		}

		//String unsynced = "   --   unsynced   --   ";

		public static void sendXrpPayment ( String account, String destination, Decimal xrpamount, Decimal fee, String secret) {

			xrpamount = xrpamount * 1000000m; // convert to drops

			try {

				ulong lamount = (ulong) xrpamount;
				sendDropsPayment(account, destination, (Decimal)lamount, fee, secret);
			}

			catch (OverflowException ex) {

				if (Debug.SendRipple) {
					Logging.write("OverflowException : can't convert xrp to drops because value can't fit inside unsigned long");
				}

				MessageDialog.showMessage("OverflowException : can't convert xrp to drops because value can't fit inside unsigned long");

				return;
			}

			catch (Exception ex) {
				if (Debug.SendRipple) {
					Logging.write("Exception thrown while converting to drops : " + ex.Message);
				}

				MessageDialog.showMessage("Exception thrown while converting to drops : " + ex.Message);
					return;
			}



		}


		public static void sendDropsPayment ( String account, String destination, Decimal xrpamount, Decimal fee, String secret) {

			if (Debug.SendRipple) {
				Logging.write("send drops payment of " + xrpamount.ToString() + " drops");
			}

			RippleSeedAddress seed = new RippleSeedAddress(secret);

			RippleAddress payee = new RippleAddress(destination);

			RippleAddress payer = new RippleAddress(account);

			if (payer!=seed.getPublicRippleAddress()) {
				// TODO debug
			}

			DenominatedIssuedCurrency amnt = new DenominatedIssuedCurrency(xrpamount);
			DenominatedIssuedCurrency dafee = new DenominatedIssuedCurrency(fee);

			RipplePaymentTransaction tx = new RipplePaymentTransaction(seed.getPublicRippleAddress(),payee,amnt,dafee, MainWindow.currentInstance.sequence,null); // Todo implement sequemce number. int 23 is an arbatrary number for testing 

			tx.sign(seed);
			tx.submit();

			//RippleBinaryObject rbo = tx.getBinaryObject().getObjectSorted();
			//rbo = new RippleSigner(seed.getPrivateKey(0)).sign(rbo);

			//byte[] signedTXBytes = new BinarySerializer().writeBinaryObject(rbo).ToArray();

			//String blob = Base58.ByteArrayToHexString(signedTXBytes);

			//object ob = new {
			//	command = "submit",
			//	tx_blob = blob
			//};

			//String jso = DynamicJson.Serialize(ob);



			//NetworkInterface.currentInstance.sendToServer(jso);

		}



		protected void OnSendXRPButtonClicked (object sender, EventArgs e)
		{
			if (MainWindow.currentInstance == null) {
				return;
			}

			RippleWallet rw = MainWindow.currentInstance.getRippleWallet();

			if (rw==null) {
				return;
			}

			if (rw.seed == null) {
				// TODO TOO F'N DOOOO!!!
			}

			Thread th = new Thread( new ParameterizedThreadStart(sendThread));

			String amount = this.amountcomboboxentry.ActiveText;		//this.amountEntry.Text; // used to be a text entry
			String destination = this.destinationcomboboxentry.ActiveText;		//this.destinationentry.Text;

			String account = MainWindow.currentInstance.getRippleWallet().getStoredReceiveAddress();//getReceiveAddress ();
			String secret = rw.seed.ToString();

			if (destination == null) {

				return;
			} else {
				if (Debug.SendRipple) {
					Logging.write("SendRipple.OnSendXRPButtonClicked : destination = " + destination.ToString());
				}
			}

			if (account == null) {
				// no need to show dialog MainWindow.currentInstance.getReceiveAddress (); does so, simply return

				return;
			}

			if (secret == null) {
				return;
			}

			if (account.Equals(destination)) {
				MessageDialog.showMessage ("You're trying to send xrp to yourself?");
				return;
			}

			String units = this.unitsSelectBox.ActiveText;

			threadParam tp = new threadParam (amount, destination,account,secret,units);

			th.Start(tp);

		}

		class threadParam 
		{
			public threadParam(String amount, String destination, String account, String secret, String units) {
				this.amount = amount;
				this.destination = destination;
				this.account = account;
				this.secret = secret;
				this.units = units;
			}

			public String amount;
			public String destination;
			public String account;
			public String secret;
			public String units;
		}

		private static void sendThread (object param) {

			threadParam tp = param as threadParam;



			if (tp == null) {
				throw new InvalidCastException("Unable to cast object to type threadParam");
			}



			if (Debug.SendRipple) {

				Logging.write("Units = " + (string)((tp.units == null) ? "null" : tp.units));

				Logging.write("Send Ripple : requesting Server Info\n");
			}

			ServerInfo.refresh_blocking();

			if (Debug.SendRipple) {
				Logging.write("Send Ripple : refresh_blocking returned, ServerInfo.transaction_fee = " + ServerInfo.transaction_fee.ToString());
			}




			if ("drops".Equals(tp.units)) {
				Logging.write("drops");


				ulong? amountl = DenominatedIssuedCurrency.parseUInt64(tp.amount, "Amount of drops");
				if (amountl!=null) {
					sendDropsPayment(tp.account,tp.destination,(decimal)amountl,new decimal(ServerInfo.transaction_fee),tp.secret);
				}
				return;


			}

			else if ("XRP".Equals(tp.units)) {
				Logging.write ("XRP");

				Decimal? amountd = DenominatedIssuedCurrency.parseDecimal(tp.amount, "Amount Entered");
				if (amountd!=null) {
					sendXrpPayment (tp.account,tp.destination,(Decimal)amountd,new decimal(ServerInfo.transaction_fee),tp.secret);
				}
				return;

			}

			else {
				Logging.write ("Please specify a currency unit, eg. \"drops\"");
				return;
			}




		}


		public void setDropBalance (decimal balance) {

			Gtk.Application.Invoke ( delegate 
			                        {

				if (this.unitsSelectBox == null || this.unitsSelectBox.ActiveText == null ) {
					// Todo debuging

					return;
				}

				if (this.unitsSelectBox.ActiveText.Equals("") || this.unitsSelectBox.ActiveText.Equals(" ")) {
						
					// todo debug
					return;
				}
					
				if (this.unitsSelectBox.ActiveText.Equals("drops")) {
					this.balanceLabel.Text = Base58.truncateTrailingZerosFromString( balance.ToString() );
					return;
				}
					
				if (this.unitsSelectBox.ActiveText.Equals("XRP")) {
					this.balanceLabel.Text = Base58.truncateTrailingZerosFromString( (balance / 1000000.0m).ToString() );
					return;
				}
					
					
				// todo debugging
				return;

				}
				);

		}


		protected void OnUnitsSelectBoxChanged (object sender, EventArgs e)
		{
			if ( ReceiveWidget.UNSYNCED.Equals(this.balanceLabel.Text)) {
				// TODO ??



				return;
			}

			if (MainWindow.currentInstance != null) {

				this.setDropBalance ( MainWindow.currentInstance.dropBalance );
				return;
			}
			//
		}

		protected void OnAmountEntryActivated (object sender, EventArgs e)
		{

			if (this.destinationcomboboxentry == null) {
				return;
			}

			this.destinationcomboboxentry.GrabFocus();

		}

		protected void OnDestinationentryActivated (object sender, EventArgs e)
		{
			if (this.sendXRPButton == null) {
				// bad news this bug would be :/
				return;
			}

			this.sendXRPButton.GrabFocus ();
		}
	}


}

