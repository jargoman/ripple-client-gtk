/*
 *	License : Le Ice Sense 
 */

using Gtk;
using System;
using System.IO;

namespace RippleClientGtk
{
	public class Logging
	{
		public Logging ()
		{
		}

		public static bool WRITE_TO_CONSOLE = true;

		public static void write (String message) 
		{

			if (message == null) {
				return;
			}

			if (textview!=null) {
				Gtk.Application.Invoke ( delegate 
				    {
						TextBuffer buff = textview.Buffer;
						buff.Text += message;
						TextIter it = buff.EndIter;
						textview.ScrollToIter(it,0,false,0,0);

					}
				);
			}


			if (WRITE_TO_CONSOLE) {
				System.Console.WriteLine(message); // namespace is needed because I named a class Console like an idiot
			}
		}

		public static Gtk.TextView textview;
	}
}

