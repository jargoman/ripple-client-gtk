using System;


namespace RippleClientGtk
{
	public abstract class Plugin : RippleClientGtk.IPlugin
	{
		public Plugin ()
		{

		}


		public abstract void preStartup ();  // splash

		public abstract void postStartup();  // 



		public abstract Gtk.Widget getOptionTab();

		public abstract Gtk.Widget getMainTab();

		// implemented 
		//public String getTitle () { if (title==null) {title = "";} return title; }
		public RippleClientGtk.IPlugin getPlugIn () { return (RippleClientGtk.IPlugin)this; }

		public String title = null; 

		public String jsonConf = null;

		public String tab_label = null;
	}
}