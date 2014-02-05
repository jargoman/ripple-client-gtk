using System;

namespace RippleClientGtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Console : Gtk.Bin
	{
		public Console () 
		{
			this.Build ();

			if (Debug.Console) {
				Logging.write ("new Console :\n");
			}

			this.consoleentry.Activated += new EventHandler (this.OnConsoleentryActivated);
			this.sendbutton.Clicked += new EventHandler (this.OnSendbuttonClicked);

			Logging.textview = this.consoleView;

		}

		private void send ()
		{
			if (Debug.Console) {
				Logging.write ("Console : method send : begin\n");
			}

			// simple... send the raw imput to server

			String mess = this.consoleentry.Text;

			if (mess == null || mess.Equals("")) {
				if (Debug.Console) {
					Logging.write ("Console : entry text is empty\n");
				}
				return;
			}


			if (NetworkInterface.currentInstance == null) {

				if (Debug.Console) {
					Logging.write ("Console : NetworkInterface.currentInstance == null");
				}

				MessageDialog.showMessage ("You have to be connected to a server to send a json command.\nGo to the network settings tab");
				return;
			}
			NetworkInterface.currentInstance.sendToServer (mess);
		}

		protected void OnSendbuttonClicked (object sender, EventArgs e)
		{
			if (Debug.Console) {
				Logging.write ("Console : Event OnSendbuttonClicked fired\n");
			}

			send ();

		}

		protected void OnConsoleentryActivated (object sender, EventArgs e)
		{
			if (Debug.Console) {
				Logging.write ("Console : Event OnConsoleentryActivated fired\n");
			}

			send ();
		}
	}
}

