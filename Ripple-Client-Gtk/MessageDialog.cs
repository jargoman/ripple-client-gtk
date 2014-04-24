using System;

namespace RippleClientGtk
{
	public partial class MessageDialog : Gtk.Dialog
	{
		public MessageDialog (String message)
		{
			this.Build ();

			this.textview1.Buffer.Text = message;
		}

		public static void showMessage (String message) {

			Gtk.Application.Invoke( delegate {
			MessageDialog mg = new MessageDialog (message);
			mg.Modal = true;

			mg.Run ();

			mg.Destroy ();

		});



		}
	}
}

