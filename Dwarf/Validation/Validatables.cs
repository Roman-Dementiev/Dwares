using System;
using System.Collections.Generic;


namespace Dwares.Dwarf.Validation
{
	public class Validatables : BaseValidatable
	{
		List<IValidatable> list;

		public Validatables()
		{
			list = new List<IValidatable>();
		}

		public Validatables(IEnumerable<IValidatable> list)
		{
			this.list = new List<IValidatable>(list);
		}

		public Validatables(params IValidatable[] list)
		{
			this.list = new List<IValidatable>(list);
		}

		public void Add(IValidatable obj) => list.Add(obj);
		public void Add(IEnumerable<IValidatable> list) => this.list.AddRange(list);
		public bool Remove(IValidatable obj) => list.Remove(obj);

		protected override bool DoValidation(bool allRules)
		{
			bool valid = true;

			foreach (var obj in list) {
				if (!obj.Validate(allRules)) {
					errors.AddRange(obj.Errors);
					valid = false;
					if (!allRules)
						break;
				}
			}
			return valid;
		}
	}
}
