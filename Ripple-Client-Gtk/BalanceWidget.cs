using System;
using System.Collections.Generic;
using Gtk;

namespace RippleClientGtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class BalanceWidget : Gtk.Bin
	{
		public BalanceWidget ()
		{
			this.Build ();

			currentInstance = this;


			//scrolledWindow1.ToString();
		}

		public static BalanceWidget currentInstance;

		public Table table = null;
		public void setTable (List<CurrencyWidget> li)
		{
			if (li == null) {
				table.Destroy();
				return;
			}

			if (Debug.BalanceWidget) {
				Logging.write ("BalanceWidget : setTable Fired\n" + li.ToString() + "\n");
			}

			CurrencyWidget[] widgets = li.ToArray();

			if (table!=null) {
				table.Destroy();
			}

			uint len = 0;

			try {
				len = checked ((uint)widgets.Length);
			}

			catch (System.OverflowException e) {
				Logging.write ("Exception thrown in Class BalanceWidget, widgets.Lenth is not a valid unsigned int. " + e.Message);
				
				return;
			}

			if (len == 0) {
				if (Debug.BalanceWidget) {
					Logging.write ("BalanceWidget.setTable : No widgets to display\n");
				}
				return;
			}

			if (Debug.BalanceWidget) {
				Logging.write ("BalanceWidget : Creating Table");
			}

			table = new Table (
				len,
				1,
				true
				);

			for (uint y = 0; y < len; y++) {

				table.Attach(widgets[y],0,1,y,y+1);
				widgets[y].Show();

			}


			this.scrolledwindow1.AddWithViewport(table);
			table.Show();
			MainWindow.currentInstance.ShowAll();
		}

	}
}

