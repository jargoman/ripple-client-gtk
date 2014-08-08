
// This file has been generated by the GUI designer. Do not modify.
namespace RippleClientGtk
{
	public partial class PasswordCreateDialog
	{
		private global::Gtk.VBox vbox3;
		private global::Gtk.Label label1;
		private global::Gtk.HSeparator hseparator3;
		private global::Gtk.TextView textview1;
		private global::Gtk.HSeparator hseparator4;
		private global::Gtk.Table table1;
		private global::Gtk.Label label17;
		private global::Gtk.Label label18;
		private global::Gtk.Entry secretentry;
		private global::Gtk.Entry secretentry1;
		private global::Gtk.Button buttonCancel;
		private global::Gtk.Button buttonOk;
		
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget RippleClientGtk.PasswordCreateDialog
			this.Name = "RippleClientGtk.PasswordCreateDialog";
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child RippleClientGtk.PasswordCreateDialog.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.label1 = new global::Gtk.Label ();
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("<big><u><b>Password Create</b></u></big>");
			this.label1.UseMarkup = true;
			this.vbox3.Add (this.label1);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.label1]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			w2.Padding = ((uint)(5));
			// Container child vbox3.Gtk.Box+BoxChild
			this.hseparator3 = new global::Gtk.HSeparator ();
			this.hseparator3.Name = "hseparator3";
			this.vbox3.Add (this.hseparator3);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.hseparator3]));
			w3.Position = 1;
			w3.Expand = false;
			w3.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.textview1 = new global::Gtk.TextView ();
			this.textview1.Name = "textview1";
			this.textview1.Editable = false;
			this.textview1.CursorVisible = false;
			this.textview1.AcceptsTab = false;
			this.vbox3.Add (this.textview1);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.textview1]));
			w4.Position = 2;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hseparator4 = new global::Gtk.HSeparator ();
			this.hseparator4.Name = "hseparator4";
			this.vbox3.Add (this.hseparator4);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.hseparator4]));
			w5.Position = 3;
			w5.Expand = false;
			w5.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table (((uint)(2)), ((uint)(2)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.label17 = new global::Gtk.Label ();
			this.label17.Name = "label17";
			this.label17.Xalign = 0F;
			this.label17.LabelProp = global::Mono.Unix.Catalog.GetString ("<b>Create a password</b>");
			this.label17.UseMarkup = true;
			this.table1.Add (this.label17);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.table1 [this.label17]));
			w6.XOptions = ((global::Gtk.AttachOptions)(4));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label18 = new global::Gtk.Label ();
			this.label18.Name = "label18";
			this.label18.Xalign = 0F;
			this.label18.LabelProp = global::Mono.Unix.Catalog.GetString ("<b>Confirm Password</b>");
			this.label18.UseMarkup = true;
			this.table1.Add (this.label18);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.table1 [this.label18]));
			w7.TopAttach = ((uint)(1));
			w7.BottomAttach = ((uint)(2));
			w7.XOptions = ((global::Gtk.AttachOptions)(4));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.secretentry = new global::Gtk.Entry ();
			this.secretentry.CanDefault = true;
			this.secretentry.CanFocus = true;
			this.secretentry.Name = "secretentry";
			this.secretentry.IsEditable = true;
			this.secretentry.Visibility = false;
			this.secretentry.InvisibleChar = '●';
			this.table1.Add (this.secretentry);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.table1 [this.secretentry]));
			w8.LeftAttach = ((uint)(1));
			w8.RightAttach = ((uint)(2));
			w8.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.secretentry1 = new global::Gtk.Entry ();
			this.secretentry1.CanFocus = true;
			this.secretentry1.Name = "secretentry1";
			this.secretentry1.IsEditable = true;
			this.secretentry1.Visibility = false;
			this.secretentry1.InvisibleChar = '●';
			this.table1.Add (this.secretentry1);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.table1 [this.secretentry1]));
			w9.TopAttach = ((uint)(1));
			w9.BottomAttach = ((uint)(2));
			w9.LeftAttach = ((uint)(1));
			w9.RightAttach = ((uint)(2));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox3.Add (this.table1);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.table1]));
			w10.Position = 4;
			w10.Expand = false;
			w10.Fill = false;
			w1.Add (this.vbox3);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(w1 [this.vbox3]));
			w11.Position = 0;
			w11.Padding = ((uint)(5));
			// Internal child RippleClientGtk.PasswordCreateDialog.ActionArea
			global::Gtk.HButtonBox w12 = this.ActionArea;
			w12.Name = "dialog1_ActionArea";
			w12.Spacing = 10;
			w12.BorderWidth = ((uint)(5));
			w12.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button ();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			this.AddActionWidget (this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w13 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w12 [this.buttonCancel]));
			w13.Expand = false;
			w13.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			this.AddActionWidget (this.buttonOk, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w14 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w12 [this.buttonOk]));
			w14.Position = 1;
			w14.Expand = false;
			w14.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 400;
			this.DefaultHeight = 300;
			this.secretentry.HasDefault = true;
			this.Show ();
			this.secretentry1.Activated += new global::System.EventHandler (this.OnSecretentry1Activated);
			this.secretentry.Activated += new global::System.EventHandler (this.OnSecretentryActivated);
		}
	}
}
