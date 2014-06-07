/*
 *	License : Le Ice Sense 
 */

using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Gtk;

namespace RippleClientGtk
{
	public class Rijndaelio : RippleClientGtk.IEncrypt
	{
		public Rijndaelio ()
		{
			this.Name = default_name;
		}

		public static readonly string default_name = "Rijndaelio";

		public static bool isDescribedByString (String s) {


			return (s.Equals(default_name) || s.Equals("default") || s.Equals("default_encryption"));
			
		}

		private static readonly byte [] SALT = new byte[] { 0x24, 0xa7, 0xfc, 0x12, 0x90, 0xb3, 0x5e, 0x6d, 0xe8, 0xc1, 0xa8, 0x3d, 0x03, 0x72, 0x99, 0xf4};  // Note : although this is a randomly derived array changing it's contents may break compatibility with decrypting existing wallets.

		public byte[] encrypt (byte[] message, String password) 
		{

			try {

				using (Rijndael myRijndael = Rijndael.Create())
				{


					using ( Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes (password, SALT) ) 
					{
							myRijndael.Key = bytes.GetBytes(32);
							myRijndael.IV = bytes.GetBytes(16);

							using (MemoryStream memorystream = new MemoryStream ()) 
							{
						
								using (CryptoStream cryptostream = new CryptoStream (memorystream, myRijndael.CreateEncryptor(), CryptoStreamMode.Write))
								{

							

									//byte[] encode = Encoding.ASCII.GetBytes(message);
									byte[] encode = message;
									cryptostream.Write(encode,0,encode.Length);
									cryptostream.Close();

									return memorystream.ToArray();
								}
							}


					}
				}
				

			}


			catch (Exception e) 
			{
				// TODO print to screen debug ect this.mainWindow.
				Logging.write ("Encryption Error, Exception Thrown,\n" + e.Message + "/n");
				return null;
			}


		}

		public byte[] decrypt ( byte [] cipher, String password) {

			try {

				using (Rijndael rijndael = Rijndael.Create())
				{
					using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes (password, SALT))
					{
						rijndael.Key = bytes.GetBytes(32); // if the stupid editor won't let you type this without autocompleting into something else just write it as a comment then uncomment it
						rijndael.IV = bytes.GetBytes(16);

						using (MemoryStream memorystream = new MemoryStream()) 
						{
							using (CryptoStream cryptostream = new CryptoStream (memorystream,rijndael.CreateDecryptor(),CryptoStreamMode.Write))
							{
								cryptostream.Write(cipher, 0, cipher.Length);
								cryptostream.Close();

								byte[] array = memorystream.ToArray();
								//string value = ASCIIEncoding.ASCII.GetString(array);
								//return value;

								return array;
							}
						}

					}
				}
			}

			catch (Exception e) {
				// TODO print/debug
				Logging.write ("Decryption Error, Exception Thrown\n" + e.Message + "\n");
				return null;
			}
		}

		public string Name {
			get {return "Rijndaelio";}
			set {}

		}



	} // class
}  // namespace

