using System;
using System.Collections.Generic;
using Codeplex.Data;

namespace RippleClientGtk
{
	public class RippleBinaryObject
	{

		public Dictionary<BinaryFieldType,Object> fields = null;

		static BinarySerializer binSer = new BinarySerializer ();

		public RippleBinaryObject ()
		{
			fields  = new Dictionary<BinaryFieldType,Object>();
		}

		public RippleBinaryObject ( RippleBinaryObject ob ) : this (ob.fields)
		{

		}

		public RippleBinaryObject (Dictionary<BinaryFieldType,object> fields) : this() // todo is this() necessary?
		{
			//this.fields = new Dictionary<BinaryFieldType, object>();

			// I hope this is sufficient for a copy. TODO fix me for complete copy
			foreach (var pair in fields) {
				this.fields.Add(pair.Key, pair.Value);
			}

		}

		public RippleBinaryObject getUnsignedCopy ()
		{
			// TODO may be a good idea to completely copy. In this case the values are copied as well, see constructor
			RippleBinaryObject copy = new RippleBinaryObject( this );

			copy.removeField(BinaryFieldType.TxnSignature);

			return copy;
		}

		public bool removeField (BinaryFieldType bin) {

			return fields.Remove(bin);

		}

		public byte[] generateHashFromBinaryObject ()
		{
			byte[] bytesToSign = binSer.writeBinaryObject(this).ToArray();

			// Prefix bytesToSign with the magic hashing prefix (32bit) 'STX\0'
			byte[] prefixedBytesToHash = new byte[bytesToSign.Length + 4];

			prefixedBytesToHash	[0]=(byte) 'S';
			prefixedBytesToHash [1]=(byte) 'T';
			prefixedBytesToHash [2]=(byte) 'X';
			prefixedBytesToHash [3]=(byte) 0;

			System.Array.Copy (bytesToSign, 0, prefixedBytesToHash, 4, bytesToSign.Length);

			byte[] hashOfBytes = RippleDeterministicKeyGenerator.halfSHA512(prefixedBytesToHash);

			return hashOfBytes;

		}

		public byte[] getTransactionHash ()
		{
			//Convert to bytes again
			byte[] signedbytes = binSer.writeBinaryObject(this).ToArray();

			//Prefix bytesToSign with the magic sigining prefix (32bit) 'TXN\0'
			byte[] prefixedSignedBytes = new byte[signedbytes.Length+4];

			prefixedSignedBytes[0]=(byte) 'T';
			prefixedSignedBytes[1]=(byte) 'X';
			prefixedSignedBytes[2]=(byte) 'N';
			prefixedSignedBytes[3]=(byte) 0;

			System.Array.Copy(signedbytes, 0, prefixedSignedBytes, 4, signedbytes.Length);

			// Hash again, this wields the TransactionID
			byte[] hashOfTransaction = RippleDeterministicKeyGenerator.halfSHA512 (prefixedSignedBytes);
			return hashOfTransaction;
		}

		public Object getField (BinaryFieldType transactiontype) // why variable named transaction type O_o
		{
			Object obj = null;
			fields.TryGetValue (transactiontype, out obj);

			if (obj == null) {
				return null; // TODO refactor with Maybe object?
			}
			return obj;
		}

		public void putField ( BinaryFieldType field, Object value )
		{

			if (Debug.RippleBinaryObject) {
				Logging.write("RippleBinaryObject.putField ( " + field.ToString() + " " + value.ToString());
			}

			if (value == null) {
				if (Debug.RippleBinaryObject) {
					Logging.write("RippleBinaryObject.putField : Can not set BinaryFieldType " + field.ToString() + "to null");
				}

				throw new ArgumentNullException(field.ToString(), "Can not set BinaryFieldType " + field.ToString() + "to null");
			}
			fields.Add (field, value);
		}

		public TransactionType getTransactionType ()
		{
			Object txTypeObj = getField(BinaryFieldType.TransactionType);
			if (txTypeObj==null) {
				throw new NullReferenceException("No transaction type field found");
			}

			return TransactionType.fromType((byte)txTypeObj);
		}


		public String toJSONString ()
		{
			if (Debug.RippleBinaryObject) {
				Logging.write("RippleBinaryObject.toJSONString() : begin ");
			}

			String json = "{";

			int num = fields.Count;

			int count = 1; // We're starting at one because unlike arrays fields.Count returns the count and not count - 1

			if (num == 0) {
				return "{}";
			}

			// yahaaaa!!!! doing things the old fashioned way!

			foreach (KeyValuePair<BinaryFieldType, Object> field in fields) {
				BinaryType primative = field.Key.type;

				if (primative==null) {
					if (Debug.RippleBinaryObject) {
						Logging.write("BinaryType primitive should not be null");
					}

					throw new ArgumentNullException("BinaryType primitive should not be null");
				}

				else {
					if (Debug.RippleBinaryObject) {
						Logging.write("primative = " + primative.ToString());
					}
				}

				if (field.Value == null) {
					if (Debug.RippleBinaryObject) {
						Logging.write("Field value is null");
					}
				}

				else {
					if (Debug.RippleBinaryObject) {
						//if () {
						//}
						Logging.write("value is " + field.Value.ToString());
					}
				}


				if (primative.typeCode==BinaryType.UINT8 || primative.typeCode==BinaryType.UINT16 ||
				    primative.typeCode==BinaryType.UINT32 || primative.typeCode==BinaryType.UINT64) {

					json += "\"" + primative.ToString() + "\":" + field.Value.ToString();


				}

				else {
					json += "\"" + primative.ToString() + "\":\"" + field.Value.ToString() + "\"";
				}

				if (count!=num) {
						json += ",";
				}

				num++;
			}

			return json;

		}

		public RippleBinaryObject getObjectSorted ()
		{
			List<BinaryFieldType> fie = getSortedFields();

			RippleBinaryObject sorted = new RippleBinaryObject();


			foreach (BinaryFieldType bft in fie) {
				object o = null;
				bool success = this.fields.TryGetValue(bft,out o);
				if (!success || o == null) {
					throw new FieldAccessException("Unknown error sorting BinaryFieldType");
				}

				sorted.putField(bft, o);
			}

			return sorted;
		}

		public List<BinaryFieldType> getSortedFields ()
		{
			List<BinaryFieldType> unsortedFields = new List<BinaryFieldType> (fields.Keys);

			unsortedFields.Sort();

			return (List<BinaryFieldType>)unsortedFields;

			/*
			//List<BinaryFieldType> sortedFields = new List<BinaryFieldType> ();

			//BinaryFieldType[] orderarray = BinaryFieldType.getValues ();




			// Todo verify removal of items durring iteration is ok, may have to use an iterator type
			foreach (BinaryFieldType next in orderarray) {
				foreach (BinaryFieldType field in unsortedFields) {
					if (next.type == field.type && next.value == field.value) {
						sortedFields.Add (field);
						unsortedFields.Remove (field);
					}
				}
			}

			if (unsortedFields.Count != 0) {
				throw new MissingFieldException("Class : RippleBinaryObject Method : getSortedField() : Some fields have remained unsorted");
			}



			return sortedFields;
			*/
		}

	}
}

