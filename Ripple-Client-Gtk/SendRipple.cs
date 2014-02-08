using System;
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

		}

		String unsynced = "   --   unsynced   --   ";

		protected void sendXrpPayment ( String account, String destination, Decimal  xrpamount, String secret) {

			/*
			 * 
			 * // old json way // non signed 
			object ob = new {
				command="submit",
				tx_json=new {
					TransactionType = "Payment",
					Account = account,
					Destination = destination,
					Amount = amount.ToString()
				},
				secret = secret
			};

			String json = DynamicJson.Serialize(ob);
			*/

			//Logging.write(json + "\n");

			Logging.write(xrpamount.ToString());

			RippleSeedAddress seed = new RippleSeedAddress(secret);
			RippleAddress payee = new RippleAddress(destination);
			DenominatedIssuedCurrency amnt = new DenominatedIssuedCurrency(xrpamount);
			RipplePaymentTransaction tx = new RipplePaymentTransaction(seed.getPublicRippleAddress(),payee,amnt,23); // Todo implement sequemce number. int 23 is an arbatrary number for testing 
		}


		protected void sendDropsPayment ( String account, String destination, ulong  amount, String secret) {
			/*
			 * 
			 * // old json way // non signed
			object ob = new {
				command="submit",
				tx_json = new {
					Account = account,
					Destination = destination,
					Amount = amount.ToString()
				},
				secret = secret
			};

			String json = DynamicJson.Serialize (ob);

			Logging.write(json + "\n");

			if (NetworkInterface.currentInstance != null) {
				AreYouSure ays = new AreYouSure ("You are about to send " + amount.ToString() + " drops to address " + destination );

				int resp = ays.Run ();

				if (resp == (int) ResponseType.Ok) {

				}
			}

			*/


		}



		protected void OnSendXRPButtonClicked (object sender, EventArgs e)
		{

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



			if ("drops".Equals(this.unitsSelectBox.ActiveText)) {
				Logging.write("drops");


				try {

					ulong amountl = Convert.ToUInt64( amount );

					if (amountl<0) {
						MessageDialog.showMessage("Sending negative amounts is not supported. Please enter a valid amount");
						return;
					}

					this.sendDropsPayment(account,destination,amountl,secret);
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
					MessageDialog.showMessage ("Amount is fomated incorrectly for sending drops.\n It must be a valid integer\n");
					return;
				}






			}

			else if ("XRP".Equals(this.unitsSelectBox.ActiveText)) {
				Logging.write ("XRP");

				try {

					Decimal amountd = Convert.ToDecimal(amount);

					if (amountd < 0) {
						MessageDialog.showMessage("Sending negative amounts is not supported. Please enter a valid amount");
						return;
					}

					this.sendXrpPayment (account,destination,amountd,secret);
					return;
				}

				catch (FormatException ex) {

					MessageDialog.showMessage ("Amount entered is not a valid number of xrp. e.g 1.2345 " + ex.ToString());

					return;
				}

				catch (OverflowException ex) {
					MessageDialog.showMessage ("Send amount is greater than a double. NO ONE's got that much money\n");
					return;
				}

				catch (Exception ex ) {
					MessageDialog.showMessage ("Amount entered is not a valid number of xrp. e.g 1.2345 " + ex.ToString());
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
					this.balanceLabel.Text = balance.ToString();
					return;
				}
					
				if (this.unitsSelectBox.ActiveText.Equals("XRP")) {
					this.balanceLabel.Text = (balance / 1000000.0m).ToString();
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

