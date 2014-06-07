using System;

namespace RippleClientGtk
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Wallet : Gtk.Bin
	{
		public Wallet ()
		{
			this.Build ();

			button132.Clicked += delegate(object sender, EventArgs e) {

				Gtk.Application.Invoke( delegate {

				if (MainWindow.currentInstance!=null) {
					// todo debug

					MainWindow.currentInstance.Hide();
						MainWindow.currentInstance.setRippleWallet(null);

				}

				if (WalletManagerWindow.currentInstance!=null) {
					// todo debug

					WalletManagerWindow.currentInstance.Show();

				}

				

				

				});
			};
		}


		public void setWallet (RippleWallet rw)
		{
			this.wallet = rw;


			Gtk.Application.Invoke( delegate {

			this.label8.Text = ReceiveWidget.UNSYNCED;
			this.label9.Text = ReceiveWidget.UNSYNCED;
			this.label10.Text = ReceiveWidget.UNSYNCED;

			if (rw == null) {
				return;
			}

			if (rw.walletname!=null) {
				this.label8.Text = rw.walletname;
			}

				this.label9.Text = rw.getStoredReceiveAddress();

			if (checkbutton1.Active) {
				if (rw.seed!=null) {
						this.label10.Text = rw.seed.ToString();
				}
			}

			else {
				if (rw.seed!=null) {
						this.label10.Text = rw.seed.ToHiddenString();
				}
			}
			});



		}

		public RippleWallet getWallet () {
			return this.wallet;
		}

		private RippleWallet wallet = null;
	}
}

