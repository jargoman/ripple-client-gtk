using System;
using System.IO;
using System.Text;
using Org.BouncyCastle.Math;
using System.Collections.Generic;

namespace RippleClientGtk
{
	public class BinarySerializer
	{
		//private static readonly int MIN_OFFSET = -96;
		//private static readonly int MAX_OFFSET = 80;
		//private static readonly long MIN_VALUE = 1000000000000000;
		//private static readonly long MAX_VALUE = 9999999999999999;

		public BinarySerializer ()
		{
			
		}

		private static byte[] FromDecimal(decimal d)
			{
				byte[] bytes = new byte[16];
 
				int[] bits = decimal.GetBits(d);
				int lo = bits[0];
				int mid = bits[1];
				int hi = bits[2];
				int flags = bits[3];
 				
				bytes[0] = (byte)lo;
				bytes[1] = (byte)(lo >> 8);
				bytes[2] = (byte)(lo >> 0x10);
				bytes[3] = (byte)(lo >> 0x18);
				bytes[4] = (byte)mid;
				bytes[5] = (byte)(mid >> 8);
				bytes[6] = (byte)(mid >> 0x10);
				bytes[7] = (byte)(mid >> 0x18);
				bytes[8] = (byte)hi;
				bytes[9] = (byte)(hi >> 8);
				bytes[10] = (byte)(hi >> 0x10);
				bytes[11] = (byte)(hi >> 0x18);
				bytes[12] = (byte)flags;
				bytes[13] = (byte)(flags >> 8);
				bytes[14] = (byte)(flags >> 0x10);
				bytes[15] = (byte)(flags >> 0x18);
 
				return bytes;
			}


		public static int getDecimalScale (Decimal d)
		{
			byte[] bytes = FromDecimal(d);

				
				
				
			sbyte scale = (sbyte) bytes[14];

 

			if (Debug.BinarySerializer) {
				Logging.write("BinarySerializer : getDecimalScale : scale of Decimal " + d.ToString() + " is " + scale.ToString() + "\n");
			}

			return (int)scale;



		}

		public static BigInteger getDecimalUnscaledValue (Decimal d)
		{

				byte[] bytes = FromDecimal (d);

				byte[] unscaledValueBytes = new byte[12];
				Array.Copy(bytes, unscaledValueBytes, unscaledValueBytes.Length);

				// because decimal is little endian and BigInteger is big endian  // TODO MAKE DAMN SURE!!!
				Array.Reverse(unscaledValueBytes);
				
				var unscaledValue = new BigInteger(1, unscaledValueBytes);
				
				if (bytes[15] == 128)
				unscaledValue = unscaledValue.Negate();  //.Multiply(BigInteger.ValueOf(-1)); // changes the sign
 

			if (Debug.BinarySerializer) {
				Logging.write("BinarySerializer : getDecimalUnscaledValue : unscaled of Decimal " + d.ToString() + " is " + unscaledValue.ToString() + "\n");
			}

			return unscaledValue;
		}

		/*
		public static byte[] flipEndianess ( byte[] input )
		{
			byte[] output = new byte[ input.Length ];

			// basically flip the bytes
			for (int i = 0; i < output.Length; i++) {
				output[i]=input[output.Length-(i+1)];
			}

			return output;
		}
		*/

		/*
		public static byte[] prepareBigIntegerBytes (byte[] input)
		{
			byte[] returnMe = new byte[input.Length+1];
			byte[] flipped = flipEndianess(input);
			System.Array.Copy(flipped,returnMe,flipped.Length);
			returnMe[input.Length]=0; // make the last element zero for the bigint


			return returnMe;
		}
		*/

		/*
		public static void serializeTransaction ()
		{

		} */

		public RippleBinaryObject readBinaryObject (MemoryStream ms)
		{
			RippleBinaryObject serializedObject = new RippleBinaryObject ();
			 
			 
				using ( BigEndianReader reader = new BigEndianReader(ms) ) {
					
					for (int i = 0; i < ms.Length;i++) {
						byte firstByte = reader.ReadByte(); 
						int type = (0xF0 & firstByte) >> 4; // The & (AND) is redundant with unsigned bytes but I'll leave it incase I switch to sbyte
						if (type==0) 
						{
							type = reader.ReadByte(); // if the type is zero, the next byte is the type
						}

						int field=0x0f & firstByte; // zero out first four bits of byte. 
						if (field==0) {
							field = reader.ReadByte(); 
							//firstByte=(byte)field; // I commented this out because it's never used
						}

						BinaryFieldType serializedField = BinaryFieldType.lookup(type,field);
						Object value = readPrimitive (reader, serializedField.type);
						serializedObject.fields.Add(serializedField, value);



					}
				}

			return serializedObject;
		}


		protected Object readPrimitive (BigEndianReader input, BinaryType primative)
		{
			// I'll comment out the ported java code and replace it with c# equivelent. 
			// I think the java code is "hacked" to accomodate the missing unsinged primitives that c# provides

			if (primative.typeCode == BinaryType.UINT16) {
				//return 0xFFFFFFFF & input.ReadInt16 ();
				return input.ReadUInt16 ();

			} else if (primative.typeCode == BinaryType.UINT32) {
				//return 0xFFFFFFFFFFFFFFFF & input.ReadInt32();
				return input.ReadUInt32 ();

			} else if (primative.typeCode == BinaryType.UINT64) {
				//byte[] eightBytes = input.ReadBytes(8);
				//return new BigInteger(eightBytes);
				return input.ReadUInt64 ();
			} else if (primative.typeCode == BinaryType.HASH128) {
				return input.ReadBytes (16);
			} else if (primative.typeCode == BinaryType.HASH256) {
				return input.ReadBytes (32);
			} else if (primative.typeCode == BinaryType.AMOUNT) {
				return readAmount (input);
			} else if (primative.typeCode == BinaryType.VARIABLE_LENGTH) {
				return readVariableLength (input);
			} else if (primative.typeCode == BinaryType.ACCOUNT) {
				return readAccount (input);
			} else if (primative.typeCode == BinaryType.OBJECT) {
				// TODO implement
				throw new NotImplementedException("Object type, not yet supported");
			} else if (primative.typeCode == BinaryType.ARRAY) {
				throw new NotImplementedException("Array type, not yet supported");
			} else if (primative.typeCode == BinaryType.UINT8) {
				return 0xFFFF & input.ReadByte(); // I suppose it is intended for the hex 0xFFFF to be 16 bits long?
			} else if (primative.typeCode == BinaryType.HASH160) {
				return readIssuer(input); // not a bug?
			} else if (primative.typeCode == BinaryType.PATHSET) {
				return readPathSet(input);
			} else if (primative.typeCode == BinaryType.VECTOR256) {
				throw new NotImplementedException("VECTOR256 type, not yet supported");
			}
			throw new NotImplementedException("Unsupported primitive " + primative);
		}


		protected RippleAddress readAccount(BigEndianReader input) {
			 byte[] accountBytes = readVariableLength(input);
			return new RippleAddress(accountBytes);
		}

		byte[] readVariableLength (BigEndianReader input)
		{
			int byteLen = 0;
			byte firstByte = input.ReadByte ();
			byte secondByte = 0;
			if (firstByte < 192) {  // why 192?
				byteLen = firstByte;
			} else if (firstByte < 240) {
				secondByte = input.ReadByte ();
				byteLen = 193 + (firstByte - 193) * 256 + secondByte; // TODO come back to this ported code and make sense of this lol

			} else if (firstByte < 254) {
				secondByte = input.ReadByte ();
				byte thirdByte = input.ReadByte ();
				byteLen = 12481 + (firstByte - 241) * 65536 + secondByte * 256 + thirdByte; // one of the bytes obviously marks the length?
			} else {
				// TODO Error checking
				throw new Exception("firstByte="+firstByte+", value reserved");
			}

			//byte[] variableBytes = new byte[byteLen];
			return input.ReadBytes(byteLen); // the wonderful moment the code starts to make sense. 
		}


		protected DenominatedIssuedCurrency readAmount (BigEndianReader input)
		{

			// TODO go back over this and make sure calculations are precise enough for financials. 
			// Also take into account the sign byte

			//long offsetNativeSignMagnitudeBytes = input.ReadInt64 ();
			ulong offsetNativeSignMagnitudeBytes = input.ReadUInt64();

			//1 bit for Native // I'm confused because of the sign bit

			// I added a cast to long.

			//Boolean isXRPAmount = false;

			// TODO figure this out. ?????
			//unchecked {
			//	isXRPAmount = ((long)0x8000000000000000 & offsetNativeSignMagnitudeBytes) == 0;
				// 
			//}

			Boolean isXRPAmount = (0x8000000000000000 & offsetNativeSignMagnitudeBytes) == 0; // zero's everything but the isXRP bit

			// checked
			//isXRPAmount = (0x8000000000000000 & offsetNativeSignMagnitudeBytes) == 0;


			int sign = ((0x4000000000000000 & offsetNativeSignMagnitudeBytes) == 0) ? -1 : 1;

			ulong precast = offsetNativeSignMagnitudeBytes & 0x3FC0000000000000;

			int offset = (int)(precast >> 54); 

			ulong longMagnitude = offsetNativeSignMagnitudeBytes & 0x3FFFFFFFFFFFFF;

			// confused even more because of scale

			if (isXRPAmount) {
				Decimal magnitude = longMagnitude;
				magnitude = magnitude * sign;
				return new DenominatedIssuedCurrency (magnitude);

			} else {
				String currencyStr = readCurrency(input);
				RippleAddress issuer = readIssuer(input);

				if (offset==0 || longMagnitude == 0) {
					return new DenominatedIssuedCurrency (Decimal.Zero, issuer, currencyStr);
				}

				int decimalPosition = 97-offset;

				if (decimalPosition<DenominatedIssuedCurrency.MIN_SCALE || decimalPosition>DenominatedIssuedCurrency.MAX_SCALE) {
					throw new ArgumentOutOfRangeException("invalid scale "+decimalPosition);
				}

				//BigInteger biMagnitude = longMagnitude;


				Decimal fractionalValue = new decimal(longMagnitude) * new decimal(Math.Pow (10,  decimalPosition));  // veryfy this is correct
				if (sign < 0) {
					fractionalValue *= -1m;
				}

				return new DenominatedIssuedCurrency (fractionalValue, issuer, currencyStr);
			}

		}

		protected RippleAddress readIssuer (BigEndianReader input)
		{
			byte[] issuerbytes = input.ReadBytes(20);

			// TODO If the issuer is all 0, this means any issuer

			return new RippleAddress (issuerbytes);
		}

		protected String readCurrency (BigEndianReader input) 
		{
			char[] unknown = input.ReadChars(12);

			char[] currency = input.ReadChars(8);

			return new String(currency, 0, 3);

			 //TODO See https://ripple.com/wiki/Currency_Format for format
		}

		protected RipplePathSet readPathSet ( BigEndianReader input/*ArraySegment<Byte> bb*/)
		{
			RipplePathSet pathSet = new RipplePathSet ();
			RipplePath path = null;
			//using (MemoryStream ms = new MemoryStream ( bb.Array )) { 
				//using (BinaryReader input = new BinaryReader(ms)) {

					while (true) {
						byte pathElementType = input.ReadByte();
						if (pathElementType==(byte)0x00) {
							break;
						}

						if (path==null) {
							path = new RipplePath();

							pathSet.Add(path);
						}

						if (pathElementType==(byte)0xff) {
							path = null;
							continue;
						}

						RipplePathElement pathElement = new RipplePathElement();
						path.Add(pathElement);

						// NOTE bit tinkering is not my thing. I know so many languages I get them confused. 
						// this code was ported from java. The line (pathElementType&0x01)!=0 and such may be a 
						// hack to deal with the sign bit. Java lacks unsigned data types but c# doesn't lack these types

						if ((pathElementType&0x01)!=0) { // Account bit is set
							pathElement.account = readIssuer(input);
						}

						if ((pathElementType&0x10)!=0) {
							pathElement.currency = readCurrency(input);
						}

						if ((pathElementType&0x20)!=0) {
							pathElement.issuer = readIssuer(input);
						}
					}

					return pathSet;
				//}
			//}
		}

		public MemoryStream writeBinaryObject (RippleBinaryObject serializedObj)
		{
			if (Debug.BinarySerializer) {
				Logging.write("Binary Serializer : write binary object : " + serializedObj.toJSONString());
			}

			MemoryStream memstream = new MemoryStream ();
			using (BigEndianWriter output = new BigEndianWriter(memstream)) {

				List<BinaryFieldType> sortedFeilds = serializedObj.getSortedFields();
				foreach (BinaryFieldType field in sortedFeilds) {
					byte typeHalfByte=0;
					if (field.type.typeCode<=15) {
						typeHalfByte = (byte) (field.type.typeCode<<4);
					}
					byte fieldHalfByte = 0;
					if (field.value<=15) {
						fieldHalfByte = (byte) (field.value&0x0F);
					}

					output.Write((byte) (typeHalfByte|fieldHalfByte));  // ooohh the moment of zen I see the bytes being OR'ed together
					// Note it seems the values are written to stream even if they are zero.


					if (typeHalfByte==0) {
						output.Write((byte) field.type.typeCode);
					}

					if (fieldHalfByte==0) {
						output.Write((byte) field.value);
					}

					object valu = serializedObj.getField(field);

					if (Debug.BinarySerializer) {
						Logging.write("BinarySerializer : BinaryFieldType " + field.ToString() + " = Type : " + field.type.ToString() + ", Value : " + valu.ToString());
					}

					writePrimitive (output, field.type, valu);




				}

				output.Flush();
				memstream.Flush();
			} // end using BinaryWriter



			return memstream;
		}


		protected void writePrimitive (BigEndianWriter output, BinaryType primitive, Object value)
		{
			if (Debug.BinarySerializer) {
				Logging.write("BinarySerializer : writePrimitive : expected BinaryType is " + primitive.ToString() + ", actual value is " + value.GetType().ToString());
			}

			// ok going to comment much of this ported code and take advantage of c#'s unsigned types
			if (primitive.typeCode == BinaryType.UINT16) {

				UInt16 intValue = (UInt16) value; 

				output.Write (intValue);

			} else if (primitive.typeCode == BinaryType.UINT32) {
				UInt32 longValue = (UInt32) value;
				//if (longValue>(long)0xFFFFFFFF) {
				//	throw new InternalBufferOverflowException( "UINT32 overflow for value " + value );
				//}
				//output.Write((byte) (longValue>>24&0xFF));
				//output.Write((byte) (longValue>>16&0xFF));
				//output.Write((byte) (longValue>>8&0xFF));
				//output.Write((byte) (longValue&0xFF));

				//Logging.write(value.GetType().ToString());

				output.Write ((UInt32)value);

			} else if (primitive.typeCode == BinaryType.UINT64) {
				// spend days porting RipplePrivateKey.bigIntegerToBytes() only to clue in there is an uint in c# :O
				//byte[] biBytes = (uint) value; //  RipplePrivateKey.bigIntegerToBytes((BigInteger) value, 8);
				UInt64 ulvalue = (UInt64)value;
				output.Write (ulvalue);

			} else if (primitive.typeCode == BinaryType.HASH128) {
				byte[] sixteenBytes = (byte[])value;  // change to sbyte?
				if (sixteenBytes.Length != 16) {
					throw new InvalidCastException ("Value " + value + " is not a HASH128");
				}
				output.Write (sixteenBytes); 

			} else if (primitive.typeCode == BinaryType.HASH256) {
				byte[] thirtyTwoBytes = (byte[])value;
				if (thirtyTwoBytes.Length != 32) {
					throw new InvalidCastException ("Value " + value + " is not a HASH256");
				}
			} else if (primitive.typeCode == BinaryType.AMOUNT) {

				writeAmount (output, (DenominatedIssuedCurrency)value);

			} else if (primitive.typeCode == BinaryType.VARIABLE_LENGTH) {
				writeVariableLength (output, ((byte[])value));
			} else if (primitive.typeCode == BinaryType.ACCOUNT) {
				writeAccount (output, (RippleAddress)value);
			} else if (primitive.typeCode == BinaryType.OBJECT) {
				throw new NotImplementedException ("Object type, not yet supported");
			} else if (primitive.typeCode == BinaryType.ARRAY) {
				throw new NotImplementedException ("Array type, not yet supported");
			} else if (primitive.typeCode == BinaryType.UINT8) {
				int intValue = (int)value;
				if (intValue > 0xFF) {
					throw new OverflowException ("UINT8 overflow for value " + value);
				}
				output.Write ((byte)value);
			} else if (primitive.typeCode == BinaryType.HASH160) {
				writeIssuer (output, (RippleAddress)value);
			} else if (primitive.typeCode == BinaryType.PATHSET) {
				writePathSet (output, (RipplePathSet)value);
			} else if (primitive.typeCode == BinaryType.VECTOR256) {
				throw new NotImplementedException ("VECTOR256 type, not yet supported");
			} else {
				throw new NotSupportedException ("Unsupported primitive "+primitive);
			}
		}

		protected void writePathSet (BigEndianWriter output, RipplePathSet pathSet)
		{

			for (int i = 0; i < pathSet.Count; i++) {
				RipplePath path=pathSet[i];
				for (int j=0; j<path.Count; j++) {
					RipplePathElement pathElement = path[j];
					byte pathElementType=0;
					if (pathElement.account!=null) {
						pathElementType|=0x01;  // TODO why 0x01 when BinaryType.account = 0x08??
					}
					if (pathElement.currency!=null) {
						pathElementType|=0x10;   // I assume they are bit flags. I remember seeing this on the wiki
					}
					if (pathElement.issuer!=null) {
						pathElementType|=0x20;
					}
					output.Write(pathElementType);

					if (pathElement.account!=null) {
						writeIssuer(output, pathElement.account);
					}

					if (pathElement.currency!=null) {
						writeCurrency(output, pathElement.currency);
					}

					if (pathElement.issuer!=null) {
						writeIssuer(output, pathElement.issuer);
					}

					if ( (i+1==pathSet.Count) && (j+1==path.Count) ) {
						goto END; // that's right, I used a goto. Sue me lol
					}
				}

				output.Write((byte) 0xFF);

			}

			END:
				output.Write((byte) 0);
		}

		protected void writeIssuer (BigEndianWriter output, RippleAddress value)
		{
			if (Debug.BinarySerializer) {
				Logging.write("BinarySerializer : writeIssuer : begin");
			}
			byte[] issuerBytes = value.getBytes ();
			if (Debug.BinarySerializer) {
				Logging.write("BinarySerializer : writeIssuer : issuerBytes.Length = " + issuerBytes.Length.ToString());
			}
			output.Write(issuerBytes);
		}

		protected void writeAccount(BigEndianWriter output, RippleAddress address) {
                writeVariableLength(output, address.getBytes());
        }


		

		protected void writeAmount (BigEndianWriter output, DenominatedIssuedCurrency denominatedCurrency)
		{
			if (Debug.BinarySerializer) {
				Logging.write ("\nBinarySerializer : writeAmount begin");
			}

			ulong offsetNativeSignMagnitudeBytes = 0;
			if (denominatedCurrency.amount > 0m) {

				if (Debug.BinarySerializer) {
					Logging.write ("BinarySerializer : writeAmount : amount > 0m");
				}
				offsetNativeSignMagnitudeBytes |= 0x4000000000000000;  // Note I thought this was incorrect untill I read this https://ripple.com/wiki/Binary_Format#Native_Currency
			} 


			if (denominatedCurrency.isNative()) {//if (denominatedCurrency.currency == null) {
				if (Debug.BinarySerializer) {
					Logging.write("BinarySerializer : writeAmount : is Native currency");
				}

				if (denominatedCurrency.amount > ulong.MaxValue) {
					throw new OverflowException ("denominatedCurrency.amount is larger than long.MaxValue");
				}

				ulong drops = (ulong)denominatedCurrency.amount;
				offsetNativeSignMagnitudeBytes |= drops;
				output.Write (offsetNativeSignMagnitudeBytes);

			} else {
				if (Debug.BinarySerializer) {
					Logging.write("BinarySerializer : writeAmount : is Non Native");
				}

				offsetNativeSignMagnitudeBytes|=0x8000000000000000;

				BigInteger unscaledValue = getDecimalUnscaledValue (denominatedCurrency.amount);


				if (!(unscaledValue.Equals(BigInteger.Zero))) {
					if (Debug.BinarySerializer) {
						Logging.write("BinarySerializer : writeAmount : unscaledValue != 0");
					}

					int scale = getDecimalScale(denominatedCurrency.amount); // I don't think there's a such thing as a negative scale. I could be wrong. 
					UInt64 mantissa = (UInt64)unscaledValue.Abs().LongValue;

					while (mantissa > DenominatedIssuedCurrency.MAX_MANTISSA ) {
						mantissa /= 10;
						scale--;  // scale is flipped

						if (scale > DenominatedIssuedCurrency.MAX_SCALE) {
							throw new OverflowException("Scale is greater than MAX_SCALE");
						}
					}

					while (mantissa < DenominatedIssuedCurrency.MIN_MANTISSA) {
						mantissa *= 10;
						scale++;

						if (scale < DenominatedIssuedCurrency.MIN_SCALE) {
							throw new OverflowException("Scale is smaller than MIN_SCALE");
						}
					}

					if (Debug.BinarySerializer) {
						Logging.write("BinarySerializer : writeAmount : mantissa = " + mantissa.ToString());
					}

					ulong orme = 97UL - (ulong)scale; // scale is flipped

					orme <<= 54;
					orme |= mantissa;

					offsetNativeSignMagnitudeBytes |= orme;

				}
				if (Debug.BinarySerializer) {
					byte[] bits = BitConverter.GetBytes(offsetNativeSignMagnitudeBytes);

					if (BitConverter.IsLittleEndian) {
						Array.Reverse(bits);
					}

					String bitstring = "";
					foreach (byte b in bits) {
						bitstring +=  Convert.ToString(b, 2).PadLeft(8,'0') + " ";
					}

					Logging.write("BinarySerializer : writeAmount : offsetNativeSignMagnitudeBytes = " + bitstring);
				}

				output.Write(offsetNativeSignMagnitudeBytes);
				writeCurrency(output,denominatedCurrency.currency);
				writeIssuer(output, denominatedCurrency.issuer);
			}
		}


		// TODO Unit test this function
		protected void writeVariableLength (BigEndianWriter output, byte[] value)
		{
			if (value.Length<192) {
				output.Write((byte) value.Length);
			}
			else if (value.Length<12480) { //193 + (b1-193)*256 + b2

						// TODO figure out what Pmarches meant by this is not right
						//FIXME This is not right...
                        int firstByte=(value.Length/256)+193;
                        output.Write((byte) firstByte);
                        //FIXME What about arrays of length 193?
                        int secondByte=value.Length-firstByte-193;
                        output.Write((byte) secondByte);
			}

			else if (value.Length<918744) {
						int firstByte=(value.Length/65536)+241;
                        output.Write((byte) firstByte);
                        int secondByte=(value.Length-firstByte)/256;
                        output.Write((byte) secondByte);
                        int thirdByte=value.Length-firstByte-secondByte-12481;
                        output.Write((byte) thirdByte);
			}
			output.Write(value);
		}

		protected void writeCurrency (BigEndianWriter output, String currency)
		{
			if (Debug.BinarySerializer) {
				Logging.write("BinarySerializer : writeCurrency : begin : currency = " + currency);
			}

			byte[] currencyBytes = new byte[20];
			// I don't think this is needed. Trying everything to get this to work :/
			for (int i = 0; i < currencyBytes.Length; i++) {
				currencyBytes[i] = 0;
			}
			//char[] str = currency.ToCharArray ();



			// TODO what to do if ripple implements currency codes larger than 3 bytes. 
			/*
			if (currency.Length > 3) {
				throw new NotImplementedException ("Currency codes greater than 3 chars are not currently supported");
			}
			*/

			/*
			byte[] source = new byte[3];
			try {
				int i = 0;
				foreach (char c in str) {
					source[i++] = Convert.ToByte(c);
				}
			} catch (OverflowException e) {
				// TODO debug
				throw e;
			}
			*/

			//byte[] sour = Encoding.UTF8.GetBytes(currency);

			byte[] sour = ASCIIEncoding.ASCII.GetBytes(currency);

			if (Debug.BinarySerializer) {
				Logging.write("BinarySerializer : writeCurrency : Ascii length = " + sour.Length);
			}
			/*
			byte[] source = Encoding.BigEndianUnicode.GetBytes(currency);
			if (Debug.BinarySerializer) {
				Logging.write("BinarySerializer : writeCurrency : bigendianUnicode length = " + source.Length);
			}
			*/


             System.Array.Copy(sour, 0, currencyBytes, 12, 3);
				
				
                output.Write(currencyBytes);

		}
	}

}

