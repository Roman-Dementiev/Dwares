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

		public override Exception Validate()
		{
			Exception error = null;
			foreach (var item in list) {
				error = item.Validate();
				if (error != null)
					break;
			}
			return error;
		}

		public override List<Exception> ValidateAll()
		{
			List<Exception> errors = null;
			foreach (var item in list) {
				var itemErrors = item.ValidateAll();
				if (itemErrors != null && itemErrors.Count > 0) {
					if (errors == null) {
						errors = itemErrors;
					} else {
						errors.AddRange(itemErrors);
					}
				}
			}
			return errors;
		}
	}
}
