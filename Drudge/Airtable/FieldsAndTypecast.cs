using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


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


		[DataMember(Name = "fields", EmitDefaultValue = false)]
		public Dictionary<string, object> Fields { get; internal set; }

		[DataMember(Name = "typecast", EmitDefaultValue = false)]
		public bool Typecast { get; internal set; } = false;
	}
}
