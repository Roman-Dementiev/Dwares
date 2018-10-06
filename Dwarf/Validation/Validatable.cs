using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf.Validation
{
	public class Validatable<T> : BaseValidatable
	{
		protected readonly List<IValidationRule<T>> rules;

		public Validatable()
		{
			rules = new List<IValidationRule<T>>();
		}

		public Validatable(IValidationRule<T> rule) :
			this()
		{
			rules.Add(rule);
		}

		public Validatable(IEnumerable<IValidationRule<T>> rules)
		{
			this.rules = new List<IValidationRule<T>>(rules);
		}

		public static implicit operator T(Validatable<T> validatable)
		{
			return validatable.Value;
		}

		public static explicit operator Validatable<T>(T value)
		{
			return new Validatable<T>() { Value = value };
		}


		private T value;
		public T Value {
			get => this.value;
			set => SetProperty(ref this.value, value);
		}

		public void AddRule(IValidationRule<T> rule)
		{
			if (rule != null) {
				rules.Add(rule);
			}
		}

		public void RemoveRule(IValidationRule<T> rule)
		{
			if (rule != null) {
				rules.Remove(rule);
			}
		}

		protected override bool DoValidation(bool allRules)
		{
			var errors = Errors;
			bool valid = true;

			foreach (var rule in rules) {
				if (!rule.IsValid(value)) {
					errors.Add(rule.ValidationMessage);
					valid = false;

					if (!allRules)
						break;
				}
			}
			return valid;
		}
	}
}
