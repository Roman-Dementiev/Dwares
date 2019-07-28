using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Runtime;


namespace Dwares.Dwarf
{
	public class Package : PackageUnit
	{
		public static readonly Package Instance = new Package();

		Package() : base(typeof(Package)) { }
	}

	public interface INull
	{
		Type Type();
	}

	class Null<T> : INull
	{
		public Type Type() => typeof(T);
	}

	public static class Null
	{
		static Null<T> As<T>() => new Null<T>();
	}


	public class Never
	{
		private Never() { }
		public T As<T>() => default(T);
	}
}
