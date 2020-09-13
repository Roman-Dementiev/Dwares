using System;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Dwarf.Toolkit;

namespace DraggableListView.Models
{
	public class Marine : Model, IOrdinal, INestedOrdinal
	{
		//static ClassRef @class = new ClassRef(typeof(Marine));

		public Marine()
		{
			//Debug.EnableTracing(@class);
		}

		public Guid Id { get; set; } = Guid.NewGuid();

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public MarineCorpsRank Rank { get; set; }
		public string FullName {
			get => $"{LastName}, {FirstName}";
		}

		public int Ordinal {
			get => ordinal;
			set => SetProperty(ref ordinal, value);
		}
		int ordinal;

		public int NestedOrdinal {
			get => nestedOrdinal;
			set => SetProperty(ref nestedOrdinal, value);
		}
		int nestedOrdinal;

		public override string ToString() => FullName;
		//public int GetOrdinal(OrdinalType type)
		//{
		//	return type == OrdinalType.Nested ? TeamOrdinal : Ordinal;
		//}

		//public void SetOrdinal(int ordinal, OrdinalType type)
		//{
		//	if (type == OrdinalType.Nested) {
		//		TeamOrdinal = ordinal;
		//	} else {
		//		Ordinal = ordinal;
		//	}
		//}
	}
}
