using System;

// duel license 

namespace RippleClientGtk
{
	public class BinaryType
	{
		// another poor mans enum lol

		public static readonly byte UINT16 = 0x01;
		public static readonly byte UINT32 = 0x02;
		public static readonly byte UINT64 = 0x03;
		public static readonly byte HASH128 = 0x04;
		public static readonly byte HASH256 = 0x05;
		public static readonly byte AMOUNT = 0x06;
		public static readonly byte VARIABLE_LENGTH = 0x07;
		public static readonly byte ACCOUNT = 0x08;
		public static readonly byte OBJECT = 0x0e;
		public static readonly byte ARRAY = 0x0f;
		public static readonly byte UINT8 = 0x10;
		public static readonly byte HASH160 = 0x11;
		public static readonly byte PATHSET= 0x12;
		public static readonly byte VECTOR256 = 0x13;

		// ater porting all the java code. I'm starting to think the below is unneeded
		// why would one need an independed type when a byte could be used. 
		// I made things waaaayyyy more complicated than necessary 
		// plus
		// UINT16 == UINT16
		// but 
		// new BinaryType(UINT16) != new BinaryType(UINT16)

		public byte typeCode;
		public String str;

		public static byte MAXBYTEVALUE = 0;




		public BinaryType (byte typeCode)
		{
			this.typeCode = typeCode;

			switch (typeCode) {
			case 0x01:  //BinaryType.UINT16:
				this.str = "UINT16";
				break;

			case 0x02:  //BinaryType.UINT32:
				this.str = "UINT32";
				break;

			case 0x03:  //BinaryType.UINT64:
				this.str = "UINT64";
				break;

			case 0x04: //BinaryType.HASH128:
				this.str = "HASH128";
				break;

			case 0x05: // BinaryType.HASH256:
				this.str = "HASH256";
				break;

			case 0x06: //BinaryType.AMOUNT:
				this.str = "AMOUNT";
				break;

			case 0x07:  // BinaryType.VARIABLE_LENGTH:
				this.str = "VARIABLE_LENGTH";
				break;

			case 0x08:  // BinaryType.ACCOUNT:
				this.str = "ACCOUNT";
				break;

			case 0x0e:  // BinaryType.OBJECT:
				this.str = "OBJECT";
				break;

			case 0x0f:  // BinaryType.ARRAY:
				this.str = "ARRAY";
				break;

			case 0x10:  // BinaryType.UINT8:
				this.str = "UINT8";
				break;

			case 0x11:  // BinaryType.HASH160:
				this.str = "HASH160";
				break;

			case 0x12:  // BinaryType.PATHSET:
				this.str = "PATHSET";
				break;

			case 0x13:  // BinaryType.VECTOR256:
				this.str = "VECTOR256";
				break;

			default:
				throw new NotImplementedException();
			}


			
		}

		static BinaryType ()
		{
			foreach (BinaryType type in getValues()) {
				//MAXBYTEVALUE = Math.Max (MAXBYTEVALUE, type);
				if (type.typeCode>MAXBYTEVALUE) {
					MAXBYTEVALUE = type.typeCode;
				}
			}

			MAXBYTEVALUE++;

			reverseLookup = new BinaryType[MAXBYTEVALUE];

			foreach (BinaryType type in getValues()) {
				reverseLookup[type.typeCode]=type;
			}
		}

		private static BinaryType[] valuesCache = null;
		public static BinaryType[] getValues ()
		{
			if (valuesCache == null) {
				valuesCache = new BinaryType[] {
					new BinaryType(UINT16),
					new BinaryType (UINT32),
					new BinaryType (UINT64),
					new BinaryType (HASH128),
					new BinaryType (HASH256),
					new BinaryType (AMOUNT),
					new BinaryType (VARIABLE_LENGTH),
					new BinaryType (ACCOUNT),
					new BinaryType (OBJECT),
					new BinaryType (ARRAY),
					new BinaryType (UINT8),
					new BinaryType (HASH160),
					new BinaryType (PATHSET),
					new BinaryType (VECTOR256)
				};
			}

			return valuesCache;
		}

		static readonly BinaryType[] reverseLookup;

		public static BinaryType fromByteValue (byte type)
		{
			if (type<0 || type>=MAXBYTEVALUE) {
				return null;
			}
			return reverseLookup[type];
		}


	}
}

