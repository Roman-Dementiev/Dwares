using System;
using System.Collections.Generic;
using System.Threading;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Dwarf.Validation
{
	public abstract class BaseValidatable : PropertyNotifier, IValidatable
	{
		private bool? isValid = null;
		public bool IsValid {
			get {
				if (isValid == null) {
					isValid = Validate();
				}
				return isValid == true;
			}
			set => SetProperty(ref isValid, value);
		}

		//public void ClearIsValid()
		//{
		//	if (isValid != null) {
		//		isValid = null;
		//		RaisePropertyChanged(nameof(IsValid));
		//	}
		//}

		protected List<string> errors;
		public IList<string> Errors => LazyInitializer.EnsureInitialized(ref errors);
		public string FirstError => Errors.Count > 0 ? Errors[0] : null;

		public bool Validate() => Validate(true);

		public bool Validate(bool allRules)
		{
			Errors.Clear();

			bool valid = DoValidation(allRules);

			IsValid = valid;
			return valid;
		}

		protected abstract bool DoValidation(bool allRules);
	}

}
