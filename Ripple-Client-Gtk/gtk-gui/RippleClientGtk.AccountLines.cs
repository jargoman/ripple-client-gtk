
// This file has been generated by the GUI designer. Do not modify.
namespace RippleClientGtk
{
	public partial class AccountLines
	{
		private global::Gtk.VBox vbox1;
		private global::Gtk.Label label1;
		private global::Gtk.HSeparator hseparator1;
		private global::Gtk.HBox hbox3;
		private global::Gtk.Label label4;
		private global::Gtk.ComboBoxEntry comboboxentry1;
		private global::Gtk.Button syncbutton;
		private global::Gtk.ScrolledWindow scrolledwindow1;
		private global::RippleClientGtk.PagerWidget pagerwidget1;
		
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget RippleClientGtk.AccountLines
			global::Stetic.BinContainer.Attach (this);
			this.Name = "RippleClientGtk.AccountLines";
			// Container child RippleClientGtk.AccountLines.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("<big><b><u>Account Lines</u></b></big>");
			this.label1.UseMarkup = true;
			this.vbox1.Add (this.label1);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.label1]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			w1.Padding = ((uint)(5));
			// Container child vbox1.Gtk.Box+BoxChild
			this.hseparator1 = new global::Gtk.HSeparator ();
			this.hseparator1.Name = "hseparator1";
			this.vbox1.Add (this.hseparator1);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hseparator1]));
			w2.Position = 1;
			w2.Expand = false;
			w2.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox3 = new global::Gtk.HBox ();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			// Container child hbox3.Gtk.Box+BoxChild
			this.label4 = new global::Gtk.Label ();
			this.label4.Name = "label4";
			this.label4.LabelProp = global::Mono.Unix.Catalog.GetString ("View Account : ");
			this.hbox3.Add (this.label4);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.label4]));
			w3.Position = 0;
			w3.Expand = false;
			w3.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.comboboxentry1 = global::Gtk.ComboBoxEntry.NewText ();
			this.comboboxentry1.Name = "comboboxentry1";
			this.hbox3.Add (this.comboboxentry1);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.comboboxentry1]));
			w4.Position = 1;
			// Container child hbox3.Gtk.Box+BoxChild
			this.syncbutton = new global::Gtk.Button ();
			this.syncbutton.CanFocus = true;
			this.syncbutton.Name = "syncbutton";
			this.syncbutton.UseUnderline = true;
			this.syncbutton.Label = global::Mono.Unix.Catalog.GetString ("Refresh");
			this.hbox3.Add (this.syncbutton);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.syncbutton]));
			w5.PackType = ((global::Gtk.PackType)(1));
			w5.Position = 2;
			w5.Expand = false;
			w5.Fill = false;
			this.vbox1.Add (this.hbox3);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox3]));
			w6.Position = 2;
			w6.Expand = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.scrolledwindow1 = new global::Gtk.ScrolledWindow ();
			this.scrolledwindow1.CanFocus = true;
			this.scrolledwindow1.Name = "scrolledwindow1";
			this.scrolledwindow1.VscrollbarPolicy = ((global::Gtk.PolicyType)(0));
			this.scrolledwindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			this.vbox1.Add (this.scrolledwindow1);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.scrolledwindow1]));
			w7.Position = 3;
			// Container child vbox1.Gtk.Box+BoxChild
			this.pagerwidget1 = new global::RippleClientGtk.PagerWidget ();
			this.pagerwidget1.Events = ((global::Gdk.EventMask)(256));
			this.pagerwidget1.Name = "pagerwidget1";
			this.vbox1.Add (this.pagerwidget1);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.pagerwidget1]));
			w8.Position = 4;
			w8.Expand = false;
			w8.Fill = false;
			this.Add (this.vbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
		}
	}
}
