using System;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;
using Dwares.Dwarf.Toolkit;

namespace Beylen.Storage.Air
{
	public class RouteTable : AirTable<RouteRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(RouteTable));

		public RouteTable(AirBase airBase) :
			base(airBase, "Route")
		{
			//Debug.EnableTracing(@class);
		}
	}

	public class RouteRecord : AirRecord
	{
		public const string _ = "#";
		public const string DATE = "Date";
		public const string SEQ = "Seq";
		public const string TAGS = "Tags";
		public const string STATUS = "Status";
		public const string CODE_NAME = "Code name";

		public const string Enroute = nameof(Enroute);
		public const string Arrived = nameof(Arrived);
		public const string Departed = nameof(Departed);

		public RouteRecord() { }

		public DateOnly Date {
			get => GetField<DateOnly>(DATE);
			set => SetField(DATE, value);
		}

		public int Seq {
			get => GetField<int>(SEQ);
			set => SetField(SEQ, value);
		}

		public string Tags {
			get => GetField<string>(TAGS);
			set => SetField(TAGS, value);
		}

		public string Status {
			get => GetField<string>(STATUS);
			set => SetField(STATUS, value);
		}

		public string CodeName {
			get => GetField<string>(CODE_NAME);
			set => SetField(CODE_NAME, value);
		}
	}
}
