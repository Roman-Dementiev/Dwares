using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Dwares.Dwarf;


namespace Dwares.Drudge.Airtable
{
    [DataContract]
    public class Fields
	{
		//static ClassRef @class = new ClassRef(typeof(Fields));

		public Fields()
		{
			//Debug.EnableTracing(@class);
			FieldsCollection = new Dictionary<string, object>();
		}

		[DataMember(Name = "fields", EmitDefaultValue = false)]
		public Dictionary<string, object> FieldsCollection { get; set; }

		public void AddField(string fieldName, object fieldValue)
		{
			FieldsCollection.Add(fieldName, fieldValue);
		}
	}
}
