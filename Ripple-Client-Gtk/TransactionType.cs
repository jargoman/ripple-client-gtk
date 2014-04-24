using System;

namespace RippleClientGtk
{
	public class TransactionType
	{
		// Yet another poor mans enum class. 
		// Enums in c# can only be primitive data types. 
		// I want them to be full fledged classes as in java
		// this is the solution I came up with :/

		public static readonly UInt16 PAYMENT = 0x00;
		public static readonly UInt16 CLAIM = 0x01;
		public static readonly UInt16 WALLET_ADD = 0x02;
		public static readonly UInt16 ACCOUNT_SET = 0x03;
		public static readonly UInt16 PASSWORD_FUND = 0x04;
		public static readonly UInt16 REGULAR_KEY_SET = 0x05;
		public static readonly UInt16 NICKNAME_SET = 0x06;
		public static readonly UInt16 OFFER_CREATE =0x07;
		public static readonly UInt16 OFFER_CANCEL = 0x08;
		public static readonly UInt16 CONTRACT = 0x09;
		public static readonly UInt16 CONTRACT_REMOVE = 0x0a;
		public static readonly UInt16 TRUST_SET = 0x14;
		public static readonly UInt16 FEATURE = 0x64;
		public static readonly UInt16 FEE = 0x65;

		public static int MAXBYTEVALUE=0;
		static TransactionType[] reverseLookup;

		static TransactionType ()
		{
			foreach (TransactionType type in getValues()) {  // note not values as is the case in the java code, values could be null
				MAXBYTEVALUE = Math.Max (MAXBYTEVALUE, (int)type.uint16Value);
			}

			MAXBYTEVALUE++;

			reverseLookup = new TransactionType[MAXBYTEVALUE];
			foreach (TransactionType type in getValues()) {
				reverseLookup[type.uint16Value] = type; // may have to cast byteValue to int
			}
		}


		private static TransactionType[] values = null;


		public static TransactionType[] getValues() {

			loadTransactionTypes();

			return TransactionType.values;
		}

		public static void loadTransactionTypes ()
		{
			if (TransactionType.values==null) {

				TransactionType.values = new TransactionType[] {
					new TransactionType(TransactionType.PAYMENT),
					new TransactionType(TransactionType.CLAIM),
					new TransactionType(TransactionType.WALLET_ADD),
					new TransactionType(TransactionType.ACCOUNT_SET),
					new TransactionType(TransactionType.PASSWORD_FUND),
					new TransactionType(TransactionType.REGULAR_KEY_SET),
					new TransactionType(TransactionType.NICKNAME_SET),
					new TransactionType(TransactionType.OFFER_CREATE),
					new TransactionType(TransactionType.OFFER_CANCEL),
					new TransactionType(TransactionType.CONTRACT),
					new TransactionType(TransactionType.CONTRACT_REMOVE),
					new TransactionType(TransactionType.TRUST_SET),
					new TransactionType(TransactionType.FEATURE),
					new TransactionType(TransactionType.FEE)
				};
			}
		}

		public UInt16 uint16Value;
		//public String ToString = "";

		TransactionType (UInt16 txTypeByteValue)
		{
			this.uint16Value = txTypeByteValue;



		}

		public static TransactionType fromType ( UInt16 txType )
		{
			if (txType < 0 || txType >= MAXBYTEVALUE) {
				return null;
			}

			return reverseLookup[txType];
		}

		public override bool Equals (Object obj)
		{
			if (obj == null) {
				return false;
			}

			TransactionType tst = obj as TransactionType;
			if ((Object)tst == null) {
				return false;
			}

			return this.Equals((TransactionType)tst);
		}

		public bool Equals (TransactionType tst)
		{
			if ((Object)tst == null) {
				return false;
			}

			return this.uint16Value == tst.uint16Value;
		}

		public static bool operator ==(TransactionType left, TransactionType right)
		{
			return left.Equals(right);
		}
 
		public static bool operator !=(TransactionType left, TransactionType right)
		{
			return !left.Equals(right);
		}


	}
}

