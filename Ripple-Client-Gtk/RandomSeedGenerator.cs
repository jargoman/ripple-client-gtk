using System;
using Org.BouncyCastle.Math;

namespace RippleClientGtk
{
	public partial class RandomSeedGenerator : Gtk.Dialog
	{
		public RandomSeedGenerator ()
		{
			this.Build ();



			Gdk.Window gwin = this.drawingarea1.GdkWindow;
			Gdk.Color background = new Gdk.Color(150,150,150);

			this.drawingarea1.ModifyBg(Gtk.StateType.Normal, background);


			this.drawingarea1.AddEvents ((int) Gdk.EventMask.PointerMotionMask);

			this.drawingarea1.MotionNotifyEvent += delegate(object o, Gtk.MotionNotifyEventArgs args) {
				Gdk.EventMotion Event = args.Event;

				// SSLLOOOOWWWW !!!!
				//if (Debug.RandomSeedGenerator) {
				//	Logging.write("RandomSeedGenerator.MotionNotifyEvent. x = " + Event.X.ToString() + ", y = " + Event.Y.ToString() + ", time = " + Event.Time.ToString());
				//}

				//bigInteger = bigInteger.Add( BigInteger.ValueOf( (long)Event.X * (long)Event.Y * (long)Event.Time));

				//bigInteger = bigInteger.Add( BigInteger.ValueOf( (long)Event.X * (long)Event.Y ));
				//bigInteger = bigInteger.Add( BigInteger.ValueOf( (long) Event.Time));
				BigInteger big = BigInteger.ValueOf((long) (Event.XRoot * Event.YRoot)).Multiply(BigInteger.ValueOf((long) Event.Time)).Add(BigInteger.ValueOf((long)Event.X).Add(BigInteger.ValueOf((long)Event.Y)));

				Random ra = new Random(big.IntValue);

				ra.NextBytes(bytesBuff);

				bigInteger = bigInteger.Xor(new BigInteger(1, bytesBuff));

				Gdk.GC gc = new Gdk.GC((Gdk.Drawable)gwin);
				//gwin.DrawPoint(gc, (int)Event.X, (int)Event.Y);

				int cirsize = 5;
				gwin.DrawArc(gc, true, (int)Event.X - (cirsize / 2), (int)Event.Y - (cirsize / 2), cirsize, cirsize, 0, 23040);
				//gwin.DrawArc(
				update();
			};

			update ();
		}

		private static BigInteger initBigint (int timeseed)
		{
			String hostname = System.Environment.MachineName;

			timeseed *= getIntFromString(hostname);

			Random rand = new Random(timeseed); 


			rand.NextBytes(bytesBuff);

			return new BigInteger(1, bytesBuff);
		}

		public void update ()
		{
			Gtk.Application.Invoke( delegate {
				this.bigintlabel.Text = bigInteger.ToString();
			});
		}

		public RippleSeedAddress getGeneratedSeed ()
		{
			int timeseed = timeSeed();

			if (bigInteger == null) { // won't happen
				bigInteger = initBigint(timeSeed());
			}

			String userstring = this.entry1.Text;

			if (userstring!=null || userstring.Equals("")) {
				timeseed*= getIntFromString(userstring);
			}

			Random rand = new Random(timeseed);

			byte[] seedbuff = new byte[16];

			int count = 0;
			while (count < 7) { // seven should be more than enough, the purpose of the loop is the unlikely event that the xor causes

				rand.NextBytes(bytesBuff);

				bigInteger = bigInteger.Xor(new BigInteger(1, bytesBuff));

				byte[] buffer = bigInteger.ToByteArray();

				if (buffer.Length < 16) {
					continue;
				}

				System.Array.Copy( buffer, buffer.Length - 16, seedbuff, 0, 16);

				return new RippleSeedAddress(seedbuff);
			}

			throw new Exception("The generated seedbytes aren't 16 bytes long");
		}

		private static int timeSeed ()
		{
			return DateTime.UtcNow.Hour * DateTime.UtcNow.Month * DateTime.UtcNow.Day * DateTime.UtcNow.Minute * DateTime.UtcNow.Second * DateTime.UtcNow.Millisecond;

		}

		private static int getIntFromString(  String str ) {

			int result = 0;
			if (str!=null) {
				char[] chacha = str.ToCharArray();
				foreach (char move in chacha) {
					result *= (int) move;
				}
			}

			if (result == 0) {
				result++;
			}

			return result;
		}

		public static void startupSeed ()
		{
			bigInteger = initBigint(timeSeed());
		}

		//public static RandomSeedGenerator currentInstance


		static BigInteger bigInteger = null;
		static byte[] bytesBuff = new byte[16];
	}
}

