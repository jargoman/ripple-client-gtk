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


				try 
				{

					if (Debug.AccountLines) {
						Logging.write ("AccountLines : on message event.\n" + e.Message + "\n");
					}
					 

					dynamic dynamo = DynamicJson.Parse(e.Message);


					Gtk.Application.Invoke ( delegate 
					                        {
						// try must be inside of Gtk Invoke to successfully catch the exception 
						// Therefore there MUST be two try catch, one for each thread.
						// The try catch below is necessary for program functionality. 

						try 
						{
							// basically this WILL fail. It saves from typing dynamo.IsDefined("result") && dynamo.IsDefined("result.account_data") && dynam... ect. It quickly became ridiculous. 


							var arr = dynamo.result.lines;  // often throws runtime binder exception. 

							List<TrustLine> lines = new List<TrustLine>(); // list of trust lines. 



							foreach (dynamic l in arr) {
								TrustLine trust = new TrustLine(l);
								// if new trust is a type safe object


								if (trust.currency.Equals(license) && trust.getBalanceAsDouble() > 0.987654321) {
									hasIce = true;
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

						} catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex) {
							if (Debug.AccountLines) {
								Logging.write ("This exception is propably intended. If client functions as inteded all is well\n");
								Logging.write(ex.Message);  // make sure it's ex.Message and not e.Message
							}

						}


					} 
					);



				}

				catch (Exception ex) {

					Logging.write ("AccountLines : Error parsing json.\n" + ex.Message + "\n");

				}

			};  // end NetworkInterface.onMessage += delegate

		}

		// variables !!



		public Table tabl = null;

		public void setTable (List<TrustLine> li) {
			if (Debug.AccountLines) {
				Logging.write ("AccountLines : setTable Fired\n" + li.ToString() + "\n");
			}
			TrustLine[] lines = li.ToArray ();

			//Logging.write (lines[0].account);

			if (tabl!=null) {
				//this.scrolledwindow1.Remove (tabl);
				//this.scrolledwindow1
				tabl.Destroy ();
			}
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
					Logging.write ("No Account lines to display\n");
				}
				return;
			}

			if (Debug.AccountLines) {
				Logging.write ("Creating Table");
			}

			tabl = new Table (
				len + 1,  // add one for the title row
				TrustLine.numColumns,
				false
				);



			String[] titles = TrustLine.getTableTitleRow ();

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
		}

		private void addUpCurrencies () {

			if (lines==null) {
				// TODO  debug
				return;
			}



			cash = new Dictionary<string, double> ();


			if (Debug.AccountLines) {
				Logging.write ("AccountLines : Adding up IOU's");
			}
			foreach (TrustLine line in AccountLines.lines) {
				String cur = line.currency;

				if (Debug.AccountLines) {
					Logging.write (cur);
				}


				if (cash.ContainsKey (cur)) {

					double total;

					cash.TryGetValue (cur, out total);

					total += line.getBalanceAsDouble ();

					cash.Remove (cur);

					if (Debug.AccountLines) {
						Logging.write (cur + " = " + total);
					}

					cash.Add (cur, total);
				} else {

					if (Debug.AccountLines) {
						Logging.write (cur + " = " + line.getBalanceAsDouble());
					}

					cash.Add (cur, line.getBalanceAsDouble());
				}

			}

			Dictionary<String,Double>.ValueCollection vc = cash.Values;

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

			if (SendIOU.currentInstance != null) {
				SendAndConvert.currentInstance.setCurrencies (currencies);

			} else {
				// TODO debug
			}
		}


		public static List<TrustLine> lines = null;
		public static Dictionary <String, Double> cash = null;

		public static bool hasIce = false;
		private String license = "ICE";
	}
}

