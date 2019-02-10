using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf.Validation
{
	public interface IValidatable
	{
		bool IsValid { get; }
		Exception Validate();
		List<Exception> ValidateAll();
	}
}
