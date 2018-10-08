using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf.Toolkit
{
	public interface IValueHolder<T>
	{
		T Value { get; set; }
	}
}
