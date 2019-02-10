using System;
using System.Collections.Generic;
using System.Threading;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Dwarf.Validation
{
	public abstract class BaseValidatable : PropertyNotifier, IValidatable
	{
		protected bool? isValid = null;
		public virtual bool IsValid {
			get {
				if (isValid == null) {
					Error = Validate();
					isValid = Error == null;
				}
				return isValid == true;
			}
		}

		Exception error;
		public Exception Error {
			get => error;
			protected set => SetProperty(ref error, value);
		}

		public abstract Exception Validate();
		public abstract List<Exception> ValidateAll();
	}
}
