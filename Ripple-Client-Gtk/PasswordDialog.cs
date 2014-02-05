using System;

namespace RippleClientGtk
{
	public partial class PasswordDialog : Gtk.Dialog
	{
		public PasswordDialog (String message)
		{
			this.Build ();

			if (Debug.PasswordDialog) {
				Logging.write("new PasswordDialog\n");
			}

			this.textview1.Buffer.Text = message;
			//this.textview1.A

			this.entry1.GrabFocus ();
		}

		public string getPassword () {
			if (Debug.PasswordDialog) {
				Logging.write ("PasswordDialog : method getPassword : begin\n");
			}
			return this.entry1.Text;
		}

		protected void OnEntry1Activated (object sender, EventArgs e)
		{
			if (Debug.PasswordDialog) {
				Logging.write ("PasswordDialog : OnEntry1Activated : begin\n");
			}
			this.buttonOk.Activate ();
		}
	}
}

