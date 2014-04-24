/*
 *	License : Le Ice Sense 
 */

using System;
using System.IO;
using Codeplex.Data;

namespace RippleClientGtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class NetworkSettings : Gtk.Bin
	{
		public NetworkSettings ()
		{
			this.Build ();

			if (Debug.NetworkSettings) {
				Logging.write ("new NetworkSettings");
			}





			NetworkInterface.onOpen += delegate(object sender, EventArgs e) {
				if (Debug.NetworkSettings) {
					Logging.write ("NetworkSettings : event NetworkInterface.onOpend : delegate begin\n");
				}


				this.setConnected();

				this.saveSettings();


			};

			NetworkInterface.onClose += delegate(object sender, EventArgs e) {
				// 

				if (Debug.NetworkSettings) {
					Logging.write ("NetworkSettings : event NetworkInterface.onClose : delegate begin\n");
				}

				this.setDisConnected();

			};

			NetworkInterface.onError += delegate(object sender, SuperSocket.ClientEngine.ErrorEventArgs e) {
				if (Debug.NetworkSettings) {
					Logging.write("NetworkSettings : event NetworkInterface.onError : delegate begin\n");
				}

				this.setDisConnected();
			};


			currentInstance = this;
		}

		static NetworkSettings ()
		{
			if (Debug.NetworkSettings) {
				Logging.write("NetworkSettings static method");
			}

			settingsPath = FileHelper.getSettingsPath (settingsFileName);

			//this.loadSettings ();
		}

		public void setConnected ()
		{

			if (Debug.NetworkSettings) {
				Logging.write("NetworkSettings : method setConnected : begin\n");
			}

			Gtk.Application.Invoke ( delegate 
				                        {
					if (Debug.NetworkSettings) {
						Logging.write ("NetworkSettings : gtk onOpen delegate begin\n");
					}

					this.connectStatusLabel.Text = "Connected";

				}
				);
		}


		public void setDisConnected ()
		{
			if (Debug.NetworkSettings) {
				Logging.write("NetworkSettings : method setDisConnected : begin\n");
			}
			Gtk.Application.Invoke ( delegate 
				                        {
					if (Debug.NetworkSettings) {
						Logging.write ("NetworkSettings : setConnected gtk delegate begin\n");
					}

					this.connectStatusLabel.Text = "Disconnected";

				}
				);

		}

		/*///// !!!VARIABLES!!! ///////*/ 

		//static string DEFAULT_SERVER = "wss://s-west.ripple.com";

		public static NetworkSettings currentInstance = null;
		static String settingsFileName = "networkSettings.jsn";
		static String settingsPath = "";


		static String json = null;
		public static void readSettings ()
		{
			if (Debug.NetworkSettings) {
				Logging.write("NetworkSettings : method readSettings : begin\n");
			}



			try {

				if (File.Exists(settingsPath)) {

					Logging.write ("Found network settings file at " + settingsPath + "\n");

					json = File.ReadAllText(settingsPath);
					if (json==null) {
						Logging.write ("Unknown error, reading network settings file\n" );
					}
				}

				else {
					Logging.write("No network settings found\n");
						
					return;
				}
			}
			catch (Exception e) {
				Logging.write ("NetworkSettings : method loadSettings : Exception thrown reading " + settingsPath + "\n" + e.Message);
			}
		}

		public void loadSettings () {
			if (Debug.NetworkSettings) {
				Logging.write("NetworkSettings : method loadSettings : begin\n");
			}

			Gtk.Application.Invoke (
				delegate 
				{

				if (Debug.NetworkSettings) {
					Logging.write("NetworkSettings : method loadSettings : delegate invoked\n");
				}

				


					try {
						if (json!=null) {
							dynamic dyn = DynamicJson.Parse(json);

							if (dyn.IsDefined("ServerUrl")) {

								if (Debug.NetworkSettings) {
									Logging.write ("NetworkSettings : method loadSettings : Restoring server url entry");
								}

								this.serverentry.Text = (String)dyn.ServerUrl;
							}

							if (dyn.IsDefined("LocalUrl")) {
								if (Debug.NetworkSettings) {
									Logging.write ("NetworkSettings : method loadSettings : Restoring local url entry");
								}

								this.localentry.Text = (String)dyn.LocalUrl;
							}

							if (dyn.IsDefined("UserAgent")) {
								if (Debug.NetworkSettings) {
									Logging.write ("NetworkSettings : method loadSettings : Restoring user agent entry");
								}

								this.agententry.Text = (String)dyn.UserAgent;
							}

							if (dyn.IsDefined("reconnect") ) {
								if (Debug.NetworkSettings) {
									Logging.write ("NetworkSettings : method loadSettings : Restoring reconnect checkbox");
								}

								bool b = (bool)dyn.reconnect;

								this.autoconnectbutton.Active = b;
							}

						}
					}

					catch (Exception e) {
						Logging.write ("NetworkSettings : method loadSettings : Error parsing settings file : Excpetion thown\n" + e.Message);
					}
				

				// has to be inside the delegate code that runs AFTER everything is set up. 

				if (this.autoconnectbutton.Active) {
					if (Debug.NetworkSettings) {
						Logging.write ("NetworkSettings : method loadSettings : autoconnectbutton.Active == true\n");
					}
					this.connect ();
				}
			

				}
			


			);

		}

		public void saveSettings () {
			if (Debug.NetworkSettings) {
				Logging.write ("NetworkSettings : method saveSettings : begin\n");
			}

		Gtk.Application.Invoke (
			delegate 
			{
				if (Debug.NetworkSettings) {
					Logging.write ("NetworkSettings : method saveSettings : delegate invoke\n");
				}

				Object ob = new 
					{
						ServerUrl = (String)this.serverentry.Text,
						LocalUrl = (String)this.localentry.Text,
						UserAgent = (String)this.agententry.Text,

						reconnect = (bool)this.autoconnectbutton.Active
					};

				String json = DynamicJson.Serialize(ob);

				if (Debug.NetworkSettings) {
					Logging.write ("NetworkSettings : method saveSettings : json = " + json + "\n");
				}

				File.WriteAllText(settingsPath, json);


			}
		);

		}

		protected void connect () {
			if (Debug.NetworkSettings) {
				Logging.write ("NetworkSettings : method connect : begin\n");
			}

			Gtk.Application.Invoke( delegate {

		

			String server = serverentry.Text;
			String local = localentry.Text;
			String agent = agententry.Text;

			// TODO validate url, yuck :/
			bool autoconnect = false;
			if (autoconnectbutton == null) {
				// TODO debug
				Logging.write ("NetworkSettings : method connect : Critical Error : autoconnectbutton = null");

				return;
			}

			autoconnect = autoconnectbutton.Active;

			NetworkInterface net = new NetworkInterface (server, local, agent, autoconnect);
			if (net != null) {
				net.connect ();
			}

			});
		}

		protected void OnConnectbuttonClicked (object sender, EventArgs e)
		{
			if (Debug.NetworkSettings) {
				Logging.write ("NetworkSettings : method OnConnectbuttonClicked : begin\n");
			}

			connect ();
		}

		protected void OnDisconnectbuttonClicked (object sender, EventArgs e)
		{

			if (Debug.NetworkSettings) {
				Logging.write ("NetworkSettings : method OnDisconnectbuttonClicked : begin\n");
			}

			if (NetworkInterface.currentInstance != null) {
				NetworkInterface.currentInstance.disconnect ();
			} else {
				MessageDialog.showMessage ("You haven't even connected to a server yet.");
			}
		}

		protected void OnAutoconnectbuttonToggled (object sender, EventArgs e)
		{

			if (Debug.NetworkSettings) {
				Logging.write ("NetworkSettings : method OnAutoconnectbuttonToggled : begin\n");
			}

			if (NetworkInterface.currentInstance!=null) {

				NetworkInterface.currentInstance.reconnect = this.autoconnectbutton.Active;
			}

			// no else needed 
		}

		protected void OnServerentryActivated (object sender, EventArgs e)
		{
			if (Debug.NetworkSettings) {
				Logging.write ("NetworkSettings : method OnServerentryActivated : begin\n");
			}

			if (localentry == null) {

				Logging.write ("NetworkSettings : method OnServerentryActivated : Critical Error : localentry = null\n");
				return;
			}

			this.localentry.GrabFocus ();
		}

		protected void OnLocalentryActivated (object sender, EventArgs e)
		{
			if (Debug.NetworkSettings) {
				Logging.write ("NetworkSettings : method OnLocalentryActivated : begin\n");
			}
			if (agententry == null) {

				Logging.write ("NetworkSettings : method OnLocalentryActivated : Critical Error : agententry = null\n");
				return;

			}

			this.agententry.GrabFocus ();
		}

		protected void OnAgententryActivated (object sender, EventArgs e)
		{

			if (Debug.NetworkSettings) {
				Logging.write ("NetworkSettings : method OnAgententryActivated : begin\n");
			}

			if (connectbutton==null) {
				// TODO debhug
				Logging.write ("NetworkSettings : method OnAgententryActivated : Critical Error : connectbutton = null\n");
				return;
			}

			this.connectbutton.GrabFocus ();
		}
	}
}

