using System;

namespace RippleClientGtk
{
	public partial class PasswordCreateDialog : Gtk.Dialog
	{
		public PasswordCreateDialog (String message)
		{
			this.Build ();
			if (Debug.PasswordCreateDialog) {
				Logging.write ("new PasswordCreateDialog\n");
			}
			this.textview1.Buffer.Text = message;

			this.secretentry.GrabFocus ();
		}


		public int verifyPasswords () {
			if (Debug.PasswordCreateDialog) {
				Logging.write ("PasswordCreateDialog : method verifyPasswords : begin\n");
			}
			String passone = this.secretentry.Text;
			String passtwo = this.secretentry1.Text;
			if (!passone.Equals(passtwo)) {
				if (Debug.PasswordCreateDialog) {
					Logging.write ("PasswordCreateDialog : method verifyPasswords : Passwords do not match\n");
				}
				return PASSNOTMATCH;
			}

			if (Debug.PasswordCreateDialog) {
				Logging.write ("PasswordCreateDialog : method verifyPasswords : Passwords match\n");
			}
			return PASSISVALID;
		}

		public String getPassword () {
			if (Debug.PasswordCreateDialog) {
				Logging.write ("PasswordCreateDialog : method getPassword : begin\n");
			}
			return this.secretentry.Text;
		}


		public static int PASSNOTMATCH = 1;

		public static int PASSISVALID = 0;

		protected void OnSecretentryActivated (object sender, EventArgs e)
		{
			if (Debug.PasswordCreateDialog) {
				Logging.write ("PasswordCreateDialog : method OnSecretentryActivated : begin\n");
			}


			this.secretentry1.GrabFocus ();
		}

		protected void OnSecretentry1Activated (object sender, EventArgs e)
		{
			if (Debug.PasswordCreateDialog) {
				Logging.write ("PasswordCreateDialog : method OnSecretentry1Activated : begin\n");
			}
			this.buttonOk.GrabFocus ();
		}
	}
}

