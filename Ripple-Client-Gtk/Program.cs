/*
 *	License : Le Ice Sense 
 */

using System;
using System.Text;
using System.IO;
using System.Threading;
using Gtk;


namespace RippleClientGtk
{
	class MainClass
	{
		static Thread thr = new Thread( new ThreadStart(ThreadRoutine));

		static ThreadNotify notify;

		public static SplashWindow splash = null;
		public static MainWindow win = null;

		public static void Main (string[] args)
		{

			Application.Init ();

			//Thread.CurrentThread.Priority = ThreadPriority.Highest;
			//Thread.Yield()
			//if (Debug.Program) {
			//Logging.write ("Thread priority = " + Thread.CurrentThread.Priority);
			//}

			SplashWindow.loadIsSplash(); // loads splash config // must be first

			Gtk.Application.Invoke (
				delegate {
					thr.Start();
				}
			);

			//thr.Priority = ThreadPriority.Lowest;
			//Logging.write("thr" + thr.Priority.ToString());

			// 
			//

			//notify = new ThreadNotify (new ReadyEvent(ready));

			Application.Run ();
		}


		static void ThreadRoutine ()
		{  
			/* 
			 * This function is massively multithreaded. 
			 * It's the loadup process while the splash screen is displayed.
			 * Things are staggared the way they are to use multi-threading
			 * and to take advantage of more than one processor. 
			 * the threads are staggared in placement
			 * 
			 * non-gtk-thread
			 * gtk-thread
			 * non-gtk
			 * gtk
			 * non
			 * gtk
			 * non
			 * ect...
			 */


			//Thread.Sleep(2);
			Gtk.Application.Invoke (
				delegate {
					if (SplashWindow.isSplash) {
						splash = new SplashWindow ();
						//splash.ShowAll();
						//splash.
						//splash.QueueDraw();
						//splash.Realize();
					}
				}
			);


			Thread.Sleep(5000);
			Gtk.Application.Invoke (
				delegate {
					if (SplashWindow.isSplash) {
						//splash = new SplashWindow ();
						splash.ShowAll();
						//splash.
						//splash.QueueDraw();
						//splash.Realize();
					}
				}
			);
			Thread.Sleep(100);

			//while (Gtk.Application.EventsPending()) {
			//	Gtk.Application.RunIteration();
			//}


			/* instead of wasting time sleeping 
			 * we'll put loading tasks here to let the splash screen load 
			 * before opening the behemoth of an invoke. 
			 */

			//


			//Thread.Sleep (100);
			//RandomSeedGenerator.startupSeed();


			if (Debug.testVectors) {
				RippleDeterministicKeyGenerator.testVectors ();
			} else {
				Thread.Sleep (5);
			}


			Thread.Sleep(10);

			NetworkSettings.readSettings();


			Thread.Sleep(3);

			Wallet.loadWallet (Wallet.walletpath); // NOT DECRYPTED

			Thread.Sleep(3);


			Gtk.Application.Invoke (delegate {

				Logging.write ("Thread priority = " + Thread.CurrentThread.Priority);
				win = new MainWindow ();  // not visible yet
				win.Hide ();

				if (Debug.Program) {
					Logging.write ("Main : finished creating window \n");
				}

			}
			);

			Thread.Sleep(2);

			Console.loadHistory ();

			//Thread.Sleep (1);


			// make sure window is loaded
			while (win==null) {  
				Thread.Sleep(5);
			}

			Gtk.Application.Invoke( delegate {
				NetworkSettings ns = NetworkSettings.currentInstance;
				if (ns!=null) {
					if (Debug.Program) {
						Logging.write("Main : Gtk Invoke : ns!=null \n");
					}

					ns.loadSettings();
				}

				else {
					if (Debug.Program) {
						Logging.write("Main : Gtk Invoke : ns==null");
					}
				}
			});

			Thread.Sleep(3);

			RandomSeedGenerator.startupSeed ();

			Thread.Sleep(3);

			Gtk.Application.Invoke (delegate {


				Wallet wall = MainWindow.currentInstance.getWallet ();

				if (wall!=null) {
					if (Debug.Program) {
						Logging.write ("Main : Gtk Invoke : wall!=null \n");
					}
					wall.decryptWallet();
					wall.startup = false;
				}

				else {
					if (Debug.Program) {
						Logging.write ("Main : Gtk Invoke : wall==null \n");
					}
				}

				//win.Show (); // show and run the main program
			});





			Thread.Sleep(3);

			BalanceTabOptionsWidget.loadBalanceConfig();

			Thread.Sleep(3);


			// uses gtk invoke
			BalanceTab.currentInstance.set (BalanceTabOptionsWidget.actual_values);

			Thread.Sleep(3);

			TransactionType.loadTransactionTypes();

			Thread.Sleep(5);


			Gtk.Application.Invoke( delegate {
				win.Show();
				killSplash();
			}
			);


				

		}



		public static void killSplash() {

			if (splash!=null) {
				splash.Hide();
				splash.Destroy();
				splash = null;
			}

		}


	}  // end class


} // end namespace
