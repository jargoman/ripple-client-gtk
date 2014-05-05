using System;
using System.Collections.Generic;
using System.IO;
using Codeplex.Data;

namespace RippleClientGtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class OptionsWidget : Gtk.Bin
	{
		public OptionsWidget ()
		{
			this.Build ();

			if ( plugWidgets == null && PluginController.currentInstance!=null ) {
				plugWidgets = PluginController.currentInstance.getOptionsWidgets();
			}
		}

		private static List<Gtk.Widget> plugWidgets = null;



		//private dynamic dynamo = null;
		//private DynamicJson dynamo = null;

		public void processOptions ()
		{



		}

		private void processConsole ()
		{

		}

		private void processFavorites ()
		{

			string[] faves = this.balancetaboptionswidget2.getFavorites();

			
		}

		private void processSplash ()
		{

		}







	}
}

