using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Dwares.Dwarf;


namespace Dwares.Rookie.Airtable
{
	public class AirResponse
	{
		public AirResponse(string body)
		{
			Success = false;
			Error = null;
			Body = body;
		}

		public AirResponse(AirException error)
		{
			Success = false;
			Error = error;
			Body = null;
		}

		public bool Success { get; }
		public AirException Error { get; }
		public string Body { get; }
	}

	public class AirListRecordsResponse : AirResponse
	{
		public AirListRecordsResponse(AirException error) : base(error)
		{
			Offset = null;
			Records = null;
		}


		public AirListRecordsResponse(string body) : base(body)
		{
			var recordList = JsonConvert.DeserializeObject<AirRecordList>(Body);

			Offset = recordList.Offset;
			Records = recordList.Records;
		}

		public readonly IEnumerable<AirRecord> Records;
		public readonly string Offset;
	}

	public class AirCUROpResponce : AirResponse
	{
		public AirCUROpResponce(string body) : base(body)
		{
			Record = JsonConvert.DeserializeObject<AirRecord>(body);
		}

		public AirRecord Record { get; }
	}
}
