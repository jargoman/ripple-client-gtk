using System;
using Gtk;
using Codeplex.Data;
using RippleClientGtk;

namespace RippleClientGtk
{
	public class RipplePaymentTransaction : RippleClientGtk.RippleTransaction
	{
		public RippleAddress payer; 
		public RippleAddress payee;

		public DenominatedIssuedCurrency amount;
		public DenominatedIssuedCurrency fee;

		public String signedTransactionBlob = null;



		public String txHash;
		//String signature;

		public String publicKeyUsedToSign;



		public UInt32 tfPartialPayment = 0x00020000;

		public DenominatedIssuedCurrency sendmax = null;

		public RipplePaymentTransaction (RippleAddress payer, RippleAddress payee, DenominatedIssuedCurrency amount, DenominatedIssuedCurrency fee, UInt32 sequencenumber, DenominatedIssuedCurrency sendmax) : base (sequencenumber)
		{
			this.payer = payer;
			this.payee = payee;
			this.amount = amount;

			this.fee = fee;

			this.sendmax = sendmax;
		}

		public RipplePaymentTransaction (RippleBinaryObject serObj)
		{
			if (serObj.getTransactionType ().uint16Value != TransactionType.PAYMENT) {
				throw new Exception ("The RippleBinaryObject is not a payment transaction, but a " + serObj.getTransactionType ().ToString ());
			}

			payer = (RippleAddress)serObj.getField (BinaryFieldType.Account);
			payee = (RippleAddress)serObj.getField (BinaryFieldType.Destination);
			amount = (DenominatedIssuedCurrency)serObj.getField (BinaryFieldType.Amount);
			sequenceNumber = (UInt32)serObj.getField (BinaryFieldType.Sequence);
			fee = (DenominatedIssuedCurrency)serObj.getField (BinaryFieldType.Fee);
			flags = (UInt32)serObj.getField (BinaryFieldType.Flags);

			object sm = (object)serObj.getField (BinaryFieldType.SendMax);
			if (sm != null) {
				this.sendmax = (DenominatedIssuedCurrency)sm;
			}

			//int[] s = new int[0];
		}

		public override RippleBinaryObject getBinaryObject ()
		{
			RippleBinaryObject rbo = new RippleBinaryObject();
			rbo.putField(BinaryFieldType.TransactionType, TransactionType.PAYMENT);
			rbo.putField(BinaryFieldType.Account, this.payer);
			rbo.putField(BinaryFieldType.Destination, this.payee);
			rbo.putField(BinaryFieldType.Amount, this.amount);
			rbo.putField(BinaryFieldType.Sequence, this.sequenceNumber);
			rbo.putField(BinaryFieldType.Fee, this.fee);

			if (this.sendmax!=null) {
				rbo.putField(BinaryFieldType.SendMax, this.sendmax);
			}

			rbo.putField(BinaryFieldType.Flags, this.flags);

			return rbo;
		}







		public void submit ()
		{

			if (this.signedTransactionBlob == null) {
				// todo report error. Transaction must be signed first. 
				return;
			}

			object ob = new {
				command = "submit",
				tx_blob = this.signedTransactionBlob
			};

			String json = DynamicJson.Serialize(ob);

			Logging.write ("Sending payment: " + json + "\n");

			Logging.write("Amount : " + amount.ToString() + "  Sendmax : " + sendmax.ToString());

			if (NetworkInterface.currentInstance != null) {

				Gtk.Application.Invoke ( delegate {

					String message = "You are about to send " + amount.amount.ToString() + " " + amount.currency + " to address " + payee.ToString();
					if (sendmax!=null) {
						message += " spending a maximum of " + sendmax.amount.ToString() + " " + sendmax.currency;
					}
			
					AreYouSure ays = new AreYouSure (message);


					int resp = ays.Run ();
					ays.Destroy ();

					if (resp == (int)ResponseType.Ok) {

						NetworkInterface.currentInstance.sendToServer (json);

					} else {
					// user canseled
						return;
					}
				});

			} else {
				MessageDialog.showMessage ("To send a payment you need to be connected to a server. Please go to network settings and connect");
			}
		}
	}
}

