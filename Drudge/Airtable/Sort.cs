using System;
using System.Runtime.Serialization;
using Dwares.Dwarf;


namespace Dwares.Drudge.Airtable
{
	public enum SortDirection
	{
		Asc,
		Desc
	}

	public class Sort
	{
		//static ClassRef @class = new ClassRef(typeof(Sort));

		public Sort()
		{
			//Debug.EnableTracing(@class);
		}

		[DataMember(Name = "fields", EmitDefaultValue = false)]
		public string Fields { get; set; }

		[DataMember(Name = "direction", EmitDefaultValue = false)]
		public SortDirection Direction { get; set; }
	}

}
