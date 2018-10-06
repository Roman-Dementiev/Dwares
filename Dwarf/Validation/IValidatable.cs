using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf.Validation
{
	public interface IValidatable
	{
		bool IsValid { get; }
		bool Validate();
		bool Validate(bool allRules);
		IList<string> Errors { get; }
	}
}
