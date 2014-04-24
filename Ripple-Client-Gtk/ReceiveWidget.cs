/*
 *	License : Le Ice Sense 
 */

using System;
using System.Collections.Generic;
using Codeplex.Data;

namespace RippleClientGtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class ReceiveWidget : Gtk.Bin
	{
		public ReceiveWidget ()
		{
			this.Build ();

			if (Debug.ReceiveWidget) {
				Logging.write ("new ReceiveWidget\n");
			}

			this.receiveLabel.Selectable = true;

			this.syncbutton.Clicked += new EventHandler (this.onSyncClicked);

			NetworkInterface.onOpen += delegate (object sender, EventArgs e) {
				if (Debug.ReceiveWidget) {
					Logging.write ("ReceiveWidget : NetworkInterface.onOpen : delegate begin\n");
				}

				if (this.isSet) {
					this.requestInfo(this.getReceiveAddress());
				}


			};
		}

		static String UNSYNCED = " -- unsynced -- ";
		String address = UNSYNCED;

		/* no receive address on start */
		private bool isSet = false;

		/* user clicked the synced button */
		protected void onSyncClicked (object sender, EventArgs e)
		{

			Gtk.Application.Invoke ( delegate 
			                        {
				if (!isSet) {
					warn();
					return;
				}

				String address = this.receiveLabel.Text; 
				requestInfo (address);
			}
			);


		}

		protected void OnButton8Clicked (object sender, EventArgs e)
		{
			//this.receiveLabel

			if (this.receiveLabel == null) {
				Logging.write ("Error in class ReceiveWidget. receiveLabel is null\n");
				return;
			}

			else {

				if (isSet) {
					receiveLabel.SelectRegion (0, receiveLabel.Text.Length);
					return;
				} else {
					warn ();
					return;
				}



			}


		}

		public void warn () {
			MessageDialog.showMessage ("You need a public and private key. Go to wallet tab and enter your wallet key pair\n");
		}

		public void setReceiveAddress ( String address ) {

			AccountLines.cash = new Dictionary<string, Decimal> ();

			Gtk.Application.Invoke ( delegate 
				{
					this.receiveLabel.Text = address;
				}
			);

			this.address = address;

			this.isSet = true;

			requestInfo (address);

		}

		private void requestInfo (String address) {

			//String request = "{\"command\":\"account_info\",\"account\":\"" + address + "\"}";

			Object ob = new {command="account_info", account=address};

			Object ob2 = new {command="account_lines", account=address};

			String request = DynamicJson.Serialize (ob);
			String request2 = DynamicJson.Serialize (ob2);

			if (NetworkInterface.currentInstance!=null) {
				NetworkInterface.currentInstance.sendToServer (request);

				NetworkInterface.currentInstance.sendToServer (request2);
			}


		}

		public String getReceiveAddress () {

			if (!isSet) {

				warn ();
				return null;
			}

			return this.address;
		}
		
	}
}

