//using System;
//using Dwares.Dwarf.Toolkit;
//using System.Text;
//using System.Collections.Generic;

//namespace Dwares.Dwarf.Data
//{
//	public class DataRecord : Implicit<IDataRecord>
//	{
//		public DataRecord() { }


//		public DataRecord(IDataRecord record) 
//		{
//			Guard.ArgumentNotNull(record, nameof(record));

//			Value = record;
//		}

//		//public IDataRecord Record { get; set; }

//		public object RecordId => Value?.RecordId;
//		public IEnumerable<string> GetFieldNames() => Value?.GetFieldNames();

//		public bool GetField<T>(string fieldName, out T value) 
//			=> GetField(fieldName, out value, default(T));
		
//		public T GetField<T>(string fieldName, T defaultValue = default(T)) { 
//			GetField(fieldName, out T value, defaultValue);
//			return value;
//		}

//		public bool GetField<T>(string fieldName, out T value, T defaultValue)
//		{
//			if (Value != null) {
//				return Value.GetField(fieldName, out value);
//			}

//			value = defaultValue;
//			return false;
//		}

//		public void SetField<T>(string fieldName, T value)
//		{
//			Value?.SetField(fieldName, value);
//		}
//	}
//}
