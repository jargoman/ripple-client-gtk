
// This file has been generated by the GUI designer. Do not modify.
namespace RippleClientGtk
{
	public partial class Wallet
	{
		private global::Gtk.VBox vbox1;
		private global::Gtk.Label label11;
		private global::Gtk.HSeparator hseparator3;
		private global::Gtk.Table table1;
		private global::Gtk.HBox hbox1;
		private global::Gtk.Entry secretentry;
		private global::Gtk.Button button1;
		private global::Gtk.CheckButton showsecretcheckbox;
		private global::Gtk.HBox hbox8;
		private global::Gtk.Button useButton;
		private global::Gtk.Button rememberbutton;
		private global::Gtk.Button forgetbutton;
		private global::Gtk.CheckButton encryptCheckBox;
		private global::Gtk.Label label16;
		private global::Gtk.Label label17;
		private global::Gtk.Label label21;
		private global::Gtk.Entry receiveAddress;
		private global::Gtk.HBox hbox4;
		private global::Gtk.Label label19;
		private global::Gtk.Button button2;
		private global::Gtk.Label label20;
		private global::Gtk.Button button3;
		private global::Gtk.HSeparator hseparator1;
		
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget RippleClientGtk.Wallet
			global::Stetic.BinContainer.Attach (this);
			this.Name = "RippleClientGtk.Wallet";
			// Container child RippleClientGtk.Wallet.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			this.vbox1.BorderWidth = ((uint)(5));
			// Container child vbox1.Gtk.Box+BoxChild
			this.label11 = new global::Gtk.Label ();
			this.label11.Name = "label11";
			this.label11.LabelProp = global::Mono.Unix.Catalog.GetString ("<big><b><u>Wallet</u></b></big>");
			this.label11.UseMarkup = true;
			this.vbox1.Add (this.label11);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.label11]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hseparator3 = new global::Gtk.HSeparator ();
			this.hseparator3.Name = "hseparator3";
			this.vbox1.Add (this.hseparator3);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hseparator3]));
			w2.Position = 1;
			w2.Expand = false;
			w2.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table (((uint)(3)), ((uint)(2)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.secretentry = new global::Gtk.Entry ();
			this.secretentry.WidthRequest = 354;
			this.secretentry.CanFocus = true;
			this.secretentry.Name = "secretentry";
			this.secretentry.IsEditable = true;
			this.secretentry.Visibility = false;
			this.secretentry.InvisibleChar = '●';
			this.hbox1.Add (this.secretentry);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.secretentry]));
			w3.Position = 0;
			// Container child hbox1.Gtk.Box+BoxChild
			this.button1 = new global::Gtk.Button ();
			this.button1.CanFocus = true;
			this.button1.Name = "button1";
			this.button1.UseUnderline = true;
			this.button1.Label = global::Mono.Unix.Catalog.GetString ("Generate");
			this.hbox1.Add (this.button1);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.button1]));
			w4.PackType = ((global::Gtk.PackType)(1));
			w4.Position = 1;
			w4.Expand = false;
			w4.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.showsecretcheckbox = new global::Gtk.CheckButton ();
			this.showsecretcheckbox.WidthRequest = 0;
			this.showsecretcheckbox.CanFocus = true;
			this.showsecretcheckbox.Name = "showsecretcheckbox";
			this.showsecretcheckbox.Label = global::Mono.Unix.Catalog.GetString ("show");
			this.showsecretcheckbox.DrawIndicator = true;
			this.showsecretcheckbox.UseUnderline = true;
			this.hbox1.Add (this.showsecretcheckbox);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.showsecretcheckbox]));
			w5.PackType = ((global::Gtk.PackType)(1));
			w5.Position = 2;
			this.table1.Add (this.hbox1);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.table1 [this.hbox1]));
			w6.TopAttach = ((uint)(1));
			w6.BottomAttach = ((uint)(2));
			w6.LeftAttach = ((uint)(1));
			w6.RightAttach = ((uint)(2));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.hbox8 = new global::Gtk.HBox ();
			this.hbox8.Name = "hbox8";
			this.hbox8.Spacing = 6;
			// Container child hbox8.Gtk.Box+BoxChild
			this.useButton = new global::Gtk.Button ();
			this.useButton.CanFocus = true;
			this.useButton.Name = "useButton";
			this.useButton.UseUnderline = true;
			this.useButton.Label = global::Mono.Unix.Catalog.GetString ("_Use");
			this.hbox8.Add (this.useButton);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox8 [this.useButton]));
			w7.Position = 0;
			// Container child hbox8.Gtk.Box+BoxChild
			this.rememberbutton = new global::Gtk.Button ();
			this.rememberbutton.CanFocus = true;
			this.rememberbutton.Name = "rememberbutton";
			this.rememberbutton.UseUnderline = true;
			this.rememberbutton.Label = global::Mono.Unix.Catalog.GetString ("_Remember");
			this.hbox8.Add (this.rememberbutton);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox8 [this.rememberbutton]));
			w8.Position = 1;
			// Container child hbox8.Gtk.Box+BoxChild
			this.forgetbutton = new global::Gtk.Button ();
			this.forgetbutton.CanFocus = true;
			this.forgetbutton.Name = "forgetbutton";
			this.forgetbutton.UseUnderline = true;
			this.forgetbutton.Label = global::Mono.Unix.Catalog.GetString ("_Forget");
			this.hbox8.Add (this.forgetbutton);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hbox8 [this.forgetbutton]));
			w9.Position = 2;
			// Container child hbox8.Gtk.Box+BoxChild
			this.encryptCheckBox = new global::Gtk.CheckButton ();
			this.encryptCheckBox.WidthRequest = 115;
			this.encryptCheckBox.CanFocus = true;
			this.encryptCheckBox.Name = "encryptCheckBox";
			this.encryptCheckBox.Label = global::Mono.Unix.Catalog.GetString ("Encrypt Wallet");
			this.encryptCheckBox.DrawIndicator = true;
			this.encryptCheckBox.UseUnderline = true;
			this.hbox8.Add (this.encryptCheckBox);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.hbox8 [this.encryptCheckBox]));
			w10.PackType = ((global::Gtk.PackType)(1));
			w10.Position = 3;
			w10.Expand = false;
			this.table1.Add (this.hbox8);
			global::Gtk.Table.TableChild w11 = ((global::Gtk.Table.TableChild)(this.table1 [this.hbox8]));
			w11.TopAttach = ((uint)(2));
			w11.BottomAttach = ((uint)(3));
			w11.LeftAttach = ((uint)(1));
			w11.RightAttach = ((uint)(2));
			w11.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label16 = new global::Gtk.Label ();
			this.label16.Name = "label16";
			this.label16.Xalign = 0F;
			this.label16.LabelProp = global::Mono.Unix.Catalog.GetString ("<b>Receive Address</b>");
			this.label16.UseMarkup = true;
			this.table1.Add (this.label16);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.table1 [this.label16]));
			w12.XOptions = ((global::Gtk.AttachOptions)(4));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label17 = new global::Gtk.Label ();
			this.label17.Name = "label17";
			this.label17.Xalign = 0F;
			this.label17.LabelProp = global::Mono.Unix.Catalog.GetString ("<b>Secret</b>");
			this.label17.UseMarkup = true;
			this.table1.Add (this.label17);
			global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.table1 [this.label17]));
			w13.TopAttach = ((uint)(1));
			w13.BottomAttach = ((uint)(2));
			w13.XOptions = ((global::Gtk.AttachOptions)(4));
			w13.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label21 = new global::Gtk.Label ();
			this.label21.Name = "label21";
			this.label21.Xalign = 0F;
			this.label21.LabelProp = global::Mono.Unix.Catalog.GetString ("<b>Use Keypair</b>");
			this.label21.UseMarkup = true;
			this.table1.Add (this.label21);
			global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.table1 [this.label21]));
			w14.TopAttach = ((uint)(2));
			w14.BottomAttach = ((uint)(3));
			w14.XOptions = ((global::Gtk.AttachOptions)(4));
			w14.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.receiveAddress = new global::Gtk.Entry ();
			this.receiveAddress.CanFocus = true;
			this.receiveAddress.Name = "receiveAddress";
			this.receiveAddress.IsEditable = true;
			this.receiveAddress.InvisibleChar = '●';
			this.table1.Add (this.receiveAddress);
			global::Gtk.Table.TableChild w15 = ((global::Gtk.Table.TableChild)(this.table1 [this.receiveAddress]));
			w15.LeftAttach = ((uint)(1));
			w15.RightAttach = ((uint)(2));
			w15.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox1.Add (this.table1);
			global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.table1]));
			w16.Position = 2;
			w16.Expand = false;
			w16.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox4 = new global::Gtk.HBox ();
			this.hbox4.Name = "hbox4";
			this.hbox4.Homogeneous = true;
			this.hbox4.Spacing = 6;
			// Container child hbox4.Gtk.Box+BoxChild
			this.label19 = new global::Gtk.Label ();
			this.label19.Name = "label19";
			this.label19.Xalign = 0F;
			this.label19.LabelProp = global::Mono.Unix.Catalog.GetString ("<b>Restore Wallet</b>");
			this.label19.UseMarkup = true;
			this.hbox4.Add (this.label19);
			global::Gtk.Box.BoxChild w17 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.label19]));
			w17.Position = 0;
			w17.Expand = false;
			w17.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.button2 = new global::Gtk.Button ();
			this.button2.CanFocus = true;
			this.button2.Name = "button2";
			this.button2.UseUnderline = true;
			this.button2.Label = global::Mono.Unix.Catalog.GetString ("_Import");
			this.hbox4.Add (this.button2);
			global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.button2]));
			w18.Position = 1;
			w18.Expand = false;
			w18.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.label20 = new global::Gtk.Label ();
			this.label20.Name = "label20";
			this.label20.Xalign = 0F;
			this.label20.LabelProp = global::Mono.Unix.Catalog.GetString ("<b>Backup Wallet</b>");
			this.label20.UseMarkup = true;
			this.hbox4.Add (this.label20);
			global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.label20]));
			w19.Position = 2;
			w19.Expand = false;
			w19.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.button3 = new global::Gtk.Button ();
			this.button3.CanFocus = true;
			this.button3.Name = "button3";
			this.button3.UseUnderline = true;
			this.button3.Label = global::Mono.Unix.Catalog.GetString ("_export");
			this.hbox4.Add (this.button3);
			global::Gtk.Box.BoxChild w20 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.button3]));
			w20.Position = 3;
			w20.Expand = false;
			w20.Fill = false;
			this.vbox1.Add (this.hbox4);
			global::Gtk.Box.BoxChild w21 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox4]));
			w21.PackType = ((global::Gtk.PackType)(1));
			w21.Position = 3;
			w21.Expand = false;
			w21.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hseparator1 = new global::Gtk.HSeparator ();
			this.hseparator1.Name = "hseparator1";
			this.vbox1.Add (this.hseparator1);
			global::Gtk.Box.BoxChild w22 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hseparator1]));
			w22.PackType = ((global::Gtk.PackType)(1));
			w22.Position = 4;
			w22.Expand = false;
			w22.Fill = false;
			this.Add (this.vbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.receiveAddress.Activated += new global::System.EventHandler (this.OnReceiveAddressActivated);
			this.useButton.Clicked += new global::System.EventHandler (this.OnUseButtonClicked);
			this.rememberbutton.Clicked += new global::System.EventHandler (this.OnRememberbuttonClicked);
			this.forgetbutton.Clicked += new global::System.EventHandler (this.OnForgetbuttonClicked);
			this.secretentry.Activated += new global::System.EventHandler (this.OnSecretentryActivated);
			this.showsecretcheckbox.Toggled += new global::System.EventHandler (this.OnShowsecretcheckboxToggled);
			this.button2.Clicked += new global::System.EventHandler (this.OnButton2Clicked);
			this.button3.Clicked += new global::System.EventHandler (this.OnButton3Clicked);
		}
	}
}