using System;
using Dwares.Dwarf.Runtime;


namespace Dwares.Dwarf
{
	public struct ClassRef
	{
		public ClassRef(Type type)
		{
			Type = type;
			unit = null;
		}

		public Type Type { get; }

		ClassUnit unit;
		public ClassUnit Unit {
			get {
				if (unit == null) {
					unit = CompilationUnit.GetClass(Type);
				}
				return unit;
			}
		}

		public static implicit operator Type(ClassRef @ref) => @ref.Type;
		public static implicit operator ClassUnit(ClassRef @ref) => @ref.Unit;
	}
}
