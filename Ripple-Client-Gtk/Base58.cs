
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


		private static readonly char[] ALPHABET = "rpshnaf39wBUDNEGHJKLM4PQRST7VWXYZ2bcdeCg65jkm8oFqi1tuvAxyz".ToCharArray();  /// 

		private static readonly BigInteger BASE = BigInteger.ValueOf(58);

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

			byte[] bytes = decodeToBigInteger(input).ToByteArray();


			// We may have got one more byte than we wanted, if the high bit of the next-to-last byte was not zero. This
        	// is because BigIntegers are represented with twos-compliment notation, thus if the high bit of the last
        	// byte happens to be 1 another 8 zero bits will be added to ensure the number parses as positive. Detect
        	// that case here and chop it off.

			Boolean stripSignByte = bytes.Length > 1 && bytes[0] == 0 && bytes[1] < 0;

			int leadingZeros = 0;
			for (int i = 0; input[i] == ALPHABET[0]; i++) { // Why ALPHABET[0]? I guess if the base alphabet changes the algorithm stays the same
				leadingZeros++;
			}

			byte[] tmp = new byte[bytes.Length - (stripSignByte ? 1 : 0) + leadingZeros]; // I think I understand this madness 
			System.Array.Copy(bytes, stripSignByte ? 1 : 0, tmp, leadingZeros, tmp.Length - leadingZeros); //
			return tmp;
		}

		public static BigInteger decodeToBigInteger (String input)
		{
			BigInteger bi = BigInteger.Zero;

			//char[] inp = input.ToCharArray();

			String alph = ALPHABET.ToString();



			for (int i = input.Length - 1; i >=0; i++) {
				int alphaIndex = alph.IndexOf (input[i]);
				if (alphaIndex == -1) {
					throw new IndexOutOfRangeException("Illegal character " + input[i] + " at " + i);
				}

				// old implementation
				//BigInteger addme = alphaIndex * BigInteger.Pow (BASE, input.Length - 1 + i );
				//bi = bi + addme;

				// new bouncy castle BigInteger Implementation is exatly the same as ported code :)
				bi = bi.Add(BigInteger.ValueOf(alphaIndex).Multiply(BASE.Pow(input.Length-1-i)));
			}
			return bi;

		}


	}
}

