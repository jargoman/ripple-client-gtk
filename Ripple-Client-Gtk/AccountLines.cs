/*
 *	License : Le Ice Sense 
 */


using System;
using System.Collections.Generic;
using Codeplex.Data;
using Gtk;

namespace RippleClientGtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class AccountLines : Gtk.Bin
	{
		public AccountLines ()
		{
			this.Build ();

			if (Debug.AccountLines) {
				Logging.write ("new AccountLine :\n");
			}


			NetworkInterface.onMessage += delegate(object sender, WebSocket4Net.MessageReceivedEventArgs e) {


				// basically this WILL fail. It saves from typing dynamo.IsDefined("result") && dynamo.IsDefined("result.account_data") && dynam... ect. It quickly became ridiculous. 
				try 
				{

					if (Debug.AccountLines) {
						Logging.write ("AccountLines : on message event.\n" + e.Message + "\n");
					}


					dynamic dynamo = DynamicJson.Parse(e.Message);
					var arr = dynamo.result.lines; // moved this up here

						



							  // often throws runtime binder exception. 

					String account = "";

					List<TrustLine> lines = new List<TrustLine>(); // list of trust lines. 



					foreach (dynamic l in arr) {
						TrustLine trust = new TrustLine(l);
						// if new trust is a type safe object


						if ( trust.currency.Equals(LICENSE) ) {

							if (trust.account.Equals(AccountLines.LICENSE_ISSUER)) {

								if (trust.getBalanceAsDecimal() > AccountLines.LICENSE_FEE) {
									hasIce = true; // ta da !!
								}

								else {
									Gtk.Application.Invoke ( delegate {
										RippleClientGtk.MessageDialog.showMessage("Use this application requires the account " + account + "to be funded with " + AccountLines.LICENSE_FEE + " " + AccountLines.LICENSE + ".");
									});
									return;
								}
							}

							else {
								Gtk.Application.Invoke( delegate {

						
									MessageDialog.showMessage("Warning : only " + AccountLines.LICENSE + " issued by account " + AccountLines.LICENSE_ISSUER + " are valid licenses");
								});
							}

						}

						lines.Add ( trust );
					}

					AccountLines.lines = lines; // different variables with different scope, same name.

					this.setTable(lines);
					this.addUpCurrencies();

							/*
							if (this.highestLedger < index) 
							{

							} 
							*/

				} 

				catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex) {
					if (Debug.AccountLines) {
						Logging.write ("This exception is propably intended. If client functions as inteded all is well\n");
						Logging.write(ex.Message);  // make sure it's ex.Message and not e.Message
					}

				}


				catch (Exception ex) {


				}

			};  // end NetworkInterface.onMessage += delegate

		}

		public void autoTrustLicense ()
		{
			//AreYouSure ays = new AreYouSure();
		}

		// variables !!



		public Table tabl = null;

		public void setTable (List<TrustLine> li) {
			if (Debug.AccountLines) {
				Logging.write ("AccountLines : setTable Fired\n" + li.ToString() + "\n");
			}
			TrustLine[] lines = li.ToArray ();

			//Logging.write (lines[0].account);


			uint len = 0;

			try {
				len = checked ((uint)lines.Length);
			}
			catch (System.OverflowException e) {
				Logging.write ("Exception thrown in Class AccountLines, lines.Lenth is not a valid unsigned int. " + e.Message);
				return;
			}

			if (len == 0) {
				if (Debug.AccountLines) {
					Logging.write ("AccountLines : No Account lines to display\n");
				}
				return;
			}

			if (Debug.AccountLines) {
				Logging.write ("AccountLines : Creating Table");
			}

			String[] titles = TrustLine.getTableTitleRow ();

	
			Gtk.Application.Invoke( delegate {

		

				if (tabl!=null) {
					//this.scrolledwindow1.Remove (tabl);
					//this.scrolledwindow1
					tabl.Destroy ();
				}

				tabl = new Table (
					len + 1,  // add one for the title row
					TrustLine.numColumns,
					false
				);





				//Logging.write ("Beginning for loop");
				for (uint x = 0; x < TrustLine.numColumns; x++) {


					String text = "<big><b><u> " + titles [x] + " </u></b></big>";
					Label title = new Label (text);
					title.UseMarkup = true;

					tabl.Attach (
						title,
						x,
						x + 1,
						0,
						1,
						AttachOptions.Expand,
						AttachOptions.Shrink,
						5,
						4

					);

					title.Show ();



					for (uint y = 0; y < len; y++) {
				
						TrustLine line = lines [y];

						String tex = line.getTableXIndex (x).ToString();

						Label lab = new Label (tex);


						lab.Selectable = true;

						lab.Visible = true;

						lab.CanFocus = false;

						lab.Sensitive = true;




						lab.Justify = Justification.Left;

						tabl.Attach (
							lab,
							x,
							x + 1,
							y + 1,
							y + 2,
							AttachOptions.Shrink,
							AttachOptions.Shrink,
							5,
							2
						);

						lab.Yalign = 0.5f;
						lab.Xalign = 0.0f;
						//lab.Ypad = 0;
					//lab.Xpad = 0;

						lab.Show ();

						if (Debug.AccountLines) {
							Logging.write ("Account Line Table : x = " + x.ToString() + " y = " + y.ToString() + " label " + tex );
						}

					}


				}

				//this.scrolledwindow1.Add (tabl);
				this.scrolledwindow1.AddWithViewport (tabl);
				tabl.Show ();

				MainWindow.currentInstance.ShowAll ();
			});
		}

		private void addUpCurrencies ()
		{

			if (lines == null) {
				if (Debug.AccountLines) {
					Logging.write ("AccountLines : addUpCurrencies : returning early because lines == null");
				}
				return;
			}

			if (Debug.AccountLines) {
				Logging.write ("AccountLines : Adding up IOU's");
			}

			if (cash != null) {
				cash.Clear ();  // avoid calling new by clearing the old "cash cache"
			} else {
				cash = new Dictionary<string, decimal> ();
			}

			foreach (TrustLine line in AccountLines.lines) {
				String cur = line.currency;

				if (Debug.AccountLines) {
					Logging.write (cur);
				}


				if (cash.ContainsKey (cur)) {

					decimal total;

					cash.TryGetValue (cur, out total);

					total += line.getBalanceAsDecimal ();

					cash.Remove (cur);

					if (Debug.AccountLines) {
						Logging.write (cur + " = " + total);
					}

					cash.Add (cur, total);
				} else {

					if (Debug.AccountLines) {
						Logging.write (cur + " = " + line.getBalanceAsDecimal());
					}

					cash.Add (cur, line.getBalanceAsDecimal());
				}

			}

			Dictionary<String,Decimal>.ValueCollection vc = cash.Values;

			var keys = cash.Keys;

			String[] currencies = new string[keys.Count];

			int x = 0;
			foreach (String k in keys) {
				currencies [x++] = k;
			}

			if (SendIOU.currentInstance != null) {
				SendIOU.currentInstance.setCurrencies (currencies);

			} else {
				// TODO debug
			}

			if (SendAndConvert.currentInstance != null) {
				SendAndConvert.currentInstance.setCurrencies (currencies);

			} else {
				// TODO debug
			}

			if (BalanceWidget.currentInstance != null) {


				Gtk.Application.Invoke ( delegate  {
					List<CurrencyWidget> widglist = new List<CurrencyWidget>(x);

					foreach (String str in currencies) {
						CurrencyWidget widge = new CurrencyWidget();

						widge.set(str, getCurrencyTotal(str).ToString());
						widglist.Add(widge);
					}


					BalanceWidget.currentInstance.setTable(widglist);
				});
			}

			if (BalanceTab.currentInstance != null) {
				BalanceTab.currentInstance.set(BalanceTabOptionsWidget.actual_values);
			}
		}

		public static decimal getCurrencyTotal (String currency)
		{
			if (currency == null) {
				if (Debug.AccountLines) {
					Logging.write("AccountLines.getCurrencyTotal : currency = null");
				}
				return 0m;
			}

			if (Debug.AccountLines) {
				Logging.write("AccountLines.getCurrencyTotal : begin");
			}

			Decimal result = 0m;

			if (cash == null) {
				// TODO can cash be populated durring boot
				return result; 
			}

			bool success = cash.TryGetValue (currency, out result);



			if (success) {
				if (result == null) {
					return 0m;
				}

				return result;
			} else {
				return 0m;
			}
		}

		public static List<String> getIssuersForCurrency (String currency)
		{
			List<String> strings = new List<string>();

			if (lines != null) {
				foreach (TrustLine line in lines) {
					if (line.currency.Equals(currency)) {
						strings.Add(line.account);
					}
	
				}
			}

			return strings;
		}


		public static List<TrustLine> lines = null;
		public static Dictionary <String, Decimal> cash = null;

		public static bool hasIce = false;

		public static String LICENSE = "ICE";

		public static String LICENSE_ISSUER = RippleAddress.RIPPLE_ADDRESS_ICE_ISSUER.ToString();

		public static decimal LICENSE_FEE = 0.987654321m;
	}
}

