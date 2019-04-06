using System;
using Newtonsoft.Json;


namespace Dwares.Rookie.Airtable
{
	public class AirRecordList<TRecord> where TRecord : AirRecord
	{
		[JsonProperty("offset")]
		public string Offset { get; internal set; }

		[JsonProperty("records")]
		public TRecord[] Records { get; internal set; }
	}

	public class AirRecordList : AirRecordList<AirRecord>
	{
	}

}
