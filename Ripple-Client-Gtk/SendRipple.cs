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

			this.balanceLabel.Text = unsynced;

			this.unitsSelectBox.Changed += OnUnitsSelectBoxChanged;

		}

		String unsynced = "   --   unsynced   --   ";

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
			DenominatedIssuedCurrency amnt = new DenominatedIssuedCurrency(xrpamount);
			DenominatedIssuedCurrency dafee = new DenominatedIssuedCurrency(fee);

			RipplePaymentTransaction tx = new RipplePaymentTransaction(seed.getPublicRippleAddress(),payee,amnt,dafee, MainWindow.currentInstance.sequence,null); // Todo implement sequemce number. int 23 is an arbatrary number for testing 
			RippleBinaryObject rbo = tx.getBinaryObject().getObjectSorted();
			rbo = new RippleSigner(seed.getPrivateKey(0)).sign(rbo);

			byte[] signedTXBytes = new BinarySerializer().writeBinaryObject(rbo).ToArray();

			String blob = Base58.ByteArrayToHexString(signedTXBytes);

			object ob = new {
				command = "submit",
				tx_blob = blob
			};

			String jso = DynamicJson.Serialize(ob);



			//NetworkInterface.currentInstance.sendToServer(jso);

		}



		protected void OnSendXRPButtonClicked (object sender, EventArgs e)
		{

			Thread th = new Thread( new ParameterizedThreadStart(sendThread));

			String amount = this.amountEntry.Text;
			String destination = this.destinationentry.Text;

			String account = MainWindow.currentInstance.getReceiveAddress ();
			String secret = MainWindow.currentInstance.getSecret ();

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

			Logging.write("Units = " + tp.units);

			if (tp == null) {
				throw new InvalidCastException("Unable to cast object to type threadParam");
			}

			if (Debug.SendRipple) {
				Logging.write("Send Ripple : requesting Server Info\n");
			}

			ServerInfo.refresh_blocking();

			if (Debug.SendRipple) {
				Logging.write("Send Ripple : refresh_blocking returned, ServerInfo.transaction_fee = " + ServerInfo.transaction_fee.ToString());
			}




			if ("drops".Equals(tp.units)) {
				Logging.write("drops");


				try {

					ulong amountl = Convert.ToUInt64( tp.amount );

					if (amountl<0) {
						MessageDialog.showMessage("Sending negative amounts is not supported. Please enter a valid amount");
						return;
					}

					sendDropsPayment(tp.account,tp.destination,(decimal)amountl,new decimal(ServerInfo.transaction_fee),tp.secret);
				}

				catch (FormatException ex) {

					MessageDialog.showMessage ("Amount is fomated incorrectly for sending drops.\n It must be a valid integer\n");
					return;

				}

				catch (OverflowException ex) {
					MessageDialog.showMessage ("Send amount is greater than an unsignd long. No one's got that much money\n");
					return;
				}

				catch (Exception ex) {
					MessageDialog.showMessage ("Unknown error formatting string\n");
					return;
				}






			}

			else if ("XRP".Equals(tp.units)) {
				Logging.write ("XRP");

				try {

					Decimal amountd = Convert.ToDecimal(tp.amount);

					if (amountd < 0) {
						MessageDialog.showMessage("Sending negative amounts is not supported. Please enter a valid amount");
						return;
					}

					sendXrpPayment (tp.account,tp.destination,amountd,new decimal(ServerInfo.transaction_fee),tp.secret);
					return;
				}

				catch (FormatException ex) {

					MessageDialog.showMessage ("Amount entered is not a valid number of xrp. e.g 1.2345 " + ex.ToString());

					if (Debug.SendRipple) {
						Logging.write(ex.Message);
					}

					return;
				}

				catch (OverflowException ex) {
					MessageDialog.showMessage ("Send amount is greater than a double. NO ONE's got that much money\n");
					if (Debug.SendRipple) {
						Logging.write(ex.Message);
					}

					return;
				}

				catch (Exception ex ) {
					MessageDialog.showMessage ("Amount entered is not a valid number of xrp. e.g 1.2345 " + ex.ToString());
					if (Debug.SendRipple) {
						Logging.write(ex.Message);
					}
					return;
				}

			}

			else {
				Logging.write ("Please specify a currency unit, eg. \"drops\"");
				return;
			}




		}

		public void setXrpBalance (decimal balance) {

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
			if ( this.unsynced.Equals(this.balanceLabel.Text)) {
				// TODO ??



				return;
			}

			if (MainWindow.currentInstance != null) {

				this.setXrpBalance ( MainWindow.currentInstance.xrpBalance );
				return;
			}
			//
		}

		protected void OnAmountEntryActivated (object sender, EventArgs e)
		{

			if (this.destinationentry == null) {
				return;
			}
			this.destinationentry.GrabFocus ();
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

