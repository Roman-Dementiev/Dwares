using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Dwares.Dwarf;


namespace Dwares.Drudge.Airtable
{
	public class FieldsAndTypecast
	{
		//static ClassRef @class = new ClassRef(typeof(FieldsAndTypecast));

		public FieldsAndTypecast() : this(null, false) { }

		public FieldsAndTypecast(Dictionary<string, object> fields, bool typecast)
		{
			//Debug.EnableTracing(@class);

			//Fields = fields ?? new Dictionary<string, object>();
			Fields = fields;
			Typecast = false;
		}


		[JsonProperty("fields")]
		public Dictionary<string, object> Fields { get; internal set; }

		[JsonProperty("typecast")]
		public bool Typecast { get; internal set; } = false;

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
		}
	}
}
