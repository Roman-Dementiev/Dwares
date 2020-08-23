using System;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;


namespace Beylen.Storage.Air
{
	public class PropertiesTable : AirTable<PropertyRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(PropertiesTable));

		public PropertiesTable(AirBase airBase) :
			base(airBase, "Properties")
		{
			//Debug.EnableTracing(@class);
		}
	}

	public class PropertyRecord : AirRecord
	{
		public const string NAME = "Name";
		public const string VALUE = "Value";

		public PropertyRecord() { }

		public string Name {
			get => GetField<string>(NAME);
			set => SetField(NAME, value);
		}

		public string Value {
			get => GetField<string>(VALUE);
			set => SetField(VALUE, value);
		}
	}
}
