using System;
using System.Runtime.Serialization;
using Dwares.Dwarf.Data;


namespace Dwares.Drudge.Airtable
{
    [DataContract]
    public class AirRecordList<TRecord> where TRecord : AirRecord
	{
		//static ClassRef @class = new ClassRef(typeof(AirRecordList<TRecord>));

		public AirRecordList()
		{
			//Debug.EnableTracing(@class);
		}

		[DataMember(Name = "offset", EmitDefaultValue = false)]
		public string Offset { get; internal set; }

		[DataMember(Name = "records", EmitDefaultValue = false)]
		public TRecord[] Records { get; internal set; }
	}

	public class AirRecordList : AirRecordList<AirRecord>
	{
	}
}
