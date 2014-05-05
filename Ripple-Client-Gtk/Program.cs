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

			SplashWindow.loadSplash(); // loads splash config // must be first

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

			//Mono.CSharp.Evaluator.

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

			while (!SplashWindow.loaded) {
				//Logging.write("sleeping...");
				Thread.Sleep(10);
			}

			if (SplashWindow.delay == null) {
				SplashWindow.delay = SplashWindow.default_delay;
			}

			if (SplashWindow.delay!=null) {
				Thread.Sleep((Int32)SplashWindow.delay);
			}

			PluginController.initPlugins ();

			Thread.Sleep (5);

			//PluginController.

			//Thread.Sleep(5);


			NetworkSettings.readSettings();

			if (Debug.testVectors) {
				RippleDeterministicKeyGenerator.testVectors ();
			} else {
				Thread.Sleep (5);
			}




			Wallet.loadWallet (Wallet.walletpath); // NOT DECRYPTED

			Thread.Sleep(3);

			if (PluginController.currentInstance!=null) {
				PluginController.currentInstance.preStartUp();
			}

			Gtk.Application.Invoke (delegate {

				Logging.write ("Thread priority = " + Thread.CurrentThread.Priority);
				win = new MainWindow ();  // not visible yet
				win.Hide ();

				if (Debug.Program) {
					Logging.write ("Main : finished creating window \n");
				}

			}
			);



			//Thread.Sleep(2);

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

			Thread.Sleep(10);

			if (PluginController.currentInstance!=null) {
				PluginController.currentInstance.postStartUp();
			}
				

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
