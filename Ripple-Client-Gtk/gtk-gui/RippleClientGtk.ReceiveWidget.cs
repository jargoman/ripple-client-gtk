
// This file has been generated by the GUI designer. Do not modify.
namespace RippleClientGtk
{
	public partial class ReceiveWidget
	{
		private global::Gtk.HBox hbox6;
		private global::Gtk.Button button8;
		private global::Gtk.Label receiveLabel;
		private global::Gtk.Button syncbutton;
		
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget RippleClientGtk.ReceiveWidget
			global::Stetic.BinContainer.Attach (this);
			this.Name = "RippleClientGtk.ReceiveWidget";
			// Container child RippleClientGtk.ReceiveWidget.Gtk.Container+ContainerChild
			this.hbox6 = new global::Gtk.HBox ();
			this.hbox6.Name = "hbox6";
			this.hbox6.Spacing = 6;
			// Container child hbox6.Gtk.Box+BoxChild
			this.button8 = new global::Gtk.Button ();
			this.button8.CanFocus = true;
			this.button8.Name = "button8";
			this.button8.UseUnderline = true;
			this.button8.Label = global::Mono.Unix.Catalog.GetString ("_Receive Address --->");
			this.hbox6.Add (this.button8);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.hbox6 [this.button8]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child hbox6.Gtk.Box+BoxChild
			this.receiveLabel = new global::Gtk.Label ();
			this.receiveLabel.WidthRequest = 315;
			this.receiveLabel.Name = "receiveLabel";
			this.receiveLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("-- unsynced --");
			this.receiveLabel.Selectable = true;
			this.hbox6.Add (this.receiveLabel);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox6 [this.receiveLabel]));
			w2.Position = 1;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox6.Gtk.Box+BoxChild
			this.syncbutton = new global::Gtk.Button ();
			this.syncbutton.CanFocus = true;
			this.syncbutton.Name = "syncbutton";
			this.syncbutton.UseUnderline = true;
			this.syncbutton.Label = global::Mono.Unix.Catalog.GetString ("Sync");
			this.hbox6.Add (this.syncbutton);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox6 [this.syncbutton]));
			w3.PackType = ((global::Gtk.PackType)(1));
			w3.Position = 2;
			w3.Expand = false;
			this.Add (this.hbox6);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.button8.Clicked += new global::System.EventHandler (this.OnButton8Clicked);
		}
	}
}
