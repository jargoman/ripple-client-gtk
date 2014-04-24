using System;
using System.IO;
using Codeplex.Data;

namespace RippleClientGtk
{
	public partial class SplashWindow : Gtk.Window
	{
		public SplashWindow () : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build ();


		}

		public static readonly String configName = "splash.jsn";


		public static bool isSplash = true; 
		public static String jsonConfig = null;
		public static String configPath = null;

		static SplashWindow ()
		{
			configPath = FileHelper.getSettingsPath( configName );


		}


		public static void loadIsSplash ()
		{

			jsonConfig = FileHelper.getJsonConf(configPath);

			if (jsonConfig != null) {
				try {
					dynamic d = DynamicJson.Parse (jsonConfig);
					if (d.isDefined ("enable_splash")) {
						isSplash = d.enable_splash;  // true or false decided by config file
					}
					else {
						isSplash = false; // ommitting "enable_splash"=true defaults to false
					}

				} catch (Exception e) {
					isSplash = false;  // don't show invalid splash config
				}
			} else {
				isSplash = true; // show splash on lack of config
			}

		}

		public static void setIsSplash ( bool setme)
		{
			isSplash = setme;
			Object o = new {
				setme
			};

			String js = DynamicJson.Serialize(o);

			File.WriteAllText(configPath, js);

		}

	}
}

