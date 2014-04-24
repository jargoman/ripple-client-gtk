using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace RippleClientGtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Console : Gtk.Bin
	{
		public Console ()
		{
			this.Build ();

			if (Debug.Console) {
				Logging.write ("new Console :\n");
			}

			this.consoleentry.Activated += new EventHandler (this.OnConsoleentryActivated);
			this.sendbutton.Clicked += new EventHandler (this.OnSendbuttonClicked);

			Logging.textview = this.consoleView;

			//this.consoleView.

			if (tokens != null) {
				history = new List< string > (tokens);
			} else {
				history = new List<string> ();
			}

			currentInstance = this;

		}

		static Console () {
			settingsPath = FileHelper.getSettingsPath(historyFileName);
		}

		public static Console currentInstance = null;

		public static readonly int max_lines_default = 50;

		public int max_lines = max_lines_default;

		public static String historyFileName = "consoleHistory.txt";
		public static String settingsPath = null;

		public static String[] tokens = null;

		List<String> history = new List<string>();

		private void send ()
		{
			if (Debug.Console) {
				Logging.write ("Console : method send : begin\n");
			}

			// simple... send the raw imput to server

			String mess = this.consoleentry.Text;

			if (mess == null || mess.Equals("")) {
				if (Debug.Console) {
					Logging.write ("Console : entry text is empty\n");
				}
				return;
			}

			this.updateHistory(mess);

			if (NetworkInterface.currentInstance == null) {

				if (Debug.Console) {
					Logging.write ("Console : NetworkInterface.currentInstance == null");
				}

				MessageDialog.showMessage ("You have to be connected to a server to send a json command.\nGo to the network settings tab");
				return;
			}
			NetworkInterface.currentInstance.sendToServer (mess);
		}

		protected void OnSendbuttonClicked (object sender, EventArgs e)
		{
			if (Debug.Console) {
				Logging.write ("Console : Event OnSendbuttonClicked fired\n");
			}

			send ();

		}

		protected void OnConsoleentryActivated (object sender, EventArgs e)
		{
			if (Debug.Console) {
				Logging.write ("Console : Event OnConsoleentryActivated fired\n");
			}

			send ();
		}

		private void updateHistory (String str)
		{
			this.history.Add(str);

			while (history.Count > max_lines) {
				history.RemoveAt(history.Count-1);
			}

		}		

		protected void historyUp (object sender, EventArgs e)
		{
			historyIndex++;

			if (historyIndex >= history.Count ) {
				historyIndex = history.Count - 1;
			}

			historySeek();
		}		

		protected void historyDown (object sender, EventArgs e)
		{
			historyIndex--;

			if (historyIndex < 0) {
				historyIndex = 0;
			}

			historySeek();
		}

		private void historySeek ()
		{
			if (history.Count <= 0) {
				return;
			}

			if (history.Count < historyIndex) {
				return;
			}

			String str = history[history.Count - historyIndex - 1];

			if (str == null) {
				return;
			}

			this.consoleentry.Text = str;
		}

		public static void loadHistory ()
		{

			if (settingsPath == null) {
				// Todo debug
			}

			String his = FileHelper.getJsonConf (settingsPath);

			if (his != null) {
				tokens = his.Split( new string[] {"\n"}, StringSplitOptions.RemoveEmptyEntries );

			}



		}

		public void saveHistory ()
		{
			StringBuilder str = new StringBuilder();

			foreach (String s in history) {
				str.Append(s + "\n");
			}

			String st = str.ToString();

			File.WriteAllText( settingsPath, st);
		}

		int historyIndex = 0;


	}
}

