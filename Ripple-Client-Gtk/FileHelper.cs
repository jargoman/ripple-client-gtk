using System;
using System.Collections.Generic;
using System.IO;

namespace RippleClientGtk
{
	public static class FileHelper
	{
		public static void setFolderPath (String pat)
		{

			if (Debug.FileHelper) {
				Logging.write("FileHelper : setFolderPath ( pat = "+ pat + ")");
			}

			if (pat != null) {
				 if (Directory.Exists (pat)) {
					DATA_FOLDER_PATH = pat;
				}

				else {
					// Todo debug 
					Logging.write("Configuration directory " + pat + " does not exist!");
					Gtk.Application.Quit();
				}
			} else {
				DATA_FOLDER_PATH = Environment.GetFolderPath ( Environment.SpecialFolder.ApplicationData );
			}

			if (DATA_FOLDER_PATH == null) {
				// Todo debug
				Gtk.Application.Quit();
			}

			if (Debug.FileHelper) {
				Logging.write("DATA_FOLDER_PATH = " + DATA_FOLDER_PATH);
			}

			assureDirectory(DATA_FOLDER_PATH);

			DATA_FOLDER = System.IO.Path.Combine (DATA_FOLDER_PATH, DATA_FOLDER);
			assureDirectory(DATA_FOLDER);

			PLUGIN_FOLDER_PATH = Path.Combine (DATA_FOLDER_PATH, DATA_FOLDER, PLUGIN_FOLDER);
			assureDirectory(PLUGIN_FOLDER_PATH);

			ENCRYPTION_FOLDER_PATH = Path.Combine(DATA_FOLDER_PATH, DATA_FOLDER, ENCRYPTION_FOLDER);
			assureDirectory(ENCRYPTION_FOLDER_PATH);

			WALLET_FOLDER_PATH = Path.Combine(DATA_FOLDER_PATH, DATA_FOLDER, WALLET_FOLDER);
			assureDirectory(WALLET_FOLDER_PATH);

			CLASS_FOLDER_PATH = Path.Combine(DATA_FOLDER_PATH, DATA_FOLDER, CLASS_FOLDER);
			assureDirectory(CLASS_FOLDER_PATH);
		}

		public static String getSettingsPath (String filename)
		{

			//if (!(System.IO.Directory.Exists(DATA_FOLDER))) {
			//	System.IO.Directory.CreateDirectory (DATA_FOLDER);
			//}

			if (DATA_FOLDER_PATH == null || DATA_FOLDER == null || DATA_FOLDER == null) {
				Logging.write("Critical error. invalid");
				Gtk.Application.Quit();
				return null;
			}

			String result = System.IO.Path.Combine (DATA_FOLDER_PATH, DATA_FOLDER, filename);
			return result;
		}

		public static String[] getDirFileNames (String path, String pattern)
		{
			try {
				return Directory.GetFiles(path, pattern);    //(PLUGIN_FOLDER_PATH, "*");
			}

			catch (Exception e) {
				return null;
			}
			//string[] me = new string[filePaths.Length];

			//for (int i = 0; i < filePaths.Length; i++) {
			//	me[i] = filePaths[i].
			//}

		}

		public static String[] getDirFileNames (String path) {
			return getDirFileNames(path, "*");
		}

		public static String getPluginPath (String name)
		{
			return Path.Combine(PLUGIN_FOLDER_PATH, name);
		}

		public static String getWalletPath (String name)
		{
			name = name + ".ice";

			return Path.Combine(WALLET_FOLDER_PATH, name);
		}

		public static bool testPluginPathAvailability (String name)
		{
			if (name == null || name.Equals("")) {
				return false;
			}

			try {

				String path = getPluginPath(name);

				if (name == null || name.Equals("")) {  // would never happen. 
					return false;
				}
			
				if (File.Exists(path)) {
					return false;
				}

				return true;

			}

			catch (Exception e) {
				Logging.write(e.Message);
				return false;
			}



		}



		public static void assureDirectory (String dir)
		{
			if (Debug.FileHelper) {
				Logging.write("FileHelper : Assuring directory " + dir );
			}

			if (!(System.IO.Directory.Exists(dir))) {
				try {
					System.IO.Directory.CreateDirectory (dir);
				}

				catch (Exception e) {
					Logging.write(e.Message);

					Gtk.Application.Quit();
				}
			}
		}


		public static bool saveConfig (String path, String config)
		{

			try {
				File.WriteAllText(path, config);
				if (configCache.ContainsKey(path)) {
					configCache[path] = config;
				}
				return true;
			}
			catch (Exception e) {
				return false;
			}

			return true;
		}


		public static string getJsonConf (String path)
		{
			String str = null;
			if (path == null) {
				return null;
			}

			if (configCache == null) {
				configCache = new Dictionary<string, string> ();
			}

			if (configCache.TryGetValue (path, out str)) {
				return str;
			}



			try {
				if (File.Exists(path)) {

					str = File.ReadAllText(path);
					if (str!=null) {
						configCache.Add(path,str);
					}
					return str;
				}
			}

			catch (Exception e) {
				Logging.write(e.Message.ToString());
				return null;
			}

			return str;

		}

		public static String userSetPath = null;

		public static String DATA_FOLDER = "rippleClientGtk"; // folder that contains all settings
		public static String DATA_FOLDER_PATH = null;  //

		public static String PLUGIN_FOLDER = "plugins";
		public static String PLUGIN_FOLDER_PATH = null;

		public static String ENCRYPTION_FOLDER = "encryption";
		public static String ENCRYPTION_FOLDER_PATH = null;

		public static String WALLET_FOLDER = "wallets";
		public static String WALLET_FOLDER_PATH = null;

		public static String CLASS_FOLDER = "classes";
		public static String CLASS_FOLDER_PATH = null;

		public static string examplename = "exampleplugin";

		public static Dictionary<String, String> configCache = new Dictionary<String,String>();



		//public static String 
	}
}

