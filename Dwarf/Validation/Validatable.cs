//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Dwares.Dwarf.Validation
//{
//	public class Validatable<T> : BaseValidatable
//	{
//		protected List<IValidationRule> rules;

//		public Validatable() { }

//		public Validatable(IValidationRule rule)
//		{
//			AddRule(rule);
//		}


//		protected T _value;
//		public virtual T Value {
//			get => _value;
//			set {
//				if (SetProperty(ref _value, value)) {
//					isValid = null;
//					PropertiesChanged(nameof(IsValid));
//				}
//			}
//		}

//		public void AddRule(IValidationRule rule)
//		{
//			if (rule != null) {
//				if (rules == null) {
//					rules = new List<IValidationRule>();
//				}
//				rules.Add(rule);
//			}
//		}

//		public void RemoveRule(IValidationRule rule)
//		{
//			if (rule != null && rules != null) {
//				rules.Remove(rule);
//			}
//		}

//		public override Exception Validate()
//		{
//			if (rules != null) {
//				try {
//					foreach (var rule in rules) {
//						string message;
//						if (!rule.IsValid(Value, out message)) {
//							return new ValidationError(message);
//						}
//					}
//				} catch (Exception exc) {
//					return exc;
//				}
//			}
//			return null;
//		}

//		public override List<Exception> ValidateAll()
//		{
//			List<Exception> errors = null;
//			if (rules != null) {
//				foreach (var rule in rules) {
//					try {
//						string message;
//						if (!rule.IsValid(Value, out message)) {
//							if (errors == null) {
//								errors = new List<Exception>();
//							}
//							errors.Add(new ValidationError(message));
//						}
//					} catch (Exception exc) {
//						if (errors == null) {
//							errors = new List<Exception>();
//						}
//						errors.Add(exc);
//					}
//				}
//			}
//			return errors;

//		}

//		public Validatable(IEnumerable<IValidationRule> rules)
//		{
//			this.rules = new List<IValidationRule>(rules);
//		}

//		public static implicit operator T(Validatable<T> validatable)
//		{
//			return validatable.Value;
//		}

//		//public static explicit operator Validatable<T>(T value)
//		//{
//		//	return new Validatable<T>() { Value = value };
//		//}

//	}
//}
