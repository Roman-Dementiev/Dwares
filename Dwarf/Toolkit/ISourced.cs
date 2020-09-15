using System;


namespace Dwares.Dwarf.Toolkit
{
	public interface ISourced<T>
	{
		T Source { get; }
	}
}
