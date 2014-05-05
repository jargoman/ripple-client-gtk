using System;
using System.Collections.Generic;
using System.IO;

namespace RippleClientGtk
{
	public static class FileHelper
	{
		static FileHelper () {

			DATA_FOLDER_PATH = Environment.GetFolderPath ( Environment.SpecialFolder.ApplicationData );
			assureDirectory(DATA_FOLDER_PATH);

			DATA_FOLDER = System.IO.Path.Combine (DATA_FOLDER_PATH, DATA_FOLDER);
			assureDirectory(DATA_FOLDER);

			PLUGIN_FOLDER_PATH = Path.Combine (DATA_FOLDER_PATH, DATA_FOLDER, PLUGIN_FOLDER);
			assureDirectory(PLUGIN_FOLDER_PATH);



		}

		public static String getSettingsPath (String filename) {

			//if (!(System.IO.Directory.Exists(DATA_FOLDER))) {
			//	System.IO.Directory.CreateDirectory (DATA_FOLDER);
			//}
			String result = System.IO.Path.Combine (DATA_FOLDER_PATH, DATA_FOLDER, filename);
			return result;
		}

		public static String[] getPluginDirFileNames ()
		{
			return Directory.GetFiles(PLUGIN_FOLDER_PATH, "*");
			//string[] me = new string[filePaths.Length];

			//for (int i = 0; i < filePaths.Length; i++) {
			//	me[i] = filePaths[i].
			//}

		}

		public static String getPluginPath (String name)
		{
			return Path.Combine(PLUGIN_FOLDER_PATH, name);
		}



		public static void assureDirectory (String dir)
		{
			if (!(System.IO.Directory.Exists(dir))) {
				try {
					System.IO.Directory.CreateDirectory (dir);
				}

				catch (Exception e) {

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

		public static String DATA_FOLDER_PATH = null;

		public static readonly String DATA_FOLDER = "rippleClientGtk";

		public static readonly String PLUGIN_FOLDER = "plugins";

		public static string examplename = "exampleplugin";

		public static Dictionary<String, String> configCache = new Dictionary<String,String>();

		public static String PLUGIN_FOLDER_PATH = null;

		//public static String 
	}
}

