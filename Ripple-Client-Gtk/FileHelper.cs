using System;
using System.IO;

namespace RippleClientGtk
{
	public static class FileHelper
	{
		public static String getSettingsPath (String filename) {

			if (DATA_FOLDER_PATH==null) { 

				DATA_FOLDER_PATH = Environment.GetFolderPath ( Environment.SpecialFolder.ApplicationData );

				DATA_FOLDER = System.IO.Path.Combine (DATA_FOLDER_PATH, DATA_FOLDER);

			}

			if (!(System.IO.Directory.Exists(DATA_FOLDER))) {
				System.IO.Directory.CreateDirectory (DATA_FOLDER);
			}


			String result = System.IO.Path.Combine (DATA_FOLDER_PATH, DATA_FOLDER, filename);


			return result;
		}

		public static string getJsonConf (String path) {
			String str = null;
			try {
				if (File.Exists(path)) {

					str = File.ReadAllText(path);
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

		public static String DATA_FOLDER = "rippleClientGtk";

		//public static String 
	}
}

