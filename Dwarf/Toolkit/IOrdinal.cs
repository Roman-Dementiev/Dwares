using System;


namespace Dwares.Dwarf.Toolkit
{
	//public enum OrdinalType
	//{
	//	Default,
	//	Nested
	//}

	public interface IOrdinal
	{
		int Ordinal { get; set; }
		//int GetOrdinal(OrdinalType type);
		//void SetOrdinal(int ordinal, OrdinalType type);
	}

	public interface INestedOrdinal
	{
		int NestedOrdinal { get; set; }
	}
}
