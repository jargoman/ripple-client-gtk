using System;

namespace RippleClientGtk
{
	public partial class AreYouSure : Gtk.Dialog
	{
		public AreYouSure (String message)
		{
			this.Build ();

			this.textview2.Buffer.Text = message;

			this.Modal = true;
		}
	}
}

