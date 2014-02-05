using System;
using RippleClientGtk;
using Gtk;
using Codeplex.Data;
using System.Collections.Generic;



public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{

			if (Debug.MainWindow) {
				Logging.write ("new MainWindow : begin\n");
			}

		Build ();

		if (Debug.MainWindow) {
			if (currentInstance!=null) {
				Logging.write ("Warning. Another instance of MainWindow already exists.\n");
			}


			Logging.write ("MainWindow : build complete\n");
		}


		currentInstance = this;



		// we'll put some global json parsing here but every plugin can subscripe to events
		NetworkInterface.onMessage += delegate(object sender, WebSocket4Net.MessageReceivedEventArgs e) {
			if (Debug.MainWindow) {
				Logging.write ("MainWindow onMessage event\n");
			}

			try 
			{
				if ( Debug.MainWindow ) {
					Logging.write (e.Message + "\n");
				}

				dynamic dynamo = DynamicJson.Parse(e.Message);

				Gtk.Application.Invoke ( delegate 
					{
						// try must be inside of Gtk Invoke to successfully catch the exception 
						// Therefore there MUST be two try catch, one for each thread.
						// The try catch below is necessary for program functionality. 

						if (Debug.MainWindow) {
							Logging.write ("MainWindow onMessage Gtk invoke\n");
						}

						try 
						{
								// basically this WILL fail. It saves from typing dynamo.IsDefined("result") && dynamo.IsDefined("result.account_data") && dynam... ect. It quickly became ridiculous. 
							String balance = dynamo.result.account_data.Balance;
							double index = dynamo.result.ledger_current_index;


							if (this.highestLedger < index) 
							{
									
									try 
									{

										this.xrpBalance = Convert.ToDouble(balance);

										this.sendripple2.setXrpBalance(this.xrpBalance);

										double d = this.xrpBalance / 1000000.0;

										this.balanceLabel.Text = d.ToString();

										} catch (FormatException exf) {
											Logging.write(exf.Message);	
											return; 

										} catch (OverflowException exo) {
											Logging.write(exo.Message);
											return;
										}

								} 

								else {

									Logging.write ("MainWindow : discarding old message");
								}
								
								

								
								

						} catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex) {

							//Logging.write ("This exception is propably intended. If client functions as inteded all is well\n");
							if (Debug.MainWindow) {
								Logging.write(ex.Message);  // make sure it's ex.Message and not e.Message
							}
						}


					} 
				);


				
			}

			catch (Exception ex) {

				Logging.write ("Error parsing json.\n" + ex.Message + "\n");

			}

		};  // end NetworkInterface.onMessage += delegate

	}

	private double highestLedger = 0;

	public static MainWindow currentInstance = null;

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		this.networksettings1.saveSettings ();

		AreYouSure aus = new AreYouSure ("Are you sure you want to close Ripple Client Gtk?");
		aus.Modal = true;

		if (aus.Run () == (int)ResponseType.Ok) {
			aus.Destroy ();
			Application.Quit ();
			a.RetVal = true;
		} else {
			aus.Destroy ();
			a.RetVal = false;
			Application.Run ();
		}




	}

	public Wallet getWallet() {

		return this.wallet1;
	}

	public void setReceiveAddress (String address) {



		this.receivewidget2.setReceiveAddress (address);

	}

	public String getReceiveAddress () {

		return this.receivewidget2.getReceiveAddress();
	}

	public String getSecret() {

		if (this.secret == null) {
			RippleClientGtk.MessageDialog.showMessage ("You need a public and private key. Go to wallet tab and enter your wallet key pair\n");
		}
		return this.secret;
	}

	public void setSecret (String secret) {
		this.secret = secret;
	}

	public double xrpBalance = 0;

	private String secret = null;





}
