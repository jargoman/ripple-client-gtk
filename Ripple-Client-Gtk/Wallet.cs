using System;
using System.IO;
using System.Text;
using Gtk;
using RippleClientGtk;
using Codeplex.Data;
using Microsoft.CSharp;



namespace RippleClientGtk
{



	[System.ComponentModel.ToolboxItem(true)]
	public partial class Wallet : Gtk.Bin
	{
		public Wallet ()
		{
			if (Debug.Wallet) {
				Logging.write ("new Wallet (Gtk widget)\n");
			}

			this.Build ();

			if (Debug.Wallet) {
				Logging.write ("new Wallet (Gtk widget) build complete\n");
			}


			ontoggle = new global::System.EventHandler (this.OnEncryptCheckBoxClicked);
			this.encryptCheckBox.Clicked += ontoggle;

			walletpath = FileHelper.getSettingsPath ("treasure.dat");

			showsecretcheckbox.Active = false;

			this.secretentry.Visibility = false;



		}


		System.EventHandler ontoggle;



		public byte[] walletcryptomark = System.Text.Encoding.ASCII.GetBytes ("This is an encrypted wallet:"); // Never change this string 

		public string walletpath;

		public bool startup = true;

		//;

		protected void use () {
			if (Debug.Wallet) {
				Logging.write("Wallet : method use() : begin\n");
			}



			String address = this.receiveAddress.Text;
			String secret = this.secretentry.Text;

			// TODO validate keypair

			setKeyPair (address, secret);

		}

		protected void remember ()
		{

			this.remember (walletpath);
		}

		protected bool remember (String path)
		{

			if (Debug.Wallet) {
				Logging.write("Wallet : method remember() : begin \n\twalletpath = " + walletpath);
			}

			// remember keypair

			String pub = this.receiveAddress.Text;
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

								byte[] jsonBytes = rij.encrypt (jsonKey, pass);


								jsonBytearray = new byte[jsonBytes.Length + walletcryptomark.Length];

								walletcryptomark.CopyTo(jsonBytearray, 0);
								jsonBytes.CopyTo(jsonBytearray, walletcryptomark.Length);
								this.setIsWalletEncrypted(true);

							}
						} else {

							Logging.write ("Encrypt wallet has been cancelled\n");

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



		protected bool forget ()
		{

			if (Debug.Wallet) {
				Logging.write("Wallet : method forget : begin\n");
			}

			if (File.Exists (walletpath)) {
				if (Debug.Wallet) {
					Logging.write("Wallet : method forget : Wallet Exists\n");
				}

				AreYouSure ayu = new AreYouSure ("You are about to delete your wallet from the harddrive."
				                                 + " Make sure you have a copy of your address and secret otherwise your coins would be lost\n\n"
				                                 + "Click ok to delete file " + walletpath );

				ayu.Modal = true;
				ResponseType ret = (ResponseType) ayu.Run ();

				if (ret == ResponseType.Ok) {
					if (Debug.Wallet) {
						Logging.write("Wallet : method forget : User clicked ok, deleting file" + walletpath +"\n");
					}

					File.Delete (walletpath);

					ayu.Destroy ();

					return true;
				}



				ayu.Destroy ();
				return false;



			} else {
				if (Debug.Wallet) {
					Logging.write("Wallet : method forget : Wallet DOESN'T Exists\n");
				}
				MessageDialog.showMessage ("There is no wallet to delete");
				return false;

			}
		}

		public bool setKeyPair (String address, String secret) {
			if (Debug.Wallet) {
				Logging.write ("Wallet : method setKeyPair : begin\n"); 
			}
			Gtk.Application.Invoke (delegate {
			if (Debug.Wallet) {
				Logging.write ("Wallet : method setKeyPair : Gtk Invoke : begin\n"); 
			}
				this.receiveAddress.Text = address;
				this.secretentry.Text = secret;


			}
			);

			// TODO verify address and return false if invalid

			MainWindow.currentInstance.setReceiveAddress (address);
			MainWindow.currentInstance.setSecret (secret);

			return true;
		}



		protected void OnUseButtonClicked (object sender, EventArgs e)
		{
			if (Debug.Wallet) {
				Logging.write("Wallet : method OnUseButtonClicked : begin\n");
			}

			// TODO verify addresses
			use ();

		}

		protected void OnRememberbuttonClicked (object sender, EventArgs e)
		{

			if (Debug.Wallet) {
				Logging.write("Wallet : method OnRememberbuttonClicked : begin\n");
			}

			if (File.Exists(walletpath)) {

			if (Debug.Wallet) {
				Logging.write("Wallet : method OnRememberbuttonClicked : file " + walletpath + " already exists\n");
			}


				AreYouSure ays = new AreYouSure ("File " + walletpath + "You are about to overwrite an existing wallet");

				int res = ays.Run ();

				if (res != (int)ResponseType.Ok) {
					if (Debug.Wallet) {
						Logging.write ("Wallet : method OnRememberbuttonClicked : User did not click ok, exiting\n");
					}
					return;
				}

			}

			remember ();
		}

		protected void OnForgetbuttonClicked (object sender, EventArgs e)
		{
			if (Debug.Wallet) {
				Logging.write ("Wallet : method OnForgetbuttonClicked : begin\n");
			}
			forget ();
		}

		public bool loadWallet () {

			return this.loadWallet (walletpath);

		}

		public bool loadWallet ( String path) {
			if (Debug.Wallet) {
				Logging.write("Wallet : method loadWallet ( " + path + " ) : begin\n");
			}

			try {

				Logging.write ("Looking for wallet at " + path.ToString() + "\n");

				if (File.Exists(path)) { // a wallet needs to be loaded

					Logging.write ("Wallet " + path + " Exists!\n");

					String plain = null;

					byte[] bytes = null;


					bytes = File.ReadAllBytes(path);


					byte[] mark = walletcryptomark;


					bool matches = true;
					// check if the bytes from mark match the first bytes from bytes

					if (bytes.Length < mark.Length) { // seems redundant but very important, avoids index out of bounds exception by not bothering to check
						if (Debug.Wallet) {
							Logging.write ("Wallet : method loadwallet : The number of bytes stored in file " + path + " is less than walletcryptomark and therefore not an encrypted wallet\n");
						}
						matches = false;
					} else {
						if (Debug.Wallet) {
							Logging.write ("Wallet : method loadwallet : checking if file is an encrypted gtk wallet\n");
						}
						for (byte i = 0; i < mark.Length; i++) {
							if (!(mark [i] == bytes [i])) {
								matches = false;
							}
						}


					}



					if (matches) {
						// It's an encrypted wallet !!
						if (Debug.Wallet) {
							Logging.write ("Wallet : method loadwallet : file " + path + " has the byte signature of a ripple gtk wallet\n");
						}

						PasswordDialog dial = new PasswordDialog ("Enter the password you used to encrypt your wallet\n");
						dial.Modal = true;


						bool repeat = true;
						do {
							if (Debug.Wallet) {
								Logging.write ("Wallet : method loadwallet : Do while begin : Showing password dialog\n");
							}

							ResponseType resp = (ResponseType)dial.Run ();

							if (resp == ResponseType.Ok) {
								if (Debug.Wallet) {
									Logging.write ("Wallet : method loadwallet : User clicked Ok\n");
								}

								Rijndaelio rij = new Rijndaelio ();

								byte[] encrypted = new byte[bytes.Length - mark.Length]; // above we already verified bytes.Length is larger than mark.Length

								if (Debug.Wallet) {
									Logging.write ("Wallet : method loadwallet : encrypted.Length = " + encrypted.Length + "\n");
								}

								for (int i = 0; i < encrypted.Length; i++) {
									encrypted [i] = bytes [i + mark.Length];
								}



								plain = rij.decrypt (encrypted, dial.getPassword ());

								if (plain == null) {
									if (Debug.Wallet) {
										Logging.write ("Wallet : method loadwallet : decrypted text is null : Password Denied\n");
									}
									MessageDialog.showMessage("Password Denied\n");
									repeat = true;
									continue;
								}

								if (!(processJsonWallet ( plain))) {
									if (Debug.Wallet) {
										Logging.write ("Wallet : method loadwallet : decrypted text is jiberish : Password Denied\n");
									}

									MessageDialog.showMessage("Password Denied\n");
									repeat = true;
									continue;
								}

								else {
									repeat = false;
									Logging.write ("Decrypted Wallet successfully\n");
									if (startup) {
										this.setIsWalletEncrypted(true);
									}

									else {
										this.setIsWalletEncrypted(false);
									}

									dial.Destroy();
									return true;
								}


							} else {
								Logging.write ("Cancelled\n");
								repeat = false;
								dial.Destroy();
								this.setIsWalletEncrypted(true);
								return false;
							}


						} while (repeat);

						// Window closed
						dial.Destroy ();
						this.setIsWalletEncrypted(true);
						return false;

					} else {
						// load plaintext wallet instead of decrypting

						if (Debug.Wallet) {
							Logging.write ("Wallet : method loadwallet : file " + path + " is not an encrypted wallet, treating it as plaintext\n");
						}

						plain = System.Text.ASCIIEncoding.ASCII.GetString (bytes);

						// Logging.write (plain); // Don't uncomment. This is for debug only. It displays the secret to the console. 

						bool loaded = processJsonWallet (plain);



						if (   !loaded  ) {
							String mess = "Invalid treasure.dat";
							MessageDialog.showMessage(mess);
							Logging.write (mess);
							//this.setIsWalletEncrypted(false);
							return false;

						}

						else {
							Logging.write ("Plaintext Wallet successfully loaded\n");
							this.setIsWalletEncrypted(false);
							Logging.write(this.encryptCheckBox.Active.ToString());
							return true;
						}

					}

					/*/////*/






				} // end if wallet file exists

				else {
					Logging.write ("Wallet could not be found\n");
					return false;
				}

			} 

			catch (Exception e) {
				Logging.write ("Wallet : method loadwallet : uncaught exception : " + e.Message + "\n");
				return false;
			}
		} // end loadwallet


		public bool processJsonWallet (String plain) {
			if (Debug.Wallet) {
				Logging.write ("Wallet : method processJsonWallet : begin\n");
			}

			// WARNING variable plain contains secret and should never be printed even in debug mode

			plain = plain.Trim ();

			try {
				if (plain != null && plain.Contains ("{") && plain.Contains ("}") && plain.Contains ("Account") && plain.Contains ("secret")) {
					// it's most likely a wallet
					if (Debug.Wallet) {
						Logging.write ("Wallet :  method processJsonWallet : plaintext passed basic validity text\n");
					}
					//dynamic dynowall = JsonConvert.DeserializeObject<dynamic> (plain);


					dynamic dynowall = DynamicJson.Parse(plain);

					if (dynowall.IsDefined("Account") && dynowall.IsDefined ("secret") ) {
						if (Debug.Wallet) {
							Logging.write("Wallet : method processJsonWallet : both Account and secret are defined\n");
						}

						if ((String)dynowall.Account is String && (String)dynowall.secret is String) {  // redundant?
							if (Debug.Wallet) {
								Logging.write("Wallet : method processJsonWallet : both Account and secret are String\n");
							}
							String acc = (String)dynowall.Account;
							String sec = (String)dynowall.secret;

							setKeyPair (acc, sec);



							//MessageDialog.showMessage ("\n"); // why show message on success although cross platform targeted towards linux users
							return true;

						} else {
							// TODO change the messages
							MessageDialog.showMessage ("Account and Secret aren't valid strings, this might be a bug");
							return false;
						}

					} else {
						MessageDialog.showMessage ("Account and/or Secret are null, this might be a bug\n");
						return false;
					}

				} else {
					MessageDialog.showMessage ("Invalid wallet\n");
					return false;
				}


			} catch (Exception e) {
				Logging.write ("Wallet : method processJsonWallet : uncaught exception : " + e.Message + "\n");
				return false;
			}
		}

		protected void OnShowsecretcheckboxToggled ( object sender, EventArgs e )
		{
			if (Debug.Wallet) {
				Logging.write ("Wallet : method OnShowsecretcheckboxToggled : begin : ");
			}

			this.secretentry.Visibility = showsecretcheckbox.Active; // K.I.S.S :D

		}


		protected void OnEncryptCheckBox ()
		{
			if (Debug.Wallet) {
				Logging.write ("Wallet : checkbox event\n");
			}

			//Gtk.sig




			bool b = this.loadWallet ();
			if (Debug.Wallet) {
				Logging.write ("Wallet : loadwallet result =  " + b.ToString () + "\n");
			}


			if (b) {


				this.remember ();
				//this.toggleIsWalletEncrypted ();

			} else {
				//this.toggleIsWalletEncrypted ();
			}

			//this.firstRun = false;
		}

		private void ImportWallet () {
			if (Debug.Wallet) {
				Logging.write ("Wallet : method ImportWallet : begin\n");
			}

			FileChooserDialog fcd = new FileChooserDialog ("Import Wallet", 
			                                               MainWindow.currentInstance, 
			                                               FileChooserAction.Open,
			                                               "Cancel", ResponseType.Cancel,
			                                               "Open", ResponseType.Accept);


			if (fcd.Run () == (int)ResponseType.Accept) {
				if (Debug.Wallet) {
					Logging.write ("Wallet : method ImportWallet : user chose file " + fcd.Filename + "\n");
				}
				loadWallet (fcd.Filename);
			}

			fcd.Destroy ();

		}

		private void ExportWallet () {
			if (Debug.Wallet) {
				Logging.write ("Wallet : method ExportWallet : begin\n");
			}

			FileChooserDialog fcd = new FileChooserDialog ("Export Wallet",
			                                               MainWindow.currentInstance,
			                                               FileChooserAction.Save,
			                                               "Cancel", ResponseType.Cancel,
			                                               "Save", ResponseType.Accept);

			if (fcd.Run () == (int)ResponseType.Accept) {
				if (Debug.Wallet) {
					Logging.write ("Wallet : method ExportWallet : user chose to export to file " + fcd.Filename + "\n");
				}

				remember (fcd.Filename);
			}

			fcd.Destroy ();
		}

		private void setIsWalletEncrypted (bool b) {
			if (Debug.Wallet) {
				Logging.write ("Wallet : method setIsWalletEncrypted ( " + b.ToString() + " ) : begin\n");
			}

			Gtk.Application.Invoke (
			delegate 
			{
				if (Debug.Wallet) {
					Logging.write ("Wallet : method setIsWalletEncrypted : Delegate = " + b.ToString() + "\n");
					Logging.write("Wallet : method setIsWalletEncrypted : this.encryptCheckBox.Active = " + this.encryptCheckBox.Active + " : before \n");
				}


				this.encryptCheckBox.Clicked -= ontoggle;
				this.encryptCheckBox.Active = b;
				this.encryptCheckBox.Clicked += ontoggle;

				if (Debug.Wallet) {
					Logging.write("Wallet : method setIsWalletEncrypted : this.encryptCheckBox.Active = " + this.encryptCheckBox.Active + " : after \n");
				}

			}

			

			);
		}


		private void toggleIsWalletEncrypted () {
			if (Debug.Wallet) {
				Logging.write("Wallet : method toggleIsWalletEncrypted : begin\n");
			}

			Gtk.Application.Invoke (
				delegate 
				{

					if (Debug.Wallet) {
						Logging.write("Wallet : method toggleIsWalletEncrypted : delegate\n");
					}

					this.encryptCheckBox.Clicked -= ontoggle;
					this.encryptCheckBox.Active = !this.encryptCheckBox.Active;
					this.encryptCheckBox.Clicked += ontoggle;


				}



			);
		}



		// import //
		protected void OnButton2Clicked (object sender, EventArgs e)
		{
			if (Debug.Wallet) {
				Logging.write ("Wallet : method OnButton2Clicked (import wallet): begin\n");
			}
			ImportWallet ();
			//throw new NotImplementedException ();
		}

		// export //
		protected void OnButton3Clicked (object sender, EventArgs e)
		{
			if (Debug.Wallet) {
				Logging.write ("Wallet : method OnButton3Clicked (export wallet): begin\n");
			}
			ExportWallet ();
		}

		protected void OnEncryptCheckBoxClicked (object sender, EventArgs e)
		{
			if (Debug.Wallet) {
				Logging.write ("Wallet : method OnEncryptCheckBoxClicked: begin\n");
			}
			OnEncryptCheckBox();
		}

		protected void OnReceiveAddressActivated (object sender, EventArgs e)
		{

			if (Debug.Wallet) {
				Logging.write ("Wallet : method OnReceiveAddressActivated : begin\n");
			}
			if (secretentry == null) {
				// TODO debug

				Logging.write ("Wallet : method OnReceiveAddressActivated : Critical bug : secretentry = null\n");

				return;
			}

			secretentry.GrabFocus ();
		}

		protected void OnSecretentryActivated (object sender, EventArgs e)
		{
			if (Debug.Wallet) {
				Logging.write("Wallet : method OnSecretentryActivated() : begin\n");
			}

			if (useButton == null) {
				// TODO debug
				Logging.write("Wallet : method OnSecretentryActivated() : Critical bug, useButton \n");

				return;
			}

			useButton.GrabFocus ();

		}



		protected void getReceiveFromSecret (object sender, EventArgs e)
		{
			String Secret = this.secretentry.Text;

			if (Secret == null || Secret.Trim().Equals("")) {
				MessageDialog msg = new MessageDialog("Please enter or generate a secret ripple seed address");
				msg.Run();

				return;
			}

			RippleSeedAddress rsa = new RippleSeedAddress(Secret);

			RippleAddress ra = rsa.getPublicRippleAddress();

			this.receiveAddress.Text = ra.ToString();

		}		

		protected void generateRandomSecret (object sender, EventArgs e)
		{

			bool hastext = false;

			if (receiveAddress.Text == null || receiveAddress.Text.Equals ("")) {
				hastext = true;
			}

			if (secretentry.Text == null || secretentry.Text.Equals ("")) {
				hastext = true;
			}



			if ( !hastext) {
				AreYouSure ays = new AreYouSure("Are you sure you want to overwrite your existing keypair. If you haven't written down your secret all funds in the accound would be lost.");
				ResponseType re = (ResponseType) ays.Run();
				ays.Destroy();
				if (ResponseType.Ok != re) {
					return;
				}
			}

			RandomSeedGenerator rsg = new RandomSeedGenerator();
			rsg.Modal = true;
			ResponseType resp = (ResponseType)rsg.Run ();

			if (resp == ResponseType.Ok) {

				RippleSeedAddress seed = rsg.getGeneratedSeed();

				this.secretentry.Text = seed.ToString();

				RippleAddress ra = seed.getPublicRippleAddress();

				receiveAddress.Text = ra.ToString();
			}

			rsg.Destroy();

		}


	}
}

