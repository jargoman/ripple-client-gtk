using System;
using System.Text;
using System.IO;
using System.Threading;
using Gtk;


namespace RippleClientGtk
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (Debug.Program) {
				Logging.write ("Main : begin \n");
			}
		
			Application.Init ();

			if (Debug.Program) {
				Logging.write ("Main : init complete \n");
			}

			MainWindow win = new MainWindow ();  // not visible yet

			if (Debug.Program) {
				Logging.write ("Main : finished creating window \n");
			}

			win.Hide ();
			Thread.Sleep (1230);

			Gtk.Application.Invoke ( delegate {
				if (Debug.Program) {
					Logging.write ("Main : Gtk Invoke : begin \n");
				}

				Wallet wall = win.getWallet ();

				if (wall!=null) {
					if (Debug.Program) {
						Logging.write ("Main : Gtk Invoke : wall!=null \n");
					}
					wall.loadWallet();  // goes into the wallet and finds the path to saved wallet, then loads it, it also blocks for password input
					wall.startup = false;
				}

				else {
					if (Debug.Program) {
						Logging.write ("Main : Gtk Invoke : wall==null \n");
					}
				}

	       		
				win.Show (); // show and run the main program
			});

			Application.Run ();
		}

		/*   // this originally ran the password dialog in it's own thread
		static void ThreadRoutine () {
			Gtk.Application.Invoke (delegate {



			});
		}
		*/





	}  // end class


} // end namespace