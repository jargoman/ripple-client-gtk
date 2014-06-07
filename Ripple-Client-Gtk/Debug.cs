using System;
using System.Reflection;

namespace RippleClientGtk
{
	public static class Debug
	{

		public static bool AccountLines = false;

		public static bool allowInsecureDebugging = false; // VERY IMPORTANT. IF SET SEED / OR PASSWORDS WILL BE INCLUDED IN DEBUGGING INFORMATION

		public static bool BalanceWidget = false;

		public static bool Base58 = false;

		public static bool BinarySerializer = false;

		public static bool BinaryType = false;

		public static bool Console = false;

		public static bool FileHelper = false;

		public static bool FromSecretDialog = false;

		public static bool MainWindow = false;

		public static bool NetworkInterface = false;

		public static bool NetworkSettings = false;



		public static bool PasswordCreateDialog = false;
		public static bool PasswordDialog = false;

		public static bool PluginController = false;

		public static bool ProcessSplash = false;

		public static bool Program = false;

		public static bool RandomSeedGenerator = false;

		public static bool ReceiveWidget = false;

		public static bool RippleBinaryObject = false;

		public static bool RippleDeterministicKeyGenerator = false;

		public static bool RippleTrustSetTransaction = false;

		public static bool RippleIdentifier = false;

		public static bool RippleSeedAddress = false;

		public static bool RippleWallet = false;

		public static bool SendIce = false; // this does NOT toggle if Ice can be sent from client. It toggles debbug mode of SendIce class


		public static bool SendIOU = false;

		public static bool SendRipple = true;

		public static bool ServerInfo = false;

		public static bool testVectors = false;

		public static bool TrustSetter = true;




		public static bool Wallet = false;

		public static bool WalletManager = false;

		public static bool WalletManagerWidget = false;

		public static bool WalletTree = false;

		public static bool WalletViewPort = false;


		public static void setAll (bool boo)
		{
			Type type = typeof (Debug);
			FieldInfo[] fields = type.GetFields();
			foreach (var field in fields) {
				if (field.GetType() == typeof(Boolean)) {
					Logging.write(field.Name + " is a boolean");

					field.SetValue(null,boo); 
				}

				else {
					Logging.write(field.Name + " is NOT a boolean");
				}
			}

		}

		public static bool setDebug (String value)
		{
			//this.GetType().GetField(s);

			String[] options = { "all", "full", "total", "complete", "everything", "allclasses" };

			String[] prefixes = { "debug", "allow", "enable", "set" };

			options = CommandLineParser.attachPrefixes ( options, prefixes );

			foreach (String s in options) {
				if ( value.Equals( s ) ) {

					setAll (true);
					return true;
				}
			}

			String[] values = value.Split(',');

			Type type = typeof(Debug);
			FieldInfo[] fields = type.GetFields ();
			foreach (var s in values) {
				try {
					FieldInfo fi = type.GetField(s);
					if (fi!=null) {
						//fi.SetValue(
						fi.SetValue(null, true); // mark the field that corresponds to a class in the debugger
					}

					else {
						Logging.write ("Value " + (string)(s == null ? "null" : s) + " in not a valid debug symbol" );
					}

				}
				catch (Exception e) {
					Logging.write("Exception in debugger " + e.Message);
					// Todo should return on debuuger input error? I say no unless theres a security risk
					//return false;
				}
			}

			return false;
		}


	}

}

