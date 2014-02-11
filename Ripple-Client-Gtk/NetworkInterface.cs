using System;
using System.Threading;
using SuperSocket.ClientEngine;
using WebSocket4Net;

namespace RippleClientGtk
{
	public class NetworkInterface
	{
		public NetworkInterface (String server, String local, String userAgent, bool reconnect)
		{
			if (Debug.NetworkInterface) {
				Logging.write ("new NetworkInterface\n");
			}

			this.url = server; 
			this.local = local; 
			this.userAgent = userAgent;
			this.reconnect = reconnect;

			currentInstance = this; // my buddy laughs at this 

		}

		int SLEEP_TIME = 2000;
		//Thread.Sleep(
		WebSocket websocket = null;

		String url = "";
		String local = "";
		String userAgent = "";

		public static bool PRINT_RESPONSE = true;


		public bool reconnect = false;

		bool shouldconnect = false; // You don't want to connect automatically 

		public int max_reconnect = 3;

		private int connectattempts = 0;

		private bool stopConnect = false;

		public void disconnect ()
		{

			if (Debug.NetworkInterface) {
				Logging.write ("NetworkInterface : method disconnect() : begin\n");
			}

			shouldconnect = false;

			if (websocket == null) {
				if (Debug.NetworkInterface) {
					Logging.write("NetworkInterface : method disconnect() : websocket == null\n");
				}
				return;
			}


			if (Debug.NetworkInterface) {
				Logging.write ("NetworkInterface : Websocket.State = " + websocket.State.ToString() + "\n");
			}

			switch (websocket.State) {

				case WebSocketState.Closed:
			{
				Logging.write("Connection is already closed\n");
				break;
			}

				case WebSocketState.Closing:
			{
				Logging.write ("Connection is already currently closing\n");
				break;
			}

				case WebSocketState.Open:
			{
				Logging.write ("Closing connection\n");
				try {
					this.websocket.Close ();
				}
				catch (Exception e) {
					Logging.write (e.Message + "\n");
				}
				break;
			}

				case WebSocketState.Connecting:
			{
				Logging.write ("Can't disconnect. Currently connecting.\n");
				try {
					//this.websocket.Close ();
				}
				catch (Exception e) {
					Logging.write ("Exception thrown\n" + e.Message + "\n");
				}
				break;
			}


				default:

			{
				Logging.write ("Socket in an unknown state\n");
				break;
			}
			}
		}

		public bool connecting = false;

		public void connect ()
		{
			if (Debug.NetworkInterface) {
				Logging.write ("NetworkInterface : method connect() : begin\n");
			}

			// launch in new thread 
			Thread thr = new Thread (new ThreadStart (connectthread));
			thr.Start();
		}

		public void connectthread ()
		{
			connecting = true;
			if (Debug.NetworkInterface) {
				Logging.write ("NetworkInterface : method connectthread() : begin\n");
			}

			this.connectattempts = 0;
			shouldconnect = true;

			bool returned = false;
			stopConnect = false;
			while (this.max_reconnect > this.connectattempts && !stopConnect) {
				returned = tryconnect ();
				if (Debug.NetworkInterface) {
					Logging.write("NetworkInterface : method connectthread() : sleep\n");
				}
				Thread.Sleep(SLEEP_TIME);
				if (Debug.NetworkInterface) {
					Logging.write("NetworkInterface : method connectthread() : wake\n");
				}
			}

			if (returned == false && Debug.NetworkInterface && (this.max_reconnect <= this.connectattempts)) {
				Logging.write("NetworkInterface : method connectthread() : exceeded max connect attempts\n");
			}
			connecting = false;
			return;

		}

		private bool tryconnect () {

			if (Debug.NetworkInterface) {
				Logging.write ("NetworkInterface : method tryconnect() : begin\n");
			}

			if (this.max_reconnect <= this.connectattempts) {
				if (Debug.NetworkInterface) {
					Logging.write ("NetworkInterface : method tryconnect() : exceeded max connect attempts, BUG???\n"); // should never fire
				}
				stopConnect = true;
				return false;
			}

			this.connectattempts++;

			if (Debug.NetworkInterface) {
				Logging.write("NetworkInterface : method tryconnect() : connectionattempts = " + connectattempts.ToString() + "\n");
			}

			this.webSocketDisposal ();




			// TODO validate url's

			String url = this.url;
			String userAgent = this.userAgent;
			String local = this.local;

			// prints in debug mode or not
			Logging.write ("Initiating connecting to server " + this.url + "\n");

			try {

				//this.websocket = new WebSocket (this.url);
				this.websocket = new WebSocket (
					url,
					"",
					null,
					null,
					userAgent,
					local,
					(WebSocketVersion)(-1) );

			}

			catch (ArgumentException e){



				Logging.write ("Exception thrown, Invalid URL : "  + url + "\n");


				Logging.write ("Syntax is [protocol]://[domain]:[port]\nExample : ws://s1.ripple.com:80 or (ssl) wss://s1.ripple.com:443\n");

				if (Debug.NetworkInterface) {
					Logging.write(e.Message);
				}

				stopConnect = true;

				return false;
			}

			catch (Exception e) {

				Logging.write ("Exception thrown : \n" + e.Message + "\n");


				// TODO, we have to decide if we want to reconnect or not based on error. 

				Logging.write ("Exception thrown : \n" + e + "\n");

				stopConnect = true;
				return false;

			}

			if (websocket==null) {
				Logging.write ("Unable to create websocket, did you use a valid url?\n");
				stopConnect = true;
				return false;
			}

			websocket.Opened += new EventHandler(websocket_Opened);

			websocket.Error += delegate(object sender, ErrorEventArgs e) {

				stopConnect = true;





				if ( e.Exception.Message.Equals("RemoteCertificateNotAvailable") ) {

					String message = "Unable to establish an ssl connection, you need to install the servers ssl security certificate\n"
					+ "Example # certmgr --ssl " + url + "       ( using the command line of your local operating system terminal/cmd.exe ect )\n";

					//MainWindow.currentInstance


					Logging.write (message);

					Gtk.Application.Invoke ( delegate {
						MessageDialog.showMessage(message);
					});




					return;
				}

				if (Debug.NetworkInterface) {
					Logging.write ( "Error Occured\n" + e.Exception.Message + "\n" );
				}
				stopConnect = true;
				return;
			};

			websocket.Closed += new EventHandler(websocket_Closed);

			websocket.MessageReceived += delegate(object sender, MessageReceivedEventArgs e) {
				if (Debug.NetworkInterface) {
					Logging.write("NetworkInterface : event websocket.MessageReceived : begin");
				}

				if (PRINT_RESPONSE || Debug.NetworkInterface) {
					Logging.write ("Response from server " + url + ":\n" + e.Message + "\n\n");
				}

				this.processIncomingJson(e);
			};


			try {
				websocket.Open();
			}
			catch (Exception e) 
			{
				Logging.write (e.ToString() + "\nError opening connection to " + url + " : \n" + e.Message + "\n" );
			}

			if (websocket.State == WebSocketState.Open) {
				if (Debug.NetworkInterface) {
					Logging.write("NetworkInterface : method tryconnect() : websocket.State = Open, url = " + url + ", this.url = " + this.url + "/n" );
				}

				return true;
			}

			else {

				if (Debug.NetworkInterface) {
					Logging.write("NetworkInterface : method tryconnect() : websocket.State = " + websocket.State.ToString() + ", url = " + url + ", this.url = " + this.url + "/n");
				}

				return false;
			}



		}

		private void websocket_Closed (object sender, EventArgs e)
		{
			Logging.write ("Connection Closed\n");
			if (onClose != null) {
				onClose (sender, e);
			} else {
				Logging.write("NetworkInterface : Error : onClose == null\n");
			}

			if (this.reconnect && this.shouldconnect && !this.connecting) {
				if (Debug.NetworkInterface) {
					Logging.write("NetworkInterface : method websocket_Closed() : auto reconnecting\n");
				}
				//this.connect ();
			}
		}



		private void websocket_Opened (object sender, EventArgs e)
		{
			this.stopConnect = true;
			this.connectattempts = 0;
			Logging.write ("Connection Opened\n");
			if (onOpen != null) {
				onOpen (sender, e);
			}
			else {
				Logging.write ("NetworkInterface : Error: onOpen == null\n");
			}


		}



		public void sendToServer( String message) {
			if ( websocket == null || websocket.State != WebSocketState.Open) {

				if (this.reconnect && this.shouldconnect) {
					this.tryconnect ();

					// let the server breathe
					System.Threading.Thread.Sleep (10);

					if (websocket == null || websocket.State != WebSocketState.Open) {
						Logging.write ("You need to be connected to a server to send a message.\n");
						return;
					}


				} else {

					Logging.write ("You need to be connected to a server to send a message.\n");
					return;
				}
			}

			else {
				Logging.write ( "Sending message :\n" + message + "\n" );
				try {

					this.websocket.Send ( message );

				}

				catch (Exception e) {

					Logging.write ( "Error sending message : An exception was thrown.\n" + e.Message);
					return;
				}


			}

			if (websocket != null) {

				// TODO I don't think this web socket library has any error flags to check??? (websockets4net), it's event based, see on error event, also catching exceptions above


			}

			else {
				Logging.write ("Error, websocket address is now null. Is another thread calling websocket_dispose?\n");
			}

		}

		public void sendToServer (byte[] message)	{
			sendToServer(message,0, message.Length);
		}
		
		public void sendToServer( byte[] message, int byteoffset, int bytelength) {
			if ( websocket == null || websocket.State != WebSocketState.Open) {

				if (this.reconnect && this.shouldconnect) {
					this.tryconnect ();

					// let the server breathe
					System.Threading.Thread.Sleep (10);

					if (websocket == null || websocket.State != WebSocketState.Open) {
						Logging.write ("You need to be connected to a server to send a message.\n");
						return;
					}


				} else {

					Logging.write ("You need to be connected to a server to send a message.\n");
					return;
				}
			}

			else {
				Logging.write ( "Sending binary message\n"/*:\n" + message + "\n"*/ );
				try {

					this.websocket.Send ( message, byteoffset, bytelength );


				}

				catch (Exception e) {

					Logging.write ( "Error sending message : An exception was thrown.\n" + e.Message);
					return;
				}


			}

			if (websocket != null) {

				// TODO I don't think this web socket library has any error flags to check??? (websockets4net), it's event based, see on error event, also catching exceptions above


			}

			else {
				Logging.write ("Error, websocket address is now null. Is another thread calling websocket_dispose?\n");
			}

		}


		protected void webSocketDisposal () {
			if (Debug.NetworkInterface) {
				Logging.write("NetworkingInterface : method webSocketDisposal() : begin \n");
			}

			if (this.websocket!=null) {
				if (Debug.NetworkInterface) {
					Logging.write("NetworkingInterface : method webSocketDisposal() : this.websocket!=null\n");
				}
				this.disconnect ();
				this.websocket = null;
			}

			else {
				if (Debug.NetworkInterface) {
					Logging.write("NetworkingInterface : method webSocketDisposal() : websocket=null\n");
				}
			}
		}




		private void processIncomingJson (MessageReceivedEventArgs e)
		{
			//dynamic incoming = JsonConvert.DeserializeObject<dynamic>(jsonString);

			if (Debug.NetworkInterface) {
				Logging.write ("NetworkingInterface : method processIncomingJson (MessageReceivedEventArgs e) : begin ");
			}

			if (onMessage != null) {

				onMessage (this, e);

			} else {
				Logging.write("NetworkInterface : method processIncomingJson : Critical Error : onMessage == null\n");
			}

		}


		public delegate void OnMessageEventHandler(object sender, MessageReceivedEventArgs e);

			public static OnMessageEventHandler onMessage;
			
			
		public static NetworkInterface currentInstance;


		public delegate void connectEventHandler (object sender, EventArgs e);

			public static connectEventHandler onOpen;

			public static connectEventHandler onClose;

		public delegate void errorEventHandler (object sender, ErrorEventArgs e);

		public static errorEventHandler onError;

	}





}

