
/*
 *	License : Le Ice Sense 
 */

using System;
using Microsoft.CSharp;
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
		this.loadPlugins();



		if (Debug.MainWindow) {
			if (currentInstance!=null) {
				Logging.write ("Warning. Another instance of MainWindow already exists.\n");
			}

			Logging.write ("MainWindow : build complete\n");
		}






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


			// try must be inside of Gtk Invoke to successfully catch the exception 
						// Therefore there MUST be two try catch, one for each thread.
						// The try catch below is necessary for program functionality. 

			if (Debug.MainWindow) {
				Logging.write ("MainWindow onMessage Gtk invoke\n");
			}

			if (dynamo.IsDefined("error")) {

				String s = dynamo.error as String;
				String account = "";

				if (s != null) {
					if (s.Equals("actNotFound"))  {
						
					
						if (dynamo.isDefined("request.account")) {
							try {
								account = (String)dynamo.request.account;
							}
			
							catch (Exception ex) {
								account = "";
							}
						}
						else {
								account = "";
						}
						

									
						Gtk.Application.Invoke( delegate {
							RippleClientGtk.MessageDialog.showMessage("Account Not Found : To use account " + account + " it must be funded with enough reserve xrp and also an ICE");
						});

						return;
					}
				}


			}

			//dynamo.IsDefined("result")
			String balance;
			if (dynamo.IsDefined("result.account_data.Balance")) {
				balance = dynamo.result.account_data.Balance;
			}
			
			else {
				return;
			}
							
			String acc;
			if (dynamo.IsDefined("result.account_data.Account")) {
				acc = dynamo.result.account_data.Account;
			}

			else {
				return;
			}

			double index; 
			if (dynamo.IsDefined("result.ledger_current_index")) {
				index = dynamo.result.ledger_current_index;
			}
			else {
				return;
			}

			double sequence;
			if (dynamo.IsDefined("result.account_data.Sequence")) {
				sequence = dynamo.result.account_data.Sequence;
			}

			else {
				return;
			}

			this.sequence = (UInt32)sequence;

			if (acc != null && acc == this.getReceiveAddress()) {
							
				if (this.highestLedger < (UInt32)index) 
				{
					Gtk.Application.Invoke ( delegate {

				
						try 
						{
							if (balance == null) {
									return;
							}
							this.xrpBalance = Convert.ToDecimal(balance);

							if (sendripple2!=null) {
								this.sendripple2.setXrpBalance(this.xrpBalance);
							}
							decimal d = this.xrpBalance / 1000000.0m;
									
							String balstr = Base58.truncateTrailingZerosFromString(d.ToString());
											
							if (Debug.MainWindow) {
								Logging.write("Set balanceLabel.Text to " + balstr);
							}

							//this.balanceLabel.Text = balstr;
							this.highestLedger = (UInt32)index;

						//}  catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex) {

							//Logging.write ("This exception is propably intended. If client functions as inteded all is well\n");
							if (Debug.MainWindow) {
								//Logging.write(ex.Message + "\n");  // make sure it's ex.Message and not e.Message
							}
						}
						

						catch (FormatException exf) {
							Logging.write(exf.Message);	
							return; 

						} catch (OverflowException exo) {
							Logging.write(exo.Message);
							return;
						} catch (Exception ex) {
								Logging.write(ex.Message);
						}

					});

				} 

				else {

					Logging.write ("MainWindow : discarding old message\n");
				}
								
			}

			else {
				if (Debug.MainWindow) {
					Logging.write("Received account info for account " + acc.ToString() + " while not current active account");
				}
			}

								
								

		//} catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex) {

			//Logging.write ("This exception is propably intended. If client functions as inteded all is well\n");
			if (Debug.MainWindow) {
		//		Logging.write(ex.Message + "\n");  // make sure it's ex.Message and not e.Message
			}
		}

		catch (Exception ex) {
			Logging.write ("Error parsing json.\n" + ex.Message + "\n");
		}

	};  // end NetworkInterface.onMessage += delegate
		currentInstance = this;
	}

	private UInt32 highestLedger = 0;

	public static MainWindow currentInstance = null;

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		if (this.networksettings1!=null) {
			this.networksettings1.saveSettings ();
		}
		if (RippleClientGtk.Console.currentInstance!=null) {
			RippleClientGtk.Console.currentInstance.saveHistory();
		}

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

	public void loadPlugins ()
	{
		if (PluginController.currentInstance!=null && PluginController.allow_plugins) {
			foreach (Plugin p in PluginController.pluginList) {
				if (p==null) continue;

				Gtk.Widget mainTab = p.getMainTab() as Gtk.Widget;
				if (mainTab!=null) {
					if (this.notebook1!=null) {
						//if (Debug.MainWindow) {
							Logging.write("Appending page");
						//}
						this.notebook1.AppendPage(mainTab,new Label(p.tab_label));
						this.notebook1.ShowAll();
						this.ShowAll();
					}
				}
			}
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

	public decimal xrpBalance = 0;

	private String secret = null;

	public UInt32 sequence = 0;



}
