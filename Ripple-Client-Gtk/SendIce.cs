/*
 *	License : Le Ice Sense 
 */

using System;


namespace RippleClientGtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class SendIce : Gtk.Bin
	{
		public SendIce ()
		{

			this.Build ();

			this.label2.Text = AccountLines.LICENSE_ISSUER;
			this.label1.Text = "<u>" + AccountLines.LICENSE + "</u>";

			this.label1.UseMarkup = true;


		}

		protected void OnSendIOUButtonClicked (object sender, EventArgs e)
		{
			if (Debug.SendIce) {

			}
		}

	}
}

