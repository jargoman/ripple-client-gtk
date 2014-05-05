using System;
using System.Threading;
using Codeplex.Data;

namespace RippleClientGtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class ServerInfo : Gtk.Bin
	{
		public ServerInfo ()
		{
			if (Debug.ServerInfo) {
				Logging.write("Server Info : constructor\n");
			}
			this.Build ();

			if (Debug.ServerInfo) {
				Logging.write("Server Info : build complete\n");
			}

			NetworkInterface.onOpen += new NetworkInterface.connectEventHandler(connectionOpened);

			NetworkInterface.onMessage += delegate(object sender, WebSocket4Net.MessageReceivedEventArgs e) 
			{

				Gtk.Application.Invoke(delegate {

			
				try {


					dynamic dynovar = DynamicJson.Parse(e.Message);

					if (firstconnect) { // faster if we only set these values once per connect
						if (dynovar.IsDefined("result.info.build_version")) {
							build_version = dynovar.result.info.build_version as String;
						}

						else {
								return;
						}
						if (dynovar.IsDefined("result.info.hostid")) {
							hostid = dynovar.result.info.hostid as String;
						}

						else {
								return;
						}
						
						if (build_version!=null) {
							this.build_version_label_var.Text = build_version;
						}

						if (hostid!=null) {
							this.host_id_label_var.Text = hostid;
						}
						firstconnect = false;
					}
					
					if (dynovar.IsDefined("result.info.complete_ledgers")) {
						complete_ledgers = dynovar.result.info.complete_ledgers as String;
					}

					if (complete_ledgers!=null) {
						this.complete_ledgers_label_var.Text = complete_ledgers;
					}

					if (dynovar.IsDefined("result.info.load_factor")) {
						load_factor = new decimal (dynovar.result.info.load_factor);
					}
					
					Decimal xrp_base_fee;
					if (dynovar.IsDefined("result.info.validated_ledger.base_fee_xrp")) {
						xrp_base_fee = new decimal (dynovar.result.info.validated_ledger.base_fee_xrp);
					}
					else {
							return;
					}
					base_fee_drops = (ulong)(xrp_base_fee * 1000000m);
					transaction_fee = (ulong)((xrp_base_fee * 1000000m) * load_factor);

					Gtk.Application.Invoke (delegate {  // exceptions in this thread are not caught but placing it here ensures it only runs if server returns valid info

						this.load_factor_label_var.Text = Base58.truncateTrailingZerosFromString(load_factor.ToString());
						this.base_fee_label_var.Text = base_fee_drops.ToString();
						this.transaction_fee_label_var.Text = transaction_fee.ToString();
					});

				}

				catch ( Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex ) {
					if (Debug.ServerInfo) {
						Logging.write("ServerInfo : onMessage : " + ex.Message + "\n" + ex.ToString() + "\n");
					}
				}

				catch (Exception ex) {
					if (Debug.ServerInfo) {
						Logging.write("ServerInfo : onMessage : exception thrown " + ex.Message + "\n" + ex.ToString() + "\n");
					}
				}


				if (Debug.ServerInfo) {
					Logging.write("Server Info : firing and resetting wait Handle\n");
				}

				//Thread.Sleep(10000); // I used this for testing threading it should never be uncommented

				_waitHandle.Set();
				

				}); // end gtk invoke
			};
 
		}

		private void connectionOpened (object sender, EventArgs e)
		{
			firstconnect = true;

			refresh();


		}


		public static void refresh ()
		{

			Object ob = new {
				command = "server_info"
			};

			String str = DynamicJson.Serialize(ob);
			NetworkInterface.currentInstance.sendToServer(str);
		}

		public static void refresh_blocking ()
		{
			if (Debug.ServerInfo) {
				Logging.write("Server Info : refresh_blocking\n");
			}
			_waitHandle.Reset();
			refresh();
			_waitHandle.WaitOne(); // race condition if network fires before method return?
			if (Debug.ServerInfo) {
				Logging.write("Server Info : refresh_continue\n");
			}
		}

		//bool refresh_block = false;

		bool firstconnect = true;

		public static String build_version = "";
		public static String complete_ledgers = "";
		public static String hostid = "";

		public static Decimal load_factor = 1;
		public static ulong base_fee_drops = 10;

		public static ulong transaction_fee = 10;

		static EventWaitHandle _waitHandle = new ManualResetEvent(true);
	}
}

