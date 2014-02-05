using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Gtk;

namespace RippleClientGtk
{
	public class Rijndaelio
	{
		public Rijndaelio ()
		{

		}

		private static readonly byte [] SALT = new byte[] { 0x24, 0xa7, 0xfc, 0x12, 0x90, 0xb3, 0x5e, 0x6d, 0xe8, 0xc1, 0xa8, 0x3d, 0x03, 0x72, 0x99, 0xf4};

		public byte[] encrypt (String message, String password) 
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

							

									byte[] encode = Encoding.ASCII.GetBytes(message);

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

		public String decrypt ( byte [] cipher, String password) {

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
								string value = ASCIIEncoding.ASCII.GetString(array);
								return value;
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



	} // class
}  // namespace

