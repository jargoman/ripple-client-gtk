using System;

namespace RippleClientGtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class OptionsWidget : Gtk.Bin
	{
		public OptionsWidget ()
		{
			this.Build ();
		}

		private UInt32 parseInt (String s)
		{
			/*
			try {
				mills = Convert.ToUInt32(millstr);
			}

			catch (Exception e) {
				// TODO debug
				return;
			}

			return mills;*/

			return 0;
		}

		public void processOptions ()
		{

			Gtk.Application.Invoke(
				delegate {

			
					// following must be run by gtk thread
			
					string[] faves = this.balancetaboptionswidget2.getFavorites();

					bool showsplash = this.checkbutton1.Active;

					String millstr = this.entry4.Text;
					int mills;
					

					string path = label16.Text;

					int splshwidth;

					try {
						//splshwidth =
					}

					catch (Exception e) {

					}
				}

					
			);

		}

	}
}

