using System;

namespace RippleClientGtk
{
	public class TrustLine
	{


		public TrustLine ()
		{

			this.account = null;
			this.balance = null;
			this.currency = null;
			this.limit = null;
			this.limit_peer = null;

			this.quality_in = (ulong)0;
			this.quality_out = (ulong)0;
		}


		public TrustLine (String account, String balance, String currency, String limit, String limit_peer, ulong quality_in, ulong quality_out)
		{
			this.account = account;
			this.balance = balance;
			this.currency = currency;
			this.limit = limit;
			this.limit_peer = limit_peer;

			this.quality_in = quality_in;
			this.quality_out = quality_out;
		}

		public Object getTableXIndex (uint x)
		{
			return getTableXIndex((int)x);
		}

		public Object getTableXIndex (int x) {
			string method_sig = clsstr + "getTableXIndex("+ x.ToString() + ") : ";

			if (Debug.TrustLine) {
				Logging.write(method_sig + "begin" );
			}
			switch (x) {

			case 0:
				return this.account;
				//break;

			case 1:
				return this.balance;
				//break;

			case 2:
				return this.currency;
				//break;

			case 3:
				return this.limit;
				//break;
			case 4:
				return this.limit_peer;
				//break;
			case 5:
				return this.quality_in;
				//break;
			case 6:
				return this.quality_out;
				//break;
			default:
				Logging.write (method_sig + "Line.getTableXIndex index out of bounds\n");
				return null;
				//break;
			}


		}

		public decimal getBalanceAsDecimal () {

			String method_sig = clsstr + "getBalanceAsDecimal () : ";

			try {
				//return Convert.ToDouble(this.balance);
				if (this.balance == null) {
					if (Debug.TrustLine) {
						Logging.write(method_sig + "this.balance  == null");
					}
					return 0;
				}


				decimal d = Convert.ToDecimal(this.balance);
				if (Debug.TrustLine) {
					Logging.write(method_sig + " returning " + d.ToString());
				}
				return d;
			}

			catch (Exception e) {
				Logging.write (method_sig + "Error in TrustLine, Exception thrown converting string to double. String balance == " + (String) ((this.balance == null)? "null" : this.balance));
				Logging.write (e.Message);
				return 0;
			}
		}




		public static readonly String[] titles = { "Account","Balance","Currency","Limit","Limit Peer","Quality In","Quality Out" };

		public String account { get; set;}

		public String balance { get; set; }

		public String currency { get; set; }

		public String limit  { get; set;}

		public String limit_peer  { get; set;}

		public ulong quality_in  { get; set;}

		public ulong quality_out  { get; set;}

		public static readonly uint numColumns = 7;

		private static readonly String clsstr = "TrustLine : ";
	}
}

