using System;

namespace RippleClientGtk
{
	public static class Debug
	{



		public static bool AccountLines = false;

		public static bool allowInsecureDebugging = false; // VERY IMPORTANT. IF SET SEED / OR PASSWORDS WILL BE INCLUDED IN DEBUGGING INFORMATION

		public static bool BalanceWidget = false;

		public static bool Base58 = false;

		public static bool BinarySerializer = true;

		public static bool BinaryType = false;

		public static bool Console = false;

		public static bool MainWindow = false;

		public static bool NetworkInterface = false;

		public static bool NetworkSettings = false;



		public static bool PasswordCreateDialog = false;
		public static bool PasswordDialog = false;
		public static bool Program = false;

		public static bool RandomSeedGenerator = true;

		public static bool ReceiveWidget = false;

		public static bool RippleBinaryObject = false;

		public static bool RippleDeterministicKeyGenerator = false;

		public static bool RippleIdentifier = false;

		public static bool RippleSeedAddress = false;

		public static bool SendIce = false; // this does NOT toggle if Ice can be sent from client. It toggles debbug mode of SendIce class


		public static bool SendIOU = true;

		public static bool SendRipple = false;

		public static bool ServerInfo = true;

		public static bool testVectors = false;




		public static bool Wallet = false;


	}

}

