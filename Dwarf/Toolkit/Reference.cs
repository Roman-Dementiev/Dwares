using System;


namespace Dwares.Dwarf.Toolkit
{
	public class Reference<T> : IValueHolder<T>
	{
		//static ClassRef @class = new ClassRef(typeof(Reference));

		public Reference(T value = default(T))
		{
			//Debug.EnableTracing(@class);
			Value = value;
		}

		public T Value { get; set; }
	}

	public class Implicit<T> : Reference<T>
	{
		public Implicit(T value = default(T)) : base(value) {}

		public static implicit operator T(Implicit<T> @ref) => @ref.Value;
		//public static implicit operator Implicit<T>(T value) => new Implicit<T>(value);
	}

	public class Explicit<T> : Reference<T>
	{
		public Explicit(T value = default(T)) : base(value) { }

		public static explicit operator T(Explicit<T> @ref) => @ref.Value;
		//public static explicit operator Explicit<T>(T value) => new Explicit<T>(value);
	}
}
