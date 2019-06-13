using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Drudge.Airtable;
using Dwares.Dwarf;


namespace Drive.Storage.Air
{
	public class TagsTable : AirTable<TagRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(TagsTable));

		public TagsTable(AirBase airBase) :
			base(airBase, "Tags")
		{
			//Debug.EnableTracing(@class);
		}
	}

	public class TagRecord : ARecord
	{
		public const string APPLY_TO = "Apply To";

		public TagRecord() { }

		public List<string> ApplyTo {
			get => GetLinks(APPLY_TO);
			//set => SetLinks(APPLY_TO, value);
		}
	}

}
