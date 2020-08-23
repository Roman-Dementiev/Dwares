using System;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;
using System.Collections.Concurrent;

namespace Beylen.Storage.Air
{
	public class ProduceTable : AirTable<ProduceRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(ProduceTable));

		public ProduceTable(AirBase airBase) :
			base(airBase, "Produce")
		{
			//Debug.EnableTracing(@class);
		}
	}

	public class ProduceRecord : AirRecord
	{
		public const string NAME = "Name";
		public const string PACKAGE = "Package";
		public const string CPP = "Cpp";

		public ProduceRecord() { }

		public string Name {
			get => GetField<string>(NAME);
			set => SetField(NAME, value);
		}

		public string Package {
			get => GetField<string>(PACKAGE);
			set => SetField(PACKAGE, value);
		}

		public int Cpp {
			get => GetField<int>(CPP);
			set => SetField(CPP, value);
		}
	}
}
