using System;
using System.IO;
using Gtk;
using Codeplex.Data;

namespace RippleClientGtk
{
	public class RippleWallet
	{
		public RippleWallet (String secret)
		{
			this.seed = new RippleSeedAddress(secret);
			this.Account = seed.getPublicRippleAddress().ToString();
		}

		public RippleWallet (RippleSeedAddress rseed)
		{
			this.seed = rseed;
		}

		public RippleWallet (String encrypted, String encryptionType)
		{
			if (encrypted == null) {
				// Todo debug
				return;
			} else {
				this.encrypted_wallet = encrypted;

				if (encrypted!=null) {
					this.encryption_type = encryptionType;
				}

				else {
					this.encryption_type = Rijndaelio.default_name;
				}
			}




		}

		public RippleWallet (String encrypted, String encryptionType, String receiveAddress) : this ( encrypted, encryptionType ) {
			this.Account = receiveAddress;  // can't verify
		}

		/*
		public void deleteWallet ()
		{
			if (WalletManager.currentInstance!=null) {
				AreYouSure ays = new AreYouSure("Warning you are about to delete wallet " + ((this.walletname!=null) ? this.walletname : "[ No Name ]") + "/n" + ((this.getStoredReceiveAddress())));
					
				ResponseType res = (ResponseType) ays.Run();

				if ( ResponseType.Ok == res ) {
					WalletManager.currentInstance.deleteWallet(this);
				}


			}
		}
		*/

		public string getStoredEncryptionType ()
		{
			if (encryption_type != null) {
				return encryption_type;
			} else {
				if (encrypted_wallet==null && seed != null) {
					return "plaintext";

				}

				else {
					return Rijndaelio.default_name;
				}
			}
		}
		public string getStoredReceiveAddress ()
		{
			if (seed!=null) {
				return seed.getPublicRippleAddress().ToString();
			}

			if (Account!=null) {
				return Account;
			}

			return  "";
		}
		public string walletname = null;

		public RippleSeedAddress seed = null;
		int nextTransactionSequenceNumber = -1; // TODO what's a good "invalid" default number. 

		String encrypted_wallet = null;  // encryptes bytes encoded as ripple Identifier

		String encryption_type = null;

		String Account = null;

		public string walletpath = null;

		public void encrypt (String password, IEncrypt ie)
		{
			byte[] payload = seed.getBytes();

			byte[] enc = ie.encrypt(payload, password);

			encrypted_wallet = Base58.encode(enc);

			encryption_type = ie.Name;

			seed = null;
		}

		public void decrypt (String password, IEncrypt ie)
		{
			byte[] decoded = Base58.decode(encrypted_wallet);

			byte[] decrypted = ie.decrypt(decoded, password);

			seed = new RippleSeedAddress(decrypted);


		}

		public bool forgetDialog ()
		{
		
			if (Debug.RippleWallet) {
				Logging.write("RippleWallet : method forget : begin\n");
			}

			if (File.Exists (walletpath)) {
				if (Debug.RippleWallet) {
					Logging.write("RippleWallet : method forget : Wallet Exists\n");
				}





				AreYouSure ayu = new AreYouSure ("You are about to delete wallet "  
				                                 + ((String)(this.walletname!=null ? this.walletname : " ")) 
				                                 + "from the harddrive."

				                                 + " Make sure you have a copy of your address and secret otherwise your funds at address "
				                                 + this.getStoredReceiveAddress()
				                                 + " would be lost\n\n"
				                                 + "Click \"Ok\" to delete file " 
				                                 + walletpath );

				ayu.Modal = true;
				ResponseType ret = (ResponseType) ayu.Run ();

				if (ret == ResponseType.Ok) {
					if (Debug.RippleWallet) {
						Logging.write("Wallet : method forget : User clicked ok, deleting file" + walletpath +"\n");
					}


					if (WalletManager.currentInstance!=null && walletname!=null) {
						WalletManager.currentInstance.deleteWallet(walletname);
					}

					ayu.Destroy ();

					return true;
				}



				ayu.Destroy ();
				return false;



			} else {
				if (Debug.RippleWallet) {
					Logging.write("Wallet : method forget : Wallet DOESN'T Exists\n");
				}
				MessageDialog.showMessage ("There is no wallet to delete");
				return false;

			}
		}


		public bool forget ()
		{
			try {
				if (walletpath == null) {
					return true;
				}

				if (!File.Exists(walletpath)) {
					return true;
				}

				File.Delete (walletpath);
				return true;
			}

			catch (Exception e) {
				// todo debug
				return false;
			}

		}

		public String toJsonString ()
		{	
			try {  // just in case :) I don't trust dyamic variables, especially in c#


				dynamic d = new DynamicJson ();

				if (seed != null) {
					d.secret = seed.ToString ();
					RippleAddress r = seed.getPublicRippleAddress ();
					if (r != null) {
						d.Account = r.ToString ();
					} else {
						// todo debug
					}
				}

				if (encrypted_wallet != null) {
					d.encrypted = encrypted_wallet;

					if (encryption_type!=null) {
						d.encryption_type = encryption_type;
					}

				} else {
					if (seed!=null && seed.ToString()!=null) {
						d.secret = seed.ToString();
					}
				}

				if (walletname!=null) {
					d.name = walletname;
				}

				object ao = new object {

				};

				return d.ToString();

			}
			catch (Exception e) {
				return null;
			}
		}

		public void ExportWallet () {
			if (Debug.RippleWallet) {
				Logging.write ("RippleWallet : method ExportWallet : begin\n");
			}

			FileChooserDialog fcd = new FileChooserDialog ("Export Wallet",
			                                               MainWindow.currentInstance,
			                                               FileChooserAction.Save,
			                                               "Cancel", ResponseType.Cancel,
			                                               "Save", ResponseType.Accept);

			if (fcd.Run () == (int)ResponseType.Accept) {
				if (Debug.RippleWallet) {
					Logging.write ("Wallet : method ExportWallet : user chose to export to file " + fcd.Filename + "\n");
				}

				save (fcd.Filename);
			}

			fcd.Destroy ();
		}

		public bool save ()
		{
			if (walletpath == null) {
				// todo debug
				return false;
			}

			return this.save(walletpath);
		}

		public bool save (String path) {

			String method_sig = "RippleWallet : save ( " + (String)((path==null) ? "null" : path) + " ) : ";

			if (Debug.RippleWallet) {
				Logging.write(method_sig + "Begin");
			}

			if ( path == null ) {
				// todo debug
				return false;
			}

			String s = toJsonString();

			try {

				File.WriteAllText(path,s);
				return true;
			}

			catch (Exception e) {
				Logging.write(method_sig + "Exception thrown, " + e.Message);
				return false;
			}

		}

		/*
		protected void remember ()
		{

			this.remember (walletpath);
		}

		protected bool remember (String path)
		{

			string method_sig = "RippleWallet : method remember( " + path + " ) : ";

			if (Debug.Wallet) {
				Logging.write( method_sig + "begin \n" );
			}

			// remember keypair

			String pub = this.//this.receiveAddress.Text;
			String priv = this.secretentry.Text;

			Logging.write ("Remember address " + pub + " and secret xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx....\n");


			if ( pub == null || pub.Trim().Equals("") ) { // if empty string
				if (Debug.Wallet) {
					Logging.write("Wallet : method remember() : empty receiveAddress\n");
				}
				MessageDialog.showMessage ("Please enter your receive address.");
				return false;
			}

			if (priv == null || priv.Trim().Equals("")) {
				if (Debug.Wallet) {
					Logging.write("Wallet : method remember() : empty secret string\n");
				}
				MessageDialog.showMessage ("Please enter your secret");
				return false;
			}



			// TODO validate addresses

			dynamic key = new Codeplex.Data.DynamicJson ();

			key.Account = (String)pub;
			key.secret = (String)priv;


		
			String jsonKey = key.ToString ();
			byte[] jsonBytearray = null;


			if (this.encryptCheckBox.Active) {
				if (Debug.Wallet) {
					Logging.write("Wallet : method remember : encryptCheckBox is ticked\n");
				}

				using (PasswordCreateDialog passDial = new PasswordCreateDialog ("Enter a password to encrypt your wallet with. You'll be asked to enter this password when you start the client")) {

					passDial.Modal = true;

					bool repeat = true;

					do {
						if (Debug.Wallet) {
							Logging.write ("Wallet : method remember : Do while begin\n");
						}

						ResponseType resp = (ResponseType)passDial.Run ();

						if (resp == ResponseType.Ok) {
							if (Debug.Wallet) {
								Logging.write("Wallet : method remember : User clicked O.K on password dialog\n");
							}

							int verify = passDial.verifyPasswords ();

							if (verify == PasswordCreateDialog.PASSNOTMATCH) {


								MessageDialog.showMessage("Passwords do not match");
								//Logging.write ("Passwords do not match");
								repeat = true;
								continue;

							} else if (verify == PasswordCreateDialog.PASSISVALID) {

								String pass = passDial.getPassword ();
								passDial.Destroy ();
								repeat = false;

								Logging.write ("User entered two matching valid strings\n");


								// YOU HAVE A PASSWORD, ENCRYPT THE STRING 

								//jsonBytearray

								Rijndaelio rij = new Rijndaelio ();

								byte[] jsonBytes = rij.encrypt (new byte[0], pass);


								jsonBytearray = new byte[jsonBytes.Length + walletcryptomark.Length];

								walletcryptomark.CopyTo(jsonBytearray, 0);
								jsonBytes.CopyTo(jsonBytearray, walletcryptomark.Length);
								this.setIsWalletEncrypted(true);

							}
						} else {
							if (Debug.Wallet) {
								Logging.write ("Encrypt wallet has been cancelled\n");
							}
							//this.setIsWalletEncrypted(false);
							passDial.Destroy ();
							repeat = false;
							return false;
						}
				

					} while (repeat);
				}

			} // END IF ENCRYPT WALLET

			else {

				if (Debug.Wallet) {
					Logging.write("Wallet : method remember : encryptCheckBox is NOT ticked\n");
				}
				jsonBytearray = System.Text.Encoding.ASCII.GetBytes ( jsonKey );

			}


			// so at this point jsonBytearray should contain the wallet bytes for file storage.

			File.WriteAllBytes( path , jsonBytearray);
			return true;

		}
		*/
		
	}
}

