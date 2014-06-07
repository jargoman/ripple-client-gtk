
/**
* Copyright 2013 jargoman. // this is a port of pmarches RippleBase58.java 
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Text;
using Org.BouncyCastle.Math;
//using System.Numerics;


namespace RippleClientGtk
{
	public static class Base58
	{


		public static readonly String ALPHABET = "rpshnaf39wBUDNEGHJKLM4PQRST7VWXYZ2bcdeCg65jkm8oFqi1tuvAxyz";  /// 

		private static readonly BigInteger BASE = BigInteger.ValueOf(58);

		public static bool isBase58 (String str)
		{
			foreach (char c in str) {
				if (!isBase58(c)) {
					return false;
				}

				else {
					continue;
				}
			}

			return true;

		}

		public static bool isBase58 (Char c)
		{
			foreach (char a in ALPHABET) {
				if (a.Equals(c)) {return true;};
			}

			return false;
		}

		public static String encode (byte[] input)
		{
			BigInteger bi = new BigInteger (1, input);
			StringBuilder s = new StringBuilder();

			try {
				while (bi.CompareTo(BASE)>=0) { // while 
					BigInteger mod = bi.Mod(BASE); 

					//
					//s.Insert(0,ALPHABET[(int)mod]); // old implementation // this is the cast that could throw an overflow exception
					s.Insert(0,ALPHABET[mod.IntValue]); // The new implementation shouldn't throw an exception. It uses BouncyCastles BigInteger

					//bi = (bi - mod) / BASE; // old implementation
					bi = bi.Subtract(mod).Divide(BASE);
				}
				// found a bug in old implementation it was s.Insert(0,ALPHABET[0]); the second zero being the bug. Must have been ide autocomplete
				s.Insert(0, ALPHABET[bi.IntValue]); 

			//} catch (OverflowException e) { // old
			} catch (Exception e) {
				// TODO debug
				// Far less likely an exception would be thrown. Might as well keep a generic catch for potentially unforseen bugs. Divide by zero? ?? who knows. 
				throw e;

			}

			foreach (byte anInput in input) {
				if (anInput == 0) {
					s.Insert(0, ALPHABET[0]);
				}
				else {
					break;
				}
			}

			return s.ToString(); 
		}

		public static byte[] decode (string input)
		{

			if (Debug.Base58) {
				Logging.write ("Base58.decode " + input + "\n");
			}

			byte[] bytes = decodeToBigInteger (input).ToByteArray ();

			if (Debug.Base58) {
				var sb = new StringBuilder ("ToByteArray() = { ");
				foreach (var b in bytes) {
					sb.Append (b + ", ");
				}
				sb.Append ("}\n");

				Logging.write (sb.ToString ());

			}

			// We may have got one more byte than we wanted, if the high bit of the next-to-last byte was not zero. This
			// is because BigIntegers are represented with twos-compliment notation, thus if the high bit of the last
			// byte happens to be 1 another 8 zero bits will be added to ensure the number parses as positive. Detect
			// that case here and chop it off.

			Boolean stripSignByte = bytes.Length > 1 && bytes [0] == 0 && (sbyte)bytes [1] < 0;



			int leadingZeros = 0;
			for (int i = 0; input[i] == ALPHABET[0]; i++) { // Why ALPHABET[0]? I guess if the base alphabet changes the algorithm stays the same
				leadingZeros++;
			}

			byte[] tmp = new byte[bytes.Length - (stripSignByte ? 1 : 0) + leadingZeros]; // I think I understand this madness 
			System.Array.Copy(bytes, stripSignByte ? 1 : 0, tmp, leadingZeros, tmp.Length - leadingZeros); //

			//byte[] tmp = new byte[bytes.Length - (stripSignByte ? leadingZeros : 0)]; // I think I understand this madness 
			//System.Array.Copy(bytes, stripSignByte ? leadingZeros : 0, tmp, 0, tmp.Length - leadingZeros); //


			if (Debug.Base58) {
				Logging.write("stripSignByte = " + stripSignByte + "\n");
				Logging.write("leadingZeros = " + leadingZeros + "\n");
				var sb = new StringBuilder ("tmp[] = { ");
				foreach (var b in tmp) {
					sb.Append (b + ", ");
				}
				sb.Append ("}\n");

				Logging.write (sb.ToString ());

			}

			return tmp;
		}

		public static BigInteger decodeToBigInteger (String input)
		{
			if (Debug.Base58) {
				Logging.write ("Base58.decodeToBigInteger " + input + "\n");
			}

			BigInteger bi = BigInteger.Zero;

			//char[] inp = input.ToCharArray();

			String alph = ALPHABET;
			if (Debug.Base58) {
				Logging.write ("ALPHABET = " + alph + "\n");
			}


			for (int i = input.Length - 1; i >=0; i--) {
				int alphaIndex = alph.IndexOf (input [i]);

				if (Debug.Base58) {
					Logging.write (input [i] + " = " + alphaIndex.ToString () + ", ");
				}

				if (alphaIndex == -1) {
					throw new IndexOutOfRangeException ("Illegal character " + input [i] + " at " + i);
				}

				// old implementation
				//BigInteger addme = alphaIndex * BigInteger.Pow (BASE, input.Length - 1 + i );
				//bi = bi + addme;

				// new bouncy castle BigInteger Implementation is exatly the same as ported code :)
				bi = bi.Add (BigInteger.ValueOf (alphaIndex).Multiply (BASE.Pow (input.Length - 1 - i)));
			}

			if (Debug.Base58) {
				Logging.write("returning bigInt : " + bi.ToString());
			}

			return bi;

		}


		public static string ByteArrayToHexString (byte[] bytes)
		{
			StringBuilder result = new StringBuilder (bytes.Length * 2);
			string HexAlphabet = "0123456789ABCDEF";

			foreach (byte b in bytes) {
				result.Append(HexAlphabet[(int) (b >> 4)]);
				result.Append(HexAlphabet[(int) (b & 0x0F)]);
			}

			return result.ToString();
		}

		// We could move this later. I put it here so not to add another class just for a single function. Same with the hex function above
		public static string truncateTrailingZerosFromString (String str)
		{
			if (str == null || str == "" ) {
				return str;
			}

			str.Trim();

			if (str.Contains(".")) return str.TrimEnd('0').TrimEnd('.');

			return str;
		}

	}
}

