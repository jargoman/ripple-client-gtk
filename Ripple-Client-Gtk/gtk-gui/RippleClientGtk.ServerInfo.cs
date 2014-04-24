
// This file has been generated by the GUI designer. Do not modify.
namespace RippleClientGtk
{
	public partial class ServerInfo
	{
		private global::Gtk.VBox vbox1;
		private global::Gtk.Label label26;
		private global::Gtk.HSeparator hseparator1;
		private global::Gtk.Table table1;
		private global::Gtk.Label base_fee_label;
		private global::Gtk.Label base_fee_label_var;
		private global::Gtk.Label build_version_label;
		private global::Gtk.Label build_version_label_var;
		private global::Gtk.Label complete_ledgers_label_var;
		private global::Gtk.Label host_id_label;
		private global::Gtk.Label host_id_label_var;
		private global::Gtk.Label label3;
		private global::Gtk.Label load_factor_label;
		private global::Gtk.Label load_factor_label_var;
		private global::Gtk.Label transaction_fee_label;
		private global::Gtk.Label transaction_fee_label_var;
		
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget RippleClientGtk.ServerInfo
			global::Stetic.BinContainer.Attach (this);
			this.Name = "RippleClientGtk.ServerInfo";
			// Container child RippleClientGtk.ServerInfo.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.label26 = new global::Gtk.Label ();
			this.label26.Name = "label26";
			this.label26.LabelProp = global::Mono.Unix.Catalog.GetString ("<big><b><u>Server Info</u></b></big>");
			this.label26.UseMarkup = true;
			this.label26.Justify = ((global::Gtk.Justification)(2));
			this.vbox1.Add (this.label26);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.label26]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hseparator1 = new global::Gtk.HSeparator ();
			this.hseparator1.Name = "hseparator1";
			this.vbox1.Add (this.hseparator1);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hseparator1]));
			w2.Position = 1;
			w2.Expand = false;
			w2.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table (((uint)(6)), ((uint)(2)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.base_fee_label = new global::Gtk.Label ();
			this.base_fee_label.Name = "base_fee_label";
			this.base_fee_label.Xalign = 0F;
			this.base_fee_label.LabelProp = global::Mono.Unix.Catalog.GetString ("Base Fee Drops");
			this.table1.Add (this.base_fee_label);
			global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.table1 [this.base_fee_label]));
			w3.TopAttach = ((uint)(4));
			w3.BottomAttach = ((uint)(5));
			w3.XOptions = ((global::Gtk.AttachOptions)(4));
			w3.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.base_fee_label_var = new global::Gtk.Label ();
			this.base_fee_label_var.Name = "base_fee_label_var";
			this.table1.Add (this.base_fee_label_var);
			global::Gtk.Table.TableChild w4 = ((global::Gtk.Table.TableChild)(this.table1 [this.base_fee_label_var]));
			w4.TopAttach = ((uint)(4));
			w4.BottomAttach = ((uint)(5));
			w4.LeftAttach = ((uint)(1));
			w4.RightAttach = ((uint)(2));
			w4.XOptions = ((global::Gtk.AttachOptions)(4));
			w4.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.build_version_label = new global::Gtk.Label ();
			this.build_version_label.Name = "build_version_label";
			this.build_version_label.Xalign = 0F;
			this.build_version_label.LabelProp = global::Mono.Unix.Catalog.GetString ("Build Version ");
			this.table1.Add (this.build_version_label);
			global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.table1 [this.build_version_label]));
			w5.XOptions = ((global::Gtk.AttachOptions)(4));
			w5.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.build_version_label_var = new global::Gtk.Label ();
			this.build_version_label_var.Name = "build_version_label_var";
			this.table1.Add (this.build_version_label_var);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.table1 [this.build_version_label_var]));
			w6.LeftAttach = ((uint)(1));
			w6.RightAttach = ((uint)(2));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.complete_ledgers_label_var = new global::Gtk.Label ();
			this.complete_ledgers_label_var.Name = "complete_ledgers_label_var";
			this.table1.Add (this.complete_ledgers_label_var);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.table1 [this.complete_ledgers_label_var]));
			w7.TopAttach = ((uint)(2));
			w7.BottomAttach = ((uint)(3));
			w7.LeftAttach = ((uint)(1));
			w7.RightAttach = ((uint)(2));
			w7.XOptions = ((global::Gtk.AttachOptions)(4));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.host_id_label = new global::Gtk.Label ();
			this.host_id_label.Name = "host_id_label";
			this.host_id_label.Xalign = 0F;
			this.host_id_label.LabelProp = global::Mono.Unix.Catalog.GetString ("Host I.D");
			this.table1.Add (this.host_id_label);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.table1 [this.host_id_label]));
			w8.TopAttach = ((uint)(1));
			w8.BottomAttach = ((uint)(2));
			w8.XOptions = ((global::Gtk.AttachOptions)(4));
			w8.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.host_id_label_var = new global::Gtk.Label ();
			this.host_id_label_var.Name = "host_id_label_var";
			this.table1.Add (this.host_id_label_var);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.table1 [this.host_id_label_var]));
			w9.TopAttach = ((uint)(1));
			w9.BottomAttach = ((uint)(2));
			w9.LeftAttach = ((uint)(1));
			w9.RightAttach = ((uint)(2));
			w9.XOptions = ((global::Gtk.AttachOptions)(4));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label3 = new global::Gtk.Label ();
			this.label3.Name = "label3";
			this.label3.Xalign = 0F;
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("Complete Ledgers");
			this.table1.Add (this.label3);
			global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.table1 [this.label3]));
			w10.TopAttach = ((uint)(2));
			w10.BottomAttach = ((uint)(3));
			w10.XOptions = ((global::Gtk.AttachOptions)(4));
			w10.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.load_factor_label = new global::Gtk.Label ();
			this.load_factor_label.Name = "load_factor_label";
			this.load_factor_label.Xalign = 0F;
			this.load_factor_label.LabelProp = global::Mono.Unix.Catalog.GetString ("Load Factor");
			this.table1.Add (this.load_factor_label);
			global::Gtk.Table.TableChild w11 = ((global::Gtk.Table.TableChild)(this.table1 [this.load_factor_label]));
			w11.TopAttach = ((uint)(3));
			w11.BottomAttach = ((uint)(4));
			w11.XOptions = ((global::Gtk.AttachOptions)(4));
			w11.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.load_factor_label_var = new global::Gtk.Label ();
			this.load_factor_label_var.Name = "load_factor_label_var";
			this.table1.Add (this.load_factor_label_var);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.table1 [this.load_factor_label_var]));
			w12.TopAttach = ((uint)(3));
			w12.BottomAttach = ((uint)(4));
			w12.LeftAttach = ((uint)(1));
			w12.RightAttach = ((uint)(2));
			w12.XOptions = ((global::Gtk.AttachOptions)(4));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.transaction_fee_label = new global::Gtk.Label ();
			this.transaction_fee_label.Name = "transaction_fee_label";
			this.transaction_fee_label.Xalign = 0F;
			this.transaction_fee_label.LabelProp = global::Mono.Unix.Catalog.GetString ("Transaction Fee");
			this.table1.Add (this.transaction_fee_label);
			global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.table1 [this.transaction_fee_label]));
			w13.TopAttach = ((uint)(5));
			w13.BottomAttach = ((uint)(6));
			w13.XOptions = ((global::Gtk.AttachOptions)(4));
			w13.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.transaction_fee_label_var = new global::Gtk.Label ();
			this.transaction_fee_label_var.Name = "transaction_fee_label_var";
			this.table1.Add (this.transaction_fee_label_var);
			global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.table1 [this.transaction_fee_label_var]));
			w14.TopAttach = ((uint)(5));
			w14.BottomAttach = ((uint)(6));
			w14.LeftAttach = ((uint)(1));
			w14.RightAttach = ((uint)(2));
			w14.XOptions = ((global::Gtk.AttachOptions)(4));
			w14.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox1.Add (this.table1);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.table1]));
			w15.Position = 2;
			w15.Expand = false;
			w15.Fill = false;
			this.Add (this.vbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
		}
	}
}
