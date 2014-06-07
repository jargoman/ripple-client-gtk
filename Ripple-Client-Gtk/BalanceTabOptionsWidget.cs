using System;
using System.IO; 
using Codeplex.Data;

namespace RippleClientGtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class BalanceTabOptionsWidget : Gtk.Bin
	{
		public BalanceTabOptionsWidget ()
		{
			this.Build ();
		}


		public String[] getFavorites ()
		{

			String [] arr = new string[6];

			arr[0] = this.comboboxentry1.Entry.Text;
			arr[1] = this.comboboxentry2.Entry.Text;
			arr[2] = this.comboboxentry3.Entry.Text;
			arr[3] = this.comboboxentry4.ActiveText;
			arr[4] = this.comboboxentry5.ActiveText;
			arr[5] = this.comboboxentry6.ActiveText;

			return arr;
		}

		public void setFavorites (String[] arr)
		{
			Gtk.Application.Invoke( delegate {
				this.comboboxentry1.Entry.Text = arr[0];
				this.comboboxentry2.Entry.Text = arr[1];
				this.comboboxentry3.Entry.Text = arr[2];
				this.comboboxentry4.Entry.Text = arr[3];
				this.comboboxentry5.Entry.Text = arr[4];
				this.comboboxentry6.Entry.Text = arr[5];
			});
		}


		public static String[] favoritesFromJson (String json)
		{
			dynamic dyn = DynamicJson.Parse (json);
			if (dyn.IsDefined ("array")) {

				try 
				{

					String[] array = dyn.array as String[];
					return array;

				} catch (Exception e) {
					return null;
				}
			}


			return null;

		}



		public static String getJson (String[] array)
		{
			var obj = new {array};

			return DynamicJson.Serialize(obj);

		}

		public static readonly String[] default_values = {"BTC", "LTC", "USD", "CAD", "EUR", "NMC"};
		public static String[] actual_values = null; // values being used live

		public static readonly String configName = "favorites.jsn";
		public static String balanceConfigPath = "";

		public static String jsonConfig = null;



		public static bool loadBalanceConfig ()
		{

			balanceConfigPath = FileHelper.getSettingsPath (configName);

			jsonConfig = FileHelper.getJsonConf (balanceConfigPath);
			//jsonConfig = FileHelper.getJsonConf (balanceConfigPath);  // from harddisk


			// if favorites weren't set via command line
			if (actual_values == null) {

				// if there is no config then generate a default one
				if (jsonConfig == null) {
					jsonConfig = getJson (default_values);
					actual_values = default_values;

				} else {

					actual_values = favoritesFromJson (jsonConfig);

					if (actual_values != null) {
						return true;
					}
				}
			} else {
				saveConfig(actual_values);
			}

			return false;

		}

		public static void setFavoriteParam (String param)
		{
			if (param==null) {
				// todo debug
			}

			String[] values = param.Split(',');

			if (values.Length!=6) {
				Logging.write("Commad line argument Favorites must specify six currencies");
				Logging.write("Example : favorites=CAD,BTC,EUR,LTC,JED,CNY");
			}

			// todo sanity check. valid currencies?

			actual_values = values;


		}

		public static void saveConfig (String[] values) {
			jsonConfig = getJson(values);
			try {
				File.WriteAllText(balanceConfigPath,jsonConfig);
			}
			catch (Exception e) {
				// todo debug
				Logging.write(e.Message);
			}

		}

		protected void OnComboboxentry1Changed (object sender, EventArgs e)
		{
			// no need to do anything ?
			//throw new System.NotImplementedException ();
		}


	}
}

