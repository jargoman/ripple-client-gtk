
// This file has been generated by the GUI designer. Do not modify.
namespace RippleClientGtk
{
	public partial class AreYouSure
	{
		private global::Gtk.VBox vbox3;
		private global::Gtk.Label label11;
		private global::Gtk.HSeparator hseparator3;
		private global::Gtk.TextView textview2;
		private global::Gtk.Button buttonCancel;
		private global::Gtk.Button buttonOk;
		
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget RippleClientGtk.AreYouSure
			this.Name = "RippleClientGtk.AreYouSure";
			this.Title = global::Mono.Unix.Catalog.GetString ("Are you sure?");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child RippleClientGtk.AreYouSure.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.label11 = new global::Gtk.Label ();
			this.label11.Name = "label11";
			this.label11.LabelProp = global::Mono.Unix.Catalog.GetString ("<big><b><u>Are you sure?</u></b></big>");
			this.label11.UseMarkup = true;
			this.vbox3.Add (this.label11);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.label11]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hseparator3 = new global::Gtk.HSeparator ();
			this.hseparator3.Name = "hseparator3";
			this.vbox3.Add (this.hseparator3);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.hseparator3]));
			w3.Position = 1;
			w3.Expand = false;
			w3.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.textview2 = new global::Gtk.TextView ();
			this.textview2.CanFocus = true;
			this.textview2.Name = "textview2";
			this.textview2.Editable = false;
			this.textview2.CursorVisible = false;
			this.textview2.AcceptsTab = false;
			this.textview2.WrapMode = ((global::Gtk.WrapMode)(2));
			this.vbox3.Add (this.textview2);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.textview2]));
			w4.Position = 2;
			w1.Add (this.vbox3);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(w1 [this.vbox3]));
			w5.Position = 0;
			w5.Padding = ((uint)(5));
			// Internal child RippleClientGtk.AreYouSure.ActionArea
			global::Gtk.HButtonBox w6 = this.ActionArea;
			w6.Name = "dialog1_ActionArea";
			w6.Spacing = 10;
			w6.BorderWidth = ((uint)(5));
			w6.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button ();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			this.AddActionWidget (this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w7 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w6 [this.buttonCancel]));
			w7.Expand = false;
			w7.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			this.AddActionWidget (this.buttonOk, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w8 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w6 [this.buttonOk]));
			w8.Position = 1;
			w8.Expand = false;
			w8.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 400;
			this.DefaultHeight = 300;
			this.buttonOk.HasDefault = true;
			this.Show ();
		}
	}
}
